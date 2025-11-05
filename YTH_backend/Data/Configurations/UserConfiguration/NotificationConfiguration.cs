using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.UserConfiguration;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder
            .ToTable("notifications");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .HasIndex(x => x.UserId)
            .HasDatabaseName("IX_notifications_user_id");

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
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .HasColumnName("title");
        
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder
            .Property(x => x.NotificationText)
            .IsRequired()
            .HasColumnType("text")
            .HasColumnName("notification_text");
        
        builder
            .Property(x => x.IsRead)
            .IsRequired()
            .HasDefaultValue("false")
            .HasColumnType("boolean")
            .HasColumnName("is_read");
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_users_notifications")
            .OnDelete(DeleteBehavior.Cascade);
    }
}