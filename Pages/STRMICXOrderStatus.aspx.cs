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
using System.Text.RegularExpressions;
//madesh 08/01/2019....
public partial class Pages_STRMICXOrderStatus : System.Web.UI.Page
{

    #region Variable Declaration
    myConnection con = new myConnection();
    GlobalClass gblcls = new GlobalClass();
    MySqlDataReader mdr;
    DataSet ds = new DataSet();
    DataSet dset = new DataSet();
    DateTime dt = new DateTime();
    DataTable dttest = new DataTable();

    string strfrmdate = string.Empty;
    string strtodate = string.Empty;
    string strusername = string.Empty;
    string strdate = string.Empty;
    string strcheckday = string.Empty;
    string id = string.Empty;
    string ono = string.Empty;
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("STRMICXLogin.aspx");
        if (!Page.IsPostBack)
        {
            this.Title = "ORDER TRACKING";
            txtfrmdate.Text = gblcls.setdate();
            txttodate.Text = gblcls.setdate();
            LoadStateCount();
            fetchallusername();
            fetchallstatename();
            //Sidepanel.Visible = false;
            PanelReset.Visible = false;
            Counts.Visible = false;

            pagedimmer.Visible = false;
            commentsdetails.Visible = false;
        }
        //btnpostaudit.Attributes.Add("onclick", "window.open('PostAudit.aspx'); return false;");
        //btnabstract.Attributes.Add("onclick", "window.open('ParcelAbstract.aspx'); return false;"); 
    }

    private void fetchallusername()
    {
        string query = "";
        query = "select Id,User_Name from user_status";
        DataSet ds = gblcls.ExecuteQuery(query);
        Usernamelist.DataSource = ds.Tables[0];
        Usernamelist.DataTextField = "User_Name";
        Usernamelist.DataValueField = "User_Name";
        Usernamelist.DataBind();
        Usernamelist.Items.Insert(0, new ListItem("--Select--", "NA"));
    }

    private void fetchallstatename()
    {
        string query = "";
        query = "select id,state from timezone order by state";
        DataSet ds = gblcls.ExecuteQuery(query);
        Statelist.DataSource = ds.Tables[0];
        Statelist.DataTextField = "state";
        Statelist.DataValueField = "state";
        Statelist.DataBind();
        Statelist.Items.Insert(0, new ListItem("--Select--", "NA"));
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
                if (count <= count1)
                {
                    //btnpostaudit.Visible = false;
                }
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
            //lbltotalcount.Text = ds.Tables[0].Rows[0]["TotalOrders"].ToString();
        }
        else
        {
        }//lbltotalcount.Text = "0";
    }
    public DataSet UsernameLoad()
    {
        ds.Dispose();
        ds.Reset();
        string strquery = "select User_Name from user_status order by User_Name";
        ds = con.ExecuteQuery(strquery);
        return ds;
    }
    #endregion

    //amrock
    #region Fetch Orders
    protected void btnordershow_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtfrmdate.Text != "")
            {
                dt = Convert.ToDateTime(txtfrmdate.Text);
                strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

                DataTable dttracking = new DataTable();
                if (txttodate.Text != "")
                {
                    dt = Convert.ToDateTime(txttodate.Text);
                    strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                    if (strfrmdate != "" && strtodate != "" && Statelist.Text == "NA" && Usernamelist.Text == "NA")
                    {
                        dttracking = gblcls.FetchTrackingDetails(strfrmdate, strtodate, "", "", "ALL");
                        GridUser.DataSource = dttracking;
                        GridUser.DataBind();
                        showoverallcount();
                        //mdr = gblcls.Tracking(strquery, strfrmdate, strtodate, "ALL"); 
                        //ShowGrid(mdr, strfrmdate, strtodate);
                        //ShowDataViewAll(); 
                        //Sidepanel.Visible = true;
                        PanelReset.Visible = true;
                        Counts.Visible = true;
                    }
                    else if (strfrmdate != "" && strtodate != "" && Statelist.Text != "NA" && Usernamelist.Text == "NA")
                    {
                        dttracking = gblcls.FetchTrackingDetails(strfrmdate, strtodate, Statelist.Text, "", "Statewise");
                        GridUser.DataSource = dttracking;
                        GridUser.DataBind();
                        showoverallcount();
                        //mdr = gblcls.Tracking(strquery, strfrmdate, strtodate, "ALL"); 
                        //ShowGrid(mdr, strfrmdate, strtodate);
                        //ShowDataViewAll(); 
                        //Sidepanel.Visible = true;
                        PanelReset.Visible = true;
                        Counts.Visible = true;
                    }
                    else if (strfrmdate != "" && strtodate != "" && Statelist.Text == "NA" && Usernamelist.Text != "NA")
                    {
                        dttracking = gblcls.FetchTrackingDetails(strfrmdate, strtodate, "", Usernamelist.Text, "UserNamewise");
                        GridUser.DataSource = dttracking;
                        GridUser.DataBind();
                        showoverallcount();
                        //mdr = gblcls.Tracking(strquery, strfrmdate, strtodate, "ALL"); 
                        //ShowGrid(mdr, strfrmdate, strtodate);
                        //ShowDataViewAll(); 
                        //Sidepanel.Visible = true;
                        PanelReset.Visible = true;
                        Counts.Visible = true;
                    }
                    else if (strfrmdate != "" && strtodate != "" && Statelist.Text != "NA" && Usernamelist.Text != "NA")
                    {
                        dttracking = gblcls.FetchTrackingDetails(strfrmdate, strtodate, Statelist.Text, Usernamelist.Text, "overall");
                        GridUser.DataSource = dttracking;
                        GridUser.DataBind();
                        showoverallcount();
                        //mdr = gblcls.Tracking(strquery, strfrmdate, strtodate, "ALL"); 
                        //ShowGrid(mdr, strfrmdate, strtodate);
                        //ShowDataViewAll(); 
                        //Sidepanel.Visible = true;
                        PanelReset.Visible = true;
                        Counts.Visible = true;
                    }
                }
                else
                {
                    errorlabel.Text = "Please Select the To date";
                    //Sidepanel.Visible = false;
                    PanelReset.Visible = false;
                    Counts.Visible = true;
                }
            }
            else
            {
                errorlabel.Text = "Please Select the From date";
                //Sidepanel.Visible = false;
                PanelReset.Visible = false;
                Counts.Visible = true;
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
                    //Sidepanel.Visible = true;
                }
                else
                {
                    errorlabel.Text = "Please Select the To date";
                    //Sidepanel.Visible = false;
                }
            }
            else
            {
                errorlabel.Text = "Please Select the From date";
                //Sidepanel.Visible = false;
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
                GridUserUtilization.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
            //DataView dataview = gblcls.ConvertDataReaderToDataView(mdr);
            DataView dataview = gblcls.ConvertDataSetToDataViewSample1(mdr);
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
                GridUser.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
            ds = gblcls.GetOrderCountAll();
            if (ds.Tables[0].Rows.Count > 0)
            {
                //DetailsView1.DataSource = ds;
                //DetailsView1.DataBind();
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
            ds = gblcls.GetOrderCount(strfrmdate, strtodate);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //DetailsView2.DataSource = ds;
                //DetailsView2.DataBind();
            }
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    #endregion

    //amrock
    protected void LnkId_Click(object sender, EventArgs e)
    {

    }




    #region Search Order
    //protected void btnsearch_Click(object sender, EventArgs e)
    //{
    //    string strordersearch = "";
    //    try
    //    {
    //        if (txtordersearch.Text != "")
    //        {
    //            ds.Dispose();
    //            ds.Reset();
    //            dset.Dispose();
    //            dset.Reset();
    //            strordersearch = txtordersearch.Text;
    //            ds = gblcls.FetchOrderDetails(strordersearch);
    //            DataTable dttt = ds.Tables[0].Copy();
    //            //DataTable dtt = ds.Tables[1].Copy();
    //            //DataTable dt3 = ds.Tables[2].Copy();
    //            //dtt.Merge(dttt);
    //            //dtt.Merge(dt3);
    //            dset.Tables.Add(dttt);
    //            ShowSearchGrid(dset);      
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errorlabel.Text = ex.ToString();
    //    }
    //}

    public void ShowSearchGrid(DataSet ds)
    {
        try
        {
            DataView dataview = gblcls.ConvertDataSetToDataViewSample(ds);
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
                GridUser.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
        LoadDaywiseCount("ALL");
    }
    protected void ftlnkbtnyts_Click(object sender, EventArgs e)
    {
        LoadDaywiseCount("YTS");
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
        GridUser.Visible = false;
        GridUserUtilization.Visible = false;
        //Sidepanel.Visible = true;
        errorlabel.Visible = true;
        try
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);
            dt = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", dt);
            ds.Dispose();
            ds.Reset();
            //string strquery = "sp_Tracking";
            string strquery = "sp_TrackingNew";
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

    protected void wiporders_Click(object sender, EventArgs e)
    {
        DataTable wiporders = new DataTable();
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    wiporders = gblcls.FetchTrackingDetails(strfrmdate, strtodate, "", "", "WIP");
                    GridUser.DataSource = wiporders;
                    GridUser.DataBind();
                    showoverallcount();
                    PanelReset.Visible = true;
                    Counts.Visible = true;
                }
            }
        }
    }

    protected void ytsorders_Click(object sender, EventArgs e)
    {
        DataTable yts = new DataTable();
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    yts = gblcls.FetchTrackingDetails(strfrmdate, strtodate, "", "", "YTS");
                    GridUser.DataSource = yts;
                    GridUser.DataBind();
                    showoverallcount();
                    PanelReset.Visible = true;
                    Counts.Visible = true;
                }
            }
        }
    }

    protected void mailwayorders_Click(object sender, EventArgs e)
    {
        DataTable mailwayorders = new DataTable();
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    mailwayorders = gblcls.FetchTrackingDetails(strfrmdate, strtodate, "", "", "Mailaway");
                    GridUser.DataSource = mailwayorders;
                    GridUser.DataBind();
                    showoverallcount();
                    PanelReset.Visible = true;
                    Counts.Visible = true;
                }
            }
        }
    }

    protected void inprocessorders_Click(object sender, EventArgs e)
    {
        DataTable inprocessorders = new DataTable();
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    inprocessorders = gblcls.FetchTrackingDetails(strfrmdate, strtodate, "", "", "Inprocess");
                    GridUser.DataSource = inprocessorders;
                    GridUser.DataBind();
                    showoverallcount();
                    PanelReset.Visible = true;
                    Counts.Visible = true;
                }
            }
        }
    }

    protected void deliveredorders_Click(object sender, EventArgs e)
    {
        DataTable deliveredorders = new DataTable();
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    deliveredorders = gblcls.FetchTrackingDetails(strfrmdate, strtodate, "", "", "Delivered");
                    GridUser.DataSource = deliveredorders;
                    GridUser.DataBind();
                    showoverallcount();
                    PanelReset.Visible = true;
                    Counts.Visible = true;
                }
            }
        }
    }

    protected void rejectedorders_Click(object sender, EventArgs e)
    {
        DataTable rejectedorders = new DataTable();
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    rejectedorders = gblcls.FetchTrackingDetails(strfrmdate, strtodate, "", "", "Rejected");
                    GridUser.DataSource = rejectedorders;
                    GridUser.DataBind();
                    showoverallcount();
                    PanelReset.Visible = true;
                    Counts.Visible = true;
                }
            }
        }
    }

    string keyinguser = "";
    protected void Assign_Click(object sender, EventArgs e)
    {
        lblerror.Text = "";
        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "Assign();", true);
        PanelManualInfo.Visible = true;
        DataTable dtassign = new DataTable();
        dtassign.Columns.AddRange(new DataColumn[6] { new DataColumn("Order_No"), new DataColumn("State"), new DataColumn("County"), new DataColumn("Status"), new DataColumn("HP"), new DataColumn("Key OP") });
        foreach (GridViewRow row in GridUser.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
                if (chkRow.Checked)
                {
                    LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);
                    string state = row.Cells[7].Text;
                    string county = row.Cells[8].Text;
                    string status = row.Cells[10].Text;
                    string Priority = row.Cells[11].Text;
                    keyinguser = row.Cells[12].Text;
                    dtassign.Rows.Add(Convert.ToString(orderno.Text), state, county, status, Priority, keyinguser);
                    gvorderdetails.DataSource = dtassign;
                    gvorderdetails.DataBind();
                    dttest = dtassign.Copy();
                    gvorderdetails.Columns[5].Visible = false;
                    bindusername();
                }
            }
        }
    }

    private void bindusername()
    {
        try
        {
            DataSet dsprd = new DataSet();
            dsprd = gblcls.GetUsers();
            lstuserdetails.DataSource = dsprd;
            lstuserdetails.DataTextField = "User_Name";
            lstuserdetails.DataBind();
        }
        catch (Exception ex)
        {
            myConnection.setError(ex.ToString());
        }
    }

    string username = "";
    protected void lstuserdetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        username = lstuserdetails.SelectedItem.Text;
    }

    //Balaji
    protected void btnassign_Click(object sender, EventArgs e)
    {
        string unassignedorder = string.Empty;
        string orderno = "";
        string state = "";
        string county = "";
        string status = "";
        string priority = "";
        string usr = "";
        string k_op = "";
        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "Assign();", true);
        PanelManualInfo.Visible = true;
        string query = "";
        DataTable dttable = new DataTable();
        dttable.Columns.AddRange(new DataColumn[6] { new DataColumn("Order_No"), new DataColumn("State"), new DataColumn("County"), new DataColumn("Status"), new DataColumn("HP"), new DataColumn("Key OP") });
        foreach (GridViewRow row in gvorderdetails.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (row.Cells[6].FindControl("chkorders") as CheckBox);

                orderno = row.Cells[0].Text;
                state = row.Cells[1].Text;
                county = row.Cells[2].Text;
                status = row.Cells[3].Text;
                priority = row.Cells[4].Text;
                usr = lstuserdetails.SelectedItem.Text; 
                k_op = row.Cells[5].Text;
                dttable.Rows.Add(row.Cells[0].Text, row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text);

                DataSet dsfetchuser = new DataSet();
                string queryfetch = "select User_Name from user_status where User_Name = '" + usr + "' and Keying = '1'";
                dsfetchuser = con.ExecuteQuery(queryfetch);
                if (chkRow.Checked)
                {
                    if (dsfetchuser.Tables[0].Rows.Count > 0)
                    {
                        if (status != "Key Started" && status != "Qc Started" && status != "Key Done")
                        {
                            query = "update record_status set K1_OP='" + usr + "',k1=0,qc=0,status=0,Pend='0',Tax='0',Parcel='0' where Order_No='" + orderno + "'";
                            con.ExecuteSPNonQuery(query);
                            for (int i = dttable.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dr = dttable.Rows[i];
                                if (dr["status"].ToString() != "Key Started")
                                {
                                    dr.Delete();
                                }
                            }
                            dttable.AcceptChanges();
                            gvorderdetails.DataSource = dttable;
                            gvorderdetails.DataBind();
                            lblerror.Visible = true;
                            lblerror.Text = "Order Assigned Successfully";
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Order Cannot Be Assigned";
                            gvorderdetails.DataSource = dttable;
                            gvorderdetails.DataBind();
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Invalid User Status";
                    }
                }
            }
        }
        btnordershow_Click(sender, e);
    }


    protected void btnqcassign_Click(object sender, EventArgs e)
    {
        string orderno = "";
        string state = "";
        string county = "";
        string status = "";
        string priority = "";
        string usr = "";
        string k_op = "";
        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "Assign();", true);
        PanelManualInfo.Visible = true;
        string query = "";
        DataTable dttable = new DataTable();
        dttable.Columns.AddRange(new DataColumn[6] { new DataColumn("Order_No"), new DataColumn("State"), new DataColumn("County"), new DataColumn("Status"), new DataColumn("HP"), new DataColumn("Key OP") });
        foreach (GridViewRow row in gvorderdetails.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (row.Cells[6].FindControl("chkorders") as CheckBox);

                orderno = row.Cells[0].Text;
                state = row.Cells[1].Text;
                county = row.Cells[2].Text;
                status = row.Cells[3].Text;
                priority = row.Cells[4].Text;
                usr = lstuserdetails.SelectedItem.Text; 
                k_op = row.Cells[5].Text;
                dttable.Rows.Add(row.Cells[0].Text, row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text);

                DataSet dsfetchuser = new DataSet();
                string queryfetch = "select User_Name from user_status where User_Name = '" + usr + "' and QC = '1'";
                dsfetchuser = con.ExecuteQuery(queryfetch);
                if (chkRow.Checked)
                {
                    if (dsfetchuser.Tables[0].Rows.Count > 0)
                    {
                        if (status != "Qc Started")
                        {
                            query = "update record_status set QC_OP ='" + usr + "',pend='0',Parcel='0',Tax='0' where Order_No='" + orderno + "'";
                            con.ExecuteSPNonQuery(query);
                            for (int i = dttable.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dr = dttable.Rows[i];
                                if (dr["status"].ToString() != "Qc Started")
                                {
                                    dr.Delete();
                                }
                            }
                            dttable.AcceptChanges();
                            lblerror.Visible = true;
                            lblerror.Text = "Order Assigned Successfully";
                            gvorderdetails.DataSource = dttable;
                            gvorderdetails.DataBind();
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Order Cannot Be Assigned";
                            gvorderdetails.DataSource = dttable;
                            gvorderdetails.DataBind();
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Invalid User Status";
                    }
                }
            }
        }

        btnordershow_Click(sender, e);
    }
   

    //protected void Hold_Click(object sender, EventArgs e)
    //{
    //    if (txtfrmdate.Text != "")
    //    {
    //        dt = Convert.ToDateTime(txtfrmdate.Text);
    //        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

    //        DataTable dttracking = new DataTable();
    //        if (txttodate.Text != "")
    //        {
    //            dt = Convert.ToDateTime(txttodate.Text);
    //            strtodate = String.Format("{0:MM/dd/yyyy}", dt);

    //            if (strfrmdate != "" && strtodate != "")
    //            {
    //                foreach (GridViewRow row in GridUser.Rows)
    //                {
    //                    if (row.RowType == DataControlRowType.DataRow)
    //                    {
    //                        CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
    //                        if (chkRow.Checked)
    //                        {
    //                            LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);
    //                            gblcls.GetOrderHold(orderno.Text, strfrmdate, strtodate);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Changed Successfully')", true);
    //    btnordershow_Click(sender, e);
    //}


    //protected void UnHold_Click(object sender, EventArgs e)
    //{
    //    if (txtfrmdate.Text != "")
    //    {
    //        dt = Convert.ToDateTime(txtfrmdate.Text);
    //        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

    //        DataTable dttracking = new DataTable();
    //        if (txttodate.Text != "")
    //        {
    //            dt = Convert.ToDateTime(txttodate.Text);
    //            strtodate = String.Format("{0:MM/dd/yyyy}", dt);

    //            if (strfrmdate != "" && strtodate != "")
    //            {
    //                foreach (GridViewRow row in GridUser.Rows)
    //                {
    //                    if (row.RowType == DataControlRowType.DataRow)
    //                    {
    //                        CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
    //                        if (chkRow.Checked)
    //                        {
    //                            LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);

    //                            gblcls.GetOrderUnHold(orderno.Text, strfrmdate, strtodate);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Changed Successfully')", true);
    //    btnordershow_Click(sender, e);
    //}

    protected void Reject_Click(object sender, EventArgs e)
    {
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    foreach (GridViewRow row in GridUser.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
                            if (chkRow.Checked)
                            {
                                LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);
                                string status = row.Cells[10].Text.Trim();

                                if (status != "Key Started" && status != "Qc Started")
                                {
                                    gblcls.GetOrderReject(orderno.Text, strfrmdate, strtodate);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Changed Successfully')", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Cannot Be Changed')", true);
                                }
                            }
                        }
                    }
                }
            }
        }
        btnordershow_Click(sender, e);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    foreach (GridViewRow row in GridUser.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
                            if (chkRow.Checked)
                            {
                                LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);

                                gblcls.GetOrderDelete(orderno.Text, strfrmdate, strtodate);
                            }
                        }
                    }
                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Changed Successfully')", true);
        btnordershow_Click(sender, e);
    }

    protected void Lock_Click(object sender, EventArgs e)
    {
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    foreach (GridViewRow row in GridUser.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
                            if (chkRow.Checked)
                            {
                                LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);
                                string status = row.Cells[10].Text.Trim();

                                if (status != "Key Started" && status != "Qc Started")
                                {
                                    gblcls.GetOrderLock(orderno.Text, strfrmdate, strtodate);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Changed Successfully')", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Status Cannot be Changed')", true);
                                }
                            }
                        }
                    }
                }
            }
        }
        btnordershow_Click(sender, e);
    }

 
    protected void UnLock_Click(object sender, EventArgs e)
    {
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    foreach (GridViewRow row in GridUser.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
                            if (chkRow.Checked)
                            {
                                LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);
                                string status = row.Cells[10].Text;
                                if (status != "Key Started" && status != "QC Started")
                                {
                                    gblcls.GetOrderUnLock(orderno.Text, strfrmdate, strtodate);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Changed Successfully')", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Status Cannot Be Changed')", true);
                                }
                            }
                        }
                    }
                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Changed Successfully')", true);
        btnordershow_Click(sender, e);
    }



    protected void Priority_Click(object sender, EventArgs e)
    {
        string status = "";
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    foreach (GridViewRow row in GridUser.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
                            status = row.Cells[10].Text;
                            if (chkRow.Checked)
                            {

                                LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);

                                if (status != "Key Started" && status != "QC Started")
                                {
                                    gblcls.GetOrderPriority(orderno.Text, strfrmdate, strtodate);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Changed Successfully')", true);
        btnordershow_Click(sender, e);
    }

    protected void DePriority_Click(object sender, EventArgs e)
    {
        string status = "";
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    foreach (GridViewRow row in GridUser.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
                            status = row.Cells[10].Text;
                            if (chkRow.Checked)
                            {
                                if (status != "Key Started" && status != "QC Started")
                                {
                                    LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);
                                    gblcls.GetOrderDePriority(orderno.Text, strfrmdate, strtodate);
                                }
                            }
                        }
                    }
                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Order Status Changed Successfully')", true);
        btnordershow_Click(sender, e);
    }

    protected void YTS_Click(object sender, EventArgs e)
    {
        string status = "";
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DataTable dttracking = new DataTable();
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);

                if (strfrmdate != "" && strtodate != "")
                {
                    foreach (GridViewRow row in GridUser.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chktrackdetails") as CheckBox);
                            if (chkRow.Checked)
                            {
                                LinkButton orderno = (row.Cells[2].FindControl("Lnkorder") as LinkButton);
                                status = row.Cells[10].Text;
                                if (status != "Key Started" && status != "QC Started")
                                {
                                    gblcls.GetOrderYTS(orderno.Text, strfrmdate, strtodate);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Status Changed Successfully')", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Status Cannot Be Changed')", true);
                                }
                            }
                        }
                    }
                }
            }
        }
        btnordershow_Click(sender, e);
    }
    #endregion
    //amrock
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
                    LogoutReason.Visible = true;
                }

                else
                {
                    Session["TimePro"] = DateTime.Now;
                    if (SessionHandler.AuditQA == "1") Response.Redirect("~/Pages/ProductionAuditNew.aspx?id=" + id);
                    else Response.Redirect("STRMICXProduction.aspx?id=" + id);
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

    }
    protected void btnlogoutclose_Click(object sender, EventArgs e)
    {
        pagedimmer.Visible = false;
        LogoutReason.Visible = false;
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
        else Response.Redirect("STRMICXProduction.aspx?id=" + Session["id"].ToString());
    }

    protected void GridUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Pager)
        {
            //Image imglocked = e.Row.FindControl("Imglocked") as Image;

            if (e.Row.Cells[10].Text == "Delivered")
            {
                //e.Row.Cells[10].BackColor = System.Drawing.Color.FromArgb(163, 186, 71);
                //e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;
                //imglocked.Visible = false;
                //e.Row.Cells[1].Enabled = false;

            }
            if (e.Row.Cells[10].Text == "Rejected")
            {
                //e.Row.Cells[10].BackColor = System.Drawing.Color.SandyBrown;
                //e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;
                //imglocked.Visible = false;
            }
            if (e.Row.Cells[10].Text == "On Hold")
            {
                //e.Row.Cells[10].BackColor = System.Drawing.Color.SkyBlue;
                //e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;
                //imglocked.Visible = false;
            }
            if (e.Row.Cells[10].Text == "Mail Away")
            {
                //e.Row.Cells[10].BackColor = System.Drawing.Color.FromArgb(170, 255, 212);
                //e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;
                //imglocked.Visible = false;
            }
            if (e.Row.Cells[10].Text == "ParcelID")
            {
                //e.Row.Cells[10].BackColor = System.Drawing.Color.FromArgb(250, 250, 142);
                //e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;
                //imglocked.Visible = false;
            }
            if (e.Row.Cells[10].Text == "In Process")
            {
                //e.Row.Cells[10].BackColor = System.Drawing.Color.FromArgb(250, 200, 211);
                //e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;
                //imglocked.Visible = false;
            }
            if (e.Row.Cells[10].Text == "YTS" || e.Row.Cells[10].Text == "Key Done" || e.Row.Cells[10].Text == "Others" || e.Row.Cells[13].Text == "Order Missing")
            {
                //imglocked.Visible = false;
            }
            if ((e.Row.Cells[10].Text == "Key Start") || (e.Row.Cells[10].Text == "QC Started") || (e.Row.Cells[13].Text == "In Process Started") || (e.Row.Cells[13].Text == "Mail Away Started") || (e.Row.Cells[10].Text == "ParcelID Started"))
            {
                //e.Row.Cells[10].BackColor = System.Drawing.Color.FromArgb(96, 219, 207);
                //e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;
                //imglocked.Visible = true;
            }
            if ((e.Row.Cells[10].Text == "Key Start") || (e.Row.Cells[10].Text == "ParcelID Started") || (e.Row.Cells[10].Text == "Mail Away Started") || (e.Row.Cells[10].Text == "In Process Started"))
            {
                //string StartTime, EndTime = "";
                //string State, County = "";
                //StartTime = e.Row.Cells[16].Text;
                //EndTime = DateTime.Now.ToString();
                //State = e.Row.Cells[8].Text;
                //County = e.Row.Cells[9].Text;
                //if (StartTime == "&nbsp;") { return; }
                //DateTime startTime = DateTime.Parse(StartTime);
                //DateTime endTime = DateTime.Parse(EndTime);
                //TimeSpan ts = endTime.Subtract(startTime);
                //string time = ts.ToString();
                //time = time.Replace(".", "");
                //int TAT = int.Parse(time.Replace(":", ""));
                //string query = "Select sf_getordertype('" + State + "','" + County + "')";
                //string result = con.ExecuteScalar(query);
                //if (result == "Website" || result == "Phone/ Website")
                //{
                //    if (TAT >= 000000 && TAT <= 000300) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                //    else if (TAT > 000300 && TAT <= 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 102);
                //    else if (TAT > 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
                //}
                //else if (result == "Phone")
                //{
                //    if (TAT >= 000000 && TAT <= 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                //    else if (TAT > 000500 && TAT <= 000900) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 102);
                //    else if (TAT > 000900) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
                //}
            }
            if (e.Row.Cells[10].Text == "QC Started")
            {
                //string StartTime, EndTime = "";
                //string State, County = "";
                //StartTime = e.Row.Cells[20].Text;
                //EndTime = DateTime.Now.ToString();
                //State = e.Row.Cells[8].Text;
                //County = e.Row.Cells[9].Text;
                //if (StartTime == "&nbsp;") { return; }
                //DateTime startTime = DateTime.Parse(StartTime);
                //DateTime endTime = DateTime.Parse(EndTime);
                //TimeSpan ts = endTime.Subtract(startTime);
                //string time = ts.ToString();
                //time = time.Replace(".", "");
                //int TAT = int.Parse(time.Replace(":", ""));
                //string query = "Select sf_getordertype('" + State + "','" + County + "')";
                //string result = con.ExecuteScalar(query);
                //if (result == "Website" || result == "Phone/ Website")
                //{
                //    if (TAT >= 000000 && TAT <= 000200) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                //    else if (TAT > 000200 && TAT <= 000300) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 102);
                //    else if (TAT > 000300) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
                //}
                //else if (result == "Phone")
                //{
                //    if (TAT >= 000000 && TAT <= 000300) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                //    else if (TAT > 000300 && TAT <= 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 102);
                //    else if (TAT > 000500) e.Row.BackColor = System.Drawing.Color.FromArgb(235, 97, 61);
                //}
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

    private void showoverallcount()
    {
        if (txtfrmdate.Text != "")
        {
            dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);
            if (txttodate.Text != "")
            {
                dt = Convert.ToDateTime(txttodate.Text);
                strtodate = String.Format("{0:MM/dd/yyyy}", dt);
                ds = gblcls.GetOrderCountAll(strfrmdate, strtodate);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    wiporders.Text = ds.Tables[0].Rows[0]["Working"].ToString();
                    ytsorders.Text = ds.Tables[0].Rows[0]["Yts"].ToString();
                    mailwayorders.Text = ds.Tables[0].Rows[0]["Mailaway"].ToString();
                    inprocessorders.Text = ds.Tables[0].Rows[0]["Inprocess"].ToString();
                    deliveredorders.Text = ds.Tables[0].Rows[0]["Delivered"].ToString();
                    rejectedorders.Text = ds.Tables[0].Rows[0]["Rejected"].ToString();
                    totalorders.Text = ds.Tables[0].Rows[0]["Total"].ToString();
                }
            }
            else
            {
                errorlabel.Text = "Please Select the To date";
            }
        }
    }
}
