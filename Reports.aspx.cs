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
using System.IO;
using System.Text;


public partial class Pages_Reports : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataView dataview = new DataView();
    GlobalClass gblcls = new GlobalClass();
    myConnection con = new myConnection();

    string strfrmdate = string.Empty;
    string strtodate = string.Empty;
    string strusrname = string.Empty;
    public DataSet dsTER = new DataSet();
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        if (SessionHandler.IsAdmin == false)
        {
            SessionHandler.UserName = "";
            Response.Redirect("Loginpage.aspx");
        }

        if (!Page.IsPostBack)
        {
            txtfrmdate.Text = gblcls.setdate();
            txttodate.Text = gblcls.setdate();
            UsernameLoad();
            ClearText();
            Togglepanel(PanelAssign);
            lblusername.Visible = false;
            ddlusername.Visible = false;
            GridQuality.Visible = false;
        }
        btnsavecheque.Visible = false;
        DivCommentsUpdate.Visible = false;
        pagedimmer.Visible = false;
    }
    public void UsernameLoad()
    {
        ds.Dispose();
        ds.Reset();
        string strquery = "select User_Name from user_status order by User_Name";
        ds = con.ExecuteQuery(strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlusername.DataSource = ds;
            ddlusername.DataTextField = "User_Name";
            ddlusername.DataBind();
            ddlusername.Items.Insert(0, "ALL");
        }
    }
    public void Togglepanel(Panel sPanel)
    {
        PanelAssign.Visible = false;
        PanelCheque.Visible = false;
        PanelStateComments.Visible = false;
        PanelThroughput.Visible = false;
        PanelBreak.Visible = false;
        //PanelTat.Visible = false;
        //pnltat.Visible = false;
        sPanel.Visible = true;
    }
    public void Togglegrid(GridView sGrid)
    {
        GridEOD.Visible = false;
        GridQuality.Visible = false;
        grd_break_total.Visible = false;
        Gridtatreport.Visible = true;

        sGrid.Visible = true;
    }
    #endregion

    #region EOD
    protected void Lnkeod1_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            //strfrmdate = txtfrmdate.Text;
            //strtodate = txttodate.Text;

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                ds1.Dispose();
                ds1.Reset();
                string strquery = "select Order_no,Pdate,rs.State,ts.Tier,County,Prior,Webphone,Borrowername,Township,Lock1,K1,QC,Pend,Status,Parcel,Tax,Rejected,K1_OP,QC_OP,DeliveredDate,Lastcomment,serpro from record_status rs join tier_states ts on ts.State = rs.State where serpro!='Tax Typing - Advanced' and (DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '18:30' HOUR_MINUTE),'%m/%d/%Y') between '" + strfrmdate + "' and '" + strtodate + "' ) or (Pdate between '" + strfrmdate + "' and '" + strtodate + "' and  (tax='3' or tax='0') and parcel<>'3' and (k1='2' or k1='7') and (qc='2' or qc='7') and (status='2' or status='7'))";
                string strquery1 = "select Order_no,Pdate,rs.State,ts.Tier,County,Prior,Webphone,Borrowername,Township,Lock1,K1,QC,Pend,Status,Parcel,Tax,Rejected,K1_OP,QC_OP,DeliveredDate,Lastcomment,serpro from record_status rs join tier_states ts on ts.State = rs.State where Pdate not between '" + strfrmdate + "' and '" + strtodate + "' and  (tax='3' or tax='0') and parcel<>'3' and k1='2' and qc='2' and status='2' and  serpro!='Tax Typing - Advanced'";
                ds = con.ExecuteQuery(strquery);
                ds1 = con.ExecuteQuery(strquery1);
                ds.Merge(ds1);
                ShowGridEOD(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridEOD(DataSet ds)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataview = gblcls.CovertEODDstoDataview(ds);
                GridEOD.DataSource = dataview;
                GridEOD.DataBind();
                GridEOD.Visible = true;
                GridQuality.Visible = false;
                Togglepanel(PanelAssign);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    protected void Lnkeod2_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                ds1.Dispose();
                ds1.Reset();
                string strquery = "select Order_no,Pdate,rs.State,ts.Tier,County,Prior,Webphone,Borrowername,Township,Lock1,K1,QC,Pend,Status,Parcel,Tax,Rejected,K1_OP,QC_OP,DeliveredDate,Lastcomment,serpro from record_status rs join tier_states ts on ts.State = rs.State where serpro!='Tax Typing - Advanced' and pdate between '" + strfrmdate + "' and '" + strtodate + "' and (k1='4' or k1='2') and (qc='4' or qc='2') and (status='4' or status='2') and(parcel='3' or parcel='0') and pend<>'3' and tax<>'3'";
                string strquery1 = "select Order_no,Pdate,rs.State,ts.Tier,County,Prior,Webphone,Borrowername,Township,Lock1,K1,QC,Pend,Status,Parcel,Tax,Rejected,K1_OP,QC_OP,DeliveredDate,Lastcomment,serpro from record_status rs join tier_states ts on ts.State = rs.State where pdate not between '" + strfrmdate + "' and '" + strtodate + "' and (k1='4' or k1='2') and (qc='4' or qc='2') and (status='4' or status='2') and (parcel='3' or parcel='0') and pend<>'3' and tax<>'3' and serpro!='Tax Typing - Advanced'";
                ds = con.ExecuteQuery(strquery);
                ds1 = con.ExecuteQuery(strquery1);
                ds.Merge(ds1);
                ShowGridEOD1(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridEOD1(DataSet ds)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataview = gblcls.CovertEODDstoDataview1(ds);
                GridEOD.DataSource = dataview;
                GridEOD.DataBind();
                GridEOD.Visible = true;
                GridQuality.Visible = false;
                Togglepanel(PanelAssign);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridEOD2(DataSet ds)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataview = gblcls.CovertEODDstoDataview1(ds);
                GridEOD.DataSource = dataview;
                GridEOD.DataBind();
                GridEOD.Visible = true;
                GridQuality.Visible = false;
                Togglepanel(PanelAssign);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    #endregion

    #region Quality
    protected void Lnkquality1_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select K1_op,Error,Errorfield from record_status where pdate between '" + strfrmdate + "' and '" + strtodate + "' and k1='5' and qc='5' and status='5' group by K1_op";
                ds = con.ExecuteQuery(strquery);
                ShowGridQuality(ds, strfrmdate, strtodate);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    public void ShowGridQuality(DataSet ds, string strfrmdate, string strtodate)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataview = gblcls.CovertQualityDStoDataview1(ds, strfrmdate, strtodate);
                GridEOD.DataSource = dataview;
                GridEOD.DataBind();
                GridEOD.Visible = true;
                GridQuality.Visible = false;
                Togglepanel(PanelAssign);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void Lnkquality2_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select pdate,order_no,State,County,k1_op,delivereddate,error,errorfield,webphone,correct,incorrect,qc_op,QC_OP_Comments as `lastcomment` from record_status where (DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y')) between '" + strfrmdate + "' and '" + strtodate + "' and k1='5' and qc='5' and status='5' and Direct!='1' order by pdate desc";
                ds = con.ExecuteQuery(strquery);
                ShowGridQuality1(ds, "Qc");
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void Lnkquality3_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select pdate,order_no,State,County,k1_op,qc_op,delivereddate,error1,errorfield1,webphone,correct1,incorrect1,Review_op,lastcomment from record_status where DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strfrmdate + "' and '" + strtodate + "' and k1='5' and qc='5' and status='5' and Review='5'";
                ds = con.ExecuteQuery(strquery);
                ShowGridQuality1(ds, "Review");
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    public void ShowGridQuality1(DataSet ds, string Process)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataview = gblcls.CovertQualityDStoDataview(ds, Process);
                GridEOD.DataSource = dataview;
                GridEOD.DataBind();
                GridEOD.Visible = true;
                GridQuality.Visible = false;
                Togglepanel(PanelAssign);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void lnkerroraccepted_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select Order_No,Pdate,State,County,ErrorField,Incorrect,Correct,Error_Comments as ErrorComments,QC_OP as 'QC Name',K1_OP as AcceptedBy from record_status where DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strfrmdate + "' and '" + strtodate + "' and Error='Error'";
                ds = con.ExecuteQuery(strquery);
                GridEOD.DataSource = ds;
                GridEOD.DataBind();
                GridEOD.Visible = true;
                GridQuality.Visible = false;
                Togglepanel(PanelAssign);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void Lnkqaerroraccepted_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select Order_No,Pdate,State,County,ErrorField1 as ErrorField,Incorrect1 as Incorrect,Correct1 as Correct,Error_Comments1 as ErrorComments,Review_OP,QC_OP as AcceptedBy,Cause,Suggestion from record_status where DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strfrmdate + "' and '" + strtodate + "' and Error1='Error'";
                ds = con.ExecuteQuery(strquery);
                GridQuality.DataSource = ds;
                GridQuality.DataBind();
                GridEOD.Visible = false;
                GridQuality.Visible = true;
                Togglepanel(PanelAssign);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void GridQuality_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            string strord = Convert.ToString(GridQuality.DataKeys[gvr.RowIndex].Values[0]);
            string strcause = Convert.ToString(gvr.Cells[11].Text);
            string strsuggestion = Convert.ToString(gvr.Cells[12].Text);
            LblOrderno.Text = strord;
            if (strcause != "&nbsp;") txtcause.Text = strcause;
            else txtcause.Text = "";
            if (strsuggestion != "&nbsp;") txtsuggestion.Text = strsuggestion;
            else txtsuggestion.Text = "";
            pagedimmer.Visible = true;
            DivCommentsUpdate.Visible = true;
        }

    }
    protected void GridQuality_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GridQuality_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void btnupdatecomments_Click(object sender, EventArgs e)
    {
        string strquery = "update record_status set Cause='" + txtcause.Text.Trim() + "',Suggestion='" + txtsuggestion.Text.Trim() + "' where Order_No='" + LblOrderno.Text + "' limit 1";
        int result = con.ExecuteSPNonQuery(strquery);
        if (result > 0)
        {
            DivCommentsUpdate.Visible = false;
            pagedimmer.Visible = false;
            Lnkqaerroraccepted_Click(sender, e);
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        DivCommentsUpdate.Visible = false;
        pagedimmer.Visible = false;
    }
    protected void LnkFpyReport_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                ds = gblcls.FpyReport(strfrmdate, strtodate);
                DataTable dtt = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dtt = GetDataTable(ds.Tables[0]);
                    GridView1.DataSource = dtt;
                    GridView1.DataBind();
                    GridView1.HeaderRow.Visible = false;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    GridView2.DataSource = ds.Tables[1];
                    GridView2.DataBind();
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    GridEOD.DataSource = ds.Tables[2];
                    GridEOD.DataBind();
                }

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                {
                    ExportFpyReport();
                }
                GridEOD.Visible = false;
                GridView1.Visible = false;
                GridView2.Visible = false;
                GridQuality.Visible = false;
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    private DataTable GetDataTable(DataTable dataTable)
    {
        DataTable dtTable = new DataTable();
        DataColumn dcolumn;

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Header";
        dcolumn.Caption = "Header";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Header Count";
        dcolumn.Caption = "Header Count";
        dtTable.Columns.Add(dcolumn);

        if (dataTable.Rows.Count > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                DataRow dtrow = dtTable.NewRow();
                if (i == 0) dtrow[0] = "Total No.of Pre-Audited Orders";
                else if (i == 1) dtrow[0] = "No Errors";
                else if (i == 2) dtrow[0] = "Errors";
                else if (i == 3) dtrow[0] = "Quality Percentage";
                dtrow[1] = dataTable.Rows[0][i];
                dtTable.Rows.Add(dtrow);
            }
        }
        return dtTable;
    }
    #endregion

    #region Mailaway
    protected void Lnkmailaway_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select Order_no,pDate,Mailaway_date,Followup_date,Address,Cheque_payable,Amount,Req_Type,Return_Type,Cheque_Required,TaxType,Borrowername,BorrowerAddress,ParcelId,TrackingNo,ChequeNo from mailaway_tbl where (Req_Type='THANKS REQUEST' or Req_Type='REGULAR') and tDate between '" + strfrmdate + "' and '" + strtodate + "' order by Order_no,Cheque_payable";
                ds = con.ExecuteQuery(strquery);
                ShowGridMailaway(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridMailaway(DataSet ds)
    {
        dataview = gblcls.CovertMailawayDStoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }
    protected void Lnkmailwayups_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select Order_no,pDate,Mailaway_date,Followup_date,Address,Cheque_payable,Amount,Req_Type,Return_Type,Cheque_Required,TaxType,Borrowername,BorrowerAddress,ParcelId,TrackingNo,ChequeNo from mailaway_tbl where Req_Type <> 'THANKS REQUEST' and Req_Type <> 'REGULAR' and tDate between '" + strfrmdate + "' and '" + strtodate + "' order by Order_no,Cheque_payable";
                ds = con.ExecuteQuery(strquery);
                ShowGridMailaway(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    #endregion

    #region Consolidated
    protected void Lnkbtnreglrcon_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select Order_no,pDate,concat(Cheque_payable,', ',address) as Cheque_payable,Amount,Req_Type,Return_Type,Cheque_Required from mailaway_tbl where (Req_Type='THANKS REQUEST' or Req_Type='REGULAR') and tDate between '" + strfrmdate + "' and '" + strtodate + "' order by Order_no,Cheque_payable";
                ds = con.ExecuteQuery(strquery);
                ShowGridRegularConsolidated(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridRegularConsolidated(DataSet ds)
    {
        dataview = gblcls.CovertRegularDStoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }

    protected void Lnkbtnupscon_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select Order_no,Borrowername as `Property Owner Name`,BorrowerAddress as `Property Address`,pDate,Cheque_payable,Address,Amount,Req_Type,Return_Type,Cheque_Required from mailaway_tbl where Req_Type <> 'THANKS REQUEST' and Req_Type <> 'REGULAR' and tDate between '" + strfrmdate + "' and '" + strtodate + "' order by Order_no,Cheque_payable";
                ds = con.ExecuteQuery(strquery);
                ShowGridUPSConsolidated(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridUPSConsolidated(DataSet ds)
    {
        dataview = gblcls.CovertUPSDStoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }

    protected void Lnkbtnupstemp_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select Order_no,Req_Type,Cheque_payable from mailaway_tbl where Req_Type <> 'THANKS REQUEST' and Req_Type <> 'REGULAR' and tDate between '" + strfrmdate + "' and '" + strtodate + "' order by Order_no,Cheque_payable";
                ds = con.ExecuteQuery(strquery);
                ShowGridUPSTemplate(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridUPSTemplate(DataSet ds)
    {
        dataview = gblcls.CovertUPSTempDStoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }

    #endregion

    #region Invoice
    protected void Lnkbtninvoice_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select Order_No,pdate,DeliveredDate,WebPhone,borrowername,State,County,k1,qc,status,tax from record_status where (DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + strfrmdate + "' and '" + strtodate + "' and k1='5' and qc='5' and status='5' and Tax ='0' and Parcel='0' and Pend='0')";
                ds = con.ExecuteQuery(strquery);
                ShowGridInvoice(ds, strfrmdate, strtodate);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridInvoice(DataSet ds, string strfrmdate, string strtodate)
    {
        dataview = gblcls.CovertInvoiceDStoDataview(ds, strfrmdate, strtodate);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }
    #endregion

    #region Checklist

    protected void Lnkbtnlogchklst_Click(object sender, EventArgs e)
    {
        string struser = string.Empty;
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:dd-MM-yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:dd-MM-yyyy}", df);

            strusrname = ddlusername.SelectedItem.Text.ToString();
            if (strusrname == "ALL") { struser = ""; }
            else { struser = "and username='" + strusrname + "'"; }

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select username,pdate,login_time,logout_time,attendance,biometrice,mobile_restriction,id_card_dress_code,heatset_allot,login_hardware,work_place_clean,headset_over,switchoff_system from checklist_login_report where pdate between '" + strfrmdate + "' and '" + strtodate + "' " + struser + " order by pdate";
                ds = con.ExecuteQuery(strquery);
                ShowGridLoginChecklist(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridLoginChecklist(DataSet ds)
    {
        dataview = gblcls.CovertLoginCheckListDStoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }

    protected void Lnkbtnchklst_Click(object sender, EventArgs e)
    {
        string struser = string.Empty;
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            strusrname = ddlusername.SelectedItem.Text.ToString();
            if (strusrname == "ALL") { struser = ""; }
            else { struser = "and operator='" + strusrname + "'"; }

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                string strquery = "select Order_no,pdate,processname,operator,status,comments from checklist_report_new where pdate between '" + strfrmdate + "' and '" + strtodate + "' " + struser + " order by status";
                ds = con.ExecuteQuery(strquery);
                ShowGridChecklist(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    public void ShowGridChecklist(DataSet ds)
    {
        dataview = gblcls.CovertCheckListDStoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }

    protected void Lnkdeletelogin_Click(object sender, EventArgs e)
    {
        lblusername.Visible = true;
        ddlusername.Visible = true;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:dd-MM-yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:dd-MM-yyyy}", df);

        DateTime ydate = gblcls.ToDate();
        string strcurrdate = String.Format("{0:dd-MM-yyyy}", ydate);

        string struser = string.Empty;
        struser = ddlusername.SelectedItem.Text;
        struser = gblcls.TCase(struser);
        if (txtfrmdate.Text == txttodate.Text)
        {
            if (struser != SessionHandler.UserName)
            {
                string strquery = "";
                int result = 0;
                if (strcurrdate == strfrmdate)
                {
                    strquery = "delete from checklist_login_report where username='" + struser + "' and pdate='" + strfrmdate + "'";
                    result = con.ExecuteSPNonQuery(strquery);
                    if (result > 0)
                    {
                        lblerror.Text = "Login Deleted Successfully";
                    }
                }
                else
                {
                    strquery = "update checklist_login_report set flag=0,logout_time=now(),work_place_clean='CHECKED',headset_over='CHECKED',switchoff_system='CHECKED' where username='" + struser + "' and pdate='" + strfrmdate + "'";
                    result = con.ExecuteSPNonQuery(strquery);
                    if (result > 0)
                    {
                        lblerror.Text = "Logout Details Updated Successfully";
                    }
                }
            }
            else
            {
                lblerror.Text = "Can't Delete your login details";
            }
        }
        else
        {
            lblerror.Text = "Please select from date and to date as same";
        }
    }

    #endregion

    #region Mailaway Consolidated
    protected void LnkRegrequest_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtfrmdate.Text == txttodate.Text)
            {

                lblusername.Visible = false;
                ddlusername.Visible = false;

                string strdate = string.Empty;
                string query = "select Output_Path_Regular from master_path";
                string query1 = "select Output_Consolidated from master_path";
                DateTime pdte = Convert.ToDateTime(txtfrmdate.Text);
                strdate = String.Format("{0:dd MMM yyyy}", pdte);
                string strfile = strdate + "-" + "Regular.doc";
                string filepath = getfullpath(query);
                string filepath1 = getfullpath(query1);
                string[] files = Directory.GetFiles(filepath, "*.doc", SearchOption.AllDirectories);
                Array.Sort(files);
                if (files.Length != 0)
                {
                    FileStream outputFile = new FileStream(filepath1 + "/" + strfile, FileMode.Create);
                    //FileStream outputFile = new FileStream(@"E:\Karthikeyan\Task\TSI Taxes\Reports" + "/" + strfile, FileMode.Create);
                    using (BinaryWriter ws = new BinaryWriter(outputFile))
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            byte[] data = File.ReadAllBytes(files[i]);
                            MemoryStream ms = new MemoryStream(data);
                            ms.WriteTo(outputFile);
                            //ws.Write(System.IO.File.ReadAllBytes(files[i]));
                        }
                    }
                    lblerror.Text = "Mailaway Consolidated Request Created Successfully";
                }
                else
                {
                    lblerror.Text = "There is no files in that particular date";
                }
            }
            else
            {
                lblerror.Text = "Please select from date and to date as same";
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    protected void Lnkupsrequest_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtfrmdate.Text == txttodate.Text)
            {
                lblusername.Visible = false;
                ddlusername.Visible = false;

                string strdate = string.Empty;
                string query = "select Output_Path_Ups from master_path";
                string query1 = "select Output_Consolidated from master_path";
                DateTime pdte = Convert.ToDateTime(txtfrmdate.Text);
                strdate = String.Format("{0:dd MMM yyyy}", pdte);
                string strfile = strdate + "-" + "UPS.doc";
                string filepath = getfullpath(query);
                string filepath1 = getfullpath(query1);
                string[] files = Directory.GetFiles(filepath, "*.doc", SearchOption.AllDirectories);
                Array.Sort(files);
                if (files.Length != 0)
                {
                    FileStream outputFile = new FileStream(filepath1 + "/" + strfile, FileMode.Create);
                    //FileStream outputFile = new FileStream(@"E:\Karthikeyan\Task\TSI Taxes\Reports" + "/" + strfile, FileMode.Create);
                    using (BinaryWriter ws = new BinaryWriter(outputFile))
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            byte[] data = File.ReadAllBytes(files[i]);
                            MemoryStream ms = new MemoryStream(data);
                            ms.WriteTo(outputFile);
                            //ws.Write(System.IO.File.ReadAllBytes(files[i]));
                        }
                    }
                    lblerror.Text = "Mailaway Consolidated Request Created Successfully";
                }
                else
                {
                    lblerror.Text = "There is no files in that particular date";
                }
            }
            else
            {
                lblerror.Text = "Please select from date and to date as same";
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    private string getfullpath(string query)
    {
        string slash = @"\";
        string dec, sourcePath, pdatee, month, year, path = "";
        //string query = "select Output_Path from master_path";
        MySqlParameter[] mParam = new MySqlParameter[1];
        MySqlDataReader mDataReader = con.ExecuteStoredProcedure(query, false, mParam);
        if (mDataReader.HasRows)
        {
            if (mDataReader.Read())
            {
                sourcePath = mDataReader.GetString(0);
                DateTime pde;
                pde = Convert.ToDateTime(txtfrmdate.Text);
                pdatee = String.Format("{0:dd MMM yy}", pde);
                month = String.Format("{0:MMMM}", pde);
                year = String.Format("{0:yyyy}", pde);
                dec = sourcePath + slash + year + slash + month + slash + pdatee;
                dir(dec);
                path = dec;
            }
        }
        mDataReader.Close();
        return path;
    }

    //private string getfullpath1()
    //{
    //    string slash = @"\";
    //    string dec, sourcePath, pdatee, month, year, path = "";
    //    string query = "select Output_Consolidated from master_path";
    //    MySqlParameter[] mParam = new MySqlParameter[1];
    //    MySqlDataReader mDataReader = con.ExecuteStoredProcedure(query, false, mParam);
    //    if (mDataReader.HasRows)
    //    {
    //        if (mDataReader.Read())
    //        {
    //            sourcePath = mDataReader.GetString(0);
    //            DateTime pde;
    //            pde = Convert.ToDateTime(txtfrmdate.Text);
    //            pdatee = String.Format("{0:dd MMM yy}", pde);
    //            month = String.Format("{0:MMMM}", pde);
    //            year = String.Format("{0:yyyy}", pde);
    //            dec = sourcePath + slash + year + slash + month + slash + pdatee;
    //            dir(dec);
    //            path = dec;
    //        }
    //    }
    //    return path;
    //}

    private void dir(string path)
    {
        //System.Security.AccessControl.DirectorySecurity dsec = new System.Security.AccessControl.DirectorySecurity();
        //System.Security.AccessControl.FileSystemAccessRule fsac;
        //fsac = new System.Security.AccessControl.FileSystemAccessRule("everyone", System.Security.AccessControl.FileSystemRights.Modify, System.Security.AccessControl.AccessControlType.Allow);
        //dsec.AddAccessRule(fsac);

        try
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                System.IO.DirectoryInfo dIn = new System.IO.DirectoryInfo(path);
                //dIn.SetAccessControl(dsec);
            }
        }
        catch (System.IO.DirectoryNotFoundException)
        {

        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region Dinner Breaks
    protected void Lnkbtnbreak_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);

        if (strfrmdate != "" && strtodate != "")
        {
            ds.Dispose();
            ds.Reset();
            ds = gblcls.TotalBreakTimeReport(strfrmdate, strtodate);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    GridEOD.DataSource = ds;
            //    GridEOD.DataBind();
            //    GridEOD.Visible = true;
            //    Togglepanel(PanelAssign);
            //}
            ShowGridBreakTime(ds);
        }
        else
        {
            lblerror.Text = "Please select from date and to date";
        }
    }

    public void ShowGridBreakTime(DataSet ds)
    {
        dataview = gblcls.CovertBreakDStoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }

    protected void Lnkbtnunlock_Click(object sender, EventArgs e)
    {
        lblusername.Visible = true;
        ddlusername.Visible = true;

        string struser = ddlusername.SelectedItem.Text;
        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);


        if (struser == "ALL" || (struser != "ALL" && strfrmdate == strtodate))
        {
            ds.Dispose();
            ds.Reset();
            ds = gblcls.Production_Lock(strfrmdate, strtodate, struser);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridEOD.DataSource = ds;
                GridEOD.DataBind();
                Togglepanel(PanelAssign);
                GridEOD.Visible = true;
                GridQuality.Visible = false;
            }
            else
            {
                GridEOD.DataSource = null;
                GridEOD.DataBind();
            }
        }
        else
        {
            lblerror.Text = "Please select Username, from date and to date as same";
        }
    }
    #endregion

    #region Hourly Count Report
    protected void Lnkhourcount_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;

        string username = "";
        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);

        if (strfrmdate != "" && strtodate != "")
        {
            ds.Dispose();
            ds.Reset();
            ds = gblcls.GetHourlyCount(strfrmdate, strtodate);
            ShowGridHourlyCount(ds);

        }
        else
        {
            lblerror.Text = "Please select from date and to date";
        }
    }

    public void ShowGridHourlyCount(DataSet ds)
    {
        dataview = gblcls.CovertHourlyCountDStoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }
    #endregion

    #region Download and Upload Pattern

    protected void Lnkbtndownload_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;
        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);

        if (strfrmdate != "" && strtodate != "")
        {
            //gblcls.ClearDownloaduploadpattern();
            //ds.Dispose();
            //ds.Reset();
            //ds = gblcls.getdownloadpattern(strfrmdate, strtodate, "DownloadPattern");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    DetailsDownload.DataSource = ds;
            //    DetailsDownload.DataBind();
            //    InsertDownloaduploadpattern(DetailsDownload, "Download");
            //}
            //FetchDownloadpattern();
            //PanelUploadDownload.Visible = false;


            ds.Dispose();
            ds.Reset();
            ds = gblcls.getdownloadpattern(strfrmdate, strtodate, "DownloadPattern");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataview = gblcls.GetUpDowndstodataview(ds);
                GridEOD.DataSource = dataview;
                GridEOD.DataBind();
                GridEOD.Visible = true;
                GridQuality.Visible = false;
                Togglepanel(PanelAssign);
            }
        }
    }

    protected void Lnkbtnupload_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);

        if (strfrmdate != "" && strtodate != "")
        {
            //gblcls.ClearDownloaduploadpattern();
            //ds1.Dispose();
            //ds1.Reset();
            //ds1 = gblcls.getdownloadpattern(strfrmdate, strtodate, "UploadPattern");
            //if (ds1.Tables[0].Rows.Count > 0)
            //{
            //    DetailsUpload.DataSource = ds1;
            //    DetailsUpload.DataBind();
            //    InsertDownloaduploadpattern(DetailsUpload, "Upload");
            //}
            //FetchUploadpattern();
            //PanelUploadDownload.Visible = false;

            ds.Dispose();
            ds.Reset();
            ds = gblcls.getdownloadpattern(strfrmdate, strtodate, "UploadPattern");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataview = gblcls.GetUpDowndstodataview(ds);
                GridEOD.DataSource = dataview;
                GridEOD.DataBind();
                GridEOD.Visible = true;
                GridQuality.Visible = false;
                Togglepanel(PanelAssign);
            }
        }
    }

    private void InsertDownloaduploadpattern(DetailsView detailsview, string patterntype)
    {
        foreach (DetailsViewRow dview in detailsview.Rows)
        {
            string time = dview.Cells[0].Text;
            string count = dview.Cells[1].Text;
            if (count != "&nbsp;")
            {
                gblcls.InsertPattern(time, count, patterntype);
            }
        }
    }
    private void FetchDownloadpattern()
    {
        ds.Dispose();
        ds.Reset();
        string query = "select Time,Download from downloaduploadpattern";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridEOD.DataSource = ds;
            GridEOD.DataBind();
            GridEOD.Visible = true;
            GridQuality.Visible = false;
        }
    }
    private void FetchUploadpattern()
    {
        ds.Dispose();
        ds.Reset();
        string query = "select Time,Upload from downloaduploadpattern";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridEOD.DataSource = ds;
            GridEOD.DataBind();
            GridEOD.Visible = true;
            GridQuality.Visible = false;
        }
    }
    #endregion

    #region Logout Reason
    protected void Lnklogout_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);

        if (strfrmdate != "" && strtodate != "")
        {
            string query = "select Order_No,Dt,User_ID,File_Status,Reason,time(Start_time) as Start_time,time(Cancel_time) as Cancel_time,timediff(Cancel_time,Start_time) as TAT from cancel_def where Dt between '" + strfrmdate + "' and '" + strtodate + "' order by Dt,User_ID";
            ds.Dispose();
            ds.Reset();
            ds = con.ExecuteQuery(query);
            ShowGridLogoutReason(ds);
        }
        else
        {
            lblerror.Text = "Please select from date and to date as same";
        }
    }
    public void ShowGridLogoutReason(DataSet ds)
    {
        dataview = gblcls.CovertLogoutDStoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }
    #endregion

    #region Cheque Payable
    private void ToogleButton(Button sButton)
    {
        btnsavecheque.Visible = false;
        btnnewcheque.Visible = false;

        sButton.Visible = true;
    }
    protected void Lnkcheqpay_Click(object sender, EventArgs e)
    {
        Togglepanel(PanelCheque);
        lblusername.Visible = false;
        ddlusername.Visible = false;
    }

    protected void Lnkviewcheq_Click(object sender, EventArgs e)
    {
        try
        {
            lblusername.Visible = false;
            ddlusername.Visible = false;

            ds.Dispose();
            ds.Reset();
            string strquery = "select Cheque_payable,Address,Charges,Req_Type,Tax_type from cheque_payable_details Order by Cheque_payable";
            ds = con.ExecuteQuery(strquery);
            ShowGridCheque(ds);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    private void ShowGridCheque(DataSet ds)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataview = gblcls.CovertChequeDStoDataview(ds);
                GridEOD.DataSource = dataview;
                GridEOD.DataBind();
                GridEOD.Visible = true;
                GridQuality.Visible = false;
                Togglepanel(PanelAssign);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    private void ClearText()
    {
        txtchqpay.Text = "";
        ddlrequesttype.SelectedIndex = 0;
        txtaddress.Text = "";
        txtamount.Text = "";
        txttaxtype.Text = "";
    }
    protected void btngetaddress_Click(object sender, EventArgs e)
    {
        string req_type = "";
        ds.Dispose();
        ds.Reset();
        if (txtchqpay.Text != "")
        {
            string strquery = "Select Address,Charges,Req_Type,Tax_type from cheque_payable_details where Cheque_payable='" + txtchqpay.Text + "' limit 1";
            ds = con.ExecuteQuery(strquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtamount.Text = ds.Tables[0].Rows[0]["Charges"].ToString();
                txttaxtype.Text = ds.Tables[0].Rows[0]["Tax_type"].ToString();

                req_type = ds.Tables[0].Rows[0]["Req_Type"].ToString();
                if (req_type == "UPS") ddlrequesttype.SelectedIndex = 1;
                else if (req_type == "UPS/R") ddlrequesttype.SelectedIndex = 2;
                else if (req_type == "UPS/SASE") ddlrequesttype.SelectedIndex = 3;
                else if (req_type == "REGULAR") ddlrequesttype.SelectedIndex = 4;
                else if (req_type == "THANKS REQUEST") ddlrequesttype.SelectedIndex = 5;
            }
        }
    }
    protected void btnnewcheque_Click(object sender, EventArgs e)
    {
        ClearText();
        ToogleButton(btnsavecheque);

    }
    protected void btnsavecheque_Click(object sender, EventArgs e)
    {
        int success = gblcls.InsertChequePayable(txtchqpay.Text.Trim(), ddlrequesttype.SelectedItem.Text, txtaddress.Text, txtamount.Text, txttaxtype.Text);
        ToogleButton(btnnewcheque);
        lblerror.Text = "New Cheque Payable Details added Successfully.";
    }
    protected void btnupdatecheque_Click(object sender, EventArgs e)
    {
        int success = gblcls.UpdateChequePayable(txtchqpay.Text.Trim(), ddlrequesttype.SelectedItem.Text, txtaddress.Text, txtamount.Text, txttaxtype.Text);
        ClearText();
        lblerror.Text = "Cheque Payable Details Updated Successfully.";
    }
    protected void btndeletecheque_Click(object sender, EventArgs e)
    {
        string strquery = "delete from cheque_payable_details where Cheque_payable='" + txtchqpay.Text + "' limit 1";
        int result = con.ExecuteSPNonQuery(strquery);
        if (result > 0)
        {
            lblerror.Text = "Cheque Payable Details Deleted Successfully.";
            ClearText();
        }
    }
    //protected void Lnkstatecmd_Click(object sender, EventArgs e)
    //{
    //    Togglepanel(PanelStateComments);
    //    lblusername.Visible = false;
    //    ddlusername.Visible = false;
    //    txtstate.Text = "";
    //    txtstateaddress.Text = "";
    //}
    protected void btnstateaddress_Click(object sender, EventArgs e)
    {
        string state = txtstate.Text;
        string strquery = "";
        if (HIDCommentsType.Value.ToString() == "ST")
        {
            strquery = "select State_Comment from state_comments where  state='" + state + "' limit 1";
        }
        else if (HIDCommentsType.Value.ToString() == "GEN")
        {
            strquery = "select Comments from getcomments where  State='" + state + "' Order by id";
        }
        //else if (RDStateComments.Checked == false && RDGetComments.Checked == false)
        //{
        //    errlable.Text = "Please Select Comment Type..!";
        //}

        if (strquery != "")
        {
            string result = con.ExecuteScalar(strquery);
            if (result != "")
            {
                txtstateaddress.Text = result;
                btnstatesave.Enabled = false;
                btnstateupdate.Enabled = true;
            }
            else
            {

                btnstatesave.Enabled = true;
                btnstateupdate.Enabled = false;
                txtstateaddress.Text = "";
                errlable.Text = "State not available..";
            }
        }
    }
    protected void btnstatesave_Click(object sender, EventArgs e)
    {

        string Cmnttype = "";
        if (HIDCommentsType.Value.ToString() == "ST")
        {
            Cmnttype = "state";
        }
        else if (HIDCommentsType.Value.ToString() == "GEN")
        {
            Cmnttype = "get";
        }
        else
        {
            errlable.Text = "Please Select Comment Type..!";
        }


        if (txtstate.Text == "" || txtstateaddress.Text == "") { errlable.Text = "Please Enter the State and State Comments"; }
        int success = gblcls.InsertStatecomment(txtstate.Text.Trim(), txtstateaddress.Text, Cmnttype);
        if (success > 0)
        {
            errlable.Text = "New State Comment added Successfully.";
        }
        else errlable.Text = "State Comment Already Available.";
    }
    protected void btnstateupdate_Click(object sender, EventArgs e)
    {
        if (txtstate.Text == "" || txtstateaddress.Text == "") { errlable.Text = "Please Enter the State and State Comments"; }
        string query = "";
        if (HIDCommentsType.Value.ToString() == "ST")
        {
            query = "Update state_comments set state='" + txtstate.Text + "',State_Comment='" + txtstateaddress.Text + "' where state='" + txtstate.Text + "' limit 1";
            int result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                errlable.Text = "State Comment Details Updated Successfully.";
            }
        }
        else if (HIDCommentsType.Value.ToString() == "GEN")
        {
            query = "Update getcomments set Comments='" + txtstateaddress.Text + "' where State='General' limit 1";
            int result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                errlable.Text = "General Comment Details Updated Successfully.";


                string Query = "Select * from getcomments where State='General'";
                DataSet Ds = new DataSet();
                ds = con.ExecuteQuery(Query);

                if (ds != null && ds.Tables.Count > 0)
                {
                    txtstateaddress.Text = Convert.ToString(ds.Tables[0].Rows[0]["Comments"]);
                }
            }
        }





    }
    protected void btnstatedelete_Click(object sender, EventArgs e)
    {
        if (txtstate.Text == "") { errlable.Text = "Please Enter the State"; }
        string strquery = "";
        if (HIDCommentsType.Value.ToString() == "ST")
        {
            strquery = "delete from state_comments where state='" + txtstate.Text + "' limit 1";
        }
        else if (HIDCommentsType.Value.ToString() == "GEN")
        {

            strquery = "delete from getcomments where State='" + txtstate.Text + "' limit 1";
        }

        int result = con.ExecuteSPNonQuery(strquery);
        if (result > 0)
        {
            errlable.Text = "State Comment Details Deleted Successfully.";
        }
    }
    #endregion

    #region TAT Report
    protected void Lnkbtntatreport_Click(object sender, EventArgs e)
    {
         GridTat.DataSource = null;
        GridTat.DataBind();
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);

        if (strfrmdate != "" && strtodate != "")
        {
            ds.Dispose();
            ds.Reset();
            ds = gblcls.Tatreport(strfrmdate, strtodate);
            //GridTat.DataSource = ds.Tables[0];
            //GridTat.DataBind();
            //GridTat.Visible = true;
            //GridQuality.Visible = false;
            //Togglepanel(PanelAssign);
            ShowTatReport(ds);
        }
        else
        {
            lblerror.Text = "Please select from date and to date";
        }
    }
    protected void Lnkbtntatreportdetail_Click(object sender, EventArgs e)
    {
        Gridtatreport.DataSource = null;
        Gridtatreport.DataBind();
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);

        if (strfrmdate != "" && strtodate != "")
        {
            ds.Dispose();
            ds.Reset();
            ds = gblcls.Tatreportdetail(strfrmdate, strtodate);
            GridTat.DataSource = ds.Tables[0];
            GridTat.DataBind();
            GridTat.Visible = true;
            GridQuality.Visible = false;
            Gridtatreport.Visible = false;
            GridEOD.Visible = false;
            Togglepanel(PanelAssign);
            //ShowTatReport(ds);
        }
        else
        {
            lblerror.Text = "Please select from date and to date";
        }
    }
    public void ShowTatReport(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            dataview = gblcls.CovertTatDstoDataview(ds);
           // dataview = gblcls.CovertTatreportDstoDataview(ds);

            Gridtatreport.DataSource = dataview;
            Gridtatreport.DataBind();
            Gridtatreport.Visible = true;
            GridQuality.Visible = false;
            GridEOD.Visible = false;
            GridTat.Visible = false;
            Togglepanel(PanelAssign);
        }
        else
        {
            Gridtatreport.DataSource = null;
            Gridtatreport.DataBind();
            Gridtatreport.Visible = true;
            GridQuality.Visible = false;
            GridEOD.Visible = false;
            GridTat.Visible = false;
        }
    }
    #endregion

    #region Project Report
    protected void LnkIndividual_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:yyyy-MM-dd}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:yyyy-MM-dd}", df);
        if (strfrmdate != strtodate) { lblerror.Text = "Please select from date and to date as same."; return; }
        if (strfrmdate == "" && strtodate == "") { lblerror.Text = "Please select from date and to date"; return; }

        ds.Dispose();
        ds.Reset();
        ds = gblcls.IndividualReport(strfrmdate, strtodate, "update");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string strquery = "Update individual_report set Target='" + ds.Tables[0].Rows[i]["TotTarget"] + "' where Username='" + ds.Tables[0].Rows[i]["Name"] + "' limit 1;";
                con.ExecuteSPNonQuery(strquery);
            }
        }
        ds.Dispose();
        ds.Reset();
        ds = gblcls.IndividualReport(strfrmdate, strtodate, "select");

        dataview = gblcls.CovertIndivDStoDataview(ds);
        if (dataview.Count > 0)
        {
            GridEOD.DataSource = dataview;
            GridEOD.DataBind();
            GridEOD.Visible = true;
            GridQuality.Visible = false;
            Togglepanel(PanelAssign);
        }
        else
        {
            GridEOD.DataSource = null;
            GridEOD.DataBind();
            GridEOD.Visible = true;
        }
    }
    protected void LnkProjectReport_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:yyyy-MM-dd}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:yyyy-MM-dd}", df);

        if (strfrmdate == "" && strtodate == "") { lblerror.Text = "Please select from date and to date"; return; }

        ds.Dispose();
        ds.Reset();
        ds = gblcls.ProjectReport(strfrmdate, strtodate, "update");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string strquery = "Update individual_report set Target='" + ds.Tables[0].Rows[i]["TotTarget"] + "' where Username='" + ds.Tables[0].Rows[i]["Name"] + "' and pdate='" + ds.Tables[0].Rows[i]["pdate"] + "' limit 1;";
                con.ExecuteSPNonQuery(strquery);
            }
        }
        ds.Dispose();
        ds.Reset();
        ds = gblcls.ProjectReport(strfrmdate, strtodate, "select");

        dataview = gblcls.CovertProjectDStoDataview(ds);
        if (dataview.Count > 0)
        {
            GridEOD.DataSource = dataview;
            GridEOD.DataBind();
            GridEOD.Visible = true;
            GridQuality.Visible = false;
            Togglepanel(PanelAssign);
        }
        else
        {
            GridEOD.DataSource = null;
            GridEOD.DataBind();
            GridEOD.Visible = true;
        }
    }
    protected void LnkThroughput_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:yyyy-MM-dd}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:yyyy-MM-dd}", df);

        if (strfrmdate == "" && strtodate == "") { lblerror.Text = "Please select from date and to date"; return; }
        dsTER.Dispose();
        dsTER.Reset();
        dsTER = gblcls.PendingReport(strfrmdate, strtodate, "sp_Throughput_Report");
        Togglepanel(PanelThroughput);
    }
    protected void LnkPendingReport_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);

        if (strfrmdate == "" && strtodate == "") { lblerror.Text = "Please select from date and to date"; return; }
        ds.Dispose();
        ds.Reset();
        ds = gblcls.PendingReport(strfrmdate, strtodate, "sp_Untouch_OrdersReport");
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridEOD.DataSource = ds;
            GridEOD.DataBind();
            GridEOD.Visible = true;
            GridQuality.Visible = false;
            Togglepanel(PanelAssign);
        }
        else
        {
            GridEOD.DataSource = null;
            GridEOD.DataBind();
            GridEOD.Visible = true;
        }
    }
    protected void LnkAutoFtp_Click(object sender, EventArgs e)
    {
        lblusername.Visible = false;
        ddlusername.Visible = false;

        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:dd-MM-yyyy}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:dd-MM-yyyy}", df);

        if (strfrmdate == "" && strtodate == "") { lblerror.Text = "Please select from date and to date"; return; }
        ds.Dispose();
        ds.Reset();
        ds = gblcls.PendingReport(strfrmdate, strtodate, "sp_AutoFTP_LogReport");
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridEOD.DataSource = ds;
            GridEOD.DataBind();
            GridEOD.Visible = true;
            GridQuality.Visible = false;
            Togglepanel(PanelAssign);
        }
        else
        {
            GridEOD.DataSource = null;
            GridEOD.DataBind();
            GridEOD.Visible = true;
        }
    }
    #endregion

    #region Export
    protected void Lnkexport_Click(object sender, ImageClickEventArgs e)
    {
        if (PanelThroughput.Visible == false)
        {
            if (GridEOD.Rows.Count > 0)
            {
                GridViewExportUtil.Export("Report.xls", this.GridEOD);
            }
            else if (GridQuality.Rows.Count > 0)
            {
                GridViewExportUtil.Export("Report.xls", this.GridQuality);
            }
            else if (Gridtatreport.Rows.Count > 0)
            {
                GridViewExportUtil.Export("TatReport.xls", this.Gridtatreport);
            }
            else if (GridTat.Rows.Count > 0)
            {
                GridViewExportUtil.Export("TatReportDetail.xls", this.GridTat);
            }
            else if (PanelBreak.Visible == true)
            {
                breaktimereport();

            }
        }
        else if (PanelThroughput.Visible == true)
        {
            ExportThroughputReport();

        }
       
    }

    private void ExportThroughputReport()
    {
        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:yyyy-MM-dd}", dt);

        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:yyyy-MM-dd}", df);

        if (strfrmdate == "" && strtodate == "") { lblerror.Text = "Please select from date and to date"; return; }
        dsTER.Dispose();
        dsTER.Reset();
        dsTER = gblcls.PendingReport(strfrmdate, strtodate, "sp_Throughput_Data");

        if (dsTER.Tables[0].Rows.Count > 0)
        {
            Response.AppendHeader("Content-Disposition", "attachment; filename=Throughput_Report.xls");
            Response.ContentType = "application/ms-excel";

            Response.Write("<table border='solid 1px #525252'>");
            Response.Write("<thead>");
            Response.Write("<tr>");
            Response.Write("<th rowspan='2'>Sl.No</th>");
            int strcolcount = (dsTER.Tables[0].Columns.Count - 8) / 4;
            int colcnt = 0;
            for (int i = 0; i < strcolcount + 1; i++)
            {
                if (i == 0)
                {
                    Response.Write("<th rowspan='2'>" + dsTER.Tables[0].Columns[0].ToString() + "</th>");
                }
                else
                {
                    if (i == 1) colcnt = i;
                    else colcnt = colcnt + 4;
                    Response.Write(" <th colspan='4'>" + dsTER.Tables[0].Columns[colcnt].ToString() + "</th>");
                }
            }
            Response.Write("<th rowspan='2'>Total Target</th>");
            Response.Write("<th rowspan='2'>Total Achieved</th>");
            Response.Write("<th rowspan='2'>Efficiency</th>");
            Response.Write("<th rowspan='2'>Total Production Achieved</th>");
            Response.Write("<th rowspan='2'>Avg Production time</th>");
            Response.Write("<th rowspan='2'>Total QC Achieved</th>");
            Response.Write("<th rowspan='2'>Avg QC Time</th>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            for (int i = 0; i < dsTER.Tables[1].Rows.Count; i++)
            {
                Response.Write("<th>Target</th>");
                Response.Write("<th>Production Achieved</th>");
                Response.Write("<th>Target</th>");
                Response.Write("<th>QC Achieved</th>");
            }
            Response.Write("</tr>");
            Response.Write("</thead>");
            Response.Write("<tbody>");
            for (int i = 0; i < dsTER.Tables[0].Rows.Count; i++)
            {
                Response.Write("<tr>");
                Response.Write("<td>" + (i + 1) + "</td>");
                for (int j = 0; j < dsTER.Tables[0].Columns.Count; j++)
                {
                    Response.Write("<td>" + dsTER.Tables[0].Rows[i][j].ToString() + "</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</tbody>");
            Response.Write("</table>");
            Response.End();
        }
        else return;
    }

    private void breaktimereport()
    {
        DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);
        DateTime df = Convert.ToDateTime(txttodate.Text);
        strtodate = String.Format("{0:MM/dd/yyyy}", df);

      

        if (strfrmdate == "" && strtodate == "") { lblerror.Text = "Please select from date and to date"; return; }
        dsTER.Dispose();
        dsTER.Reset();
        dsTER = gblcls.PendingReport(strfrmdate, strtodate, "sp_breakreport_new");

        if (dsTER.Tables[0].Rows.Count > 0)
        {
            Response.AppendHeader("Content-Disposition", "attachment; filename=BreakTime.xls");
            Response.ContentType = "application/ms-excel";

            Response.Write("<table border='solid 1px #525252'>");
            Response.Write("<thead>");
            Response.Write("<tr>");
            Response.Write("<th rowspan='2'>Sl.No</th>");
            int strcolcount = (dsTER.Tables[0].Columns.Count - 8) / 4;
            int colcnt = 0;
            for (int i = 0; i < strcolcount + 1; i++)
            {
                if (i == 0)
                {
                    Response.Write("<th rowspan='2'>" + dsTER.Tables[0].Columns[0].ToString() + "</th>");
                }
                else
                {
                    if (i == 1) colcnt = i;
                    else colcnt = colcnt + 4;
                    Response.Write(" <th colspan='4'>" + dsTER.Tables[0].Columns[colcnt].ToString() + "</th>");
                }
            }
            Response.Write("<th rowspan='2'>Name</th>");
            Response.Write("<th rowspan='2'>Date</th>");
            Response.Write("<th rowspan='2'>Time Taken</th>");
            //Response.Write("<th rowspan='2'>Total Production Achieved</th>");
            //Response.Write("<th rowspan='2'>Avg Production time</th>");
            //Response.Write("<th rowspan='2'>Total QC Achieved</th>");
            //Response.Write("<th rowspan='2'>Avg QC Time</th>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            for (int i = 0; i < dsTER.Tables[1].Rows.Count; i++)
            {
                //Response.Write("<th>Date</th>");
                //Response.Write("<th>Name</th>");
                //Response.Write("<th>Break Reason</th>");
                //Response.Write("<th>TimeTaken</th>");
            }
            Response.Write("</tr>");
            Response.Write("</thead>");
            Response.Write("<tbody>");
            for (int i = 0; i < dsTER.Tables[0].Rows.Count; i++)
            {
                Response.Write("<tr>");
                Response.Write("<td>" + (i + 1) + "</td>");
                for (int j = 0; j < dsTER.Tables[0].Columns.Count; j++)
                {
                    Response.Write("<td>" + dsTER.Tables[0].Rows[i][j].ToString() + "</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("<th rowspan='2'>Sl.No</th>");
            Response.Write("<th rowspan='2'>Date</th>");
            Response.Write("<th rowspan='2'>Name</th>");
            Response.Write("<th rowspan='2'>Break Reason</th>");
            Response.Write("<th rowspan='2'>Start Time</th>");
            Response.Write("<th rowspan='2'>End Time</th>");
            Response.Write("<th rowspan='2'>Total Time</th>");
            
            Response.Write("</tr>");
            Response.Write("<tr>");
            for (int i = 0; i < dsTER.Tables[1].Rows.Count; i++)
            {
                Response.Write("<tr>");
                Response.Write("<td>" + (i + 1) + "</td>");
                for (int j = 0; j < dsTER.Tables[1].Columns.Count; j++)
                {
                    Response.Write("<td>" + dsTER.Tables[1].Rows[i][j].ToString() + "</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</tbody>");
            Response.Write("</table>");
            Response.End();
          
          
        }
        else return;
    }
    protected void ExportFpyReport()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=FPY_Reports.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        PrepareForExport(GridView1);
        PrepareForExport(GridView2);
        PrepareForExport(GridEOD);

        Table tb = new Table();
        TableRow tr1 = new TableRow();

        TableCell cell1 = new TableCell();
        cell1.Controls.Add(GridView1);
        tr1.Cells.Add(cell1);

        TableCell cell3 = new TableCell();
        cell3.Controls.Add(GridView2);

        TableCell cell5 = new TableCell();
        cell5.Controls.Add(GridEOD);

        TableCell cell2 = new TableCell();
        cell2.Text = "&nbsp;";

        TableCell cell4 = new TableCell();
        cell4.Text = "&nbsp;";

        TableRow tr2 = new TableRow();
        tr2.Cells.Add(cell2);

        TableRow tr3 = new TableRow();
        tr3.Cells.Add(cell3);

        TableRow tr4 = new TableRow();
        tr4.Cells.Add(cell4);

        TableRow tr5 = new TableRow();
        tr5.Cells.Add(cell5);

        tb.Rows.Add(tr1);
        tb.Rows.Add(tr2);
        tb.Rows.Add(tr3);
        tb.Rows.Add(tr4);
        tb.Rows.Add(tr5);

        tb.RenderControl(hw);
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }
    protected void PrepareForExport(GridView Gridview)
    {
        //Gridview.AllowPaging = Convert.ToBoolean(rbPaging.SelectedItem.Value);
        //Gridview.DataBind();

        //Change the Header Row back to white color
        Gridview.HeaderRow.Style.Add("background-color", "#FFFFFF");

        //Apply style to Individual Cells
        for (int k = 0; k < Gridview.HeaderRow.Cells.Count; k++)
        {
            Gridview.HeaderRow.Cells[k].Style.Add("background-color", "Gray");
        }

        for (int i = 0; i < Gridview.Rows.Count; i++)
        {
            GridViewRow row = Gridview.Rows[i];

            //Change Color back to white
            row.BackColor = System.Drawing.Color.White;

            //Apply text style to each Row
            row.Attributes.Add("class", "textmode");

            //Apply style to Individual Cells of Alternating Row
            if (i % 2 != 0)
            {
                for (int j = 0; j < Gridview.Rows[i].Cells.Count; j++)
                {
                    row.Cells[j].Style.Add("background-color", "#C2D69B");
                }
            }
        }
    }
    #endregion

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

    #region State Comments
    protected void Lnkstatecmd_Click(object sender, EventArgs e)
    {
        LblTittle.Text = "State Comments";
        LblSubtittle.Text = "State";
        CommentTittle.Text = "State Comment";
        DivState.Visible = true;
        LblSubtittle.Visible = true;
        btnstatesave.Visible = true;
        btnstateaddress.Visible = true;

        HIDCommentsType.Value = "ST";
        errlable.Text = "";
        Togglepanel(PanelStateComments);
        lblusername.Visible = false;
        ddlusername.Visible = false;
        txtstate.Text = "";
        txtstateaddress.Text = "";
        btnstatesave.Enabled = true;
    }

    protected void LnkGeneralcmd_Click(object sender, EventArgs e)
    {
        string Query = "Select * from getcomments where State='General'";
        DataSet Ds = new DataSet();
        ds = con.ExecuteQuery(Query);

        if (ds != null && ds.Tables.Count > 0)
        {
            txtstateaddress.Text = Convert.ToString(ds.Tables[0].Rows[0]["Comments"]);
        }

        LblTittle.Text = "General Comments";
        LblSubtittle.Visible = false;
        CommentTittle.Text = "General Comment";
        DivState.Visible = false;
        btnstatesave.Visible = false;
        btnstateaddress.Visible = false;
        HIDCommentsType.Value = "GEN";
        errlable.Text = "";
        Togglepanel(PanelStateComments);
        lblusername.Visible = false;
        ddlusername.Visible = false;
        btnstatesave.Enabled = true;
        btnstatedelete.Visible = false;
    }
    #endregion
    protected void lnkhourcounttotal_Click(object sender, EventArgs e)
    {
        {
            lblusername.Visible = false;
            ddlusername.Visible = false;

            string username = "";
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                ds = gblcls.GetHourlyCounttotal(strfrmdate, strtodate);
                ShowGridHourlyCount(ds);

            }
            else
            {
                lblerror.Text = "Please select from date and to date";
            }
        }

    }

    protected void lnkovrprcstmerept_Click(object sender, EventArgs e)
    {
        {
            lblusername.Visible = false;
            ddlusername.Visible = false;

            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                ds = gblcls.OverallProcessTimeReport(strfrmdate, strtodate);
                ShowGridOverallProcessTime(ds);
            }
            else
            {
                lblerror.Text = "Select from and to date";
            }
        }
    }
    public void ShowGridOverallProcessTime(DataSet ds)
    {
        dataview = gblcls.CovertOverallProcessTimetoDataview(ds);
        GridEOD.DataSource = dataview;
        GridEOD.DataBind();
        GridEOD.Visible = true;
        GridQuality.Visible = false;
        Togglepanel(PanelAssign);
    }

    protected void GridTat_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string otype = e.Row.Cells[5].Text;
            int pdate = Convert.ToInt32(e.Row.Cells[7].Text.Replace("/", ""));
            int ddate = Convert.ToInt32(e.Row.Cells[8].Text.Replace("/", ""));

           DateTime pp = Convert.ToDateTime(e.Row.Cells[7].Text);
           DateTime dp = Convert.ToDateTime(e.Row.Cells[8].Text);

           TimeSpan t = dp - pp;
           int NrOfDays = Convert.ToInt32(t.TotalDays);
           if (otype == "Website")
           {
               if (pdate == ddate)
               {
                   e.Row.Cells[6].Text = "YES";
               }
               else
               {
                   e.Row.Cells[6].Text = "NO";
               }
           }
           else if(otype == "Phone")
           {

               if (3 >= NrOfDays)
               {
                   e.Row.Cells[6].Text = "YES";
               }
               else
               {
                   e.Row.Cells[6].Text = "NO";
               }
           }
           else if (otype == "Mailaway")
           {
               if (10 >= NrOfDays)
               {
                   e.Row.Cells[6].Text = "YES";
               }
               else
               {
                   e.Row.Cells[6].Text = "NO";
               }
           }

           e.Row.Cells[7].Visible = false;
           e.Row.Cells[8].Visible = false;
        }
    }
    protected void Lnk_break_rpt(object sender, EventArgs e)
    {
        {
            lblusername.Visible = false;
            ddlusername.Visible = false;

            string username = "";
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM-dd-yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM-dd-yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                ds = gblcls.getbreaktimetot(strfrmdate, strtodate);
                grd_break_total.DataSource = ds.Tables[0];
                grd_break_total.DataBind();
                GridEOD.Visible = false;
                GridQuality.Visible = false;
                Togglepanel(PanelBreak);

            }
            else
            {
                lblerror.Text = "Please select from date and to date";
            }


        }
    }
    protected void grd_break_total_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM-dd-yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM-dd-yyyy}", df);

            string name = e.Row.Cells[1].Text.ToString();
            string pdate = e.Row.Cells[2].Text.ToString();
            GridView gvOrders = e.Row.FindControl("gvOrders") as GridView;
            if (strfrmdate != "" && strtodate != "")
            {
                
                ds = gblcls.getbreaktimetot1(strfrmdate, strtodate,name,pdate);
                gvOrders.DataSource = ds.Tables[0];
                gvOrders.DataBind();
               
            }         
        }
    }
    protected void Gridtatreport_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
        //    strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

        //    DateTime df = Convert.ToDateTime(txttodate.Text);
        //    strtodate = String.Format("{0:MM/dd/yyyy}", df);

        //    string name = e.Row.Cells[1].Text.ToString();
        //   // string pdate = e.Row.Cells[1].Text.ToString();
        //    GridView GridTat = e.Row.FindControl("GridTat") as GridView;
        //    if (strfrmdate != "" && strtodate != "")
        //    {

        //        ds = gblcls.Tatreportdetail(strfrmdate, strtodate, name);
        //        GridTat.DataSource = ds.Tables[0];
        //        GridTat.DataBind();

        //    }
        //}
    }
    protected void Lnkeod3_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(txtfrmdate.Text);
            strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);

            DateTime df = Convert.ToDateTime(txttodate.Text);
            strtodate = String.Format("{0:MM/dd/yyyy}", df);

            if (strfrmdate != "" && strtodate != "")
            {
                ds.Dispose();
                ds.Reset();
                ds1.Dispose();
                ds1.Reset();
                string strquery = "select Order_no,Pdate,rs.State,ts.Tier,County,Prior,Webphone,Borrowername,Township,Lock1,K1,QC,Pend,Status,Parcel,Tax,Rejected,K1_OP,QC_OP,DeliveredDate,Lastcomment,serpro from record_status rs join tier_states ts on ts.State = rs.State where DeliveredDate between '" + txtfrmdate.Text + "' and '" + txttodate.Text + "' and (k1='2' or k1='7' or k1='5' or k1='4' or k1='9') and (qc='2' or qc='7' or qc='5' or qc='4' or qc='9') and (status='2' or status='7' or status='5' or status='4' or status='9' ) and serpro='Tax Typing - Advanced' and (parcel='3' or parcel='0') and pend<>'3' and tax<>'3'";
                //string strquery1 = "select Order_no,Pdate,State,County,Prior,Webphone,Borrowername,Township,Lock1,K1,QC,Pend,Status,Parcel,Tax,Rejected,K1_OP,QC_OP,DeliveredDate,Lastcomment,serpro from record_status where pdate  between '" + strfrmdate + "' and '" + strtodate + "' and (k1='2' or k1='7' or k1='5' or k1='4' or  k1='9') and (qc='2' or qc='7' or qc='5' or qc='4' or qc='9') and (status='2' or status='7' or status='5' or status='4' or status='9' ) and serpro='Tax Typing - Advanced' and (parcel='3' or parcel='0') and pend<>'3' and tax<>'3'"; ;
                string strquery1 = "select Order_no,Pdate,rs.State,ts.Tier,County,Prior,Webphone,Borrowername,Township,Lock1,K1,QC,Pend,Status,Parcel,Tax,Rejected,K1_OP,QC_OP,DeliveredDate,Lastcomment,serpro from record_status rs join tier_states ts on ts.State = rs.State where Pdate not between '" + strfrmdate + "' and '" + strtodate + "' and  (tax='3' or tax='0') and parcel<>'3' and (K1='2' or K1='4') and (qc='2' or qc='4') and (status='2' or status='4') and  serpro='Tax Typing - Advanced'";
                ds = con.ExecuteQuery(strquery);
                ds1 = con.ExecuteQuery(strquery1);
                ds.Merge(ds1);
                ShowGridEOD2(ds);
                lblusername.Visible = false;
                ddlusername.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
}