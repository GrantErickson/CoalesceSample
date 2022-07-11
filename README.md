# CoalesceSample

## Coalesce Tutorial

Learn Coalesce by creating a board game management system. (BGMS)

1. Create a simple project
  1. Mkdir CoalesceSample
  2. cd CoalesceSample
  3. dotnet new --install IntelliTect.Coalesce.Vue.Template
  4. dotnet new coalescevue
  5. cd \*.data
  6. If EF tooling not installed: dotnet tool install --global dotnet-ef
  7. dotnet ef migrations add Initial
  8. cd ..\*.web
  9. npm i
  10. dotnet restore
  11. dotnet coalesce
  12. dotnet run
  13. Browse to: [https://localhost:5001](https://localhost:5001/)
2. Add a simple class called Game
  1. Open the Solution file in Visual Studio
    1. All above commands are what you need to run in development
  2. In src folder in the \*.Data project in the Models folder add a new class file
  3. Make class public
  4. Add a primary key property &#39;[classname]Id&#39; as an int
    1. publicint GameId { get; set; }
  5. Add Other Properties
    1. publicstring Name { get; set; } = null!;
    2. publicstring Description { get; set; } = null!;
    3. publicdouble AverageDurationInHours { get; set; }
    4. publicint MaxPlayers { get; set; }
    5. publicint MinPlayers { get; set; }
  6. Add a DbSet to the AppDbContext
    1. public DbSet\&lt;Game\&gt; Games =\&gt; Set\&lt;Game\&gt;();
  7. Open the Developer PowerShell terminal window
  8. From the \*.Data folder run
  9. Dotnet ef Migrations Add &#39;[NewlyAddedClassName]&#39;
  10. cd ..\*.web
  11. dotnet coalesce
  12. Run the app with Kestral
  13. Note that there is now an editor for the Game class
3. Add Swagger
4. Add a class with a parent: Genre
5. Add many to many with Tags
6. Create a read-only game list page
7. Add authentication with database accounts
8. Make the read-only page public
9.

Anonymous Game viewing and liking

User login (Database accounts, OAuth) (could do Windows at some point)