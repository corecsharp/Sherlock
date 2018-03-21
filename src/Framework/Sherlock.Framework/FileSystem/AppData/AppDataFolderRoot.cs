using Microsoft.Extensions.Options;
using Sherlock.Framework.Environment;
using System.IO;

namespace Sherlock.Framework.FileSystem.AppData
{
    public class AppDataFolderRoot : IAppDataFolderRoot
    {
        private IPathProvider _pathProvider;
        private ISherlockEnvironment _environment;
        private string _appName;


        public AppDataFolderRoot(IPathProvider pathProvider, ISherlockEnvironment environment, IOptions<SherlockOptions> options)
        {
            Guard.ArgumentNotNull(pathProvider, nameof(pathProvider));
            Guard.ArgumentNotNull(environment, nameof(environment));
            Guard.ArgumentNotNull(options, nameof(options));

            _environment = environment;
            _pathProvider = pathProvider;
            _appName = options.Value?.AppSystemName ?? "Unnamed App";
        }
        public virtual string RootPath
        {
            get { return "~/App_Data"; }
        }
        public string RootPhysicalFolder
        {
            get
            {
                    return _pathProvider.MapApplicationPath(RootPath);
            }
        }
    }
}