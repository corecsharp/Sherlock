using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sherlock.Framework.FileSystem.AppData;
using Sherlock.Helpers;
using System;

namespace Sherlock.Framework.Environment
{
    public class DefaultInstanceIdProvider : IInstanceIdProvider
    {
        private String _instanceId;
        private IServiceProvider _serviceProvider;
        private string _appName = null;
        public DefaultInstanceIdProvider(IServiceProvider serviceProvider, IOptions<SherlockOptions> SherlockOptions)
        {
            Guard.ArgumentNotNull(serviceProvider, nameof(serviceProvider));
            Guard.ArgumentNotNull(SherlockOptions, nameof(SherlockOptions));

            _serviceProvider = serviceProvider;
            _appName = SherlockOptions.Value.AppSystemName.IfNullOrEmpty("NULL_APPNAME");
        }
        
        public String GetInstanceId()
        {
            if (_instanceId.IsNullOrEmpty())
            {
                lock(this)
                {
                    if (_instanceId.IsNullOrEmpty())
                    {
                        IAppDataFolder folder = _serviceProvider.GetRequiredService<IAppDataFolder>();
                        string id = ToolHelper.NewShortId();
                        string fileName = $"{_appName}.instance";
                        if (!folder.FileExists(fileName))
                        {
                            folder.CreateFile(fileName, id);
                        }
                        else
                        {
                            id = folder.ReadFile(fileName);
                        }
                        _instanceId = id;
                    }
                }
            }
            return _instanceId;
        }
    }
}
