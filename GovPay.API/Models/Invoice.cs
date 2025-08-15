using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GovPay.API.Models
{
    public class Invoice
    {
        [BsonId] // Marks this as the _id field
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("gatewayInvoiceRef")]
        public string GatewayInvoiceRef { get; set; } = default!;

        [BsonElement("invoiceId")]
        public string InvoiceId { get; set; } = default!;

        [BsonElement("sellerCode")]
        public string SellerCode { get; set; } = default!;

        [BsonElement("sellerName")]
        public string SellerName { get; set; } = default!;

        [BsonElement("payerName")]
        public string PayerName { get; set; } = default!;

        [BsonElement("payerId")]
        public string PayerId { get; set; } = default!;

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("currency")]
        public string Currency { get; set; } = default!;

        [BsonElement("dueDate")]
        public DateTime DueDate { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "PENDING"; // Default status

        [BsonElement("description")]
        public string Description { get; set; } = default!;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
