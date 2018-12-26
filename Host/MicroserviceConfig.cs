using IdentityServer4.MicroService.ApiResource;
using System.ComponentModel;

namespace Host
{
    /// <summary>
    /// Client权限定义
    /// </summary>
    public class ClientScopes 
    {
        #region 验证使用方式示例
        [Description("验证使用方式示例 - 读取")]
        [PolicyClaimValues("demo", "read")]
        public const string Read = "scope:read";

        [Description("验证使用方式示例 - 取钱")]
        [PolicyClaimValues("demo", "withdrawmoney")]
        public const string WithdrawMoney = "scope:withdrawmoney";
        #endregion
    }

    /// <summary>
    /// User权限定义
    /// </summary>
    public class UserPermissions
    {
        #region 验证使用方式示例
        [Description("验证使用方式示例 - 读取")]
        [PolicyClaimValues("demo", "read")]
        public const string Read = "permission:read";

        [Description("验证使用方式示例 - 取钱")]
        [PolicyClaimValues("demo", "withdrawmoney")]
        public const string WithdrawMoney = "permission:withdrawmoney";
        #endregion
    }
}