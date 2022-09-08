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
    public async Task<ItemResult<List<Review>>> GetReviews(Guid gameId, int page=1, int reviewsPerPage = 10, double minRating = 0, double maxRating=5)
    {
        List<Review> reviews = await Db.Reviews
            .Where(r =>
                r.GameId == gameId &&
                !r.IsDeleted &&
                r.Rating >= minRating &&
                r.Rating <= maxRating
                ).Skip(Math.Max(0,(page-1)) * reviewsPerPage).Take(reviewsPerPage)
                .ToListAsync();

        return reviews;
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
        game.AverageRating = game.TotalRating / game.NumberOfRatings;
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
        Guid userGuid, reviewerGuid;
        Guid.TryParse(existingUser.Id, out userGuid);
        Guid.TryParse(review.Reviewer.Id.ToString(), out reviewerGuid);

        if (!user.IsInRole(Roles.SuperAdmin) ||
            reviewerGuid != userGuid)
        {
            return "You do not have permission to delete this review.";
        }
        review.IsDeleted = true;
        Game? game = Db.Games.Where(g => g.GameId == review.GameId).FirstOrDefault();
        if (game != null)
        {
            if (game.NumberOfRatings == 0)
            {
                game.AverageRating = 0;
            }
            else
            {
                game.AverageRating = game.TotalRating / game.NumberOfRatings;
            }
        }
        Db.SaveChanges();
        return true;
    }
}
