using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTH_backend.Models.Course;

public class UserCourseRegistration
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User.User User { get; set; } = null!;
    
    [Required]
    public Guid CourseId { get; set; }
    
    [ForeignKey(nameof(CourseId))]
    public Models.Course.Course Course { get; set; } = null!;
}