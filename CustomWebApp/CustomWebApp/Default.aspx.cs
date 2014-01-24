using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomWebApp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //This creates an administrator for the sample app if it does not exist
            if (Membership.FindUsersByName("admin").Count == 0)
            {
                MembershipUser newUser = Membership.CreateUser("admin", "intuit");
            }
            try { IppRealmOAuthProfile ippRealmOAuthProfile = IppRealmOAuthProfile.Read();
                
            }
            catch { }
        }
    }
}