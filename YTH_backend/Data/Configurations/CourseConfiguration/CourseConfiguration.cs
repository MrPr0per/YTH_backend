using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.Course;

namespace YTH_backend.Data.Configurations.CourseConfiguration;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        // builder
        //     .HasIndex(x => x.CategoryId);
        //
        // builder
        //     .HasOne(x => x.Category)
        //     .WithMany(x => x.Courses)
        //     .HasForeignKey(x => x.CategoryId)
        //     .HasConstraintName("fk_courses_categories")
        //     .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(512)
            .HasColumnType("varchar(512)")
            .HasColumnName("name");
        
        builder
            .Property(x => x.ShortDescription)
            .HasMaxLength(512)
            .HasColumnType("varchar(512)")
            .HasColumnName("short_description");
        
        builder
            .Property(x => x.Description)
            .HasColumnType("text")
            .HasColumnName("description");
        
        builder
            .Property(x => x.Link)
            .HasMaxLength(512)
            .HasColumnType("varchar(512)")
            .HasColumnName("link");
        
        // builder
        //     .Property(x => x.CategoryId)
        //     .HasColumnName("category_id");
        
        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");

        // builder
        //     .HasMany(c => c.CourseTags)
        //     .WithMany(t => t.Courses)
        //     .UsingEntity(j => j.ToTable("courses_tags"));
        
        builder
            .HasMany(x => x.Users)
            .WithMany(x => x.Courses)
            .UsingEntity(j => j.ToTable("courses_users"));
        
        // builder
        //     .HasIndex(x => x.CategoryId)
        //     .HasDatabaseName("IX_category_id");
        
        builder
            .HasIndex(x => x.Name)
            .HasDatabaseName("IX_name");
    }
}