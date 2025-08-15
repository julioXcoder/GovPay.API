namespace GovPay.API.DTOs
{
    // Create Invoice DTO
    public record CreateInvoiceDto
    {
        public string GatewayInvoiceRef { get; init; } = default!;
        public string InvoiceId { get; init; } = default!;
        public string SellerCode { get; init; } = default!;
        public string SellerName { get; init; } = default!;
        public string PayerName { get; init; } = default!;
        public string PayerId { get; init; } = default!;
        public decimal Amount { get; init; }
        public string Currency { get; init; } = default!;
        public DateTime DueDate { get; init; }
        public string Description { get; init; } = default!;
    }

    // Update Invoice DTO
    public record UpdateInvoiceDto
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; } = default!;
        public DateTime DueDate { get; init; }
        public string Status { get; init; } = default!; // e.g., PENDING, PAID, CANCELLED
        public string Description { get; init; } = default!;
    }

    // Read Invoice DTO
    public record InvoiceDto
    {
        public string Id { get; init; } = default!;
        public string GatewayInvoiceRef { get; init; } = default!;
        public string InvoiceId { get; init; } = default!;
        public string SellerCode { get; init; } = default!;
        public string SellerName { get; init; } = default!;
        public string PayerName { get; init; } = default!;
        public string PayerId { get; init; } = default!;
        public decimal Amount { get; init; }
        public string Currency { get; init; } = default!;
        public DateTime DueDate { get; init; }
        public string Status { get; init; } = default!;
        public string Description { get; init; } = default!;
        public DateTime CreatedAt { get; init; }
    }
}
