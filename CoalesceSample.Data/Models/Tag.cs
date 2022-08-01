using IntelliTect.Coalesce.DataAnnotations;
using System.Collections;

namespace CoalesceSample.Data.Models;
public class Tag
{
    public int TagId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [ManyToMany("Game")]
    public ICollection<GameTag> Games { get; set; } = new List<GameTag>();
}
