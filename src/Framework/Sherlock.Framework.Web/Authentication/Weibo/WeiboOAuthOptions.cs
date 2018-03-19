using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Sherlock.Framework.Web.Authentication.Weibo
{
    public class WeiboOAuthOptions : OAuthOptions
    {
        public WeiboOAuthOptions()
        {
            this.AuthenticationScheme = "Weibo";
            this.DisplayName = "微博";
            this.AuthorizationEndpoint = "https://api.weibo.com/oauth2/authorize";
            this.TokenEndpoint = "https://api.weibo.com/oauth2/access_token";
            this.UserInformationEndpoint = "https://api.weibo.com/oauth2/get_token_info";
            this.CallbackPath = new PathString(@"/signin-weibo");
        }
    }
}
