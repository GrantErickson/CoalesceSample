
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
        /// Method: GetGames
        /// </summary>
        [HttpPost("GetGames")]
        [AllowAnonymous]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<GameDtoGen>>> GetGames()
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.GetGames();
            var _result = new ItemResult<System.Collections.Generic.ICollection<GameDtoGen>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList().Select(o => Mapper.MapToDto<CoalesceSample.Data.Models.Game, GameDtoGen>(o, _mappingContext, includeTree)).ToList();
            return _result;
        }

        /// <summary>
        /// Method: GetGamesFromIds
        /// </summary>
        [HttpPost("GetGamesFromIds")]
        [AllowAnonymous]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<GameDtoGen>>> GetGamesFromIds(System.Collections.Generic.ICollection<int> gameIds)
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.GetGamesFromIds(gameIds.ToList());
            var _result = new ItemResult<System.Collections.Generic.ICollection<GameDtoGen>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList().Select(o => Mapper.MapToDto<CoalesceSample.Data.Models.Game, GameDtoGen>(o, _mappingContext, includeTree)).ToList();
            return _result;
        }

        /// <summary>
        /// Method: GetGameDetails
        /// </summary>
        [HttpPost("GetGameDetails")]
        [AllowAnonymous]
        public virtual async Task<ItemResult<GameDtoGen>> GetGameDetails(int gameId)
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.GetGameDetails(gameId);
            var _result = new ItemResult<GameDtoGen>(_methodResult);
            _result.Object = Mapper.MapToDto<CoalesceSample.Data.Models.Game, GameDtoGen>(_methodResult.Object, _mappingContext, includeTree);
            return _result;
        }

        /// <summary>
        /// Method: GetGameImage
        /// </summary>
        [HttpPost("GetGameImage")]
        [AllowAnonymous]
        public virtual async Task<ItemResult<string>> GetGameImage(int gameId)
        {
            var _methodResult = await Service.GetGameImage(gameId);
            var _result = new ItemResult<string>(_methodResult);
            _result.Object = _methodResult.Object;
            return _result;
        }

        /// <summary>
        /// Method: UploadGameImage
        /// </summary>
        [HttpPost("UploadGameImage")]
        [Authorize(Roles = "SuperAdmin")]
        public virtual async Task<ItemResult> UploadGameImage(int gameId, Microsoft.AspNetCore.Http.IFormFile image)
        {
            var _methodResult = await Service.UploadGameImage(User, gameId, image == null ? null : new IntelliTect.Coalesce.Models.File { Name = image.FileName, ContentType = image.ContentType, Length = image.Length, Content = image.OpenReadStream() });
            var _result = new ItemResult(_methodResult);
            return _result;
        }

        /// <summary>
        /// Method: GetAllTags
        /// </summary>
        [HttpPost("GetAllTags")]
        [Authorize(Roles = "User")]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<TagDtoGen>>> GetAllTags()
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.GetAllTags();
            var _result = new ItemResult<System.Collections.Generic.ICollection<TagDtoGen>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList().Select(o => Mapper.MapToDto<CoalesceSample.Data.Models.Tag, TagDtoGen>(o, _mappingContext, includeTree)).ToList();
            return _result;
        }

        /// <summary>
        /// Method: GetGameTags
        /// </summary>
        [HttpPost("GetGameTags")]
        [AllowAnonymous]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<GameTagDtoGen>>> GetGameTags(int gameId)
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.GetGameTags(gameId);
            var _result = new ItemResult<System.Collections.Generic.ICollection<GameTagDtoGen>>();
            _result.Object = _methodResult?.ToList().Select(o => Mapper.MapToDto<CoalesceSample.Data.Models.GameTag, GameTagDtoGen>(o, _mappingContext, includeTree)).ToList();
            return _result;
        }

        /// <summary>
        /// Method: SetGameTags
        /// </summary>
        [HttpPost("SetGameTags")]
        [Authorize(Roles = "User")]
        public virtual async Task<ItemResult> SetGameTags(int gameId, System.Collections.Generic.ICollection<int> tagIds)
        {
            var _methodResult = await Service.SetGameTags(gameId, tagIds.ToList());
            var _result = new ItemResult(_methodResult);
            return _result;
        }

        /// <summary>
        /// Method: AddLike
        /// </summary>
        [HttpPost("AddLike")]
        [AllowAnonymous]
        public virtual async Task<ItemResult> AddLike(int gameId)
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
        public virtual async Task<ItemResult> RemoveLike(int gameId)
        {
            var _methodResult = await Service.RemoveLike(gameId);
            var _result = new ItemResult(_methodResult);
            return _result;
        }
    }
}
