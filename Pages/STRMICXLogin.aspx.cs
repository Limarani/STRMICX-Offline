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
using System.Text.RegularExpressions;
using MySql.Data;
using MySql.Data.MySqlClient;
public partial class Pages_STRLogin : System.Web.UI.Page
{
        GlobalClass gl = new GlobalClass();
        myConnection con = new myConnection();
        DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    #region Login Button Events
    protected void Lnksignin_Click(object sender, EventArgs e)
    {
        Regex reg = new Regex("(?=^.{8,}$)(?=.*\\d)(?=.*\\W+)(?![.\n]).*$");
        MySqlDataReader mdra;
        string UserName = "", admin = "", process = "", pend = "", audit = "", sys = "";
        sys = System.Web.HttpContext.Current.Request.UserHostAddress;
        ds.Dispose();
        ds.Reset();
        ds = gl.GetcheckLogin(txtusername.Text, txtpassword.Text, sys);
        if (ds.Tables.Count == 1)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                string strerr = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                if (strerr == "Login Error") Label1.Text = "Please check the username and password...";
            }
        }
        else
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                admin = Convert.ToString(ds.Tables[0].Rows[0]["Admin"]);
                process = Convert.ToString(ds.Tables[0].Rows[0]["Process"]);
                pend = Convert.ToString(ds.Tables[0].Rows[0]["Pending"]);
                audit = Convert.ToString(ds.Tables[0].Rows[0]["Audit"]);

                if (admin == "1") SessionHandler.IsAdmin = true;
                else if (admin == "0") SessionHandler.IsAdmin = false;

                if (process.Contains("1")) SessionHandler.IsprocessMenu = "1";
                else SessionHandler.IsprocessMenu = "0";

                if (pend.Contains("1")) SessionHandler.IspendingMenu = "1";
                else SessionHandler.IspendingMenu = "0";

                //if (audit == "11") SessionHandler.AuditQA = "1";
                //else SessionHandler.AuditQA = "0";
                if (audit == "1") SessionHandler.AuditQA = "1";
                else SessionHandler.AuditQA = "0";

                UserName = gl.TCase(Convert.ToString(ds.Tables[0].Rows[0]["User_Name"]));
                SessionHandler.UserName = UserName;
                bool chk = reg.IsMatch(txtpassword.Text);
                CheckuserNew(chk);
            }
            else
            {
                Label1.Text = "Please check the username and password...";
                return;
            }
        }
      
    }
    
    private void CheckuserNew(bool chk)
    {
        if (ds.Tables[1].Rows.Count > 0)
        {
            string strerr = Convert.ToString(ds.Tables[1].Rows[0]["Status"]);
            if (strerr == "Home" || strerr == "Add Checklist")
            {
                if (chk)
                {
                    SessionHandler.CheckPwd = true;
                    if (strerr == "Home")
                    {
                        if (SessionHandler.IsAdmin == true) Response.Redirect("~/Pages/Home.aspx");
                        else if (SessionHandler.IsAdmin == false) Response.Redirect("~/Pages/NonAdminHome.aspx");
                        Response.Write("<script>alert('Login Successfully...')</script>");
                    }
                    else Response.Redirect("~/Pages/STRMICXHOME.aspx");

                }
                else
                {
                    SessionHandler.CheckPwd = false;
                    Response.Redirect("ChangePassword.aspx");
                }
            }
            else if (strerr == "Checklist")
            {
                SessionHandler.CheckPwd = true;
                Response.Redirect("~/Pages/STRMICXHOME.aspx");
            }
            else if (strerr == "Login Entry")
            {
                SessionHandler.UserName = "";
                Label1.Text = "Already current date Login entry is exists. Please contact your Team Lead...!";
                return;
            }
            else if (strerr == "Logout Missing")
            {
                SessionHandler.UserName = "";
                Label1.Text = "Yesterday you didn't logout internal tool. Please contact your Team Lead...!";
                return;
            }

        }
    }
    protected void txtpassword_TextChanged(object sender, EventArgs e)
    {
        Lnksignin_Click(sender, e);
    }
    private void SetSysName()
    {
        string update, sys = "";
        sys = System.Web.HttpContext.Current.Request.UserHostAddress;
        update = "update user_status set System='" + sys + "' where user_id='" + SessionHandler.UserName + "'";
        con.ExecuteSPNonQuery(update);
    }
    #endregion
}