# Coalesce Tutorial

Learn Coalesce by creating a Board Game Management System. (BGMS)

## Prerequisites
* Visual Studio 2022 or later (is it possible with VS Code as well, but the instructions are for Visual Studio)
* SQL Server Local DB (it is possible to use another database by modifying the connection string and NuGet packages)
* Node installed
* Understanding of C#, ASP.NET, EF Core, etc.

> **Notes**:
> * When referring to the file `Program.cs` in the instructions, it is referring to the file in the project `ColesceSample.Web`.


## Steps

### 1. Create the Template Project

These commands are designed for the Windows Console or Windows PowerShell. Substitute appropriate commands for Linux.

These commands are what you need to set up a Coalesce development environment

  1. `Mkdir CoalesceSample`
  2. `cd CoalesceSample`
  3. `dotnet new --install IntelliTect.Coalesce.Vue.Template`
  4. `dotnet new coalescevue`
  5. `cd \*.data`
  6. If EF tooling is not installed run: `dotnet tool install --global dotnet-ef`
  7. `dotnet ef migrations add Initial`
  8. `cd ..\*.web`
  9. `npm ci`
  10. `dotnet restore`
  11. `dotnet coalesce`
  12. `dotnet run`
  13. Browse to: [https://localhost:5001](https://localhost:5001/)

### 2. Add Swagger
  1. Add the `Swashbuckle.AspNetCore` package to the project.

  2. In `program.cs`, add the SwaggerGen service to the builder with bearer token authorization options:
```
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
```
  3. Further down in the HTTP Pipelines region, add the Swagger middleware to the development environment:
```
app.UseSwagger()
app.UseSwaggerUI();    
```
  4. Visit the Swagger endpoint at [https://localhost:5001/swagger](https://localhost:5001/swagger)

### 3. Set Up Identity and Authentication Middleware
  1. In the `ApplicationUser` class found in the Models folder, extend `IdentityUser` and remove the `ApplicationUserId` property.
  2. In `AppDbContext.cs`, extend `IdentityDbContext<ApplicationUser>` instead of `DbContext`
  3. Add the identity service to `program.cs`
```
services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider)
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
```
  4. Between `services.AddIdentity` and `services.AddControllersWithViews` add our authentication middleware:
```
services.AddAuthentication(auth =>
{
    auth.DefaultScheme = "JWT_OR_COOKIE";
    // set to null so the default scheme takes effect (was changed by .AddIdentity)
    auth.DefaultChallengeScheme = auth.DefaultAuthenticateScheme = null;
})
```
  5. In the Identity folder, create the public class `JwtConfiguration` to store data on how the token will be generated:
```
namespace CoalesceSample.Data.Identity;
public class JwtConfiguration
{
    public string SigningKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationInMinutes { get; set; } = 1440;
}
```
  6. Set the configuration values for this class in `appsettings.json` in a new section called `JwtConfig`:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CoalesceSample;Trusted_Connection=True;MultipleActiveResultSets=True;"
  },
  "JwtConfig": {
    "SigningKey": "SecretKeyToChange<-ChangeMe",
    "Issuer": "https://localhost:5001",
    "Audience": "https://localhost:5001"
  }
}
```
  7. Just above the middleware, add the JWT configuration as a singleton service:
```
JwtConfiguration jwtConfiguration = builder.Configuration.GetSection("JwtConfig").Get<JwtConfiguration>();
services.AddSingleton(jwtConfiguration);

```
  8. To this middleware, add the JWT Bearer configuration:
```
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
    options.Events = new JwtBearerEvents
    {

        OnMessageReceived = context =>
        {
            var path = context.Request.Path;
            // Pull the token from the querystring if it is present there.
            context.Token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (context.Request.QueryString.Value?.Contains("token") ?? false)
                context.Token = context.Request.Query.Where(q => q.Key == "token").First().Value;

            return Task.CompletedTask;
        },
    };
})
```
  9. Add the cookie configuration:
```
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Events.OnRedirectToLogin = c =>
    {
        c.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.FromResult<object>(null!);
    };
})
```
  10. Finally, add a policy to select the proper authentication scheme:
```
.AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
{
    // runs on each request
    options.ForwardDefaultSelector = context =>
    {
        // use jwt if there is a token set
        string authorization = context.Request.Headers[HeaderNames.Authorization];
        if (!string.IsNullOrEmpty(authorization) && !authorization.Contains("null"))
            return JwtBearerDefaults.AuthenticationScheme;

        // otherwise always check for cookie auth
        return IdentityConstants.ApplicationScheme;
    };
});
```
  11. With the authentication middleware in place, add authentication and authorization to the app in the HTTP pipeline directly after the build:
```
app.UseAuthentication();
app.UseAuthorization();
```
  12. Now that there is proper authentication set up, remove the dummy authentication from the HTTP pipeline region in `app.Use`.
```
app.Use(async (context, next) =>
{
    await next.Invoke();
});
```

### 4. Set Up User Login and Roles
  1. In a new folder called `Identity` in the `CoalesceSample.Data` project, create a new public static class called `Roles`
  2. Add static constants for SuperAdmin and User roles and a static array to track all roles
```
    public const string SuperAdmin = nameof(SuperAdmin);
    public const string User = nameof(User);

    public static readonly string[] AllRoles = new[]
    {
        SuperAdmin,
        User,
    };
