using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sherlock.Framework.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Sherlock.Framework.Web
{
    /// <summary>
    /// Sherlock Web 特性选项（默认加载 Sherlock : Web 配置节）。
    /// </summary>
    public class SherlockWebOptions
    {
        private int _sessionTimeoutMinutes;

        public SherlockWebOptions()
        {
            this._sessionTimeoutMinutes = 30;
        }

        /// <summary>
        /// 获取或设置一个值，指示包含的 ASP.Net MVC 特性（默认为 <see cref="MvcFeatures.Full"/>）。
        /// </summary>
        public MvcFeatures MvcFeatures { get; set; } = MvcFeatures.Full;

        /// <summary>
        /// 获取或设置一个值，指示框架是否包含 Session 功能，默认为 true。
        /// </summary>
        public bool UseSession { get; set; } = true;

        /// <summary>
        /// 获取或设置 Session 的超时时间（默认为30分钟, 小于 1 分钟视为 1 分钟）。
        /// </summary>
        public int SessionTimeoutMinutes
        {
            get { return this._sessionTimeoutMinutes; }
            set { this._sessionTimeoutMinutes = Math.Max(1, value); }
        }

        /// <summary>
        /// 获取或设置一值，指示在获取用户时候对获取操作使用缓存的过期时间（分钟），默认为10分钟。
        /// 重写 <see cref="Services.AbstractionWebIdentityService{TUser}.GetByIdAsync(long)"/> 方法时应考虑此参数。
        /// </summary>
        public int IdentityCacheTimeoutMinutes { get; set; } = 10;

        /// <summary>
        /// 登录页面路径（默认为 /Login）。
        /// </summary>
        public PathString LoginPath { get; set; } = new PathString("/Login");

        /// <summary>
        /// 注销 action 路径（默认为 /LogOff）。
        /// </summary>
        public PathString LogoutPath { get; set; } = new PathString("/LogOff");

        /// <summary>
        /// 确定 Cookie 是否只应根据 HTTPS 请求传输。 默认值是当正在执行登录的页面也是 HTTPS 时将 Cookie 限制为 HTTPS 请求。 如果你有 HTTPS 登录页并且你的部分站点是 HTTP，则可能需要更改此值。 
        /// </summary>
        public CookieSecurePolicy CookieSecure { get; set; } = CookieSecurePolicy.SameAsRequest;

        /// <summary>
        /// 获取或设置JSON的格式化的拼写风格（默认为 camel ）。
        /// </summary>
        public CapitalizationStyle JsonCapitalizationStyle { get; set; } = CapitalizationStyle.CamelCase;

        /// <summary>
        /// 获取或设置 JSON 序列化时是否将 long 序列化为 string 类型 （通常用于解决 javascript 不支持 64 位整数问题）。
        /// </summary>
        public bool JsonSerializeLongAsString { get; set; } = false;

    }

    /// <summary>
    /// 表示 AspNetCore 的 Identity 特性可用性。
    /// </summary>
    [Flags]
    public enum IdentityUsage
    {
        /// <summary>
        ///  表示服务（如 UserManager, SiginManager 等）。
        /// </summary>
        Service = 1,
        /// <summary>
        /// 表示存储（使用 Entity Framework 默认存储）。
        /// </summary>
        EntityFrameworkStore = 3,
        /// <summary>
        /// 表示中间键（Cookie 身份验证等）。
        /// </summary>
        Middleware = 5,
    }
}
