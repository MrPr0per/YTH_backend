using System.ComponentModel.DataAnnotations;
using YTH_backend.Data.Configurations.UserConfiguration;
using YTH_backend.Enums;
using YTH_backend.Models.Course;
using YTH_backend.Models.Event;

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
    public string PasswordHash { get; set; } = null!;
    
    [Required, MaxLength(512)]
    public string PasswordSalt { get; set; } = null!;
    
    [Required]
    public Roles Role { get; set; }
    
    // [MaxLength(256)]
    // public string MoodleLogin { get; set; } = null!;
    //
    // [MaxLength(256)]
    // public string MoodlePassword { get; set; } = null!;
    //
    // public string MoodlePasswordSalt { get; set; } = null!;
    
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    
    public ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();
    
    public ICollection<ExpertApplicationResolution> ExpertApplicationResolutions { get; set; } = new List<ExpertApplicationResolution>();
    
    public ICollection<UserEventRegistration> UserEventRegistration { get; set; } = new List<UserEventRegistration>();
    
    public ICollection<UserCourseRegistration> UserCourseRegistration { get; set; } = new List<UserCourseRegistration>();
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    
    public ICollection<Post.Post> Posts { get; set; } = new List<Post.Post>();
    
    public AcceptedForReviewExpertApplications? AcceptedForReviewExpertApplication { get; set; }
}