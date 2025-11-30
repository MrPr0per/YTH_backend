using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTH_backend.Models.ExpertApplication;

public class ExpertApplicationAction
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid ExpertApplicationId { get; set; }

    [ForeignKey(nameof(ExpertApplicationId))]
    public Models.ExpertApplication.ExpertApplication ExpertApplication { get; set; } = null!;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public Enums.ExpertApplicationActionType ExpertApplicationActionType { get; set; }
    
    [Column(TypeName = "jsonb")]
    public string? Other { get; set; }
}