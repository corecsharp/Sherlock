using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Sherlock.Framework.Web.Authentication.Wechat
{
    public class WeChatOptions : OAuthOptions
    {
        public WeChatOptions()
        {
            this.AuthenticationScheme = "WeChat";
            this.DisplayName = "微信";
            this.AuthorizationEndpoint = "https://open.weixin.qq.com/connect/qrconnect";
            this.TokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/access_token";
            this.UserInformationEndpoint = "https://graph.qq.com/me";
            
            this.CallbackPath = new PathString(@"/signin-weixin");
        }
    }
}
