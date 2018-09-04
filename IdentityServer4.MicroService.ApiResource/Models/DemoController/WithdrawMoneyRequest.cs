using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.MicroService.ApiResource.Models.DemoController
{
    /// <summary>
    /// 取钱请求实体
    /// </summary>
    public class WithdrawMoneyRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Required(ErrorMessage = "用户账号不得为空")]
        [StringLength(18, MinimumLength = 4, ErrorMessage = "账号长度应在4至18位之间")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "用户密码不得为空")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "密码长度应在6至18位之间")]
        public string UserPwd { get; set; }


        /// <summary>
        /// 提取金额
        /// </summary>
        [Range(0, long.MaxValue, ErrorMessage = "提取额度范围应在0至9223372036854775807之间")]
        public long Money { get; set; }
    }
}
