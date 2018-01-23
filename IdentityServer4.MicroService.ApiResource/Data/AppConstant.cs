using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.MicroService.ApiResource.Data
{
    public class AppConstant
    {
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
        /// 对应ClaimTypes.Scope
        /// Client的Scope权限定义
        /// 格式：resourceName.scopeName
        /// 对应Identity的ClaimValue
        /// </summary>
        public class ClientScopes
        {
            public const string Create = "apiresource.create";
            public const string Read = "apiresource.read";
            public const string Update = "apiresource.update";
            public const string Delete = "apiresource.delete";
            public const string Approve = "apiresource.approve";
            public const string Reject = "apiresource.reject";
            public const string Upload = "apiresource.upload";
            public const string All = "apiresource.all";
        }

        /// <summary>
        /// 对应ClaimTypes.Permission
        /// User的Permission权限定义
        /// </summary>
        public class UserPermissions
        {
            public const string Create = "create";
            public const string Read = "read";
            public const string Update = "update";
            public const string Delete = "delete";
            public const string Approve = "approve";
            public const string Reject = "reject";
            public const string Upload = "upload";
            public const string All = "all";
        }

        /// <summary>
        /// 角色
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
    }
}
