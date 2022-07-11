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
  9. `npm i`
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
    public DbSet<Game>; Games => Set<Game>();
```

  7. Open the Developer PowerShell terminal window
  8. From the `CoalesceSample.Data` folder run
  9. `Dotnet ef Migrations Add AddGame`
  10. `cd ..\*.web`
  11. `dotnet coalesce`
  12. Run the app with Kestral
  13. Note that there is now an editor for the Game class

### Add Swagger

### Add a class with a parent: Genre

### Add many to many with Tags

### Create a read-only game list page

### Add authentication with database accounts

### Make the read-only page public

### Anonymous Game viewing and liking

### User login: Database accounts

### User login: OAuth
