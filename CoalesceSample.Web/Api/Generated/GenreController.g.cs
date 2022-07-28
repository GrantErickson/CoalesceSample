
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
    [Route("api/Genre")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class GenreController
        : BaseApiController<CoalesceSample.Data.Models.Genre, GenreDtoGen, CoalesceSample.Data.AppDbContext>
    {
        public GenreController(CoalesceSample.Data.AppDbContext db) : base(db)
        {
            GeneratedForClassViewModel = ReflectionRepository.Global.GetClassViewModel<CoalesceSample.Data.Models.Genre>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<GenreDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<CoalesceSample.Data.Models.Genre> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<GenreDtoGen>> List(
            ListParameters parameters,
            IDataSource<CoalesceSample.Data.Models.Genre> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<CoalesceSample.Data.Models.Genre> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<GenreDtoGen>> Save(
            GenreDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<CoalesceSample.Data.Models.Genre> dataSource,
            IBehaviors<CoalesceSample.Data.Models.Genre> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<GenreDtoGen>> Delete(
            int id,
            IBehaviors<CoalesceSample.Data.Models.Genre> behaviors,
            IDataSource<CoalesceSample.Data.Models.Genre> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
