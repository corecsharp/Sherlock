using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Sherlock.Framework.Web.Authentication.QQ
{
    public class QQOAuthOptions : OAuthOptions
    {
        public QQOAuthOptions()
        {
            this.AuthenticationScheme = "QQ";
            this.DisplayName = this.AuthenticationScheme;
            this.AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";
            this.TokenEndpoint = "https://graph.qq.com/oauth2.0/token";
            this.UserInformationEndpoint = "https://graph.qq.com/oauth2.0/me";
            this.CallbackPath = new PathString(@"/signin-qq");
        }
    }
}
