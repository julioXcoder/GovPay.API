using GovPay.API.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace GovPay.API.Data
{
    public class GovPayContext : DbContext
    {
        // DbSets for MongoDB collections
        public DbSet<Invoice> Invoices { get; init; } = default!;
        public DbSet<Payment> Payments { get; init; } = default!;
        public DbSet<Notification> Notifications { get; init; } = default!;

        // Constructor for dependency injection
        public GovPayContext(DbContextOptions<GovPayContext> options)
            : base(options)
        { }

        // Optional factory method if you have an IMongoDatabase instance
        public static GovPayContext Create(IMongoDatabase database) =>
            new(new DbContextOptionsBuilder<GovPayContext>()
                .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                .Options);

        // Map entities to their MongoDB collections
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invoice>().ToCollection("invoices");
            modelBuilder.Entity<Payment>().ToCollection("payments");
            modelBuilder.Entity<Notification>().ToCollection("notifications");
        }
    }
}
