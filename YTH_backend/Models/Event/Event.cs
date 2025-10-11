using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YTH_backend.Enums;

namespace YTH_backend.Models.Event;

public class Event
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    public EventCategory Category { get; set; } = null!;
    
    [Required, MaxLength(256)]
    public string Name { get; set; } = null!;
    
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    public string? Description { get; set; } = null!;
    
    [MaxLength(256)]
    public string? ShortDescription { get; set; }
    
    [Required]
    public EventTypes Type { get; set; }
    
    [MaxLength(256)]
    public string? Address { get; set; } = null!;

    public ICollection<User.User> Users { get; set; } = new List<User.User>();
    
    public ICollection<EventTag> Tags { get; set; } = new List<EventTag>();
}