using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GovPay.API.Models
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("gatewayInvoiceRef")]
        public string GatewayInvoiceRef { get; set; } = default!;

        [BsonElement("status")]
        public string Status { get; set; } = default!;

        [BsonElement("sentTo")]
        public string SentTo { get; set; } = default!;

        [BsonElement("payload")]
        public string Payload { get; set; } = default!;

        [BsonElement("sentAt")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
