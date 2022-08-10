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
    public Task<ItemResult<List<Review>>> GetReviews(int gameId);

    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAuthorized)]
    public Task<ItemResult> addReview(ClaimsPrincipal user, int gameId, string reviewTitle, string reviewBody, double rating);
}
