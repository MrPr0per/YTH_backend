using System.ComponentModel.DataAnnotations;
using YTH_backend.Enums;

namespace YTH_backend.Models.User;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required, MaxLength(256)]
    public string UserName { get; set; } = null!;
    
    [Required, MaxLength(256), EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required, MaxLength(256)]
    public string Password { get; set; } = null!;
    
    [Required]
    public string PasswordSalt { get; set; } = null!;

    [Required] 
    public bool IsEmailConfirmed { get; set; } = false;
    
    [Required]
    public Roles Role { get; set; }
    
    [MaxLength(256)]
    public string MoodleLogin { get; set; } = null!;
    
    [MaxLength(256)]
    public string MoodlePassword { get; set; } = null!;
    
    public string MoodlePasswordSalt { get; set; } = null!;
    
    public ICollection<Event.Event> Events { get; set; } = new List<Event.Event>();
    
    public ICollection<Course.Course> Courses { get; set; } = new List<Course.Course>();
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    
    public ICollection<AdminAppointment> AppointmentsMade { get; set; } = new List<AdminAppointment>();
    
    public ICollection<AdminAppointment> AppointmentsReceived { get; set; } = new List<AdminAppointment>();
    
    public ICollection<AdminAppointment>? AppointmentsRevoked { get; set; } = new List<AdminAppointment>();
    
    public ICollection<Post.Post> Posts { get; set; } = new List<Post.Post>();
}