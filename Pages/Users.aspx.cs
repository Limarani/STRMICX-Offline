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

public partial class Pages_Users : System.Web.UI.Page
{
    GlobalClass gl = new GlobalClass();
    myConnection con = new myConnection();
    DBConnection conn = new DBConnection();
    DataSet ds = new DataSet();
    string strdate = "";

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("STRMICXLogin.aspx");
		if (SessionHandler.IsAdmin == false)
        {
            SessionHandler.UserName = "";
            Response.Redirect("Loginpage.aspx");
        }
        if (!Page.IsPostBack)
        {
            strdate = gl.setdate();
            Lnkviewuser_Click(sender, e);
            PanelEdit.Visible = false;
        }
        //Chkmail.Attributes.Add("onclick", "javascript:Uncheck('" + Chkmail.ClientID + "','" + Chkkey.ClientID + "','" + ChkQC.ClientID + "','" + Chkdu.ClientID + "','" + Chkpending.ClientID + "','" + Chkparcelid.ClientID + "','" + ChkOnhold.ClientID + "')");
        //Chkpending.Attributes.Add("onclick", "javascript:Uncheck('" + Chkpending.ClientID + "','" + Chkkey.ClientID + "','" + ChkQC.ClientID + "','" + Chkdu.ClientID + "','" + Chkparcelid.ClientID + "','" + ChkOnhold.ClientID + "')");
        //Chkparcelid.Attributes.Add("onclick", "javascript:Uncheck('" + Chkparcelid.ClientID + "','" + Chkkey.ClientID + "','" + ChkQC.ClientID + "','" + Chkdu.ClientID + "','" + Chkpending.ClientID + "','" + ChkOnhold.ClientID + "')");
        //ChkOnhold.Attributes.Add("onclick", "javascript:Uncheck('" + ChkOnhold.ClientID + "','" + Chkkey.ClientID + "','" + ChkQC.ClientID + "','" + Chkdu.ClientID + "','" + Chkpending.ClientID + "','" + Chkparcelid.ClientID + "')");
        //Chkkey.Attributes.Add("onclick", "javascript:Uncheck1('" + Chkkey.ClientID + "','" + Chkdu.ClientID + "'");
        //Chkdu.Attributes.Add("onclick", "javascript:Uncheck1('" + Chkdu.ClientID + "','" + Chkkey.ClientID + "')");        
    }
    protected void Page_preInit(object sender, EventArgs e)
    {
        Page.Theme = SessionHandler.Theme;
    }
    #endregion

    #region Toggle Button
    private void TooglePanel(Panel sPanel)
    {
        PanelViewuser.Visible = false;

        sPanel.Visible = true;
    }
    #endregion

    #region View User Details
    protected void Lnkviewuser_Click(object sender, EventArgs e)
    {
        TooglePanel(PanelViewuser);
        Loadviewsetting("ALL");
        Loadonoffcount();
    }

    private void Loadonoffcount()
    {
        string query = "select (select count(id) from user_status where SST=0 and System is not null) as Online,(select count(id) from user_status where SST=0 and System is null) as Offline";
        ds.Dispose();
        ds.Reset();
        ds = con.ExecuteQuery(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblonline.Text = Convert.ToString(ds.Tables[0].Rows[0]["Online"]);
            lbloffline.Text = Convert.ToString(ds.Tables[0].Rows[0]["Offline"]);
        }
    }
    protected void userGrid_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void userGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    private void Loadviewsetting(string strstatus)
    {
        ds.Dispose();
        ds.Reset();
        ds = gl.GetuserDetails(strstatus);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Griduserdetails.DataSource = ds;
            Griduserdetails.DataBind();
        }
        else
        {
            Griduserdetails.DataSource = null;
            Griduserdetails.DataBind();
        }
    }
    protected void Lnkoffline_Click(object sender, EventArgs e)
    {
        Loadviewsetting("OFFLINE");
    }
    protected void Lnkonline_Click(object sender, EventArgs e)
    {
        Loadviewsetting("ONLINE");
    }
    protected void Griduserdetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "1") e.Row.Cells[1].Text = "Y";
            else e.Row.Cells[1].Text = "";

            if (e.Row.Cells[2].Text == "1") e.Row.Cells[2].Text = "Y";
            else e.Row.Cells[2].Text = "";

            if (e.Row.Cells[3].Text == "1") e.Row.Cells[3].Text = "Y";
            else e.Row.Cells[3].Text = "";

            if (e.Row.Cells[4].Text == "1") e.Row.Cells[4].Text = "Y";
            else e.Row.Cells[4].Text = "";

            if (e.Row.Cells[5].Text == "1") e.Row.Cells[5].Text = "Y";
            else e.Row.Cells[5].Text = "";

            if (e.Row.Cells[6].Text == "1") e.Row.Cells[6].Text = "Y";
            else e.Row.Cells[6].Text = "";

            if (e.Row.Cells[7].Text == "1") e.Row.Cells[7].Text = "Y";
            else e.Row.Cells[7].Text = "";

            if (e.Row.Cells[8].Text == "1") e.Row.Cells[8].Text = "Y";
            else e.Row.Cells[8].Text = "";

            if (e.Row.Cells[9].Text == "1") e.Row.Cells[9].Text = "Y";
            else e.Row.Cells[9].Text = "";

            if (e.Row.Cells[10].Text == "1") e.Row.Cells[10].Text = "Y";
            else e.Row.Cells[10].Text = "";

            if (e.Row.Cells[11].Text == "1") e.Row.Cells[11].Text = "Website";
            else if (e.Row.Cells[11].Text == "2") e.Row.Cells[11].Text = "Phone";
            else e.Row.Cells[11].Text = "";

            if (e.Row.Cells[12].Text != "" && e.Row.Cells[12].Text != "&nbsp;") e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            else e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
        }
    }
    #endregion

    #region Auto Refresh
    protected void Chkrefresh_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkrefresh.Checked == true) Refresh.Enabled = true;
        else Refresh.Enabled = false;
    }
    protected void Refresh_Tick(object sender, EventArgs e)
    {
        Lnkviewuser_Click(sender, e);
    }
    #endregion

    protected void Imguser_Click(object sender, EventArgs e)
    {
        TogglePanel(PanelEdit);
        ToggleButton(btnsave);
    }
    private void ToggleButton(Button sButton)
    {
        btnsave.Visible = false;
        Btupdate.Visible = false;
        sButton.Visible = true;
    }
    private void TogglePanel(Panel sPanel)
    {
        PanelViewuser.Visible = false;
        PanelEdit.Visible = false;
        sPanel.Visible = true;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        int success = 0;

        string Ad = "0", Qc = "0", du = "0",   pro = "0", ordertype = "0";

        if (ChkAdmin.Checked)
        { Ad = "1"; }

        if (ChkDU.Checked)
        { du = "1"; }

        if (ChkPro.Checked)
        { Qc = "1"; }

        if (ChkQC.Checked)
        { pro = "1"; }

       
        success = gl.InsertUser(txtusername.Text.Trim(), Ad, du, Qc, pro, ordertype);

      

        if (success > 0) { MessageBox("Username Added Successfully.");

            Response.Redirect("Users.aspx");
        } 

     
    }
    private void MessageBox(string message)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Alert", "alert('" + message + "');", true);
    }
    protected void Btupdate_Click(object sender, EventArgs e)
    {
        int success = 0;

        string Ad = "0", Qc = "0", du = "0", pro = "0", ordertype = "0";

        if (ChkAdmin.Checked)
        { Ad = "1"; }

        if (ChkDU.Checked)
        { du = "1"; }

        if (ChkPro.Checked)
        { pro = "1"; }

        if (ChkQC.Checked)
        { Qc = "1"; }

        success = gl.UpdateUser(txtusername.Text.Trim(), Ad,Qc, pro, du, ordertype);

        if (success > 0)
        {
            MessageBox("Username Updated Successfully.");

            Response.Redirect("Users.aspx");
        }
    }
    protected void Griduserdetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Edit"))
        {
            //LtlMsg.Text = string.Empty;
            GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            LinkButton hf1 = (LinkButton)gvr.FindControl("UserName");
            Hiddenid.Value = hf1.Text;
            FetchUserDetails(Hiddenid.Value);
        }

        if (e.CommandName.Equals("Delete"))
        {
            GridViewRow gvr = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            LinkButton hf1 = (LinkButton)gvr.FindControl("UserName");
            Hiddenid.Value = hf1.Text;
            string delete = "delete from user_status where User_Name ='" + Hiddenid.Value + "'";
            conn.ExecuteSPNonQuery(delete);
            MessageBox("Successfully Deleted");
            // UserDetails();
            Response.Redirect("Users.aspx");
        }
    }
    private void FetchUserDetails(string id)
    {
        string admin = "", username = "",  du = "", production = "", qc = "";
      
        string query = "select * from user_status where User_Name='" + id + "' limit 1";
     DataSet mdr = conn.ExecuteQuery(query);
        if (mdr.Tables[0].Rows.Count > 0)          
            {
            username = mdr.Tables[0].Rows[0]["User_Name"].ToString ();
            admin= mdr.Tables[0].Rows[0]["Admin"].ToString();
            du = mdr.Tables[0].Rows[0]["DU"].ToString();
            production= mdr.Tables[0].Rows[0]["Keying"].ToString();
            qc= mdr.Tables[0].Rows[0]["QC"].ToString();

           FillDatas(admin, username,du, production,qc);

        }
    }
    private void FillDatas(string admin,string username,string du,string production,string qc)
    {
        TogglePanel(PanelEdit);
        txtusername.Text = username;
        if (admin == "1") ChkAdmin.Checked = true;
        else ChkAdmin.Checked = false;

        if (du == "1") ChkDU.Checked = true;
        else ChkDU.Checked = false;

        if (production == "1") ChkPro.Checked = true;
        else ChkPro.Checked = false;

        if (qc == "1") ChkQC.Checked = true;
        else ChkQC.Checked = false;


    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Users.aspx");
    }
    protected void LnkUserName_Click(object sender, EventArgs e)
    {
        TogglePanel(PanelEdit);
        ToggleButton(Btupdate);
    }
}