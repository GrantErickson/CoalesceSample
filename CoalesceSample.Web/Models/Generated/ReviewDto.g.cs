using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CoalesceSample.Web.Models
{
    public partial class ReviewDtoGen : GeneratedDto<CoalesceSample.Data.Models.Review>
    {
        public ReviewDtoGen() { }

        private System.Guid? _ReviewId;
        private double? _Rating;
        private System.DateTime? _ReviewDate;
        private string _ReviewerName;
        private string _ReviewTitle;
        private string _ReviewBody;

        public System.Guid? ReviewId
        {
            get => _ReviewId;
            set { _ReviewId = value; Changed(nameof(ReviewId)); }
        }
        public double? Rating
        {
            get => _Rating;
            set { _Rating = value; Changed(nameof(Rating)); }
        }
        public System.DateTime? ReviewDate
        {
            get => _ReviewDate;
            set { _ReviewDate = value; Changed(nameof(ReviewDate)); }
        }
        public string ReviewerName
        {
            get => _ReviewerName;
            set { _ReviewerName = value; Changed(nameof(ReviewerName)); }
        }
        public string ReviewTitle
        {
            get => _ReviewTitle;
            set { _ReviewTitle = value; Changed(nameof(ReviewTitle)); }
        }
        public string ReviewBody
        {
            get => _ReviewBody;
            set { _ReviewBody = value; Changed(nameof(ReviewBody)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(CoalesceSample.Data.Models.Review obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            // Fill the properties of the object.

            this.ReviewId = obj.ReviewId;
            this.Rating = obj.Rating;
            this.ReviewDate = obj.ReviewDate;
            this.ReviewerName = obj.ReviewerName;
            this.ReviewTitle = obj.ReviewTitle;
            this.ReviewBody = obj.ReviewBody;
        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(CoalesceSample.Data.Models.Review entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(ReviewId))) entity.ReviewId = (ReviewId ?? entity.ReviewId);
            if (ShouldMapTo(nameof(Rating))) entity.Rating = (Rating ?? entity.Rating);
            if (ShouldMapTo(nameof(ReviewDate))) entity.ReviewDate = (ReviewDate ?? entity.ReviewDate);
            if (ShouldMapTo(nameof(ReviewerName))) entity.ReviewerName = ReviewerName;
            if (ShouldMapTo(nameof(ReviewTitle))) entity.ReviewTitle = ReviewTitle;
            if (ShouldMapTo(nameof(ReviewBody))) entity.ReviewBody = ReviewBody;
        }
    }
}
