using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTH_backend.Models.User;

public class AcceptedForReviewExpertApplications
{
    [Key] 
    public Guid Id { get; set; }

    [Required]
    public Guid AdminId { get; set; }

    [ForeignKey(nameof(AdminId))] 
    public User Admin { get; set; } = null!;
    
    [Required]
    public Guid ExpertApplicationId { get; set; }
    
    [ForeignKey(nameof(ExpertApplicationId))]
    public ExpertApplication ExpertApplication { get; set; } = null!;
}