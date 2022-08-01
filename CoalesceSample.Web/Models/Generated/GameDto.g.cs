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
        private double? _AverageDurationInHours;
        private int? _MaxPlayers;
        private int? _MinPlayers;
        private int? _GenreId;
        private CoalesceSample.Web.Models.GenreDtoGen _Genre;
        private System.Collections.Generic.ICollection<CoalesceSample.Web.Models.GameTagDtoGen> _GameTags;

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
        public System.Collections.Generic.ICollection<CoalesceSample.Web.Models.GameTagDtoGen> GameTags
        {
            get => _GameTags;
            set { _GameTags = value; Changed(nameof(GameTags)); }
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
            this.AverageDurationInHours = obj.AverageDurationInHours;
            this.MaxPlayers = obj.MaxPlayers;
            this.MinPlayers = obj.MinPlayers;
            this.GenreId = obj.GenreId;
            if (tree == null || tree[nameof(this.Genre)] != null)
                this.Genre = obj.Genre.MapToDto<CoalesceSample.Data.Models.Genre, GenreDtoGen>(context, tree?[nameof(this.Genre)]);

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
            if (ShouldMapTo(nameof(AverageDurationInHours))) entity.AverageDurationInHours = (AverageDurationInHours ?? entity.AverageDurationInHours);
            if (ShouldMapTo(nameof(MaxPlayers))) entity.MaxPlayers = (MaxPlayers ?? entity.MaxPlayers);
            if (ShouldMapTo(nameof(MinPlayers))) entity.MinPlayers = (MinPlayers ?? entity.MinPlayers);
            if (ShouldMapTo(nameof(GenreId))) entity.GenreId = (GenreId ?? entity.GenreId);
        }
    }
}
