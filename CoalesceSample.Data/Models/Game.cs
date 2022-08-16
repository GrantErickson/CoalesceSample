using IntelliTect.Coalesce;
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

    #region DATASOURCE
    [InternalUse]
    [DefaultDataSource]
    public class DefaultDataSource : StandardDataSource<Game, AppDbContext>
    {
        public DefaultDataSource(CrudContext<AppDbContext> context) : base(context) { }

        [Coalesce]
        public int GameId { get; set; }

        public override IQueryable<Game> GetQuery(IDataSourceParameters parameters)
        {
            IQueryable<Game> query = base.GetQuery(parameters);

            query = query.Where(g => g.GameId == GameId);

            return query;
        }
    }
    #endregion
}
