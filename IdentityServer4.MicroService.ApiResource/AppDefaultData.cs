using System.Collections.Generic;

namespace IdentityServer4.MicroService.ApiResource
{
    public class AppDefaultData
    {
        /// <summary>
        /// 初始Client(可以用来测试)
        /// </summary>
        public class TestClient
        {
            public const string ClientId = "test";
            public const string ClientName = "测试专用";
            public const string ClientSecret = "1";
            public static List<string> RedirectUris = new List<string>()
            {
                "https://{0}/swagger/oauth2-redirect.html"
            };
        }
    }
}
