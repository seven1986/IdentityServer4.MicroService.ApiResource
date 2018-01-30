using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.MicroService.ApiResource.Data.AppConstant;

namespace IdentityServer4.MicroService.ApiResource.Controllers
{
    [Route("Test")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 无需认证
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllowAnonymous")]
        [AllowAnonymous]
        public IActionResult AllowAnonymous()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 基本认证
        /// </summary>
        /// <returns></returns>
        [HttpGet("Authorize")]
        [Authorize]
        public IActionResult Authorize()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 基本认证 + 用户角色(Users)
        /// </summary>
        /// <returns></returns>
        [HttpGet("AuthorizeUser")]
        [Authorize(Roles = Roles.Users)]
        public IActionResult AuthorizeUser()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 基本认证 + 用户角色(Users) + 用户权限(Approve/All)
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
        /// 基本认证 + 用户角色(Users) + 用户角色(Owner/Partners/Administrators)
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
        /// 基本认证 + 用户角色(Users) + 用户角色(Owner/Partners/Administrators) + 客户端权限(Approve/All)
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
        /// 基本认证 + 用户角色(Users) + 用户角色(Owner/Partners/Administrators) + 客户端权限(Approve/All) + 用户权限(Approve/All)
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
    }
}
