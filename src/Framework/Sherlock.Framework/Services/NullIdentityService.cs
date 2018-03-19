using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sherlock.Framework.Domain;

namespace Sherlock.Framework.Services
{
    public class NullIdentityService : IIdentityService
    {
        public IUser CreateAnonymous()
        {
            return null;
        }

        public IUser GetAuthenticatedUser()
        {
            return null;
        }

        public Task<IUser> GetByIdAsync(long userId)
        {
            return Task.FromResult<IUser>(null);
        }

        public Task RefreshIdentityAsync(long userId)
        {
            return Task.FromResult(0);
        }
    }
}
