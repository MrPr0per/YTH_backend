using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.Event;

namespace YTH_backend.Data.Configurations.EventConfiguration;

public class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
{
    public void Configure(EntityTypeBuilder<EventCategory> builder)
    {
        builder.ToTable("event_categories");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(c => c.Name)
            .HasMaxLength(256)
            .IsRequired()
            .HasColumnType("varchar(256)")
            .HasColumnName("name");
    }
}