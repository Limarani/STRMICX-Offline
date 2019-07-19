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

public partial class Pages_NonAdmin : System.Web.UI.Page
{
    //test
    #region Variable Declaration
    myConnection con = new myConnection();
    GlobalClass gl = new GlobalClass();
    DataTable Alltbls = new DataTable();
    DataSet ds = new DataSet();
    DataSet dset = new DataSet();
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        LoadGrid();
    }
    #endregion

    #region Fetch Orders for Nonadmin Users    
    private void LoadGrid()
    {        
        string query = "",result="";
        
        query = "select Pending from user_status where User_Id='" + SessionHandler.UserName + "' limit 1;";
        result = con.ExecuteScalar(query);
        if (result == "1")
        {
            DataTable Inprocess = gl.LoadPendingOrders("INPROCESS");
            Alltbls.Merge(Inprocess);
        }

        query = "select Parcelid from user_status where User_Id='" + SessionHandler.UserName + "' limit 1;";
        result = con.ExecuteScalar(query);
        if (result == "1")
        {
            DataTable Parcelid = gl.LoadPendingOrders("PARCELID");
            Alltbls.Merge(Parcelid);
        }

        query = "select Onhold from user_status where User_Id='" + SessionHandler.UserName + "' limit 1;";
        result = con.ExecuteScalar(query);
        if (result == "1")
        {
            DataTable Onhold = gl.LoadPendingOrders("ONHOLD");
            Alltbls.Merge(Onhold);
        }

        query = "select mailaway from user_status where User_Id='" + SessionHandler.UserName + "' limit 1;";
        result = con.ExecuteScalar(query);
        if (result == "1")
        {
            DataTable Inprocess = gl.LoadPendingOrders("MAILAWAY");
            Alltbls.Merge(Inprocess);
        }

         DataTable PaStateYTS = gl.LoadPendingOrders("YTS");
        Alltbls.Merge(PaStateYTS);
        

        if (Alltbls.Rows.Count > 0)
        {
            GridUser.DataSource = Alltbls;
            GridUser.DataBind();
        }
        else
        {
            Alltbls.Rows.Add(Alltbls.NewRow());
            GridUser.DataSource = Alltbls;
            GridUser.DataBind();
            int Totalcolumns = GridUser.Rows[0].Cells.Count;
            GridUser.Rows[0].Cells.Clear();
            GridUser.Rows[0].Cells.Add(new TableCell());
            GridUser.Rows[0].Cells[0].ColumnSpan = Totalcolumns;
            GridUser.Rows[0].Cells[0].Text = "No Records Found";
            GridUser.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
        }       
    }
    #endregion

    #region GridEvents
    protected void GridUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Process")
        {
            GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string id = GridUser.DataKeys[gvr.RowIndex].Values[0].ToString();            
            Session["TimePro"] = DateTime.Now;
            Response.Redirect("Production.aspx?id=" + id);
        }
    }
    protected void GridUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Pager)
        {
            if (e.Row.Cells[9].Text == "On Hold")
            {
                e.Row.Cells[9].BackColor = System.Drawing.Color.SkyBlue;
                e.Row.Cells[9].ForeColor = System.Drawing.Color.Black;
            }
            if (e.Row.Cells[9].Text == "Mail Away")
            {
                e.Row.Cells[9].BackColor = System.Drawing.Color.FromArgb(170, 255, 212);
                e.Row.Cells[9].ForeColor = System.Drawing.Color.Black;
            }
            if (e.Row.Cells[9].Text == "ParcelID")
            {
                e.Row.Cells[9].BackColor = System.Drawing.Color.FromArgb(250, 250, 142);
                e.Row.Cells[9].ForeColor = System.Drawing.Color.Black;
            }
            if (e.Row.Cells[9].Text == "In Process")
            {
                e.Row.Cells[9].BackColor = System.Drawing.Color.FromArgb(250, 200, 211);
                e.Row.Cells[9].ForeColor = System.Drawing.Color.Black;
            }
        }
    }
    #endregion


    #region Search Order
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        string strordersearch = "";
        try
        {
            if (txtordersearch.Text != "")
            {
                ds.Dispose();
                ds.Reset();
                dset.Dispose();
                dset.Reset();
                strordersearch = txtordersearch.Text;
                ds = gl.TrackingSearch("%" + strordersearch + "%");
                DataTable dttt = ds.Tables[0].Copy();
                DataTable dtt = ds.Tables[1].Copy();
                DataTable dt3 = ds.Tables[2].Copy();
                dtt.Merge(dttt);
                dtt.Merge(dt3);
                dset.Tables.Add(dtt);
                ShowSearchGrid(dset);
            }
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }

    public void ShowSearchGrid(DataSet ds)
    {
        try
        {
            DataView dataview = gl.ConvertDataSetToDataView(ds);
            DataTable dt = dataview.ToTable();

            if (dt.Rows.Count > 0)
            {
                GridUser.DataSource = dt;
                GridUser.DataBind();
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                GridUser.DataSource = dt;
                GridUser.DataBind();
                int Totalcolumns = GridUser.Rows[0].Cells.Count;
                GridUser.Rows[0].Cells.Clear();
                GridUser.Rows[0].Cells.Add(new TableCell());
                GridUser.Rows[0].Cells[0].ColumnSpan = Totalcolumns;
                GridUser.Rows[0].Cells[0].Text = "No Records Found";
                GridUser.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                GridUser.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Middle;
            }

            GridUser.Visible = true;
            errorlabel.Visible = false;
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    #endregion

}
