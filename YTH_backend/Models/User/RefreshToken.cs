using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTH_backend.Models.User;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public Models.User.User User { get; set; } = null!;
    
    [Required]
    public string TokenHash { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public DateTime ExpiresAt { get; set; }
    
    // public bool Revoked { get; set; } = false;
    //
    // public Guid? NewRefreshTokenId { get; set; } = null;
    //
    // [ForeignKey("NewRefreshTokenId")]
    // public RefreshToken NewRefreshToken { get; set; } = null!;
}