using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;

namespace Sherlock.Framework.Web.Authentication.Weibo
{
    public class WeiboOAuthMiddleware : OAuthMiddleware<WeiboOAuthOptions>
    {
        public WeiboOAuthMiddleware(RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            IOptions<SharedAuthenticationOptions> externalOptions,
            IOptions<WeiboOAuthOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, urlEncoder, externalOptions, options)
        {
            Guard.ArgumentNotNull(options, nameof(options));

            if (String.IsNullOrWhiteSpace(options.Value?.ClientSecret))
            {
                throw new InvalidOperationException("Webo client secret must be provided");
            }

            if (String.IsNullOrWhiteSpace(options.Value?.ClientId))
            {
                throw new InvalidOperationException("Webo client id must be provided");
            }
        }



        protected override AuthenticationHandler<WeiboOAuthOptions> CreateHandler()
        {
            return new WeiboOAuthHandler(base.Backchannel);
        }
    }
}