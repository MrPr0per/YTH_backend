using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTH_backend.Models.Course;

public class Course
{
    [Key]
    public Guid Id { get; set; }
    
    // [Required]
    // public Guid CategoryId { get; set; }
    //
    // [ForeignKey("CategoryId")] 
    // public CourseCategory Category { get; set; } = null!;
    
    [Required,  MaxLength(512)]
    public string Name { get; set; } = null!;
    
    [MaxLength(512)]
    public string? ShortDescription { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [MaxLength(512)]
    public string? Link { get; set; }
    
    // public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();
    
    public ICollection<User.User> Users { get; set; } = new List<User.User>();
}