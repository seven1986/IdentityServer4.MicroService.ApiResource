using System.ComponentModel;

namespace IdentityServer4.MicroService.ApiResource.Enums
{
    /// <summary>
    /// TestController Enums
    /// </summary>
    internal enum TestControllerEnums
    {
        /// <summary>
        /// AllowAnonymous_1
        /// </summary>
        [Description("AllowAnonymous_1")]
        AllowAnonymous_1 = 100001,

        /// <summary>
        /// AllowAnonymous_2
        /// </summary>
        [Description("AllowAnonymous_2")]
        AllowAnonymous_2 = 100002,

        /// <summary>
        /// Authorize_1
        /// </summary>
        [Description("Authorize_1")]
        Authorize_1 = 100003,

        /// <summary>
        /// Authorize_2
        /// </summary>
        [Description("Authorize_2")]
        Authorize_2 = 100004,
    }
}
