using System.Collections;

namespace CoalesceSample.Data.Models;
public class Genre
{
    public int GenreId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Game> Games { get; set; } = new List<Game>();
}
