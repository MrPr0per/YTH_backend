using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.ExpertApplication;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.ExpertApplicationConfiguration;

public class ExpertApplicationConfiguration : IEntityTypeConfiguration<ExpertApplication>
{
    public void Configure(EntityTypeBuilder<ExpertApplication> builder)
    {
        builder
            .ToTable("expert_applications");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .HasIndex(x => x.UserId)
            .HasDatabaseName("IX_expert_applications_user_id");
        
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.Message)
            .HasMaxLength(512)
            .HasColumnType("text")
            .HasColumnName("message");
        
        builder
            .Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasColumnName("status");

        builder
            .Property(x => x.AcceptedBy)
            .HasColumnName("accepted_by");
        
        builder
            .Property(x => x.IsApproved)
            .HasColumnType("bool")
            .HasColumnName("is_approved");

        builder
            .Property(x => x.ResolutionMessage)
            .HasMaxLength(512)
            .HasColumnType("varchar(512)")
            .HasColumnName("resolution_message");
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.ExpertApplications)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_expert_applications_users")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.AcceptedByUser)
            .WithMany(x => x.AcceptedByExpertApplications)
            .HasForeignKey(x => x.AcceptedBy)
            .HasConstraintName("fk_expert_applications_users_accepted_by")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasCheckConstraint(
            "CK_expert_applications_status_fields_consistency",
            @"(
                   (status IN ('NotSent','Sent') AND accepted_by IS NULL AND is_approved IS NULL AND resolution_message IS NULL)
                   OR
                   (status = 'AcceptedForReview' AND accepted_by IS NOT NULL AND is_approved IS NULL AND resolution_message IS NULL)
                   OR
                   (status = 'Reviewed' AND accepted_by IS NOT NULL AND is_approved IS NOT NULL AND resolution_message IS NOT NULL)
                 )"
        );
    }
}