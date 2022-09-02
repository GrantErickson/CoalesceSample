using CoalesceSample.Data;
using CoalesceSample.Data.Identity;
using CoalesceSample.Data.Models;
using CoalesceSample.Data.Services;
using IntelliTect.Coalesce;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Explicit declaration prevents ASP.NET Core from erroring if wwwroot doesn't exist at startup:
    WebRootPath = "wwwroot"
});
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo { Title = "Coalesce Sample", Version = "v1" });
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new List<string>()
        }
    });
});

// Use better requirements for production deployments!
//https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;
});

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
services.AddScoped<IReviewService, ReviewService>();
services.AddScoped<IApplicationUserService, ApplicationUserService>();
services.AddScoped<TokenValidator>();

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


JwtConfiguration jwtConfiguration = builder.Configuration.GetSection("JwtConfig").Get<JwtConfiguration>();
services.AddSingleton(jwtConfiguration);

services.AddAuthentication(auth =>
    {
        auth.DefaultScheme = "JWT_OR_COOKIE";
        auth.DefaultChallengeScheme = "JWT_OR_COOKIE";
        //auth.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //auth.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //auth.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
        options.SaveToken = true;
        options.SecurityTokenValidators.Clear();
        options.SecurityTokenValidators.Add(new TokenValidator(jwtConfiguration));
        options.Events = new JwtBearerEvents
        {

            OnMessageReceived = context =>
            {
                var path = context.Request.Path;
                // Pull the token from the querystring if it is present there.
                context.Token = context.Request.Headers["Authorization"].ToString().Replace("bearer ", "");
                if (context.Request.QueryString.Value?.Contains("token") ?? false)
                    context.Token = context.Request.Query.Where(q => q.Key == "token").First().Value;

                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                return Task.CompletedTask;
            },
        };
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Events.OnRedirectToLogin = c =>
        {
            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.FromResult<object>(null!);
        };
    })
    .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
    {
        // runs on each request
        options.ForwardDefaultSelector = context =>
        {
            // filter by auth type
            string authorization = context.Request.Headers[HeaderNames.Authorization];
            if (context.Request.QueryString.Value?.Contains("token") ?? false)
                return JwtBearerDefaults.AuthenticationScheme;
            if (!string.IsNullOrEmpty(authorization) && !authorization.Contains("null"))
                return JwtBearerDefaults.AuthenticationScheme;

            // otherwise always check for cookie auth
            return CookieAuthenticationDefaults.AuthenticationScheme;
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(
        "JWT_OR_COOKIE"
        ).RequireAssertion(_ => true).Build();
});

services.AddControllersWithViews();
#endregion



#region Configure HTTP Pipeline

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

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
        await next.Invoke();
        var test = context;
        Console.WriteLine(test.User.Identity.Name);
        Console.WriteLine(test.User.Identity.AuthenticationType);
        Console.WriteLine(test.User.Identity.IsAuthenticated);
        if (test.User.Identity.IsAuthenticated)
        {
            Console.WriteLine("AUTHENTICATED DATA:");
            test.User.Claims.ToList().ForEach(x => Console.Write(x.ToString() + ", "));
            Console.WriteLine(test.User.Identities);
        }
    });
    // End Dummy Authentication.
}


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

    foreach (string role in Roles.AllRoles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    if (!db.Users.Any())
    {
        var userAccount = new ApplicationUser()
        {
            Name = "User",
            Email = "user@intellitect.com",
            UserName = "user@intellitect.com",
        };
        var adminAccount = new ApplicationUser()
        {
            Name = "Admin",
            Email = "admin@intellitect.com",
            UserName = "admin@intellitect.com",
        };
        UserManager<ApplicationUser>? userManager = serviceScope.GetService<UserManager<ApplicationUser>>();
        if (userManager != null)
        {
            await userManager.CreateAsync(userAccount, "user");
            await userManager.AddToRoleAsync(userAccount, Roles.User);
            await userManager.CreateAsync(adminAccount, "admin");
            await userManager.AddToRoleAsync(adminAccount, Roles.User);
            await userManager.AddToRoleAsync(adminAccount, Roles.SuperAdmin);
        }
    }
}

app.Run();

#endregion
