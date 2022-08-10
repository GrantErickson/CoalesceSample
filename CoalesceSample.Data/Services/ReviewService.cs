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
    public async Task<ItemResult<List<Review>>> GetReviews(int gameId)
    {
        Game? game = Db.Games
            .Where(g => g.GameId == gameId)
            .Include(g => g.Reviews)
            .FirstOrDefault();
        if (game == null)
        {
            return "Could not find the requested game";
        }
        return game.Reviews.ToList();
    }
    public async Task<ItemResult> addReview(ClaimsPrincipal user, int gameId, string reviewTitle, string reviewBody, double rating)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {

            return "Unable to find your account.";
        }
        ApplicationUser? existingUser = Db.Users.FirstOrDefault(u => u.Id == claim.Value);
        if (existingUser == null)
        {
            return "Unable to find tyourhe account.";
        }

        Game? game = await Db.Games.FirstOrDefaultAsync(g => g.GameId == gameId);

        if (game == null)
        {
            return "Unable to find the game.";
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
        game.TotalRating += rating;
        game.NumberOfRatings++;
        await Db.SaveChangesAsync();
        return true;
    }
}
