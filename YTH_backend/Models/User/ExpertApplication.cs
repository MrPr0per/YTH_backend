using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YTH_backend.Enums;

namespace YTH_backend.Models.User;

public class ExpertApplication
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required]
    public string Message { get; set; }

    [Required] 
    public ExpertApplicationStatus Status { get; set; } = ExpertApplicationStatus.NotSent;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public ExpertApplicationResolution? Resolution { get; set; }
}