using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YTH_backend.Enums;

namespace YTH_backend.Models.Post;

public class Post
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid AuthorId { get; set; }
    
    [ForeignKey(nameof(AuthorId))]
    public User.User User { get; set; } = null!;
    
    // [Required]
    // public Guid CategoryId { get; set; }
    //
    // [ForeignKey("CategoryId")]
    // public PostCategory Category { get; set; } =  null!;
    
    [Required, MaxLength(512)]
    public string Title { get; set; } = null!;
    
    // [MaxLength(512)]
    // public string ShortDescription { get; set; } = null!;
    
    [Required]
    public string Description { get; set; } = null!;

    [Required] 
    public PostStatus PostStatus { get; set; } = PostStatus.Posted;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // public ICollection<PostTag> Tags { get; set; } = new List<PostTag>();
}