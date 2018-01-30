using System;

namespace IdentityServer4.MicroService.ApiResource.Data
{
    public class AppConstant
    {
        /// <summary>
        /// 当前应用程序的微服务名称
        /// 在identityserver中也需要唯一
        /// </summary>
        public const string MicroServiceName = "apiresource";

        /// <summary>
        /// 策略
        /// 对应Identity的ClaimType
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

        /// <summary>
        /// Client权限定义
        /// 对应Token中的claim的scope字段
        /// 字段名：用去controller 的 action 标记
        /// 字段值：策略的名称
        /// 字段自定义属性：策略的权限集合，
        /// 聚合PolicyClaimValues所有的值（除了"all"），去重后登记到IdentityServer的ApiResource中去
        /// 例如PolicyClaimValues("apiresource.create", "apiresource.all", "all"),代表
        /// 当前apiresource项目的create权限，或者 apiresource.all权限，或者all权限
        /// </summary>
        public class ClientScopes
        {
            [PolicyClaimValues(MicroServiceName + ".create", MicroServiceName + ".all", "all")]
            public const string Create = "scope:create";

            [PolicyClaimValues(MicroServiceName + ".read", MicroServiceName + ".all", "all")]
            public const string Read = "scope:read";

            [PolicyClaimValues(MicroServiceName + ".update", MicroServiceName + ".all", "all")]
            public const string Update = "scope:update";

            [PolicyClaimValues(MicroServiceName + ".delete", MicroServiceName + ".all", "all")]
            public const string Delete = "scope:delete";

            [PolicyClaimValues(MicroServiceName + ".approve", MicroServiceName + ".all", "all")]
            public const string Approve = "scope:approve";

            [PolicyClaimValues(MicroServiceName + ".reject", MicroServiceName + ".all", "all")]
            public const string Reject = "scope:reject";

            [PolicyClaimValues(MicroServiceName + ".upload", MicroServiceName + ".all", "all")]
            public const string Upload = "scope:upload";
        }

        /// <summary>
        /// User权限定义
        /// 对应Token中的claim的permission字段
        /// 字段名：用去controller 的 action 标记
        /// 字段值：策略的名称
        /// 字段自定义属性：策略的权限集合，可按需设置User表的claims的permission属性
        /// </summary>
        public class UserPermissions
        {
            [PolicyClaimValues(MicroServiceName + ".create", MicroServiceName + ".all", "all")]
            public const string Create = "permission:create";

            [PolicyClaimValues(MicroServiceName + ".read", MicroServiceName + ".all", "all")]
            public const string Read = "permission:read";

            [PolicyClaimValues(MicroServiceName + ".update", MicroServiceName + ".all", "all")]
            public const string Update = "permission:update";

            [PolicyClaimValues(MicroServiceName + ".delete", MicroServiceName + ".all", "all")]
            public const string Delete = "permission:delete";

            [PolicyClaimValues(MicroServiceName + ".approve", MicroServiceName + ".all", "all")]
            public const string Approve = "permission:approve";

            [PolicyClaimValues(MicroServiceName + ".reject", MicroServiceName + ".all", "all")]
            public const string Reject = "permission:reject";

            [PolicyClaimValues(MicroServiceName + ".upload", MicroServiceName + ".all", "all")]
            public const string Upload = "permission:upload";
        }

        /// <summary>
        /// 角色
        /// 角色可以自定义，新增的个性化的角色也需要添加到identityserver的角色数据库里
        /// </summary>
        public class Roles
        {
            /// <summary>
            ///  用户
            /// </summary>
            public const string Users = "users";
            public const string UsersNormalizedName = "用户";

            /// <summary>
            /// 合作商
            /// </summary>
            public const string Partners = "partners";
            public const string PartnersNormalizedName = "合作商";

            /// <summary>
            /// 开发者
            /// </summary>
            public const string Developer = "developer";
            public const string DeveloperNormalizedName = "开发者";

            /// <summary>
            /// 管理员
            /// </summary>
            public const string Administrators = "administrators";
            public const string AdministratorsNormalizedName = "管理员";

            /// <summary>
            /// 艺人
            /// </summary>
            public const string Star = "star";
            public const string StarNormalizedName = "艺人";
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
        public class PolicyClaimValuesAttribute : Attribute
        {
            public string[] ClaimsValues { get; set; }

            public PolicyClaimValuesAttribute() { }

            public PolicyClaimValuesAttribute(params string[] ClaimsValues)
            {
                this.ClaimsValues = ClaimsValues;
            }
        }
    }
}
