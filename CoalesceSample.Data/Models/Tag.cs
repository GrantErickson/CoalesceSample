using IntelliTect.Coalesce.DataAnnotations;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CoalesceSample.Data.Models;
public class Tag
{
    public int TagId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [ManyToMany("Game")]
    public ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();
}
