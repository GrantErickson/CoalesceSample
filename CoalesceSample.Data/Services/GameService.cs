using CoalesceSample.Data.Models;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        List<Game> games = await Db.Games.ToListAsync();
        if(!games.Any())
        {
            return "No games currently exist.";
        }
        return games;
    }

}
