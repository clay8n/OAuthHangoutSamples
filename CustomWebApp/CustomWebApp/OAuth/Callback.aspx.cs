using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using Intuit.Ipp.Core;
using IppDotNetSdkQuickBooksApiV3SampleWebFormsApp.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomWebApp.OAuth
{
    public partial class Callback : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var consumerKey = ConfigurationManager.AppSettings["consumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];

            //These values are passed back from IPP in the query string after successful authorization.  Parse what is received from IPP.
            Dictionary<string, string> oAuthValuesFromCallbackFromIpp = parseOAuthCallbackFromIpp();

            //Exchange the request token, along with the verifier that we received in the callback, for an access token to complete authorization with IPP
            IToken accessToken = exchangeRequestTokenForAccessToken(consumerKey, consumerSecret, (IToken)Session["oauthRequestToken"], oAuthValuesFromCallbackFromIpp["oauth_verifier"]);

            //We now have a set of tokens that are valid for 6 months unless revoked.  Save them to the user's profile so we can make calls to IPP QuickBooks APIs.
            saveAuthorizationToSettings(oAuthValuesFromCallbackFromIpp, accessToken);

            //The page will now render, refresh the parent page to reflect the new connected status and close this dialog

        }

        private Dictionary<string, string> parseOAuthCallbackFromIpp()
        {
            Dictionary<string, string> callbackValues = new Dictionary<string, string>();
            callbackValues.Add("oauth_verifier", Request.QueryString["oauth_verifier"].ToString());
            callbackValues.Add("realmId", Request.QueryString["realmId"].ToString());
            callbackValues.Add("dataSource", Request.QueryString["dataSource"].ToString());
            return callbackValues;
        }

        public IToken exchangeRequestTokenForAccessToken(string consumerKey, string consumerSecret, IToken requestToken, string oauthVerifier)
        {
            IOAuthSession oauthSession = createDevDefinedOAuthSession(consumerKey, consumerSecret);
            return oauthSession.ExchangeRequestTokenForAccessToken(requestToken, oauthVerifier);
        }

        public void saveAuthorizationToSettings(Dictionary<string, string> valuesFromCallback, IToken authorizedAccessToken)
        {
            IppRealmOAuthProfile ippRealmOAuthProfile = new IppRealmOAuthProfile();
            ippRealmOAuthProfile.realmId = Request.QueryString["realmId"].ToString();
            switch (Request.QueryString["dataSource"].ToString().ToLower())
            {
                case "qbo": ippRealmOAuthProfile.dataSource = (int)IntuitServicesType.QBO; break;
                case "qbd": ippRealmOAuthProfile.dataSource = (int)IntuitServicesType.QBD; break;
            }
            ippRealmOAuthProfile.accessToken = authorizedAccessToken.Token;
            ippRealmOAuthProfile.accessSecret  = authorizedAccessToken.TokenSecret;
            ippRealmOAuthProfile.expirationDateTime = DateTime.Now;
            ippRealmOAuthProfile.Save();
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