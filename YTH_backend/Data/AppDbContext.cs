using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using YTH_backend.Models.Course;
using YTH_backend.Models.Event;
using YTH_backend.Models.ExpertApplication;
using YTH_backend.Models.Infrastructure;
using YTH_backend.Models.Post;
using YTH_backend.Models.User;

namespace YTH_backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ExpertApplication> ExpertApplications => Set<ExpertApplication>();
    public DbSet<UserCourseRegistration> UserCourseRegistrations => Set<UserCourseRegistration>();
    public DbSet<UserEventRegistration> UserEventRegistrations => Set<UserEventRegistration>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<ExpertApplicationAction> ExpertApplicationActions => Set<ExpertApplicationAction>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}