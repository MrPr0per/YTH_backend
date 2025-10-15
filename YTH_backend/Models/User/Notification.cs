using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTH_backend.Models.User;

public class Notification
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
    
    [Required, MaxLength(256)]
    public string Message { get; set; } = null!;
}