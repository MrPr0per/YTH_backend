// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using YTH_backend.Models.Post;
//
// namespace YTH_backend.Data.Configurations.PostConfigurations;
//
// public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
// {
//     public void Configure(EntityTypeBuilder<PostTag> builder)
//     {
//         builder
//             .ToTable("post_tags");
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