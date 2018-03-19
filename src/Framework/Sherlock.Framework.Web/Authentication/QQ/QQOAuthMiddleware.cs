using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;

namespace Sherlock.Framework.Web.Authentication.QQ
{
    public class QQOAuthMiddleware : OAuthMiddleware<QQOAuthOptions>
    {

        public QQOAuthMiddleware(RequestDelegate next, 
            IDataProtectionProvider dataProtectionProvider, 
            ILoggerFactory loggerFactory, 
            UrlEncoder urlEncoder,
            IOptions<SharedAuthenticationOptions> externalOptions,
            IOptions<QQOAuthOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, urlEncoder, externalOptions, options)
        {
            Guard.ArgumentNotNull(options, nameof(options));
            if (String.IsNullOrWhiteSpace(options.Value?.ClientSecret))
            {
                throw new InvalidOperationException("QQ oauth client secret must be provided");
            }

            if (String.IsNullOrWhiteSpace(options.Value?.ClientId))
            {
                throw new InvalidOperationException("QQ oauth id must be provided");
            }
        }

        protected override AuthenticationHandler<QQOAuthOptions> CreateHandler()
        {
            return new QQOAuthHandler(base.Backchannel);
        }
    }
}
