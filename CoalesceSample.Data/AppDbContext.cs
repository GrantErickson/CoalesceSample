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
            // TODO: Or, use Database.EnsureCreated() instead:
            // this.Database.EnsureCreated();
        }
        catch (InvalidOperationException e) when (e.Message == "No service for type 'Microsoft.EntityFrameworkCore.Migrations.IMigrator' has been registered.")
        {
            // this exception is expected when using an InMemory database
        }
    }
}
