using System.ComponentModel.DataAnnotations;

namespace YTH_backend.Models.Course;

public class CourseTag
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(256)]
    public string Name { get; set; } = null!;
    
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}