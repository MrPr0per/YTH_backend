using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.Event;

namespace YTH_backend.Data.Configurations.EventConfiguration;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder
            .ToTable("events");
        
        builder
            .HasKey(e => e.Id);
        
        // builder
        //     .HasOne(x => x.Category)
        //     .WithMany(x => x.Events)
        //     .HasForeignKey(x => x.CategoryId)
        //     .HasConstraintName("fk_events_categories")
        //     .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(512)
            .HasColumnType("varchar(512)")
            .HasColumnName("name");
        
        builder
            .Property(e => e.Description)
            .HasColumnType("text")
            .HasColumnName("description");
        
        // builder
        //     .Property(e => e.CategoryId)
        //     .HasColumnName("category_id");
        
        builder
            .Property(x => x.ShortDescription)
            .HasMaxLength(512)
            .HasColumnType("varchar(512)")
            .HasColumnName("short_description");
        
        builder
            .Property(x => x.Address)
            .HasMaxLength(512)
            .HasColumnType("varchar(512)")
            .HasColumnName("address");
        
        builder
            .Property(x => x.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasColumnType("varchar(50)")
            .HasColumnName("type");

        builder
            .Property(x => x.Date)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasColumnName("date");
        
        // builder
        //     .HasMany(x => x.Tags)
        //     .WithMany(x => x.Events)
        //     .UsingEntity(j => j.ToTable("events_tags"));
        
        builder
            .HasMany(x => x.Users)
            .WithMany(x => x.Events)
            .UsingEntity(j => j.ToTable("events_users"));

        builder
            .HasIndex(x => x.Date)
            .HasDatabaseName("IX_date");
        
        // builder
        //     .HasIndex(x => x.CategoryId)
        //     .HasDatabaseName("IX_category_id");
        
        builder
            .HasIndex(x => x.Name)
            .HasDatabaseName("IX_name");
    }
}