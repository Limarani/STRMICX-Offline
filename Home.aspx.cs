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
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        if (!Page.IsPostBack)
        {
            FetchtodaysQuality("Today");
        }
    }
    #endregion   

    #region Utilization Repors Bind in GridView
    private void FetchtodaysQuality(string Day)
    {
        string fdate = "", tdate = "";

        DateTime df = gl.ToDate(); 
        tdate = df.ToString("MM/dd/yyyy");

        //tdate = "05/10/2013";

        if (Day == "Today") { fdate = tdate; Lblinfo.Text = "Today Utilization"; }
        else if (Day == "LastWeek") { fdate = df.AddDays(-7).ToString("MM/dd/yyyy"); Lblinfo.Text = "Last Week Utilization"; }
        else if (Day == "Last30") { fdate = df.AddDays(-30).ToString("MM/dd/yyyy"); Lblinfo.Text = "Last 30days Utilization"; }
        else if (Day == "Last90") {fdate = df.AddDays(-90).ToString("MM/dd/yyyy");Lblinfo.Text = "Last 90days Utilization";}

        QualityReport(fdate, tdate, Gridutilization, GridutilizationQC);
        DashBoardQualityReport(fdate, tdate, QualityGrid, QualityGridPostAudit);

    }
    private void QualityReport(string fdate, string tdate, GridView Gridview, GridView Gridview1)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Dispose();
            ds.Reset();
            ds = gl.QualityReport(fdate, tdate, "");
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
        catch (Exception ex)
        {
            myConnection.setError(ex.ToString());
            gl.RedirectErrorPage();
        }
    }
    #endregion

    #region GridEvents
    public decimal error = 0;
    public decimal Rccount = 0;
    public decimal quality = 0;

    public decimal error1 = 0;
    public decimal Rccount1 = 0;
    public decimal quality1 = 0;
    public decimal ptotal = 0;
    public decimal pphone = 0;
    public decimal pwebsite = 0;
    public decimal pmailaway = 0;

    public decimal qtotal = 0;
    public decimal qphone = 0;
    public decimal qwebsite = 0;
    public decimal qmailaway = 0;

    public decimal total = 0;
    public decimal phone = 0;
    public decimal website = 0;
    public decimal mailaway = 0;
    public TimeSpan datetime;
    public TimeSpan datetime1;
    public TimeSpan datetime2;
    public TimeSpan datetime3;

    public decimal total1 = 0;
    public decimal phone1 = 0;
    public decimal website1 = 0;
    public decimal mailaway1 = 0;
    public TimeSpan datetime4;
    public TimeSpan datetime5;
    public TimeSpan datetime6;
    public TimeSpan datetime7;

    public TimeSpan datetime8;
    public TimeSpan datetime9;

    protected void Gridutilization_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strday, strhour, strmin, strmsec = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /* Calculate Total orders */
            int count, count1, count2, count3 = 0;
            TimeSpan dtime = new TimeSpan();
            TimeSpan dtime1 = new TimeSpan();
            TimeSpan dtime2 = new TimeSpan();
            TimeSpan dtime3 = new TimeSpan();

            TimeSpan dtime8 = new TimeSpan();

            count1 = Int32.Parse(e.Row.Cells[2].Text);
            phone = phone + count1;

            count2 = Int32.Parse(e.Row.Cells[3].Text);
            website = website + count2;

            count3 = Int32.Parse(e.Row.Cells[4].Text);
            mailaway = mailaway + count3;

            count = Int32.Parse(e.Row.Cells[5].Text);
            total = total + count;

            dtime = TimeSpan.Parse(e.Row.Cells[6].Text);
            datetime = datetime.Add(dtime);

            dtime1 = TimeSpan.Parse(e.Row.Cells[7].Text);
            datetime1 = datetime1.Add(dtime1);

            dtime2 = TimeSpan.Parse(e.Row.Cells[8].Text);
            datetime2 = datetime2.Add(dtime2);

            dtime3 = TimeSpan.Parse(e.Row.Cells[9].Text);
            datetime3 = datetime3.Add(dtime3);

            dtime8 = TimeSpan.Parse(e.Row.Cells[10].Text);
            datetime8 = datetime8.Add(dtime8);

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            int day, hour = 0;
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = phone.ToString();
            e.Row.Cells[3].Text = website.ToString();
            e.Row.Cells[4].Text = mailaway.ToString();
            e.Row.Cells[5].Text = total.ToString();

            strday = String.Format("{0:0}", datetime.Days);
            strhour = String.Format("{0:0}", datetime.Hours);
            strmin = String.Format("{0:0}", datetime.Minutes);
            strmsec = String.Format("{0:0}", datetime.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[6].Text = hour.ToString() + ":" + strmin + ":" + strmsec;

            strday = String.Format("{0:0}", datetime1.Days);
            strhour = String.Format("{0:0}", datetime1.Hours);
            strmin = String.Format("{0:0}", datetime1.Minutes);
            strmsec = String.Format("{0:0}", datetime1.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[7].Text = hour.ToString() + ":" + strmin + ":" + strmsec;

            strday = String.Format("{0:0}", datetime2.Days);
            strhour = String.Format("{0:0}", datetime2.Hours);
            strmin = String.Format("{0:0}", datetime2.Minutes);
            strmsec = String.Format("{0:0}", datetime2.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[8].Text = hour.ToString() + ":" + strmin + ":" + strmsec;

            strday = String.Format("{0:0}", datetime3.Days);
            strhour = String.Format("{0:0}", datetime3.Hours);
            strmin = String.Format("{0:0}", datetime3.Minutes);
            strmsec = String.Format("{0:0}", datetime3.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[9].Text = hour.ToString() + ":" + strmin + ":" + strmsec;

            strday = String.Format("{0:0}", datetime8.Days);
            strhour = String.Format("{0:0}", datetime8.Hours);
            strmin = String.Format("{0:0}", datetime8.Minutes);
            strmsec = String.Format("{0:0}", datetime8.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[10].Text = hour.ToString() + ":" + strmin + ":" + strmsec;
        }
    }

    protected void GridutilizationQC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strday, strhour, strmin, strmsec = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /* Calculate Total orders */
            int count4, count5, count6, count7 = 0;
            TimeSpan dtime4 = new TimeSpan();
            TimeSpan dtime5 = new TimeSpan();
            TimeSpan dtime6 = new TimeSpan();
            TimeSpan dtime7 = new TimeSpan();
            TimeSpan dtime9 = new TimeSpan();

            count5 = Int32.Parse(e.Row.Cells[2].Text);
            phone1 = phone1 + count5;

            count6 = Int32.Parse(e.Row.Cells[3].Text);
            website1 = website1 + count6;

            count7 = Int32.Parse(e.Row.Cells[4].Text);
            mailaway1 = mailaway1 + count7;

            count4 = Int32.Parse(e.Row.Cells[5].Text);
            total1 = total1 + count4;

            dtime4 = TimeSpan.Parse(e.Row.Cells[6].Text);
            datetime4 = datetime.Add(dtime4);

            dtime5 = TimeSpan.Parse(e.Row.Cells[7].Text);
            datetime5 = datetime1.Add(dtime5);

            dtime6 = TimeSpan.Parse(e.Row.Cells[8].Text);
            datetime6 = datetime6.Add(dtime6);

            dtime7 = TimeSpan.Parse(e.Row.Cells[9].Text);
            datetime7 = datetime7.Add(dtime7);

            dtime9 = TimeSpan.Parse(e.Row.Cells[10].Text);
            datetime9 = datetime9.Add(dtime9);

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            int day, hour = 0;
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = phone1.ToString();
            e.Row.Cells[3].Text = website1.ToString();
            e.Row.Cells[4].Text = mailaway1.ToString();
            e.Row.Cells[5].Text = total1.ToString();

            strday = String.Format("{0:0}", datetime4.Days);
            strhour = String.Format("{0:0}", datetime4.Hours);
            strmin = String.Format("{0:0}", datetime4.Minutes);
            strmsec = String.Format("{0:0}", datetime4.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[6].Text = hour.ToString() + ":" + strmin + ":" + strmsec;

            strday = String.Format("{0:0}", datetime5.Days);
            strhour = String.Format("{0:0}", datetime5.Hours);
            strmin = String.Format("{0:0}", datetime5.Minutes);
            strmsec = String.Format("{0:0}", datetime5.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[7].Text = hour.ToString() + ":" + strmin + ":" + strmsec;

            strday = String.Format("{0:0}", datetime6.Days);
            strhour = String.Format("{0:0}", datetime6.Hours);
            strmin = String.Format("{0:0}", datetime6.Minutes);
            strmsec = String.Format("{0:0}", datetime6.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[8].Text = hour.ToString() + ":" + strmin + ":" + strmsec;

            strday = String.Format("{0:0}", datetime7.Days);
            strhour = String.Format("{0:0}", datetime7.Hours);
            strmin = String.Format("{0:0}", datetime7.Minutes);
            strmsec = String.Format("{0:0}", datetime7.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[9].Text = hour.ToString() + ":" + strmin + ":" + strmsec;

            strday = String.Format("{0:0}", datetime9.Days);
            strhour = String.Format("{0:0}", datetime9.Hours);
            strmin = String.Format("{0:0}", datetime9.Minutes);
            strmsec = String.Format("{0:0}", datetime9.Seconds);
            day = Convert.ToInt32(strday);
            hour = Convert.ToInt32(strhour);
            hour = (day * 24) + hour;
            e.Row.Cells[10].Text = hour.ToString() + ":" + strmin + ":" + strmsec;
        }
    }

    protected void QualityGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /* Calculate Total orders */
            int count, count1, count2, count3, count4 = 0;

            count1 = Int32.Parse(e.Row.Cells[2].Text);
            qphone = qphone + count1;

            count2 = Int32.Parse(e.Row.Cells[3].Text);
            qwebsite = qwebsite + count2;

            count3 = Int32.Parse(e.Row.Cells[4].Text);
            qmailaway = qmailaway + count3;

            count = Int32.Parse(e.Row.Cells[5].Text);
            qtotal = qtotal + count;

            count4 = Int32.Parse(e.Row.Cells[6].Text);
            error = error + count4;

            /* Calculate Total Quality */
            Rccount = Rccount + 1;
            if (e.Row.Cells[7].Text != "&nbsp;")
            {
                string q1 = e.Row.Cells[7].Text;
                int q2 = Int32.Parse(q1);
                quality = quality + q2;
            }

            e.Row.Cells[7].Text = e.Row.Cells[7].Text + "%";
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = qphone.ToString();
            e.Row.Cells[3].Text = qwebsite.ToString();
            e.Row.Cells[4].Text = qmailaway.ToString();
            e.Row.Cells[5].Text = qtotal.ToString();
            e.Row.Cells[6].Text = error.ToString();

            if (quality != 0)
            {
                decimal totalquality = Math.Ceiling(quality / Rccount);
                e.Row.Cells[7].Text = totalquality.ToString() + "%";
            }
        }
    }

    protected void QualityGridPostAudit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /* Calculate Total orders */
            int count5, count6, count7, count8, count9 = 0;

            count5 = Int32.Parse(e.Row.Cells[2].Text);
            pphone = pphone + count5;

            count6 = Int32.Parse(e.Row.Cells[3].Text);
            pwebsite = pwebsite + count6;

            count7 = Int32.Parse(e.Row.Cells[4].Text);
            pmailaway = pmailaway + count7;

            count8 = Int32.Parse(e.Row.Cells[5].Text);
            ptotal = ptotal + count8;

            count9 = Int32.Parse(e.Row.Cells[6].Text);
            error1 = error1 + count9;

            /* Calculate Total Quality */
            Rccount1 = Rccount1 + 1;
            if (e.Row.Cells[7].Text != "&nbsp;")
            {
                string q1 = e.Row.Cells[7].Text;
                int q2 = Int32.Parse(q1);
                quality1 = quality1 + q2;
            }

            e.Row.Cells[7].Text = e.Row.Cells[7].Text + "%";
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = pphone.ToString();
            e.Row.Cells[3].Text = pwebsite.ToString();
            e.Row.Cells[4].Text = pmailaway.ToString();
            e.Row.Cells[5].Text = ptotal.ToString();
            e.Row.Cells[6].Text = error1.ToString();

            if (quality1 != 0)
            {
                decimal totalquality = Math.Ceiling(quality1 / Rccount1);
                e.Row.Cells[7].Text = totalquality.ToString() + "%";
            }
        }
    }

    protected void Gridutilization_Sorting(object sender, GridViewSortEventArgs e)
    {
        ColumnName(e.SortExpression);
    }
    protected void GridutilizationQC_Sorting(object sender, GridViewSortEventArgs e)
    {
        ColumnName1(e.SortExpression);
    }

    private void ColumnName(string SortColumn)
    {
        string fdate = "", tdate = "";
        DateTime df = gl.ToDate();
        tdate = df.ToString("MM/dd/yyyy");
        //tdate = "05/10/2013";

        if (Lblinfo.Text == "Today Utilization") fdate = tdate;
        else if (Lblinfo.Text == "Last Week Utilization") fdate = df.AddDays(-7).ToString("MM/dd/yyyy");
        else if (Lblinfo.Text == "Last 30days Utilization") fdate = df.AddDays(-30).ToString("MM/dd/yyyy");
        else if (Lblinfo.Text == "Last 90days Utilization") fdate = df.AddDays(-90).ToString("MM/dd/yyyy");
        ds.Dispose();
        ds.Reset();

        ds = gl.QualityReport(fdate, tdate,"");
        if (ds.Tables[0].Rows.Count > 0)
        {
            dataview = new DataView(ds.Tables[0]);
            dataview.Sort = SortColumn + " " + gl.getSDirection();
            Gridutilization.DataSource = dataview;
            Gridutilization.DataBind();
        }
    }

    private void ColumnName1(string SortColumn)
    {
        string fdate = "", tdate = "";
        DateTime df = gl.ToDate();
        tdate = df.ToString("MM/dd/yyyy");
        //tdate = "05/10/2013";

        if (Lblinfo.Text == "Today Utilization") fdate = tdate;
        else if (Lblinfo.Text == "Last Week Utilization") fdate = df.AddDays(-7).ToString("MM/dd/yyyy");
        else if (Lblinfo.Text == "Last 30days Utilization") fdate = df.AddDays(-30).ToString("MM/dd/yyyy");
        else if (Lblinfo.Text == "Last 90days Utilization") fdate = df.AddDays(-90).ToString("MM/dd/yyyy");
        ds.Dispose();
        ds.Reset();
        ds = gl.QualityReport(fdate, tdate, "");
        if (ds.Tables[1].Rows.Count > 0)
        {
            dataview = new DataView(ds.Tables[1]);
            dataview.Sort = SortColumn + " " + gl.getSDirection();
            GridutilizationQC.DataSource = dataview;
            GridutilizationQC.DataBind();
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
   
}
