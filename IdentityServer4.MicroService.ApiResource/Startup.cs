using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.MicroService.ApiResource.Attributes;
using IdentityServer4.MicroService.ApiResource.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using static IdentityServer4.MicroService.ApiResource.MicroserviceConfig;

namespace IdentityServer4.MicroService.ApiResource
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Cors
            services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                    builder.AllowCredentials();
                });
            });
            #endregion

            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            #region Authentication
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = "https://" + Configuration["IdentityServer"];
                        options.ApiName = MicroServiceName;
                    });
            #endregion

            #region 权限定义
            services.AddAuthorization(options =>
            {
                #region Client的权限策略
                var scopes = typeof(ClientScopes).GetFields();

                foreach (var scope in scopes)
                {
                    var scopeName = scope.GetRawConstantValue().ToString();

                    var scopeValues = scope.GetCustomAttribute<PolicyClaimValuesAttribute>().ClaimsValues;

                    options.AddPolicy(scopeName, policy => policy.RequireClaim(ClaimTypes.ClientScope, scopeValues));
                }
                #endregion

                #region User的权限策略
                var permissions = typeof(UserPermissions).GetFields();

                foreach (var permission in permissions)
                {
                    var permissionName = permission.GetRawConstantValue().ToString();

                    var permissionValues = permission.GetCustomAttribute<PolicyClaimValuesAttribute>().ClaimsValues;

                    options.AddPolicy(permissionName,
                        policy => policy.RequireAssertion(context =>
                        {
                            var userPermissionClaim = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.UserPermission));

                            if (userPermissionClaim != null && !string.IsNullOrWhiteSpace(userPermissionClaim.Value))
                            {
                                var userPermissionClaimValue = userPermissionClaim.Value.ToLower().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                                if (userPermissionClaimValue != null && userPermissionClaimValue.Length > 0)
                                {
                                    foreach (var userPermissionItem in userPermissionClaimValue)
                                    {
                                        if (permissionValues.Contains(userPermissionItem))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }

                            return false;
                        }));
                }
                #endregion
            });
            #endregion

            #region SwaggerGen
            services.AddSwaggerGen(c =>
            {
                // c.TagActionsBy(x => x.RelativePath.Split('/')[0]);

                c.AddSecurityDefinition("SubscriptionKey",
                    new ApiKeyScheme()
                    {
                        Name = "Ocp-Apim-Subscription-Key",
                        Type = "apiKey",
                        In = "header",
                        Description = "从开放平台申请的Subscription Key，从网关调用接口时必需传入。",
                    });

                c.AddSecurityDefinition("AccessToken",
                    new ApiKeyScheme()
                    {
                        Name = "Authorization",
                        Type = "apiKey",
                        In = "header",
                        Description = "从身份认证中心颁发的Token，根据接口要求决定是否传入。",
                    });

                c.AddSecurityDefinition("OAuth2",
                    new OAuth2Scheme()
                    {
                        Type = "oauth2",
                        Flow = "accessCode",
                        AuthorizationUrl = "https://" + Configuration["IdentityServer"] + "/connect/authorize",
                        TokenUrl = "https://" + Configuration["IdentityServer"] + "/connect/token",
                        Description = "勾选授权范围，获取Token",
                        Scopes = new Dictionary<string, string>(){
                            { "openid","用户标识" },
                            { "profile","用户资料" },
                            { MicroServiceName+ ".all","所有接口权限"},
                        }
                    });

                c.OperationFilter<FormFileOperationFilter>();

                var provider = services.BuildServiceProvider()
                               .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new Info
                    {
                        Title = assemblyName,
                        Version = description.ApiVersion.ToString(),
                        License = new License()
                        {
                            Name = "MIT",
                            Url = "https://spdx.org/licenses/MIT.html"
                        },
                        // Contact = new Contact()
                        // {
                        //     Url = "",
                        //     Name = "",
                        //     Email = ""
                        // },
                        // Description = "Swagger document",
                    });
                }

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, assemblyName + ".xml");

                c.IncludeXmlComments(filePath);
            });
            #endregion

            #region Mvc + localization
            // Configure supported cultures and localization options
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN"),
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture("zh-CN", "zh-CN");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

                // You can change which providers are configured to determine the culture for requests, or even add a custom
                // provider with your own logic. The providers will be asked in order to provide a culture for each request,
                // and the first to provide a non-null result that is in the configured supported cultures list will be used.
                // By default, the following built-in providers are configured:
                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
                //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                //{
                //  // My custom request culture logic
                //  return new ProviderCultureResult("en");
                //}));
            });

            //https://github.com/Microsoft/aspnet-api-versioning/wiki/API-Documentation#aspnet-core
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            })
            .AddDataAnnotationsLocalization()
            //https://stackoverflow.com/questions/34753498/self-referencing-loop-detected-in-asp-net-core
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider provider)
        {
            app.UseCors("default");

            #region Localization
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            #region swagger
            app.UseSwagger(x =>
            {
                x.PreSerializeFilters.Add((doc, req) =>
                {
                    doc.Schemes = new[] { "https" };
                    doc.Host = Configuration["ApplicationHost"];
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

            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());

                    c.OAuthClientId(AppDefaultData.TestClient.ClientId);
                    c.OAuthClientSecret(AppDefaultData.TestClient.ClientSecret);
                    c.OAuthAppName(AppDefaultData.TestClient.ClientName);
                    c.OAuth2RedirectUrl(string.Format(AppDefaultData.TestClient.RedirectUris[0], Configuration["ApplicationHost"]));
                }

                c.DocExpansion(DocExpansion.None);
            });
            #endregion
        }
    }
}
