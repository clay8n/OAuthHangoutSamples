using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CustomWebApp
{
    [Serializable]
    public class IppRealmOAuthProfile
    {
        public string accessToken { get; set; }
        public string accessSecret { get; set; }
        public string realmId { get; set; }
        public int dataSource { get; set; }
        public DateTime expirationDateTime { get; set; }

        public IppRealmOAuthProfile()
        {
            accessToken = "";
            accessSecret = "";
            realmId = "";
            dataSource = -1;
            expirationDateTime = DateTime.Now.AddSeconds(-1);
        }

        #region " Encrypted User Profile Management on Disk "

        public void Save()
        {
            if (accessToken.Length == 0 && File.Exists("ipp.xml")) { File.Delete("ipp.xml"); }
            else
            {
                using (var fileStream = File.OpenWrite("ipp.xml"))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                    StringWriter textWriter = new StringWriter();
                    xmlSerializer.Serialize(textWriter, this);
                    string encryptedRealmProfile = StringCipher.Encrypt(textWriter.ToString(), "<FILL_IN_PASSWORD_OR_OTHER_UNIQUE_APPLICATION_VALUE>");
                    byte[] bytes = new byte[encryptedRealmProfile.Length * sizeof(char)];
                    System.Buffer.BlockCopy(encryptedRealmProfile.ToCharArray(), 0, bytes, 0, bytes.Length);
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static IppRealmOAuthProfile Read()
        {
            if (!File.Exists("ipp.xml")) { return new IppRealmOAuthProfile(); }
            string encryptedRealmProfile = File.ReadAllText("ipp.xml", Encoding.Unicode);
            if (encryptedRealmProfile.Length == 0) { return new IppRealmOAuthProfile(); }
            string decryptedRealmProfile = StringCipher.Decrypt(encryptedRealmProfile, "<FILL_IN_PASSWORD_OR_OTHER_UNIQUE_APPLICATION_VALUE>");
            TextReader textReader = new StringReader(decryptedRealmProfile);
            XmlSerializer xmlSerializer = new XmlSerializer((new IppRealmOAuthProfile()).GetType());
            IppRealmOAuthProfile ippRealmOAuthProfile = (IppRealmOAuthProfile)xmlSerializer.Deserialize(textReader);
            if (ippRealmOAuthProfile.expirationDateTime.Subtract(DateTime.Now).TotalDays < 30)
            {
                ippRealmOAuthProfile = callReconnect(ippRealmOAuthProfile);
                ippRealmOAuthProfile.Save();
            }
            return ippRealmOAuthProfile;
        }

        private static IppRealmOAuthProfile callReconnect(IppRealmOAuthProfile ippRealmOAuthProfile)
        {
            string xmlResponse = RestHelper.callPlatform(ippRealmOAuthProfile, "https://appcenter.intuit.com/api/v1/Connection/Reconnect");
            XmlDocument reconnectResponse = new XmlDocument();
            reconnectResponse.LoadXml(xmlResponse);
            foreach (XmlNode childNode in reconnectResponse.DocumentElement)
            {
                switch (childNode.Name)
                {
                    case "OAuthToken": ippRealmOAuthProfile.accessToken = childNode.InnerText; break;
                    case "OAuthTokenSecret": ippRealmOAuthProfile.accessSecret = childNode.InnerText; break;
                    default: break;
                }
            }
            ippRealmOAuthProfile.expirationDateTime = DateTime.Now.AddMonths(6);
            return ippRealmOAuthProfile;
        }

        #endregion
    }
}
