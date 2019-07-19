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

public partial class Pages_BreakTime : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    GlobalClass gblcls = new GlobalClass();
    myConnection con = new myConnection();

    string strdate, strusrname = "";
    string breakout1, breakout2 = "";
    string breakin1, breakin2 = "";

    string strdnrin, strdnrout, strmeetin, strmeetout, strtrainin, strtrainout = "";

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
		if (SessionHandler.IsAdmin == false)
        {
            SessionHandler.UserName = "";
            Response.Redirect("Loginpage.aspx");
        }
        strdate = gblcls.setdate();
        DateTime dt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", dt);

        if (!Page.IsPostBack)
        {
            UsernameLoad();
            LoadGrid("");
        }
    }

    public void LoadGrid(string struser)
    {
        DateTime dt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", dt);

        //string strusr = "";
        ds.Dispose();
        ds.Reset();
        ds = gblcls.BreakTimeDetails(struser, strdate);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridBreakDetails.DataSource = ds;
            GridBreakDetails.DataBind();
        }
        else
        {
            GridBreakDetails.DataSource = null;
            GridBreakDetails.DataBind();
        }
    }

    public void UsernameLoad()
    {
        ds.Dispose();
        ds.Reset();
        string strquery = "select User_Name from user_status order by User_Name";
        ds = con.ExecuteQuery(strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Lstuser.DataSource = ds;
            Lstuser.DataTextField = "User_Name";
            Lstuser.DataBind();
        }
    }
    #endregion

    //#region End Time
    //protected void btnend_Click(object sender, EventArgs e)
    //{
    //    DateTime dt = Convert.ToDateTime(strdate);
    //    strdate = String.Format("{0:MM/dd/yyyy}", dt);

    //    for (int i = 0; i < Lstuser.Items.Count; i++)
    //    {
    //        if (Lstuser.Items[i].Selected == true)
    //        {
    //            strusrname = Lstuser.Items[i].Text;
    //            if (strusrname != "")
    //            {
    //                CheckOrder(strusrname);
    //                string query = "Select sf_breaktime('" + strusrname + "','" + strdate + "')";
    //                string result = con.ExecuteScalar(query);
    //                if (result != "")
    //                {
    //                    ds.Dispose();
    //                    ds.Reset();
    //                    ds = gblcls.BreakTimeDetails(strusrname, strdate);
    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        breakout1 = Convert.ToString(ds.Tables[0].Rows[0]["Break1_Out"]);
    //                        breakout2 = Convert.ToString(ds.Tables[0].Rows[0]["Break2_Out"]);
    //                        strdnrout = Convert.ToString(ds.Tables[0].Rows[0]["Dinner_Out"]);
    //                        strmeetout = Convert.ToString(ds.Tables[0].Rows[0]["Meeting_Out"]);
    //                        strtrainout = Convert.ToString(ds.Tables[0].Rows[0]["Training_Out"]);

    //                        breakin1 = Convert.ToString(ds.Tables[0].Rows[0]["Break1_In"]);
    //                        breakin2 = Convert.ToString(ds.Tables[0].Rows[0]["Break2_In"]);
    //                        strdnrin = Convert.ToString(ds.Tables[0].Rows[0]["Dinner_In"]);
    //                        strmeetin = Convert.ToString(ds.Tables[0].Rows[0]["Meeting_In"]);
    //                        strtrainin = Convert.ToString(ds.Tables[0].Rows[0]["Training_In"]);

    //                        if (strdnrout != "" && strdnrin == "")
    //                        {
    //                            string strquery = "update user_breaktime set Dinner_In=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                            con.ExecuteSPNonQuery(strquery);
    //                        }
    //                        else if (strmeetout != "" && strmeetin == "")
    //                        {
    //                            string strquery = "update user_breaktime set Meeting_In=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                            con.ExecuteSPNonQuery(strquery);
    //                        }
    //                        else if (strtrainout != "" && strtrainin == "")
    //                        {
    //                            string strquery = "update user_breaktime set Training_In=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                            con.ExecuteSPNonQuery(strquery);
    //                        }
    //                        else if (breakout1 != "" && breakin1 == "")
    //                        {
    //                            string strquery = "update user_breaktime set Break1_In=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                            con.ExecuteSPNonQuery(strquery);
    //                        }
    //                        else if (breakout1 != "" && breakout2 != "" && breakin1 != "" && breakin2 == "")
    //                        {
    //                            string strquery = "update user_breaktime set Break2_In=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                            con.ExecuteSPNonQuery(strquery);
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                errorlabel.Text = "Please select Username";
    //            }
    //        }
    //    }
    //    LoadGrid();
    //}

    //public void CheckOrder(string strname)
    //{
    //    string process, query, query1, result = "";
    //    int count = 0;
    //    strname = gblcls.TCase(strname);
    //    process = "KEY";
    //    query = "Select sf_keying('" + strname + "','" + process + "')";
    //    result = con.ExecuteScalar(query);
    //    if (result == "")
    //    {
    //        process = "QC";
    //        query = "Select sf_keying('" + strname + "','" + process + "')";
    //        result = con.ExecuteScalar(query);
    //        if (result == "")
    //        {
    //            process = "DU";
    //            query = "Select sf_keying('" + strname + "','" + process + "')";
    //            result = con.ExecuteScalar(query);
    //            if (result == "")
    //            {
    //                process = "INPROCESS";
    //                query = "Select sf_keying('" + strname + "','" + process + "')";
    //                result = con.ExecuteScalar(query);
    //                if (result == "")
    //                {
    //                    process = "PARCELID";
    //                    query = "Select sf_keying('" + strname + "','" + process + "')";
    //                    result = con.ExecuteScalar(query);
    //                    if (result == "")
    //                    {
    //                        process = "ONHOLD";
    //                        query = "Select sf_keying('" + strname + "','" + process + "')";
    //                        result = con.ExecuteScalar(query);
    //                        if (result == "")
    //                        {
    //                            process = "MAILAWAY";
    //                            query = "Select sf_keying('" + strname + "','" + process + "')";
    //                            result = con.ExecuteScalar(query);
    //                        }
    //                        else
    //                        {
    //                            count = gblcls.BreakResetOrder(process, result);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        count = gblcls.BreakResetOrder(process, result);
    //                    }
    //                }
    //                else
    //                {
    //                    count = gblcls.BreakResetOrder(process, result);
    //                }
    //            }
    //            else
    //            {
    //                count = gblcls.BreakResetOrder(process, result);
    //            }
    //        }
    //        else
    //        {
    //            count = gblcls.BreakResetOrder(process, result);
    //        }
    //    }
    //    else
    //    {
    //        count = gblcls.BreakResetOrder(process, result);
    //    }
    //}
    //#endregion

    //#region Break Time
    //protected void btnbreak_Click(object sender, EventArgs e)
    //{

    //    string strquery = "", result = "";
    //    for (int i = 0; i < Lstuser.Items.Count; i++)
    //    {
    //        if (Lstuser.Items[i].Selected == true)
    //        {
    //            strusrname = Lstuser.Items[i].Text;
    //            if (strusrname != "")
    //            {
    //                CheckOrder(strusrname);
    //                strusrname = Lstuser.Items[i].Text;
    //                result = CheckResult(strusrname, strdate);

    //                if (result != "")
    //                {
    //                    ds.Dispose();
    //                    ds.Reset();
    //                    ds = gblcls.BreakTimeDetails(strusrname, strdate);
    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        breakout1 = ds.Tables[0].Rows[0]["Break1_Out"].ToString();
    //                        breakout2 = ds.Tables[0].Rows[0]["Break2_Out"].ToString();
    //                        breakin1 = ds.Tables[0].Rows[0]["Break1_In"].ToString();
    //                        breakin2 = ds.Tables[0].Rows[0]["Break2_In"].ToString();
    //                        if (breakout1 == "" && breakin1 == "")
    //                        {
    //                            strquery = "update user_breaktime set Break1_Out=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                        }
    //                        else if (breakin1 != "" && breakout2 == "")
    //                        {
    //                            strquery = "update user_breaktime set Break2_Out=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    strquery = "insert into user_breaktime(Tdate,UserName,Break1_Out)values('" + strdate + "','" + strusrname + "',now())";
    //                }
    //                con.ExecuteSPNonQuery(strquery);
    //            }
    //        }
    //    }
    //    LoadGrid();
    //}
    //#endregion

    //#region Dinner Time
    //protected void btndinner_Click(object sender, EventArgs e)
    //{
    //    string strquery = "", result = "";
    //    for (int i = 0; i < Lstuser.Items.Count; i++)
    //    {
    //        if (Lstuser.Items[i].Selected == true)
    //        {
    //            strusrname = Lstuser.Items[i].Text;
    //            if (strusrname != "")
    //            {
    //                CheckOrder(strusrname);
    //                strusrname = Lstuser.Items[i].Text;
    //                result = CheckResult(strusrname, strdate);

    //                if (result != "")
    //                {
    //                    strquery = "update user_breaktime set Dinner_Out=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                }
    //                else
    //                {
    //                    strquery = "insert into user_breaktime(Tdate,UserName,Dinner_Out)values('" + strdate + "','" + strusrname + "',now())";
    //                }
    //                con.ExecuteSPNonQuery(strquery);
    //            }
    //        }
    //    }
    //    LoadGrid();
    //}
    //#endregion

    //#region Meeting Time
    //protected void btnmeeting_Click(object sender, EventArgs e)
    //{
    //    string strquery = "", result = "";
    //    for (int i = 0; i < Lstuser.Items.Count; i++)
    //    {
    //        if (Lstuser.Items[i].Selected == true)
    //        {
    //            strusrname = Lstuser.Items[i].Text;
    //            if (strusrname != "")
    //            {
    //                CheckOrder(strusrname);
    //                strusrname = Lstuser.Items[i].Text;
    //                result = CheckResult(strusrname, strdate);

    //                if (result != "")
    //                {
    //                    strquery = "update user_breaktime set Meeting_Out=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                }
    //                else
    //                {
    //                    strquery = "insert into user_breaktime(Tdate,UserName,Meeting_Out)values('" + strdate + "','" + strusrname + "',now())";
    //                }
    //                con.ExecuteSPNonQuery(strquery);
    //            }
    //        }
    //    }
    //    LoadGrid();
    //}
    //#endregion

    //#region Trainind Time
    //protected void btntraining_Click(object sender, EventArgs e)
    //{
    //    string strquery = "", result = "";
    //    for (int i = 0; i < Lstuser.Items.Count; i++)
    //    {
    //        if (Lstuser.Items[i].Selected == true)
    //        {
    //            strusrname = Lstuser.Items[i].Text;
    //            if (strusrname != "")
    //            {
    //                CheckOrder(strusrname);
    //                strusrname = Lstuser.Items[i].Text;
    //                result = CheckResult(strusrname, strdate);

    //                if (result != "")
    //                {
    //                    strquery = "update user_breaktime set Training_Out=now() where Tdate='" + strdate + "' and UserName='" + strusrname + "'";
    //                }
    //                else
    //                {
    //                    strquery = "insert into user_breaktime(Tdate,UserName,Training_Out)values('" + strdate + "','" + strusrname + "',now())";
    //                }
    //                con.ExecuteSPNonQuery(strquery);
    //            }
    //        }
    //    }
    //    LoadGrid();
    //}
    //#endregion

    private string CheckResult(string strusrname, string strdate)
    {
        string query = "Select sf_breaktime('" + strusrname + "','" + strdate + "')";
        string result = con.ExecuteScalar(query);

        return result;
    }

    protected void GridBreakDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //string StartTime, EndTime = "";
        //int TAT = 0;
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (e.Row.Cells[3].Text == "&nbsp;" || e.Row.Cells[3].Text == "")
        //    {
        //        if (e.Row.Cells[6].Text == "&nbsp;" || e.Row.Cells[6].Text == "")
        //        {
        //            if (e.Row.Cells[9].Text == "&nbsp;" || e.Row.Cells[9].Text == "")
        //            {
        //                return;
        //            }
        //            else if (e.Row.Cells[9].Text != "" && (e.Row.Cells[10].Text == "&nbsp;" || e.Row.Cells[10].Text == ""))
        //            {
        //                StartTime = e.Row.Cells[9].Text;
        //                EndTime = DateTime.Now.ToString();
        //                TAT = GridColor(StartTime, EndTime);
        //                if (TAT > 003000) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
        //            }
        //        }
        //        else if (e.Row.Cells[6].Text != "" && (e.Row.Cells[7].Text == "&nbsp;" || e.Row.Cells[7].Text == ""))
        //        {
        //            StartTime = e.Row.Cells[6].Text;
        //            EndTime = DateTime.Now.ToString();
        //            TAT = GridColor(StartTime, EndTime);
        //            if (TAT > 001500) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
        //        }
        //    }
        //    else if (e.Row.Cells[3].Text != "" && (e.Row.Cells[4].Text == "&nbsp;" || e.Row.Cells[4].Text == ""))
        //    {
        //        StartTime = e.Row.Cells[3].Text;
        //        EndTime = DateTime.Now.ToString();
        //        TAT = GridColor(StartTime, EndTime);
        //        if (TAT > 001500) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
        //    }
        //}
    }

    public int GridColor(string StartTime, string EndTime)
    {
        DateTime startTime = DateTime.Parse(StartTime);
        DateTime endTime = DateTime.Parse(EndTime);
        TimeSpan ts = endTime.Subtract(startTime);
        string time = ts.ToString();
        int TAT = int.Parse(time.Replace(":", ""));

        return TAT;
    }

    #region Auto Refresh
    protected void Chkrefresh_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkrefresh.Checked == true) Refresh.Enabled = true;
        else Refresh.Enabled = false;
    }
    protected void Refresh_Tick(object sender, EventArgs e)
    {
        Page_Load(sender, e);
    }
    #endregion

    protected void Lstuser_SelectedIndexChanged(object sender, EventArgs e)
    {
        string struser = Lstuser.SelectedItem.Text;
        LoadGrid(struser);
    }
}
