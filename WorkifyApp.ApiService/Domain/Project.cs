using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkifyApp.ApiService.Domain;

public enum ProjectStatus
{
    NotStarted,
    InProgress,
    OnHold,
    Completed,
    Cancelled
}

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public Guid ClientId { get; set; }
    public Client? Client { get; set; }

    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; }

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }

    // Navigation property for tasks could be added here
}
