# 说明
微服务项目示例。

## 使用方法

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
        {
services.AddMicroService(new MicroserviceOptions()
            {
                MicroServiceName = "apiresource",

                AssemblyName = Assembly.GetExecutingAssembly().GetName().Name,

                Scopes = typeof(ClientScopes),

                Permissions = typeof(UserPermissions)
            });
                //...
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

            app.UseStaticFiles();

            app.UseMicroservice();

            app.UseMvc();
        }
```


#### 主要特性（自定义权限、多语言、多版本、入参/出参统一格式）

![swagger](swagger.png)

#### 多语言配置

    参照Resources/Controllers结构添加即可,名称必须与controller对应

#### 微服务配置（在MicroserviceConfig.cs文件）

  * Client权限定义：ClientScopes Class

  * User权限定义：UserPermissions Class

#### 其他配置（appsettings.json）
    * IdentityServer：
        identityserver4授权中心的服务器地址，如：ids.ixingban.com

#### 数据库配置

```text
The generated database code requires Entity Framework Core Migrations. Run the following commands:
1. dotnet ef migrations add CreateDbSchema
2. dotnet ef database update
 Or from the Visual Studio Package Manager Console:
1. Add-Migration CreateIdentitySchema
2. Update-Database
```

#### 设计参考文档
  
  - https://docs.microsoft.com/zh-cn/azure/architecture/best-practices/api-design
  
  - https://github.com/Microsoft/api-guidelines/blob/master/Guidelines.md
  
  - http://restcookbook.com/
  
  - https://mathieu.fenniak.net/the-api-checklist/
  
  - https://docs.microsoft.com/zh-cn/azure/architecture/best-practices/api-implementation#more-information