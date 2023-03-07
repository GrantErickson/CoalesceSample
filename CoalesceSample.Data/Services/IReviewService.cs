using CoalesceSample.Data.Models;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.DataAnnotations;
using IntelliTect.Coalesce.Models;
using System.Security.Claims;

namespace CoalesceSample.Data.Services;
[Coalesce, Service]
public interface IReviewService
{
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    public Task<ItemResult<List<Review>>> GetReviews(Guid gameId, DateTime? firstDate, DateTime? secondDate, int page = 1, int reviewsPerPage = 10, double minRating = 0, double maxRating = 5);

    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAuthorized, Roles = Roles.User)]
    public Task<ItemResult<Review>> AddReview(ClaimsPrincipal user, Guid gameId, string reviewTitle, string reviewBody, double rating);

    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAuthorized, Roles = Roles.User)]
    public Task<ItemResult> DeleteReview(ClaimsPrincipal user, Guid reviewId);
}
