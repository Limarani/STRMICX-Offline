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
using MySql.Data.MySqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

public partial class Pages_Scraping_Tracking : System.Web.UI.Page
{
    #region Variable Declaration
    myConnection con = new myConnection();
    GlobalClass gblcls = new GlobalClass();
    MySqlDataReader mdr;
    DataSet ds = new DataSet();
    DataSet dset = new DataSet();
    DateTime dt = new DateTime();

    string strfrmdate = string.Empty;
    string strtodate = string.Empty;
    string strusername = string.Empty;
    string strdate = string.Empty;
    string strcheckday = string.Empty;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        if (!Page.IsPostBack)
        {
            txtfrmdate.Text = gblcls.setdate();
            txttodate.Text = gblcls.setdate();
            //CheckPostAudit(sender, e);

            LoadStateCount();
            Sidepanel.Visible = false;
            diveMulti.Visible = false;
            pagedimmer.Visible = false;
            commentsdetails.Visible = false;
        }
        btnpostaudit.Attributes.Add("onclick", "window.open('PostAudit.aspx'); return false;");
        btnabstract.Attributes.Add("onclick", "window.open('ParcelAbstract.aspx'); return false;");
    }

    private void CheckPostAudit(object sender, EventArgs e)
    {
        if (txtfrmdate.Text != "" && txttodate.Text != "")
        {

            dt = DateTime.Now;
            strcheckday = strdate = String.Format("{0:dddd}", dt);
            if (strcheckday != "Monday")
            {
                dt = dt.AddHours(-31);
                strdate = String.Format("{0:MM/dd/yyyy}", dt);
            }
            else
            {
                dt = dt.AddDays(-3);
                strdate = String.Format("{0:MM/dd/yyyy}", dt);
            }


            string result, result1 = "";
            string strquery = "select count(order_no) from record_status where k1=5 and qc=5 and status=5 and (review=0 or review=3 or review=5) and DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strdate + "' and '" + strdate + "' ";
            result = con.ExecuteScalarst(strquery);
            double ordercnt = Convert.ToDouble(Convert.ToDouble(result) * 0.1);
            int count = Convert.ToInt32(ordercnt);
            if (count > 0)
            {
                string strquery1 = "select count(order_no) from record_status where k1=5 and qc=5 and status=5 and (review=3 or review=5) and DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strdate + "' and '" + strdate + "'";
                result1 = con.ExecuteScalarst(strquery1);
                int count1 = Convert.ToInt32(result1);
                if (count <= count1) btnpostaudit.Visible = false;
                else btnpostaudit_Click(sender, e);
            }
        }
    }

    private void LoadStateCount()
    {
        dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);
        dt = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", dt);
        ds.Dispose();
        ds.Reset();
        string query = "Select count(Order_no) as TotalOrders from record_status where State='ME' and county='Cumberland' and K1=0 and QC=0 and Status=0 and Lock1=0 and Pdate between '" + strfrmdate + "' and '" + strtodate + "'";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbltotalcount.Text = ds.Tables[0].Rows[0]["TotalOrders"].ToString();
        }
        else lbltotalcount.Text = "0";
    }
    public DataSet UsernameLoad()
    {
        ds.Dispose();
        ds.Reset();
        string strquery = "select User_Name from user_status order by User_Name";
        ds = con.ExecuteQuery(strquery);
        return ds;
    }


    #region Fetch Orders
    protected void btnordershow_Click(object sender, EventArgs e)
    {
        Session["ViewParcel"] = "";
        try
        {
            if (txtfrmdate.Text != "")
            {
                dt = Convert.ToDateTime(txtfrmdate.Text);
                strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

                if (txttodate.Text != "")
                {
                    dt = Convert.ToDateTime(txttodate.Text);
                    strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                    string strquery = "sp_TrackingScrape";
                    //strusername = ddlusername.SelectedItem.Text;
                    mdr = gblcls.Tracking(strquery, strfrmdate, strtodate, "ALL");
                    ShowGrid(mdr, strfrmdate, strtodate);
                    Sidepanel.Visible = true;
                }
                else
                {
                    errorlabel.Text = "Please Select the To date";
                    Sidepanel.Visible = false;
                }
            }
            else
            {
                errorlabel.Text = "Please Select the From date";
                Sidepanel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    protected void btnutilshow_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtfrmdate.Text != "")
            {
                dt = Convert.ToDateTime(txtfrmdate.Text);
                strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);
                if (txttodate.Text != "")
                {
                    dt = Convert.ToDateTime(txttodate.Text);
                    strtodate = String.Format("{0:MM/dd/yyyy}", dt);
                    mdr = gblcls.Util_Tracking(strfrmdate, strtodate);
                    ShowGridUtil(mdr, strfrmdate, strtodate);
                    Sidepanel.Visible = true;
                }
                else
                {
                    errorlabel.Text = "Please Select the To date";
                    Sidepanel.Visible = false;
                }
            }
            else
            {
                errorlabel.Text = "Please Select the From date";
                Sidepanel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }

    public void ShowGridUtil(MySqlDataReader mdr, string strfrmdate, string strtodate)
    {
        try
        {
            DataView dataview = gblcls.ConvertDataReaderToUtilDataView(mdr);
            DataTable dt = dataview.ToTable();

            if (dt.Rows.Count > 0)
            {
                GridUserUtilization.DataSource = dt;
                GridUserUtilization.DataBind();
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                GridUserUtilization.DataSource = dt;
                GridUserUtilization.DataBind();
                int Totalcolumns = GridUserUtilization.Rows[0].Cells.Count;
                GridUserUtilization.Rows[0].Cells.Clear();
                GridUserUtilization.Rows[0].Cells.Add(new TableCell());
                GridUserUtilization.Rows[0].Cells[0].ColumnSpan = Totalcolumns;
                GridUserUtilization.Rows[0].Cells[0].Text = "No Records Found";
                GridUserUtilization.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                GridUserUtilization.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Middle;
            }

            GridUser.Visible = false;
            GridUserUtilization.Visible = true;
            errorlabel.Visible = false;
            ShowDataViewAll();
            ShowDataView(strfrmdate, strtodate);
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }

    public void ShowGrid(MySqlDataReader mdr, string strfrmdate, string strtodate)
    {
        try
        {
            DataView dataview = gblcls.ConvertDataReaderToDataView(mdr);
            DataTable dt = dataview.ToTable();

            if (dt.Rows.Count > 0)
            {
                GridUser.DataSource = dt;
                GridUser.DataBind();
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                GridUser.DataSource = dt;
                GridUser.DataBind();
                int Totalcolumns = GridUser.Rows[0].Cells.Count;
                GridUser.Rows[0].Cells.Clear();
                GridUser.Rows[0].Cells.Add(new TableCell());
                GridUser.Rows[0].Cells[0].ColumnSpan = Totalcolumns;
                GridUser.Rows[0].Cells[0].Text = "No Records Found";
                GridUser.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                GridUser.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Middle;
            }

            GridUser.Visible = true;
            GridUserUtilization.Visible = false;
            errorlabel.Visible = false;
            ShowDataViewAll();
            ShowDataView(strfrmdate, strtodate);
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    public void ShowDataViewAll()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            //string strquery = "select (select count(order_no) from record_status where K1='1' or QC='1') as Working, (select count(order_no) from record_status where K1='2' and QC='2' and Status='2' and Tax ='3' and Parcel='0' and Pend='0') as Mailaway,(select count(order_no) from record_status where K1='2' and QC='2' and Status='2' and Tax ='0' and Parcel='0' and Pend='3') as Inprocess,(Select count(order_no) from record_status where K1='2' and QC='2' and Status='2' and Tax ='0' and Parcel='3' and Pend='0') as ParcelID, (Select count(order_no) from record_status where K1='4' and QC='4' and Status='4' and Tax ='0' and Parcel='0' and Pend='0') as Onhold,(select count(order_no) from record_status where (K1='2' or K1='4') and (QC='2' or QC='4') and (Status='2' or Status='4') or (Tax ='3' or Parcel='3' or Pend='3')) as Total";
            //ds = con.ExecuteQuery(strquery);
            ds = gblcls.GetOrderCountAll_scrape();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DetailsView1.DataSource = ds;
                DetailsView1.DataBind();
            }
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    public void ShowDataView(string strfrmdate, string strtodate)
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            //string strquery = "select(select count(order_no) from record_status where pdate between '" + strfrmdate + "' and '" + strtodate + "' )as Total,(select count(order_no) from record_status where K1='0' and QC='0' and Status='0' and Tax ='0' and Parcel='0' and Pend='0' and pdate between '" + strfrmdate + "' and '" + strtodate + "' ) as YTS,(select count(order_no) from record_status where (K1='1' or QC='1') and pdate between '" + strfrmdate + "' and '" + strtodate + "' ) as Working,(select count(order_no) from record_status where K1='2' and QC='0' and Status='2' and key_status<>'Others' and Tax ='0' and Parcel='0' and Pend='0' and pdate between '" + strfrmdate + "' and '" + strtodate + "' ) as KeyCompleted,(select count(order_no) from record_status where K1='5' and QC='5' and Status='5' and pdate between '" + strfrmdate + "' and '" + strtodate + "') as Completed,(select count(order_no) from record_status where K1='5' and QC='5' and Status='5' and pdate between '" + strfrmdate + "' and '" + strtodate + "') as Completed,(select count(order_no) from record_status where K1='5' and QC='5' and Status='5' and DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strfrmdate + "' and '" + strtodate + "') as CompletedAll,(select count(order_no) from record_status where K1='2' and QC='2' and Status='2' and Tax ='3' and Parcel='0' and Pend='0' and pdate between '" + strfrmdate + "' and '" + strtodate + "' ) as Mainaway,(select count(order_no) from record_status where K1='2' and QC='2' and Status='2' and Tax ='0' and Parcel='0' and Pend='3' and pdate between '" + strfrmdate + "' and '" + strtodate + "') as Inprocess,(Select count(order_no) from record_status where K1='2' and QC='2' and Status='2' and Tax ='0' and Parcel='3' and Pend='0' and pdate between '" + strfrmdate + "' and '" + strtodate + "') as ParcelID,(select count(order_no) from record_status where K1='4' and QC='4' and Status='4' and Tax ='0' and Parcel='0' and Pend='0' and pdate between '" + strfrmdate + "' and '" + strtodate + "') as OnHold,(select count(order_no) from record_status where K1='7' and QC='7' and Status='7' and Tax ='0' and Parcel='0' and Pend='0' and pdate between '" + strfrmdate + "' and '" + strtodate + "') as Rejected,(select count(order_no) from record_status where K1='2' and QC='0' and Status='2' and key_status='Others' and Tax ='0' and Parcel='0' and Pend='0' and pdate between '" + strfrmdate + "' and '" + strtodate + "' ) as Others,(Select count(order_no) from record_status where K1='5' and QC='5' and Status='5' and (Review='3' or Review='5') and Tax ='0' and Parcel='0' and Pend='0' and DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strfrmdate + "' and '" + strtodate + "') as PostAudit";
            //ds = con.ExecuteQuery(strquery);
            ds = gblcls.GetOrderCount_scrape(strfrmdate, strtodate);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DetailsView2.DataSource = ds;
                DetailsView2.DataBind();
            }
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    #endregion

    #region Search Order
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        string strordersearch = "";
        try
        {
            if (txtordersearch.Text != "")
            {
                ds.Dispose();
                ds.Reset();
                dset.Dispose();
                dset.Reset();
                strordersearch = txtordersearch.Text;
                ds = gblcls.TrackingSearch("%" + strordersearch + "%");
                DataTable dttt = ds.Tables[0].Copy();
                DataTable dtt = ds.Tables[1].Copy();
                DataTable dt3 = ds.Tables[2].Copy();
                dtt.Merge(dttt);
                dtt.Merge(dt3);
                dset.Tables.Add(dtt);
                ShowSearchGrid(dset);
            }
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }

    public void ShowSearchGrid(DataSet ds)
    {
        try
        {
            DataView dataview = gblcls.ConvertDataSetToDataView(ds);
            DataTable dt = dataview.ToTable();

            if (dt.Rows.Count > 0)
            {
                GridUser.DataSource = dt;
                GridUser.DataBind();
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                GridUser.DataSource = dt;
                GridUser.DataBind();
                int Totalcolumns = GridUser.Rows[0].Cells.Count;
                GridUser.Rows[0].Cells.Clear();
                GridUser.Rows[0].Cells.Add(new TableCell());
                GridUser.Rows[0].Cells[0].ColumnSpan = Totalcolumns;
                GridUser.Rows[0].Cells[0].Text = "No Records Found";
                GridUser.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                GridUser.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Middle;
            }

            GridUser.Visible = true;
            GridUserUtilization.Visible = false;
            errorlabel.Visible = false;
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    #endregion

    #region Load Day Wise Count

    protected void ftlnkbtntotal_Click(object sender, EventArgs e)
    {
        Session["ViewParcel"] = "";
        LoadDaywiseCount("ALL");
    }
    protected void ftlnkbtnscrapcomp_Click(object sender, EventArgs e)
    {
        Session["ViewParcel"] = "";
        LoadDaywiseCount("ScrapingCompleted");
    }
    protected void ftlnkbtnyts_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("STARSYTS");
    }
    protected void ftlnkbtnerror_Click(object sender, EventArgs e)
    {
        Session["ViewParcel"] = "";
        LoadDaywiseCount("SCRAPINGERROR");
    }

    protected void ftlbkmanual_Click(object sender, EventArgs e)
    {       
        LoadDaywiseCount("MANUAL");
    }
    protected void ftlnkbtnmulti_Click(object sender, EventArgs e)
    {
        Session["ViewParcel"] = "Yes";
        LoadDaywiseCount("MULTIPARCEL");
    }
    protected void ftlnkbtnmnway_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("MAILAWAY");
    }
    protected void ftlnkbtninproc_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("INPROCESS");
    }
    protected void ftlnkbtnwrkng_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("WORKING");
    }
    protected void ftlnkbtnpid_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("PARCELID");
    }
    protected void ftlnkbtnonhold_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("ONHOLD");
    }
    protected void ftlnkbtnrej_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("REJECTED");
    }
    protected void ftlnkbtnkeycmd_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("KEYCOMPLETED");
    }
    protected void ftlnkbtncmd_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("COMPLETED");
    }
    protected void ftlnkbtnothers_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("OTHERS");
    }
    protected void lnkbtnpostaudit_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("POSTAUDIT");
    }
    protected void lnkbtnmissing_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("ORDERMISSING");
    }
    protected void lnkbtnhp_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("HP");
    }
    protected void lnkbtnfollowup_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("FOLLOWUP");
    }
    protected void lnkbtnclosedate_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("CLOSEDATE");
    }

    private void LoadDaywiseCount(string status)
    {
        //madesh

        GridUser.Visible = false;
        GridUserUtilization.Visible = false;
        Sidepanel.Visible = true;
        errorlabel.Visible = true;
        try
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);
            dt = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", dt);
            ds.Dispose();
            ds.Reset();
            //string strquery = "sp_TrackingScrape";
            string strquery = "sp_TrackingScrape";
            //strusername = ddlusername.SelectedItem.Text;
            mdr = gblcls.Tracking(strquery, strfrmdate, strtodate, status);
            ShowGrid(mdr, strfrmdate, strtodate);
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    #endregion

    #region Details View All Records Bind Grid 
    //New
    //protected void lnkbtnKeytotal_Click(object sender, EventArgs e)
    //{
    //    LoadDaywiseCount("KEYCOMPLETEDALL");
    //}
    //New

    protected void lnkbtnmnway_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("MAILAWAYALL");
    }
    protected void lnkbtninproc_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("INPROCESSALL");
    }
    protected void lnkbtnwrkng_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("WORKINGALL");
    }
    protected void lnkbtnpid_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("PARCELIDALL");
    }
    protected void lnkbtnonhold_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("ONHOLDALL");
    }
    protected void lnkbtncmdall_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("COMPLETEDALL");
    }
    protected void lnkbtntotal_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("TOTALALL");
    }

    #endregion

    #region Grid Events
    protected void GridUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Process")
        {
            GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            Session["id"] = GridUser.DataKeys[gvr.RowIndex].Values[0].ToString();
            string id = GridUser.DataKeys[gvr.RowIndex].Values[0].ToString();
            string query = "select order_no, scrape, scrape_status from record_status where id ='" + Session["id"].ToString() + "'";
            ds = con.ExecuteQuery(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["ono"] = ds.Tables[0].Rows[0]["order_no"].ToString();
                string autorecord = ds.Tables[0].Rows[0]["scrape"].ToString();
                string scrape_status = ds.Tables[0].Rows[0]["scrape_status"].ToString();
                if (autorecord == "1" && scrape_status == "0")
                {
                    pagedimmer.Visible = true;
                    ManualReason.Visible = true;
                }

                else
                {
                    Session["TimePro"] = DateTime.Now;
                    if (SessionHandler.AuditQA == "1") Response.Redirect("~/Pages/ProductionAuditNew.aspx?id=" + id);
                    else Response.Redirect("ProductionNew.aspx?id=" + id);
                }

            }


        }
        if (e.CommandName == "DateProcess")
        {
            GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string id = GridUser.DataKeys[gvr.RowIndex].Values[0].ToString();
            ds.Dispose();
            ds.Reset();
            string query = "Select Comments_Det from record_status where id='" + id + "'";
            ds = con.ExecuteQuery(query);
            txtstatecomments.Text = ds.Tables[0].Rows[0]["Comments_Det"].ToString();
            pagedimmer.Visible = true;
            commentsdetails.Visible = true;
        }



        //if (e.CommandName == "Process")
        //{
        //    GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
        //    string id = GridUser.DataKeys[gvr.RowIndex].Values[0].ToString();
        //    Session["TimePro"] = DateTime.Now;
        //    if (SessionHandler.AuditQA == "1") Response.Redirect("~/Pages/ProductionAuditNew.aspx?id=" + id);
        //    else Response.Redirect("ProductionNew.aspx?id=" + id);
        //}
        //if (e.CommandName == "DateProcess")
        //{
        //    GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
        //    string id = GridUser.DataKeys[gvr.RowIndex].Values[0].ToString();
        //    ds.Dispose();
        //    ds.Reset();
        //    string query = "Select Comments_Det from record_status where id='" + id + "'";
        //    ds = con.ExecuteQuery(query);
        //    txtstatecomments.Text = ds.Tables[0].Rows[0]["Comments_Det"].ToString();
        //    pagedimmer.Visible = true;
        //    commentsdetails.Visible = true;
        //}
    }
    protected void btnlogoutclose_Click(object sender, EventArgs e)
    {
        pagedimmer.Visible = false;
        ManualReason.Visible = false;
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        string strlogout = "";
        strlogout = txtlogreason.Text;
        gblcls.insert_manual_reason(strlogout, Session["ono"].ToString());
        RRedirect();
    }
    private void RRedirect()
    {
        if (SessionHandler.AuditQA == "1") Response.Redirect("~/Pages/ProductionAuditNew.aspx?id=" + Session["id"].ToString());
        else Response.Redirect("ProductionNew.aspx?id=" + Session["id"].ToString());
    }
    protected void GridUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {



        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Pager)
        {
            Image imglocked = e.Row.FindControl("Imglocked") as Image;
            CheckBox chkViewPar = e.Row.FindControl("chkViewParcel") as CheckBox;
            if (Session["ViewParcel"] != null)
            {
                if (Session["ViewParcel"].ToString() == "Yes")
                {
                    if (chkViewPar != null)
                    {
                        chkViewPar.Visible = true;
                    }
                }
            }
            if (e.Row.Cells[13].Text == "Delivered")
            {
                e.Row.Cells[13].BackColor = System.Drawing.Color.FromArgb(163, 186, 71);
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Black;
                imglocked.Visible = false;
                e.Row.Cells[1].Enabled = false;

            }
            if (e.Row.Cells[13].Text == "Rejected")
            {
                e.Row.Cells[13].BackColor = System.Drawing.Color.SandyBrown;
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Black;
                imglocked.Visible = false;
            }
            if (e.Row.Cells[13].Text == "On Hold")
            {
                e.Row.Cells[13].BackColor = System.Drawing.Color.SkyBlue;
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Black;
                imglocked.Visible = false;
            }
            if (e.Row.Cells[13].Text == "Mail Away")
            {
                e.Row.Cells[13].BackColor = System.Drawing.Color.FromArgb(170, 255, 212);
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Black;
                imglocked.Visible = false;
            }
            if (e.Row.Cells[13].Text == "ParcelID")
            {
                e.Row.Cells[13].BackColor = System.Drawing.Color.FromArgb(250, 250, 142);
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Black;
                imglocked.Visible = false;
            }
            if (e.Row.Cells[13].Text == "In Process")
            {
                e.Row.Cells[13].BackColor = System.Drawing.Color.FromArgb(250, 200, 211);
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Black;
                imglocked.Visible = false;
            }
            if (e.Row.Cells[13].Text == "YTS" || e.Row.Cells[13].Text == "Key Completed" || e.Row.Cells[13].Text == "Others" || e.Row.Cells[13].Text == "Order Missing")
            {
                imglocked.Visible = false;
            }
            if ((e.Row.Cells[13].Text == "Key Started") || (e.Row.Cells[13].Text == "QC Started") || (e.Row.Cells[13].Text == "In Process Started") || (e.Row.Cells[13].Text == "Mail Away Started") || (e.Row.Cells[13].Text == "ParcelID Started"))
            {
                e.Row.Cells[13].BackColor = System.Drawing.Color.FromArgb(96, 219, 207);
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Black;
                imglocked.Visible = true;
            }
            if ((e.Row.Cells[13].Text == "Key Started") || (e.Row.Cells[13].Text == "ParcelID Started") || (e.Row.Cells[13].Text == "Mail Away Started") || (e.Row.Cells[13].Text == "In Process Started"))
            {
                string StartTime, EndTime = "";
                string State, County = "";
                StartTime = e.Row.Cells[16].Text;
                EndTime = DateTime.Now.ToString();
                State = e.Row.Cells[8].Text;
                County = e.Row.Cells[9].Text;
                if (StartTime == "&nbsp;") { return; }
                DateTime startTime = DateTime.Parse(StartTime);
                DateTime endTime = DateTime.Parse(EndTime);
                TimeSpan ts = endTime.Subtract(startTime);
                string time = ts.ToString();
                time = time.Replace(".", "");
                int TAT = int.Parse(time.Replace(":", ""));
                string query = "Select sf_getordertype('" + State + "','" + County + "')";
                string result = con.ExecuteScalar(query);
                if (result == "Website" || result == "Phone/ Website")
                {
                    if (TAT >= 000000 && TAT <= 000300) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    else if (TAT > 000300 && TAT <= 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 102);
                    else if (TAT > 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
                }
                else if (result == "Phone")
                {
                    if (TAT >= 000000 && TAT <= 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    else if (TAT > 000500 && TAT <= 000900) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 102);
                    else if (TAT > 000900) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
                }
            }
            if (e.Row.Cells[11].Text == "QC Started")
            {
                string StartTime, EndTime = "";
                string State, County = "";
                StartTime = e.Row.Cells[20].Text;
                EndTime = DateTime.Now.ToString();
                State = e.Row.Cells[8].Text;
                County = e.Row.Cells[9].Text;
                if (StartTime == "&nbsp;") { return; }
                DateTime startTime = DateTime.Parse(StartTime);
                DateTime endTime = DateTime.Parse(EndTime);
                TimeSpan ts = endTime.Subtract(startTime);
                string time = ts.ToString();
                time = time.Replace(".", "");
                int TAT = int.Parse(time.Replace(":", ""));
                string query = "Select sf_getordertype('" + State + "','" + County + "')";
                string result = con.ExecuteScalar(query);
                if (result == "Website" || result == "Phone/ Website")
                {
                    if (TAT >= 000000 && TAT <= 000200) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    else if (TAT > 000200 && TAT <= 000300) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 102);
                    else if (TAT > 000300) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
                }
                else if (result == "Phone")
                {
                    if (TAT >= 000000 && TAT <= 000300) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    else if (TAT > 000300 && TAT <= 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 102);
                    else if (TAT > 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
                }
            }
        }
    }
    #endregion

    #region Export
    protected void Lnkexport_Click(object sender, ImageClickEventArgs e)
    {
        if (GridUser.Visible == true)
        {
            if (GridUser.Rows.Count > 0)
            {
                GridViewExportUtil.Export("Tracking.xls", this.GridUser);
            }
        }
        else if (GridUserUtilization.Visible == true)
        {
            if (GridUserUtilization.Rows.Count > 0)
            {
                GridViewExportUtil.Export("UserUtilization.xls", this.GridUserUtilization);
            }
        }
    }
    #endregion

    #region Post Audit

    protected void btnpostaudit_Click(object sender, EventArgs e)
    {
        dt = DateTime.Now;
        dt = dt.AddHours(-31);
        strdate = String.Format("{0:MM/dd/yyyy}", dt);

        dt = DateTime.Now;
        strcheckday = strdate = String.Format("{0:dddd}", dt);
        if (strcheckday != "Monday")
        {
            dt = dt.AddHours(-31);
            strdate = String.Format("{0:MM/dd/yyyy}", dt);
        }
        else
        {
            dt = dt.AddDays(-3);
            strdate = String.Format("{0:MM/dd/yyyy}", dt);
        }

        string result, result1 = "";
        ds = UsernameLoad();
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strusername = ds.Tables[0].Rows[i]["User_Name"].ToString();
                string strquery = "select count(order_no) from record_status where k1=5 and qc=5 and status=5 and review=0 and ((K1_OP='" + strusername + "' and QC_OP='-')  or QC_OP='" + strusername + "') and DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strdate + "' and '" + strdate + "' ";
                result = con.ExecuteScalarst(strquery);
                double ordercnt = Convert.ToDouble(Convert.ToDouble(result) * 0.1);
                int count = Convert.ToInt32(ordercnt);
                if (count > 0)
                {
                    string strquery1 = "select count(order_no) from record_status where k1=5 and qc=5 and status=5 and (review=3 or review=5) and ((K1_OP='" + strusername + "' and QC_OP='-')  or QC_OP='" + strusername + "') and DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strdate + "' and '" + strdate + "'";
                    result1 = con.ExecuteScalarst(strquery1);
                    int count1 = Convert.ToInt32(result1);
                    if (count > count1)
                    {
                        string query = "Update record_status set review=3 where k1=5 and qc=5 and status=5 and review=0 and ((K1_OP='" + strusername + "' and QC_OP='-')  or QC_OP='" + strusername + "') and DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strdate + "' and '" + strdate + "' order by Pdate desc,rand() limit " + (count - count1) + "";
                        con.ExecuteSPNonQuery(query);
                    }
                }
            }
        }

        dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);
        dt = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", dt);

        CheckPostAudit(sender, e);
        ShowDataView(strfrmdate, strtodate);
    }
    #endregion

    protected void btnclose_Click(object sender, EventArgs e)
    {
        pagedimmer.Visible = false;
        commentsdetails.Visible = false;
    }

    #region Auto Refresh
    protected void Chkrefresh_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkrefresh.Checked == true) Refresh.Enabled = true;
        else Refresh.Enabled = false;
    }
    protected void Refresh_Tick(object sender, EventArgs e)
    {
        btnordershow_Click(sender, e);
    }
    #endregion
    protected void chkViewParcel_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            foreach (GridViewRow gvrow in GridUser.Rows)
            {
                CheckBox chk = gvrow.Cells[0].Controls[0].FindControl("chkViewParcel") as CheckBox;

                if (chk.Checked)
                {
                    LinkButton lnkBtn = gvrow.Cells[1].Controls[0].FindControl("Lnkorder") as LinkButton;
                    string orderNumber = lnkBtn.Text;
                    string state = gvrow.Cells[8].Text;
                    string county = gvrow.Cells[9].Text;
                    Session["statemulti"] = state;
                    Session["countymulti"] = county;
                    DataTable dt = gblcls.MultiGridDisplay(orderNumber, state, county);
                    GridMulti.DataSource = dt;
                    GridMulti.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        //open amrock order details page in new tab.....
                        string urlNo = "";
                        if(orderNumber.Contains("_U"))
                        {
                            urlNo = Before(orderNumber, "_");
                        }
                        else
                        {
                            urlNo = orderNumber;
                        }
                        string url = "https://portal.amrock.com/Vendor/Tax/OrderDetails.aspx?oid=" + urlNo;
                        ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('" + url + "','_blank');", true);
                       
                        chk.Checked = false;
                        chk.Enabled = false;
                        diveMulti.Visible = true;
                        GridMulti.Visible = true;
                    }
                }
            }
        }
        catch
        {

        }
    }
    public static string Before(string value, string a)
    {
        int posA = value.IndexOf(a);
        if (posA == -1)
        {
            return "";
        }
        return value.Substring(0, posA);
    }
    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
        CheckBox chkBxmulti = (CheckBox)row.FindControl("chkselect");
        string orderNumber = row.Cells[1].Text.ToString().Trim();
        string parcelNumber = row.Cells[2].Text.ToString().Trim();
        row.BackColor = System.Drawing.Color.LightGreen;
        string apiURL = "", data = "";
        if (Session["statemulti"] != null && Session["countymulti"] != null)
        {
            DataSet ds = gblcls.GetCountyId(Session["statemulti"].ToString(), Session["countymulti"].ToString());
            apiURL = ds.Tables[0].Rows[0]["service_url"].ToString();
        }
        if (Session["statemulti"] != null && Session["countymulti"] != null)
        {
            string team = gblcls.ReturnTeam(Session["statemulti"].ToString(), Session["countymulti"].ToString());
            if (team == "Internal")
            {
                object input = new
                {
                    address = "",
                    houseno = "",
                    sname = "",
                    sttype = "",
                    parcelNumber = parcelNumber,
                    searchType = "parcel",
                    orderNumber = orderNumber,
                    ownername = "",
                    directParcel = "",
                    account = "",
                    direction = "",
                    unitNumber = "",
                    assessmentID = "",
                    city = "",
                    state = "",
                    county = ""
                };
                data = ScrapOrder(apiURL, input);
                if (data.Contains("Data Inserted Successfully") || data.Contains("Timeout"))
                {                  
                    string taxquery = "update record_status set scrape_status = 2,orderstaus='M' where order_no = '" + orderNumber + "'";
                    int result1 = gblcls.ExecuteSPNonQuery(taxquery);

                }
                else 
                {               
                    string taxquery = "update record_status set scrape_status = 3,orderstaus='M' where order_no = '" + orderNumber + "'";
                    int result1 = gblcls.ExecuteSPNonQuery(taxquery);
                }
                diveMulti.Visible = false;
            }
            else if (team == "External")
            {
                object input = new
                {
                    Address = "",
                    StreetNumber = "",
                    HouseNumberFrom = "",
                    HouseNumberTo = "",
                    AssessorNumber = "",
                    AlternateID = "",
                    PPIN = "",
                    StreetName = "",
                    OwnerName = "",
                    OwnerLastName = "",
                    OwnerFirstName = "",
                    ParcelNumber = parcelNumber,
                    City = "",
                    Zipcode = "",
                    DistrictCode = "",
                    AccountNumber = "",
                    Direction = "",
                    StreetType = "",
                    SubDivision = "",
                    UnitNumber = "",
                    Folio = "",
                    TaxNumber = "",
                    OrganizationName = "",
                    County = "",
                    CountyID = "",
                    OrderID = orderNumber,
                    titleflexSearchId = ""
                };
                data = ScrapOrder(apiURL, input);
                if (data.Contains("Success") || data.Contains("Timeout"))
                {
                    string taxquery = "update record_status set scrape_status = 2,orderstaus='M' where order_no = '" + orderNumber + "'";
                    int result1 = gblcls.ExecuteSPNonQuery(taxquery);
                }
                else
                {
                    string taxquery = "update record_status set scrape_status = 3,orderstaus='M' where order_no = '" + orderNumber + "'";
                    int result1 = gblcls.ExecuteSPNonQuery(taxquery);
                }
                diveMulti.Visible = false;
            }
            if (data.Contains("Success") || data.Contains("Timeout"))
            {
                string taxquery = "update record_status set scrape_status = 2,orderstaus='M' where order_no = '" + orderNumber + "'";
                int result1 = gblcls.ExecuteSPNonQuery(taxquery);
            }
            else
            {
                string taxquery = "update record_status set scrape_status = 3,orderstaus='M' where order_no = '" + orderNumber + "'";
                int result1 = gblcls.ExecuteSPNonQuery(taxquery);
            }
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        diveMulti.Visible = false;
        GridMulti.Visible = false;
    }
    public string ScrapOrder(string apiUrl, object inputObj)
    {

        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsJsonAsync(apiUrl, inputObj).Result;
            // response.EnsureSuccessStatusCode();                        
            var result = response.Content.ReadAsStringAsync().Result;
            var s = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "An error has occured";
            }
            else
            {
                return s.ToString();
            }
        }
        catch
        {
            return "Timeout";
        }
    }
}