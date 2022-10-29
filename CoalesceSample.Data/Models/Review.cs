using IntelliTect.Coalesce;
using IntelliTect.Coalesce.DataAnnotations;
using IntelliTect.Coalesce.Models;
using Microsoft.EntityFrameworkCore;

namespace CoalesceSample.Data.Models;
[Read(SecurityPermissionLevels.AllowAll)]
public class Review
{
    public Guid ReviewId { get; set; }
    public double Rating { get; set; }
    public DateTime ReviewDate { get; set; }
    [InternalUse]
    [Read(Roles = "SuperAdmin")]
    [Edit(Roles = "SuperAdmin")]
    public ApplicationUser Reviewer { get; set; } = null!;
    public string ReviewerName { get; set; } = null!;
    public string ReviewTitle { get; set; } = null!;
    public string ReviewBody { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
    public Guid GameId { get; set; }
    [InternalUse]
    public Game ReviewedGame { get; set; } = null!;

    [Coalesce]
    public class ReviewDataSource : StandardDataSource<Review, AppDbContext>
    {
        [Coalesce]
        public string FilterGameId { get; set; }
        [Coalesce]
        public DateTime? FirstDate { get; set; } = null;
        [Coalesce]
        public DateTime? SecondDate { get; set; } = null;
        [Coalesce]
        public double MinRating { get; set; } = 0;
        [Coalesce]
        public double MaxRating { get; set; } = 5;

        public ReviewDataSource(CrudContext<AppDbContext> context) : base(context) { }
        public override IQueryable<Review> GetQuery(IDataSourceParameters parameters)
        {
            if (FirstDate == null || SecondDate == null)
            {
                FirstDate = DateTime.MinValue;
                SecondDate = DateTime.MaxValue;
            }
            var reviews = Db.Reviews
                .Where(r =>
                    r.GameId == new Guid(FilterGameId) &&
                    !r.IsDeleted &&
                    r.Rating >= MinRating &&
                    r.Rating <= MaxRating &&
                    r.ReviewDate.Date >= FirstDate.Value.Date &&
                    r.ReviewDate.Date <= SecondDate.Value.Date
                    )
                .OrderByDescending(r => r.ReviewDate);

            return reviews;
        }
    }

}

[Coalesce]
public class ReviewBehaviors : StandardBehaviors<Review, AppDbContext>
{
    public ReviewBehaviors(CrudContext<AppDbContext> context) : base(context) { }

    public override Task<ItemResult> BeforeSaveAsync(SaveKind kind, Review? oldItem, Review item)
    {
        if (item.IsDeleted && (!oldItem?.IsDeleted ?? false))
        {
            Game? game = Db.Games.Include(g => g.Reviews)
                .Where(g => g.GameId == item.GameId)
                .FirstOrDefault();
            if (game != null)
            {
                if (game.NumberOfRatings == 0)
                {
                    game.AverageRating = 0;
                }
                else
                {
                    game.AverageRating = game.TotalRating / game.NumberOfRatings;
                }
            }
        }
        return base.BeforeSaveAsync(kind, oldItem, item);
    }

    public override ItemResult BeforeSave(SaveKind kind, Review? oldItem, Review item)
    {
        if (item.IsDeleted && (!oldItem?.IsDeleted ?? false))
        {
            Game? game = Db.Games.IncludeChildren()
                .Where(g => g.GameId == item.GameId)
                .FirstOrDefault();
            if (game != null)
            {
                if (game.NumberOfRatings == 0)
                {
                    game.AverageRating = 0;
                }
                else
                {
                    game.AverageRating = game.TotalRating / game.NumberOfRatings;
                }
            }
        }
        return base.BeforeSave(kind, oldItem, item);
    }
}
