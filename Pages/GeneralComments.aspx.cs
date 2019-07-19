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

public partial class Pages_GEneralComments : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    myConnection con = new myConnection();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        if (!Page.IsPostBack)
        {
            LoadComments();
        }
    }

    private void LoadComments()
    {
        ds.Dispose();
        ds.Reset();
        txtstatecomments.Text = "";
        string query = "select Comments from getcomments where State='General' or State='" + myVariables.State + "' Order by id";
        ds = con.ExecuteQuery(query);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            txtstatecomments.Text = txtstatecomments.Text + ds.Tables[0].Rows[i]["Comments"].ToString();
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'> { self.close(); }</script>");
    }
}
