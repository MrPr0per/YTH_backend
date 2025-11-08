using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.ExpertApplicationConfiguration;

public class ExpertApplicationResolutionConfiguration : IEntityTypeConfiguration<ExpertApplicationResolution>
{
    public void Configure(EntityTypeBuilder<ExpertApplicationResolution> builder)
    {
        builder
            .ToTable("expert_application_resolution");

        builder
            .HasKey(x => x.Id);
        
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.DecidedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .HasColumnName("decided_at");
        
        builder
            .Property(x => x.DecidedById)
            .IsRequired()
            .HasColumnName("decided_by_id");
        
        builder
            .Property(x => x.IsApproved)
            .IsRequired()
            .HasColumnType("boolean")
            .HasColumnName("is_approved");
        
        builder
            .Property(x => x.Message)
            .IsRequired()
            .HasColumnType("text")
            .HasColumnName("message");
        
        builder
            .HasOne(x => x.ExpertApplication)
            .WithOne(x => x.Resolution)
            .HasForeignKey<ExpertApplicationResolution>(x => x.ApplicationId)
            .HasConstraintName("fk_expert_application_resolution_expert_application")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(x => x.DecidedByUser)
            .WithMany(x => x.ExpertApplicationResolutions)
            .HasForeignKey(x => x.DecidedById)
            .HasConstraintName("fk_expert_application_resolution_decided_by_id")
            .OnDelete(DeleteBehavior.Restrict);
            
    }
}