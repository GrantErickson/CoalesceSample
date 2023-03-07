using IntelliTect.Coalesce.DataAnnotations;
using System.Collections;

namespace CoalesceSample.Data.Models;
public class Genre
{
    public int GenreId { get; set; }
    [Search(SearchMethod = SearchAttribute.SearchMethods.Contains)]
    public string Name { get; set; } = null!;
    [Search(SearchMethod = SearchAttribute.SearchMethods.Contains)]
    public string? Description { get; set; }
}
