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
}