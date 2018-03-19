using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Caching
{
    public class DistributedCacheAdapter : IDistributedCache
    {
        private ICacheManager _cacheManager;
        public string _cacheRegion = null;
        public DistributedCacheAdapter(ICacheManager cacheManager, string cacheRegion)
        {
            Guard.ArgumentNotNull(cacheManager, nameof(cacheManager));
            _cacheManager = cacheManager;
            _cacheRegion = cacheRegion.IfNullOrWhiteSpace("aspnet");
        }

        public void Connect()
        {
            
        }

        public Task ConnectAsync()
        {
            return Task.FromResult(0);
        }

        public byte[] Get(string key)
        {
            return _cacheManager.Get(key, _cacheRegion) as byte[];
        }

        public Task<byte[]> GetAsync(string key)
        {
            return Task.FromResult(this.Get(key));
        }

        public void Refresh(string key)
        {
            _cacheManager.Refresh(key, _cacheRegion);
        }

        public Task RefreshAsync(string key)
        {
            return Task.Run(()=>this.Refresh(key));
        }

        public void Remove(string key)
        {
            this._cacheManager.Remove(key, _cacheRegion);
        }

        public Task RemoveAsync(string key)
        {
            return Task.Run(() => this.RemoveAsync(key));
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            if (options.SlidingExpiration.HasValue)
            {
                _cacheManager.Set(key, value, options.SlidingExpiration, _cacheRegion, true);
            }
            else
            {
                _cacheManager.Set(key, value, options.AbsoluteExpirationRelativeToNow, _cacheRegion, false);
            }
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            return Task.Run(() => this.Set(key, value, options));
        }
    }
}
