using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.Post;

namespace YTH_backend.Data.Configurations.PostConfigurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder
            .ToTable("posts");
        
        builder
            .HasKey(c => c.Id);
        
        builder
            .Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(512)
            .HasColumnType("varchar(512)")
            .HasColumnName("title");
        
        // builder
        //     .Property(c => c.ShortDescription)
        //     .HasMaxLength(512)
        //     .HasColumnType("varchar(512)")
        //     .HasColumnName("short_description");
        
        builder
            .Property(x => x.Description)
            .IsRequired()
            .HasColumnType("text")
            .HasColumnName("description");
        
        builder
            .Property(x => x.PostStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasColumnType("varchar(50)")
            .HasColumnName("status");
        
        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        
        builder
            .Property(x => x.AuthorId)
            .HasColumnName("author_id");
        
        builder
            .Property(x => x.ImageUrl)
            .HasMaxLength(512)
            .HasColumnType("varchar(512)")
            .HasColumnName("image_url");
        
        // builder
        //     .Property(x => x.CategoryId)
        //     .HasColumnName("category_id");
        //
        // builder
        //     .HasOne(x => x.Category)
        //     .WithMany(x => x.Posts)
        //     .HasForeignKey(x => x.CategoryId)
        //     .HasConstraintName("fk_posts_categories")
        //     .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Posts)
            .HasForeignKey(x => x.AuthorId)
            .HasConstraintName("fk_posts_users")
            .OnDelete(DeleteBehavior.Restrict);
        
        // builder
        //     .HasMany(x => x.Tags)
        //     .WithMany(x => x.Posts)
        //     .UsingEntity(j => j.ToTable("posts_tags"));
        //
        // builder
        //     .HasIndex(x => x.CategoryId)
        //     .HasDatabaseName("IX_category_id");
    }
}