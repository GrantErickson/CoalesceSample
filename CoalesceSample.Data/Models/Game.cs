using IntelliTect.Coalesce;
using IntelliTect.Coalesce.DataAnnotations;
using IntelliTect.Coalesce.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoalesceSample.Data.Models;
[Read(SecurityPermissionLevels.AllowAll)]
public class Game
{
    public Guid GameId { get; set; }
    [Search(SearchMethod = SearchAttribute.SearchMethods.Contains)]
    public string Name { get; set; } = string.Empty;
    [Search(SearchMethod = SearchAttribute.SearchMethods.Contains)]
    public string Description { get; set; } = null!;
    public DateTime? ReleaseDate { get; set; }
    public int Likes { get; set; } = 0;
    public double TotalRating => Reviews.Where(r => !r.IsDeleted).Select(r => r.Rating).Sum();
    public int NumberOfRatings => Reviews.Where(r => !r.IsDeleted).Count();
    public double AverageRating { get; set; }
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

    #region DATASOURCE
    [DefaultDataSource]
    [Coalesce]
[Read(SecurityPermissionLevels.AllowAll)]
    public class GameDataSource : StandardDataSource<Game, AppDbContext>
    {
        public GameDataSource(CrudContext<AppDbContext> context) : base(context) { FilterTags = ""; }

        [Coalesce]
        public string FilterTags { get; set; }
        [Coalesce]
        public double FilterRatingsUpper { get; set; } = 5;
        [Coalesce]
        public double FilterRatingsLower { get; set; } = 0;
        public override IQueryable<Game> GetQuery(IDataSourceParameters parameters)
        {

            IQueryable<Game> query = Db.Games;
            if (FilterTags != null)
            {
                IEnumerable<int> tags = FilterTags.Split(',').Where(x => int.TryParse(x, out _)).Select(Int32.Parse);
                query = query
                    .AsNoTracking()
                    .Include(g => g.Genre)
                    .Include(g => g.GameTags)
                        .ThenInclude(gt=>gt.Tag)
                    .Include(g => g.Reviews.Where(r => !r.IsDeleted))
                    .Where(g =>
                    g.GameTags.Where(gt => tags.Contains(gt.TagId)).Count() == tags.Count() &&
                    g.AverageRating >= FilterRatingsLower &&
                    g.AverageRating <= FilterRatingsUpper
                    );

            }

            return query;
        }
    }
    #endregion
}
