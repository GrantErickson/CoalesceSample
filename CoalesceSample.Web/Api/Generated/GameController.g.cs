
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
    [Route("api/Game")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class GameController
        : BaseApiController<CoalesceSample.Data.Models.Game, GameDtoGen, CoalesceSample.Data.AppDbContext>
    {
        public GameController(CoalesceSample.Data.AppDbContext db) : base(db)
        {
            GeneratedForClassViewModel = ReflectionRepository.Global.GetClassViewModel<CoalesceSample.Data.Models.Game>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<GameDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<CoalesceSample.Data.Models.Game> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<GameDtoGen>> List(
            ListParameters parameters,
            IDataSource<CoalesceSample.Data.Models.Game> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<CoalesceSample.Data.Models.Game> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<GameDtoGen>> Save(
            GameDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<CoalesceSample.Data.Models.Game> dataSource,
            IBehaviors<CoalesceSample.Data.Models.Game> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<GameDtoGen>> Delete(
            int id,
            IBehaviors<CoalesceSample.Data.Models.Game> behaviors,
            IDataSource<CoalesceSample.Data.Models.Game> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
