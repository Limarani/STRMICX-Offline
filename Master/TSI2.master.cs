using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Master_TSI2 : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Lbltime.Text = DateTime.Now.ToLongDateString();

        if (SessionHandler.UserName == "") { Lblusername.Text = "Welcome .."; Imgtitle.Visible = false; }
        else if (SessionHandler.UserName != "") { Lblusername.Text = "Welcome " + SessionHandler.UserName + " .."; Imgtitle.Visible = true; }
    }
    protected void SignOut_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("LoginChecklist.aspx");
    }
}
