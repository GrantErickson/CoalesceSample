using IntelliTect.Coalesce.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoalesceSample.Data.Models;
public class Review
{
    public Guid ReviewId { get; set; }
    public double Rating { get; set; }
    public DateTime ReviewDate { get; set; }
    [InternalUse]
    public ApplicationUser Reviewer { get; set; } = null!;
    public string ReviewerName { get; set; } = null!;
    public string ReviewTitle { get; set; } = null!;
    public string ReviewBody { get; set; } = null!;

}