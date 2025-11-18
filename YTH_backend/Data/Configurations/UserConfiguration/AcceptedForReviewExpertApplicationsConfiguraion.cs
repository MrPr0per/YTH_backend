using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.UserConfiguration;

public class AcceptedForReviewExpertApplicationsConfiguraion : IEntityTypeConfiguration<AcceptedForReviewExpertApplications>
{
    public void Configure(EntityTypeBuilder<AcceptedForReviewExpertApplications> builder)
    {
        builder
            .ToTable("accepted_for_review_expert_applications");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.AdminId)
            .HasColumnName("admin_id");
        
        builder
            .Property(x => x.ExpertApplicationId)
            .HasColumnName("expert_application_id");

        builder
            .HasOne(x => x.ExpertApplication)
            .WithOne(x => x.AcceptedForReviewExpertApplication)
            .HasForeignKey<AcceptedForReviewExpertApplications>(x => x.ExpertApplicationId)
            .HasConstraintName("fk_accepted_for_review_expert_applications_expert_applications")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(x => x.Admin)
            .WithOne(x => x.AcceptedForReviewExpertApplication)
            .HasForeignKey<AcceptedForReviewExpertApplications>(x => x.AdminId)
            .HasConstraintName("fk_accepted_for_review_expert_applications_user")
            .OnDelete(DeleteBehavior.Cascade);
    }
}