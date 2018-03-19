﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sherlock.Framework.Caching;
using Sherlock.Framework.Domain;
using Sherlock.Framework.Services;
using Sherlock.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Sherlock.Framework.Services
{
    public abstract class AbstractionWebIdentityService<TUser> : IIdentityService
        where TUser : class, IUser
    {
        private Lazy<SherlockWebOptions> _options;
        private Lazy<UserManager<TUser>> _userManager;
        private Lazy<ICacheManager> _cacheManager;
        private IHttpContextAccessor _httpContextAccessor;
        private long _userId;

        public AbstractionWebIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            Guard.ArgumentNotNull(httpContextAccessor, nameof(httpContextAccessor));
            IServiceProvider serviceProvider = httpContextAccessor.HttpContext.RequestServices;

            _userId = -1;
            _options = new Lazy<SherlockWebOptions>(()=> serviceProvider.GetRequiredService<IOptions<SherlockWebOptions>>().Value);
            _cacheManager = new Lazy<ICacheManager>(() => serviceProvider.GetRequiredService<ICacheManager>());
            _userManager = new Lazy<UserManager<TUser>>(() => serviceProvider.GetRequiredService<UserManager<TUser>>());

            _httpContextAccessor = httpContextAccessor;
        }

        private long? GetAuthenticatedUserId()
        {
            if (_userId == -1)
            {
                var idString = _userManager.Value.GetUserId(_httpContextAccessor.HttpContext?.User);
                long id = 0;
                if (idString.IsNullOrWhiteSpace() || !long.TryParse(idString, out id))
                {
                    _userId = 0;
                }
                else
                {
                    _userId = id;
                    return _userId;
                }
            }
            if (_userId > 0)
            {
                return _userId;
            }
            return null;
        }

        protected abstract Task<TUser> GetUserByIdAsync(IServiceProvider serviceProvider, long userId);
        protected abstract TUser CreateAnonymous();

        public async Task<IUser> GetByIdAsync(long userId)
        {
            TUser u = _cacheManager.Value.GetUser(userId) as TUser;
            if (u != null)
            {
                return u;
            }
            var serviceProvider = _httpContextAccessor.HttpContext.RequestServices;
            u = await this.GetUserByIdAsync(serviceProvider, userId);
            if (u != null && _options.Value.IdentityCacheTimeoutMinutes > 0)
            {
                _cacheManager.Value.SetUser(u, TimeSpan.FromMinutes(_options.Value.IdentityCacheTimeoutMinutes));
            }
            return u;
        }

        IUser IIdentityService.CreateAnonymous()
        {
            return this.CreateAnonymous();
        }

        public Task RefreshIdentityAsync(long userId)
        {
            _cacheManager.Value.RemoveUser(userId);
            return Task.FromResult(0);
        }

        public IUser GetAuthenticatedUser()
        {
            var userId = this.GetAuthenticatedUserId();
            if (userId.HasValue)
            {
                return this.GetByIdAsync(userId.Value).GetAwaiter().GetResult();
            }
            return null;
        }
    }
}
