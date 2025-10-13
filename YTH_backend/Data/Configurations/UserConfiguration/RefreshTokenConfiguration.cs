using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.UserConfiguration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .ToTable("refresh_tokens");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id");
        
        builder
            .Property(x => x.TokenHash)
            .IsRequired()
            .HasColumnType("text")
            .HasColumnName("token_hash");
        
        builder
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        
        builder
            .Property(x => x.ExpiresAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasColumnName("expires_at");
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.UserId)
            .HasDatabaseName("IX_user_id");
    }
}