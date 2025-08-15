using GovPay.API.Data;
using GovPay.API.DTOs;
using GovPay.API.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace GovPay.API.Controllers
{
    public static class NotificationEndPoints
    {
        public static IEndpointRouteBuilder MapNotificationEndPoints(this WebApplication app)
        {
            var group = app.MapGroup("/notifications");

            // GET all notifications
            group.MapGet("/", async (GovPayContext db) =>
            {
                var notifications = await db.Notifications.ToListAsync();
                return Results.Ok(notifications);
            });

            // GET notification by ID
            group.MapGet("/{id}", async (string id, GovPayContext db) =>
            {
                if (!ObjectId.TryParse(id, out var objectId))
                    return Results.BadRequest("Invalid ObjectId format.");

                var notification = await db.Notifications.FindAsync(objectId);
                return notification is not null ? Results.Ok(notification) : Results.NotFound();
            });

            // POST create notification
            group.MapPost("/", async (CreateNotificationDto dto, GovPayContext db) =>
            {
                var notification = new Notification
                {
                    GatewayInvoiceRef = dto.GatewayInvoiceRef,
                    Status = dto.Status,
                    SentTo = dto.SentTo,
                    Payload = dto.Payload,
                    SentAt = DateTime.UtcNow
                };

                db.Notifications.Add(notification);
                await db.SaveChangesAsync();

                return Results.Created($"/notifications/{notification.Id}", notification);
            });

            // PUT update notification
            group.MapPut("/{id}", async (string id, UpdateNotificationDto dto, GovPayContext db) =>
            {
                if (!ObjectId.TryParse(id, out var objectId))
                    return Results.BadRequest("Invalid ObjectId format.");

                var notification = await db.Notifications.FindAsync(objectId);
                if (notification is null)
                    return Results.NotFound();

                notification.Status = dto.Status;
                notification.Payload = dto.Payload;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE notification
            group.MapDelete("/{id}", async (string id, GovPayContext db) =>
            {
                if (!ObjectId.TryParse(id, out var objectId))
                    return Results.BadRequest("Invalid ObjectId format.");

                var notification = await db.Notifications.FindAsync(objectId);
                if (notification is null)
                    return Results.NotFound();

                db.Notifications.Remove(notification);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            return group;
        }
    }
}
