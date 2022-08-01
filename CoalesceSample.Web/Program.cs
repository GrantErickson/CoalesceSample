using IntelliTect.Coalesce;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CoalesceSample.Data;
using CoalesceSample.Data.Services;
using CoalesceSample.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CoalesceSample.Data.Identity;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Explicit declaration prevents ASP.NET Core from erroring if wwwroot doesn't exist at startup:
    WebRootPath = "wwwroot"
});

builder.Services.AddSwaggerGen();

builder.Logging
    .AddConsole()
    // Filter out Request Starting/Request Finished noise:
    .AddFilter<ConsoleLoggerProvider>("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Warning);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

#region Configure Services
var services = builder.Services;

services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), opt => opt
        .EnableRetryOnFailure()
        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    ));

services.AddScoped<GameService>();
services.AddScoped<ILoginService, LoginService>();

services.AddCoalesce<AppDbContext>();

services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider)
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();

services
    .AddMvc()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    });
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Login";
            options.LogoutPath = "/";
            options.AccessDeniedPath = "Login";
        });
var jwtConfiguration = builder.Configuration.GetSection("JwtConfig").Get<JwtConfiguration>();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfiguration.Issuer,
            ValidAudience = jwtConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SigningKey)),
        };
    });
services.AddControllersWithViews();

#endregion



#region Configure HTTP Pipeline

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();


    app.UseDeveloperExceptionPage();

    app.UseViteDevelopmentServer(c =>
    {
        c.DevServerPort = 5002;
    });

    app.MapCoalesceSecurityOverview("coalesce-security");

    // TODO: Dummy authentication for initial development.
    // Replace this with ASP.NET Core Identity, Windows Authentication, or some other scheme.
    // This exists only because Coalesce restricts all generated pages and API to only logged in users by default.
    app.Use(async (context, next) =>
    {
        Claim[] claims = new[] { new Claim(ClaimTypes.Name, "developmentuser") };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await context.SignInAsync(context.User = new ClaimsPrincipal(identity));

        await next.Invoke();
    });
    // End Dummy Authentication.
}

app.UseAuthentication();
app.UseAuthorization();

var containsFileHashRegex = new Regex(@"\.[0-9a-fA-F]{8}\.[^\.]*$", RegexOptions.Compiled);
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // vite puts 8-hex-char hashes before the file extension.
        // Use this to determine if we can send a long-term cache duration.
        if (containsFileHashRegex.IsMatch(ctx.File.Name))
        {
            ctx.Context.Response.GetTypedHeaders().CacheControl =
                new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromDays(30) };
        }
    }
});

// For all requests that aren't to static files, disallow caching by default.
// Individual endpoints may override this.
app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new CacheControlHeaderValue { NoCache = true, NoStore = true, };

    await next();
});

app.MapControllers();

// API fallback to prevent serving SPA fallback to 404 hits on API endpoints.
app.Map("/api/{**any}", () => Results.NotFound());

app.MapFallbackToController("Index", "Home");

#endregion



#region Launch

// Initialize/migrate database.
using (var scope = app.Services.CreateScope())
{
    var serviceScope = scope.ServiceProvider;

    // Run database migrations.
    using var db = serviceScope.GetRequiredService<AppDbContext>();
    db.Initialize();

    RoleManager<IdentityRole> roleManager = serviceScope.GetRequiredService<RoleManager<IdentityRole>>();

    foreach(string role in Roles.AllRoles)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();

#endregion
