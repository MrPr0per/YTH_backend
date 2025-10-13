using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.Course;

namespace YTH_backend.Data.Configurations.CourseConfiguration;

public class CourseCategoryConfiguration : IEntityTypeConfiguration<CourseCategory> 
{
    public void Configure(EntityTypeBuilder<CourseCategory> builder)
    {
        builder.ToTable("course_categories");
        
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