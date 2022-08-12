using Microsoft.EntityFrameworkCore;
using CoalesceSample.Data.Models;
using IntelliTect.Coalesce;
using System.Security.Claims;
using CoalesceSample.Data.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CoalesceSample.Data;

[Coalesce]
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<GameTag> GameTags => Set<GameTag>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Image> Images => Set<Image>();

    public AppDbContext()
    {
    }

    public IScopedOperationContext OperationContext { get; set; }
    public ClaimsPrincipal? User => OperationContext.User;

    public AppDbContext(IScopedOperationContext operationContext, DbContextOptions<AppDbContext> options) : base(options)
    {
        OperationContext = operationContext;
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Remove cascading deletes.
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    /// <summary>
    /// Migrates the database and sets up items that need to be set up from scratch.
    /// </summary>
    public void Initialize()
    {
        try
        {
            this.Database.Migrate();
            SeedTags();
            SeedGeneres();
            SeedGames();
            SeedGameTags();

            // TODO: Or, use Database.EnsureCreated() instead:
            // this.Database.EnsureCreated();
        }
        catch (InvalidOperationException e) when (e.Message == "No service for type 'Microsoft.EntityFrameworkCore.Migrations.IMigrator' has been registered.")
        {
            // this exception is expected when using an InMemory database
        }
    }

    public void SeedTags()
    {
        List<Tag> addTags = new();
        if (!Tags.Any(t => t.Name == "Multiplayer"))
        {
            addTags.Add(
            new Tag()
            {
                Name = "Multiplayer",
                Description = "Play with friends"
            });
        }
        if (!Tags.Any(t => t.Name == "Singleplayer"))
        {
            addTags.Add(
            new Tag()
            {
                Name = "Singleplayer",
                Description = "Play alone"
            });
        }
        if (!Tags.Any(t => t.Name == "Online"))
        {
            addTags.Add(
            new Tag()
            {
                Name = "Online",
                Description = "Play online"
            });
        }
        this.Tags.AddRange(addTags);
        SaveChanges();
    }

    public void SeedGeneres()
    {
        if (!Genres.Any(g => g.Name == "Test Genre"))
        {
            Genre testGenre = new Genre()
            {
                Name = "Test Genre",
                Description = "A genre for testing",
            };
            Genres.Add(testGenre);
            SaveChanges();
        }
    }

    public void SeedGames()
    {
        Genre testGenre = Genres.First(g => g.Name == "Test Genre");

        if (!Games.Any(t => t.Name == "Test Game 1"))
        {
            Game game = new()
            {
                Name = "Test Game 1",
                Description = "The first testing game",
                AverageDurationInHours = 5,
                Genre = testGenre,
                TotalRating = 0,
                NumberOfRatings = 0,
                MinPlayers = 1,
                MaxPlayers = 2,
                ReleaseDate = DateTime.Now,
                Likes = 50,
            };
            Games.Add(game);
        }

        if (!Games.Any(t => t.Name == "Test Game Number 2"))
        { 
            Game game = new()
            {
                Name = "Test Game Number 2",
                Description = "Another Testing Game 123 123 123",
                AverageDurationInHours = 5,
                Genre = testGenre,
                TotalRating = 0,
                NumberOfRatings = 0,
                MinPlayers = 1,
                MaxPlayers = 42,
                ReleaseDate = DateTime.Now.AddDays(-50),
                Likes = 250,
            };
            Games.Add(game);
        }

        SaveChanges();
    }

    public void SeedGameTags()
    {

        List<GameTag> addGameTags = new();
        Game game1 = Games.First(g => g.Name == "Test Game 1");
        Game game2 = Games.First(g => g.Name == "Test Game Number 2");

        Tag singleplayer = Tags.First(t => t.Name == "Singleplayer");
        Tag multiplayer = Tags.First(t => t.Name == "Multiplayer");
        Tag online = Tags.First(t => t.Name == "Online");

        List<GameTag> existingTags = GameTags.Where(gt => gt.GameId == game1.GameId).ToList();
        RemoveRange(existingTags);
        existingTags = GameTags.Where(gt => gt.GameId == game2.GameId).ToList();
        RemoveRange(existingTags);

        addGameTags.Add(new GameTag()
        {
            GameId = game1.GameId,
            TagId = singleplayer.TagId
        });
        addGameTags.Add(new GameTag()
        {
            GameId = game1.GameId,
            TagId = multiplayer.TagId
        }); 
        addGameTags.Add(new GameTag()
        {
            GameId = game2.GameId,
            TagId = singleplayer.TagId
        });
        addGameTags.Add(new GameTag()
        {
            GameId = game2.GameId,
            TagId = multiplayer.TagId
        });
        addGameTags.Add(new GameTag()
        {
            GameId = game2.GameId,
            TagId = online.TagId
        });

        GameTags.AddRange(addGameTags);
        SaveChanges();
    }
}
