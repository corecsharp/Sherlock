using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Sherlock.Framework
{
    public class SherlockOptionsSetup : ConfigureFromConfigurationOptions<SherlockOptions>
    {
        public SherlockOptionsSetup(IConfiguration configuration)
            : base(configuration)
        {

        }
    }
}
