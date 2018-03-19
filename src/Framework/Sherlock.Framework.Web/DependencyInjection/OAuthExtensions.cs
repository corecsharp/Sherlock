using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sherlock.Framework.Web.Authentication;
using Sherlock.Framework.Web.Authentication.QQ;
using Sherlock.Framework.Web.Authentication.Wechat;
using Sherlock.Framework.Web.Authentication.Weibo;
using System;
using System.ComponentModel;

namespace Sherlock
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OAuthExtensions
    {
        /// <summary>
        /// 使用QQ登陆。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder UseQQAuthentication(this AuthenticationBuilder app, Action<QQOAuthOptions> configureOptions = null)
        {
            QQOAuthOptions options = new QQOAuthOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }
            return app.UseOAuthMiddleware<QQOAuthMiddleware, QQOAuthOptions>(options);
        }

        /// <summary>
        /// 使用微博登陆。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder UseWeiboAuthentication(this AuthenticationBuilder app, Action<WeiboOAuthOptions> configureOptions = null)
        {
            WeiboOAuthOptions options = new WeiboOAuthOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }
            return app.UseOAuthMiddleware<WeiboOAuthMiddleware, WeiboOAuthOptions>(options);
        }

        /// <summary>
        /// 使用微信登陆。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder UseWeChatAuthentication(this AuthenticationBuilder app, Action<WeChatOptions> configureOptions = null)
        {
            WeChatOptions options = new WeChatOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }
            return app.UseOAuthMiddleware<WeChatOAuthMiddleware, WeChatOptions>(options);
        }
    }
}