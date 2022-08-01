using IntelliTect.Coalesce.DataAnnotations;

namespace CoalesceSample.Data.Models;
public class Game
{
    public int GameId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double AverageDurationInHours { get; set; }
    public int MaxPlayers { get; set; }
    public int MinPlayers { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
    [ManyToMany("Tag")]
    public ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();
}
