# Sherlock
### Sherlock来自于Schubert（肖平：https://github.com/endink/ ），简化Schubert功能，同时自身更新，为.NET Core 开源贡献绵薄之力

## C# 统一开发框架

基于.NetCore BLC、Asp.Net Core, 包含：

配置、日志、领域事件、缓存接口、Social OAuth 2.0（QQ、微博）、Ioc、Respository for Dapper、FluentValidator for Asp.Net Core、Asp.Net Identity with Dapper


>nuget地址： https://api.nuget.org/v3/index.json

>应用:https://github.com/corecsharp/push

---

**关于数据库**   

推荐使用 Dapper 作为数据访问层，框架支持 Dapper，以下是数据库支持情况：   

---   


|               | Mysql  |  Sql Server | Oracle 
---------- |----------|--------------|------
Dapper  |      Y      |    N               |    N


---

# 开发文档

>https://github.com/corecsharp/Sherlock/blob/master/README.md

**配置文件架构（schema）**

Sherlock 使用 asp.net core 的标准配置模型，你可以通过任意格式（如XML、JSON等）文件，甚至自定义方式对 Sherlock 框架的内置功能进行配置。    

我们推荐使用 json 格式的配置文件，Sherlock 为 json 配置文件提供了架构（schema）支持，通过使用 json schema 可以在 Visual Studio 中获取配置文件的智能提示。   

>以下是 json schema 文件的在线地址：

>`v2.0.*`

>catalog schema : https://github.com/corecsharp/Sherlock/blob/master/schemas/catalog.json

>appsettings schema : https://github.com/corecsharp/Sherlock/blob/master/schemas/appsettings.json   

>module schema : https://github.com/corecsharp/Sherlock/blob/master/schemas/module.json


**Web项目打包发布使用说明** 
> VS2017:
[点击这里](https://github.com/corecsharp/Sherlock/blob/master/src/Tools/Sherlock.Framework.Modularity.Tools.Vs2017/ReadMe.md)

# QuickStart

**在Asp.Net Core 中使用：**

```java
public void ConfigureServices(IServiceCollection services)
{
            services.AddSherlockFramework(this.Configuration,
                setup =>
                {
                    setup.AddSnowflakeIdGenerationService();
                    setup.AddJobScheduling();
                    setup.AddDapperDataFeature(options =>
                    {
                        options.DefaultConnectionName = "default";
                        options.ConnectionStrings = new Dictionary<string, string>
                        {
                            { "default", "server=10.66.2.33;Port=3306;user id=root;password=setpay@123;database=test_identity;" }
                        };
                    });
                    setup.AddWebFeature(web =>
                    {
                        web.ConfigureFeature(f => f.MvcFeatures = Sherlock.Framework.Web.MvcFeatures.Full);
                        web.AddFluentValidationForMvc();                        
                        web.AddIdentityWithDapperStores<UserBase,RoleBase, DapperIdentityService>();
                    });
                },
                scope =>
                {
                    scope.LoggerFactory.AddDebug();
                });

        }
}
```
**完成启动：**

```javascript
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
   
    app.StartSherlockWebApplication();
}
```



使用范例（appsettings）：
```javascript

{
  "$schema": "https://github.com/corecsharp/Sherlock/blob/master/schemas/appsettings.json",
  "Sherlock": {
    "Group": "InfraTeam",
    "AppSystemName": "SherlockSample",
    "AppName": "Sherlock示例",
    "DefaultCulture": "zh-Hans",
    "DefaultTimeZone": "China Standard Time",
    "Version": "2.0.0",
    "Configuration": {
      "ConnectionString": "10.66.30.95:2181,10.66.30.95:2182,10.66.30.95:2183",
      "ConnectionTimeoutSeconds": 10,
      "OperatingTimeoutSeconds": 60,
      "SessionTimeoutSeconds": 20
    },
    "Network": {
      "DataCenterId": 1,
      "Lans": [
        {
          "DataCenterId": 1,
          "LAN1IPMask": "10.66.10.*",
          "LAN2IPMask": "10.66.11.*",
          "LAN3IPMask": "192.168.*.*",
          "LAN4IPMask": "10.66.52.*"
        },
        {
          "DataCenterId": 2,
          "LAN1IPMask": "10.66.10.*",
          "LAN2IPMask": "10.66.11.*",
          "LAN3IPMask": "192.168.*.*",
          "LAN4IPMask": "10.66.52.*"
        }
      ]
    },
    "Eyes": {
      "Logging": {
        "QueryServiceAddress": "10.66.4.75:19000"
      },
      "Kafka": {
        "KafkaServers": "10.66.4.74:9092,10.66.4.75:9092,10.66.4.76:9092"
      }
    },
    "Scheduling": {
      "Jobs": {
        "Sample.TestJob": "0 */1 * * * ?"
      }
    }
  }
}



```

