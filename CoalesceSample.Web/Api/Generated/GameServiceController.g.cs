
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
    [Route("api/GameService")]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class GameServiceController : Controller
    {
        protected CoalesceSample.Data.Services.GameService Service { get; }

        public GameServiceController(CoalesceSample.Data.Services.GameService service)
        {
            Service = service;
        }

        /// <summary>
        /// Method: GetGameImage
        /// </summary>
        [HttpPost("GetGameImage")]
        [AllowAnonymous]
        public virtual async Task<ItemResult<ImageDtoGen>> GetGameImage(System.Guid gameId)
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.GetGameImage(gameId);
            var _result = new ItemResult<ImageDtoGen>(_methodResult);
            _result.Object = Mapper.MapToDto<CoalesceSample.Data.Models.Image, ImageDtoGen>(_methodResult.Object, _mappingContext, includeTree);
            return _result;
        }

        /// <summary>
        /// Method: UploadGameImage
        /// </summary>
        [HttpPost("UploadGameImage")]
        [Authorize(Roles = "SuperAdmin")]
        public virtual async Task<ItemResult<ImageDtoGen>> UploadGameImage(System.Guid gameId, Microsoft.AspNetCore.Http.IFormFile image)
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.UploadGameImage(User, gameId, image == null ? null : new IntelliTect.Coalesce.Models.File { Name = image.FileName, ContentType = image.ContentType, Length = image.Length, Content = image.OpenReadStream() });
            var _result = new ItemResult<ImageDtoGen>(_methodResult);
            _result.Object = Mapper.MapToDto<CoalesceSample.Data.Models.Image, ImageDtoGen>(_methodResult.Object, _mappingContext, includeTree);
            return _result;
        }

        /// <summary>
        /// Method: SetGameTags
        /// </summary>
        [HttpPost("SetGameTags")]
        [Authorize(Roles = "User")]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<GameTagDtoGen>>> SetGameTags(System.Guid gameId, System.Collections.Generic.ICollection<int> tagIds)
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.SetGameTags(gameId, tagIds.ToList());
            var _result = new ItemResult<System.Collections.Generic.ICollection<GameTagDtoGen>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList().Select(o => Mapper.MapToDto<CoalesceSample.Data.Models.GameTag, GameTagDtoGen>(o, _mappingContext, includeTree)).ToList();
            return _result;
        }

        /// <summary>
        /// Method: AddLike
        /// </summary>
        [HttpPost("AddLike")]
        [AllowAnonymous]
        public virtual async Task<ItemResult> AddLike(System.Guid gameId)
        {
            var _methodResult = await Service.AddLike(gameId);
            var _result = new ItemResult(_methodResult);
            return _result;
        }

        /// <summary>
        /// Method: RemoveLike
        /// </summary>
        [HttpPost("RemoveLike")]
        [AllowAnonymous]
        public virtual async Task<ItemResult> RemoveLike(System.Guid gameId)
        {
            var _methodResult = await Service.RemoveLike(gameId);
            var _result = new ItemResult(_methodResult);
            return _result;
        }
    }
}
