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
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

public partial class Pages_AssignJob : System.Web.UI.Page
{
    GlobalClass gblcls = new GlobalClass();
    myConnection con = new myConnection();
    DataSet ds = new DataSet();

    public delegate void MultipleValue(string strorderno, string strdate);
    MultipleValue Result;

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "") Response.Redirect("STRMICXLogin.aspx");
        if (SessionHandler.IsAdmin == false)
        {
            SessionHandler.UserName = "";
            Response.Redirect("STRMICXLogin.aspx");
        }
        if (!Page.IsPostBack)
        {
            TogglePanel(PanelSettings);
            txtdate.Text = gblcls.setdate();
            txtedate.Text = gblcls.setdate();
            txt_date.Text = gblcls.setdate();
            txtfrmdate.Text = gblcls.setdate();
            txttodate.Text = gblcls.setdate();
            Bindusernames();
            UsernameLoad();
        }
        Chkkey.Attributes.Add("onclick", "javascript:Uncheck1('" + Chkkey.ClientID + "','" + Chkdu.ClientID + "'");
        Chkdu.Attributes.Add("onclick", "javascript:Uncheck1('" + Chkdu.ClientID + "','" + Chkkey.ClientID + "')");
    }

    public void UsernameLoad()
    {
        ds.Dispose();
        ds.Reset();
        string strquery = "select User_Name from user_status where sst=0 order by User_Name";
        ds = con.ExecuteQuery(strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlusername.DataSource = ds;
            ddlusername.DataTextField = "User_Name";
            ddlusername.DataBind();
            ddlusername.Items.Insert(0, "");

            ddlusernameassign.DataSource = ds;
            ddlusernameassign.DataTextField = "User_Name";
            ddlusernameassign.DataBind();
            ddlusernameassign.Items.Insert(0, "Select Username");
        }
    }
    private void Clearfields()
    {
        lblstatuserror.Text = "";
        rdbtnstatuschange.Enabled = false;
        rdbtnstatuschange.Items[0].Selected = false;
        rdbtnstatuschange.Items[1].Selected = false;
        rdbtnstatuschange.Items[2].Selected = false;
        rdbtnstatuschange.Items[3].Selected = false;
        rdbtnstatuschange.Items[4].Selected = false;
        rdbtnstatuschange.Items[5].Selected = false;
    }

    private void TogglePanel(Panel sPanel)
    {
        PanelAssign.Visible = false;
        PanelReset.Visible = false;
        PanelPriority.Visible = false;
        PanelExcelAssign.Visible = false;
        PanelClearDb.Visible = false;
        PanelDeleteDb.Visible = false;
        PanelTracking.Visible = false;
        PanelStatusChange.Visible = false;
        PanelHighPriority.Visible = false;
        PanelSettings.Visible = false;
        PanelOrdertype.Visible = false;
        pnlClrYts.Visible = false;
        PanelDeleteyts.Visible = false;
        PanelAssignOrder.Visible = false;

        sPanel.Visible = true;
    }

    private void ToogleButton(Button sButton)
    {
        btnsave.Visible = false;
        BtnUpdate.Visible = false;

        sButton.Visible = true;
    }
    #endregion

    #region Order Assign
    private void BindDatarow(string orderno, string loan, string county, string state, string pry, DataTable dt, string expected, string serprovied)
    {
        DataRow dr = dt.NewRow();
        string ordno = orderno + "%";
        string query = "Select sf_ordersChk('" + ordno + "')";
        string result = con.ExecuteScalar(query);
        if (result == "" || result == "555" || result == "777")
        {
            dr = dt.NewRow();

            if (result == "") dr[0] = orderno.Trim();
            else dr[0] = GetOrders(orderno.Trim());

            dr[1] = county.Trim();
            dr[2] = state.Trim();
            dr[3] = pry.Trim();
            DateTime sam = Convert.ToDateTime(txtdate.Text);
            dr[4] = String.Format("{0:MM'/'dd'/'yyyy}", sam);
            dr[5] = expected;
            dr[6] = serprovied;
            dt.Rows.Add(dr);
        }
    }
    private string GetOrders(string ordno)
    {
        string orderno = "", strquery = "", result = "", strquery1 = "", result1 = "";
        int rscount, rs_backcount = 0;

        strquery = "select count(order_no) from record_status where order_no like '" + ordno + "%'";
        strquery1 = "select count(order_no) from record_status_backup where order_no like '" + ordno + "%'";
        result = con.ExecuteScalarst(strquery);
        result1 = con.ExecuteScalarst(strquery1);
        rscount = Convert.ToInt32(result);
        rs_backcount = Convert.ToInt32(result1);

        int ordercnt = rscount + rs_backcount;
        if (ordercnt == 1)
        {
            orderno = ordno + "_U";
        }
        else if (ordercnt >= 2)
        {
            //ordercnt = ordercnt - 1;
            orderno = ordno + "_U" + ordercnt;
        }
        return orderno;
    }
    protected void LnkUpload_Click(object sender, EventArgs e)
    {
        TogglePanel(PanelAssign);
    }
    protected void btntransmit_Click(object sender, EventArgs e)
    {
        int rowcnt = 0;
        DataTable dt = new DataTable();

        dt.Columns.Add("Orderno");
        dt.Columns.Add("County");
        dt.Columns.Add("State");
        dt.Columns.Add("Priority");
        dt.Columns.Add("CreatedDate");
        dt.Columns.Add("ExpectedClosing");
        dt.Columns.Add("serprovied");

        if (txtAssign.Text == "") { return; }
        string orderno, loandescription, county, state, countyandstate, priority, strexpected, expected;
        string txt = txtAssign.Text;
        string[] splitnewline = txtAssign.Text.Split('\n');
        for (int i = 0; i < splitnewline.Length; i = i + 1)
        {
            string[] splitslash = splitnewline[i].Split('\t');
            orderno = ""; loandescription = ""; county = ""; state = ""; countyandstate = ""; priority = ""; strexpected = ""; expected = "0";
            string n1 = (string)Regex.Replace(splitslash[0], @"[\D]", "");
            orderno = n1.Substring(0, 8);
            countyandstate = splitslash[1];
            string[] splitycounty = countyandstate.Split('-');
            state = splitycounty[0];
            county = splitycounty[1];
            //priority = (string)Regex.Replace(splitslash[3], @"[\D]", "");

            string[] p2 = splitslash[2].Split('-');
            priority = p2[0].ToString().Trim();

            string[] arrexpect = p2[1].ToString().Split(new string[] { "Date " }, StringSplitOptions.None);
            strexpected = arrexpect[0].ToString().ToLower().Trim();

            if (strexpected == "expected closing") expected = "1";
            else expected = "0";

            string serprovied = splitslash[3].Trim();

            //serpro = p3[0].ToString().Trim();

            BindDatarow(orderno, loandescription, county, state, priority, dt, expected, serprovied);
        }
        GridOrders.DataSource = dt;
        GridOrders.DataBind();
        rowcnt = GridOrders.Rows.Count;
        GridOrders.Visible = true;
        lblerrmsg.Text = rowcnt + " rows added to grid";
        lblerrmsg.Visible = true;
    }
    protected void Btnupload_Click(object sender, EventArgs e)
    {
        try
        {
            PanelPriority.Visible = false;
            int count = 0;
            foreach (GridViewRow gr in GridOrders.Rows)
            {
                string o_no = gr.Cells[1].Text.Trim();
                string country = gr.Cells[2].Text.Trim();
                string state = gr.Cells[3].Text.Trim();
                string pri = gr.Cells[4].Text.Trim();
                string date = gr.Cells[5].Text.Trim();
                string expect = gr.Cells[6].Text.Trim();
                string serpro = gr.Cells[7].Text.Trim();
                if (myVariables.pDate == "")
                {
                    myVariables.pDate = txtdate.Text;
                }
                count += gblcls.InsertData_New(gr.Cells[1].Text, state, country, pri, date, expect, "", serpro);
            }
            GridOrders.Visible = false;
            lblerrmsg.Text = count + " orders assigned.";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Reset Panel
    protected void LnkReset_Click(object sender, EventArgs e)
    {
        try
        {
            TogglePanel(PanelReset);
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    protected void LnkPriority_Click(object sender, EventArgs e)
    {
        try
        {
            TogglePanel(PanelPriority);
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_date.Text == "") return;
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txt_date.Text);
            string pdate = String.Format("{0:MM/dd/yyyy}", dt);
            gblcls.GetOrders(lstbx, pdate);
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            Click("YTS");
            lblerrormsg.Text = "Order Moved to YTS Successfully";
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            Click("Delete");
            lblerrormsg.Text = "Order Deleted Successfully";
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    protected void btnlock_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txt_date.Text);
            string pdate = String.Format("{0:MM/dd/yyyy}", dt);
            if (btnlock.Text == "Lock")
            {
                Click("Lock");
                lblerrormsg.Text = "Order Locked Successfully";
            }
            else if (btnlock.Text == "Unlock")
            {
                Click("Unlock");
                btnlock.Text = "Lock";
                lblerrormsg.Text = "Order Unlocked Successfully";
            }
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    protected void btnprior_Click(object sender, EventArgs e)
    {
        try
        {
            Click("Priority");
            lblerrormsg.Text = "Order Successfully Assiged to Priority...";
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
        }
    }
    protected void btnreject_Click(object sender, EventArgs e)
    {
        try
        {
            Click("Reject");
            lblerrormsg.Text = "Order Rejected";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btninproces_Click(object sender, EventArgs e)
    {
        try
        {
            Click("Inprocess");
            lblerrormsg.Text = "Order Successfully Moved to Inprocess";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btdelyts_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txt_date.Text);
            string pdate = String.Format("{0:MM/dd/yyyy}", dt);
            string strquery = "Update record_status set Pdate='" + pdate + "',DownloadtimeEST=now() where k1='0' and qc='0' and status='0' and pend='0' and Tax='0' and parcel='0'";
            int result = con.ExecuteSPNonQuery(strquery);
            lblerrormsg.Text = "Order Date Successfully Updated";
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
        }
    }
    protected void btndelmissing_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txt_date.Text);
            string pdate = String.Format("{0:MM/dd/yyyy}", dt);
            //string strquery = "Update record_status set Pdate='" + pdate + "',DownloadtimeEST=now() where k1='0' and qc='0' and status='0' and pend='0' and Tax='0' and parcel='0'";
            string strquery = "Delete from record_status where K1='9' and QC='9' and Status='9' and Review='9' and Pdate='" + pdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
            if (result > 0) lblerrormsg.Text = "Order Missing Deleted Successfully";
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
        }
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {
        try
        {
            string strquery = "update record_status set k1=0,qc=0,status=0,K1_OP=null,K1_ST=null,K1_ET=null where k1=2 and qc=0 and key_status='Others' and Comments_Det1 like '%Order locked%'";
            int result = con.ExecuteSPNonQuery(strquery);
            if (result > 0) lblerrormsg.Text = "Others moved to YTS Successfully";
            //DateTime dt = new DateTime();
            //dt = Convert.ToDateTime(txt_date.Text);
            //string pdate = String.Format("{0:MM/dd/yyyy}", dt);
            //gblcls.GetOrdersShow(lstbx, pdate);
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = ex.ToString();
        }
    }
    public void Click(string strresult)
    {
        try
        {
            if (strresult == "Delete")
            {
                //Result = new MultipleValue(gblcls.GetOrderDelete);
            }
            else if (strresult == "Lock")
            {
                //Result = new MultipleValue(gblcls.GetOrderLock);
            }
            else if (strresult == "Unlock")
            {
                //Result = new MultipleValue(gblcls.GetOrderUnlock);
            }
            else if (strresult == "Priority")
            {
                //Result = new MultipleValue(gblcls.GetOrderPriority);
            }
            else if (strresult == "Reject")
            {
                //Result = new MultipleValue(gblcls.GetOrderReject);
            }
            else if (strresult == "Inprocess")
            {
                //Result = new MultipleValue(gblcls.GetOrderInprocess);
            }
            else if (strresult == "YTS")
            {
                //Result = new MultipleValue(gblcls.GetOrderYTS);
            }
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txt_date.Text);
            string pdate = string.Format("{0:MM/dd/yyyy}", dt);
            for (int i = 0; i < lstbx.Items.Count; i++)
            {
                if (lstbx.Items[i].Selected == true)
                {
                    Result(lstbx.Items[i].Text, pdate);
                }
            }
            gblcls.GetOrders(lstbx, pdate);
        }
        catch (Exception ex)
        {
            errorlabel.Text = ex.ToString();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorSearch.Text = "";
            lstbx.SelectedIndex = -1;
            if (txtSearch.Text == "")
            {
                lblErrorSearch.Text = "Please Enter the Order No to Search";
            }
            else
            {
                ListItem lstitm = lstbx.Items.FindByValue(txtSearch.Text);
                if (lstitm != null)
                {
                    lstbx.Items.FindByValue(txtSearch.Text).Selected = true;
                }
                else
                {
                    lblErrorSearch.Text = "Order No Not Found";
                }
            }

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
        }
    }
    protected void lstbx_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Text = "";
            lstStatus.Items.Clear();
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txt_date.Text);
            string pdate = String.Format("{0:MM/dd/yyyy}", dt);
            string strorderno = lstbx.SelectedItem.Text;
            ds.Dispose();
            ds.Reset();
            string strquery = "Select K1,QC,Lock1,Rejected,HP,status,Pend,Parcel,Tax from record_status WHERE record_status.Order_No ='" + strorderno + "' and pdate='" + pdate + "'";
            ds = con.ExecuteQuery(strquery);

            if (ds.Tables[0].Rows.Count > 0)
            {
                int key = Convert.ToInt32(ds.Tables[0].Rows[0]["K1"]);
                int qc = Convert.ToInt32(ds.Tables[0].Rows[0]["QC"]);
                int lck = Convert.ToInt32(ds.Tables[0].Rows[0]["Lock1"]);
                int rej = Convert.ToInt32(ds.Tables[0].Rows[0]["Rejected"]);
                int hp = Convert.ToInt32(ds.Tables[0].Rows[0]["HP"]);
                int status = Convert.ToInt32(ds.Tables[0].Rows[0]["status"]);
                int pend = Convert.ToInt32(ds.Tables[0].Rows[0]["Pend"]);
                int parcel = Convert.ToInt32(ds.Tables[0].Rows[0]["Parcel"]);
                int tax = Convert.ToInt32(ds.Tables[0].Rows[0]["Tax"]);

                if (lck == 1)
                {
                    btnlock.Text = "Unlock";
                    lstStatus.Items.Add("Locked");
                    return;
                }
                else
                {
                    btnlock.Text = "Lock";
                }
                if (key == 0 && qc == 0 && status == 0 && pend == 0 && parcel == 0 && tax == 0) { lstStatus.Items.Add("YTS"); return; }
                if (key == 1 && qc == 0 && status == 1 && pend == 0 && parcel == 0 && tax == 0) { lstStatus.Items.Add("Locked in Production"); return; }
                if (key == 2 && qc == 0 && status == 2 && pend == 0 && parcel == 0 && tax == 0) { lstStatus.Items.Add("Keying Completed"); return; }
                if (key == 2 && qc == 1 && status == 1 && pend == 0 && parcel == 0 && tax == 0) { lstStatus.Items.Add("Locked in QC"); return; }
                if (key == 5 && qc == 5 && status == 5 && pend == 0 && parcel == 0 && tax == 0) { lstStatus.Items.Add("Delivered"); return; }

                if (key == 2 && qc == 2 && status == 2 && pend == 3 && parcel == 0 && tax == 0) { lstStatus.Items.Add("Pending"); return; }
                if (key == 2 && qc == 1 && status == 1 && pend == 1 && parcel == 0 && tax == 0) { lstStatus.Items.Add("Pending Started"); return; }

                if (key == 2 && qc == 2 && status == 2 && pend == 0 && parcel == 0 && tax == 3) { lstStatus.Items.Add("MailAway"); return; }
                if (key == 2 && qc == 1 && status == 1 && pend == 0 && parcel == 0 && tax == 1) { lstStatus.Items.Add("MailAway Started"); return; }

                if (key == 2 && qc == 2 && status == 2 && pend == 0 && parcel == 3 && tax == 0) { lstStatus.Items.Add("ParcelId"); return; }
                if (key == 2 && qc == 1 && status == 1 && pend == 0 && parcel == 1 && tax == 0) { lstStatus.Items.Add("ParcelId Started"); return; }

                if (key == 4 && qc == 4 && status == 4 && pend == 0 && parcel == 0 && tax == 0) { lstStatus.Items.Add("OnHold"); return; }

                if (key == 7 && qc == 7 && status == 7 && pend == 0 && parcel == 0 && tax == 0) { lstStatus.Items.Add("Rejected"); return; }

                //if (pend == 1) { lstStatus.Items.Add("Pending"); return; }                
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
        }
    }
    #endregion

    #region Priority Panel
    protected void btnpriortransmit_Click(object sender, EventArgs e)
    {
        string strorderdetails = string.Empty;
        string[] rowcnt;
        string[] colcnt;
        int slnocnt = 0;
        try
        {
            strorderdetails = txtpriority.Text;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("S.No");
            dt.Columns.Add("Order No");
            dt.Columns.Add("Priority");
            strorderdetails = strorderdetails.Trim('\r', '\n');
            rowcnt = strorderdetails.Split('\n');
            foreach (string rowdata in rowcnt)
            {
                dr = dt.NewRow();
                colcnt = rowdata.Split('\t');
                if (colcnt.Length > 1)
                {
                    string orderno = "";
                    orderno = CheckOrders(colcnt[0].Trim());
                    if (orderno != "")
                    {
                        slnocnt++;
                        dr[0] = slnocnt.ToString();
                        dr[1] = orderno;
                        dr[2] = colcnt[1].Trim('\r').ToString();
                        dt.Rows.Add(dr);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                ds.Dispose();
                ds.Reset();
                ds.Tables.Add(dt);
                grdpriority.DataSource = ds;
                grdpriority.DataBind();
                lblerror.Text = dt.Rows.Count + " rows added to grid";
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
        }
    }
    private string CheckOrders(string ono)
    {
        ono = ono + "%";
        string query = "Select sp_getOrderno('" + ono + "')";
        return con.ExecuteScalar(query);
    }
    private string CheckOrderType(string state, string county)
    {
        string query = "Select sf_getordertype('" + state + "','" + county + "')";
        return con.ExecuteScalar(query);
    }
    protected void btnpriorityupdate_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            foreach (GridViewRow grd in grdpriority.Rows)
            {
                int result = 0;
                string strquery = "";
                string o_no = grd.Cells[1].Text.Trim();
                string strpriority = grd.Cells[2].Text.Trim();

                strquery = "Update record_status set prior='" + grd.Cells[2].Text + "' where order_no='" + grd.Cells[1].Text + "'";
                result = con.ExecuteSPNonQuery(strquery);
                count += 1;
                if (result == 1)
                {
                    lblerror.Text = count + " rows Priority updated";
                    lblerror.Visible = true;
                }
                else
                {
                    lblerror.Text = "Priority cannot be change";
                    lblerror.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region High Priority
    protected void LnkHPriority_Click(object sender, EventArgs e)
    {
        try
        {
            TogglePanel(PanelHighPriority);
        }
        catch (Exception ex)
        {
            lblhperror.Text = ex.ToString();
        }
    }
    protected void btnhpriotrans_Click(object sender, EventArgs e)
    {
        string strorderdetails = string.Empty;
        string[] rowcnt;
        string[] colcnt;
        int slnocnt = 0;
        try
        {
            strorderdetails = txthp.Text;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("S.No");
            dt.Columns.Add("Order No");
            strorderdetails = strorderdetails.Trim('\r', '\n');
            rowcnt = strorderdetails.Split('\n');
            foreach (string rowdata in rowcnt)
            {
                dr = dt.NewRow();
                colcnt = rowdata.Split('\t');
                if (colcnt.Length == 1)
                {
                    string orderno = "";
                    orderno = CheckOrders(colcnt[0].Trim());
                    if (orderno != "")
                    {
                        slnocnt++;
                        dr[0] = slnocnt.ToString();
                        dr[1] = orderno;
                        dt.Rows.Add(dr);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                ds.Dispose();
                ds.Reset();
                ds.Tables.Add(dt);
                grdhpriority.DataSource = ds;
                grdhpriority.DataBind();
                lblhperror.Text = dt.Rows.Count + " rows added to grid";
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
        }
    }
    protected void btnhprioupdate_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            foreach (GridViewRow grd in grdhpriority.Rows)
            {
                int result = 0;
                string strquery = "";
                string o_no = grd.Cells[1].Text.Trim();

                strquery = "Update record_status set HP='1' where order_no='" + grd.Cells[1].Text + "'";
                result = con.ExecuteSPNonQuery(strquery);
                count += 1;
                if (result == 1)
                {
                    lblhperror.Text = count + " rows High Priority updated";
                    lblhperror.Visible = true;
                }
                else
                {
                    lblhperror.Text = "Priority cannot be change";
                    lblhperror.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    protected void btneupload_Click(object sender, EventArgs e)
    {
        try
        {
            PanelPriority.Visible = false;
            int count = 0;
            foreach (GridViewRow gr in GridExcelOrders.Rows)
            {
                string o_no = gr.Cells[1].Text.Trim();
                string country = gr.Cells[2].Text.Trim();
                string state = gr.Cells[3].Text.Trim();
                string pri = gr.Cells[4].Text.Trim();
                string date = gr.Cells[5].Text.Trim();
                if (myVariables.pDate == "")
                {
                    myVariables.pDate = txtdate.Text;
                }
                count += gblcls.InsertData_New(gr.Cells[1].Text, state, country, pri, date, "", "", "");
            }
            GridExcelOrders.Visible = false;
            lbleerrmsg.Text = count + " orders assigned.";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnetransmit_Click(object sender, EventArgs e)
    {
        int rowcnt = 0;
        DataTable dt = new DataTable();

        dt.Columns.Add("Orderno");
        dt.Columns.Add("County");
        dt.Columns.Add("State");
        dt.Columns.Add("Priority");
        dt.Columns.Add("CreatedDate");

        if (txteAssign.Text == "") { return; }
        string orderno, loandescription, county, state, priority;
        string txt = txteAssign.Text.Trim();
        string[] splitnewline = txt.Split('\n');
        int count = 0;
        int count1 = 0;
        foreach (string str in splitnewline)
        {
            string[] splitslash = str.Split('\t');
            orderno = ""; loandescription = ""; county = ""; state = ""; priority = "";
            orderno = splitslash[0];
            state = splitslash[1];
            county = splitslash[2];
            priority = splitslash[3];

            if (count == 0)
            {
                BindDatarow(orderno, loandescription, county, state, priority, dt, "", "");
            }
            else if (count > 0)
            {
                count1 = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (orderno != dt.Rows[i]["Orderno"].ToString())
                    {
                        count1++;
                    }
                }
                if (count1 == dt.Rows.Count)
                {
                    BindDatarow(orderno, loandescription, county, state, priority, dt, "", "");
                }
            }
            count++;
        }
        GridExcelOrders.DataSource = dt;
        GridExcelOrders.DataBind();
        rowcnt = GridExcelOrders.Rows.Count;
        GridExcelOrders.Visible = true;
        lbleerrmsg.Text = rowcnt + " rows added to grid";
        lbleerrmsg.Visible = true;
    }

    protected void LnkUplodExcel_Click(object sender, EventArgs e)
    {
        TogglePanel(PanelExcelAssign);
    }

    #region Clear Database
    protected void LnkClearDatabase_Click(object sender, EventArgs e)
    {
        lbldberror.Text = "";
        lblerrordb.Text = "";
        TogglePanel(PanelClearDb);
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        if (txtpassword.Text == "tsitaxes123$") TogglePanel(PanelDeleteDb);
        else lbldberror.Text = "Please Enter Correct Password";
    }
    protected void btnordershow_Click(object sender, EventArgs e)
    {
        string frmdate, todate = "";
        DateTime frmdt = Convert.ToDateTime(txtfrmdate.Text);
        frmdate = String.Format("{0:MM/dd/yyyy}", frmdt);
        DateTime todt = Convert.ToDateTime(txttodate.Text);
        todate = String.Format("{0:MM/dd/yyyy}", todt);

        if (frmdate != "" && todate != "")
        {
            string query = "select pdate as Date,count(id) as OrderCount from record_status where str_to_date(pdate,'%m/%d/%Y') between str_to_date('" + frmdate + "','%m/%d/%Y') and str_to_date('" + todate + "','%m/%d/%Y') and (K1=5 or K1=7) and (QC=5 or QC=7) and (`Status`=5 or `Status`=7) group by str_to_date(pdate,'%m/%d/%Y')";
            ds = con.ExecuteQuery(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridClearDb.DataSource = ds;
                GridClearDb.DataBind();
            }
            else
            {
                GridClearDb.DataSource = null;
                GridClearDb.DataBind();
            }
        }
        else
        {
            lblerrordb.Text = "Please Select From date and To date";
        }
    }
    protected void btnshowdelete_Click(object sender, EventArgs e)
    {
        string frmdate, todate = "";
        DateTime frmdt = Convert.ToDateTime(txtfrmdate.Text);
        frmdate = String.Format("{0:MM/dd/yyyy}", frmdt);
        DateTime todt = Convert.ToDateTime(txttodate.Text);
        todate = String.Format("{0:MM/dd/yyyy}", todt);

        int result = gblcls.DeleteDB(frmdate, todate);
        btnordershow_Click(sender, e);
        lblerrordb.Text = "Order's Deleted Successfully";
    }
    #endregion

    #region Tracking No Update
    protected void LnkTracking_Click(object sender, EventArgs e)
    {
        lbltrackingerror.Text = "";
        txttracking.Text = "";
        TogglePanel(PanelTracking);
    }
    protected void btntracking_Click(object sender, EventArgs e)
    {
        if (txttracking.Text != "")
        {
            string strtracking, strorderno, strtrackingno, strchequeno = "";
            strtracking = txttracking.Text;
            int count = 0;
            string[] splitnewline = strtracking.Split('\n');
            for (int i = 0; i < splitnewline.Length - 1; i++)
            {
                string str = splitnewline[i];
                string[] splitslash = str.Split('\t');
                strorderno = splitslash[0].Trim().ToString();
                strtrackingno = splitslash[1].Trim().ToString();
                strchequeno = splitslash[2].Trim().ToString();

                string query = "Update mailaway_tbl set TrackingNo='" + strtrackingno + "',ChequeNo='" + strchequeno + "' where Order_no='" + strorderno + "'";
                int result = con.ExecuteSPNonQuery(query);
                count++;
            }
            lbltrackingerror.Text = "" + count + " Orders Updated Tracking# and Cheque#";
        }
    }
    #endregion

    //#region Status Change Panel
    //protected void LnkStatuschange_Click(object sender, EventArgs e)
    //{
    //    lblstatuserror.Text = "";
    //    txtstatuschange.Text = "";
    //    TogglePanel(PanelStatusChange);
    //}
    //protected void btnstatuschange_Click(object sender, EventArgs e)
    //{
    //    if (txtstatuschange.Text != "")
    //    {
    //        string strstatus, strorderno = "";
    //        strstatus = txtstatuschange.Text;
    //        int count = 0;
    //        string[] splitnewline = strstatus.Split('\n');
    //        for (int i = 0; i < splitnewline.Length - 1; i++)
    //        {
    //            strorderno = splitnewline[i].Trim().ToString();
    //            string query = "Update record_status set K1=0,QC=0,Status=0,Direct=0,Review=0,Parcel=0,Pend=0,Tax=0,Lock1=0,K1_OP=null,K1_ST=null,K1_ET=null,QC_OP=null,QC_ST=null,QC_ET=null,key_status=null,ComStatus=null,Error=null,ErrorField=null,Incorrect=null,Correct=null where Order_no='" + strorderno + "'";
    //            int result = con.ExecuteSPNonQuery(query);
    //            count++;
    //        }
    //        lblstatuserror.Text = "" + count + " Orders Moved To YTS Status";
    //    }
    //}

    //protected void btnassignorder_Click(object sender, EventArgs e)
    //{
    //    string strusername = ddlusername.SelectedItem.Text;
    //    strusername = gblcls.TCase(strusername);
    //    string strorderno = txtstatuschange.Text;
    //    int count = 0;
    //    if (strusername != "" && strorderno != "")
    //    {
    //        string[] splitnewline = strorderno.Split('\n');
    //        for (int i = 0; i < splitnewline.Length - 1; i++)
    //        {
    //            string orders = splitnewline[i].Trim().ToString();
    //            string query = "update record_status set K1_OP='" + strusername + "' where Order_No='" + orders + "' and K1=0 and QC=0 and Status=0 and Tax=0 and Parcel=0 and Pend=0";
    //            int result = con.ExecuteSPNonQuery(query);
    //            if (result > 0) count++;
    //        }
    //        lblstatuserror.Text = "" + count + " Orders Assiged To " + strusername + ".";
    //    }
    //}
    //#endregion



    #region Status Change Panel
    protected void LnkStatuschange_Click(object sender, EventArgs e)
    {
        try
        {
            txtstatuschange.Text = "";
            txtmovestatus.Text = "";
            Clearfields();
            TogglePanel(PanelStatusChange);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnmove_Click(object sender, EventArgs e)
    {
        string strorderno = txtmovestatus.Text;
        string[] ord_no = strorderno.Split(new string[] { "\r\n" }, StringSplitOptions.None);

        string orders, query = "";
        for (int i = 0; i < ord_no.Length; i++)
        {
            orders = ord_no[i];
            if (rdbtnstatuschange.Items[0].Selected == true)
            {
                query = "update record_status set k1='2',qc='2',status='2',tax='0',pend='3',parcel='0',rejected='0',lock1='0' where Order_No='" + ord_no[i].ToString() + "'";
            }
            else if (rdbtnstatuschange.Items[1].Selected == true)
            {
                query = "update record_status set k1='2',qc='2',status='2',tax='0',pend='0',parcel='3',rejected='0',lock1='0' where Order_No='" + ord_no[i].ToString() + "'";
            }
            else if (rdbtnstatuschange.Items[2].Selected == true)
            {
                query = "update record_status set k1='2',qc='2',status='2',tax='3',pend='0',parcel='0',rejected='0',lock1='0' where Order_No='" + ord_no[i].ToString() + "'";
            }
            else if (rdbtnstatuschange.Items[3].Selected == true)
            {
                query = "update record_status set k1='4',qc='4',status='4',tax='0',pend='0',parcel='0',rejected='0',lock1='0' where Order_No='" + ord_no[i].ToString() + "'";
            }
            else if (rdbtnstatuschange.Items[4].Selected == true)
            {
                query = "update record_status set k1='7',qc='7',status='7', tax='0',pend='0',parcel='0',rejected='0',lock1='0' where Order_No='" + ord_no[i].ToString() + "'";
            }
            else if (rdbtnstatuschange.Items[5].Selected == true)
            {
                query = "update record_status set K1=0,QC=0,Status=0,Direct=0,Review=0,Parcel=0,Pend=0,Tax=0,Lock1=0,K1_OP=null,K1_ST=null,K1_ET=null,QC_OP=null,QC_ST=null,QC_ET=null,key_status=null,ComStatus=null,Error=null,ErrorField=null,Incorrect=null,Correct=null where Order_No='" + ord_no[i].ToString() + "'";
            }
            else if (rdbtnstatuschange.Items[7].Selected == true)
            {
                query = "update record_status set K1=2,QC=0,Status=2,Review=0,Parcel=0,Pend=0,Tax=0,Lock1=0,QC_OP=null,QC_ST=null,QC_ET=null,key_status='Completed',ComStatus=null where Order_No='" + ord_no[i].ToString() + "'";
            }
            con.ExecuteSPNonQuery(query);
        }
        txtstatuschange.Text = txtmovestatus.Text;
        txtmovestatus.Text = "";
        Clearfields();
        lblstatuserror.Text = "Orders Moved Successfully";
    }

    protected void txtmovestatus_TextChanged(object sender, EventArgs e)
    {
        if (txtmovestatus.Text != "")
        {
            rdbtnstatuschange.Enabled = true;
        }
    }

    protected void btnassignorder_Click(object sender, EventArgs e)
    {
        string strusername = ddlusername.SelectedItem.Text;
        string sostate = "";
        string strquery = "";
        if (strusername != " ")
        {
            string strorderno = txtmovestatus.Text;
            string[] ord_no = strorderno.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            string orders, query = "";
            for (int i = 0; i < ord_no.Length; i++)
            {
                orders = ord_no[i];
                strquery = "select State from record_status where Order_No='" + orders + "'";

                string sstate = con.ExecuteScalar(strquery);
                if (sostate == "")
                {
                    sostate = sstate;
                }
                else
                {
                    sostate = sostate + "," + sstate;
                }

                if (rdbtnstatuschange.Items[0].Selected == true)
                {
                    query = "update record_status set K1_OP='" + strusername + "',k1=2,qc=1,status=1,pend=1,tax=0,parcel=0 where Order_No='" + ord_no[i].ToString() + "'";
                }
                else if (rdbtnstatuschange.Items[1].Selected == true)
                {
                    query = "update record_status set K1_OP='" + strusername + "',k1=2,qc=1,status=1,pend=0,tax=0,parcel=0 where Order_No='" + ord_no[i].ToString() + "'";
                }
                else if (rdbtnstatuschange.Items[2].Selected == true)
                {
                    query = "update record_status set K1_OP='" + strusername + "',k1=2,qc=1,status=1,pend=0,tax=1,parcel=0 where Order_No='" + ord_no[i].ToString() + "'";
                }
                else if (rdbtnstatuschange.Items[3].Selected == true)
                {
                    query = "update record_status set QC_OP='" + strusername + "',k1=2,qc=1,status=1,pend=0,tax=0,parcel=0 where Order_No='" + ord_no[i].ToString() + "'";
                    break;
                }
                else if (rdbtnstatuschange.Items[4].Selected == true)
                {
                    lblstatuserror.Text = "Can't Assign Orders for Rejected Status ";
                    break;
                }
                else if (rdbtnstatuschange.Items[5].Selected == true)
                {
                    query = "update record_status set K1_OP='" + strusername + "',k1=1,qc=0,status=1 where Order_No='" + ord_no[i].ToString() + "'";
                }
                else if (rdbtnstatuschange.Items[6].Selected == true)
                {
                    query = "update record_status set K1_OP='" + strusername + "',k1=1,qc=0,status=1,Direct=1 where Order_No='" + ord_no[i].ToString() + "'";
                }
                else if (rdbtnstatuschange.Items[7].Selected == true)
                {
                    lblstatuserror.Text = "Can't Assign Orders for QC Status ";
                    break;
                }
                con.ExecuteSPNonQuery(query);
            }
            string query1 = "update user_status set State='" + sostate + "' where User_Name='" + strusername + "'";
            con.ExecuteSPNonQuery(query1);
            if (rdbtnstatuschange.Items[1].Selected != true && rdbtnstatuschange.Items[2].Selected != true && rdbtnstatuschange.Items[3].Selected != true && rdbtnstatuschange.Items[5].Selected != true && rdbtnstatuschange.Items[6].Selected != true)
            {
                txtstatuschange.Text = txtmovestatus.Text;
                txtmovestatus.Text = "";
                Clearfields();
                lblstatuserror.Text = "Orders Assigned Successfully";
            }
        }
        else
        {
            lblstatuserror.Text = "Please Select Username";
        }
    }
    #endregion

    #region Intialization
    private void Pageinitialize()
    {
        //LoadStateCount();
        //BindState();
        BindOrderType();
        Bindusernames();
        Lstuser.SelectedIndex = 0;
        ClearFields();
        EditUser(Lstuser.SelectedItem.Text);
        gblcls.EditUsers(SessionHandler.UserName);
        ToogleButton(BtnUpdate);
        if (myVariables.SST == "1") Chkadmin.Visible = true;
        else Chkadmin.Visible = false;
    }

    //private void BindState()
    //{
    //    ds.Dispose();
    //    ds.Reset();
    //    string query = "select State from state_countylist group by State";
    //    ds = con.ExecuteQuery(query);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        LstState.DataSource = ds;
    //        LstState.DataTextField = "State";
    //        LstState.DataBind();
    //    }
    //}
    private void BindOrderType()
    {
        //ddlotype.Items.Clear();
        //ListItem li = new ListItem("Select Order Type");
        //ddlotype.Items.Add(li);
        //ddlotype.AppendDataBoundItems = true;
        //DataSet ds = gblcls.BindDropdown();
        //if (ds.Tables.Count > 0)
        //{
        //    ddlotype.DataSource = ds;
        //    ddlotype.DataTextField = "type";
        //    ddlotype.DataBind();

        //}
    }
    private void Bindusernames()
    {
        try
        {
            DataSet dsprd = new DataSet();
            dsprd = gblcls.GetUsers();
            Lstuser.DataSource = dsprd;
            Lstuser.DataTextField = "User_Name";
            Lstuser.DataBind();
            Lstuser.Items.Insert(0, "");
            //Chkkey.Checked = true;            
            if (myVariables.IsErr == true) { gblcls.RedirectErrorPage(); }
        }
        catch (Exception ex)
        {
            myConnection.setError(ex.ToString());
            gblcls.RedirectErrorPage();
        }
    }
    public void LoadStateCount()
    {
        ds.Dispose();
        ds.Reset();
        string fdate = "";
        DateTime dtt = gblcls.ToDate();
        fdate = String.Format("{0:MM/dd/yyyy}", dtt);
        //string query = "select State,count(Order_No) as StateCount,sum(if(Isclosedate='1',1,0)) as 'Close Date' from record_status where pdate between '" + fdate + "' and '" + fdate + "' and k1='0' and qc='0' and status='0'and Tax ='0' and Parcel='0' and Pend='0' group by state order by StateCount desc";
        //ds = con.ExecuteQuery(query);
        //Gridstatecount.DataSource = ds;
        //Gridstatecount.DataBind();
    }
    #endregion

    #region User Settings
    protected void Lnksettings_Click(object sender, EventArgs e)
    {
        TogglePanel(PanelSettings);
        Pageinitialize();
    }

    #region Button Events
    protected void btnsave_Click(object sender, EventArgs e)
    {
        int success = 0;

        string Ad = "0", Keya = "0", Qc = "0", pend = "0", du = "0", ordertype = "0", mail = "0", onhold = "0", parcelid = "0", review = "0", priority = "0", prior = "0", QA = "0";

        if (!validation()) { return; }

        //ordertype = ddlotype.SelectedItem.Text;

        if (Chkadmin.Checked)
        { Ad = "1"; }

        if (Chkprio.Checked)
        { priority = "1"; }

        if (Chkkey.Checked)
        { Keya = "1"; }

        if (ChkQC.Checked)
        { Qc = "1"; }

        //if (Chkreview.Checked)
        //{ review = "1"; }

        if (Chkpending.Checked)
        { pend = "1"; }

        if (Chkdu.Checked)
        { du = "1"; }

        //if (Chkmail.Checked)
        //{ mail = "1"; }

        //if (Chkparcelid.Checked)
        //{ parcelid = "1"; }

        if (ChkOnhold.Checked)
        { onhold = "1"; }

        if (chkbxqa.Checked)
        { QA = "1"; }

        if (chkpriority.Checked)
        { prior = "1"; }

        success = gblcls.InsertUser(txtfulname.Text.Trim(), Ad, Keya, Qc, du, pend, mail, parcelid, onhold, ordertype, review, priority, QA, prior);

        if (myVariables.IsErr == true) { Showmessage(myVariables.ErrMsg); return; }

        ClearFields();

        if (success > 0) { Showmessage("Username Added Successfully."); }

        Pageinitialize();
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        int success = 0;

        string Ad = "0", Keya = "0", Qc = "0", pend = "0", du = "0", mail = "0", ordertype = "0", parcelid = "0", onhold = "0", review = "0", priority = "0", qa = "0", prior = "0";
        string strstate = "";

        if (!validation()) { return; }

        //ordertype = ddlotype.SelectedItem.Text;

        if (Chkadmin.Checked)
        { Ad = "1"; }

        if (Chkkey.Checked)
        { Keya = "1"; }

        if (ChkQC.Checked)
        { Qc = "1"; }

        //if (Chkreview.Checked)
        //{ review = "1"; }

        if (Chkpending.Checked)
        { pend = "1"; }

        if (Chkdu.Checked)
        { du = "1"; }

        //if (Chkmail.Checked)
        //{ mail = "1"; }

        //if (Chkparcelid.Checked)
        //{ parcelid = "1"; }

        if (ChkOnhold.Checked)
        { onhold = "1"; }

        if (Chkprio.Checked)
        { priority = "1"; }

        if (chkbxqa.Checked)
        { qa = "1"; }

        if (chkpriority.Checked)
        { prior = "1"; }

        foreach (GridViewRow row in Gridstatecount.Rows)
        {
            CheckBox chkbx = (CheckBox)row.FindControl("chkselect");
            if (chkbx.Checked == true)
            {
                if (strstate == "") strstate = row.Cells[1].Text;
                else strstate = strstate + "," + row.Cells[1].Text;
            }
        }

        for (int i = 0; i < Lstuser.Items.Count; i++)
        {
            if (Lstuser.Items[i].Selected == true)
            {
                success = gblcls.UpdateUser(Lstuser.Items[i].Text.Trim().ToString(), Ad, Keya, Qc, du, pend, mail, parcelid, onhold, ordertype, review, strstate, priority, qa, prior);
            }
        }

        if (myVariables.IsErr == true) { Showmessage(myVariables.ErrMsg); return; }

        ClearFields();

        if (success > 0) { Showmessage("Username Updated Successfully."); }

        //Pageinitialize();

    }
    protected void Btnnewuser_Click(object sender, EventArgs e)
    {
        ClearFields();
        ToogleButton(btnsave);
    }
    protected void Btnreset_Click(object sender, EventArgs e)
    {
        if (Lstuser.SelectedValue == "") { return; }
        int result = gblcls.ResetPassword(Lstuser.SelectedItem.Text);
        if (myVariables.IsErr == true) { Showmessage(myVariables.ErrMsg); return; }
        if (result > 0) Showmessage("Password Reset Successfully.");
    }
    protected void Btndelete_Click(object sender, EventArgs e)
    {
        string strquery = "delete from user_status where User_Name='" + Lstuser.SelectedItem.Text + "'";
        int result = con.ExecuteSPNonQuery(strquery);
        if (result > 0)
        {
            Bindusernames();
            Lstuser.SelectedIndex = 0;
            Lstuser_SelectedIndexChanged(sender, e);
            Showmessage("Username Deleted Successfully.");
        }
    }
    #endregion

    #region Other Fuction
    private bool validation()
    {
        bool result = true;
        if (txtfulname.Text == string.Empty) { Showmessage("Please fill the fullname"); result = false; return result; }
        if (txtusername.Text == string.Empty) { Showmessage("Please fill the username"); result = false; return result; }
        if (Chkkey.Checked == false & ChkQC.Checked == false && Chkpending.Checked == false && Chkdu.Checked == false && ChkOnhold.Checked == false && Chkprio.Checked == false) { Showmessage("Please select any one of process(i.e. Key,Qc,Pend or Mailaway)"); result = false; return result; }
        //if (ddlotype.SelectedIndex == 0)
        //{
        //    Showmessage("Please select the Ordertype");
        //    result = false;
        //    return result;
        //}
        return result;
    }
    private void Showmessage(string msg)
    {
        LiteralErr.Text = msg;
    }
    private void ClearFields()
    {
        txtfulname.Text = "";
        txtfulname.ReadOnly = false;
        txtusername.Text = "";
        txtusername.ReadOnly = false;
        //ddlotype.SelectedIndex = 0;
        Chkadmin.Checked = false;
        Chkkey.Checked = false;
        ChkQC.Checked = false;
        Chkdu.Checked = false;
        Chkpending.Checked = false;
        //Chkmail.Checked = false;
        //Chkparcelid.Checked = false;
        ChkOnhold.Checked = false;
        //Chkreview.Checked = false;
        Chkprio.Checked = false;
        chkbxqa.Checked = false;
        chkpriority.Checked = false;
        //Lstuser.SelectedIndex = 0;
        LiteralErr.Text = "";
    }

    private void Clearcheckbox()
    {
        txtfulname.ReadOnly = false;
        txtusername.ReadOnly = false;
        Chkadmin.Checked = false;
        Chkkey.Checked = false;
        ChkQC.Checked = false;
        Chkdu.Checked = false;
        Chkpending.Checked = false;
        //Chkmail.Checked = false;
        //Chkparcelid.Checked = false;
        ChkOnhold.Checked = false;
        //Chkreview.Checked = false;
        Chkprio.Checked = false;
        chkbxqa.Checked = false;
        LiteralErr.Text = "";
    }

    protected void Lstuser_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        for (int i = 0; i < Lstuser.Items.Count; i++)
        {
            if (Lstuser.Items[i].Selected == true) count++;
        }
        if (count <= 1) EditUser(Lstuser.SelectedItem.Text);
        else Clearcheckbox();
        LoadStateCount();
    }
    private void EditUser(string uid)
    {
        if (uid == "") { ClearFields(); return; }
        gblcls.EditUsers(uid);
        txtfulname.Text = myVariables.Fullname;
        txtfulname.ReadOnly = true;
        txtusername.Text = myVariables.Username;
        txtusername.ReadOnly = true;

        //if (myVariables.Ordertype == "") ddlotype.SelectedIndex = 0;
        //else ddlotype.SelectedIndex = Convert.ToInt16(myVariables.Ordertype);

        if (myVariables.Admin == "0") Chkadmin.Checked = false;
        else Chkadmin.Checked = true;

        if (myVariables.Key == "0") Chkkey.Checked = false;
        else Chkkey.Checked = true;

        if (myVariables.QC == "0") ChkQC.Checked = false;
        else ChkQC.Checked = true;

        //if (myVariables.Review == "0") Chkreview.Checked = false;
        //else Chkreview.Checked = true;

        if (myVariables.Pend == "0") Chkpending.Checked = false;
        else Chkpending.Checked = true;

        if (myVariables.DU == "0") Chkdu.Checked = false;
        else Chkdu.Checked = true;

        //if (myVariables.Mailaway == "0") Chkmail.Checked = false;
        //else Chkmail.Checked = true;

        //if (myVariables.Parcelid == "0") Chkparcelid.Checked = false;
        //else Chkparcelid.Checked = true;

        if (myVariables.Onhold == "0") ChkOnhold.Checked = false;
        else ChkOnhold.Checked = true;

        if (myVariables.Priority == "0") Chkprio.Checked = false;
        else Chkprio.Checked = true;

        if (myVariables.QA == "0") chkbxqa.Checked = false;
        else chkbxqa.Checked = true;

        if (myVariables.AssPriority == "0") chkpriority.Checked = false;
        else chkpriority.Checked = true;

        //lblstates.Text = myVariables.States.Replace(",", ",\r");

        ToogleButton(BtnUpdate);
    }
    #endregion

    protected void Gridstatecount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chk = (CheckBox)e.Row.FindControl("chkselect");
            if (myVariables.States != "" && myVariables.States != null)
            {
                if (myVariables.States.Contains(e.Row.Cells[1].Text))
                {
                    chk.Checked = true;
                }
                else
                {
                    chk.Checked = false;
                }
            }
            else
            {
                chk.Checked = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((CheckBox)e.Row.FindControl("chkselect")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("chkselect")).ClientID + "')");
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
        if (PanelAssign.Visible == true) LnkUpload_Click(sender, e);
        else if (PanelReset.Visible == true) LnkReset_Click(sender, e);
        else if (PanelPriority.Visible == true) LnkPriority_Click(sender, e);
        else if (PanelExcelAssign.Visible == true) LnkUplodExcel_Click(sender, e);
        else if (PanelClearDb.Visible == true) LnkClearDatabase_Click(sender, e);
        else if (PanelTracking.Visible == true) LnkTracking_Click(sender, e);
        else if (PanelStatusChange.Visible == true) LnkStatuschange_Click(sender, e);
        else if (PanelHighPriority.Visible == true) LnkHPriority_Click(sender, e);
        else if (PanelSettings.Visible == true) Lnksettings_Click(sender, e);
    }
    #endregion

    #region Update OrderType
    protected void Lnkordertype_Click(object sender, EventArgs e)
    {
        TogglePanel(PanelOrdertype);
    }
    protected void btntypetransmit_Click(object sender, EventArgs e)
    {
        string strordertype = "";
        string[] rowcnt;
        string[] colcnt;
        int slnocnt = 0;
        try
        {
            strordertype = txtordertype.Text;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("S.No");
            dt.Columns.Add("State");
            dt.Columns.Add("County");
            dt.Columns.Add("Order Type");
            strordertype = txtordertype.Text.Trim('\r', '\n');
            rowcnt = strordertype.Split('\n');
            foreach (string rowdata in rowcnt)
            {
                dr = dt.NewRow();
                colcnt = rowdata.Split('\t');
                string strvalue, state, county, ordertype = "";
                state = colcnt[0].Trim().ToString();
                county = colcnt[1].Trim().ToString();
                ordertype = colcnt[2].Trim().ToString();
                strvalue = CheckOrderType(state, county);
                if (strvalue != "")
                {
                    slnocnt++;
                    dr[0] = slnocnt.ToString();
                    dr[1] = state;
                    dr[2] = county;
                    dr[3] = ordertype;
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ds.Dispose();
                ds.Reset();
                ds.Tables.Add(dt);
                GridOrdertype.DataSource = ds;
                GridOrdertype.DataBind();
                lbltypeerror.Text = dt.Rows.Count + " rows added to grid";
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
        }
    }
    protected void btntypeupdate_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            foreach (GridViewRow grd in GridOrdertype.Rows)
            {
                int result = 0;
                string strquery = "";
                string o_no = grd.Cells[1].Text.Trim();

                strquery = "Update state_countylist set Ordertype='" + grd.Cells[3].Text + "' where State='" + grd.Cells[1].Text + "' and County='" + grd.Cells[2].Text + "'";
                result = con.ExecuteSPNonQuery(strquery);
                count += 1;
                if (result == 1)
                {
                    lbltypeerror.Text = count + " rows Ordertype updated";
                    lbltypeerror.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region ClearYTS
    protected void LnkClearyts_Click(object sender, EventArgs e)
    {
        lblerroryts.Text = "";
        txtdateyts.Text = "";
        TogglePanel(pnlClrYts);
    }
    protected void btnClrYts_Click(object sender, EventArgs e)
    {
        if (txtClrYts.Text == "tsitaxes123$") TogglePanel(PanelDeleteyts);
        else lblYtsError.Text = "Please Enter Correct Password";
    }
    protected void btndeleteyts_Click(object sender, EventArgs e)
    {
        int res = con.ExecuteSPNonQuery("CALL `sp_deleteyts`('" + txtdateyts.Text + "','" + SessionHandler.UserName + "')");
        if (res > 0)
        {

            lblerroryts.Text = "Deleted";
        }
    }
    #endregion ClearYTS

    protected void btn_reset_brk_Click(object sender, EventArgs e)
    {
        DateTime dt = new DateTime();
        dt = DateTime.Now;
        string pdate = string.Empty;

        string UserName = txtusername.Text;
        if (txt_reason.Text == string.Empty)
        {
            lbl_brk_error.Text = "Please fill the delay reason...";
            lbl_brk_error.ForeColor = System.Drawing.Color.Red;
            return;
        }
        TimeSpan ptime = DateTime.Now.TimeOfDay;
        TimeSpan start = new TimeSpan(0, 0, 0);
        TimeSpan end = new TimeSpan(07, 0, 0);

        if (start <= ptime && ptime <= end)
        {
            dt = DateTime.Now.AddDays(-1);
            pdate = dt.ToString("dd-MMM-yyyy");
        }

        else
        {
            dt = DateTime.Now;
            pdate = dt.ToString("dd-MMM-yyyy");
        }

        bool result = gblcls.reset_break(UserName, pdate, txt_reason.Text, SessionHandler.UserName);
        if (result == true)
        {
            lbl_brk_error.Text = "Break time reseted sucessfully";
        }
        txt_reason.Text = "";
    }

    protected void LnkAssignOrder_Click(object sender, EventArgs e)
    {
        try
        {
            // txtassignorderchange.Text = "";
            //  txtmovestatus.Text = "";
            //Clearfields();
            TogglePanel(PanelAssignOrder);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void txtassignorder_TextChanged(object sender, EventArgs e)
    {
        if (txtassignorder.Text != "")
        {
            // rdbtnstatuschange.Enabled = true;
        }

    }

    protected void btnmoveassignorder_Click(object sender, EventArgs e)
    {
        string strusername = ddlusernameassign.SelectedItem.Text;
        string strorderno = txtassignorder.Text;
        string[] ord_no = strorderno.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        int count = 1;
        string orders, query = "";
        for (int i = 0; i < ord_no.Length; i++)
        {
            orders = ord_no[i];
            query = "update record_status set QC_OP='" + strusername + "',qc=0,status=0,UserHP='" + count + "' where Order_No='" + ord_no[i].ToString() + "'";
            con.ExecuteSPNonQuery(query);
            count++;
        }
        txtassignorder.Text = "";
        // Clearfields();
        lblstatusassignorder.Text = "Orders Assigned to QC Successfully";
    }

    protected void btnassignorderuser_Click(object sender, EventArgs e)
    {
        string strusername = ddlusernameassign.SelectedItem.Text;
        string strquery = "";
        if (strusername != " ")
        {
            string strorderno = txtassignorder.Text.Trim();
            string[] ord_no = strorderno.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string orders, query = "";
            int count = 1;
            for (int i = 0; i < ord_no.Length; i++)
            {
                orders = ord_no[i];

                query = "update record_status set K1_OP='" + strusername + "',k1=0,qc=0,status=0,Direct=0,UserHP='" + count + "' where Order_No='" + ord_no[i].ToString() + "'";
                con.ExecuteSPNonQuery(query);
                count++;
            }
            lblstatusassignorder.Text = "Orders Assigned to Production Successfully";
        }
        else
        {
            lblstatuserror.Text = "Please Select Username";
        }
    }

    protected void Unnamed_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please enter User Name');", true);
        //ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Choose Tax Type');", true);
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('test 9'); window.location='" + Request.ApplicationPath + "/Loginpage.aspx';", true);
        return;

    }
}
