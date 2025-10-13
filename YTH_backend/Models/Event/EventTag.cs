using System.ComponentModel.DataAnnotations;

namespace YTH_backend.Models.Event;

public class EventTag
{
    [Key]
    public Guid Id { get; set; }
    
    [Required, MaxLength(256)]
    public string Name { get; set; } = null!;
    
    public ICollection<Event> Events { get; set; } = new List<Event>();
}