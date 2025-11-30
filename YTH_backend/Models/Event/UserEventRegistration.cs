using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTH_backend.Models.Event;

public class UserEventRegistration
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User.User User { get; set; } = null!;
    
    [Required]
    public Guid EventId { get; set; }
    
    [ForeignKey(nameof(EventId))]
    public Models.Event.Event Event { get; set; } = null!;
}