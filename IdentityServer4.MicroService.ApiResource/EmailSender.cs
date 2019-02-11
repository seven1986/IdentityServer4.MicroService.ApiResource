using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer4.MicroService.ApiResource
{
    public class EmailSenderOptions
    {
        public string apiUser { get; set; }

        public string apiKey { get; set; }

        public string fromEmail { get; set; }

        public string fromName { get; set; }
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    public class EmailService
    {
        // 邮件服务
        List<EmailSenderOptions> _options { get; set; }

        ILogger<EmailService> _logger { get; }

        public EmailService(IConfiguration _config, ILogger<EmailService> logger)
        {
            _options = _config.GetValue<List<EmailSenderOptions>>("EmailOptions");

            _logger = logger;
        }

            public async Task<bool> SendEmailAsync(
            string subject, 
            string templateInvokeName,
            Dictionary<string, string[]> vars,
            string[] toEmailAddress,
            string senderApiUser)
        {
            var senderOption = _options.FirstOrDefault(x => x.apiKey == senderApiUser);

            var xsmtpapi = JsonConvert.SerializeObject(new
            {
                to = toEmailAddress,
                vars
            });

            var paramList = new SortedDictionary<string, string>()
                {
                    { "apiUser", senderOption.apiUser },
                    { "apiKey", senderOption.apiKey},
                    { "from",  senderOption.fromEmail},
                    { "fromName",senderOption.fromName},
                    { "xsmtpapi", xsmtpapi},
                    { "subject", subject},
                    { "templateInvokeName", templateInvokeName},
                };

            var postContent = new FormUrlEncodedContent(paramList);

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync("http://api.sendcloud.net/apiv2/mail/sendtemplate", postContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        try
                        {
                            var responseJson = JObject.Parse(responseString);

                            var statusCode = responseJson["statusCode"].Value<string>();

                            if (statusCode.Equals("200"))
                            {
                                return true;
                            }

                            else
                            {
                                _logger.LogError(responseString);
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                catch (Exception e)
                {
                    _logger.LogError("Send Email Error!Message :{0} ", e.Message);
                }
            }

            return false;
        }
    }
}
