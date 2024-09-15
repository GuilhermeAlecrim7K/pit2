using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public sealed record Product
{
    [Key]
    public Guid Id { get; private init; } = Guid.NewGuid();
    [StringLength(maximumLength: 30, MinimumLength = 2)]
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; internal set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; internal set; }
}