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



    string strfrmdate, strtodate = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("Loginpage.aspx");
        if (!Page.IsPostBack)
        {
            Lblusername.Text = "Welcome " + SessionHandler.UserName + " ..";
            txtfrmdate.Text = gl.setdate();
            txttodate.Text = gl.setdate();

            UsernameLoad();
            Clearfields();
            //LoadGrid();
        }
    }
    private void Clearfields()
    {
        ddlotype.SelectedIndex = 0;
        ddlusername.SelectedIndex = 0;
    }
    public void UsernameLoad()
    {
        ds.Dispose();
        ds.Reset();
        string strquery = "select User_Name from user_status order by User_Name";
        ds = con.ExecuteQuery(strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlusername.DataSource = ds;
            ddlusername.DataTextField = "User_Name";
            ddlusername.DataBind();
            ddlusername.Items.Insert(0, "ALL");
        }
    }
    private void LoadGrid()
    {
        try
        {
            if (txtfrmdate.Text != "")
            {
                dt = Convert.ToDateTime(txtfrmdate.Text);
                strfrmdate = String.Format("{0:MM/dd/yyyy}", dt);
                if (txttodate.Text != "")
                {
                    dt = Convert.ToDateTime(txttodate.Text);
                    strtodate = String.Format("{0:MM/dd/yyyy}", dt);
                    string strquery = "sp_postauditsearch";
                    string strOtype = ddlotype.SelectedItem.Text;
                    string strusr = ddlusername.SelectedItem.Text;
                    if (strOtype == "") { errorlabel.Text = "Please Select Order Type"; return; }
                    mdr = gl.PostTracking(strquery, strfrmdate, strtodate, strOtype, strusr);
                    ShowGrid(mdr, strfrmdate, strtodate);
                    errorlabel.Text = "";
                }
                else
                {
                    errorlabel.Text = "Please Select the To date";
                }
            }
            else
            {
                errorlabel.Text = "Please Select the From date";
            }
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }


    public void ShowGrid(MySqlDataReader mdr, string strfrmdate, string strtodate)
    {
        try
        {
            DataView dataview = gl.ConvertDataReaderToDataView(mdr);
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
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }

    protected void btnordershow_Click(object sender, EventArgs e)
    {
        LoadGrid();
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
    protected void btnmvereview_Click(object sender, EventArgs e)
    {
        int count = 0;
        foreach (GridViewRow row in GridUser.Rows)
        {
            CheckBox chkbx = (CheckBox)row.FindControl("chkselect");
            if (chkbx.Checked == true)
            {

                string query = "update record_status set Review ='3',Review_OP='" + SessionHandler.UserName + "' where Order_No='" + row.Cells[2].Text + "'";
                int result = con.ExecuteSPNonQuery(query);
                count++;
            }

        }
        LoadGrid();
        errorlabel.Text = count + " Order's Moved to Post Audit";
    }
}
