using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTH_backend.Models.User;

public class Request
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}