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

public partial class Pages_PostAudit : System.Web.UI.Page
{

    GlobalClass gl = new GlobalClass();
    DataSet ds = new DataSet();
    myConnection con = new myConnection();
    DateTime dt = new DateTime();
    MySqlDataReader mdr;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        if (!Page.IsPostBack)
        {
            Lblusername.Text = "Welcome " + SessionHandler.UserName + " ..";
            Loadgrid();
        }
    }

    private void Loadgrid()
    {
        string strquery = "Select order_no as 'Order No' from record_status where K1='2' and QC='2' and Status='2' and Tax ='0' and Parcel='3' and Pend='0'";
        ds.Dispose();
        ds.Reset();
        ds = con.ExecuteQuery(strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridUser.DataSource = ds;
            GridUser.DataBind();
        }
    }
    protected void Btnclose_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'> { self.close(); }</script>");
    }
    protected void GridUser_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkall = (CheckBox)e.Row.FindControl("chkselectall");
            if (chkall != null)
            {
                chkall.Attributes.Add("onclick", "checkUncheckAll(this)");
            }
        }
    }
    protected void btnordershow_Click(object sender, EventArgs e)
    {
        string strtoturl = "", strurl = "";
        foreach (GridViewRow row in GridUser.Rows)
        {
            CheckBox chkbx = (CheckBox)row.FindControl("chkselect");
            if (chkbx.Checked == true)
            {
                string[] strorder = row.Cells[1].Text.Split('_');
                strurl = "window.open('https://portal.titlesource.com/vendor/Tax/OrderDetails.aspx?oid=" + strorder[0].ToString() + "');";
                strtoturl = strtoturl + strurl;
            }
        }
        string javascript = "<script type='text/javascript'>" + strtoturl + "</script>";
        this.RegisterStartupScript("", javascript);
    }
}
