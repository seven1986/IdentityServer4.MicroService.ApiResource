using System.ComponentModel;

namespace Host.Enums
{
    /// <summary>
    /// DemoControllerEnums Enums
    /// </summary>
    internal enum ProductsControllerEnums
    {
        /// <summary>
        /// 无需验证
        /// </summary>
        [Description("无需验证")]
        AllowAnonymous = 100001,

        /// <summary>
        /// 基本验证
        /// </summary>
        [Description("基本验证")]
        Authorize = 100002,

        /// <summary>
        /// 基本验证 + 用户角色
        /// </summary>
        [Description("基本验证_用户角色")]
        AuthorizeUser = 100003,

        /// <summary>
        /// 基本验证 + 用户角色 + 用户权限
        /// </summary>
        [Description("基本验证_用户角色_用户权限")]
        AuthorizeUserPermission = 100004,

        /// <summary>
        /// 基本验证 + 用户角色组合
        /// </summary>
        [Description("基本验证_用户角色组合")]
        AuthorizeUserRoles = 100005,

        /// <summary>
        /// 基本验证 + 用户角色组合 + 用户权限
        /// </summary>
        [Description("基本验证_用户角色组合_用户权限")]
        AuthorizeUserRolesWithPermission = 100006,

        /// <summary>
        /// 基本验证 + 用户角色组合 + 客户端权限
        /// </summary>
        [Description("基本验证_用户角色组合_客户端权限")]
        AuthorizeUserRolesAndClientScope = 100007,

        /// <summary>
        /// 基本验证+用户角色组合+用户权限+客户端权限
        /// </summary>
        [Description("基本验证_用户角色组合_用户权限_客户端权限")]
        AuthorizeUserRolesWithPermissionAndClientScope = 100008,

        /// <summary>
        /// 基本验证 + 客户端权限
        /// </summary>
        [Description("基本验证_客户端权限")]
        AuthorizeClientScope = 100009,

        /// <summary>
        /// 每次最多提取2000元
        /// </summary>
        [Description("每次最多提取2000元")]
        WithdrawMoneyMoreThan2000 = 100010,

        /// <summary>
        /// 每次最多提取3000元
        /// </summary>
        [Description("每次最多提取3000元")]
        WithdrawMoneyMoreThan3000 = 100011,
    }
}
