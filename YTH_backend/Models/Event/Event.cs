using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YTH_backend.Enums;

namespace YTH_backend.Models.Event;

public class Event
{
    [Key]
    public Guid Id { get; set; }
    
    // [Required]
    // public Guid CategoryId { get; set; }
    //
    // [ForeignKey("CategoryId")]
    // public EventCategory Category { get; set; } = null!;
    
    [Required, MaxLength(512)]
    public string Name { get; set; } = null!;
    
    [Required]
    public DateTime Date { get; set; }
    
    public string? Description { get; set; } = null!;
    
    [MaxLength(512)]
    public string? ShortDescription { get; set; }
    
    [Required]
    public EventTypes Type { get; set; }
    
    [MaxLength(512)]
    public string? Address { get; set; } = null!;

    public ICollection<User.User> Users { get; set; } = new List<User.User>();
    
    // public ICollection<EventTag> Tags { get; set; } = new List<EventTag>();
}