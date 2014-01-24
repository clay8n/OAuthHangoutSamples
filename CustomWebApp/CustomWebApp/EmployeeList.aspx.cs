using Intuit.Ipp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomWebApp
{
    public partial class EmployeeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated || !RestHelper.appTokensSet() || IppRealmOAuthProfile.Read().accessToken.Length == 0)
                {
                    Response.Redirect("~/Default.aspx");
                }
                employeesView.DataSource = RestHelper.getEmployeeList(IppRealmOAuthProfile.Read());
                employeesView.DataBind();
            }
            catch
            {
            }
        }
    }
}