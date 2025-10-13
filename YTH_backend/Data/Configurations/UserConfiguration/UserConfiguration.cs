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
            .HasDatabaseName("UX_users_email");
        
        builder
            .HasIndex(x => x.UserName)
            .HasDatabaseName("IX_users_username");
        
        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .HasColumnName("email");
        
        builder
            .Property(x => x.Password)
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
        
        builder
            .Property(x => x.MoodleLogin)
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .HasColumnName("moodle_login");
        
        builder
            .Property(x => x.MoodlePassword)
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .HasColumnName("moodle_password");
        
        builder
            .Property(x => x.MoodlePasswordSalt)
            .HasColumnType("text")
            .HasColumnName("moodle_password_salt");
        
        builder
            .Property(x => x.IsEmailConfirmed)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnType("boolean")
            .HasColumnName("is_email_confirmed");
        
        builder
            .HasMany(x => x.Events)
            .WithMany(x => x.Users)
            .UsingEntity(j => j.ToTable("users_events"));
        
        builder
            .HasMany(x => x.Courses)
            .WithMany(x => x.Users)
            .UsingEntity(j => j.ToTable("users_courses"));
    }
}