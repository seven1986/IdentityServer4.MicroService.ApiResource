using IdentityServer4.MicroService.ApiResource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;

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
                    options.IdentityServerUri = new Uri(Configuration["IdentityServer:Host"]);
                }
                catch
                {
                    //throw new KeyNotFoundException("appsettings.json文件，没有配置IdentityServer:Host");
                }
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

            if (options.EnableAuthorizationPolicy)
            {
                builder.UseAuthentication();
            }

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
                            c.OAuth2RedirectUrl(string.Format(httpsEndpoint + "/swagger/oauth2-redirect.html", httpsEndpoint));
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
    }
}