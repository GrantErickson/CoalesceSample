using IntelliTect.Coalesce;
using IntelliTect.Coalesce.DataAnnotations;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoalesceSample.Data.Services;
[Coalesce, Service]
public interface IApplicationUserService
{
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAuthorized)]
    public Task<ItemResult<List<string>>> GetRoles(ClaimsPrincipal user);

    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAuthorized)]
    public Task<ItemResult> HasRole(ClaimsPrincipal user, string role);
}
