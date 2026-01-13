using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkifyApp.ApiService.Domain;

public class Client
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [EmailAddress]
    [MaxLength(150)]
    public required string Email { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? CompanyName { get; set; }

    // Storing historical changes in JSONB columns
    [Column(TypeName = "jsonb")]
    public string TimelineHistory { get; set; } = "[]";

    // Vector-based lead matching (using pgvector usually, represented here as float array)
    // Note: Requires pgvector extension in Postgres
    // [Column(TypeName = "vector(1536)")] // Example for OpenAI embeddings
    public float[]? LeadVector { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
