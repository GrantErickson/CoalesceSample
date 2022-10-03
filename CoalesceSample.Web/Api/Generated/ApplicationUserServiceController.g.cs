
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
    [Route("api/ApplicationUserService")]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class ApplicationUserServiceController : Controller
    {
        protected CoalesceSample.Data.Services.IApplicationUserService Service { get; }

        public ApplicationUserServiceController(CoalesceSample.Data.Services.IApplicationUserService service)
        {
            Service = service;
        }

        /// <summary>
        /// Method: GetRoles
        /// </summary>
        [HttpPost("GetRoles")]
        [AllowAnonymous]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<string>>> GetRoles()
        {
            var _methodResult = await Service.GetRoles(User);
            var _result = new ItemResult<System.Collections.Generic.ICollection<string>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList();
            return _result;
        }

        /// <summary>
        /// Method: HasRole
        /// </summary>
        [HttpPost("HasRole")]
        [AllowAnonymous]
        public virtual async Task<ItemResult> HasRole(string role)
        {
            var _methodResult = await Service.HasRole(User, role);
            var _result = new ItemResult(_methodResult);
            return _result;
        }

        /// <summary>
        /// Method: GetUserReviews
        /// </summary>
        [HttpPost("GetUserReviews")]
        [AllowAnonymous]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<System.Guid>>> GetUserReviews()
        {
            var _methodResult = await Service.GetUserReviews(User);
            var _result = new ItemResult<System.Collections.Generic.ICollection<System.Guid>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList();
            return _result;
        }

        /// <summary>
        /// Method: GetAllUsersInfo
        /// </summary>
        [HttpPost("GetAllUsersInfo")]
        [Authorize(Roles = "SuperAdmin")]
        public virtual async Task<ItemResult<System.Collections.Generic.ICollection<UserInfoDtoDtoGen>>> GetAllUsersInfo()
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(User);
            var _methodResult = await Service.GetAllUsersInfo(User);
            var _result = new ItemResult<System.Collections.Generic.ICollection<UserInfoDtoDtoGen>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList().Select(o => Mapper.MapToDto<CoalesceSample.Data.Dto.UserInfoDto, UserInfoDtoDtoGen>(o, _mappingContext, includeTree)).ToList();
            return _result;
        }

        /// <summary>
        /// Method: GetRoleList
        /// </summary>
        [HttpPost("GetRoleList")]
        [Authorize(Roles = "SuperAdmin")]
        public virtual async Task<ItemResult<string[]>> GetRoleList()
        {
            var _methodResult = await Service.GetRoleList(User);
            var _result = new ItemResult<string[]>(_methodResult);
            _result.Object = _methodResult.Object?.ToArray();
            return _result;
        }

        /// <summary>
        /// Method: ToggleUserRole
        /// </summary>
        [HttpPost("ToggleUserRole")]
        [Authorize(Roles = "SuperAdmin")]
        public virtual async Task<ItemResult> ToggleUserRole(string userEmail, string role, bool currentState)
        {
            var _methodResult = await Service.ToggleUserRole(User, userEmail, role, currentState);
            var _result = new ItemResult(_methodResult);
            return _result;
        }
    }
}
