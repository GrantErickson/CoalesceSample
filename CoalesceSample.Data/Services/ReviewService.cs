using CoalesceSample.Data.Models;
using IntelliTect.Coalesce.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoalesceSample.Data.Services;
public class ReviewService : IReviewService
{
    private AppDbContext Db { get; set; }

    public ReviewService(AppDbContext db)
    {
        Db = db;
    }
    public async Task<ItemResult<List<Review>>> GetReviews(Guid gameId)
    {
        Game? game = Db.Games
            .Where(g => g.GameId == gameId)
            .Include(g => g.Reviews.Where(r => !r.IsDeleted))
            .FirstOrDefault();
        if (game == null)
        {
            return "Could not find the requested game";
        }
        return game.Reviews.ToList();
    }
    public async Task<ItemResult<Review>> AddReview(ClaimsPrincipal user, Guid gameId, string reviewTitle, string reviewBody, double rating)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {

            return "Unable to find your account.";
        }

        ApplicationUser? existingUser = Db.Users.FirstOrDefault(u => u.Id == claim.Value);
        if (existingUser == null)
        {
            return "Unable to find your account.";
        }

        Game? game = await Db.Games.FirstOrDefaultAsync(g => g.GameId == gameId);
        if (game == null)
        {
            return "Unable to find the game.";
        }

        if (string.IsNullOrEmpty(reviewTitle.Trim()) || string.IsNullOrEmpty(reviewBody.Trim()))
        {
            return "Cannot add a review without a title and body";
        }
        Review newReview = new()
        {
            ReviewTitle = reviewTitle.Trim(),
            ReviewBody = reviewBody.Trim(),
            Rating = Math.Clamp(rating, 0, 10),
            Reviewer = existingUser,
            ReviewerName = existingUser.Name,
            ReviewDate = DateTime.Now,
        };
        game.Reviews.Add(newReview);
        await Db.SaveChangesAsync();
        return newReview;
    }

    public async Task<ItemResult> DeleteReview(ClaimsPrincipal user, Guid reviewId)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {
            return "Unable to find your account.";
        }
        ApplicationUser? existingUser = Db.Users.FirstOrDefault(u => u.Id == claim.Value);
        if (existingUser == null)
        {
            return "Unable to find your account.";
        }

        Review? review = await Db.Reviews
            .Include(r => r.ReviewedGame)
            .Include(r => r.Reviewer)
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.ReviewedGame != null && !r.IsDeleted);
        if (review == null)
        {
            return "Unable to find the review.";
        }
        ItemResult<Guid> userGuid = Guid.Parse(existingUser.Id);
        ItemResult<Guid> reviewerGuid = Guid.Parse(review.Reviewer.Id);
        bool guidsValid = userGuid.WasSuccessful && reviewerGuid.WasSuccessful;
        
        if (!user.IsInRole(Roles.SuperAdmin) &&
            guidsValid &
            reviewerGuid.Object != userGuid.Object)
        {
            return "You do not have permission to delete this review.";
        }
        review.IsDeleted = true;
        Db.SaveChanges();
        return true;
    }
}
