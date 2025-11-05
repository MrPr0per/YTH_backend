using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.Course;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.UserConfiguration;

public class UserCourseRegistrationConfiguration : IEntityTypeConfiguration<UserCourseRegistration>
{
    public void Configure(EntityTypeBuilder<UserCourseRegistration> builder)
    {
        builder
            .ToTable("user_course_registration");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .HasIndex(x => x.UserId)
            .HasDatabaseName("IX_user_course_registration_user_id");
        
        builder
            .HasIndex(x => x.CourseId)
            .HasDatabaseName("IX_user_course_registration_course_id");
        
        builder
            .HasIndex(x => new { x.UserId, x.CourseId })
            .IsUnique()
            .HasDatabaseName("IX_user_course_registration_user_id_course_id");
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");

        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder
            .Property(x => x.CourseId)
            .IsRequired()
            .HasColumnName("course_id");

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserCourseRegistration)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_user_course_registration_user_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(x => x.Course)
            .WithMany(x => x.UserCourseRegistration)
            .HasForeignKey(x => x.CourseId)
            .HasConstraintName("fk_user_course_registration_course_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}