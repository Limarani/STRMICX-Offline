using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class Pages_STRMICX_Production : System.Web.UI.Page
{
   
    GlobalClass gl = new GlobalClass();
    StringBuilder strresponse = new StringBuilder();
    string responsemsg = "";
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == null || SessionHandler.UserName == "")
        {
            //SessionHandler.Redirect("Login.aspx"); 
            return;
        }

        if (!Page.IsPostBack)
        {
            //LoadStatus();
           
            //Orderallotment();
            //PanelLien.Style.Add("height", "500");
            //myVariables.Logopath = Server.MapPath("~/Images/coversheet.jpg");
            //Lblerror.Text = "";
        }
        divresware.Visible = true;
    }

    #region tab
    protected void Tab1_Click(object sender, EventArgs e)
    {
        Tab1.CssClass = "Clicked";
        Tab2.CssClass = "Initial";
        Tab3.CssClass = "Initial";
        Tab4.CssClass = "Initial";
        //MainView.ActiveViewIndex = 0;
        //loadgridgeneral();
    }
    protected void Tab2_Click(object sender, EventArgs e)
    {
        Tab1.CssClass = "Initial";
        Tab2.CssClass = "Clicked";
        Tab3.CssClass = "Initial";
        Tab4.CssClass = "Initial";
        //MainView.ActiveViewIndex = 1;
        //loadgridchain();
    }
    protected void Tab3_Click(object sender, EventArgs e)
    {
        Tab1.CssClass = "Initial";
        Tab2.CssClass = "Initial";
        Tab3.CssClass = "Clicked";
        Tab4.CssClass = "Initial";
        MainView.ActiveViewIndex = 0;
        //loadgridtax();
    }
    protected void Tab4_Click(object sender, EventArgs e)
    {
        Tab1.CssClass = "Initial";
        Tab2.CssClass = "Initial";
        Tab3.CssClass = "Initial";
        Tab4.CssClass = "Clicked";
        //MainView.ActiveViewIndex = 3;
        //loadgridlien();
    }
    #endregion

    #region Production
    private void LoadStatus()
    {
        DataSet ds = gl.LoadStatus();
        if (ds.Tables.Count > 0)
        {
            ddlstatus.DataSource = ds;
            ddlstatus.DataTextField = "status";
            ddlstatus.DataBind();
        }
    }
    //protected void btnsave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string comments = "";
    //        if (txtcommnets.Value == string.Empty)
    //        {
    //            lblProdErr.Text = "Search comment is required.";
    //            //Response.Write("<script>alert('Search comment is required.');</script>");
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalError();", true);
    //            return;
    //        }

    //        comments = txtcommnets.Value;

    //        int result = 0;
    //        if (lblProcess.Text.ToUpper() == "KEYING" || lblProcess.Text.ToUpper() == "KEYINGDU")
    //        { gl.InsertKeyingData(); }
    //        if (ddlstatus.Value == "Completed")
    //        {
    //            if (lblProcess.Text.ToLower() == "searchqc" || lblProcess.Text.ToLower() == "searchdu" || lblProcess.Text.ToLower() == "search")
    //            {
    //                if (txtpagecount.Value == string.Empty)
    //                {
    //                    lblProdErr.Text = "Page count is required.";
    //                    //Response.Write("<script>alert('Page count is required.');</script>");
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalError();", true);
    //                    return;
    //                }
    //            }

    //            if (lblProcess.Text.ToLower() == "searchqc" || lblProcess.Text.ToLower() == "searchdu")
    //            {
    //                // string spackuplod = SessionHandler.Spackupload;
    //                string spackuplod = gl.GetSpackupload(SessionHandler.OrderId);
    //                if (spackuplod == "0")
    //                {
    //                    lblProdErr.Text = "Need to upload package.";
    //                    // Response.Write("<script>alert('Need to upload package');</script>");
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalError();", true);
    //                    return;
    //                }
    //            }
    //            //else
    //            //{ btnsave.Enabled = true; }

    //            if (lblProcess.Text.ToLower() == "searchqc" || lblProcess.Text.ToLower() == "searchdu" || lblProcess.Text.ToLower() == "search")
    //            {
    //                result = gl.UpdateOrdersstatus(lblOrderNo.Text, lblCreatedDate.Text, lblState.Text, lblCounty.Text, lblProcess.Text, txtProType.Text, ddlOrderType.Text, txtBrwrName.Value, txtPropAdrs.Value, comments, txtpagecount.Value, ddlstatus.Value, txtgensearchdate.Value.Trim(), txtgennamesrchd.Value.Trim(), txtgenadrsprclsrchd.Value.Trim());
    //            }
    //            else
    //            {
    //                result = gl.UpdateOrdersstatus(lblOrderNo.Text, lblCreatedDate.Text, lblState.Text, lblCounty.Text, lblProcess.Text, txtProType.Text, ddlOrderType.Text, txtBrwrName.Value, txtPropAdrs.Value, comments, "", ddlstatus.Value, "", "", "");
    //            }
    //            // lblglobalerror.Text = "Order moved to SearchQC";
    //            if (lblProcess.Text.ToUpper() == "KEYINGQC" || lblProcess.Text.ToUpper() == "KEYINGDU")
    //            {
    //                btnordersave.Disabled = true;
    //                btnResware.Visible = true;
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
    //                //Orderallotment();
    //            }
    //        }
    //        else
    //        {
    //            if (lblProcess.Text.ToLower() == "searchqc" || lblProcess.Text.ToLower() == "searchdu" || lblProcess.Text.ToLower() == "search")
    //            {
    //                result = gl.UpdateOrdersstatus(lblOrderNo.Text, lblCreatedDate.Text, lblState.Text, lblCounty.Text, lblProcess.Text, txtProType.Text, ddlOrderType.Text, txtBrwrName.Value, txtPropAdrs.Value, comments, txtpagecount.Value, ddlstatus.Value, txtgensearchdate.Value.Trim(), txtgennamesrchd.Value.Trim(), txtgenadrsprclsrchd.Value.Trim());
    //            }
    //            else
    //            {
    //                result = gl.UpdateOrdersstatus(lblOrderNo.Text, lblCreatedDate.Text, lblState.Text, lblCounty.Text, lblProcess.Text, txtProType.Text, ddlOrderType.Text, txtBrwrName.Value, txtPropAdrs.Value, comments, "", ddlstatus.Value, "", "", "");
    //            }

    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
    //        }
    //    }

    //    catch (Exception ex)
    //    {
    //        gl.Insertlog("error", "production", "STRFAM", ex.Message.ToString(), SessionHandler.UserName);
    //        //  lblglobalerror.Text = ex.Message.ToString();
    //    }
    //}
    private void Clearcontrols()
    {
        lblOrderNo.Text = "";
        lblProUsrNme.Text = "";
        lblCreatedDate.Text = "";
        lblState.Text = "";
        lblCounty.Text = "";
        lblProcess.Text = "";
        txtProType.Text = "";
        //ddlOrderType.SelectedIndex = 0;
        //txtBrwrName.Value = "";
        //txtsellerName.Value = "";
        //txtPropAdrs.Value = "";
        //txtgeneffdate.Value = "";
        //txtgenvesting.Value = "";
        //txtgenlegal.Value = "";
        //lblOperator.Text = "";
        //Richpackage.Text = "";
        //txtpagecount.Value = "";
        lblMessage.Text = "";

        //txtgensearchdate.Value = "";
        //txtgennamesrchd.Value = "";
        //txtgenadrsprclsrchd.Value = "";
    }
    private void Orderallotment()
    {
        //$('#divresware').show();
        //divresware.Visible = true;
    }
    //private void Orderallotment()
    //{
    //    Session["TimePro"] = DateTime.Now.ToString("H:mm:ss");
    //    Clearcontrols();
    //    //lblglobalerror.Text = "";
    //    DataSet ds = gl.Orderallotment();
    //    if (ds.Tables.Count > 0)
    //    {
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            string orderno = ds.Tables[0].Rows[0]["orderno"].ToString();
    //            lblOrderNo.Text = orderno.Trim();

    //            lblProUsrNme.Text = SessionHandler.UserName.Trim();

    //            string pdate = ds.Tables[0].Rows[0]["pdate"].ToString();
    //            lblCreatedDate.Text = pdate.Trim();

    //            string state = ds.Tables[0].Rows[0]["state"].ToString();
    //            lblState.Text = state.Trim();

    //            string county = ds.Tables[0].Rows[0]["county"].ToString();
    //            lblCounty.Text = county.Trim();

    //            string uRights = ds.Tables[0].Rows[0]["uRights"].ToString();
    //            lblProcess.Text = uRights.Trim();

    //            string protype = ds.Tables[0].Rows[0]["producttype"].ToString();
    //            txtProType.Text = protype.Trim();

    //            string ordertype = ds.Tables[0].Rows[0]["ordertype"].ToString();
    //            ddlOrderType.Text = ordertype.Trim();

    //            string clientaccountno = ds.Tables[0].Rows[0]["clientaccountno"].ToString();
    //            lblClntAccNo.Text = clientaccountno.Trim();

    //            string Loanamount = ds.Tables[0].Rows[0]["Loanamount"].ToString();
    //            lblloanamount.Text = Loanamount.Trim();

    //            string fees = ds.Tables[0].Rows[0]["Fees"].ToString();
    //            lblfees.Text = fees.Trim();

    //            lblprotiertype.Text = ds.Tables[0].Rows[0]["protiertype"].ToString();
    //            SessionHandler.ProcessName = lblProcess.Text;
    //            SessionHandler.OrderId = ds.Tables[0].Rows[0]["O_ID"].ToString();
    //            if (lblProcess.Text.ToLower() == "searchdu" || lblProcess.Text.ToLower() == "searchqc")
    //            {
    //                SessionHandler.Spackupload = ds.Tables[0].Rows[0]["spackage_uploded"].ToString();
    //            }

    //            txtPropAdrs.Value = ds.Tables[0].Rows[0]["propertyaddress"].ToString();

    //            if (lblProcess.Text == "search" || lblProcess.Text == "searchqc" || lblProcess.Text == "searchdu")
    //            {
    //                DataSet dsbwr = gl.GetBorrowerName(SessionHandler.OrderId, lblOrderNo.Text);
    //                if (dsbwr.Tables.Count > 0)
    //                {
    //                    if (dsbwr.Tables[0].Rows.Count > 0)
    //                    { txtBrwrName.Value = dsbwr.Tables[0].Rows[0]["Buyer"].ToString(); }
    //                    if (dsbwr.Tables[1].Rows.Count > 0)
    //                    { txtsellerName.Value = dsbwr.Tables[1].Rows[0]["Seller"].ToString(); }
    //                }
    //                tblgeneralkey.Visible = true;
    //                btnGenCvrSht.Visible = false;
    //            }
    //            if (lblProcess.Text == "searchqc" || lblProcess.Text == "searchdu")
    //            { panelpackage.Visible = true; }
    //            //if (lblProcess.Text == "searchqc" || lblProcess.Text == "searchdu")
    //            //{
    //            //    string spackuplod = ds.Tables[0].Rows[0]["spackage_uploded"].ToString();
    //            //    if (spackuplod == "1") { btnordersave.Disabled = false; }
    //            //    else { btnordersave.Disabled = true; }
    //            //    btnGenCvrSht.Visible = false;
    //            //}
    //            //else
    //            //{ btnsave.Enabled = true; }

    //            if ((lblProcess.Text == "keying" || lblProcess.Text == "keyingqc" || lblProcess.Text == "keyingdu") && (ddlOrderType.Text == "Online" || ddlOrderType.Text == "DataTrace"))
    //            {
    //                btnGenCvrSht.Visible = true;
    //            }
    //            if ((lblProcess.Text == "keying" || lblProcess.Text == "keyingqc" || lblProcess.Text == "keyingdu") && (ddlOrderType.Text == "Abstractor"))
    //            {
    //                btnGenCvrSht.Visible = false;
    //            }

    //            if (lblProcess.Text == "keying" || lblProcess.Text == "keyingqc" || lblProcess.Text == "keyingdu")
    //            {
    //                divresware.Visible = true;
    //                Tab1.CssClass = "Clicked";
    //                MainView.ActiveViewIndex = 0;
    //                //loaddeedtype();
    //                //loadlientype();
    //                loadtaxtype();
    //                filltaxentity();
    //                //loadgridgeneral();
    //                //loadgridchain();
    //                loadgridtax();
    //                //loadgridlien();
    //                //loadgeneralinfo();
    //                txtpaymentfrequency.SelectedIndex = 0;
    //                //btnGenCvrSht.Visible = true;
    //            }
    //        }
    //        //else SessionHandler.Redirect("Noorders.aspx");
    //    }
    //    //else SessionHandler.Redirect("Noorders.aspx");
    //}
    #endregion



    #region Taxes
    protected void loadtaxtype()
    {
        string query = "select taxtypeid,taxtypename from tbl_taxtypeid";
        DataSet ds = new DataSet();
        ds = gl.ExecuteQuery(query);
        //txttaxtype.DataSource = ds.Tables[0];
        //txttaxtype.DataTextField = "taxtypename";
        //txttaxtype.DataValueField = "taxtypename";
        txttaxtype.DataBind();

    }
    protected void filltaxentity()
    {
        //if (lblprotiertype.Text.ToLower() == "equity")
        //{ txttaxentity.Value = "EQU-IN32"; }
        //else if (lblprotiertype.Text.ToLower() == "dto")
        //{ txttaxentity.Value = "SER-IN32"; }
        //else if (lblprotiertype.Text.ToLower() == "postclose")
        //{ txttaxentity.Value = "PCP-IN32"; }

    }
    protected void loadgridtax()
    {
        string query = "";
        if (lblProcess.Text == "keying" || lblProcess.Text == "keyingdu")
        {
            query = "select * from tbl_search_tax_key where OrderNo='" + lblOrderNo.Text + "' order by Priority asc";
        }
        else
        {
            query = "select * from tbl_search_tax_qc where OrderNo='" + lblOrderNo.Text + "' order by Priority asc";
        }
        DataSet ds = gl.ExecuteQuery(query);
        Gvtax.DataSource = ds.Tables[0];
        Gvtax.DataBind();
        if (Gvtax.Rows.Count > 0)
        {
            GridViewRow FirstRow = Gvtax.Rows[0];
            Button btnUp = (Button)FirstRow.FindControl("btnUp");
            btnUp.Enabled = false;
            GridViewRow LastRow = Gvtax.Rows[Gvtax.Rows.Count - 1];
            Button btnDown = (Button)LastRow.FindControl("btnDown");
            btnDown.Enabled = false;
        }
    }
    protected void txtpaymentfrequency_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (txtpaymentfrequency.Text == "Annual")
        {
            tabletax1.Visible = true;
            tabletax2.Visible = false;
            tabletax3.Visible = false;
            tabletax4.Visible = false;
        }
        else if (txtpaymentfrequency.Text == "Semiannual")
        {
            tabletax1.Visible = true;
            tabletax2.Visible = true;
            tabletax3.Visible = false;
            tabletax4.Visible = false;
        }
        else if (txtpaymentfrequency.Text == "Quarterly")
        {
            tabletax1.Visible = true;
            tabletax2.Visible = true;
            tabletax3.Visible = true;
            tabletax4.Visible = true;
        }
        //loadgridtax();
    }

    protected void btntaxadd_Click(object sender, EventArgs e)
    {
        try
        {
            int payfrqid = 0;
            if (txtpaymentfrequency.Text.Trim() == "Annual")
            {
                payfrqid = 1;
            }
            else if (txtpaymentfrequency.Text.Trim() == "Semiannual")
            {
                payfrqid = 2;
            }
            else
            {
                payfrqid = 3;
            }
            int fparpaid = 0, fpaid = 0, fdue = 0, fdelinquent = 0;
            int sparpaid = 0, spaid = 0, sdue = 0, sdelinquent = 0;
            int thparpaid = 0, thpaid = 0, thdue = 0, thdelinquent = 0;
            int fhparpaid = 0, fhpaid = 0, fhdue = 0, fhdelinquent = 0;
            //if (txtfpaystatus.Value == "Partially Paid") { fparpaid = 1; }
            //else if (txtfpaystatus.Value == "Paid") { fpaid = 1; }
            //else if (txtfpaystatus.Value == "Due") { fdue = 1; }
            //else if (txtfpaystatus.Value == "Delinquent") { fdelinquent = 1; }

            //if (txtspaystatus.Value == "Partially Paid") { sparpaid = 1; }
            //else if (txtspaystatus.Value == "Paid") { spaid = 1; }
            //else if (txtspaystatus.Value == "Due") { sdue = 1; }
            //else if (txtspaystatus.Value == "Delinquent") { sdelinquent = 1; }

            //if (txtthpaystatus.Value == "Partially Paid") { thparpaid = 1; }
            //else if (txtthpaystatus.Value == "Paid") { thpaid = 1; }
            //else if (txtthpaystatus.Value == "Due") { thdue = 1; }
            //else if (txtthpaystatus.Value == "Delinquent") { thdelinquent = 1; }

            //if (txtfhpaystatus.Value == "Partially Paid") { fhparpaid = 1; }
            //else if (txtfhpaystatus.Value == "Paid") { fhpaid = 1; }
            //else if (txtfhpaystatus.Value == "Due") { fhdue = 1; }
            //else if (txtfhpaystatus.Value == "Delinquent") { fhdelinquent = 1; }
            //int? estimated = null;
            string taxtype = null, taxyear = null, taxentity = null, taxephone = null, taxestreet1 = null;
            string taxestreet2 = null, taxecity = null, taxestate = null, taxeszip = null, paymentfrequency = null;
            string taxidnumber = null, taxidnumberfurdesc = null, taxstateidnumber = null, taxnotes = null;
            string fduedate = null, fdelinqdate = null, ftaxoutdate = null, fdiscdate = null, fgooddate = null;
            string sduedate = null, sdelinqdate = null, staxoutdate = null, sdiscdate = null, sgooddate = null;
            string thduedate = null, thdelinqdate = null, thtaxoutdate = null, thdiscdate = null, thgooddate = null;
            string fhduedate = null, fhdelinqdate = null, fhtaxoutdate = null, fhdiscdate = null, fhgooddate = null;

            //if (txttaxtype.Text.Trim() != string.Empty)
            //{ taxtype = txttaxtype.Text.Trim(); }
            if (txttaxyear.Value.Trim() != string.Empty)
            { taxyear = txttaxyear.Value.Trim(); }
            //if (txttaxentity.Value.Trim() != string.Empty)
            //{ taxentity = txttaxentity.Value.Trim(); }
            //if (txttaxephone.Value.Trim() != string.Empty)
            //{ taxephone = txttaxephone.Value.Trim(); }
            //if (txttaxestreet1.Value.Trim() != string.Empty)
            //{ taxestreet1 = txttaxestreet1.Value.Trim(); }
            //if (txttaxestreet2.Value.Trim() != string.Empty)
            //{ taxestreet2 = txttaxestreet2.Value.Trim(); }
            //if (txttaxecity.Value.Trim() != string.Empty)
            //{ taxecity = txttaxecity.Value.Trim(); }
            //if (txttaxestate.Value.Trim() != string.Empty)
            //{ taxestate = txttaxestate.Value.Trim(); }
            //if (txttaxeszip.Value.Trim() != string.Empty)
            //{ taxeszip = txttaxeszip.Value.Trim(); }
            //if (txtpaymentfrequency.Text.Trim() != string.Empty)
            //{ paymentfrequency = txtpaymentfrequency.Text.Trim(); }
            //if (txttaxidnumber.Value.Trim() != string.Empty)
            //{ taxidnumber = txttaxidnumber.Value.Trim(); }
            //if (txttaxidnumberfurdesc.Value.Trim() != string.Empty)
            //{ taxidnumberfurdesc = txttaxidnumberfurdesc.Value.Trim(); }
            //if (txttaxstateidnumber.Value.Trim() != string.Empty)
            //{ taxstateidnumber = txttaxstateidnumber.Value.Trim(); }
            //if (txttaxnotes.Value.Trim() != string.Empty)
            //{ taxnotes = txttaxnotes.Value.Trim(); }

            //if (txtfduedate.Value.Trim() != string.Empty)
            //{ fduedate = txtfduedate.Value.Trim(); }
            //if (txtfdelinqdate.Value.Trim() != string.Empty)
            //{ fdelinqdate = txtfdelinqdate.Value.Trim(); }
            //if (txtftaxoutdate.Value.Trim() != string.Empty)
            //{ ftaxoutdate = txtftaxoutdate.Value.Trim(); }
            //if (txtfdiscdate.Value.Trim() != string.Empty)
            //{ fdiscdate = txtfdiscdate.Value.Trim(); }
            //if (txtfgooddate.Value.Trim() != string.Empty)
            //{ fgooddate = txtfgooddate.Value.Trim(); }

            //if (txtsduedate.Value.Trim() != string.Empty)
            //{ sduedate = txtsduedate.Value.Trim(); }
            //if (txtsdelinqdate.Value.Trim() != string.Empty)
            //{ sdelinqdate = txtsdelinqdate.Value.Trim(); }
            //if (txtstaxoutdate.Value.Trim() != string.Empty)
            //{ staxoutdate = txtstaxoutdate.Value.Trim(); }
            //if (txtsdiscdate.Value.Trim() != string.Empty)
            //{ sdiscdate = txtsdiscdate.Value.Trim(); }
            //if (txtsgooddate.Value.Trim() != string.Empty)
            //{ sgooddate = txtsgooddate.Value.Trim(); }

            //if (txtthduedate.Value.Trim() != string.Empty)
            //{ thduedate = txtthduedate.Value.Trim(); }
            //if (txtthdelinqdate.Value.Trim() != string.Empty)
            //{ thdelinqdate = txtthdelinqdate.Value.Trim(); }
            //if (txtthtaxoutdate.Value.Trim() != string.Empty)
            //{ thtaxoutdate = txtthtaxoutdate.Value.Trim(); }
            //if (txtthdiscdate.Value.Trim() != string.Empty)
            //{ thdiscdate = txtthdiscdate.Value.Trim(); }
            //if (txtthgooddate.Value.Trim() != string.Empty)
            //{ thgooddate = txtthgooddate.Value.Trim(); }

            //if (txtfhduedate.Value.Trim() != string.Empty)
            //{ fhduedate = txtfhduedate.Value.Trim(); }
            //if (txtfhdelinqdate.Value.Trim() != string.Empty)
            //{ fhdelinqdate = txtfhdelinqdate.Value.Trim(); }
            //if (txtfhtaxoutdate.Value.Trim() != string.Empty)
            //{ fhtaxoutdate = txtfhtaxoutdate.Value.Trim(); }
            //if (txtfhdiscdate.Value.Trim() != string.Empty)
            //{ fhdiscdate = txtfhdiscdate.Value.Trim(); }
            //if (txtfhgooddate.Value.Trim() != string.Empty)
            //{ fhgooddate = txtfhgooddate.Value.Trim(); }

            //int result = gl.InsertTaxData(lblOrderNo.Text.Trim(), taxtype, taxyear, taxentity, taxephone, taxestreet1, taxestreet2, taxecity, taxestate, taxeszip,
            //  TryConvertToDecimal(txttaxtotanntax.Value.Trim()), payfrqid, paymentfrequency, taxidnumber, taxidnumberfurdesc, taxstateidnumber, TryConvertToDecimal(txttaxLand.Value.Trim()), TryConvertToDecimal(txttaximprovement.Value), TryConvertToDecimal(txttaxexmortgage.Value.Trim()), TryConvertToDecimal(txttaxexhome.Value.Trim()), TryConvertToDecimal(txttaxexhomesup.Value.Trim()), TryConvertToDecimal(txttaxexhomeadd.Value.Trim()),
            //   taxnotes, estimated, TryConvertToDecimal(txtftaxamount.Value.Trim()), fduedate, fdelinqdate, ftaxoutdate, fdiscdate, fgooddate, fparpaid, TryConvertToDecimal(txtfparpaidamount.Value.Trim()), fpaid, fdue, fdelinquent, TryConvertToDecimal(txtstaxamount.Value.Trim()),
            //   sduedate, sdelinqdate, staxoutdate, sdiscdate, sgooddate, sparpaid, TryConvertToDecimal(txtsparpaidamount.Value.Trim()), spaid, sdue, sdelinquent, TryConvertToDecimal(txtthtaxamount.Value.Trim()), thduedate,
            //   thdelinqdate, thtaxoutdate, thdiscdate, thgooddate, thparpaid, TryConvertToDecimal(txtthparpaidamount.Value.Trim()), thpaid, thdue, thdelinquent, TryConvertToDecimal(txtfhtaxamount.Value.Trim()), fhduedate, fhdelinqdate, fhtaxoutdate,
            //   fhdiscdate, fhgooddate, fhparpaid, TryConvertToDecimal(txtfhparpaidamount.Value.Trim()), fhpaid, fhdue, fhdelinquent, "Insert", lblProcess.Text, "", SessionHandler.OrderId, txttaxAssYear.Value.Trim(), txttaxproptype.Value.Trim(), txttaxtornsabs.Value.Trim(), txttaxscndinsYear.Value.Trim(), txttaxthrdinsYear.Value.Trim(), txttaxfrthinsYear.Value.Trim(), txttaxprdelcmnts.Value.Trim());
            loadgridtax();
            cleartaxfields();
        }
        catch (Exception ex)
        { throw ex; }
    }
    protected void Gvtax_PreRender(object sender, EventArgs e)
    {
        if (Gvtax.Rows.Count > 0)
        {
            Gvtax.UseAccessibleHeader = true;
            Gvtax.HeaderRow.TableSection = TableRowSection.TableHeader;
            Gvtax.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }

    protected void Gvtax_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void Gvtax_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Gvtax.EditIndex = -1;
            loadgridtax();
        }
        catch (Exception ex)
        { throw ex; }


    }

    protected void Gvtax_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Gvtax.EditIndex = e.NewEditIndex;
        }
        catch (Exception ex)
        { throw ex; }


        //loadgridtax();
    }

    protected void Gvtax_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string GVCommand = e.CommandName.ToLower();

            if (GVCommand == "edit")
            {
                string Item_ID = (e.CommandArgument).ToString();
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rid = row.RowIndex;
                string query = "";
                if (lblProcess.Text == "keying" || lblProcess.Text == "keyingdu")
                {
                    query = "select * from tbl_search_tax_key where ID='" + Item_ID + "'";
                }
                else
                { query = "select * from tbl_search_tax_qc where ID='" + Item_ID + "'"; }

                DataSet ds = new DataSet();
                ds = gl.ExecuteQuery(query);

              
                string fparpaid = ds.Tables[0].Rows[0]["FirstPartiallyPaid"].ToString(), fpaid = ds.Tables[0].Rows[0]["FirstPaid"].ToString(), fdue = ds.Tables[0].Rows[0]["FirstDue"].ToString(), fdelinquent = ds.Tables[0].Rows[0]["FirstDelinquent"].ToString();
                string sparpaid = ds.Tables[0].Rows[0]["SecondPartiallyPaid"].ToString(), spaid = ds.Tables[0].Rows[0]["SecondPaid"].ToString(), sdue = ds.Tables[0].Rows[0]["SecondDue"].ToString(), sdelinquent = ds.Tables[0].Rows[0]["SecondDelinquent"].ToString();
                string thparpaid = ds.Tables[0].Rows[0]["ThirdPartiallyPaid"].ToString(), thpaid = ds.Tables[0].Rows[0]["ThirdPaid"].ToString(), thdue = ds.Tables[0].Rows[0]["ThirdDue"].ToString(), thdelinquent = ds.Tables[0].Rows[0]["ThirdDelinquent"].ToString();
                string fhparpaid = ds.Tables[0].Rows[0]["FourthPartiallyPaid"].ToString(), fhpaid = ds.Tables[0].Rows[0]["FourthPaid"].ToString(), fhdue = ds.Tables[0].Rows[0]["FourthDue"].ToString(), fhdelinquent = ds.Tables[0].Rows[0]["FourthDelinquent"].ToString();

                //if (fparpaid == "1") { txtfpaystatus.Value = "Partially Paid"; }
                //else if (fpaid == "1") { txtfpaystatus.Value = "Paid"; }
                //else if (fdue == "1") { txtfpaystatus.Value = "Due"; }
                //else if (fdelinquent == "1") { txtfpaystatus.Value = "Delinquent"; }

                //if (sparpaid == "1") { txtspaystatus.Value = "Partially Paid"; }
                //else if (spaid == "1") { txtspaystatus.Value = "Paid"; }
                //else if (sdue == "1") { txtspaystatus.Value = "Due"; }
                //else if (sdelinquent == "1") { txtspaystatus.Value = "Delinquent"; }

                //if (thparpaid == "1") { txtthpaystatus.Value = "Partially Paid"; }
                //else if (thpaid == "1") { txtthpaystatus.Value = "Paid"; }
                //else if (thdue == "1") { txtthpaystatus.Value = "Due"; }
                //else if (thdelinquent == "1") { txtthpaystatus.Value = "Delinquent"; }

                //if (fhparpaid == "1") { txtfhpaystatus.Value = "Partially Paid"; }
                //else if (fhpaid == "1") { txtfhpaystatus.Value = "Paid"; }
                //else if (fhdue == "1") { txtfhpaystatus.Value = "Due"; }
                //else if (fhdelinquent == "1") { txtfhpaystatus.Value = "Delinquent"; }

                //  int? estimated = null;
                ////txttaxtype.Text = ds.Tables[0].Rows[0]["TaxTypeName"].ToString().Replace("&nbsp;", "");
                txttaxyear.Value = ds.Tables[0].Rows[0]["Year"].ToString().Replace("&nbsp;", "");
                // txttaxentity.Value = ds.Tables[0].Rows[0]["TaxingEntity"].ToString().Replace("&nbsp;", "");
                //txttaxephone.Value = ds.Tables[0].Rows[0]["TaxingEntityPhone"].ToString().Replace("&nbsp;", "");
                //txttaxestreet1.Value = ds.Tables[0].Rows[0]["TaxingEntityStreet1"].ToString().Replace("&nbsp;", "");
                //txttaxestreet2.Value = ds.Tables[0].Rows[0]["TaxingEntityStreet2"].ToString().Replace("&nbsp;", "");
                //txttaxecity.Value = ds.Tables[0].Rows[0]["TaxingEntityCity"].ToString().Replace("&nbsp;", "");
                //txttaxestate.Value = ds.Tables[0].Rows[0]["TaxingEntityState"].ToString().Replace("&nbsp;", "");
                //txttaxeszip.Value = ds.Tables[0].Rows[0]["TaxingEntityZipCode"].ToString().Replace("&nbsp;", "");
                //txttaxtotanntax.Value = ds.Tables[0].Rows[0]["TotalAnnualTax"].ToString().Replace("&nbsp;", "");

                txtpaymentfrequency.Text = ds.Tables[0].Rows[0]["PaymentFrequencyTypeName"].ToString().Replace("&nbsp;", "");
                //txttaxidnumber.Value = ds.Tables[0].Rows[0]["TaxIDNumber"].ToString().Replace("&nbsp;", "");
                //txttaxidnumberftxtfpaystatusurdesc.Value = ds.Tables[0].Rows[0]["TaxIDNumberFurtherDescribed"].ToString().Replace("&nbsp;", "");
                //txttaxstateidnumber.Value = ds.Tables[0].Rows[0]["StateIDNumber"].ToString().Replace("&nbsp;", "");
                //txttaxLand.Value = ds.Tables[0].Rows[0]["Land"].ToString().Replace("&nbsp;", "");
                //txttaximprovement.Value = ds.Tables[0].Rows[0]["Improvements"].ToString().Replace("&nbsp;", "");
                //txttaxexmortgage.Value = ds.Tables[0].Rows[0]["ExemptionMortgage"].ToString().Replace("&nbsp;", "");
                //txttaxexhome.Value = ds.Tables[0].Rows[0]["ExemptionHomeowners"].ToString().Replace("&nbsp;", "");
                //txttaxexhomesup.Value = ds.Tables[0].Rows[0]["ExemptionHomesteadSupplemental"].ToString().Replace("&nbsp;", "");
                //txttaxexhomeadd.Value = ds.Tables[0].Rows[0]["ExemptionAdditional"].ToString().Replace("&nbsp;", "");
                //txttaxnotes.Value = ds.Tables[0].Rows[0]["Notes"].ToString().Replace("&nbsp;", "");

                //  txtfpaystatus.Value = ds.Tables[0].Rows[0]["TaxTypeName"].ToString();

                //txtftaxamount.Value = ds.Tables[0].Rows[0]["FirstInstallment"].ToString().Replace("&nbsp;", "");

                //txtfduedate.Value = ds.Tables[0].Rows[0]["FirstDueDate"].ToString().Replace("&nbsp;", "");
                //txtfdelinqdate.Value = ds.Tables[0].Rows[0]["FirstDelinquentDate"].ToString().Replace("&nbsp;", "");
                //txtftaxoutdate.Value = ds.Tables[0].Rows[0]["FirstTaxesOutDate"].ToString().Replace("&nbsp;", "");
                //txtfdiscdate.Value = ds.Tables[0].Rows[0]["FirstDiscountDate"].ToString().Replace("&nbsp;", "");
                //txtfgooddate.Value = ds.Tables[0].Rows[0]["FirstGoodthroughDate"].ToString().Replace("&nbsp;", "");


                //txtfparpaidamount.Value = ds.Tables[0].Rows[0]["FirstPartiallyPaidAmount"].ToString();

                //txtstaxamount.Value = ds.Tables[0].Rows[0]["SecondInstallment"].ToString();

                //txtsduedate.Value = ds.Tables[0].Rows[0]["SecondDueDate"].ToString();
                //txtsdelinqdate.Value = ds.Tables[0].Rows[0]["SecondDelinquentDate"].ToString();
                //txtstaxoutdate.Value = ds.Tables[0].Rows[0]["SecondTaxesOutDate"].ToString();
                //txtsdiscdate.Value = ds.Tables[0].Rows[0]["SecondDiscountDate"].ToString();
                //txtsgooddate.Value = ds.Tables[0].Rows[0]["SecondGoodthroughDate"].ToString();

                //txtsparpaidamount.Value = ds.Tables[0].Rows[0]["SecondPartiallyPaidAmount"].ToString();

                //txtthtaxamount.Value = ds.Tables[0].Rows[0]["ThirdInstallment"].ToString();
                //txtthduedate.Value = ds.Tables[0].Rows[0]["ThirdDueDate"].ToString();
                //txtthdelinqdate.Value = ds.Tables[0].Rows[0]["ThirdDelinquentDate"].ToString();
                //txtthtaxoutdate.Value = ds.Tables[0].Rows[0]["ThirdTaxesOutDate"].ToString();
                //txtthdiscdate.Value = ds.Tables[0].Rows[0]["ThirdDiscountDate"].ToString();
                //txtthgooddate.Value = ds.Tables[0].Rows[0]["ThirdGoodthroughDate"].ToString();

                //txtthparpaidamount.Value = ds.Tables[0].Rows[0]["ThirdPartiallyPaidAmount"].ToString();

                //txtfhtaxamount.Value = ds.Tables[0].Rows[0]["FourthInstallment"].ToString();
                //txtfhduedate.Value = ds.Tables[0].Rows[0]["FourthDueDate"].ToString();
                //txtfhdelinqdate.Value = ds.Tables[0].Rows[0]["FourthDelinquentDate"].ToString();
                //txtfhtaxoutdate.Value = ds.Tables[0].Rows[0]["FourthTaxesOutDate"].ToString();
                //txtfhdiscdate.Value = ds.Tables[0].Rows[0]["FourthDiscountDate"].ToString();
                //txtfhgooddate.Value = ds.Tables[0].Rows[0]["FourthGoodthroughDate"].ToString();

                //txtfhparpaidamount.Value = ds.Tables[0].Rows[0]["FourthPartiallyPaidAmount"].ToString();
                //txtpaymentfrequency_SelectedIndexChanged(sender, e);

                //txttaxAssYear.Value = ds.Tables[0].Rows[0]["assyear"].ToString();
                //txttaxproptype.Value = ds.Tables[0].Rows[0]["proptype"].ToString();
                //txttaxtornsabs.Value = ds.Tables[0].Rows[0]["tornsabs"].ToString();
                //txttaxscndinsYear.Value = ds.Tables[0].Rows[0]["scndinsyear"].ToString();
                //txttaxthrdinsYear.Value = ds.Tables[0].Rows[0]["thrdinsyear"].ToString();
                //txttaxfrthinsYear.Value = ds.Tables[0].Rows[0]["frthinsyear"].ToString();
                //txttaxprdelcmnts.Value = ds.Tables[0].Rows[0]["priordelcmnts"].ToString();

                LinkButton lnkedit = (LinkButton)Gvtax.Rows[rid].FindControl("LnkEdit");
                btntaxadd.Enabled = false;
                // lnkedit.Text = "Update";
                lnkedit.CommandName = "Update";
                lnkedit.CssClass = "glyphicon glyphicon-ok";
                lnkedit.ToolTip = "Update";
                lnkedit.OnClientClick = "javascript : return confirm('Are you sure, want to update this Row?');";
                LinkButton lnkcancel = (LinkButton)Gvtax.Rows[rid].FindControl("LnkCancel");
                lnkcancel.Visible = true;
                //// loadgridtax();
            }

            else if (GVCommand == "update")
            {

                string Item_ID = (e.CommandArgument).ToString();
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rid = row.RowIndex;
                LinkButton lnkedit = (LinkButton)Gvtax.Rows[rid].FindControl("LnkEdit");
                //  lnkedit.Text = "Edit";
                lnkedit.CommandName = "Edit";
                lnkedit.CssClass = "glyphicon glyphicon-edit";
                lnkedit.ToolTip = "Edit";
                btntaxadd.Enabled = true;
                int payfrqid = 0;
                if (txtpaymentfrequency.Text.Trim() == "Annual")
                {
                    payfrqid = 1;
                }
                else if (txtpaymentfrequency.Text.Trim() == "Semiannual")
                {
                    payfrqid = 2;
                }
                else
                {
                    payfrqid = 3;
                }
                int fparpaid = 0, fpaid = 0, fdue = 0, fdelinquent = 0;
                int sparpaid = 0, spaid = 0, sdue = 0, sdelinquent = 0;
                int thparpaid = 0, thpaid = 0, thdue = 0, thdelinquent = 0;
                int fhparpaid = 0, fhpaid = 0, fhdue = 0, fhdelinquent = 0;
                //if (txtfpaystatus.Value == "Partially Paid") { fparpaid = 1; }
                //else if (txtfpaystatus.Value == "Paid") { fpaid = 1; }
                //else if (txtfpaystatus.Value == "Due") { fdue = 1; }
                //else if (txtfpaystatus.Value == "Delinquent") { fdelinquent = 1; }

                //if (txtspaystatus.Value == "Partially Paid") { sparpaid = 1; }
                //else if (txtspaystatus.Value == "Paid") { spaid = 1; }
                //else if (txtspaystatus.Value == "Due") { sdue = 1; }
                //else if (txtspaystatus.Value == "Delinquent") { sdelinquent = 1; }

                //if (txtthpaystatus.Value == "Partially Paid") { thparpaid = 1; }
                //else if (txtthpaystatus.Value == "Paid") { thpaid = 1; }
                //else if (txtthpaystatus.Value == "Due") { thdue = 1; }
                //else if (txtthpaystatus.Value == "Delinquent") { thdelinquent = 1; }

                //if (txtfhpaystatus.Value == "Partially Paid") { fhparpaid = 1; }
                //else if (txtfhpaystatus.Value == "Paid") { fhpaid = 1; }
                //else if (txtfhpaystatus.Value == "Due") { fhdue = 1; }
                //else if (txtfhpaystatus.Value == "Delinquent") { fhdelinquent = 1; }
                int? estimated = null;
                string taxtype = null, taxyear = null, taxentity = null, taxephone = null, taxestreet1 = null;
                string taxestreet2 = null, taxecity = null, taxestate = null, taxeszip = null, paymentfrequency = null;
                string taxidnumber = null, taxidnumberfurdesc = null, taxstateidnumber = null, taxnotes = null;

                string fduedate = null, fdelinqdate = null, ftaxoutdate = null, fdiscdate = null, fgooddate = null;
                string sduedate = null, sdelinqdate = null, staxoutdate = null, sdiscdate = null, sgooddate = null;
                string thduedate = null, thdelinqdate = null, thtaxoutdate = null, thdiscdate = null, thgooddate = null;
                string fhduedate = null, fhdelinqdate = null, fhtaxoutdate = null, fhdiscdate = null, fhgooddate = null;

                //if (txttaxtype.Text.Trim() != string.Empty)
                //{ taxtype = txttaxtype.Text.Trim(); }
                if (txttaxyear.Value.Trim() != string.Empty)
                { taxyear = txttaxyear.Value.Trim(); }
                //if (txttaxentity.Value.Trim() != string.Empty)
                //{ taxentity = txttaxentity.Value.Trim(); }
                //if (txttaxephone.Value.Trim() != string.Empty)
                //{ taxephone = txttaxephone.Value.Trim(); }
                //if (txttaxestreet1.Value.Trim() != string.Empty)
                //{ taxestreet1 = txttaxestreet1.Value.Trim(); }
                //if (txttaxestreet2.Value.Trim() != string.Empty)
                //{ taxestreet2 = txttaxestreet2.Value.Trim(); }
                //if (txttaxecity.Value.Trim() != string.Empty)
                //{ taxecity = txttaxecity.Value.Trim(); }
                //if (txttaxestate.Value.Trim() != string.Empty)
                //{ taxestate = txttaxestate.Value.Trim(); }
                //if (txttaxeszip.Value.Trim() != string.Empty)
                //{ taxeszip = txttaxeszip.Value.Trim(); }
                if (txtpaymentfrequency.Text.Trim() != string.Empty)
                { paymentfrequency = txtpaymentfrequency.Text.Trim(); }
                //if (txttaxidnumber.Value.Trim() != string.Empty)
                //{ taxidnumber = txttaxidnumber.Value.Trim(); }
                //if (txttaxidnumberfurdesc.Value.Trim() != string.Empty)
                //{ taxidnumberfurdesc = txttaxidnumberfurdesc.Value.Trim(); }
                //if (txttaxstateidnumber.Value.Trim() != string.Empty)
                //{ taxstateidnumber = txttaxstateidnumber.Value.Trim(); }
                //if (txttaxnotes.Value.Trim() != string.Empty)
                //{ taxnotes = txttaxnotes.Value.Trim(); }

                //if (txtfduedate.Value.Trim() != string.Empty)
                //{ fduedate = txtfduedate.Value.Trim(); }
                //if (txtfdelinqdate.Value.Trim() != string.Empty)
                //{ fdelinqdate = txtfdelinqdate.Value.Trim(); }
                //if (txtftaxoutdate.Value.Trim() != string.Empty)
                //{ ftaxoutdate = txtftaxoutdate.Value.Trim(); }
                //if (txtfdiscdate.Value.Trim() != string.Empty)
                //{ fdiscdate = txtfdiscdate.Value.Trim(); }
                //if (txtfgooddate.Value.Trim() != string.Empty)
                //{ fgooddate = txtfgooddate.Value.Trim(); }

                //if (txtsduedate.Value.Trim() != string.Empty)
                //{ sduedate = txtsduedate.Value.Trim(); }
                //if (txtsdelinqdate.Value.Trim() != string.Empty)
                //{ sdelinqdate = txtsdelinqdate.Value.Trim(); }
                //if (txtstaxoutdate.Value.Trim() != string.Empty)
                //{ staxoutdate = txtstaxoutdate.Value.Trim(); }
                //if (txtsdiscdate.Value.Trim() != string.Empty)
                //{ sdiscdate = txtsdiscdate.Value.Trim(); }
                //if (txtsgooddate.Value.Trim() != string.Empty)
                //{ sgooddate = txtsgooddate.Value.Trim(); }

                //if (txtthduedate.Value.Trim() != string.Empty)
                //{ thduedate = txtthduedate.Value.Trim(); }
                //if (txtthdelinqdate.Value.Trim() != string.Empty)
                //{ thdelinqdate = txtthdelinqdate.Value.Trim(); }
                //if (txtthtaxoutdate.Value.Trim() != string.Empty)
                //{ thtaxoutdate = txtthtaxoutdate.Value.Trim(); }
                //if (txtthdiscdate.Value.Trim() != string.Empty)
                //{ thdiscdate = txtthdiscdate.Value.Trim(); }
                //if (txtthgooddate.Value.Trim() != string.Empty)
                //{ thgooddate = txtthgooddate.Value.Trim(); }

                //if (txtfhduedate.Value.Trim() != string.Empty)
                //{ fhduedate = txtfhduedate.Value.Trim(); }
                //if (txtfhdelinqdate.Value.Trim() != string.Empty)
                //{ fhdelinqdate = txtfhdelinqdate.Value.Trim(); }
                //if (txtfhtaxoutdate.Value.Trim() != string.Empty)
                //{ fhtaxoutdate = txtfhtaxoutdate.Value.Trim(); }
                //if (txtfhdiscdate.Value.Trim() != string.Empty)
                //{ fhdiscdate = txtfhdiscdate.Value.Trim(); }
                //if (txtfhgooddate.Value.Trim() != string.Empty)
                //{ fhgooddate = txtfhgooddate.Value.Trim(); }

                //int result = gl.InsertTaxData(lblOrderNo.Text.Trim(), taxtype, taxyear, taxentity, taxephone, taxestreet1, taxestreet2, taxecity, taxestate, taxeszip,
                //  TryConvertToDecimal(txttaxtotanntax.Value.Trim()), payfrqid, paymentfrequency, taxidnumber, taxidnumberfurdesc, taxstateidnumber, TryConvertToDecimal(txttaxLand.Value.Trim()), TryConvertToDecimal(txttaximprovement.Value), TryConvertToDecimal(txttaxexmortgage.Value.Trim()), TryConvertToDecimal(txttaxexhome.Value.Trim()), TryConvertToDecimal(txttaxexhomesup.Value.Trim()), TryConvertToDecimal(txttaxexhomeadd.Value.Trim()),
                //   taxnotes, estimated, TryConvertToDecimal(txtftaxamount.Value.Trim()), txtfduedate.Value.Trim(), txtfdelinqdate.Value.Trim(), txtftaxoutdate.Value.Trim(), txtfdiscdate.Value.Trim(), txtfgooddate.Value.Trim(), fparpaid, TryConvertToDecimal(txtfparpaidamount.Value.Trim()), fpaid, fdue, fdelinquent, TryConvertToDecimal(txtstaxamount.Value.Trim()),
                //   txtsduedate.Value.Trim(), txtsdelinqdate.Value.Trim(), txtstaxoutdate.Value.Trim(), txtsdiscdate.Value.Trim(), txtsgooddate.Value.Trim(), sparpaid, TryConvertToDecimal(txtsparpaidamount.Value.Trim()), spaid, sdue, sdelinquent, TryConvertToDecimal(txtthtaxamount.Value.Trim()), txtthduedate.Value.Trim(),
                //   txtthdelinqdate.Value.Trim(), txtthtaxoutdate.Value.Trim(), txtthdiscdate.Value.Trim(), txtthgooddate.Value.Trim(), thparpaid, TryConvertToDecimal(txtthparpaidamount.Value.Trim()), thpaid, thdue, thdelinquent, TryConvertToDecimal(txtfhtaxamount.Value.Trim()), txtfhduedate.Value.Trim(), txtfhdelinqdate.Value.Trim(), txtfhtaxoutdate.Value.Trim(),
                //   txtfhdiscdate.Value.Trim(), txtfhgooddate.Value.Trim(), fhparpaid, TryConvertToDecimal(txtfhparpaidamount.Value.Trim()), fhpaid, fhdue, fhdelinquent, "Update", lblProcess.Text, Item_ID, SessionHandler.OrderId);
                loadgridtax();
                cleartaxfields();
                LinkButton lnkcancel = (LinkButton)Gvtax.Rows[rid].FindControl("LnkCancel");
                lnkcancel.Visible = false;
            }
            else if (GVCommand == "delete")
            {
                string Item_ID = (e.CommandArgument).ToString();
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rid = row.RowIndex;        
                loadgridtax();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("ShowMessageSucess('");
                sb.Append("')};");
                sb.Append("</script>");
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", sb.ToString());
            }
            else if (GVCommand == "Cancel")
            {
                btntaxadd.Enabled = true;
            }

            int index = 0;
            GridViewRow gvrow;
            GridViewRow previousRow;
            if (e.CommandName == "Up")
            {
                index = Convert.ToInt32(e.CommandArgument);
                gvrow = Gvtax.Rows[index];
                previousRow = Gvtax.Rows[index - 1];
                int taxPriority = Convert.ToInt32(Gvtax.DataKeys[gvrow.RowIndex].Values[1].ToString());
                int taxId = Convert.ToInt32(Gvtax.DataKeys[gvrow.RowIndex].Values[0].ToString());
                int previousId = Convert.ToInt32(Gvtax.DataKeys[previousRow.RowIndex].Values[0].ToString());
                string query = "";
                if (lblProcess.Text.ToLower() == "keying" || lblProcess.Text.ToLower() == "keyingdu")
                { query = "update tbl_search_tax_key set Priority='" + (taxPriority - 1) + "' where id='" + taxId + "'; update tbl_search_tax_key set Priority='" + (taxPriority) + "' where id='" + previousId + "'"; }
                else { query = "update tbl_search_tax_qc set Priority='" + (taxPriority - 1) + "' where id='" + taxId + "'; update tbl_search_tax_qc set Priority='" + (taxPriority) + "' where id='" + previousId + "'"; }
                //int result = gl.ExecuteNonQuery(query);
                loadgridtax();
            }
            if (e.CommandName == "Down")
            {
                index = Convert.ToInt32(e.CommandArgument);
                gvrow = Gvtax.Rows[index];
                previousRow = Gvtax.Rows[index + 1];
                int taxPriority = Convert.ToInt32(Gvtax.DataKeys[gvrow.RowIndex].Values[1].ToString());
                int taxId = Convert.ToInt32(Gvtax.DataKeys[gvrow.RowIndex].Values[0].ToString());
                int previousId = Convert.ToInt32(Gvtax.DataKeys[previousRow.RowIndex].Values[0].ToString());
                string query = "";
                if (lblProcess.Text.ToLower() == "keying" || lblProcess.Text.ToLower() == "keyingdu")
                { query = "update tbl_search_tax_key set Priority='" + (taxPriority + 1) + "' where id='" + taxId + "'; update tbl_search_tax_key set Priority='" + (taxPriority) + "' where id='" + previousId + "'"; }
                else { query = "update tbl_search_tax_qc set Priority='" + (taxPriority + 1) + "' where id='" + taxId + "'; update tbl_search_tax_qc set Priority='" + (taxPriority) + "' where id='" + previousId + "'"; }
                //int result = gl.ExecuteNonQuery(query);
                loadgridtax();
            }
        } 
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Gvtax_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        Gvtax.EditIndex = -1;
        btntaxadd.Enabled = true;
        loadgridtax();
        cleartaxfields();
    }
    protected void cleartaxfields()
    {
        
        //txtfpaystatus.Value = "Select";
        //txtspaystatus.Value = "Select";
        //txtthpaystatus.Value = "Select";
        //txtfhpaystatus.Value = "Select";
        txtpaymentfrequency.SelectedIndex = 0;
        tabletax2.Visible = false;
        tabletax3.Visible = false;
        tabletax4.Visible = false;
        //  int? estimated = null;
        //txttaxtype.Text = null;
        txttaxyear.Value = null;
        // txttaxentity.Value = null;
        //txttaxephone.Value = null;
        //txttaxestreet1.Value = null;
        //txttaxestreet2.Value = null;
        //txttaxecity.Value = null;
        //txttaxestate.Value = null;
        //txttaxeszip.Value = null;
        //txttaxtotanntax.Value = null;

        txtpaymentfrequency.Text = null;
        //txttaxidnumber.Value = null;
        //txttaxidnumberfurdesc.Value = null;
        //txttaxstateidnumber.Value = null;
        //txttaxLand.Value = null;
        //txttaximprovement.Value = null;
        //txttaxexmortgage.Value = null;
        //txttaxexhome.Value = null;
        //txttaxexhomesup.Value = null;
        //txttaxexhomeadd.Value = null;
        //txttaxnotes.Value = null;

        //  txtfpaystatus.Value = ds.Tables[0].Rows[0]["TaxTypeName"].ToString();

        //txtftaxamount.Value = null;

        //txtfduedate.Value = null;
        //txtfdelinqdate.Value = null;
        //txtftaxoutdate.Value = null;
        //txtfdiscdate.Value = null;
        //txtfgooddate.Value = null;


        //txtfparpaidamount.Value = null;

        //txtstaxamount.Value = null;

        //txtsduedate.Value = null;
        //txtsdelinqdate.Value = null;
        //txtstaxoutdate.Value = null;
        //txtsdiscdate.Value = null;
        //txtsgooddate.Value = null;

        //txtsparpaidamount.Value = null;

        //txtthtaxamount.Value = null;
        //txtthduedate.Value = null;
        //txtthdelinqdate.Value = null;
        //txtthtaxoutdate.Value = null;
        //txtthdiscdate.Value = null;
        //txtthgooddate.Value = null;

        //txtthparpaidamount.Value = null;

        //txtfhtaxamount.Value = null;
        //txtfhduedate.Value = null;
        //txtfhdelinqdate.Value = null;
        //txtfhtaxoutdate.Value = null;
        //txtfhdiscdate.Value = null;
        //txtfhgooddate.Value = null;

        //txtfhparpaidamount.Value = null;
        ////txtpaymentfrequency_SelectedIndexChanged(sender, e);
        //txttaxAssYear.Value = null;
        //txttaxproptype.Value = null;
        //txttaxtornsabs.Value = null;
        //txttaxscndinsYear.Value = null;
        //txttaxthrdinsYear.Value = null;
        //txttaxfrthinsYear.Value = null;
        //txttaxprdelcmnts.Value = null;
    }
    #endregion

  
}
