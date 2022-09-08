using IntelliTect.Coalesce;
using IntelliTect.Coalesce.DataAnnotations;

namespace CoalesceSample.Data.Models;
public class Review
{
    public Guid ReviewId { get; set; }
    public double Rating { get; set; }
    public DateTime ReviewDate { get; set; }
    [InternalUse]
    public ApplicationUser Reviewer { get; set; } = null!;
    public string ReviewerName { get; set; } = null!;
    public string ReviewTitle { get; set; } = null!;
    public string ReviewBody { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
    public Guid GameId { get; set; }
    public Game ReviewedGame { get; set; } = null!;
}