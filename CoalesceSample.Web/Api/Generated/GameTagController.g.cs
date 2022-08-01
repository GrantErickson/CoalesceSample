
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
    [Route("api/GameTag")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class GameTagController
        : BaseApiController<CoalesceSample.Data.Models.GameTag, GameTagDtoGen, CoalesceSample.Data.AppDbContext>
    {
        public GameTagController(CoalesceSample.Data.AppDbContext db) : base(db)
        {
            GeneratedForClassViewModel = ReflectionRepository.Global.GetClassViewModel<CoalesceSample.Data.Models.GameTag>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<GameTagDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<CoalesceSample.Data.Models.GameTag> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<GameTagDtoGen>> List(
            ListParameters parameters,
            IDataSource<CoalesceSample.Data.Models.GameTag> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<CoalesceSample.Data.Models.GameTag> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<GameTagDtoGen>> Save(
            GameTagDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<CoalesceSample.Data.Models.GameTag> dataSource,
            IBehaviors<CoalesceSample.Data.Models.GameTag> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<GameTagDtoGen>> Delete(
            int id,
            IBehaviors<CoalesceSample.Data.Models.GameTag> behaviors,
            IDataSource<CoalesceSample.Data.Models.GameTag> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
