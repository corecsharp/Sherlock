using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Localization
{
    public class SherlockStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IResourceNamesCache _resourceNamesCache = new ResourceNamesCache();
        private ILocalizedStringManager _localizedStringManager = null;

        public SherlockStringLocalizerFactory(ILocalizedStringManager localizedStringManager)
        {
            Guard.ArgumentNotNull(localizedStringManager, nameof(localizedStringManager));

            _localizedStringManager = localizedStringManager;
        }
        public IStringLocalizer Create(Type resourceSource)
        {
            return new SherlockStringLocalizer(_resourceNamesCache, _localizedStringManager);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new SherlockStringLocalizer(_resourceNamesCache, _localizedStringManager);
        }
    }
}
