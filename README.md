# Coalesce Tutorial

- [Coalesce Tutorial](#coalesce-tutorial)
  - [1. Prerequisites](#1-prerequisites)
  - [2. Steps](#2-steps)
    - [2.1. Create the Template Project](#21-create-the-template-project)
    - [2.2. Add Swagger](#22-add-swagger)
    - [2.3. Set Up Identity and Authentication Middleware](#23-set-up-identity-and-authentication-middleware)
    - [2.4. Set Up User Login and Roles](#24-set-up-user-login-and-roles)
    - [2.5. Add a simple class called Game](#25-add-a-simple-class-called-game)
    - [2.6. Add a class with a parent: Genre](#26-add-a-class-with-a-parent-genre)
    - [2.7. Add many-to-many with Tags](#27-add-many-to-many-with-tags)
    - [2.8. Create a public game list page](#28-create-a-public-game-list-page)
    - [2.9. Make the read-only page public](#29-make-the-read-only-page-public)
    - [2.10. Anonymous Game viewing and liking](#210-anonymous-game-viewing-and-liking)

Learn Coalesce by creating a Board Game Management System. (BGMS)

## 1. Prerequisites

- Visual Studio 2022 or later (is it possible with VS Code as well, but the instructions are for Visual Studio)
- SQL Server Local DB (it is possible to use another database by modifying the connection string and NuGet packages)
- Node installed
- Understanding of C#, ASP.NET, EF Core, etc.

> **Notes**:
>
> - When referring to the file `Program.cs` in the instructions, always referes to the file in the project `ColesceSample.Web`.

## 2. Steps

### 2.1. Create the Template Project

These commands are designed for the Windows Console or Windows PowerShell. Substitute appropriate commands for Linux.

These commands are what you need to set up a Coalesce development environment

  1. `Mkdir CoalesceSample`
  2. `cd CoalesceSample`
  3. `dotnet new --install IntelliTect.Coalesce.Vue.Template`
  4. `dotnet new coalescevue`
  5. `cd *.data`
  6. If EF tooling is not installed run: `dotnet tool install --global dotnet-ef`
  7. `dotnet ef migrations add Initial`
  8. `cd ..\*.web`
  9. `npm ci`
  10. `dotnet restore`
  11. `dotnet coalesce`
  12. `dotnet run`
  13. Browse to: [https://localhost:5001](https://localhost:5001/)

### 2.2. Add Swagger

  1. Add the `Swashbuckle.AspNetCore` package to the project.
  2. In `program.cs`, add the SwaggerGen service to the builder with bearer token authorization options:

        ```cs
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

        ```cs
        app.UseSwagger()
        app.UseSwaggerUI();    
        ```

  4. Visit the Swagger endpoint at [https://localhost:5001/swagger](https://localhost:5001/swagger)

### 2.3. Set Up Identity and Authentication Middleware

  1. Add the `Microsoft.AspNetCore.Identity.EntityFrameworkCore` package to the project.
  2. In the `ApplicationUser` class found in the Models folder, extend `IdentityUser` and remove the `ApplicationUserId` property since `IdentityUser` provides an id property.
  3. In `AppDbContext.cs`, extend `IdentityDbContext<ApplicationUser>` instead of `DbContext`
  4. Add the identity service to `program.cs`

        ```cs
        services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AppDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider)
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
        ```

  5. Between `services.AddIdentity` and `services.AddControllersWithViews` add our authentication middleware:

        ```cs
        services.AddAuthentication(auth =>
        {
            auth.DefaultScheme = "JWT_OR_COOKIE";
            // set to null so the default scheme takes effect (was changed by .AddIdentity)
            auth.DefaultChallengeScheme = auth.DefaultAuthenticateScheme = null;
        })
        ```

  6. In the Identity folder, create the public class `JwtConfiguration` to store data on how the token will be generated:

        ```cs
        namespace CoalesceSample.Data.Identity;
        public class JwtConfiguration
        {
            public string SigningKey { get; set; } = null!;
            public string Issuer { get; set; } = null!;
            public string Audience { get; set; } = null!;
            public int ExpirationInMinutes { get; set; } = 1440;
        }
        ```

  7. Set the configuration values for this class in `appsettings.json` in a new section called `JwtConfig`:

        ```json
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

  8. Just above the middleware, add the JWT configuration as a singleton service:

        ```cs
        JwtConfiguration jwtConfiguration = builder.Configuration.GetSection("JwtConfig").Get<JwtConfiguration>();
        services.AddSingleton(jwtConfiguration);

        ```

  9. Add the Authorization middleware to the service, the default scheme will be specified and used to switch between JWT Bearer and Cookie auth:

        ```cs
        services.AddAuthentication(auth =>
        {
            auth.DefaultScheme = "JWT_OR_COOKIE";
            // set to null so the default scheme takes effect (was changed by .AddIdentity)
            auth.DefaultChallengeScheme = auth.DefaultAuthenticateScheme = null;
        })
        ```

  10. To this middleware, add the JWT Bearer configuration:

        ```cs
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

  11. Add the cookie configuration:

        ```cs
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Events.OnRedirectToLogin = c =>
            {
                c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.FromResult<object>(null!);
            };
        })
        ```

  12. Finally, add a policy to select the proper authentication scheme:

        ```cs
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

  13. With the authentication middleware in place, add authentication and authorization to the app in the HTTP pipeline directly after the build:

        ```cs
        app.UseAuthentication();
        app.UseAuthorization();
        ```

  14. Now that there is proper authentication set up, remove the dummy authentication from the HTTP pipeline region in `app.Use`.

        ```cs
        app.Use(async (context, next) =>
        {
            await next.Invoke();
        });
        ```

### 2.4. Set Up User Login and Roles

  1. In a new folder called `Identity` in the `CoalesceSample.Data` project, create a new public static class called `Roles`
  2. Add static constants for SuperAdmin and User roles and a static array to track all roles

        ```cs
        public const string SuperAdmin = nameof(SuperAdmin);
        public const string User = nameof(User);


        public static string[] AllRoles => typeof(Roles).GetFields().Select(role =>role.Name).ToArray();
        ```

  3. In the `CoalesceSample.Data` project create a `Services` folder and inside create a new public interface called `ILoginService` annotated with `[Coalesce, Service]`
  4. Create a new public class in the same folder called `LoginService` that implements `ILoginService`
  5. Register this service in `programs.cs` by adding the following scoped service:

        ```cs
        services.AddScoped<ILoginService, LoginService>();
        ```

  6. In `LoginService.cs` add the following properties and constructor to inject dependencies:

        ```cs
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

  7. To handle the Cookie auth, add the method `Login` to the interface annotated with `[Execute(PermissionLevel=SecurityPermissionLevels.AllowAll)]` and the implementation:

        ```cs
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

  8. To handle JWT auth, add the method `GetToken` to the interface annotated with `[Execute(PermissionLevel=SecurityPermissionLevels.AllowAll)]` and the implementation:

        ```cs
        public async Task<ItemResult<dynamic>> GetToken(string email, string password)
        {
            ApplicationUser? user = Db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && await UserManager.CheckPasswordAsync(user, password))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.SigningKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                    };

                var userRoles = await UserManager.GetRolesAsync(user);
                foreach (var role in userRoles)
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
                await SignInManager.SignInAsync(user, false);
                return new { token = jwtToken };
            }
            return "Unable to log in, please check your credentials.";
        }
        ```

  9. Finally, implement a logout function so users can sign out of the site. Add the method to the interface and annotate it with `[Execute(PermissionLevel=SecurityPermissionLevels.AllowAll)]`

        ```cs
        public async Task<ItemResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return true;
        }
        ```

  10. Some other useful methods are included in the `LoginService` for user management and may or may not be needed depending on the needed/wanted login features.

### 2.5. Add a simple class called Game

  1. In the `\CoalesceSample.Data` project in the `Models` folder add a new class file called `Game.cs`
  2. Make the class `public`
  3. Add a primary key property `GameId` as a Guid

        ```cs
        public Guid GameId { get; set; }
        ```

  4. Add Other Properties

        ```cs
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? ReleaseDate { get; set; }
        public double AverageDurationInHours { get; set; }
        public int MaxPlayers { get; set; }
        public int MinPlayers { get; set; }
        public int Likes { get; set; } = 0;
        ```

  5. Decorate the `Game` class with public read access. This will allow the `ListViewModel` we use later to retrieve game data for anonymous users. Access will be applied using `SecurityPermissionLevels` which is a enumerable provided by Coalesce useable in [security attributes](https://intellitect.github.io/Coalesce/modeling/model-components/attributes/security-attribute.html).

        ```cs
        [Read(SecurityPermissionLevels.AllowAll)]
        public class Game
        { ...
        ```

  6. Add a DbSet to `\CoalesceSample.Data\AppDbContext.cs`

        ```cs
        public DbSet<Game> Games => Set<Game>();
        ```

  7. Open the Developer PowerShell terminal window`
  8. From the `CoalesceSample.Data` folder run:
     1. `Dotnet ef Migrations Add AddGame`
     2. `cd ..\*.web`
     3. `dotnet coalesce`
  9. Run the app with Kestrel
  10. Note that there is now an editor for the Game class in the Application User Admin Table.
  11. Manually create your first game in the database. Notice the autosave will produce an error until the nullable fields have data.

### 2.6. Add a class with a parent: Genre

  1. Create a new public class in the `Models` folder called `Genre`
  2. Add a primary key property `GenreId` as an int

        ```cs
        public int GenreId { get; set; }
        ```

  3. Add references to the Genre model to `Game.cs`

        ```cs
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        ```

  4. Add Other Properties

        ```cs
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        ```

  5. Add a DbSet to the AppDbContext

        ```cs
        public DbSet<GameGenre> GameGenres => Set<GameGenre>();
        ```

  6. In the console, from the `CoalesceSample.Data` folder, run:
     1. `Dotnet ef Migrations Add AddGenres`
     2. `cd ..\*.web`
     3. `dotnet coalesce`
  7. After restarting the application, there will now be a `Genre` table in the Admin Page Links.

### 2.7. Add many-to-many with Tags

  1. Add a primary key property `TagId` as an int

        ```cs
        public int TagId { get; set; }
        ```

  2. Add Other Properties, you can find more information about the `[ManyToMany]` tag here on [the Coalesce docs](https://intellitect.github.io/Coalesce/modeling/model-components/attributes/many-to-many.html)

        ```cs
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [ManyToMany("Game")]
        public ICollection<GameTag> Games { get; set; } = new List<GameTag>();
        ```

  3. Add the many-to-many relationship to the Game class

        ```cs
        [ManyToMany("Tag")]
        public ICollection<GameTag> Tags { get; set; } = new List<GameTag>();
        ```

  4. Complete the many-to-many relationship by adding the public class for the inner table called `GameTags` with the following properties

        ```cs
        public int GameTagId { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
        ```

  5. Add DbSets to `AppDbContext.cs`

        ```cs
        public DbSet<Tag> GameTags => Set<Tag>();
        public DbSet<GameTag> GameTags => Set<GameTag>();
        ```

  6. In the console, from the `CoalesceSample.Data` folder, run:
     1. `Dotnet ef Migrations Add AddTags`
     2. `cd ..\*.web`
     3. `dotnet coalesce`

### 2.8. Create a public game list page

  1. Create a new folder in `\CoalesceSameple.Data` called `services`
  2. Create a new public class called `GameService` in the `services` folder annotated with `[Coalesce, Service]`
  3. Add an AppDbContext property to the class and assign it in the constructor

        ```cs
        private AppDbContext Db { get; set; }
        public GameService(AppDbContext db)
        {
            Db = db;
        }
        ```

  4. Add the service as scoped to `Program.cs` in the `ConfigureServices` region

        ```cs
        services.AddScoped<GameService>();
        ```

  5. To get the list of games, the GamesListViewModel will be used. [ListViewModels](https://intellitect.github.io/Coalesce/stacks/vue/layers/viewmodels.html#listviewmodels) are provided by Coalesce for models marked by the `[Coalesce]` attribute. To modify the ListViewModel, a [DataSource](https://intellitect.github.io/Coalesce/modeling/model-components/data-sources.html#standard-data-source) will be used to specify the data that is included when the GameListViewModel loads data. The data source will be specified in the `Game.cs` file in the `Game` class.

        ```cs
        [DefaultDataSource]
        [Coalesce]
        public class GameDataSource : StandardDataSource<Game, AppDbContext>
        {
            public GameDataSource(CrudContext<AppDbContext> context) : base(context) { }
            public override IQueryable<Game> GetQuery(IDataSourceParameters parameters)
            {
                IQueryable<Game> query = Db.Games
                    .AsNoTracking()
                    .Include(g => g.Genre)
                    .Include(g => g.GameTags)
                        .ThenInclude(gt=>gt.Tag)
                    .Include(g => g.Reviews);
                return query;
            }
        }
        ```

  6. In `\CoalesceSample.Web\src\views` create a new file called `GameList.vue` to hold the components for displaying the full list of games.
  7. In the `<template>` section, wrap components that will use the game list in a `c-loader-status` element to wait for the list of game objects to return successfully before other components use the data in the HTML. If the `ItemResult` returned by the call has an error, the error message will be displayed. The `c-loader-status` documentation can be found [here](https://intellitect.github.io/Coalesce/stacks/vue/coalesce-vue-vuetify/components/c-loader-status.html) for more information on the available customization options.

        ```html
        <c-loader-status
        v-slot
        class="ma-4"
        :loaders="{
            'no-secondary-progress no-initial-content': [gamesList.$load],
        }"
        >
        <!--Other components here-->
        </c-loader-status>
        ```

  8. Create an instance of the `GameListViewModel` class to load the data

        ```ts
        await this.gamesList.$load();
        ```

  9. Using the `v-for` property, loop through the loaded items which can be accessed with `gameList.$items` to display some of the game information on the list page. This information can be made into a clickable link to take the users to a more detailed information page about the particular game.
  10. In the `views` folder create another new page called `GameDetails.vue`. When navigating to the new page, the Id of the clicked game will be sent to the page in the URL.
      1. Router entry

            ```ts
            {
            path: "/gamedetails/:gameId",
            name: "game-details",
            component: () => import("./views/GameDetails.vue"),
            props: true,
            },
            ```

      2. Navigation call:

            ```ts
            async gameDetails(gameId: string) {
                await this.$router.push({
                name: "game-details",
                params: { gameId: gameId },
                });
            }
            ```

      3. Set up the property:

            ```ts
            @Prop({ required: true })
            gameId!: string;
            ```

  11. Create a `GameViewModel` on `GameDetails` that will be used to load, view, and update information about the game. `ViewModel` objects are generated by Coalesce from your models. Detailed information about the functionality and options is available in the [docs](https://intellitect.github.io/Coalesce/stacks/vue/layers/viewmodels.html#viewmodels).

        ```ts
        game: GameViewModel = new GameViewModel();
        ```

  12. Use the `created` method in `GameDetails.vue` to load the game information into the `GameViewModel`

        ```ts
        await this.game.$load(this.gameId);
        ```

  13. Create a getter to get the list of game objects to use in the HTML section of the page. This is also a good time to load the game's image data and place it on the game object to use on the page.

        ```ts
        get games() {
        let image = new ImageViewModel(this.gameService.getGameImage.result);
        this.gameViewModel = new GameViewModel(
            this.gameService.getGameDetails.result!
        );
        this.gameViewModel.image = image;
        return this.gameViewModel;
        }
        ```

### 2.9. Make the read-only page public

### 2.10. Anonymous Game viewing and liking
