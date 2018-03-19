using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Sherlock.Framework.Web
{
    public class SherlockWebOptionsSetup : ConfigureFromConfigurationOptions<SherlockWebOptions>
    {
        public SherlockWebOptionsSetup(IConfiguration configuration)
            :base(configuration)
        {

        }
    }
}
