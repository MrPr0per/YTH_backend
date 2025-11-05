using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.UserConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("users");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder
            .Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .HasColumnName("username");
        
        builder
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("IX_users_email");
        
        builder
            .HasIndex(x => x.UserName)
            .IsUnique()
            .HasDatabaseName("IX_users_username");
        
        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .HasColumnName("email");
        
        builder
            .Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .HasColumnName("password");
        
        builder
            .Property(x => x.PasswordSalt)
            .IsRequired()
            .HasColumnType("text")
            .HasColumnName("password_salt");
        
        builder
            .Property(x => x.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasColumnType("varchar(50)")
            .HasColumnName("role");
        
        // builder
        //     .Property(x => x.MoodleLogin)
        //     .HasMaxLength(256)
        //     .HasColumnType("varchar(256)")
        //     .HasColumnName("moodle_login");
        //
        // builder
        //     .Property(x => x.MoodlePassword)
        //     .HasMaxLength(256)
        //     .HasColumnType("varchar(256)")
        //     .HasColumnName("moodle_password");
        //
        // builder
        //     .Property(x => x.MoodlePasswordSalt)
        //     .HasColumnType("text")
        //     .HasColumnName("moodle_password_salt");
        
        builder
            .HasMany(x => x.UserCourseRegistration)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_users_user_course_registration")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(x => x.UserEventRegistration)
            .WithOne(x => x.User)
            .HasConstraintName("fk_users_user_event_registration")
            .OnDelete(DeleteBehavior.Cascade);
    }
}