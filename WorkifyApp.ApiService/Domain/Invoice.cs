using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkifyApp.ApiService.Domain;

public enum InvoiceStatus
{
    Draft,
    Sent,
    Paid,
    Overdue,
    Cancelled
}

public class Invoice
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ClientId { get; set; }
    public Client? Client { get; set; }

    public DateTimeOffset InvoiceDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? PaidDate { get; set; }

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(100)]
    public string? StripeInvoiceId { get; set; }

    [MaxLength(500)]
    public string? PdfUrl { get; set; }

    public ICollection<InvoiceLineItem> Items { get; set; } = new List<InvoiceLineItem>();
}
