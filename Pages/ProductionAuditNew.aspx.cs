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
using System.IO;
using System.Net;
using MySql.Data.MySqlClient;
using System.Net.Mail;

public partial class Pages_ProductionAuditNew : System.Web.UI.Page
{
    GlobalClass gl = new GlobalClass();
    myConnection con = new myConnection();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt = new DateTime();
    MySqlParameter[] mparam;
    string strdate = "";

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        strdate = gl.setdate();
        Lblusername.Text = "Welcome " + SessionHandler.UserName + " ..";
        Lbltime.Text = DateTime.Now.ToLongDateString();
        if (!Page.IsPostBack)
        {
            break_mis();
            Session["TimePro"] = DateTime.Now;
            string id = Request.QueryString["id"];
            Defaultcontrol();
            if (id == "12f7tre5") Allotment(sender, e);
            else ManualAllotment(id, sender, e);
        }
            
        if (lnkOthers.Text == "UnBreak")
        {
            pagedimmer.Visible = true;
            Other_breakMsgbx.Visible = true;
        }

        else
        {
            pagedimmer.Visible = false;
            Other_breakMsgbx.Visible = false;

        }
    }

    public bool CheckOrderallotment()
    {
        string fdate = "", tdate = "";

        DateTime df = gl.ToDate();
        tdate = df.ToString("MM/dd/yyyy");
        fdate = tdate;

        string count = "";
        ds.Dispose();
        ds.Reset();
        string strquery = "select count(Order_No) as Orders from record_status where DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + fdate + "' and '" + tdate + "' and ((K1_OP='" + SessionHandler.UserName + "' and Error='Error' and Error_Comments is null) or (QC_OP='" + SessionHandler.UserName + "' and Error1='Error' and Error_Comments1 is null))";
        ds = con.ExecuteQuery(strquery);
        //string count = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            count = ds.Tables[0].Rows[0]["Orders"].ToString();
        }
        if (count == "0") return true;
        else return false;
    }

    public void LoadTaxType()
    {
        ds.Dispose();
        ds.Reset();
        string query = "select TaxType from mailaway_taxtype";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddltaxtype1.DataSource = ds;
            ddltaxtype1.DataTextField = "TaxType";
            ddltaxtype1.DataBind();
            ddltaxtype1.Items.Insert(0, "-----Select Tax Type-----");
        }
    }

    public void LoadLogoutreason()
    {
        ds.Dispose();
        ds.Reset();
        string query = "select Logout_Type from logout_reasonlist";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddllogout.DataSource = ds;
            ddllogout.DataTextField = "Logout_Type";
            ddllogout.DataBind();
            ddllogout.Items.Insert(0, "");
        }
    }

    public void Loaderrorcategory()
    {
        ds.Dispose();
        ds.Reset();
        string strerror = ddlerror.SelectedItem.Text;
        string query = "select Error_Category from error_details_new where Error='" + strerror + "' Group by Error_Category";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlerrorcat.DataSource = ds;
            ddlerrorcat.DataTextField = "Error_Category";
            ddlerrorcat.DataBind();
            ddlerrorcat.Items.Insert(0, "");
        }
        ddlerrorarea.Items.Clear();
        ddlerrortype.Items.Clear();
        ddlcombined.Items.Clear();
    }

    public void Loaderrorarea()
    {
        ds.Dispose();
        ds.Reset();
        string strerrcat = ddlerrorcat.SelectedItem.Text;
        if (strerrcat == "") return;
        string query = "select Error_Area from error_details_new where Error_Category='" + strerrcat + "' group by Error_Area";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlerrorarea.DataSource = ds;
            ddlerrorarea.DataTextField = "Error_Area";
            ddlerrorarea.DataBind();
            ddlerrorarea.Items.Insert(0, "");
        }
        ddlerrortype.Items.Clear();
        ddlcombined.Items.Clear();
    }

    public void Loaderrortype()
    {
        ds.Dispose();
        ds.Reset();
        string strerrcat = ddlerrorcat.SelectedItem.Text;
        string strerrarea = ddlerrorarea.SelectedItem.Text;
        if (strerrcat == "" && strerrcat == "") return;
        string query = "select Error_Type from error_details_new where Error_Category='" + strerrcat + "' and Error_Area='" + strerrarea + "'";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlerrortype.DataSource = ds;
            ddlerrortype.DataTextField = "Error_Type";
            ddlerrortype.DataBind();
            ddlerrortype.Items.Insert(0, "");
        }
        ddlcombined.Items.Clear();
    }

    public void Loadcombined()
    {
        ds.Dispose();
        ds.Reset();
        string strerrcat = ddlerrorcat.SelectedItem.Text;
        string strerrarea = ddlerrorarea.SelectedItem.Text;
        string strerrtype = ddlerrortype.SelectedItem.Text;
        if (strerrcat == "" && strerrcat == "" && strerrtype == "") return;
        string query = "select Error_Combined from error_details_new where Error_Category='" + strerrcat + "' and Error_Area='" + strerrarea + "' and Error_Type='" + strerrtype + "'";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcombined.DataSource = ds;
            ddlcombined.DataTextField = "Error_Combined";
            ddlcombined.DataBind();
            ddlcombined.Items.Insert(0, "");
        }
    }

    public void LoadProductionComments(string strstatus)
    {
        ds.Dispose();
        ds.Reset();
        string query = "select Status_Comment from status_comments where Status='" + strstatus + "'";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlprdcomments.DataSource = ds;
            ddlprdcomments.DataTextField = "Status_Comment";
            ddlprdcomments.DataBind();
            ddlprdcomments.Items.Insert(0, "");
        }
    }
    #endregion

    #region TogglePanel
    private void TogglePanel(Panel sPanel)
    {
        PanelOrderallotment.Visible = false;
        PanelStatus.Visible = false;

        sPanel.Visible = true;
    }
    private void Defaultcontrol()
    {
        PanelQc.Visible = false;
        btnMovecall.Visible = false;
        btnrequest.Visible = false;
        Lnkcomments.Visible = false;
        Btnmoveqc.Visible = false;

        Lnkcomments.Attributes.Add("onclick", "window.open('MailAwayComments.aspx'); return false;");
        pagedimmer.Visible = false;
        ReportPanel.Visible = false;
        pagedimmer1.Visible = false;
        statecomments.Visible = false;
        getcomments.Visible = false;
        LogoutReason.Visible = false;
        Taxinfomail.Visible = false;
        LogoutText();
    }
    private void LogoutText()
    {
        if (Lblorderno.Text == "")
        { LogoutBtn.Text = "Back"; }
        else LogoutBtn.Text = "Logout";
    }
    private void Clearfields()
    {
        txtTreasphone.Text = "";
        txtTreasurer.Text = "";
        txtassessor.Text = "";
        txtassphone.Text = "";
        Lblprocessname.Text = "";
        Lblorderno.Text = "";
        Lbldate.Text = "";
        Lblstate.Text = "";
        txtqcerrorcmts.Text = "";
        txtgetcomments.Text = "";
        txtcommentshistory.Text = "";
        txtstatecomments.Text = "";
        lblprdcomments.Text = "";
        ddlemailtype.SelectedIndex = 0;
        txtcountyname.Text = "";
        Lblcouny.Text = "";
        txtordertype.Text = "";
        txtBorrower.Text = "";
        txttownship.Text = "";
        ddlstatus.Text = "";
        txtaddcomments.Text = "";
        ddlerror.SelectedIndex = 0;
        ddlprdcomments.SelectedIndex = 0;
        txtexcomments.Text = "";
        lblqcerrorcmts.Visible = true;
        txtqcerrorcmts.Visible = true;
        txtexcomments.Visible = false;
        chkclientcmt.Visible = false;
        chkclientcmt.Checked = false;
        chksavelink.Checked = false;
        lblkeycomplete.Visible = false;
        lblkeycomplete.Text = "";

        Griderrors.DataSource = null;
        Griderrors.DataBind();

        myVariables.Orderno = "";
        myVariables.Date = "";
        myVariables.State = "";
        myVariables.County = "";
        myVariables.WebPhone = "";
        myVariables.TimeZone = "";
        myVariables.Lastcomment = "";
        myVariables.Borrower = "";
        myVariables.Township = "";
        myVariables.KeyStatus = "";
        myVariables.QCStatus = "";
        myVariables.Ordertype = "";
        myVariables.Zipcode = "";
        myVariables.Followupdate = "";
        myVariables.pType = "";
        myVariables.ChecklistComments = "";
        myVariables.EntityComments = "";
    }
    private string GetTimeZone()
    {
        string tz = "";
        switch (myVariables.TimeZone)
        {
            case "1":
                tz = "EST";
                break;
            case "2":
                tz = "CST";
                break;
            case "3":
                tz = "MST";
                break;
            case "4":
                tz = "PST";
                break;
            case "5":
                tz = "AKST";
                break;
            case "6":
                tz = "HST";
                break;
            default:
                break;
        }
        return tz;
    }
    #endregion

    #region Allotment Types

    #region ManualAllotment
    private void ManualAllotment(string id, object sender, EventArgs e)
    {
        Clearfields();
        ds.Dispose();
        ds.Reset();
        ds = gl.Allotment("ManualAllotment", id);
        DatasetAllotment(ds, sender, e);
    }
    #endregion

    #region AutoAllotment
    private void Allotment(object sender, EventArgs e)
    {
        string locks = gl.get_user_lock(SessionHandler.UserName);
        if (locks == "2")
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "Your break time exceeded more than one hour. So please contact your Team Lead...! ";
            LogoutText();
            return;
        }
        else
        {
            Clearfields();
            ds.Dispose();
            ds.Reset();
            ds = gl.Allotment("Autoallotment", "");
            DatasetAllotment(ds, sender, e);
        }
    }
    #endregion

    #region FillData
    private void DatasetAllotment(DataSet ds, object sender, EventArgs e)
    {
        if (ds.Tables.Count == 0)
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No orders for process.";
            LogoutText();
            return;
        }
        else if (ds.Tables.Count == 1)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                string strresult = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                if (strresult == "Order Error")
                {
                    TogglePanel(PanelStatus);
                    lblinfo.Text = "You have an error in one Order. So please accept your error...!";
                    LogoutText();
                    return;
                }
                else if (strresult == "Production Locked")
                {
                    TogglePanel(PanelStatus);
                    lblinfo.Text = "Your break time exceeded more than one hour. So please contact your Team Lead...! ";
                    LogoutText();
                    return;
                }
                else if (strresult == "Check Rights")
                {
                    TogglePanel(PanelStatus);
                    lblinfo.Text = "Please check your Rights...!";
                    LogoutText();
                    return;
                }
            }
        }
        else
        {
            TogglePanel(PanelOrderallotment);
            FillDataset(ds);
            if (PanelOrderallotment.Visible == true) ddlstatus_SelectedIndexChanged(sender, e);
        }
    }

    private void FillDataset(DataSet ds)
    {
        string etypes = "", scount = "";

        #region Table 1
        if (ds.Tables[0].Rows.Count > 0)
        {
            myVariables.pType = Convert.ToString(ds.Tables[0].Rows[0]["pType"]);
            myVariables.Orderno = Convert.ToString(ds.Tables[0].Rows[0]["Order_No"]);
            myVariables.Date = Convert.ToString(ds.Tables[0].Rows[0]["PDate"]);
            myVariables.State = Convert.ToString(ds.Tables[0].Rows[0]["State"]);
            myVariables.County = Convert.ToString(ds.Tables[0].Rows[0]["County"]);
            myVariables.WebPhone = Convert.ToString(ds.Tables[0].Rows[0]["WebPhone"]);
            myVariables.TimeZone = Convert.ToString(ds.Tables[0].Rows[0]["Time_Zone"]);
            myVariables.HP = Convert.ToString(ds.Tables[0].Rows[0]["HP"]);
            myVariables.Prior = Convert.ToString(ds.Tables[0].Rows[0]["Prior"]);
            myVariables.Zipcode = Convert.ToString(ds.Tables[0].Rows[0]["zipcode"]);
            myVariables.Followupdate = Convert.ToString(ds.Tables[0].Rows[0]["followupdate"]);
            if (SessionHandler.AuditQA != "1") myVariables.Lastcomment = Convert.ToString(ds.Tables[0].Rows[0]["Comments_Det1"]);
            else myVariables.Lastcomment = Convert.ToString(ds.Tables[0].Rows[0]["Comments_Det"]);

            if (myVariables.pType == "KEY" || myVariables.pType == "PRIORITYKEY" || myVariables.pType == "DU" || myVariables.pType == "PRIORITYDU" || myVariables.pType == "PRIORKEY" || myVariables.pType == "PRIORDU")
            { myVariables.OrderTp = Convert.ToString(ds.Tables[0].Rows[0]["OrderType"]); }


            if (myVariables.pType == "QC")
            {
                myVariables.Borrower = Convert.ToString(ds.Tables[0].Rows[0]["borrowername"]);
                myVariables.Township = Convert.ToString(ds.Tables[0].Rows[0]["Township"]);
                myVariables.KeyStatus = Convert.ToString(ds.Tables[0].Rows[0]["key_status"]);
            }
            else if (myVariables.pType == "INPROCESS" || myVariables.pType == "PARCELID" || myVariables.pType == "ONHOLD" || myVariables.pType == "MAILAWAY" || myVariables.pType == "REVIEW")
            {
                myVariables.Borrower = Convert.ToString(ds.Tables[0].Rows[0]["borrowername"]);
                myVariables.Township = Convert.ToString(ds.Tables[0].Rows[0]["Township"]);
                myVariables.QCStatus = Convert.ToString(ds.Tables[0].Rows[0]["ComStatus"]);
                if (myVariables.QCStatus == "") { myVariables.QCStatus = Convert.ToString(ds.Tables[0].Rows[0]["key_status"]); }
            }

            BindData();
        }
        else
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No orders for process.";
            LogoutText();
            return;
        }
        #endregion

        #region Table 2
        if (ds.Tables[1].Rows.Count > 0)
        {
            txtassessor.Text = Convert.ToString(ds.Tables[1].Rows[0]["Assessor"]);
            txtassphone.Text = Convert.ToString(ds.Tables[1].Rows[0]["Assessor_Phone"]);
            txtTreasurer.Text = Convert.ToString(ds.Tables[1].Rows[0]["Treasurer"]);
            txtTreasphone.Text = Convert.ToString(ds.Tables[1].Rows[0]["Treasurer_Phone"]);
            btnlinksave.Visible = false;
            btnlinkupdate.Visible = true;

            string strurl1 = "", strurl2 = "", strurl3 = "";
            if (txtassessor.Text != "") strurl1 = "window.open('" + txtassessor.Text.Trim().ToString() + "');";
            if (txtTreasurer.Text != "") strurl2 = "window.open('" + txtTreasurer.Text.Trim().ToString() + "');";

            string[] strord = Lblorderno.Text.Split('_');
            strurl3 = "window.open('https://portal.titlesource.com/vendor/Tax/OrderDetails.aspx?oid=" + strord[0].ToString() + "');";

            string strurl = "http://publicrecords.netronline.com/state/" + myVariables.State + "/county/" + Lblcouny.Text.ToLower() + "/";
            string javascript = "<script type='text/javascript'>window.open('" + strurl + "');" + strurl1 + "" + strurl2 + "" + strurl3 + "</script>";
            this.RegisterStartupScript("", javascript);
        }
        else
        {
           // btnlinksave.Visible = true;
            btnlinkupdate.Visible = false;
        }
        #endregion

        #region Table 3
        ddlstatus.Items.Clear();
        ddlstatus.Items.Add("");
        ddlstatus.AppendDataBoundItems = true;

        if (ds.Tables[2].Rows.Count > 0)
        {
            ddlstatus.DataSource = ds.Tables[2];
            ddlstatus.DataTextField = "statusname";
            ddlstatus.DataBind();
            if (Lblprocessname.Text == "PRODUCTION" || Lblprocessname.Text == "DU") ddlstatus.Items.Insert(8, "Others");
        }
        #endregion

        #region Table 4
        if (Lblprocessname.Text == "QC")
        {
            ddlstatus.SelectedValue = myVariables.KeyStatus;
            PanelQc.Visible = true;
            if (ds.Tables[3].Rows.Count > 0)
            {
                lblkeycomplete.Text = "Remaining Key Completed : " + ds.Tables[3].Rows[0]["Key Completed"].ToString() + "";
                lblkeycomplete.Visible = true;
            }
        }
        else if (Lblprocessname.Text == "INPROCESS" || Lblprocessname.Text == "PARCELID" || Lblprocessname.Text == "ONHOLD" || Lblprocessname.Text == "REVIEW")
        {
            ddlstatus.SelectedValue = myVariables.QCStatus;
        }
        else if (Lblprocessname.Text == "MAILAWAY")
        {
            ddlstatus.SelectedValue = myVariables.QCStatus;
            btnrequest.Visible = true;
            Lnkcomments.Visible = true;
        }
        #endregion

        #region Table 5
        //if (ds.Tables[4].Rows.Count > 0)
        //{
        //    txtcommentshistory.Text = Convert.ToString(ds.Tables[4].Rows[0]["State_Comment"]);
        //}
        //else txtcommentshistory.Text = "";
        if (myVariables.State == "ME" && myVariables.County.ToLower() == "cumberland")
        {
            txtexcomments.Text = "For City of Portland - Need to order Taxcert, charges $25.00.(No Phone or Email or Fax).";
            //txtexcomments.Visible = true;
            chkclientcmt.Visible = true;
        }
        #endregion

        #region Table 6
        if (ds.Tables[5].Rows.Count > 0)
        {
            etypes = ds.Tables[5].Rows[0]["etype"].ToString();
            scount = ds.Tables[5].Rows[0]["ecount"].ToString();
            myVariables.ecount = scount;
            //txtexcomments.Visible = true;
            txtexcomments.Text = txtexcomments.Text + System.Environment.NewLine + "EntityType:" + System.Environment.NewLine + etypes.ToString();
        }
        #endregion

        #region Table 7
        if (ds.Tables[6].Rows.Count > 0)
        {
            lblbreak.Text = ds.Tables[6].Rows[0]["Total Time"].ToString();
            if (lblbreak.Text == "") lblbreak.Text = "00:00:00";
        }
        else lblbreak.Text = "00:00:00";
        #endregion

        #region Table 8
        if (ds.Tables[7].Rows.Count > 0)
        {
            ddltaxtype1.DataSource = ds.Tables[7];
            ddltaxtype1.DataTextField = "TaxType";
            ddltaxtype1.DataBind();
            ddltaxtype1.Items.Insert(0, "-----Select Tax Type-----");
        }
        #endregion

        #region Table 9
        if (ds.Tables[8].Rows.Count > 0)
        {
            Gridutilization.DataSource = ds.Tables[8];
            Gridutilization.DataBind();
        }
        else
        {
            Gridutilization.DataSource = null;
            Gridutilization.DataBind();
        }
        #endregion

        #region Table 10
        if (ds.Tables.Count > 9 && (Lblprocessname.Text == "PRODUCTION" || Lblprocessname.Text == "DU" || Lblprocessname.Text == "INPROCESS"))
        {
            if (ds.Tables[9].Rows.Count > 0)
            {
                Gridnextorder.DataSource = ds.Tables[9];
                Gridnextorder.DataBind();
            }
            else
            {
                Gridnextorder.DataSource = null;
                Gridnextorder.DataBind();
            }
        }
        else
        {
            Gridnextorder.DataSource = null;
            Gridnextorder.DataBind();
        }
        #endregion
    }

    private void BindData()
    {
        Btnmoveqc.Visible = false;
        string pType = myVariables.pType;
        if (pType == "KEY") { Lblprocessname.Text = "PRODUCTION"; }
        else if (pType == "QC") { Lblprocessname.Text = "QC"; }
        else if (pType == "DU") { Lblprocessname.Text = "DU"; Btnmoveqc.Visible = true; }
        else if (pType == "MAILAWAY") { Lblprocessname.Text = "MAILAWAY"; }
        else if (pType == "INPROCESS") { Lblprocessname.Text = "INPROCESS"; Btnmoveqc.Visible = true; }
        else if (pType == "PARCELID") { Lblprocessname.Text = "PARCELID"; }
        else if (pType == "ONHOLD") { Lblprocessname.Text = "ONHOLD"; }
        else if (pType == "REVIEW") { Lblprocessname.Text = "REVIEW"; }
        else if (pType == "PRIORITYKEY" || pType == "PRIORKEY") { Lblprocessname.Text = "PRODUCTION"; }
        else if (pType == "PRIORITYDU" || pType == "PRIORDU") { Lblprocessname.Text = "DU"; }
        string timezone = GetTimeZone();

        Lblorderno.Text = myVariables.Orderno;
        Lbldate.Text = myVariables.Date;
        Lblstate.Text = myVariables.State + " - " + timezone;
        Lblcouny.Text = myVariables.County;
        txtzipcode.Text = myVariables.Zipcode;
        txtBorrower.Text = myVariables.Borrower;
        txttownship.Text = myVariables.Township;
        txtcommentshistory.Text = myVariables.Lastcomment;
        if (myVariables.WebPhone == "") txtordertype.Text = "";
        else txtordertype.Text = myVariables.WebPhone;
    }
    #endregion

    #endregion

    #region Button Events
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (checkValidate())
        {
            ds.Dispose();
            ds.Reset();
            if (Lblprocessname.Text == "PRODUCTION") { ds = UpdateProduction("sp_UpdateKey_New"); }
            if (Lblprocessname.Text == "DU") ds = UpdateProduction("sp_UpdateDU_New");
            if (Lblprocessname.Text == "QC" || Lblprocessname.Text == "INPROCESS" || Lblprocessname.Text == "PARCELID" || Lblprocessname.Text == "ONHOLD" || Lblprocessname.Text == "MAILAWAY")
            { ds = UpdateProduction("sp_UpdateQC_New"); }
            if (Lblprocessname.Text == "REVIEW") { ds = UpdateProduction("sp_UpdateReview_New"); }
            
            Session["TimePro"] = DateTime.Now;
            Clearfields();
            DatasetAllotment(ds, sender, e);
        }
    }

    private DataSet UpdateProduction(string Procedurename)
    {
        string township = Titlecase(txttownship.Text);
        string borrower = Titlecase(txtBorrower.Text);
        DateTime dat = DateTime.Now;
        dat = dat.AddHours(-7);
        string date1 = String.Format("{0:MM/dd/yyyy HH:mm:ss}", dat);
        string commen = date1 + SessionHandler.UserName + ":";
        string com_det = date1 + ":";
        string ist, ist1 = "";
        ist = System.Environment.NewLine + commen + ddlprdcomments.SelectedItem.Text;
        ist1 = System.Environment.NewLine + com_det + ddlprdcomments.SelectedItem.Text;
        string oStatus = ddlstatus.SelectedItem.Text;
        string ordertype = txtordertype.Text;

        string error = "", errorcategory = "", errorfield = "", correct = "", incorrect = "", qcerrorcomment = "";

        if (Lblprocessname.Text != "PRODUCTION" && Lblprocessname.Text != "DU")
        {
            if (ddlstatus.Text == "Completed")
            {
                error = ddlerror.SelectedItem.Text;
                if (error == "") { Lblerror.Text = "Please Select all Error fields"; return null; }
                errorcategory = ddlerrorcat.SelectedItem.Text;
                if (errorcategory == "") { Lblerror.Text = "Please Select all Error fields"; return null; }
                errorfield = ddlerrorarea.SelectedItem.Text;
                if (errorfield == "") { Lblerror.Text = "Please Select all Error fields"; return null; }
                incorrect = ddlerrortype.SelectedItem.Text;
                if (incorrect == "") { Lblerror.Text = "Please Select all Error fields"; return null; }
                correct = ddlcombined.SelectedItem.Text;
                if (correct == "") { Lblerror.Text = "Please Select all Error fields"; return null; }
            }
        }

        return gl.UpdateOrdersNew(Procedurename, Lblorderno.Text, township, borrower, ist, Lbldate.Text, oStatus, ordertype, error, errorfield, correct, incorrect, Lblprocessname.Text, ist1, txtqcerrorcmts.Text, errorcategory, "", txtzipcode.Text, "","");
    }
    private string Titlecase(string txt)
    {
        txt = txt.ToLower();
        return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(txt);
    }
    private bool checkValidate()
    {
        Lblerror.Text = "";
        bool result = true;
        if (txtordertype.Text == "") { Lblerror.Text = Lblerror.Text + "</Br>" + "OrderType is Blank."; result = false; }
        if (txtBorrower.Text == "") { Lblerror.Text = Lblerror.Text + "</Br>" + "Borrowername is Empty."; result = false; }
        if (txttownship.Text == "") { Lblerror.Text = Lblerror.Text + "</Br>" + "Township is Empty."; result = false; }
        if (ddlstatus.SelectedIndex == 0) { Lblerror.Text = Lblerror.Text + "</Br>" + "Please Select the status."; result = false; }
        if (myVariables.State == "ME" && myVariables.County.ToLower() == "cumberland")
        {
            if (chkclientcmt.Checked == false)
            {
                Lblerror.Text = "Check the Client Comments.";
                result = false;
            }
        }
        if (btnlinksave.Visible == true && chksavelink.Checked == false) { Lblerror.Text = Lblerror.Text + "</Br>" + "Please Save Assessor and Treasurer website Link"; result = false; }

        if (ddlprdcomments.SelectedItem.Text == "") { Lblerror.Text = Lblerror.Text + "</Br>" + "Comments field is Empty."; result = false; }

        return result;
    }
    protected void btnMovecall_Click(object sender, EventArgs e)
    {
        if (checkValidate())
        {
            string township = Titlecase(txttownship.Text);
            string borrower = Titlecase(txtBorrower.Text);
            DateTime dat = DateTime.Now;
            string date1 = String.Format("{0:MM/dd/yyyy HH:mm:ss}", dat);
            string commen = date1 + SessionHandler.UserName + ":";
            string com_det = date1 + ":";
            string ist, ist1 = "";
            ist = System.Environment.NewLine + commen + ddlprdcomments.SelectedItem.Text;
            ist1 = System.Environment.NewLine + com_det + ddlprdcomments.SelectedItem.Text;
            string oStatus = ddlstatus.SelectedItem.Text;
            int result = gl.MoveToCall("sp_MovetoCall", Lblorderno.Text, township, borrower, ist, Lbldate.Text, oStatus, txtordertype.Text, ist1, txtzipcode.Text, "", Lblprocessname.Text);
            RRedirect();
        }
    }
    private void RRedirect()
    {
        if (SessionHandler.IsAdmin == true) Response.Redirect("Home.aspx");
        else if (SessionHandler.IsAdmin == false) Response.Redirect("NonAdminHome.aspx");
    }
    protected void LogoutBtn_Click(object sender, EventArgs e)
    {
        if (LogoutBtn.Text == "Back")
        {
            if (SessionHandler.IsAdmin == true) Response.Redirect("Home.aspx");
            else if (SessionHandler.IsAdmin == false) Response.Redirect("NonAdminHome.aspx");
        }
        else
        {
            ds.Dispose();
            ds.Reset();
            ds = gl.LoadLogoutReason();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddllogout.DataSource = ds;
                ddllogout.DataTextField = "Logout_Type";
                ddllogout.DataBind();
                ddllogout.Items.Insert(0, "");
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    string strrev = Convert.ToString(ds.Tables[1].Rows[i]["Break_Reason"]);
                    if (strrev != "Meeting" && strrev != "Training") ddllogout.Items.Remove(strrev);
                }
            }

            pagedimmer1.Visible = true;
            LogoutReason.Visible = true;
         
            txtlogreason.Text = "";
            txtlogreason.Visible = true;
        }
    }
    protected void ddlerror_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlerror.SelectedIndex == 1) Loaderrorcategory();
        else
        {
            Loaderrorcategory();
            ddlerrorcat.SelectedIndex = 1;
            Loaderrorarea();
            ddlerrorarea.SelectedIndex = 1;
            Loaderrortype();
            ddlerrortype.SelectedIndex = 1;
            Loadcombined();
            ddlcombined.SelectedIndex = 1;
        }
        if (ddlstatus.SelectedItem.Text == "Completed" && (Lblprocessname.Text == "QC" || Lblprocessname.Text == "REVIEW"))
        {
            lblqcerrorcmts.Visible = true;
            txtqcerrorcmts.Visible = true;
        }
        else
        {
            lblqcerrorcmts.Visible = false;
            txtqcerrorcmts.Visible = false;
        }
    }
    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblprdcomments.Text = "";
        string strddlstatus = string.Empty;
        btnrequest.Visible = false;
        Lnkcomments.Visible = false;
        strddlstatus = ddlstatus.SelectedItem.Text;
        LoadProductionComments(strddlstatus);
        if (Lblprocessname.Text != "PRODUCTION" && Lblprocessname.Text != "DU")
        {
            if (ddlstatus.SelectedItem.Text == "Completed") { PanelQc.Visible = true; }
            else PanelQc.Visible = false;
        }

        if (ddlstatus.SelectedItem.Text == "Mail Away")
        {
            Clearpanel();
            pagedimmer.Visible = true;
            ReportPanel.Visible = true;
            ddlrequesttype.SelectedIndex = 0;
            btnrequest.Visible = true;
            Lnkcomments.Visible = true;
            btnaddcomments.Visible = true;
            txtaddcomments.Visible = true;
            txtbrrname.Text = txtBorrower.Text;
        }
        else
        {
            ReportPanel.Visible = false;
            btnaddcomments.Visible = false;
            txtaddcomments.Visible = false;
        }
        if (strddlstatus == "In Process")
        {
            btnaddcomments.Visible = true;
            txtaddcomments.Visible = true;
        }
        if (strddlstatus == "Completed" && (Lblprocessname.Text == "PRODUCTION" || Lblprocessname.Text == "REVIEW"))
        {
            btnaddcomments.Visible = true;
            txtaddcomments.Visible = true;
        }
    }

    protected void txtordertype_TextChanged(object sender, EventArgs e)
    {
        if (txtordertype.Text.ToLower() == "phone") btnMovecall.Visible = true;
        else btnMovecall.Visible = false;
    }
    protected void Btnmoveqc_Click(object sender, EventArgs e)
    {
        if (checkValidate())
        {
            ds.Dispose();
            ds.Reset();
            ds = UpdateProduction("sp_UpdateKey");
            Session["TimePro"] = DateTime.Now;
            Clearfields();
            DatasetAllotment(ds, sender, e);
        }
    }

    protected void btnsendmail_Click(object sender, EventArgs e)
    {
        try
        {
            string Mailid = "", cc = "", fromid = "";
            string query = "select Toid,Ccid,Fromid from mail";
            string[] ordno = Lblorderno.Text.Split('_');
            mparam = new MySqlParameter[1];
            MySqlDataReader mdr = con.ExecuteSPReader(query, false, mparam);
            if (mdr != null)
            {
                if (mdr.HasRows)
                {
                    if (mdr.Read())
                    {
                        Mailid = mdr.GetString(0);
                        cc = mdr.GetString(1);
                        fromid = mdr.GetString(2);
                    }
                }
                mdr.Close();
            }

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress(fromid);
            mailMsg.To.Add(new MailAddress(Mailid));
            //mailMsg.To.Add(new MailAddress("m.karthikeyan@stringinformation.com"));

            string[] emailcc = cc.Split(',');

            foreach (string emails in emailcc)
            {
                if (emails.Trim().Length > 0)
                {
                    mailMsg.CC.Add(new MailAddress(emails));
                }
            }
            if (ddlemailtype.SelectedItem.Text != "")
            {
                if (ddlemailtype.SelectedItem.Text == "County")
                {
                    mailMsg.Subject = "String/ TSI Taxes - Change in County name – " + ordno[0].ToString();
                }
                else if (ddlemailtype.SelectedItem.Text == "Escrow")
                {
                    mailMsg.Subject = "String/ TSI Taxes - Escrow Status – " + ordno[0].ToString();
                }
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = "Hi";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "For the Order #" + ordno[0].ToString();
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                if (ddlemailtype.SelectedItem.Text == "County")
                {
                    if (txtcountyname.Text != "")
                    {
                        mailMsg.Body += "Could you please change the county name to " + txtcountyname.Text + "?";
                    }
                    else
                    {
                        Lblerror.Text = "Please Enter County name...";
                        return;
                    }
                }
                else if (ddlemailtype.SelectedItem.Text == "Escrow")
                {
                    mailMsg.Body += "Could you please check the escrow status?";
                }
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Thanks & Regards";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "-------------------------------------------------------------------";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Service Delivery Team";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "String Real Estate Information Services";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "#237 on the Inc 500, #13 among all real estate firms nationwide";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Phone : 202-470-0648 / 49";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "www.stringinfo.com";

                mailMsg.Body = "<font color='Blue'>" + mailMsg.Body + "</font>";

                mailMsg.Priority = MailPriority.Normal;
                SmtpClient mSmtpClient = new SmtpClient();
                mSmtpClient.Send(mailMsg);
                Lblerror.Text = "Mail Successfully Delivered...";
            }
            else
            {
                Lblerror.Text = "Please Select anyone email type...";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        pagedimmer1.Visible = false;
        statecomments.Visible = false;
    }
    protected void btnstatecomments_Click(object sender, EventArgs e)
    {
        pagedimmer1.Visible = true;
        statecomments.Visible = true;
        txtstatecomments.Text = GetStateComment(myVariables.State);
    }

    public string GetStateComment(string strstate)
    {
        DataSet Dset = new DataSet();
        Dset.Dispose();
        Dset.Reset();
        string strcomments = "";
        string query = "select State,State_Comment from state_comments where State='" + strstate + "' limit 1";
        Dset = con.ExecuteQuery(query);
        if (Dset.Tables[0].Rows.Count > 0)
        {
            strcomments = Dset.Tables[0].Rows[0]["State_Comment"].ToString();
        }
        return strcomments;
    }

    protected void btnaddcomments_Click(object sender, EventArgs e)
    {
        string query = "";
        int result = 0;
        if (ddlstatus.SelectedItem.Text == "Mail Away")
        {
            query = "Update status_comments set Status_Comment='" + txtaddcomments.Text + "' where Status='Mail Away' and id='25'";
            result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                LoadProductionComments("Mail Away");
                txtaddcomments.Text = "";
            }
        }
        else if (ddlstatus.SelectedItem.Text == "In Process")
        {
            query = "Update status_comments set Status_Comment='" + txtaddcomments.Text + "' where Status='In Process' and id='27'";
            result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                LoadProductionComments("In Process");
                txtaddcomments.Text = "";
            }
        }
        else if (ddlstatus.SelectedItem.Text == "Completed" && (Lblprocessname.Text == "PRODUCTION" || Lblprocessname.Text == "REVIEW"))
        {
            query = "Update status_comments set Status_Comment='" + txtaddcomments.Text + "' where Status='Completed' and id='30'";
            result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                LoadProductionComments("Completed");
                txtaddcomments.Text = "";
            }
        }
    }
    protected void btngetcomments_Click(object sender, EventArgs e)
    {
        ds.Dispose();
        ds.Reset();
        pagedimmer.Visible = true;
        getcomments.Visible = true;
        txtgetcomments.Text = "";
        string query = "select Comments from getcomments where State='General' or State='" + myVariables.State + "' Order by id";
        ds = con.ExecuteQuery(query);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            txtgetcomments.Text = txtgetcomments.Text + ds.Tables[0].Rows[i]["Comments"].ToString();
        }
    }
    protected void btngetclose_Click(object sender, EventArgs e)
    {
        pagedimmer.Visible = false;
        getcomments.Visible = false;
    }
    #endregion

    #region MailAway

    public void Clearpanel()
    {
        txtdate.Text = "";
        txtchqpay.Text = "";
        txtaddress.Text = "";
        txtbrrname.Text = "";
        txtbrraddress.Text = "";
        txtparcelid.Text = "";
        txtamount.Text = "";
        txttaxtype.Text = "";
        Lblsuccess.Text = "";
        txtcity.Text = "";
        ddlrequesttype.SelectedIndex = 0;
    }

    protected void btncreatetreq_Click(object sender, EventArgs e)
    {
        int sucess = 0;
        string order_no = string.Empty;
        string address = string.Empty;
        string cheque_pay = string.Empty;
        string amount = string.Empty;
        string req_type = string.Empty;
        string tax_type = string.Empty;
        string brrname = string.Empty;
        string brraddress = string.Empty;
        string parcelid = string.Empty;
        string strdate = string.Empty;
        string city = string.Empty;
        string tdate = string.Empty;
        try
        {
            tdate = gl.setdate();
            DateTime pDt = Convert.ToDateTime(tdate);
            tdate = String.Format("{0:MM/dd/yyyy}", pDt);

            ds.Dispose();
            ds.Reset();
            string query = "select Order_no,Address from mailaway_tbl where order_no='" + Lblorderno.Text.ToString() + "' and PDate='" + Lbldate.Text.Trim() + "'";
            ds = con.ExecuteQuery(query);
            if (ds.Tables[0].Rows.Count >= 0)
            {
                int noofrequst = ds.Tables[0].Rows.Count + 1;
                sucess = gl.InsertMailaway(Lblorderno.Text.Trim(), txtchqpay.Text.Trim(), txtaddress.Text.Trim(), txtbrrname.Text.Trim(), txtbrraddress.Text.Trim(), txtparcelid.Text.Trim(), txtamount.Text.Trim(), ddlrequesttype.SelectedItem.Text.ToString(), txttaxtype.Text.Trim(), txtdate.Text.Trim(), noofrequst, Lbldate.Text.Trim(), txtcity.Text.Trim(), tdate.Trim());
                if (sucess > 0)
                {
                    ds.Dispose();
                    ds.Reset();
                    string strquery = "select Order_no,Address,Cheque_payable,Amount,Req_Type,TaxType,Borrowername,BorrowerAddress,ParcelId,city from mailaway_tbl where order_no='" + Lblorderno.Text.ToString() + "' and No_of_Request='" + noofrequst + "' and pDate='" + Lbldate.Text.Trim() + "' and amount='" + txtamount.Text.Trim() + "' limit 1";
                    ds = con.ExecuteQuery(strquery);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        order_no = ds.Tables[0].Rows[0]["Order_no"].ToString();
                        address = ds.Tables[0].Rows[0]["Address"].ToString();
                        cheque_pay = ds.Tables[0].Rows[0]["Cheque_payable"].ToString();
                        amount = ds.Tables[0].Rows[0]["Amount"].ToString();
                        req_type = ds.Tables[0].Rows[0]["Req_Type"].ToString();
                        tax_type = ds.Tables[0].Rows[0]["TaxType"].ToString();
                        brrname = ds.Tables[0].Rows[0]["Borrowername"].ToString();
                        brraddress = ds.Tables[0].Rows[0]["BorrowerAddress"].ToString();
                        parcelid = ds.Tables[0].Rows[0]["ParcelId"].ToString();
                        city = ds.Tables[0].Rows[0]["City"].ToString();
                        dt = Convert.ToDateTime(txtdate.Text);
                        strdate = String.Format("{0:dd-MMM-yyyy}", dt);
                        CreateReport(order_no, address, cheque_pay, amount, req_type, tax_type, brrname, brraddress, parcelid, strdate);
                        Clearpanel();
                        Lblsuccess.Text = "Reports Generated Successfully.";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.ToString();
        }
    }

    public void CreateReport(string order_no, string address, string cheque_pay, string amount, string req_type, string tax_type, string brrname, string brraddress, string parcelid, string strdate)
    {
        int count = 0;
        StringBuilder sb = new StringBuilder();
        StringBuilder strbder = new StringBuilder();
        string strFile = string.Empty;
        for (int i = 0; i < order_no.Length; i++)
        {
            if (i % 3 == 0 && i != 0)
            {
                sb.Append('-');
            }
            sb.Append(order_no[i]);
        }
        string strorderno = sb.ToString();
        string strreq_type = req_type.Replace('/', '-');
        if (req_type == "UPS/SASE")
        {
            strreq_type = "UPS";
        }
        else if (req_type == "REGULAR" || req_type == "THANKS REQUEST")
        {
            strreq_type = "REG";
        }

        strbder.Append("<html><body><form id='form1' runat='server'>");
        strbder.Append("<p style='page-break-before: always'>");
        strbder.Append("<div style='font-family:Arial;font-size:smaller;'>");
        strbder.Append("<div style='text-align:right'><b> Report No: " + strreq_type + "-" + strorderno + "</b></div>");
        strbder.Append("<div style='text-align:right'><b>Date: " + strdate + "</b></div>");
        strbder.Append("<br />");
        strbder.Append("<div><b>To</b></div>");
        strbder.Append("<div><b>" + cheque_pay + "</b></div>");
        string tempaddress = address.Replace("\r\n", "+");
        string[] straddress = tempaddress.Split('+');
        for (int i = 0; i < straddress.Length; i++)
        {
            strbder.Append("<div><b>" + straddress[i] + "</b></div>");
        }
        strbder.Append("<br /><br />");
        strbder.Append("<div>To Whom It May Concern:</b></div>");
        strbder.Append("<br />");
        if (req_type != "THANKS REQUEST")
        {
            strbder.Append("<div>Requesting <b>" + tax_type + "</b> information on a property located at <b>" + brraddress + "</b> .Owner name is <b>" + brrname + "</b>. The Parcel Id number is <b>" + parcelid + "</b>. This is for<b> Refinance.</b></div>");
            strbder.Append("<br /><br />");
            strbder.Append("<div>I will need to know the following:</div>");
            strbder.Append("<br />");
            strbder.Append("<div>Type of collection - annually, semi - annually, or quarterly</div>");
            strbder.Append("<div>Status with specific date - paid, due, delinquent</div>");
            strbder.Append("<div>Amounts and Due dates - discount, face and penalty</div>");
            strbder.Append("<div>Delinquent taxes, if any</div>");
            strbder.Append("<div>If any other taxes are applicable, please provide name and phone number of the entity responsible for collection.</div>");
            strbder.Append("<br />");
        }
        strbder.Append("<div>Please find enclosed a check:</div>");
        strbder.Append("<br />");
        strbder.Append("<div>Check Number:</div>");
        strbder.Append("<div>Check Amount: $" + amount + "</div>");
        strbder.Append("<div>Favor: " + cheque_pay + "</div>");
        strbder.Append("<br />");
        if (req_type != "THANKS REQUEST")
        {
            if (req_type == "UPS" || req_type == "REGULAR")
            {
                strbder.Append("<div><b>Please fax this information to me if possible at 240-223-2060.</b></div>");
            }
            else if (req_type == "UPS/R")
            {
                strbder.Append("<div>I have included a return UPS.<b> Please fax this information to me if possible at 800-349-4782.</b></div>");
            }
            else if (req_type == "UPS/SASE")
            {
                strbder.Append("<div>I have included a Self Addressed Stamped Envelope.<b> Please fax this information to me if possible at 800-349-4782.</b></div>");
            }
            strbder.Append("<br />");
            strbder.Append("<div>Please mention the Report # <b>" + order_no + "</b> in the Tax Certification</div>");
            strbder.Append("<br />");
            strbder.Append("<div><b>Kindly Note: \"This request is being made on behalf of Title Source \"</b></div>");
            strbder.Append("<br />");
        }
        else
        {
            strbder.Append("<div>We thank you for the tax information provided over phone on <b>" + strdate + ".</b>This is for the property located at <b>" + brraddress + "</b>. Owner name is <b>" + brrname + ".</b> The Parcel Id number is <b>" + parcelid + ".</b></div>");
            strbder.Append("<br /><br />");
            strbder.Append("<div>We appreciate your kind co-operation for the information provided.</div>");
            strbder.Append("<br /><br />");
        }
        strbder.Append("<div>Sincerely,</div>");
        strbder.Append("<div>Prashant P Kothari</div>");
        strbder.Append("</div>");
        strbder.Append("</form></body></html>");

    e:
        try
        {
            string query = string.Empty;
            if (req_type == "THANKS REQUEST" || req_type == "REGULAR") { query = "select Output_Path_Regular from master_path"; }
            else { query = "select Output_Path_Ups from master_path"; }
            if (count == 0) strFile = order_no + " - " + cheque_pay + ".doc";
            else if (count > 0) strFile = order_no + " - " + cheque_pay + count + ".doc";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(strbder.ToString());
            MemoryStream ms = new MemoryStream(data);
            string FilePath = getfullpath(strFile, query);
            if (FilePath == "") return;
            FileStream fs = new FileStream(FilePath + "/" + strFile + "", FileMode.Create);
            //FileStream fs = new FileStream(@"E:\Karthikeyan\Task\TSI Taxes\Reports" + "/" + strFile + "", FileMode.CreateNew);     
            ms.WriteTo(fs);
            ms.Close();
            fs.Close();
            fs.Dispose();
        }
        catch
        {
            count++;
            goto e;
        }
    }

    private string getfullpath(string filename, string query)
    {
        string slash = @"\";
        string dec, sourcePath, pdatee, month, year, path = "";      
        MySqlParameter[] mParam = new MySqlParameter[1];
        MySqlDataReader mDataReader = con.ExecuteStoredProcedure(query, false, mParam);
        if (mDataReader.HasRows)
        {
            if (mDataReader.Read())
            {
                sourcePath = mDataReader.GetString(0);
                DateTime pde = DateTime.Now;
                pde = pde.AddHours(-12);
                pdatee = String.Format("{0:dd MMM yy}", pde);
                month = String.Format("{0:MMMM}", pde);
                year = String.Format("{0:yyyy}", pde);
                dec = sourcePath + slash + year + slash + month + slash + pdatee + slash + Lblorderno.Text;
                dir(dec);
                path = dec;
            }
        }
        mDataReader.Close();
        return path;
    }
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

    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {
            ReportPanel.Visible = false;
            pagedimmer.Visible = false;
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.ToString();
        }
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

    #endregion

    #region ViewRequest from DB
    protected void btnrequest_Click(object sender, EventArgs e)
    {
        string id = "", date = "", sRet = "", sPath = "";
        id = Lblorderno.Text.Trim();
        date = Lbldate.Text.Trim();
        sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
        sRet = oInfo.Name;
        string Url = sPath.Replace(sRet, "Request.aspx?id=" + id + "&date=" + date);
        ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + Url + "')</script>");
    }
    #endregion

    protected void btnreferences_Click(object sender, EventArgs e)
    {
        ddlreferences.Items.Clear();
        ddlreferences.Items.Insert(0, "");
        string filepath = getfullpath1();

        DirectoryInfo di = new DirectoryInfo(filepath);
        FileInfo[] files = di.GetFiles("" + Lblorderno.Text + "*.*", SearchOption.AllDirectories);
        foreach (FileInfo file in files)
        {
            ddlreferences.Items.Add(file.Name);
        }
    }

    private string getfullpath1()
    {
        string slash = @"\";
        string dec, sourcePath, pdatee, month, year, path = "";
        string query = "select Output_References from master_path";
        MySqlParameter[] mParam = new MySqlParameter[1];
        MySqlDataReader mDataReader = con.ExecuteStoredProcedure(query, false, mParam);
        if (mDataReader.HasRows)
        {
            if (mDataReader.Read())
            {
                sourcePath = mDataReader.GetString(0);
                DateTime pde;
                pde = Convert.ToDateTime(Lbldate.Text);
                pdatee = String.Format("{0:dd MMM yy}", pde);
                month = String.Format("{0:MMMM}", pde);
                year = String.Format("{0:yyyy}", pde);
                dec = sourcePath + slash + year + slash + month;
                dir(dec);
                path = dec;
            }
        }
        mDataReader.Close();
        return path;
    }

    protected void ddlreferences_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string url = getfullpath1();
            string filename = url + @"\" + ddlreferences.SelectedItem.Text;

            if (File.Exists(filename))
            {
                FileInfo fileDet = new System.IO.FileInfo(filename);
                Response.Clear();
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileDet.Name));
                Response.AddHeader("Content-Length", fileDet.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(fileDet.FullName);
                Response.End();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        string strlogout = "", strlog = "";
       
      strlogout = txtlogreason.Text;
        string strprocess = Lblprocessname.Text;

        gl.Logout_New(strprocess, strlog, strlogout);
        RRedirect();
    }

    protected void btnlogoutclose_Click(object sender, EventArgs e)
    {
        pagedimmer1.Visible = false;
        LogoutReason.Visible = false;
    }

    protected void ddlerrorcat_SelectedIndexChanged(object sender, EventArgs e)
    {
        Loaderrorarea();
    }
    protected void ddlerrorarea_SelectedIndexChanged(object sender, EventArgs e)
    {
        Loaderrortype();
    }
    protected void ddlerrortype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Loadcombined();
    }
    protected void ddlprdcomments_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblprdcomments.Text = "";
        lblprdcomments.Text = ddlprdcomments.SelectedItem.Text;
    }
    protected void ddllogout_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddllogout.SelectedItem.Text == "Other") txtlogreason.Visible = true;
        else txtlogreason.Visible = false;
    }

    #region Save Link
    protected void btnlinksave_Click(object sender, EventArgs e)
    {
        int result = 0;
        GetReplaceValue(txtassphone.Text, txtTreasphone.Text);
        string[] strstate = Lblstate.Text.Split('-');
        result = gl.SaveCountyLink(strstate[0].Trim().ToString(), Lblcouny.Text, txtassessor.Text.Trim().ToString(), txtassphone.Text.Trim().ToString(), txtTreasurer.Text.Trim().ToString(), txtTreasphone.Text.Trim().ToString());
        if (result > 0)
        {
            Lblerror.Text = "Assessor and Treasurer details saved successfully...";
        }
    }
    protected void btnlinkupdate_Click(object sender, EventArgs e)
    {
        int result = 0;
        GetReplaceValue(txtassphone.Text, txtTreasphone.Text);
        string[] strstate = Lblstate.Text.Split('-');
        result = gl.UpdateCountyLink(strstate[0].Trim().ToString(), Lblcouny.Text, txtassessor.Text.Trim().ToString(), txtassphone.Text.Trim().ToString(), txtTreasurer.Text.Trim().ToString(), txtTreasphone.Text.Trim().ToString());
        if (result > 0)
        {
            Lblerror.Text = "Assessor and Treasurer details updated successfully...";
        }
    }
    public void GetReplaceValue(string strass, string strtres)
    {

        if (strass != "")
        {
            strass = strass.Replace("(", "");
            strass = strass.Replace(")", "");
            strass = strass.Replace("-", "");
        }
        if (strtres != "")
        {
            strtres = strtres.Replace("(", "");
            strtres = strtres.Replace(")", "");
            strtres = strtres.Replace("-", "");
        }

        txtassphone.Text = strass;
        txtTreasphone.Text = strtres;
    }
    #endregion

    #region Tax Information Mail
    protected void chktaxinfo_CheckedChanged(object sender, EventArgs e)
    {
        if (chktaxinfo.Checked == true)
        {
            Taxinfomail.Visible = true;
            pagedimmer.Visible = true;
        }
        else
        {
            Taxinfomail.Visible = false;
            pagedimmer.Visible = false;
        }
    }
    protected void btnsendtaxmail_Click(object sender, EventArgs e)
    {
        string fromid, ccid, toid, strsubject = "";
        string[] ordno;

        try
        {
            fromid = "taxcerts@stringinformation.com";
            toid = txttaxtoid.Text;
            ccid = "taxcerts@stringinformation.com";
            strsubject = "Requesting tax information - ";
            if (fromid != null && toid != null && ccid != null && strsubject != null)
            {
                string[] emailcc = ccid.Split(',');
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress(fromid);
                mailMsg.To.Add(new MailAddress(toid));
                foreach (string emails in emailcc)
                {
                    if (emails.Trim().Length > 0)
                    {
                        mailMsg.CC.Add(new MailAddress(emails));
                    }
                }
                ordno = Lblorderno.Text.Split('_');
                mailMsg.Subject = strsubject + ordno[0].ToString();

                mailMsg.IsBodyHtml = true;
                mailMsg.Body = "Hi";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Requesting " + ddltaxtype1.SelectedItem.Text + " information on a property located at " + txttaxadd.Text + ". Owner name is " + txtBorrower.Text + ". The Parcel Id number is " + txtparcelno.Text + ".";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "This is for Refinance.";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "I will need to know the following:";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Tax amounts___________. If paid the paid amount ___________ and the paid date ___________.";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Delinquent if any, payoff till month end.";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Tax sale ever filed, if so detailed information.";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Please fax this information to me if possible at (240) 223-2060 or email to taxcerts@stringinformation.com";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Thanks & Regards";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "-------------------------------------------------------------------";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Service Delivery Team";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "String Real Estate Information Services";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "#237 on the Inc 500, #13 among all real estate firms nationwide";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Phone : 202-470-0648 / 49";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "www.stringinfo.com";

                mailMsg.Body = "<font color='Blue'>" + mailMsg.Body + "</font>";

                mailMsg.Priority = MailPriority.Normal;
                SmtpClient mSmtpClient = new SmtpClient();
                mSmtpClient.Send(mailMsg);
                lbltaxerror.Text = "Mail Successfully Delivered...";
            }
            else
            {
                lbltaxerror.Text = "Pease save Fromid,Toid,CCid for this process.";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btntaxcancel_Click(object sender, EventArgs e)
    {
        Taxinfomail.Visible = false;
        pagedimmer.Visible = false;
        chktaxinfo.Checked = false;
    }
    #endregion

    #region Add Error
    protected void btnadderror_Click(object sender, EventArgs e)
    {
        int result = gl.AddAuditError(Lblorderno.Text, Lbldate.Text, ddlerror.SelectedItem.Text, ddlerrorcat.SelectedItem.Text, ddlerrorarea.SelectedItem.Text, ddlerrortype.SelectedItem.Text, ddlcombined.SelectedItem.Text);
        LoadErrorGrid();
    }
    public void LoadErrorGrid()
    {
        ds.Dispose();
        ds.Reset();
        string query = "Select Order_No,Pdate,K1_OP,QC_OP,Error,Error_Category,Error_Area,Error_Type,Combined from audit_error where Order_No='" + Lblorderno.Text + "' and Pdate='" + Lbldate.Text + "'";
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Griderrors.DataSource = ds;
            Griderrors.DataBind();
        }
        else
        {
            Griderrors.DataSource = null;
            Griderrors.DataBind();
        }
    }
    protected void Griderrors_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            string query, strerror = "";
            WebControl wc = e.CommandSource as WebControl;
            GridViewRow row = wc.NamingContainer as GridViewRow;
            strerror = ((Label)row.FindControl("GridLblerror")).Text;
            query = "Delete from audit_error where Order_No='" + Lblorderno.Text + "' and Pdate='" + Lbldate.Text + "' and Username='" + SessionHandler.UserName + "' and Combined='" + strerror + "'";
            int result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                LoadErrorGrid();
            }
        }
    }
    protected void Griderrors_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion
    #region Breaktime
    protected void btn_brk_start_Click(object sender, EventArgs e)
    {
        hid_Ticker.Value = new TimeSpan(0, 0, 0).ToString();
        string query = "";
        txt_break_reason.Text = "";
        lblothererror.Text = "";
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

        result = gl.insert_breaks(gl.order_no, pdate, gl.Process, drp_break.SelectedItem.Text, drp_break.SelectedItem.Value.ToString());

        string act_time = result.Tables[0].Rows[0]["act_time"].ToString();
        string brk_time = result.Tables[1].Rows[0]["brk_time"].ToString();
        if (brk_time == "")
        {
            brk_time = "01:00:00";
        }

        TimeSpan act = TimeSpan.Parse(act_time);
        TimeSpan brk = TimeSpan.Parse(brk_time);

        if (act > brk && drp_break.SelectedItem.Text == "Meeting")
        {
            if (brk.ToString().Contains("-"))
            {
                lblothererror.Text = "You have already Completed your breaklimits";
            }
            else
            {
                lblothererror.Text = "You have remaining " + " " + brk + " " + ":Mins";
            }

        }

        else
        {
            lblothererror.Text = "";
        }
        div_brk_start.Visible = false;
        lbl_brk_name.Text = drp_break.SelectedItem.Text.ToString();
        div_brk_cmd.Visible = true;
        div_brk_countdown.Visible = true;
        div_brk_stop.Visible = false;
    }
    protected void btn_brk_cancel_Click(object sender, EventArgs e)
    {
        lnkOthers.Text = "Break";

        pagedimmer.Visible = false;
        Other_breakMsgbx.Visible = false;
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        hid_Ticker.Value = TimeSpan.Parse(hid_Ticker.Value).Add(new TimeSpan(0, 0, 1)).ToString();
        lit_Timer.Text = hid_Ticker.Value.ToString();
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

        ds_time_diff = gl.timeiff_breaks(lbl_brk_name.Text, pdate);

        string act_time = ds_time_diff.Tables[0].Rows[0]["act_time"].ToString();
        string brk_time = ds_time_diff.Tables[1].Rows[0]["diff_time"].ToString();

        TimeSpan act = TimeSpan.Parse(act_time);
        TimeSpan brk = TimeSpan.Parse(brk_time);

        if (act < brk)
        {

            div_brk_countdown.Visible = false;
            div_brk_stop.Visible = true;
            lbl_brk_cmd_err.Text = "Please enter the Delay reason..";
        }

        else
        {
            ds_time = gl.update_breaks(lbl_brk_name.Text, txt_break_reason.Text, pdate);

            //ds_time = gl.update_breaks(lbl_brk_name.Text, txt_break_reason.Text, pdate);
            string tottime = ds_time.Tables[0].Rows[0]["brk_time"].ToString();
            TimeSpan time1 = TimeSpan.Parse(tottime);
            TimeSpan tmlimit = new TimeSpan(01, 0, 0);

            if (time1 > tmlimit)
            {
                bool result = gl.sp_lock_user();

            }

        }

        lnkOthers.Text = "Break";
        pagedimmer.Visible = false;
        Other_breakMsgbx.Visible = false;


    }
    protected void btn_brk_reson_Click(object sender, EventArgs e)
    {
        if (txt_break_reason.Text == "")
        {
            lbl_brk_cmd_err.Text = "Please enter the comments...";
            return;
        }
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

        ds_time = gl.update_breaks(lbl_brk_name.Text, txt_break_reason.Text, pdate);
        string tottime = ds_time.Tables[0].Rows[0]["brk_time"].ToString();
        TimeSpan time1 = TimeSpan.Parse(tottime);
        TimeSpan tmlimit = new TimeSpan(01, 0, 0);

        if (time1 > tmlimit)
        {
            bool result = gl.sp_lock_user();

        }


        lnkOthers.Text = "Break";
        pagedimmer.Visible = false;
        Other_breakMsgbx.Visible = false;


    }
    public void lnkOthers_Click(object sender, EventArgs e)
    {
        lnkOthers.Text = "UnBreak";
        pagedimmer.Visible = true;
        Other_breakMsgbx.Visible = true;
        div_brk_start.Visible = true;
        div_brk_cmd.Visible = false;
    }


    public void break_mis()
    {


        DataSet ds = new DataSet();
        ds = gl.get_diff_time();
        if (ds.Tables[0].Rows.Count > 0)
        {
            lnkOthers.Text = "UnBreak";
            pagedimmer.Visible = true;
            Other_breakMsgbx.Visible = true;
            div_brk_start.Visible = false;
            lbl_brk_name.Text = "";
            div_brk_cmd.Visible = true;
            div_brk_countdown.Visible = true;
            div_brk_stop.Visible = false;

            string brk_typ = ds.Tables[0].Rows[0]["break_type"].ToString();
            string dif_tm = ds.Tables[0].Rows[0]["diff_time"].ToString();

            drp_break.Text = brk_typ;


            lbl_brk_name.Text = brk_typ;
            string[] splt_time = dif_tm.Split(':');
            int hr = 0, min = 0, sec = 0;
            hr = Convert.ToInt32(splt_time[0].ToString());
            min = Convert.ToInt32(splt_time[1].ToString());
            sec = Convert.ToInt32(splt_time[2].ToString());
            hid_Ticker.Value = new TimeSpan(hr, min, sec).ToString();

        }
    }
    #endregion
}
