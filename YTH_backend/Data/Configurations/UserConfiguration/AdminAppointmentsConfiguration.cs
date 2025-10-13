using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.UserConfiguration;

public class AdminAppointmentsConfiguration : IEntityTypeConfiguration<AdminAppointments>
{
    public void Configure(EntityTypeBuilder<AdminAppointments> builder)
    {
        builder
            .ToTable("admin_appointments");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.AppointorId)
            .HasColumnName("appointor_id");
        
        builder
            .Property(x => x.AppointeeId)
            .HasColumnName("appointee_id");
        
        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        
        builder
            .Property(x => x.RevokedAt)
            .HasColumnType("timestamp with time zone")
            .HasColumnName("revoked_at");
        
        builder
            .Property(x => x.RevokedById)
            .HasColumnName("revoked_by_id");
        
        builder
            .Property(x => x.RevokeReason)
            .HasColumnType("text")
            .HasColumnName("revoke_reason");
        
        builder.HasOne(x => x.Appointee)
            .WithMany(x => x.AppointmentsReceived)
            .HasForeignKey(x => x.AppointeeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.Appointor)
            .WithMany(x => x.AppointmentsMade)
            .HasForeignKey(x => x.AppointorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.Revoker)
            .WithMany(x => x.AppointmentsRevoked)
            .HasForeignKey(x => x.RevokedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasIndex(x => x.AppointeeId)
            .HasDatabaseName("IX_appointee_id");
        
        builder
            .HasIndex(x => x.AppointorId)
            .HasDatabaseName("IX_appointor_id");
        
        
    }
}