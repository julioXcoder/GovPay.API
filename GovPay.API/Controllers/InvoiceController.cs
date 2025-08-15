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

            // GET invoice by ID
            group.MapGet("/{id}", async (string id, GovPayContext db) =>
            {
                if (!ObjectId.TryParse(id, out var objectId))
                    return Results.BadRequest("Invalid ObjectId format.");

                var invoice = await db.Invoices.FindAsync(objectId);
                return invoice is not null ? Results.Ok(invoice) : Results.NotFound();
            });

            // POST create invoice
            group.MapPost("/", async (CreateInvoiceDto dto, GovPayContext db) =>
            {
                var invoice = new Invoice
                {
                    GatewayInvoiceRef = dto.GatewayInvoiceRef,
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

                return Results.Created($"/invoices/{invoice.Id}", invoice);
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
