using System;

namespace IdentityServer4.MicroService.ApiResource
{
    public class MicroserviceOptions
    {
        /// <summary>
        /// 微服务名称。必须是小写英文
        /// </summary>
        public string MicroServiceName { get; set; }

        /// <summary>
        /// 当前项目的网址(默认读取IdentityServer:Host)
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
        /// 启用WebEncoders
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
    }
}
