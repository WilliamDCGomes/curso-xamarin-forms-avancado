using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Newtonsoft.Json;
using Xamarin.Auth;
using LoginSocial.Droid;
using LoginSocial;

[assembly:ExportRenderer(typeof(LoginFacebookPage), typeof(LoginFacebookRender))]
namespace LoginSocial.Droid
{
    public class LoginFacebookRender : PageRenderer
    {
        [Obsolete]
        public LoginFacebookRender()
        {
            var oauth = new OAuth2Authenticator(
                clientId: "134196161879925",
                scope: "email",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_sucess.html")
            );
            oauth.Completed += async (sender, args) =>
            {
                if (args.IsAuthenticated)
                {
                    var token = args.Account.Properties["access_token"].ToString();
                    var requisicao = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=name,email"), null, args.Account);
                    var resposta = await requisicao.GetResponseAsync();
                    dynamic obj = JsonConvert.DeserializeObject(resposta.GetResponseText());
                    var Nome = obj.name.ToString();
                    var Email = obj.email.ToString();
                }
            };
            var activity = this.Context as Activity;
            activity.StartActivity(oauth.GetUI(activity));
        }
    }
}