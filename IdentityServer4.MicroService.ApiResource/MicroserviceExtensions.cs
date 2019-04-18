using IdentityServer4.AccessTokenValidation;
using IdentityServer4.MicroService.ApiResource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
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
        /// <param name="configuration">The config.</param>
        /// <returns></returns>
        public static IMicroserviceBuilder AddMicroService(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<MicroserviceOptions> msOptions = null)
        {
            var Options = new MicroserviceOptions();

            if (msOptions != null)
            {
                msOptions.Invoke(Options);
            }

            if (string.IsNullOrWhiteSpace(Options.MicroServiceName))
            {
                throw new Exception("Not config MicroServiceName");
            }

            var builder = new MicroserviceBuilder(services);

            builder.Services.AddSingleton(Options);

            #region Cors
            if (Options.EnableCors)
            {
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("cors-allowanonymous", x =>
                    {
                        x.AllowAnyHeader();
                        x.AllowAnyMethod();
                        x.AllowAnyOrigin();
                        x.AllowCredentials();
                    });
                });
            }
            #endregion

            #region WebEncoders
            if (Options.EnableWebEncoders)
            {
                services.AddWebEncoders(opt =>
                {
                    opt.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
                });
            }
            #endregion

            #region AuthorizationPolicy
            if (Options.EnableAuthorizationPolicy)
            {
                builder.Services.AddAuthorization(options =>
                {
                    var EntryTypes = Assembly.GetEntryAssembly().GetTypes()
                    .Where(x => x.BaseType != null && 
                    x.BaseType.Name.Equals("ApiControllerBase")||
                    x.BaseType.Name.Equals("ControllerBase")).ToList();

                    var entry_policies = PolicyConfigs(EntryTypes);

                    foreach (var policyConfig in entry_policies)
                    {
                        #region Client的权限策略
                        policyConfig.Scopes.ForEach(x =>
                        {
                            var policyName = $"{PolicyKey.ClientScope}:{x}";

                            var policyValues = new List<string>()
                            {
                                $"{Options.MicroServiceName}.{x}",
                                $"{Options.MicroServiceName}.{policyConfig.ControllerName}.all",
                                $"{Options.MicroServiceName}.all"
                            };

                            options.AddPolicy(policyName,
                                policy => policy.RequireClaim(PolicyKey.ClientScope, policyValues));
                        });
                        #endregion

                        #region User的权限策略
                        policyConfig.Permissions.ForEach(x =>
                        {
                            var policyName = $"{PolicyKey.UserPermission}:{x}";

                            var policyValues = new List<string>()
                            {
                                $"{Options.MicroServiceName}.{x}",
                                $"{Options.MicroServiceName}.{policyConfig.ControllerName}.all",
                                $"{Options.MicroServiceName}.all"
                            };

                            options.AddPolicy(policyName,
                                policy => policy.RequireAssertion(handler =>
                                {
                                    var claim = handler.User.Claims
                                    .FirstOrDefault(c => c.Type.Equals(PolicyKey.UserPermission));

                                    if (claim != null && !string.IsNullOrWhiteSpace(claim.Value))
                                    {
                                        var claimValues = claim.Value.ToLower().Split(new string[] { "," },
                                            StringSplitOptions.RemoveEmptyEntries);

                                        if (claimValues != null && claimValues.Length > 0)
                                        {
                                            foreach (var item in claimValues)
                                            {
                                                if (policyValues.Contains(item))
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                    return false;
                                }));
                        });
                        #endregion
                    }
                });
            }
            #endregion

            #region SwaggerGen
            if (Options.EnableSwaggerGen)
            {
                services.AddSwaggerGen(c =>
                {
                    c.EnableAnnotations();

                    //c.TagActionsBy(x => x.RelativePath.Split('/')[0]);

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

                    if (Options.IdentityServerUri != null)
                    {
                        c.AddSecurityDefinition("OAuth2",
                            new OAuth2Scheme()
                            {
                                Type = "oauth2",
                                Flow = "accessCode",
                                AuthorizationUrl = Options.IdentityServerUri.OriginalString + "/connect/authorize",
                                TokenUrl = Options.IdentityServerUri.OriginalString + "/connect/token",
                                Description = "勾选授权范围，获取Token",
                                Scopes = new Dictionary<string, string>(){
                            { "openid","用户标识" },
                            { "profile","用户资料" },
                            { Options.MicroServiceName+ ".all","所有接口权限"},
                                }
                            });
                    }

                    var provider = services.BuildServiceProvider()
                                   .GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerDoc(description.GroupName, new Info
                        {
                            Title = AppConstant.AssemblyName,
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

                    var SiteSwaggerFilePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                        AppConstant.AssemblyName + ".xml");

                    if (File.Exists(SiteSwaggerFilePath))
                    {
                        c.IncludeXmlComments(SiteSwaggerFilePath);
                    }
                });
            }
            #endregion

            #region Localization
            if (Options.EnableLocalization)
            {
                builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

                services.Configure<RequestLocalizationOptions>(options =>
                {
                    var supportedCultures = new[]
                    {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN"),
                };
                    options.DefaultRequestCulture = new RequestCulture("zh-CN", "zh-CN");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

                builder.Services.AddMvc()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization()
                    .AddJsonOptions(o =>
                    {
                        o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });
            }
            #endregion

            #region ApiVersioning
            if (Options.EnableApiVersioning)
            {
                builder.Services.AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

                builder.Services.AddApiVersioning(o =>
                {
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.ReportApiVersions = true;
                });
            }
            #endregion

            #region Authentication
            builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(isAuth =>
            {
                isAuth.Authority = Options.IdentityServerUri.OriginalString;
                isAuth.ApiName = Options.MicroServiceName;
                isAuth.RequireHttpsMetadata = true;
            });
            #endregion

            #region ResponseCaching
            if (Options.EnableResponseCaching)
            {
                builder.Services.AddResponseCaching();
            }
            #endregion

            return builder;
        }

        private static List<PolicyConfig> PolicyConfigs(List<Type> types)
        {
            var policies = new List<PolicyConfig>();

            foreach (var type in types)
            {
                var policyObject = policies.FirstOrDefault(x => x.ControllerName.Equals(type.Name.ToLower()));

                if (policyObject == null)
                {
                    policyObject = new PolicyConfig()
                    {
                        ControllerName = type.Name.ToLower().Replace("controller", "")
                    };
                }

                var ControllerAttributes = type.GetMethods().Select(x => x.GetCustomAttributes<AuthorizeAttribute>()).ToList();

                foreach (var attr in ControllerAttributes)
                {
                    var ProlicyAttributes = attr.Where(x => !string.IsNullOrWhiteSpace(x.Policy)).ToList();

                    if (ProlicyAttributes.Count < 1) { continue; }

                    #region Scope
                    var ProlicyAttributes_Scope = ProlicyAttributes
                                    .Where(x => x.Policy.IndexOf($"{PolicyKey.ClientScope}:") > -1).ToList();

                    if (ProlicyAttributes_Scope.Count > 0)
                    {
                        ProlicyAttributes_Scope.ForEach(x =>
                        {
                            policyObject.Scopes.Add(x.Policy.Replace($"{PolicyKey.ClientScope}:", ""));
                        });
                    }
                    #endregion

                    #region Permission
                    var ProlicyAttributes_Permission = ProlicyAttributes
                                    .Where(x => x.Policy.IndexOf($"{PolicyKey.UserPermission}:") > -1).ToList();

                    if (ProlicyAttributes_Permission.Count > 0)
                    {
                        ProlicyAttributes_Permission.ForEach(x =>
                        {
                            policyObject.Permissions.Add(x.Policy.Replace($"{PolicyKey.UserPermission}:", ""));
                        });
                    } 
                    #endregion
                }

                policies.Add(policyObject);
            }

            return policies;
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
