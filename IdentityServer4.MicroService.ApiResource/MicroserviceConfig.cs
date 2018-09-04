using System.ComponentModel;
using IdentityServer4.MicroService.ApiResource.Attributes;

namespace IdentityServer4.MicroService.ApiResource
{
    public class MicroserviceConfig
    {
        /// <summary>
        /// 微服务名称
        /// </summary>
        public const string MicroServiceName = "apiresource";

        /// <summary>
        /// Client权限定义
        /// 验证权限时，会根据request.token.claims.permission判断
        /// 定义说明：
        /// 1，描述：
        ///     controller中文名称 - action中文名称
        /// 2，字段名：
        ///     跟controller 的 action 保持一直
        /// 3，字段值：
        ///     scope: {controller的action小写}
        /// 4，字段自定义属性：
        ///     [PolicyClaimValues(MicroServiceName + ".{controller的action小写}", MicroServiceName + ".all")]
        /// </summary>
        public class ClientScopes
        {
            #region 验证使用方式示例
            [Description("验证使用方式示例 - 读取")]
            [PolicyClaimValues(MicroServiceName + ".read", MicroServiceName + ".all")]
            public const string Read = "scope:read";

            [Description("验证使用方式示例 - 取钱")]
            [PolicyClaimValues(MicroServiceName + ".withdrawmoney", MicroServiceName + ".all")]
            public const string WithdrawMoney = "scope:withdrawmoney";
            #endregion
        }

        /// <summary>
        /// User权限定义
        /// 验证权限时，会根据request.token.claims.permission判断
        /// 定义说明：
        /// 1，描述：
        ///     controller中文名称 - action中文名称
        /// 2，字段名：
        ///     跟controller 的 action 保持一直
        /// 3，字段值：
        ///     permission: controller的action小写
        /// 4，字段自定义属性：
        ///     [PolicyClaimValues(MicroServiceName + ".{controller的action小写}", MicroServiceName + ".all")]
        /// </summary>
        public class UserPermissions
        {
            #region 验证使用方式示例
            [Description("验证使用方式示例 - 读取")]
            [PolicyClaimValues(MicroServiceName + ".read", MicroServiceName + ".all")]
            public const string Read = "permission:read";

            [Description("验证使用方式示例 - 取钱")]
            [PolicyClaimValues(MicroServiceName + ".withdrawmoney", MicroServiceName + ".all")]
            public const string WithdrawMoney = "permission:withdrawmoney";
            #endregion
        }

        /// <summary>
        /// 角色
        /// </summary>
        public class Roles
        {
            /// <summary>
            ///  用户
            /// </summary>
            [DisplayName("用户")]
            public const string Users = "users";

            /// <summary>
            /// 合作商
            /// </summary>
            [DisplayName("合作商")]
            public const string Partners = "partners";

            /// <summary>
            /// 开发者
            /// </summary>
            [DisplayName("开发者")]
            public const string Developer = "developer";

            /// <summary>
            /// 管理员
            /// </summary>
            [DisplayName("管理员")]
            public const string Administrators = "administrators";
        }

        /// <summary>
        /// UserPermission,ClientScope
        /// </summary>
        public class ClaimTypes
        {
            /// <summary>
            /// for User
            /// </summary>
            public const string UserPermission = "permission";

            /// <summary>
            /// for client
            /// </summary>
            public const string ClientScope = "scope";
        }
    }
}