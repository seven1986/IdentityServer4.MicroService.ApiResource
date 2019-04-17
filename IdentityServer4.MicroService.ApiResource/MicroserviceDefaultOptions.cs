using System.ComponentModel;

namespace IdentityServer4.MicroService.ApiResource
{
   public class MicroserviceDefaultOptions
    {
        public class Roles
        {
            /// <summary>
            ///  用户
            /// </summary>
            [DisplayName("用户")]
            public const string Users = "user";

            /// <summary>
            /// 合作商
            /// </summary>
            [DisplayName("合作商")]
            public const string Partners = "partner";

            /// <summary>
            /// 开发者
            /// </summary>
            [DisplayName("开发者")]
            public const string Developer = "developer";

            /// <summary>
            /// 管理员
            /// </summary>
            [DisplayName("管理员")]
            public const string Administrators = "administrator";
        }
    }
}
