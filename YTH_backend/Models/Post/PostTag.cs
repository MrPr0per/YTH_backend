using System.ComponentModel.DataAnnotations;

namespace YTH_backend.Models.Post;

public class PostTag
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(256)]
    public string Name { get; set; } = null!;
    
    public ICollection<Models.Post.Post> Posts { get; set; } = new List<Models.Post.Post>();
}