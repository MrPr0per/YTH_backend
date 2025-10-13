using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.Post;

namespace YTH_backend.Data.Configurations.PostConfigurations;

public class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
{
    public void Configure(EntityTypeBuilder<PostCategory> builder)
    {
        builder.ToTable("post_categories");
        
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