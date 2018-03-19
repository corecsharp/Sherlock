using Sherlock.Framework.Domain;
using Sherlock.Framework.Environment;
using Sherlock.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Environment
{
    public class UserStateProvider : IWorkContextStateProvider
    {
        private IIdentityService _identityService;

        public UserStateProvider(IIdentityService identityService)
        {
            Guard.ArgumentNotNull(identityService, nameof(identityService));
            
            _identityService = identityService;
        }

        public Func<WorkContext, Object> Get(string name)
        {
            if (name == WorkContext.CurrentUserStateName)
            {
                return ctx =>
                {
                    IUser u = _identityService.GetAuthenticatedUser();
                    return u ?? _identityService.CreateAnonymous();
                };
            }
            return null;
        }
    }
}
