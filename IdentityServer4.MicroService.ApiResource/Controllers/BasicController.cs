using IdentityServer4.MicroService.ApiResource.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace IdentityServer4.MicroService.ApiResource.Controllers
{
    [Authorize]
    public class BasicController : ControllerBase
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public virtual DbContext db { get; set; }

        /// <summary>
        /// 全球化
        /// </summary>
        public virtual IStringLocalizer l { get; set; }

        /// <summary>
        /// 日志
        /// </summary>
        public virtual ILogger log { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        protected readonly Random random = new Random(DateTime.UtcNow.Second);

        /// <summary>
        /// 获取入参错误信息
        /// </summary>
        /// <returns></returns>
        protected string ModelErrors()
        {
            var errObject = new JObject();

            foreach (var errKey in ModelState.Keys)
            {
                var errValues = ModelState[errKey];

                var errMessages = errValues.Errors.Select(x => !string.IsNullOrWhiteSpace(x.ErrorMessage) ? l[x.ErrorMessage] : x.Exception.Message).ToList();

                if (errMessages.Count > 0)
                {
                    errObject.Add(errKey, JToken.FromObject(errMessages));
                }
            }

            return JsonConvert.SerializeObject(errObject);
        }

        #region Token Claims
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
        protected long UserId
        {
            get
            {
                return long.Parse(UserClaims["sub"]);
            }
        } 
        #endregion

        /// <summary>
        /// 生成MD5
        /// </summary>
        /// <returns></returns>
        protected string MD5String(String str)
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
        protected List<ErrorCodeModel> _Codes<T>()
        {
            var t = typeof(T);

            var items = t.GetFields()
                .Where(x => x.CustomAttributes.Count() > 0).ToList();

            var result = new List<ErrorCodeModel>();

            foreach (var item in items)
            {
                var code = long.Parse(item.GetRawConstantValue().ToString());

                var codeName = item.Name;

                var desc = item.GetCustomAttribute<DescriptionAttribute>();

                var codeItem = new ErrorCodeModel()
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
