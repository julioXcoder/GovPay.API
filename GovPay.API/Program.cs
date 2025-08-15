using GovPay.API.Controllers;
using GovPay.API.Data;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Mongo");
var dbName = builder.Configuration.GetConnectionString("MongoDatabaseName");

if (string.IsNullOrEmpty(connString) || string.IsNullOrEmpty(dbName))
{
    throw new InvalidOperationException("Connection string or database name is not configured.");
}

builder.Services.AddMongoDB<GovPayContext>(
    connString,
    dbName
);

var app = builder.Build();

app.MapPaymentEndPoints();
app.MapInvoiceEndPoints();
app.MapNotificationEndPoints();

app.Run();
