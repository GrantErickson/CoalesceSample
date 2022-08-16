using CoalesceSample.Data.Dto;
using CoalesceSample.Data.Identity;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.DataAnnotations;
using IntelliTect.Coalesce.Models;
using System.Security.Claims;

namespace CoalesceSample.Data.Services;
[Coalesce, Service]
public interface ILoginService
{
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    Task<ItemResult> Login(string email, string password);

    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    Task<ItemResult<dynamic>> GetToken(string email, string password);

    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    Task<ItemResult> Logout();

    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    Task<ItemResult> CreateAccount(string name, string email, string password);

    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAuthorized)]
    Task<ItemResult> ChangePassword(ClaimsPrincipal user, string currentPassword, string newPassword);

    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    Task<ItemResult> IsLoggedIn(ClaimsPrincipal user);
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    Task<ItemResult<UserInfoDto>> GetUserInfo(ClaimsPrincipal user);
}
