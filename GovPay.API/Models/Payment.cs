using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GovPay.API.Models
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("paymentRef")]
        public string PaymentRef { get; set; } = default!;

        [BsonElement("gatewayInvoiceRef")]
        public string GatewayInvoiceRef { get; set; } = default!;

        [BsonElement("pspCode")]
        public string PspCode { get; set; } = default!;

        [BsonElement("amountPaid")]
        public decimal AmountPaid { get; set; }

        [BsonElement("currency")]
        public string Currency { get; set; } = default!;

        [BsonElement("paymentDate")]
        public DateTime PaymentDate { get; set; }

        [BsonElement("receiptNumber")]
        public string ReceiptNumber { get; set; } = default!;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
