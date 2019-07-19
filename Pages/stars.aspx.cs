using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Net.Http;
using System.Data;
using Newtonsoft.Json;
using System.Web.Services;
using System.ComponentModel;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Xml;
using System.Text;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Globalization;

public partial class Pages_scrap : System.Web.UI.Page
{
    GlobalClass gl = new GlobalClass();
    DBConnection db = new DBConnection();
    MySqlConnection con = new MySqlConnection();
    string ServerConnection = ConfigurationManager.AppSettings["server"].ToString();
    string connetionString = "";
    static string condominiumSearch = "";
    DataSet ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            captcharead.Visible = false;
            divmultiowner.Visible = false;
            BindScrapeList();

        }
    }
    protected void BindScrapeList()
    {
        DataSet ds = new DataSet();
        ds = gl.getscrapeorderlist();
        if (ds.Tables[0].Rows.Count > 0)
        {
            Grid99.DataSource = ds;
            Grid99.DataBind();
        }
    }
    protected void getcountyname()
    {
        try
        {

            DataSet ds = new DataSet();
            connetionString = "server=" + ServerConnection + ";database=demo;uid=root;pwd=Text526$t;";
            string query = "select CountyID from countydetails";
            MySqlConnection cnn = new MySqlConnection(connetionString);
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(query, cnn);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            cnn.Close();
        }
        catch (Exception e)
        {
        }
    }

    public static DataSet get_datas(string ordrid)
    {
        DataSet ds = new DataSet();

        string DbName = "";
        string connetionString = "";

        DbName = ConfigurationManager.AppSettings["DBName"].ToString();
        string ServerConnection = ConfigurationManager.AppSettings["server"].ToString();
        connetionString = "server=" + ServerConnection + ";database=demo;uid=root;pwd=Text526$t;";

        MySqlConnection con = new MySqlConnection(connetionString);
        MySqlCommand cmd = new MySqlCommand("sp_get_scrap_details", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@$OrderNo", ordrid);
        con.Open();
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        da.Fill(ds);

        return ds;
    }


    protected void btnGetRecord_Click(object sender, EventArgs e)
    {
        RunOrderList();
    }

    private void BindMultiParcelList(string orderNum, int count)
    {
        DataSet ds = new DataSet();
        connetionString = "server=" + ServerConnection + ";database=demo;uid=root;pwd=Text526$t;";
        string query = "select PropertyAddress,MailingAddress,ParcelID,OwnerName from multiowner where OrderId='" + orderNum + "'";
        if (count == 0)
        {
            query = "select PropertyAddress,MailingAddress,ParcelID,OwnerName from multiowner where OrderId='" + orderNum + "'";
        }
        else if (count == 1)
        {
            condominiumSearch = "Yes";
            query = "select PropertyAddress,ParcelID from multiowner where OrderId='" + orderNum + "'";
        }
        MySqlConnection cnn = new MySqlConnection(connetionString);
        cnn.Open();
        MySqlCommand cmd = new MySqlCommand(query, cnn);
        MySqlDataAdapter da = new MySqlDataAdapter();
        da.SelectCommand = cmd;
        da.Fill(ds);
        GridView3.DataSource = ds;
        GridView3.DataBind();
        cnn.Close();
    }

    private void MessageBox(string message)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Alert", "alert('" + message + "');", true);
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/scrap.aspx");
    }
    public class UserDetails
    {
        public string PropertyAddress { get; set; }
        public string MailingAddress { get; set; }
        public string ParcelID { get; set; }
        public string OwnerName { get; set; }

    }


    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {

        GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
        CheckBox chkBx = (CheckBox)row.FindControl("chkselect");
        string number = "";
        if (condominiumSearch == "")
        {
            number = row.Cells[3].Text.ToString();
        }
        else if (condominiumSearch == "Yes")
        {
            number = row.Cells[2].Text.ToString();
        }
        row.BackColor = Color.Green;

    }
    protected void scrap(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Home.aspx");
    }
    protected void GridAddressShow_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        TextBox3.Text = "";
        GlobalClass.sDriver.FindElement(By.Id("recaptcha_reload_btn")).SendKeys(Keys.Enter);
        Thread.Sleep(2000);
        var imgId = GlobalClass.sDriver.FindElement(By.Id("recaptcha_challenge_image"));
        GlobalClass.imgURL = imgId.GetAttribute("src");
        string outPath = System.Web.HttpContext.Current.Server.MapPath("~/captcha\\") + GlobalClass.global_parcelNo + ".png";
        WebClient captcha = new WebClient();
        captcha.DownloadFile(GlobalClass.imgURL, outPath);
        Image1.ImageUrl = "~/captcha/" + GlobalClass.global_parcelNo + ".png";
    }
    public string GetCountyId(string state, string county)
    {

        string stateCountyId = "";
        using (MySqlConnection cnn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Scraping"].ConnectionString))
        {
            string query = "SELECT Address_Type,State_County_Id FROM state_county_master where State_Name = '" + state + "' and County_Name='" + county + "'";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(query, cnn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                stateCountyId = dr["State_County_Id"].ToString();
            }
            dr.Close();
            cnn.Close();
        }
        return stateCountyId;
    }
    protected void Button5_Click(object sender, EventArgs e)
    {

        RunOrderList();
        captcharead.Visible = false;

    }
    public void RunOrderList()
    {

        string orderNumber = "", county = "", state = "", ownerName = "", propertyAddress = "";
        string cityName = "", streetNumber = "", direction = "", streetName = "", zipCode = "", streetLine = "", streetType = "", unitnumber = "",postdir="",taddress="";

        foreach (GridViewRow row in Grid99.Rows)
        {
            CheckBox chk = row.Cells[0].Controls[0].FindControl("chkselect") as CheckBox;

            if (chk.Checked)
            {
                row.BackColor = Color.Green;
                orderNumber = row.Cells[2].Text;
                county = row.Cells[3].Text;
                state = row.Cells[4].Text;
                Session["sessioncounty"] = county;
                Session["sessionstate"] = state;
                ownerName = row.Cells[5].Text;
                propertyAddress = row.Cells[6].Text;

                //get county id...
                ds = gl.GetCountyId(state, county);
                string addresstype = ds.Tables[0].Rows[0]["Address_Type"].ToString();
                string countyID = ds.Tables[0].Rows[0]["State_County_Id"].ToString();
                string apiUrl = ds.Tables[0].Rows[0]["service_url"].ToString();

                AddressParser.AddressParser splitAddr = new AddressParser.AddressParser();
                var splitAddrList = splitAddr.ParseAddress(propertyAddress);
                cityName = splitAddrList.City;
                streetNumber = splitAddrList.Number;
                direction = splitAddrList.Predirectional;
                streetName = splitAddrList.Street;
                zipCode = splitAddrList.Zip;
                streetLine = splitAddrList.StreetLine;
                streetType = splitAddrList.Suffix;
                unitnumber = splitAddrList.SecondaryNumber;
                if (streetType != null)
                {
                    if (countyID == "2")
                    {
                        streetType = splitAddrList.Suffix;
                    }
                    else
                    {
                        streetType = gl.ReturnStType(streetType);
                    }
                }
                else
                {
                    streetType = "";
                }
                if (direction == null)
                {
                    direction = "";
                }
                if (unitnumber == null)
                {
                    unitnumber = "";
                }

                chk.Checked = false;
                if (countyID == "12")
                {
                    if (GlobalClass.imgURL != "")
                    {
                        captcharead.Visible = true;
                        string outPath = System.Web.HttpContext.Current.Server.MapPath("~/captcha\\") + GlobalClass.parcelNumber_la.Replace("-", "") + ".png";
                        Image1.ImageUrl = "~/captcha/" + GlobalClass.parcelNumber_la.Replace("-", "") + ".png";
                        GlobalClass.imgURL = "";
                        return;
                    }

                }

                else if (countyID == "23")
                {
                    object input = new
                    {
                        address = streetLine,
                        parcelNumber = "",
                        ownername = "",
                        SearchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        //4 == Multiparcel
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);




                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetWashoeNV(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }


                }
                else if (countyID == "20")
                {
                    string[] sname = streetName.Split(' ');
                    streetName = sname[0].ToString();
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        blockno = "",
                        parcelNumber = "",
                        SearchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = unitnumber

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetWAPierce(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }

                }
                else if (countyID == "40")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "");
                    }
                    propertyAddress = streetNumber + " " + direction + " " + stname;
                    object input = new
                    {
                        address = propertyAddress,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParce = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        //1 == Multiparcel
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetMultnomaOR(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                else if (countyID == "22")
                {
                    object input = new
                    {
                        address = streetLine,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }

                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetGwinnettGA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }
                //river
                else if (countyID == "19")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "");
                    }

                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = stname,
                        sttype = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = unitnumber

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }

                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetWAPierce(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }

                }

                else if (countyID == "34")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address"; propertyAddress = streetNumber + " " + streetName + " " + streetType;
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }

                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetKernCA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }

                }

                else if (countyID == "33")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetSaintLouisMO(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }

                }


                else if (countyID == "30")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        // streetLine.Replace("APT", "").Replace("UNIT", "").Replace("#", "");
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        propertyAddress = streetNumber + " " + streetName;
                        sear_type = "address";
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        ownerName = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetMecklenburgCA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }

                }
                else if (countyID == "32")
                {
                    string[] sname = streetName.Split(' ');
                    streetName = sname[0].ToString();
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        blockno = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = unitnumber
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetDistofColumbiaDC(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                else if (countyID == "29")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetNumber + " " + streetName + " " + streetType + " " + unitnumber;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName + " " + streetType;
                    }


                    object input = new
                    {
                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetDekalbGA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }
                else if (countyID == "31")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR") || stname.Contains(" LN "))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "").Replace(" LN", "");
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = stname,
                        blockno = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetFranklinOH(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }


                //placer
                else if (countyID == "56")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetPlacerCA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //san joaquin
                else if (countyID == "36")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }


                    object input = new
                    {

                        address = propertyAddress,
                        unitNumber = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetSanJoaquinCA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //san francisco
                else if (countyID == "35")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    //propertyAddress = streetNumber + " " + streetName + " " + streetType;
                    object input = new
                    {
                        address = streetLine,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasettitleflex(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //fresno
                else if (countyID == "37")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasettitleflex(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //harrison
                else if (countyID == "41")
                {
                    if (streetName.Contains("HIGHWAY"))
                    {
                        streetName = streetName.Replace("HIGHWAY", "HWY");
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetHarrison(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //bernalillo
                else if (countyID == "54")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directSearch = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetowneraddress(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //stcharles
                else if (countyID == "55")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetStcharles(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //tulsa
                else if (countyID == "57")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = "",
                        parcelNumber = "",
                        searchType = "address",
                        accountnumber = "",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {

                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetTulsa(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //utah
                else if (countyID == "58")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = "",
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        accountnumber = "",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetowneraddress(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //summit
                else if (countyID == "60")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        accountnumber = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetsummit(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //hennepin
                else if (countyID == "49")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = "",
                        sttype = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetHennepin(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //new castle
                else if (countyID == "59")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetNewcastle(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //pinal
                else if (countyID == "61")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        unitNumber = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetpinal(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //marion
                else if (countyID == "76")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        accountNumber = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetmarion(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //shelby
                else if (countyID == "74")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetowneraddress(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //clackamas
                else if (countyID == "73")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = "",
                        direction = direction,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetclackamas(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //east baton rouge
                else if (countyID == "78")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetowneraddress(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //baltimore
                else if (countyID == "72")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        unitNumber = unitnumber
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetbaltimore(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //polk
                else if (countyID == "63")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetpolk(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }
                //charleston
                else if (countyID == "82")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        account = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetaddressowner(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }



                //jeferson

                else if (countyID == "93")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //douglas

                else if (countyID == "83")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {

                        houseno = streetNumber,
                        sname = streetName,
                        ownername = "",
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //Anoka



                else if (countyID == "100")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //stark

                else if (countyID == "101")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        direction = "",
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //cherokee

                else if (countyID == "103")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        direction = direction,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //hillsborough

                else if (countyID == "2")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sttype = streetType,
                        sname = streetName,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //denver

                else if (countyID == "8")
                {

                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //harford

                else if (countyID == "114")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //Yolo


                else if (countyID == "152")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //clayton

                else if (countyID == "141")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }


                //Berkeley


                else if (countyID == "128")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //Newton
                else if (countyID == "158")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //dakota

                else if (countyID == "132")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }


                //ALMadison

                else if (countyID == "116")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        account = "",
                        parcelNumber = "",
                        searchType = "address",
                        ownername = "",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //ARWashington

                else if (countyID == "149")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        account = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //AZMaricopa

                else if (countyID == "13")
                {                   
                    object input = new
                    {
                        address = streetLine,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //CA Alameda

                else if (countyID == "9")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                    TextInfo textInfo = cultureInfo.TextInfo;
                    object input = new
                    {
                        houseno = streetNumber,
                        direction = "",
                        sname = streetName,
                        sttype = unitnumber,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        city = textInfo.ToTitleCase(cityName.ToLower())

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //CA Contracosta

                else if (countyID == "11")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = "",
                        city = cityName,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //CAEldorado


                else if (countyID == "95")
                {
                    object input = new
                    {
                        address = streetLine,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //CAMonterery
                else if (countyID == "86")
                {

                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        state = "CA",
                        county = "Monterey"

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //CASacramento

                else if (countyID == "10")
                {

                    string sear_type = "titleflex";
                    propertyAddress = streetLine;


                    object input = new
                    {
                        address = propertyAddress,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //CASantabarabara

                else if (countyID == "88")
                {
                    propertyAddress = streetNumber + " " + streetName + " " + streetType;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLDuval

                else if (countyID == "4")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {

                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLBroward

                else if (countyID == "3")
                {
                    streetName = Regex.Match(streetName, @"\d+").Value;
                    if (streetName.Any(char.IsDigit))
                    {
                        Regex.Replace(streetName, "[^a-zA-Z]", "");
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLCollier

                else if (countyID == "138")
                {
                    object input = new
                    {

                        address = streetLine,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLManatee
                else if (countyID == "122")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //FLMiamedade

                else if (countyID == "6")
                {
                    if (streetName.ToUpper().Contains("STREET"))
                    {
                        streetName = streetName.Replace("STREET", "");
                    }
                    object input = new
                    {

                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //FLorange

                else if (countyID == "7")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {

                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLOsceola

                else if (countyID == "151")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {

                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLPalmBeach
                else if (countyID == "5")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        streetLine.Replace("APT", "").Replace("UNIT", "").Replace("#", "");
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName + " " + streetType;
                    }


                    object input = new
                    {

                        address = propertyAddress,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                }

                //Polk

                else if (countyID == "129")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {

                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //Pasco

                else if (countyID == "99")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        streetName = streetName.Replace("\r\n", "").Trim();
                        if (streetName.Contains(" DRNEW") || streetName.Contains(" RDNEW") || streetName.Contains(" CIRNEW") || streetName.Contains(" STNEW") || streetName.Contains(" LNNEW") || streetName.Contains(" AVENEW"))
                        {
                            streetName = streetName.Replace(" DRNEW", "").Replace(" RDNEW", "").Replace(" CIRNEW", "").Replace(" STNEW", "").Replace(" LNNEW", "").Replace(" AVENEW", "");
                            propertyAddress = streetNumber + " " + streetName;
                        }
                    }


                    object input = new
                    {
                        houseno = streetNumber,
                        direction = "",
                        sname = streetName,
                        sttype = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLSarasotta

                else if (countyID == "96")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {

                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",



                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLStlucie

                else if (countyID == "112")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        direction = "",
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        unitNumber = unitnumber



                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FlVolusia

                else if (countyID == "80")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR") || stname.Contains(" AVE"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "").Replace(" AVE", "");
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = stname,
                        direction = "",
                        account = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //ILWill

                else if (countyID == "115")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        city = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //INHamilton
                else if (countyID == "139")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {

                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //INMarion
                else if (countyID == "97")
                {
                    propertyAddress = streetNumber + " " + direction + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //MdCarrol
                else if (countyID == "153")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "");
                    }

                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = stname,
                        direction = direction,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = unitnumber

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //NVClark

                else if (countyID == "1")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else

                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //OHhamilton

                else if (countyID == "94")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        unitNumber = unitnumber,
                        orderNumber = orderNumber,

                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //OkCleveland

                else if (countyID == "119")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //ORDeschutes



                else if (countyID == "98")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //Sanlouispoca

                else if (countyID == "125")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //WYLaramine

                else if (countyID == "140")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                if (countyID == "23")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        parcelNumber = "",
                        ownername = "",
                        SearchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        //4 == Multiparcel
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);




                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetWashoeNV(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }


                }
                else if (countyID == "20")
                {
                    string[] sname = streetName.Split(' ');
                    streetName = sname[0].ToString();
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        blockno = "",
                        parcelNumber = "",
                        SearchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = unitnumber

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetWAPierce(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }

                }
                else if (countyID == "40")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "");
                    }
                    propertyAddress = streetNumber + " " + direction + " " + stname;
                    object input = new
                    {
                        address = propertyAddress,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParce = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        //1 == Multiparcel
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetMultnomaOR(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }
                //gwinnet
                else if (countyID == "22")
                {
                    object input = new
                    {
                        address = streetLine,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }

                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetGwinnettGA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }
                //river
                else if (countyID == "19")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "");
                    }

                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = stname,
                        sttype = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = unitnumber

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }

                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetWAPierce(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }

                }

                else if (countyID == "34")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address"; propertyAddress = streetNumber + " " + streetName + " " + streetType;
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }

                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetKernCA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }

                }

                else if (countyID == "33")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetSaintLouisMO(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }


                else if (countyID == "30")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        // streetLine.Replace("APT", "").Replace("UNIT", "").Replace("#", "");
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        propertyAddress = streetNumber + " " + streetName;
                        sear_type = "address";
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        ownerName = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetMecklenburgCA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }

                }
                else if (countyID == "32")
                {
                    string[] sname = streetName.Split(' ');
                    streetName = sname[0].ToString();
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        blockno = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = unitnumber
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetDistofColumbiaDC(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }
                //deklab
                else if (countyID == "29")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetNumber + " " + streetName + " " + streetType + " " + unitnumber;
                    }
                    else
                    {
                        if (streetType == "WY")
                        {
                            streetType = "WAY";
                        }
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName + " " + streetType;
                    }


                    object input = new
                    {
                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetDekalbGA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }
                else if (countyID == "31")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR") || stname.Contains(" LN "))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "").Replace(" LN", "");
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = stname,
                        sttype = streetType,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = unitnumber
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetFranklinOH(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }


                //placer
                else if (countyID == "56")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetPlacerCA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //san joaquin
                else if (countyID == "36")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }


                    object input = new
                    {

                        address = propertyAddress,
                        unitNumber = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetSanJoaquinCA(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //san francisco
                else if (countyID == "35")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    //propertyAddress = streetNumber + " " + streetName + " " + streetType;
                    object input = new
                    {
                        address = streetLine,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasettitleflex(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //fresno
                else if (countyID == "37")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasettitleflex(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //harrison
                else if (countyID == "41")
                {
                    if (streetName.Contains("HIGHWAY"))
                    {
                        streetName = streetName.Replace("HIGHWAY", "HWY");
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetHarrison(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //bernalillo
                else if (countyID == "54")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directSearch = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetowneraddress(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //stcharles
                else if (countyID == "55")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetStcharles(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //tulsa
                else if (countyID == "57")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = "",
                        parcelNumber = "",
                        searchType = "address",
                        accountnumber = "",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {

                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetTulsa(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //utah
                else if (countyID == "58")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = "",
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        accountnumber = "",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetowneraddress(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //summit
                else if (countyID == "60")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        accountnumber = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetsummit(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //hennepin
                else if (countyID == "49")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = "",
                        sttype = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetHennepin(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //new castle
                else if (countyID == "59")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetNewcastle(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //pinal
                else if (countyID == "61")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        unitNumber = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetpinal(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //marion
                else if (countyID == "76")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        accountNumber = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetmarion(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //shelby
                else if (countyID == "74")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetowneraddress(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //clackamas
                else if (countyID == "73")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = "",
                        direction = direction,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetclackamas(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //east baton rouge
                else if (countyID == "78")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetowneraddress(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //baltimore
                else if (countyID == "72")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        unitNumber = unitnumber
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetbaltimore(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }

                //polk
                else if (countyID == "63")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetpolk(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }
                //charleston
                else if (countyID == "82")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        account = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                        //IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(json);
                        //DataTable dt = DtConvert.ToDataTable(UserList);
                        //dt = SplitData.SplitDatasetaddressowner(dt);
                        //GridView3.DataSource = dt;
                        //GridView3.DataBind();
                        //divmultiowner.Visible = true;
                        //MessageBox("Multiple Parcels....");
                        //return;
                    }
                }



                //jeferson

                else if (countyID == "93")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //douglas

                else if (countyID == "83")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {

                        houseno = streetNumber,
                        sname = streetName,
                        ownername = "",
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //Anoka



                else if (countyID == "100")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //stark

                else if (countyID == "101")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        direction = "",
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //cherokee

                else if (countyID == "103")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        direction = direction,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //hillsborough

                else if (countyID == "2")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sttype = streetType,
                        sname = streetName,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //denver

                else if (countyID == "8")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //harford

                else if (countyID == "114")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //Yolo


                else if (countyID == "152")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //clayton

                else if (countyID == "141")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }


                //Berkeley


                else if (countyID == "128")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //Newton
                else if (countyID == "158")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //dakota

                else if (countyID == "132")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }


                //ALMadison

                else if (countyID == "116")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        account = "",
                        parcelNumber = "",
                        searchType = "address",
                        ownername = "",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //ARWashington

                else if (countyID == "149")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        account = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //AZMaricopa

                else if (countyID == "13")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        if (direction != "")
                        {
                            propertyAddress = streetNumber + " " + direction + " " + streetName;
                        }
                        else
                        {
                            propertyAddress = streetNumber + " " + streetName;
                        }
                    }

                    object input = new
                    {
                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //CA Alameda

                else if (countyID == "9")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR") || stname.Contains(" AVE"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "").Replace(" AVE", "");
                    }
                    CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                    TextInfo textInfo = cultureInfo.TextInfo;
                    object input = new
                    {
                        houseno = streetNumber,
                        direction = "",
                        sname = stname,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        city = textInfo.ToTitleCase(cityName.ToLower()),
                        unitNumber = unitnumber

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //CA Contracosta

                else if (countyID == "11")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = "",
                        city = cityName,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = direction

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //CAEldorado


                else if (countyID == "95")
                {
                    object input = new
                    {
                        address = streetLine,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //CAMonterery
                else if (countyID == "86")
                {

                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        state = "CA",
                        county = "Monterey"

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //CASacramento

                else if (countyID == "10")
                {

                    string sear_type = "titleflex";
                    propertyAddress = streetLine;


                    object input = new
                    {
                        address = propertyAddress,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //CASantabarabara

                else if (countyID == "88")
                {
                    propertyAddress = streetNumber + " " + streetName + " " + streetType;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLDuval

                else if (countyID == "4")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {

                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLBroward

                else if (countyID == "3")
                {


                    //if (streetName.Any(char.IsDigit))
                    //{
                    //    streetName = Regex.Match(streetName, @"\d+").Value;
                    //}

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                        unitNumber = unitnumber
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLCollier

                else if (countyID == "138")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {

                        address = streetLine,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLManatee
                else if (countyID == "122")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //FLMiamedade

                else if (countyID == "6")
                {
                    if (streetName.ToUpper().Contains("STREET"))
                    {
                        streetName = streetName.Replace("STREET", "");
                    }
                    object input = new
                    {

                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //FLorange

                else if (countyID == "7")
                {

                    object input = new
                    {

                        address = streetLine,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLOsceola

                else if (countyID == "151")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {

                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLPalmBeach
                else if (countyID == "5")
                {
                    string sear_type = "";
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR") || stname.Contains(" AVE") || stname.Contains(" LN") || stname.Contains(" PL"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "").Replace(" AVE", "").Replace(" LN", "").Replace(" PL", "");
                    }
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        streetLine.Replace("APT", "").Replace("UNIT", "").Replace("#", "");
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        if (streetType == "WY")
                        {
                            streetType = "WAY";
                        }
                        if (direction != "")
                        {
                            if (streetType == "LK")
                            {
                                propertyAddress = streetNumber + " " + direction + " " + stname;
                            }
                            else
                            {
                                propertyAddress = streetNumber + " " + direction + " " + stname + " " + streetType;
                            }

                        }
                        else
                        {
                            if (streetType == "LK")
                            {
                                propertyAddress = streetNumber + " " + stname;
                            }
                            else
                            {
                                propertyAddress = streetNumber + " " + stname + " " + streetType;
                            }
                        }
                        sear_type = "address";
                    }


                    object input = new
                    {

                        address = propertyAddress,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                }

                //Polk

                else if (countyID == "129")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {

                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //Pasco

                else if (countyID == "99")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        streetName = streetName.Replace("\r\n", "").Trim();
                        if (streetName.Contains(" DRNEW") || streetName.Contains(" RDNEW") || streetName.Contains(" CIRNEW") || streetName.Contains(" STNEW") || streetName.Contains(" LNNEW") || streetName.Contains(" AVENEW") || streetName.Contains(" CTNEW"))
                        {
                            streetName = streetName.Replace(" DRNEW", "").Replace(" RDNEW", "").Replace(" CIRNEW", "").Replace(" STNEW", "").Replace(" LNNEW", "").Replace(" AVENEW", "").Replace(" CTNEW", "");
                            propertyAddress = streetNumber + " " + streetName;
                        }
                    }


                    object input = new
                    {
                        houseno = streetNumber,
                        direction = "",
                        sname = streetName,
                        sttype = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLSarasotta

                else if (countyID == "96")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {

                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",



                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FLStlucie

                else if (countyID == "112")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        direction = "",
                        sname = streetName,
                        sttype = streetType,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        unitNumber = unitnumber



                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //FlVolusia

                else if (countyID == "80")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR") || stname.Contains(" AVE"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "").Replace(" AVE", "");
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = stname,
                        direction = "",
                        account = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //ILWill

                else if (countyID == "115")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        city = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //INHamilton
                else if (countyID == "139")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        sear_type = "address";
                        propertyAddress = streetNumber + " " + streetName;
                    }

                    object input = new
                    {

                        address = propertyAddress,
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //INMarion
                else if (countyID == "97")
                {
                    propertyAddress = streetNumber + " " + direction + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //MdCarrol
                else if (countyID == "153")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "");
                    }

                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = stname,
                        direction = direction,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = unitnumber

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //NVClark

                else if (countyID == "1")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else

                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //OHhamilton

                else if (countyID == "94")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        unitNumber = unitnumber,
                        orderNumber = orderNumber,

                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //OkCleveland

                else if (countyID == "119")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        account = "",
                        parcelNumber = "",
                        ownername = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }

                //ORDeschutes



                else if (countyID == "98")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        address = propertyAddress,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //Sanlouispoca

                else if (countyID == "125")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        ownername = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                //WYLaramine

                else if (countyID == "140")
                {
                    propertyAddress = streetNumber + " " + streetName;
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed                        
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error                        
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else
                    {
                        string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);

                    }
                }
                // san bernardino
                else if (countyID == "17")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                        city = cityName,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Orange
                else if (countyID == "16")
                {

                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        unitNumber = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }


                //Santa Clara
                else if (countyID == "15")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        assessmentid = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // WA king
                else if (countyID == "18")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        direction = direction
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //NC Forsyth
                else if (countyID == "185")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //KS Johnson
                else if (countyID == "108")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //NC Guilford
                else if (countyID == "159")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //SC spartanburg
                else if (countyID == "111")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = "",
                        accno = "",
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //WV Berkeley
                else if (countyID == "173")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "titleflex";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        housedir = direction,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //NE Sarpy
                else if (countyID == "160")
                {
                    string sear_type = "";                    
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //AR Saline
                else if (countyID == "174")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Dorchester SC
                else if (countyID == "165")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Kings NY
                else if (countyID == "164")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = streetType,
                        city = cityName,
                        unitNumber = unitnumber,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Medina OH
                else if (countyID == "186")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Shasta CA
                else if (countyID == "192")
                {

                    object input = new
                    {
                        houseno = streetNumber,
                        direction = direction,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Larimer CO 
                else if (countyID == "62")
                {
                    string sear_type = "";
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR") || stname.Contains(" AVE") || stname.Contains(" LN") || stname.Contains(" PL") || stname.Contains(" HWY") || stname.Contains(" BLVD"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "").Replace(" AVE", "").Replace(" LN", "").Replace(" PL", "").Replace(" HWY", "").Replace("BLVD", "");
                    }
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = stname,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Jefferson
                else if (countyID == "190")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                else if (countyID == "176")
                {
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        direction = direction,
                        sttype = streetType,
                        unitNumber = unitnumber,
                        account = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Charlotte
                else if (countyID == "178")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //calcasieu
                else if (countyID == "181")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                        city = cityName,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Fayette
                else if (countyID == "189")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Marion FL
                else if (countyID == "193")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        assessmentid = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }


                //Hall
                else if (countyID == "204")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        assessmentid = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //coweta
                else if (countyID == "166")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        assessmentid = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //williamston
                else if (countyID == "38")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        assessmentid = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //knox
                else if (countyID == "196")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        assessmentid = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //stafford
                else if (countyID == "197")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        address = streetLine,
                        assessmentid = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Hall
                else if (countyID == "204")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                        assessmentID = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //St John
                else if (countyID == "175")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Kootenai
                else if (countyID == "200")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        streetno = streetNumber,
                        streetname = streetName,
                        streettype = streetType,
                        direction = direction,
                        city = cityName,
                        unitnumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Lubbock
                else if (countyID == "154")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        streetno = streetNumber,
                        streetname = streetName,
                        streettype = streetType,
                        direction = direction,
                        unitnumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Hidalgo
                else if (countyID == "85")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        streetno = streetNumber,
                        streetname = streetName,
                        streettype = streetType,
                        direction = direction,
                        unitnumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // TN Hamilton
                else if (countyID == "208")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        streetno = streetNumber,
                        streetname = streetName,
                        streettype = streetType,
                        direction = direction,
                        unitnumber = unitnumber,
                        ownernm = "",
                        accno = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // TX Nueces
                else if (countyID == "142")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                // WV Kanawha
                else if (countyID == "201")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // CO Boulder
                else if (countyID == "207")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";

                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        city = "",
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // WA Cowlitz
                else if (countyID == "206")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // GA Fayette
                else if (countyID == "211")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";

                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // MO St Louis City
                else if (countyID == "212")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownernm = "",
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // Bell TX
                else if (countyID == "137")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        assessment_id = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // OH Delaware
                else if (countyID == "205")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        city = "",
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                // TX Ellis
                else if (countyID == "161")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        city = "",
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // OH  Mahoning
                else if (countyID == "214")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        city = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                // CO Araphoe
                else if (countyID == "48")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        city = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }                

                //TX El Paso
                else if (countyID == "52")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Galveston TX
                else if (countyID == "81")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //AL Jefferson
                else if (countyID == "124")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                //Montgomery OH
                else if (countyID == "127")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                        account = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //AZ Mohave

                else if (countyID == "117")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Sedgwick KS
                else if (countyID == "106")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                        account = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //KY Jefferson
                else if (countyID == "109")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //AL Montgomery
                else if (countyID == "177")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // TX Fort Bend
                else if (countyID == "182")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownernm = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                        account = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // NC Buncombe
                else if (countyID == "157")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // TX Guadalupe
                else if (countyID == "162")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        city = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        account = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //CA Merced
                else if (countyID == "163")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // OR Yamhill
                else if (countyID == "156")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // CA Ventura
                else if (countyID == "226")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //IL COOK
                else if (countyID == "216")
                {

                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //CA Santa Cruz
                else if (countyID == "92")
                {

                    object input = new
                    {
                        address = streetLine,
                        unitNumber = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // TX Travis
                else if (countyID == "184")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        city = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // TX Brazoria
                else if (countyID == "77")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //AZ Mohave
                else if (countyID == "117")
                {

                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                // TX Comal
                else if (countyID == "180")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }


                //CA Marin
                else if (countyID == "113")
                {

                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                //OH Lorain
                else if (countyID == "133")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //CA Stanislaus
                else if (countyID == "236")
                {
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //CA sonoma
                else if (countyID == "251")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        assessmentID = "",
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //CA Nevada
                else if (countyID == "406")
                {

                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //CA Imperial
                else if (countyID == "414")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                //CA Madera
                else if (countyID == "381")
                {

                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = "titleflex",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //CA Napa 
                else if (countyID == "400")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                //CA Solano
                else if (countyID == "240")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //Rework
                //GA Forsyth

                else if (countyID == "126")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //MD Frederick
                else if (countyID == "107")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //AZ Pima
                else if (countyID == "24")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //MD Montgomery

                else if (countyID == "25")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                //OH Cuyahoga
                else if (countyID == "26")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //WA snohomish
                else if (countyID == "28")
                {
                    string sear_type = "";
                    if (postdir != "")
                    {
                        streetName = streetName + " " + streetType + " " + postdir;
                    }
                    else
                    {
                        streetName = streetName + " " + streetType;
                    }
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }
                //MS Desoto
                else if (countyID == "144")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //MO Jackson
                else if (countyID == "45")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        address = streetLine,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //GA Paulding
                else if (countyID == "146")
                {
                    string sear_type = "";
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        sear_type = "titleflex";
                    }
                    else
                    {
                        sear_type = "address";
                    }
                    object input = new
                    {
                        houseno = streetNumber,
                        sname = streetName,
                        sttype = streetType,
                        direction = direction,
                        unitNumber = unitnumber,
                        ownername = "",
                        parcelNumber = "",
                        searchType = sear_type,
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatusSST(json, orderNumber);
                }

                //AB Team Counties Started.....
                //Adams
                else if (countyID == "69")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                    }
                    object input = new
                    {
                        address = taddress,
                        StreetNumber = streetNumber,
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                        ParcelNumber = "",
                        AccountNumber = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                //beaufort
                else if (countyID == "148")
                {

                    object input = new
                    {
                        Address = streetLine,
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                        ParcelNumber = "",
                        OwnerName = "",
                        AlternateID = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                //cobb
                else if (countyID == "27")
                {

                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                    }
                    object input = new
                    {
                        Address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        ParcelNumber = "",
                        OwnerName = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //anne
                else if (countyID == "44")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR") || stname.Contains(" AVE") || stname.Contains(" LN") || stname.Contains(" PL") || stname.Contains(" HWY") || stname.Contains(" BLVD"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "").Replace(" AVE", "").Replace(" LN", "").Replace(" PL", "").Replace(" HWY", "").Replace("BLVD", "");
                    }
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetName = "";
                        streetNumber = "";
                    }
                    object input = new
                    {
                        Address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = stname,
                        DistrictCode = "",
                        SubDivision = "",
                        AccountNumber = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Honolulu - HI
                else if (countyID == "47")
                {
                    object input = new
                    {
                        taddress = "",
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        UnitNumber = unitnumber,
                        Direction = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                
                //Lee - FL
                else if (countyID == "46")
                {
                    string stname = streetName;

                    propertyAddress = streetNumber + " " + streetName;
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        if (streetLine.Contains(" FT") || streetLine.Contains(" CPE"))
                        {
                            streetLine = streetLine.Replace(" FT", "").Replace("CPE", "");
                            propertyAddress = streetLine.Replace("\r\n", " ");
                        }
                        else
                        {
                            propertyAddress = streetLine.Replace("\r\n", " ");
                        }

                    }

                    object input = new
                    {
                        Address = propertyAddress,
                        OwnerName = "",
                        ParcelNumber = "",
                        Folio = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }


                //Pinellas - FL
                else if (countyID == "50")
                {
                    object input = new
                    {
                        Address = streetLine,
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }


                //Thurston WA
                else if (countyID == "91")
                {

                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                        streetType = "";
                        postdir = "";
                    }
                    object input = new
                    {
                        Address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        OrganizationName = "",
                        OwnerLastName = "",
                        OwnerFirstName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //York SC
                else if (countyID == "71")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR") || stname.Contains(" TRL"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "").Replace(" TRL", "");
                    }

                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        propertyAddress = streetNumber + " " + stname;
                    }
                    object input = new
                    {
                        Address = propertyAddress,
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                //brevard FL
                else if (countyID == "65")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        if (direction != "")
                        {
                            propertyAddress = streetNumber + " " + direction + " " + streetName + " " + streetType;
                        }
                        else
                        { propertyAddress = streetNumber + " " + streetName + " " + streetType; }

                    }
                    object input = new
                    {
                        Address = propertyAddress,
                        OwnerName = "",
                        ParcelNumber = "",
                        AccountNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }


                //Elpasco CO
                else if (countyID == "66")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                        streetType = "";
                        direction = "";
                    }
                    object input = new
                    {
                        address = taddress,
                        StreetName = streetName,
                        HouseNumberFrom = streetNumber,
                        HouseNumberTo = streetNumber,
                        StreetNumber = "",
                        StreetType = streetType,
                        Direction = direction,
                        OwnerLastName = "",
                        OwnerFirstName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Horry SC
                else if (countyID == "70")
                {
                    object input = new
                    {
                        Address = streetLine,
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,

                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }



                //Benton AR
                else if (countyID == "84")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                        cityName = "";
                    }
                    object input = new
                    {

                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        City = "",
                        Address = taddress,
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Henry GA
                else if (countyID == "87")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                        streetType = "";

                    }
                    object input = new
                    {
                        address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        StreetType = streetType,
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                //Richland SC..
                else if (countyID == "90")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        propertyAddress = streetNumber + " " + streetName;
                    }
                    object input = new
                    {
                        Address = propertyAddress,
                        TaxNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }



                //Wed CO
                else if (countyID == "89")
                {
                    object input = new
                    {
                        Address = streetLine,
                        AccountNumber = "",
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,


                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                //frederick MD

                else if (countyID == "107")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetName = "";
                        streetNumber = "";
                    }
                    object input = new
                    {
                        Address = streetLine,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        DistrictCode = "",
                        AccountNumber = "",
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                //Hawai HI
                else if (countyID == "104")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                        streetType = "";
                        direction = "";
                        unitnumber = "";
                    }
                    object input = new
                    {
                        address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        StreetType = streetType,
                        Direction = direction,
                        UnitNumber = unitnumber,
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                //Lake OH

                else if (countyID == "118")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                    }
                    object input = new
                    {
                        Address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        OwnerLastName = "",
                        OwnerFirstName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }


                //Mobile AL

                else if (countyID == "105")
                {
                    object input = new
                    {
                        Address = streetLine,
                        OwnerLastName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //seminole

                else if (countyID == "110")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetName = "";
                        streetNumber = "";
                        direction = "";
                    }

                    object input = new
                    {
                        Address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        Direction = direction,
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }


                //LakeIndiana

                else if (countyID == "136")
                {

                    object input = new
                    {
                        Address = streetLine,
                        OwnerName = "",
                        ParcelNumber = "",
                        TaxNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Lexington

                else if (countyID == "134")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        propertyAddress = streetLine;
                    }
                    else
                    {
                        propertyAddress = streetNumber + " " + streetName;
                    }
                    object input = new
                    {
                        Address = propertyAddress,
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }


                //Lucas
                else if (countyID == "131")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";

                    }
                    object input = new
                    {
                        address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        AssessorNumber = "",
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }




                //Baldwin

                else if (countyID == "167")
                {
                    streetName = streetNumber + " " + streetType;
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";
                    }

                    object input = new
                    {
                        Address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        PPIN = "",
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                

                //Douglas NE
                else if (countyID == "145")
                {
                    streetName = Regex.Match(streetName, @"\d+").Value;
                    if (streetName.Any(char.IsDigit))
                    {
                        Regex.Replace(streetName, "[^a-zA-Z]", "");
                    }

                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";
                        direction = "";
                    }

                    object input = new
                    {
                        OwnerName = "",
                        TaxNumber = "",
                        StreetType = "",
                        Address = taddress,
                        Direction = direction,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        HouseNumberFrom = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }



                //kane
                else if (countyID == "168")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";
                    }

                    object input = new
                    {
                        address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        HouseNumberFrom = streetNumber,
                        HouseNumberTo = streetNumber,
                        City = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }


                //LakeIllinois
                else if (countyID == "169")
                {
                    string stname = streetName;
                    if (stname.Contains(" DR") || stname.Contains(" RD") || stname.Contains(" ST") || stname.Contains(" CT") || stname.Contains(" CIR"))
                    {
                        stname = stname.Replace(" DR", "").Replace(" RD", "").Replace(" ST", "").Replace(" CT", "").Replace(" CIR", "");
                    }
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetName = "";
                        streetNumber = "";
                    }
                    object input = new
                    {
                        Address = taddress,
                        StreetNumber = streetNumber,
                        StreetName = stname,
                        Zipcode = zipCode.Substring(0,5),
                        OwnerName = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Maui
                else if (countyID == "147")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetName = "";
                        streetNumber = "";
                        streetType = "";
                        direction = "";
                    }
                    object input = new
                    {
                        OwnerName = "",
                        TaxNumber = "",
                        StreetType = streetType,
                        Address = taddress,
                        Direction = direction,
                        StreetNumber = streetNumber,
                        StreetName = streetName,
                        HouseNumberFrom = "",
                        ParcelNumber = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //McHenry
                else if (countyID == "172")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetName = "";
                        streetNumber = "";
                    }
                    object input = new
                    {
                        address = taddress,
                        StreetName = streetName,
                        HouseNumberFrom = streetNumber,
                        city = "",
                        ParcelNumber = "",
                        OwnerName = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                

                //Washington
                else if (countyID == "170")
                {
                    object input = new
                    {


                        ParcelNumber = "",
                        Address = streetLine,
                        OwnerName = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Davidson TN

                else if (countyID == "79")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";

                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Dupage IL
                else if (countyID == "75")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";
                        direction = "";
                        unitnumber = "";
                        cityName = "";
                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        StreetType = streetType,
                        Direction = direction,
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //IN Marion
                else if (countyID == "97")
                {
                    object input = new
                    {

                        ParcelNumber = "",
                        Address = streetLine,
                        OwnerName = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Queens NY
                else if (countyID == "150")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";
                        direction = "";
                        unitnumber = "";
                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        StreetType = streetType,
                        AptNumber = unitnumber,
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //VA Loudoun
                else if (countyID == "215")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";
                        direction = "";
                        unitnumber = "";
                        cityName = "";
                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        StreetType = streetType,
                        UnitNumber = unitnumber,
                        TaxMapNumber="",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                //Butler OH
                else if (countyID == "194")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";
                        direction = "";
                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        StreetType = streetType,
                        UnitNumber = unitnumber,
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Weber UT
                else if (countyID == "209")
                {
                    object input = new
                    {
                        ParcelNumber = "",
                        Address = streetLine,
                        OwnerName = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }
                //Yavapavi AZ
                else if (countyID == "102")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";
                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        StreetType = streetType,
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Oklahoma OK
                else if (countyID == "53")
                {
                    object input = new
                    {
                        ParcelNumber = "",
                        Address = streetLine,
                        OwnerName = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //NC Wake
                else if (countyID == "51")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //NV Douglas
                else if (countyID == "145")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetType = "";
                        streetName = "";
                        direction = "";
                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        StreetType = streetType,
                        Direction = direction,
                        UnitNumber = unitnumber,
                        TaxNumber = "",
                        HouseNumberFrom = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Jefferson CO
                else if (countyID == "67")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                        direction = "";
                        unitnumber = "";


                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        Direction = direction,
                        UnitNumber = unitnumber,
                        TaxNumber = "",
                        PPIN = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //FL Lake
                else if (countyID == "121")
                {
                    object input = new
                    {
                        ParcelNumber = "",
                        Address = streetLine,
                        OwnerName = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                // Lancaster NE
                else if (countyID == "130")
                {
                    if (streetLine.Contains("APT") || streetLine.Contains("UNIT") || streetLine.Contains("#"))
                    {
                        taddress = streetLine;
                        streetNumber = "";
                        streetName = "";
                    }

                    object input = new
                    {
                        ParcelNumber = "",
                        Address = taddress,
                        OwnerName = "",
                        StreetName = streetName,
                        StreetNumber = streetNumber,
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //Allen
                else if (countyID == "210")
                {
                    object input = new
                    {
                        ParcelNumber = "",
                        Address = streetLine,
                        OwnerName = "",
                        County = "",
                        CountyID = "",
                        OrderID = orderNumber,
                    };
                    string json = ScrapOrder(apiUrl, input);
                    updateRecordStatus(json, orderNumber);
                }

                //last
                
            }
        }
        BindScrapeList();
        MessageBox("Orders Retrieved Successfully.....");
    }
    public void updateRecordStatus(string json, string orderNumber)
    {
        if (json.Contains("Success") || json.Contains("Timeout"))
        {
            string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
            int result1 = gl.ExecuteSPNonQuery(taxquery);
        }
        else if (json.Contains("MultiRecord"))
        {
            string taxquery = "update record_status set scrape_status = 5 where order_no = '" + orderNumber + "'";
            int result1 = gl.ExecuteSPNonQuery(taxquery);
        }
        else if (json.Contains("MultiRecordInserted") || json.Contains("Order_No") || json.Contains("[]"))
        {
            string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
            int result1 = gl.ExecuteSPNonQuery(taxquery);
        }
        else
        {
            string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
            int result1 = gl.ExecuteSPNonQuery(taxquery);
        }
    }
    public void updateRecordStatusSST(string json, string orderNumber)
    {
        if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
        {
            // 2 completed                        
            string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
            int result1 = gl.ExecuteSPNonQuery(taxquery);
        }
        else if (json.Contains("An error has occured"))
        {
            //3 == scraping error                        
            string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
            int result1 = gl.ExecuteSPNonQuery(taxquery);
        }
        else
        {
            //1 == Multiparcel
            string taxquery = "update record_status set scrape_status = 4 where order_no = '" + orderNumber + "'";
            int result1 = gl.ExecuteSPNonQuery(taxquery);

        }
    }

    protected void btnscrap_Click(object sender, EventArgs e)
    {
        string orderNumber = "", county = "", state = "", ownerName = "", propertyAddress = "";
        ds = gl.GetCountyId(Session["sessionstate"].ToString(), Session["sessioncounty"].ToString());
        string addresstype = ds.Tables[0].Rows[0]["Address_Type"].ToString();
        string countyID = ds.Tables[0].Rows[0]["State_County_Id"].ToString();
        string apiUrl = ds.Tables[0].Rows[0]["service_url"].ToString();
        foreach (GridViewRow row in GridView3.Rows)
        {
            CheckBox chk = row.Cells[0].Controls[0].FindControl("chkselect") as CheckBox;

            if (chk.Checked)
            {
                row.BackColor = Color.Green;
                string number = row.Cells[2].Text.ToString();
                orderNumber = row.Cells[1].Text.ToString();
                if (countyID == "12")
                {
                    chk.Checked = false;
                    //WebDriverLA losAng = new WebDriverLA();
                    //losAng.FTP_LA(propertyAddress, "", "address", orderNumber, "", "");

                    if (GlobalClass.imgURL != "")
                    {
                        captcharead.Visible = true;
                        string outPath = System.Web.HttpContext.Current.Server.MapPath("~/captcha\\") + GlobalClass.parcelNumber_la.Replace("-", "") + ".png";
                        Image1.ImageUrl = "~/captcha/" + GlobalClass.parcelNumber_la.Replace("-", "") + ".png";
                        GlobalClass.imgURL = "";
                        return;
                    }

                }
                else if (countyID == "13")
                {
                    //WebDriverTest maricopaTest = new WebDriverTest();
                    //maricopaTest.FTP(propertyAddress, orderNumber, "address", "");
                    if (GlobalClass.multipleParcel == "Maximum")
                    {
                        GlobalClass.multipleParcel = "";
                        MessageBox("Search contains more results.go for manual search....");
                        return;
                    }
                    if (GlobalClass.multipleParcel == "Yes")
                    {
                        GlobalClass.multipleParcel = "";
                        MessageBox("Multiple Parcels....");
                        multi_AZMaricopa(orderNumber);
                        return;
                    }
                }
                else if (countyID == "23")
                {
                    // string apiUrl = ConfigurationManager.AppSettings["WashoeNVService"];
                    object input = new
                    {
                        address = "",
                        parcelNumber = number,
                        ownername = "",
                        SearchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                else if (countyID == "40")
                {
                    // string apiUrl = ConfigurationManager.AppSettings["MultnomahORService"];
                    object input = new
                    {
                        address = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        directParce = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                else if (countyID == "34")
                {

                    object input = new
                    {
                        address = "",
                        ownerName = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                else if (countyID == "33")
                {
                    //  string apiUrl = ConfigurationManager.AppSettings["SaintLouisMOService"];
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        blockno = "",
                        parcelNumber = number,
                        ownername = "",
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                else if (countyID == "32")
                {

                    //  string apiUrl = ConfigurationManager.AppSettings["DistofColumbiaDCService"];
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        blockno = "",
                        parcelNumber = number,
                        ownername = "",
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                else if (countyID == "20")
                {

                    // string apiUrl = ConfigurationManager.AppSettings["PierceWAService"];
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        blockno = "",
                        parcelNumber = number,
                        SearchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                else if (countyID == "30")
                {
                    //  string apiUrl = ConfigurationManager.AppSettings["MecklenburgService"];
                    object input = new
                    {
                        address = "",
                        ownerName = "",
                        parcelNumber = number,
                        searchType = "address",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                else if (countyID == "29")
                {

                    // string apiUrl = ConfigurationManager.AppSettings["DekalbGAservice"];
                    object input = new
                    {
                        address = "",
                        parcelNumber = number,
                        ownername = "",
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                else if (countyID == "22")
                {
                    //string apiUrl = ConfigurationManager.AppSettings["GwinnettGAService"];
                    object input = new
                    {
                        address = "",
                        parcelNumber = number,
                        ownername = "",
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                else if (countyID == "31")
                {
                    //  string apiUrl = ConfigurationManager.AppSettings["FranklinOHService"];
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        blockno = "",
                        parcelNumber = number,
                        ownername = "",
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        // 2 completed

                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        //3 == scraping error

                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                //placer
                else if (countyID == "56")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        assessmentID = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                else if (countyID == "36")
                {
                    object input = new
                    {
                        address = "",
                        unitNumber = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                else if (countyID == "35")
                {
                    object input = new
                    {
                        address = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        directParcel = "",
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                //fresno
                else if (countyID == "37")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        parcelNumber = number,
                        searchType = "parcels",
                        orderNumber = orderNumber,
                        directParcel = "",
                        ownername = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //harrison
                else if (countyID == "41")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""
                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //bernalillo
                else if (countyID == "54")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directSearch = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //tulsa
                else if (countyID == "57")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        direction = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        accountnumber = "",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                else if (countyID == "55")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                //utah
                else if (countyID == "58")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        direction = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        accountnumber = "",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""


                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //summit
                else if (countyID == "60")
                {
                    object input = new
                    {
                        address = "",
                        accountnumber = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                //hennepin
                else if (countyID == "49")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        direction = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                //new castle
                else if (countyID == "59")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //pinal
                else if (countyID == "61")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        direction = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        unitNumber = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //marion
                else if (countyID == "76")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        accountNumber = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //shelby
                else if (countyID == "74")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }

                //clackamas
                else if (countyID == "73")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //east baton rouge
                else if (countyID == "78")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        account = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //baltimore
                else if (countyID == "72")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = "",
                        unitNumber = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //polk
                else if (countyID == "63")
                {
                    object input = new
                    {
                        houseno = "",
                        sname = "",
                        direction = "",
                        sttype = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }
                //charleston
                else if (countyID == "82")
                {
                    object input = new
                    {
                        address = "",
                        account = "",
                        parcelNumber = number,
                        searchType = "parcel",
                        orderNumber = orderNumber,
                        ownername = "",
                        directParcel = ""

                    };
                    string json = ScrapOrder(apiUrl, input);
                    if (json.Contains("Data Inserted Successfully") || json.Contains("Timeout"))
                    {
                        string taxquery = "update record_status set scrape_status = 2 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    else if (json.Contains("An error has occured"))
                    {
                        string taxquery = "update record_status set scrape_status = 3 where order_no = '" + orderNumber + "'";
                        int result1 = gl.ExecuteSPNonQuery(taxquery);
                    }
                    divmultiowner.Visible = false;
                }




                RunOrderList();
                MessageBox("Orders Scraped Successfully.....");


            }
        }

    }

    private void BindAPN_Gwinnett(string orderNo)
    {
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 22  where DFM.Category_Id = 7  and DVM.order_no='" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);
                //situs_address~legal_description
                DataTable multiownertable = new DataTable();
                DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
                DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
                DataColumn situs_address = multiownertable.Columns.Add("situs_address", typeof(string));
                DataColumn Ownername = multiownertable.Columns.Add("Ownername", typeof(string));


                int dtcount = ds.Tables[0].Rows.Count;
                string[] multiarray;
                for (int i = 0; i < dtcount; i++)
                {
                    multiarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = multiarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');

                        DataRow row = multiownertable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(situs_address, split[0]);
                        row.SetField(Ownername, split[1]);
                        multiownertable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiownertable;
                GridView3.DataBind();
            }
        }
    }
    private void BindMulti_Dekalp(string orderNo)
    {
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 29  where DFM.Category_Id = 7  and DVM.order_no='" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);

                DataTable multiownertable = new DataTable();
                DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
                DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
                DataColumn Owner_Name = multiownertable.Columns.Add("Owner_Name", typeof(string));
                DataColumn Property_Address = multiownertable.Columns.Add("Property_Address", typeof(string));

                string[] multiarray;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    multiarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = multiarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');
                        DataRow row = multiownertable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(Owner_Name, split[0]);
                        row.SetField(Property_Address, split[1]);
                        multiownertable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiownertable;
                GridView3.DataBind();

            }
        }
    }
    public void Bindmulti_DC(string orderNo)
    {
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 32  where DFM.Category_Id = 7  and DVM.order_no='" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);
                //Owner_Name~Premise_Address
                DataTable multiaddresstable = new DataTable();
                DataColumn order_no = multiaddresstable.Columns.Add("order_no", typeof(string));
                DataColumn parcelno = multiaddresstable.Columns.Add("Parcel_no", typeof(string));
                DataColumn Owner_Name = multiaddresstable.Columns.Add("Owner_Name", typeof(string));
                DataColumn Address = multiaddresstable.Columns.Add("Address", typeof(string));


                int dtcount = ds.Tables[0].Rows.Count;
                string[] multiarray;
                for (int i = 0; i < dtcount; i++)
                {
                    multiarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = multiarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');

                        DataRow row = multiaddresstable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(Owner_Name, split[0]);
                        row.SetField(Address, split[1]);
                        multiaddresstable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiaddresstable;
                GridView3.DataBind();
            }
        }
    }
    private void Bindmulti_mecklenberg(string orderNo)
    {
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 30  where DFM.Category_Id = 7  and DVM.order_no='" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);

                DataTable multiownertable = new DataTable();
                DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
                DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
                DataColumn Owner_Name = multiownertable.Columns.Add("Owner_Name", typeof(string));
                DataColumn Property_Address = multiownertable.Columns.Add("Property_Address", typeof(string));
                DataColumn Description = multiownertable.Columns.Add("Description", typeof(string));

                string[] multiarray;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    multiarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = multiarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');

                        DataRow row = multiownertable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(Owner_Name, split[0]);
                        row.SetField(Property_Address, split[1]);
                        row.SetField(Description, split[2]);
                        multiownertable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiownertable;
                GridView3.DataBind();

            }
        }
    }
    private void BindAPN_STLouis(string orderNo)
    {
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 33  where DFM.Category_Id = 7  and DVM.order_no='" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);
                //situs_address~legal_description
                DataTable multiownertable = new DataTable();
                DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
                DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
                DataColumn parcel_address = multiownertable.Columns.Add("parcel_address", typeof(string));
                DataColumn owner_name = multiownertable.Columns.Add("owner_name", typeof(string));


                int dtcount = ds.Tables[0].Rows.Count;
                string[] multiarray;
                for (int i = 0; i < dtcount; i++)
                {
                    multiarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = multiarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');

                        DataRow row = multiownertable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(parcel_address, split[0]);
                        row.SetField(owner_name, split[1]);
                        multiownertable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiownertable;
                GridView3.DataBind();
            }
        }
    }
    private void multi_CAKern(string orderNo)
    {

        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 34  where DFM.Category_Id = 7  and DVM.order_no='" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);
                //siteaddress~City
                DataTable multiownertable = new DataTable();
                DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
                DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
                DataColumn siteaddress = multiownertable.Columns.Add("Site Address", typeof(string));
                DataColumn City = multiownertable.Columns.Add("City", typeof(string));

                string[] multiarray;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    multiarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = multiarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');

                        DataRow row = multiownertable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(siteaddress, split[0]);
                        row.SetField(City, split[1]);
                        multiownertable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiownertable;
                GridView3.DataBind();

            }
        }
    }
    private void Bindmulti_ORMultmoah(string orderNo)
    {
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 40  where DFM.Category_Id = 7  and DVM.order_no='" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);
                //Owner_Name~Alternate_Account_Number~Situs_Address~legal_Description
                DataTable multiownertable = new DataTable();
                DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
                DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
                DataColumn Owner_Name = multiownertable.Columns.Add("Owner_Name", typeof(string));
                DataColumn Alternate_Account_Number = multiownertable.Columns.Add("Alternate_Account_Number", typeof(string));
                DataColumn Situs_Address = multiownertable.Columns.Add("Situs_Address", typeof(string));
                DataColumn legal_Description = multiownertable.Columns.Add("legal_Description", typeof(string));


                int dtcount = ds.Tables[0].Rows.Count;
                string[] multiarray;
                for (int i = 0; i < dtcount; i++)
                {
                    multiarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = multiarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');

                        DataRow row = multiownertable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(Owner_Name, split[0]);
                        row.SetField(Alternate_Account_Number, split[1]);
                        row.SetField(Situs_Address, split[2]);
                        row.SetField(legal_Description, split[3]);
                        multiownertable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiownertable;
                GridView3.DataBind();
            }
        }
    }
    private void BindAPN_NVwashoe(string orderNo)
    {
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select order_no,parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 23  where DFM.Category_Id = 7 and DVM.Order_No = '" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);
                //parcel_no~type~situs_address~taxpayer_name
                DataTable multiownertable = new DataTable();
                DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
                DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
                DataColumn type = multiownertable.Columns.Add("type", typeof(string));
                DataColumn situs_address = multiownertable.Columns.Add("situs_address", typeof(string));
                DataColumn taxpayer_name = multiownertable.Columns.Add("taxpayer_name", typeof(string));


                int dtcount = ds.Tables[0].Rows.Count;
                string[] propertyarray;
                for (int i = 0; i < dtcount; i++)
                {
                    propertyarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = propertyarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');

                        DataRow row = multiownertable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(type, split[0]);
                        row.SetField(situs_address, split[1]);
                        row.SetField(taxpayer_name, split[2]);
                        multiownertable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiownertable;
                GridView3.DataBind();
            }
        }
    }
    public void multi_AZMaricopa(string orderNo)
    {
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 13  where DFM.Category_Id = 7  and DVM.order_no='" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);
                //Owner1~Address1~subdivision1~mcr1~str1~conto_owner~legal_description
                DataTable multiownertable = new DataTable();
                DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
                DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
                DataColumn Owner1 = multiownertable.Columns.Add("Owner1", typeof(string));
                DataColumn Address1 = multiownertable.Columns.Add("Address1", typeof(string));
                DataColumn subdivision1 = multiownertable.Columns.Add("subdivision1", typeof(string));
                DataColumn mcr1 = multiownertable.Columns.Add("mcr1", typeof(string));
                DataColumn str1 = multiownertable.Columns.Add("str1", typeof(string));
                DataColumn conto_owner = multiownertable.Columns.Add("conto_owner", typeof(string));
                DataColumn legal_description = multiownertable.Columns.Add("legal_description", typeof(string));


                int dtcount = ds.Tables[0].Rows.Count;
                string[] multiarray;
                for (int i = 0; i < dtcount; i++)
                {
                    multiarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = multiarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');

                        DataRow row = multiownertable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(Owner1, split[0]);
                        row.SetField(Address1, split[1]);
                        row.SetField(subdivision1, split[2]);
                        row.SetField(mcr1, split[3]);
                        row.SetField(str1, split[4]);
                        row.SetField(conto_owner, split[5]);
                        row.SetField(legal_description, split[6]);
                        multiownertable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiownertable;
                GridView3.DataBind();
            }
        }
    }
    private void Bindmulti_WAPierce(string orderNo)
    {
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 20  where DFM.Category_Id = 7  and DVM.order_no='" + orderNo + "' order by 1", con))
            {
                con.Open();
                DataSet ds = new DataSet();
                MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
                Sda.Fill(ds);
                //Type~Status~TaxpayerName~SiteAddress
                DataTable multiownertable = new DataTable();
                DataColumn order_no = multiownertable.Columns.Add("Order No", typeof(string));
                DataColumn parcelno = multiownertable.Columns.Add("Parcel No", typeof(string));
                DataColumn Type = multiownertable.Columns.Add("Type", typeof(string));
                DataColumn Status = multiownertable.Columns.Add("Status", typeof(string));
                DataColumn TaxpayerName = multiownertable.Columns.Add("Taxpayer Name", typeof(string));
                DataColumn SiteAddress = multiownertable.Columns.Add("Situs Address", typeof(string));


                int dtcount = ds.Tables[0].Rows.Count;
                string[] multiarray;
                for (int i = 0; i < dtcount; i++)
                {
                    multiarray = new string[] { ds.Tables[0].Rows[i]["Data_Field_value"].ToString() };
                    string source = multiarray[0].ToString();

                    string[] lines = source.Split('\n');

                    foreach (var line in lines)
                    {
                        string[] split = line.Split('~');

                        DataRow row = multiownertable.NewRow();
                        row.SetField(order_no, ds.Tables[0].Rows[i]["order_no"].ToString());
                        row.SetField(parcelno, ds.Tables[0].Rows[i]["parcel_no"].ToString());
                        row.SetField(Type, split[0]);
                        row.SetField(Status, split[1]);
                        row.SetField(TaxpayerName, split[2]);
                        row.SetField(SiteAddress, split[3]);
                        multiownertable.Rows.Add(row);
                    }
                }
                GridView3.DataSource = multiownertable;
                GridView3.DataBind();
            }
        }
    }
    public string ScrapOrder(string apiUrl, object inputObj)
    {

        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsJsonAsync(apiUrl, inputObj).Result;
            // response.EnsureSuccessStatusCode();                        
            var result = response.Content.ReadAsStringAsync().Result;
            var s = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "An error has occured";
            }
            else
            {
                return s.ToString();
            }
        }
        catch
        {
            return "Timeout";
        }
    }

    protected void btnclose_Click(object sender, EventArgs e)
    {
        divmultiowner.Visible = false;
    }

    protected void LnkLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("LoginChecklist.aspx");
    }
    public class Testbind
    {
        public string Order_No { get; set; }
        public string Parcel_no { get; set; }
        public string Data_Field_value { get; set; }
    }
}

