using System;
using System.Collections.Generic;

namespace IdentityServer4.MicroService.ApiResource
{
    public class MicroserviceOptions
    {
        /// <summary>
        /// 微服务名称。必须是小写英文（非必填）
        /// 优先级1：Startup.cs
        /// 优先级2：appsettings.json：MicroService:Name
        /// 优先级3：AssemblyName
        /// </summary>
        public string MicroServiceName { get; set; }

        /// <summary>
        /// 微服务显示名称。（非必填）
        /// 优先级1：Startup.cs
        /// 优先级2：appsettings.json：MicroService:DisplayName
        /// 优先级3：取MicroServiceName
        /// </summary>
        public string MicroServiceDisplayName { get; set; }

        /// <summary>
        /// 微服务简介。（非必填）
        /// 优先级1：Startup.cs
        /// 优先级2：appsettings.json：MicroService:Description
        /// 优先级3：取MicroServiceName
        /// </summary>
        public string MicroServiceDescription { get; set; }

        /// <summary>
        /// 微服务授权Client的ID集合（非必填）
        /// </summary>
        public List<string> MicroServiceClientIDs { get; set; } = new List<string>();

        /// <summary>
        /// 微服务授权的Client的回调地址集合（必填）
        /// 优先级1：Startup.cs
        /// 优先级2：appsettings.json：MicroService:Description
        /// 未填写将报错
        /// </summary>
        public List<string> MicroServiceRedirectUrls { get; set; } = new List<string>();

        /// <summary>
        /// 身份认证中心网址
        /// 默认读取appsettings.json文件IdentityServer:Host
        /// 如：https://127.0.0.1（必须为https，网址结尾无需带/）
        /// </summary>
        public Uri IdentityServerUri { get; set; }

        /// <summary>
        /// 启用跨域（默认true）
        /// </summary>
        public bool EnableCors { get; set; } = true;

        /// <summary>
        /// 启用多语言（默认true）
        /// </summary>
        public bool EnableLocalization { get; set; } = true;

        /// <summary>
        /// 启用版本（默认true）
        /// </summary>
        public bool EnableApiVersioning { get; set; } = true;

        /// <summary>
        /// 启用WebEncoders（默认true）
        /// </summary>
        public bool EnableWebEncoders { get; set; } = true;

        /// <summary>
        /// 启用缓存（默认true）
        /// </summary>
        public bool EnableResponseCaching { get; set; } = true;

        /// <summary>
        /// 启用权限（默认true）
        /// </summary>
        public bool EnableAuthorizationPolicy { get; set; } = true;

        /// <summary>
        /// 启用SwaggerGen（默认true）
        /// </summary>
        public bool EnableSwaggerGen { get; set; } = true;

        /// <summary>
        /// 启用SwaggerUI（默认true）
        /// </summary>
        public bool EnableSwaggerUI { get; set; } = true;

        /// <summary>
        /// 注册服务到IdentityServer（默认true）
        /// </summary>
        public bool ImportToIdentityServer { get; set; } = true;
    }
}