```
  3. In the `CoalesceSample.Data` project create a `Services` folder and inside create a new public interface called `ILoginService` annotated with `[Coalesce, Service]`
  4. Create a new public class in the same folder called `LoginService` that implements `ILoginService`
  5. Register this service in `programs.cs` by adding the following scoped service:
```
services.AddScoped<ILoginService, LoginService>();
```
  6. In `LoginService.cs` add the following properties and constructor to inject dependencies:
```
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
```
  7. Add the method `Login` to the interface annotated with `[Execute(PermissionLevel=SecurityPermissionLevels.AllowAll)]` and the implementation:
```
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
```


### 5. Add a simple class called Game
  1. Open the Solution file in Visual Studio
  2. In the `\CoalesceSample.Data` project in the `Models` folder add a new class file called `Game.cs`
  3. Make the class `public`
  4. Add a primary key property `GameId` as a Guid
```
    public Guid GameId { get; set; }
```
  5. Add Other Properties
```
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double AverageDurationInHours { get; set; }
    public int MaxPlayers { get; set; }
    public int MinPlayers { get; set; }
```
  6. Add a DbSet to the AppDbContext
```
    public DbSet<Game> Games => Set<Game>();
```
  7. Open the Developer PowerShell terminal window
  8. From the `CoalesceSample.Data` folder run
  9. `Dotnet ef Migrations Add AddGame`
  10. `cd ..\*.web`
  11. `dotnet coalesce`
  12. Run the app with Kestrel
  13. Note that there is now an editor for the Game class in the Application User Admin Table.
  14. Manually create your first game in the database. Notice the autosave will produce an error until the nullable fields have data.

### 6. Add a class with a parent: Genre
  1. Add a primary key property `GenreId` as an int
```
    public int GenreId { get; set; }
```
  2. Add Other Properties
```
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Game> Games { get; set; } = new List<Game>();
```
  3. Add a DbSet to the AppDbContext
```
    public DbSet<GameGenre> GameGenres => Set<GameGenre>();
```
### 7. Add many to many with Tags
  1. Add a primary key property `TagId` as an int
```
    public int TagId { get; set; }
```
  2. Add Other Properties, you can find more information about the ManyToMany tag here on [the Coalesce docs](https://intellitect.github.io/Coalesce/modeling/model-components/attributes/many-to-many.html)
```
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [ManyToMany("Game")]
    public ICollection<GameTag> Games { get; set; } = new List<GameTag>();
```
  3. Add the many-to-many relationship to the Game class
```
    [ManyToMany("Tag")]
    public ICollection<GameTag> Tags { get; set; } = new List<GameTag>();
```
  4. Complete the many-to-many relationship by adding a class for the inner table called `GameTags` with the following properties
```
    public int GameTagId { get; set; }
    public int TagId { get; set; }
    public int GameId { get; set; }
```
  4. Add a DbSet to the AppDbContext
```
    public DbSet<GameTag> GameTags => Set<GameTag>();
```

### 8. Create a read-only game list page
  1. Create a new folder in `\CoalesceSameple.Data` called `services`
  2. Create a new public class called `GameService` in the `services` folder annotated with `[Coalesce, Service]`
  3. Add an AppDbContext property to the class and assign it in the constructor
```
    private AppDbContext Db { get; set; }

    public GameService(AppDbContext db)
    {
        Db = db;
    }
```
  4. Add the service as scoped to `Program.cs` in the `ConfigureServices` region
```
    services.AddScoped<GameService>();
```
  5. Add a method to get the list of games from the database annotated with `[Coalesce]`
```
    public async Task<ItemResult<List<Game>>> GetGames()
    {
        List<Game> games = await Db.Games.ToListAsync();
        if(!games.Any())
        {
            return "No games currently exist.";
        }
        return games;
    }
```
  6. In `\CoalesceSample.Web\Pages` create a new file called `GameList.vue`
  7. In the `<template>` section, wrap your game list in a `c-loader-status` element to allow your page to wait for the game service to return success before it loads the data.
```
    <c-loader-status
        v-slot
        :loaders="{
          'no-secondary-progress no-initial-content no-error-content': [
            gameService.getGames,
          ],
        }"
      >
      <--List of games components here-->
      </c-loader-status>
```
  8. Create an instance of the `GameServiceViewModel` class to get access to the methods in `GameService.cs`
```
  gameService = new GameServiceViewModel();
```
  9. Use the `created` method to get the list of games from the `GameService`
```
  async created() {
    await this.gameService.getGames();
  }
```
  10. Create a getter to get the list of game objects to use in the HTML section of the page
```
  get games() {
    return this.gameService.getGames.result;
  }
```
  11. When designing your list, use a `v-if` with the condition `gameService.getGames.wasSuccessful` to determine if you have data to display, and display an alternate message as appropriate.


### 8. Make the read-only page public

### 9. Anonymous Game viewing and liking

