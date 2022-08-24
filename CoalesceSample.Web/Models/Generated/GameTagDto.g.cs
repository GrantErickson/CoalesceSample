using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CoalesceSample.Web.Models
{
    public partial class GameTagDtoGen : GeneratedDto<CoalesceSample.Data.Models.GameTag>
    {
        public GameTagDtoGen() { }

        private int? _GameTagId;
        private int? _TagId;
        private CoalesceSample.Web.Models.TagDtoGen _Tag;
        private System.Guid? _GameId;
        private CoalesceSample.Web.Models.GameDtoGen _Game;

        public int? GameTagId
        {
            get => _GameTagId;
            set { _GameTagId = value; Changed(nameof(GameTagId)); }
        }
        public int? TagId
        {
            get => _TagId;
            set { _TagId = value; Changed(nameof(TagId)); }
        }
        public CoalesceSample.Web.Models.TagDtoGen Tag
        {
            get => _Tag;
            set { _Tag = value; Changed(nameof(Tag)); }
        }
        public System.Guid? GameId
        {
            get => _GameId;
            set { _GameId = value; Changed(nameof(GameId)); }
        }
        public CoalesceSample.Web.Models.GameDtoGen Game
        {
            get => _Game;
            set { _Game = value; Changed(nameof(Game)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(CoalesceSample.Data.Models.GameTag obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            // Fill the properties of the object.

            this.GameTagId = obj.GameTagId;
            this.TagId = obj.TagId;
            this.GameId = obj.GameId;
            if (tree == null || tree[nameof(this.Tag)] != null)
                this.Tag = obj.Tag.MapToDto<CoalesceSample.Data.Models.Tag, TagDtoGen>(context, tree?[nameof(this.Tag)]);

            if (tree == null || tree[nameof(this.Game)] != null)
                this.Game = obj.Game.MapToDto<CoalesceSample.Data.Models.Game, GameDtoGen>(context, tree?[nameof(this.Game)]);

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(CoalesceSample.Data.Models.GameTag entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(GameTagId))) entity.GameTagId = (GameTagId ?? entity.GameTagId);
            if (ShouldMapTo(nameof(TagId))) entity.TagId = (TagId ?? entity.TagId);
            if (ShouldMapTo(nameof(GameId))) entity.GameId = (GameId ?? entity.GameId);
        }
    }
}
