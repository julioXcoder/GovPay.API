namespace GovPay.API.DTOs
{
    // Create Notification DTO
    public record CreateNotificationDto
    {
        public string GatewayInvoiceRef { get; init; } = default!;
        public string Status { get; init; } = default!;
        public string SentTo { get; init; } = default!;
        public string Payload { get; init; } = default!;
    }

    // Update Notification DTO
    public record UpdateNotificationDto
    {
        public string Status { get; init; } = default!;
        public string Payload { get; init; } = default!;
    }

    // Read Notification DTO
    public record NotificationDto
    {
        public string Id { get; init; } = default!;
        public string GatewayInvoiceRef { get; init; } = default!;
        public string Status { get; init; } = default!;
        public string SentTo { get; init; } = default!;
        public string Payload { get; init; } = default!;
        public DateTime SentAt { get; init; }
    }
}
