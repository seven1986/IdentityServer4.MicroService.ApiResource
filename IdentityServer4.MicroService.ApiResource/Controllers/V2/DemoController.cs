using IdentityServer4.MicroService.ApiResource.Enums;
using IdentityServer4.MicroService.ApiResource.Models.Common;
using IdentityServer4.MicroService.ApiResource.Models.DemoController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IdentityServer4.MicroService.ApiResource.Controllers.V2
{
    [Route("Demo")]
    [Route("v{version:apiVersion}/Demo")]
    [ApiVersion("2.0-Alpha")] // 内部测试版
    //[ApiVersion("2.0")] // 正式版
    //[ApiVersion("2.0-Beta")]  // 公开测试版
    //[ApiVersion("2.0-RC")] // 候选版本
    //[ApiVersion("2017-11-06.1-RC")] // 带日期的候选版本
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class DemoController : BasicController
    {
        #region 构造函数
        public DemoController(
            IStringLocalizer<DemoController> localizer,
            ILogger<DemoController> logger
            )
        {
            l = localizer;
            log = logger;
        }
        #endregion

        /// <summary>
        /// 无需认证
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllowAnonymous")]
        [AllowAnonymous]
        [SwaggerOperation("Demo/AllowAnonymous")]
        public string AllowAnonymous()
        {
            return l["无需认证"];
        }

        #region 取钱（示例）
        /// <summary>
        /// 取钱（示例）
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 每次最多提取3000元
        /// </remarks>
        [HttpPost("WithdrawMoney")]
        [AllowAnonymous]
        [SwaggerOperation("Demo/WithdrawMoney")]
        public ApiResult<string> WithdrawMoney([FromBody]WithdrawMoneyRequest value)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResult<string>()
                {
                    code = (int)BasicControllerEnums.UnprocessableEntity,

                    message = ModelErrors(),
                };
            }

            if (value.Money > 3000)
            {
                return new ApiResult<string>(l, DemoControllerEnums.WithdrawMoneyMoreThan3000);
            }

            return new ApiResult<string>("3000");
        }
        #endregion
    }
}
