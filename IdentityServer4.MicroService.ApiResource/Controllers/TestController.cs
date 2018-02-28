using System.Collections.Generic;
using System.Linq;
using IdentityServer4.MicroService.ApiResource.Enums;
using IdentityServer4.MicroService.ApiResource.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using static IdentityServer4.MicroService.ApiResource.MicroserviceConfig;

namespace IdentityServer4.MicroService.ApiResource.Controllers
{
    /// <summary>
    ///  测试
    /// </summary>
    [Route("Test")]
    [Produces("application/json")]
    public class TestController : BasicController
    {
        public TestController(
            IStringLocalizer<TestController> localizer,
            ILogger<TestController> logger
            )
        {
            l = localizer;
            log = logger;
        }

        /// <summary>
        /// 测试 - 无需认证
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllowAnonymous")]
        [AllowAnonymous]
        public IActionResult AllowAnonymous()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 测试 - 基本认证 (token格式必须有效)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Authorize")]
        public IActionResult Authorize()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 测试 - 基本认证 + 用户角色 (Users)
        /// </summary>
        /// <returns></returns>
        [HttpGet("AuthorizeUser")]
        [Authorize(Roles = Roles.Users)]
        public IActionResult AuthorizeUser()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 测试 - 基本认证 + 用户角色(Users) + 用户权限(Approve)
        /// </summary>
        /// <returns></returns>
        [HttpGet("AuthorizeUserPermission")]
        [Authorize(Roles = Roles.Users)]
        [Authorize(Policy = UserPermissions.Approve)]
        public IActionResult AuthorizeUserPermission()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 测试 - 基本认证 + 用户角色(Users) + 用户角色(Owner/Partners/Administrators)
        /// </summary>
        /// <returns></returns>
        [HttpGet("AuthorizeClient")]
        [Authorize(Roles = Roles.Users)]
        [Authorize(Roles = Roles.Developer + "," + Roles.Partners + "," + Roles.Administrators)]
        public IActionResult AuthorizeClient()
        {
            var d = User.Claims.FirstOrDefault(x => x.Type.Equals("role")).Value;

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 测试 - 基本认证 + 用户角色(Users) + 用户角色(Owner/Partners/Administrators) + 客户端权限(Approve)
        /// </summary>
        /// <returns></returns>
        [HttpGet("AuthorizeClientScope")]
        [Authorize(Roles = Roles.Users)]
        [Authorize(Roles = Roles.Developer + "," + Roles.Partners + "," + Roles.Administrators)]
        [Authorize(Policy = ClientScopes.Approve)]
        public IActionResult AuthorizeClientScope()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 测试 - 基本认证 + 用户角色(Users) + 用户角色(Owner/Partners/Administrators) + 客户端权限(Approve) + 用户权限(Approve)
        /// </summary>
        /// <returns></returns>
        [HttpGet("AuthorizeClientScopeAndUserPermission")]
        [Authorize(Roles = Roles.Users)]
        [Authorize(Roles = Roles.Developer + "," + Roles.Partners + "," + Roles.Administrators)]
        [Authorize(Policy = ClientScopes.Approve)]
        [Authorize(Policy = UserPermissions.Approve)]
        public IActionResult AuthorizeClientScopeAndUserPermission()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        #region 测试 - 错误码表
        /// <summary>
        /// 测试 - 错误码表
        /// </summary>
        [HttpGet("Codes")]
        [AllowAnonymous]
        [SwaggerOperation("User/Codes")]
        public List<ErrorCodeModel> Codes()
        {
            var result = _Codes<TestControllerEnums>();

            return result;
        }
        #endregion
    }
}
