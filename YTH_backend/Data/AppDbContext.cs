using Microsoft.EntityFrameworkCore;
using YTH_backend.Models.Course;
using YTH_backend.Models.Event;
using YTH_backend.Models.Post;
using YTH_backend.Models.User;

namespace YTH_backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();
    //public DbSet<CourseCategory> CourseCategories => Set<CourseCategory>();
    //public DbSet<CourseTag> CourseTags => Set<CourseTag>();
    public DbSet<Event> Events => Set<Event>();
    //public DbSet<EventCategory> EventCategories => Set<EventCategory>();
   // public DbSet<EventTag> EventTags => Set<EventTag>();
    public DbSet<Post> Posts => Set<Post>();
    //public DbSet<PostCategory> PostCategories => Set<PostCategory>();
    //public DbSet<PostTag> PostTags => Set<PostTag>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ExpertApplication> ExpertApplications => Set<ExpertApplication>();
    public DbSet<ExpertApplicationResolution> ExpertApplicationResolutions => Set<ExpertApplicationResolution>();
    public DbSet<UserCourseRegistration> UserCourseRegistrations => Set<UserCourseRegistration>();
    public DbSet<UserEventRegistration> UserEventRegistrations => Set<UserEventRegistration>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AcceptedForReviewExpertApplications> AcceptedForReviewExpertApplications => Set<AcceptedForReviewExpertApplications>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}