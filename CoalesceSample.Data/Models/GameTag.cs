using IntelliTect.Coalesce.DataAnnotations;
using System.Collections;

namespace CoalesceSample.Data.Models;
public class GameTag
{
    public int GameTagId { get; set; }
    public int TagId { get; set; }
    public int GameId { get; set; }
}
