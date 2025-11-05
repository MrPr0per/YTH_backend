using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YTH_backend.Enums;

namespace YTH_backend.Models.User;

public class UserCourseRegistration
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    
    [Required]
    public Guid CourseId { get; set; }
    
    [ForeignKey(nameof(CourseId))]
    public Course.Course Course { get; set; } = null!;
}