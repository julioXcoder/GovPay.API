using GovPay.API.Data;
using GovPay.API.DTOs;
using GovPay.API.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace GovPay.API.Controllers
{
    public static class PaymentEndPoints
    {
        public static IEndpointRouteBuilder MapPaymentEndPoints(this WebApplication app)
        {
            var group = app.MapGroup("/payments");

            // GET all payments
            group.MapGet("/", async (GovPayContext db) =>
            {
                var payments = await db.Payments.ToListAsync();
                return Results.Ok(payments);
            });

            // GET payment by Id
            group.MapGet("/{id}", async (string id, GovPayContext db) =>
            {
                if (!ObjectId.TryParse(id, out var objectId))
                    return Results.BadRequest("Invalid ObjectId format.");

                var payment = await db.Payments.FindAsync(objectId);
                return payment is not null ? Results.Ok(payment) : Results.NotFound();
            });

            // POST create payment
            group.MapPost("/", async (CreatePaymentDto dto, GovPayContext db) =>
            {
                var receiptNumber = $"RCT-{DateTime.UtcNow:yyyy}-{db.Payments.Count() + 1}";

                // 1. Create new payment
                var payment = new Payment
                {
                    PaymentRef = dto.PaymentRef,
                    GatewayInvoiceRef = dto.GatewayInvoiceRef,
                    PspCode = dto.PspCode,
                    AmountPaid = dto.AmountPaid,
                    Currency = dto.Currency,
                    PaymentDate = dto.PaymentDate,
                    ReceiptNumber = receiptNumber,
                    CreatedAt = DateTime.UtcNow
                };

                db.Payments.Add(payment);
                await db.SaveChangesAsync();

                // 2. Find the invoice related to this payment
                var invoice = await db.Invoices
                    .FirstOrDefaultAsync(i => i.GatewayInvoiceRef == dto.GatewayInvoiceRef);

                if (invoice is null)
                {
                    return Results.NotFound(new
                    {
                        status = "FAILED",
                        message = "Invoice not found"
                    });
                }

                // 3. Calculate the total amount paid so far for this invoice
                var totalPaid = await db.Payments
                    .Where(p => p.GatewayInvoiceRef == dto.GatewayInvoiceRef)
                    .SumAsync(p => p.AmountPaid);

                // 4. Compare paid vs invoice amount
                if (totalPaid >= invoice.Amount)
                {
                    return Results.Ok(new
                    {
                        status = "SUCCESS",
                        message = "Payment recorded and invoice cleared",
                        receiptNumber = payment.ReceiptNumber,
                        totalPaid,
                        invoiceAmount = invoice.Amount
                    });
                }
                else
                {
                    return Results.Ok(new
                    {
                        status = "PENDING",
                        message = "Payment recorded but invoice not fully cleared",
                        receiptNumber = payment.ReceiptNumber,
                        totalPaid,
                        invoiceAmount = invoice.Amount
                    });
                }
            });

            // DELETE payment
            group.MapDelete("/{id}", async (string id, GovPayContext db) =>
            {
                if (!ObjectId.TryParse(id, out var objectId))
                    return Results.BadRequest("Invalid ObjectId format.");

                var payment = await db.Payments.FindAsync(objectId);
                if (payment is null)
                    return Results.NotFound();

                db.Payments.Remove(payment);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            return group;
        }
    }
}
