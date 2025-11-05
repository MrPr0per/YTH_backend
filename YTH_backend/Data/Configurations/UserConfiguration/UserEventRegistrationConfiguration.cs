using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.UserConfiguration;

public class UserEventRegistrationConfiguration : IEntityTypeConfiguration<UserEventRegistration>
{
    public void Configure(EntityTypeBuilder<UserEventRegistration> builder)
    {
        builder
            .ToTable("user_event_registrations");
        
        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.UserId)
            .HasDatabaseName("IX_user_event_registration_user_id");
        
        builder
            .HasIndex(x => x.EventId)
            .HasDatabaseName("IX_user_event_registration_event_id");
        
        builder
            .HasIndex(x => new { x.UserId, x.EventId })
            .IsUnique()
            .HasDatabaseName("IX_user_event_registration_user_id_event_id");
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder
            .Property(x => x.EventId)
            .IsRequired()
            .HasColumnName("event_id");
        
        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserEventRegistration)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_user_event_registrations_users")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.UserEventRegistration)
            .HasForeignKey(x => x.EventId)
            .HasConstraintName("fk_user_event_registrations_events")
            .OnDelete(DeleteBehavior.Cascade);
    }
}