using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Text.Encodings.Web;

namespace Sherlock.Framework.Web.Authentication.Wechat
{
    public class WeChatOAuthMiddleware : OAuthMiddleware<WeChatOptions>
    {
        public WeChatOAuthMiddleware(RequestDelegate next, 
            IDataProtectionProvider dataProtectionProvider, 
            ILoggerFactory loggerFactory, 
            UrlEncoder urlEncoder,
            IOptions<SharedAuthenticationOptions> externalOptions,
            IOptions<WeChatOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, urlEncoder, externalOptions, options)
        {
            Guard.ArgumentNotNull(options, nameof(options));

            if (String.IsNullOrWhiteSpace(options.Value?.ClientSecret))
            {
                throw new InvalidOperationException("WeChat client secret must be provided");
            }

            if (String.IsNullOrWhiteSpace(options.Value?.ClientId))
            {
                throw new InvalidOperationException("WeChat client id must be provided");
            }
        }

        protected override AuthenticationHandler<WeChatOptions> CreateHandler()
        {
            return new WeChatOAuthHandler(base.Backchannel);
        }
    }
}
