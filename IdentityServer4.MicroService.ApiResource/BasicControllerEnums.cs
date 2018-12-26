using System.ComponentModel;

namespace IdentityServer4.MicroService.ApiResource
{
    /// <summary>
    /// 公用接口业务代码常量表
    /// </summary>
    public enum BasicControllerEnums
    {
        /// <summary>
        /// 正确
        /// </summary>
        [Description("ok")]
        Status200OK = 200,

        /// <summary>
        /// 请求实体错误
        /// </summary>
        [Description("请求实体错误")]
        UnprocessableEntity = 422,

        /// <summary>
        /// 服务器内部错误
        /// </summary>
        [Description("服务器内部错误")]
        ExpectationFailed = 417,

        /// <summary>
        /// 未找到内容
        /// </summary>
        [Description("未找到内容")]
        NotFound = 404,

        [Description("无权限进行操作")]
        NoPermission = 401,
    }
}
