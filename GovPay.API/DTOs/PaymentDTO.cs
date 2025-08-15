namespace GovPay.API.DTOs
{
    // Create Payment DTO
    public record CreatePaymentDto
    {
        public string PaymentRef { get; init; } = default!;
        public string GatewayInvoiceRef { get; init; } = default!;
        public string PspCode { get; init; } = default!;
        public decimal AmountPaid { get; init; }
        public string Currency { get; init; } = default!;
        public DateTime PaymentDate { get; init; }
        public string ReceiptNumber { get; init; } = default!;
    }

    // Update Payment DTO
    public record UpdatePaymentDto
    {
        public string PspCode { get; init; } = default!;
        public decimal AmountPaid { get; init; }
        public string Currency { get; init; } = default!;
        public DateTime PaymentDate { get; init; }
        public string ReceiptNumber { get; init; } = default!;
    }

    // Read Payment DTO
    public record PaymentDto
    {
        public string Id { get; init; } = default!;
        public string PaymentRef { get; init; } = default!;
        public string GatewayInvoiceRef { get; init; } = default!;
        public string PspCode { get; init; } = default!;
        public decimal AmountPaid { get; init; }
        public string Currency { get; init; } = default!;
        public DateTime PaymentDate { get; init; }
        public string ReceiptNumber { get; init; } = default!;
        public DateTime CreatedAt { get; init; }
    }
}
