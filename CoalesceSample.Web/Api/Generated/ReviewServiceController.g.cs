
using CoalesceSample.Web.Models;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Api;
using IntelliTect.Coalesce.Api.Controllers;
using IntelliTect.Coalesce.Api.DataSources;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Mapping.IncludeTrees;
using IntelliTect.Coalesce.Models;
using IntelliTect.Coalesce.TypeDefinition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoalesceSample.Web.Api
{
    [Route("api/ReviewService")]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class ReviewServiceController : Controller
    {
        protected CoalesceSample.Data.Services.IReviewService Service { get; }

        public ReviewServiceController(CoalesceSample.Data.Services.IReviewService service)
        {
            Service = service;
        }

        /// <summary>
        /// Method: GetReviews
        /// </summary>
        [HttpPost("GetReviews")]
        [AllowAnonymous]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<ReviewDtoGen>>> GetReviews(int gameId)
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.GetReviews(gameId);
            var _result = new ItemResult<System.Collections.Generic.ICollection<ReviewDtoGen>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList().Select(o => Mapper.MapToDto<CoalesceSample.Data.Models.Review, ReviewDtoGen>(o, _mappingContext, includeTree)).ToList();
            return _result;
        }

        /// <summary>
        /// Method: addReview
        /// </summary>
        [HttpPost("addReview")]
        [Authorize]
        public virtual async Task<ItemResult> addReview(int gameId, string reviewTitle, string reviewBody, double rating)
        {
            var _methodResult = await Service.addReview(User, gameId, reviewTitle, reviewBody, rating);
            var _result = new ItemResult(_methodResult);
            return _result;
        }
    }
}
