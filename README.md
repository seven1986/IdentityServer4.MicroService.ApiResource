# 说明
微服务项目示例。

## 使用方法

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
        {
services.AddMicroService(new MicroserviceOptions()
            {
                // 微服务名称
                MicroServiceName = "apiresource",

                // 固定写法
                // 然后设置项目——生成——勾选生成项目xml文件
                AssemblyName = Assembly.GetExecutingAssembly().GetName().Name,

                // 启用API权限验证
                AuthorizationPolicy = true,
                Scopes = typeof(ClientScopes),
                Permissions = typeof(UserPermissions)，
                
                // 缓存SQL数据库链接地址，为空将不会启用
                // 初始化：dotnet sql-cache create {sqlConnection} dbo AppCache
                SQLCacheConnection = Configuration.GetConnectionString("DefaultConnection")
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

#### API权限配置 1，
  （参考Host项目的MicroserviceConfig.cs文件）
  * Client权限定义：ClientScopes Class
  * User权限定义：UserPermissions Class

#### API权限配置 2，
微服务模式，将API连接到IdentityServer4，配置appsettings.json文件
```json
{
  IdentityServer:"identityserver4地址"
}
```

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