using CoalesceSample.Data.Models;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.DataAnnotations;
using IntelliTect.Coalesce.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using File = IntelliTect.Coalesce.Models.File;

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
    public async Task<ItemResult<Image>> GetGameImage(Guid gameId)
    {
        Game? game = Db.Games.Include(g => g.Image).FirstOrDefault(g => g.GameId == gameId);
        if (game == null)
        {
            return "Unable to find the game.";
        }
        if (game.Image.Content == null || game.Image.Content.Length==0)
        {
            return "There is no image uploaded for this game.";

        }
        return game.Image;
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAuthorized, Roles = Roles.SuperAdmin)]
    public async Task<ItemResult<Image>> UploadGameImage(ClaimsPrincipal claim, Guid gameId, IFile image)
    {
        Game? game = await Db.Games.FirstOrDefaultAsync(g => g.GameId == gameId);
        if (game == null)
        {
            return "Unable to find the game";
        }
        if (image == null || image.Content == null)
        {
            return "Unable to upload this image";
        }
        Image dbImage = Db.Images.First(i => i.ImageId == game.ImageId);

        byte[] content = new byte[image.Length];
        await image.Content.ReadAsync(content.AsMemory());

        dbImage.Content = content;

        await Db.SaveChangesAsync();
        return dbImage;
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAuthorized, Roles = Roles.User)]
    public async Task<ItemResult<List<GameTag>>> SetGameTags(Guid gameId, List<int> tagIds)
    {
        IQueryable<GameTag>? tags = Db.GameTags.Include(gt=>gt.Tag).Where(gt => gt.GameId == gameId);
        Db.GameTags.RemoveRange(tags);
        var tagList = new List<GameTag>();
        tagIds.ForEach(id => tagList.Add(new GameTag() { GameId = gameId, TagId = id }));
        Db.AddRange(tagList);
        await Db.SaveChangesAsync();
        return await tags.ToListAsync();
    }

    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAll)]
    public async Task<ItemResult> AddLike(Guid gameId)
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
    public async Task<ItemResult> RemoveLike(Guid gameId)
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
