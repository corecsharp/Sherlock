using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Sherlock.Framework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.Domain;
using Sherlock.Framework;
using Sherlock.Framework.Services;

namespace Sherlock.Framework.Web
{
    public class DapperIdentityStarter<TUser, TRole, TIdentityService> : WebStarter
        where TUser : class, IUser
        where TRole : class, IRole
        where TIdentityService : IIdentityService
    {

        private static void SetupIdentityOptions(IdentityOptions op, SherlockWebOptions options)
        {
            op.Password.RequireLowercase = false;
            op.Password.RequireDigit = false;
            op.Password.RequireNonAlphanumeric = false;
            op.Password.RequireUppercase = false;
            op.Password.RequireDigit = false;

            op.SignIn.RequireConfirmedEmail = false;
            op.SignIn.RequireConfirmedPhoneNumber = false;

            op.Cookies.ApplicationCookie.LoginPath = options.LoginPath;
            op.Cookies.ApplicationCookie.LogoutPath = options.LogoutPath;

            op.Cookies.ApplicationCookie.CookieSecure = options.CookieSecure;
            op.Cookies.ExternalCookie.CookieSecure = options.CookieSecure;
            op.Cookies.TwoFactorRememberMeCookie.CookieSecure = options.CookieSecure;
            op.Cookies.TwoFactorUserIdCookie.CookieSecure = options.CookieSecure;
        }

        public override void ConfigureServices(SherlockServicesBuilder servicesBuilder, SherlockWebOptions options)
        {
            var identitySvcdescriptor = ServiceDescriber.Scoped<IIdentityService, TIdentityService>();
            servicesBuilder.ServiceCollection.AddSmart(identitySvcdescriptor);

            servicesBuilder.ServiceCollection
                        .AddIdentity<TUser, TRole>(iop => SetupIdentityOptions(iop, options))
                        .AddDefaultTokenProviders()
                        .AddDapperStores<TUser, TRole>();
        }

        public override void Start(IApplicationBuilder appBuilder, SherlockWebOptions options)
        {
            appBuilder.UseIdentity();
        }
    }
}
