using IntelliTect.Coalesce.DataAnnotations;
using IntelliTect.Coalesce.Models;
using System.ComponentModel.DataAnnotations;

namespace CoalesceSample.Data.Models;
public class Game
{
    public int GameId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = null!;
    public DateTime? ReleaseDate { get; set; }
    public int Likes { get; set; } = 0;
    [InternalUse]
    public double TotalRating { get; set; } = 0;
    public int NumberOfRatings { get; set; } = 0;
    public double AverageRating => NumberOfRatings == 0 ? 0 : TotalRating / NumberOfRatings;
    public double AverageDurationInHours { get; set; }
    public int MaxPlayers { get; set; }
    public int MinPlayers { get; set; } = 1;
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
    public int ImageId { get; set; }
    public Image Image { get; set; } = new Image();
    [ManyToMany("Tag")]
    public ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
