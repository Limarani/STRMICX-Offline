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

public partial class Pages_Request : System.Web.UI.Page
{
    GlobalClass gl = new GlobalClass();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        Lblusername.Text = "Welcome " + SessionHandler.UserName;
        if (!Page.IsPostBack)
        {
            GetDatas();
        }
    }
    private void GetDatas()
    {
        string Orderno = "", d1 = "";
        Orderno = Request.QueryString["id"];
        d1 = Request.QueryString["date"];
        LoadRequest(Orderno, d1);
    }
    private void LoadRequest(string orderid,string odate)
    {
       try
        {
            DataTable dt = new DataTable();
            dt = gl.FetchRequest(orderid,odate);
            if (myVariables.IsErr == true) { gl.RedirectErrorPage(); }
            if (dt.Rows.Count > 0)
            {
                GridRequest.DataSource = dt;
                GridRequest.DataBind();
            }
            else
            {
                GridRequest.DataSource = null;
                GridRequest.DataBind();
            }
        }
        catch (Exception ex)
        {
            myConnection.setError(ex.ToString());
            gl.RedirectErrorPage();
        }    
    }
    protected void GridRequest_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Received")
        {
            GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string id = GridRequest.DataKeys[gvr.RowIndex].Values[0].ToString();
            gl.UpdateRequeststatus(id);
            GetDatas(); 
        }
    }
    protected void GridRequest_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton Lnk = e.Row.FindControl("Lnkstatus") as LinkButton;
            if (e.Row.Cells[13].Text == "Received")
            {                
                Lnk.Visible = false;
            }
            else
            {
                Lnk.Visible = true;
            }
        }
    }
}
