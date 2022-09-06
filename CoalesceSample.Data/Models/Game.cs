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
    [DefaultDataSource]
    [Coalesce]
    public class GameDataSource : StandardDataSource<Game, AppDbContext>
    {
        public GameDataSource(CrudContext<AppDbContext> context) : base(context) { FilterTags = ""; }

        [Coalesce]
        public string FilterTags { get; set; }
        public override IQueryable<Game> GetQuery(IDataSourceParameters parameters)
        {

            IQueryable<Game> query = base.GetQuery(parameters);
            if (FilterTags != null)
            {
                IEnumerable<int> tags = FilterTags.Split(',').Where(x => int.TryParse(x, out _)).Select(Int32.Parse);
                query = query
                    .Include(g=>g.Reviews.Where(r=>!r.IsDeleted))
                    .Where(g => g.GameTags.Where(gt => tags.Contains(gt.TagId)).Count() == tags.Count());
            }

            return query;
        }
    }
    #endregion

    #region BEHAVIORS
    [Coalesce]
    public class GameBehaviors : StandardBehaviors<Game, AppDbContext>
    {
        public GameBehaviors(CrudContext<AppDbContext> context) : base(context)
        {
        }
        [Execute(SecurityPermissionLevels.AllowAll)]
        protected override DbSet<Game> GetDbSet()
        {
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            Console.WriteLine("Behavior Triggered");
            return base.GetDbSet();
        }
    }
    #endregion
}
