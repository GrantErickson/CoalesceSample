# Coalesce Tutorial

Learn Coalesce by creating a Board Game Management System. (BGMS)

## Prerequisites
* Visual Studio 2022 or later (is it possible with VS Code as well, but the instructions are for Visual Studio)
* SQL Server Local DB (it is possible to use another database by modifying the connection string and NuGet packages)
* Node installed
* Understanding of C#, ASP.NET, EF Core, etc.

## Steps

### 1. Create a scaffolded project

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

### 2. Add a simple class called Game
  1. Open the Solution file in Visual Studio
  2. In the `src` folder in the `\CoalesceSample.Data` project in the `Models` folder add a new class file called `Game.cs`
  3. Make class `public`
  4. Add a primary key property `GameId` as an int
  
```
    public int GameId { get; set; }
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
  12. Run the app with Kestral
  13. Note that there is now an editor for the Game class in the Application User Admin Table.
  14. Manually create your first game in the database. Notice the autosave will produce an error until the nullable fields have data.

### 3. Add Swagger
  1. Add the `Swashbuckle.AspNetCore` package to the project.

  3. In the `\CoalesceSample.Web\program.cs` file, add the SwaggerGen service to the builder:
```
    builder.Services.AddSwaggerGen();
```
  4. Further down in the HTTP Pipelines region, add the Swagger middleware to the development environment:
```
    app.UseSwagger()
    app.UseSwaggerUI();    
```
  5. Visit the Swagger endpoint at `localhost:5001/swagger`

### 4. Add a class with a parent: Genre
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
### 5. Add many to many with Tags
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

### 6. Create a read-only game list page
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

### 7. Add authentication with database accounts
  1. In the `\CoalesceSample.Data` folder, create a new static class called `Roles`
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
  3. In the `\CoalesceSample.Data\Services` folder, create a new interface called `ILoginService` annotated with `[Coalesce, Service]`
  4. 

### 8. Make the read-only page public

### 9. Anonymous Game viewing and liking

### 10. User login: Database accounts

### 11. User login: OAuth
