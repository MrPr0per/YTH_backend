using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.UserConfiguration;

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
            .HasColumnType("text")
            .HasColumnName("message");
        
        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        
        builder
            .Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasColumnName("status");
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.ExpertApplications)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_expert_applications_users")
            .OnDelete(DeleteBehavior.Restrict);
    }
}