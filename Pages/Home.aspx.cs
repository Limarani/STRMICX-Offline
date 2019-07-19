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

public partial class Pages_Home : System.Web.UI.Page
{

    GlobalClass gl = new GlobalClass();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataView dataview = new DataView();
    myConnection con = new myConnection();

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        //testing
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        if (SessionHandler.IsAdmin == false)
        {
            SessionHandler.UserName = "";
            Response.Redirect("Loginpage.aspx");
        }
        if (!Page.IsPostBack)
        {
            FetchtodaysQuality("Today");
            Targetuser.Visible = false;
            pagedimmer1.Visible = false;
            UsernameLoad();

            //New
            strdate = gl.setdate();

            if (SessionHandler.IsAdmin == true)
            {
                pnlTax.Visible = true;
                LoadTaxGrid();
                pnlInproc.Visible = true;
                LoadInProcessOrdrs();
                divscrape.Visible = true;
            }
            else
            {
                pnlTax.Visible = false;
            }
            //New
        }
    }

    //New
    string strdate = "";
    public void LoadTaxGrid()
    {
        DateTime pDt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", pDt);

        ds.Dispose();
        ds.Reset();
        //string query = "select order_no,pdate, username,ptype, tax_amount from entity_name where pdate between '" + strdate + "' and '" + strdate + "' and ptype <> 'Onhold' and ptype <> 'Inprocess'and ptype <> 'Parcelid' and ptype <> 'Mailaway' order by order_no,ptype";
        string query = "SELECT p.order_no, p.pdate, p.username as ProductionAgent, p.tax_amount as Production_Tax,q.username as QCAgent, q.tax_amount as QC_Tax,p.Pay_Status FROM entity_name p INNER JOIN entity_name q ON p.order_no = q.order_no  where p.pdate = '" + strdate.Replace("-", "/") + "' and p.pType='production' and q.pType='QC' and p.Tax_Amount <> q.Tax_Amount order by p.order_no";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdTax.DataSource = ds;
            grdTax.DataBind();
        }
    }
    public void LoadInProcessOrdrs()
    {
        DateTime pDt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", pDt);

        ds.Dispose();
        ds.Reset();
        //string query = "select orderno,count(*) as `InProcessAttempts` from tbl_working where orderstaus = 'In Process' and productiondate = '" + strdate.Replace("-", "/") + "' group by orderno having count(*) >=3";
	string query = "select p.orderno as `OrderNo#`, count(p.id) as `Inprocess-Count` from tbl_working p join record_status r on p.orderno=r.order_no where p.orderstaus = 'In Process' and (r.Pend='3' or r.Tax='3' or r.Parcel='3') group by p.orderno having count(p.id) >=3";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdinproc.DataSource = ds;
            grdinproc.DataBind();
        }
    }
    //New

    public void LoadTotal(string fdate, string tdate)
    {
        string strquery = "select count(order_no) as 'TotalDeliver' from record_status where K1='5' and QC='5' and Status='5' and DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + fdate + "' and '" + tdate + "'";
    }
    public void UsernameLoad()
    {
        string strquery = "select User_Name,(case when((Keying=1 and QC=0)) then 'FPY' else 'POA' end) as 'Type' from user_status where SST=0 order by User_Name";
        ds = con.ExecuteQuery(strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Gridusername.DataSource = ds;
            Gridusername.DataBind();
            Gridusername1.Visible = false;
            Gridusername.Visible = true;
        }
    }
    #endregion   

    #region Utilization Repors Bind in GridView
    private void FetchtodaysQuality(string Day)
    {
        string fdate = "", tdate = "";

        DateTime df = gl.ToDate();
        //tdate = df.ToString("MM/dd/yyyy");
        tdate = df.ToString("yyyy-MM-dd");

        //tdate = "07/25/2013";

        if (Day == "Today") { fdate = tdate; Lblinfo.Text = "Today's Target Status"; }
        else if (Day == "LastWeek") { fdate = df.AddDays(-7).ToString("yyyy-MM-dd"); Lblinfo.Text = "Last Week Utilization"; }
        else if (Day == "Last30") { fdate = df.AddDays(-30).ToString("yyyy-MM-dd"); Lblinfo.Text = "Last 30days Utilization"; }
        else if (Day == "Last90") { fdate = df.AddDays(-90).ToString("yyyy-MM-dd"); Lblinfo.Text = "Last 90days Utilization"; }

        QualityReport(fdate, tdate, Gridutilization);
        //LoadTotal(fdate, tdate);
        //DashBoardQualityReport(fdate, tdate, QualityGrid, QualityGridPostAudit);

    }
    private void QualityReport(string fdate, string tdate, GridView Gridview)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Dispose();
            ds.Reset();
            ds = gl.QualityReport(fdate, tdate, "update");
            if (myVariables.IsErr == true) { gl.RedirectErrorPage(); }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string strquery = "Update dash_board set Target='" + ds.Tables[0].Rows[i]["TotTarget"] + "',AuditType='" + ds.Tables[0].Rows[i]["Audit_Type"] + "' where Username='" + ds.Tables[0].Rows[i]["Name"] + "' limit 1;";
                    con.ExecuteSPNonQuery(strquery);
                }
            }
            ds.Dispose();
            ds.Reset();
            ds = gl.QualityReport(fdate, tdate, "select");
            if (myVariables.IsErr == true) { gl.RedirectErrorPage(); }
            if (ds.Tables[0].Rows.Count > 0)
            {
                Gridview.DataSource = ds.Tables[0];
                Gridview.DataBind();
            }
            else
            {
                Gridview.DataSource = null;
                Gridview.DataBind();
            }
        }
        catch (Exception ex)
        {
            myConnection.setError(ex.ToString());
            gl.RedirectErrorPage();
        }
    }
    #endregion

    #region GridEvents

    #region Variables
    public decimal utilization = 0;
    public decimal avgutili = 0;
    public decimal rowcnt = 0;
    #endregion

    protected void Gridutilization_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    int count = 0;

        //    rowcnt = rowcnt + 1;
        //    count = Int32.Parse(e.Row.Cells[6].Text);
        //    utilization = utilization + count;
        //}
        //if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    avgutili = utilization / rowcnt;
        //    //Lblutil.Text = "Average Utilization  " + Convert.ToString(avgutili * 10);
        //}

        //madesh
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    int count = 0;

        //    rowcnt = rowcnt + 1;
        //    if (e.Row.Cells[6].Text != "&nbsp;")
        //    {
        //        count = Int32.Parse(e.Row.Cells[6].Text);
        //    }
        //    utilization = utilization + count;
        //}
        //if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    avgutili = utilization / rowcnt;
        //    //Lblutil.Text = "Average Utilization  " + Convert.ToString(avgutili * 10);
        //}
    }

    protected void Gridutilization_Sorting(object sender, GridViewSortEventArgs e)
    {
        ColumnName(e.SortExpression);
    }

    private void ColumnName(string SortColumn)
    {
        string fdate = "", tdate = "";
        DateTime df = gl.ToDate();
        tdate = df.ToString("MM/dd/yyyy");
        //tdate = "07/25/2013";

        if (Lblinfo.Text == "Today Utilization") fdate = tdate;
        else if (Lblinfo.Text == "Last Week Utilization") fdate = df.AddDays(-7).ToString("yyyy-MM-dd");
        else if (Lblinfo.Text == "Last 30days Utilization") fdate = df.AddDays(-30).ToString("yyyy-MM-dd");
        else if (Lblinfo.Text == "Last 90days Utilization") fdate = df.AddDays(-90).ToString("yyyy-MM-dd");
        ds.Dispose();
        ds.Reset();

        ds = gl.QualityReport(fdate, tdate, "select");
        if (ds.Tables[0].Rows.Count > 0)
        {
            dataview = new DataView(ds.Tables[0]);
            dataview.Sort = SortColumn + " " + gl.getSDirection();
            Gridutilization.DataSource = dataview;
            Gridutilization.DataBind();
        }
    }

    #endregion

    #region Sidemenu Events
    protected void lastweek_Click(object sender, EventArgs e)
    {
        FetchtodaysQuality("LastWeek");
    }
    protected void last30days_Click(object sender, EventArgs e)
    {
        FetchtodaysQuality("Last30");
    }
    protected void last90days_Click(object sender, EventArgs e)
    {
        FetchtodaysQuality("Last90");
    }
    #endregion

    #region Quality Report

    private void DashBoardQualityReport(string fdate, string tdate, GridView Gridview, GridView Gridview1)
    {
        ds.Dispose();
        ds.Reset();

        ds = gl.DashBoardQuality(fdate, tdate);
        if (myVariables.IsErr == true) { gl.RedirectErrorPage(); }
        if (ds.Tables[0].Rows.Count > 0)
        {
            Gridview.DataSource = ds.Tables[0];
            Gridview.DataBind();
        }
        else
        {
            Gridview.DataSource = null;
            Gridview.DataBind();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            Gridview1.DataSource = ds.Tables[1];
            Gridview1.DataBind();
        }
        else
        {
            Gridview1.DataSource = null;
            Gridview1.DataBind();
        }
    }

    #endregion

    protected void lnkscraping_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/stars.aspx");
    }
    protected void lnkusertarget_Click(object sender, EventArgs e)
    {
        ds.Dispose();
        ds.Reset();
        string pdate = "";
        DateTime df = gl.ToDate();
        pdate = df.ToString("MM/dd/yyyy");
        //pdate = "07/25/2013";

        string query = "select sf_targetcheck('" + pdate + "') as Pdate";
        ds = con.ExecuteQuery(query);
        string result = "";
        if (ds.Tables[0].Rows.Count > 0) result = Convert.ToString(ds.Tables[0].Rows[0]["Pdate"]);
        else result = "";
        if (result != "")
        {
            ds.Dispose();
            ds.Reset();
            string strquery = "select User_Name,(case when((Keying=1 and QC=0)) then 'FPY' else 'POA' end) as 'Type',Target,Comments from user_status us join user_target ut on (us.User_Name=ut.Name) where us.SST=0 and ut.Pdate='" + pdate + "' order by User_Name";
            ds = con.ExecuteQuery(strquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Gridusername1.DataSource = ds;
                Gridusername1.DataBind();
                Gridusername1.Visible = true;
                Gridusername.Visible = false;
            }
        }
        Targetuser.Visible = true;
        pagedimmer1.Visible = true;
    }
    protected void btnsettarget_Click(object sender, EventArgs e)
    {
        DateTime df = gl.ToDate();
        string pdate = df.ToString("MM/dd/yyyy");
        //string pdate = "07/25/2013";
        ds.Dispose();
        ds.Reset();
        string query = "select sf_targetcheck('" + pdate + "') as Pdate";
        ds = con.ExecuteQuery(query);
        string result = "";
        if (ds.Tables[0].Rows.Count > 0) result = Convert.ToString(ds.Tables[0].Rows[0]["Pdate"]);
        else result = "";
        if (result == "") Inserttarget(pdate);
        else Updatetarget(pdate);

        FetchtodaysQuality("Today");
        Targetuser.Visible = false;
        pagedimmer1.Visible = false;
    }
    public void Inserttarget(string pdate)
    {
        string strusr, strtype, strtarget, strquery = "";
        foreach (GridViewRow rowvalues in Gridusername.Rows)
        {
            TextBox txttarget = (TextBox)rowvalues.FindControl("txttargetvalue");
            strusr = rowvalues.Cells[0].Text.ToString();
            strtype = rowvalues.Cells[1].Text.ToString();
            strtarget = txttarget.Text;
            if (strtarget == "") strtarget = "0";
            strquery = "Insert into user_target (Pdate,Name,Audit_Type,Target) Values ('" + pdate + "','" + strusr + "','" + strtype + "','" + strtarget + "')";
            int result = con.ExecuteSPNonQuery(strquery);
        }
    }
    public void Updatetarget(string pdate)
    {
        string strusr, strtype, strtarget, strcomments, strquery = "";
        foreach (GridViewRow rowvalues in Gridusername1.Rows)
        {
            TextBox txttarget = (TextBox)rowvalues.FindControl("txttargetvalue1");
            TextBox txtcomm = (TextBox)rowvalues.FindControl("txtcomments");

            strusr = rowvalues.Cells[0].Text.ToString();
            strtype = rowvalues.Cells[1].Text.ToString();
            strtarget = txttarget.Text;
            strcomments = txtcomm.Text;
            if (strtarget == "") strtarget = "0";
            if (strcomments != "") strquery = "Update user_target set Target='" + strtarget + "',Audit_Type='" + strtype + "',Comments='" + strcomments + "' where Pdate='" + pdate + "' and Name='" + strusr + "'";
            else strquery = "Update user_target set Target='" + strtarget + "',Audit_Type='" + strtype + "' where Pdate='" + pdate + "' and Name='" + strusr + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Targetuser.Visible = false;
        pagedimmer1.Visible = false;
        FetchtodaysQuality("Today");
    }
    protected void lnkadminerror_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/NonAdminHome.aspx");
    }
    protected void Lnkposterror_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/PostAuditError.aspx");
    }

    #region Auto Refresh
    protected void Chkrefresh_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkrefresh.Checked == true) Refresh.Enabled = true;
        else Refresh.Enabled = false;
    }
    protected void Refresh_Tick(object sender, EventArgs e)
    {
        FetchtodaysQuality("Today");
    }
    #endregion

    static string prevPage = String.Empty;


    protected void rrr_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Test.aspx");
    }
}
