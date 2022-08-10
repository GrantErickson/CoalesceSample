using CoalesceSample.Data.Models;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Models;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ItemResult<Game>> GetGameDetails(int gameId)
    {
        Game? game = Db.Games
            .Where(g => g.GameId == gameId)
            .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
            .Include(g => g.Genre)
            .FirstOrDefault();
        if (game == null)
        {
            return "Could not find the requested game";
        }
        return game;
    }

    [Coalesce]
    public async Task<ItemResult> LikeGame(int gameId)
    {
        Game? game = await Db.Games.FirstOrDefaultAsync(g => g.GameId == gameId);
        if (game != null)
        {
            game.Likes += 1;
            await Db.SaveChangesAsync();
            return true;
        }
        return "Unable to like game.";
    }


    [Coalesce]
    public async Task<ItemResult<Uri>> GetGameImage(int gameId)
    {
        Game? game = await Db.Games.FirstOrDefaultAsync(g => g.GameId == gameId);
        return "No image uploaded for this game.";
    }
}
