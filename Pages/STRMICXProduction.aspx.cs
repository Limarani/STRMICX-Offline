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
using System.Globalization;
using System.Diagnostics;

//balaji
public partial class Pages_STRMICXProduction : System.Web.UI.Page
{
    myConnection con = new myConnection();
    GlobalClass gl = new GlobalClass();
    DataTable dtfetch = new DataTable();
    DataTable dttaxauthorities = new DataTable();
    DataTable dtfetchauthority1 = new DataTable();
    DataSet ds = new DataSet();
    string taxidnew = "";
    string orderid = string.Empty;
    string processname = string.Empty;
    string payeename = string.Empty;
    string id = "";
    string s = "";
    string taxagencytype = "";
    decimal Inst1, Inst2, Inst3, Inst4, Instoutput;
    public bool chk = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //string connect = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
        //using (MySqlConnection con = new MySqlConnection(connect))
        //{
        //var query = "select Id,taxid,taxyear,endyear from tbl_taxparcel";
        //MySqlCommand cmd = new MySqlCommand(query, con);
        //con.Open();
        //MySqlDataReader dr = cmd.ExecuteReader();

        if (!IsPostBack)
        {
            date1.Attributes["disabled"] = "disabled";
            date2.Attributes["disabled"] = "disabled";

            btntaxparcels.Enabled = true;
            DeliquentStatusAdd.Enabled = true;
            ExemptionAdd.Enabled = true;
            SpecialAdd.Enabled = true;
            btnpriordelinquenttax.Enabled = true;
            TaxStatus.Visible = true;
            gvTax.Visible = true;
            gvTaxParcel.Visible = true;
            taxPar.Visible = true;
            GvTaxStatus.Visible = true;
            TaxStatus.Visible = true;
            PnlTaxStatus.Visible = true;
            tblExestatus.Visible = true;
            gvExemption.Visible = true;
            tblSpecialstatus.Visible = true;
            gvSpecial.Visible = true;
            tblDeliquentStatus.Visible = true;
            gvDeliquentStatus.Visible = true;
            GrdPriordelinquent.Visible = true;


            DataTable dt = new DataTable();
            gvTaxParcel.DataSource = dt;
            gvTaxParcel.DataBind();
            GvTaxStatus.DataSource = dt;
            GvTaxStatus.DataBind();
            TaxStatus.Visible = true;
            gvTax.Visible = true;
            gvTaxParcel.Visible = true;
            taxPar.Visible = true;
            GvTaxStatus.Visible = true;
            TaxStatus.Visible = true;
            PnlTaxStatus.Visible = true;
            tblExestatus.Visible = true;
            gvExemption.Visible = true;
            tblSpecialstatus.Visible = true;
            gvSpecial.Visible = true;
            tblDeliquentStatus.Visible = true;
            gvDeliquentStatus.Visible = true;
            GrdPriordelinquent.Visible = true;

            id = Request.QueryString["id"];
            if (id == "12f7tre5") Allotment(sender, e);
            else ManualAllotment(id, sender, e);
            FetchTax_Status();
            //loadgridtaxparcel();
            fetchtaxparcel();
            fetchDeliquentStatus();
            fetchexemptionsAll();
            fetchspecialAll();
            fetchAllpriordelinquent();
            FetchAdd_Notes();
            PnlTax1.Visible = false;
            Lblusername.Text = SessionHandler.UserName;
            fetchtaxparceldetails();
            Prior.Visible = false;
        }
    }



    private void ManualAllotment(string id, object sender, EventArgs e)
    {
        ds.Dispose();
        ds.Reset();
        ds = gl.Allotment("ManualAllotment", id);
        DatasetAllotment(ds, sender, e);
    }

    private void Allotment(object sender, EventArgs e)
    {
        string locks = gl.get_user_lock(SessionHandler.UserName);
        if (locks == "2")
        {
            return;
        }
        else
        {
            ds.Dispose();
            ds.Reset();
            ds = gl.Allotment("Autoallotment", "");
            DatasetAllotment(ds, sender, e);
        }
    }


    private void DatasetAllotment(DataSet ds, object sender, EventArgs e)
    {
        if (ds.Tables.Count == 0)
        {

        }
        else if (ds.Tables.Count == 1)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                string strresult = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                if (strresult == "Order Error")
                {

                    //lblinfo.Text = "You have an error in one Order. So please accept your error...!";
                    //LogoutText();
                    return;
                }
                else if (strresult == "Production Locked")
                {
                    //LogoutText();
                    return;
                }
                else if (strresult == "Check Rights")
                {
                    //lblinfo.Text = "Please check your Rights...!";
                    //LogoutText();
                    return;
                }
            }
        }
        else
        {
            orderallotment(ds);
        }
    }

    private void fetchtaxparceldetails()
    {
        string query = "";
        query = "select taxid,status from tbl_taxparcel where orderno = '" + lblord.Text + "' and (status = 'M' or status = 'C' or status = 'CD')";
        DataSet ds = gl.ExecuteQuery(query);

        txtTaxNo.DataSource = ds.Tables[0];
        txtTaxNo.DataTextField = "taxid";
        txtTaxNo.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string status = ds.Tables[0].Rows[i]["status"].ToString();
                string taxid = ds.Tables[0].Rows[i]["taxid"].ToString();

                if (status == "C")
                {
                    txtTaxNo.Items[i].Attributes.Add("style", "background-color:#32CD32;color:white;font-weight:bold;");
                }

                else if (status == "CD")
                {
                    txtTaxNo.Items[i].Attributes.Add("style", "background-color:#32CD32;color:white;font-weight:bold;");
                }

                else if (status == "M")
                {

                }
            }
        }
        txtTaxNo.Items.Insert(0, "--Select--");
    }



    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string customerId = gvTaxParcel.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvOrders = e.Row.FindControl("gvOrders") as GridView;

            string query = "";
            query = "select Id,Orderno, TaxId, AgencyId, TaxAuthorityName, TaxAgencyType, TaxAgencyState, Phone, TaxYearStartDate, PreferredContactMethod, JobTitle, City, County, State, ContactType, Zip, Address1  from tbl_taxauthorities2 where TaxId = '" + customerId + "'";
            DataSet ds = gl.ExecuteQuery(query);
            gvOrders.DataSource = ds.Tables[0];
            gvOrders.DataBind();
        }
    }
    private static DataTable GetData(string query)
    {
        string strConnString = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
        using (MySqlConnection con = new MySqlConnection(strConnString))
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = query;
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }



    protected void btnaddauthority_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        string agen = "";
        string taxid = "";
        string taxagencytype = "";
        DataSet dsinsert = new DataSet();
        DataSet dstest = new DataSet();
        dttaxauthorities.Columns.AddRange(new DataColumn[6] { new DataColumn("AgencyId"), new DataColumn("TaxAuthorityName"), new DataColumn("TaxAgencyType"), new DataColumn("TaxAgencyState"), new DataColumn("Phone"), new DataColumn("TaxYearStartDate") });

        foreach (GridViewRow row in gvtaxauthorities.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (row.Cells[5].FindControl("chkauthority") as CheckBox);
                if (chkRow.Checked)
                {
                    string taxidnew = LblTaxID.Text.Trim();
                    string AgencyId = row.Cells[0].Text.Trim();
                    string TaxAuthorityName = row.Cells[1].Text.Trim();
                    string TaxAgencyType = row.Cells[2].Text.Trim();
                    string TaxAgencyState = row.Cells[3].Text.Trim();
                    string Phone = row.Cells[4].Text.Trim();
                    string taxyearstartdate = row.Cells[5].Text.Trim();
                    dttaxauthorities.Rows.Add(AgencyId, TaxAuthorityName, TaxAgencyType, TaxAgencyState, Phone, taxyearstartdate);

                    string query = "";
                    query = "select AgencyId,TaxId,TaxAgencyType from tbl_taxauthorities2 where Orderno = '" + lblord.Text + "' and TaxId = '" + taxidnew + "' and AgencyId = '" + AgencyId + "' and TaxAgencyType = '" + TaxAgencyType + "'";
                    DataSet ds = gl.ExecuteQuery(query);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            agen = ds.Tables[0].Rows[i]["AgencyId"].ToString().Trim();
                            taxid = ds.Tables[0].Rows[i]["TaxId"].ToString().Trim();
                            taxagencytype = ds.Tables[0].Rows[i]["TaxAgencyType"].ToString().Trim();

                            if (TaxAgencyType != taxagencytype)
                            {
                                dsinsert = gl.inserttaxauthorities(lblord.Text.Trim(), taxidnew.Trim(), AgencyId.Trim());
                                if (dsinsert.Tables[0].Rows.Count == 0)
                                {
                                    int update;
                                    dstest = gl.test(lblord.Text.Trim(), AgencyId.Trim(), TaxAgencyType.Trim());
                                    update = gl.Updatetaxauthoritiesdetails(lblord.Text, taxidnew, AgencyId.Trim());
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Agency Id Already Exist')", true);
                            }
                        }
                    }
                    else if (ds.Tables[0].Rows.Count == 0)
                    {
                        dsinsert = gl.inserttaxauthorities(lblord.Text.Trim(), taxidnew.Trim(), AgencyId.Trim());
                        if (dsinsert.Tables[0].Rows.Count == 0)
                        {
                            int update;
                            dstest = gl.test(lblord.Text.Trim(), AgencyId.Trim(), TaxAgencyType.Trim());
                            update = gl.Updatetaxauthoritiesdetails(lblord.Text.Trim(), taxidnew.Trim(), AgencyId.Trim());
                        }
                    }
                }
            }
        }
        gvTaxParcel.Focus();
        fetchtaxparcel();
        fetchtaxparceldetails();
    }
    private void checkagencydetails(string AgencyId)
    {

    }


    protected void lnkgvOrders_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lb.NamingContainer;
        int index;
        DataTable dtfetchauthority = new DataTable();
        if (row != null)
        {
            index = row.RowIndex; //gets the row index selected                                                        

            PnlTax.Visible = true;
            btnsavetaxauthorities.Visible = false;
            string AgencyId = lb.Text;
            Session["AgencyId"] = AgencyId;


            LblAgencyId1.Text = lb.Text;
            LblTaxID.Text = Server.HtmlDecode(row.Cells[6].Text.Trim());


            //txtAuthorityname.Text = Server.HtmlDecode(row.Cells[2].Text.Trim());                                 

            GridView gvwnested = (GridView)gvTaxParcel.Rows[0].Cells[1].FindControl("gvOrders");
            GridViewRow Gv2Row = (GridViewRow)((LinkButton)sender).NamingContainer;
            GridView Childgrid = (GridView)(Gv2Row.Parent.Parent);

            foreach (GridViewRow rowcolor in Childgrid.Rows)
            {
                if (rowcolor.BackColor == System.Drawing.Color.LightGreen)
                {
                    rowcolor.BackColor = System.Drawing.Color.White;
                }
            }


            LinkButton lnktaxtype = (LinkButton)Childgrid.Rows[index].Cells[3].FindControl("lnkAgnecy");
            taxagencytype = lnktaxtype.Text;
            txtTaxType.Text = taxagencytype;

            row.BackColor = System.Drawing.Color.LightGreen;

            LblAgencyID.Text = lb.Text;
            LblTaxId1.Text = Server.HtmlDecode(row.Cells[6].Text.Trim());
            dtfetchauthority = gl.FetchTaxAuthorityDetails(lblord.Text, LblTaxID.Text, LblAgencyID.Text, taxagencytype);
            fetchDeliquentStatus();
            fetchexemptionsAll();
            fetchspecialAll();
            fetchAllpriordelinquent();
            cleardelinquentfields();
            if (dtfetchauthority.Rows.Count > 0)
            {
                //contact information
                txtAuthorityname.Text = dtfetchauthority.Rows[0]["TaxAuthorityName"].ToString().Trim();
                txtprefcontactmethod.Text = dtfetchauthority.Rows[0]["PreferredContactMethod"].ToString().Trim();
                txtemail.Text = dtfetchauthority.Rows[0]["mainemail"].ToString().Trim();
                txtphone.Text = dtfetchauthority.Rows[0]["MainPhoneNumber"].ToString().Trim();
                Lbljobtitle.Text = dtfetchauthority.Rows[0]["JobTitle"].ToString().Trim();
                LblCity1.Text = dtfetchauthority.Rows[0]["City"].ToString().Trim();
                txtCounty.Text = dtfetchauthority.Rows[0]["County"].ToString().Trim();


                txtfax.Text = dtfetchauthority.Rows[0]["MainFax"].ToString().Trim();
                txtState.Text = dtfetchauthority.Rows[0]["State"].ToString().Trim();
                txtstartyeardate.Text = dtfetchauthority.Rows[0]["TaxYearStartDate"].ToString().Trim();
                txtZip.Text = dtfetchauthority.Rows[0]["Zip"].ToString().Trim();
                Lbladdress.Text = dtfetchauthority.Rows[0]["Address1"].ToString().Trim();
                txtmisc.Text = dtfetchauthority.Rows[0]["MiscInfo"].ToString().Trim();
                lblOperation.Text = dtfetchauthority.Rows[0]["OperationHours"].ToString().Trim();
                lblphNos.Text = "";


                instamount1.Value = dtfetchauthority.Rows[0]["Instamount1"].ToString().Trim();
                instamount2.Value = dtfetchauthority.Rows[0]["Instamount2"].ToString().Trim();
                instamount3.Value = dtfetchauthority.Rows[0]["Instamount3"].ToString().Trim();
                instamount4.Value = dtfetchauthority.Rows[0]["Instamount4"].ToString().Trim();

                instamountpaid1.Value = dtfetchauthority.Rows[0]["Instamountpaid1"].ToString().Trim();
                instamountpaid2.Value = dtfetchauthority.Rows[0]["Instamountpaid2"].ToString().Trim();
                instamountpaid3.Value = dtfetchauthority.Rows[0]["Instamountpaid3"].ToString().Trim();
                instamountpaid4.Value = dtfetchauthority.Rows[0]["Instamountpaid4"].ToString().Trim();

                instpaiddue1.Value = dtfetchauthority.Rows[0]["InstPaidDue1"].ToString().Trim();
                instpaiddue2.Value = dtfetchauthority.Rows[0]["InstPaidDue2"].ToString().Trim();
                instpaiddue3.Value = dtfetchauthority.Rows[0]["InstPaidDue3"].ToString().Trim();
                instpaiddue4.Value = dtfetchauthority.Rows[0]["InstPaidDue4"].ToString().Trim();

                remainingbalance1.Value = dtfetchauthority.Rows[0]["Remainingbalance1"].ToString().Trim();
                remainingbalance2.Value = dtfetchauthority.Rows[0]["Remainingbalance2"].ToString().Trim();
                remainingbalance3.Value = dtfetchauthority.Rows[0]["Remainingbalance3"].ToString().Trim();
                remainingbalance4.Value = dtfetchauthority.Rows[0]["Remainingbalance4"].ToString().Trim();


                if (dtfetchauthority.Rows[0]["duedate1"].ToString().Trim() != "")
                {
                    string date1 = dtfetchauthority.Rows[0]["duedate1"].ToString().Trim();

                    if (date1.Contains("T"))
                    {
                        instdate1.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["duedate1"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        instdate1.Value = dtfetchauthority.Rows[0]["duedate1"].ToString().Trim();
                    }
                }
                if (dtfetchauthority.Rows[0]["duedate2"].ToString().Trim() != "")
                {
                    string date2 = dtfetchauthority.Rows[0]["duedate2"].ToString().Trim();

                    if (date2.Contains("T"))
                    {
                        instdate2.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["duedate2"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        instdate2.Value = dtfetchauthority.Rows[0]["duedate2"].ToString().Trim();
                    }
                }
                if (dtfetchauthority.Rows[0]["duedate3"].ToString().Trim() != "")
                {
                    string date3 = dtfetchauthority.Rows[0]["duedate3"].ToString().Trim();
                    if (date3.Contains("T"))
                    {
                        instdate3.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["duedate3"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        instdate3.Value = dtfetchauthority.Rows[0]["duedate3"].ToString().Trim();
                    }
                }
                if (dtfetchauthority.Rows[0]["duedate4"].ToString().Trim() != "")
                {
                    string date4 = dtfetchauthority.Rows[0]["duedate4"].ToString().Trim();
                    if (date4.Contains("T"))
                    {
                        instdate4.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["duedate4"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        instdate4.Value = dtfetchauthority.Rows[0]["duedate4"].ToString().Trim();
                    }
                }


                if (dtfetchauthority.Rows[0]["delinquentdate1"].ToString().Trim() != "")
                {
                    string duedate1 = dtfetchauthority.Rows[0]["delinquentdate1"].ToString().Trim();

                    if (duedate1.Contains("T"))
                    {
                        delinq1.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["delinquentdate1"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        delinq1.Value = dtfetchauthority.Rows[0]["delinquentdate1"].ToString().Trim();
                    }
                }

                if (dtfetchauthority.Rows[0]["delinquentdate2"].ToString().Trim() != "")
                {
                    string duedate2 = dtfetchauthority.Rows[0]["delinquentdate2"].ToString().Trim();
                    if (duedate2.Contains("T"))
                    {
                        delinq2.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["delinquentdate2"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        delinq2.Value = dtfetchauthority.Rows[0]["delinquentdate2"].ToString().Trim();
                    }
                }

                if (dtfetchauthority.Rows[0]["delinquentdate3"].ToString().Trim() != "")
                {
                    string duedate3 = dtfetchauthority.Rows[0]["delinquentdate3"].ToString().Trim();
                    if (duedate3.Contains("T"))
                    {
                        delinq3.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["delinquentdate3"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        delinq3.Value = dtfetchauthority.Rows[0]["delinquentdate3"].ToString().Trim();
                    }
                }

                if (dtfetchauthority.Rows[0]["delinquentdate4"].ToString().Trim() != "")
                {
                    string duedat4 = dtfetchauthority.Rows[0]["delinquentdate4"].ToString().Trim();
                    if (duedat4.Contains("T"))
                    {
                        delinq4.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["delinquentdate4"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        delinq4.Value = dtfetchauthority.Rows[0]["delinquentdate4"].ToString().Trim();
                    }
                }

                if (dtfetchauthority.Rows[0]["DiscountDate1"].ToString().Trim() != "")
                {
                    string disdat1 = dtfetchauthority.Rows[0]["DiscountDate1"].ToString().Trim();
                    if (disdat1.Contains("T"))
                    {
                        discdate1.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["DiscountDate1"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        discdate1.Value = dtfetchauthority.Rows[0]["DiscountDate1"].ToString().Trim();
                    }
                }

                if (dtfetchauthority.Rows[0]["DiscountDate2"].ToString().Trim() != "")
                {
                    string disdat2 = dtfetchauthority.Rows[0]["DiscountDate2"].ToString().Trim();
                    if (disdat2.Contains("T"))
                    {
                        discdate2.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["DiscountDate2"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        discdate2.Value = dtfetchauthority.Rows[0]["DiscountDate2"].ToString().Trim();
                    }
                }

                if (dtfetchauthority.Rows[0]["DiscountDate3"].ToString().Trim() != "")
                {
                    string disdat3 = dtfetchauthority.Rows[0]["DiscountDate3"].ToString().Trim();
                    if (disdat3.Contains("T"))
                    {
                        discdate3.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["DiscountDate3"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        discdate3.Value = dtfetchauthority.Rows[0]["DiscountDate3"].ToString().Trim();
                    }
                }

                if (dtfetchauthority.Rows[0]["DiscountDate4"].ToString().Trim() != "")
                {
                    string disdat4 = dtfetchauthority.Rows[0]["DiscountDate4"].ToString().Trim();
                    if (disdat4.Contains("T"))
                    {
                        discdate4.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["DiscountDate4"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        discdate4.Value = dtfetchauthority.Rows[0]["DiscountDate4"].ToString().Trim();
                    }
                }
                discamt1.Value = dtfetchauthority.Rows[0]["Discountamount1"].ToString().Trim();
                discamt2.Value = dtfetchauthority.Rows[0]["Discountamount2"].ToString().Trim();
                discamt3.Value = dtfetchauthority.Rows[0]["Discountamount3"].ToString().Trim();
                discamt4.Value = dtfetchauthority.Rows[0]["Discountamount4"].ToString().Trim();

                //discdate1.Value = dtfetchauthority.Rows[0]["DiscountDate1"].ToString().Trim();
                //discdate2.Value = dtfetchauthority.Rows[0]["DiscountDate2"].ToString().Trim();
                //discdate3.Value = dtfetchauthority.Rows[0]["DiscountDate3"].ToString().Trim();
                //discdate4.Value = dtfetchauthority.Rows[0]["DiscountDate4"].ToString().Trim();

                exemptrelevy1.Value = dtfetchauthority.Rows[0]["ExemptRelevy1"].ToString().Trim();

                if (exemptrelevy1.Value == "yes")
                {
                    this.exemptrelevy1.Checked = true;
                }
                else
                {
                    this.exemptrelevy1.Checked = false;
                }

                exemptrelevy2.Value = dtfetchauthority.Rows[0]["ExemptRelevy2"].ToString();

                if (exemptrelevy2.Value == "yes")
                {
                    this.exemptrelevy2.Checked = true;
                }
                else
                {
                    this.exemptrelevy2.Checked = false;
                }

                exemptrelevy3.Value = dtfetchauthority.Rows[0]["ExemptRelevy3"].ToString();

                if (exemptrelevy3.Value == "yes")
                {
                    this.exemptrelevy3.Checked = true;
                }
                else
                {
                    this.exemptrelevy3.Checked = false;
                }

                exemptrelevy4.Value = dtfetchauthority.Rows[0]["ExemptRelevy4"].ToString();

                if (exemptrelevy4.Value == "yes")
                {
                    this.exemptrelevy4.Checked = true;
                }
                else
                {
                    this.exemptrelevy4.Checked = false;
                }

                //if (dtfetchauthority.Rows[0]["billingdate1"].ToString().Trim() != "")
                //{
                //    string nextbildate1 = dtfetchauthority.Rows[0]["billingdate1"].ToString().Trim();

                //    if (nextbildate1.Contains("T"))
                //    {
                //        nextbilldate1.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["billingdate1"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                //    }
                //    else
                //    {
                //        nextbilldate1.Value = dtfetchauthority.Rows[0]["billingdate1"].ToString().Trim();
                //    }
                //}

                //if (dtfetchauthority.Rows[0]["billingdate2"].ToString().Trim() != "")
                //{
                //    string nextbildate2 = dtfetchauthority.Rows[0]["billingdate2"].ToString().Trim();

                //    if (nextbildate2.Contains("T"))
                //    {
                //        nextbilldate2.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["billingdate2"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                //    }
                //    else
                //    {
                //        nextbilldate2.Value = dtfetchauthority.Rows[0]["billingdate2"].ToString().Trim();
                //    }
                //}


                nextbilldate1.Value = dtfetchauthority.Rows[0]["nextbilldate1"].ToString();
                nextbilldate2.Value = dtfetchauthority.Rows[0]["nextbilldate2"].ToString();

                taxbill.Value = dtfetchauthority.Rows[0]["taxbill"].ToString();
                instcomm.Value = dtfetchauthority.Rows[0]["installmentcomments"].ToString();

                if (dtfetchauthority.Rows[0]["BillingPeriodStartDate"].ToString().Trim() != "")
                {
                    string startdate = dtfetchauthority.Rows[0]["BillingPeriodStartDate"].ToString().Trim();

                    if (startdate.Contains("T"))
                    {
                        txtbillstartdate.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["BillingPeriodStartDate"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        txtbillstartdate.Value = dtfetchauthority.Rows[0]["BillingPeriodStartDate"].ToString().Trim();
                    }
                }

                if (dtfetchauthority.Rows[0]["BillingPeriodEndDate"].ToString().Trim() != "")
                {
                    string enddate = dtfetchauthority.Rows[0]["BillingPeriodEndDate"].ToString().Trim();

                    if (enddate.Contains("T"))
                    {
                        txtbillenddate.Value = Convert.ToDateTime(dtfetchauthority.Rows[0]["BillingPeriodEndDate"].ToString().Trim().Remove(10)).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        txtbillenddate.Value = dtfetchauthority.Rows[0]["BillingPeriodEndDate"].ToString().Trim();
                    }
                }

                if (instamount1.Value != "")
                {
                    Inst1 = decimal.Parse(instamount1.Value, CultureInfo.InvariantCulture);
                }
                if (instamount2.Value != "")
                {
                    Inst2 = decimal.Parse(instamount2.Value, CultureInfo.InvariantCulture);
                }
                if (instamount3.Value != "")
                {
                    Inst3 = decimal.Parse(instamount3.Value, CultureInfo.InvariantCulture);
                }
                if (instamount4.Value != "")
                {
                    Inst4 = decimal.Parse(instamount4.Value, CultureInfo.InvariantCulture);
                }

                if (instamount1.Value != "" || instamount2.Value != "" || instamount3.Value != "" || instamount4.Value != "")
                {
                    Instoutput = Inst1 + Inst2 + Inst3 + Inst4;
                    txtAnnualTaxAmount.Text = (Instoutput.ToString("#,##0.00"));
                }
                txtdeliquent.Text = dtfetchauthority.Rows[0]["IsDelinquent"].ToString().Trim();




                txtexemption.Text = dtfetchauthority.Rows[0]["IsExemption"].ToString().Trim();
                SecialAssmnt.Text = dtfetchauthority.Rows[0]["IsSpecial"].ToString().Trim();

                if (lblclientName.Text == "ORMS")
                {
                    Prior.Visible = true;
                    pastDeliquent.Visible = true;
                    pastDeliquent.Text = dtfetchauthority.Rows[0]["IsPastDelinquent"].ToString().Trim();
                }

                paymentfrequency.Value = dtfetchauthority.Rows[0]["TaxFrequency"].ToString().Trim();

                txtdeliquent.SelectedValue = dtfetchauthority.Rows[0]["IsDelinquent"].ToString().Trim();

                if (txtdeliquent.SelectedValue == "No")
                {
                    txtdeliquent.SelectedIndex = 0;
                }


                DataTable dtsdeliquent = new DataTable();
                //if (txtdeliquent.SelectedValue == "Yes")
                //{
                dtsdeliquent = gl.Fetchdeliquent(lblord.Text, LblAgencyId1.Text);

                if (dtsdeliquent.Rows.Count > 0)
                {
                    txtdeliPayee.Text = dtsdeliquent.Rows[0]["payee"].ToString().Trim();
                    txtdelitAddress.Text = dtsdeliquent.Rows[0]["address"].ToString().Trim();
                    txtdelitCity.Text = dtsdeliquent.Rows[0]["city"].ToString().Trim();
                    txtdelitState.Text = dtsdeliquent.Rows[0]["state"].ToString().Trim();
                    txtdelitzip.Text = dtsdeliquent.Rows[0]["zip"].ToString().Trim();
                    //}
                }

                //if (paymentfrequency.Value == "1")
                //{
                //    string inputData = instdate1.Value;
                //    DateTime iinstdate1 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                //    iinstdate1 = iinstdate1.AddYears(1);
                //    nextbilldate1.Value = iinstdate1.ToString("MM/dd/yyyy");
                //}
                //else if (paymentfrequency.Value == "2")
                //{
                //    string inputData = instdate2.Value;
                //    DateTime iinstdate2 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                //    iinstdate2 = iinstdate2.AddYears(1);
                //    nextbilldate1.Value = iinstdate2.ToString("MM/dd/yyyy");                                        
                //}
                //else if (paymentfrequency.Value == "3")
                //{
                //    string inputData = instdate3.Value;
                //    DateTime instdate3 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                //    instdate3 = instdate3.AddYears(1);
                //    nextbilldate1.Value = instdate3.ToString("MM/dd/yyyy");
                //}
                //else if (paymentfrequency.Value == "4")
                //{
                //    string inputData = instdate4.Value;
                //    DateTime instdate4 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                //    instdate4 = instdate4.AddYears(1);
                //    nextbilldate1.Value = instdate4.ToString("MM/dd/yyyy");
                //}
            }

            DataSet dstest = new DataSet();
            DataSet update = new DataSet();
            dtfetchauthority1 = gl.FetchTaxAuthoritywebsiteDetails(lblord.Text, LblTaxID.Text, LblAgencyID.Text);
            if (dtfetchauthority1.Rows.Count == 0)
            {
                dstest = gl.fetchwebsite(lblord.Text, AgencyId);
                update = gl.fetchwebsiteupdated(lblord.Text, AgencyId, LblTaxID.Text);
                dtfetchauthority1 = gl.FetchTaxAuthoritywebsiteDetails(lblord.Text, LblTaxID.Text, LblAgencyID.Text);
                gridwebsite.DataSource = dtfetchauthority1;
                gridwebsite.DataBind();
            }

            DataTable dtwebsite = new DataTable();
            if (dtfetchauthority1.Rows.Count > 0)
            {
                gridwebsite.DataSource = dtfetchauthority1;
                gridwebsite.DataBind();
            }
        }

        paymentfreq(paymentfrequency.Value);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }

    protected void btnAddAuthor_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtPastDel();", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        btnsavetaxauthorities.Visible = true;
        //btnTaxParcelSave.Visible = false;
    }
    protected void btnAddTaxParcelModal_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtPastDel();", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        PnlTax.Visible = true;
    }

    protected void gvTaxParcel_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        gvTaxParcel.EditIndex = -1;
        fetchtaxparcel();
        txtdrop.Value = "";
        txtTaxYear.Text = "";
        txtEndYear.Text = "";
    }

    protected void gvTaxParcel_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            gvTaxParcel.EditIndex = e.NewEditIndex;
            //loadgridtaxparcel();
            fetchtaxparcel();
        }
        catch (Exception ex)
        {
            throw ex;
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }

    protected void gvTaxParcel_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            TextBox id = gvTaxParcel.Rows[e.RowIndex].FindControl("HdnId") as TextBox;
            HtmlInputHidden Id = gvTaxParcel.Rows[e.RowIndex].FindControl("HdnId") as HtmlInputHidden;
            var taxid = gvTaxParcel.Rows[e.RowIndex].Cells[2].Text.ToString().Trim();
            var strValue = gvTaxParcel.Rows[e.RowIndex].Cells[1].Text.ToString().Trim();

            gl.update_taxparcel(strValue.ToString(), txtdrop.Value, txtTaxYear.Text, txtEndYear.Text, taxid, lblord.Text);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            btntaxparcels.Enabled = true;
            gvTaxParcel.EditIndex = -1;
            fetchtaxparcel();
            fetchtaxparceldetails();
            txtdrop.Value = "";
            txtTaxYear.Text = "";
            txtEndYear.Text = "";
            LblAgencyID.Text = "";
            LblAgencyId1.Text = "";
            LblTaxID.Text = "";
            LblTaxId1.Text = "";
            PnlTax.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Updated')", true);
            return;
            throw ex;
        }
    }


    protected void loadgridtaxparcel()
    {
        string query = "";

        query = "select * from tbl_taxparcel where OrderNo='" + lblord.Text + "'";
        DataSet ds = gl.ExecuteQuery(query);
        gvTaxParcel.DataSource = ds.Tables[0];
        gvTaxParcel.DataBind();
    }

    //amrock
    protected void btnTaxParcelSave_Click(object sender, EventArgs e)
    {
        int update = 0;

        if (exemptrelevy1.Checked == true)
        {
            exemptrelevy1.Value = "yes";
        }
        else
        {
            exemptrelevy1.Value = "no";
        }

        if (exemptrelevy2.Checked == true)
        {
            exemptrelevy2.Value = "yes";
        }
        else
        {
            exemptrelevy2.Value = "no";
        }

        if (exemptrelevy3.Checked == true)
        {
            exemptrelevy3.Value = "yes";
        }
        else
        {
            exemptrelevy3.Value = "no";
        }

        if (exemptrelevy4.Checked == true)
        {
            exemptrelevy4.Value = "yes";
        }
        else
        {
            exemptrelevy4.Value = "no";
        }

        update = gl.update_tax_authorities_paymentdetails(lblord.Text, LblTaxID.Text.ToString(), LblAgencyID.Text, txtTaxType.Text, txtstartyeardate.Text, instamount1.Value, instamount2.Value, instamount3.Value, instamount4.Value, instamountpaid1.Value, instamountpaid2.Value, instamountpaid3.Value, instamountpaid4.Value, instpaiddue1.Value, instpaiddue2.Value, instpaiddue3.Value, instpaiddue4.Value, remainingbalance1.Value, remainingbalance2.Value, remainingbalance3.Value, remainingbalance4.Value, instdate1.Value, instdate2.Value, instdate3.Value, instdate4.Value, delinq1.Value, delinq2.Value, delinq3.Value, delinq4.Value, discamt1.Value, discamt2.Value, discamt3.Value, discamt4.Value, discdate1.Value, discdate2.Value, discdate3.Value, discdate4.Value, exemptrelevy1.Value, exemptrelevy2.Value, exemptrelevy3.Value, exemptrelevy4.Value, nextbilldate1.Value, nextbilldate2.Value, taxbill.Value, paymentfrequency.Value, txtbillstartdate.Value, txtbillenddate.Value, ddlfuturetaxcalc.Text, instcomm.Value, "2");
        if (update == 1)
        {
            fetchtaxparcel();
            fetchtaxparceldetails();
            paymentfreq(paymentfrequency.Value);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Saved Successfully')", true);
            return;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Saved')", true);
            return;
        }
    }

    protected void btnsavetaxauthorities_Click(object sender, EventArgs e)
    {
        btnsavetaxauthorities.Enabled = true;
        int insert = 0;
        insert = gl.Insert_tax_authorities_paymentdetails(lblord.Text, LblTaxID.Text.ToString(), LblAgencyID.Text, txtTaxType.Text, instmanamount1.Value, instmanamount2.Value, instmanamount3.Value, instmanamount4.Value, instmanamtpaid1.Value, instmanamtpaid2.Value, instmanamtpaid3.Value, instmanamtpaid4.Value, ddlmaninstpaiddue1.Value, ddlmaninstpaiddue2.Value, ddlmaninstpaiddue3.Value, ddlmaninstpaiddue4.Value, txtmanurembal1.Value, txtmanurembal2.Value, txtmanurembal3.Value, txtmanurembal4.Value, txtmaninstdate1.Value, txtmaninstdate2.Value, txtmaninstdate3.Value, txtmaninstdate4.Value, txtmandeliqdate1.Value, txtmandeliqdate2.Value, txtmandeliqdate3.Value, txtmandeliqdate4.Value, txtmandisamount1.Value, txtmandisamount2.Value, txtmandisamount3.Value, txtmandisamount4.Value, txtmandisdate1.Value, txtmandisdate2.Value, txtmandisdate3.Value, txtmandisdate4.Value, chkexrelmanu1.Value, chkexrelmanu2.Value, chkexrelmanu3.Value, chkexrelmanu4.Value, txtmanunextbilldate1.Value, txtmanunextbilldate2.Value, ddlmanutaxbill.Value, ddlpayfreqmanu.Value, txtmanubillstartdate.Value, txtmanubillenddate.Value, txtinstcommentsmanual.Value);
        if (insert == 1)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Saved Successfully')", true);
            return;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Saved')", true);
            return;
        }
    }
    protected void btnTaxParcelModal_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string GVCommand = e.CommandName.ToLower();

        if (GVCommand == "edit")
        {
            fetchtaxparceldetails();
            string Item_ID = (e.CommandArgument).ToString();
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTaxParcel.Rows[rowIndex];
            txtdrop.Value = Server.HtmlDecode(row.Cells[2].Text.Trim());
            txtTaxYear.Text = Server.HtmlDecode(row.Cells[3].Text.Trim());
            txtEndYear.Text = Server.HtmlDecode(row.Cells[4].Text.Trim());
            LinkButton lnkedit = (LinkButton)gvTaxParcel.Rows[rowIndex].FindControl("LnkEdit");
            btntaxparcels.Enabled = false;
            lnkedit.CommandName = "Update";
            lnkedit.CssClass = "glyphicon glyphicon-ok";
            lnkedit.ToolTip = "Update";
            lnkedit.OnClientClick = "javascript : return confirm('Are you sure, want to update this Row?');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        }

        if (GVCommand == "selectpardelete")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            string Item_ID = (e.CommandArgument).ToString();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int result = gl.DeleteGridParcel(row.Cells[1].Text.Trim());
            if (result == 1)
            {
                btntaxparcels.Enabled = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully')", true);
            }
            btntaxparcels.Enabled = true;
            fetchtaxparcel();
            fetchtaxparceldetails();
            PnlTax.Visible = false;
            PnlTax1.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        }
        if (GVCommand == "selectaddauthor")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTaxParcel.Rows[rowIndex];

            LblTaxID.Text = Server.HtmlDecode(row.Cells[2].Text.Trim());
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "AuthorityopenModal();", true);
            PanelAuthorityInfo.Visible = true;

            DataTable dtfetchauthority = new DataTable();
            dtfetchauthority = gl.FetchAUDetails(lblord.Text);

            gvtaxauthorities.DataSource = dtfetchauthority;
            gvtaxauthorities.DataBind();
        }
    }

    private void orderallotment(DataSet ds)
    {
        DataTable dtfetch = new DataTable();

        if (ds.Tables[0].Rows.Count > 0)
        {
            orderid = ds.Tables[0].Rows[0]["Order_No"].ToString();
            processname = ds.Tables[0].Rows[0]["pType"].ToString();
            dtfetch = gl.FetchOrderDetailsnew(orderid);
        }

        if (dtfetch.Rows.Count == 1)
        {
            processtatus.Text = processname;
            lblord.Text = dtfetch.Rows[0]["OrderDetailId"].ToString();
            lbltransactiontype.Text = dtfetch.Rows[0]["TransactionType"].ToString();
            lblloannumber.Text = dtfetch.Rows[0]["LoanNumber"].ToString();
            lblestimatedvalue.Text = dtfetch.Rows[0]["EstimatedValue"].ToString();
            lblpurchaseprice.Text = dtfetch.Rows[0]["PurchasePrice"].ToString();
            lblborrowername.Text = dtfetch.Rows[0]["borrowername"].ToString();
            lblsellername.Text = dtfetch.Rows[0]["sellername"].ToString();
            lblyearbuilt.Text = dtfetch.Rows[0]["YearBuilt"].ToString();
            lblcounty.Text = dtfetch.Rows[0]["CountyName"].ToString();
            lblcity.Text = dtfetch.Rows[0]["City"].ToString();
            lblstate.Text = dtfetch.Rows[0]["StateCode"].ToString();
            lblzipcode.Text = dtfetch.Rows[0]["Zip"].ToString();
            lblbrieflegal.Text = dtfetch.Rows[0]["BriefLegal"].ToString();
            lblstreetaddress1.Text = dtfetch.Rows[0]["StreetAddress1"].ToString();
            lblclientName.Text = dtfetch.Rows[0]["client_name"].ToString();
            date1.Value = dtfetch.Rows[0]["expecteddate"].ToString();
            date2.Value = dtfetch.Rows[0]["followupdate"].ToString();
        }

        if (lbltransactiontype.Text != "Purchase")
        {
            dd.Visible = false;
            ddlfuturetaxcalc.Visible = false;

            lblbillingstartdate.Visible = false;
            lblbillingenddate.Visible = false;
            txtbillstartdate.Visible = false;
            txtbillenddate.Visible = false;
        }
        else
        {
            dd.Visible = true;
            ddlfuturetaxcalc.Visible = true;
            lblbillingstartdate.Visible = true;
            lblbillingenddate.Visible = true;
            txtbillstartdate.Visible = true;
            txtbillenddate.Visible = true;
        }
    }

    protected void btntaxstatus_Click(object sender, EventArgs e)
    {
        InsertTax_Status();
        FetchTax_Status();
        cleartaxstatus();
    }
    private void InsertTax_Status()
    {
        DataTable OUTPUT = new DataTable();
        int insert = 0;
        insert = gl.insert_taxcertinfo(lblord.Text, ddlordstatus.Text.ToString(), txtComments.Text, DateTime.Now.ToString("MM/dd/yyyy"), SessionHandler.UserName);

        if (insert == 1)
        {
            OUTPUT.Columns.Add("Status");
            OUTPUT.Rows.Add("Data Inserted Successfully");
        }
        else
        {
            OUTPUT.Columns.Add("Status");
            OUTPUT.Rows.Add("Insertion Failed");
        }
    }

    private void cleartaxstatus()
    {
        ddlordstatus.SelectedIndex = 0;
        date1.Value = "";
        date2.Value = "";
    }

    private void FetchTax_Status()
    {
        dtfetch = gl.Fetchtaxcertinfo(lblord.Text);
        if (dtfetch.Rows.Count > 0)
        {
            GvTaxStatus.Visible = true;
            GvTaxStatus.DataSource = dtfetch;
            GvTaxStatus.DataBind();
        }
    }

    protected void GvTaxStatus_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GvTaxStatus.EditIndex = e.NewEditIndex;
    }

    protected void GvTaxStatus_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GvTaxStatus.EditIndex = -1;
        FetchTax_Status();

    }

    protected void GvTaxStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string GVCOMMAND = e.CommandName.ToLower();

        if (GVCOMMAND == "edit")
        {
            string Item_ID = (e.CommandArgument).ToString();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rid = row.RowIndex;
            date1.Value = GvTaxStatus.Rows[rid].Cells[2].Text.Trim().Replace("&nbsp;", "");
            date2.Value = GvTaxStatus.Rows[rid].Cells[3].Text.Trim().Replace("&nbsp;", "");
            LinkButton lnkedit = (LinkButton)GvTaxStatus.Rows[rid].FindControl("LnkEdit");
            lnkedit.CommandName = "Update";
            lnkedit.ToolTip = "Update";
            LinkButton lnkcancel = (LinkButton)GvTaxStatus.Rows[rid].FindControl("LnkCancel");
            lnkcancel.Visible = true;
            lnkedit.CssClass = "glyphicon glyphicon-ok";
            lnkedit.OnClientClick = "javascript : return confirm('Are you sure, want to update this Row?');";
        }
        else if (GVCOMMAND == "update")
        {
            string Item_ID = (e.CommandArgument).ToString();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rid = row.RowIndex;
            LinkButton lnkedit = (LinkButton)GvTaxStatus.Rows[rid].FindControl("LnkEdit");
            lnkedit.CommandName = "Edit";
            lnkedit.CssClass = "glyphicon glyphicon-edit";
            lnkedit.ToolTip = "Edit";
            LinkButton lnkcancel = (LinkButton)GvTaxStatus.Rows[rid].FindControl("LnkCancel");
            lnkcancel.Visible = false;
            int result = gl.updatetaxstatus("", date1.Value, date2.Value, "");
            FetchTax_Status();
        }
        else if (GVCOMMAND == "delete")
        {
            string Item_ID = (e.CommandArgument).ToString();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rid = row.RowIndex;
            int result = gl.DeleteGrid(lblord.Text);
            FetchTax_Status();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("ShowMessageSucess('");
            sb.Append("')};");
            sb.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", sb.ToString());
        }
    }


    private void fetchtaxparcel()
    {
        DataTable dtfetch = new DataTable();
        dtfetch = gl.Fetchtaxparcel(lblord.Text);
        gvTaxParcel.DataSource = dtfetch;
        gvTaxParcel.DataBind();
    }

    protected void btntaxparcel_Click(object sender, EventArgs e)
    {
        gvTax.Visible = true;
        gvTaxParcel.Visible = true;
        taxPar.Visible = true;
        GvTaxStatus.Visible = true;
        TaxStatus.Visible = true;
        PnlTaxStatus.Visible = true;
        DataTable OUTPUT = new DataTable();
        int insert = 0;
        string query = "";

        query = "select orderno,taxid,taxyear,endyear from tbl_taxparcel where taxid = '" + txtdrop.Value + "' and status = 'M' ";
        DataSet ds = gl.ExecuteQuery(query);

        if (txtdrop.Value != "" || txtTaxYear.Text != "" || txtEndYear.Text != "")
        {
            if (ds.Tables[0].Rows.Count == 0)
            {
                if (chkEst.Checked == true && chkTBD.Checked == false)
                {
                    insert = gl.insert_taxparcel(lblord.Text, txtdrop.Value, txtTaxYear.Text + "EST", txtEndYear.Text, "M");
                }
                else if (chkEst.Checked == false && chkTBD.Checked == false)
                {
                    if (txtdrop.Value != "--Select--")
                    {
                        insert = gl.insert_taxparcel(lblord.Text, txtdrop.Value, txtTaxYear.Text, txtEndYear.Text, "M");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Enter TaxId Number')", true);
                        return;
                    }
                }
                else if (chkTBD.Checked == true && chkEst.Checked == false)
                {
                    query = "select orderno,taxid,taxyear,endyear from tbl_taxparcel where orderno = '" + lblord.Text + "' and taxid = 'TBD' and (status = 'M' or status = 'C')";
                    DataSet ds1 = gl.ExecuteQuery(query);

                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        insert = gl.insert_taxparcel(lblord.Text, "TBD", txtTaxYear.Text, txtEndYear.Text, "M");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('TaxId Number Already Exists')", true);
                        return;
                    }
                }
                else if (chkTBD.Checked == true && chkEst.Checked == true)
                {
                    query = "select orderno,taxid,taxyear,endyear from tbl_taxparcel where orderno = '" + lblord.Text + "' and taxid = 'TBD' and (status = 'M' or status = 'C')";
                    DataSet ds1 = gl.ExecuteQuery(query);

                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        insert = gl.insert_taxparcel(lblord.Text, "TBD", txtTaxYear.Text + "EST", txtEndYear.Text, "M");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('TaxId Number Already Exists')", true);
                        return;
                    }
                }
                fetchtaxparcel();
                fetchtaxparceldetails();
                txtdrop.Value = "";
                txtTaxYear.Text = "";
                txtEndYear.Text = "";
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('TaxId Number Already Exists')", true);
                return;
            }
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }


    protected void MdlOrderStatus_Click(object sender, EventArgs e)
    {
        if (ddlordstatus.SelectedIndex != 0)
        {
            InsertTax_Status();
            FetchTax_Status();
            ddlordstatus.SelectedIndex = 0;
            txtComments.Text = "";
            lbltaxcerterror.Text = "";
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }
    protected void btnEditDates_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);

        date1.Attributes["disabled"] = "disabled";
        date2.Attributes["disabled"] = "disabled";
        btnTaxOrderStatus.Visible = false;
        btneditdates.Visible = false;
        btnsavedates.Visible = true;
        btncanceldates.Visible = true;
    }
    protected void btnEditDatesSave_Click(object sender, EventArgs e)
    {
        //btneditdates.Visible = true;
        //btnsavedates.Visible = false;
        //btncanceldates.Visible = false;
        //btnTaxOrderStatus.Visible = true;
        //date1.Visible = true;
        //txtdate2.Visible = true;

        if (lblord.Text != "")
        {
            int update = 0;
            update = gl.updatedate(lblord.Text, date1.Value, date2.Value);
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }

    protected void btnExemptionAdd_Click(object sender, EventArgs e)
    {
        int insertexe = 0;
        if (LblTaxId1.Text == "" && LblAgencyId1.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Select Tax Parcel ID And Agencies ID')", true);
            return;
        }
        else
        {
            insertexe = gl.insert_Exemptions(lblord.Text, LblTaxId1.Text.ToString(), LblAgencyId1.Text.ToString(), txtexetype.Text, txtexeamount.Text, txtTaxType.Text);
            if (insertexe == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                string update = "";
                update = "update tbl_taxauthorities2 set IsExemption = '" + txtexemption.Text + "' where Orderno = '" + lblord.Text + "' and TaxId = '" + LblTaxId1.Text + "' and AgencyId = '" + LblAgencyId1.Text + "'";
                gl.ExecuteSPNonQuery(update);
                fetchexemptionsAll();
                tblExestatus.Visible = true;
                gvExemption.Visible = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Saved Successfully')", true);
                tblExestatus.Focus();
                clearexemptionfields();
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Saved')", true);
                fetchexemptionsAll();
                tblExestatus.Visible = true;
                gvExemption.Visible = true;
                return;
            }
        }
    }

    protected void gvExemption_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            fetchexemptionsAll();
        }
    }
    protected void gvExemption_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ExemptionAdd.Enabled = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        gvExemption.EditIndex = -1;
        fetchexemptionsAll();
        clearexemptionfields();
    }
    protected void gvExemption_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            ExemptionAdd.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            gvExemption.EditIndex = e.NewEditIndex;
            fetchexemptionsAll();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void loadgridexemptions()
    {
        string query = "";

        query = "select Id,taxid,agencyid,exemptiontype,exemptionamount from tbl_exemption_taxauthority";
        DataSet ds = gl.ExecuteQuery(query);
        gvExemption.DataSource = ds.Tables[0];
        gvExemption.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {

        }
        if (ds.Tables[0].Rows.Count == 0)
        {

        }
    }
    protected void gvExemption_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            ExemptionAdd.Enabled = false;
            var strValue1 = gvExemption.Rows[e.RowIndex].Cells[0].Text.Trim().Replace("&nbsp;", "");
            if (LblTaxId1.Text == "" && LblAgencyId1.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();;", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Select Tax Parcel ID And Agencies ID')", true);
                return;
            }
            else
            {
                int result = gl.update_Exemptions(strValue1.ToString(), lblord.Text, LblTaxId1.Text.ToString(), LblAgencyId1.Text.ToString(), txtexetype.Text, txtexeamount.Text);
                if (result == 1)
                {
                    ExemptionAdd.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                    gvExemption.EditIndex = -1;
                    fetchexemptionsAll();
                    clearexemptionfields();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();;", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Updated')", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btngvExemption_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string GVCommand = e.CommandName;

        if (GVCommand == "Edit")
        {
            string Item_ID = (e.CommandArgument).ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            gvExemption.Visible = true;
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rid = row.RowIndex;

            DataTable fetchdeliquent = new DataTable();
            string query = "";
            query = "select * from tbl_exemption_taxauthority where Id = '" + row.Cells[0].Text.Trim() + "' and orderno = '" + lblord.Text + "' and taxid = '" + LblTaxId1.Text + "' and agencyid = '" + LblAgencyId1.Text + "'";
            DataSet ds = gl.ExecuteQuery(query);
            gvExemption.DataSource = ds.Tables[0];
            gvExemption.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtexetype.Text = ds.Tables[0].Rows[0]["exemptiontype"].ToString();
                txtexeamount.Text = ds.Tables[0].Rows[0]["exemptionamount"].ToString();

                ExemptionAdd.Enabled = false;
            }
        }

        if (GVCommand == "DeleteExemption")
        {
            string Item_ID = (e.CommandArgument).ToString();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int result = gl.DeleteGridExemption(row.Cells[0].Text.Trim());
            if (result == 1)
            {
                ExemptionAdd.Enabled = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                fetchexemptionsAll();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully')", true);
                return;
            }
            else
            {
                ExemptionAdd.Enabled = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record Not Deleted')", true);
                fetchexemptionsAll();
                return;
            }
        }
    }

    protected void btnSpecialAdd_Click(object sender, EventArgs e)
    {
        int insertspecial = 0;

        if (LblTaxId1.Text == "" && LblAgencyId1.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Select Tax Parcel ID And Agencies ID')", true);
            return;
        }
        else
        {
            insertspecial = gl.insert_SpecialAssessment(lblord.Text, LblTaxId1.Text.ToString(), LblAgencyId1.Text.ToString(), txtdescription.Text, txtspecialassno.Text, txtnoinstall.Text, txtinstallpaid.Text, txtInstallRemain.Text, txtduedate.Text, txtamountspecial.Text, txtsperembal.Text, txtspecdate.Text, txtspecperdiem.Text, txtspecpayee.Text, txtspeccomments.Text, txtTaxType.Text);
            if (insertspecial == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Saved Successfully')", true);

                string update = "";
                update = "update tbl_taxauthorities2 set IsSpecial = '" + SecialAssmnt.Text + "' where Orderno = '" + lblord.Text + "' and TaxId = '" + LblTaxId1.Text + "' and AgencyId = '" + LblAgencyId1.Text + "'";
                gl.ExecuteSPNonQuery(update);
                fetchspecialAll();
                tblSpecialstatus.Visible = true;
                gvSpecial.Visible = true;
                clearSpecialAssementfields();
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Saved')", true);
                fetchspecialAll();
                tblSpecialstatus.Visible = true;
                gvSpecial.Visible = true;
                return;
            }
        }
    }

    protected void gvSpecialAssessment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        SpecialAdd.Enabled = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        gvSpecial.EditIndex = -1;
        fetchspecialAll();
        clearSpecialAssementfields();
    }
    protected void gvSpecialAssessment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            tblSpecialstatus.Visible = true;
            gvSpecial.EditIndex = e.NewEditIndex;
            fetchspecialAll();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvSpecialAssessment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (LblTaxId1.Text.ToString() == "" && LblAgencyId1.Text.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Tax Id and Agency Id Not Found')", true);
                return;
            }

            if (LblTaxId1.Text.ToString() != "" || LblAgencyId1.Text.ToString() != "")
            {
                var strValue = gvSpecial.Rows[e.RowIndex].Cells[0].Text.ToString().Trim().Replace("&nbsp;", "");
                int result = gl.update_SpecialAssessment(Convert.ToInt32(strValue), LblTaxId1.Text.ToString(), LblAgencyId1.Text.ToString(), txtdescription.Text.ToString(), txtspecialassno.Text, txtnoinstall.Text, txtinstallpaid.Text, txtamountspecial.Text);
                if (result == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                    gvSpecial.EditIndex = -1;
                    fetchspecialAll();
                    clearSpecialAssementfields();
                    SpecialAdd.Enabled = true;
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Updated')", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void loadgridspecialassessment()
    {
        string query = "";
        query = "select Id,taxid,agencyid,description,specialassessmentno,noofinstallment,installmentpaid,amount from tbl_specialassessment_authority";
        DataSet ds = gl.ExecuteQuery(query);
        gvSpecial.DataSource = ds.Tables[0];
        gvSpecial.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {

        }
        if (ds.Tables[0].Rows.Count == 0)
        {

        }
    }

    protected void btnSpecialAssessment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string GVCommand = e.CommandName.ToString();

        if (GVCommand == "Edit")
        {
            string Item_ID = (e.CommandArgument).ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSpecial.Rows[rowIndex];

            string query = "";
            query = "select * from tbl_specialassessment_authority where Id = '" + row.Cells[0].Text.Trim() + "' and orderno = '" + lblord.Text + "' and taxid = '" + LblTaxId1.Text + "' and agencyid = '" + LblAgencyId1.Text + "'";
            DataSet ds = gl.ExecuteQuery(query);
            gvSpecial.DataSource = ds.Tables[0];
            gvSpecial.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtdescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                txtspecialassno.Text = ds.Tables[0].Rows[0]["specialassessmentno"].ToString();
                txtnoinstall.Text = ds.Tables[0].Rows[0]["noofinstallment"].ToString();
                txtinstallpaid.Text = ds.Tables[0].Rows[0]["installmentpaid"].ToString();
                txtInstallRemain.Text = ds.Tables[0].Rows[0]["InstallmentsRemaining"].ToString();
                txtduedate.Text = ds.Tables[0].Rows[0]["DueDate"].ToString();
                txtamountspecial.Text = ds.Tables[0].Rows[0]["amount"].ToString();
                txtsperembal.Text = ds.Tables[0].Rows[0]["RemainingBalance"].ToString();
                txtspecdate.Text = ds.Tables[0].Rows[0]["GoodThroughDate"].ToString();
                txtspecperdiem.Text = ds.Tables[0].Rows[0]["PerDiem"].ToString();
                txtspecpayee.Text = ds.Tables[0].Rows[0]["Payee"].ToString();
                txtspeccomments.Text = ds.Tables[0].Rows[0]["Comments"].ToString();
                SpecialAdd.Enabled = false;

            }
        }

        if (GVCommand == "DeleteSpecial")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            string Item_ID = (e.CommandArgument).ToString();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int result = gl.DeleteGridSpecialAssessment(row.Cells[0].Text.Trim());
            if (result == 1)
            {
                SpecialAdd.Enabled = true;
                fetchspecialAll();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            }
            else
            {
                SpecialAdd.Enabled = true;
                fetchspecialAll();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record Not Deleted')", true);
                return;
            }
        }
    }


    //Delinquent Status

    private void fetchDeliquentStatus()
    {
        DataTable dtfetch = new DataTable();
        dtfetch = gl.FetchDeliquentStatusAll(lblord.Text, LblAgencyId1.Text);
        gvDeliquentStatus.DataSource = dtfetch;
        gvDeliquentStatus.DataBind();
    }

    //Special Assessment
    private void fetchspecialAll()
    {
        DataTable dtfetch = new DataTable();
        dtfetch = gl.FetchSpecialAssessmentAll(lblord.Text, LblTaxID.Text, LblAgencyID.Text);

        if (dtfetch.Rows.Count > 0)
        {
            //txtdescription.Text = dtfetch.Rows[0]["description"].ToString();
            //txtspecialassno.Text = dtfetch.Rows[0]["specialassessmentno"].ToString();
            //txtnoinstall.Text = dtfetch.Rows[0]["noofinstallment"].ToString();
            //txtinstallpaid.Text = dtfetch.Rows[0]["installmentpaid"].ToString();
            //txtInstallRemain.Text = dtfetch.Rows[0]["InstallmentsRemaining"].ToString();
            //txtduedate.Text = dtfetch.Rows[0]["DueDate"].ToString();
            //txtamountspecial.Text = dtfetch.Rows[0]["amount"].ToString();
            //txtsperembal.Text = dtfetch.Rows[0]["RemainingBalance"].ToString();
            //txtspecdate.Text = dtfetch.Rows[0]["GoodThroughDate"].ToString();
            //txtspecperdiem.Text = dtfetch.Rows[0]["PerDiem"].ToString();
            //txtspecpayee.Text = dtfetch.Rows[0]["Payee"].ToString();
        }
        gvSpecial.DataSource = dtfetch;
        gvSpecial.DataBind();
    }

    //Exemption
    private void fetchexemptionsAll()
    {
        DataTable dtfetch = new DataTable();
        dtfetch = gl.FetchExemptionAll(lblord.Text, LblTaxID.Text, LblAgencyID.Text);

        if (dtfetch.Rows.Count > 0)
        {
            //txtexetype.Text = dtfetch.Rows[0]["exemptiontype"].ToString();
            //txtexeamount.Text = dtfetch.Rows[0]["exemptionamount"].ToString();
        }
        gvExemption.DataSource = dtfetch;
        gvExemption.DataBind();
    }
    protected void btnDeliquentStatusAdd_Click(object sender, EventArgs e)
    {
        int insertdelinquent = 0;
        if (LblTaxId1.Text == "" && LblAgencyId1.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Select Tax Parcel ID And Agencies ID')", true);
            return;
        }
        else
        {
            if (txtdeliPayee.Text != "" && txtdelitAddress.Text != "" && txtdelitCity.Text != "" && txtdelitState.Text != "" && txtdelitzip.Text != "" && txtdelitaxyear.Text != "" && txtpayoffamount.Text != "" && txtpayoffgood.Text != "" && txtinitialinstall.Text != "")
            {
                insertdelinquent = gl.insert_DeliquentStatus(lblord.Text, LblTaxId1.Text.ToString(), LblAgencyId1.Text.ToString(), txtdeliPayee.Text, txtdelitAddress.Text, txtdelitCity.Text,
                txtdelitState.Text, txtdelitzip.Text, txtdelitaxyear.Text, txtpayoffamount.Text, txtdelitcomment.InnerText, txtpayoffgood.Text, txtinitialinstall.Text, txtnotapplicable.Text, txtdatetaxsale.Text, txtlastdayred.Text, txtbaseamntdue.Text, txtrolloverdate.Text, txtpenlatyamt.Text, txtpencalfre.SelectedValue, txtaddpenAmnt.Text, txtPerdiem.Text, txtpenamtdue.Text, txtTaxType.Text);
                if (insertdelinquent == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Saved Successfully')", true);


                    string update = "";
                    update = "update tbl_taxauthorities2 set IsDelinquent = '" + txtdeliquent.Text + "' where Orderno = '" + lblord.Text + "' and TaxId = '" + LblTaxId1.Text + "' and AgencyId = '" + LblAgencyId1.Text + "'";
                    gl.ExecuteSPNonQuery(update);

                    fetchDeliquentStatus();
                    tblSpecialstatus.Visible = true;
                    gvSpecial.Visible = true;
                    cleardelinquentfields();
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Saved')", true);
                    fetchDeliquentStatus();
                    tblDeliquentStatus.Visible = true;
                    gvDeliquentStatus.Visible = true;
                    return;
                }
            }
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Please Fill the Fields:Payee,Address,City,State,Zip,Tax Year,PayOff Amount,PayOff Through Date,Initial Installement DueDate')", true);
            //    return;
            //}
        }

    }

    private void cleardelinquentfields()
    {
        txtdeliPayee.Text = "";
        txtdelitAddress.Text = "";
        txtdelitCity.Text = "";
        txtdelitState.Text = "";
        txtdelitzip.Text = "";
        txtdelitaxyear.Text = "";
        txtpayoffamount.Text = "";
        txtdelitcomment.InnerText = "";
        txtpayoffgood.Text = "";
        txtinitialinstall.Text = "";
        txtnotapplicable.SelectedIndex = 0;
        txtdatetaxsale.Text = "";
        txtlastdayred.Text = "";
        txtspecialassno.Text = "";
        txtbaseamntdue.Text = "";
        txtrolloverdate.Text = "";
        txtpenlatyamt.Text = "";
        txtpencalfre.SelectedValue = "--Select--";
        txtaddpenAmnt.Text = "";
        txtPerdiem.Text = "";
        txtpenamtdue.Text = "";
    }

    private void clearexemptionfields()
    {
        txtexetype.SelectedIndex = 0;
        txtexeamount.Text = "";
    }

    private void clearSpecialAssementfields()
    {
        txtdescription.Text = "";
        txtspecialassno.Text = "";
        txtnoinstall.Text = "";
        txtinstallpaid.Text = "";
        txtInstallRemain.Text = "";
        txtduedate.Text = "";
        txtamountspecial.Text = "";
        txtsperembal.Text = "";
        txtspecdate.Text = "";
        txtspecperdiem.Text = "";
        txtspecpayee.Text = "";
        txtspeccomments.Text = "";
    }

    private void clearPriorDeliqfields()
    {
        txtpriodeli.Text = "";
        txtpriorigamtdue.Text = "";
        txtprideliqdate.Text = "";
        txtpriamtpaid.Text = "";
        txtprideliqcommts.Text = "";
    }
    protected void gvDeliquentStatus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DeliquentStatusAdd.Enabled = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        gvDeliquentStatus.EditIndex = -1;
        fetchDeliquentStatus();
        txtdeliPayee.Text = "";
        txtdelitAddress.Text = "";
        txtdelitCity.Text = "";
        txtdelitState.Text = "";
        txtdelitzip.Text = "";
        txtdelitaxyear.Text = "";
        txtpayoffamount.Text = "";
        txtdelitcomment.InnerText = "";
        txtpayoffgood.Text = "";
        txtinitialinstall.Text = "";
        txtnotapplicable.SelectedIndex = 0;
        txtdatetaxsale.Text = "";
        txtlastdayred.Text = "";
        txtspecialassno.Text = "";
        txtbaseamntdue.Text = "";
        txtrolloverdate.Text = "";
        txtpenlatyamt.Text = "";
        txtpencalfre.SelectedValue = "--Select--";
        txtaddpenAmnt.Text = "";
        txtPerdiem.Text = "";
        txtpenamtdue.Text = "";
    }

    protected void gvDeliquentStatus_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            //deliexemspecial.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            tblDeliquentStatus.Visible = true;
            gvDeliquentStatus.EditIndex = e.NewEditIndex;
            fetchDeliquentStatus();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvDeliquentStatus_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (LblTaxId1.Text.ToString() == "" && LblAgencyId1.Text.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Select Tax Parcel ID And Agencies ID')", true);
                return;
            }

            if (LblTaxId1.Text.ToString() != "" && LblAgencyId1.Text.ToString() != "")
            {
                var strValue = gvDeliquentStatus.Rows[e.RowIndex].Cells[0].Text.ToString().Trim().Replace("&nbsp;", "");
                int result = gl.update_DeliquentStatus(Convert.ToInt32(strValue), LblTaxId1.Text.ToString(), LblAgencyId1.Text.ToString(),
                  txtdeliPayee.Text, txtdelitAddress.Text, txtdelitCity.Text,
                  txtdelitState.Text, txtdelitzip.Text, txtdelitaxyear.Text, txtpayoffamount.Text, txtdelitcomment.InnerText,
                  txtpayoffgood.Text, txtinitialinstall.Text,
                  txtnotapplicable.Text, txtdatetaxsale.Text, txtlastdayred.Text, txtbaseamntdue.Text, txtrolloverdate.Text, txtpenlatyamt.Text, txtpencalfre.SelectedValue, txtaddpenAmnt.Text, txtPerdiem.Text, txtpenamtdue.Text);
                if (result == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                    gvDeliquentStatus.EditIndex = -1;
                    fetchDeliquentStatus();
                    cleardelinquentfields();
                    DeliquentStatusAdd.Enabled = true;
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Updated')", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Updated')", true);
                return;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void loadgridDeliquentStatus()
    {
        string query = "";
        query = "select Id,taxid,agencyid,description,specialassessmentno,noofinstallment,installmentpaid,amount from tbl_deliquent_input";
        DataSet ds = gl.ExecuteQuery(query);
        gvDeliquentStatus.DataSource = ds.Tables[0];
        gvDeliquentStatus.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {

        }
        if (ds.Tables[0].Rows.Count == 0)
        {

        }
    }

    protected void btnDeliquentStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string GVCommand = e.CommandName.ToString();

        if (GVCommand == "Edit")
        {
            string Item_ID = (e.CommandArgument).ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            DataTable fetchdeliquent = new DataTable();
            string query = "";
            query = "select * from tbl_deliquent where Id = '" + row.Cells[0].Text.Trim() + "' and orderno = '" + lblord.Text + "' and taxid = '" + LblTaxId1.Text + "' and agencyid = '" + LblAgencyId1.Text + "'";
            DataSet ds = gl.ExecuteQuery(query);
            gvDeliquentStatus.DataSource = ds.Tables[0];
            gvDeliquentStatus.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtdeliPayee.Text = ds.Tables[0].Rows[0]["payee"].ToString();
                txtdelitAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                txtdelitCity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                txtdelitState.Text = ds.Tables[0].Rows[0]["state"].ToString();
                txtdelitzip.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                txtdelitaxyear.Text = ds.Tables[0].Rows[0]["deliquenttaxyear"].ToString();
                txtpayoffamount.Text = ds.Tables[0].Rows[0]["payoffamount"].ToString();
                txtdelitcomment.InnerText = ds.Tables[0].Rows[0]["comments"].ToString();
                txtpayoffgood.Text = ds.Tables[0].Rows[0]["goodthuruDate"].ToString();
                txtinitialinstall.Text = ds.Tables[0].Rows[0]["installmentduedate"].ToString();
                txtnotapplicable.Text = ds.Tables[0].Rows[0]["taxsalenotapplicable"].ToString();
                txtdatetaxsale.Text = ds.Tables[0].Rows[0]["dateofTaxsale"].ToString();
                txtlastdayred.Text = ds.Tables[0].Rows[0]["lastdaytoredeem"].ToString();

                txtbaseamntdue.Text = ds.Tables[0].Rows[0]["BaseAmountDue"].ToString();
                txtrolloverdate.Text = ds.Tables[0].Rows[0]["RollOverDate"].ToString();
                txtpenlatyamt.Text = ds.Tables[0].Rows[0]["PenaltyAmount"].ToString();
                txtpencalfre.SelectedValue = ds.Tables[0].Rows[0]["PenaltyAmountFrequency"].ToString();
                txtaddpenAmnt.Text = ds.Tables[0].Rows[0]["AdditionalPenaltyAmount"].ToString();
                txtPerdiem.Text = ds.Tables[0].Rows[0]["PerDiem"].ToString();
                txtpenamtdue.Text = ds.Tables[0].Rows[0]["PenaltyDueDate"].ToString();
                DeliquentStatusAdd.Enabled = false;
            }
        }

        if (GVCommand == "DeleteDelinquent")
        {
            string Item_ID = (e.CommandArgument).ToString();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int result = gl.DeleteGridDeliquentStatus(row.Cells[0].Text.Trim());
            if (result == 1)
            {
                DeliquentStatusAdd.Enabled = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                fetchDeliquentStatus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record Deleted Successfully')", true);
                return;
            }
            else
            {
                DeliquentStatusAdd.Enabled = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                fetchDeliquentStatus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record Not Deleted')", true);
                return;
            }
        }
    }

    protected void ddlfuturetaxcalc_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        PnlTax1.Visible = true;
        DataTable dtfetchauthority = new DataTable();
        dtfetchauthority = gl.FetchTaxAuthorityDetails(lblord.Text, LblTaxID.Text, LblAgencyID.Text, taxagencytype);
        fetchDeliquentStatus();
        fetchexemptionsAll();
        fetchspecialAll();
        PnlTax1.Focus();
        if (ddlfuturetaxcalc.SelectedItem.Text == "Manual")
        {
            btnsavetaxauthorities.Visible = true;
            if (dtfetchauthority.Rows.Count > 0)
            {
                txtmaninstdate1.Value = dtfetchauthority.Rows[0]["Installmentdate1"].ToString();
                if (txtmaninstdate1.Value != "")
                {
                    string inputData = txtmaninstdate1.Value;
                    DateTime maninstdate1 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    maninstdate1 = maninstdate1.AddYears(1);
                    txtmaninstdate1.Value = maninstdate1.ToString("MM/dd/yyyy");
                }

                txtmaninstdate2.Value = dtfetchauthority.Rows[0]["Installmentdate2"].ToString();
                if (txtmaninstdate2.Value != "")
                {
                    string inputData = txtmaninstdate2.Value;
                    DateTime maninstdate2 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    maninstdate2 = maninstdate2.AddYears(1);
                    txtmaninstdate2.Value = maninstdate2.ToString("MM/dd/yyyy");
                }

                txtmaninstdate3.Value = dtfetchauthority.Rows[0]["Installmentdate3"].ToString();
                if (txtmaninstdate3.Value != "")
                {
                    string inputData = txtmaninstdate3.Value;
                    DateTime maninstdate3 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    maninstdate3 = maninstdate3.AddYears(1);
                    txtmaninstdate3.Value = maninstdate3.ToString("MM/dd/yyyy");
                }


                txtmaninstdate4.Value = dtfetchauthority.Rows[0]["Installmentdate4"].ToString();
                if (txtmaninstdate4.Value != "")
                {
                    string inputData = txtmaninstdate4.Value;
                    DateTime maninstdate4 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    maninstdate4 = maninstdate4.AddYears(1);
                    txtmaninstdate4.Value = maninstdate4.ToString("MM/dd/yyyy");
                }



                txtmandeliqdate1.Value = dtfetchauthority.Rows[0]["DelinquentDate1"].ToString();
                if (txtmandeliqdate1.Value != "")
                {
                    string inputData = txtmandeliqdate1.Value;
                    DateTime mandeliqdate1 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    mandeliqdate1 = mandeliqdate1.AddYears(1);
                    txtmandeliqdate1.Value = mandeliqdate1.ToString("MM/dd/yyyy");
                }


                txtmandeliqdate2.Value = dtfetchauthority.Rows[0]["DelinquentDate2"].ToString();
                if (txtmandeliqdate2.Value != "")
                {
                    string inputData = txtmandeliqdate2.Value;
                    DateTime mandeliqdate2 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    mandeliqdate2 = mandeliqdate2.AddYears(1);
                    txtmandeliqdate2.Value = mandeliqdate2.ToString("MM/dd/yyyy");
                }


                txtmandeliqdate3.Value = dtfetchauthority.Rows[0]["DelinquentDate3"].ToString();
                if (txtmandeliqdate3.Value != "")
                {
                    string inputData = txtmandeliqdate3.Value;
                    DateTime mandeliqdate3 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    mandeliqdate3 = mandeliqdate3.AddYears(1);
                    txtmandeliqdate3.Value = mandeliqdate3.ToString("MM/dd/yyyy");
                }


                txtmandeliqdate4.Value = dtfetchauthority.Rows[0]["DelinquentDate4"].ToString();
                if (txtmandeliqdate4.Value != "")
                {
                    string inputData = txtmandeliqdate4.Value;
                    DateTime mandeliqdate4 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    mandeliqdate4 = mandeliqdate4.AddYears(1);
                    txtmandeliqdate4.Value = mandeliqdate4.ToString("MM/dd/yyyy");
                }

                txtmandisdate1.Value = dtfetchauthority.Rows[0]["DiscountDate1"].ToString();
                if (txtmandisdate1.Value != "")
                {
                    string inputData = txtmandisdate1.Value;
                    DateTime mandisdate1 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    mandisdate1 = mandisdate1.AddYears(1);
                    txtmandisdate1.Value = mandisdate1.ToString("MM/dd/yyyy");
                }

                txtmandisdate2.Value = dtfetchauthority.Rows[0]["DiscountDate2"].ToString();
                if (txtmandisdate2.Value != "")
                {
                    string inputData = txtmandisdate2.Value;
                    DateTime mandisdate2 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    mandisdate2 = mandisdate2.AddYears(1);
                    txtmandisdate2.Value = mandisdate2.ToString("MM/dd/yyyy");
                }

                txtmandisdate3.Value = dtfetchauthority.Rows[0]["DiscountDate3"].ToString();
                if (txtmandisdate3.Value != "")
                {
                    string inputData = txtmandisdate3.Value;
                    DateTime mandisdate3 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    mandisdate3 = mandisdate3.AddYears(1);
                    txtmandisdate3.Value = mandisdate3.ToString("MM/dd/yyyy");
                }

                txtmandisdate4.Value = dtfetchauthority.Rows[0]["DiscountDate4"].ToString();
                if (txtmandisdate4.Value != "")
                {
                    string inputData = txtmandisdate4.Value;
                    DateTime mandisdate4 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                    mandisdate4 = mandisdate4.AddYears(1);
                    txtmandisdate4.Value = mandisdate4.ToString("MM/dd/yyyy");
                }
            }
        }

        else if (ddlfuturetaxcalc.SelectedItem.Text == "Same As Above")
        {
            if (dtfetchauthority.Rows.Count > 0)
            {
                instmanamount1.Value = dtfetchauthority.Rows[0]["Instamount1"].ToString();
                instmanamount2.Value = dtfetchauthority.Rows[0]["Instamount2"].ToString();
                instmanamount3.Value = dtfetchauthority.Rows[0]["Instamount3"].ToString();
                instmanamount4.Value = dtfetchauthority.Rows[0]["Instamount4"].ToString();

                instmanamtpaid1.Value = dtfetchauthority.Rows[0]["Instamountpaid1"].ToString();
                instmanamtpaid2.Value = dtfetchauthority.Rows[0]["Instamountpaid2"].ToString();
                instmanamtpaid3.Value = dtfetchauthority.Rows[0]["Instamountpaid3"].ToString();
                instmanamtpaid4.Value = dtfetchauthority.Rows[0]["Instamountpaid4"].ToString();

                ddlmaninstpaiddue1.Value = dtfetchauthority.Rows[0]["InstPaidDue1"].ToString();
                ddlmaninstpaiddue2.Value = dtfetchauthority.Rows[0]["InstPaidDue2"].ToString();
                ddlmaninstpaiddue3.Value = dtfetchauthority.Rows[0]["InstPaidDue3"].ToString();
                ddlmaninstpaiddue4.Value = dtfetchauthority.Rows[0]["InstPaidDue4"].ToString();

                txtmanurembal1.Value = dtfetchauthority.Rows[0]["Remainingbalance1"].ToString();
                txtmanurembal2.Value = dtfetchauthority.Rows[0]["Remainingbalance2"].ToString();
                txtmanurembal3.Value = dtfetchauthority.Rows[0]["Remainingbalance3"].ToString();
                txtmanurembal4.Value = dtfetchauthority.Rows[0]["Remainingbalance4"].ToString();


                txtmandisamount1.Value = dtfetchauthority.Rows[0]["Discountamount1"].ToString();
                txtmandisamount2.Value = dtfetchauthority.Rows[0]["Discountamount2"].ToString();
                txtmandisamount3.Value = dtfetchauthority.Rows[0]["Discountamount3"].ToString();
                txtmandisamount4.Value = dtfetchauthority.Rows[0]["Discountamount4"].ToString();

                chkexrelmanu1.Value = dtfetchauthority.Rows[0]["ExemptRelevy1"].ToString();
                chkexrelmanu2.Value = dtfetchauthority.Rows[0]["ExemptRelevy2"].ToString();
                chkexrelmanu3.Value = dtfetchauthority.Rows[0]["ExemptRelevy3"].ToString();
                chkexrelmanu4.Value = dtfetchauthority.Rows[0]["ExemptRelevy4"].ToString();

                txtmanunextbilldate1.Value = dtfetchauthority.Rows[0]["nextbilldate1"].ToString();
                ddlmanutaxbill.Value = dtfetchauthority.Rows[0]["taxbill"].ToString();
                ddlpayfreqmanu.Value = dtfetchauthority.Rows[0]["paymentfrequency"].ToString();

                txtinstcommentsmanual.Value = dtfetchauthority.Rows[0]["installmentcomments"].ToString();
                txtmanubillstartdate.Value = dtfetchauthority.Rows[0]["BillingStartDate"].ToString();
                txtmanubillenddate.Value = dtfetchauthority.Rows[0]["BillingEndDate"].ToString();



                if (dtfetchauthority.Rows.Count > 0)
                {
                    txtmaninstdate1.Value = dtfetchauthority.Rows[0]["Installmentdate1"].ToString();
                    if (txtmaninstdate1.Value != "")
                    {
                        string inputData = txtmaninstdate1.Value;
                        DateTime maninstdate1 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        maninstdate1 = maninstdate1.AddYears(1);
                        txtmaninstdate1.Value = maninstdate1.ToString("MM/dd/yyyy");
                    }


                    txtmaninstdate2.Value = dtfetchauthority.Rows[0]["Installmentdate2"].ToString();
                    if (txtmaninstdate2.Value != "")
                    {
                        string inputData = txtmaninstdate2.Value;
                        DateTime maninstdate2 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        maninstdate2 = maninstdate2.AddYears(1);
                        txtmaninstdate2.Value = maninstdate2.ToString("MM/dd/yyyy");
                    }


                    txtmaninstdate3.Value = dtfetchauthority.Rows[0]["Installmentdate3"].ToString();
                    if (txtmaninstdate3.Value != "")
                    {
                        string inputData = txtmaninstdate3.Value;
                        DateTime maninstdate3 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        maninstdate3 = maninstdate3.AddYears(1);
                        txtmaninstdate3.Value = maninstdate3.ToString("MM/dd/yyyy");
                    }


                    txtmaninstdate4.Value = dtfetchauthority.Rows[0]["Installmentdate4"].ToString();
                    if (txtmaninstdate4.Value != "")
                    {
                        string inputData = txtmaninstdate4.Value;
                        DateTime maninstdate4 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        maninstdate4 = maninstdate4.AddYears(1);
                        txtmaninstdate4.Value = maninstdate4.ToString("MM/dd/yyyy");
                    }



                    txtmandeliqdate1.Value = dtfetchauthority.Rows[0]["DelinquentDate1"].ToString();
                    if (txtmandeliqdate1.Value != "")
                    {
                        string inputData = txtmandeliqdate1.Value;
                        DateTime mandeliqdate1 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        mandeliqdate1 = mandeliqdate1.AddYears(1);
                        txtmandeliqdate1.Value = mandeliqdate1.ToString("MM/dd/yyyy");
                    }


                    txtmandeliqdate2.Value = dtfetchauthority.Rows[0]["DelinquentDate2"].ToString();
                    if (txtmandeliqdate2.Value != "")
                    {
                        string inputData = txtmandeliqdate2.Value;
                        DateTime mandeliqdate2 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        mandeliqdate2 = mandeliqdate2.AddYears(1);
                        txtmandeliqdate2.Value = mandeliqdate2.ToString("MM/dd/yyyy");
                    }


                    txtmandeliqdate3.Value = dtfetchauthority.Rows[0]["DelinquentDate3"].ToString();
                    if (txtmandeliqdate3.Value != "")
                    {
                        string inputData = txtmandeliqdate3.Value;
                        DateTime mandeliqdate3 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        mandeliqdate3 = mandeliqdate3.AddYears(1);
                        txtmandeliqdate3.Value = mandeliqdate3.ToString("MM/dd/yyyy");
                    }


                    txtmandeliqdate4.Value = dtfetchauthority.Rows[0]["DelinquentDate4"].ToString();
                    if (txtmandeliqdate4.Value != "")
                    {
                        string inputData = txtmandeliqdate4.Value;
                        DateTime mandeliqdate4 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        mandeliqdate4 = mandeliqdate4.AddYears(1);
                        txtmandeliqdate4.Value = mandeliqdate4.ToString("MM/dd/yyyy");
                    }

                    txtmandisdate1.Value = dtfetchauthority.Rows[0]["DiscountDate1"].ToString();
                    if (txtmandisdate1.Value != "")
                    {
                        string inputData = txtmandisdate1.Value;
                        DateTime mandisdate1 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        mandisdate1 = mandisdate1.AddYears(1);
                        txtmandisdate1.Value = mandisdate1.ToString("MM/dd/yyyy");
                    }

                    txtmandisdate2.Value = dtfetchauthority.Rows[0]["DiscountDate2"].ToString();
                    if (txtmandisdate2.Value != "")
                    {
                        string inputData = txtmandisdate2.Value;
                        DateTime mandisdate2 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        mandisdate2 = mandisdate2.AddYears(1);
                        txtmandisdate2.Value = mandisdate2.ToString("MM/dd/yyyy");
                    }

                    txtmandisdate3.Value = dtfetchauthority.Rows[0]["DiscountDate3"].ToString();
                    if (txtmandisdate3.Value != "")
                    {
                        string inputData = txtmandisdate3.Value;
                        DateTime mandisdate3 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        mandisdate3 = mandisdate3.AddYears(1);
                        txtmandisdate3.Value = mandisdate3.ToString("MM/dd/yyyy");
                    }

                    txtmandisdate4.Value = dtfetchauthority.Rows[0]["DiscountDate4"].ToString();
                    if (txtmandisdate4.Value != "")
                    {
                        string inputData = txtmandisdate4.Value;
                        DateTime mandisdate4 = DateTime.ParseExact(inputData, "MM/dd/yyyy", null);
                        mandisdate4 = mandisdate4.AddYears(1);
                        txtmandisdate4.Value = mandisdate4.ToString("MM/dd/yyyy");
                    }
                }
            }
        }
    }

    protected void lnkviewwebsite_Click(object sender, EventArgs e)
    {
        //System.Diagnostics.Process.Start(txtwebsite1.Text);
    }

    protected void lnkwebsite2_Click(object sender, EventArgs e)
    {
        //System.Diagnostics.Process.Start(txtwebsite2.Text);
    }

    protected void lnkwebsite3_Click(object sender, EventArgs e)
    {
        //System.Diagnostics.Process.Start(txtwebsite3.Text);
    }
    protected void logoutreason_Click(object sender, EventArgs e)
    {
        string strlogout = "", strlog = "";
        strlogout = txtreason.Text;
        string strprocess = processtatus.Text;
        gl.Logout_New(strprocess, strlog, strlogout);
        Response.Redirect("STRMICXHome.aspx");
    }

    protected void lnkwebsite_Click(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).Parent.Parent as GridViewRow;
        LinkButton chkBx = (LinkButton)row.FindControl("lnkwebsite");
        string number = chkBx.Text;
        System.Diagnostics.Process.Start(number);
    }


    protected void btnpriordelinquenttax_Click(object sender, EventArgs e)
    {
        int insertprior = 0;

        if (LblTaxId1.Text == "" && LblAgencyId1.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Select Tax Parcel ID And Agencies ID')", true);
            return;
        }
        else
        {
            if (txtpriodeli.Text != "" && txtpriorigamtdue.Text != "" && txtprideliqdate.Text != "" && txtpriamtpaid.Text != "" && txtprideliqcommts.Text != "")
            {
                insertprior = gl.insert_priordelinquent(lblord.Text, LblTaxId1.Text.ToString(), LblAgencyId1.Text.ToString(), txtpriodeli.Text, txtpriorigamtdue.Text, txtprideliqdate.Text, txtpriamtpaid.Text, txtprideliqcommts.Text, txtTaxType.Text);
                if (insertprior == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Saved Successfully')", true);
                    string update = "";
                    update = "update tbl_taxauthorities2 set IsPastDelinquent = '" + pastDeliquent.Text + "' where Orderno = '" + lblord.Text + "' and TaxId = '" + LblTaxId1.Text + "' and AgencyId = '" + LblAgencyId1.Text + "'";
                    gl.ExecuteSPNonQuery(update);
                    fetchAllpriordelinquent();
                    tblSpecialstatus.Visible = true;
                    GrdPriordelinquent.Visible = true;
                    clearPriorDeliqfields();
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Saved')", true);
                    fetchAllpriordelinquent();
                    btnpriordelinquenttax.Visible = true;
                    GrdPriordelinquent.Visible = true;
                    return;
                }
            }
        }
        fetchAllpriordelinquent();
    }


    private void fetchAllpriordelinquent()
    {
        DataTable dtfetch = new DataTable();
        dtfetch = gl.FetchPriorDelinquentAll(lblord.Text, LblTaxID.Text, LblAgencyID.Text);
        GrdPriordelinquent.Visible = true;

        if (dtfetch.Rows.Count > 0)
        {
            //txtpriodeli.Text = dtfetch.Rows[0]["delinquenttaxyear"].ToString();
            //txtpriorigamtdue.Text = dtfetch.Rows[0]["originalamountdue"].ToString();
            //txtprideliqdate.Text = dtfetch.Rows[0]["originaldelinquencydate"].ToString();
            //txtpriamtpaid.Text = dtfetch.Rows[0]["amountpaid"].ToString();
            //txtprideliqcommts.Text = dtfetch.Rows[0]["delinquencycomments"].ToString();
        }

        GrdPriordelinquent.DataSource = dtfetch;
        GrdPriordelinquent.DataBind();
    }



    protected void GrdPriordelinquent_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
            tblPastDeliquent.Visible = true;
            GrdPriordelinquent.EditIndex = e.NewEditIndex;
            fetchAllpriordelinquent();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GrdPriordelinquent_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        btnpriordelinquenttax.Enabled = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        GrdPriordelinquent.EditIndex = -1;
        fetchAllpriordelinquent();
        clearPriorDeliqfields();
    }



    protected void GrdPriordelinquent_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (LblTaxId1.Text.ToString() == "" && LblAgencyId1.Text.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                return;
            }

            if (LblTaxId1.Text.ToString() != "" || LblAgencyId1.Text.ToString() != "")
            {
                if (txtpriodeli.Text != "" && txtpriorigamtdue.Text != "" && txtprideliqdate.Text != "" && txtpriamtpaid.Text != "" && txtprideliqcommts.Text != "")
                {
                    var strValue = GrdPriordelinquent.Rows[e.RowIndex].Cells[0].Text.ToString().Trim().Replace("&nbsp;", "");
                    int result = gl.update_priordelinquent(Convert.ToInt32(strValue), LblTaxId1.Text.ToString(), LblAgencyId1.Text.ToString(), txtpriodeli.Text, txtpriorigamtdue.Text, txtprideliqdate.Text, txtpriamtpaid.Text, txtprideliqcommts.Text);
                    if (result == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                        GrdPriordelinquent.EditIndex = -1;
                        fetchAllpriordelinquent();
                        clearPriorDeliqfields();
                        btnpriordelinquenttax.Enabled = true;
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Updated')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Data Not Updated')", true);
                    return;
                }
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtPastDel();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Please Fill the Fields:Delinquent Tax Year, Original Amount Due, Originally Delinquency Date, Amount Paid, Delinquency Comments')", true);
                return;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GrdPriordelinquent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string GVCommand = e.CommandName.ToString();
        btnpriordelinquenttax.Enabled = false;

        if (GVCommand == "Edit")
        {
            string Item_ID = (e.CommandArgument).ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtPastDel();", true);
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GrdPriordelinquent.Rows[rowIndex];

            string query = "";
            query = "select * from tbl_priordelinquent where Id = '" + row.Cells[0].Text.Trim() + "' and orderno = '" + lblord.Text + "' and taxid = '" + LblTaxId1.Text + "' and agencyid = '" + LblAgencyId1.Text + "'";
            DataSet ds = gl.ExecuteQuery(query);
            GrdPriordelinquent.DataSource = ds.Tables[0];
            GrdPriordelinquent.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtpriodeli.Text = ds.Tables[0].Rows[0]["delinquenttaxyear"].ToString();
                txtpriorigamtdue.Text = ds.Tables[0].Rows[0]["originalamountdue"].ToString();
                txtprideliqdate.Text = ds.Tables[0].Rows[0]["originaldelinquencydate"].ToString();
                txtpriamtpaid.Text = ds.Tables[0].Rows[0]["amountpaid"].ToString();
                txtprideliqcommts.Text = ds.Tables[0].Rows[0]["delinquencycomments"].ToString();
                SpecialAdd.Enabled = false;
            }
        }

        if (GVCommand == "DeleteSpecial")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtPastDel();", true);
            string Item_ID = (e.CommandArgument).ToString();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int result = gl.DeleteGridPriorDelinquent(row.Cells[0].Text.Trim());
            if (result == 1)
            {
                SpecialAdd.Enabled = true;
                fetchAllpriordelinquent();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtPastDel();", true);
            }
            else
            {
                SpecialAdd.Enabled = true;
                fetchAllpriordelinquent();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtPastDel();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record Not Deleted')", true);
                return;
            }
        }
    }

    string OStatus, process = "";
    protected void btnsaverecord_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        process = processtatus.Text;
        OStatus = ddlstatus.Value;


        string query = "";
        string query1 = "";

        query = "select count(AgencyId) as inputcount from tbl_taxauthorities2 where orderno = '" + lblord.Text + "'";
        DataSet inputcou = con.ExecuteQuery(query);

        string icou = inputcou.Tables[0].Rows[0]["inputcount"].ToString();

        query1 = "select count(authoritystatus) as outputcount from tbl_taxauthorities2 where orderno = '" + lblord.Text + "' and authoritystatus = '2' ";
        DataSet outputcou = con.ExecuteQuery(query1);

        string ocou = outputcou.Tables[0].Rows[0]["outputcount"].ToString();

        if (icou == ocou)
        {
            if (process == "KEY")
            {
                UpdateProduction("sp_UpdateKey_User");

                if (id == "12f7tre5")
                {
                    Response.Redirect("STRMICXProduction.aspx?id=" + id);
                }
                else
                {
                    id = "12f7tre5";
                    Response.Redirect("STRMICXProduction.aspx?id=" + id);
                }
            }
            else if (process == "DU")
            {
                UpdateProduction("sp_UpdateDU_User");

                if (id == "12f7tre5")
                {
                    Response.Redirect("STRMICXProduction.aspx?id=" + id);
                }
                else
                {
                    Response.Redirect("STRMICXProduction.aspx?id=" + id);
                }
            }
            else if (process == "QC" || process == "INPROCESS" || process == "PARCELID" || process == "ONHOLD" || process == "MAILAWAY")
            {
                UpdateProduction("sp_UpdateQC_User");

                if (id == "12f7tre5")
                {
                    Response.Redirect("STRMICXProduction.aspx?id=" + id);
                }
                else
                {
                    Response.Redirect("STRMICXProduction.aspx?id=" + id);
                }
            }
            else if (process == "REVIEW")
            {
                UpdateProduction("sp_UpdateReview_New");

                if (id == "12f7tre5")
                {
                    Response.Redirect("STRMICXProduction.aspx?id=" + id);
                }
                else
                {
                    Response.Redirect("STRMICXProduction.aspx?id=" + id);
                }
            }
        }
    }

    private DataSet UpdateProduction(string Procedurename)
    {
        return gl.UpdateOrderStatusNew(Procedurename, lblord.Text, process, lblzipcode.Text, OStatus);
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        hid_Ticker.Value = TimeSpan.Parse(hid_Ticker.Value).Add(new TimeSpan(0, 0, 1)).ToString();
        lit_Timer.Text = hid_Ticker.Value.ToString();
    }
    //madesh
    protected void btnOrders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string GVCommand = e.CommandName.ToString();
        if (GVCommand == "DeleteOrders")
        {
            try
            {
                string Item_ID = (e.CommandArgument).ToString();
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                gl.DeleteGridOrders(row.Cells[0].Text.Trim());
                fetchtaxparcel();
                PnlTax.Visible = false;
                PnlTax1.Visible = false;
            }
            catch(Exception ex)
            {                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Record Not Deleted')", true);
                return;
            }
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }


    protected void paymentfreq(string payemntfrequency)
    {
        if (payemntfrequency == "Annual" || payemntfrequency == "1")
        {
            SetTaxBillValue(delinq1.Value);
            instamount2.Attributes.Add("disabled", "false");
            instamountpaid2.Attributes.Add("disabled", "false");
            instpaiddue2.Attributes.Add("disabled", "false");
            remainingbalance2.Attributes.Add("disabled", "false");
            instdate2.Attributes.Add("disabled", "false");
            delinq2.Attributes.Add("disabled", "false");
            discamt2.Attributes.Add("disabled", "false");
            discdate2.Attributes.Add("disabled", "false");
            exemptrelevy2.Attributes.Add("disabled", "false");

            instamount3.Attributes.Add("disabled", "false");
            instamountpaid3.Attributes.Add("disabled", "false");
            instpaiddue3.Attributes.Add("disabled", "false");
            remainingbalance3.Attributes.Add("disabled", "false");
            instdate3.Attributes.Add("disabled", "false");
            delinq3.Attributes.Add("disabled", "false");
            discamt3.Attributes.Add("disabled", "false");
            discdate3.Attributes.Add("disabled", "false");
            exemptrelevy3.Attributes.Add("disabled", "false");

            instamount4.Attributes.Add("disabled", "false");
            instamountpaid4.Attributes.Add("disabled", "false");
            instpaiddue4.Attributes.Add("disabled", "false");
            remainingbalance4.Attributes.Add("disabled", "false");
            instdate4.Attributes.Add("disabled", "false");
            delinq4.Attributes.Add("disabled", "false");
            discamt4.Attributes.Add("disabled", "false");
            discdate4.Attributes.Add("disabled", "false");
            exemptrelevy4.Attributes.Add("disabled", "false");
        }

        if (payemntfrequency == "Semi-Annual" || payemntfrequency == "2")
        {
            SetTaxBillValue(delinq2.Value);
            instamount3.Attributes.Add("disabled", "false");
            instamountpaid3.Attributes.Add("disabled", "false");
            instpaiddue3.Attributes.Add("disabled", "false");
            remainingbalance3.Attributes.Add("disabled", "false");
            instdate3.Attributes.Add("disabled", "false");
            delinq3.Attributes.Add("disabled", "false");
            discamt3.Attributes.Add("disabled", "false");
            discdate3.Attributes.Add("disabled", "false");
            exemptrelevy3.Attributes.Add("disabled", "false");

            instamount4.Attributes.Add("disabled", "false");
            instamountpaid4.Attributes.Add("disabled", "false");
            instpaiddue4.Attributes.Add("disabled", "false");
            remainingbalance4.Attributes.Add("disabled", "false");
            instdate4.Attributes.Add("disabled", "false");
            delinq4.Attributes.Add("disabled", "false");
            discamt4.Attributes.Add("disabled", "false");
            discdate4.Attributes.Add("disabled", "false");
            exemptrelevy4.Attributes.Add("disabled", "false");
        }

        if (payemntfrequency == "Quarterly" || payemntfrequency == "4")
        {
            SetTaxBillValue(delinq4.Value);
            instamount4.Attributes.Add("disabled", "false");
            instamountpaid4.Attributes.Add("disabled", "false");
            instpaiddue4.Attributes.Add("disabled", "false");
            remainingbalance4.Attributes.Add("disabled", "false");
            instdate4.Attributes.Add("disabled", "false");
            delinq4.Attributes.Add("disabled", "false");
            discamt4.Attributes.Add("disabled", "false");
            discdate4.Attributes.Add("disabled", "false");
            exemptrelevy4.Attributes.Add("disabled", "false");
        }


    }

    protected void SetTaxBillValue(string delinquentDate)
    {
        if (delinquentDate != "")
        {
            string test = delinquentDate;
            DateTime dt1 = DateTime.Now;
            string currentdate = dt1.ToShortDateString();

            if (Convert.ToDateTime(test) > Convert.ToDateTime(currentdate))
            {
                taxbill.SelectedIndex = 1;
            }
            else
            {
                taxbill.SelectedIndex = 2;
            }
            //else if (Convert.ToDateTime(test) > Convert.ToDateTime(currentdate))
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", "alert('Delnquent date should not be less than current date')", true);
            //    return;
            //}

            //taxbill.Disabled = true;
        }
    }

    protected void btncanceldates_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);

        date1.Attributes["disabled"] = "disabled";
        date2.Attributes["disabled"] = "disabled";

        btneditdates.Visible = true;
        btnTaxOrderStatus.Visible = true;
        btnsavedates.Visible = false;
        btncanceldates.Visible = false;
    }


    protected void btntaxtypeupdate_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        int update;
        string taxid = "", agencyid = "", id = "";
        if (Session["Ttaxid"] != null)
        {
            taxid = Session["Ttaxid"].ToString();
        }

        if (Session["Tagencyid"] != null)
        {
            agencyid = Session["Tagencyid"].ToString();
        }

        if (Session["Tid"] != null)
        {
            id = Session["Tid"].ToString();
        }
        string mdftaxtype = drotxttype.SelectedValue;


        string query = "";
        query = "select TaxAgencyType from tbl_taxauthorities2 where orderno = '" + lblord.Text + "' and TaxId = '" + taxid + "' and TaxAgencyType = '" + mdftaxtype + "'";
        DataSet ds = gl.ExecuteQuery(query);

        if (ds.Tables[0].Rows.Count == 0)
        {
            update = gl.updatetaxtypedetails(mdftaxtype, id);
        }
        txtTaxType.Text = mdftaxtype;
        fetchtaxparcel();
    }

    protected void lnkAgnecy_Click(object sender, EventArgs e)
    {
        GridView gvwnested = (GridView)gvTaxParcel.Rows[0].Cells[1].FindControl("gvOrders");

        GridViewRow Gv2Row = (GridViewRow)((LinkButton)sender).NamingContainer;
        GridView Childgrid = (GridView)(Gv2Row.Parent.Parent);

        GridViewRow Gv1Row = (GridViewRow)(Childgrid.NamingContainer);
        int b = Gv1Row.RowIndex;

        LinkButton sst = (LinkButton)Childgrid.Rows[0].Cells[0].FindControl("lnkOrder");
        Session["Tagencyid"] = sst.Text;
        LinkButton lb = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lb.NamingContainer;
        int index;
        DataTable dtfetchauthority = new DataTable();
        if (row != null)
        {
            index = row.RowIndex;
            Session["Ttaxid"] = Server.HtmlDecode(row.Cells[6].Text.Trim());
            Session["Tid"] = Server.HtmlDecode(row.Cells[0].Text.Trim());
            taxagencytype = Server.HtmlDecode(row.Cells[3].Text.Trim());
        }
        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "TaxtypeModal();", true);
        PanelTaxtype.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }


    protected void btndelidelinquent_Click(object sender, EventArgs e)
    {
        string delete = "";
        delete = "delete from tbl_deliquent where Orderno = '" + lblord.Text + "' and TaxId = '" + LblTaxId1.Text + "' and AgencyId = '" + LblAgencyId1.Text + "'";
        gl.ExecuteSPNonQuery(delete);
        string update = "";
        update = "update tbl_taxauthorities2 set IsDelinquent = '" + txtdeliquent.Text + "' where Orderno = '" + lblord.Text + "' and TaxId = '" + LblTaxId1.Text + "' and AgencyId = '" + LblAgencyId1.Text + "'";
        gl.ExecuteSPNonQuery(update);
        fetchDeliquentStatus();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }

    protected void btnexemption_Click(object sender, EventArgs e)
    {
        string delete = "";
        delete = "delete from tbl_exemption_taxauthority where orderno = '" + lblord.Text + "' and taxid = '" + LblTaxId1.Text + "' and agencyid = '" + LblAgencyId1.Text + "'";
        gl.ExecuteSPNonQuery(delete);
        string update = "";
        update = "update tbl_taxauthorities2 set IsExemption = '" + txtexemption.Text + "' where Orderno = '" + lblord.Text + "' and TaxId = '" + LblTaxId1.Text + "' and AgencyId = '" + LblAgencyId1.Text + "'";
        gl.ExecuteSPNonQuery(update);
        fetchexemptionsAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }

    protected void btnspecdet_Click(object sender, EventArgs e)
    {
        string delete = "";
        delete = "delete from tbl_specialassessment_authority where orderno = '" + lblord.Text + "' and taxid = '" + LblTaxId1.Text + "' and agencyid = '" + LblAgencyId1.Text + "'";
        gl.ExecuteSPNonQuery(delete);
        string update = "";
        update = "update tbl_taxauthorities2 set IsSpecial = '" + SecialAssmnt.Text + "' where Orderno = '" + lblord.Text + "' and TaxId = '" + LblTaxId1.Text + "' and AgencyId = '" + LblAgencyId1.Text + "'";
        gl.ExecuteSPNonQuery(update);
        fetchspecialAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }

    protected void btnprideliqstatus_Click(object sender, EventArgs e)
    {
        string delete = "";
        delete = "delete from tbl_priordelinquent where orderno = '" + lblord.Text + "' and taxid = '" + LblTaxId1.Text + "' and agencyid = '" + LblAgencyId1.Text + "'";
        gl.ExecuteSPNonQuery(delete);
        string update = "";
        update = "update tbl_taxauthorities2 set IsPastDelinquent = '" + SecialAssmnt.Text + "' where Orderno = '" + lblord.Text + "' and TaxId = '" + LblTaxId1.Text + "' and AgencyId = '" + LblAgencyId1.Text + "'";
        gl.ExecuteSPNonQuery(update);
        fetchspecialAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
    }

    protected void btnaddnotes_Click(object sender, EventArgs e)
    {
        if (txtnotes.InnerText != "")
        {
            InsertAdd_Notes();
            FetchAdd_Notes();
            txtnotes.InnerText = "";
            GvAddNotes.Focus();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "txtexeSpecial();", true);
        }
    }

    private void InsertAdd_Notes()
    {
        DataTable OUTPUT1 = new DataTable();
        int insert1 = 0;
        insert1 = gl.insert_Addnotes(lblord.Text, txtnotes.InnerText.ToString(), "General Note", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt"), SessionHandler.UserName);

        if (insert1 == 1)
        {
            OUTPUT1.Columns.Add("Status");
            OUTPUT1.Rows.Add("Data Inserted Successfully");
        }
        else
        {
            OUTPUT1.Columns.Add("Status");
            OUTPUT1.Rows.Add("Insertion Failed");
        }
    }

    private void FetchAdd_Notes()
    {
        dtfetch = gl.Fetchaddnotes(lblord.Text);
        if (dtfetch.Rows.Count > 0)
        {
            GvAddNotes.Visible = true;
            GvAddNotes.DataSource = dtfetch;
            GvAddNotes.DataBind();
        }
    }
}





