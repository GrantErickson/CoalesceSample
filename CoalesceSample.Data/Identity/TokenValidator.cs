using IntelliTect.Coalesce.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoalesceSample.Data.Identity;
public class TokenValidator : ISecurityTokenValidator
{
    public TokenValidator(JwtConfiguration jwtConfiguration)
    {
        this.JwtConfiguration = jwtConfiguration;
        User = new();
        Token = new JwtSecurityToken();
    }

    public bool CanValidateToken => true;

    public int MaximumTokenSizeInBytes { get; set; }
    JwtConfiguration JwtConfiguration { get; set; }
    ClaimsPrincipal User { get; set; }
    SecurityToken Token { get; set; }

    public bool CanReadToken(string securityToken)
    {
        if (securityToken == null)
            return false;
        JwtSecurityTokenHandler tokenHandler = new();
        try
        {
            SecurityToken validatedToken;
            tokenHandler.ValidateToken(securityToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JwtConfiguration.Issuer,
                ValidAudience = JwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.SigningKey)),
                RoleClaimType = "role",
                NameClaimType = "name",
            },
            out validatedToken);

            return validatedToken != null;
        }
        catch (Exception)
        { return false; }
    }

    public ItemResult<ClaimsPrincipal> Validate(string token)
    {
        if (token == null)
            return false;
        JwtSecurityTokenHandler tokenHandler = new();
        try
        {
            SecurityToken validatedToken;
            var output = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JwtConfiguration.Issuer,
                ValidAudience = JwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.SigningKey)),
            },
            out validatedToken);
            User = output;
            return User;
            var jwt = (JwtSecurityToken)validatedToken;
            Token = validatedToken;
            var identity = new ClaimsIdentity(jwt.Claims, "Bearer");
            User = new ClaimsPrincipal(identity);
            return User;
        }
        catch (Exception) { return false; }
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        Validate(securityToken);
        validatedToken = Token ?? new JwtSecurityToken();
        return User;
    }
}
