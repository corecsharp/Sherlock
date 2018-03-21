﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.DependencyInjection;
using Sherlock.Framework.Domain;
using Sherlock.Framework.Services;
using System;

namespace Sherlock.Framework.Web
{
    public class DapperIdentityStarter<TUser, TRole, TIdentityService> : WebStarter
        where TUser : class, IUser
        where TRole : class, IRole
        where TIdentityService : IIdentityService
    {
        private Action<IdentityOptions> _configure;

        public DapperIdentityStarter(Action<IdentityOptions> configure)
        {
            _configure = configure;
        }

        public override void ConfigureServices(SherlockServicesBuilder servicesBuilder, SherlockWebOptions options)
        {
            var identitySvcdescriptor = ServiceDescriber.Scoped<IIdentityService, TIdentityService>();
            servicesBuilder.ServiceCollection.AddSmart(identitySvcdescriptor);

            servicesBuilder.ServiceCollection
                        .AddIdentity<TUser, TRole>(iop => _configure?.Invoke(iop))
                        .AddDefaultTokenProviders()
                        .AddDapperStores<TUser, TRole>();
        }

        public override void Start(IApplicationBuilder appBuilder, SherlockWebOptions options)
        {
        }
    }
}
