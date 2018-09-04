using System.Collections.Generic;
using IdentityServer4.MicroService.ApiResource.Enums;
using IdentityServer4.MicroService.ApiResource.Models.Common;
using IdentityServer4.MicroService.ApiResource.Models.DemoController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using static IdentityServer4.MicroService.ApiResource.MicroserviceConfig;

namespace IdentityServer4.MicroService.ApiResource.Controllers
{
    /// <summary>
    ///  验证使用方式示例
    /// </summary>
    [Route("Demo")]
    [Produces("application/json")]
    public class DemoController : BasicController
    {
        #region 构造函数
        public DemoController(
            IStringLocalizer<DemoController> localizer,
            ILogger<DemoController> logger
            )
        {
            l = localizer;
            log = logger;
        } 
        #endregion

        #region 无需验证
        /// <summary>
        /// 无需验证
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 验证范围：
        ///  无需验证
        /// </remarks>
        [HttpGet("AllowAnonymous")]
        [AllowAnonymous]
        [SwaggerOperation("Demo/AllowAnonymous")]
        public string AllowAnonymous()
        {
            return l["无需认证"];
        } 
        #endregion

        #region 基本验证
        /// <summary>
        /// 基本验证
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 验证范围：
        ///  1，token格式
        /// </remarks>
        [HttpGet("Authorize")]
        [SwaggerOperation("Demo/Authorize")]
        public string Authorize()
        {
            return l["基本验证"];
        } 
        #endregion

        #region 基本验证 + 用户角色
        /// <summary>
        /// 基本验证 + 用户角色
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 验证范围：
        ///  1，token格式
        ///  2，用户角色必须包含：Users
        /// </remarks>
        [HttpGet("AuthorizeUser")]
        [Authorize(Roles = Roles.Users)]
        [SwaggerOperation("Demo/AuthorizeUser")]
        public string AuthorizeUser()
        {
            return l["基本验证_用户角色"];
        }
        #endregion

        #region 基本验证 + 用户角色 + 用户权限
        /// <summary>
        /// 基本验证 + 用户角色 + 用户权限
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 验证范围：
        ///  1，token格式
        ///  2，用户角色必须包含：Users
        ///  3，用户权限必须包含：Approve
        /// </remarks>
        [HttpGet("AuthorizeUserPermission")]
        [Authorize(Roles = Roles.Users)]
        [Authorize(Policy = UserPermissions.Read)]
        [SwaggerOperation("Demo/AuthorizeUserPermission")]
        public string AuthorizeUserPermission()
        {
            return l["基本验证_用户角色_用户权限"];
        }
        #endregion

        #region 基本验证 + 用户角色组合
        /// <summary>
        /// 基本验证 + 用户角色组合
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 验证范围：
        ///  1，token格式
        ///  2，用户角色必须包含：Users
        ///  3，用户角色必须包含：Owner/Partners/Administrators
        /// </remarks>
        [HttpGet("AuthorizeUserRoles")]
        [Authorize(Roles = Roles.Users)]
        [Authorize(Roles = Roles.Developer + "," + Roles.Partners + "," + Roles.Administrators)]
        [SwaggerOperation("Demo/AuthorizeUserRoles")]
        public string AuthorizeUserRoles()
        {
            return l["基本验证_用户角色组合"];
        }
        #endregion

        #region 基本验证 + 用户角色组合 + 用户权限
        /// <summary>
        /// 基本验证 + 用户角色组合 + 用户权限
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 验证范围：
        ///  1，token格式
        ///  2，用户角色必须包含：Users
        ///  3，用户角色必须包含：Owner/Partners/Administrators
        ///  4，用户权限必须包含：Approve
        /// </remarks>
        [HttpGet("AuthorizeUserRolesWithPermission")]
        [Authorize(Roles = Roles.Users)]
        [Authorize(Roles = Roles.Developer + "," + Roles.Partners + "," + Roles.Administrators)]
        [Authorize(Policy = UserPermissions.Read)]
        [SwaggerOperation("Demo/AuthorizeUserRolesWithPermission")]
        public string AuthorizeUserRolesWithPermission()
        {
            return l["基本验证_用户角色组合_用户权限"];
        }
        #endregion

        #region 基本验证 + 用户角色组合 + 客户端权限
        /// <summary>
        /// 基本验证 + 用户角色组合 + 客户端权限
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 验证范围：
        ///  1，token格式
        ///  2，用户角色必须包含：Users
        ///  3，用户角色必须包含：Owner/Partners/Administrators
        ///  4，应用权限必须包含：Approve
        /// </remarks>
        [HttpGet("AuthorizeUserRolesAndClientScope")]
        [Authorize(Roles = Roles.Users)]
        [Authorize(Roles = Roles.Developer + "," + Roles.Partners + "," + Roles.Administrators)]
        [Authorize(Policy = ClientScopes.Read)]
        [SwaggerOperation("Demo/AuthorizeUserRolesAndClientScope")]
        public string AuthorizeUserRolesAndClientScope()
        {
            return l["基本验证_用户角色组合_客户端权限"];
        }
        #endregion

