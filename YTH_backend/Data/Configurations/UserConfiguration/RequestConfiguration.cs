using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTH_backend.Models.User;

namespace YTH_backend.Data.Configurations.UserConfiguration;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder
            .ToTable("requests");
        
        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.UserId)
            .HasDatabaseName("IX_requests_user_id");
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Requests)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_users_requests")
            .OnDelete(DeleteBehavior.Cascade);
    }
}