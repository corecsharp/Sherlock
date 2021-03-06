# Sherlock
### Sherlock来自于Schubert（肖平：https://github.com/endink/ ），简化Schubert功能，同时自身更新，为.NET Core 开源贡献绵薄之力

## C# 统一开发框架

基于.NetCore BLC、Asp.Net Core, 包含：

配置、日志、领域事件、缓存接口、Social OAuth 2.0（QQ、微博）、Ioc、Respository for Dapper、FluentValidator for Asp.Net Core、Asp.Net Identity with Dapper


>nuget地址： https://api.nuget.org/v3/index.json

>应用:https://github.com/corecsharp/push

---

##

**编译框架**   

请下载最新的.net core 2.0sdk ，安装之后查看版本号，修改https://github.com/corecsharp/Sherlock/blob/master/global.json 中sdk的version修改为版本号

---   

##

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
	"Data": {
      "DefaultConnectionName": "default",
      "ConnectionStrings": {
        "default": "server=;Database=;UID=;PWD=;Charset=utf8;SslMode=None;",
        "dataBase2": "server=;Database=;UID=;PWD=;Charset=utf8;SslMode=None;"
      },
	  //表结构映射介绍，参照 Sherlock.Framework.Data.DapperOptions
      "Dapper": {
        "IdentifierMappingStrategy": "PascalCase",
        "CapitalizationRule": "Original"
      }
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

## Windows IIS 部署

组件安装：

1、安装 DotNetCore.2.0.5-WindowsHosting.exe

2、执行命令： 

net stop was /y

net start w3svc

3、查看IIS中模块是否存在AspNetCoreModule，如果存在即安装成功

4、搭建IIS站点，应用程序池选择 无托管代码，集成

5、也可以使用dotnet .\应用程序名.dll 进行测试

>WindowsHosting下载地址： https://download.microsoft.com/download/1/1/0/11046135-4207-40D3-A795-13ECEA741B32/DotNetCore.2.0.5-WindowsHosting.exe
---


#  Centos7及以上 部署

环境准备：

1、sudo rpm --import https://packages.microsoft.com/keys/microsoft.asc

2、sudo sh -c 'echo -e "[packages-microsoft-com-prod]\nname=packages-microsoft-com-prod \nbaseurl= https://packages.microsoft.com/yumrepos/microsoft-rhel7.3-prod\nenabled=1\ngpgcheck=1\ngpgkey=https://packages.microsoft.com/keys/microsoft.asc" > /etc/yum.repos.d/dotnetdev.repo'

3、sudo yum update

4、sudo yum install libunwind libicu

5、sudo yum install dotnet-sdk-2.1.101

创建监听进程

1、export ASPNETCORE_ENVIRONMENT=testing

2、export ASPNETCORE_URLS=http://*:5001

3、dotnet ./应用名.dll（依赖客户端） nohup dotnet ./应用名.dll（后台运行）


>.NetCore 配置参考地址： https://www.microsoft.com/net/download/linux-package-manager/centos/sdk-2.1.101
---

## 联系方式

QQ: 280894830

微信：feng_wei_2013



