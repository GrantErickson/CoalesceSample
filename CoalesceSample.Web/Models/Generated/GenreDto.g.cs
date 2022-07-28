using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CoalesceSample.Web.Models
{
    public partial class GenreDtoGen : GeneratedDto<CoalesceSample.Data.Models.Genre>
    {
        public GenreDtoGen() { }

        private int? _GenreId;
        private string _Name;
        private string _Description;
        private System.Collections.Generic.ICollection<CoalesceSample.Web.Models.GameDtoGen> _Games;

        public int? GenreId
        {
            get => _GenreId;
            set { _GenreId = value; Changed(nameof(GenreId)); }
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
        public System.Collections.Generic.ICollection<CoalesceSample.Web.Models.GameDtoGen> Games
        {
            get => _Games;
            set { _Games = value; Changed(nameof(Games)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(CoalesceSample.Data.Models.Genre obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            // Fill the properties of the object.

            this.GenreId = obj.GenreId;
            this.Name = obj.Name;
            this.Description = obj.Description;
            var propValGames = obj.Games;
            if (propValGames != null && (tree == null || tree[nameof(this.Games)] != null))
            {
                this.Games = propValGames
                    .OrderBy(f => f.Name)
                    .Select(f => f.MapToDto<CoalesceSample.Data.Models.Game, GameDtoGen>(context, tree?[nameof(this.Games)])).ToList();
            }
            else if (propValGames == null && tree?[nameof(this.Games)] != null)
            {
                this.Games = new GameDtoGen[0];
            }

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(CoalesceSample.Data.Models.Genre entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(GenreId))) entity.GenreId = (GenreId ?? entity.GenreId);
            if (ShouldMapTo(nameof(Name))) entity.Name = Name;
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
        }
    }
}
