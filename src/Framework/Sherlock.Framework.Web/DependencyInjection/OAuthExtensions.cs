using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.Web.Authentication.QQ;
using Sherlock.Framework.Web.Authentication.Wechat;
using Sherlock.Framework.Web.Authentication.Weibo;
using Sherlock.Framework.Web.DependencyInjection;
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
        public static SherlockWebBuilder AddQQSignIn(this SherlockWebBuilder app, Action<QQOAuthOptions> configureOptions = null)
        {
            QQOAuthOptions options = new QQOAuthOptions();
            configureOptions?.Invoke(options);
            return app.ConfigureServices(s => 
            s.AddAuthentication().AddOAuth<QQOAuthOptions, QQOAuthHandler>(QQDefaults.AuthenticationScheme, QQDefaults.DisplayName, configureOptions));
        }

        /// <summary>
        /// 使用微博登陆。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static SherlockWebBuilder AddWeiboSignIn(this SherlockWebBuilder app, Action<WeiboOAuthOptions> configureOptions = null)
        {
            WeiboOAuthOptions options = new WeiboOAuthOptions();
            configureOptions?.Invoke(options);
            return  app.ConfigureServices(s => s.AddAuthentication().AddOAuth<WeiboOAuthOptions, WeiboOAuthHandler>(WeiboDefaults.AuthenticationScheme, WeiboDefaults.DisplayName, configureOptions));
        }

        /// <summary>
        /// 使用微信登陆。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static SherlockWebBuilder AddWeChatSignIn(this SherlockWebBuilder app, Action<WeChatOptions> configureOptions = null)
        {
            WeChatOptions options = new WeChatOptions();
            configureOptions?.Invoke(options);
            return app.ConfigureServices(s => s.AddAuthentication().AddOAuth<WeChatOptions, WeChatOAuthHandler>(WeiboDefaults.AuthenticationScheme, WechatDefaults.DisplayName, configureOptions));
        }
    }
}