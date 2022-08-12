using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CoalesceSample.Web.Models
{
    public partial class GameDtoGen : GeneratedDto<CoalesceSample.Data.Models.Game>
    {
        public GameDtoGen() { }

        private int? _GameId;
        private string _Name;
        private string _Description;
        private System.DateTime? _ReleaseDate;
        private int? _Likes;
        private int? _NumberOfRatings;
        private double? _AverageRating;
        private double? _AverageDurationInHours;
        private int? _MaxPlayers;
        private int? _MinPlayers;
        private int? _GenreId;
        private CoalesceSample.Web.Models.GenreDtoGen _Genre;
        private int? _ImageId;
        private CoalesceSample.Web.Models.ImageDtoGen _Image;
        private System.Collections.Generic.ICollection<CoalesceSample.Web.Models.GameTagDtoGen> _GameTags;
        private System.Collections.Generic.ICollection<CoalesceSample.Web.Models.ReviewDtoGen> _Reviews;

        public int? GameId
        {
            get => _GameId;
            set { _GameId = value; Changed(nameof(GameId)); }
        }
        public string Name
        {
            get => _Name;
            set { _Name = value; Changed(nameof(Name)); }
        }
        public string Description
        {
            get => _Description;
            set { _Description = value; Changed(nameof(Description)); }
        }
        public System.DateTime? ReleaseDate
        {
            get => _ReleaseDate;
            set { _ReleaseDate = value; Changed(nameof(ReleaseDate)); }
        }
        public int? Likes
        {
            get => _Likes;
            set { _Likes = value; Changed(nameof(Likes)); }
        }
        public int? NumberOfRatings
        {
            get => _NumberOfRatings;
            set { _NumberOfRatings = value; Changed(nameof(NumberOfRatings)); }
        }
        public double? AverageRating
        {
            get => _AverageRating;
            set { _AverageRating = value; Changed(nameof(AverageRating)); }
        }
        public double? AverageDurationInHours
        {
            get => _AverageDurationInHours;
            set { _AverageDurationInHours = value; Changed(nameof(AverageDurationInHours)); }
        }
        public int? MaxPlayers
        {
            get => _MaxPlayers;
            set { _MaxPlayers = value; Changed(nameof(MaxPlayers)); }
        }
        public int? MinPlayers
        {
            get => _MinPlayers;
            set { _MinPlayers = value; Changed(nameof(MinPlayers)); }
        }
        public int? GenreId
        {
            get => _GenreId;
            set { _GenreId = value; Changed(nameof(GenreId)); }
        }
        public CoalesceSample.Web.Models.GenreDtoGen Genre
        {
            get => _Genre;
            set { _Genre = value; Changed(nameof(Genre)); }
        }
        public int? ImageId
        {
            get => _ImageId;
            set { _ImageId = value; Changed(nameof(ImageId)); }
        }
        public CoalesceSample.Web.Models.ImageDtoGen Image
        {
            get => _Image;
            set { _Image = value; Changed(nameof(Image)); }
        }
        public System.Collections.Generic.ICollection<CoalesceSample.Web.Models.GameTagDtoGen> GameTags
        {
            get => _GameTags;
            set { _GameTags = value; Changed(nameof(GameTags)); }
        }
        public System.Collections.Generic.ICollection<CoalesceSample.Web.Models.ReviewDtoGen> Reviews
        {
            get => _Reviews;
            set { _Reviews = value; Changed(nameof(Reviews)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(CoalesceSample.Data.Models.Game obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            // Fill the properties of the object.

            this.GameId = obj.GameId;
            this.Name = obj.Name;
            this.Description = obj.Description;
            this.ReleaseDate = obj.ReleaseDate;
            this.Likes = obj.Likes;
            this.NumberOfRatings = obj.NumberOfRatings;
            this.AverageRating = obj.AverageRating;
            this.AverageDurationInHours = obj.AverageDurationInHours;
            this.MaxPlayers = obj.MaxPlayers;
            this.MinPlayers = obj.MinPlayers;
            this.GenreId = obj.GenreId;
            this.ImageId = obj.ImageId;
            if (tree == null || tree[nameof(this.Genre)] != null)
                this.Genre = obj.Genre.MapToDto<CoalesceSample.Data.Models.Genre, GenreDtoGen>(context, tree?[nameof(this.Genre)]);

            if (tree == null || tree[nameof(this.Image)] != null)
                this.Image = obj.Image.MapToDto<CoalesceSample.Data.Models.Image, ImageDtoGen>(context, tree?[nameof(this.Image)]);

            var propValGameTags = obj.GameTags;
            if (propValGameTags != null && (tree == null || tree[nameof(this.GameTags)] != null))
            {
                this.GameTags = propValGameTags
                    .OrderBy(f => f.GameTagId)
                    .Select(f => f.MapToDto<CoalesceSample.Data.Models.GameTag, GameTagDtoGen>(context, tree?[nameof(this.GameTags)])).ToList();
            }
            else if (propValGameTags == null && tree?[nameof(this.GameTags)] != null)
            {
                this.GameTags = new GameTagDtoGen[0];
            }

            var propValReviews = obj.Reviews;
            if (propValReviews != null && (tree == null || tree[nameof(this.Reviews)] != null))
            {
                this.Reviews = propValReviews
                    .OrderBy(f => f.ReviewId)
                    .Select(f => f.MapToDto<CoalesceSample.Data.Models.Review, ReviewDtoGen>(context, tree?[nameof(this.Reviews)])).ToList();
            }
            else if (propValReviews == null && tree?[nameof(this.Reviews)] != null)
            {
                this.Reviews = new ReviewDtoGen[0];
            }

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(CoalesceSample.Data.Models.Game entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(GameId))) entity.GameId = (GameId ?? entity.GameId);
            if (ShouldMapTo(nameof(Name))) entity.Name = Name;
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
            if (ShouldMapTo(nameof(ReleaseDate))) entity.ReleaseDate = ReleaseDate;
            if (ShouldMapTo(nameof(Likes))) entity.Likes = (Likes ?? entity.Likes);
            if (ShouldMapTo(nameof(NumberOfRatings))) entity.NumberOfRatings = (NumberOfRatings ?? entity.NumberOfRatings);
            if (ShouldMapTo(nameof(AverageDurationInHours))) entity.AverageDurationInHours = (AverageDurationInHours ?? entity.AverageDurationInHours);
            if (ShouldMapTo(nameof(MaxPlayers))) entity.MaxPlayers = (MaxPlayers ?? entity.MaxPlayers);
            if (ShouldMapTo(nameof(MinPlayers))) entity.MinPlayers = (MinPlayers ?? entity.MinPlayers);
            if (ShouldMapTo(nameof(GenreId))) entity.GenreId = (GenreId ?? entity.GenreId);
            if (ShouldMapTo(nameof(ImageId))) entity.ImageId = (ImageId ?? entity.ImageId);
        }
    }
}
