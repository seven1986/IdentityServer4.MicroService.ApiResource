using System;

namespace IdentityServer4.MicroService.ApiResource
{
    public class MicroserviceOptions
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 微服务名称，必须是英文
        /// </summary>
        public string MicroServiceName { get; set; }

        /// <summary>
        /// IdentityServer服务器地址
        /// </summary>
        public Uri IdentityServer { get; set; }

        /// <summary>
        /// 启用跨域
        /// </summary>
        public bool Cors { get; set; } = true;

        /// <summary>
        /// 启用多语言
        /// </summary>
        public bool Localization { get; set; } = true;

        /// <summary>
        /// 启用版本
        /// </summary>
        public bool ApiVersioning { get; set; } = true;

        /// <summary>
        /// 启用权限
        /// </summary>
        public bool AuthorizationPolicy { get; set; } = true;

        /// <summary>
        /// 启用SwaggerGen
        /// </summary>
        public bool SwaggerGen { get; set; } = true;

        /// <summary>
        /// 启用SwaggerUI
        /// </summary>
        public bool SwaggerUI { get; set; } = true;

        /// <summary>
        /// SwaggerUI ClientID
        /// </summary>
        public string SwaggerUIClientID { get; set; } = "test";

        /// <summary>
        /// SwaggerUI ClientName
        /// </summary>
        public string SwaggerUIClientName { get; set; } = "测试";

        /// <summary>
        /// SwaggerUI ClientSecret
        /// </summary>
        public string SwaggerUIClientSecret { get; set; } = "1";

        /// <summary>
        /// 客户端权限
        /// </summary>
        public Type Scopes { get; set; }

        /// <summary>
        /// 用户权限
        /// </summary>
        public Type Permissions { get; set; }

        /// <summary>
        /// 启用缓存
        /// </summary>
        public bool EnableResponseCaching { get; set; } = true;

        /// <summary>
        /// 启用缓存的SQL数据库链接
        /// </summary>
        public string SQLCacheConnection { get; set; }

    }

    public class ClaimTypes
    {
        /// <summary>
        /// permission (for User)
        /// </summary>
        public const string UserPermission = "permission";

        /// <summary>
        /// scope (for client)
        /// </summary>
        public const string ClientScope = "scope";
    }
}
