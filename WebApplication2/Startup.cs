using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.MicroService.ApiResource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApplication2
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMicroService(Configuration, options => {

                // 非必填，如果不设置就不会启用API鉴权
                options.IdentityServerUri = new Uri("IdentityServer4服务器地址");

                // 建议填写
                options.MicroServiceRedirectUrls = new List<string>()
                {
                    "https://{当前项目网址}/swagger/oauth2-redirect.html"
                };

                // 非必填
                //options.MicroServiceName = Assembly.GetExecutingAssembly().GetName().Name;
                //options.MicroServiceDisplayName = "MicroServiceDisplayName";
                //options.MicroServiceDescription = "MicroServiceDescription";
                //options.MicroServiceClientIDs = new List<string>() { "swagger" };
                //options.EnableApiVersioning = true;
                //options.EnableAuthorizationPolicy = true;
                //options.EnableCors = true;
                //options.EnableLocalization = true;
                //options.EnableResponseCaching = true;
                //options.EnableSwaggerGen = true;
                //options.EnableSwaggerUI = true;
                //options.EnableWebEncoders = true;
                //options.ImportToIdentityServer = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMicroservice();

            app.UseMvc();
        }
    }
}
