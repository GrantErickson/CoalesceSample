using CoalesceSample.Data.Models;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoalesceSample.Data.Services;
public class ApplicationUserService : IApplicationUserService
{
    private AppDbContext Db { get; set; }
    public ApplicationUserService(AppDbContext db)
    {
        Db = db;
    }

    public async Task<ItemResult<List<Guid>>> GetUserReviews(ClaimsPrincipal user)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {
            return new();
        }
        ApplicationUser? existingUser = Db.Users.FirstOrDefault(u => u.Id == claim.Value);
        if (existingUser == null)
        {
            return new();
        }
        IQueryable<Review> reviews = Db.Reviews.Where(r=>r.Reviewer == existingUser);
        return reviews.Where(r => !r.IsDeleted).Select(r => r.ReviewId).ToList();
    }

    public async Task<ItemResult<List<string>>> GetRoles(ClaimsPrincipal user)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {

            return new List<string>();
        }

        var roles = new List<string>();
        foreach (var role in Roles.AllRoles)
        {
            if (user.IsInRole(role))
            {
                roles.Add(role);
            }
        }
        return roles;
    }

    public async Task<ItemResult> HasRole(ClaimsPrincipal user, string role)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {

            return "Unable to find the requested user";
        }

        return user.IsInRole(role);
    }
}
