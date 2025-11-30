using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Infrastructure;
using YTH_backend.Models.ExpertApplication;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.ExpertApplicationConfiguration;

public class ExpertApplicationActionConfiguration : IEntityTypeConfiguration<ExpertApplicationAction>
{
    public void Configure(EntityTypeBuilder<ExpertApplicationAction> builder)
    {
        builder
            .ToTable("expert_application_action");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.ExpertApplicationId)
            .IsRequired()
            .HasColumnName("expert_application_id");

        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()");

        builder
            .Property(x => x.ExpertApplicationActionType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(100)
            .HasColumnName("expert_application_action_type");

        builder
            .Property(x => x.Other)
            .HasColumnName("other")
            .HasColumnType("jsonb");
            // .HasConversion(
            //     // object → JSON (сериализация)
            //     payload => payload == null
            //         ? null 
            //         : JsonSerializer.Serialize(payload, JsonPayloadConverter.Default.ActionPayload),
            //     // JSON → object (десериализация)
            //     json => string.IsNullOrWhiteSpace(json) 
            //         ? null 
            //         : JsonSerializer.Deserialize<ActionPayload>(json, JsonPayloadConverter.Default.ActionPayload)
            // );
        
        builder
            .HasOne(x => x.ExpertApplication)
            .WithMany(x => x.Actions)
            .HasForeignKey(x => x.ExpertApplicationId)
            .HasConstraintName("fk_expert_application_action_expert_application")
            .OnDelete(DeleteBehavior.Cascade);
    }
}