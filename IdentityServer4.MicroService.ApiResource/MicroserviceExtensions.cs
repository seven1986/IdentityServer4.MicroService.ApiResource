using IdentityServer4.AccessTokenValidation;
using IdentityServer4.MicroService.ApiResource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MicroserviceExtensions
    {
        /// <summary>
        /// Creates a builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IMicroserviceBuilder AddMicroServiceBuilder(this IServiceCollection services)
        {
            return new MicroserviceBuilder(services);
        }

        /// <summary>
        /// Creates a builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The config.</param>
        /// <returns></returns>
        public static IMicroserviceBuilder AddMicroService(
            this IServiceCollection services,
            MicroserviceOptions options)
        {
            var builder = services.AddMicroServiceBuilder();

            if (!string.IsNullOrWhiteSpace(options.MicroServiceName))
            {
                options.MicroServiceName = options.MicroServiceName.ToLower().Trim();
            }
            else
            {
                options.MicroServiceName = options.AssemblyName.ToLower().Trim();
            }

            builder.Services.AddSingleton(options);

            if (options.Cors)
            {
                builder.Services.AddCors(o =>
                {
                    o.AddPolicy("default", b =>
                    {
                        b.AllowAnyHeader();
                        b.AllowAnyMethod();
                        b.AllowAnyOrigin();
                        b.AllowCredentials();
                    });
                });
            }

            if (options.IdentityServer != null)
            {
                builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                        .AddIdentityServerAuthentication(o =>
                        {
                            o.Authority = options.IdentityServer.Scheme + options.IdentityServer.Host;
                            o.ApiName = options.MicroServiceName;
                        });
            }

            if (options.AuthorizationPolicy)
            {
                builder.Services.AddAuthorization(o =>
                {
                    #region Client的权限策略
                    var scopes = options.Scopes.GetFields();

                    foreach (var scope in scopes)
                    {
                        var scopeName = scope.GetRawConstantValue().ToString();

                        var scopeItem = scope.GetCustomAttribute<PolicyClaimValuesAttribute>();

                        var scopeValues = scopeItem.PolicyValues;

                        var scopeValuesList = new List<string>();

                        for (var i = 0; i < scopeValues.Length; i++)
                        {
                            scopeValues[i] = options.MicroServiceName + "." + scopeValues[i];

                            scopeValuesList.Add(scopeValues[i]);
                        }

                        scopeValuesList.Add(options.MicroServiceName + "." + scopeItem.ControllerName + ".all");

                        scopeValuesList.Add(options.MicroServiceName + ".all");

                        o.AddPolicy(scopeName, policy => policy.RequireClaim(ClaimTypes.ClientScope, scopeValuesList));
                    }
                    #endregion

                    #region User的权限策略
                    var permissions = options.Permissions.GetFields();

                    foreach (var permission in permissions)
                    {
                        var permissionName = permission.GetRawConstantValue().ToString();

                        var permissionItem = permission.GetCustomAttribute<PolicyClaimValuesAttribute>();

                        var permissionValues = permissionItem.PolicyValues;

                        var permissionValuesList = new List<string>();

                        for (var i = 0; i < permissionValues.Length; i++)
                        {
                            permissionValues[i] = options.MicroServiceName + "." + permissionValues[i];

                            permissionValuesList.Add(permissionValues[i]);
                        }

                        permissionValuesList.Add(options.MicroServiceName + "." + permissionItem.ControllerName + ".all");

                        permissionValuesList.Add(options.MicroServiceName + ".all");

                        o.AddPolicy(permissionName,
                            policy => policy.RequireAssertion(context =>
                            {
                                var userPermissionClaim = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.UserPermission));

                                if (userPermissionClaim != null && !string.IsNullOrWhiteSpace(userPermissionClaim.Value))
                                {
                                    var userPermissionClaimValue = userPermissionClaim.Value.ToLower().Split(new string[] { "," },
                                        StringSplitOptions.RemoveEmptyEntries);

                                    if (userPermissionClaimValue != null && userPermissionClaimValue.Length > 0)
                                    {
                                        foreach (var userPermissionItem in userPermissionClaimValue)
                                        {
                                            if (permissionValuesList.Contains(userPermissionItem))
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
            }

            if (options.SwaggerGen)
            {
                builder.Services.AddSwaggerGen(c =>
                {
                    c.EnableAnnotations();

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

                    if (options.IdentityServer != null)
                    {
                        c.AddSecurityDefinition("OAuth2",
                            new OAuth2Scheme()
                            {
                                Type = "oauth2",
                                Flow = "accessCode",
                                AuthorizationUrl = "https://" + options.IdentityServer.Host + "/connect/authorize",
                                TokenUrl = "https://" + options.IdentityServer.Host + "/connect/token",
                                Description = "勾选授权范围，获取Token",
                                Scopes = new Dictionary<string, string>(){
                                    { "openid","用户标识" },
                                    { "profile","用户资料" },
                                    { options.MicroServiceName+ ".all","所有接口权限" },
                                }
                            });
                    }

                    var provider = services.BuildServiceProvider()
                                   .GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerDoc(description.GroupName, new Info
                        {
                            Title = options.AssemblyName,
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

                        c.CustomSchemaIds(x => x.FullName);
                    }

                    var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, options.AssemblyName + ".xml");

                    c.IncludeXmlComments(filePath);
                });
            }

            if (options.Localization) {
                // Configure supported cultures and localization options
                builder.Services.AddLocalization(o => o.ResourcesPath = "Resources");

                builder.Services.Configure<RequestLocalizationOptions>(o =>
                {
                    var supportedCultures = new[]
                    {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN"),
                    };

                    // State what the default culture for your application is. This will be used if no specific culture
                    // can be determined for a given request.
                    o.DefaultRequestCulture = new RequestCulture("zh-CN", "zh-CN");

                    // You must explicitly state which cultures your application supports.
                    // These are the cultures the app supports for formatting numbers, dates, etc.
                    o.SupportedCultures = supportedCultures;

                    // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                    o.SupportedUICultures = supportedCultures;

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
            }

            if (options.ApiVersioning)
            {
                //https://github.com/Microsoft/aspnet-api-versioning/wiki/API-Documentation#aspnet-core
                builder.Services.AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

                builder.Services.AddApiVersioning(o =>
                {
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.ReportApiVersions = true;
                });
            }

            if (options.WebEncoders)
            {
                services.AddWebEncoders(opt =>
                {
                    opt.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
                });
            }

            builder.Services.AddMvc()
            .AddDataAnnotationsLocalization()
            //https://stackoverflow.com/questions/34753498/self-referencing-loop-detected-in-asp-net-core
            .AddJsonOptions(o =>
            {
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddSingleton<AzureStorageService>();

            builder.Services.AddTransient<EmailService>();

            builder.Services.AddResponseCaching();

            if (!string.IsNullOrWhiteSpace(options.SQLCacheConnection))
            {
                builder.Services.AddDistributedSqlServerCache(x =>
                {
                    x.ConnectionString = options.SQLCacheConnection;
                    x.SchemaName = "dbo";
                    x.TableName = "AppCache";
                });
            }

            return builder;
        }

       
    }

    public interface IMicroserviceBuilder
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        IServiceCollection Services { get; }
    }

    public class MicroserviceBuilder : IMicroserviceBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IMicroserviceBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public MicroserviceBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public IServiceCollection Services { get; }
    }
}
