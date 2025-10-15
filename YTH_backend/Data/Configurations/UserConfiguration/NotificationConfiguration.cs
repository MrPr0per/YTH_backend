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
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder
            .Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("message");
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_users_notifications")
            .OnDelete(DeleteBehavior.Cascade);
    }
}