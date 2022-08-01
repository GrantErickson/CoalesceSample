using System.Security.Claims;
namespace CoalesceSample.Data.Services;

public interface IScopedOperationContext
{
    ClaimsPrincipal? User { get; }
}
