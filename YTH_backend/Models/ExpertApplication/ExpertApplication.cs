using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YTH_backend.Enums;

namespace YTH_backend.Models.ExpertApplication;

public class ExpertApplication
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User.User User { get; set; } = null!;

    [Required, MaxLength(512)]
    public string Message { get; set; } = null!;

    [Required] 
    public ExpertApplicationStatus Status { get; set; } = ExpertApplicationStatus.Created;
    
    public Guid? AcceptedBy { get; set; }
    
    [ForeignKey(nameof(AcceptedBy))]
    public User.User? AcceptedByUser { get; set; }
    
    public bool? IsApproved { get; set; }
    
    [MaxLength(512)]
    public string? ResolutionMessage { get; set; }
    
    
    public ICollection<ExpertApplicationAction> Actions { get; set; } = null!;
}