using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CoalesceSample.Web.Models
{
    public partial class TagDtoGen : GeneratedDto<CoalesceSample.Data.Models.Tag>
    {
        public TagDtoGen() { }

        private int? _TagId;
        private string _Name;
        private string _Description;
        private System.Collections.Generic.ICollection<CoalesceSample.Web.Models.GameTagDtoGen> _GameTags;

        public int? TagId
        {
            get => _TagId;
            set { _TagId = value; Changed(nameof(TagId)); }
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
        public System.Collections.Generic.ICollection<CoalesceSample.Web.Models.GameTagDtoGen> GameTags
        {
            get => _GameTags;
            set { _GameTags = value; Changed(nameof(GameTags)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(CoalesceSample.Data.Models.Tag obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            // Fill the properties of the object.

            this.TagId = obj.TagId;
            this.Name = obj.Name;
            this.Description = obj.Description;
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
        public override void MapTo(CoalesceSample.Data.Models.Tag entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(TagId))) entity.TagId = (TagId ?? entity.TagId);
            if (ShouldMapTo(nameof(Name))) entity.Name = Name;
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
        }
    }
}
