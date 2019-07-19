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

public partial class Pages_MailAwayComments : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    myConnection con = new myConnection();
    GlobalClass gl = new GlobalClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");        
        if (!Page.IsPostBack)
        {            
            LblType.Text = "Regular Mail";
            ddlmailtype.SelectedIndex = 1;
            TxtMailDate.Text = String.Format("{0:MM-dd-yyyy}", DateTime.Now);
            LoadTaxType();
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
            ddltaxtype.DataSource = ds;
            ddltaxtype.DataTextField = "TaxType";
            ddltaxtype.DataBind();
            ddltaxtype.Items.Insert(0, "-----Select Tax Type-----");
        }
    }
    protected void LnkRegularMail_Click(object sender, EventArgs e)
    {
        showMailDate("Regular Mail");
        LblType.Text = "Regular Mail";
        ddlmailtype.SelectedIndex = 1;
    }
    protected void LnkUpsMail_Click(object sender, EventArgs e)
    {
        showMailDate("UPS Mail");
        LblType.Text = "UPS Mail";
        ddlmailtype.SelectedIndex = 2;
    }
    protected void LnkReturnUps_Click(object sender, EventArgs e)
    {
        showMailDate("Return UPS");
        LblType.Text = "Return UPS";
        ddlmailtype.SelectedIndex = 3;
    }
    protected void BtGetComments_Click(object sender, EventArgs e)
    {
        string taxtype = "", fee = "", mailtype = "", followupdate = "", eta = "", finaltext = "";
        if (!Validation()) { return; }
        DateTime mdate = Convert.ToDateTime(TxtMailDate.Text);
        string maildate = String.Format("{0:dd-MMM-yyyy}", mdate);
        taxtype = ddltaxtype.SelectedItem.Text;
        fee = "$" + Convert.ToDouble(TxtFee.Text);
        mailtype = ddlmailtype.SelectedItem.Text;
        if (LblType.Text == "Regular Mail")
        {
            followupdate = String.Format("{0:dd-MMM-yyyy}", TxtFollowUpDate.Text);
            eta = String.Format("{0:dd-MMM-yyyy}", TxtETA.Text);
        }
        else
        {
            finaltext = "Tracking number to be updated.";
            if (LblType.Text == "UPS Mail")
            {
                followupdate = String.Format("{0:dd-MMM-yyyy}", TxtFollowUpDate.Text);
                eta = String.Format("{0:dd-MMM-yyyy}", TxtETA.Text);
            }
            else if (LblType.Text == "Return UPS")
            {
                followupdate = String.Format("{0:dd-MMM-yyyy}", TxtFollowUpDate.Text);
                eta = String.Format("{0:dd-MMM-yyyy}", TxtETA.Text);
            }
            else
            {

            }
        }
        TxtComments.Text = taxtype + "-" + "Mail request sent to tax office with search fee of " + fee + " via " + mailtype + " on " + maildate + " ETA :" + eta + " Follow Up Date :" + followupdate + "  " + finaltext;
        DateTime dttime = Convert.ToDateTime(TxtFollowUpDate.Text);
        string followup = String.Format("{0:MM/dd/yyyy}", dttime);
        string query = "Update record_status set followup='" + followup + "' where Order_No='" + myVariables.Orderno + "'";
        int result = con.ExecuteSPNonQuery(query);
    }
    protected void BtClear_Click(object sender, EventArgs e)
    {
        LblType.Text = "Regular Mail";
        ddlmailtype.SelectedIndex = 1;
        TxtMailDate.Text = String.Format("{0:MM-dd-yyyy}", DateTime.Now);
        TxtFee.Text = "";
        ddltaxtype.SelectedIndex = 0;
        TxtComments.Text = "";
    }
    private bool Validation()
    {
        bool valid = true;
        if (ddltaxtype.SelectedItem.Text  == string.Empty) { Lblerror.Text = "Taxtype is blank."; valid = false; return valid; }
        if (TxtFee.Text == string.Empty) { Lblerror.Text = "Fee is blank."; valid = false; return valid; }
        if (ddlmailtype.SelectedItem.Text == string.Empty) { Lblerror.Text = "Mailtype is blank."; valid = false; return valid; }
        if (TxtMailDate.Text == string.Empty) { Lblerror.Text = "Maildate is blank."; valid = false; return valid; }
        if (TxtFollowUpDate.Text == string.Empty) { Lblerror.Text = "Followup is blank."; valid = false; return valid; }
        if (TxtETA.Text == string.Empty) { Lblerror.Text = "ETA is blank."; valid = false; return valid; }
        return valid;
    }
    protected void btnsavedate_Click(object sender, EventArgs e)
    {
        if (TxtMailDate.Text != " " || TxtFollowUpDate.Text != " " || TxtETA.Text != "")
        {
            string query = ("insert into `mailawaydate`(`mailType`,`mailDate`,`followupDate`,`ETA`)values('" + LblType.Text + "','" + TxtMailDate.Text + "','" + TxtFollowUpDate.Text + "','" + TxtETA.Text + "')");
            con.ExecuteSPNonQuery(query);
        }
    }
    private void showMailDate(string mailType)
    {
        ds.Dispose();
        ds.Reset();
        string query = ("select `mailDate`,`followupDate`,`ETA` from `mailawaydate` where `mailType`='" + mailType + "' order by `sno` desc limit 1");
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtMailDate.Text = ds.Tables[0].Rows[0]["mailDate"].ToString();
            TxtFollowUpDate.Text = ds.Tables[0].Rows[0]["followupDate"].ToString();
            TxtETA.Text = ds.Tables[0].Rows[0]["ETA"].ToString();
        }
        else
        {
            TxtMailDate.Text = "";
            TxtFollowUpDate.Text = "";
            TxtETA.Text = "";
        }
    }
}
