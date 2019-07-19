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

public partial class Pages_LoginChecklist : System.Web.UI.Page
{
    myConnection con = new myConnection();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{

        //}
        Checkuser();
    }

    public void Checkuser()
    {
        string strflag = "";
        ds.Dispose();
        ds.Reset();
        string strquery = "select flag from checklist_login_report where username='" + SessionHandler.UserName + "' and pdate=DATE_FORMAT(DATE_SUB(now(),INTERVAL '07:00' HOUR_MINUTE),'%d-%m-%Y')";
        ds = con.ExecuteQuery(strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            strflag = ds.Tables[0].Rows[0]["flag"].ToString();
            if (strflag == "1") { TogglePanel(LogoutPanel); }
            else { TogglePanel(LoginPanel); }
        }
        else { TogglePanel(LoginPanel); }
    }
    private void TogglePanel(Panel sPanel)
    {
        LoginPanel.Visible = false;
        LogoutPanel.Visible = false;

        sPanel.Visible = true;
    }
    protected void btncreatetlogin_Click(object sender, EventArgs e)
    {
        if (chkattendance.Checked == true && chkmobile.Checked == true && chkidcard.Checked == true && chkbiometric.Checked == true && chkhardware.Checked == true && chkheadset.Checked == true)
        {
            int result = 0;
            string strquery = "update checklist_login_report set flag=1,attendance='CHECKED',biometrice='CHECKED',mobile_restriction='CHECKED',id_card_dress_code='CHECKED',heatset_allot='CHECKED',login_hardware='CHECKED' where username='" + SessionHandler.UserName + "' and pdate=DATE_FORMAT(DATE_SUB(now(),INTERVAL '07:00' HOUR_MINUTE),'%d-%m-%Y')";
            result = con.ExecuteSPNonQuery(strquery);
            if (result > 0) 
            {
                if (SessionHandler.IsAdmin == true) Response.Redirect("Home.aspx");
                else if (SessionHandler.IsAdmin == false) Response.Redirect("NonAdminHome.aspx");
            }
            else { lblerror.Text = "Login Details does not saved"; }
        }
        else
        {
            lblerror.Text = "Check the missed CheckList.";
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        string strquery = "delete from checklist_login_report where username='" + SessionHandler.UserName + "' and pdate=DATE_FORMAT(DATE_SUB(now(),INTERVAL '07:00' HOUR_MINUTE),'%d-%m-%Y')";
        int result = con.ExecuteSPNonQuery(strquery);
        if (result > 0)
        {
            ResetSysName();
            SessionHandler.UserName = "";
            SessionHandler.IsAdmin = false;
            SessionHandler.IsprocessMenu = "0";
            SessionHandler.IspendingMenu = "0";
            Response.Redirect("Loginpage.aspx");
        }
    }

    protected void btncreatelogout_Click(object sender, EventArgs e)
    {
        if (chkheadsethandovr.Checked == true && chkplaceclean.Checked == true && chkswitchoff.Checked == true)
        {
            int result = 0;
            string strquery = "update checklist_login_report set flag=0,logout_time=now(),work_place_clean='CHECKED',headset_over='CHECKED',switchoff_system='CHECKED' where username='" + SessionHandler.UserName + "' and pdate=DATE_FORMAT(DATE_SUB(now(),INTERVAL '07:00' HOUR_MINUTE),'%d-%m-%Y')";
            result = con.ExecuteSPNonQuery(strquery);
            if (result > 0) 
            {
                ResetSysName();
                SessionHandler.UserName = "";
                SessionHandler.IsAdmin = false;
                SessionHandler.IsprocessMenu = "0";
                SessionHandler.IspendingMenu = "0";
                Response.Redirect("Loginpage.aspx");
            }
            else { lbllogerror.Text = "Logout Details does not saved"; }
        }
        else
        {
            lbllogerror.Text = "Check the missed CheckList.";
        }
    }
    protected void btnlogcancel_Click(object sender, EventArgs e)
    {
        if (SessionHandler.IsAdmin == true) Response.Redirect("Home.aspx");
        else if (SessionHandler.IsAdmin == false) Response.Redirect("NonAdminHome.aspx");
    }
    private void ResetSysName()
    {
        string update, sys = "";
        sys = System.Web.HttpContext.Current.Request.UserHostAddress;
        update = "update user_status set System=null where user_id='" + SessionHandler.UserName + "'";
        con.ExecuteSPNonQuery(update);
    }
}
