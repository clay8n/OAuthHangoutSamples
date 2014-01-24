using Intuit.Ipp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomWebApp
{
    public partial class _Admin_Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated || System.Web.HttpContext.Current.User.Identity.Name != "admin")
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (accessToken.Text.Length == 0) { throw new Exception("Invalid Access Token"); }
                if (accessSecret.Text.Length == 0) { throw new Exception("Invalid Access Secret"); }
                if (realmId.Text.Length == 0) { throw new Exception("Invalid Realm ID"); }

                IppRealmOAuthProfile ippRealmOAuthProfile = new IppRealmOAuthProfile();
                ippRealmOAuthProfile.realmId = realmId.Text;
                switch (dataSource.SelectedValue.ToLower())
                {
                    case "qbo": ippRealmOAuthProfile.dataSource = (int)IntuitServicesType.QBO; break;
                    case "qbd": ippRealmOAuthProfile.dataSource = (int)IntuitServicesType.QBD; break;
                }
                ippRealmOAuthProfile.accessToken = accessToken.Text;
                ippRealmOAuthProfile.accessSecret = accessSecret.Text;
                ippRealmOAuthProfile.expirationDateTime = DateTime.Now;

                RestHelper.getEmployeeList(ippRealmOAuthProfile);

                ippRealmOAuthProfile.Save();
            }
            catch
            {
                errorMessage.Visible = true;
            }

        }
    }
}