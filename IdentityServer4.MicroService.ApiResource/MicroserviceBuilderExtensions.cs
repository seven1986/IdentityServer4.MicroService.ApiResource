using IdentityServer4.MicroService.ApiResource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class MicroserviceBuilderExtensions
    {
        public static IApplicationBuilder UseMicroservice(this IApplicationBuilder builder)
        {
            var env = builder.ApplicationServices.GetService<IHostingEnvironment>();

            var Configuration = builder.ApplicationServices.GetService<IConfiguration>();

            var options = builder.ApplicationServices.GetService<MicroserviceOptions>();

            if (options.IdentityServerUri == null)
            {
                try
                {
                    options.IdentityServerUri = new Uri(Configuration["MicroService:IdentityServer"]);
                }
                catch
                {
                    //throw new KeyNotFoundException("appsettings.json文件，没有配置IdentityServer:Host");
                }
            }

            if (options.ImportToIdentityServer)
            {
                ImportToIdentityServer(options);
            }

            if (options.EnableCors)
            {
                builder.UseCors("cors-allowanonymous");
            }

            if (options.EnableLocalization)
            {
                var locOptions = builder.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

                builder.UseRequestLocalization(locOptions.Value);
            }

            builder.UseAuthentication();

            var httpsEndpoint = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");

            if (string.IsNullOrWhiteSpace(httpsEndpoint))
            {
                httpsEndpoint = "localhost:" + Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT");
            }

            if (options.EnableSwaggerGen)
            {
                builder.UseSwagger(x =>
                {
                    x.PreSerializeFilters.Add((doc, req) =>
                    {
                        doc.Schemes = new[] { "https" };
                        doc.Host = httpsEndpoint;
                        doc.Security = new List<IDictionary<string, IEnumerable<string>>>()
                        {
                            new Dictionary<string, IEnumerable<string>>()
                            {
                                { "SubscriptionKey", new string[]{ } },
                                { "AccessToken", new string[]{ } },
                                { "OAuth2", new string[]{ } },
                            }
                        };
                    });
                });
            }

            if (options.EnableSwaggerUI)
            {
                builder.UseSwaggerUI(c =>
                    {
                        var provider = builder.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            c.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());

                            c.OAuthAppName(AppConstant.SwaggerUIClientName);
                            c.OAuthClientId(AppConstant.SwaggerUIClientId);
                            c.OAuthClientSecret(AppConstant.SwaggerUIClientSecret);
                            c.OAuth2RedirectUrl($"https://{httpsEndpoint}/swagger/oauth2-redirect.html");
                        }

                        c.DocExpansion(DocExpansion.None);
                    });
            }

            if (options.EnableResponseCaching)
            {
                builder.UseResponseCaching();
            }

            return builder;
        }


        static void ImportToIdentityServer(MicroserviceOptions MSOptions)
        {
            if (MSOptions.IdentityServerUri == null) { return; }

            var url = $"{MSOptions.IdentityServerUri.OriginalString}/api/ApiResource/Import";

            using (var hc = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    MSOptions.MicroServiceName,

                    MSOptions.MicroServiceDisplayName,

                    MSOptions.MicroServiceDescription,

                    MSOptions.MicroServiceClientIDs,

                    MSOptions.MicroServiceRedirectUrls,

                    MicroServicePolicies = MicroserviceExtensions.EntryAssemblyPolicies()

                }), Encoding.UTF8, "application/json");

                try
                {
                    var response = hc.PostAsync(url, content).Result;
                }
                catch {

                }
            }
        }
    }
}