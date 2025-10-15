// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using YTH_backend.Models.Course;
//
// namespace YTH_backend.Data.Configurations.CourseConfiguration;
//
// public class CourseTagConfiguration : IEntityTypeConfiguration<CourseTag>
// {
//     public void Configure(EntityTypeBuilder<CourseTag> builder)
//     {
//         builder
//             .ToTable("course_tags");
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