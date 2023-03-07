using IntelliTect.Coalesce.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CoalesceSample.Data.Models;

#nullable disable
[InternalUse]
public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }

    // Required for coalesce/EF
    public ApplicationUser() { }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

#nullable restore
}
