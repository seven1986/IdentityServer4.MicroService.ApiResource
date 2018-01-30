using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.MicroService.ApiResource.Controllers.V2
{
    [Route("Test")]
    [Route("v{version:apiVersion}/Test")]
    [ApiVersion("2.0-Alpha")] // 内部测试版
    //[ApiVersion("2.0")] // 正式版
    //[ApiVersion("2.0-Beta")]  // 公开测试版
    //[ApiVersion("2.0-RC")] // 候选版本
    //[ApiVersion("2017-11-06.1-RC")] // 带日期的候选版本
    //[ApiExplorerSettings(IgnoreApi = true)]
    //[ServiceFilter(typeof(ApiTracker.ApiTracker), IsReusable = true)]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 无需认证
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllowAnonymous")]
        [AllowAnonymous]
        public IActionResult AllowAnonymous()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
