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
        public static IApplicationBuilder UseMicroservice(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();

            var Configuration = app.ApplicationServices.GetService<IConfiguration>();

            var options = app.ApplicationServices.GetService<MicroserviceOptions>();

            var identityServer = Configuration["IdentityServer"];

            if (!string.IsNullOrWhiteSpace(identityServer))
            {
                options.IdentityServer = new Uri(identityServer);
            }

            if (options.Cors)
            {
                app.UseCors("default");
            }

            if (options.Localization)
            {
                var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

                app.UseRequestLocalization(locOptions.Value);
            }

            if (options.AuthorizationPolicy)
            {
                app.UseAuthentication();
            }

            var httpsEndpoint = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");

            if (string.IsNullOrWhiteSpace(httpsEndpoint))
            {
                httpsEndpoint = "localhost:" + Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT");
            }

            if (options.SwaggerGen)
            {
                app.UseSwagger(x =>
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

            if (options.SwaggerUI)
            {
                app.UseSwaggerUI(c =>
                    {
                        var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            c.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());

                            c.OAuthAppName(options.SwaggerUIClientName);
                            c.OAuthClientId(options.SwaggerUIClientID);
                            c.OAuthClientSecret(options.SwaggerUIClientSecret);
                            c.OAuth2RedirectUrl(string.Format(httpsEndpoint + "/swagger/oauth2-redirect.html", httpsEndpoint));
                        }

                        c.DocExpansion(DocExpansion.None);
                    });
            }

            if (options.EnableResponseCaching)
            {
                app.UseResponseCaching();
            }

            return app;
        }
    }
}