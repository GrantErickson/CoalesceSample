using CoalesceSample.Data.Models;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.DataAnnotations;
using IntelliTect.Coalesce.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoalesceSample.Data.Services;
[Coalesce, Service]
public class GameService
{
    private AppDbContext Db { get; set; }

    public GameService(AppDbContext db)
    {
        Db = db;
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    public async Task<ItemResult<List<Game>>> GetGames()
    {
        List<Game> games = await Db.Games
            .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
            .Include(g => g.Genre)
            .ToListAsync();
        if (!games.Any())
        {
            return "No games currently exist.";
        }
        return games;
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    public async Task<ItemResult<List<Game>>> GetGamesFromIds(List<int> gameIds)
    {
        List<Game> games = await Db.Games
            .Where(g => gameIds.Contains(g.GameId))
            .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
            .Include(g => g.Genre)
            .ToListAsync();
        if (!games.Any())
        {
            return new List<Game>();
        }
        return games;
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    public async Task<ItemResult<Game>> GetGameDetails(int gameId)
    {
        Game? game = Db.Games
            .Where(g => g.GameId == gameId)
            .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
            .Include(g => g.Genre)
            .Include(g => g.Reviews)
            .FirstOrDefault();

        if (game == null)
        {
            return "Could not find the requested game";
        }
        if (game.Name == null)
        {
            return "Game name was null";
        }
        return game;
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    public async Task<ItemResult<string>> GetGameImage(int gameId)
    {
        Game? game = await Db.Games.Include(g => g.Image).FirstOrDefaultAsync(g => g.GameId == gameId);
        if (game == null)
        {
            return new ItemResult<string>
            {
                Message = "Unable to find the game.",
                WasSuccessful = false
            };
        }
        if (game.Image.Base64Image == null)
        {
            return new ItemResult<string>
            {
                Message = "There is no image uploaded for this game.",
                WasSuccessful = false
            };
        }
        return new ItemResult<string>
        {
            Object = game.Image.Base64Image,
            WasSuccessful = true
        };
    }

    [Coalesce]
    public async Task<ItemResult> UploadGameImage(ClaimsPrincipal claim, int gameId, IFile image)
    {
        Game? game = await Db.Games.FirstOrDefaultAsync(g => g.GameId == gameId);
        if (game == null)
        {
            return "Unable to find the game";
        }
        if (image.Content == null)
        {
            return "Unable to upload this image";
        }
        Image dbImage = Db.Images.First(i => i.ImageId == game.ImageId);
        string? imageBase64;
        using (MemoryStream imageContents = new MemoryStream())
        {
            image.Content.CopyTo(imageContents);
            imageBase64 = "data:image/jpeg;base64," + Convert.ToBase64String(imageContents.ToArray());
        }
        if (imageBase64 == null || imageBase64 == "data:image/jpeg;base64,")
        {
            return "Unable to upload image";
        }
        dbImage.Base64Image = imageBase64;
        await Db.SaveChangesAsync();
        return true;
    }

    [Coalesce]
    public async Task<ItemResult<List<Tag>>> GetAllTags()
    {
        if (!Db.Tags.Any())
        {
            return "There are no tags in the database";
        }
        return await Db.Tags.ToListAsync();
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    public async Task<List<GameTag>> GetGameTags(int gameId)
    {
        Game? game = Db.Games.Include(g => g.GameTags).ThenInclude(gt => gt.Tag).FirstOrDefault(i => i.GameId == gameId);
        if (game == null)
        {
            return new List<GameTag>();
        }
        return game.GameTags.ToList();
    }

    [Coalesce]
    public async Task<ItemResult> SetGameTags(int gameId, List<int> tagIds)
    {
        IQueryable<GameTag>? tags = Db.GameTags.Where(gt => gt.GameId == gameId);
        Db.GameTags.RemoveRange(tags);
        //foreach(int id in tagIds)
        //{
        //    game.GameTags.Add(new GameTag() { GameId = gameId, TagId = id });
        //}
        tagIds.ForEach(id => Db.Add(new GameTag() { GameId = gameId, TagId = id }));
        await Db.SaveChangesAsync();
        return true;
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    public async Task<ItemResult> AddLike(int gameId)
    {
        Game? game = Db.Games.Include(g => g.GameTags).ThenInclude(gt => gt.Tag).FirstOrDefault(i => i.GameId == gameId);
        if (game == null)
        {
            return "Unable to find the requested game";
        }
        game.Likes++;
        await Db.SaveChangesAsync();
        return true;
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    public async Task<ItemResult> RemoveLike(int gameId)
    {
        Game? game = Db.Games.Include(g => g.GameTags).ThenInclude(gt => gt.Tag).FirstOrDefault(i => i.GameId == gameId);
        if (game == null)
        {
            return "Unable to find the requested game";
        }
        game.Likes--;
        await Db.SaveChangesAsync();
        return true;
    }
}
