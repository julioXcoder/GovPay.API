using GovPay.API.Data;
using GovPay.API.DTOs;
using GovPay.API.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace GovPay.API.Controllers
{
    public static class InvoiceEndPoints
    {
        public static IEndpointRouteBuilder MapInvoiceEndPoints(this WebApplication app)
        {
            var group = app.MapGroup("/invoices");

            // GET all invoices
            group.MapGet("/", async (GovPayContext db) =>
            {
                var invoices = await db.Invoices.ToListAsync();
                return Results.Ok(invoices);
            });

            // GET invoice by gatewayInvoiceRef
            group.MapGet("/{gatewayInvoiceRef}", async (string gatewayInvoiceRef, GovPayContext db) =>
            {
                // 1. Find invoice by GatewayInvoiceRef
                var invoice = await db.Invoices
                    .FirstOrDefaultAsync(i => i.GatewayInvoiceRef == gatewayInvoiceRef);

                if (invoice is null)
                {
                    return Results.NotFound(new
                    {
                        status = "FAILED",
                        message = "Invoice not found"
                    });
                }

                // 2. Get all payments linked to this invoice
                var payments = await db.Payments
                    .Where(p => p.GatewayInvoiceRef == gatewayInvoiceRef)
                    .ToListAsync();

                // 3. Prepare response
                var result = new
                {
                    gatewayInvoiceRef = invoice.GatewayInvoiceRef,
                    invoiceId = invoice.InvoiceId,
                    status = invoice.Status,
                    amount = invoice.Amount,
                    currency = invoice.Currency,
                    paymentRefs = payments.Select(p => p.PaymentRef).ToArray(),
                    clearedDate = invoice.Status == "PAID" 
                                    ? payments.Max(p => (DateTime?)p.PaymentDate)  // last payment date
                                    : null
                };

                return Results.Ok(result);
            });


            // POST create invoice
            group.MapPost("/", async (CreateInvoiceDto dto, GovPayContext db) =>
            {
                // Will generate the GatewayInvoiceRef using GWY-current year- count
                var gatewayInvoiceRef = $"GWY-{DateTime.UtcNow:yyyy}-{db.Invoices.Count() + 1}";

                var invoice = new Invoice
                {
                    GatewayInvoiceRef = gatewayInvoiceRef,
                    InvoiceId = dto.InvoiceId,
                    SellerCode = dto.SellerCode,
                    SellerName = dto.SellerName,
                    PayerName = dto.PayerName,
                    PayerId = dto.PayerId,
                    Amount = dto.Amount,
                    Currency = dto.Currency,
                    DueDate = dto.DueDate,
                    Status = "PENDING", // default
                    Description = dto.Description,
                    CreatedAt = DateTime.UtcNow
                };

                db.Invoices.Add(invoice);
                await db.SaveChangesAsync();

                var response = new
                {
                    status = "success",
                    message = "Invoice registered successfully",
                    gatewayInvoiceRef = invoice.GatewayInvoiceRef
                };

                return Results.Created($"/invoices/{invoice.Id}", response);
            });

            // PUT update invoice
            group.MapPut("/{id}", async (string id, UpdateInvoiceDto dto, GovPayContext db) =>
            {
                if (!ObjectId.TryParse(id, out var objectId))
                    return Results.BadRequest("Invalid ObjectId format.");

                var invoice = await db.Invoices.FindAsync(objectId);
                if (invoice is null)
                    return Results.NotFound();

                invoice.Amount = dto.Amount;
                invoice.Currency = dto.Currency;
                invoice.DueDate = dto.DueDate;
                invoice.Status = dto.Status;
                invoice.Description = dto.Description;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE invoice
            group.MapDelete("/{id}", async (string id, GovPayContext db) =>
            {
                if (!ObjectId.TryParse(id, out var objectId))
                    return Results.BadRequest("Invalid ObjectId format.");

                var invoice = await db.Invoices.FindAsync(objectId);
                if (invoice is null)
                    return Results.NotFound();

                db.Invoices.Remove(invoice);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            return group;
        }
    }
}
