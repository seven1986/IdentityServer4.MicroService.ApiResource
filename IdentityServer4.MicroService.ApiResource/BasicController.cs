using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace IdentityServer4.MicroService.ApiResource
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class BasicController : ControllerBase
    {
        /// <summary>
        /// 全球化
        /// </summary>
        public virtual IStringLocalizer l { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        protected readonly Random random = new Random(DateTime.UtcNow.Second);

        #region Claims
        Dictionary<string, string> _UserClaims;
        Dictionary<string, string> UserClaims
        {
            get
            {
                if (_UserClaims == null)
                {
                    _UserClaims = new Dictionary<string, string>();

                    var claims = User.Claims.ToList();

                    foreach (var c in claims)
                    {
                        if (!_UserClaims.ContainsKey(c.Type))
                        {
                            _UserClaims.Add(c.Type, c.Value);
                        }
                    }
                }

                return _UserClaims;
            }
        }
        #endregion

        protected long UserId
        {
            get
            {
                return long.Parse(UserClaims["sub"]);
            }
        }

        /// <summary>
        /// 生成MD5
        /// </summary>
        /// <returns></returns>
        protected string MD5String(string str)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bs = Encoding.UTF8.GetBytes(str);
            bs = md5.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToUpper());
            }
            var password = s.ToString();
            return password;
        }

        /// <summary>
        /// 根据枚举，返回值与名称的字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected List<ApiCodeModel> ApiCodes<T>()
        {
            var t = typeof(T);

            var items = t.GetFields()
                .Where(x => x.CustomAttributes.Count() > 0).ToList();

            var result = new List<ApiCodeModel>();

            foreach (var item in items)
            {
                var code = long.Parse(item.GetRawConstantValue().ToString());

                var codeName = item.Name;

                var desc = item.GetCustomAttribute<DescriptionAttribute>();

                var codeItem = new ApiCodeModel()
                {
                    Code = code,
                    Name = codeName,
                    Description = l != null ? l[desc.Description] : desc.Description
                };

                result.Add(codeItem);
            }

            return result;
        }
    }
}
