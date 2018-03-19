using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Web.Authentication
{
    public sealed class AuthenticationBuilder
    {
        private IApplicationBuilder _appBuilder = null;

        internal AuthenticationBuilder(IApplicationBuilder builder)
        {
            Guard.ArgumentNotNull(builder, nameof(builder));
            _appBuilder = builder;
        }

        /// <summary>
        /// 使用 OAuth 中间件。
        /// </summary>
        /// <typeparam name="TMiddleware">中间件类型。</typeparam>
        /// <typeparam name="TOAuthOptions">OAuth配置参数类型</typeparam>
        /// <param name="options">OAuth 配置。</param>
        /// <returns></returns>
        public AuthenticationBuilder UseOAuthMiddleware<TMiddleware, TOAuthOptions>(TOAuthOptions options)
            where TOAuthOptions : OAuthOptions, new()
            where TMiddleware : OAuthMiddleware<TOAuthOptions>
        {
            _appBuilder.UseMiddleware<TMiddleware>(Options.Create<TOAuthOptions>(options));
            return this;
        }
    }
}
