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
public partial class Master_STRMICX_Offline : System.Web.UI.MasterPage
{
    DataSet ds = new DataSet();
    GlobalClass gl = new GlobalClass();
    myConnection con = new myConnection();

    string strdate = "";
    string StrOldPAss = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Lblusername.Text = SessionHandler.UserName;
            DivError.InnerHtml = "";
        }
    }

    public void LoadGrid()
    {
        DateTime pDt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", pDt);

        ds.Dispose();
        ds.Reset();
        //string query = "select count(Orderno) as Productivity,sum(if(WebPhone='Phone',1,0)) as Phone,sum(if(WebPhone='Website',1,0)) as Website,sum(if(WebPhone='Mailaway',1,0)) as Mailaway,sum(if(processType='DU',1,0) + if(processType='PRODUCTION',1,0)) as Production,sum(if(processType='QC',1,0)) as QC,cast(concat(Round((((count(Orderno)-sum(if(Quality='Error',1,0)))/count(Orderno))*100),0),'%') as char) as Quality,cast(round(sum(TIME_TO_SEC(Totalprocesstime))/60) as char) as Utilization from tbl_working where (Orderstaus='Completed' and DeliveryDate between '" + strdate + "' and '" + strdate + "' and Username='" + SessionHandler.UserName + "' and Isreview=0) or (processType='PRODUCTION' and Orderstaus='Completed' and Username='" + SessionHandler.UserName + "' and Iskey=1) group by Username";
        string query = "select count(Orderno) as Productivity,sum(if(WebPhone='Phone',1,0)) as Phone,sum(if(WebPhone='Website',1,0)) as Website,sum(if(WebPhone='Mailaway',1,0)) as Mailaway,sum(if(processType='DU',1,0) + if(processType='PRODUCTION',1,0)) as Production,sum(if(processType='QC',1,0)) as QC,cast(concat(Round((((count(Orderno)-sum(if(Quality='Error',1,0)))/count(Orderno))*100),0),'%') as char) as Quality,cast(round(sum(TIME_TO_SEC(Totalprocesstime))/60) as char) as Utilization from tbl_working where Orderstaus='Completed' and DeliveryDate between '" + strdate + "' and '" + strdate + "' and Username='" + SessionHandler.UserName + "' and Isreview=0 group by Username";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Gridutilization.DataSource = ds;
            //Gridutilization.DataBind();
        }
    }
    public void LoadBreak()
    {
        DateTime pDt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", pDt);
        string struser = SessionHandler.UserName;
        ds.Dispose();
        ds.Reset();
        ds = gl.TotalBreakTime(strdate, strdate, struser);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //lblbreak.Text = ds.Tables[0].Rows[0]["Total Time"].ToString();
            //if (lblbreak.Text == "") lblbreak.Text = "00:00:00";
        }
        //else lblbreak.Text = "00:00:00";
    }
    private void GetRights()
    {
        if (SessionHandler.CheckPwd == true)
        {
            if (SessionHandler.UserName == "")
            {
                //Lnkproduction.Visible = false;
                //Lnkpassword.Visible = false;
                //Lnkassignjob.Visible = false;
                //Lnkorderstatus.Visible = false;
                //LnkUsers.Visible = false;
                //LnkReports.Visible = false;
                //LnkLogout.Visible = false;
                //Lnkbreaktime.Visible = false;
                //LnkMIS.Visible = false;
                //LnkScrapingstatus.Visible = false;
            }
            else if (SessionHandler.IsAdmin == true)
            {
                //Lnkproduction.Visible = true;
                //Lnkpassword.Visible = true;
                //Lnkassignjob.Visible = true;
                //Lnkorderstatus.Visible = true;
                //LnkUsers.Visible = true;
                //LnkReports.Visible = true;
                //Lnkbreaktime.Visible = false;
                //LnkMIS.Visible = true;
                //LnkScrapingstatus.Visible = true;
            }
            else if (SessionHandler.AuditQA == "1")
            {
                //Lnkproduction.Visible = true;
                //Lnkpassword.Visible = true;
                //Lnkorderstatus.Visible = true;
                //Lnkassignjob.Visible = false;
                //LnkUsers.Visible = false;
                //LnkReports.Visible = true;
                //Lnkbreaktime.Visible = false;
                //LnkMIS.Visible = false;
                //LnkScrapingstatus.Visible = true;
            }
            else if (SessionHandler.IsprocessMenu == "1" && SessionHandler.IspendingMenu == "0")
            {
                //Lnkproduction.Visible = true;
                //Lnkpassword.Visible = true;
                //Lnkassignjob.Visible = false;
                //Lnkorderstatus.Visible = false;
                //LnkUsers.Visible = false;
                //LnkReports.Visible = false;
                //Lnkbreaktime.Visible = false;
                //LnkMIS.Visible = false;
                //LnkScrapingstatus.Visible = false;

            }
            else if (SessionHandler.IsprocessMenu == "0" && SessionHandler.IspendingMenu == "1")
            {
                //Lnkproduction.Visible = true;
                //Lnkpassword.Visible = true;
                //Lnkorderstatus.Visible = true;
                //Lnkassignjob.Visible = false;
                //LnkUsers.Visible = false;
                //LnkReports.Visible = true;
                //Lnkbreaktime.Visible = false;
                //LnkMIS.Visible = false;
                //LnkScrapingstatus.Visible = true;
            }
            else if (SessionHandler.IsprocessMenu == "1" && SessionHandler.IspendingMenu == "1")
            {
                //Lnkproduction.Visible = true;
                //Lnkpassword.Visible = true;
                //Lnkorderstatus.Visible = true;
                //Lnkassignjob.Visible = false;
                //LnkUsers.Visible = false;
                //LnkReports.Visible = true;
                //Lnkbreaktime.Visible = false;
                //LnkMIS.Visible = false;
                //LnkScrapingstatus.Visible = true;
            }
        }
        else
        {
            //LnkHome.Visible = false;
            //Lnkproduction.Visible = false;
            //Lnkpassword.Visible = false;
            //Lnkassignjob.Visible = false;
            //Lnkorderstatus.Visible = false;
            //LnkUsers.Visible = false;
            //LnkReports.Visible = false;
            //LnkLogout.Visible = false;
            //Lnkbreaktime.Visible = false;
            //LnkMIS.Visible = false;
            //LnkScrapingstatus.Visible = false;
        }
    }
    protected void LnkUsers_Click1(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Users.aspx");
    }
    protected void Lnkproduction_Click(object sender, EventArgs e)
    {
        string id = "12f7tre5";
        if (SessionHandler.AuditQA == "1") Response.Redirect("~/Pages/ProductionAuditNew.aspx?id=" + id);
        else Response.Redirect("~/Pages/ProductionNew.aspx?id=" + id);
    }
    protected void Lnkpassword_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/ChangePassword.aspx");
    }
    protected void Lnkassignjob_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/AssignJob.aspx");
    }
    protected void Lnkorderstatus_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/STRMICXOrderStatus.aspx");
    }
    protected void LnkScrapingstatus_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Stars_Tracking.aspx");
    }
    protected void LnkHome_Click(object sender, EventArgs e)
    {
        if (SessionHandler.IsAdmin == true) Response.Redirect("~/Pages/Home.aspx");
        else if (SessionHandler.IsAdmin == false) Response.Redirect("~/Pages/NonAdminHome.aspx");
    }
    protected void SignOut_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("LoginChecklist.aspx");
        SessionHandler.UserName = "";
        SessionHandler.IsAdmin = false;
        SessionHandler.IsprocessMenu = "0";
        SessionHandler.IspendingMenu = "0";
        Response.Redirect("STRMICXLogin.aspx");
    }
    protected void LnkReports_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Reports.aspx");
    }
    protected void Lnkbreaktime_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/BreakTime.aspx");
    }
    protected void LnkMIS_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/MISReport.aspx");
    }
    protected void btn_brk_start_Click(object sender, EventArgs e)
    {
        //hid_Ticker.Value = new TimeSpan(0, 0, 0).ToString();
        string query = "";
        //txt_break_reason.Text = "";
        //lblothererror.Text = "";
        DataSet result = new DataSet();
        DateTime dt = new DateTime();
        dt = DateTime.Now;
        string pdate = string.Empty;


        TimeSpan ptime = DateTime.Now.TimeOfDay;
        TimeSpan start = new TimeSpan(0, 0, 0);
        TimeSpan end = new TimeSpan(07, 0, 0);

        if (start <= ptime && ptime <= end)
        {
            dt = DateTime.Now.AddDays(-1);
            pdate = dt.ToString("MM-dd-yyyy");
        }

        else
        {
            dt = DateTime.Now;
            pdate = dt.ToString("MM-dd-yyyy");
        }

        //result = gl.insert_breaks(gl.order_no, pdate, gl.Process, drp_break.SelectedItem.Text, drp_break.SelectedItem.Value.ToString());

        string act_time = result.Tables[0].Rows[0]["act_time"].ToString();
        string brk_time = result.Tables[1].Rows[0]["brk_time"].ToString();
        if (brk_time == "")
        {
            brk_time = "01:00:00";
        }

        TimeSpan act = TimeSpan.Parse(act_time);
        TimeSpan brk = TimeSpan.Parse(brk_time);

        //if (act > brk && drp_break.SelectedItem.Text == "Meeting")
        //{
        //    if (brk.ToString().Contains("-"))
        //    {
        //        lblothererror.Text = "You have already Completed your breaklimits";
        //    }
        //    else
        //    {
        //        lblothererror.Text = "You have remaining " + " " + brk + " " + ":Mins";
        //    }

        //}

        //else
        //{
        //    lblothererror.Text = "";
        //}
        //div_brk_start.Visible = false;
        //lbl_brk_name.Text = drp_break.SelectedItem.Text.ToString();
        //div_brk_cmd.Visible = true;
        //div_brk_countdown.Visible = true;
        //div_brk_stop.Visible = false;
    }
    protected void btn_brk_cancel_Click(object sender, EventArgs e)
    {
        //lnkOthers.Text = "Break";

        //pagedimmer.Visible = false;
        //Other_breakMsgbx.Visible = false;
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //hid_Ticker.Value = TimeSpan.Parse(hid_Ticker.Value).Add(new TimeSpan(0, 0, 1)).ToString();
        //lit_Timer.Text = hid_Ticker.Value.ToString();
    }
    protected void btn_brk_stop_Click(object sender, EventArgs e)
    {
        DataSet ds_time_diff = new DataSet();
        DataSet ds_time = new DataSet();
        DateTime dt = new DateTime();
        dt = DateTime.Now;
        string pdate = string.Empty;


        TimeSpan ptime = DateTime.Now.TimeOfDay;
        TimeSpan start = new TimeSpan(0, 0, 0);
        TimeSpan end = new TimeSpan(07, 0, 0);

        if (start <= ptime && ptime <= end)
        {
            dt = DateTime.Now.AddDays(-1);
            pdate = dt.ToString("MM-dd-yyyy");
        }

        else
        {
            dt = DateTime.Now;
            pdate = dt.ToString("MM-dd-yyyy");
        }

        //ds_time_diff = gl.timeiff_breaks(lbl_brk_name.Text, pdate);

        string act_time = ds_time_diff.Tables[0].Rows[0]["act_time"].ToString();
        string brk_time = ds_time_diff.Tables[1].Rows[0]["diff_time"].ToString();

        TimeSpan act = TimeSpan.Parse(act_time);
        TimeSpan brk = TimeSpan.Parse(brk_time);

        if (act < brk)
        {

            //div_brk_countdown.Visible = false;
            //div_brk_stop.Visible = true;
            //lbl_brk_cmd_err.Text = "Please enter the Delay reason..";
        }

        else
        {
            //ds_time = gl.update_breaks(lbl_brk_name.Text, txt_break_reason.Text, pdate);

            //ds_time = gl.update_breaks(lbl_brk_name.Text, txt_break_reason.Text, pdate);
            string tottime = ds_time.Tables[0].Rows[0]["brk_time"].ToString();

            if (tottime != "")
            {
                TimeSpan time1 = TimeSpan.Parse(tottime);
                TimeSpan tmlimit = new TimeSpan(01, 0, 0);

                if (time1 > tmlimit)
                {
                    bool result = gl.sp_lock_user();

                }
            }
            //lnkOthers.Text = "Break";
            //pagedimmer.Visible = false;
            //Other_breakMsgbx.Visible = false;
        }



    }
    protected void btn_brk_reson_Click(object sender, EventArgs e)
    {
        //if (txt_break_reason.Text == "")
        //{
        //    lbl_brk_cmd_err.Text = "Please enter the comments...";
        //    return;
        //}
        DataSet ds_time = new DataSet();
        DateTime dt = new DateTime();
        dt = DateTime.Now;
        string pdate = string.Empty;


        TimeSpan ptime = DateTime.Now.TimeOfDay;
        TimeSpan start = new TimeSpan(0, 0, 0);
        TimeSpan end = new TimeSpan(07, 0, 0);

        if (start <= ptime && ptime <= end)
        {
            dt = DateTime.Now.AddDays(-1);
            pdate = dt.ToString("MM-dd-yyyy");
        }

        else
        {
            dt = DateTime.Now;
            pdate = dt.ToString("MM-dd-yyyy");
        }

        //ds_time = gl.update_breaks(lbl_brk_name.Text, txt_break_reason.Text, pdate);
        string tottime = ds_time.Tables[0].Rows[0]["brk_time"].ToString();
        TimeSpan time1 = TimeSpan.Parse(tottime);
        TimeSpan tmlimit = new TimeSpan(01, 0, 0);

        if (time1 > tmlimit)
        {
            bool result = gl.sp_lock_user();

        }


        //lnkOthers.Text = "Break";
        //pagedimmer.Visible = false;
        //Other_breakMsgbx.Visible = false;


    }
    public void lnkOthers_Click(object sender, EventArgs e)
    {
        //lnkOthers.Text = "UnBreak";
        //pagedimmer.Visible = true;
        //Other_breakMsgbx.Visible = true;
        //div_brk_start.Visible = true;
        //div_brk_cmd.Visible = false;
    }
    public void break_mis()
    {


        DataSet ds = new DataSet();
        ds = gl.get_diff_time();
        if (ds.Tables[0].Rows.Count > 0)
        {
            //lnkOthers.Text = "UnBreak";
            //pagedimmer.Visible = true;
            //Other_breakMsgbx.Visible = true;
            //div_brk_start.Visible = false;
            //lbl_brk_name.Text = "";
            //div_brk_cmd.Visible = true;
            //div_brk_countdown.Visible = true;
            //div_brk_stop.Visible = false;

            string brk_typ = ds.Tables[0].Rows[0]["break_type"].ToString();
            string dif_tm = ds.Tables[0].Rows[0]["diff_time"].ToString();

            //drp_break.Text = brk_typ;


            //lbl_brk_name.Text = brk_typ;
            string[] splt_time = dif_tm.Split(':');
            int hr = 0, min = 0, sec = 0;
            hr = Convert.ToInt32(splt_time[0].ToString());
            min = Convert.ToInt32(splt_time[1].ToString());
            sec = Convert.ToInt32(splt_time[2].ToString());
            //hid_Ticker.Value = new TimeSpan(hr, min, sec).ToString();

        }
    }

    protected void btnsave_Click2(object sender, EventArgs e)
    {
        GetUserDetails();
        if (StrOldPAss == txtOldPassword.Text)
        {
            string strUsername = SessionHandler.UserName;

            int ResultObj = gl.ChangePassword(SessionHandler.UserName, txtNewPassword.Text);
            if (ResultObj > 0)
            {
                Response.Redirect("STRMICXLogin.aspx");
                //DivError.InnerHtml = "New Password Hasbeen Changed Successfully!";
            }
        }
        else
        {           
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ChangeyopenModal();", true);
            PanelChangePassword.Visible = true;
            DivError.InnerHtml = "Old password is incorrect..!";
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


    protected void changepassword_Click(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "Pop", "ChangeyopenModal();", true);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ChangeyopenModal();", true);
        PanelChangePassword.Visible = true;
        DivError.InnerHtml = "";
    }

    protected void productionpage_ServerClick(object sender, EventArgs e)
    {
        string id = "12f7tre5";
        if (SessionHandler.AuditQA == "1") Response.Redirect("~/Pages/STRMICXProduction.aspx?id=" + id);
        else Response.Redirect("~/Pages/STRMICXProduction.aspx?id=" + id);
    }
}

