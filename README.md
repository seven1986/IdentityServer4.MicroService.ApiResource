# 说明
用于开发单个微服务项目的示例，供参考用。


#### 集成步骤
* 1，Clone当前项目，编写业务代码
* 2，发布到自有服务器
* 3，注册到IdentityServer4微服务中
```
第3步需要确认已经部署好了
1,IdentityServer4.MicroService （授权中心）
2,IdentityServer4.MicroService.AdminUI （微服务统一管理UI）
```


#### 特性

##### 1 自动生成swagger，多版本
![swagger](swagger.png)

##### 2 多语言
```html
参照Resources/Controllers结构添加即可,名称必须与controller对应
```

##### 3 自定义微服务的权限/角色
```csharp

        /*
        * 扩展这2个类即可
        */

        /// <summary>
        /// Client权限定义
        /// 对应Token中的claim的scope字段
        /// 字段名：用去controller 的 action 标记
        /// 字段值：策略的名称
        /// 字段自定义属性：策略的权限集合，
        /// 聚合PolicyClaimValues所有的值（除了"all"），去重后登记到IdentityServer的ApiResource中去
        /// 例如PolicyClaimValues("apiresource.create", "apiresource.all", "all"),代表
        /// 当前apiresource项目的create权限，或者 apiresource.all权限，或者all权限
        /// </summary>
        public class ClientScopes
        {
            [PolicyClaimValues(MicroServiceName + ".create", MicroServiceName + ".all", "all")]
            public const string Create = "scope:create";

            [PolicyClaimValues(MicroServiceName + ".read", MicroServiceName + ".all", "all")]
            public const string Read = "scope:read";

            [PolicyClaimValues(MicroServiceName + ".update", MicroServiceName + ".all", "all")]
            public const string Update = "scope:update";

            [PolicyClaimValues(MicroServiceName + ".delete", MicroServiceName + ".all", "all")]
            public const string Delete = "scope:delete";

            [PolicyClaimValues(MicroServiceName + ".approve", MicroServiceName + ".all", "all")]
            public const string Approve = "scope:approve";

            [PolicyClaimValues(MicroServiceName + ".reject", MicroServiceName + ".all", "all")]
            public const string Reject = "scope:reject";

            [PolicyClaimValues(MicroServiceName + ".upload", MicroServiceName + ".all", "all")]
            public const string Upload = "scope:upload";
        }

        /// <summary>
        /// User权限定义
        /// 对应Token中的claim的permission字段
        /// 字段名：用去controller 的 action 标记
        /// 字段值：策略的名称
        /// 字段自定义属性：策略的权限集合，可按需设置User表的claims的permission属性
        /// </summary>
        public class UserPermissions
        {
            [PolicyClaimValues(MicroServiceName + ".create", MicroServiceName + ".all", "all")]
            public const string Create = "permission:create";

            [PolicyClaimValues(MicroServiceName + ".read", MicroServiceName + ".all", "all")]
            public const string Read = "permission:read";

            [PolicyClaimValues(MicroServiceName + ".update", MicroServiceName + ".all", "all")]
            public const string Update = "permission:update";

            [PolicyClaimValues(MicroServiceName + ".delete", MicroServiceName + ".all", "all")]
            public const string Delete = "permission:delete";

            [PolicyClaimValues(MicroServiceName + ".approve", MicroServiceName + ".all", "all")]
            public const string Approve = "permission:approve";

            [PolicyClaimValues(MicroServiceName + ".reject", MicroServiceName + ".all", "all")]
            public const string Reject = "permission:reject";

            [PolicyClaimValues(MicroServiceName + ".upload", MicroServiceName + ".all", "all")]
            public const string Upload = "permission:upload";
        }
```

##### 4 其他
```html
<--在appsettings.json文件中-->
  "IdentityServer": ""，
  "ApiTrackerSetting": {
    "ElasticConnection": "",
    "DocumentName": "identityserver4-microservice-apiresource",
    "TimeOut": 500
  }

IdentityServer：identityserver4（授权中心）的服务器地址，如：https://ids.ixingban.com
ApiTrackerSetting：elasticsearch的配置节，可选
```