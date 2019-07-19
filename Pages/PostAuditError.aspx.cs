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

public partial class Pages_NonAdminHome : System.Web.UI.Page
{
    GlobalClass gl = new GlobalClass();
    myConnection con = new myConnection();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        if (!Page.IsPostBack)
        {
            LoadGrid();
            Lblinfo.Text = "Post Audit Error";
            pagedimmer1.Visible = false;
            AcceptReason.Visible = false;
        }
    }
    public void LoadGrid()
    {
        string fdate = "", tdate = "";

        DateTime df = gl.ToDate();
        tdate = df.ToString("MM/dd/yyyy");
        //tdate = "08/22/2013";
        fdate = tdate;
        ds.Dispose();
        ds.Reset();
        string strquery = "select id,Order_No,ErrorCategory1,ErrorField1,Incorrect1,Correct1,Error_Comments1,Review_OP_Comments,Audit_AcceptReason from record_status where pdate between '" + fdate + "' and '" + tdate + "' and Error1='Error'";
        ds = con.ExecuteQuery(strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridErrorDetails.DataSource = ds;
            GridErrorDetails.DataBind();
        }
    }

    protected void Lnkbtnaccept_Click(object sender, EventArgs e)
    {
        Control closeLink = (Control)sender;
        GridViewRow row = (GridViewRow)closeLink.NamingContainer;
        Session["OrderNo"] = row.Cells[0].Text;

        pagedimmer1.Visible = true;
        AcceptReason.Visible = true;
        txtreason.Text = "";
        lblerror.Text = "";
    }
    protected void btnok_Click(object sender, EventArgs e)
    {
        if (txtreason.Text == "") { lblerror.Text = "Please enter the Reason"; return; }
        string strorder = Convert.ToString(Session["OrderNo"]);

        string query = "Update record_status set Error_Comments1='Accepted',Audit_AcceptReason='" + txtreason.Text + "' where Order_No='" + strorder + "'";
        int result = con.ExecuteSPNonQuery(query);
        if (result > 0)
        {
            LoadGrid();
            pagedimmer1.Visible = false;
            AcceptReason.Visible = false;
        }
    }
    protected void btnno_Click(object sender, EventArgs e)
    {
        if (txtreason.Text == "") { lblerror.Text = "Please enter the Reason"; return; }

        string strorder = Convert.ToString(Session["OrderNo"]);

        string query = "Update record_status set Error_Comments1='Not Accepted',Audit_AcceptReason='" + txtreason.Text + "' where Order_No='" + strorder + "'";
        int result = con.ExecuteSPNonQuery(query);
        if (result > 0)
        {
            LoadGrid();
            pagedimmer1.Visible = false;
            AcceptReason.Visible = false;
        }
    }
}
