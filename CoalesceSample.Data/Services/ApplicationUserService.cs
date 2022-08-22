﻿using IntelliTect.Coalesce.Models;
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
    public async Task<ItemResult<Dto.UserInfoDto>> GetUserInfo(ClaimsPrincipal user)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {
            return "Cannot find user";
        }
        var dbUser = Db.Users.Where(u=> u.userI)
        var userDto = new Dto.UserInfoDto()

    }
    public async Task<ItemResult<List<string>>> GetRoles(ClaimsPrincipal user)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {

            return new List<string>();
        }

        var roles = new List<string>();
        foreach(var role in Roles.AllRoles)
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
