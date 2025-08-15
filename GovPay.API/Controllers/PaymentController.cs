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
                var payment = new Payment
                {
                    PaymentRef = dto.PaymentRef,
                    GatewayInvoiceRef = dto.GatewayInvoiceRef,
                    PspCode = dto.PspCode,
                    AmountPaid = dto.AmountPaid,
                    Currency = dto.Currency,
                    PaymentDate = dto.PaymentDate,
                    ReceiptNumber = dto.ReceiptNumber,
                    CreatedAt = DateTime.UtcNow
                };

                db.Payments.Add(payment);
                await db.SaveChangesAsync();

                return Results.Created($"/payments/{payment.Id}", payment);
            });

            // PUT update payment
            group.MapPut("/{id}", async (string id, UpdatePaymentDto dto, GovPayContext db) =>
            {
                if (!ObjectId.TryParse(id, out var objectId))
                    return Results.BadRequest("Invalid ObjectId format.");

                var payment = await db.Payments.FindAsync(objectId);
                if (payment is null)
                    return Results.NotFound();

                payment.PspCode = dto.PspCode;
                payment.AmountPaid = dto.AmountPaid;
                payment.Currency = dto.Currency;
                payment.PaymentDate = dto.PaymentDate;
                payment.ReceiptNumber = dto.ReceiptNumber;

                await db.SaveChangesAsync();
                return Results.NoContent();
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
