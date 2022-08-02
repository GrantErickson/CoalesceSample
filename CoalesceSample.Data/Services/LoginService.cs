using CoalesceSample.Data.Identity;
using CoalesceSample.Data.Models;
using IntelliTect.Coalesce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoalesceSample.Data.Services;
public class LoginService : ILoginService
{
    private AppDbContext Db { get; set; }
    private SignInManager<ApplicationUser> SignInManager { get; }
    private UserManager<ApplicationUser> UserManager { get; }
    private JwtConfiguration JwtConfiguration { get; }

    public LoginService(AppDbContext db, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, JwtConfiguration jwtConfiguration)
    {
        Db = db;
        SignInManager = signInManager;
        UserManager = userManager;
        JwtConfiguration = jwtConfiguration;
    }
    public async Task<ItemResult> Login(string email, string password)
    {
        SignInResult? result = await SignInManager.PasswordSignInAsync(email, password, false, false);

        if (result.Succeeded)
        {
            return true;
        }
        else
        {
            return "Unable to log in, please check your credentials.";
        }
    }
    public async Task<ItemResult<dynamic>> GetToken(string email, string password)
    {
        ApplicationUser? user = Db.Users.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            if (await UserManager.CheckPasswordAsync(user, password))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.SigningKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email)
                };

                var userRoles = await UserManager.GetRolesAsync(user);
                foreach(var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = new JwtSecurityToken(
                    issuer: JwtConfiguration.Issuer,
                    audience: JwtConfiguration.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(JwtConfiguration.ExpirationInMinutes),
                    signingCredentials: credentials
                    );
                string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return new { token = jwtToken };
            }
        }
        return "Unable to log in, please check your credentials.";
    }

    public async Task<ItemResult> Logout()
    {
        await SignInManager.SignOutAsync();
        return true;
    }

    public async Task<ItemResult> CreateAccount(string name, string email, string password)
    {
        if (Db.Users.Any(u => u.Email == email))
        {
            return "The provided email address is already in use.";
        }
        else
        {
            ApplicationUser newUser = new ApplicationUser(name, email) { UserName = email };
            IdentityResult? createUserResult = await UserManager.CreateAsync(newUser, password);
            if (createUserResult.Succeeded)
            {
                await UserManager.AddToRoleAsync(newUser, Roles.User);
                await Db.SaveChangesAsync();
                return true;
            }
            return $"Unable to create the account: {createUserResult.Errors}";
        }
    }

    public async Task<ItemResult> ChangePassword(ClaimsPrincipal user, string currentPassword, string newPassword)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim != null)
        {

            ApplicationUser? existingUser = Db.Users.FirstOrDefault(u => u.Id == claim.Value);
            if (existingUser == null)
            {
                return "Unable to find the account.";
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(existingUser, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                return "Unable to update the password.";
            }

            return true;
        }
        else
        {
            return "Unauthorized to change password";
        }
    }

    public ItemResult IsLoggedIn(ClaimsPrincipal user)
    {
        if (SignInManager.IsSignedIn(user))
        {
            return true;
        }
        else
        {
            return "You are not signed in";
        }
    }
}
