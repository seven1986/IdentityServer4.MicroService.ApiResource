# 说明
微服务项目示例。

https://webapplication420180903114128.chinacloudsites.cn/swagger/index.html


#### 主要特性（自定义权限、多语言、多版本、入参/出参统一格式）

![swagger](swagger.png)

#### 多语言配置

    参照Resources/Controllers结构添加即可,名称必须与controller对应

#### 微服务配置（在MicroserviceConfig.cs文件）

  * 微服务名称：MicroServiceName 字段

  * Client权限定义：ClientScopes Class

  * User权限定义：UserPermissions Class

  * User角色定义：Roles Class

#### 其他配置（appsettings.json）
    * IdentityServer：
        identityserver4授权中心的服务器地址，如：ids.ixingban.com

    * ApplicationHost：
        当前项目的host地址



#### 设计参考文档
  
  - https://docs.microsoft.com/zh-cn/azure/architecture/best-practices/api-design
  
  - https://github.com/Microsoft/api-guidelines/blob/master/Guidelines.md
  
  - http://restcookbook.com/
  
  - https://mathieu.fenniak.net/the-api-checklist/
  
  - https://docs.microsoft.com/zh-cn/azure/architecture/best-practices/api-implementation#more-information