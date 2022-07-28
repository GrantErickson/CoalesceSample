
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
        [Authorize]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<GameDtoGen>>> GetGames()
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.GetGames();
            var _result = new ItemResult<System.Collections.Generic.ICollection<GameDtoGen>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList().Select(o => Mapper.MapToDto<CoalesceSample.Data.Models.Game, GameDtoGen>(o, _mappingContext, includeTree)).ToList();
            return _result;
        }
    }
}
