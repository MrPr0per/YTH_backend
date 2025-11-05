using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace YTH_backend.Models.User;

public class ExpertApplicationResolution
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime DecidedAt { get; set; } = DateTime.Now;

    [Required]
    public Guid DecidedById { get; set; }
    
    [ForeignKey(nameof(DecidedById))]
    public User DecidedByUser { get; set; } = null!;
    
    [Required]
    public bool IsApproved { get; set; }
    
    [Required]
    public string Message { get; set; }
    
    public Guid? ApplicationId { get; set; }
    
    [ForeignKey(nameof(ApplicationId))]
    public ExpertApplication? ExpertApplication { get; set; }
}