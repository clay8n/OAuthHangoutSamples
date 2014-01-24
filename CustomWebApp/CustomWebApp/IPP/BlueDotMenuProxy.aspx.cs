using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomWebApp
{
    public partial class BlueDotMenuProxy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IppRealmOAuthProfile profile = IppRealmOAuthProfile.Read();
            Response.Write(RestHelper.callPlatform(profile, Constants.IppEndPoints.BlueDotAppMenuUrl));
        }
    }
}