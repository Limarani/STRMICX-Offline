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
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;


public partial class Pages_ChangePassword : System.Web.UI.Page
{

    #region GlobalComponents
    GlobalClass gl = new GlobalClass();
    string StrOldPAss = "";
    #endregion


    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DivError.InnerHtml = "";
        }
    }
    #endregion

    #region Button Events
    protected void btnsave_Click2(object sender, EventArgs e)
    {
        GetUserDetails();
        if (StrOldPAss == txtOldPassword.Text)
        {
            string strUsername = SessionHandler.UserName;

            int ResultObj = gl.ChangePassword(SessionHandler.UserName, txtNewPassword.Text);
            if (ResultObj > 0)
            {
                Response.Redirect("Loginpage.aspx");
                //DivError.InnerHtml = "New Password Hasbeen Changed Successfully!";
            }
        }
        else
        {
            DivError.InnerHtml="Old password is incorrect..!";
        }
    }
    private void GetUserDetails()
    {
        try
        {
            DataSet DSPass = new DataSet();
            DSPass = gl.GetUserRecordsnew();
            if (DSPass.Tables[0].Rows[0]["pwd"] != null)
            {
                StrOldPAss = DSPass.Tables[0].Rows[0]["pwd"].ToString();
            }
        }
        catch (Exception ex)
        {
            myConnection.setError(ex.ToString());
            gl.RedirectErrorPage();
        }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        //if (SessionHandler.IsAdmin == true) Response.Redirect("Home.aspx");
        //else if (SessionHandler.IsAdmin == false) Response.Redirect("NonAdminHome.aspx");
        SessionHandler.UserName = "";
        Response.Redirect("Loginpage.aspx");
    }
    #endregion
}
