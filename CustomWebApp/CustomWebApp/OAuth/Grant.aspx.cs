using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using System.Configuration;

namespace CustomWebApp.OAuth
{
    public partial class Grant : System.Web.UI.Page
    {
        /// <summary>
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event Args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var consumerKey = ConfigurationManager.AppSettings["consumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];

            //Ask IPP for a request token to start the authorization process
            IToken token = getOAuthRequestTokenFromIpp(consumerKey, consumerSecret);
            Session["oauthRequestToken"] = token; //Save the request token to the current session.  This will be required on the callback page.

            //This will call back to /OAuth/Callback.aspx after the user has Authorized the connection
            var oauthCallbackUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + ConfigurationManager.AppSettings["oauth_callback_url"];

            //Redirect to IPP User Authorization page to ask them for permission to connect to your application
            //Pass the request token we received above along with the URL that IPP should redirect to after authorization has succeeded
            redirectToIntuitForUserAuthorization(token.Token, oauthCallbackUrl);
        }


        private IToken getOAuthRequestTokenFromIpp(string consumerKey, string consumerSecret)
        {
            IOAuthSession oauthSession = createDevDefinedOAuthSession(consumerKey, consumerSecret);
            return oauthSession.GetRequestToken();
        }

        private void redirectToIntuitForUserAuthorization(string requestToken, string callbackUrl)
        {
            var ippUserAuthorizationUrl = Constants.OauthEndPoints.AuthorizeUrl + "?oauth_token=" + requestToken + "&oauth_callback=" + UriUtility.UrlEncode(callbackUrl);
            Response.Redirect(ippUserAuthorizationUrl);
        }


        #region " DevDefined Helper Functions "

        private IOAuthSession createDevDefinedOAuthSession(string consumerKey, string consumerSecret)
        {

            var oauthRequestTokenUrl = Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlRequestToken;
            var oauthAccessTokenUrl = Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlAccessToken;
            var oauthUserAuthorizeUrl = Constants.OauthEndPoints.AuthorizeUrl;

            OAuthConsumerContext consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                SignatureMethod = SignatureMethod.HmacSha1
            };

            return new OAuthSession(consumerContext, oauthRequestTokenUrl, oauthUserAuthorizeUrl, oauthAccessTokenUrl);

        }

        #endregion

    }
}