using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTH_backend.Models.User;

public class AdminAppointment
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid AppointorId { get; set; }
    
    [ForeignKey("AppointorId")]
    public User Appointor { get; set; } = null!;
    
    [Required]
    public Guid AppointeeId { get; set; }
    
    [ForeignKey("AppointeeId")]
    public User Appointee { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? RevokedAt { get; set; } = null;
    
    public Guid? RevokedById { get; set; } = null;
    
    [ForeignKey("RevokedById")]
    public User? Revoker { get; set; } = null;
    
    public string? RevokeReason { get; set; } = null;
}