        #region 基本验证 + 用户角色组合 + 用户权限 + 客户端权限
        /// <summary>
        /// 基本验证 + 用户角色组合 + 用户权限 + 客户端权限
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 验证范围：
        ///  1，token格式
        ///  2，用户角色必须包含：Users
        ///  3，用户角色必须包含：Owner/Partners/Administrators
        ///  4，用户权限必须包含：Approve
        ///  5，应用权限必须包含：Approve
        /// </remarks>
        [HttpGet("AuthorizeUserRolesWithPermissionAndClientScope")]
        [Authorize(Roles = Roles.Users)]
        [Authorize(Roles = Roles.Developer + "," + Roles.Partners + "," + Roles.Administrators)]
        [Authorize(Policy = ClientScopes.Read)]
        [Authorize(Policy = UserPermissions.Read)]
        [SwaggerOperation("Demo/AuthorizeUserRolesWithPermissionAndClientScope")]
        public string AuthorizeClientScopeAndUserPermission()
        {
            return l["基本验证_用户角色组合_用户权限_客户端权限"];
        }
        #endregion

        #region 基本验证 + 客户端权限
        /// <summary>
        /// 基本验证 + 客户端权限
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 验证范围：
        ///  1，token格式
        ///  2，应用权限必须包含：Approve
        /// </remarks>
        [HttpGet("AuthorizeClientScope")]
        [Authorize(Policy = ClientScopes.Read)]
        [SwaggerOperation("Demo/AuthorizeClientScope")]
        public string AuthorizeClientScope()
        {
            return l["基本验证_客户端权限"];
        }
        #endregion

        #region 测试 - 错误码表
        /// <summary>
        /// 测试 - 错误码表
        /// </summary>
        [HttpGet("Codes")]
        [AllowAnonymous]
        [SwaggerOperation("Demo/Codes")]
        public List<ErrorCodeModel> Codes()
        {
            var result = _Codes<DemoControllerEnums>();

            return result;
        }
        #endregion

        #region 示例：取钱
        /// <summary>
        /// 示例：取钱（无需验证）
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 每次最多提取2000元
        /// </remarks>
        [HttpPost("WithdrawMoney")]
        [AllowAnonymous]
        [SwaggerOperation("Demo/WithdrawMoney")]
        public ApiResult<string> WithdrawMoney([FromBody]WithdrawMoneyRequest value)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResult<string>()
                {
                    code = (int)BasicControllerEnums.UnprocessableEntity,

                    message = ModelErrors(),
                };
            }

            if (value.Money > 2000)
            {
                return new ApiResult<string>(l, DemoControllerEnums.WithdrawMoneyMoreThan2000);
            }

            return new ApiResult<string>("2000");
        }

        /// <summary>
        /// 示例：取钱（基本验证 + 用户权限）
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 每次最多提取2000元
        /// 验证范围：
        ///  1，token格式
        ///  2，用户权限必须包含：withdrawmoney
        /// </remarks>
        [HttpPost("WithdrawMoneyCheckUserPermission")]
        [Authorize(Policy = UserPermissions.WithdrawMoney)]
        [SwaggerOperation("Demo/WithdrawMoneyCheckUserPermission")]
        public ApiResult<string> WithdrawMoneyCheckUserPermission([FromBody]WithdrawMoneyRequest value)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResult<string>()
                {
                    code = (int)BasicControllerEnums.UnprocessableEntity,

                    message = ModelErrors(),
                };
            }

            if (value.Money > 2000)
            {
                return new ApiResult<string>(l, DemoControllerEnums.WithdrawMoneyMoreThan2000);
            }

            return new ApiResult<string>("2000");
        }

        /// <summary>
        /// 示例：取钱（基本验证 + 客户端权限）
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 每次最多提取2000元
        /// 验证范围：
        ///  1，token格式
        ///  2，客户端权限必须包含：withdrawmoney
        /// </remarks>
        [HttpPost("WithdrawMoneyCheckClientScope")]
        [Authorize(Policy = ClientScopes.WithdrawMoney)]
        [SwaggerOperation("Demo/WithdrawMoneyCheckClientScope")]
        public ApiResult<string> WithdrawMoneyCheckClientScope([FromBody]WithdrawMoneyRequest value)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResult<string>()
                {
                    code = (int)BasicControllerEnums.UnprocessableEntity,

                    message = ModelErrors(),
                };
            }

            if (value.Money > 2000)
            {
                return new ApiResult<string>(l, DemoControllerEnums.WithdrawMoneyMoreThan2000);
            }

            return new ApiResult<string>("2000");
        }

        /// <summary>
        /// 示例：取钱（基本验证 + 用户权限 + 客户端权限）
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 每次最多提取2000元
        /// 验证范围：
        ///  1，token格式
        ///  2，用户权限必须包含：withdrawmoney
        ///  3，客户端权限必须包含：withdrawmoney
        /// </remarks>
        [HttpPost("WithdrawMoneyCheckUserPermissionAndClientScope")]
        [Authorize(Policy = UserPermissions.WithdrawMoney)]
        [Authorize(Policy = ClientScopes.WithdrawMoney)]
        [SwaggerOperation("Demo/WithdrawMoneyCheckUserPermissionAndClientScope")]
        public ApiResult<string> WithdrawMoneyCheckUserPermissionAndClientScope([FromBody]WithdrawMoneyRequest value)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResult<string>()
                {
                    code = (int)BasicControllerEnums.UnprocessableEntity,

                    message = ModelErrors(),
                };
            }

            if (value.Money > 2000)
            {
                return new ApiResult<string>(l, DemoControllerEnums.WithdrawMoneyMoreThan2000);
            }

            return new ApiResult<string>("2000");
        }
        #endregion
    }
}
