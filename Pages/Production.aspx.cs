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


public partial class Pages_Production : System.Web.UI.Page
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
        //LoadGrid();
        if(!Page.IsPostBack)
        {
            Session["TimePro"] = DateTime.Now;
            
            CheckBreaktime();
            exeptionsfalse();
            if (CheckProductionLock())
            {
                string id = Request.QueryString["id"];
                Defaultcontrol();
                if (id == "12f7tre5")
                {
                    if (CheckOrderallotment())
                    { AutoAllotment(sender, e); }
                    else RRedirect();
                }
                else ManualAllotment(id, sender, e);

                if (Lblorderno.Text == "")
                { LogoutBtn.Text = "Back"; }
                else
                { 
                    //LogoutBtn.Attributes.Add("onClick", "return disp_prompt();"); 
                    LogoutBtn.Text = "Logout";
                }

                Lnkcomments.Attributes.Add("onclick", "window.open('MailAwayComments.aspx'); return false;");
                btngetcomments.Attributes.Add("onclick", "window.open('GeneralComments.aspx'); return false;");
                pagedimmer.Visible = false;
                ReportPanel.Visible = false;
                pagedimmer1.Visible = false;
                statecomments.Visible = false;
                getcomments.Visible = false;
                LogoutReason.Visible = false;
                ParcelInformation.Visible = false;
                Taxinfomail.Visible = false;
                DivEmail.Visible = false;
                DivTaxEmail.Visible = false;
            }
            else
            {
                RRedirect();
            }
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

    public void LoadGrid()
    {
        DateTime pDt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", pDt);
        DataSet dset = new DataSet();
        dset.Dispose();
        dset.Reset();
        //string query = "select count(Orderno) as Productivity,sum(if(WebPhone='Phone',1,0)) as Phone,sum(if(WebPhone='Website',1,0)) as Website,sum(if(WebPhone='Mailaway',1,0)) as Mailaway,sum(if(processType='DU',1,0) + if(processType='PRODUCTION',1,0)) as Production,sum(if(processType='QC',1,0)) as QC,cast(concat(Round((((count(Orderno)-sum(if(Quality='Error',1,0)))/count(Orderno))*100),0),'%') as char) as Quality,cast(round(sum(TIME_TO_SEC(Totalprocesstime))/60) as char) as Utilization from tbl_working where (Orderstaus='Completed' and DeliveryDate between '" + strdate + "' and '" + strdate + "' and Username='" + SessionHandler.UserName + "' and Isreview=0) or (processType='PRODUCTION' and Orderstaus='Completed' and Username='" + SessionHandler.UserName + "' and Iskey=1) group by Username";
        string query = "select count(Orderno) as Productivity,sum(if(WebPhone='Phone',1,0)) as Phone,sum(if(WebPhone='Website',1,0)) as Website,sum(if(WebPhone='Mailaway',1,0)) as Mailaway,sum(if(processType='DU',1,0) + if(processType='PRODUCTION',1,0)) as Production,sum(if(processType='QC',1,0)) as QC,cast(concat(Round((((count(Orderno)-sum(if(Quality='Error',1,0)))/count(Orderno))*100),0),'%') as char) as Quality,cast(round(sum(TIME_TO_SEC(Totalprocesstime))/60) as char) as Utilization from tbl_working where Orderstaus='Completed' and DeliveryDate between '" + strdate + "' and '" + strdate + "' and Username='" + SessionHandler.UserName + "' and Isreview=0 group by Username";
        dset = con.ExecuteQuery(query);
        if (dset.Tables[0].Rows.Count > 0)
        {
            Gridutilization.DataSource = dset;
            Gridutilization.DataBind();
        }
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

    public bool CheckProductionLock()
    {
        bool retresult = true;
        if (ds.Tables[3].Rows.Count > 0)
        {
            string strvalue = ds.Tables[3].Rows[0]["retvalue"].ToString();
            if (strvalue == "0") retresult = false;
            else if (strvalue == "1") retresult = true;
        }

        return retresult;
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
        string strerrcat=ddlerrorcat.SelectedItem.Text;
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

    public void CheckBreaktime()
    {
        string strdate, strusrname = "";
        strusrname = SessionHandler.UserName;
        strdate = gl.setdate();
        DateTime pDt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", pDt);

        if (strusrname != "")
        {
            ds.Dispose();
            ds.Reset();
            ds = gl.BreakTimeDetailsNew(strusrname, strdate);

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblbreak.Text = ds.Tables[0].Rows[0]["Total Time"].ToString();
                if (lblbreak.Text == "") lblbreak.Text = "00:00:00";
            }
            else lblbreak.Text = "00:00:00";

            if (ds.Tables[1].Rows.Count > 0)
            {
                ddltaxtype1.DataSource = ds.Tables[1];
                ddltaxtype1.DataTextField = "TaxType";
                ddltaxtype1.DataBind();
                ddltaxtype1.Items.Insert(0, "-----Select Tax Type-----");
            }

            if (ds.Tables[2].Rows.Count > 0) SessionHandler.UserState = Convert.ToString(ds.Tables[2].Rows[0]["State"]);
            else SessionHandler.UserState = "";
        }
    }
    #endregion

    #region TogglePanel
    private void TogglePanel(Panel sPanel)
    {
        PanelOrderallotment.Visible = false;
        PanelStatus.Visible = false;

        sPanel.Visible =true;
    }
    private void TogglePanel1(Panel sPanel)
    {
        PanelCompleted.Visible = false;
        PanelInproHold.Visible = false;
        PanelMailaway.Visible = false;
        PanelParcelID.Visible = false;
        PanelRejected.Visible = false;
        PanelExemptions.Visible = false;
        sPanel.Visible = true;
    }
    private void Defaultcontrol()
    {
        //Paneliferror.Visible = false;
        PanelQc.Visible = false;
        btnMovecall.Visible = false;
        btnrequest.Visible = false;
        Lnkcomments.Visible = false;
        Btnmoveqc.Visible = false;
    }
    #endregion

    #region Allotment Types
    private void ManualAllotment(string id, object sender, EventArgs e)
    {
        myVariables.Userright = "";
        myVariables.pType = "";
        Clearfields();
        
        string OrderStatus = gl.GetOrdersStatus(id);
        if (OrderStatus != "YTS" || OrderStatus == "YTS")
        {
            OrderList(id, sender, e);
            //gl.GetUserrights("Manualallotment");
            //switch (myVariables.Userright)
            //{
            //    case "1000":
            //        myVariables.pType = "MAILAWAY";
            //        MailAway(id);
            //        break;
            //    case "0100":
            //        myVariables.pType = "INPROCESS";
            //        InPrcoess(id);
            //        break;
            //    case "0010":
            //        myVariables.pType = "PARCELID";
            //        ParcelID(id);
            //        break;
            //    case "0001":
            //        myVariables.pType = "ONHOLD";
            //        OnHold(id);
            //        break;
            //    default:
            //        lblinfo.Text = "Please Check the Rights.";
            //        TogglePanel(PanelStatus);
            //        break;
            //}
        }
        //else
        //{
        //    myVariables.pType = "DU";
        //    PAState(id);
        //}
    }

    private void AutoAllotment(object sender, EventArgs e)
    {
        myVariables.Userright = "";
        myVariables.pType = "";
        Clearfields();
        gl.GetUserrights("Autoallotment");
        switch (myVariables.Userright)
        {
            case "001000000":
                myVariables.pType = "DU";
                DU(sender, e);
                break;
            case "001001000":
                myVariables.pType = "DU";
                DU(sender, e);
                break;
            case "100000000":
                myVariables.pType = "KEY";
                Key(sender, e);
                break;
            case "010000000":
                myVariables.pType = "QC";
                ddlerror_SelectedIndexChanged(sender, e);
                QC(sender, e);
                break;
            case "110000000":
                myVariables.pType = "KEYQC";
                KEYQC(sender, e);
                break;
            case "011000000":
                myVariables.pType = "DUQC";
                DUQC(sender, e);
                break;
            case "000100000":
                myVariables.pType = "PARCELID";
                Parcel(sender, e);
                break;
            case "000010000":
                myVariables.pType = "INPROCESS";
                Inprocess(sender, e);
                break;
            case "000001000":
                myVariables.pType = "MAILAWAY";
                Mail_away(sender, e);
                break;
            case "000000100":
                myVariables.pType = "REVIEW";
                Review(sender, e);
                break;
            case "100000010":
                myVariables.pType = "PRIORITYKEY";
                Priorkey(sender, e);
                break;
            case "100000001":
                myVariables.pType = "PRIORKEY";
                Priorkey(sender, e);
                break;
            case "001000010":
                myVariables.pType = "PRIORITYDU";
                Priordu(sender, e);
                break;
            case "001000001":
                myVariables.pType = "PRIORDU";
                Priordu(sender, e);
                break;
            default:
                lblinfo.Text = "Please Check the Rights.";
                TogglePanel(PanelStatus);
                break;
        }        
    }
    #endregion

    #region Process Type
    private void DU(object sender, EventArgs e)
    {
        if (!Filldata("DU",""))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";            
            return;
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }
    private void Key(object sender, EventArgs e)
    {
        if (!Filldata("KEY",""))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }
    private void QC(object sender, EventArgs e)
    {
        if (!Filldata("QC",""))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }
    private void KEYQC(object sender, EventArgs e)
    {
        if (!Filldata("QC",""))
        {
            if (!Filldata("KEY", ""))
            {
                TogglePanel(PanelStatus);
                lblinfo.Text = "No order allotted for you, Please contact Admin.";
                return;
            }
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }

    private void Review(object sender, EventArgs e)
    {
        if (!Filldata("REVIEW", ""))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }

    private void Priorkey(object sender, EventArgs e)
    {
        if (!Filldata(myVariables.pType, ""))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }

    private void Priordu(object sender, EventArgs e)
    {
        if (!Filldata(myVariables.pType, ""))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }


    private void Parcel(object sender, EventArgs e)
    {
        if (!Filldata("PARCELID", ""))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }

    private void Mail_away(object sender, EventArgs e)
    {
        if (!Filldata("MAILAWAY", ""))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }

    private void Inprocess(object sender, EventArgs e)
    {
        if (!Filldata("INPROCESS", ""))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }

    private void DUQC(object sender, EventArgs e)
    {
        if (!Filldata("QC",""))
        {
            if (!Filldata("DU", ""))
            {
                TogglePanel(PanelStatus);
                lblinfo.Text = "No order allotted for you, Please contact Admin.";
                return;
            }
        }
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }
    private void MailAway(string id)
    {
        if (!Filldata("MAILAWAY", id))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
    }   
    private void InPrcoess(string id)
    {
        if (!Filldata("INPROCESS",id))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);        
    }
    private void ParcelID(string id)
    {
        if (!Filldata("PARCELID", id))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);        
    }
    private void OnHold(string id)
    {
        if (Filldata("ONHOLD", id))
        {
            TogglePanel(PanelOrderallotment);
            return;
        }
        TogglePanel(PanelStatus);
        lblinfo.Text = "No order allotted for you, Please contact Admin.";
    }
    private void OrderList(string id, object sender, EventArgs e)
    {
        ds.Dispose();
        ds.Reset();
        ds1.Dispose();
        ds1.Reset();
        string strquery = "select state,K1,QC,Review,Direct,Status,Parcel,Pend,Tax from record_status where id=" + id + "";
        string strquery1 = "select DU as Urights from user_status where User_Name='" + SessionHandler.UserName + "' limit 1";
        ds = con.ExecuteQuery(strquery);
        ds1 = con.ExecuteQuery(strquery1);
        int pend = Convert.ToInt32(ds.Tables[0].Rows[0]["Pend"]);
        int tax = Convert.ToInt32(ds.Tables[0].Rows[0]["Tax"]);
        int parcel = Convert.ToInt32(ds.Tables[0].Rows[0]["Parcel"]);
        int k1 = Convert.ToInt32(ds.Tables[0].Rows[0]["K1"]);
        int qc = Convert.ToInt32(ds.Tables[0].Rows[0]["QC"]);
        int stat = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]);
        int direct = Convert.ToInt32(ds.Tables[0].Rows[0]["Direct"]);
        int review = Convert.ToInt32(ds.Tables[0].Rows[0]["Review"]);
        string strstate = ds.Tables[0].Rows[0]["state"].ToString();
        int durights = Convert.ToInt32(ds1.Tables[0].Rows[0]["Urights"]);
        if (pend == 3) { myVariables.pType = "INPROCESS"; }
        else if (tax == 3) { myVariables.pType = "MAILAWAY"; }
        else if (parcel == 3) { myVariables.pType = "PARCELID"; }
        else if (k1 == 4 && qc == 4 && stat == 4) { myVariables.pType = "ONHOLD"; }
        else if (k1 == 0 && qc == 0 && durights == 1) { myVariables.pType = "DU"; }
        else if (k1 == 0 && qc == 0) { myVariables.pType = "KEY"; }
        else if (k1 == 2 && qc == 0) { myVariables.pType = "QC"; }
        else if (k1 == 5 && qc == 5 && stat == 5 && (review == 3 || review == 0)) { myVariables.pType = "REVIEW"; }
        if (!Filldata(myVariables.pType, id))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        
        TogglePanel(PanelOrderallotment);
        ddlstatus_SelectedIndexChanged(sender, e);
    }
    private void PAState(string id)
    {
        if (!Filldata("DU", id))
        {
            TogglePanel(PanelStatus);
            lblinfo.Text = "No order allotted for you, Please contact Admin.";
            return;
        }
        TogglePanel(PanelOrderallotment);
    }  
    private void Clearfields()
    {
        txtfollowupdate.Text = "";
        txtzipcode.Text = "";
        txtcmdhistory.Text = "";
        txtentitycount.Text = "";
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
        lblordertype.Text = "";
        //txterrorfield.Text = "";
        //ddlerrorfield.SelectedIndex = 0;
        //txtincorrect.Text = "";
        //txtcorrect.Text = "";
        ddlemailtype.SelectedIndex = 0;
        txtcountyname.Text = "";
        Lblcouny.Text = "";
        txtordertype.Text = "";
        txtBorrower.Text = "";
        txttownship.Text = "";
        ddlstatus.Text = "";
        txtaddcomments.Text = "";
        //chkparcel.Checked  = false ;
        //chkimprovements.Checked = false;
        //chkcitytax.Checked = false;
        //txtcomments.Text = "";
        ddlerror.SelectedIndex = 0;
        //ddlerrorfield.SelectedIndex = 0;
        ddlprdcomments.SelectedIndex = 0;
        txtexcomments.Text = "";
        lblqcerrorcmts.Visible = false;
        txtqcerrorcmts.Visible = false;
        txtexcomments.Visible = false;
        chkclientcmt.Visible = false;
        chkclientcmt.Checked = false;
        chksavelink.Checked = false;

        chktranstype.Checked = false;
        chktranstype1.Checked = false;
        chktranstype2.Checked = false;
        chktranstype3.Checked = false;
        chktranstype4.Checked = false;
        chkorderdoc.Checked = false;
        chkorderdoc1.Checked = false;
        chkorderdoc2.Checked = false;
        chkorderdoc3.Checked = false;
        ddlmultiple.SelectedIndex = 0;
        ddlmultiple1.SelectedIndex = 0;
        ddlmultiple2.SelectedIndex = 0;
        chkparcelid.Checked = false;
        chkparcelid1.Checked = false;
        chkparcelid2.Checked = false;
        ddlcorrect.SelectedIndex = 0;
        ddlcorrect1.SelectedIndex = 0;
        ddlcorrect2.SelectedIndex = 0;
        ddlfollow.SelectedIndex = 0;
        ddlfollow1.SelectedIndex = 0;
        ddlfollow2.SelectedIndex = 0;
        chktaxyear.Checked = false;
        chkamount.Checked = false;
        chktaxstatus.Checked = false;
        chkdates.Checked = false;
        chkbillstatus.Checked = false;
        ddlexemption.SelectedIndex = 0;
        chkcomments.Checked = false;
        chkcomments1.Checked = false;
        chkcomments2.Checked = false;
        chkcomments3.Checked = false;
        chkcomments4.Checked = false;
        ddlfollowupdate1.SelectedIndex = 0;
        ddlfollowupdate2.SelectedIndex = 0;
        ddlfollowupdate3.SelectedIndex = 0;
        ddletadate1.SelectedIndex = 0;
        ddletadate2.SelectedIndex = 0;
        chkreqtype2.Checked = false;
        GridEntity.DataSource = null;
        GridEntity.DataBind();
        //lblkeycomplete.Visible = false;
        //lblkeycomplete.Text = "";
    }
    private bool Filldata(string pType,string id)
    {
        bool result = false;
        string strstate = "";
        if (id == "")
        {
            strstate = SessionHandler.UserState;
            result = gl.GetDatas(pType, id, strstate);
        }
        else result = gl.GetDatas(pType, id, "");
        if(result)
        {
            Btnmoveqc.Visible = false;

            if (pType == "KEY") { Lblprocessname.Text = "PRODUCTION";}
            else if (pType == "QC") { Lblprocessname.Text = "QC"; }
            else if (pType == "DU") { Lblprocessname.Text = "DU"; Btnmoveqc.Visible = true; }
            else if (pType == "MAILAWAY") { Lblprocessname.Text = "MAILAWAY"; }
            else if (pType == "INPROCESS") { Lblprocessname.Text = "INPROCESS"; Btnmoveqc.Visible = true; }
            else if (pType == "PARCELID") { Lblprocessname.Text = "PARCELID"; }
            else if (pType == "ONHOLD") { Lblprocessname.Text = "ONHOLD"; }
            else if (pType == "REVIEW") { Lblprocessname.Text = "REVIEW"; }
            else if (pType == "PRIORITYKEY" || pType == "PRIORKEY") { Lblprocessname.Text = "PRODUCTION"; }
            else if (pType == "PRIORITYDU" || pType == "PRIORDU") { Lblprocessname.Text = "DU"; }

            //return strcomments + System.Environment.NewLine + etypes;


            Lblorderno.Text = myVariables.Orderno;
            Lbldate.Text = myVariables.Date;

            string timezone = GetTimeZone();
            
            Lblstate.Text = myVariables.State + " - " + timezone;

            if (myVariables.State == "FL") LnkbtnFLcalc.Visible = true;
            else LnkbtnFLcalc.Visible = false;

            //txtcommentshistory.Text = myVariables.Lastcomment;
            txtcmdhistory.Text = myVariables.Lastcomment;
            Lblcouny.Text = myVariables.County;

            lblhp.Text = myVariables.HP;
            lblprior.Text = myVariables.Prior;
            txtzipcode.Text = myVariables.Zipcode;
            txtBorrower.Text = myVariables.Borrower;
            txttownship.Text = myVariables.Township;
            if (myVariables.WebPhone == "") txtordertype.Text = "";
            else txtordertype.Text = myVariables.WebPhone;

            if (myVariables.Followupdate != "")
            {
                DateTime datime = Convert.ToDateTime(myVariables.Followupdate);
                txtfollowupdate.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", datime);
            }
            else txtfollowupdate.Text = "";

            if (Lblprocessname.Text == "PRODUCTION" || Lblprocessname.Text == "DU")
            {
                Panelprocesstime.Visible = true;
                lblordertype.Text = myVariables.OrderTp;
                if (lblordertype.Text == "Website")
                {
                    Timer1.Interval = 300000;
                    lblprocesstime.BackColor = System.Drawing.Color.Green;
                    Timer1.Enabled = true;
                }
                else if (lblordertype.Text == "Phone")
                {
                    Timer1.Interval = 420000;
                    lblprocesstime.BackColor = System.Drawing.Color.Green;
                    Timer1.Enabled = true;
                }
            }
            else Panelprocesstime.Visible = false;

            //Lbltype.Text = myVariables.WebPhone;
			//if (pType == "QC")
            //{
            //    string query = "Insert into test_tbl(Order_no,Borrower,Township,OrderType,Orderid) Values('" + myVariables.Orderno.ToString() + "','" + myVariables.Borrower.ToString() + "','" + myVariables.Township.ToString() + "','" + myVariables.WebPhone.ToString() + "','"+ id +"')";
            //    con.ExecuteSPNonQuery(query);
            //}
            //LoadStatus();

            LoadCountyLink();
            string strurl1 = "", strurl2 = "", strurl3 = "";
            if (txtassessor.Text != "") strurl1 = "window.open('" + txtassessor.Text.Trim().ToString() + "');";
            if (txtTreasurer.Text != "") strurl2 = "window.open('" + txtTreasurer.Text.Trim().ToString() + "');";

            string[] strord = Lblorderno.Text.Split('_');
            strurl3 = "window.open('https://portal.titlesource.com/vendor/Tax/OrderDetails.aspx?oid=" + strord[0].ToString() + "');";

            string strurl = "http://publicrecords.netronline.com/state/" + myVariables.State + "/county/" + Lblcouny.Text.ToLower() + "/";
            string javascript = "<script type='text/javascript'>window.open('" + strurl + "');" + strurl1 + "" + strurl2 + "" + strurl3 + "</script>";
            this.RegisterStartupScript("", javascript);

            BindOrderstatus();
        }
        return result;
    }
    private void LoadCountyLink()
    {
        ds.Dispose();
        ds.Reset();
        string[] strstate = Lblstate.Text.Split('-');
        ds = gl.CountyLink(strstate[0].Trim().ToString(), Lblcouny.Text.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtassessor.Text = Convert.ToString(ds.Tables[0].Rows[0]["Assessor"]);
            txtassphone.Text = Convert.ToString(ds.Tables[0].Rows[0]["Assessor_Phone"]);
            txtTreasurer.Text = Convert.ToString(ds.Tables[0].Rows[0]["Treasurer"]);
            txtTreasphone.Text = Convert.ToString(ds.Tables[0].Rows[0]["Treasurer_Phone"]);
            btnlinksave.Visible = false;
            btnlinkupdate.Visible = true;
        }
        else
        {
          //  btnlinksave.Visible = true;
            btnlinkupdate.Visible = false;
        }


        ddlstatus.Items.Clear();
        ddlstatus.Items.Add("");
        ddlstatus.AppendDataBoundItems = true;

        if (ds.Tables[1].Rows.Count > 0)
        {
            ddlstatus.DataSource = ds.Tables[1];
            ddlstatus.DataTextField = "statusname";
            ddlstatus.DataBind();
            if (Lblprocessname.Text == "PRODUCTION" || Lblprocessname.Text == "DU") ddlstatus.Items.Insert(8, "Others");
        }
    }
    private void BindOrderstatus()
    {
        string etypes = "", scount = "";
        int hp = Convert.ToInt32(lblhp.Text);
        int prior = Convert.ToInt32(lblprior.Text);
        if (Lblprocessname.Text == "QC")
        {
            ddlstatus.SelectedValue = myVariables.KeyStatus;
            PanelQc.Visible = true;
            if (ds.Tables[2].Rows.Count > 0)
            {
                lblkeycomplete.Text = "Remaining Key Completed : " + ds.Tables[2].Rows[0]["Key Completed"].ToString() + "";
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
            Lnkcomments.Visible = true ;
        }

        if (ds.Tables[3].Rows.Count > 0)
        {
            //txtcommentshistory.Text = GetStateComment(myVariables.State);
            txtcommentshistory.Text = Convert.ToString(ds.Tables[3].Rows[0]["State_Comment"]);
        }
        if (myVariables.State == "ME" && myVariables.County.ToLower() == "cumberland")
        {
            txtexcomments.Text = "For City of Portland - Need to order Taxcert, charges $25.00.(No Phone or Email or Fax).";
            //txtexcomments.Visible = true;
            chkclientcmt.Visible = true;
        }
        if (ds.Tables[4].Rows.Count > 0)
        {
            etypes = ds.Tables[4].Rows[0]["etype"].ToString();
            scount = ds.Tables[4].Rows[0]["ecount"].ToString();
            myVariables.ecount = scount;
            //txtexcomments.Visible = true;
            txtexcomments.Text = txtexcomments.Text + System.Environment.NewLine + "EntityType:" + System.Environment.NewLine + etypes.ToString();
        }
        if (Lblprocessname.Text == "PRODUCTION" && hp == 0 && prior > 5)
        {
            int res = Convert.ToInt32(ds.Tables[5].Rows[0]["OrderNo"]);
            if (res == 1) LoadNextOrder("Key");
        }
        else if (Lblprocessname.Text == "DU" && hp == 0 && prior > 5)
        {
            int res = Convert.ToInt32(ds.Tables[6].Rows[0]["OrderNo"]);
            if (res == 1) LoadNextOrder("DU");
        }
        else if (Lblprocessname.Text == "INPROCESS")
        {
            int res = Convert.ToInt32(ds.Tables[7].Rows[0]["OrderNo"]);
            if (res == 1) LoadNextOrder("Inprocess");
        }
    }
    private void LoadNextOrder(string status)
    {
        ds.Dispose();
        ds.Reset();
        ds = gl.LoadNextOrders(myVariables.State, Lblcouny.Text.Trim(), status);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Gridnextorder.DataSource = ds;
            Gridnextorder.DataBind();
        }
		else
        {
            Gridnextorder.DataSource = null;
            Gridnextorder.DataBind();
        }
    }
    private void LoadStatus()
    {
        ddlstatus.Items.Clear();
        ddlstatus.Items.Add("");
        ddlstatus.AppendDataBoundItems = true;
        DataSet ds = gl.LoadStatus();
        if (ds.Tables.Count > 0)
        {
            ddlstatus.DataSource = ds;
            ddlstatus.DataTextField = "statusname";
            ddlstatus.DataBind();
            if (Lblprocessname.Text == "PRODUCTION" || Lblprocessname.Text == "DU") ddlstatus.Items.Insert(8, "Others"); 
        }
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

    #region Button Events
    protected void btnsave_Click(object sender, EventArgs e)
    {
        int result = 0;
        int result1 = 0;
        if (checkValidate())
        {
            //checkValidateentity();
            result1 = ChecklistReport();
            if (ddlstatus.SelectedItem.Text == "Completed") result1 = SaveEntities();
            if (Lblprocessname.Text == "PRODUCTION" ) { result = UpdateProduction("sp_UpdateKey"); }
            if (Lblprocessname.Text == "DU") result = UpdateProduction("sp_UpdateDU");
            if (Lblprocessname.Text == "QC" || Lblprocessname.Text == "INPROCESS" || Lblprocessname.Text == "PARCELID" || Lblprocessname.Text == "ONHOLD" || Lblprocessname.Text == "MAILAWAY")
            { result = UpdateProduction("sp_UpdateQC"); }
            if (Lblprocessname.Text == "REVIEW") { result = UpdateProduction("sp_UpdateReview"); }
            if (result > 0)
            {
                if (Lblprocessname.Text == "PRODUCTION" || Lblprocessname.Text == "DU" || Lblprocessname.Text == "QC" || Lblprocessname.Text == "PARCELID" || Lblprocessname.Text == "INPROCESS" || Lblprocessname.Text == "MAILAWAY" || Lblprocessname.Text == "REVIEW")
                {
                    System.Threading.Thread.Sleep(2000);
                    if (CheckOrderallotment())
                    {
                        AutoAllotment(sender, e);
                    }
                    else
                    {
                        RRedirect();
                    }
                }
                else RRedirect();
            }
        }
    }

    private int SaveEntities()
    {
        string entity = "";
        int i = 0;
        foreach (GridViewRow rowvalues in GridEntity.Rows)
        {
            TextBox txtentitytype = (TextBox)rowvalues.FindControl("txtentitytype");
            if (i == 0) entity = txtentitytype.Text;
            else entity = entity + "," + txtentitytype.Text;
            i += 1;
        }
        int result = gl.SaveEntities(Lblorderno.Text, Lblprocessname.Text, Lbldate.Text, entity,"","","");
        return result;
    }

    private int ChecklistReport()
    {
        int result = 0;
        string status = "", transtype = "", order_doc = "", multiple_parcel = "", correct = "", follow = "", tax_year = "", amount = "", tax_status = "";
        string dates = "", tax_billstatus = "", exemption = "", followup_date = "", eta_date = "", comments = "", request_type = "", parcelid = "";
        string multiple_parcel1 = "", multiple_parcel2 = "", correct1 = "", correct2 = "", follow1 = "", follow2 = "", followup_date1 = "", followup_date2 = "";
        string eta_date1 = "";
        status = ddlstatus.SelectedItem.Text;
        if (chktranstype.Checked == true || chktranstype1.Checked == true || chktranstype2.Checked == true || chktranstype3.Checked == true || chktranstype4.Checked == true) { transtype = "CHECKED"; }
        if (chkorderdoc.Checked == true || chkorderdoc1.Checked == true || chkorderdoc2.Checked == true || chkorderdoc3.Checked == true) { order_doc = "CHECKED"; }
        multiple_parcel = ddlmultiple.SelectedItem.Text;
        multiple_parcel1 = ddlmultiple1.SelectedItem.Text;
        multiple_parcel2 = ddlmultiple2.SelectedItem.Text;
        if (chkparcelid.Checked == true || chkparcelid1.Checked == true || chkparcelid2.Checked == true) { parcelid = "CHECKED"; }
        correct = ddlcorrect.SelectedItem.Text;
        correct1 = ddlcorrect1.SelectedItem.Text;
        correct2 = ddlcorrect2.SelectedItem.Text;
        follow = ddlfollow.SelectedItem.Text;
        follow1 = ddlfollow1.SelectedItem.Text;
        follow2 = ddlfollow2.SelectedItem.Text;
        if (chktaxyear.Checked == true) { tax_year = "CHECKED"; }
        if (chkamount.Checked == true) { amount = "CHECKED"; }
        if (chktaxstatus.Checked == true) { tax_status = "CHECKED"; }
        if (chkdates.Checked == true) { dates = "CHECKED"; }
        if (chkbillstatus.Checked == true) { tax_billstatus = "CHECKED"; }
        exemption = ddlexemption.SelectedItem.Text;
        if (chktaxyear.Checked == true) { tax_year = "CHECKED"; }
        followup_date = ddlfollowupdate1.SelectedItem.Text;
        followup_date1 = ddlfollowupdate2.SelectedItem.Text;
        followup_date2 = ddlfollowupdate3.SelectedItem.Text;
        eta_date = ddletadate1.SelectedItem.Text;
        eta_date1 = ddletadate2.SelectedItem.Text;
        if (chkcomments.Checked == true || chkcomments1.Checked == true || chkcomments2.Checked == true || chkcomments3.Checked == true || chkcomments4.Checked == true) { comments = "CHECKED"; }
        if (chkreqtype2.Checked == true) { request_type = "CHECKED"; } else

        if (status == "Completed")
        {
            result = gl.Insert_CompletedChecklist(Lblorderno.Text, Lbldate.Text, Lblprocessname.Text, status, transtype, order_doc, multiple_parcel, parcelid, correct, follow, tax_year, amount, tax_status, dates, tax_billstatus, exemption, comments);
        }
        else if (status == "In Process" || status == "On Hold")
        {
            result = gl.Insert_Inpro_HoldChecklist(Lblorderno.Text, Lbldate.Text, Lblprocessname.Text, status, transtype, order_doc, multiple_parcel1, parcelid, correct1, follow1, followup_date, eta_date, comments);
        }
        else if (status == "Mail Away")
        {
            result = gl.Insert_MailawayChecklist(Lblorderno.Text, Lbldate.Text, Lblprocessname.Text, status, transtype, order_doc, multiple_parcel2, parcelid, correct2, follow2, followup_date1, eta_date1, request_type, comments);
        }
        else if (status == "ParcelID")
        {
            result = gl.Insert_ParcelidChecklist(Lblorderno.Text, Lbldate.Text, Lblprocessname.Text, status, transtype, order_doc, followup_date2, comments);
        }
        else if (status == "Rejected")
        {
            result = gl.Insert_RejectedChecklist(Lblorderno.Text, Lbldate.Text, Lblprocessname.Text, status, transtype, comments);
        }

        return result;
    }

    private int UpdateProduction(string Procedurename)
    {
        string township = Titlecase(txttownship.Text);
        string borrower = Titlecase(txtBorrower.Text);
        DateTime dat = DateTime.Now;
        dat = dat.AddHours(-7);
        //string date1 = String.Format("{0:MM/dd}", dat);
        string date1 = String.Format("{0:MM/dd/yyyy HH:mm:ss}", dat);
        string commen = date1 + SessionHandler.UserName + ":";
        string com_det = date1 + ":";
        //string commen = date1;
        string ist, ist1 = "";
        ist = System.Environment.NewLine + commen + ddlprdcomments.SelectedItem.Text;
        ist1 = System.Environment.NewLine + com_det + ddlprdcomments.SelectedItem.Text;
        string oStatus =ddlstatus.SelectedItem.Text;
        string ordertype = txtordertype.Text;

        
        string error = "", errorcategory = "", errorfield = "", correct = "", incorrect = "", qcerrorcomment = "";

        if (Lblprocessname.Text != "PRODUCTION" && Lblprocessname.Text != "DU")
        {
            if (ddlstatus.Text == "Completed")
            {
                error = ddlerror.SelectedItem.Text;
                if (error == "") { Lblerror.Text = "Please Select all Error fields"; return -1; }
                errorcategory = ddlerrorcat.SelectedItem.Text;
                if (errorcategory == "") { Lblerror.Text = "Please Select all Error fields"; return -1; }
                errorfield = ddlerrorarea.SelectedItem.Text;
                if (errorfield == "") { Lblerror.Text = "Please Select all Error fields"; return -1; }
                incorrect = ddlerrortype.SelectedItem.Text;
                if (incorrect == "") { Lblerror.Text = "Please Select all Error fields"; return -1; }
                correct = ddlcombined.SelectedItem.Text;
                if (correct == "") { Lblerror.Text = "Please Select all Error fields"; return -1; }
            }
            
        }
        //if (ddlstatus.Text == "Completed")
        //{
        //    ExemptionSave();
        //}
        string followupdate = null;
        if (txtfollowupdate.Text != "")
        {
            DateTime datime = Convert.ToDateTime(txtfollowupdate.Text);
            followupdate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", datime);
        }

        return gl.UpdateOrders(Procedurename, Lblorderno.Text, township, borrower, ist, Lbldate.Text, oStatus, ordertype, error, errorfield, correct, incorrect, Lblprocessname.Text, ist1, txtqcerrorcmts.Text, errorcategory, txtentitycount.Text.Trim(), txtzipcode.Text, followupdate);
    }
    private string Titlecase(string txt)
    {
        txt = txt.ToLower();
        return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(txt);
    }
    protected void ExemptionSave()
    {
          

        MySqlParameter[] mParam = new MySqlParameter[9];

        mParam[0] = new MySqlParameter("?$OrderNo", Lblorderno.Text);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$pdate", Lbldate.Text);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$state", myVariables.State);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$Process", Lblprocessname.Text);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$Name", SessionHandler.UserName);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        string exem1 = "", exem2 = "", exem3 = "", exem4 = "", exem5 = "", exem6 = "", exem7 = "", exem8 = "", exem9 = "";

        exem1 = lblexm1.Text + "-" + ddlexm1.Text;

        mParam[5] = new MySqlParameter("?$Exemption1", exem1);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        if (lblexm2.Visible == true)
        {
            exem2 = lblexm2.Text + "-" + ddlexm2.Text;
            mParam[6] = new MySqlParameter("?$Exemption2", exem2);
            mParam[6].MySqlDbType = MySqlDbType.VarChar;
        }
        else
        {
            mParam[6] = new MySqlParameter("?$Exemption2", "");
            mParam[6].MySqlDbType = MySqlDbType.VarChar;
        }
        if (lblexm3.Visible == true)
        {
            exem3 = lblexm3.Text + "-" + ddlexm3.Text;
            mParam[7] = new MySqlParameter("?$Exemption3", exem3);
            mParam[7].MySqlDbType = MySqlDbType.VarChar;
        }
        else
        {
            mParam[7] = new MySqlParameter("?$Exemption3", "");
            mParam[7].MySqlDbType = MySqlDbType.VarChar;
        }
        if (lblexm4.Visible == true)
        {
            exem4 = lblexm4.Text + "-" + ddlexm4.Text;
            mParam[8] = new MySqlParameter("?$Exemption4", exem4);
            mParam[8].MySqlDbType = MySqlDbType.VarChar;
        }
        else
        {
            mParam[8] = new MySqlParameter("?$Exemption4", "");
            mParam[8].MySqlDbType = MySqlDbType.VarChar;
        }
       

        int result = con.ExecuteSPNonQuery("sp_SaveExemption", true, mParam);

    }
    private bool checkValidateentity()
    {
        Lblerror.Text = "";
        bool result = true;
        
        if (txtentitycount.Text == "" || txtentitycount.Text == null || txtentitycount.Text == "0")
        {
            Lblerror.Text = "Please enter entity count.";
            result = false;
        }
      
    
        //if (ddlexm1.Text == "" || ddlexm2.Text == "" || ddlexm3.Text == "" || ddlexm4.Text == "" || ddlexm1.Text == null || ddlexm2.Text == null || ddlexm3.Text == null || ddlexm4.Text == null)
        //{
        //    Lblerror.Text = "Select the exemption type.";
        //    result = false;
        //}
       return result;
    }
    private bool checkValidate()
    {
        Lblerror.Text = "";
        bool result = true;
        if (txtordertype.Text == "") { Lblerror.Text = Lblerror.Text + "</Br>" + "OrderType is Blank."; result = false; }
        if (txtBorrower.Text == "") { Lblerror.Text = Lblerror.Text + "</Br>" + "Borrowername is Empty."; result = false; }
        if (txttownship.Text == "") { Lblerror.Text = Lblerror.Text + "</Br>" + "Township is Empty."; result = false; }
        if (txtzipcode.Text == "") { Lblerror.Text = Lblerror.Text + "</Br>" + "Zipcode is Empty."; result = false; }
        if (ddlstatus.SelectedIndex == 0) { Lblerror.Text = Lblerror.Text + "</Br>" + "Please Select the status."; result = false; }
       
        if (myVariables.State == "ME" && myVariables.County.ToLower() == "cumberland")
        {
            if (chkclientcmt.Checked == false)
            {
                Lblerror.Text = Lblerror.Text + "</Br>" + "Check the Client Comments.";
                result = false;
            }
        }

        if (ddlstatus.SelectedItem.Text == "Completed")
        {
            if (chktranstype.Checked == false || chkorderdoc.Checked == false || ddlmultiple.SelectedIndex == 0 || chkparcelid.Checked == false || ddlcorrect.SelectedIndex == 0 || ddlfollow.SelectedIndex == 0 || chktaxyear.Checked == false || chkamount.Checked == false || chktaxstatus.Checked == false || chkdates.Checked == false || chkbillstatus.Checked == false || chkcomments.Checked == false || ddlexemption.SelectedIndex == 0)
            {
                Lblerror.Text = Lblerror.Text + "</Br>" + "Check the missed CheckList.";
                result = false;
            }
        }
        if (ddlstatus.SelectedItem.Text == "In Process" || ddlstatus.SelectedItem.Text == "On Hold")
        {
            if (chktranstype1.Checked == false || chkorderdoc1.Checked == false || ddlmultiple1.SelectedIndex == 0 || chkparcelid1.Checked == false || ddlcorrect1.SelectedIndex == 0 || ddlfollow1.SelectedIndex == 0 || ddlfollowupdate1.SelectedIndex == 0 || ddletadate1.SelectedIndex == 0 || chkcomments1.Checked == false)
            {
                Lblerror.Text = Lblerror.Text + "</Br>" + "Check the missed CheckList";
                result = false;
            }
        }
        if (ddlstatus.SelectedItem.Text == "Mail Away")
        {
            if (chktranstype2.Checked == false || chkorderdoc2.Checked == false || ddlmultiple2.SelectedIndex == 0 || chkparcelid2.Checked == false || ddlcorrect2.SelectedIndex == 0 || ddlfollow2.SelectedIndex == 0 || ddlfollowupdate2.SelectedIndex == 0 || ddletadate2.SelectedIndex == 0 || chkreqtype2.Checked == false || chkcomments2.Checked == false)
            {
                Lblerror.Text = Lblerror.Text + "</Br>" + "Check the missed CheckList";
                result = false;
            }
        }
        if (ddlstatus.SelectedItem.Text == "ParcelID")
        {
            if (chktranstype3.Checked == false || chkorderdoc3.Checked == false || ddlfollowupdate3.SelectedIndex == 0 || chkcomments3.Checked == false)
            {
                Lblerror.Text = Lblerror.Text + "</Br>" + "Check the missed CheckList";
                result = false;
            }
        }
        if (ddlstatus.SelectedItem.Text == "Rejected")
        {
            if (chktranstype4.Checked == false || chkcomments4.Checked == false)
            {
                Lblerror.Text = Lblerror.Text + "</Br>" + "Check the missed CheckList";
                result = false;
            }
        }
        if ((txtentitycount.Text == "" || txtentitycount.Text == null || txtentitycount.Text == "0") && ddlstatus.SelectedItem.Text == "Completed")
        {
            Lblerror.Text = Lblerror.Text + "</Br>" + "Please enter entity count.";
            result = false;
        }
        if (ddlstatus.SelectedItem.Text == "Completed")
        {
            int gridcount = 0;
            foreach (GridViewRow rowvalues in GridEntity.Rows)
            {
                TextBox entity_type = (TextBox)rowvalues.FindControl("txtentitytype");
                string entity = entity_type.Text.ToLower();
                if (entity.Contains(Lblcouny.Text.ToLower()) == true)
                {
                    break;
                }
                else if (entity.Contains(txttownship.Text.ToLower()) == true)
                {
                    break;
                }
                else
                {
                    gridcount = gridcount + 1;
                    if (gridcount == GridEntity.Rows.Count)
                    {
                        Lblerror.Text = Lblerror.Text + "</Br>" + "Please check Entity Type...!";
                        result = false;
                    }
                }
            }
            return result;

            //if (ddlexm1.Visible == true || ddlexm2.Visible == false || ddlexm3.Visible == false || ddlexm4.Visible == false)
            //{
            //    if (ddlexm1.Text == "" || ddlexm1.Text == null)
            //    {
            //        Lblerror.Text = Lblerror.Text + "</Br>" + "Select the exemption type.";
            //        result = false;
            //    }
            //}
            //else if (ddlexm1.Visible == true || ddlexm2.Visible == true || ddlexm3.Visible == false || ddlexm4.Visible == false)
            //{
            //    if (ddlexm1.Text == "" || ddlexm1.Text == null || ddlexm2.Text == "" || ddlexm2.Text == null)
            //    {
            //        Lblerror.Text = Lblerror.Text + "</Br>" + "Select the exemption type.";
            //        result = false;
            //    }
            //}
            //else if (ddlexm1.Visible == true || ddlexm2.Visible == true || ddlexm3.Visible == true || ddlexm4.Visible == false)
            //{
            //    if (ddlexm1.Text == "" || ddlexm1.Text == null || ddlexm2.Text == "" || ddlexm2.Text == null || ddlexm3.Text == "" || ddlexm3.Text == null)
            //    {
            //        Lblerror.Text = Lblerror.Text + "</Br>" + "Select the exemption type.";
            //        result = false;
            //    }
            //}
            //else if (ddlexm1.Visible == true || ddlexm2.Visible == true || ddlexm3.Visible == true || ddlexm4.Visible == true)
            //{
            //    if (ddlexm1.Text == "" || ddlexm1.Text == null || ddlexm2.Text == "" || ddlexm2.Text == null || ddlexm3.Text == "" || ddlexm3.Text == null || ddlexm4.Text == "" || ddlexm4.Text == null)
            //    {
            //        Lblerror.Text = Lblerror.Text + "</Br>" + "Select the exemption type.";
            //        result = false;
            //    }
            //}
        }

        //if (chkcitytax.Checked == false || chkimprovements.Checked == false && chkparcel.Checked == false)
        //{
        //    Lblerror.Text = "Check the missed CheckList.";
        //    result = false;
        //}

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
            int result = gl.MoveToCall("sp_MovetoCall", Lblorderno.Text, township, borrower, ist, Lbldate.Text, oStatus, txtordertype.Text, ist1, txtzipcode.Text, txtentitycount.Text, Lblprocessname.Text);
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
            //ddllogout.SelectedIndex = 0;
            txtlogreason.Text = "";
            txtlogreason.Visible = true;
        }
    }
    protected void ddlerror_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlerror.SelectedIndex == 1)
        {
            //Paneliferror.Visible = true;
            Loaderrorcategory();
            //if (ddlstatus.SelectedItem.Text == "Completed" && (Lblprocessname.Text == "QC" || Lblprocessname.Text == "REVIEW"))
            //{
            //    lblqcerrorcmts.Visible = true;
            //    txtqcerrorcmts.Visible = true;
            //}
            //else
            //{
            //    lblqcerrorcmts.Visible = false;
            //    txtqcerrorcmts.Visible = false;
            //}

            lblqcerrorcmts.Visible = true;
            txtqcerrorcmts.Visible = true;
        }
        else
        {
            //Paneliferror.Visible = false;
            Loaderrorcategory();
            ddlerrorcat.SelectedIndex = 1;
            Loaderrorarea();
            ddlerrorarea.SelectedIndex = 1;
            Loaderrortype();
            ddlerrortype.SelectedIndex = 1;
            Loadcombined();
            ddlcombined.SelectedIndex = 1;

            lblqcerrorcmts.Visible = false;
            txtqcerrorcmts.Visible = false;
        }
        //LoadErrorField();
    }
    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblprdcomments.Text = "";
        string strddlstatus = string.Empty;
        btnrequest.Visible = false;
        Lnkcomments.Visible = false;
        PanelExemptions.Visible = false;
        strddlstatus = ddlstatus.SelectedItem.Text;
        LoadProductionComments(strddlstatus);
        if (Lblprocessname.Text != "PRODUCTION" && Lblprocessname.Text != "DU")
        {
            if (strddlstatus == "Completed" || strddlstatus == "In Process" || strddlstatus == "Mail Away" || strddlstatus == "ParcelID" || strddlstatus == "On Hold") { PanelQc.Visible = true; }//Paneliferror.Visible = false; }
            else PanelQc.Visible = false;
        }
        if (strddlstatus == "Order Missing") ddlprdcomments.SelectedIndex = 1;
        if (strddlstatus == "Completed") { TogglePanel1(PanelCompleted); }
        else if (strddlstatus == "In Process" || strddlstatus == "On Hold") { TogglePanel1(PanelInproHold); }
        else if (strddlstatus == "Mail Away") { TogglePanel1(PanelMailaway); }
        else if (strddlstatus == "ParcelID") { TogglePanel1(PanelParcelID); }
        else if (strddlstatus == "Rejected") { TogglePanel1(PanelRejected); }
        else if (strddlstatus == "" || strddlstatus == "Others" || strddlstatus == "Order Missing")
        {
            PanelCompleted.Visible = false;
            PanelInproHold.Visible = false;
            PanelMailaway.Visible = false;
            PanelParcelID.Visible = false;
            PanelRejected.Visible = false;
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
        if (strddlstatus == "In Process" || strddlstatus == "Rejected" || strddlstatus == "On Hold" || strddlstatus == "Others" || strddlstatus == "ParcelID")
        {
            btnaddcomments.Visible = true;
            txtaddcomments.Visible = true;
        }
        if (strddlstatus == "Completed" && Lblprocessname.Text == "PRODUCTION")
        {
            btnaddcomments.Visible = true;
            txtaddcomments.Visible = true;
        }
        //string[] ststate = Lblstate.Text.Split('-');
        string sstate = myVariables.State;
        if (strddlstatus == "Completed")
        {
            PanelExemptions.Visible = false;
            //Loadddlexeceptions();
            
            //EntityExemptions(sstate);
        }
        else
        {
            PanelExemptions.Visible = false;
            lblexm1.Visible = false;
            lblexm2.Visible = false;
            lblexm3.Visible = false;
            lblexm4.Visible = false;
            ddlexm1.Visible = false;
            ddlexm2.Visible = false;
            ddlexm3.Visible = false;
            ddlexm4.Visible = false;
        }

        //CheckResetOrder();
    }
    private void exeptionsfalse()
    {
        
        lblexm1.Visible = false;
        lblexm2.Visible = false;
        lblexm3.Visible = false;
        lblexm4.Visible = false;
        ddlexm1.Visible = false;
        ddlexm2.Visible = false;
        ddlexm3.Visible = false;
        ddlexm4.Visible = false;
    }
    private void CheckResetOrder()
    {
        string process, query, result = "";
        string orderno = Lblorderno.Text;
        process = Lblprocessname.Text;
        query = "Select sf_getusername('" + orderno + "','" + process + "')";
        result = con.ExecuteScalar(query);
        if (result != "" && SessionHandler.UserName != result)
        {
            string strurl = "";
            if (SessionHandler.IsAdmin == true) strurl = "Home.aspx";
            else if (SessionHandler.IsAdmin == false) strurl = "NonAdminHome.aspx";
            Response.Write(@"<script language='javascript'>alert('This Order already alloted for another user \n" + result + " .So Please close the window.');window.location.href='" + strurl + "';</script>");
        }
        else if (result == "")
        {
            string strurl = "";
            if (SessionHandler.IsAdmin == true) strurl = "Home.aspx";
            else if (SessionHandler.IsAdmin == false) strurl = "NonAdminHome.aspx";
            Response.Write(@"<script language='javascript'>alert('This Order reseted. So please close the window and take the new Order.');window.location.href='" + strurl + "';</script>");
        }
    }

    protected void txtordertype_TextChanged(object sender, EventArgs e)
    {
        if (txtordertype.Text.ToLower() == "phone") btnMovecall.Visible = true;
        else btnMovecall.Visible = false;
    }
    protected void Btnmoveqc_Click(object sender, EventArgs e)
    {
        int result = 0;
        int result1 = 0;
        if (checkValidate())
        {
            result1 = ChecklistReport();
            result = UpdateProduction("sp_UpdateKey");
            if (result > 0) RRedirect();
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

            //mailMsg.From = (new MailAddress("m.karthikeyan@stringinformation.com"));
            //mailMsg.To.Add(new MailAddress("m.karthikeyan@stringinformation.com"));
            //mailMsg.CC.Add(new MailAddress("m.karthikeyan@stringinformation.com"));

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
                    mailMsg.Subject = "String/ TSI Taxes - Change in County name - " + ordno[0].ToString();
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
                mailMsg.Body += "<br/>"; 
                //mailMsg.Body += "Thanks & Regards";
                //mailMsg.Body += "<br/>";
                //mailMsg.Body += "-------------------------------------------------------------------";
                //mailMsg.Body += "<br/>";
                //mailMsg.Body += "Service Delivery Team";
                //mailMsg.Body += "<br/>";
                //mailMsg.Body += "String Real Estate Information Services";
                //mailMsg.Body += "<br/>";
                //mailMsg.Body += "#237 on the Inc 500, #13 among all real estate firms nationwide";
                //mailMsg.Body += "<br/>";
                //mailMsg.Body += "Phone : 202-470-0648 / 49";
                //mailMsg.Body += "<br/>";
                //mailMsg.Body += "www.stringinfo.com";

                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/image010.png", FileMode.Open, FileAccess.Read);
                byte[] b = new byte[fs.Length];
                fs.Read(b, 0, (int)fs.Length);
                fs.Close();

                FileStream fsredLine = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/Redline.png", FileMode.Open, FileAccess.Read);
                byte[] bline = new byte[fsredLine.Length];
                fsredLine.Read(bline, 0, (int)fsredLine.Length);
                fsredLine.Close();


                FileStream fsLinked = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/ln.gif", FileMode.Open, FileAccess.Read);
                byte[] bLinked = new byte[fsLinked.Length];
                fsLinked.Read(bLinked, 0, (int)fsLinked.Length);
                fsLinked.Close();

                FileStream fsTwiter = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/twi.jpg", FileMode.Open, FileAccess.Read);
                byte[] bTwitter = new byte[fsTwiter.Length];
                fsTwiter.Read(bTwitter, 0, (int)fsTwiter.Length);
                fsTwiter.Close();
                
                mailMsg.Body = "<font color='Blue'>" + mailMsg.Body + "</font>";

                mailMsg.Body += DivEmail.InnerHtml.ToString();

                string body = DivEmail.InnerHtml;

                System.Net.Mail.AlternateView plainTextView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailMsg.Body, null, "text/plain");
                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString("<html><body>" + mailMsg.Body, null, "text/html");

                System.Net.Mail.LinkedResource imageResource = new System.Net.Mail.LinkedResource(fs.Name);
                imageResource.ContentId = "LOGO";


                System.Net.Mail.LinkedResource imageResourceLine = new System.Net.Mail.LinkedResource(fsredLine.Name);
                imageResourceLine.ContentId = "REDLINE";
                htmlView.LinkedResources.Add(imageResourceLine);


                System.Net.Mail.LinkedResource imageResourceLinked = new System.Net.Mail.LinkedResource(fsLinked.Name);
                imageResourceLinked.ContentId = "LN";
                htmlView.LinkedResources.Add(imageResourceLinked);

                System.Net.Mail.LinkedResource imageResourceTeitter = new System.Net.Mail.LinkedResource(fsTwiter.Name);
                imageResourceTeitter.ContentId = "TWTR";
                htmlView.LinkedResources.Add(imageResourceTeitter);

                htmlView.LinkedResources.Add(imageResource);
                mailMsg.AlternateViews.Add(plainTextView);
                mailMsg.AlternateViews.Add(htmlView);

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
        txtstatecomments.Text = myVariables.Lastcomment;
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
        else if (ddlstatus.SelectedItem.Text == "ParcelID")
        {
            query = "Update status_comments set Status_Comment='" + txtaddcomments.Text + "' where Status='ParcelID' and id='36'";
            result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                LoadProductionComments("ParcelID");
                txtaddcomments.Text = "";
            }
        }
        else if (ddlstatus.SelectedItem.Text == "Rejected")
        {
            query = "Update status_comments set Status_Comment='" + txtaddcomments.Text + "' where Status='Rejected' and id='35'";
            result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                LoadProductionComments("Rejected");
                txtaddcomments.Text = "";
            }
        }
        else if (ddlstatus.SelectedItem.Text == "Others")
        {
            query = "Update status_comments set Status_Comment='" + txtaddcomments.Text + "' where Status='Others' and id='34'";
            result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                LoadProductionComments("Others");
                txtaddcomments.Text = "";
            }
        }
        else if (ddlstatus.SelectedItem.Text == "On Hold")
        {
            query = "Update status_comments set Status_Comment='" + txtaddcomments.Text + "' where Status='On Hold' and id='33'";
            result = con.ExecuteSPNonQuery(query);
            if (result > 0)
            {
                LoadProductionComments("On Hold");
                txtaddcomments.Text = "";
            }
        }
        else if (ddlstatus.SelectedItem.Text == "Completed" && Lblprocessname.Text == "PRODUCTION")
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
    //protected void btngetcomments_Click(object sender, EventArgs e)
    //{
    //    ds.Dispose();
    //    ds.Reset();
    //    pagedimmer.Visible = true;
    //    getcomments.Visible = true;
    //    txtgetcomments.Text = "";
    //    string query = "select Comments from getcomments where State='General' or State='" + myVariables.State + "' Order by id";
    //    ds = con.ExecuteQuery(query);
    //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //    {
    //        txtgetcomments.Text = txtgetcomments.Text + ds.Tables[0].Rows[i]["Comments"].ToString();
    //    }
    //}
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
        txtbrraddress.Text = "";
        txtparcelid.Text = "";
        txtamount.Text = "";
        txttaxtype.Text = "";        
        Lblsuccess.Text = "";
        txtcity.Text = "";
        lblcity.Visible = false;
        txtcity.Visible = false;
        ddlrequesttype.SelectedIndex = 0;
        //ddltaxtype.SelectedIndex = 0;
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
                //string req_query = "select Order_no from mailaway_tbl where order_no='" + Lblorderno.Text.ToString() + "' and pDate='" + Lbldate.Text.Trim() + "' and Cheque_payable='" + txtchqpay.Text + "'";
                //int result = con.ExecuteSPNonQuery(req_query);
                //if (result != 0)
                //{
                //    string req_query1 = "delete from mailaway_tbl where order_no='" + Lblorderno.Text.ToString() + "' and pDate='" + Lbldate.Text.Trim() + "' and Cheque_payable='" + txtchqpay.Text + "'";
                //    con.ExecuteSPNonQuery(req_query1);
                //}
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
        string tempaddress = address.Replace("\r\n","+");
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
            //strbder.Append("<div>I will need to know the following:</div>");
            strbder.Append("<div>We need to know the following:</div>");
            strbder.Append("<br />");
            strbder.Append("<div>Type of collection - Annually, Semi - Annually, or Quarterly</div>");
            strbder.Append("<div>Status with specific date - paid, due, delinquent</div>");
            strbder.Append("<div>Amounts and Due dates - discount, face and penalty</div>");
            strbder.Append("<div>Delinquent taxes, if any</div>");
            //strbder.Append("<div>If any other taxes are applicable, please provide name and phone number of the entity responsible for collection.</div>");
            strbder.Append("<div>Are any other taxes are applicable? </div>");
            strbder.Append("<div>Name and phone number of the entity responsible for collection.</div>");
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
            if (req_type == "UPS" || req_type == "REGULAR" || req_type == "USPS")
            {
                strbder.Append("<div><b>Please fax this information to me if possible at 240-223-2060 or mail to taxcerts@stringinformation.com.</b></div>");
            }
            else if (req_type == "UPS/R")
            {
                strbder.Append("<div>I have included a return UPS.<b> Please fax this information to me if possible at 800-349-4782 or mail to taxcerts@stringinformation.com.</b></div>");
            }
            else if (req_type == "UPS/SASE")
            {
                strbder.Append("<div>I have included a Self Addressed Stamped Envelope.<b> Please fax this information to me if possible at 800-349-4782 or mail to taxcerts@stringinformation.com.</b></div>");
            }
            strbder.Append("<br />");
            strbder.Append("<div><b>If you want to mail the information please do so to the address: 7338 Baltimore Ave, Suite 112, College Park, MD 20740.</b></div>");
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
        strbder.Append("<div>Service Delivery Team</div>");
        strbder.Append("<div>String Real Estate Information Services</div>");


        //strbder.Append("<div style='font-family:Arial;font-size:smaller;'><table border='0'>");
        //strbder.Append("<tr><td colspan='2'><img src='../App_themes/Black/images/2.png' width='350px;'></td></tr>");
        //strbder.Append("<tr><td rowspan='4'><img src='../App_themes/Black/images/1.png'></td><td style='font-size:12px;'><span style='color:#F11C76;'>(v):</span> 202-470-0648 </td></tr>");
        //strbder.Append("<tr><td style='font-size:12px;'><span style='color:#F11C76;'>(e):</span><span style='color:Blue'><u>taxcerts@stringinformation.com</u></span></td></tr>");
        //strbder.Append("<tr><td style='font-size:12px;'><span style='color:#F11C76;'>(w):</span><span style='color:Blue'><u> www.stringinfo.com </u></span></td></tr>");
        //strbder.Append("<tr><td style='font-size:12px;'>String Information Services &nbsp;");
        //strbder.Append("<a href='http://www.linkedin.com/company/50494?trk=tyah&amp;trkInfo=tas:String%20,idx:' title='Linkedin'><img src='../App_themes/Black/images/3.png' border='0'></a>");
        //strbder.Append("<a href='https://twitter.com/Stringre' title='Twitter'><img src='../App_themes/Black/images/4.jpg' border='0'></a></td></tr>");
        //strbder.Append("</table></div>");
        strbder.Append("</div>");
        strbder.Append("</form></body></html>");
        
        e:
        try
        {
            string query = string.Empty;
            if (req_type == "THANKS REQUEST" || req_type == "REGULAR") { query = "select Output_Path_Regular from master_path"; }
            else { query = "select Output_Path_Ups from master_path";}
            if (count == 0) strFile = order_no + " - " + cheque_pay + ".doc";
            else if (count > 0) strFile = order_no + " - " + cheque_pay + count + ".doc";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(strbder.ToString());
            MemoryStream ms = new MemoryStream(data);
            string FilePath = getfullpath(strFile, query);
            if (FilePath == "") return;
            FileStream fs = new FileStream(FilePath + "/" + strFile + "", FileMode.Create);
            //FileStream fs = new FileStream(@"D:\Karthikeyan\Task\Working\Taxes\TSI Taxes\Reports\Template" + "/" + strFile + "", FileMode.CreateNew);     
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
        //string query = "select Output_Path from master_path";        
        MySqlParameter[] mParam = new MySqlParameter[1];
        MySqlDataReader mDataReader = con.ExecuteStoredProcedure(query, false, mParam);
        if (mDataReader.HasRows)
        {
            if (mDataReader.Read())
            {
                sourcePath = mDataReader.GetString(0);                
                //DateTime pde;
                //pde = Convert.ToDateTime(Lbldate.Text);
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
        string id = "", date = "", sRet = "", sPath="";        
        id = Lblorderno.Text.Trim();
        date = Lbldate.Text.Trim();        
        sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
        sRet = oInfo.Name;
        string Url =  sPath.Replace(sRet,"Request.aspx?id=" + id + "&date=" + date);
        ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + Url + "')</script>");
    }
    #endregion

    #region MailawayComments
    //protected void Lnkcomments_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("MailAwayComments.aspx"); 
    //}
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
        strlog = ddllogout.SelectedItem.Text;
        if (strlog == "Other") strlogout = strlog + "-" + txtlogreason.Text;
        string strprocess = Lblprocessname.Text;

        gl.Logout_New(strprocess, strlog, strlogout);
        RRedirect();
    }


    private void ToolLogout()
    {
        int result = 0;
        string strquery = "update checklist_login_report set flag=0,logout_time=now(),work_place_clean='CHECKED',headset_over='CHECKED',switchoff_system='CHECKED' where username='" + SessionHandler.UserName + "' and pdate=DATE_FORMAT(DATE_SUB(now(),INTERVAL '07:00' HOUR_MINUTE),'%d-%m-%Y')";
        result = con.ExecuteSPNonQuery(strquery);
        if (result > 0)
        {
            SessionHandler.UserName = "";
            SessionHandler.IsAdmin = false;
            SessionHandler.IsprocessMenu = "0";
            SessionHandler.IspendingMenu = "0";
            Response.Redirect("Loginpage.aspx");
        }
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
            LoadCountyLink();
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
            LoadCountyLink();
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

    protected void ddlprdcomments_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblprdcomments.Text = "";
        lblprdcomments.Text = ddlprdcomments.SelectedItem.Text;
        if (lblprdcomments.Text == "No answer in the Tax office" || lblprdcomments.Text == "No answer in the Assessor office" ||
            lblprdcomments.Text == "No answer in the Tax office, Tax office closed for the day" || lblprdcomments.Text == "Tax office closed for the day" ||
            lblprdcomments.Text == "Tax office closed, need to call later")
        {
            txtordertype.Text = "Phone";
        }
    }

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
            //fromid = "m.karthikeyan@stringinformation.com";
            toid = txttaxtoid.Text;
            ccid = "taxcerts@stringinformation.com";
            //ccid = "m.karthikeyan@stringinformation.com";
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

                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/image010.png", FileMode.Open, FileAccess.Read);
                byte[] b = new byte[fs.Length];
                fs.Read(b, 0, (int)fs.Length);
                fs.Close();

                FileStream fsredLine = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/Redline.png", FileMode.Open, FileAccess.Read);
                byte[] bline = new byte[fsredLine.Length];
                fsredLine.Read(bline, 0, (int)fsredLine.Length);
                fsredLine.Close();


                FileStream fsLinked = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/ln.gif", FileMode.Open, FileAccess.Read);
                byte[] bLinked = new byte[fsLinked.Length];
                fsLinked.Read(bLinked, 0, (int)fsLinked.Length);
                fsLinked.Close();

                FileStream fsTwiter = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/twi.jpg", FileMode.Open, FileAccess.Read);
                byte[] bTwitter = new byte[fsTwiter.Length];
                fsTwiter.Read(bTwitter, 0, (int)fsTwiter.Length);
                fsTwiter.Close();

                mailMsg.Body = "<font color='Blue'>" + mailMsg.Body + "</font>";

                mailMsg.Body += DivTaxEmail.InnerHtml.ToString();

                string body = DivTaxEmail.InnerHtml;

                System.Net.Mail.AlternateView plainTextView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailMsg.Body, null, "text/plain");
                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString("<html><body>" + mailMsg.Body, null, "text/html");

                System.Net.Mail.LinkedResource imageResource = new System.Net.Mail.LinkedResource(fs.Name);
                imageResource.ContentId = "LOGO";


                System.Net.Mail.LinkedResource imageResourceLine = new System.Net.Mail.LinkedResource(fsredLine.Name);
                imageResourceLine.ContentId = "REDLINE";
                htmlView.LinkedResources.Add(imageResourceLine);


                System.Net.Mail.LinkedResource imageResourceLinked = new System.Net.Mail.LinkedResource(fsLinked.Name);
                imageResourceLinked.ContentId = "LN";
                htmlView.LinkedResources.Add(imageResourceLinked);

                System.Net.Mail.LinkedResource imageResourceTeitter = new System.Net.Mail.LinkedResource(fsTwiter.Name);
                imageResourceTeitter.ContentId = "TWTR";
                htmlView.LinkedResources.Add(imageResourceTeitter);

                htmlView.LinkedResources.Add(imageResource);
                mailMsg.AlternateViews.Add(plainTextView);
                mailMsg.AlternateViews.Add(htmlView);

               // mailMsg.Body += "Thanks & Regards";
               // mailMsg.Body += "<br/>";
               // mailMsg.Body += "-------------------------------------------------------------------";
               // mailMsg.Body += "<br/>";
               // mailMsg.Body += "Service Delivery Team";
               // mailMsg.Body += "<br/>";
               // mailMsg.Body += "String Real Estate Information Services";
               // mailMsg.Body += "<br/>";
               // mailMsg.Body += "#237 on the Inc 500, #13 among all real estate firms nationwide";
               // mailMsg.Body += "<br/>";
               //// mailMsg.Body += "Phone : 202-470-0648 / 49";
               // mailMsg.Body += "Phone : 202-470-2173/0655";
               // mailMsg.Body += "<br/>";
               // mailMsg.Body += "www.stringinfo.com";

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

    protected void ddllogout_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddllogout.SelectedItem.Text == "Other") txtlogreason.Visible = true;
        else txtlogreason.Visible = false;
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Timer1.Enabled = false;
        if (lblordertype.Text == "Website")
        {
            lblprocesstime.BackColor = System.Drawing.Color.Blue;
            Timer2.Interval = 120000;
        }
        else if (lblordertype.Text == "Phone")
        {
            lblprocesstime.BackColor = System.Drawing.Color.Blue;
            Timer2.Interval = 180000;
        }
        Timer2.Enabled = true;
    }
    protected void Timer2_Tick(object sender, EventArgs e)
    {
        Timer2.Enabled = false;
        lblprocesstime.BackColor = System.Drawing.Color.Red;
    }
    protected void txtentitycount_TextChanged(object sender, EventArgs e)
    {
        string scount = Convert.ToString(myVariables.ecount);
        if (txtentitycount.Text == "" || txtentitycount.Text == null || txtentitycount.Text == "0" )
        {
            lblexemhead.Text = "Please enter entity count.";
            pagedimmer1.Visible = true;
            exemptionsmsgbox.Visible = true;
        }
        else
        {
            if (scount != txtentitycount.Text)
            {
                lblexemhead.Text = "Do you want proceed further.";
                pagedimmer1.Visible = true;
                exemptionsmsgbox.Visible = true;
            }
            int entcount = Convert.ToInt32(txtentitycount.Text);
            DataTable dtt = new DataTable();
            dtt.Columns.Add("EntityTypes");
            for (int i = 0; i < entcount; i++)
            {
                DataRow dr = dtt.NewRow();
                dr[0] = "";
                dtt.Rows.Add(dr);
            }
            GridEntity.DataSource = dtt;
            GridEntity.DataBind();
            //btnaddtxtbox_Click(sender, e);
        }

    }
    protected void btnExemptionsOk_Click(object sender, EventArgs e)
    {
        pagedimmer1.Visible = false;
        exemptionsmsgbox.Visible = false;
    }
    protected void Loadddlexeceptions()
    {
        DataSet ds = new DataSet();

        string query = "select EStatus from exemptionstatus";
        ds = con.ExecuteQuery(query);

        ddlexm1.DataSource = ds;
        ddlexm1.DataTextField = "EStatus";
        ddlexm1.DataBind();
        ddlexm2.DataSource = ds;
        ddlexm2.DataTextField = "EStatus";
        ddlexm2.DataBind();
        ddlexm3.DataSource = ds;
        ddlexm3.DataTextField = "EStatus";
        ddlexm3.DataBind();
        ddlexm4.DataSource = ds;
        ddlexm4.DataTextField = "EStatus";
        ddlexm4.DataBind();
        

    }
    protected void EntityExemptions(string enstate)
    {

        lblexm1.Visible = false;
        ddlexm1.Visible = false;
        lblexm2.Visible = false;
        ddlexm2.Visible = false;
        lblexm3.Visible = false;
        ddlexm3.Visible = false;
        lblexm4.Visible = false;
        ddlexm4.Visible = false;
         if (enstate == "AL")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
            
        }
        else if (enstate == "AK")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
           
        }
        else  if (enstate == "AZ")
        {
            lblexm1.Text = "State Aid Credit:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
           
        }
         else if (enstate == "AR")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "CA")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
           
        }
        else if (enstate == "CO")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
           
        }
        else if (enstate == "CT")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
       
        else if (enstate == "DE")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "DC")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "FL")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
          
            
        }

        else if (enstate == "GA")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "HI")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "ID")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "IL")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate.ToString() == "IN")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "IA")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "KS")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "KY")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "LA")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "ME")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
           
        }

        else if (enstate == "MD")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            

        }
        else if (enstate == "MA")
         {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
         }
        else if (enstate == "MI")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
           
        }
        else if (enstate == "MN")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
           
        }
        else if (enstate == "MS")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "MO")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "MT")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "NE")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "NV")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
          
            
        }
        else if (enstate == "NH")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "NJ")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "NM")
        {

            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "NY")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "NC")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "ND")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "OH")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "OK")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "OR")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "PA")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "RI")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "SC")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "SD")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            

        }
        else if (enstate == "TN")
        {
            lblexm1.Text = "Homestead Exemption:";

            lblexm1.Visible = true;
            ddlexm1.Visible = true;
           

        }
        else if (enstate == "TX")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "UT")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "VT")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
        else if (enstate == "VA")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            
            
        }
        else if (enstate == "WA")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
           
        }
        else if (enstate == "WV")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;
        }
        else if (enstate == "WI")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            
           
        }
        else if (enstate == "WY")
        {
            lblexm1.Text = "Homestead Exemption:";
            lblexm2.Text = "Disabled Veterans Exemption:";
            lblexm3.Text = "Disabled Exemption:";
            lblexm4.Text = "Over 65 Exemption:";
            lblexm1.Visible = true;
            ddlexm1.Visible = true;
            lblexm2.Visible = true;
            ddlexm2.Visible = true;
            lblexm3.Visible = true;
            ddlexm3.Visible = true;
            lblexm4.Visible = true;
            ddlexm4.Visible = true;

        }
    }

    protected void ddlexm1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string st = myVariables.State;
        if (ddlexm1.Text == "" || ddlexm1.Text == null)
        {
            lblexemhead.Text = "Please select any one exemption type.";
            pagedimmer1.Visible = true;
            exemptionsmsgbox.Visible = true;
        }
        else
        {
          if (st == "CO" || st == "CT" || st == "DE" || st == "MO" || st == "NV" || st == "NH" || st == "OR" || st == "TN" || st == "VT" || st == "VA" || st == "WI")
            {
                if (ddlexm1.Text != "NO")
                {
                    lblexemhead.Text = "Do you want proceed further.";
                    pagedimmer1.Visible = true;
                    exemptionsmsgbox.Visible = true;
                }
            }
            else
            {
                if (ddlexm1.Text != "YES")
                {
                    lblexemhead.Text = "Do you want proceed further.";
                    pagedimmer1.Visible = true;
                    exemptionsmsgbox.Visible = true;
                }
            }
        }
    }
    protected void ddlexm2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlexm2.Text == null || ddlexm2.Text == "")
        {
            lblexemhead.Text = "Please select any one exemption type.";
            pagedimmer1.Visible = true;
            exemptionsmsgbox.Visible = true;
        }
        else
        {
            if (ddlexm2.Text != "YES")
            {
                lblexemhead.Text = "Do you want proceed further.";
                pagedimmer1.Visible = true;
                exemptionsmsgbox.Visible = true;
            }
        }
    }
    protected void ddlexm3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlexm3.Text == null || ddlexm3.Text == "")
        {
            lblexemhead.Text = "Please select any one exemption type.";
            pagedimmer1.Visible = true;
            exemptionsmsgbox.Visible = true;
        }
        else
        {
            if (ddlexm3.Text != "YES")
            {
                lblexemhead.Text = "Do you want proceed further.";
                pagedimmer1.Visible = true;
                exemptionsmsgbox.Visible = true;
            }
        }
    }
    protected void ddlexm4_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlexm4.Text == null || ddlexm4.Text == "")
        {
            lblexemhead.Text = "Please select any one exemption type.";
            pagedimmer1.Visible = true;
            exemptionsmsgbox.Visible = true;
        }
        else
        {
            if (ddlexm4.Text != "YES")
            {
                lblexemhead.Text = "Do you want proceed.";
                pagedimmer1.Visible = true;
                exemptionsmsgbox.Visible = true;
            }
        }
    }
    protected void LnkbtnFLcalc_Click(object sender, EventArgs e)
    {
        pagedimmer.Visible = true;
        getcomments.Visible = true;
        txtflamount.Text = "";
        txtgetcomments.Text = "";
        lblFLerror.Text = "";
    }
    protected void Lnkcalculate_Click(object sender, EventArgs e)
    {
        txtgetcomments.Text = "";
        double amount = 0;
        try
        {
            amount = Convert.ToDouble(txtflamount.Text);
        }
        catch (Exception ex)
        {
            lblFLerror.Text = "Please enter the valid amount.";
            return;
        }
        double onepercen, twopercen, threepercen, fourpercen = 0.00;
        onepercen = amount - ((amount / 100) * 1);
        twopercen = amount - ((amount / 100) * 2);
        threepercen = amount - ((amount / 100) * 3);
        fourpercen = amount - ((amount / 100) * 4);

        onepercen = Math.Round(onepercen, 2);
        twopercen = Math.Round(twopercen, 2);
        threepercen = Math.Round(threepercen, 2);
        fourpercen = Math.Round(fourpercen, 2);

        txtgetcomments.Text = txtgetcomments.Text + "4% of Amount - " + fourpercen + Environment.NewLine;
        txtgetcomments.Text = txtgetcomments.Text + "3% of Amount - " + threepercen + Environment.NewLine;
        txtgetcomments.Text = txtgetcomments.Text + "2% of Amount - " + twopercen + Environment.NewLine;
        txtgetcomments.Text = txtgetcomments.Text + "1% of Amount - " + onepercen + Environment.NewLine;

        txtgetcomments.Text = txtgetcomments.Text + Environment.NewLine + Environment.NewLine + "Taxes on Land and Improvements. If paid by 12/31/2013, amount due is $" + threepercen + "; If paid by 01/31/2014, amount due is $" + twopercen + "; If paid by 02/28/2014, amount due is $" + onepercen + "";
        lblFLerror.Text = "";
    }
    protected void btnparcelmail_Click(object sender, EventArgs e)
    {
        string fromid, ccid, toid, strsubject = "";
        string[] ordno;

        try
        {
            fromid = "tsi@stringinfo.com";
            //fromid = "m.karthikeyan@stringinformation.com";
            toid = "tsitaxvendor@titlesource.com";
            //toid = "m.karthikeyan@stringinformation.com";
            ccid = "tsi@stringinfo.com";
            //ccid = "m.karthikeyan@stringinformation.com";
            strsubject = " String/TSI - Order # ";
            if (fromid != null && toid != null && ccid != null && strsubject != null)
            {
                ordno = Lblorderno.Text.Split('_');
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
                mailMsg.Subject = strsubject + ordno[0].ToString();

                mailMsg.IsBodyHtml = true;
                mailMsg.Body = "Hi";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "For the order# " + ordno[0].ToString();
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "We have found " + txtnoofparcels.Text + " parcels mentioned above while searching through the owners name.";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += txt_parcels.Text.Replace("\r\n", "<br/>");
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "But in the abstract we have found only one parcel # " + tst_parcelno.Text;
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "Please advise.";
                mailMsg.Body += "<br/>";
                mailMsg.Body += "<br/>";

                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/image010.png", FileMode.Open, FileAccess.Read);
                byte[] b = new byte[fs.Length];
                fs.Read(b, 0, (int)fs.Length);
                fs.Close();

                FileStream fsredLine = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/Redline.png", FileMode.Open, FileAccess.Read);
                byte[] bline = new byte[fsredLine.Length];
                fsredLine.Read(bline, 0, (int)fsredLine.Length);
                fsredLine.Close();


                FileStream fsLinked = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/ln.gif", FileMode.Open, FileAccess.Read);
                byte[] bLinked = new byte[fsLinked.Length];
                fsLinked.Read(bLinked, 0, (int)fsLinked.Length);
                fsLinked.Close();

                FileStream fsTwiter = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/App_themes/Black/images/twi.jpg", FileMode.Open, FileAccess.Read);
                byte[] bTwitter = new byte[fsTwiter.Length];
                fsTwiter.Read(bTwitter, 0, (int)fsTwiter.Length);
                fsTwiter.Close();

                mailMsg.Body = "<font color='Blue'>" + mailMsg.Body + "</font>";

                mailMsg.Body += DivEmail.InnerHtml.ToString();

                string body = DivEmail.InnerHtml;

                System.Net.Mail.AlternateView plainTextView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailMsg.Body, null, "text/plain");
                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString("<html><body>" + mailMsg.Body, null, "text/html");

                System.Net.Mail.LinkedResource imageResource = new System.Net.Mail.LinkedResource(fs.Name);
                imageResource.ContentId = "LOGO";


                System.Net.Mail.LinkedResource imageResourceLine = new System.Net.Mail.LinkedResource(fsredLine.Name);
                imageResourceLine.ContentId = "REDLINE";
                htmlView.LinkedResources.Add(imageResourceLine);


                System.Net.Mail.LinkedResource imageResourceLinked = new System.Net.Mail.LinkedResource(fsLinked.Name);
                imageResourceLinked.ContentId = "LN";
                htmlView.LinkedResources.Add(imageResourceLinked);

                System.Net.Mail.LinkedResource imageResourceTeitter = new System.Net.Mail.LinkedResource(fsTwiter.Name);
                imageResourceTeitter.ContentId = "TWTR";
                htmlView.LinkedResources.Add(imageResourceTeitter);

                htmlView.LinkedResources.Add(imageResource);
                mailMsg.AlternateViews.Add(plainTextView);
                mailMsg.AlternateViews.Add(htmlView);

                // mailMsg.Body += "Thanks & Regards";
                // mailMsg.Body += "<br/>";
                // mailMsg.Body += "-------------------------------------------------------------------";
                // mailMsg.Body += "<br/>";
                // mailMsg.Body += "Service Delivery Team";
                // mailMsg.Body += "<br/>";
                // mailMsg.Body += "String Real Estate Information Services";
                // mailMsg.Body += "<br/>";
                // mailMsg.Body += "#237 on the Inc 500, #13 among all real estate firms nationwide";
                // mailMsg.Body += "<br/>";
                //// mailMsg.Body += "Phone : 202-470-0648 / 49";
                // mailMsg.Body += "Phone : 202-470-2173/0655";
                // mailMsg.Body += "<br/>";
                // mailMsg.Body += "www.stringinfo.com";

                mailMsg.Priority = MailPriority.Normal;
                SmtpClient mSmtpClient = new SmtpClient();
                mSmtpClient.Send(mailMsg);
                lblparcelerror.Text = "Mail Successfully Delivered...";
            }
            else
            {
                lblparcelerror.Text = "Pease save Fromid,Toid,CCid for this process.";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnparcelcancel_Click(object sender, EventArgs e)
    {
        txtnoofparcels.Text = "";
        txt_parcels.Text = "";
        tst_parcelno.Text = "";
        ParcelInformation.Visible = false;
        pagedimmer.Visible = false;
        chkparcelmail.Checked = false;
    }
    protected void chkparcelmail_CheckedChanged(object sender, EventArgs e)
    {
        if (chkparcelmail.Checked == true)
        {
            ParcelInformation.Visible = true;
            pagedimmer.Visible = true;
        }
        else
        {
            ParcelInformation.Visible = false;
            pagedimmer.Visible = false;
        }
    }
    protected void btnaddtxtbox_Click(object sender, EventArgs e)
    {
        //if (txtentitycount.Text == "" || txtentitycount.Text == null || txtentitycount.Text == "0") return;
        //int txtcount = Convert.ToInt32(txtentitycount.Text);
        //TextBox txtbox;
        //for (int i = 0; i < txtcount; i++)
        //{
        //    txtbox = new TextBox();
        //    txtbox.ID = "txtentity" + i + 1;
        //    Panelprocesstime.Controls.Add(txtbox);
        //    txtbox.CssClass = "txtuser";
        //    txtbox.Width = 212;
        //}

        //int gridcount = 0;
        //foreach (GridViewRow rowvalues in GridEntity.Rows)
        //{
        //    TextBox entity_type = (TextBox)rowvalues.FindControl("txtentitytype");
        //    string entity = entity_type.Text;
        //    if (entity.Contains(Lblcouny.Text) == true)
        //    {
        //        Lblerror.Text = "";
        //        break;
        //    }
        //    else
        //    {
        //        gridcount = gridcount + 1;
        //        if (gridcount == GridEntity.Rows.Count)
        //        {
        //            Lblerror.Text = Lblerror.Text + "</Br>" + "Please check Entity Type...!";
        //        }
        //    }
        //}
    }
    protected void txtfollowupdate_TextChanged(object sender, EventArgs e)
    {
        DateTime dat = Convert.ToDateTime(txtfollowupdate.Text);
        string date = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dat);
    }
}
