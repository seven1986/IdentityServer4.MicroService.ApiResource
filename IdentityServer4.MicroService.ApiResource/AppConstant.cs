using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace IdentityServer4.MicroService.ApiResource
{
    public class AppConstant
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public static string AssemblyName = Assembly.GetEntryAssembly().GetName().Name;

        public static string SwaggerUIClientId = "swagger";

        public static string SwaggerUIClientName = "swagger";

        public static string SwaggerUIClientSecret = "swagger";

        public const string TenantTokenKey = "tenant";
    }

    public class PolicyConfig
    {
        public string ControllerName { get; set; }

        public List<string> Scopes { get; set; } = new List<string>();

        public List<string> Permissions { get; set; } = new List<string>();
    }

    /// <summary>
    /// PolicyKey(ClaimType)
    /// </summary>
    public class PolicyKey
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
    /// 角色
    /// </summary>
    public class DefaultRoles
    {
        /// <summary>
        ///  用户
        /// </summary>
        [DisplayName("用户")]
        public const string User = "user";

        /// <summary>
        /// 合作商
        /// </summary>
        [DisplayName("合作商")]
        public const string Partner = "partner";

        /// <summary>
        /// 开发者
        /// </summary>
        [DisplayName("开发者")]
        public const string Developer = "developer";

        /// <summary>
        /// 管理员
        /// </summary>
        [DisplayName("管理员")]
        public const string Administrator = "administrator";
    }
}
