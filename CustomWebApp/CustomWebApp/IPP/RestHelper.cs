using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.LinqExtender;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Intuit.Ipp.Exception;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace CustomWebApp
{
    public static class RestHelper
    {

        public static void disconnectRealm(IppRealmOAuthProfile profile)
        {
            RestHelper.callPlatform(profile, Constants.IppEndPoints.DisconnectUrl);
            clearProfile(profile);
        }

        public static void clearProfile(IppRealmOAuthProfile profile)
        {
            profile.accessToken = "";
            profile.accessSecret = "";
            profile.realmId = "";
            profile.dataSource = -1;
            profile.Save();
        }

        public static string callPlatform(IppRealmOAuthProfile profile, string url)
        {

            OAuthConsumerContext consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString(),
                SignatureMethod = SignatureMethod.HmacSha1,
                ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"].ToString()
            };

            OAuthSession oSession = new OAuthSession(consumerContext, Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlRequestToken,
                                  Constants.OauthEndPoints.AuthorizeUrl,
                                  Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlAccessToken);

            oSession.ConsumerContext.UseHeaderForOAuthParameters = true;
            if (profile.accessToken.Length > 0)
            {
                oSession.AccessToken = new TokenBase
                {
                    Token = profile.accessToken,
                    ConsumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString(),
                    TokenSecret = profile.accessSecret
                };

                IConsumerRequest conReq = oSession.Request();
                conReq = conReq.Get();
                conReq = conReq.ForUrl(url);
                try
                {
                    conReq = conReq.SignWithToken();
                    return conReq.ReadBody();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "";
        }

        private static DataService getDataService(IppRealmOAuthProfile profile)
        {
            ServiceContext serviceContext = getServiceContext(profile);
            return new DataService(serviceContext);
        }

        private static ServiceContext getServiceContext(IppRealmOAuthProfile profile)
        {
            var consumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString();
            var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"].ToString();
            OAuthRequestValidator oauthValidator = new OAuthRequestValidator(profile.accessToken, profile.accessSecret, consumerKey, consumerSecret);
            return new ServiceContext(profile.realmId, (IntuitServicesType)profile.dataSource, oauthValidator);
        }

        public static List<Employee> getEmployeeList(IppRealmOAuthProfile profile)
        {
            ServiceContext serviceContext = getServiceContext(profile);
            QueryService<Employee> employeeQueryService = new QueryService<Employee>(serviceContext);
            return employeeQueryService.Select(c => c).ToList();
        }

        public static bool appTokensSet()
        {
            try
            {
                var consumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString();
                var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"].ToString();
                return consumerKey.Length > 0 && consumerSecret.Length > 0;
            }
            catch { return false; }
        }
    }
}