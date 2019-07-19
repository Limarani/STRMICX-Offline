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
        LoadGrid();
    }
    public void LoadGrid()
    {
        string fdate = "", tdate = "";

        DateTime df = gl.ToDate();
        tdate = df.ToString("MM/dd/yyyy");
        fdate = tdate;
        ds.Dispose();
        ds.Reset();
        string strquery = "select id,QC_OP,Review_OP,Order_No,Error,ErrorField,Incorrect,Correct,Error_Comments,QC_OP_Comments,Error1,ErrorField1,Incorrect1,Correct1,Error_Comments1,Review_OP_Comments from record_status where DATE_FORMAT(DATE_SUB(UploadTime,INTERVAL '07:00' HOUR_MINUTE),'%m/%d/%Y') between '" + fdate + "' and '" + tdate + "' and ((K1_OP='" + SessionHandler.UserName + "' and Error='Error') or (QC_OP='" + SessionHandler.UserName + "' and Error1='Error'))";
        ds = con.ExecuteQuery(strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dView = gl.ConvertDStoDataview(ds);
            GridErrorDetails.DataSource = dView;
            GridErrorDetails.DataBind();
        }
    }

    protected void GridErrorDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void GridErrorDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Accept")
        {
            GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int index = gvr.RowIndex;
            string strorder, strprocess = "";
            strorder = GridErrorDetails.DataKeys[index].Values[0].ToString();
            strprocess = GridErrorDetails.Rows[index].Cells[2].Text.ToString();
            string query = "";
            if (strprocess == "Production") query = "Update record_status set Error_Comments='Accepted' where Order_No='" + strorder + "'";
            else if (strprocess == "QC") query = "Update record_status set Error_Comments1='Accepted' where Order_No='" + strorder + "'";
            int result = con.ExecuteSPNonQuery(query);
            if (result > 0) LoadGrid();
        }
    }
}
