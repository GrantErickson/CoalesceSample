using IntelliTect.Coalesce.DataAnnotations;
using System.Collections;

namespace CoalesceSample.Data.Models;
public class GameTag
{
    public int GameTagId { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
}
