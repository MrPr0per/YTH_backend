// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using YTH_backend.Models.Event;
//
// namespace YTH_backend.Data.Configurations.EventConfiguration;
//
// public class EventTagConfiguration : IEntityTypeConfiguration<EventTag>
// {
//     public void Configure(EntityTypeBuilder<EventTag> builder)
//     {
//         builder
//             .ToTable("event_tags");
//         
//         builder
//             .HasKey(c => c.Id);
//         
//         builder
//             .Property(c => c.Id)
//             .ValueGeneratedOnAdd()
//             .HasColumnName("id");
//         
//         builder.Property(x => x.Name)
//             .IsRequired()
//             .HasMaxLength(256)
//             .HasColumnType("varchar(256)")
//             .HasColumnName("name");
//     }
// }