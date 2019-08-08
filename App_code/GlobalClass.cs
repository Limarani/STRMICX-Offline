using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing.Imaging;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Firefox;
using System.Diagnostics;
using OpenQA.Selenium.Remote;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data;
using System.Text;
using System.Collections;
using System.Security;
//madesh
/// <summary>
/// Summary description for GlobalClass
/// </summary>
public class GlobalClass : myConnection
{
    string strInput = ConfigurationManager.AppSettings["TitleFlexInput"];
    string strOutput = ConfigurationManager.AppSettings["TitleFlexOutput"];
    myConnection con = new myConnection();
    DBConnection db = new DBConnection();
    private string _order_no = null;
    public string order_no
    {
        get { return _order_no; }
        set { _order_no = value; }
    }
    private string _Process = null;
    public string Process
    {
        get { return _Process; }
        set { _Process = value; }
    }
    private string _lockuser = null;
    public string lockuser
    {
        get { return _lockuser; }
        set { _lockuser = value; }
    }

    #region Variable Declareation
    MySqlDataAdapter mDa;
    MySqlDataReader mDr;
    MySqlParameter[] mParam;
    MySqlCommand cmd;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    MySqlConnection mConnection = new MySqlConnection();
    DataView dataview;
    #endregion

    #region Others
    public string setdate()
    {
        string result = "";
        Int64 setdate = 0;
        string assdate = String.Format("{0:HH:mm:ss}", DateTime.Now);
        setdate = Convert.ToInt64(assdate.Replace(":", ""));
        if (setdate >= 000000 && setdate <= 183000) result = String.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddDays(-1));
        //else result = String.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddMonths(0).AddDays(-1));
        else result = String.Format("{0:dd-MMM-yyyy}", DateTime.Now);
        return result;
    }
    public DateTime ToDate()
    {
        Int64 setdate = 0;
        string assdate = String.Format("{0:HH:mm:ss}", DateTime.Now);
        setdate = Convert.ToInt64(assdate.Replace(":", ""));
        if (setdate >= 000000 && setdate <= 070000) return DateTime.Now.AddMonths(0).AddDays(-1);
        //else result = String.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddMonths(0).AddDays(-1));
        else return DateTime.Now;
    }
    #endregion

    #region ErrorPage
    public void RedirectErrorPage()
    {
        // HttpContext.Current.Response.Redirect("Error.aspx");
    }
    #endregion

    #region CheckNullDB
    public string checkNullDB(MySqlDataReader mdr, string field)
    {
        if (mdr[field] == DBNull.Value)
        {
            return "";
        }
        else
        {
            return mdr.GetString(field);
        }
    }
    #endregion

    #region LoginPage
    public MySqlDataReader checkLogin(string User, string Pwd)
    {
        mParam = new MySqlParameter[1];
        string query = "select User_Name,Admin,concat(Keying,QC,DU,Review) as Process,concat(Pending,Parcelid,Onhold,mailaway) as Pending,concat(QA,Review) as Audit from user_status where User_Name='" + User + "' and User_Password =aes_encrypt('" + Pwd + "','String') limit 1";
        try
        {
            mDr = con.ExecuteSPReader(query, false, mParam);
            return mDr;
        }
        catch (Exception) { return mDr; }
    }
    public String TCase(String strParam)
    {
        String strTitleCase = strParam.Substring(0, 1).ToUpper();
        strParam = strParam.Substring(1).ToLower();
        String strPrev = "";

        for (int iIndex = 0; iIndex < strParam.Length; iIndex++)
        {
            if (iIndex > 1)
            {
                strPrev = strParam.Substring(iIndex - 1, 1);
            }
            if (strPrev.Equals(" ") ||
                strPrev.Equals("\t") ||
                strPrev.Equals("\n") ||
                strPrev.Equals("."))
            {
                strTitleCase += strParam.Substring(iIndex, 1).ToUpper();
            }
            else
            {
                strTitleCase += strParam.Substring(iIndex, 1);
            }
        }
        return strTitleCase;
    }
    public void LoginDetails()
    {
        string update, sys = "";
        sys = System.Web.HttpContext.Current.Request.UserHostAddress;
        update = "insert into login_det(username,admin_rights,sys_name,login_time)values('" + SessionHandler.UserName + "','" + SessionHandler.IsAdmin + "','" + sys + "',now())";
        con.ExecuteScalar(update);
    }
    public int ChecklistLoginDetails()
    {
        int result;
        string insert, sys = "";
        sys = System.Web.HttpContext.Current.Request.UserHostAddress;
        insert = "insert into checklist_login_report(username,pdate,admin_rights,sys_name,login_time,flag)values('" + SessionHandler.UserName + "',DATE_FORMAT(DATE_SUB(now(),INTERVAL '07:00' HOUR_MINUTE),'%d-%m-%Y'),'" + SessionHandler.IsAdmin + "','" + sys + "',now(),2)";
        result = con.ExecuteSPNonQuery(insert);
        return result;
    }
    #endregion

    #region Homepage
    public DataSet QualityReport(string frdate, string todate, string strvalue)
    {
        DataSet ds = new DataSet();
        //string query = "sp_qualityreport1";
        string query = "sp_dashboard_datewise";
        mParam = new MySqlParameter[3];
        mParam[0] = new MySqlParameter("?$fdate", frdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", todate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$strvalue", strvalue);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(ds);
        return ds;
    }
    public DataSet DashBoardQuality(string fdate, string tdate)
    {
        DataSet ds = new DataSet();
        string query = "sp_dashboard1";
        mParam = new MySqlParameter[2];
        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        //mParam[2] = new MySqlParameter("?$status", status);
        //mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(ds);
        return ds;
    }
    public string getSDirection()
    {
        if (myVariables.mSortDirection == SortDirection.Ascending)
        {
            myVariables.mSortDirection = SortDirection.Descending;
            return "Desc";
        }
        else
        {
            myVariables.mSortDirection = SortDirection.Ascending;
            return "Asc";
        }
    }
    public DataView CovertDashBoardDStoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Name";
            dcolumn.Caption = "Name";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Phone";
            dcolumn.Caption = "Phone";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Website";
            dcolumn.Caption = "Website";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Mailaway";
            dcolumn.Caption = "Mailaway";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Production";
            dcolumn.Caption = "Production";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "QC";
            dcolumn.Caption = "QC";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Productivity";
            dcolumn.Caption = "Productivity";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Quality";
            dcolumn.Caption = "Quality";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Quality(QC)";
            dcolumn.Caption = "Quality(QC)";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Utilization (In Min)";
            dcolumn.Caption = "Utilization (In Min)";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            double noerror, total, quality, round = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();
                    dtrow[0] = ds.Tables[0].Rows[i]["Name"];
                    dtrow[1] = ds.Tables[0].Rows[i]["Phone"];
                    dtrow[2] = ds.Tables[0].Rows[i]["Website"];
                    dtrow[3] = ds.Tables[0].Rows[i]["Mailaway"];
                    dtrow[4] = ds.Tables[0].Rows[i]["Production"];
                    dtrow[5] = ds.Tables[0].Rows[i]["QC"];
                    dtrow[6] = ds.Tables[0].Rows[i]["Productivity"];

                    if (dtrow[4].ToString() == "0" && dtrow[5].ToString() != "0")
                    {
                        dtrow[7] = "";
                        dtrow[8] = ds.Tables[0].Rows[i]["Quality"] + "%";
                    }
                    else
                    {
                        dtrow[7] = ds.Tables[0].Rows[i]["Quality"] + "%";
                        dtrow[8] = "";
                    }
                    dtrow[9] = ds.Tables[0].Rows[i]["Utilization"];
                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }
    #endregion

    #region User Settings
    public DataSet BindDropdown()
    {
        string query = "select `type` from order_type order by status asc";
        return con.ExecuteQuery(query);
    }
    public DataSet GetUsers()
    {
        try
        {
            string query = "select * from user_status order by User_name";
            return con.ExecuteQuery(query);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetUserRecordsnew()
    {
        try
        {
            string query = "select *,cast(aes_decrypt(User_Password,'String') as char) as pwd from user_status where User_Name='" + SessionHandler.UserName + "'";
            return con.ExecuteQuery(query);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void EditUsers(string UserName)
    {
        myVariables.Fullname = "";
        myVariables.Username = "";
        myVariables.Admin = "";
        myVariables.Key = "";
        myVariables.QC = "";
        myVariables.DU = "";
        myVariables.Pend = "";
        myVariables.Ordertype = "";
        myVariables.Mailaway = "";
        myVariables.Parcelid = "";
        myVariables.Onhold = "";
        myVariables.pdu = "";
        myVariables.SST = "";

        string query = "select us.User_Name as Fullname,us.Admin as Admin,us.Keying as Key1,us.QC as QC,us.Review as Review,us.Pending as Pend,us.DU as DU,us.mailaway as mailaway,us.Parcelid as parcelid,us.Onhold as onhold,us.Order_type as type,us.State,us.SST,us.Priority,us.QA,us.Prior  from user_status us where us.User_Name ='" + UserName + "'  limit 1";
        mDr = con.ExecuteSPReader(query, false, mParam);
        if (mDr.HasRows)
        {
            if (mDr.Read())
            {
                myVariables.Fullname = checkNullDB(mDr, "Fullname");
                myVariables.Username = checkNullDB(mDr, "Username");
                myVariables.Admin = checkNullDB(mDr, "Admin");
                myVariables.Key = checkNullDB(mDr, "Key1");
                myVariables.QC = checkNullDB(mDr, "QC");
                myVariables.DU = checkNullDB(mDr, "DU");
                myVariables.Pend = checkNullDB(mDr, "Pend");
                myVariables.Ordertype = checkNullDB(mDr, "type");
                myVariables.Mailaway = checkNullDB(mDr, "mailaway");
                myVariables.Parcelid = checkNullDB(mDr, "parcelid");
                myVariables.Onhold = checkNullDB(mDr, "onhold");
                myVariables.Review = checkNullDB(mDr, "Review");
                myVariables.States = checkNullDB(mDr, "State");
                myVariables.SST = checkNullDB(mDr, "SST");
                myVariables.Priority = checkNullDB(mDr, "Priority");
                myVariables.QA = checkNullDB(mDr, "QA");
                myVariables.AssPriority = checkNullDB(mDr, "Prior");
            }
        }
        mDr.Close();
    }
    public int InsertUser(string ful, string Ad, string Keya, string Qc, string Du, string pend, string mail, string parcelid, string onhold, string ordertype, string review, string priority, string qa, string prior)
    {
        mParam = new MySqlParameter[14];

        mParam[0] = new MySqlParameter("?$User_Name", ful);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[0].IsNullable = false;

        //mParam[1] = new MySqlParameter("?$User_Id", usr);
        //mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$Admin", Ad);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$Keying", Keya);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$QC", Qc);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$Pending", pend);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$DU", Du);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$mailaway", mail);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$parcelid", parcelid);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("?$onhold", onhold);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        mParam[9] = new MySqlParameter("?$Order_type", ordertype);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;

        mParam[10] = new MySqlParameter("?$review", review);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;

        mParam[11] = new MySqlParameter("?$prio", priority);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;

        mParam[12] = new MySqlParameter("?$qa", qa);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;

        mParam[13] = new MySqlParameter("?$priority", prior);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_InsertUser", true, mParam);
    }

    public int UpdateUser(string usr, string Ad, string Keya, string Qc, string Du, string pend, string mail, string parcelid, string onhold, string ordertype, string review, string strstate, string priority, string qa, string prior)
    {
        mParam = new MySqlParameter[15];

        mParam[0] = new MySqlParameter("?$User_Id", usr);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$Admin", Ad);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$Keying", Keya);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$QC", Qc);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$Pending", pend);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$DU", Du);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$mailaway", mail);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$parcelid", parcelid);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("?$onhold", onhold);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        mParam[9] = new MySqlParameter("?$Order_type", ordertype);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;

        mParam[10] = new MySqlParameter("?$review", review);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;

        mParam[11] = new MySqlParameter("?$state", strstate);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;

        mParam[12] = new MySqlParameter("?$prio", priority);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;

        mParam[13] = new MySqlParameter("?$qa", qa);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;

        mParam[14] = new MySqlParameter("?$priority", prior);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_UpdateUser", true, mParam);

    }
    public DataSet GetuserDetails(string strstatus)
    {
        mParam = new MySqlParameter[1];

        mParam[0] = new MySqlParameter("?$strstatus", strstatus);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_Userdetails", true, mParam);
    }

    public DataSet GridScrapdetails(string strstatus)
    {
        mParam = new MySqlParameter[1];

        mParam[0] = new MySqlParameter("?$strstatus", strstatus);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("select * from grid", true, mParam);
    }

    public int ResetPassword(string usr)
    {
        string query = "update user_status set User_Password=aes_encrypt('String123$','String') where User_Id='" + usr + "' limit  1";
        return con.ExecuteSPNonQuery(query);
    }


    public int ChangePassword(string usr, string Pass)
    {
        string query = "update user_status set User_Password=aes_encrypt('" + Pass + "','String') where User_Name='" + usr + "' limit  1";
        return con.ExecuteSPNonQuery(query);
    }

    public int InsertChequePayable(string strcheque, string strreq_type, string straddress, string stramount, string strtax_type)
    {
        mParam = new MySqlParameter[5];

        mParam[0] = new MySqlParameter("?$cheque_pay", strcheque);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$req_type", strreq_type);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$address", straddress);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$amount", stramount);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$taxtype", strtax_type);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_Insert_ChequePayable", true, mParam);
    }
    public int UpdateChequePayable(string strcheque, string strreq_type, string straddress, string stramount, string strtax_type)
    {
        mParam = new MySqlParameter[5];

        mParam[0] = new MySqlParameter("?$cheque_pay", strcheque);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$req_type", strreq_type);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$address", straddress);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$amount", stramount);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$taxtype", strtax_type);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_Update_ChequePayable", true, mParam);
    }
    #endregion

    #region AssignJob
    private string checkStrings(string value)
    {
        if (value == "" || value == null || value == "&nbsp;")
        {
            return "";
        }
        else
        {
            return value;
        }
    }
    public int InsertData_New(string OrderNo, string state, string county, string prior, string pdate, string expected, string queue, string serpro)
    {
        mParam = new MySqlParameter[8];

        mParam[0] = new MySqlParameter("?$OrderNo", checkStrings(OrderNo));
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[0].IsNullable = false;

        mParam[1] = new MySqlParameter("?$stat", checkStrings(state));
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$county", checkStrings(county));
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$prior", checkStrings(prior));
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$pdate", checkStrings(pdate));
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$expected", checkStrings(expected));
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$queuedate", checkStrings(queue));
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$serpro", checkStrings(serpro));
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        int result = con.ExecuteSPNonQuery("sp_InsertOrder", true, mParam);
        return result;
    }
    #region Reset Job

    public void GetOrders(ListBox lstbx, string pdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "select order_no from record_status where pdate='" + pdate + "'";
            DataSet ds = con.ExecuteQuery(strquery);
            if (ds.Tables.Count > 0)
            {
                lstbx.DataSource = ds;
                lstbx.DataTextField = "Order_No";
                lstbx.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderDelete(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Delete from record_status where order_no='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderLock(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set Lock1=1 where order_no='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public void GetOrderUnLock(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set Lock1=0 where order_no='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderUnLockKey(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set Lock1=0,K1='0',QC='0',Status='0' where order_no='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderUnLockQC(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set Lock1=0,K1='2',QC='0',Status='2' where order_no='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    






    public void GetOrderUnlock(string strorderno, string date)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set Lock1=0 where order_no='" + strorderno + "' and pdate='" + date + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderPriority(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "update record_status set HP=1 where order_no='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderReject(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set k1='7',qc='7',status='7',Pend='0',Parcel='0',Tax='0',Lock1='0' where Order_No ='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet Gettitleflexdata2(string orderno)
    {
        using (MySqlConnection cnn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString))
        {
            string query = "select TaxYear ,AssessedYear,LandValue, ImproveValue,TotalAssessed,TotalTax, AlternateAPN ,Tra from titleflex where orderno ='" + orderno + "'";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(query, cnn);
            MySqlDataAdapter sda = new MySqlDataAdapter();
            DataSet ds = new DataSet();
            sda.SelectCommand = cmd;
            sda.Fill(ds);
            cnn.Close();
            return ds;
        }

    }

    public DataSet Gettitleflexdata1(string orderno)
    {
        //using (MySqlConnection cnn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString))
        //{
        string query = "select OwnerName ,Address ,LegalDescription,YearBuilt,EffectiveYearBuilt,Exemption from titleflex where orderno ='" + orderno + "'";
        //cnn.Open();
        //MySqlCommand cmd = new MySqlCommand(query, cnn);
        //MySqlDataAdapter sda = new MySqlDataAdapter();
        DataSet ds = new DataSet();
        ds = con.ExecuteQuery(query);
        //sda.SelectCommand = cmd;
        //sda.Fill(ds);
        //cnn.Close();
        return ds;
        //}

    }
    public void GetOrderInprocess(string strorderno, string date)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set k1='2',qc='2',status='2',pend='3',Parcel='0',Tax='0' where Order_No ='" + strorderno + "' and pdate='" + date + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void GetOrderYTS(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set k1='0',qc='0',status='0',pend='0',Parcel='0',Tax='0',Lock1='0' where Order_No ='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderHold(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set k1='4',qc='4',status='4',Pend='0',Parcel='0',Tax='0' where Order_No ='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderUnHold(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set k1='7',qc='7',status='7',Pend='0',Parcel='0',Tax='0' where Order_No ='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderDePriority(string strorderno, string fdate, string tdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "update record_status set HP=0 where order_no='" + strorderno + "' and pdate between '" + fdate + "' and '" + tdate + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void GetOrdersShow(ListBox lstbx, string pdate)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "select order_no from record_status where pdate='" + pdate + "' and LENGTH(order_no) >=9";
            mDr = con.ExecuteSPReader(strquery);
            lstbx.DataSource = mDr;
            lstbx.DataTextField = "Order_No";
            lstbx.DataBind();
            mDr.NextResult();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int DeleteDB(string frmdate, string todate)
    {
        mParam = new MySqlParameter[3];
        mParam[0] = new MySqlParameter("?$frmdate", frmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$todate", todate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$Uid", SessionHandler.UserName);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_clear_db", true, mParam);
    }
    #endregion

    #endregion

    #region Production
    public string GetOrdersStatus(string id)
    {
        string result = "", query = "", Orderstatus = "";
        query = "select concat(K1,QC,`Status`,Tax,Pend,Parcel) from record_status where id='" + id + "' limit 1";
        Orderstatus = con.ExecuteScalar(query);
        if (Orderstatus == "000000") result = "YTS";
        return result;
    }

    public void GetUserrights(string productiontype)
    {
        string query = "";

        if (productiontype == "Autoallotment") query = "select concat(Keying,QC,DU,Parcelid,Pending,mailaway,Review,Priority,Prior) as Urights from user_status where User_Name='" + SessionHandler.UserName + "' limit 1";
        else if (productiontype == "Manualallotment") query = "select concat(mailaway,Pending,Parcelid,Onhold) as Urights from user_status where User_Name='" + SessionHandler.UserName + "' limit 1";

        mDr = con.ExecuteSPReader(query, false, mParam);
        if (mDr.HasRows)
        {
            if (mDr.Read())
            {
                myVariables.Userright = checkNullDB(mDr, "Urights");
            }
        }
        mDr.Close();
    }
    public DataSet Allotment(string allotType, string id)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$allotType", allotType);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$id", id);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_Production_Validation_User", true, mParam);
    }

    public DataSet getscrapeorderlist()
    {
        return con.ExecuteQuery("sp_getScrapeOrderList", true, mParam);
    }

    public bool GetDatas(string pType, string id, string state)
    {
        bool result = false;

        myVariables.Orderno = "";
        myVariables.Date = "";
        myVariables.State = "";
        myVariables.County = "";
        myVariables.WebPhone = "";
        myVariables.TimeZone = "";
        myVariables.Lastcomment = "";
        myVariables.Borrower = "";
        myVariables.Township = "";
        myVariables.KeyStatus = "";
        myVariables.QCStatus = "";
        myVariables.Ordertype = "";
        myVariables.Zipcode = "";
        myVariables.Followupdate = "";

        mParam = new MySqlParameter[4];

        mParam[0] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$pType", pType);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$id", id);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$state", state);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        //mDr = con.ExecuteSPReader("sp_OrderAllotment1", true, mParam);
        DateTime strdate = Convert.ToDateTime("03/09");
        DateTime enddate = Convert.ToDateTime("11/13");
        if (strdate.Date <= DateTime.Now.Date && enddate.Date >= DateTime.Now.Date)
        {
            mDr = con.ExecuteSPReader("sp_OrderAllotment_MarToNov", true, mParam);
        }
        else mDr = con.ExecuteSPReader("sp_OrderAllotment_NovToMar", true, mParam);

        if (mDr.HasRows)
        {
            if (mDr.Read())
            {
                myVariables.Orderno = checkNullDB(mDr, "Order_No");
                myVariables.Date = checkNullDB(mDr, "PDate");
                myVariables.State = checkNullDB(mDr, "State");
                myVariables.County = checkNullDB(mDr, "County");
                myVariables.WebPhone = checkNullDB(mDr, "WebPhone");
                myVariables.TimeZone = checkNullDB(mDr, "Time_Zone");
                myVariables.HP = checkNullDB(mDr, "HP");
                myVariables.Prior = checkNullDB(mDr, "Prior");
                myVariables.Zipcode = checkNullDB(mDr, "zipcode");
                myVariables.Followupdate = checkNullDB(mDr, "followupdate");
                if (SessionHandler.AuditQA != "1") myVariables.Lastcomment = checkNullDB(mDr, "Comments_Det1");
                else myVariables.Lastcomment = checkNullDB(mDr, "Comments_Det");

                if (pType == "KEY" || pType == "PRIORITYKEY" || pType == "DU" || pType == "PRIORITYDU" || pType == "PRIORKEY" || pType == "PRIORDU")
                { myVariables.OrderTp = checkNullDB(mDr, "OrderType"); }


                if (pType == "QC")
                {
                    myVariables.Borrower = checkNullDB(mDr, "borrowername");
                    myVariables.Township = checkNullDB(mDr, "Township");
                    myVariables.KeyStatus = checkNullDB(mDr, "key_status");
                }
                else if (pType == "INPROCESS" || pType == "PARCELID" || pType == "ONHOLD" || pType == "MAILAWAY" || pType == "REVIEW")
                {
                    myVariables.Borrower = checkNullDB(mDr, "borrowername");
                    myVariables.Township = checkNullDB(mDr, "Township");
                    myVariables.QCStatus = checkNullDB(mDr, "ComStatus");
                    if (myVariables.QCStatus == "") { myVariables.QCStatus = checkNullDB(mDr, "key_status"); }
                }
                result = true;
            }
        }
        mDr.Close();
        return result;
    }
    public DataSet LoadStatus()
    {
        string query = "select statusname from statuscombo order by statusname asc";
        DataSet ds = con.ExecuteQuery(query);
        return ds;
    }
    public int UpdateOrders(string query, string orderno, string township, string borrower, string ist, string pdate, string oStatus, string ordertype, string error, string errorfield, string correct, string incorrect, string processname, string ist1, string qcopcomments, string errorcategory, string encount, string zipcode, string followupdate)
    {
        if (query == "sp_UpdateQC" || query == "sp_UpdateReview") mParam = new MySqlParameter[19];
        else mParam = new MySqlParameter[11];

        mParam[0] = new MySqlParameter("?$OrderNo", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$town", township);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$borrower", borrower);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$comments", ist);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$pdate", pdate);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$oStatus", oStatus);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$oType", ordertype);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$processname", processname);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("?$comments1", ist1);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        mParam[9] = new MySqlParameter("?$encount", encount);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;

        mParam[10] = new MySqlParameter("?$zipcode", zipcode);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;

        if (query == "sp_UpdateQC" || query == "sp_UpdateReview")
        {

            mParam[11] = new MySqlParameter("?$error", error);
            mParam[11].MySqlDbType = MySqlDbType.VarChar;

            mParam[12] = new MySqlParameter("?$errorfield", errorfield);
            mParam[12].MySqlDbType = MySqlDbType.VarChar;

            mParam[13] = new MySqlParameter("?$correct", correct);
            mParam[13].MySqlDbType = MySqlDbType.VarChar;

            mParam[14] = new MySqlParameter("?$incorrect", incorrect);
            mParam[14].MySqlDbType = MySqlDbType.VarChar;

            mParam[15] = new MySqlParameter("?$userid", SessionHandler.UserName);
            mParam[15].MySqlDbType = MySqlDbType.VarChar;

            mParam[16] = new MySqlParameter("?$qcerrorcomments", qcopcomments);
            mParam[16].MySqlDbType = MySqlDbType.VarChar;

            mParam[17] = new MySqlParameter("?$errorcategory", errorcategory);
            mParam[17].MySqlDbType = MySqlDbType.VarChar;

            mParam[18] = new MySqlParameter("?$followupdate", followupdate);
            mParam[18].MySqlDbType = MySqlDbType.VarChar;
        }


        return con.ExecuteSPNonQuery(query, true, mParam);
    }

    public DataSet UpdateOrdersNew(string query, string orderno, string township, string borrower, string ist, string pdate, string oStatus, string ordertype, string error, string errorfield, string correct, string incorrect, string processname, string ist1, string qcopcomments, string errorcategory, string encount, string zipcode, string followupdate, string misc)
    {
        if (query == "sp_UpdateQC_New" || query == "sp_UpdateQC_User" || query == "sp_UpdateReview_New" || query == "sp_UpdateDU_New" || query == "sp_UpdateDU_User") mParam = new MySqlParameter[25];
        else mParam = new MySqlParameter[17];

        mParam[0] = new MySqlParameter("?$OrderNo", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$town", township);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$borrower", borrower);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$comments", ist);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$pdate", pdate);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$oStatus", oStatus);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$oType", ordertype);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$processname", processname);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("?$comments1", ist1);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        mParam[9] = new MySqlParameter("?$encount", encount);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;

        mParam[10] = new MySqlParameter("?$zipcode", zipcode);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;

        mParam[11] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;

        mParam[12] = new MySqlParameter("?$chkcomments", myVariables.ChecklistComments);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;

        mParam[13] = new MySqlParameter("?$entitycomments", myVariables.EntityComments);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;

        mParam[14] = new MySqlParameter("?$taxamnt", myVariables.Tax);//xxx
        mParam[14].MySqlDbType = MySqlDbType.VarChar;

        mParam[15] = new MySqlParameter("?$paystatus", myVariables.PayStatus);//xxx
        mParam[15].MySqlDbType = MySqlDbType.VarChar;

        mParam[16] = new MySqlParameter("?$payfreq", myVariables.PayFreq);//xxx
        mParam[16].MySqlDbType = MySqlDbType.VarChar;

        if (query == "sp_UpdateQC_New" || query == "sp_UpdateQC_User" || query == "sp_UpdateReview_New" || query == "sp_UpdateDU_New" || query == "sp_UpdateDU_User")
        {

            mParam[17] = new MySqlParameter("?$error", error);
            mParam[17].MySqlDbType = MySqlDbType.VarChar;

            mParam[18] = new MySqlParameter("?$errorfield", errorfield);
            mParam[18].MySqlDbType = MySqlDbType.VarChar;

            mParam[19] = new MySqlParameter("?$correct", correct);
            mParam[19].MySqlDbType = MySqlDbType.VarChar;

            mParam[20] = new MySqlParameter("?$incorrect", incorrect);
            mParam[20].MySqlDbType = MySqlDbType.VarChar;

            mParam[21] = new MySqlParameter("?$qcerrorcomments", qcopcomments);
            mParam[21].MySqlDbType = MySqlDbType.VarChar;

            mParam[22] = new MySqlParameter("?$errorcategory", errorcategory);
            mParam[22].MySqlDbType = MySqlDbType.VarChar;

            mParam[23] = new MySqlParameter("?$followupdate", followupdate);
            mParam[23].MySqlDbType = MySqlDbType.VarChar;

            mParam[24] = new MySqlParameter("?$misc", misc);
            mParam[24].MySqlDbType = MySqlDbType.VarChar;
        }

        return con.ExecuteQuery(query, true, mParam);
    }



    public DataSet UpdateOrderStatusNew(string query, string orderno, string processname, string zipcode, string Ostatus, string comments)
    {
        mParam = new MySqlParameter[6];

        mParam[0] = new MySqlParameter("?$OrderNo", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;         

        mParam[1] = new MySqlParameter("?$processname", processname);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
                        
        mParam[2] = new MySqlParameter("?$zipcode", zipcode);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$Ostatus", Ostatus);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$comments", comments);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery(query, true, mParam);
    }


    public DataSet Updatetaxauthorities(string query, string orderno, string taxid, string agencyid, string taxtype, string isdelinquent, string exemption, string specialassessment, string priordelinquent, string primaryresidence)
    {
        mParam = new MySqlParameter[9];

        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$taxtype", taxtype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$isdelinquent", isdelinquent);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$exemption", exemption);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$specialassessment", specialassessment);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$priordelinquent", priordelinquent);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("?$primaryresidence", primaryresidence);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery(query, true, mParam);
    }



    public int MoveToCall(string query, string orderno, string township, string borrower, string ist, string pdate, string oStatus, string ordertype, string ist1, string zipcode, string encount, string processname)
    {
        mParam = new MySqlParameter[11];

        mParam[0] = new MySqlParameter("?$OrderNo", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$town", township);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$borrower", borrower);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$comments", ist);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$pdate", pdate);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$oStatus", oStatus);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$oType", ordertype);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$processname", processname);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("?$comments1", ist1);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        mParam[9] = new MySqlParameter("?$encount", encount);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;

        mParam[10] = new MySqlParameter("?$zipcode", zipcode);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery(query, true, mParam);
    }

    public void logoutreason(string orderno, string reason, string process, string ordate)
    {
        //string servername = SystemInformation.ComputerName;  
        string servername = "";
        string date = String.Format("{0:MM/dd/yyyy}", DateTime.Now);

        if (process == "PRODUCTION") process = "KEY";

        MySqlParameter[] mParam = new MySqlParameter[7];

        mParam[0] = new MySqlParameter("?$sysname", servername);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$userid", SessionHandler.UserName);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$rdate", date);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$processs", process);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$orderno", orderno);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$reason", reason);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$ordate", ordate);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        int result = con.ExecuteSPNonQuery("sp_Logout_Reason", true, mParam);
    }

    public void Logout(string process, string date, string orderno)
    {
        MySqlParameter[] mParam = new MySqlParameter[3];

        if (process == "PRODUCTION") process = "KEY";

        mParam[0] = new MySqlParameter("?$process", process);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$pdate", date);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$orderno", orderno);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        int result = con.ExecuteSPNonQuery("sp_Logout", true, mParam);
    }

    public void Logout_New(string process, string strlog, string strlogout)
    {
        string strdate = setdate();
        DateTime dt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", dt);

        MySqlParameter[] mParam = new MySqlParameter[5];

        mParam[0] = new MySqlParameter("?$processs", process);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$strlog", strlog);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$strlogout", strlogout);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$uid", SessionHandler.UserName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$pdate", strdate);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        int result = con.ExecuteSPNonQuery("sp_Logout_New", true, mParam);
    }

    public void insert_manual_reason(string reason, string ono)
    {
        string strdate = setdate();
        DateTime dt = Convert.ToDateTime(strdate);
        strdate = String.Format("{0:MM/dd/yyyy}", dt);

        MySqlParameter[] mParam = new MySqlParameter[4];

        mParam[0] = new MySqlParameter("?$order_no", ono);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$date", strdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$reason", reason);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        int result = con.ExecuteSPNonQuery("sp_insert_manual_reason", true, mParam);
    }
    public DataSet CountyLink(string strstate, string strcounty)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("?$strstate", strstate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$strcounty", strcounty);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_CountyLink", true, mParam);
    }

    #endregion

    #region NonAdmin
    public DataTable LoadPendingOrders(string process)
    {
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$pType", process);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        MySqlDataReader myDr = con.ExecuteSPReader("sp_Loadpendorders", true, mParam);

        DataView dataView = ConvertDataReaderToDataView(myDr);
        myDr.Close();
        return dataView.ToTable();
    }
    public DataView ConvertDataReaderToDataView(MySqlDataReader reader)
    {
        DataView dview;
        DataTable schemaTable = new DataTable();
        schemaTable = reader.GetSchemaTable();
        DataTable DtTable = new DataTable();
        DataColumn Dtcolumn;

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.Int32");
        Dtcolumn.ColumnName = "SlNo";
        Dtcolumn.Caption = "SlNo";
        Dtcolumn.ReadOnly = true;
        Dtcolumn.Unique = true;
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Order No";
        Dtcolumn.Caption = "Order No";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Date";
        Dtcolumn.Caption = "Date";
        Dtcolumn.ReadOnly = true;
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Download Time";
        Dtcolumn.Caption = "Download Time";
        DtTable.Columns.Add(Dtcolumn);

        //New
        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "AssignedDate";
        Dtcolumn.Caption = "AssignedDate";
        DtTable.Columns.Add(Dtcolumn);


        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "serpro";
        Dtcolumn.Caption = "serpro";
        DtTable.Columns.Add(Dtcolumn);

        //New

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "PTY";
        Dtcolumn.Caption = "PTY";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Zone";
        Dtcolumn.Caption = "Zone";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "State";
        Dtcolumn.Caption = "State";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "County";
        Dtcolumn.Caption = "County";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Township";
        Dtcolumn.Caption = "Township";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Type";
        Dtcolumn.Caption = "Type";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "OrderType";
        Dtcolumn.Caption = "OrderType";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Status";
        Dtcolumn.Caption = "Status";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1 Name";
        Dtcolumn.Caption = "K1 Name";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Comments";
        Dtcolumn.Caption = "Comments";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1Start Time";
        Dtcolumn.Caption = "K1Start Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1End Time";
        Dtcolumn.Caption = "K1End Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1Time Taken";
        Dtcolumn.Caption = "K1Time Taken";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QC Name";
        Dtcolumn.Caption = "QC Name";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCStart Time";
        Dtcolumn.Caption = "QCStart Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCEnd Time";
        Dtcolumn.Caption = "QCEnd Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCTime Taken";
        Dtcolumn.Caption = "QCTime Taken";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Upload Time";
        Dtcolumn.Caption = "Upload Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "TAT";
        Dtcolumn.Caption = "TAT";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Delivered Date";
        Dtcolumn.Caption = "Delivered Date";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "id";
        Dtcolumn.Caption = "id";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Post Audit";
        Dtcolumn.Caption = "Post Audit";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "View Data";
        Dtcolumn.Caption = "View Data";
        DtTable.Columns.Add(Dtcolumn);


        Int16 i = 1;
        string Status = "YTS";
        int hp;
        int lk;
        int rej;
        string test = "";
        string strcomments = "";

        string loc, k1, qc, hold, pend, tax, parcel, key_status, review, stat = "", view = "";
        if (reader.HasRows == false)
        {
            //DataRow emptyRow = DtTable.NewRow();
            //DtTable.Rows.Add(emptyRow);
            //dview = new DataView(DtTable);
            //return dview;
        }
        while (reader.Read())
        {
            Status = "YTS";
            hp = 0;
            lk = 0;
            rej = 0;


            DataRow dtRow = DtTable.NewRow();
            dtRow[0] = i;
            dtRow[1] = reader["Order_no"];
            dtRow[2] = reader["pdate"];
            if (dtRow[2] != "")
            {
                DateTime pDt = Convert.ToDateTime(dtRow[2].ToString());
                dtRow[2] = String.Format("{0:dd-MMM-yy}", pDt);
            }
            dtRow[3] = reader["Downloadtime"];
            //New
            dtRow[4] = reader["AssignedDate"];

            dtRow[5] = reader["serpro"];

            //New
            dtRow[6] = reader["prior"];
            dtRow[7] = reader["Time_Zone"];
            dtRow[8] = reader["State"];
            dtRow[9] = reader["County"];
            dtRow[10] = reader["Township"];
            dtRow[11] = reader["webphone"];
            dtRow[12] = reader["OrderType"];

            loc = reader["Lock1"].ToString();
            k1 = reader["k1"].ToString();
            qc = reader["qc"].ToString();
            review = reader["Review"].ToString();
            stat = reader["status"].ToString();
            pend = reader["pend"].ToString();
            tax = reader["tax"].ToString();
            parcel = reader["parcel"].ToString();
            key_status = reader["key_status"].ToString();
            view = reader["view_data"].ToString();

            if (loc == "1")
            { dtRow[13] = "Locked"; }
            else if (k1 == "0" && qc == "0")
            { dtRow[13] = "YTS"; }
            else if (k1 == "1" && qc == "0")
            { dtRow[13] = "Key Started"; }
            else if (k1 == "2" && qc == "0" && key_status != "Others")
            { dtRow[13] = "Key Completed"; }
            else if (k1 == "2" && qc == "0" && key_status == "Others")
            { dtRow[13] = "Others"; }
            else if (k1 == "2" && qc == "1")
            { dtRow[13] = "QC Started"; }
            else if (k1 == "5" && qc == "5" && stat == "5")
            { dtRow[13] = "Delivered"; }
            else if (k1 == "4" && qc == "4" && stat == "4")
            { dtRow[13] = "On Hold"; }
            else if (k1 == "7" && qc == "7" && stat == "7")
            { dtRow[13] = "Rejected"; }
            else if (k1 == "9" && qc == "9" && stat == "9" && review == "9")
            { dtRow[13] = "Order Missing"; }

            if (pend == "3")
            { dtRow[13] = "In Process"; }
            else if (pend == "1")
            { dtRow[13] = "In Process Started"; }

            //else if (pend == "1" && k1 == "1")
            //{ dtRow[10] = "In Process Key Start"; }
            //else if (pend == "3" && qc == "2")
            //{ dtRow[10] = "In Process"; }
            //else if (pend == "1")
            //{ dtRow[10] = "In Process"; }
            //if (tax == "3")
            //{ dtRow[9] = "Mail Away"; }
            //if (parcel == "3")
            //{ dtRow[10] = "ParcelID"; }

            if (tax == "3")
            { dtRow[13] = "Mail Away"; }
            else if (tax == "1")
            { dtRow[13] = "Mail Away Started"; }

            if (parcel == "3")
            { dtRow[13] = "ParcelID"; }
            else if (parcel == "1")
            { dtRow[13] = "ParcelID Started"; }

            dtRow[14] = reader["k1_op"];
            dtRow[15] = reader["Lastcomment"];
            dtRow[16] = reader["k1_st"];
            dtRow[17] = reader["k1_et"];

            if (dtRow[16].ToString() != "" && dtRow[17].ToString() != "")
            {
                DateTime StTime = DateTime.Parse(dtRow[16].ToString());
                DateTime EnTime = DateTime.Parse(dtRow[17].ToString());
                TimeSpan TimeDiff = EnTime.Subtract(StTime);
                dtRow[18] = TimeDiff;

            }
            dtRow[19] = reader["qc_op"];
            dtRow[20] = reader["qc_st"];
            dtRow[21] = reader["qc_et"];

            if (dtRow[20].ToString() != "" && dtRow[21].ToString() != "")
            {
                DateTime StTime = DateTime.Parse(dtRow[20].ToString());
                DateTime EnTime = DateTime.Parse(dtRow[21].ToString());
                TimeSpan TimeDiff = EnTime.Subtract(StTime);
                dtRow[22] = TimeDiff;
            }


            dtRow[23] = reader["uploadtime"];


            //if (dtRow[19].ToString() != "" && dtRow[20].ToString() != "")
            //{
            //    DateTime StTime = DateTime.Parse(dtRow[20].ToString());
            //    DateTime EnTime = DateTime.Parse(dtRow[19].ToString());
            //    TimeSpan TimeDiff = StTime.Subtract(EnTime);
            //    dtRow[21] = TimeDiff;
            //}

            //if (dtRow[9].ToString() != "Delivered") dtRow[21] = "No";
            //else
            //{
            //    string strweb = dtRow[8].ToString();
            //    string strorder = dtRow[1].ToString();
            //    ds.Dispose();
            //    ds.Reset();
            //    ds = GetTat(strorder, strweb);
            //    if (ds.Tables[0].Rows.Count > 0) dtRow[21] = Convert.ToString(ds.Tables[0].Rows[0]["TAT"]);

            //}

            string strtat = reader["TAT_Rep"].ToString();
            if (strtat == "0") dtRow[24] = "No";
            else if (strtat == "1") dtRow[24] = "Yes";
            dtRow[25] = reader["Delivered"];
            dtRow[26] = reader["id"];

            if (k1 == "5" && qc == "5" && stat == "5" && review == "5") dtRow[27] = "Yes";
            else dtRow[27] = "No";
            if (view == null)
            {
                dtRow[28] = "";
            }
            if (view == "00")
            {
                dtRow[28] = "Yes";
            }
            else if (view == "10")
            {
                dtRow[28] = "No";
            }
            else if (view == "Yes")
            {
                dtRow[28] = "Yes";
            }

            i += 1;
            DtTable.Rows.Add(dtRow);
        }
        dview = new DataView(DtTable);
        return dview;
    }

    public DataView ConvertDataReaderToUtilDataView(MySqlDataReader reader)
    {
        DataView dview;
        DataTable schemaTable = new DataTable();
        schemaTable = reader.GetSchemaTable();
        DataTable DtTable = new DataTable();
        DataColumn Dtcolumn;

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.Int32");
        Dtcolumn.ColumnName = "SlNo";
        Dtcolumn.Caption = "SlNo";
        Dtcolumn.ReadOnly = true;
        Dtcolumn.Unique = true;
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Order No";
        Dtcolumn.Caption = "Order No";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Date";
        Dtcolumn.Caption = "Date";
        Dtcolumn.ReadOnly = true;
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "State";
        Dtcolumn.Caption = "State";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "County";
        Dtcolumn.Caption = "County";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Township";
        Dtcolumn.Caption = "Township";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Type";
        Dtcolumn.Caption = "Type";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Process Type";
        Dtcolumn.Caption = "Process Type";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Status";
        Dtcolumn.Caption = "Status";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Name";
        Dtcolumn.Caption = "Name";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Start Time";
        Dtcolumn.Caption = "Start Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "End Time";
        Dtcolumn.Caption = "End Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "TAT";
        Dtcolumn.Caption = "TAT";
        DtTable.Columns.Add(Dtcolumn);

        Int16 i = 1;

        if (reader.HasRows == false)
        {

        }
        while (reader.Read())
        {
            DataRow dtRow = DtTable.NewRow();

            dtRow[0] = i;
            dtRow[1] = reader["Orderno"];
            dtRow[2] = reader["Pdate"];
            if (dtRow[2] != "")
            {
                DateTime pDt = Convert.ToDateTime(dtRow[2].ToString());
                dtRow[2] = String.Format("{0:dd-MMM-yy}", pDt);
            }
            dtRow[3] = reader["State"];
            dtRow[4] = reader["County"];
            dtRow[5] = reader["Township"];
            dtRow[6] = reader["WebPhone"];
            dtRow[7] = reader["processType"];
            dtRow[8] = reader["Orderstaus"];
            dtRow[9] = reader["Username"];
            dtRow[10] = reader["startime"];
            dtRow[11] = reader["endtime"];
            dtRow[12] = reader["Totalprocesstime"];

            i += 1;
            DtTable.Rows.Add(dtRow);
        }
        dview = new DataView(DtTable);
        return dview;
    }




    //TRACKING
    public DataView ConvertDataSetToDataViewSample1(MySqlDataReader reader)
    {
        DataView dview;
        DataTable schemaTable = new DataTable();
        schemaTable = reader.GetSchemaTable();
        DataTable DtTable = new DataTable();
        DataColumn Dtcolumn;

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.Int32");
        Dtcolumn.ColumnName = "SlNo";
        Dtcolumn.Caption = "SlNo";
        Dtcolumn.ReadOnly = true;
        Dtcolumn.Unique = false;
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Order No";
        Dtcolumn.Caption = "Order No";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Date";
        Dtcolumn.Caption = "Date";
        Dtcolumn.ReadOnly = true;
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "State";
        Dtcolumn.Caption = "State";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "County";
        Dtcolumn.Caption = "County";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "OrderType";
        Dtcolumn.Caption = "OrderType";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Download Time";
        Dtcolumn.Caption = "Download Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Status";
        Dtcolumn.Caption = "Status";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1 Name";
        Dtcolumn.Caption = "K1 Name";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1Start Time";
        Dtcolumn.Caption = "K1Start Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1End Time";
        Dtcolumn.Caption = "K1End Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1Time Taken";
        Dtcolumn.Caption = "K1Time Taken";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QC Name";
        Dtcolumn.Caption = "QC Name";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCStart Time";
        Dtcolumn.Caption = "QCStart Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCEnd Time";
        Dtcolumn.Caption = "QCEnd Time";
        DtTable.Columns.Add(Dtcolumn);


        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCTime Taken";
        Dtcolumn.Caption = "QCTime Taken";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Upload Time";
        Dtcolumn.Caption = "Upload Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Delivered Date";
        Dtcolumn.Caption = "Delivered Date";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Id";
        Dtcolumn.Caption = "Id";
        DtTable.Columns.Add(Dtcolumn);


        Int16 cnt = 1;
        string Status = "YTS";
        int hp;
        int lk;
        int rej;
        string test = "";
        string strcomments = "";

        string loc, k1, qc, hold, pend, tax, parcel, key_status, review, view, stat = "";
        if (reader.HasRows == false)
        {

        }

        while (reader.Read())
        {
            Status = "YTS";
            hp = 0;
            lk = 0;
            rej = 0;

            DataRow dtRow = DtTable.NewRow();
            dtRow[0] = cnt;
            dtRow[1] = reader["Order_no"];
            dtRow[2] = reader["pdate"];
            if (dtRow[2].ToString() != "")
            {
                DateTime pDt = Convert.ToDateTime(dtRow[2].ToString());
                dtRow[2] = String.Format("{0:dd-MMM-yy}", pDt);
            }
            dtRow[3] = reader["State"];
            dtRow[4] = reader["County"];
            dtRow[5] = reader["OrderType"];

            loc = reader["Lock1"].ToString();
            k1 = reader["k1"].ToString();
            qc = reader["qc"].ToString();
            review = reader["Review"].ToString();
            stat = reader["status"].ToString();

            if (loc == "1")
            { dtRow[7] = "Locked"; }
            else if (k1 == "0" && qc == "0")
            { dtRow[7] = "YTS"; }
            else if (k1 == "1" && qc == "0")
            { dtRow[7] = "Key Started"; }
            else if (k1 == "2" && qc == "0")
            { dtRow[7] = "Key Completed"; }
            else if (k1 == "2" && qc == "0")
            { dtRow[7] = "Others"; }
            else if (k1 == "2" && qc == "1")
            { dtRow[7] = "QC Started"; }
            else if (k1 == "5" && qc == "5" && stat == "5")
            { dtRow[7] = "Delivered"; }
            else if (k1 == "4" && qc == "4" && stat == "4")
            { dtRow[7] = "On Hold"; }
            else if (k1 == "7" && qc == "7" && stat == "7")
            { dtRow[7] = "Rejected"; }
            else if (k1 == "9" && qc == "9" && stat == "9" && review == "9")
            { dtRow[7] = "Order Missing"; }


            dtRow[9] = reader["K1_OP"];
            dtRow[10] = reader["k1_st"];
            dtRow[11] = reader["k1_et"];

            if (dtRow[10].ToString() != "" && dtRow[11].ToString() != "")
            {
                DateTime StTime = DateTime.Parse(dtRow[10].ToString());
                DateTime EnTime = DateTime.Parse(dtRow[11].ToString());
                TimeSpan TimeDiff = EnTime.Subtract(StTime);
                dtRow[12] = TimeDiff;

            }
            dtRow[13] = reader["qc_op"];
            dtRow[14] = reader["qc_st"];
            dtRow[15] = reader["qc_et"];

            if (dtRow[14].ToString() != "" && dtRow[15].ToString() != "")
            {
                DateTime StTime = DateTime.Parse(dtRow[14].ToString());
                DateTime EnTime = DateTime.Parse(dtRow[15].ToString());
                TimeSpan TimeDiff = EnTime.Subtract(StTime);
                dtRow[16] = TimeDiff;
            }

            dtRow[17] = reader["UploadTime"];
            dtRow[18] = reader["Id"];

            if (k1 == "5" && qc == "5" && stat == "5" && review == "5") dtRow[27] = "Yes";

            cnt += 1;
            DtTable.Rows.Add(dtRow);

        }
        dview = new DataView(DtTable);
        return dview;
    }
        
    

    //SAMPLE
    public DataView ConvertDataSetToDataViewSample(DataSet ds)
    {
        DataView dview;
        DataTable DtTable = new DataTable();
        DataColumn Dtcolumn;

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.Int32");
        Dtcolumn.ColumnName = "SlNo";
        Dtcolumn.Caption = "SlNo";
        Dtcolumn.ReadOnly = true;
        Dtcolumn.Unique = false;
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Order No";
        Dtcolumn.Caption = "Order No";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Date";
        Dtcolumn.Caption = "Date";
        Dtcolumn.ReadOnly = true;
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "State";
        Dtcolumn.Caption = "State";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "County";
        Dtcolumn.Caption = "County";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "OrderType";
        Dtcolumn.Caption = "OrderType";
        DtTable.Columns.Add(Dtcolumn);
       
        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Download Time";
        Dtcolumn.Caption = "Download Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Status";
        Dtcolumn.Caption = "Status";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1 Name";
        Dtcolumn.Caption = "K1 Name";
        DtTable.Columns.Add(Dtcolumn);
        
        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1Start Time";
        Dtcolumn.Caption = "K1Start Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1End Time";
        Dtcolumn.Caption = "K1End Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1Time Taken";
        Dtcolumn.Caption = "K1Time Taken";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QC Name";
        Dtcolumn.Caption = "QC Name";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCStart Time";
        Dtcolumn.Caption = "QCStart Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCEnd Time";
        Dtcolumn.Caption = "QCEnd Time";
        DtTable.Columns.Add(Dtcolumn);


        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCTime Taken";
        Dtcolumn.Caption = "QCTime Taken";
        DtTable.Columns.Add(Dtcolumn);
               
        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Upload Time";
        Dtcolumn.Caption = "Upload Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Delivered Date";
        Dtcolumn.Caption = "Delivered Date";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Id";
        Dtcolumn.Caption = "Id";
        DtTable.Columns.Add(Dtcolumn);


        Int16 cnt = 1;
        string Status = "YTS";
        int hp;
        int lk;
        int rej;
        string test = "";
        string strcomments = "";

        string loc, k1, qc, hold, pend, tax, parcel, key_status, review, view, stat = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Status = "YTS";
                hp = 0;
                lk = 0;
                rej = 0;
                                
                DataRow dtRow = DtTable.NewRow();
                dtRow[0] = cnt;
                dtRow[1] = ds.Tables[0].Rows[i]["Order_no"];
                dtRow[2] = ds.Tables[0].Rows[i]["pdate"];
                if (dtRow[2].ToString() != "")
                {
                    DateTime pDt = Convert.ToDateTime(dtRow[2].ToString());
                    dtRow[2] = String.Format("{0:dd-MMM-yy}", pDt);
                }
                dtRow[3] = ds.Tables[0].Rows[i]["State"];
                dtRow[4] = ds.Tables[0].Rows[i]["County"];                
                dtRow[5] = ds.Tables[0].Rows[i]["OrderType"];

                loc = ds.Tables[0].Rows[i]["Lock1"].ToString();
                k1 = ds.Tables[0].Rows[i]["k1"].ToString();
                qc = ds.Tables[0].Rows[i]["qc"].ToString();
                review = ds.Tables[0].Rows[i]["Review"].ToString();
                stat = ds.Tables[0].Rows[i]["status"].ToString();
                
                if (loc == "1")
                { dtRow[7] = "Locked"; }
                else if (k1 == "0" && qc == "0")
                { dtRow[7] = "YTS"; }
                else if (k1 == "1" && qc == "0")
                { dtRow[7] = "Key Started"; }
                else if (k1 == "2" && qc == "0") 
                { dtRow[7] = "Key Completed"; }
                else if (k1 == "2" && qc == "0")
                { dtRow[7] = "Others"; }
                else if (k1 == "2" && qc == "1")
                { dtRow[7] = "QC Started"; }
                else if (k1 == "5" && qc == "5" && stat == "5")
                { dtRow[7] = "Delivered"; }
                else if (k1 == "4" && qc == "4" && stat == "4")
                { dtRow[7] = "On Hold"; }
                else if (k1 == "7" && qc == "7" && stat == "7")
                { dtRow[7] = "Rejected"; }
                else if (k1 == "9" && qc == "9" && stat == "9" && review == "9")
                { dtRow[7] = "Order Missing"; }
                             
                
                dtRow[9]  = ds.Tables[0].Rows[i]["K1_OP"];                
                dtRow[10] = ds.Tables[0].Rows[i]["k1_st"];
                dtRow[11] = ds.Tables[0].Rows[i]["k1_et"];

                if (dtRow[10].ToString() != "" && dtRow[11].ToString() != "")
                {
                    DateTime StTime = DateTime.Parse(dtRow[10].ToString());
                    DateTime EnTime = DateTime.Parse(dtRow[11].ToString());
                    TimeSpan TimeDiff = EnTime.Subtract(StTime);
                    dtRow[12] = TimeDiff;

                }
                dtRow[13] = ds.Tables[0].Rows[i]["qc_op"];
                dtRow[14] = ds.Tables[0].Rows[i]["qc_st"];
                dtRow[15] = ds.Tables[0].Rows[i]["qc_et"];

                if (dtRow[14].ToString() != "" && dtRow[15].ToString() != "")
                {
                    DateTime StTime = DateTime.Parse(dtRow[14].ToString());
                    DateTime EnTime = DateTime.Parse(dtRow[15].ToString());
                    TimeSpan TimeDiff = EnTime.Subtract(StTime);
                    dtRow[16] = TimeDiff;
                }
                
                dtRow[17] = ds.Tables[0].Rows[i]["UploadTime"];
                dtRow[18] = ds.Tables[0].Rows[i]["Id"];

                if (k1 == "5" && qc == "5" && stat == "5" && review == "5") dtRow[27] = "Yes";
                                                
                cnt += 1;
                DtTable.Rows.Add(dtRow);
            }
        }
        dview = new DataView(DtTable);
        return dview;
    }




    public DataView ConvertDataSetToDataView(DataSet ds)
    {
        DataView dview;
        DataTable DtTable = new DataTable();
        DataColumn Dtcolumn;

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.Int32");
        Dtcolumn.ColumnName = "SlNo";
        Dtcolumn.Caption = "SlNo";
        Dtcolumn.ReadOnly = true;
        Dtcolumn.Unique = false;
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Order No";
        Dtcolumn.Caption = "Order No";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Date";
        Dtcolumn.Caption = "Date";
        Dtcolumn.ReadOnly = true;
        DtTable.Columns.Add(Dtcolumn);

        //New

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Download Time";
        Dtcolumn.Caption = "Download Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "AssignedDate";
        Dtcolumn.Caption = "AssignedDate";
        DtTable.Columns.Add(Dtcolumn);


        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "serpro";
        Dtcolumn.Caption = "serpro";
        DtTable.Columns.Add(Dtcolumn);

        //New

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "PTY";
        Dtcolumn.Caption = "PTY";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Zone";
        Dtcolumn.Caption = "Zone";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "State";
        Dtcolumn.Caption = "State";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "County";
        Dtcolumn.Caption = "County";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Township";
        Dtcolumn.Caption = "Township";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Type";
        Dtcolumn.Caption = "Type";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "OrderType";
        Dtcolumn.Caption = "OrderType";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Status";
        Dtcolumn.Caption = "Status";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1 Name";
        Dtcolumn.Caption = "K1 Name";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Comments";
        Dtcolumn.Caption = "Comments";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1Start Time";
        Dtcolumn.Caption = "K1Start Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1End Time";
        Dtcolumn.Caption = "K1End Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "K1Time Taken";
        Dtcolumn.Caption = "K1Time Taken";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QC Name";
        Dtcolumn.Caption = "QC Name";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCStart Time";
        Dtcolumn.Caption = "QCStart Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCEnd Time";
        Dtcolumn.Caption = "QCEnd Time";
        DtTable.Columns.Add(Dtcolumn);


        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "QCTime Taken";
        Dtcolumn.Caption = "QCTime Taken";
        DtTable.Columns.Add(Dtcolumn);

        //Dtcolumn = new DataColumn();
        //Dtcolumn.DataType = System.Type.GetType("System.String");
        //Dtcolumn.ColumnName = "Download Time";
        //Dtcolumn.Caption = "Download Time";
        //DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Upload Time";
        Dtcolumn.Caption = "Upload Time";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "TAT";
        Dtcolumn.Caption = "TAT";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Delivered Date";
        Dtcolumn.Caption = "Delivered Date";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "id";
        Dtcolumn.Caption = "id";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "Post Audit";
        Dtcolumn.Caption = "Post Audit";
        DtTable.Columns.Add(Dtcolumn);

        Dtcolumn = new DataColumn();
        Dtcolumn.DataType = System.Type.GetType("System.String");
        Dtcolumn.ColumnName = "View Data";
        Dtcolumn.Caption = "View Data";
        DtTable.Columns.Add(Dtcolumn);

        Int16 cnt = 1;
        string Status = "YTS";
        int hp;
        int lk;
        int rej;
        string test = "";
        string strcomments = "";

        string loc, k1, qc, hold, pend, tax, parcel, key_status, review, view, stat = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Status = "YTS";
                hp = 0;
                lk = 0;
                rej = 0;


                DataRow dtRow = DtTable.NewRow();
                dtRow[0] = cnt;
                dtRow[1] = ds.Tables[0].Rows[i]["Order_no"];
                dtRow[2] = ds.Tables[0].Rows[i]["pdate"];
                if (dtRow[2].ToString() != "")
                {
                    DateTime pDt = Convert.ToDateTime(dtRow[2].ToString());
                    dtRow[2] = String.Format("{0:dd-MMM-yy}", pDt);
                }
                dtRow[3] = ds.Tables[0].Rows[i]["Downloadtime"];
                dtRow[4] = ds.Tables[0].Rows[i]["AssignedDate"];
                dtRow[5] = ds.Tables[0].Rows[i]["serpro"];
                dtRow[6] = ds.Tables[0].Rows[i]["prior"];
                dtRow[7] = ds.Tables[0].Rows[i]["Time_Zone"];
                dtRow[8] = ds.Tables[0].Rows[i]["State"];
                dtRow[9] = ds.Tables[0].Rows[i]["County"];
                dtRow[10] = ds.Tables[0].Rows[i]["Township"];
                dtRow[11] = ds.Tables[0].Rows[i]["webphone"];
                dtRow[12] = ds.Tables[0].Rows[i]["OrderType"];

                loc = ds.Tables[0].Rows[i]["Lock1"].ToString();
                k1 = ds.Tables[0].Rows[i]["k1"].ToString();
                qc = ds.Tables[0].Rows[i]["qc"].ToString();
                review = ds.Tables[0].Rows[i]["Review"].ToString();
                stat = ds.Tables[0].Rows[i]["status"].ToString();
                pend = ds.Tables[0].Rows[i]["pend"].ToString();
                tax = ds.Tables[0].Rows[i]["tax"].ToString();
                parcel = ds.Tables[0].Rows[i]["parcel"].ToString();
                key_status = ds.Tables[0].Rows[i]["key_status"].ToString();
                view = ds.Tables[0].Rows[i]["view_data"].ToString();

                if (loc == "1")
                { dtRow[13] = "Locked"; }
                else if (k1 == "0" && qc == "0")
                { dtRow[13] = "YTS"; }
                else if (k1 == "1" && qc == "0")
                { dtRow[13] = "Key Started"; }
                else if (k1 == "2" && qc == "0" && key_status != "Others")
                { dtRow[13] = "Key Completed"; }
                else if (k1 == "2" && qc == "0" && key_status == "Others")
                { dtRow[13] = "Others"; }
                else if (k1 == "2" && qc == "1")
                { dtRow[13] = "QC Started"; }
                else if (k1 == "5" && qc == "5" && stat == "5")
                { dtRow[13] = "Delivered"; }
                else if (k1 == "4" && qc == "4" && stat == "4")
                { dtRow[13] = "On Hold"; }
                else if (k1 == "7" && qc == "7" && stat == "7")
                { dtRow[13] = "Rejected"; }
                else if (k1 == "9" && qc == "9" && stat == "9" && review == "9")
                { dtRow[13] = "Order Missing"; }

                if (pend == "3")
                { dtRow[13] = "In Process"; }
                else if (pend == "1")
                { dtRow[13] = "In Process Started"; }

                if (tax == "3")
                { dtRow[13] = "Mail Away"; }
                else if (tax == "1")
                { dtRow[13] = "Mail Away Started"; }

                if (parcel == "3")
                { dtRow[13] = "ParcelID"; }
                else if (parcel == "1")
                { dtRow[13] = "ParcelID Started"; }

                dtRow[14] = ds.Tables[0].Rows[i]["k1_op"];
                dtRow[15] = ds.Tables[0].Rows[i]["Lastcomment"];
                dtRow[16] = ds.Tables[0].Rows[i]["k1_st"];
                dtRow[17] = ds.Tables[0].Rows[i]["k1_et"];

                if (dtRow[16].ToString() != "" && dtRow[17].ToString() != "")
                {
                    DateTime StTime = DateTime.Parse(dtRow[16].ToString());
                    DateTime EnTime = DateTime.Parse(dtRow[17].ToString());
                    TimeSpan TimeDiff = EnTime.Subtract(StTime);
                    dtRow[18] = TimeDiff;

                }
                dtRow[19] = ds.Tables[0].Rows[i]["qc_op"];
                dtRow[20] = ds.Tables[0].Rows[i]["qc_st"];
                dtRow[21] = ds.Tables[0].Rows[i]["qc_et"];

                if (dtRow[20].ToString() != "" && dtRow[21].ToString() != "")
                {
                    DateTime StTime = DateTime.Parse(dtRow[20].ToString());
                    DateTime EnTime = DateTime.Parse(dtRow[21].ToString());
                    TimeSpan TimeDiff = EnTime.Subtract(StTime);
                    dtRow[22] = TimeDiff;

                }
                //dtRow[19] = ds.Tables[0].Rows[i]["DownloadtimeEST"];
                dtRow[23] = ds.Tables[0].Rows[i]["uploadtime"];

                string strtat = ds.Tables[0].Rows[i]["TAT_Rep"].ToString();
                if (strtat == "0") dtRow[24] = "No";
                else if (strtat == "1") dtRow[24] = "Yes";

                //if (dtRow[19].ToString() != "" && dtRow[20].ToString() != "")
                //{
                //    DateTime StTime = DateTime.Parse(dtRow[20].ToString());
                //    DateTime EnTime = DateTime.Parse(dtRow[19].ToString());
                //    TimeSpan TimeDiff = StTime.Subtract(EnTime);
                //    dtRow[21] = TimeDiff;
                //}
                //if (dtRow[9].ToString() != "Delivered") dtRow[21] = "No";
                //else
                //{
                //    string strweb = dtRow[8].ToString();
                //    string strorder = dtRow[1].ToString();
                //    ds1.Dispose();
                //    ds1.Reset();
                //    ds1 = GetTat(strorder, strweb);
                //    if (ds1.Tables[0].Rows.Count > 0) dtRow[21] = Convert.ToString(ds1.Tables[0].Rows[0]["TAT"]);

                //}
                dtRow[25] = ds.Tables[0].Rows[i]["Delivered"];
                dtRow[26] = ds.Tables[0].Rows[i]["id"];

                if (k1 == "5" && qc == "5" && stat == "5" && review == "5") dtRow[27] = "Yes";
                else dtRow[27] = "No";

                if (view == null)
                {
                    dtRow[28] = "";
                }
                if (view == "00")
                {
                    dtRow[28] = "Yes";
                }
                else if (view == "10")
                {
                    dtRow[28] = "No";
                }
                else if (view == "Yes")
                {
                    dtRow[28] = "Yes";
                }
                cnt += 1;
                DtTable.Rows.Add(dtRow);
            }
        }
        dview = new DataView(DtTable);
        return dview;
    }
    #endregion

    #region Tracking
    public MySqlDataReader Tracking(string procedure, string fdate, string tdate, string status)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$status", status);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        MySqlDataReader myDr = con.ExecuteSPReader(procedure, true, mParam);
        return myDr;
    }

    public MySqlDataReader PostTracking(string procedure, string fdate, string tdate, string status, string struser)
    {
        mParam = new MySqlParameter[4];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$status", status);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$struser", struser);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        MySqlDataReader myDr = con.ExecuteSPReader(procedure, true, mParam);
        return myDr;
    }

    public MySqlDataReader Util_Tracking(string fdate, string tdate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        MySqlDataReader myDr = con.ExecuteSPReader("sp_Utilization_Tracking", true, mParam);
        return myDr;
    }

    public DataSet TrackingSearch(string strordersearch)
    {
        mParam = new MySqlParameter[1];

        mParam[0] = new MySqlParameter("?$order_no", strordersearch);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        ds = con.ExecuteQuery("sp_OrderSearch", true, mParam);
        return ds;
    }

    public DataSet FetchOrderDetails(string strordersearch)
    {
        mParam = new MySqlParameter[1];

        mParam[0] = new MySqlParameter("?$orderno", strordersearch);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        ds = con.ExecuteQuery("Sp_order_details_search", true, mParam);
        return ds;
    }

   

    public DataSet GetTat(string strorder, string strweb)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$order_no", strorder);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$webtype", strweb);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;


        return con.ExecuteQuery("sp_Gettatdetails", true, mParam);

    }
    public DataSet GetOrderCountAll()
    {
        mParam = new MySqlParameter[0];
        return con.ExecuteQuery("sp_getOrdercountall", true, mParam);
    }
    public DataSet GetOrderCountAll_scrape()
    {
        mParam = new MySqlParameter[0];
        return con.ExecuteQuery("sp_getOrdercountall_scrape", true, mParam);
    }

    public DataSet GetOrderCount(string strfrmdate, string strtodate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", strfrmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strtodate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_getOrdercount", true, mParam);

    }
    public DataSet GetOrderCount_scrape(string strfrmdate, string strtodate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", strfrmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strtodate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_getOrdercount_scrape", true, mParam);

    }
    #endregion

    #region EOD Report

    public DataView CovertEODDstoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;
            string k1, qc, status, parcel, tax, pend = "";
            string strcomments = "";

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order Date";
            dcolumn.Caption = "Order Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order No";
            dcolumn.Caption = "Order No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Type";
            dcolumn.Caption = "Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Borrower Name";
            dcolumn.Caption = "Borrower Name";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "State";
            dcolumn.Caption = "State";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "County";
            dcolumn.Caption = "County";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Muncipality";
            dcolumn.Caption = "Muncipality";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Delivered Date";
            dcolumn.Caption = "Delivered Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Status";
            dcolumn.Caption = "Status";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "String Comments";
            dcolumn.Caption = "String Comments";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "TSI Feedback";
            dcolumn.Caption = "TSI Feedback";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "serpro";
            dcolumn.Caption = "serpro";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Tier";
            dcolumn.Caption = "Tier";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    k1 = ds.Tables[0].Rows[i]["K1"].ToString();
                    qc = ds.Tables[0].Rows[i]["QC"].ToString();
                    pend = ds.Tables[0].Rows[i]["Pend"].ToString();
                    parcel = ds.Tables[0].Rows[i]["Parcel"].ToString();
                    tax = ds.Tables[0].Rows[i]["Tax"].ToString();
                    status = ds.Tables[0].Rows[i]["Status"].ToString();


                    dtrow[0] = ds.Tables[0].Rows[i]["pdate"];
                    if (dtrow[0].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[0].ToString());
                        dtrow[0] = String.Format("{0:dd-MMM-yyyy}", pdt);
                    }
                    dtrow[1] = ds.Tables[0].Rows[i]["Order_No"];
                    dtrow[2] = ds.Tables[0].Rows[i]["Webphone"];
                    dtrow[3] = ds.Tables[0].Rows[i]["Borrowername"];
                    dtrow[4] = ds.Tables[0].Rows[i]["State"];
                    dtrow[5] = ds.Tables[0].Rows[i]["County"];
                    dtrow[6] = ds.Tables[0].Rows[i]["Township"];
                    dtrow[7] = ds.Tables[0].Rows[i]["DeliveredDate"];
                    if (dtrow[7].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[7].ToString());
                        dtrow[7] = String.Format("{0:dd-MMM-yyyy}", pdt);
                    }
                    if (k1 == "5" && qc == "5" && status == "5")
                    {
                        dtrow[8] = "Completed";
                    }
                    else if (k1 == "2" && qc == "2" && status == "2" && pend == "0" && parcel == "0" && tax == "3")
                    {
                        dtrow[8] = "Mail away";
                    }
                    else if (k1 == "2" && qc == "2" && status == "2" && pend == "3" && parcel == "0" && tax == "0")
                    {
                        dtrow[8] = "In process";
                    }
                    else if (k1 == "2" && qc == "2" && status == "2" && pend == "0" && parcel == "3" && tax == "0")
                    {
                        dtrow[8] = "Parcel ID";
                    }
                    else if (k1 == "4" && qc == "4" && status == "4" && pend == "0" && parcel == "0" && tax == "0")
                    {
                        dtrow[8] = "On Hold";
                    }
                    else if (k1 == "7" && qc == "7" && status == "7" && pend == "0" && parcel == "0" && tax == "0")
                    {
                        dtrow[8] = "Rejected";
                    }

                    //dtrow[9] = ds.Tables[0].Rows[i]["Lastcomment"];

                    strcomments = ds.Tables[0].Rows[i]["Lastcomment"].ToString();
                    string[] strcmd = strcomments.Split(':');
                    if (strcmd.Length == 1) { dtrow[9] = ds.Tables[0].Rows[i]["Lastcomment"].ToString(); }
                    else
                    {
                        string[] strdatesplit = strcmd[0].ToString().Split(' ');
                        string[] stryearsplit = strdatesplit[0].ToString().Split('/');
                        dtrow[9] = Convert.ToString(stryearsplit[0] + "/" + stryearsplit[1] + " - " + strcmd[3]);
                    }

                    dtrow[10] = "";
                    dtrow[11] = ds.Tables[0].Rows[i]["serpro"];
                    dtrow[12] = ds.Tables[0].Rows[i]["Tier"];
                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }

    public DataView CovertEODDstoDataview1(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;
            string k1, qc, status, parcel, tax, pend = "";
            string strcomments = "";

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order Date";
            dcolumn.Caption = "Order Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order No";
            dcolumn.Caption = "Order No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Type";
            dcolumn.Caption = "Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Borrower Name";
            dcolumn.Caption = "Borrower Name";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "State";
            dcolumn.Caption = "State";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "County";
            dcolumn.Caption = "County";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Muncipality";
            dcolumn.Caption = "Muncipality";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Status";
            dcolumn.Caption = "Status";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "String Comments";
            dcolumn.Caption = "String Comments";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "TSI Feedback";
            dcolumn.Caption = "TSI Feedback";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "serpro";
            dcolumn.Caption = "serpro";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Tier";
            dcolumn.Caption = "Tier";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    k1 = ds.Tables[0].Rows[i]["K1"].ToString();
                    qc = ds.Tables[0].Rows[i]["QC"].ToString();
                    pend = ds.Tables[0].Rows[i]["Pend"].ToString();
                    parcel = ds.Tables[0].Rows[i]["Parcel"].ToString();
                    tax = ds.Tables[0].Rows[i]["Tax"].ToString();
                    status = ds.Tables[0].Rows[i]["Status"].ToString();


                    dtrow[0] = ds.Tables[0].Rows[i]["pdate"];
                    if (dtrow[0].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[0].ToString());
                        dtrow[0] = String.Format("{0:dd-MMM-yyyy}", pdt);
                    }
                    dtrow[1] = ds.Tables[0].Rows[i]["Order_No"];
                    dtrow[2] = ds.Tables[0].Rows[i]["Webphone"];
                    dtrow[3] = ds.Tables[0].Rows[i]["Borrowername"];
                    dtrow[4] = ds.Tables[0].Rows[i]["State"];
                    dtrow[5] = ds.Tables[0].Rows[i]["County"];
                    dtrow[6] = ds.Tables[0].Rows[i]["Township"];

                    if (k1 == "5" && qc == "5" && status == "5")
                    {
                        dtrow[7] = "Completed";
                    }
                    else if (k1 == "2" && qc == "2" && status == "2" && pend == "0" && parcel == "0" && tax == "3")
                    {
                        dtrow[7] = "Mail away";
                    }
                    else if (k1 == "2" && qc == "2" && status == "2" && pend == "3" && parcel == "0" && tax == "0")
                    {
                        dtrow[7] = "In process";
                    }
                    else if (k1 == "2" && qc == "2" && status == "2" && pend == "0" && parcel == "3" && tax == "0")
                    {
                        dtrow[7] = "Parcel ID";
                    }
                    else if (k1 == "4" && qc == "4" && status == "4" && pend == "0" && parcel == "0" && tax == "0")
                    {
                        dtrow[7] = "On Hold";
                    }
                    else if (k1 == "7" && qc == "7" && status == "7" && pend == "0" && parcel == "0" && tax == "0")
                    {
                        dtrow[7] = "Rejected";
                    }

                    //dtrow[8] = ds.Tables[0].Rows[i]["Lastcomment"];

                    strcomments = ds.Tables[0].Rows[i]["Lastcomment"].ToString();
                    string[] strcmd = strcomments.Split(':');
                    if (strcmd.Length == 1) { dtrow[8] = ds.Tables[0].Rows[i]["Lastcomment"].ToString(); }
                    else
                    {
                        string[] strdatesplit = strcmd[0].ToString().Split(' ');
                        string[] stryearsplit = strdatesplit[0].ToString().Split('/');
                        dtrow[8] = Convert.ToString(stryearsplit[0] + "/" + stryearsplit[1] + " - " + strcmd[3]);
                    }

                    dtrow[9] = "";
                    dtrow[10] = ds.Tables[0].Rows[i]["serpro"];
                    dtrow[11] = ds.Tables[0].Rows[i]["Tier"];
                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }

    #endregion

    #region Quality Report

    public DataView CovertQualityDStoDataview(DataSet ds, string Process)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Processed Date";
            dcolumn.Caption = "Processed Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order No";
            dcolumn.Caption = "Order No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "State";
            dcolumn.Caption = "State";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "County";
            dcolumn.Caption = "County";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (Process == "Review")
            {

                dcolumn = new DataColumn();
                dcolumn.DataType = System.Type.GetType("System.String");
                dcolumn.ColumnName = "Processing Agent";
                dcolumn.Caption = "Processing Agent";
                dcolumn.ReadOnly = true;
                dcolumn.Unique = false;
                dtTable.Columns.Add(dcolumn);

                dcolumn = new DataColumn();
                dcolumn.DataType = System.Type.GetType("System.String");
                dcolumn.ColumnName = "QC Agent";
                dcolumn.Caption = "QC Agent";
                dcolumn.ReadOnly = true;
                dcolumn.Unique = false;
                dtTable.Columns.Add(dcolumn);
            }
            else
            {
                dcolumn = new DataColumn();
                dcolumn.DataType = System.Type.GetType("System.String");
                dcolumn.ColumnName = "Processing Agent";
                dcolumn.Caption = "Processing Agent";
                dcolumn.ReadOnly = true;
                dcolumn.Unique = false;
                dtTable.Columns.Add(dcolumn);
            }

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Delivered Date";
            dcolumn.Caption = "Delivered Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Error Status";
            dcolumn.Caption = "Error Status";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Error Field";
            dcolumn.Caption = "Error Field";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Type";
            dcolumn.Caption = "Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Correct";
            dcolumn.Caption = "Correct";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "InCorrect";
            dcolumn.Caption = "InCorrect";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (Process == "Qc")
            {
                dcolumn = new DataColumn();
                dcolumn.DataType = System.Type.GetType("System.String");
                dcolumn.ColumnName = "QC Agent";
                dcolumn.Caption = "QC Agent";
                dcolumn.ReadOnly = true;
                dcolumn.Unique = false;
                dtTable.Columns.Add(dcolumn);
            }
            else
            {
                dcolumn = new DataColumn();
                dcolumn.DataType = System.Type.GetType("System.String");
                dcolumn.ColumnName = "PostAudit Agent";
                dcolumn.Caption = "PostAudit Agent";
                dcolumn.ReadOnly = true;
                dcolumn.Unique = false;
                dtTable.Columns.Add(dcolumn);
            }

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Comments";
            dcolumn.Caption = "Comments";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    dtrow[0] = ds.Tables[0].Rows[i]["pdate"];
                    if (dtrow[0].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[0].ToString());
                        dtrow[0] = String.Format("{0:dd-MMM-yyyy}", pdt);
                    }
                    dtrow[1] = ds.Tables[0].Rows[i]["Order_No"];
                    dtrow[2] = ds.Tables[0].Rows[i]["State"];
                    dtrow[3] = ds.Tables[0].Rows[i]["County"];

                    if (Process == "Qc")
                    {
                        dtrow[4] = ds.Tables[0].Rows[i]["k1_op"];
                        dtrow[5] = ds.Tables[0].Rows[i]["delivereddate"];
                        dtrow[6] = ds.Tables[0].Rows[i]["error"];
                        dtrow[7] = ds.Tables[0].Rows[i]["errorfield"];
                        dtrow[8] = ds.Tables[0].Rows[i]["webphone"];
                        dtrow[9] = ds.Tables[0].Rows[i]["correct"];
                        dtrow[10] = ds.Tables[0].Rows[i]["incorrect"];
                        dtrow[11] = ds.Tables[0].Rows[i]["qc_op"];
                        dtrow[12] = ds.Tables[0].Rows[i]["lastcomment"];
                    }
                    else if (Process == "Review")
                    {
                        dtrow[4] = ds.Tables[0].Rows[i]["k1_op"];
                        dtrow[5] = ds.Tables[0].Rows[i]["qc_op"];
                        dtrow[6] = ds.Tables[0].Rows[i]["delivereddate"];
                        dtrow[7] = ds.Tables[0].Rows[i]["error1"];
                        dtrow[8] = ds.Tables[0].Rows[i]["errorfield1"];
                        dtrow[9] = ds.Tables[0].Rows[i]["webphone"];
                        dtrow[10] = ds.Tables[0].Rows[i]["correct1"];
                        dtrow[11] = ds.Tables[0].Rows[i]["incorrect1"];
                        dtrow[12] = ds.Tables[0].Rows[i]["Review_op"];
                        dtrow[13] = ds.Tables[0].Rows[i]["lastcomment"];
                    }

                    dtTable.Rows.Add(dtrow);

                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }

    public DataView CovertQualityDStoDataview1(DataSet ds, string strfrmdate, string strtodate)
    {
        string temp = string.Empty;
        double noproname = 0;
        double noerrfield = 0;
        double percentage = 0;
        double percen_round = 0;
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Processor";
            dcolumn.Caption = "Processor";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "No of Pre-Audited Orders";
            dcolumn.Caption = "No of Pre-Audited Orders";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "No Errors";
            dcolumn.Caption = "No Errors";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Errors";
            dcolumn.Caption = "Errors";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Quality Percentage";
            dcolumn.Caption = "Quality Percentage";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if ((temp.ToString() != "") && temp.ToString() == ds.Tables[0].Rows[i]["K1_OP"].ToString())
                    {

                    }
                    else
                    {
                        DataRow dtrow = dtTable.NewRow();
                        temp = ds.Tables[0].Rows[i]["K1_OP"].ToString();
                        dtrow[0] = temp;
                        if (temp != "")
                        {
                            mParam = new MySqlParameter[3];
                            mParam[0] = new MySqlParameter("?$processername", temp);
                            mParam[0].MySqlDbType = MySqlDbType.VarChar;

                            mParam[1] = new MySqlParameter("?$fromdate", strfrmdate);
                            mParam[1].MySqlDbType = MySqlDbType.VarChar;

                            mParam[2] = new MySqlParameter("?$todate", strtodate);
                            mParam[2].MySqlDbType = MySqlDbType.VarChar;

                            ds1 = con.ExecuteQuery("sp_QualitySheetReport", true, mParam);
                            if (ds1 != null)
                            {
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    dtrow[1] = ds1.Tables[0].Rows[0]["ProcessorName"].ToString();
                                    noproname = Convert.ToDouble(ds1.Tables[0].Rows[0]["ProcessorName"]);
                                }
                                if (ds1.Tables[1].Rows.Count > 0)
                                {
                                    dtrow[2] = ds1.Tables[1].Rows[0]["NoErrorField"].ToString();
                                    noerrfield = Convert.ToDouble(ds1.Tables[1].Rows[0]["NoErrorField"]);
                                }
                                if (ds1.Tables[2].Rows.Count > 0)
                                {
                                    dtrow[3] = ds1.Tables[2].Rows[0]["ErrorField"].ToString();
                                }
                                percentage = (noerrfield / noproname) * 100;
                                //dtrow[4] = Convert.ToString(percentage + "%");
                                percen_round = Math.Round(percentage, 2);
                                dtrow[4] = Convert.ToString(percen_round + "%");
                            }
                        }
                        dtTable.Rows.Add(dtrow);
                    }
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }

    #endregion

    #region Mailaway Report
    public DataView CovertMailawayDStoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Date Recevied";
            dcolumn.Caption = "Date Recevied";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order No";
            dcolumn.Caption = "Order No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Request Type";
            dcolumn.Caption = "Request Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Follow Up Date";
            dcolumn.Caption = "Follow Up Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Tax Collector";
            dcolumn.Caption = "Tax Collector";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Charge";
            dcolumn.Caption = "Charge";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Mailed Date";
            dcolumn.Caption = "Mailed Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Fax Time";
            dcolumn.Caption = "Fax TIme";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Invoice Status";
            dcolumn.Caption = "Invoice Status";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Check #";
            dcolumn.Caption = "Check #";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Parcel ID";
            dcolumn.Caption = "Parcel ID";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Owner Name";
            dcolumn.Caption = "Owner Name";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Tracking #";
            dcolumn.Caption = "Tracking #";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Fax/Email Number";
            dcolumn.Caption = "Fax/Email Number";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Type Of Request";
            dcolumn.Caption = "Type Of Request";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Comments";
            dcolumn.Caption = "Comments";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            string req_type = string.Empty;
            string stramount = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    dtrow[0] = ds.Tables[0].Rows[i]["pDate"];
                    if (dtrow[0].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[0].ToString());
                        dtrow[0] = String.Format("{0:dd-MMM-yy}", pdt);
                    }
                    dtrow[1] = ds.Tables[0].Rows[i]["Order_no"];
                    req_type = ds.Tables[0].Rows[i]["Req_Type"].ToString();
                    if (req_type == "THANKS REQUEST" || req_type == "REGULAR") dtrow[2] = "Regular";
                    else if (req_type == "UPS/SASE") dtrow[2] = "UPS";
                    else dtrow[2] = req_type;

                    dtrow[3] = ds.Tables[0].Rows[i]["Followup_date"];
                    if (dtrow[3].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[3].ToString());
                        dtrow[3] = String.Format("{0:MM/dd/yy}", pdt);
                    }
                    dtrow[4] = ds.Tables[0].Rows[i]["Cheque_payable"];
                    stramount = ds.Tables[0].Rows[i]["Amount"].ToString();
                    if (stramount == "") { stramount = "0"; }
                    string[] stramt = stramount.Split('.');
                    if (stramt.Length == 1) { dtrow[5] = "$" + stramount + ".00"; } else { dtrow[5] = "$" + stramount; }
                    dtrow[6] = ds.Tables[0].Rows[i]["Mailaway_date"];
                    if (dtrow[6].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[6].ToString());
                        dtrow[6] = String.Format("{0:MM/dd/yy}", pdt);
                    }
                    dtrow[7] = "";
                    dtrow[8] = "";
                    dtrow[9] = ds.Tables[0].Rows[i]["ChequeNo"];
                    dtrow[10] = ds.Tables[0].Rows[i]["ParcelId"];
                    dtrow[11] = ds.Tables[0].Rows[i]["Borrowername"];
                    dtrow[12] = ds.Tables[0].Rows[i]["TrackingNo"];
                    dtrow[13] = "";
                    if (req_type == "UPS" || req_type == "USPS" || req_type == "THANKS REQUEST" || req_type == "REGULAR")
                    {
                        dtrow[14] = "Check Only";
                    }
                    else if (req_type == "UPS/SASE") { dtrow[14] = "Self Addressed Stamped Envelope"; }
                    else if (req_type == "UPS/R") { dtrow[14] = "Return UPS"; }

                    if (dtrow[12].ToString() != "")
                    {
                        if (req_type == "UPS") dtrow[15] = "UPS Tracking number - " + dtrow[12].ToString() + "";
                        else if (req_type == "UPS/R") dtrow[15] = "UPS Tracking number - " + dtrow[12].ToString() + "\r\nReturn tracking number  - " + dtrow[12].ToString() + "";
                    }
                    else dtrow[15] = "";

                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }
    #endregion

    #region Consolidated Report
    public DataView CovertRegularDStoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order No";
            dcolumn.Caption = "Order No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Processname";
            dcolumn.Caption = "Processname";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Request Type";
            dcolumn.Caption = "Request Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Tax Collector";
            dcolumn.Caption = "Tax Collector";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Charges";
            dcolumn.Caption = "Charges";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Cheque Required";
            dcolumn.Caption = "Cheque Required";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Type Of Request";
            dcolumn.Caption = "Type Of Request";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string order_no = string.Empty;
                    string req_type = string.Empty;
                    string stramount = string.Empty;
                    StringBuilder sb = new StringBuilder();

                    DataRow dtrow = dtTable.NewRow();
                    order_no = ds.Tables[0].Rows[i]["Order_no"].ToString();
                    for (int count = 0; count < order_no.Length; count++)
                    {
                        if (count % 3 == 0 && count != 0)
                        {
                            sb.Append('-');
                        }
                        sb.Append(order_no[count]);
                    }


                    //dtrow[1] = ds.Tables[0].Rows[i]["Processname"];
                    //if (req_type == "REGULAR")
                    //{
                    //    dtrow[2] = "TSI";
                    //}
                    dtrow[0] = sb.ToString();
                    //dtrow[1] = ds.Tables[0].Rows[i]["Processname"].ToString();
                    dtrow[1] = "TSI-TAXES";
                    req_type = ds.Tables[0].Rows[i]["Req_Type"].ToString();
                    if (req_type == "THANKS REQUES" || req_type == "REGULAR")
                    {
                        dtrow[2] = "Regular";
                    }
                    else
                    {
                        dtrow[2] = req_type;
                    }

                    string[] strph = ds.Tables[0].Rows[i]["Cheque_payable"].ToString().Replace("p:", "~").Replace("P:", "~").Replace("ph:", "~").Replace("Ph:", "~").Replace("PH:", "~").Replace("Phone:", "~").Replace("PHONE:", "~").Split('~');
                    dtrow[3] = strph[0].ToString();

                    stramount = ds.Tables[0].Rows[i]["Amount"].ToString();
                    if (stramount == "") { stramount = "0"; }
                    string[] stramt = stramount.Split('.');
                    if (stramt.Length == 1) { dtrow[4] = "$" + stramount + ".00"; } else { dtrow[4] = "$" + stramount; }

                    if (dtrow[4].ToString() == "$0.00") { dtrow[5] = "NO"; }
                    else { dtrow[5] = "YES"; }
                    dtrow[6] = ds.Tables[0].Rows[i]["Return_Type"];
                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }

    public DataView CovertUPSDStoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order No";
            dcolumn.Caption = "Order No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Property Owner Name";
            dcolumn.Caption = "Property Owner Name";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Property Address";
            dcolumn.Caption = "Property Address";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Processname";
            dcolumn.Caption = "Processname";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Request Type";
            dcolumn.Caption = "Request Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "To be sent to";
            dcolumn.Caption = "To be sent to";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Charges";
            dcolumn.Caption = "Charges";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Cheque Required";
            dcolumn.Caption = "Cheque Required";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Comments";
            dcolumn.Caption = "Comments";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "UPS Tracking number to be filled in by T-Rex";
            dcolumn.Caption = "UPS Tracking number to be filled in by T-Rex";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string order_no = string.Empty;
                    string req_type = string.Empty;
                    string strreq_type = string.Empty;
                    string stramount = string.Empty;
                    string straddress = string.Empty;
                    StringBuilder sb = new StringBuilder();

                    DataRow dtrow = dtTable.NewRow();
                    order_no = ds.Tables[0].Rows[i]["Order_no"].ToString();
                    req_type = ds.Tables[0].Rows[i]["Req_Type"].ToString();
                    dtrow[1] = ds.Tables[0].Rows[i]["Property Owner Name"].ToString();
                    dtrow[2] = ds.Tables[0].Rows[i]["Property Address"].ToString();

                    for (int count = 0; count < order_no.Length; count++)
                    {
                        if (count % 3 == 0 && count != 0)
                        {
                            sb.Append('-');
                        }
                        sb.Append(order_no[count]);
                    }
                    strreq_type = req_type.Replace("/", "-");
                    if (req_type == "UPS/SASE") strreq_type = "UPS";
                    dtrow[0] = strreq_type + "-" + sb.ToString();
                    dtrow[3] = "TSI-TAXES";
                    if (req_type == "UPS/SASE") dtrow[4] = "UPS";
                    else dtrow[4] = req_type;
                    straddress = ds.Tables[0].Rows[i]["Cheque_payable"].ToString() + ", " + ds.Tables[0].Rows[i]["Address"].ToString();
                    string[] arraddress = straddress.Split(new string[] { "Ph:" }, StringSplitOptions.None);
                    dtrow[5] = arraddress[0];

                    stramount = ds.Tables[0].Rows[i]["Amount"].ToString();
                    if (stramount == "") { stramount = "0"; }
                    string[] stramt = stramount.Split('.');
                    if (stramt.Length == 1) { dtrow[6] = "$" + stramount + ".00"; } else { dtrow[6] = "$" + stramount; }

                    if (dtrow[6].ToString() == "$0.00") { dtrow[6] = "NO"; }
                    else { dtrow[7] = "YES"; }

                    //dtrow[5] = ds.Tables[0].Rows[i]["Return_Type"];
                    if (req_type == "UPS" || req_type == "USPS" || req_type == "THANKS REQUEST")
                    {
                        dtrow[8] = "Check Only";
                    }
                    else if (req_type == "UPS/R") { dtrow[8] = "Return UPS"; }
                    else if (req_type == "UPS/SASE") { dtrow[8] = "Self Addressed Stamped Envelope"; }

                    dtrow[9] = "";
                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }

    public DataView CovertUPSTempDStoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order No";
            dcolumn.Caption = "Order No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Cheque Payable";
            dcolumn.Caption = "Cheque Payable";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string order_no = string.Empty;
                    string req_type = string.Empty;
                    string strreq_type = string.Empty;
                    string stramount = string.Empty;
                    string straddress = string.Empty;

                    StringBuilder sb = new StringBuilder();

                    DataRow dtrow = dtTable.NewRow();
                    order_no = ds.Tables[0].Rows[i]["Order_no"].ToString();
                    req_type = ds.Tables[0].Rows[i]["Req_Type"].ToString();
                    for (int count = 0; count < order_no.Length; count++)
                    {
                        if (count % 3 == 0 && count != 0)
                        {
                            sb.Append('-');
                        }
                        sb.Append(order_no[count]);
                    }
                    strreq_type = req_type.Replace("/", "-");
                    if (req_type == "UPS/SASE") strreq_type = "UPS";
                    dtrow[0] = strreq_type + "-" + sb.ToString();
                    dtrow[1] = ds.Tables[0].Rows[i]["Cheque_payable"];

                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }
    #endregion

    #region Invoice Report
    public DataView CovertInvoiceDStoDataview(DataSet ds, string strfrmdate, string strtodate)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Request Type";
            dcolumn.Caption = "Request Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Taxing Authority Charges";
            dcolumn.Caption = "Taxing Authority Charges";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Postage Charges";
            dcolumn.Caption = "Postage Charges";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Received Date";
            dcolumn.Caption = "Received Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Delivered Date";
            dcolumn.Caption = "Delivered Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Reference No";
            dcolumn.Caption = "Reference No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Borrower's Name";
            dcolumn.Caption = "Borrower's Name";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "State";
            dcolumn.Caption = "State";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "County";
            dcolumn.Caption = "County";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Status";
            dcolumn.Caption = "Status";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Total Fee";
            dcolumn.Caption = "Total Fee";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Remark";
            dcolumn.Caption = "Remark";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            string k1 = "", qc = "", status = "", tax = "", strtotalfee = "";
            double totalfee = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    k1 = ds.Tables[0].Rows[i]["k1"].ToString();
                    qc = ds.Tables[0].Rows[i]["qc"].ToString();
                    status = ds.Tables[0].Rows[i]["status"].ToString();
                    tax = ds.Tables[0].Rows[i]["tax"].ToString();

                    dtrow[0] = ds.Tables[0].Rows[i]["WebPhone"];
                    dtrow[1] = "";
                    dtrow[2] = "";
                    dtrow[3] = ds.Tables[0].Rows[i]["pdate"];
                    if (dtrow[3].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[3].ToString());
                        dtrow[3] = String.Format("{0:dd/MM/yy}", pdt);
                    }
                    dtrow[4] = ds.Tables[0].Rows[i]["DeliveredDate"];
                    if (dtrow[4].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[4].ToString());
                        dtrow[4] = String.Format("{0:MM/dd/yy}", pdt);
                    }
                    dtrow[5] = ds.Tables[0].Rows[i]["Order_No"];
                    dtrow[6] = ds.Tables[0].Rows[i]["borrowername"];
                    dtrow[7] = ds.Tables[0].Rows[i]["State"];
                    dtrow[8] = ds.Tables[0].Rows[i]["County"];
                    if (k1 == "5" && qc == "5" && status == "5")
                    {
                        dtrow[9] = "Delivered";
                    }
                    else if (k1 == "2" && qc == "2" && status == "2" && tax == "3")
                    {
                        dtrow[9] = "Mailaway";
                    }
                    ds1.Dispose();
                    ds1.Reset();
                    string strsql = "select Order_no,amount from mailaway_tbl where order_no='" + ds.Tables[0].Rows[i]["Order_No"].ToString() + "' and pdate between '" + strfrmdate + "' and '" + strtodate + "'";
                    ds1 = con.ExecuteQuery(strsql);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                        {
                            strtotalfee = ds1.Tables[0].Rows[j]["amount"].ToString();
                            strtotalfee = strtotalfee.Replace("$", "");
                            totalfee += Convert.ToDouble(strtotalfee);
                        }
                        dtrow[10] = "$" + totalfee;//+ ".00";
                    }
                    else
                    {
                        dtrow[10] = "";
                    }
                    dtrow[11] = "";
                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }
    #endregion

    #region Checklist Report
    public DataView CovertCheckListDStoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order No";
            dcolumn.Caption = "Order No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order Date";
            dcolumn.Caption = "Order Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Status";
            dcolumn.Caption = "Status";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Transaction Type";
            dcolumn.Caption = "Transaction Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order documents(Legal,Abstract)";
            dcolumn.Caption = "Order documents(Legal,Abstract)";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Multiple parcels";
            dcolumn.Caption = "Multiple parcels";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Parcel ID formats";
            dcolumn.Caption = "Parcel ID formats";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Correct authorities";
            dcolumn.Caption = "Correct authorities";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Follow the Miscellaneous info notes";
            dcolumn.Caption = "Follow the Miscellaneous info notes";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Tax Year";
            dcolumn.Caption = "Tax Year";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Amounts";
            dcolumn.Caption = "Amounts";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Tax Status";
            dcolumn.Caption = "Tax Status";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Dates";
            dcolumn.Caption = "Dates";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Tax Bill Status";
            dcolumn.Caption = "Tax Bill Status";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Exemption";
            dcolumn.Caption = "Exemption";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Follow up date";
            dcolumn.Caption = "Follow up date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "ETA Date";
            dcolumn.Caption = "ETA Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Request Type";
            dcolumn.Caption = "Request Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Comments";
            dcolumn.Caption = "Comments";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string strstatus = Convert.ToString(ds.Tables[0].Rows[i]["status"]);
                    string[] strcheck = Convert.ToString(ds.Tables[0].Rows[i]["Comments"]).Split(',');

                    DataRow dtrow = dtTable.NewRow();

                    dtrow[0] = ds.Tables[0].Rows[i]["Order_no"];
                    dtrow[1] = ds.Tables[0].Rows[i]["pdate"];
                    dtrow[2] = ds.Tables[0].Rows[i]["status"];
                    if (strstatus == "Completed")
                    {
                        int count = 0;
                        for (int j = 0; j < strcheck.Length; j++)
                        {
                            count = j + 3;
                            string comment = Convert.ToString(strcheck[j]);
                            if (comment == "YES") comment = "Y";
                            else if (comment == "NO") comment = "N";
                            dtrow[count] = comment;
                        }
                        dtrow[16] = "";
                        dtrow[17] = "";
                        dtrow[18] = "";
                    }
                    else if (strstatus == "In Process" || strstatus == "On Hold")
                    {
                        int count = 0;
                        for (int j = 0; j < strcheck.Length; j++)
                        {
                            count = j + 3;
                            string comment = Convert.ToString(strcheck[j]);
                            if (comment == "YES") comment = "Y";
                            else if (comment == "NO") comment = "N";
                            if (count == 9)
                            {
                                dtrow[9] = "";
                                dtrow[count + 6] = comment;
                            }
                            else if (count == 10)
                            {
                                dtrow[10] = "";
                                dtrow[count + 6] = comment;
                            }
                            else if (count == 11)
                            {
                                dtrow[11] = "";
                                dtrow[12] = "";
                                dtrow[13] = "";
                                dtrow[14] = "";
                                dtrow[17] = "";
                                dtrow[count + 7] = comment;
                            }
                            else dtrow[count] = comment;
                        }
                    }
                    else if (strstatus == "ParcelID")
                    {
                        int count = 0;
                        for (int j = 0; j < strcheck.Length; j++)
                        {
                            count = j + 3;
                            string comment = Convert.ToString(strcheck[j]);
                            if (comment == "YES") comment = "Y";
                            else if (comment == "NO") comment = "N";
                            if (count == 5)
                            {
                                dtrow[5] = "";
                                dtrow[6] = "";
                                dtrow[7] = "";
                                dtrow[8] = "";
                                dtrow[9] = "";
                                dtrow[10] = "";
                                dtrow[11] = "";
                                dtrow[12] = "";
                                dtrow[13] = "";
                                dtrow[14] = "";
                                dtrow[count + 10] = comment;
                            }
                            else if (count == 6)
                            {
                                dtrow[16] = "";
                                dtrow[17] = "";
                                dtrow[18] = comment;
                            }
                            else dtrow[count] = comment;
                        }
                    }
                    else if (strstatus == "Rejected")
                    {
                        int count = 0;
                        for (int j = 0; j < strcheck.Length; j++)
                        {
                            count = j + 3;
                            string comment = Convert.ToString(strcheck[j]);
                            if (comment == "YES") comment = "Y";
                            else if (comment == "NO") comment = "N";
                            if (count == 4)
                            {
                                dtrow[5] = "";
                                dtrow[6] = "";
                                dtrow[7] = "";
                                dtrow[8] = "";
                                dtrow[9] = "";
                                dtrow[10] = "";
                                dtrow[11] = "";
                                dtrow[12] = "";
                                dtrow[13] = "";
                                dtrow[14] = "";
                                dtrow[15] = "";
                                dtrow[16] = "";
                                dtrow[17] = "";
                                dtrow[18] = comment;
                            }
                            else dtrow[count] = comment;
                        }
                    }
                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }
    #endregion

    #region Mailaway
    public int InsertMailaway(string strorderno, string strchepay, string straddress, string strborrower, string strborroweradd, string strparcelid, string stramount, string strregtype, string strtaxtype, string mailawaydate, int noofrequest, string pDate, string city, string tdate)
    {
        try
        {
            mParam = new MySqlParameter[14];

            mParam[0] = new MySqlParameter("?$orderno", strorderno);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

            mParam[1] = new MySqlParameter("?$address", straddress);
            mParam[1].MySqlDbType = MySqlDbType.VarChar;

            mParam[2] = new MySqlParameter("?$cheque_payable", strchepay);
            mParam[2].MySqlDbType = MySqlDbType.VarChar;

            mParam[3] = new MySqlParameter("?$borrowername", strborrower);
            mParam[3].MySqlDbType = MySqlDbType.VarChar;

            mParam[4] = new MySqlParameter("?$borroweraddress", strborroweradd);
            mParam[4].MySqlDbType = MySqlDbType.VarChar;

            mParam[5] = new MySqlParameter("?$parcelid", strparcelid);
            mParam[5].MySqlDbType = MySqlDbType.VarChar;

            mParam[6] = new MySqlParameter("?$amount", stramount);
            mParam[6].MySqlDbType = MySqlDbType.VarChar;

            mParam[7] = new MySqlParameter("?$req_type", strregtype);
            mParam[7].MySqlDbType = MySqlDbType.VarChar;

            mParam[8] = new MySqlParameter("?$taxtype", strtaxtype);
            mParam[8].MySqlDbType = MySqlDbType.VarChar;

            mParam[9] = new MySqlParameter("?$mailawaydate", mailawaydate);
            mParam[9].MySqlDbType = MySqlDbType.VarChar;

            mParam[10] = new MySqlParameter("?$pDate", pDate);
            mParam[10].MySqlDbType = MySqlDbType.VarChar;

            mParam[11] = new MySqlParameter("?$noofrequest", noofrequest);
            mParam[11].MySqlDbType = MySqlDbType.VarChar;

            mParam[12] = new MySqlParameter("?$city", city);
            mParam[12].MySqlDbType = MySqlDbType.VarChar;

            mParam[13] = new MySqlParameter("?$tDate", tdate);
            mParam[13].MySqlDbType = MySqlDbType.VarChar;


            return con.ExecuteSPNonQuery("sp_Insertmailaway1", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable FetchRequest(string orderid, string date)
    {
        DataTable dt = new DataTable();
        string query = "select id,Order_no,pDate,Mailaway_date,Req_Type,Cheque_payable,Address,Borrowername,BorrowerAddress,City,ParcelId,Amount,TaxType,Status from mailaway_tbl where Order_no='" + orderid + "' and pDate='" + date + "' order by id asc;";
        mDa = con.ExecuteSPAdapter(query, false, mParam);
        mDa.Fill(dt);
        return dt;
    }
    public void UpdateRequeststatus(string id)
    {
        string query = "Update mailaway_tbl set `Status`='Received' where id='" + id + "' limit 1";
        int result = con.ExecuteSPNonQuery(query);
    }

    #endregion

    #region Checklist

    public int Insert_CompletedChecklist(string orderno, string pdate, string processname, string status, string transtype, string order_doc, string multiple_parcel, string parcelid, string correct, string follow, string tax_year, string amount, string tax_status, string dates, string tax_billstatus, string exemption, string comments)
    {
        mParam = new MySqlParameter[18];

        mParam[0] = new MySqlParameter("?$order_no", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$pdate", pdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$processname", processname);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("?$status", status);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("?$transtype", transtype);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("?$order_doc", order_doc);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("?$multiple", multiple_parcel);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;
        mParam[8] = new MySqlParameter("?$parcelid", parcelid);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;
        mParam[9] = new MySqlParameter("?$correct", correct);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;
        mParam[10] = new MySqlParameter("?$follow", follow);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;
        mParam[11] = new MySqlParameter("?$taxyear", tax_year);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;
        mParam[12] = new MySqlParameter("?$amount", amount);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;
        mParam[13] = new MySqlParameter("?$taxstatus", tax_status);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;
        mParam[14] = new MySqlParameter("?$dates", dates);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;
        mParam[15] = new MySqlParameter("?$billstatus", tax_billstatus);
        mParam[15].MySqlDbType = MySqlDbType.VarChar;
        mParam[16] = new MySqlParameter("?$exemption", exemption);
        mParam[16].MySqlDbType = MySqlDbType.VarChar;
        mParam[17] = new MySqlParameter("?$commemts", comments);
        mParam[17].MySqlDbType = MySqlDbType.VarChar;


        return con.ExecuteSPNonQuery("sp_ChecklistCompleted", true, mParam);
    }

    public int Insert_Inpro_HoldChecklist(string orderno, string pdate, string processname, string status, string transtype, string order_doc, string multiple_parcel, string parcelid, string correct, string follow, string followup_date, string eta_date, string comments)
    {
        mParam = new MySqlParameter[14];

        mParam[0] = new MySqlParameter("?$order_no", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$pdate", pdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$processname", processname);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("?$status", status);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("?$transtype", transtype);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("?$order_doc", order_doc);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("?$multiple", multiple_parcel);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;
        mParam[8] = new MySqlParameter("?$parcelid", parcelid);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;
        mParam[9] = new MySqlParameter("?$correct", correct);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;
        mParam[10] = new MySqlParameter("?$follow", follow);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;
        mParam[11] = new MySqlParameter("?$follow_date", followup_date);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;
        mParam[12] = new MySqlParameter("?$eta_date", eta_date);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;
        mParam[13] = new MySqlParameter("?$commemts", comments);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_Checklist_Inpro_Hold", true, mParam);
    }

    public int Insert_MailawayChecklist(string orderno, string pdate, string processname, string status, string transtype, string order_doc, string multiple_parcel, string parcelid, string correct, string follow, string followup_date, string eta_date, string request_type, string comments)
    {
        mParam = new MySqlParameter[15];

        mParam[0] = new MySqlParameter("?$order_no", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$pdate", pdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$processname", processname);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("?$status", status);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("?$transtype", transtype);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("?$order_doc", order_doc);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("?$multiple", multiple_parcel);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;
        mParam[8] = new MySqlParameter("?$parcelid", parcelid);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;
        mParam[9] = new MySqlParameter("?$correct", correct);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;
        mParam[10] = new MySqlParameter("?$follow", follow);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;
        mParam[11] = new MySqlParameter("?$follow_date", followup_date);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;
        mParam[12] = new MySqlParameter("?$eta_date", eta_date);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;
        mParam[13] = new MySqlParameter("?$commemts", comments);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;
        mParam[14] = new MySqlParameter("?$req_type", request_type);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_ChecklistMailaway", true, mParam);
    }

    public int Insert_ParcelidChecklist(string orderno, string pdate, string processname, string status, string transtype, string order_doc, string followup_date, string comments)
    {
        mParam = new MySqlParameter[9];

        mParam[0] = new MySqlParameter("?$order_no", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$pdate", pdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$processname", processname);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("?$status", status);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("?$transtype", transtype);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("?$order_doc", order_doc);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("?$follow_date", followup_date);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;
        mParam[8] = new MySqlParameter("?$commemts", comments);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_ChecklistParcelid", true, mParam);

    }

    public int Insert_RejectedChecklist(string orderno, string pdate, string processname, string status, string transtype, string comments)
    {
        mParam = new MySqlParameter[7];

        mParam[0] = new MySqlParameter("?$order_no", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$pdate", pdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$processname", processname);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("?$status", status);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("?$transtype", transtype);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("?$commemts", comments);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_ChecklistRejected", true, mParam);
    }

    public DataView CovertLoginCheckListDStoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Date";
            dcolumn.Caption = "Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Username";
            dcolumn.Caption = "Username";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Attendance";
            dcolumn.Caption = "Attendance";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Biometric Access";
            dcolumn.Caption = "Biometric Access";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Mobile Restriction";
            dcolumn.Caption = "Mobile Restriction";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "ID Card & Dress Code";
            dcolumn.Caption = "ID Card & Dress Code";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Headset allotment";
            dcolumn.Caption = "Headset allotment";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Login and check if all hardware work properly";
            dcolumn.Caption = "Login and check if all hardware work properly";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Keeping the work place clean";
            dcolumn.Caption = "Keeping the work place clean";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Headset handover";
            dcolumn.Caption = "Headset handover";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Switching off systems";
            dcolumn.Caption = "Switching off systems";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    dtrow[0] = ds.Tables[0].Rows[i]["pdate"];
                    dtrow[1] = ds.Tables[0].Rows[i]["username"];
                    dtrow[2] = Getreturnstring(Convert.ToString(ds.Tables[0].Rows[i]["attendance"]));
                    dtrow[3] = Getreturnstring(Convert.ToString(ds.Tables[0].Rows[i]["biometrice"]));
                    dtrow[4] = Getreturnstring(Convert.ToString(ds.Tables[0].Rows[i]["mobile_restriction"]));
                    dtrow[5] = Getreturnstring(Convert.ToString(ds.Tables[0].Rows[i]["id_card_dress_code"]));
                    dtrow[6] = Getreturnstring(Convert.ToString(ds.Tables[0].Rows[i]["heatset_allot"]));
                    dtrow[7] = Getreturnstring(Convert.ToString(ds.Tables[0].Rows[i]["login_hardware"]));
                    dtrow[8] = Getreturnstring(Convert.ToString(ds.Tables[0].Rows[i]["work_place_clean"]));
                    dtrow[9] = Getreturnstring(Convert.ToString(ds.Tables[0].Rows[i]["headset_over"]));
                    dtrow[10] = Getreturnstring(Convert.ToString(ds.Tables[0].Rows[i]["switchoff_system"]));

                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }

    public string Getreturnstring(string strretstring)
    {
        if (strretstring == "CHECKED") strretstring = "YES";
        else if (strretstring == "") strretstring = "NO";

        return strretstring;
    }

    #endregion

    #region Hourly Count Report
    public DataSet GetHourlyCount(string fdate, string tdate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_HourlyCount_New", true, mParam);
    }
    public DataSet GetHourlyCounttotal(string fdate, string tdate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_hourlycount_logesh", true, mParam);
    }
    public DataSet getbreaktimetot(string fdate, string tdate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_break_report", true, mParam);
    }
    public DataSet getbreaktimetot1(string fdate, string tdate, string name, string pdate)
    {
        mParam = new MySqlParameter[4];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$username", name);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$pdate", pdate);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_break_report2", true, mParam);
    }


    public DataView CovertHourlyCountDStoDataview(DataSet ds)
    {
        try
        {
            string struser = "";
            int strprocess = 0;
            int strdeliver = 0;

            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "UserName";
            dcolumn.Caption = "UserName";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "06:00-07:00";
            dcolumn.Caption = "06:00-07:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "07:00 - 08:00";
            dcolumn.Caption = "07:00 - 08:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "08:00 - 09:00";
            dcolumn.Caption = "08:00 - 09:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "09:00 - 10:00";
            dcolumn.Caption = "09:00 - 10:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "10:00 - 11:00";
            dcolumn.Caption = "10:00 - 11:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "11:00 - 12:00";
            dcolumn.Caption = "11:00 - 12:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "12:00 - 01:00";
            dcolumn.Caption = "12:00 - 01:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "01:00 - 02:00";
            dcolumn.Caption = "01:00 - 02:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "02:00 - 03:00";
            dcolumn.Caption = "02:00 - 03:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "03:00 - 04:00";
            dcolumn.Caption = "03:00 - 04:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "04:00 - 05:00";
            dcolumn.Caption = "04:00 - 05:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "05:00 - 06:00";
            dcolumn.Caption = "05:00 - 06:00";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "TotalDelivered";
            dcolumn.Caption = "TotalDelivered";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    dtrow[0] = ds.Tables[0].Rows[i]["User_Name"];
                    dtrow[1] = ds.Tables[0].Rows[i]["06:00 - 07:00"];
                    strdeliver += Convert.ToInt32(dtrow[1]);
                    if (dtrow[1].ToString() == "0") dtrow[1] = "";

                    dtrow[2] = ds.Tables[0].Rows[i]["07:00 - 08:00"];
                    strdeliver += Convert.ToInt32(dtrow[2]);
                    if (dtrow[2].ToString() == "0") dtrow[2] = "";

                    dtrow[3] = ds.Tables[0].Rows[i]["08:00 - 09:00"];
                    strdeliver += Convert.ToInt32(dtrow[3]);
                    if (dtrow[3].ToString() == "0") dtrow[3] = "";

                    dtrow[4] = ds.Tables[0].Rows[i]["09:00 - 10:00"];
                    strdeliver += Convert.ToInt32(dtrow[4]);
                    if (dtrow[4].ToString() == "0") dtrow[4] = "";

                    dtrow[5] = ds.Tables[0].Rows[i]["10:00 - 11:00"];
                    strdeliver += Convert.ToInt32(dtrow[5]);
                    if (dtrow[5].ToString() == "0") dtrow[5] = "";

                    dtrow[6] = ds.Tables[0].Rows[i]["11:00 - 12:00"];
                    strdeliver += Convert.ToInt32(dtrow[6]);
                    if (dtrow[6].ToString() == "0") dtrow[6] = "";

                    dtrow[7] = ds.Tables[0].Rows[i]["12:00 - 01:00"];
                    strdeliver += Convert.ToInt32(dtrow[7]);
                    if (dtrow[7].ToString() == "0") dtrow[7] = "";

                    dtrow[8] = ds.Tables[0].Rows[i]["01:00 - 02:00"];
                    strdeliver += Convert.ToInt32(dtrow[8]);
                    if (dtrow[8].ToString() == "0") dtrow[8] = "";

                    dtrow[9] = ds.Tables[0].Rows[i]["02:00 - 03:00"];
                    strdeliver += Convert.ToInt32(dtrow[9]);
                    if (dtrow[9].ToString() == "0") dtrow[9] = "";

                    dtrow[10] = ds.Tables[0].Rows[i]["03:00 - 04:00"];
                    strdeliver += Convert.ToInt32(dtrow[10]);
                    if (dtrow[10].ToString() == "0") dtrow[10] = "";

                    dtrow[11] = ds.Tables[0].Rows[i]["04:00 - 05:00"];
                    strdeliver += Convert.ToInt32(dtrow[11]);
                    if (dtrow[11].ToString() == "0") dtrow[11] = "";

                    dtrow[12] = ds.Tables[0].Rows[i]["05:00 - 06:00"];
                    strdeliver += Convert.ToInt32(dtrow[12]);
                    if (dtrow[12].ToString() == "0") dtrow[12] = "";

                    dtrow[13] = strdeliver;
                    if (dtrow[13].ToString() == "0") dtrow[13] = "";

                    dtTable.Rows.Add(dtrow);
                    strdeliver = 0;

                }
                dataview = new DataView(dtTable);
            }
            else
            {
                DataRow emptyRow = dtTable.NewRow();
                dtTable.Rows.Add(emptyRow);
                dataview = new DataView(dtTable);
                return dataview;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }
    #endregion

    #region Download & Upload
    public DataView ConvertUploadtoDataview(string strfrmdate, string strtodate)
    {
        string strfromtime = string.Empty;
        string strtotime = string.Empty;
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Time";
            dcolumn.Caption = "Time";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Download";
            dcolumn.Caption = "Download";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Upload";
            dcolumn.Caption = "Upload";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            mParam = new MySqlParameter[4];

            mParam[0] = new MySqlParameter("?$fromdate", strfrmdate);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;
            mParam[1] = new MySqlParameter("?$todate", strtodate);
            mParam[1].MySqlDbType = MySqlDbType.VarChar;

            int h = 18;
            int h1 = 19;
            int downloadtotal = 0, uploadtotal = 0;
            string downtime = "", uptime = "";

            for (int i = 0; i <= 24; i++)
            {
                DataRow dtrow = dtTable.NewRow();
                if (i != 24)
                {
                    if (h <= 9) { strfromtime = "0" + h + ":00:00"; }
                    else if (h <= 24 && h >= 9) { strfromtime = h + ":00:00"; }
                    if (h1 <= 9) { strtotime = "0" + h1 + ":00:00"; }
                    else if (h1 <= 24 && h1 >= 9) { strtotime = h1 + ":00:00"; }

                    mParam[2] = new MySqlParameter("?$fromtiming", strfromtime);
                    mParam[2].MySqlDbType = MySqlDbType.VarChar;
                    mParam[3] = new MySqlParameter("?$totiming", strtotime);
                    mParam[3].MySqlDbType = MySqlDbType.VarChar;

                    ds = con.ExecuteQuery("sp_uploadpattern", true, mParam);

                    string[] frmtime = strfromtime.Split(':');
                    string[] totime = strtotime.Split(':');
                    downtime = frmtime[0] + ":" + frmtime[1];
                    uptime = totime[0] + ":" + totime[1];
                    dtrow[0] = downtime + " - " + uptime;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dtrow[1] = ds.Tables[0].Rows[0]["DownloadTime"];
                        downloadtotal += Convert.ToInt16(dtrow[1].ToString());
                        dtrow[2] = ds.Tables[1].Rows[0]["UploadTime"];
                        uploadtotal += Convert.ToInt16(dtrow[2].ToString());
                    }
                    h++; h1++;
                    if (h == 24) { h = 0; }
                    if (h1 == 24) { h1 = 0; }
                }
                else
                {
                    dtrow[0] = "Total";
                    dtrow[1] = downloadtotal.ToString();
                    dtrow[2] = uploadtotal.ToString();
                }
                dtTable.Rows.Add(dtrow);
            }
            dataview = new DataView(dtTable);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return dataview;
    }


    public void ClearDownloaduploadpattern()
    {
        string query = "truncate table downloaduploadpattern";
        int result = con.ExecuteSPNonQuery(query);
    }

    public DataSet getdownloadpattern(string strfrmdate, string strtodate, string pattern)
    {
        string query = "";
        if (pattern == "DownloadPattern") query = "sp_downloadpattern_new";
        else if (pattern == "UploadPattern") query = "sp_uploadpattern_new";

        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", strfrmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$tdate", strtodate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery(query, true, mParam);
    }
    public DataView GetUpDowndstodataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Hours";
            dcolumn.Caption = "Hours";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DateTime dtt = Convert.ToDateTime(ds.Tables[0].Rows[i]["pdate"]);
                string strcolumn = String.Format("{0:dd-MM-yyyy}", dtt);

                dcolumn = new DataColumn();
                dcolumn.DataType = System.Type.GetType("System.String");
                dcolumn.ColumnName = strcolumn;
                dcolumn.Caption = strcolumn;
                dcolumn.ReadOnly = true;
                dcolumn.Unique = false;
                dtTable.Columns.Add(dcolumn);
            }

            //dcolumn = new DataColumn();
            //dcolumn.DataType = System.Type.GetType("System.String");
            //dcolumn.ColumnName = "Total";
            //dcolumn.Caption = "Total";
            //dcolumn.ReadOnly = true;
            //dcolumn.Unique = false;
            //dtTable.Columns.Add(dcolumn);

            //dcolumn = new DataColumn();
            //dcolumn.DataType = System.Type.GetType("System.String");
            //dcolumn.ColumnName = "EST";
            //dcolumn.Caption = "EST";
            //dcolumn.ReadOnly = true;
            //dcolumn.Unique = false;
            //dtTable.Columns.Add(dcolumn);

            //dcolumn = new DataColumn();
            //dcolumn.DataType = System.Type.GetType("System.String");
            //dcolumn.ColumnName = "CST";
            //dcolumn.Caption = "CST";
            //dcolumn.ReadOnly = true;
            //dcolumn.Unique = false;
            //dtTable.Columns.Add(dcolumn);

            //dcolumn = new DataColumn();
            //dcolumn.DataType = System.Type.GetType("System.String");
            //dcolumn.ColumnName = "MST";
            //dcolumn.Caption = "MST";
            //dcolumn.ReadOnly = true;
            //dcolumn.Unique = false;
            //dtTable.Columns.Add(dcolumn);


            for (int i = 0; i < ds.Tables[0].Columns.Count - 1; i++)
            {
                DataRow dtrow = dtTable.NewRow();
                dtrow[0] = ds.Tables[1].Rows[i]["Time"];
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    dtrow[j + 1] = ds.Tables[0].Rows[j][i + 1];
                }
                dtTable.Rows.Add(dtrow);
            }
            dataview = new DataView(dtTable);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return dataview;
    }
    public void InsertPattern(string time, string count, string pattern)
    {
        string query = "";
        if (pattern == "Download") query = "Insert into downloaduploadpattern(Time,Download)values('" + time + "','" + count + "')";
        else if (pattern == "Upload") query = "Insert into downloaduploadpattern(Time,Upload)values('" + time + "','" + count + "')";
        int result = con.ExecuteSPNonQuery(query);
    }
    #endregion

    #region Break Time

    public DataSet BreakTimeDetails(string strusrname, string strdate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$username", strusrname);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_breaktime", true, mParam);
    }
    public DataSet BreakTimeDetailsNew(string strusrname, string strdate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$username", strusrname);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_breaktime_new", true, mParam);
    }
    public int BreakTimeDetailsLogout(string strusrname, string strdate, string reason)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("?$username", strusrname);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$reason", reason);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_breaktimelogout", true, mParam);
    }

    public int BreakResetOrder(string process, string result)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$process", process);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$orderno", result);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_resetorder", true, mParam);
    }

    public DataSet TotalBreakTime(string strfrmdate, string strtodate, string struser)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("?$fdate", strfrmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strtodate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$username", struser);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_breakreport", true, mParam);
    }

    public DataSet TotalBreakTimeReport(string strfrmdate, string strtodate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", strfrmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strtodate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_breakreport_new", true, mParam);
    }

    public DataSet Production_Lock(string strfrmdate, string strtodate, string struser)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("?$fdate", strfrmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strtodate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$username", struser);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_production_lock", true, mParam);
    }
    public DataView CovertBreakDStoDataview(DataSet ds)
    {
        DataTable dtTable = new DataTable();
        DataColumn dcolumn;

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Date";
        dcolumn.Caption = "Date";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "UserName";
        dcolumn.Caption = "UserName";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Break1 Out";
        dcolumn.Caption = "Break1 Out";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Break1 In";
        dcolumn.Caption = "Break1 In";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Break1 Total";
        dcolumn.Caption = "Break1 Total";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Break2 Out";
        dcolumn.Caption = "Break2 Out";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Break2 In";
        dcolumn.Caption = "Break2 In";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Break2 Total";
        dcolumn.Caption = "Break2 Total";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Breakfast Out";
        dcolumn.Caption = "Breakfast Out";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Breakfast In";
        dcolumn.Caption = "Breakfast In";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Breakfast Total";
        dcolumn.Caption = "Breakfast Total";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Lunch Out";
        dcolumn.Caption = "Lunch Out";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Lunch In";
        dcolumn.Caption = "Lunch In";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Lunch Total";
        dcolumn.Caption = "Lunch Total";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Dinner Out";
        dcolumn.Caption = "Dinner Out";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Dinner In";
        dcolumn.Caption = "Dinner In";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Dinner Total";
        dcolumn.Caption = "Dinner Total";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        //dcolumn = new DataColumn();
        //dcolumn.DataType = System.Type.GetType("System.String");
        //dcolumn.ColumnName = "Meeting Out";
        //dcolumn.Caption = "Meeting Out";
        //dcolumn.ReadOnly = true;
        //dcolumn.Unique = false;
        //dtTable.Columns.Add(dcolumn);

        //dcolumn = new DataColumn();
        //dcolumn.DataType = System.Type.GetType("System.String");
        //dcolumn.ColumnName = "Meeting In";
        //dcolumn.Caption = "Meeting In";
        //dcolumn.ReadOnly = true;
        //dcolumn.Unique = false;
        //dtTable.Columns.Add(dcolumn);

        //dcolumn = new DataColumn();
        //dcolumn.DataType = System.Type.GetType("System.String");
        //dcolumn.ColumnName = "Meeting Total";
        //dcolumn.Caption = "Meeting Total";
        //dcolumn.ReadOnly = true;
        //dcolumn.Unique = false;
        //dtTable.Columns.Add(dcolumn);

        //dcolumn = new DataColumn();
        //dcolumn.DataType = System.Type.GetType("System.String");
        //dcolumn.ColumnName = "Training Out";
        //dcolumn.Caption = "Training Out";
        //dcolumn.ReadOnly = true;
        //dcolumn.Unique = false;
        //dtTable.Columns.Add(dcolumn);

        //dcolumn = new DataColumn();
        //dcolumn.DataType = System.Type.GetType("System.String");
        //dcolumn.ColumnName = "Training In";
        //dcolumn.Caption = "Training In";
        //dcolumn.ReadOnly = true;
        //dcolumn.Unique = false;
        //dtTable.Columns.Add(dcolumn);

        //dcolumn = new DataColumn();
        //dcolumn.DataType = System.Type.GetType("System.String");
        //dcolumn.ColumnName = "Training Total";
        //dcolumn.Caption = "Training Total";
        //dcolumn.ReadOnly = true;
        //dcolumn.Unique = false;
        //dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Total Time";
        dcolumn.Caption = "Total Time";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    {
        //        DataRow dtrow = dtTable.NewRow();

        //        dtrow[0] = ds.Tables[0].Rows[i]["Tdate"];
        //        dtrow[1] = ds.Tables[0].Rows[i]["UserName"];

        //        dtrow[2] = ds.Tables[0].Rows[i]["Break1_Out"];
        //        dtrow[3] = ds.Tables[0].Rows[i]["Break1_In"];
        //        dtrow[4] = ds.Tables[0].Rows[i]["Break1 Total"];

        //        dtrow[5] = ds.Tables[0].Rows[i]["Break2_Out"];
        //        dtrow[6] = ds.Tables[0].Rows[i]["Break2_In"];
        //        dtrow[7] = ds.Tables[0].Rows[i]["Break2 Total"];

        //        dtrow[8] = ds.Tables[0].Rows[i]["Breakfast_Out"];
        //        dtrow[9] = ds.Tables[0].Rows[i]["Breakfast_In"];
        //        dtrow[10] = ds.Tables[0].Rows[i]["Breakfast Total"];

        //        dtrow[11] = ds.Tables[0].Rows[i]["Lunch_Out"];
        //        dtrow[12] = ds.Tables[0].Rows[i]["Lunch_In"];
        //        dtrow[13] = ds.Tables[0].Rows[i]["Lunch Total"];

        //        dtrow[14] = ds.Tables[0].Rows[i]["Dinner_Out"];
        //        dtrow[15] = ds.Tables[0].Rows[i]["Dinner_In"];
        //        dtrow[16] = ds.Tables[0].Rows[i]["Dinner Total"];

        //        dtrow[17] = ds.Tables[0].Rows[i]["Meeting_Out"];
        //        dtrow[18] = ds.Tables[0].Rows[i]["Meeting_In"];
        //        dtrow[19] = ds.Tables[0].Rows[i]["Meeting Total"];

        //        dtrow[20] = ds.Tables[0].Rows[i]["Training_Out"];
        //        dtrow[21] = ds.Tables[0].Rows[i]["Training_In"];
        //        dtrow[22] = ds.Tables[0].Rows[i]["Training Total"];

        //        dtrow[23] = ds.Tables[0].Rows[i]["Total Time"];

        //        dtTable.Rows.Add(dtrow);
        //    }
        //    dataview = new DataView(dtTable);
        //}

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    dtrow[0] = ds.Tables[0].Rows[i]["pdate"];
                    dtrow[1] = ds.Tables[0].Rows[i]["UserName"];

                    string struser = Convert.ToString(ds.Tables[0].Rows[i]["UserName"]);
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {
                        string strbreak = Convert.ToString(ds.Tables[1].Rows[j]["Break Reason"]);

                        if (struser == Convert.ToString(ds.Tables[1].Rows[j]["Name"]))
                        {
                            if (strbreak == "Short Break 1")
                            {
                                dtrow[2] = ds.Tables[1].Rows[j]["Starttime"];
                                dtrow[3] = ds.Tables[1].Rows[j]["Endtime"];
                                dtrow[4] = ds.Tables[1].Rows[j]["TimeTaken"];
                            }
                            else if (strbreak == "Short Break 2")
                            {
                                dtrow[5] = ds.Tables[1].Rows[j]["Starttime"];
                                dtrow[6] = ds.Tables[1].Rows[j]["Endtime"];
                                dtrow[7] = ds.Tables[1].Rows[j]["TimeTaken"];
                            }
                            else if (strbreak == "Breakfast")
                            {
                                dtrow[8] = ds.Tables[1].Rows[j]["Starttime"];
                                dtrow[9] = ds.Tables[1].Rows[j]["Endtime"];
                                dtrow[10] = ds.Tables[1].Rows[j]["TimeTaken"];
                            }
                            else if (strbreak == "Lunch")
                            {
                                dtrow[11] = ds.Tables[1].Rows[j]["Starttime"];
                                dtrow[12] = ds.Tables[1].Rows[j]["Endtime"];
                                dtrow[13] = ds.Tables[1].Rows[j]["TimeTaken"];
                            }
                            else if (strbreak == "Dinner Break")
                            {
                                dtrow[14] = ds.Tables[1].Rows[j]["Starttime"];
                                dtrow[15] = ds.Tables[1].Rows[j]["Endtime"];
                                dtrow[16] = ds.Tables[1].Rows[j]["TimeTaken"];
                            }
                        }
                    }
                    dtrow[17] = ds.Tables[0].Rows[i]["Total Time"];
                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        return dataview;
    }

    #endregion

    #region Logout Reason
    public DataView CovertLogoutDStoDataview(DataSet ds)
    {
        DataTable dtTable = new DataTable();
        DataColumn dcolumn;

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Order No";
        dcolumn.Caption = "Order No";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Date";
        dcolumn.Caption = "Date";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "UserName";
        dcolumn.Caption = "UserName";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Process";
        dcolumn.Caption = "Process";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Start Time";
        dcolumn.Caption = "Start Time";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "End Time";
        dcolumn.Caption = "End Time";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Total Time";
        dcolumn.Caption = "Total Time";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Reason";
        dcolumn.Caption = "Reason";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dtrow = dtTable.NewRow();

                dtrow[0] = ds.Tables[0].Rows[i]["Order_No"];
                dtrow[1] = ds.Tables[0].Rows[i]["Dt"];
                if (dtrow[1] != "")
                {
                    DateTime pDt = Convert.ToDateTime(dtrow[1].ToString());
                    dtrow[1] = String.Format("{0:dd-MMM-yyyy}", pDt);
                }

                dtrow[2] = ds.Tables[0].Rows[i]["User_ID"];
                dtrow[3] = ds.Tables[0].Rows[i]["File_Status"];
                dtrow[4] = ds.Tables[0].Rows[i]["Start_time"];
                dtrow[5] = ds.Tables[0].Rows[i]["Cancel_time"];
                dtrow[6] = ds.Tables[0].Rows[i]["TAT"];
                dtrow[7] = ds.Tables[0].Rows[i]["Reason"];

                dtTable.Rows.Add(dtrow);
            }
            dataview = new DataView(dtTable);
        }
        return dataview;
    }
    #endregion
    #region QAdetails

    public DataView CovertqadetailDStoDataview(DataSet ds)
    {
        DataTable dtTable = new DataTable();
        DataColumn dcolumn;

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "User_Id";
        dcolumn.Caption = "User_Id";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);



        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dtrow = dtTable.NewRow();

                dtrow[0] = ds.Tables[0].Rows[i]["User_ID"];

                dtTable.Rows.Add(dtrow);
            }
            dataview = new DataView(dtTable);
        }
        return dataview;
    }
    #endregion

    #region Cheque Payable
    public DataView CovertChequeDStoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Cheque payable";
            dcolumn.Caption = "Cheque payable";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Address";
            dcolumn.Caption = "Address";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Charges";
            dcolumn.Caption = "Charges";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Request Type";
            dcolumn.Caption = "Request Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Tax Type";
            dcolumn.Caption = "Tax Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    dtrow[0] = ds.Tables[0].Rows[i]["Cheque_payable"];
                    dtrow[1] = ds.Tables[0].Rows[i]["Address"];
                    dtrow[2] = ds.Tables[0].Rows[i]["Charges"];
                    dtrow[3] = ds.Tables[0].Rows[i]["Req_Type"];
                    dtrow[4] = ds.Tables[0].Rows[i]["Tax_type"];

                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }
    #endregion

    #region Save County Link
    public int SaveCountyLink(string state, string county, string assessor, string assessor_phone, string treasurer, string treasurer_phone)
    {
        mParam = new MySqlParameter[6];

        mParam[0] = new MySqlParameter("?$state", state);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$county", county);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$assessor", assessor);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$assessor_phone", assessor_phone);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$treasurer", treasurer);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$treasurer_phone", treasurer_phone);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_Savecountylink", true, mParam);
    }

    public int UpdateCountyLink(string state, string county, string assessor, string assessor_phone, string treasurer, string treasurer_phone)
    {
        mParam = new MySqlParameter[6];

        mParam[0] = new MySqlParameter("?$state", state);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$county", county);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$assessor", assessor);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$assessor_phone", assessor_phone);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$treasurer", treasurer);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$treasurer_phone", treasurer_phone);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_Updatecountylink", true, mParam);
    }
    #endregion

    #region TAT Report
    public DataSet Tatreport(object fdate, object tdate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_Gettatreport", true, mParam);
        // return con.ExecuteQuery("sp_get_tat_report", true, mParam);

    }
    public DataSet Tatreportdetail(object fdate, object tdate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        //mParam[2] = new MySqlParameter("?$otype", otype);
        //mParam[2].MySqlDbType = MySqlDbType.VarChar;


        return con.ExecuteQuery("sp_get_tat_report", true, mParam);

    }
    public DataView CovertTatDstoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order Type";
            dcolumn.Caption = "Order Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Total Delivered";
            dcolumn.Caption = "Total Delivered";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Total Tat Delivered";
            dcolumn.Caption = "Total Tat Delivered";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Percentage";
            dcolumn.Caption = "Percentage";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                decimal totcount = 0, tottat = 0;
                for (int i = 0; i <= ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    if (i != ds.Tables[0].Rows.Count)
                    {
                        dtrow[0] = ds.Tables[0].Rows[i]["Order Type"];
                        dtrow[1] = ds.Tables[0].Rows[i]["Total Delivered"];
                        dtrow[2] = ds.Tables[0].Rows[i]["Total Tat Delivered"];
                        dtrow[3] = ds.Tables[0].Rows[i]["Percentage"];

                        totcount += Convert.ToDecimal(dtrow[1]);
                        tottat += Convert.ToDecimal(dtrow[2]);
                    }
                    else
                    {
                        dtrow[0] = "Total";
                        dtrow[1] = totcount.ToString();
                        dtrow[2] = tottat.ToString();
                        decimal percen = Convert.ToDecimal(tottat / totcount);
                        percen = Math.Round((percen * 100), 0);
                        dtrow[3] = percen.ToString() + "%";
                    }

                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
            return dataview;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataView CovertTatreportDstoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "OrderNo";
            dcolumn.Caption = "OrderNo";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "DownloadTime";
            dcolumn.Caption = "DownloadTime";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "UploadTime";
            dcolumn.Caption = "UploadTime";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "OrderType";
            dcolumn.Caption = "OrderType";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "TATachived";
            dcolumn.Caption = "TATachived";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Pdate";
            dcolumn.Caption = "Pdate";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "DeliveredDate";
            dcolumn.Caption = "DeliveredDate";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                decimal totcount = 0, tottat = 0;
                for (int i = 0; i <= ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();

                    if (i != ds.Tables[0].Rows.Count)
                    {
                        dtrow[0] = ds.Tables[0].Rows[i]["Order_no"];
                        dtrow[1] = ds.Tables[0].Rows[i]["DownloadTime"];
                        dtrow[2] = ds.Tables[0].Rows[i]["UploadTime"];
                        dtrow[3] = ds.Tables[0].Rows[i]["OrderType"];
                        dtrow[4] = "";
                        dtrow[5] = ds.Tables[0].Rows[i]["pdate"];
                        dtrow[6] = ds.Tables[0].Rows[i]["DeliveredDate"];

                        //totcount += Convert.ToDecimal(dtrow[1]);
                        //tottat += Convert.ToDecimal(dtrow[2]);
                    }
                    //else
                    //{
                    //    dtrow[0] = "Total";
                    //    dtrow[1] = totcount.ToString();
                    //    dtrow[2] = tottat.ToString();
                    //    decimal percen = Convert.ToDecimal(tottat / totcount);
                    //    percen = Math.Round((percen * 100), 0);
                    //    dtrow[3] = percen.ToString() + "%";
                    //}

                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
            return dataview;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region break time new
    public DataSet insert_breaks(string order_no, string pdate, string process, string brk_type, string brk_value)
    {
        //string pp = "CALL `sp_insert_break`('" + SessionHandler.UserName + "','" + order_no + "','" + pdate + "','" + process + "','" + brk_type + "','" + brk_value + "')";
        //return ExecuteQuery(pp);


        string query = "sp_insert_break";

        mParam = new MySqlParameter[6];

        mParam[0] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$Order_No", order_no);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$PDate", pdate);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$ProcessName", process);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$break_type", brk_type);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$break_value", brk_value);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        //   return ExecuteQuery(query, true, mParam);  ExecuteSPAdapter1
        return con.ExecuteQuery("sp_insert_break", true, mParam);


    }

    public DataSet update_breaks(string brk_type, string brk_cmd, string pdate)
    {
        string pp = "CALL `sp_update_break`('" + SessionHandler.UserName + "','" + brk_type + "', '" + brk_cmd + "','" + pdate + "')";
        return con.ExecuteQuery1(pp);
    }




    public DataSet check_breaks(string pdate)
    {
        string pp = "CALL `sp_check_time_brk`('" + SessionHandler.UserName + "','" + pdate + "')";
        return con.ExecuteQuery1(pp);
    }


    public DataSet timeiff_breaks(string brk_value, string pdate)
    {
        string pp = "CALL `sp_timediff_break`('" + SessionHandler.UserName + "','" + brk_value + "', '" + pdate + "')";
        return con.ExecuteQuery1(pp);
    }


    public DataSet get_diff_time()
    {
        string pp = "CALL `sp_diff_time`('" + SessionHandler.UserName + "')";
        return con.ExecuteQuery1(pp);
    }

    public bool reset_break(string UserName, string pdate, string reason, string admin_name)
    {

        MySqlConnection connection = new MySqlConnection(ConnectionString);
        connection.Open();
        MySqlCommand command;
        string query = "CALL `sp_reset_user_break`('" + UserName + "', '" + pdate + "', '" + reason + "', '" + admin_name + "')";
        command = new MySqlCommand(query, connection);
        MySqlTransaction transaction;
        transaction = connection.BeginTransaction();
        try
        {
            command.ExecuteNonQuery();
            transaction.Commit();
            connection.Close();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public bool sp_lock_user()
    {

        MySqlConnection connection = new MySqlConnection(ConnectionString);
        connection.Open();
        MySqlCommand command;
        string query = "CALL `sp_lock_user`('" + SessionHandler.UserName + "')";
        command = new MySqlCommand(query, connection);
        MySqlTransaction transaction;
        transaction = connection.BeginTransaction();
        try
        {
            command.ExecuteNonQuery();
            transaction.Commit();
            connection.Close();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string get_user_lock(string userName)
    {
        try
        {


            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            MySqlCommand command;

            string query = "select breaks from user_status where User_Name='" + userName + "'";
            command = new MySqlCommand(query, connection);
            command.CommandTimeout = 1800;

            MySqlDataReader dataReader;
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                lockuser = Convert.ToString(dataReader["breaks"]);

            }
            dataReader.Close();
            connection.Close();
            return lockuser;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion



    public int AddAuditError(string strorder, string pdate, string error, string error_cat, string error_area, string error_type, string combined)
    {
        mParam = new MySqlParameter[8];

        mParam[0] = new MySqlParameter("?$Order_No", strorder);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$pdate", pdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$error", error);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$error_cat", error_cat);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$error_area", error_area);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$error_type", error_type);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$combined", combined);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$UserName", SessionHandler.UserName);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;


        return con.ExecuteSPNonQuery("sp_InsertAuditError", true, mParam);
    }


    public int InsertStatecomment(string state, string state_comment, string CmntType)
    {
        mParam = new MySqlParameter[3];



        mParam[0] = new MySqlParameter("?$state", state);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$state_comment", state_comment);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;


        mParam[2] = new MySqlParameter("?$CmntType", CmntType);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_InsertStateComment", true, mParam);
    }

    public DataView ConvertDStoDataview(DataSet ds)
    {
        DataTable dtTable = new DataTable();
        DataColumn dcolumn;

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Order_No";
        dcolumn.Caption = "Order_No";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Name";
        dcolumn.Caption = "Name";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "ErrorField";
        dcolumn.Caption = "ErrorField";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Incorrect";
        dcolumn.Caption = "Incorrect";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Correct";
        dcolumn.Caption = "Correct";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "OP_Comments";
        dcolumn.Caption = "OP_Comments";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "ErrorComments";
        dcolumn.Caption = "ErrorComments";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Error";
        dcolumn.Caption = "Error";
        dcolumn.ReadOnly = true;
        dcolumn.Unique = false;
        dtTable.Columns.Add(dcolumn);

        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dtrow = dtTable.NewRow();
                string strerror = Convert.ToString(ds.Tables[0].Rows[i]["Error"]);
                string strerror1 = Convert.ToString(ds.Tables[0].Rows[i]["Error1"]);

                dtrow[0] = ds.Tables[0].Rows[i]["Order_No"];
                if (strerror == "Error")
                {
                    dtrow[1] = ds.Tables[0].Rows[i]["QC_OP"];
                    dtrow[2] = ds.Tables[0].Rows[i]["ErrorField"];
                    dtrow[3] = ds.Tables[0].Rows[i]["Incorrect"];
                    dtrow[4] = ds.Tables[0].Rows[i]["Correct"];
                    dtrow[5] = ds.Tables[0].Rows[i]["Error_Comments"];
                    dtrow[6] = ds.Tables[0].Rows[i]["QC_OP_Comments"];
                    dtrow[7] = "Production";
                }
                else if (strerror1 == "Error")
                {
                    dtrow[1] = ds.Tables[0].Rows[i]["Review_OP"];
                    dtrow[2] = ds.Tables[0].Rows[i]["ErrorField1"];
                    dtrow[3] = ds.Tables[0].Rows[i]["Incorrect1"];
                    dtrow[4] = ds.Tables[0].Rows[i]["Correct1"];
                    dtrow[5] = ds.Tables[0].Rows[i]["Error_Comments1"];
                    dtrow[6] = ds.Tables[0].Rows[i]["Review_OP_Comments"];
                    dtrow[7] = "QC";
                }
                dtTable.Rows.Add(dtrow);
            }
            dataview = new DataView(dtTable);
        }
        return dataview;
    }

    public DataSet IndividualReport(string frdate, string todate, string strvalue)
    {
        DataSet ds = new DataSet();
        string query = "sp_IndividualReport";
        mParam = new MySqlParameter[3];
        mParam[0] = new MySqlParameter("?$fdate", frdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", todate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$strvalue", strvalue);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(ds);
        return ds;
    }

    public DataView CovertIndivDStoDataview(DataSet ds)
    {
        DataTable dtTable = new DataTable();
        DataColumn dcolumn;

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Name";
        dcolumn.Caption = "Name";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Efficiency";
        dcolumn.Caption = "Efficiency";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Quality";
        dcolumn.Caption = "Quality";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Utilization";
        dcolumn.Caption = "Utilization";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Total Break Time";
        dcolumn.Caption = "Total Break Time";
        dtTable.Columns.Add(dcolumn);

        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dtrow = dtTable.NewRow();
                dtrow[0] = ds.Tables[0].Rows[i]["Name"];
                dtrow[1] = ds.Tables[0].Rows[i]["Efficiency"];
                dtrow[2] = ds.Tables[0].Rows[i]["Quality"];
                dtrow[3] = ds.Tables[0].Rows[i]["Utilization"];
                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[i]["Name"]) == Convert.ToString(ds.Tables[1].Rows[j]["UserName"]))
                    {
                        dtrow[4] = ds.Tables[1].Rows[j]["Total Break Time"];
                        break;
                    }
                    else dtrow[4] = "00:00:00";

                }
                dtTable.Rows.Add(dtrow);
            }
            dataview = new DataView(dtTable);
        }
        else if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count == 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dtrow = dtTable.NewRow();
                dtrow[0] = ds.Tables[0].Rows[i]["Name"];
                dtrow[1] = ds.Tables[0].Rows[i]["Efficiency"];
                dtrow[2] = ds.Tables[0].Rows[i]["Quality"];
                dtrow[3] = ds.Tables[0].Rows[i]["Utilization"];
                dtrow[4] = "00:00:00";
                dtTable.Rows.Add(dtrow);
            }
            dataview = new DataView(dtTable);
        }
        return dataview;
    }

    public DataSet ProjectReport(string frdate, string todate, string strvalue)
    {
        DataSet ds = new DataSet();
        string query = "sp_ProjectReport";
        mParam = new MySqlParameter[3];
        mParam[0] = new MySqlParameter("?$fdate", frdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", todate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$strvalue", strvalue);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(ds);
        return ds;
    }
    public DataSet PendingReport(string frdate, string todate, string query)
    {
        DataSet ds = new DataSet();
        mParam = new MySqlParameter[2];
        mParam[0] = new MySqlParameter("?$fdate", frdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", todate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(ds);
        return ds;
    }

    public DataView CovertProjectDStoDataview(DataSet ds)
    {
        DataTable dtTable = new DataTable();
        DataColumn dcolumn;

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Date";
        dcolumn.Caption = "Date";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "No.of People";
        dcolumn.Caption = "No.of People";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Efficiency";
        dcolumn.Caption = "Efficiency";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Quality";
        dcolumn.Caption = "Quality";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "Utilization";
        dcolumn.Caption = "Utilization";
        dtTable.Columns.Add(dcolumn);

        dcolumn = new DataColumn();
        dcolumn.DataType = System.Type.GetType("System.String");
        dcolumn.ColumnName = "TAT";
        dcolumn.Caption = "TAT";
        dtTable.Columns.Add(dcolumn);

        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dtrow = dtTable.NewRow();
                dtrow[0] = ds.Tables[0].Rows[i]["pdate"];
                dtrow[1] = ds.Tables[0].Rows[i]["TotalPeople"];
                dtrow[2] = ds.Tables[0].Rows[i]["Efficiency"];
                dtrow[3] = ds.Tables[0].Rows[i]["Quality"];
                dtrow[4] = ds.Tables[0].Rows[i]["Utilization"];
                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[i]["pdate"]) == Convert.ToString(ds.Tables[1].Rows[j]["Date"]))
                    {
                        dtrow[5] = ds.Tables[1].Rows[j]["Percentage"];
                        break;
                    }
                    else dtrow[5] = "0%";

                }
                dtTable.Rows.Add(dtrow);
            }
            dataview = new DataView(dtTable);
        }
        else if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count == 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dtrow = dtTable.NewRow();
                dtrow[0] = ds.Tables[0].Rows[i]["pdate"];
                dtrow[1] = ds.Tables[0].Rows[i]["TotalPeople"];
                dtrow[2] = ds.Tables[0].Rows[i]["Efficiency"];
                dtrow[3] = ds.Tables[0].Rows[i]["Quality"];
                dtrow[4] = ds.Tables[0].Rows[i]["Utilization"];
                dtrow[5] = "0%";

                dtTable.Rows.Add(dtrow);
            }
            dataview = new DataView(dtTable);
        }
        return dataview;
    }

    public DataSet LoadNextOrders(string state, string county, string processs)
    {
        mParam = new MySqlParameter[4];

        mParam[0] = new MySqlParameter("?$state", state);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$county", county);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$uid", SessionHandler.UserName);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$processs", processs);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_LoadNextOrders", true, mParam);

    }

    public DataSet LoadLogoutReason()
    {
        DateTime dtt = ToDate();
        string strdate = String.Format("{0:MM/dd/yyyy}", dtt);

        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$pdate", strdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$uid", SessionHandler.UserName);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_LoadLogoutReason", true, mParam);

    }

    public int SaveEntities(string ordno, string pType, string pdate, string entity_type, string tax_amnt, string tax_sts, string tax_freq)
    {
        mParam = new MySqlParameter[8];

        mParam[0] = new MySqlParameter("?$ordno", ordno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$pType", pType);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$pdate", pdate);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$uid", SessionHandler.UserName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$entity_type", entity_type);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$taxamnt", tax_amnt);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$paystatus", tax_sts);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$payfreq", tax_freq);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_InsertEntityType", true, mParam);
    }

    public DataSet FpyReport(string strfrmdate, string strtodate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", strfrmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strtodate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_FpyReport", true, mParam);
    }

    public DataSet GetcheckLogin(string uid, string pword, string sys)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("?$uid", uid);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$pword", pword);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$sys", sys);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_Login", true, mParam);
    }

    public int DelayReason(string Order_no, string pdate, string pType, string oStatus, string Keystart, string Comments)
    {
        mParam = new MySqlParameter[7];

        mParam[0] = new MySqlParameter("?$uid", SessionHandler.UserName);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$OrderNo", Order_no);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$pdate", pdate);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$pType", pType);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$oStatus", oStatus);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$Keystart", Keystart);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$Comments", Comments);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteSPNonQuery("sp_InsertDelay", true, mParam);

    }

    public DataSet OverallProcessTimeReport(string fdate, string tdate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_overallprocesstime", true, mParam);
    }
    public DataView CovertOverallProcessTimetoDataview(DataSet ds)
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataColumn dcolumn;

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order No";
            dcolumn.Caption = "Order No";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Date";
            dcolumn.Caption = "Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "State";
            dcolumn.Caption = "State";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "County";
            dcolumn.Caption = "County";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Type";
            dcolumn.Caption = "Type";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Order Status";
            dcolumn.Caption = "Order Status";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            //dcolumn = new DataColumn();
            //dcolumn.DataType = System.Type.GetType("System.String");
            //dcolumn.ColumnName = "Prod-QC-Total Time";
            //dcolumn.Caption = "Prod-QC-Total Time";
            //dcolumn.ReadOnly = true;
            //dcolumn.Unique = false;
            //dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Production Total Time";
            dcolumn.Caption = "Production Total Time";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "QC Total Time";
            dcolumn.Caption = "QC Total Time";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Total Time";
            dcolumn.Caption = "Total Time";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Delivered Date";
            dcolumn.Caption = "Delivered Date";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Production Attempts";
            dcolumn.Caption = "Production Attempts";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "QC Attempts";
            dcolumn.Caption = "QC Attempts";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtrow = dtTable.NewRow();


                    dtrow[0] = ds.Tables[0].Rows[i]["orderNo"];
                    dtrow[1] = ds.Tables[0].Rows[i]["pdate"];
                    if (dtrow[1].ToString() != "")
                    {
                        DateTime pdt = Convert.ToDateTime(dtrow[1].ToString());
                        dtrow[1] = String.Format("{0:dd-MMM-yyyy}", pdt);
                    }
                    dtrow[2] = ds.Tables[0].Rows[i]["state"];
                    dtrow[3] = ds.Tables[0].Rows[i]["County"];
                    dtrow[4] = ds.Tables[0].Rows[i]["WebPhone"];
                    //dtrow[4] = ds.Tables[0].Rows[i]["processtype"];
                    dtrow[5] = ds.Tables[0].Rows[i]["key_status"];
                    dtrow[6] = ds.Tables[0].Rows[i]["ProdQCTotalProcessTime"];
                    dtrow[7] = ds.Tables[1].Rows[i]["ProdQCTotalProcessTime"];
                    dtrow[8] = ds.Tables[2].Rows[i]["ProdQCTotalProcessTime"];
                    dtrow[9] = ds.Tables[0].Rows[i]["DeliveryDate"];
                    dtrow[10] = ds.Tables[0].Rows[i]["ProductionAttempts"];
                    dtrow[11] = ds.Tables[0].Rows[i]["QCAttempts"];

                    dtTable.Rows.Add(dtrow);
                }
                dataview = new DataView(dtTable);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataview;
    }

    public static string stateYear1 = "";
    public static string stateYear2 = "";
    public static string multipleParcel = "";
    public static string multiParcel_la = "";
    public static string parcelNumber_la = "";
    public static string multiparcel_gw = "";
    public static string multiparcel_ORMulttomah = "";
    public static string multiparcel_WAPierce = "";
    public static string imgURL = "";
    public static string global_orderNo = "";
    public static string global_parcelNo = "";
    public static string parcel_status = "";
    public static string multiparcel_dc = "";
    public static string multiparcel_StLouis = "";
    public static string multipleParcel_deKalb = "";
    public static string multiParcel_washoe = "";
    public static string multiParcel_mecklenberg = "";
    public static string multiParcel_CAKern = "";
    public static string multiParcel_OHFranklin = "";
    public static string multiparcel_CASanJoaquin = "";

    //count
    public static string multiParcel_washoe_count = "";
    public static string multipleParcel_deKalb_count = "";
    public static string multiparcel_StLouis_count = "";
    public static string multiParcel_mecklenberg_count = "";
    public static string multiparcel_gw_count = "";
    public static string multiParcel_CAKern_count = "";
    public static string multiParcel_OHFranklin_Multicount = "";

    public static IWebDriver sDriver;

    public void insert_data(string orderno, string parcelno, int FieldID, string FieldValue, int Table)
    {

        mParam = new MySqlParameter[5];
        mParam[0] = new MySqlParameter("?$order_no", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[0].IsNullable = false;

        mParam[1] = new MySqlParameter("?$parcel_no", parcelno);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$field_id", FieldID);
        mParam[2].MySqlDbType = MySqlDbType.Int16;

        mParam[3] = new MySqlParameter("?$field_value", FieldValue);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$istable", Table);
        mParam[4].MySqlDbType = MySqlDbType.Int16;

        db.ExecuteSPNonQuery("sp_InsertData", true, mParam);

    }
    public DataSet GetCountyId(string state, string county)
    {
        //using (MySqlConnection cnn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString))
        //{
        string query = "SELECT Address_Type,State_County_Id,state_name,county_name ,service_url FROM state_county_master where State_Name = '" + state + "' and County_Name='" + county + "'";
        //cnn.Open();
        //MySqlCommand cmd = new MySqlCommand(query, cnn);
        //MySqlDataAdapter sda = new MySqlDataAdapter();
        DataSet ds = new DataSet();
        ds = con.ExecuteQuery(query);
        //sda.SelectCommand = cmd;
        //sda.Fill(ds);
        //cnn.Close();
        return ds;
    }


    public DataSet GetAddress(string orderno)
    {
        //using (MySqlConnection cnn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString))
        //{
        string query = "select address,scrape from record_status where order_no ='" + orderno + "'";
        //cnn.Open();
        //MySqlCommand cmd = new MySqlCommand(query, cnn);
        MySqlDataAdapter sda = new MySqlDataAdapter();
        DataSet ds = new DataSet();
        ds = con.ExecuteQuery(query);
        //sda.SelectCommand = cmd;
        //sda.Fill(ds);
        //cnn.Close();
        return ds;
        //}

    }
    public DataSet Gettitleflexdata(string orderno)
    {
        using (MySqlConnection cnn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString))
        {
            string query = "select * from titleflex where orderno ='" + orderno + "'";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(query, cnn);
            MySqlDataAdapter sda = new MySqlDataAdapter();
            DataSet ds = new DataSet();
            sda.SelectCommand = cmd;
            sda.Fill(ds);
            cnn.Close();
            return ds;
        }

    }

    //Washoe

    public DataView getassprop_washoeNV(string strorder, string countyid)
    {
        //Situs~Owner 1~Legal Description~Subdivision~Year Built
        //Valuation History~Taxable Land Value~Taxable Improvement Value~Taxable Total~Assessed Land Value~Assessed Improvement Value~Total Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Situs", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            string[] selectedColumnsdt1 = new[] { "Assessed Land Value", "Assessed Improvement Value" };
            dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
            dt.Columns.Add("Owner Name");
            dt.Columns.Add("Assessed Land Value");
            dt.Columns.Add("Assessed Improvement Value");


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Owner Name"] = "";
                dt.Rows[i]["Assessed Land Value"] = dt1.Rows[i]["Assessed Land Value"];
                dt.Rows[i]["Assessed Improvement Value"] = dt1.Rows[i]["Assessed Improvement Value"];

            }
            dt.Columns["Owner Name"].SetOrdinal(1);
            dt.Columns["Year Built"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getdata_NVWashoe(string strorder)
    {
        string strpropaddr = "", strowner = "", strlegal_desc = "", stryearbuilt = "", strtaxauthority = "", strvalue_history = "", strparcel = "", strland = "", strimprov = "", strexem = "";
        string strUnbill_prinicipal = "", strOriginal_Assesment = "", strPayoff = "";
        string strcurr_due_Principal = "", strcurr_due_Interest = "", strcurr_due_Penalty = "", strcurr_due_Total_Due = "";
        DataTable dtall = new DataTable();
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 23 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1  asc limit 1");
        DataTable proptable = new DataTable();
        DataColumn parcelno = proptable.Columns.Add("Parcel No", typeof(string));
        DataColumn Owner = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn Situs = proptable.Columns.Add("Address", typeof(string));
        DataColumn Year_Built = proptable.Columns.Add("Year Built", typeof(string));
        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = proptable.NewRow();
                //  row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Situs, split[0]);
                row.SetField(Owner, split[1]);
                row.SetField(Year_Built, split[4]);
                proptable.Rows.Add(row);

                strpropaddr = split[0]; strowner = split[1]; strlegal_desc = split[2]; stryearbuilt = split[4]; strparcel = dt.Rows[i]["parcel_no"].ToString();
            }

            proptable.AcceptChanges();
            //dataview = new DataView(proptable);
        }
        //cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 23 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 asc limit 1", con);
        //ds = new DataSet();
        //Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 23 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1  asc limit 1");
        DataTable asstable = proptable.Clone();
        DataColumn value_history = asstable.Columns.Add("Valuation History", typeof(string));
        DataColumn Tax_Land_Value = asstable.Columns.Add("Taxable Land Value", typeof(string));
        DataColumn Tax_Imp_Value = asstable.Columns.Add("Taxable Improvement Value", typeof(string));
        DataColumn Tax_Total = asstable.Columns.Add("Taxable Total", typeof(string));
        DataColumn Ass_Land_Value = asstable.Columns.Add("Assessed Land Value", typeof(string));
        DataColumn Ass_Imp_Value = asstable.Columns.Add("Assessed Improvement Value", typeof(string));
        DataColumn Total_ass = asstable.Columns.Add("Total Assessed", typeof(string));
        dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(value_history, split[0]);
                row.SetField(Tax_Land_Value, split[1]);
                row.SetField(Tax_Imp_Value, split[2]);
                row.SetField(Tax_Total, split[3]);
                row.SetField(Ass_Land_Value, split[4]);
                row.SetField(Ass_Imp_Value, split[5]);
                row.SetField(Total_ass, split[6]);
                asstable.Rows.Add(row);
                strvalue_history = split[0]; strland = split[4]; strimprov = split[5];
            }
        }


        //cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 23 where DFM.Category_Id = 6 and DVM.Order_No = '" + strorder + "' order by 1", con);
        //con.Open();
        //ds = new DataSet();
        //Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 23 where DFM.Category_Id = 6 and DVM.Order_No = '" + strorder + "' order by 1  asc limit 1");
        DataTable amgtable = new DataTable();

        DataColumn curr_due_Principal = amgtable.Columns.Add("curr_due_Principal", typeof(string));
        DataColumn curr_due_Interest = amgtable.Columns.Add("curr_due_Interest", typeof(string));
        DataColumn curr_due_Penalty = amgtable.Columns.Add("curr_due_Penalty", typeof(string));
        DataColumn curr_due_Total_Due = amgtable.Columns.Add("curr_due_Total_Due", typeof(string));

        dtcount = dt.Rows.Count;
        string[] amgarray;
        for (int i = 0; i < dtcount; i++)
        {
            amgarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = amgarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = amgtable.NewRow();
                row.SetField(curr_due_Principal, split[7]);
                row.SetField(curr_due_Interest, split[8]);
                row.SetField(curr_due_Penalty, split[9]);
                row.SetField(curr_due_Total_Due, split[11]);
                amgtable.Rows.Add(row);
                strcurr_due_Principal = split[7]; strcurr_due_Interest = split[8]; strcurr_due_Penalty = split[9]; strcurr_due_Total_Due = split[11];
            }

        }
        // asstable.Merge(proptable);
        //asstable.AcceptChanges();
        proptable.Columns.Add("Assessed Land Value");
        proptable.Columns.Add("Assessed Improvement Value");
        proptable.Columns.Add("curr_due_Principal");
        proptable.Columns.Add("curr_due_Interest");
        proptable.Columns.Add("curr_due_Penalty");
        proptable.Columns.Add("curr_due_Total_Due");

        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["Assessed Land Value"] = asstable.Rows[i]["Assessed Land Value"];
            proptable.Rows[i]["Assessed Improvement Value"] = asstable.Rows[i]["Assessed Improvement Value"];
            if (strcurr_due_Principal != "")
            {
                proptable.Rows[i]["curr_due_Principal"] = amgtable.Rows[i]["curr_due_Principal"];
                proptable.Rows[i]["curr_due_Interest"] = amgtable.Rows[i]["curr_due_Interest"];
                proptable.Rows[i]["curr_due_Penalty"] = amgtable.Rows[i]["curr_due_Penalty"];
                proptable.Rows[i]["curr_due_Total_Due"] = amgtable.Rows[i]["curr_due_Total_Due"];
            }
        }

        dataview = new DataView(proptable);
        return dataview;

    }
    //riverside
    public DataView getassprop_river(string strorder)
    {
        DataTable dtall = new DataTable();
        //DataSet ds = new DataSet();
        string strpropaddr = "", strparcel = "", strland = "", strimprove = "", strexem = "", yearbuilt = "";
        //MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString);
        //cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 19 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1", con);
        //MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);

        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 19 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        //Property_Address~Legal_Description
        DataTable proptable = new DataTable();
        // DataColumn order_no = proptable.Columns.Add("Order No", typeof(string));
        DataColumn parcelno = proptable.Columns.Add("Parcel No", typeof(string));
        DataColumn owner = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn Property_Address = proptable.Columns.Add("Property Address", typeof(string));
        DataColumn Legal_Description = proptable.Columns.Add("Legal Description", typeof(string));
        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');
                try
                {
                    DataRow row = proptable.NewRow();
                    //   row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                    row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                    row.SetField(owner, " ");
                    row.SetField(Property_Address, split[0]);
                    row.SetField(Legal_Description, split[1]);
                    proptable.Rows.Add(row);
                }
                catch
                { }

            }
        }
        //ds = new DataSet();
        //cmd = new MySqlCommand("select order_no, parcel_no, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 19 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1", con);
        //Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        //Assessment_Year~Land~Building/Structure~Full_Value~Total_Assessed_Value~Homeowner_Exemption~Net_Assessed_Value
        dt = ReturnDtAPI("select order_no, parcel_no, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 19 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");


        DataTable asstable = new DataTable();
        DataColumn Assessment_Year = asstable.Columns.Add("Assessment Year", typeof(string));
        DataColumn Land = asstable.Columns.Add("Land", typeof(string));
        DataColumn Building_Structure = asstable.Columns.Add("Building/Structure", typeof(string));
        DataColumn Full_Value = asstable.Columns.Add("Full Value", typeof(string));
        DataColumn Total_Assessed_Value = asstable.Columns.Add("Total Assessed_Value", typeof(string));
        DataColumn Homeowner_Exemption = asstable.Columns.Add("Homeowner Exemption", typeof(string));
        DataColumn Net_Assessed_Value = asstable.Columns.Add("Net Assessed Value", typeof(string));
        dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(Assessment_Year, split[0]);
                row.SetField(Land, split[1]);
                row.SetField(Building_Structure, split[2]);
                row.SetField(Full_Value, split[3]);
                row.SetField(Total_Assessed_Value, split[4]);
                row.SetField(Homeowner_Exemption, split[5]);
                row.SetField(Net_Assessed_Value, split[6]);
                asstable.Rows.Add(row);

            }
        }

        proptable.Columns.Add("Land");
        proptable.Columns.Add("Building/Structure");
        proptable.Columns.Add("Homeowner Exemption");

        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["Land"] = asstable.Rows[i]["Land"];
            proptable.Rows[i]["Building/Structure"] = asstable.Rows[i]["Building/Structure"];
            proptable.Rows[i]["Homeowner Exemption"] = asstable.Rows[i]["Homeowner Exemption"];
        }
        dataview = new DataView(proptable);
        return dataview;
    }
    //pierce
    public DataView getassprop_pierce(string strorder)
    {
        DataTable dtall = new DataTable();
        //DataSet ds = new DataSet();
        string strpropaddr = "", strparcel = "", strland = "", strimprove = "", strexem = "", stryearbuilt = "", strowner = "";
        //MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString);
        //cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 20 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 asc", con);
        //MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        //Property_Address~Legal_Description
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 20 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 asc limit 1");
        DataTable proptable = new DataTable();
        // DataColumn order_no = proptable.Columns.Add("Order No", typeof(string));
        DataColumn parcelno = proptable.Columns.Add("Parcel No", typeof(string));
        DataColumn Taxpayer_Name = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn Site_Address = proptable.Columns.Add("Property Address", typeof(string));
        DataColumn Year_Built = proptable.Columns.Add("Year Built", typeof(string));

        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = proptable.NewRow();
                //  row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Taxpayer_Name, split[0]);
                row.SetField(Site_Address, split[1]);
                row.SetField(Year_Built, split[3]);
                proptable.Rows.Add(row);
                strparcel = dt.Rows[i]["parcel_no"].ToString(); strpropaddr = split[1]; strowner = split[0]; stryearbuilt = split[3];
            }
        }
        //ds = new DataSet();
        //cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 20 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1", con);
        //Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 20 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        //Assessment_Year~Land~Building/Structure~Full_Value~Total_Assessed_Value~Homeowner_Exemption~Net_Assessed_Value
        DataTable asstable = new DataTable();
        DataColumn Assessed_Land = asstable.Columns.Add("Assessed Land", typeof(string));
        DataColumn Assessed_Improvements = asstable.Columns.Add("Assessed Improvements", typeof(string));
        DataColumn exem = asstable.Columns.Add("Exemption", typeof(string));
        dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(Assessed_Land, split[2]);
                row.SetField(Assessed_Improvements, split[3]);
                row.SetField(exem, split[6]);
                asstable.Rows.Add(row);
                strland = split[2]; strimprove = split[3]; strexem = split[6];
            }
        }

        proptable.Columns.Add("Assessed Land");
        proptable.Columns.Add("Assessed Improvements");
        proptable.Columns.Add("Exemption");

        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["Assessed Land"] = asstable.Rows[i]["Assessed Land"];
            proptable.Rows[i]["Assessed Improvements"] = asstable.Rows[i]["Assessed Improvements"];
            proptable.Rows[i]["Exemption"] = asstable.Rows[i]["Exemption"];

        }
        dataview = new DataView(proptable);
        return dataview;
    }
    //Gwinett
    public DataView getassprop_gwinett(string strorder)
    {
        DataTable dtall = new DataTable();
        //DataSet ds = new DataSet();
        string strpropaddr = "", strparcel = "", strland = "", strimprove = "", strexem = "", stryearbuilt = "", strowner = "";
        //MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString);
        //cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 22 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 asc", con);
        //MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        //Property_Address~Legal_Description
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 22 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 asc limit 1");
        DataTable proptable = new DataTable();
        //  DataColumn order_no = proptable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = proptable.Columns.Add("Parcel_no", typeof(string));
        DataColumn ownername = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn Property_Address = proptable.Columns.Add("Property Address", typeof(string));
        DataColumn Year = proptable.Columns.Add("Year", typeof(string));
        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = proptable.NewRow();
                //  row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(ownername, split[0]);
                row.SetField(Property_Address, split[2]);
                row.SetField(Year, split[4]);
                proptable.Rows.Add(row);
                strparcel = dt.Rows[i]["parcel_no"].ToString(); strpropaddr = split[2]; strowner = split[0]; stryearbuilt = split[4];
            }
        }
        //ds = new DataSet();
        //cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 22 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1", con);
        //Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        //Assessment_Year~Land~Building/Structure~Full_Value~Total_Assessed_Value~Homeowner_Exemption~Net_Assessed_Value
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 22 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1");
        DataTable asstable = new DataTable();
        DataColumn Assessed_Land_Value = asstable.Columns.Add("Assessed Land Value", typeof(string));
        DataColumn Assessed_Building_Value = asstable.Columns.Add("Assessed Building Value", typeof(string));
        dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(Assessed_Land_Value, split[4]);
                row.SetField(Assessed_Building_Value, split[6]);
                asstable.Rows.Add(row);
                strland = split[4]; strimprove = split[6];
            }
        }

        proptable.Columns.Add("Assessed Land Value");
        proptable.Columns.Add("Assessed Building Value");
        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["Assessed Land Value"] = asstable.Rows[i]["Assessed Land Value"];
            proptable.Rows[i]["Assessed Building Value"] = asstable.Rows[i]["Assessed Building Value"];
        }

        dataview = new DataView(proptable);
        return dataview;
    }
    //Dekalb
    public DataView getassprop_dekalb(string strorder)
    {
        DataTable dtall = new DataTable();
        //DataSet ds = new DataSet();
        //MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString);
        //cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 29  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1", con);
        //MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        //Property_Address~Legal_Description
        DataTable dt = ReturnDtAPI("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 29  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1 limit 1");

        DataTable proptable = new DataTable();
        //  DataColumn order_no = proptable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = proptable.Columns.Add("Parcel_no", typeof(string));
        DataColumn ownername = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn Property_Address = proptable.Columns.Add("Property Address", typeof(string));
        DataColumn Year_Built = proptable.Columns.Add("Year_Built", typeof(string));
        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = proptable.NewRow();
                //  row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(ownername, split[0]);
                row.SetField(Property_Address, split[1]);
                row.SetField(Year_Built, split[2]);
                proptable.Rows.Add(row);
            }
        }
        //ds = new DataSet();
        //cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 29  where DFM.Category_Id = 2  and DVM.order_no='" + strorder + "' order by 1", con);
        //Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        //Assessment_Year~Land~Building/Structure~Full_Value~Total_Assessed_Value~Homeowner_Exemption~Net_Assessed_Value
        dt = ReturnDtAPI("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 29  where DFM.Category_Id = 2  and DVM.order_no='" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn taxable_land = asstable.Columns.Add("Taxable Land", typeof(string));
        DataColumn taxable_improv = asstable.Columns.Add("Taxable Improvements", typeof(string));
        dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(taxable_land, split[2]);
                row.SetField(taxable_improv, split[3]);
                asstable.Rows.Add(row);
            }
        }

        proptable.Columns.Add("Taxable Land");
        proptable.Columns.Add("Taxable Improvements");
        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["Taxable Land"] = asstable.Rows[i]["Taxable Land"];
            proptable.Rows[i]["Taxable Improvements"] = asstable.Rows[i]["Taxable Improvements"];
        }
        dataview = new DataView(proptable);
        return dataview;
    }
    //Mecklenburg
    public DataView getassprop_mecklenburg(string strorder)
    {
        //MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString);
        //MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 30  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1", con);
        //con.Open();
        //DataSet ds = new DataSet();
        //MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        DataTable dt = ReturnDtAPI("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 30  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1 limit 1");
        DataTable propertydetails_table = new DataTable();
        //  DataColumn order_no = propertydetails_table.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = propertydetails_table.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = propertydetails_table.Columns.Add("Owner Name", typeof(string));
        DataColumn Location_Address = propertydetails_table.Columns.Add("Property Address", typeof(string));

        DataColumn Year_Built = propertydetails_table.Columns.Add("Year Built", typeof(string));

        string[] multiarray;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            multiarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = propertydetails_table.NewRow();
                // row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Location_Address, split[1]);
                row.SetField(Owner_Name, split[2]);
                row.SetField(Year_Built, split[7]);
                propertydetails_table.Rows.Add(row);
            }
        }



        //cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 30 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1");
        //Sda = new MySqlDataAdapter();
        //ds = new DataSet();
        //cmd.Connection = con;
        //Sda.SelectCommand = cmd;
        //Sda.Fill(ds);
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 30 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn Land_Value = asstable.Columns.Add("Land_Value", typeof(string));
        DataColumn Building_Value = asstable.Columns.Add("Building_Value", typeof(string));
        DataColumn Exemption_Deferment = asstable.Columns.Add("Exemption_Deferment", typeof(string));


        int dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(Land_Value, split[0]);
                row.SetField(Building_Value, split[1]);
                row.SetField(Exemption_Deferment, split[4]);
                asstable.Rows.Add(row);
            }
        }

        propertydetails_table.Columns.Add("Land_Value");
        propertydetails_table.Columns.Add("Building_Value");
        propertydetails_table.Columns.Add("Exemption_Deferment");
        for (int i = 0; i < propertydetails_table.Rows.Count; i++)
        {
            propertydetails_table.Rows[i]["Land_Value"] = asstable.Rows[i]["Land_Value"];
            propertydetails_table.Rows[i]["Building_Value"] = asstable.Rows[i]["Building_Value"];
            propertydetails_table.Rows[i]["Exemption_Deferment"] = asstable.Rows[i]["Exemption_Deferment"];
        }

        dataview = new DataView(propertydetails_table);
        return dataview;
    }
    //Franklin
    public DataView getassprop_franklin(string strorder)

    {
        //MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString);
        //MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 31  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1", con);
        //con.Open();
        //DataSet ds = new DataSet();
        //MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);

        string qry = "select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 31  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1 limit 1";
        string data = DtConvert.ReadDataFROMCloud(qry);
        IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(data);
        DataTable dt = DtConvert.ToDataTable(UserList);

        DataTable Propertytable = new DataTable();
        // DataColumn order_no = Propertytable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = Propertytable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = Propertytable.Columns.Add("Owner Name", typeof(string));
        DataColumn Site_Address = Propertytable.Columns.Add("Property Address", typeof(string));
        DataColumn year_Built = Propertytable.Columns.Add("Year Built", typeof(string));
        DataColumn Homestead_Credit = Propertytable.Columns.Add("Homestead Credit", typeof(string));

        string[] properarray;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            properarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = properarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = Propertytable.NewRow();
                // row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[0]);
                row.SetField(Site_Address, split[1]);
                row.SetField(year_Built, split[3]);
                row.SetField(Homestead_Credit, split[13]);
                Propertytable.Rows.Add(row);
            }
        }
        //cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 31  where DFM.Category_Id = 2  and DVM.order_no='" + strorder + "' order by 1", con);
        //ds = new DataSet();
        //Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        //Valued_Name~Land_Value~Improvement_Value~Total_Value~Valued_Type~Valued_Year~Net_AnnualTax~TaxPaid

        qry = "select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 31  where DFM.Category_Id = 2  and DVM.order_no='" + strorder + "' order by 1 limit 1";
        data = DtConvert.ReadDataFROMCloud(qry);
        UserList = JsonConvert.DeserializeObject<IList<Testbind>>(data);
        dt = DtConvert.ToDataTable(UserList);

        DataTable Assesstable = new DataTable();
        DataColumn Land_Value = Assesstable.Columns.Add("Land_Value", typeof(string));
        DataColumn Improvement_Value = Assesstable.Columns.Add("Improvement_Value", typeof(string));

        string[] Assessarray;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Assessarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = Assessarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = Assesstable.NewRow();
                row.SetField(Land_Value, split[1]);
                row.SetField(Improvement_Value, split[2]);
                Assesstable.Rows.Add(row);
            }
        }
        Propertytable.Columns.Add("Land_Value");
        Propertytable.Columns.Add("Improvement_Value");
        for (int i = 0; i < Propertytable.Rows.Count; i++)
        {
            Propertytable.Rows[i]["Land_Value"] = Assesstable.Rows[i]["Land_Value"];
            Propertytable.Rows[i]["Improvement_Value"] = Assesstable.Rows[i]["Improvement_Value"];
        }
        dataview = new DataView(Propertytable);
        return dataview;
    }
    //districct of columbia
    public DataView getassprop_doc(string strorder)
    {
        string qry = "select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 32 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1";

        string data = DtConvert.ReadDataFROMCloud(qry);
        IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(data);
        DataTable dt = DtConvert.ToDataTable(UserList);

        DataTable propertytable = new DataTable();
        // DataColumn order_no = propertytable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = propertytable.Columns.Add("Parcel_no", typeof(string));
        DataColumn ownername = propertytable.Columns.Add("Owner Name", typeof(string));
        DataColumn Property_Address = propertytable.Columns.Add("Property Address", typeof(string));
        DataColumn Homestead_Status = propertytable.Columns.Add("Homestead Status", typeof(string));
        DataColumn Year_Built = propertytable.Columns.Add("Year Built", typeof(string));
        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = propertytable.NewRow();
                //  row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(ownername, split[0]);
                row.SetField(Property_Address, split[1]);
                row.SetField(Homestead_Status, split[3]);
                row.SetField(Year_Built, split[4]);
                propertytable.Rows.Add(row);
            }

        }

        string queryAss = "select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 32 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1";
        string dataAss = DtConvert.ReadDataFROMCloud(queryAss);
        UserList = JsonConvert.DeserializeObject<IList<Testbind>>(dataAss);
        dt = DtConvert.ToDataTable(UserList);

        DataTable assessmenttable = new DataTable();
        DataColumn Assessment_Land = assessmenttable.Columns.Add("Assessment_Land", typeof(string));
        DataColumn Assessment_Improvements = assessmenttable.Columns.Add("Assessment_Improvements", typeof(string));
        dtcount = dt.Rows.Count;
        string[] assarray;
        for (int i = 0; i < dtcount; i++)
        {
            assarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = assessmenttable.NewRow();
                row.SetField(Assessment_Land, split[0]);
                row.SetField(Assessment_Improvements, split[1]);
                assessmenttable.Rows.Add(row);
            }
        }
        propertytable.Columns.Add("Assessment_Land");
        propertytable.Columns.Add("Assessment_Improvements");
        for (int i = 0; i < propertytable.Rows.Count; i++)
        {
            propertytable.Rows[i]["Assessment_Land"] = assessmenttable.Rows[i]["Assessment_Land"];
            propertytable.Rows[i]["Assessment_Improvements"] = assessmenttable.Rows[i]["Assessment_Improvements"];
        }

        dataview = new DataView(propertytable);
        return dataview;

    }
    //STLouisCounty
    public DataView getassprop_louis(string strorder)
    {
        //MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString);
        //MySqlCommand cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 33  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1", con);
        //con.Open();
        //DataSet ds = new DataSet();
        //MySqlDataAdapter Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        //situs_address~legal_description
        DataTable dt = ReturnDtAPI("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 33  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1 limit 1");
        DataTable PropertyDetails_table = new DataTable();
        // DataColumn order_no = PropertyDetails_table.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = PropertyDetails_table.Columns.Add("Parcel_no", typeof(string));
        DataColumn owner_name = PropertyDetails_table.Columns.Add("Owner Name", typeof(string));
        DataColumn taxing_address = PropertyDetails_table.Columns.Add("Property Address", typeof(string));
        DataColumn year_built = PropertyDetails_table.Columns.Add("Year Built", typeof(string));

        int dtcount = dt.Rows.Count;
        string[] proparray;
        for (int i = 0; i < dtcount; i++)
        {
            proparray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = proparray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = PropertyDetails_table.NewRow();
                //  row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(owner_name, split[0]);
                row.SetField(taxing_address, split[1]);
                row.SetField(year_built, split[5]);

                PropertyDetails_table.Rows.Add(row);
            }
        }


        //cmd = new MySqlCommand("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 33  where DFM.Category_Id = 2  and DVM.order_no='" + strorder + "' order by 1 asc limit 1", con);
        //ds = new DataSet();
        //Sda = new MySqlDataAdapter(cmd);
        //Sda.Fill(ds);
        dt = ReturnDtAPI("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = 33  where DFM.Category_Id = 2  and DVM.order_no='" + strorder + "' order by 1 asc limit 1");
        DataTable AssessmentDetails_table = new DataTable();
        DataColumn Assessed_Land_Value = AssessmentDetails_table.Columns.Add("Assessed_Land_Value", typeof(string));
        DataColumn Assessed_Improvement_Value = AssessmentDetails_table.Columns.Add("Assessed_Improvement_Value", typeof(string));

        dtcount = dt.Rows.Count;
        string[] multiarray;
        for (int i = 0; i < dtcount; i++)
        {
            multiarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = AssessmentDetails_table.NewRow();
                row.SetField(Assessed_Land_Value, split[4]);
                row.SetField(Assessed_Improvement_Value, split[5]);
                AssessmentDetails_table.Rows.Add(row);
            }
        }
        PropertyDetails_table.Columns.Add("Assessed_Land_Value");
        PropertyDetails_table.Columns.Add("Assessed_Improvement_Value");
        for (int i = 0; i < PropertyDetails_table.Rows.Count; i++)
        {
            PropertyDetails_table.Rows[i]["Assessed_Land_Value"] = AssessmentDetails_table.Rows[i]["Assessed_Land_Value"];
            PropertyDetails_table.Rows[i]["Assessed_Improvement_Value"] = AssessmentDetails_table.Rows[i]["Assessed_Improvement_Value"];
        }
        dataview = new DataView(PropertyDetails_table);
        return dataview;
    }
    //kern
    public DataView getassprop_kern(string strorder, string countyid)
    {
        //Property Address~Tax Rate Area
        //Assessed Year~Land~Improvements~Minerals~Personal Property~Other Improvements~Exemptions~Net Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Tax Rate Area" };
        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            string[] selectedColumnsdt1 = new[] { "Land", "Improvements", "Exemptions" };
            try
            {
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Improvements");
                dt.Columns.Add("Exemptions");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                    dt.Rows[i]["Exemptions"] = dt1.Rows[i]["Homeowners_Exemption"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    //multnomah
    public DataView getassprop_multnomah(string strorder)
    {
        //MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["scraping"].ConnectionString);
        //MySqlCommand cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 40 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1");
        //MySqlDataAdapter sda = new MySqlDataAdapter();
        //DataSet ds = new DataSet();
        //cmd.Connection = con;
        //sda.SelectCommand = cmd;
        //sda.Fill(ds);
        //Alternate_Parcel_Number~Map_Tax_Lot~Property_Address~Owner_Name~Legal_Description~Levy_Code_Area~Property_Use~Year_Built~Account_Status
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 40 where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable proptable = new DataTable();
        // DataColumn order_no = proptable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = proptable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn Alter_Parcelno = proptable.Columns.Add("Alter Parcel no", typeof(string));
        DataColumn Property_Address = proptable.Columns.Add("Property Address", typeof(string));

        DataColumn Year_Built = proptable.Columns.Add("Year Built", typeof(string));
        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = proptable.NewRow();
                //  row.SetField(order_no, dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Alter_Parcelno, split[0]);
                row.SetField(Property_Address, split[2]);
                row.SetField(Owner_Name, split[3]);
                row.SetField(Year_Built, split[7]);
                proptable.Rows.Add(row);
            }

        }
        //cmd = new MySqlCommand("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 40 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1");
        //sda = new MySqlDataAdapter();
        //ds = new DataSet();
        //cmd.Connection = con;
        //sda.SelectCommand = cmd;
        //sda.Fill(ds);
        //Assessed_Year~Land_Value~Building_Value~Market_Value~Exemptions~SpecialMkt_Use~Total_Assessed_Value
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 40 where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn Land = asstable.Columns.Add("Land", typeof(string));
        DataColumn Building = asstable.Columns.Add("Building", typeof(string));
        DataColumn Exemption_Amount = asstable.Columns.Add("Exemptions", typeof(string));

        dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(Land, split[1]);
                row.SetField(Building, split[2]);
                row.SetField(Exemption_Amount, split[4]);
                asstable.Rows.Add(row);
            }
        }

        proptable.Columns.Add("Land");
        proptable.Columns.Add("Building");
        proptable.Columns.Add("Exemptions");
        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["Land"] = asstable.Rows[i]["Land"];
            proptable.Rows[i]["Building"] = asstable.Rows[i]["Building"];
            proptable.Rows[i]["Exemptions"] = asstable.Rows[i]["Exemptions"];
        }
        dataview = new DataView(proptable);
        return dataview;
    }
    public DataView getassprop_sanjoaquin(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = '" + countyid + "'  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1 limit 1");
        DataTable property_table = new DataTable();
        DataColumn parcelno = property_table.Columns.Add("Parcel_no", typeof(string));
        DataColumn owner = property_table.Columns.Add("Owner Name", typeof(string));
        DataColumn property_address = property_table.Columns.Add("Property Address", typeof(string));


        string[] propertyarray;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = property_table.NewRow();
                row.SetField(parcelno, dt.Rows[i]["Parcel_no"].ToString());
                row.SetField(owner, "");
                row.SetField(property_address, split[0]);
                property_table.Rows.Add(row);
            }
        }
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn parcel_no = asstable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Land_Value = asstable.Columns.Add("Land_Value", typeof(string));
        DataColumn Structure = asstable.Columns.Add("Structure", typeof(string));
        DataColumn Homeowners_Exemption = asstable.Columns.Add("Homeowners_Exemption", typeof(string));
        int dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };

            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(parcel_no, dt.Rows[i]["Parcel_no"].ToString());
                row.SetField(Land_Value, split[0]);
                row.SetField(Structure, split[1]);
                row.SetField(Homeowners_Exemption, split[7]);
                asstable.Rows.Add(row);
            }
        }


        property_table.Columns.Add("Land_Value");
        property_table.Columns.Add("Structure");
        property_table.Columns.Add("Homeowners_Exemption");
        for (int i = 0; i < property_table.Rows.Count; i++)
        {
            property_table.Rows[i]["Land_Value"] = asstable.Rows[i]["Land_Value"];
            property_table.Rows[i]["Structure"] = asstable.Rows[i]["Structure"];
            property_table.Rows[i]["Homeowners_Exemption"] = asstable.Rows[i]["Homeowners_Exemption"];
        }
        dataview = new DataView(property_table);
        return dataview;
    }
    public DataView getassprop_bernalillo(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = '" + countyid + "'  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1 limit 1");
        DataTable property_table = new DataTable();
        DataColumn parcelno = property_table.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner = property_table.Columns.Add("Owner Name", typeof(string));
        DataColumn LocationAddress = property_table.Columns.Add("Property Address", typeof(string));
        DataColumn YearBuilt = property_table.Columns.Add("Year Built", typeof(string));

        string[] propertyarray;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = property_table.NewRow();
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner, split[3]);
                row.SetField(LocationAddress, split[4]);
                row.SetField(YearBuilt, split[8]);
                property_table.Rows.Add(row);
            }
        }
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn Fulllandvalue = asstable.Columns.Add("Fulllandvalue", typeof(string));
        DataColumn Agricland = asstable.Columns.Add("Agricland", typeof(string));
        DataColumn FullImpVlaue = asstable.Columns.Add("FullImpVlaue", typeof(string));


        int dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(Fulllandvalue, split[1]);
                row.SetField(Agricland, split[2]);
                row.SetField(FullImpVlaue, split[3]);
                asstable.Rows.Add(row);
            }
        }

        property_table.Columns.Add("Fulllandvalue");
        property_table.Columns.Add("Agricland");
        property_table.Columns.Add("FullImpVlaue");
        for (int i = 0; i < property_table.Rows.Count; i++)
        {
            property_table.Rows[i]["Fulllandvalue"] = asstable.Rows[i]["Fulllandvalue"];
            property_table.Rows[i]["Agricland"] = asstable.Rows[i]["Agricland"];
            property_table.Rows[i]["FullImpVlaue"] = asstable.Rows[i]["FullImpVlaue"];
        }
        dataview = new DataView(property_table);
        return dataview;
    }
    public DataView getassprop_sanfrancisco(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select DVM.Order_No, DVM.Parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id  join state_county_master SCM on SCM.ID = DFM.State_County_ID and DFM.State_County_ID = '" + countyid + "'  where DFM.Category_Id = 1  and DVM.order_no='" + strorder + "' order by 1 limit 1");
        DataTable property_table = new DataTable();
        DataColumn parcelno = property_table.Columns.Add("Parcel_no", typeof(string));
        DataColumn owner = property_table.Columns.Add("Owner Name", typeof(string));
        DataColumn property_address = property_table.Columns.Add("Property Address", typeof(string));
        DataColumn year_built = property_table.Columns.Add("Year Built", typeof(string));

        string[] multiarray;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            multiarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = property_table.NewRow();
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(owner, "");
                row.SetField(property_address, split[0]);
                row.SetField(year_built, split[1]);
                property_table.Rows.Add(row);
            }
        }

        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn Land_Value = asstable.Columns.Add("Land_Value", typeof(string));
        DataColumn Building_Value = asstable.Columns.Add("Building_Value", typeof(string));

        int dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(Land_Value, split[0]);
                row.SetField(Building_Value, split[1]);
                asstable.Rows.Add(row);
            }
        }

        property_table.Columns.Add("Land_Value");
        property_table.Columns.Add("Building_Value");

        for (int i = 0; i < property_table.Rows.Count; i++)
        {
            property_table.Rows[i]["Land_Value"] = asstable.Rows[i]["Land_Value"];
            property_table.Rows[i]["Building_Value"] = asstable.Rows[i]["Building_Value"];

        }
        dataview = new DataView(property_table);
        return dataview;
    }
    public DataView getassprop_fresno(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_Text_Id,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id where DFM.Category_Id = 16 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = 37 where DFM.Category_Id = 9 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Address" };

        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);

            string[] selectedColumnsdt1 = new[] { "Land", "Improvements", "Exemption" };
            dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

            dt.Columns.Add("Land");
            dt.Columns.Add("Improvements");
            dt.Columns.Add("Exemption");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                dt.Rows[i]["Exemption"] = dt1.Rows[i]["Exemption"];
            }

            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_harrison(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable proptable = new DataTable();
        DataColumn parcelno = proptable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn Property_Address = proptable.Columns.Add("Property Address", typeof(string));
        DataColumn Exempt_Code = proptable.Columns.Add("Exempt Code", typeof(string));
        DataColumn Homestead_Code = proptable.Columns.Add("Homestead Code", typeof(string));
        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');
                DataRow row = proptable.NewRow();
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[0]);
                row.SetField(Property_Address, split[1]);
                row.SetField(Exempt_Code, split[3]);
                row.SetField(Homestead_Code, split[4]);
                proptable.Rows.Add(row);
            }

        }

        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn Assessed_Land_Value = asstable.Columns.Add("Assessed_Land_Value", typeof(string));
        DataColumn Assessed_Improvement_Value = asstable.Columns.Add("Assessed_Improvement_Value", typeof(string));

        dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(Assessed_Land_Value, split[1]);
                row.SetField(Assessed_Improvement_Value, split[2]);
                asstable.Rows.Add(row);
            }
        }

        proptable.Columns.Add("Assessed_Land_Value");
        proptable.Columns.Add("Assessed_Improvement_Value");

        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["Assessed_Land_Value"] = asstable.Rows[i]["Assessed_Land_Value"];
            proptable.Rows[i]["Assessed_Improvement_Value"] = asstable.Rows[i]["Assessed_Improvement_Value"];

        }
        dataview = new DataView(proptable);
        return dataview;
    }
    public DataView getassprop_stcharles(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable propertydetails_table = new DataTable();
        DataColumn Parcel_ID = propertydetails_table.Columns.Add("Parcel ID", typeof(string));
        DataColumn Owner_Name = propertydetails_table.Columns.Add("Owner Name", typeof(string));
        DataColumn Property_Address = propertydetails_table.Columns.Add("Property Address", typeof(string));

        DataColumn Year_Built = propertydetails_table.Columns.Add("Year Built", typeof(string));

        string[] multiarray;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            multiarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = propertydetails_table.NewRow();
                row.SetField(Parcel_ID, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[1]);
                row.SetField(Property_Address, split[2]);
                row.SetField(Year_Built, split[9]);
                propertydetails_table.Rows.Add(row);
            }
        }

        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn CommercialValue = asstable.Columns.Add("CommercialValue", typeof(string));
        DataColumn Total_market_value = asstable.Columns.Add("Total_market_value", typeof(string));
        DataColumn ResidentialValue = asstable.Columns.Add("ResidentialValue", typeof(string));
        DataColumn LandValue = asstable.Columns.Add("LandValue", typeof(string));
        DataColumn AgricultureValue = asstable.Columns.Add("AgricultureValue", typeof(string));

        int dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(CommercialValue, split[0]);
                row.SetField(Total_market_value, split[1]);
                row.SetField(ResidentialValue, split[2]);
                row.SetField(LandValue, split[3]);
                row.SetField(AgricultureValue, split[4]);

                asstable.Rows.Add(row);
            }
        }
        propertydetails_table.Columns.Add("CommercialValue");
        propertydetails_table.Columns.Add("Total_market_value");
        propertydetails_table.Columns.Add("ResidentialValue");
        propertydetails_table.Columns.Add("LandValue");
        propertydetails_table.Columns.Add("AgricultureValue");

        for (int i = 0; i < propertydetails_table.Rows.Count; i++)
        {
            propertydetails_table.Rows[i]["CommercialValue"] = asstable.Rows[i]["CommercialValue"];
            propertydetails_table.Rows[i]["Total_market_value"] = asstable.Rows[i]["Total_market_value"];
            propertydetails_table.Rows[i]["ResidentialValue"] = asstable.Rows[i]["ResidentialValue"];
            propertydetails_table.Rows[i]["LandValue"] = asstable.Rows[i]["LandValue"];
            propertydetails_table.Rows[i]["AgricultureValue"] = asstable.Rows[i]["AgricultureValue"];

        }
        dataview = new DataView(propertydetails_table);
        return dataview;
    }
    public DataView getassprop_tulsa(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");

        DataTable proptable = new DataTable();
        DataColumn parcelno = proptable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn Property_Address = proptable.Columns.Add("Property Address", typeof(string));
        DataColumn Year_Built = proptable.Columns.Add("Year Built", typeof(string));

        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = proptable.NewRow();
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[1]);
                row.SetField(Property_Address, split[2]);
                row.SetField(Year_Built, split[5]);
                proptable.Rows.Add(row);
            }

        }
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn LandValue = asstable.Columns.Add("LandValue", typeof(string));
        DataColumn Improvementsvalue = asstable.Columns.Add("Improvementsvalue", typeof(string));
        DataColumn Homestead = asstable.Columns.Add("Homestead", typeof(string));
        DataColumn AdditionalHomestead = asstable.Columns.Add("AdditionalHomestead", typeof(string));


        dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(LandValue, split[7]);
                row.SetField(Improvementsvalue, split[8]);
                row.SetField(Homestead, split[9]);


                asstable.Rows.Add(row);
            }
        }
        proptable.Columns.Add("LandValue");
        proptable.Columns.Add("Improvementsvalue");
        proptable.Columns.Add("Homestead");



        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["LandValue"] = asstable.Rows[i]["LandValue"];
            proptable.Rows[i]["Improvementsvalue"] = asstable.Rows[i]["Improvementsvalue"];
            proptable.Rows[i]["Homestead"] = asstable.Rows[i]["Homestead"];

        }
        dataview = new DataView(proptable);
        return dataview;
    }
    public DataView getassprop_utah(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable proptable = new DataTable();
        DataColumn parcelno = proptable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn Property_Address = proptable.Columns.Add("Property Address", typeof(string));

        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = proptable.NewRow();
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[0]);
                row.SetField(Property_Address, split[1]);
                proptable.Rows.Add(row);
            }

        }
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        //DataTable asstable = new DataTable();
        DataTable assessmenttable = new DataTable();
        DataColumn Real_Estate = assessmenttable.Columns.Add("Real_Estate", typeof(string));
        DataColumn Taxable = assessmenttable.Columns.Add("Taxable", typeof(string));
        DataColumn Market = assessmenttable.Columns.Add("Market", typeof(string));
        dtcount = dt.Rows.Count;

        string[] propertyarray1;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray1 = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray1[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = assessmenttable.NewRow();
                row.SetField(Real_Estate, split[1]);
                row.SetField(Taxable, split[2]);
                row.SetField(Market, split[3]);
                assessmenttable.Rows.Add(row);
            }

        }
        proptable.Columns.Add("Real_Estate");
        proptable.Columns.Add("Taxable");
        proptable.Columns.Add("Market");



        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["Real_Estate"] = assessmenttable.Rows[i]["Real_Estate"];
            proptable.Rows[i]["Taxable"] = assessmenttable.Rows[i]["Taxable"];
            proptable.Rows[i]["Market"] = assessmenttable.Rows[i]["Market"];
        }
        dataview = new DataView(proptable);
        return dataview;
    }
    public DataView getassprop_summit(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable propertydetails_table = new DataTable();
        DataColumn Parcel_ID = propertydetails_table.Columns.Add("Parcel ID", typeof(string));
        DataColumn OwnerName = propertydetails_table.Columns.Add("Owner Name", typeof(string));
        DataColumn AlterNateID = propertydetails_table.Columns.Add("AlterNate ID", typeof(string));

        DataColumn PropertyAddress = propertydetails_table.Columns.Add("Property Address", typeof(string));
        DataColumn YearBuilt = propertydetails_table.Columns.Add("Year Built", typeof(string));


        string[] multiarray;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            multiarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = propertydetails_table.NewRow();
                row.SetField(Parcel_ID, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(AlterNateID, split[0]);
                row.SetField(PropertyAddress, split[1]);
                row.SetField(OwnerName, split[2]);
                row.SetField(YearBuilt, split[4]);

                propertydetails_table.Rows.Add(row);
            }
        }
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn AssessedLand = asstable.Columns.Add("AssessedLand", typeof(string));
        DataColumn AssessedBuilding = asstable.Columns.Add("AssessedBuilding", typeof(string));
        DataColumn Homestead = asstable.Columns.Add("Homestead", typeof(string));

        int dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(AssessedLand, split[4]);
                row.SetField(AssessedBuilding, split[5]);
                row.SetField(Homestead, split[7]);
                asstable.Rows.Add(row);
            }
        }
        propertydetails_table.Columns.Add("AssessedLand");
        propertydetails_table.Columns.Add("AssessedBuilding");
        propertydetails_table.Columns.Add("Homestead");

        for (int i = 0; i < propertydetails_table.Rows.Count; i++)
        {
            propertydetails_table.Rows[i]["AssessedLand"] = asstable.Rows[i]["AssessedLand"];
            propertydetails_table.Rows[i]["AssessedBuilding"] = asstable.Rows[i]["AssessedBuilding"];
            propertydetails_table.Rows[i]["Homestead"] = asstable.Rows[i]["Homestead"];
        }
        dataview = new DataView(propertydetails_table);
        return dataview;
    }
    public DataView getassprop_hennepin(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable proptable = new DataTable();
        DataColumn parcel_no = proptable.Columns.Add("parcel_no", typeof(string));
        DataColumn Owner_name = proptable.Columns.Add("Owner name", typeof(string));
        DataColumn address = proptable.Columns.Add("Property Address", typeof(string));
        DataColumn Construction_year = proptable.Columns.Add("Year Built", typeof(string));

        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');
                DataRow row = proptable.NewRow();
                row.SetField(parcel_no, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(address, split[0]);
                row.SetField(Construction_year, split[3]);
                row.SetField(Owner_name, split[4]);
                proptable.Rows.Add(row);
            }

        }
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");

        DataTable assesstable = new DataTable();
        DataColumn Land_market = assesstable.Columns.Add("Land_market", typeof(string));
        DataColumn Building_market = assesstable.Columns.Add("Building_market", typeof(string));
        DataColumn Homestead_status = assesstable.Columns.Add("Homestead_status", typeof(string));
        DataColumn Exempt_status = assesstable.Columns.Add("Exempt_status", typeof(string));
        dtcount = dt.Rows.Count;
        string[] propertyarray1;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray1 = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray1[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = assesstable.NewRow();
                row.SetField(Land_market, split[0]);
                row.SetField(Building_market, split[1]);
                row.SetField(Homestead_status, split[8]);
                row.SetField(Exempt_status, split[11]);
                assesstable.Rows.Add(row);
            }

        }

        proptable.Columns.Add("Land_market");
        proptable.Columns.Add("Building_market");
        proptable.Columns.Add("Homestead_status");
        proptable.Columns.Add("Exempt_status");


        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["Land_market"] = assesstable.Rows[i]["Land_market"];
            proptable.Rows[i]["Building_market"] = assesstable.Rows[i]["Building_market"];
            proptable.Rows[i]["Homestead_status"] = assesstable.Rows[i]["Homestead_status"];
            proptable.Rows[i]["Exempt_status"] = assesstable.Rows[i]["Exempt_status"];

        }
        dataview = new DataView(proptable);
        return dataview;
    }
    public DataView getassprop_newcastle(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable property_table = new DataTable();
        DataColumn parcelno = property_table.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner = property_table.Columns.Add("Owner Name", typeof(string));
        DataColumn Property_Address = property_table.Columns.Add("Property Address", typeof(string));

        DataColumn Year_Built = property_table.Columns.Add("Year Built", typeof(string));

        string[] propertyarray;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = property_table.NewRow();
                row.SetField(parcelno, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Property_Address, split[0]);
                row.SetField(Owner, split[2]);
                row.SetField(Year_Built, split[5]);
                property_table.Rows.Add(row);
            }
        }
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");

        DataTable asstable = new DataTable();
        DataColumn Land = asstable.Columns.Add("Land", typeof(string));
        DataColumn Structure = asstable.Columns.Add("Structure", typeof(string));

        int dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = asstable.NewRow();
                row.SetField(Land, split[0]);
                row.SetField(Structure, split[1]);
                asstable.Rows.Add(row);
            }
        }

        property_table.Columns.Add("Land");
        property_table.Columns.Add("Structure");
        for (int i = 0; i < property_table.Rows.Count; i++)
        {
            property_table.Rows[i]["Land"] = asstable.Rows[i]["Land"];
            property_table.Rows[i]["Structure"] = asstable.Rows[i]["Structure"];
        }
        dataview = new DataView(property_table);
        return dataview;
    }
    public DataView getassprop_pinal(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable proptable = new DataTable();
        DataColumn parcel_no = proptable.Columns.Add("parcel_no", typeof(string));
        DataColumn Owner_name = proptable.Columns.Add("Owner Name", typeof(string));
        DataColumn property_Address = proptable.Columns.Add("Property Address", typeof(string));
        DataColumn year_built = proptable.Columns.Add("Year Built", typeof(string));


        int dtcount = dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = proptable.NewRow();
                row.SetField(parcel_no, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_name, split[0]);
                row.SetField(property_Address, split[1]);
                row.SetField(year_built, split[4]);
                proptable.Rows.Add(row);
            }
        }

        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable asstable = new DataTable();
        DataColumn FullCashValue = asstable.Columns.Add("FullCashValue", typeof(string));
        DataColumn LimitedValue = asstable.Columns.Add("LimitedValue", typeof(string));
        DataColumn RealPropertyRatio = asstable.Columns.Add("RealPropertyRatio", typeof(string));
        DataColumn AssessedFCV = asstable.Columns.Add("AssessedFCV", typeof(string));
        DataColumn AssessedLPV = asstable.Columns.Add("AssessedLPV", typeof(string));

        dtcount = dt.Rows.Count;
        string[] assementarray;
        for (int i = 0; i < dtcount; i++)
        {
            assementarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = assementarray[0].ToString();
            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');
                DataRow row = asstable.NewRow();
                row.SetField(FullCashValue, split[1]);
                row.SetField(LimitedValue, split[2]);
                row.SetField(RealPropertyRatio, split[3]);
                row.SetField(AssessedFCV, split[4]);
                row.SetField(AssessedLPV, split[5]);
                asstable.Rows.Add(row);
            }
        }
        proptable.Columns.Add("FullCashValue");
        proptable.Columns.Add("LimitedValue");
        proptable.Columns.Add("RealPropertyRatio");
        proptable.Columns.Add("AssessedFCV");
        proptable.Columns.Add("AssessedLPV");

        for (int i = 0; i < proptable.Rows.Count; i++)
        {
            proptable.Rows[i]["FullCashValue"] = asstable.Rows[i]["FullCashValue"];
            proptable.Rows[i]["LimitedValue"] = asstable.Rows[i]["LimitedValue"];
            proptable.Rows[i]["RealPropertyRatio"] = asstable.Rows[i]["RealPropertyRatio"];
            proptable.Rows[i]["AssessedFCV"] = asstable.Rows[i]["AssessedFCV"];
            proptable.Rows[i]["AssessedLPV"] = asstable.Rows[i]["AssessedLPV"];
        }
        dataview = new DataView(proptable);
        return dataview;
    }
    public DataView getassprop_marion(string strorder, string countyid)
    {
        DataTable dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable propertydetails_table = new DataTable();
        DataColumn Parcel_ID = propertydetails_table.Columns.Add("Parcel ID", typeof(string));
        DataColumn Owner = propertydetails_table.Columns.Add("Owner Name", typeof(string));
        DataColumn AccountNo = propertydetails_table.Columns.Add("Account No", typeof(string));
        DataColumn SitusAddress = propertydetails_table.Columns.Add("Property Address", typeof(string));

        string[] multiarray;
        int dtcount = dt.Rows.Count;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            multiarray = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = propertydetails_table.NewRow();
                row.SetField(Parcel_ID, dt.Rows[i]["parcel_no"].ToString());
                row.SetField(AccountNo, split[0]);
                row.SetField(Owner, split[1]);
                row.SetField(SitusAddress, split[2]);
                propertydetails_table.Rows.Add(row);
            }
        }
        dt = ReturnDtAPI("select order_no, parcel_no,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");

        DataTable ass_table = new DataTable();
        DataColumn ImprovementsRMV = ass_table.Columns.Add("ImprovementsRMV", typeof(string));
        DataColumn LandRMV = ass_table.Columns.Add("LandRMV", typeof(string));
        DataColumn Exemptions = ass_table.Columns.Add("Exemptions", typeof(string));
        dtcount = dt.Rows.Count;
        string[] multiarray1;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            multiarray1 = new string[] { dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray1[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = ass_table.NewRow();
                row.SetField(ImprovementsRMV, split[1]);
                row.SetField(LandRMV, split[2]);
                row.SetField(Exemptions, split[4]);
                ass_table.Rows.Add(row);
            }
        }
        propertydetails_table.Columns.Add("ImprovementsRMV");
        propertydetails_table.Columns.Add("LandRMV");
        propertydetails_table.Columns.Add("Exemptions");


        for (int i = 0; i < propertydetails_table.Rows.Count; i++)
        {
            propertydetails_table.Rows[i]["ImprovementsRMV"] = ass_table.Rows[i]["ImprovementsRMV"];
            propertydetails_table.Rows[i]["LandRMV"] = ass_table.Rows[i]["LandRMV"];
            propertydetails_table.Rows[i]["Exemptions"] = ass_table.Rows[i]["Exemptions"];

        }
        dataview = new DataView(propertydetails_table);
        return dataview;
    }
    public DataView getassprop_placer(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        string[] selectedColumnsdt = new[] { "parcel_no", "Situs_Address", "Year_Built" };
        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            string[] selectedColumnsdt1 = new[] { "Land", "Structure", "Homeowners_Exemption", "Other_Exemption" };
            try
            {
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Structure");
                dt.Columns.Add("Homeowners_Exemption");
                dt.Columns.Add("Other_Exemption");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Structure"] = dt1.Rows[i]["Structure"];
                    dt.Rows[i]["Homeowners_Exemption"] = dt1.Rows[i]["Homeowners_Exemption"];
                    dt.Rows[i]["Other_Exemption"] = dt1.Rows[i]["Other_Exemption"];
                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Situs_Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_shelby(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        string[] selectedColumnsdt = new[] { "parcel_no", "ownername", "PropertyAddress" };
        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            string[] selectedColumnsdt1 = new[] { "LandAppraisal", "BuildingAppraisal" };
            try
            {
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("LandAppraisal");
                dt.Columns.Add("BuildingAppraisal");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["LandAppraisal"] = dt1.Rows[i]["LandAppraisal"];
                    dt.Rows[i]["BuildingAppraisal"] = dt1.Rows[i]["BuildingAppraisal"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_clackamas(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property_Address", "Year_Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Real_Mkt_Land", "Real_Mkt_Bldg", "Exempt" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Real_Mkt_Land");
                dt.Columns.Add("Real_Mkt_Bldg");
                dt.Columns.Add("Exempt");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Real_Mkt_Land"] = dt1.Rows[i]["Real_Mkt_Land"];
                    dt.Rows[i]["Real_Mkt_Bldg"] = dt1.Rows[i]["Real_Mkt_Bldg"];
                    dt.Rows[i]["Exempt"] = dt1.Rows[i]["Exempt"];
                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Property_Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_eastbatonrouge(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "OwnerName", "Propertyaddress" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);

            try
            {
                string[] selectedColumnsdt1 = new[] { "AssedValues", "Homestead" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("AssedValues");
                dt.Columns.Add("Homestead");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["AssedValues"] = dt1.Rows[i]["AssedValues"];
                    dt.Rows[i]["Homestead"] = dt1.Rows[i]["Homestead"];

                }
            }
            catch
            {

            }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_baltimorecity(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        string[] selectedColumnsdt = new[] { "parcel_no", "Owner_name", "Year_Built", "Homestead_Application_Status" };

        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            string[] selectedColumnsdt1 = new[] { "Land", "Building" };
            try
            {
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Land");
                dt.Columns.Add("Building");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_polk(string strorder, string countyid)
    {
        //Geoparcel~Map~Nbhd~Jurisdiction~School District~Street Address~City State Zipcode~Mailing Address~Legal Description~Acres~Year Built
        //Tax Year~Type~Class Description~Land~Buildings~Total
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Street Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Buildings" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Buildings");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Buildings"] = dt1.Rows[i]["Buildings"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Street Address"].SetOrdinal(2);
            }
            catch { }

            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_charleston(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "OwnerName", "Proprty_Address", "Year_Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "AppraisedLandValue", "AppraisedBuildingValue", "ExemptionAmount" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                // dt.Columns.Add("Owner Name");
                dt.Columns.Add("AppraisedLandValue");
                dt.Columns.Add("AppraisedBuildingValue");
                dt.Columns.Add("ExemptionAmount");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["AppraisedLandValue"] = dt1.Rows[i]["AppraisedLandValue"];
                    dt.Rows[i]["AppraisedBuildingValue"] = dt1.Rows[i]["AppraisedBuildingValue"];
                    dt.Rows[i]["ExemptionAmount"] = dt1.Rows[i]["ExemptionAmount"];
                }
                //  dt.Columns["Owner Name"].SetOrdinal(1);
                // dt.Columns["Proprty_Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Jefferson(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");

        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner_Name", "Improvement_Address", "Homestead_Exemption_Status" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land_Assessment", "Improvement_Assessment" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");



                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land_Assessment"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement_Assessment"];


                }
                var cellValue = dt.Rows[0]["Owner_Name"];
            }
            catch { }
            dataview = new DataView(dt);


        }
        return dataview;


    }
    public DataView getassprop_CharlesMD(string strorder, string countyid)
    {
        //Account Identifier~Owner Name~Mailing Address~Use~Principal Residence~Premises Address~Legal Description~Map~Grid~Parcel~Sub District~Subdivision~Section~Block~Lot~Assessment Year~Primary Structure Built~Property Land Area
        //Year~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");

        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Premises Address", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Total Assessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Total Assessed Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["Total Assessed Value"] = dt1.Rows[i]["Total Assessed Value"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
            }
            catch { }
            dataview = new DataView(dt);


        }
        return dataview;


    }
    public DataView getassprop_Douglas(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 2,1");
        DataTable dt2 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 4,1");

        if (dt.Rows.Count > 0)
        {
            //Year~Class_code~Description~actual_value~assessed_value~Tax_rate~EstTax_amount


            string[] selectedColumnsdt = new[] { "parcel_no", "Owner_Name", "Property_Address", "YearBuilt" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "assessed_value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                string[] selectedColumnsdt2 = new[] { "assessed_value" };
                dt2 = new DataView(dt2).ToTable(false, selectedColumnsdt2);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["assessed_value"];
                    dt.Rows[i]["Improvement Value"] = dt2.Rows[i]["assessed_value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Anoka(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            // Situs_Address~Property_Description~Status~Abstract_Torrens~Owner~Lot_Size~Year_Built~City_Name~School_District_Number_Name~Property_classification_2018~Property_classification_2017

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner", "Situs_Address", "Year_Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Est_Market_Land", "Est_Market_Improvement" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Est_Market_Land"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Est_Market_Improvement"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Stark(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //   Owner_Name~Property_Address~Property_Type~Year_Built~Legal_Description
            //Year~Appraised_Land_Value~Assessed_Land_Value~Appraised_Building_Value~Assessed_Building_Value~Appraised_Total_Value~Assessed_Total_Value~Override
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner_Name", "Property_Address", "Year_Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed_Land_Value", "Assessed_Building_Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Assessed_Land_Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Assessed_Building_Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Cherokee(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //PIN Number~AccountNumber~Owner Name~Exemptions~Property Address~Mailing Address~Property Type~Year Built~Legal Description
            // Building Value~Outbuilding Value~Land Value~Total Parcel Value~Deferred Value~Taxable Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Exemptions" };

            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Building Value", "Land Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Building Value"];


                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Hillsborough(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner~Situs Address~Pin~Tax District~Property Use~Neighbourhood~Subdivision~Year Built~Legal Description
            //County Market Value~County Assessed Value~County Exemptions~County Taxable Value~Public Schools Market Value~Public Schools Assessed Value~Public Schools Exemptions~Public Schools Taxable Value~Municipal Market Value~Municipal Assessed Value~Municipal Exemptions~Municipal Taxable Value~Other Districts Market Value~Other Districts Assessed Value~Other Districts Exemptions~Other Districts Taxable Value

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner", "Situs Address", "Year Built" };

            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "County Market Value", "County Assessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("County Market Value");
                dt.Columns.Add("County Assessed Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["County Market Value"] = dt1.Rows[i]["County Market Value"];
                    dt.Rows[i]["County Assessed Value"] = dt1.Rows[i]["County Assessed Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Denver(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner Address~Legal Description~Property Type~Tax District~Year Built
            //Year~Actual land~Assessed Land~Exempt Land~Actual Improvements~Assessed Improvements~Exempt Improvements~Actual Total~Assessed Total~Exempt Total
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land", "Assessed Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Assessed Land");
                dt.Columns.Add("Assessed Improvements");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Assessed Land"] = dt1.Rows[i]["Assessed Land"];
                    dt.Rows[i]["Assessed Improvements"] = dt1.Rows[i]["Assessed Improvements"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Harford(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 desc limit 1");
        if (dt.Rows.Count > 0)
        {
            //Account id number~Owner name~Address~Mail Address~Legal Description~Year Built~Use~Principal Residence~Map~Grid~Parcel~Sub District~Sub division~Sections~Block~Lot~Assessment Year~Homestead Application Status~Homeowners Tax Credit Application Status~Homeowners Tax Credit Application Date
            //Year~Total Assessed Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner name", "Address", "Year Built", "Homestead Application Status" };

            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Total Assessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Total Assessed Value");



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Total Assessed Value"] = dt1.Rows[i]["Total Assessed Value"];


                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Yolo(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Assessment Number~Tax Rate Area~Owner Name
            //Land Value~Improvement Value~Fixtures~Growing~Total Value~Personal Property~Business Property~Homeowners Exemption~Other Exemptions~Net Assessment
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value", "Homeowners Exemption", "Other Exemptions" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Homeowners Exemption");
                dt.Columns.Add("Other Exemptions");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Homeowners Exemption"] = dt1.Rows[i]["Homeowners Exemption"];
                    dt.Rows[i]["Other Exemptions"] = dt1.Rows[i]["Other Exemptions"];
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Clayton(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Location~Legal Description~District~County~Owner Name~Total Parcel Values~Comments
            //Land/OVR~Improvements/OVR~Current Year Value~Prior Year Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Location" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land/OVR", "Improvements/OVR" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land/OVR"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvements/OVR"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Berkeley(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner Name~Property Address~Mailing Address~Year Built~Legal Description
            //Land Market Value~Building Market Value~Total Taxable Value~Total Assessed Value~Homestead Exemption
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Market Value", "Building Market Value", "Homestead Exemption" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Homestead Exemption");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Homestead Exemption"] = dt1.Rows[i]["Homestead Exemption"];
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Market Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Building Market Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Newton(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Assessment Year~Previous Value~Land Value~Improvement Value~Accessory Value~Current Value
            //   Parcel ID~Short Parcel ID~Property Address~Owner Name~Mailing Address~Legal Description~Property Class~Tax District~Millage Rate~Homestead Exemption~Year Built~Comment
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Homestead Exemption" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Dakota(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 desc limit 1");
        if (dt.Rows.Count > 0)
        {
            //Tax Year~Estimated Market Value~Homestead Exclusion~Taxable Market Value~New Imp/Expired Excl~Property Class
            //  Owner Name~Joint Owner Name~Property Address~Mailing Address~Municipality~Property Use~Year Built~Legal Description
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Estimated Market Value", "Taxable Market Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Estimated Market Value");
                dt.Columns.Add("Taxable Market Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Estimated Market Value"] = dt1.Rows[i]["Estimated Market Value"];
                    dt.Rows[i]["Taxable Market Value"] = dt1.Rows[i]["Taxable Market Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Washington(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Previous Parcel~Owner Name~Property Address~Mailing Address~Legal Description~Property Type~Tax District~Millage Rate~Year Built
            //Type~Land~Building~Total
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Building");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Maricopa(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 desc limit 1");
        if (dt.Rows.Count > 0)
        {
            //Property Address~Owner Name~MCR~Description~HighSchool District~Elementary School District~Local Jurisdiction~STR~Subdivision~Construction Year
            //Tax Year~Full Cash Value~Limited Property Value~Legal Class~Description~Assessment Ratio~Assessed FCV~Assessed LPV~Property Use Code~PU Description~Tax Area Code~Valuation Source
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Construction Year" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed FCV", "Assessed LPV" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Assessed FCV");
                dt.Columns.Add("Assessed LPV");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Assessed FCV"] = dt1.Rows[i]["Assessed FCV"];
                    dt.Rows[i]["Assessed LPV"] = dt1.Rows[i]["Assessed LPV"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Contracosta(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Situs Address
            //Assessment Year~Total Net Taxable Value~Other Exemptions~Homeowner Exemption~Gross Value~Personal Property~Improvements~Land
            string[] selectedColumnsdt = new[] { "parcel_no", "Situs Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvements", "Land", "Other Exemptions", "Homeowner Exemption" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");

                dt.Columns.Add("Homeowner Exemption");
                dt.Columns.Add("Other Exemptions");
                dt.Columns.Add("Improvements");
                dt.Columns.Add("Land");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Homeowner Exemption"] = dt1.Rows[i]["Homeowner Exemption"];
                    dt.Rows[i]["Other Exemptions"] = dt1.Rows[i]["Other Exemptions"];
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];


                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Situs Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Sacromento(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //   Property Address~City Zip~Jurisdiction~Tax Rate Area Code~County Supervisor District~Approx Parcel Area
            //Tax Roll Year~Land Value~Improvement Value~Personal Property Value~Fixtures~Homeowner Exemption~Other Exemption~Net Assessed Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value", "Homeowner Exemption", "Other Exemption" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Homeowner Exemption");
                dt.Columns.Add("Other Exemption");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";

                    dt.Rows[i]["Homeowner Exemption"] = dt1.Rows[i]["Homeowner Exemption"];
                    dt.Rows[i]["Other Exemption"] = dt1.Rows[i]["Other Exemption"];
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_santabarabara(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //address1~address2~TRA~Transfer_TaxAmount~Use_Description~Jurisdiction~Acreage~Year_Built
            //Land_MineralRights~Improvements~Personal_Property~Home_OwnerExemption~Other_Exemption~Net_AssessedValue
            string[] selectedColumnsdt = new[] { "parcel_no", "address1", "address2", "Year_Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land_MineralRights", "Improvements", "Home_OwnerExemption", "Other_Exemption" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Home_OwnerExemption");
                dt.Columns.Add("Other_Exemption");
                dt.Columns.Add("Land_MineralRights");
                dt.Columns.Add("Improvements");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Home_OwnerExemption"] = dt1.Rows[i]["Home_OwnerExemption"];
                    dt.Rows[i]["Other_Exemption"] = dt1.Rows[i]["Other_Exemption"];
                    dt.Rows[i]["Land_MineralRights"] = dt1.Rows[i]["Land_MineralRights"];
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["address1"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_duval(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Property Address~Owner Name~Tax District~Property Use~Legal Description~Subdivision~Year Built
            //Year~Value Method~Total Building Value~Extra Feature Value~Land Value Market~Land Value Agric~Just Market Value~Assessed Value~Cap Diff Portability Amt~Exemptions~Taxable Value

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value Market", "Total Building Value", "Exemptions" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value Market");
                dt.Columns.Add("Total Building Value");
                dt.Columns.Add("Exemptions");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value Market"] = dt1.Rows[i]["Land Value Market"];
                    dt.Rows[i]["Total Building Value"] = dt1.Rows[i]["Total Building Value"];
                    dt.Rows[i]["Exemptions"] = dt1.Rows[i]["Exemptions"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_broward(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Property Address~Owner Name~Mailing Address~Legal Discription
            //Year~Land~Building Improvement~Just Market~Assessed OH~Tax            

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building Improvement" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Building Improvement");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building Improvement"] = dt1.Rows[i]["Building Improvement"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_orange(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //OwnerName~Physical Street Address~Property Use~Municipality~Property Description~Actual Year Built
            //Tax Year Values~Land~Building~Feature~Market Value~Portability~Assessed Value
            string[] selectedColumnsdt = new[] { "parcel_no", "OwnerName", "Physical Street Address", "Actual Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Building");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];

                }
            }
            catch
            {

            }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_osceola(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            // Owner Name~Property Address~Mailing Address~Property Type~Tax District~Year Built~Legal Description
            //Year~Land~AG Benefit~Extra Features~Buildings~Appraised Value~Assessed Value~Exemption Value~Taxable Value

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Buildings", "Exemption Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Buildings");
                dt.Columns.Add("Exemption Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Buildings"] = dt1.Rows[i]["Buildings"];
                    dt.Rows[i]["Exemption Value"] = dt1.Rows[i]["Exemption Value"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_palmbeach(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            // Location Address~Municipaliti~Sub Division~Legal Description~Acres~Year Built~Owner Name~Mailing Address
            //Tax Year~Improvement Value~Land Value~Total Market Value

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Location Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_FLpolk(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Land Value~Building Value~Misc Items Value~Land Classified Value~Just Market Value~Cap Differential and Portability~Agriculture Classification~Assessed Value~Exempt Value County~Taxable Value County
            //Owner Name~Site Address~City~State~Zip Code~Neighborhood~Subdivision~Property Use Code~Acreage~Taxing District~Community Redevelopment Area~Actual Year Built
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Site Address", "Actual Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Building Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_sarasota(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner Name&Mailing Address~Property Address~Property Type~Year Built~Legal Description
            //Year~Land~Building~Extra Feature~Just~Assessed~Exemptions~Taxable~Cap
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name&Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building", "Exemptions" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Building");
                dt.Columns.Add("Exemptions");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];
                    dt.Rows[i]["Exemptions"] = dt1.Rows[i]["Exemptions"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_stlucie(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //   Account Number~Owner Name & Mailing Address~Property Address~Property Type~Year Built~Legal Description
            //Building Value~Land Value~Just Value~Agricultural Credit~10% Cap~Assessed Value~Exemption~Taxable Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name & Mailing Address", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value", "Exemption" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Building Value");
                dt.Columns.Add("Exemption");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                    dt.Rows[i]["Exemption"] = dt1.Rows[i]["Exemption"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_ILwill(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 3 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt2 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");

        if (dt.Rows.Count > 0)
        {
            //Property Type~Year Built~Legal Description
            //Assessment Year~Land~Farm Land~Building~Form Building~Total~Form Total
            //OwnerName~Mailing Address~Assessed Value~Exemptions~Tax Code~Tax Rate~Taxing Authority


            string[] selectedColumnsdt = new[] { "Year Built" };
            dt2 = new DataView(dt2).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                string[] selectedColumnsdt2 = new[] { "parcel_no", "OwnerName", "Mailing Address", "Exemptions" };
                dt = new DataView(dt).ToTable(false, selectedColumnsdt2);

                dt.Columns.Add("Year Built");

                dt.Columns.Add("Land");
                dt.Columns.Add("Building");

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["Year Built"] = dt2.Rows[i]["Year Built"];

                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_MDCarroll(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 desc limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner Name~Property Address~Legal Description~Year Built~Use~Principal Residence~Map~Grid~Parcel~Sub District~SubDivision~Section~Block~Lot~Assessment Year~Town~Homestead Application Status~Home Owners Tax Credit Application Status~Home owners Tax Credit Application Date
            //Year~Total Assessed Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Total Assessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Total Assessed Value");



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Total Assessed Value"] = dt1.Rows[i]["Total Assessed Value"];


                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_OHHamilton(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner_Name~Property_Address~Mailing_Address~Property_Type~Year_Built~Legal_Description
            //TaxDistrict~SchoolDistrict~Tax_Year~Market_Land_Value~Market_Building_Value~Total_Market_Value~Assessed_Land_Value~Assessed_Building_Value~Total_Assessed_Value~Board_of_Revision~Rental_Registration~Homestead~Owner_Occupancy_Credit~Foreclosure~Special_Assessments~CAUV_Value~TIF_Value~Abated_Value~Exempt_Value

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner_Name", "Property_Address", "Year_Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Market_Land_Value", "Market_Building_Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Market_Land_Value");
                dt.Columns.Add("Market_Building_Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Market_Land_Value"] = dt1.Rows[i]["Market_Land_Value"];
                    dt.Rows[i]["Market_Building_Value"] = dt1.Rows[i]["Market_Building_Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_okClevland(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Account~Legal Description~Account Type~Sub Division~Tax District~Legal Acreage~Owner Name
            //Land Type~Acreage~Value~Description~Total Acres~Total Value~Market Value~Taxable Value~Land Value~GrossAssessed Value~Adjustments~Year Built
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Year Built", "Land Value", "GrossAssessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Year Built");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("GrossAssessed Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Year Built"] = dt1.Rows[i]["Year Built"];
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["GrossAssessed Value"] = dt1.Rows[i]["GrossAssessed Value"];

                }
            }
            catch { }

            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Sanlouispo(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Property Address~Owner Name~City~Year Built
            //Assessed Year~Land Value~Improvement Value~Total Assessed Value~Personal Property~Fixtures Value~Total Exemption~Net Taxable Value

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value", "Total Exemption" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                dt.Columns.Add("Total Exemption");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                    dt.Rows[i]["Total Exemption"] = dt1.Rows[i]["Total Exemption"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Larmine(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Tax ID~Tax District~PropertyOwner~StreetAddress~Location~Year~MarketValue~AssessedValue~Acres~Class~YearBuilt
            string[] selectedColumnsdt = new[] { "parcel_no", "PropertyOwner", "StreetAddress", "YearBuilt", "MarketValue", "AssessedValue" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Eldorado(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner Name~Abstractcode~Reference~SubdivisionTractNumber~SubdivisionTractName~TaxRateArea~City~YearBuilt
            //Net Roll~Total Roll~Improvement Total~Improvement Structures~Land Total~Land
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "YearBuilt" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Total", "Improvement Total" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Total");
                dt.Columns.Add("Improvement Total");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Total"] = dt1.Rows[i]["Land Total"];
                    dt.Rows[i]["Improvement Total"] = dt1.Rows[i]["Improvement Total"];

                }
            }

            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_CAMonterery(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id where DFM.Category_Id = 16 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Address~Owner_Name~County~City~State


            string[] selectedColumnsdt = new[] { "parcel_no", "Owner_Name", "Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);

            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Miamedade(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Subdivision~Property Address~Primary Land Use~Year Built~Owner Name
            //Year~Land Value~Building Value~Extra Feature Value~Market Value~Assessed Value~County Exemption Value~County TaxableValue~SCHOOL Exemption Value~SCHOOL Taxable Value~City Exemption Value~City Taxable Value~REGIONAL Exemption Value~REGIONAL Taxable Value

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Building Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_madison(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner Name~PPIN~Tax Dist~Account~Taxable Value~Assessment Value~Description~Property Address~Neighberhood~Property Class~Sub Division~Sub Desc~Section
            //Land~Building~Total Price Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Building");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_alameda(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id,DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Property Address~Use Code~Description
            // Improvement~Totatal Taxable Value~HomeOwner~Other~Total Net Taxable value~Land
            //Improvement~Totatal Taxable Value~Home Owner~Other~Tota lNet Taxable value~Land
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {

                string[] selectedColumnsdt1 = new[] { "Land", "Improvement" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Improvement");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Improvement"] = dt1.Rows[i]["Improvement"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch (Exception e)
            { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_collier(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Taxable Value~School Taxable Value~Assessed Value~Market Value~Improved Value~Land Value
            //Owner Name~Site Address~Map No~Strap no~Section~Township~Range~Acres~Millage Area~Condo~Use Code~School~Other~Total~Year Built
            //Taxable Value~ School Taxable Value~Assessed Value~10% Cap~ Market Value~Improved Value~Land Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Site Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improved Value", "Land Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Improved Value");
                dt.Columns.Add("Land Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Improved Value"] = dt1.Rows[i]["Improved Value"];
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_manatee(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //T/R/S~Property Address~Excemption~DOR Desciption~Map ID~Legal Description~Tax District~Owner Name~Mailing Address~Year Built
            //Tax Year~Just Land Value~Just Improvement Value~Total Just Value~New Construction~Addition Value~Demolition Value~Save Our Homes Savings~Non Homestead Cap Savings~Market Value of Classified Use Land~Classified Use Value~Total Assessed Value~Previous Year Just Value~Previous Year Assessed Value~Previous Year Cap Value~Non-Ad Valorem Assessments~Output

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Just Land Value", "Just Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Just Land Value");
                dt.Columns.Add("Just Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Just Land Value"] = dt1.Rows[i]["Just Land Value"];
                    dt.Rows[i]["Just Improvement Value"] = dt1.Rows[i]["Just Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_pasco(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //ownername~PropertyAddress~MailingAddress~PropertyType~YearBuilt~LegalDescription
            //AssessedYear~AgriculturalLand~LandValue~BuildingValue~ExtraFeaturesValue~JustValue~AssessedValue~Homestead~NonSchoolAdditionalHomestead~NonSchoolTaxableValue~SchoolDistrictTaxableValue~Taxable
            string[] selectedColumnsdt = new[] { "parcel_no", "ownername", "PropertyAddress", "YearBuilt" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "LandValue", "BuildingValue" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("LandValue");
                dt.Columns.Add("BuildingValue");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["LandValue"] = dt1.Rows[i]["LandValue"];
                    dt.Rows[i]["BuildingValue"] = dt1.Rows[i]["BuildingValue"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_volusia(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //FullParcelID~Alternatekey~ownername~PropertyAddress~MailingAddress~PropertyType~YearBuilt~LegalDescription
            //AssessedYear~LandValue~ImprValue~JustValue~nonSchoolAssessed~NonSchoolAssessed_Exemptions~NonSch_taxable~HR_Savings
            string[] selectedColumnsdt = new[] { "parcel_no", "ownername", "PropertyAddress", "YearBuilt" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "LandValue", "ImprValue" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("LandValue");
                dt.Columns.Add("ImprValue");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["LandValue"] = dt1.Rows[i]["LandValue"];
                    dt.Rows[i]["ImprValue"] = dt1.Rows[i]["ImprValue"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_hamilton(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //State ParcelID~Owner Name~Property Address~Mailing Address~State Tax District~Taxing Unit~Legal Description
            //Assessed Land Value~Assessed Improvement Value~Total Assessed Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land Value", "Assessed Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Assessed Land Value");
                dt.Columns.Add("Assessed Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Assessed Land Value"] = dt1.Rows[i]["Assessed Land Value"];
                    dt.Rows[i]["Assessed Improvement Value"] = dt1.Rows[i]["Assessed Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_clark(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner and Mailing Address~Location Address City Unincorporated Town~Assessor Description~Tax District~Appraisal Year~Fiscal Year~Supplemental Improvement Value~Incremental Land~Incremental Improvements~Estimated Size~Original Const Year~Land Use
            //Fiscal Year~Land~Improvements~Personal Property~Exempt~Gross Assessed (Subtotal)~Taxable Land+Imp (Subtotal)~Common Element AllocationAssd~Total Assessed Value~Total Taxable Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner and Mailing Address", "Location Address City Unincorporated Town", "Original Const Year" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Improvements");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_deschutes(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Mailing_Name~Map_Tax~AccountNo~Situs_Address~Tax_Status~Assessor_Property_Description~Assessor_Acres~Property_Class~Land~Structures~Total~Maximum_Assessed~Assessed_Value~Veterans_Exemption~Year_Built
            //Value_History~RealMarketValue_Land~RealMarketValue_Structures~TotalRealMarket_Value~MaximumAssessed_Value~TotalAssessed_Value~VeteransExemption_Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Mailing_Name", "Situs_Address", "Year_Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "RealMarketValue_Land", "RealMarketValue_Structures" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("RealMarketValue_Land");
                dt.Columns.Add("RealMarketValue_Structures");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["RealMarketValue_Land"] = dt1.Rows[i]["RealMarketValue_Land"];
                    dt.Rows[i]["RealMarketValue_Structures"] = dt1.Rows[i]["RealMarketValue_Structures"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_sanbernardino(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner and Mailing Address~Effective Date~Legal Description
            //Land Value~Improvement Value~Improvement Penalty~Pers Prop Value~Pers Prop Penalty~Total Penalties~Total Assessed Value~Homeowner Exemption~Special Exemptions~Net Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner and Mailing Address", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_CAOrange(string strorder, string countyid)
    {
        //Property Address~Tax Rate Area
        //Land Value~Mineral Rights~Improvement Value~Personal Property~Others~Total Assessed Value~Homeowner Exemption~Net Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_Santaclara(string strorder, string countyid)
    {
        //Property Address~Tax Rate Area
        //Land Value~Improvement Value~Total Assessed Value~Homeowner Exemption~Other Exemption~Total~Net Assessed Value~Tax Default Date
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address" };

            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_WAKing(string strorder, string countyid)
    {
        //Owner Name~Property Address~Legal Description~Year Built
        //Valued Year~Tax Year~Appraised Land Value~Appraised Improvement Value~Appraised Total~Taxable Land Value~Taxable Imps Value~Taxable Total
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Appraised Land Value", "Appraised Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Appraised Land Value");
                dt.Columns.Add("Appraised Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Appraised Land Value"] = dt1.Rows[i]["Appraised Land Value"];
                    dt.Rows[i]["Appraised Improvement Value"] = dt1.Rows[i]["Appraised Improvement Value"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_NCForsyth(string strorder, string countyid)
    {
        //Tax Year~REID~Property Owner~Property Address~Legal Description~Mailing Address~Old Map Number~Market Area~Township~Planning Jurisdiction~City~Fire District~Special District~Property Class~YearBuilt
        //Total Appraised Land Value~Total Appraised Building Value~Total Appraised Misc Improvements Value~Total Cost Value~Total Appraised Value-Valued By Cost~Other Exemptions~Exemption Desc~Use Value Deferred~Historic Value Deferred~Total Deferred Value~Total Taxable Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Owner", "Property Address", "YearBuilt" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Total Appraised Land Value", "Total Appraised Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Total Appraised Land Value");
                dt.Columns.Add("Total Appraised Building Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Total Appraised Land Value"] = dt1.Rows[i]["Total Appraised Land Value"];
                    dt.Rows[i]["Total Appraised Building Value"] = dt1.Rows[i]["Total Appraised Building Value"];

                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_KSJohnson(string strorder, string countyid)
    {
        //Property Address~KUP~Quick Ref~Description~Year Built
        //Type~Year~Appraised Value~Assessed Value~Appraised Change
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Appraised Value", "Assessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Appraised Value");
                dt.Columns.Add("Assessed Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Appraised Value"] = dt1.Rows[i]["Appraised Value"];
                    dt.Rows[i]["Assessed Value"] = dt1.Rows[i]["Assessed Value"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_NCGuilford(string strorder, string countyid)
    {
        //PIN #~Location Address~Property Description~Property Owner~City~Land Class~Acreage~Year Built
        //Total Appraised Land Value~Total Appraised Building Value~Total Appraised Outbuilding Value~Total Appraised Value~Other Exemptions~Use Value Deferred~Historic Value Deferred~Total Deferred Value~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Location Address", "Property Owner", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Total Appraised Land Value", "Total Appraised Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Total Appraised Land Value");
                dt.Columns.Add("Total Appraised Building Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Total Appraised Land Value"] = dt1.Rows[i]["Total Appraised Land Value"];
                    dt.Rows[i]["Total Appraised Building Value"] = dt1.Rows[i]["Total Appraised Building Value"];

                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_SCSpartanburg(string strorder, string countyid)
    {
        //Account #~Land Size~Location Address~Legal Description~Neighborhood~Property Usage~Owners
        //Year~Market Land Value~Market Improvement Value~Market Misc Value~Total Market Value~Taxable Land Value~Taxable Improvement Value~Taxable Misc Value~Ag Credit Value~Total Taxable Value~Assessed Land Value~Assessed Improvement Value~Assessed Misc Value~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Location Address", "Owners" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land Value", "Assessed Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed Land Value");
                dt.Columns.Add("Assessed Improvement Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed Land Value"] = dt1.Rows[i]["Assessed Land Value"];
                    dt.Rows[i]["Assessed Improvement Value"] = dt1.Rows[i]["Assessed Improvement Value"];

                }
            }
            catch { }
            //dt.Columns["Owner Name"].SetOrdinal(1);
            //dt.Columns["Property Address"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_WVBerkeley(string strorder, string countyid)
    {
        //Account Number~District~Map~Owner Name~Property Address~Legal Description
        //Assessment~Gross~Net
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Owner Name" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessment", "Gross", "Net" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessment");
                dt.Columns.Add("Gross");
                dt.Columns.Add("Net");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessment"] = dt1.Rows[i]["Assessment"];
                    dt.Rows[i]["Gross"] = dt1.Rows[i]["Gross"];
                    dt.Rows[i]["Net"] = dt1.Rows[i]["Net"];

                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_NESarpy(string strorder, string countyid)
    {
        //Property Address~Owner Name~COwner~Mail Address~Legal Description~Tax District~Map~Property Class~Year Built
        //Roll Year~Land Value~Impr Value~Outbuildings~Total Value~PV~Form191
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Owner Name", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Impr Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Impr Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Impr Value"] = dt1.Rows[i]["Impr Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_ARSaline(string strorder, string countyid)
    {
        //County Name~Owner Details~Mailing Address~Property Address~Total Acres~Sec-Twp-Rng~Subdivision~Legal Description~School District~Homestead Parcel~Tax Status~Over~Year Built
        //Land Appraised~Land Assessed~Improvements Appraised~Improvements Assessed~Total Value Appraised~Total Value Assessed~Taxable Value Appraised~Taxable Value Assessed~Millage Appraised~Millage Assessed~Estimated Taxes  Appraised~Estimated Taxes  Assessed~Assessment Year Appraised~Assessment Year Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Owner Details", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Assessed", "Improvements Assessed" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Assessed");
                dt.Columns.Add("Improvements Assessed");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Assessed"] = dt1.Rows[i]["Land Assessed"];
                    dt.Rows[i]["Improvements Assessed"] = dt1.Rows[i]["Improvements Assessed"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_SCDorchester(string strorder, string countyid)
    {
        //Owner~Owner Address~Account#~TMS#~Parcel Address~Total Land & Improvements~Tax District~Subdivision~Property Type~Year Built
        //TAXYEAR~ABST DESC~ACRES~ACTUAL VALUE~ASSM RATIO~CAP VALUE~TAXABLE VALUE~TAXABLE ASSESSED VALUE
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner", "Parcel Address", "Total Land & Improvements", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "TAXABLE VALUE", "TAXABLE ASSESSED VALUE" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("TAXABLE VALUE");
                dt.Columns.Add("TAXABLE ASSESSED VALUE");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["TAXABLE VALUE"] = dt1.Rows[i]["TAXABLE VALUE"];
                    dt.Rows[i]["TAXABLE ASSESSED VALUE"] = dt1.Rows[i]["TAXABLE ASSESSED VALUE"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_NYKings(string strorder, string countyid)
    {
        //Owner Name~Mailing Address~Planned Payment Date~Due~Account Type~Account ID~Transaction Type~Period Begin~Paid Amount~Total Amount Paid
        //Description~Period Begin~Charges~Interest~Discount~Balance
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Mailing Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Charges", "Interest", "Discount", "Balance" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Charges");
                dt.Columns.Add("Interest");
                dt.Columns.Add("Discount");
                dt.Columns.Add("Balance");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Charges"] = dt1.Rows[i]["Charges"];
                    dt.Rows[i]["Interest"] = dt1.Rows[i]["Interest"];
                    dt.Rows[i]["Discount"] = dt1.Rows[i]["Discount"];
                    dt.Rows[i]["Balance"] = dt1.Rows[i]["Balance"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_OHMedina(string strorder, string countyid)
    {
        //Owner Name~Property Address~Property Class~Acreage~Legal Description~Year Built
        //Land Value~CAUV Value~Building Value~Total Value~Taxable Land Value~Taxable CAUV Value~Taxable Building Value~Taxable Total Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Building Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Property Address"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_CAShasta(string strorder, string countyid)
    {
        //Assessment Number~Tax Rate Area~Property Type~Acres~Asmt Description~Asmt Status~Year Built
        //Land Value~Improvement Value~Fixtures~Growing~Total Value~Personal Property~Business Property~Homeowners Exemption~Other Exemptions~Net Assessment
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }

            dataview = new DataView(dt);
        }

        return dataview;
    }
    public DataView getassprop_COLarimer(string strorder, string countyid)
    {
        //Schedule Number~Owner Name~Property Address~Mailing Address~Legal Description~Year Built~Property Type~Occupancy
        //Year~Land Actual Value~Building Actual Value~Total Actual Value~Land Assessed Value~Building Assessed Value~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Owner Name", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Assessed Value", "Building Assessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Assessed Value");
                dt.Columns.Add("Building Assessed Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Assessed Value"] = dt1.Rows[i]["Land Assessed Value"];
                    dt.Rows[i]["Building Assessed Value"] = dt1.Rows[i]["Building Assessed Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_MOJefferson(string strorder, string countyid)
    {
        //Tax Year~Class~Tax Code~Land Use~Site Address~Mapped Acres~Assessed Value~Tax Rate~Legal Description~Section Township Range~Property Owner~Mailing Address~Appraised Value~Year Built
        //Assessment Period~Appraised Land~Assessed Land~Appraised Building~Assessed Building~Appraised Total~Assessed Total
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Site Address", "Property Owner", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land", "Assessed Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed Land");
                dt.Columns.Add("Assessed Building");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed Land"] = dt1.Rows[i]["Assessed Land"];
                    dt.Rows[i]["Assessed Building"] = dt1.Rows[i]["Assessed Building"];
                }
            }
            catch { }
            //dt.Columns["Owner Name"].SetOrdinal(1);
            //dt.Columns["Year Built"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_DonaAnaNM(string strorder, string countyid)
    {
        //Account~Situs Address~Tax Area~Legal Summary~Owner Name~Owner Address~Property Code~Acres~Actual Year Built~Effective Year Built
        //Year~Residential Land~Residential Land Assessed~Residential Improvement~Residential Improvement Assessed~Total Actual Value~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Situs Address", "Owner Name", "Actual Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Residential Land Assessed", "Residential Improvement Assessed" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Residential Land Assessed");
                dt.Columns.Add("Residential Improvement Assessed");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Residential Land Assessed"] = dt1.Rows[i]["Residential Land Assessed"];
                    dt.Rows[i]["Residential Improvement Assessed"] = dt1.Rows[i]["Residential Improvement Assessed"];
                }
            }
            catch
            {

            }
            //dt.Columns["Owner Name"].SetOrdinal(1);
            //dt.Columns["Year Built"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_CharlotteFL(string strorder, string countyid)
    {
        //Old Parcel ID Number~Owner Name & Address~Property Address~Business Name~Year Built~Map Number~Current Use~Future Land Use (Comp.Plan)~Property Zip Code~Section-Township-Range~Taxing District~Market Area / Neighborhood / Subneighborhood~SOH Base Year~Short Legal~Legal Description
        //2017 Value Summary~Land~Land Improvements~Building~Damage~Total
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name & Address", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Building");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];
                }
            }
            catch { }
            //dt.Columns["Owner Name"].SetOrdinal(1);
            //dt.Columns["Year Built"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_Calcasieu(string strorder, string countyid)
    {
        //Primary Owner~Mailing Address~Ward~Type~Legal~Property Address~Homestead~Subdivision
        //Property Class~Assessed Value~Units~Exempt
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Primary Owner", "Property Address", "Homestead" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Value", "Exempt" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed Value");
                dt.Columns.Add("Exempt");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed Value"] = dt1.Rows[i]["Assessed Value"];
                    dt.Rows[i]["Exempt"] = dt1.Rows[i]["Exempt"];
                }
            }
            catch { }
            //dt.Columns["Owner Name"].SetOrdinal(1);
            //dt.Columns["Year Built"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_fayetteFL(string strorder, string countyid)
    {
        //Parcel ID~Owner Name~Mailing Address~Property Address~Legal Description~Property Class~Land Use Code~Map Block~Lot~SubDivision~Tax District~Tax Rate~Year Built
        //Year~Fair Cash Value~Agricultural Value Land~Agricultural Value Total~Exempt~Taxable Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Agricultural Value Land", "Exempt" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Agricultural Value Land");
                dt.Columns.Add("Exempt");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Agricultural Value Land"] = dt1.Rows[i]["Agricultural Value Land"];
                    dt.Rows[i]["Exempt"] = dt1.Rows[i]["Exempt"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_MarionFL(string strorder, string countyid)
    {
        //Prime Key~Owner Name  Mailing Address~Situs~Taxes  Assessments~Map ID~Millage~Acres~Property Description~Improvement~Year Built
        //Land Just Value~Buildings~Miscellaneous~Total Just Value~Total Assessed Value~Exemptions~Total Taxable
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name  Mailing Address", "Situs", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Just Value", "Buildings" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Just Value");
                dt.Columns.Add("Buildings");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Just Value"] = dt1.Rows[i]["Land Just Value"];
                    dt.Rows[i]["Buildings"] = dt1.Rows[i]["Buildings"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);

            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_HallGA(string strorder, string countyid)
    {
        // Location Address~Legal Description~Class~Zoning~Tax District~Millage Rate~Acres~Neighborhood~Homestead Exemption~Owners Name & Mailing Address~Year Built
        //Year~Previous Value~Land Value~Improvement Value~Accessory Value~Current Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Location Address", "Owners Name & Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_CowetaGA(string strorder, string countyid)
    {
        //Location Address~Legal Description~Class~Tax District~Millage Rate~Acres~Neighborhood~Homestead Exemption~Owner~Year Built
        //Year~Previous Value~Land Value~Improvement Value~Accessory Value~Current Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Location Address", "Owner", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_WilliamstonTX(string strorder, string countyid)
    {
        //Owner Name~Property Address~Property Status~Property Type~Legal Description~Neighborhood~Account~Map Number~Owner ID~Exemptions~Percent Ownership~Mailing Address
        //Year~Improvement Homesite Value~Improvement Non-Homesite Value~Total Improvement Market Value~Land Homesite Value~Land Non-Homesite Value~Land Agricultural Market Value~Total Land Market Value~Total Market Value~Agricultural Use~Total Appraised Value~Homestead Cap Loss~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvement Homesite Value", "Land Homesite Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvement Homesite Value");
                dt.Columns.Add("Land Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvement Homesite Value"] = dt1.Rows[i]["Improvement Homesite Value"];
                    dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_KnoxTN(string strorder, string countyid)
    {
        //Alternate ID~Property Address~Owner Name & Mailing Address~Property Class~Neighborhood~Acres~Year Built~Legal Description
        //Year~Reason~Land Value~Improvement Value~Total Appraised~Land Market~Land Use~Improvement Market~Total Market
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Owner Name & Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_StJohnsFL(string strorder, string countyid)
    {
        //Owner Name~Property Address~Mailing Address~Property Type~District~Millage Rate~Acreage~Home stead~Year Built~Legal Description
        //Year~Building Value~Extra Features Value~Total Land Value~Agricultural (Assessed) Value~Agricultural (Market) Value~Just (Market) Value~Total Deferred~Assessed Value~Total Exemptions~Taxable Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Agricultural (Assessed) Value", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Agricultural (Assessed) Value");
                dt.Columns.Add("Building Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Agricultural (Assessed) Value"] = dt1.Rows[i]["Agricultural (Assessed) Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_KootenaiID(string strorder, string countyid)
    {
        //AIN~Property Address~Owner Name~Mailing Address~Tax Authority Group~Acreage~Legal Description~Property Class~Neighborhood~Year Built
        //Year~Homeowners Eligible Amt Land~Homeowners Eligible Amt Imp~Sum Homeowners Eligible Amt~Homeowners Exemption Allowed~Total Market Value~Ag/Timber Exemption~Other Exemptions~Net Taxable Value
        //Year~Homeowners Eligible Amt Land~Homeowners Eligible Amt Imp~Sum Homeowners Eligible Amt~Homeowners Exemption Allowed~Total Market Value~Ag / Timber Exemption~Other Exemptions~Net Taxable Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Owner Name", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Homeowners Eligible Amt Land", "Homeowners Eligible Amt Imp" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Homeowners Eligible Amt Land");
                dt.Columns.Add("Homeowners Eligible Amt Imp");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Homeowners Eligible Amt Land"] = dt1.Rows[i]["Homeowners Eligible Amt Land"];
                    dt.Rows[i]["Homeowners Eligible Amt Imp"] = dt1.Rows[i]["Homeowners Eligible Amt Imp"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch
            {

            }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_StaffordVA(string strorder, string countyid)
    {
        //Alternate ID/PIN~Address~Property Class~Neighborhood~Owner Name & Mailing Address~Year Built~Land Type~Acres~Description
        //Year~Reason~Appraised Land~Appraised Improvements~Appraised Total~Assessed Land~Assessed Land Use~Assessed Improvements~Assessed Total
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Address", "Owner Name & Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land", "Assessed Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed Land");
                dt.Columns.Add("Assessed Improvements");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed Land"] = dt1.Rows[i]["Assessed Land"];
                    dt.Rows[i]["Assessed Improvements"] = dt1.Rows[i]["Assessed Improvements"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_LubbockTX(string strorder, string countyid)
    {
        //Owner Name~Property Address~Property Status~Property Type~Legal Description~Neighborhood~Account~Map Number~Owner ID~Exemptions~Percent Ownership~Mailing Address~Year Built
        //Year~Improvement Homesite Value~Improvement Non-Homesite Value~Total Improvement Market Value~Land Homesite Value~Land Non-Homesite Value~Land Agricultural Market Value~Total Land Market Value~Total Market Value~Agricultural Use~Total Appraised Value~Homestead Cap Loss~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvement Homesite Value", "Land Homesite Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvement Homesite Value");
                dt.Columns.Add("Land Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvement Homesite Value"] = dt1.Rows[i]["Improvement Homesite Value"];
                    dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_HidalgoTX(string strorder, string countyid)
    {
        //Property ID~Geographic ID~Owner Information~Type~Legal Description~Address~Neighborhood~Neighborhood CD~Map ID~Exemptions~Year Built~Acres
        //Year~Improvements~Land Market~Ag Valuation~Appraised~HS Cap~Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Information", "Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvements", "Land Market" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvements");
                dt.Columns.Add("Land Market");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                    dt.Rows[i]["Land Market"] = dt1.Rows[i]["Land Market"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_HamiltonTN(string strorder, string countyid)
    {
        //Account Number~Property Address~Property Type~Land Use~District~Owner Name~Mailing Address~Year Built~Legal Description
        //Building Value~Xtra Features Value~Land Value~Total Value~Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Building Value", "Land Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Building Value");
                dt.Columns.Add("Land Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_NuecesTX(string strorder, string countyid)
    {
        //Property ID~Geographic ID~Owner Name~Mailing Address~Owner ID~% Ownership~Type~Legal Description~Address~Neighborhood~Neighborhood CD~Map ID~Exemptions~Year Built~Acres
        //Year~Improvements~Land Market~Ag Valuation~Appraised~HS Cap~Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvements", "Land Market" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvements");
                dt.Columns.Add("Land Market");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                    dt.Rows[i]["Land Market"] = dt1.Rows[i]["Land Market"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_KanawhaWV(string strorder, string countyid)
    {
        //ParcelID~TaxYear~Neighborhood~District~Map~Parcel~Sub Parcel~SpecialId~TaxClass~LandUse~Property Class~Owners Name~Owners MailAddress~Location~Legal Description1~Year Built~City~Total Acres
        //Assessed Land~Assessed Building~Mineral Value~Homestead Value~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owners Name", "Owners MailAddress", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land", "Assessed Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed Land");
                dt.Columns.Add("Assessed Building");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed Land"] = dt1.Rows[i]["Assessed Land"];
                    dt.Rows[i]["Assessed Building"] = dt1.Rows[i]["Assessed Building"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_BoulderCO(string strorder, string countyid)
    {
        //Property Address~City~Owner~Account Number~Mailing Address~Zip~Sec-Town-Range~Subdivision~Jurisdiction~Legal Description~Acres~Tax Area~Site Address~Neighborhood~MillLevy~Class~Built
        //Title~Actual~Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner", "Property Address", "Mailing Address", "Legal Description", "Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            //string[] selectedColumnsdt1 = new[] { "Assessed Land", "Assessed Building" };
            //dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
            ////dt.Columns.Add("Owner Name");
            //dt.Columns.Add("Assessed Land");
            //dt.Columns.Add("Assessed Building");

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    //dt.Rows[i]["Owner Name"] = "";
            //    dt.Rows[i]["Assessed Land"] = dt1.Rows[i]["Assessed Land"];
            //    dt.Rows[i]["Assessed Building"] = dt1.Rows[i]["Assessed Building"];
            //}
            //dt.Columns["Owner Name"].SetOrdinal(1);
            //dt.Columns["Year Built"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_CowlitzWA(string strorder, string countyid)
    {
        //Account Number~Jurisdiction~Owner Name~Mailing Address~Legal Description~Property Address~Tax District~Neighborhood~Levy Rate
        //Assessment Year~Taxes Payable Year~Type~Actual Value~Assessed Value~Acres
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Mailing Address", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Actual Value", "Assessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Actual Value");
                dt.Columns.Add("Assessed Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Actual Value"] = dt1.Rows[i]["Actual Value"];
                    dt.Rows[i]["Assessed Value"] = dt1.Rows[i]["Assessed Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_FayettaGA(string strorder, string countyid)
    {
        //Location Address~Legal Description~Property Class~Neighborhood~Tax District~Zoning~Acres~Homestead~Exemptions~Owner & Mailing Address~Year Built
        //Assessment~LUC~Class~Land Value~Building Value~Total Value~Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner & Mailing Address", "Location Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Building Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_STlouiscityMO(string strorder, string countyid)
    {
        //Primary Address~Owner name~Owner mailing address~Neighborhood~Ward~Land use~Property description~Property address~Zip code~Year built~Condominium~Number of units~Frontage~Land area~Class code~Zoning~Redevelopment code~Vacant lot
        //Year Type~Assessment Type~Assessed land~Assessed improvements~Assessed total~Appraised total
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner name", "Property address", "Year built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed land", "Assessed improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed land");
                dt.Columns.Add("Assessed improvements");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed land"] = dt1.Rows[i]["Assessed land"];
                    dt.Rows[i]["Assessed improvements"] = dt1.Rows[i]["Assessed improvements"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_BellTX(string strorder, string countyid)
    {
        //Property ID~Geographic ID~Name~Mailing Address~Owner ID~Type~Legal Description~Address~Neighborhood~Neighborhood CD~Map ID~Exemptions~Year Built~Acres~Tax Authority
        //Year~Improvements~Land Market~Ag Valuation~Appraised~HS Cap~Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Name", "Address", "Exemptions", "Year built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvements", "Land Market" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvements");
                dt.Columns.Add("Land Market");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                    dt.Rows[i]["Land Market"] = dt1.Rows[i]["Land Market"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_DelawareOH(string strorder, string countyid)
    {
        //Property Address~Owner Name~Mailing Address~Tax District~School District~Neighborhood~Use Code~Acres~Legal Description~Year Built
        //Market Land Value~CAUV~Market Improvement Value~Total~Board of Revision~Homestead Disability~Owner Occ Credit~Divided Property~New Construction~Foreclosure~Other Assessments~Front Ft~Assessed land Value~Assessed Improvements Value~Assessed Total Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed land Value", "Assessed Improvements Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed land Value");
                dt.Columns.Add("Assessed Improvements Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed land Value"] = dt1.Rows[i]["Assessed land Value"];
                    dt.Rows[i]["Assessed Improvements Value"] = dt1.Rows[i]["Assessed Improvements Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_EllisTX(string strorder, string countyid)
    {
        //Legal Description~Geographic ID~Type~Address~Map ID~Neighborhood CD~Owner ID~Owner Name~Mailing Address~Exemptions~Acres~Year Built
        //Improvement Homesite Value~Improvement Non-Homesite Value~Land Homesite Value~Land Non-Homesite Value~Agricultural Market Valuation~Market Value~Ag Use Value~Appraised Value~Homestead Cap Loss~Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Address", "Year built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvement Homesite Value", "Land Homesite Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvement Homesite Value");
                dt.Columns.Add("Land Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvement Homesite Value"] = dt1.Rows[i]["Improvement Homesite Value"];
                    dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_MahoningOH(string strorder, string countyid)
    {
        //Parcel ID~Property Address~Owner name~Mailing Address~TaxSet~School District~Neighborhood~Use Code~Acres~Legal Description~Year Built
        // Market Land Value~CAUV~Market Improvement Value~Total~Assessed land Value~Assessed Improvements Value~Assessed Total Value~Board of Revision~Homestead / Disability~Owner Occ Credit~Divided Property~New Construction~Foreclosure~Other Assessments~FrontFt.
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed land Value", "Assessed Improvements Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed land Value");
                dt.Columns.Add("Assessed Improvements Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed land Value"] = dt1.Rows[i]["Assessed land Value"];
                    dt.Rows[i]["Assessed Improvements Value"] = dt1.Rows[i]["Assessed Improvements Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_AraphoeCO(string strorder, string countyid)
    {
        //AIN~Owner Name~Property Address~Mailing Address~Legal Description~Year Built~Land use~Mill Levy
        //Year~Type~Land Value~Building Value~Total
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Building Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_ComalTX(string strorder, string countyid)
    {
        //Property ID~Geographic ID~Owner Name~Mailing Address~Owner ID~Ownership~Type~Legal Description~Address~Neighborhood~Neighborhood CD~Map ID~Exemptions~Year Built~Acres
        //Year~Improvements~Land Market~Ag Valuation~Appraised~HS Cap~Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 4 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Address", "Year built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvements", "Land Market" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvements");
                dt.Columns.Add("Land Market");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                    dt.Rows[i]["Land Market"] = dt1.Rows[i]["Land Market"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_AndersonSC(string strorder, string countyid)
    {
        //Current Owner~Address~City,State~Zip~Subdivision~Physical Address~Tax District~Market Value~Prior Value~Tax Value~Exempt~Legal Description
        //YEAR~ACRES~LOTS~LAND ASMT~BLDG~BLDG ASMT~TOT ASMT~RAT CD~RC
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Current Owner", "Address", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "LAND ASMT", "BLDG ASMT" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("LAND ASMT");
                dt.Columns.Add("BLDG ASMT");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["LAND ASMT"] = dt1.Rows[i]["LAND ASMT"];
                    dt.Rows[i]["BLDG ASMT"] = dt1.Rows[i]["BLDG ASMT"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_SussexDE(string strorder, string countyid)
    {
        //Property Address~Unit~City~State~Zip~Class~Use Code~Town~Tax District~School District~Council District~Fire District~Acres~Owner Name~Mailing Address
        //Percentage~Land Value~Improv Value~Total Value

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improv Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improv Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improv Value"] = dt1.Rows[i]["Improv Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_SandovalNM(string strorder, string countyid)
    {
        //Account~Situs Address~Tax Area~Legal Summary~Owner Name~Owner Address~Property Code~Acres~Estimated Year Built
        //Year~Residential Land~Residential Land Assessed~Residential Improvement~Residential Improvement Assessed~Total Actual Value~Total Assessed Value~Total Exemption Adjustments~Total Taxable
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Situs Address", "Estimated Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Residential Land Assessed", "Residential Improvement Assessed" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Residential Land Assessed");
                dt.Columns.Add("Residential Improvement Assessed");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Residential Land Assessed"] = dt1.Rows[i]["Residential Land Assessed"];
                    dt.Rows[i]["Residential Improvement Assessed"] = dt1.Rows[i]["Residential Improvement Assessed"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_AdaID(string strorder, string countyid)
    {
        //Parcel Status~Owner Name~Property Address~Tax Code Area~Legal Description~Subdivision~Township/Range/Section~Year Built
        //Year~Roll~Category~Acreage~Assessed Value~Valuation Method~Code Area~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            string[] selectedColumnsdt1 = new[] { "Assessed Value", "Total Assessed Value" };
            dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
            //dt.Columns.Add("Owner Name");
            dt.Columns.Add("Assessed Value");
            dt.Columns.Add("Total Assessed Value");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //dt.Rows[i]["Owner Name"] = "";
                dt.Rows[i]["Assessed Value"] = dt1.Rows[i]["Assessed Value"];
                dt.Rows[i]["Total Assessed Value"] = dt1.Rows[i]["Total Assessed Value"];
            }
            //dt.Columns["Owner Name"].SetOrdinal(1);
            //dt.Columns["Year Built"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_KitsapWA(string strorder, string countyid)
    {
        //Taxpayer Name~Mailing Address~Account ID~Site Address~Status~Property Class~Jurisdiction - Tax Code Area~Zoning~Sec - Twn - Rng - Qtr~Acres~Year Built~Tax Description
        //Tax Year~Land~Bldge etc~Market Value~Taxable Value~Exemption~Tax~Tax Without Exemption~FFP~SSWM~Nox Weed~Other~Total Billed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 4 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Taxpayer Name", "Site Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Market Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Market Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Market Value"] = dt1.Rows[i]["Market Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_SpokaneWA(string strorder, string countyid)
    {
        //Owner Name~Owner Address1~Taxpayer Name~TaxPayer Address~Parcel Type~Site Address~City~Description~TaxCode Area~Status~Parcel Class~Neighborhood Code~Neighborhood Name~Neighborhood Desc~Year Built~Acreage~Tax Authority
        //Assessed Taxyear~Taxable~TotalValue~Land~Dwelling And Structure~Current Use Land~Personal Prop
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Site Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Dwelling And Structure" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Dwelling And Structure");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Dwelling And Structure"] = dt1.Rows[i]["Dwelling And Structure"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_ShelbyAL(string strorder, string countyid)
    {
        //Owner Name~Mailing Address~Year Built
        //Tax Year~Land Value~Improvement Value~Total Value~Property Class~Exempt Code~Municipality Code~School District~Over Assessed Value~Class Use~Over 65~Disability~Homestead Year~Exemption Override Amount~Tax Sale~BOE Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["DwellingImprovement Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_SandiegoCA(string strorder, string countyid)
    {
        //Current Owner~Tax Year~Tax Type~Installment~Installment Amount~Due Date (Delinquent After)~Tax Status~Amount Due~Total Due
        //Tax Year~Land Value~Improvement Value~Total Value~Property Class~Exempt Code~Municipality Code~School District~Over Assessed Value~Class Use~Over 65~Disability~Homestead Year~Exemption Override Amount~Tax Sale~BOE Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["DwellingImprovement Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_ElPasoTX(string strorder, string countyid)
    {
        //Property Type~Property Use Code~Geographic ID~Legal Description~Property Use Description~Property Address~Map ID~Owner Name~Mailing Address~Owner ID~Ownership Percentage~Exemptions~Year Built
        //Year~Improvement Homesite Value~Improvement Non Homesite Value~Land Homesite Value~Land Non Homesite Value~Agricultural Market Valuation~Timber Market Valuation~Market Value~Agricultural/Timber Reduction~Appraised Value~HS Cap~Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvement Homesite Value", "Improvement Non Homesite Value", "Land Homesite Value", "Land Non Homesite Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvement Homesite Value");
                dt.Columns.Add("Improvement Non Homesite Value");
                dt.Columns.Add("Land Homesite Value");
                dt.Columns.Add("Land Non Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvement Homesite Value"] = dt1.Rows[i]["Improvement Homesite Value"];
                    dt.Rows[i]["Improvement Non Homesite Value"] = dt1.Rows[i]["Improvement Non Homesite Value"];
                    dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                    dt.Rows[i]["Land Non Homesite Value"] = dt1.Rows[i]["Land Non Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch
            { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_GalvestonTX(string strorder, string countyid)
    {
        //Property ID~Geographic ID~Type~Property Use Code~Property Use Description~Legal Description~Property Address~Neighborhood~Neighborhood CD~Owner Name~Mailing Address~Owner ID~Ownership~Map ID~Exemptions~Year Built~Acres
        //Owner~% Ownership~Total Value~Entity~Description~Tax Rate~Appraised Value~Taxable Value~Estimated Tax
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 3 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Appraised Value", "Taxable Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Appraised Value");
                dt.Columns.Add("Taxable Value");
                // dt.Columns.Add("Land Homesite Value");
                //dt.Columns.Add("Land Non Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Appraised Value"] = dt1.Rows[i]["Appraised Value"];
                    dt.Rows[i]["Taxable Value"] = dt1.Rows[i]["Taxable Value"];
                    //dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                    //dt.Rows[i]["Land Non Homesite Value"] = dt1.Rows[i]["Land Non Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_JeffersonAL(string strorder, string countyid)
    {
        //Owner Name~Mailing Address~Property Address~Legal Description~Year Built
        //Tax Year~Land Value~Improvement Value~Total Value~Property Class~Exempt Code~Municipality Code~School District~Over Assessed Value~Total Millage~Class Use~Over 65~Disability~Homestead Year~Exemption Override Amount~Tax Sale~BOE Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");
                // dt.Columns.Add("Land Homesite Value");
                //dt.Columns.Add("Land Non Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                    //dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                    //dt.Rows[i]["Land Non Homesite Value"] = dt1.Rows[i]["Land Non Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_MontgomeryOh(string strorder, string countyid)
    {
        //Owner Name~Property Address~Mailing Address~Legal Description~Land Use Description~Tax District Name~Year Built
        //Tax Year~Percentage~Land~Improvements~CAUV~Total
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Improvements");
                // dt.Columns.Add("Land Homesite Value");
                //dt.Columns.Add("Land Non Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                    //dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                    //dt.Rows[i]["Land Non Homesite Value"] = dt1.Rows[i]["Land Non Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_SedgwickKS(string strorder, string countyid)
    {
        //Property Address~Legal Description~Owner~Mailing Address~Geo Code~AIN~Tax Unit~Land Use~Market Land Square Feet~Total Acres~Appraisal~Assessment~Year Built~Condition~Tax Authority
        //Year~Class~Land Appraisal Values~Improvements Appraisal Values~Total Appraisal Values~Intrest
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Appraisal Values", "Improvements Appraisal Values" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Appraisal Values");
                dt.Columns.Add("Improvements Appraisal Values");
                // dt.Columns.Add("Land Homesite Value");
                //dt.Columns.Add("Land Non Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Appraisal Values"] = dt1.Rows[i]["Land Appraisal Values"];
                    dt.Rows[i]["Improvements Appraisal Values"] = dt1.Rows[i]["Improvements Appraisal Values"];
                    //dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                    //dt.Rows[i]["Land Non Homesite Value"] = dt1.Rows[i]["Land Non Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_JeffersonKy(string strorder, string countyid)
    {
        //Owner~Neighborhood
        //Assessed Value~Acres
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Value", "Improvements Appraisal Values" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Appraisal Values");
                dt.Columns.Add("Improvements Appraisal Values");
                // dt.Columns.Add("Land Homesite Value");
                //dt.Columns.Add("Land Non Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Appraisal Values"] = dt1.Rows[i]["Land Appraisal Values"];
                    dt.Rows[i]["Improvements Appraisal Values"] = dt1.Rows[i]["Improvements Appraisal Values"];
                    //dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                    //dt.Rows[i]["Land Non Homesite Value"] = dt1.Rows[i]["Land Non Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_MontgomeryAL(string strorder, string countyid)
    {
        //Owner Name~Mailing Address~Property Address~Legal Description~Year Built
        //Tax Year~Land Value~Improvement Value~Total Value~Acres~Property Class~Exempt Code~Municipality Code~School District~OVR Assessed Value~Class Use~Total Millege~Forest Acres~Pre Year Value~Key Number~Over 65 Code~Disability Code~Homestead Year~Exemption Override Amount~Tax Sale~BOE Value~Code~Taxing Authority
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");
                // dt.Columns.Add("Land Homesite Value");
                //dt.Columns.Add("Land Non Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                    //dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                    //dt.Rows[i]["Land Non Homesite Value"] = dt1.Rows[i]["Land Non Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_FortBendTX(string strorder, string countyid)
    {
        //Owner Name~Property Address~Property Status~Property Type~Legal Description~Neighborhood~Account~Map Number~Owner ID~Exemptions~Percent Ownership~Mailing Address~Year Built
        //Year~Improvement Homesite Value~Improvement Non-Homesite Value~Total Improvement Market Value~Land Homesite Value~Land Non-Homesite Value~Land Agricultural Market Value~Total Land Market Value~Total Market Value~Agricultural Use~Total Appraised Value~Homestead Cap Loss~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvement Homesite Value", "Improvement Non-Homesite Value", "Land Homesite Value", "Land Non-Homesite Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvement Homesite Value");
                dt.Columns.Add("Improvement Non-Homesite Value");
                dt.Columns.Add("Land Homesite Value");
                dt.Columns.Add("Land Non-Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvement Homesite Value"] = dt1.Rows[i]["Improvement Homesite Value"];
                    dt.Rows[i]["Improvement Non-Homesite Value"] = dt1.Rows[i]["Improvement Non-Homesite Value"];
                    dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                    dt.Rows[i]["Land Non-Homesite Value"] = dt1.Rows[i]["Land Non-Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch
            { }
            dataview = new DataView(dt);
        }
        return dataview;
    }

    public DataView getassprop_BuncombeNC(string strorder, string countyid)
    {
        //Property Location~Appraisal Area And Appraiser~Acres~Legal Reference~Class~Neighborhood~Taxauthority~County~City~Fire~School~Township
        //Tax year~Owners~Assess Acres~Land value~Building Value~Improvement Value~Exempt Value~Taxable Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Location", "Legal Reference" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Owners", "Land value", "Building Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Owners");
                dt.Columns.Add("Land value");
                dt.Columns.Add("Building Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Owners"] = dt1.Rows[i]["Owners"];
                    dt.Rows[i]["Land value"] = dt1.Rows[i]["Land value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_GuadalupeTX(string strorder, string countyid)
    {
        //Owner Name~Property Address~Property Status~Property Type~Legal Description~Neighborhood~Account~Map Number~Owner ID~Exemptions~Percent Ownership~Mailing Address
        //Year~Improvement Homesite Value~Improvement Non-Homesite Value~Total Improvement Market Value~Land Homesite Value~Land Non-Homesite Value~Land Agricultural Market Value~Total Land Market Value~Total Market Value~Agricultural Use~Total Appraised Value~Homestead Cap Loss~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 3 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvement Homesite Value", "Improvement Non-Homesite Value", "Land Homesite Value", "Land Non-Homesite Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Improvement Homesite Value");
                dt.Columns.Add("Improvement Non-Homesite Value");
                dt.Columns.Add("Land Homesite Value");
                dt.Columns.Add("Land Non-Homesite Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Improvement Homesite Value"] = dt1.Rows[i]["Improvement Homesite Value"];
                    dt.Rows[i]["Improvement Non-Homesite Value"] = dt1.Rows[i]["Improvement Non-Homesite Value"];
                    dt.Rows[i]["Land Homesite Value"] = dt1.Rows[i]["Land Homesite Value"];
                    dt.Rows[i]["Land Non-Homesite Value"] = dt1.Rows[i]["Land Non-Homesite Value"];
                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_MercedCA(string strorder, string countyid)
    {
        //Tax Rate Area~Property Type~Acres~Asmt Description~Asmt Status~Ownership~Year Built
        //Land~Structure~Fixtures~Growing~Total Land and Improvements~Manufactured Home~Personal Property~Homeowners Exemption~Other Exemption~Net Assessment
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Type", "Asmt Description", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Structure" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Structure");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Structure"] = dt1.Rows[i]["Structure"];
                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch
            { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_YamhillOR(string strorder, string countyid)
    {
        //Account Number~Property Address~Alternate Property ID~Legal Description~Property Category~Year Built~Tax Code Area~Total Rate~Account Acres~Tax Authority
        //Value Type~Assessed Value AVR~Exempt Value EAR~Taxable Value TVR~Real Market Land MKLTL~Real Market Buildings MKITL~Real Market Total MKTTL~M5 Market Land MKLND~M5 Limit SAV M5SAV~M5 Market Buildings MKIMP~M50 MAV MAVMK~Assessed Value Exception~Market Value Exception~SA Land (MAVUse Portion) SAVL~Exemptions
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Legal Description", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Value AVR", "Exempt Value EAR" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed Value AVR");
                dt.Columns.Add("Exempt Value EAR");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed Value AVR"] = dt1.Rows[i]["Assessed Value AVR"];
                    dt.Rows[i]["Exempt Value EAR"] = dt1.Rows[i]["Exempt Value EAR"];
                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_VenturaCA(string strorder, string countyid)
    {
        //Property Address~Tract Number~Map Number~Previous Parcel Number~Assessor Property Use Description~Acreage~Year Built

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Assessor Property Use Description", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            //string[] selectedColumnsdt1 = new[] { "Assessed Value AVR", "Exempt Value EAR" };
            //dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
            //dt.Columns.Add("Owner Name");
            //dt.Columns.Add("Assessed Value AVR");
            //dt.Columns.Add("Exempt Value EAR");

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["Owner Name"] = "";
            //    dt.Rows[i]["Assessed Value AVR"] = dt1.Rows[i]["Assessed Value AVR"];
            //    dt.Rows[i]["Exempt Value EAR"] = dt1.Rows[i]["Exempt Value EAR"];
            //}
            //dt.Columns["Owner Name"].SetOrdinal(1);
            //dt.Columns["Year Built"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_SantaCruz(string strorder, string countyid)
    {
        //Property Address~Property Class
        //Year~Land~Improvement~Gross Total~Exemption~Net Assessment
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Property Address", "Property Class" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Improvement" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Improvement");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Improvement"] = dt1.Rows[i]["Improvement"];
                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_TravisTX(string strorder, string countyid)
    {
        //Geographic ID~Type~Legal Description~Address~Neighborhood~Neighborhood CD~Map ID~Name~Mailing Address~Owner ID~Exemptions~Year Built~Acres
        //Year~Improvements~Land Market~Ag Valuation~Appraised~HS Cap~Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 3 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 4 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Name", "Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Market", "Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //   dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Market");
                dt.Columns.Add("Improvements");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Market"] = dt1.Rows[i]["Land Market"];
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                }
                // dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_BrazoriaTX(string strorder, string countyid)
    {
        //Property ID~Geographic ID~Type~Property Use Code~Property Use Description~Legal Description~Property Address~Neighborhood~Neighborhood CD~Owner Name~Mailing Address~Owner ID~% Ownership~Map ID~Exemptions~Year Built~Acres
        //Year~Improvements~Land Market~Ag Valuation~Appraised~HS Cap~Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 4 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Market", "Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //   dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Market");
                dt.Columns.Add("Improvements");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Market"] = dt1.Rows[i]["Land Market"];
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                }
                // dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_MohaveAZ(string strorder, string countyid)
    {
        //Owner~Ownership Type~Mailing Address~Site Address~Multiple Owners~Parcel Size~Township~Range~Section~Year Built
        //Tax Year~Tax Area~Land Value~Improvement Value~Full Cash Value~Assessed Full Cash Value~Limited Valued~Assessed Limited Value~Value Method~Exempt Amount~Exempt Type~Assessor Use Code~Assessment Ratio
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner", "Site Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                // dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                }
                //  dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_MarinCA(string strorder, string countyid)
    {
        //Parcel Number~Owner Name~Use Code~Use Code Definition~Construction Year~Tax Rate Area~Assessment City
        //Total Assessed Value for Tax Roll Year~Land~Improvements~Total Assessed Value~Home Owner~Net Assessed Value for Tax Roll Year~Total Assessed Value1~Less Total Exemptions~Net Assessed Value~Tax Authority
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_No", "Owner Name", "Construction Year" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                // dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Improvements");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                }
                //  dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_LorainOH(string strorder, string countyid)
    {
        //Owner~Location Address~Tax Bill Mailed To~Property Description~Tax District~Land Use~Neighborhood~Acres~School District~Delinquent Real Estate~Year Built
        //Owner~Location Address~Tax Bill Mailed To~Property Description~Tax District~Land Use~Neighborhood~Acres~School District~Delinquent Real Estate~Year Built
        //Market Land Value~Market Building Value~Market Total Value~Market CAUV~Market Abatement~Assessed Land Value~Assessed Building Value~Assessed Total Value~Assessed CAUV~Assessed Abatement        
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_No", "Owner", "Location Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land Value", "Assessed Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                // dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed Land Value");
                dt.Columns.Add("Assessed Building Value");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed Land Value"] = dt1.Rows[i]["Assessed Land Value"];
                    dt.Rows[i]["Assessed Building Value"] = dt1.Rows[i]["Assessed Building Value"];
                }
                //  dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_StanislausCA(string strorder, string countyid)
    {
        // Owner Name~Own Percentage~Pri~Tax Rate Area~Taxability~Land Use~Assessee~Assessment Description~Roll Values As Of~Land~Structure(s)~Fixtures~Growing Improvements~Total Land & Improvements~Personal Property~Personal Property(MH)~Home Owner Exemptions(s)~Other Exemption(s)~Net Assessment~Year Built

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_No", "Owner Name", "Year Built", "Land", "Structure(s)" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                //string[] selectedColumnsdt1 = new[] { "Assessed Land Value", "Assessed Building Value" };
                //dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //// dt.Columns.Add("Owner Name");
                //dt.Columns.Add("Assessed Land Value");
                //dt.Columns.Add("Assessed Building Value");

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    // dt.Rows[i]["Owner Name"] = "";
                //    dt.Rows[i]["Assessed Land Value"] = dt1.Rows[i]["Assessed Land Value"];
                //    dt.Rows[i]["Assessed Building Value"] = dt1.Rows[i]["Assessed Building Value"];
                //}
                //  dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_SonomaCA(string strorder, string countyid)
    {
        //Assessment Number~Tax Rate Area~Lot Size(Acres)
        //Land~Structural Imprv~Fixtures Real Property~Growing Imprv~Total Land and Improvements~Fixtures Personal Property~Personal Property~Manufactured Homes~Homeowners Exemption~Other Exemptions~Net Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_No", "Tax Rate Area" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Structural Imprv" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Structural Imprv");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Structural Imprv"] = dt1.Rows[i]["Structural Imprv"];
                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }

    public DataView getassprop_ImperialCA(string strorder, string countyid)
    {
        //Parcel_number~Tax Rate Area~Property Type~Acres~Lot Size~Asmt Description~Asmt Status~Land~Structure~Fixtures~Growing~Total Land and Improvements~Manufactured Home~Personal Property~Homeowners Exemption~Other Exemption~Net Assessment
        //Address~Roll Category~Assessment~Tax year~Installment~Paid Status~Due/Paid Date~Due~Paid~Balance~Totals Installments~Total Due~Total Paid~Total Balance~Tax Authority
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_No", "Tax Rate Area", "Land", "Structure" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Address" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Address");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Address"] = dt1.Rows[i]["Address"];

                }
                dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_NapaCA(string strorder, string countyid)
    {
        //Assessment Number~Tax Rate Area~Owner Name
        //Land~Structural Imprv~Fixtures Real Property~Growing Imprv~Total Land and Improvements~Fixtures Personal Property~Personal Property~Manufactured Homes~Homeowners Exemption~Other Exemption~Net Assessed Value~Acres
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_No", "Owner Name", "Tax Rate Area" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Structural Imprv" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Land");
                dt.Columns.Add("Structural Imprv");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Structural Imprv"] = dt1.Rows[i]["Structural Imprv"];
                }
                // dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_YavapaiAZ(string strorder, string countyid)
    {
        //Ownership Information~Physical Address~Assessor Acres~Subdivision~Map Type~County Zoning Violation~Section Township Range~Homestead~Incorporated Area~Tracts~Type~Constructed
        //Tax Year~Assessed Value(ALV)~Limited Value(LPV)~Full Cash(FCV)~Legal Class~Assessment Ratio~Usage Code
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_No", "Ownership Information", "Physical Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Value(ALV)", "Limited Value(LPV)" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                //dt.Columns.Add("Owner Name");
                dt.Columns.Add("Assessed Value(ALV)");
                dt.Columns.Add("Limited Value(LPV)");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Assessed Value(ALV)"] = dt1.Rows[i]["Assessed Value(ALV)"];
                    dt.Rows[i]["Limited Value(LPV)"] = dt1.Rows[i]["Limited Value(LPV)"];

                }
                //dt.Columns["Owner Name"].SetOrdinal(1);
                //dt.Columns["Year Built"].SetOrdinal(2);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }

    public DataView getassprop_SolanoCA(string strorder, string countyid)
    {
        //Assessment Number~Exemption~Sub Division~Unit~Lot~Block~Sub Lot~Year Built
        ////Values By Year1~Values By Year2~Values By Year3~Values By Year4~Values By Year5
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_No", "Exemption", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            //string[] selectedColumnsdt1 = new[] { "Land", "Structural Imprv" };
            //dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
            dt.Columns.Add("Owner Name");
            // dt.Columns.Add("Land");
            //dt.Columns.Add("Structural Imprv");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Owner Name"] = "";
                //dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                //dt.Rows[i]["Structural Imprv"] = dt1.Rows[i]["Structural Imprv"];
            }
            dt.Columns["Owner Name"].SetOrdinal(1);
            //dt.Columns["Year Built"].SetOrdinal(2);
            dataview = new DataView(dt);
        }
        return dataview;
    }


    //AB Team
    public DataView getassprop_adams(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Parcel ID~Account Number~Owners Name~Property Address~Legal Description~Subdivision Plat~Tax District~Mill Levy~Permit Cases~Year Built
            //Assessment Type~Account Number~Land Type~Unit of Measure~Number of Units~Fire District~School District~Vacant/Improved~Actual Value~Assessed Value~Building Number~Actual  Value~Assessed  Value
            string[] selectedColumnsdt = new[] { "Parcel ID", "Owners Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "LandValue", "ImprValue" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("LandValue");
                dt.Columns.Add("ImprValue");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["LandValue"] = dt1.Rows[i]["LandValue"];
                    dt.Rows[i]["ImprValue"] = dt1.Rows[i]["ImprValue"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_annearundel(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Account Number~Property Address~Legal Description~YearBuilt~Use~Principal Residence~Map~Grid~Parcel~Sub District~Sub Division~Section~Block~Lot~Assessment Year~Homestead Application Status~Homeowners Tax Credit Application Status~Date
            //Land~Building~Total
            string[] selectedColumnsdt = new[] { "Account Number", "Property Address", "YearBuilt" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Building");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_baldwin(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Parcel-Number~PROPERTY ADDRESS~OWNER NAME~MAILING ADDRESS~LEGAL DESCRIPTION~TAX DISTRICT~PROPERTY CLASS~ESTIMATED TAX~LAND USE
            //ASSESSED YEAR~LAND VALUE~BUILDING VALUE~TOTAL VALUE
            string[] selectedColumnsdt = new[] { "parcel_no", "OWNER NAME", "PROPERTY ADDRESS" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "LAND VALUE", "BUILDING VALUE" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("LAND VALUE");
                dt.Columns.Add("BUILDING VALUE");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["LAND VALUE"] = dt1.Rows[i]["LAND VALUE"];
                    dt.Rows[i]["BUILDING VALUE"] = dt1.Rows[i]["BUILDING VALUE"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_beaufort(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Parcel ID~Alternate ID (AIN)~Owner~Property Address~Mailing Address~Property Class Code~Legal Description~Year Built
            //Assessment Year~Appraised Value Land~Appraised Value Improvements~Total Appraised Value~Limited (Capped) Appraised Value Total~Exemption Amount~Taxable Value~Assessment Ratio~Assessed Value
            string[] selectedColumnsdt = new[] { "Parcel ID", "Owner", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Appraised Value Land", "Appraised Value Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Appraised Value Land");
                dt.Columns.Add("Appraised Value Improvements");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Appraised Value Land"] = dt1.Rows[i]["Appraised Value Land"];
                    dt.Rows[i]["Appraised Value Improvements"] = dt1.Rows[i]["Appraised Value Improvements"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_benton(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Parcel Number~County Name~Ownership Information~Property Address~Billing Information~Total Acres~Timber Acres~Sec-Twp-Rng~Lot/Block~Subdivision~Legal Description~School District~Homestead Parcel~Tax Status~Over 65?~Year Built
            //Entry~Appraised~Assessed~Assessment Year~Appraised Land~Assessed Land~Appraised Improvements~Assessed Improvements~AppraisedTotalValue~AssessedTotalValue~Taxable Value~Millage~Estimated Taxes
            string[] selectedColumnsdt = new[] { "Parcel Number", "Ownership Information", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land", "Assessed Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Assessed Land");
                dt.Columns.Add("Assessed Improvements");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Assessed Land"] = dt1.Rows[i]["Assessed Land"];
                    dt.Rows[i]["Assessed Improvements"] = dt1.Rows[i]["Assessed Improvements"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_brevard(string strorder, string countyid)
    {
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Account Number~Owner Name~Mailing Address~Property Address~Parcel ID~Property Use~Legal Description~Year Built
            //Year~Market Value~Agricultural Land Value~Assessed Value Non-School~Assessed Value School~Homestead Exemption~Additional Homestead~Other Exemptions~Taxable Value Non-School~Taxable Value School
            string[] selectedColumnsdt = new[] { "Account Number", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Agricultural Land Value", "Assessed Value Non-School", "Assessed Value School", "Homestead Exemption" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Agricultural Land Value");
                dt.Columns.Add("Assessed Value Non-School");
                dt.Columns.Add("Assessed Value School");
                dt.Columns.Add("Homestead Exemption");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Agricultural Land Value"] = dt1.Rows[i]["Agricultural Land Value"];
                    dt.Rows[i]["Assessed Value Non-School"] = dt1.Rows[i]["Assessed Value Non-School"];
                    dt.Rows[i]["Assessed Value School"] = dt1.Rows[i]["Assessed Value School"];
                    dt.Rows[i]["Homestead Exemption"] = dt1.Rows[i]["Homestead Exemption"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;
    }
    public DataView getassprop_cobb(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Parcel Number~Property Address~Neighborhood~Owner~Tax District~Subdivision Number~Property Class~Year Built
            //TAX YEAR~Land Value~Building Value~Total Assessed Value
            string[] selectedColumnsdt = new[] { "Parcel Number", "Owner", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Building Value");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Cuyahoga(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Parcel No~Owner Name~Property Address~Legal Description~Year Built~Occupancy~Assessment Year            
            //Land~Improvement~Total Assessed Value~Homestead Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Improvement" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Improvement");


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Improvement"] = dt1.Rows[i]["Improvement"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_DeSoto(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Property Address~Owner Name~County~Tax Year~Receipt Number~Legal Description
            //Tax District~Appraised Land Value~Appraised Improvement Value~Appraised Total Value~Assessed land Value~Assessed Improvement Value~Total Assessed Value~Millage Rate~Gross Tax~Homestead Credit Amount
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed land Value", "Assessed Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Assessed land Value");
                dt.Columns.Add("Assessed Improvement Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Assessed land Value"] = dt1.Rows[i]["Assessed land Value"];
                    dt.Rows[i]["Assessed Improvement Value"] = dt1.Rows[i]["Assessed Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }

    public DataView getassprop_DouglasN(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            // Owner Name~Mailing Address~Key Number~Account Type~Parcel Number~Parcel Address~Legal Description~Acres~Year Built            
            //Value~Land~Improvement~Total
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Improvement" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Improvement");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Improvement"] = dt1.Rows[i]["Improvement"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Elpasco(string strorder, string countyid)
    {
        //Schedule No~Owner Name~Location~Legal Description~Plat No~Year Built~Use~Schedule Number~Name~Mailing Address~Property Address~Property Type~Alerts~Tax Authority
        //Levy Year~Mill Levy~Exempt Status~Table~Use Code~2018 Market Value~2018 Assessed Value~Exempt~Total Assessed Land~Total Assessed Improvements~Total Assessed~Total Market Value~Base Tax Amount~Special Assessment Amount~Improvement District Amount~Total Current Year Taxes

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Total Assessed Land", "Total Assessed Improvements" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Total Assessed Land");
                dt.Columns.Add("Total Assessed Improvements");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Total Assessed Land"] = dt1.Rows[i]["Total Assessed Land"];
                    dt.Rows[i]["Total Assessed Improvements"] = dt1.Rows[i]["Total Assessed Improvements"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Forsyth(string strorder, string countyid)
    {
        //Location Address~Legal Description~Property Class~Neighborhood~Tax District~Zoning~Acres~Homestead~Exemptions~Owner & Mailing Address~Year Built
        //Assessment~LUC~Class~Land Value~Building Value~Total Value~Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_no", "Location Address", "Owner & Mailing Address", "Exemptions", "Legal Description", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Building Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_frederick(string strorder, string countyid)
    {
        //Owner Name~Mailing Address~Property Address~Legal Description~Year Built~Use~Principal Residence~Map~Grid~Parcel~Sub District~SubDivision~Section~Block~Lot~Assessment Year~Homestead Application Status~Home Owners Tax Credit Application Status~Home owners T...
        //Year~Land Value~Building Value~Personal Value~Total Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 5 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);

            //string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value" };
            //dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

            //dt.Columns.Add("Land Value");
            //dt.Columns.Add("Building Value");
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
            //    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
            //}
            dataview = new DataView(dt);
        }
        return dataview;
    }

    public DataView getassprop_Hawai(string strorder, string countyid)
    {
        //Owner Name~Parcel Number~Location Address~Property Class~Neighbourhood Code~Land Area(acres)~Legal Infomation~Year Built
        //Year~PropertyClass~MarketLandValue~DedicatedUseValue~LandExemption~NetTaxableLandValue~MarketBuildingValue~AssessedBuildingValue~BuildingExemption~NetTaxableBuildingValue~TotalTaxableValue

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Location Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "MarketLandValue", "AssessedBuildingValue" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("MarketLandValue");
                dt.Columns.Add("AssessedBuildingValue");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["MarketLandValue"] = dt1.Rows[i]["MarketLandValue"];
                    dt.Rows[i]["AssessedBuildingValue"] = dt1.Rows[i]["AssessedBuildingValue"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Henry(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner Name~Mailing Address~Location Address~Parcel Number~Property Type~Year Built~Legal Description
            //Land Value~Building Value~Misc Value~Total Value~Exemptions
            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Location Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Building Value", "Exemptions" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Building Value");
                dt.Columns.Add("Exemptions");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                    dt.Rows[i]["Exemptions"] = dt1.Rows[i]["Exemptions"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Honolulu(string strorder, string countyid)
    {
        //Parcel Number~Owner Name~Location Address~Property Class~Land Area(approximate sq ft)~Land Area(acres)~Legal Information~Year Built
        //Assessment Year~Property Class~Assessed Land Value~Dedicated Use Value~Land Exemption~Net Taxable Land Value~Assessed Building Value~Building Exemption~Net Taxable Building Value~Total Property Assessed Value~Total Property Exemption~Total Net Taxable Value

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Location Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land Value", "Assessed Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Assessed Land Value");
                dt.Columns.Add("Assessed Building Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Assessed Land Value"] = dt1.Rows[i]["Assessed Land Value"];
                    dt.Rows[i]["Assessed Building Value"] = dt1.Rows[i]["Assessed Building Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Horry(string strorder, string countyid)
    {
        //PIN~TMS~Owner~District~Estimated Acres~Estimated Year Built~Legal Description
        //Type~Residential Land~Residential Improved~Farm Land~Farm Improved~Farm Use~Other Land~Other Improved~Taxable Total~Residential Building~Farm Building~Other Building~Total Market Value~Permits Type~Assessment Year~Reason for Change~Market Land Value~Market Improvement Value~Market Total Value~LandUse Land~LandUse Improvement~LandUse Total
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner", "Estimated Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Market Land Value", "Market Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Market Land Value");
                dt.Columns.Add("Market Improvement Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Market Land Value"] = dt1.Rows[i]["Market Land Value"];
                    dt.Rows[i]["Market Improvement Value"] = dt1.Rows[i]["Market Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Jackson(string strorder, string countyid)
    {
        //Property Address~Property Description~Property Category~Status~Tax Code Area~Property Class~Taxpayer~Owner~Mortgage Company
        //Tax Year~Market Value Total~Taxable Value Total~Assessed Value Total
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner", "Property Address", "Property Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);

            //string[] selectedColumnsdt1 = new[] { "Market Value Total", "Assessed Value Total" };
            //dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

            //dt.Columns.Add("Market Value Total");
            //dt.Columns.Add("Assessed Value Total");
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["Market Value Total"] = dt1.Rows[i]["Market Value Total"];
            //    dt.Rows[i]["Assessed Value Total"] = dt1.Rows[i]["Assessed Value Total"];

            //}

            dataview = new DataView(dt);
        }
        return dataview;

    }

    public DataView getassprop_kane(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Parcel Number~Site Address~Owner Name~Mailing Address~Tax Year~Property Class~Tax Code~Tax Status~Net Taxable Value~Tax Rate~Total Tax~Legal Description
            //Level~Homesite~Dwelling~Farm Land~Farm Building~Mineral~Total
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Site Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Farm Land", "Farm Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Farm Land");
                dt.Columns.Add("Farm Building");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Farm Land"] = dt1.Rows[i]["Farm Land"];
                    dt.Rows[i]["Farm Building"] = dt1.Rows[i]["Farm Building"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    ////public DataView getassprop_LakeIL(string strorder, string countyid)
    //{
    //    //ParcelNumber~TaxID~Property Address~Legal Description~Property Class~Owner Name~Mailing Address~Year Built
    //    //Assessment Year~Reason~Total Land~Cap 1 Land~Cap 2 Land~Cap 2 Ag Land~Cap 2 LTC Land~Cap 3 Land~Total Improv~Cap 1 Improv~Cap 2 Improv~Cap 2 LTC Improv~Cap 3 Improv~Total Value
    //    DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
    //    DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
    //    if (dt.Rows.Count > 0)
    //    {

    //        string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
    //        dt = new DataView(dt).ToTable(false, selectedColumnsdt);

    //        string[] selectedColumnsdt1 = new[] { "Total Land", "Total Improv" };
    //        dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

    //        dt.Columns.Add("Total Land");
    //        dt.Columns.Add("Total Improv");
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            dt.Rows[i]["Total Land"] = dt1.Rows[i]["Total Land"];
    //            dt.Rows[i]["Total Improv"] = dt1.Rows[i]["Total Improv"];

    //        }

    //        dataview = new DataView(dt);
    //    }
    //    return dataview;

    //}
    public DataView getassprop_LakeIn(string strorder, string countyid)
    {
        //ParcelNumber~TaxID~Property Address~Legal Description~Property Class~Owner Name~Mailing Address~Year Built
        //Assessment Year~Reason~Total Land~Cap 1 Land~Cap 2 Land~Cap 2 Ag Land~Cap 2 LTC Land~Cap 3 Land~Total Improv~Cap 1 Improv~Cap 2 Improv~Cap 2 LTC Improv~Cap 3 Improv~Total Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Total Land", "Total Improv" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Total Land");
                dt.Columns.Add("Total Improv");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Total Land"] = dt1.Rows[i]["Total Land"];
                    dt.Rows[i]["Total Improv"] = dt1.Rows[i]["Total Improv"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_LakeOH(string strorder, string countyid)
    {
        //Parcel ID~Owner Name~Property Address~Mailing Address~Property Type~Legal Description~Year Built
        //Appraised Land Value~Appraised Building Value~Total Appraised Value~Assessed Land Value~Assessed Building Value~Total Assessed Value~CAUV Value~Taxable Value~2.5% Homesite Rollback~Homestead
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land Value", "Assessed Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Assessed Land Value");
                dt.Columns.Add("Assessed Building Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Assessed Land Value"] = dt1.Rows[i]["Assessed Land Value"];
                    dt.Rows[i]["Assessed Building Value"] = dt1.Rows[i]["Assessed Building Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_LeeFL(string strorder, string countyid)
    {
        //ParcelID~Folio No~Owner Name~Mailing Address~Property Address~Legal Description~Use Code~Use Code Description~Year Built
        //Assessed Year~Just~Assessed~Portability Applied~Cap Assessed~Taxable~Cap Difference
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Just", "Assessed" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Just");
                dt.Columns.Add("Assessed");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Just"] = dt1.Rows[i]["Just"];
                    dt.Rows[i]["Assessed"] = dt1.Rows[i]["Assessed"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Lexington(string strorder, string countyid)
    {
        //Parcel Number~Owner Name~Mailing Address~Property Address~Legal Description~Land use~Year Built~Assessed Year
        //Taxable Land Value~Taxable Building Value~Assessed Land Value~Assessment Building Value~HomesteadExemption~Tax Relief Exemption
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Assessed Land Value", "Assessment Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Assessed Land Value");
                dt.Columns.Add("Assessment Building Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Assessed Land Value"] = dt1.Rows[i]["Assessed Land Value"];
                    dt.Rows[i]["Assessment Building Value"] = dt1.Rows[i]["Assessment Building Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Lucas(string strorder, string countyid)
    {
        //ASSESSOR#~Tax District~Class~Land Use~Market Area~Zoning Code~Zoning Description~Owner~Property Address~Legal Desc~Census Tract~ Residential Year Built~Acres~Commercial Year Built
        //Values~35% Values~100% Values~35% Roll~100% Roll~Homestead Exemption~Owner Occupied Credit~CAUV~Agricultural District~Value Change History Details~Land~Building~Total~Tax Year~Reason~Change Date~Class / Use~Year
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner", "Property Address", " Residential Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Building");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Maui(string strorder, string countyid)
    {
        //ParcelNumber~Owner Name~Mailing Address~Location Address~Legal Information~Year Built
        //Year~TaxClass~MarketLandValue~AgriculturalLandValue~AssessedLandValue~BuildingValue~TotalAssessedValue~TotalExemptionValue~TotalNet TaxableValue
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "AssessedLandValue", "BuildingValue" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("AssessedLandValue");
                dt.Columns.Add("BuildingValue");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["AssessedLandValue"] = dt1.Rows[i]["AssessedLandValue"];
                    dt.Rows[i]["BuildingValue"] = dt1.Rows[i]["BuildingValue"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_McHenry(string strorder, string countyid)
    {
        //Parcel Number~Site Address~Owner Name~Mailing Address~Tax Year~Property Class~Tax Code~Tax Status~Net Taxable Value~Tax Rate~Total Tax~Acres~Legal Description
        //Level~Homesite~Dwelling~Farm Land~Farm Building~Mineral~Total
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Site Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Farm Land", "Farm Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Farm Land");
                dt.Columns.Add("Farm Building");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Farm Land"] = dt1.Rows[i]["Farm Land"];
                    dt.Rows[i]["Farm Building"] = dt1.Rows[i]["Farm Building"];

                }
            }
            catch
            { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_MobileAL(string strorder, string countyid)
    {
        //ParcelNumber~Owner Name~Mailing Address~Property Address~Legal Description~Year Built
        //Year~Property Class~Over 65~Exempt Code~Disability~Municipality Code~Homestead Year~School District~Exemption Override Amount~Over Assessed Value~Class Use~Tax Sale~BOE Value~Key Number~Land Value~Improvement Value~Total Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Mailing Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Montgomery(string strorder, string countyid)
    {
        //Owner Name~Property Address~Legal Description~Year Built~Use~Principal Residence~Map~Grid~Parcel~Sub District~SubDivision~Section~Block~Lot~Assessment Year~Town~Homestead Application Status~Home Owners Tax Credit Application Status~Home owners Tax Credit Application Date
        //Value~Land~Building~Total~Total Assessed Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Building");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Paulding(string strorder, string countyid)
    {
        //Owner~Owner Address~Account And Realkey~Location Address~Zip Code~Class~Tax District~Millage Rate~Acres~Neighborhood~Homestead Exemption~Landlot And District And Section~Subdivision~YearBuilt
        //Year~Previous Value~Land Value~Improvement Value~Accessory Value~Current Value
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner", "Location Address", "YearBuilt" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_PimaAZ(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Property Address~Taxpayer Information~Property Description~Tax Year~Tax Area~Year Built
            //Valuation Year~Legal Class~Assessment Ratio~Total FCV~Limited Value~Limited Assessed
            string[] selectedColumnsdt = new[] { "Parcel_no", "Property Address", "Year Built", "Property Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Limited Value", "Limited Assessed" };
                dt1 = new DataView(dt).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Owner Name");
                dt.Columns.Add("Limited Value");
                dt.Columns.Add("Limited Assessed");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Owner Name"] = "";
                    dt.Rows[i]["Limited Value"] = dt1.Rows[i]["Limited Value"];
                    dt.Rows[i]["Limited Assessed"] = dt1.Rows[i]["Limited Assessed"];
                }
                dt.Columns["Owner Name"].SetOrdinal(1);
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }

    public DataView getassprop_PinellasFL(string strorder, string countyid)
    {
        //Parcel Number~Owner/Mailing Address~Site Address~Legal Description~Tax District~Exemption~2018~2019~Year~Homestead Exemption~Just/Market Value~Assessed Value~County Taxable Value~School Taxable Value~Municipal Taxable Value
        // Year~Just / Market Value~Assessed Value / Non - HX Cap~County Taxable Value~School Taxable Value~Municipal Taxable Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner/Mailing Address", "Site Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            if (dt1.Rows.Count > 0)
            {
                string[] selectedColumnsdt1 = new[] { "Just / Market Value", "Assessed Value / Non - HX Cap" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Just / Market Value");
                dt.Columns.Add("Assessed Value / Non - HX Cap");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Just / Market Value"] = dt1.Rows[i]["Just / Market Value"];
                    dt.Rows[i]["Assessed Value / Non - HX Cap"] = dt1.Rows[i]["Assessed Value / Non - HX Cap"];

                }
            }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_RichlandSC(string strorder, string countyid)
    {
        //ParcelNumber~Owner~Property Location/Code~Legal Description~Land Type
        //Year Of Assessment~Tax District~Acreage Of Parcel~Non-Agriculture Value~Building Value~Taxable Value~Zoning~Legal Residence~Agriculture Value~Improvements
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner", "Property Location/Code" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Non-Agriculture Value", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Non-Agriculture Value");
                dt.Columns.Add("Building Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Non-Agriculture Value"] = dt1.Rows[i]["Non-Agriculture Value"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];

                }
            }
            catch
            {

            }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_seminole(string strorder, string countyid)
    {
        //Owner(s)~Property Address~Mailing~Subdivision Name~Tax District~Property Type~Exemptions~Legal Description~Year Built
        //Year~Depreciated Building Value~Depreciated EXFT Value~Land Value (Market)~Land Value Agriculture~Just/Market Value **~Portability Adj~Save Our Homes Adj~Amendment 1 Adj~P&G Adj~Assessed Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner(s)", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value Agriculture", "Assessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value Agriculture");
                dt.Columns.Add("Assessed Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value Agriculture"] = dt1.Rows[i]["Land Value Agriculture"];
                    dt.Rows[i]["Assessed Value"] = dt1.Rows[i]["Assessed Value"];

                }
            }

            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_SnohomishWA(string strorder, string countyid)
    {
        //Property Address~Owner Name~Property Description~Tax Code Area~Use Code~Year Built~Tax Authority
        //Tax Year~Taxable Value~Exemption Amount~Market Total~Assessed Value~Market Land~Market Improvement~Personal Property~Active Exemption
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Market Land", "Market Improvement" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Market Land");
                dt.Columns.Add("Market Improvement");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Market Land"] = dt1.Rows[i]["Market Land"];
                    dt.Rows[i]["Market Improvement"] = dt1.Rows[i]["Market Improvement"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_Thurston(string strorder, string countyid)
    {
        //Parcel Number~Role~Pct~Name\Street~City~State~Country~Zip  ~Situs Address~Abbreviated Legal~Sect/Town/Range~Size~Use Code~TCA Number~Taxable~Neighborhood~Property Type~Year Built~Lot Acreage~Value Information~Tax Year~Assessment Year~Market Value Land~Market Value Buildings~Market Value Total~Exemption~Active exemptions
        //Value Type~Taxable Value Regular~Exemption Amount Regular~Market Total~Assessed Value~Market Land~Market Improvement~Personal Property~Active exemptions
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Situs Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Market Land", "Market Improvement" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Market Land");
                dt.Columns.Add("Market Improvement");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Market Land"] = dt1.Rows[i]["Market Land"];
                    dt.Rows[i]["Market Improvement"] = dt1.Rows[i]["Market Improvement"];

                }
            }
            catch { }

            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_WashingtonNew(string strorder, string countyid)
    {
        //Parcel Number~Status~Current Owner~Property Address~Taxing District~Tax Description~Class~Legal Description~Year Built
        //Year~Land Value~Dwelling Value ~Improvement Value~Total Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 15 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Current Owner", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value", "Improvement Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value");
                dt.Columns.Add("Improvement Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_WedCO(string strorder, string countyid)
    {
        //Account Number~Parcel ID~Space~Property Type~Tax Year~Legal Description~Property Address~Property City~Zip~Year Built~Owner Name~Address
        //Type~Code~Description~Actual Value~Assessed Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Actual Value", "Assessed Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Actual Value");
                dt.Columns.Add("Assessed Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Actual Value"] = dt1.Rows[i]["Actual Value"];
                    dt.Rows[i]["Assessed Value"] = dt1.Rows[i]["Assessed Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_DavidsonTN(string strorder, string countyid)
    {
        //Map & Parcel~Location~Current Owner~Legal Description~Tax District~Assessment Classification~Legal Reference~Mailing Address
        //Assessment Year~Last Reappraisal Year~Improvement Value~Land Value~Total Appraisal Value~Assessed Value~Property Use~Zone~Neighborhood~Land Area~Property Type~Year Built
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Current Owner", "Location", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvement Value", "Land Value", "Year Built" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Improvement Value");
                dt.Columns.Add("Land Value");
                dt.Columns.Add("Year Built");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Improvement Value"] = dt1.Rows[i]["Improvement Value"];
                    dt.Rows[i]["Land Value"] = dt1.Rows[i]["Land Value"];
                    dt.Rows[i]["Year Built"] = dt1.Rows[i]["Year Built"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_DuPageIL(string strorder, string countyid)
    {
        //Parcel ID~Property Address~Owner Name~Mailing Address
        //Assessment Year~Last Reappraisal Year~Improvement Value~Land Value~Total Appraisal Value~Assessed Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        //DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);

            // string[] selectedColumnsdt1 = new[] { "Land:", "Building", "Year Built" };
            //dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

            //dt.Columns.Add("Land");
            //dt.Columns.Add("Building");
            //dt.Columns.Add("Year Built");
            //  for (int i = 0; i < dt.Rows.Count; i++)
            // {
            //dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
            //dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];
            // dt.Rows[i]["Year Built"] = dt1.Rows[i]["Year Built"];
            // }

            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_ButlerOH(string strorder, string countyid)
    {
        //Parcel Number~Parcel ID~Property Address~Owner Name~Mailing Address~Class~Land Use Code~Neighborhood~Total Acres~Taxing District~District Name~Gross Tax Rate~Effective Tax Rate~Non Business Credit~Owner Occupied Credit~Year Built~Legal Description~Manufactured Homes Year Built 
        //Land (100%)~Building (100%)~Total Value (100%)~CAUV~Assessed Tax Year~Land (35%)~Building (35%)~Assessed Total (35%)
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land (100%)", "Building (100%)" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land (100%)");
                dt.Columns.Add("Building (100%)");
                //dt.Columns.Add("Year Built");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land (100%)"] = dt1.Rows[i]["Land (100%)"];
                    dt.Rows[i]["Building (100%)"] = dt1.Rows[i]["Building (100%)"];
                    // dt.Rows[i]["Year Built"] = dt1.Rows[i]["Year Built"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_WeberUT(string strorder, string countyid)
    {
        //Owner~Property Address~Mailing Address~Tax Unit~Prior Parcel Numbers~Legal Description~Property Type~Year Built~Lot Size
        //Land (100%)~Building (100%)~Total Value (100%)~CAUV~Assessed Tax Year~Land (35%)~Building (35%)~Assessed Total (35%)
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land (100%)", "Building (100%)" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land (100%)");
                dt.Columns.Add("Building (100%)");
                //dt.Columns.Add("Year Built");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land (100%)"] = dt1.Rows[i]["Land (100%)"];
                    dt.Rows[i]["Building (100%)"] = dt1.Rows[i]["Building (100%)"];
                    // dt.Rows[i]["Year Built"] = dt1.Rows[i]["Year Built"];
                }
            }
            catch { }

            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_WakeNC(string strorder, string countyid)
    {
        //Parcel No.~PIN #~Owner Name~Property Address~Mailing Address~Legal Description:~Year Built
        //Land Value Assessed~Bldg. Value Assessed~Tax Relief~Land Use Value~Use Value Deferment~Historic Deferment~Total Deferred Value~Use/Hist/Tax Relief Assessed~Total Value Assessed*
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description:" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value Assessed", "Bldg. Value Assessed" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value Assessed");
                dt.Columns.Add("Bldg. Value Assessed");
                //dt.Columns.Add("Year Built");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value Assessed"] = dt1.Rows[i]["Land Value Assessed"];
                    dt.Rows[i]["Bldg. Value Assessed"] = dt1.Rows[i]["Bldg. Value Assessed"];
                    // dt.Rows[i]["Year Built"] = dt1.Rows[i]["Year Built"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }

    public DataView getassprop_YorkSC(string strorder, string countyid)
    {
        //Parcel Number~Owner Name~Mailing Address~Property Address~Legal Description~Class Code
        //Land Market Value~Improvement Market Value~Total Market Value~Taxable Value~Total Assessed
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "Parcel_no", "Owner Name", "Property Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Market Value", "Improvement Market Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Market Value");
                dt.Columns.Add("Improvement Market Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Market Value"] = dt1.Rows[i]["Land Market Value"];
                    dt.Rows[i]["Improvement Market Value"] = dt1.Rows[i]["Improvement Market Value"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }

    public DataView getassprop_DouglasNV(string strorder, string countyid)
    {

        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {
            //Owner Name~Mailing Address~Key Number~Account Type~Parcel Number~Parcel Address~Legal Description~Acres~Year Built~ParcelNumber~Property Address~Town~District~Assessed Owner Name~Legal Owner Name
            //Value~Land~Improvement~Total~Assessed Year~Assessed Land Value~Asselssed Improvements Value~Assessed Personal Property Value~Assessed Ag Land Value~Assessed Exemptions Value~Net Assessed Value~Taxable Land Value~Taxable Improvements Value~Taxable Personal Property Value~Taxable Ag Land Value~Taxable Exemptions Value~Net Taxable Value
            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Legal Description", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Improvement" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
                dt.Columns.Add("Land");
                dt.Columns.Add("Improvement");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Improvement"] = dt1.Rows[i]["Improvement"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_LakeIL(string strorder, string countyid)
    {
        //ParcelNumber~TaxID~Property Address~Legal Description~Property Class~Owner Name~Mailing Address~Year Built~Property Location~Tax Year~Pin Number~Tax Code~Acres
        //Assessment Year~Reason~Total Land~Cap 1 Land~Cap 2 Land~Cap 2 Ag Land~Cap 2 LTC Land~Cap 3 Land~Total Improv~Cap 1 Improv~Cap 2 Improv~Cap 2 LTC Improv~Cap 3 Improv~Total Value~Pin~Street Address~City~Zip Code~Land Amount~Building Amount~Total Amount~Township~Assessment Date~Class Description~Year Built / Effective Age
        //ParcelNumber~TaxID~Property Address~Legal Description~Property Class~Owner Name~Mailing Address~Year Built
        //Assessment Year~Reason~Total Land~Cap 1 Land~Cap 2 Land~Cap 2 Ag Land~Cap 2 LTC Land~Cap 3 Land~Total Improv~Cap 1 Improv~Cap 2 Improv~Cap 2 LTC Improv~Cap 3 Improv~Total Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Total Land", "Total Improv" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Total Land");
                dt.Columns.Add("Total Improv");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Total Land"] = dt1.Rows[i]["Total Land"];
                    dt.Rows[i]["Total Improv"] = dt1.Rows[i]["Total Improv"];

                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_JeffersonCO(string strorder, string countyid)
    {
        //PIN/Schedule~AIN/Parcel ID~Property Class~Owner Name~Property Address~Mailing Address~Subdivision~Block~Lot~Track/Key~Section~Township~Range~QSection~Acres~Neighborhood~Year Built
        //Tax Year~Actual Land Value~Actual Imp Value~Actual Total Value~Assessed Land Value~Assessed Imp Value~Assessed Total Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Actual Land Value", "Actual Imp Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Actual Land Value");
                dt.Columns.Add("Actual Imp Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Actual Land Value"] = dt1.Rows[i]["Actual Land Value"];
                    dt.Rows[i]["Actual Imp Value"] = dt1.Rows[i]["Actual Imp Value"];
                }
            }
            catch { }

            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_LakeFL(string strorder, string countyid)
    {
        //Alternate Key~Owner Name~Property Address~Mailing Address~Property Type~Year Built~Legal Description
        //Land Value 1~Building Value~UTILITY BUILDING - FINISHED (UBF)
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land Value 1", "Building Value" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land Value 1");
                dt.Columns.Add("Building Value");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land Value 1"] = dt1.Rows[i]["Land Value 1"];
                    dt.Rows[i]["Building Value"] = dt1.Rows[i]["Building Value"];
                }
            }
            catch { }

            dataview = new DataView(dt);


        }
        return dataview;

    }
    public DataView getassprop_LancasterNE(string strorder, string countyid)
    {
        //ParcelNumber~Owner Name~Property Address~Mailing Address~Year Built~Legal Description~Exemption~Property Class
        //Year~Land~Building~Total
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Property Address", "Year Built", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Land", "Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Land");
                dt.Columns.Add("Building");
                //dt.Columns.Add("Land Non-Residential");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Land"] = dt1.Rows[i]["Land"];
                    dt.Rows[i]["Building"] = dt1.Rows[i]["Building"];
                    //dt.Rows[i]["Land Non-Residential"] = dt1.Rows[i]["Land Non-Residential"];
                }
            }
            catch { }
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_QueensNY(string strorder, string countyid)
    {
        //Parcel Number~Owner Name~Mailing Address~Good Through Date~BBL~Credits~Due
        //Assessment Year~Reason for Change~Land Homestead~Land Residential~Land Non-Residential~Total Land~Improvements Homestead~Improvements Residential~Improvements Non-Residential~Total Improvement~Total Assessed Value
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Mailing Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);

            // string[] selectedColumnsdt1 = new[] { "Land Homestead", "Land Residential", "Land Non-Residential" };
            //dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);
            dt.Columns.Add("Year Built");
            //dt.Columns.Add("Land Homestead");
            //dt.Columns.Add("Land Residential");
            //dt.Columns.Add("Land Non-Residential");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Year Built"] = "";
                //dt.Rows[i]["Land Homestead"] = dt1.Rows[i]["Land Homestead"];
                //dt.Rows[i]["Land Residential"] = dt1.Rows[i]["Land Residential"];
                //dt.Rows[i]["Land Non-Residential"] = dt1.Rows[i]["Land Non-Residential"];
            }
            dt.Columns["Year Built"].SetOrdinal(1);
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_LoudounVA(string strorder, string countyid)
    {
        //PARID~Name Care Of~Mailing Address~Primary Address~Tax Map#~State Use Class~Total Land Area (Acreage)~Election District~Billing District~Special Ad Valorem Tax District~Subdivision~Legal Description~Year Built
        //Fair Market Land~Fair Market Building~Prorated Bldg~Effective Date~Fair Market Total~Land Use Value~Total Taxable Value~*Deferred Land Use Value~Tax Exempt Code~Tax Exempt Land~Tax Exempt Building~Tax Exempt Total~Revitalized Real Estate~Solar Exemption
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Year Built", "Primary Address" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Fair Market Land", "Fair Market Building" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Fair Market Land");
                dt.Columns.Add("Fair Market Building");
                //dt.Columns.Add("Land Non-Residential");
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["Fair Market Land"] = dt1.Rows[i]["Fair Market Land"];
                    dt.Rows[i]["Fair Market Building"] = dt1.Rows[i]["Fair Market Building"];
                    //dt.Rows[i]["Land Non-Residential"] = dt1.Rows[i]["Land Non-Residential"];
                }

            }
            catch { }
            //dt.Columns["Year Built"].SetOrdinal(1);
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public DataView getassprop_OklahomaOK(string strorder, string countyid)
    {
        //Type~Owner Name~Property Address~Mailing Address~Legal Description~Year Built
        //Assessed Year~Market Value~Taxable Market~Gross Assessed~Exemption~Net Assessed~Millage~Tax~Tax Savings
        DataTable dt = readdatafromcloudAB("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloudAB("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 2 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Owner Name", "Year Built", "Property Address", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Market Value", "Taxable Market" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Market Value");
                dt.Columns.Add("Taxable Market");
                //dt.Columns.Add("Land Non-Residential");
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["Market Value"] = dt1.Rows[i]["Market Value"];
                    dt.Rows[i]["Taxable Market"] = dt1.Rows[i]["Taxable Market"];
                    //dt.Rows[i]["Land Non-Residential"] = dt1.Rows[i]["Land Non-Residential"];
                }
            }
            catch { }
            //dt.Columns["Year Built"].SetOrdinal(1);
            dataview = new DataView(dt);
        }
        return dataview;

    }

    public DataView getassprop_TXMcLennan(string strorder, string countyid)
    {
        //Property ID~Geographic ID~Type~Property Use Code~Property Use Description~Legal Description~Property Address~Neighborhood~Neighborhood CD~Owner Name~Mailing Address~Owner ID~% Ownership~Map ID~Exemptions~Year Built~Acres
        //Year~Improvements~Land Market~Ag Valuation~Appraised~HS Cap~Assessed
        DataTable dt = readdatafromcloud("select order_no, parcel_no, DVM.Data_Field_Text_Id, DVM.Data_Field_value from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 1 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        DataTable dt1 = readdatafromcloud("select order_no, parcel_no,DVM.Data_Field_value, DVM.Data_Field_Text_Id from data_value_master DVM join data_field_master DFM on DFM.ID = DVM.Data_Field_Text_Id join state_county_master SCM on SCM.ID = DFM.State_County_ID and SCM.State_County_Id = '" + countyid + "' where DFM.Category_Id = 4 and DVM.Order_No = '" + strorder + "' order by 1 limit 1");
        if (dt.Rows.Count > 0)
        {

            string[] selectedColumnsdt = new[] { "parcel_no", "Geographic ID", "Owner Name", "Year Built", "Property Address", "Legal Description" };
            dt = new DataView(dt).ToTable(false, selectedColumnsdt);
            try
            {
                string[] selectedColumnsdt1 = new[] { "Improvements", "Land Market" };
                dt1 = new DataView(dt1).ToTable(false, selectedColumnsdt1);

                dt.Columns.Add("Improvements");
                dt.Columns.Add("Land Market");
                //dt.Columns.Add("Land Non-Residential");
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["Improvements"] = dt1.Rows[i]["Improvements"];
                    dt.Rows[i]["Land Market"] = dt1.Rows[i]["Land Market"];
                    //dt.Rows[i]["Land Non-Residential"] = dt1.Rows[i]["Land Non-Residential"];
                }
            }
            catch { }
            //dt.Columns["Year Built"].SetOrdinal(1);
            dataview = new DataView(dt);
        }
        return dataview;

    }
    public void titleflex_details(string orderNumber, string parcelNumber, string address, string ownerName, string county, string state)
    {
        XmlDocument XD = new XmlDocument();
        XmlNode MESSAGE = XD.AppendChild(XD.CreateElement("MESSAGE"));
        XmlNode DEAL_SETS = MESSAGE.AppendChild(XD.CreateElement("DEAL_SETS"));
        XmlNode DEAL_SET = DEAL_SETS.AppendChild(XD.CreateElement("DEAL_SET"));
        XmlAttribute DEAL_SETChildAtt = DEAL_SET.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        DEAL_SETChildAtt.InnerText = "";
        XmlNode DEALS = DEAL_SET.AppendChild(XD.CreateElement("DEALS"));
        XmlNode DEAL = DEALS.AppendChild(XD.CreateElement("DEAL"));
        XmlAttribute DEAL_ChildAtt = DEAL.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        XmlAttribute DEAL_ChildAtt1 = DEAL.Attributes.Append(XD.CreateAttribute("MISMOReferenceModelIdentifier"));
        XmlAttribute DEAL_ChildAtt2 = DEAL.Attributes.Append(XD.CreateAttribute("MISMOLogicalDataDictionaryIdentifier"));
        DEAL_ChildAtt.InnerText = "1";
        DEAL_ChildAtt1.InnerText = "";
        DEAL_ChildAtt2.InnerText = "";

        #region parties
        XmlNode PARTIES = DEAL.AppendChild(XD.CreateElement("PARTIES"));

        #region PARTY1
        XmlNode PARTY1 = PARTIES.AppendChild(XD.CreateElement("PARTY"));
        XmlAttribute DPARTY1ChildAtt = PARTY1.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        DPARTY1ChildAtt.InnerText = "1";

        // INDIVIDUAL

        XmlNode INDIVIDUAL1 = PARTY1.AppendChild(XD.CreateElement("INDIVIDUAL"));
        XmlNode NAME1 = INDIVIDUAL1.AppendChild(XD.CreateElement("NAME"));

        XmlNode EducationalAchievementsDescription1 = NAME1.AppendChild(XD.CreateElement("EducationalAchievementsDescription"));

        XmlNode FirstName1 = NAME1.AppendChild(XD.CreateElement("FirstName"));
        // FirstName1.InnerText = txtfirstname.Text.Trim();

        XmlNode MiddleName1 = NAME1.AppendChild(XD.CreateElement("MiddleName"));
        ///  MiddleName1.InnerText = txtmiddlename.Text.Trim();

        XmlNode LastName1 = NAME1.AppendChild(XD.CreateElement("LastName"));
        LastName1.InnerText = ownerName.Trim();

        XmlNode FullName1 = NAME1.AppendChild(XD.CreateElement("FullName"));
        XmlNode PrefixName1 = NAME1.AppendChild(XD.CreateElement("PrefixName"));
        XmlNode SuffixName1 = NAME1.AppendChild(XD.CreateElement("SuffixName"));
        XmlNode EXTENSION1 = NAME1.AppendChild(XD.CreateElement("EXTENSION"));
        XmlNode IND1EXTENSION = INDIVIDUAL1.AppendChild(XD.CreateElement("EXTENSION"));

        //Adress

        XmlNode ADDRESSES1 = PARTY1.AppendChild(XD.CreateElement("ADDRESSES"));
        XmlNode ADDRESS1 = ADDRESSES1.AppendChild(XD.CreateElement("ADDRESS"));

        XmlAttribute ADDRESS1ChildAtt = ADDRESS1.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        ADDRESS1ChildAtt.InnerText = "";

        XmlNode AddressType1 = ADDRESS1.AppendChild(XD.CreateElement("AddressType"));
        AddressType1.InnerText = "Primary";
        XmlNode AddressLineText1 = ADDRESS1.AppendChild(XD.CreateElement("AddressLineText"));
        XmlAttribute AddressLineText1_ChildAtt1 = DEAL.Attributes.Append(XD.CreateAttribute("lang"));
        XmlAttribute AddressLineText1_ChildAtt2 = DEAL.Attributes.Append(XD.CreateAttribute("SensitiveIndicator"));
        AddressLineText1_ChildAtt1.InnerText = "";
        AddressLineText1_ChildAtt2.InnerText = "";
        AddressLineText1.InnerText = address.Trim();

        XmlNode AddressTypeOtherDescription1 = ADDRESS1.AppendChild(XD.CreateElement("AddressTypeOtherDescription"));
        XmlNode AddressUnitDesignatorType1 = ADDRESS1.AppendChild(XD.CreateElement("AddressUnitDesignatorType"));
        AddressUnitDesignatorType1.InnerText = "LOT";
        XmlNode AddressUnitDesignatorTypeOtherDescription1 = ADDRESS1.AppendChild(XD.CreateElement("AddressUnitDesignatorTypeOtherDescription"));
        XmlNode AddressUnitIdentifier1 = ADDRESS1.AppendChild(XD.CreateElement("AddressUnitIdentifier"));
        XmlNode CountryName1 = ADDRESS1.AppendChild(XD.CreateElement("CountryName"));
        XmlNode CountryCode1 = ADDRESS1.AppendChild(XD.CreateElement("CountryCode"));
        XmlNode StateName1 = ADDRESS1.AppendChild(XD.CreateElement("StateName"));
        XmlNode StateCode1 = ADDRESS1.AppendChild(XD.CreateElement("StateCode"));
        StateCode1.InnerText = state.Trim();
        XmlNode CountyName1 = ADDRESS1.AppendChild(XD.CreateElement("CountyName"));
        CountyName1.InnerText = county.Trim();
        XmlNode CountyCode1 = ADDRESS1.AppendChild(XD.CreateElement("CountyCode"));
        XmlNode AddressLineText12 = ADDRESS1.AppendChild(XD.CreateElement("AddressLineText"));
        XmlNode AddressAdditionalLineText1 = ADDRESS1.AppendChild(XD.CreateElement("AddressAdditionalLineText"));
        XmlNode CityName1 = ADDRESS1.AppendChild(XD.CreateElement("CityName"));
        //CityName1.InnerText = txtcity.Text.Trim();
        XmlNode PlusFourZipCode1 = ADDRESS1.AppendChild(XD.CreateElement("PlusFourZipCode"));
        XmlNode PostalCode1 = ADDRESS1.AppendChild(XD.CreateElement("PostalCode"));
        //  PostalCode1.InnerText = txtzip.Text.Trim();


        //Extension APN

        XmlNode exten = ADDRESS1.AppendChild(XD.CreateElement("EXTENSION"));

        //  XmlNode propid = exten.AppendChild(XD.CreateElement("PropertyID"));

        XmlNode legal = exten.AppendChild(XD.CreateElement("LEGAL_DESCRIPTIONS"));
        XmlAttribute legal_ChildAtt1 = legal.Attributes.Append(XD.CreateAttribute("title"));
        XmlAttribute legal_ChildAtt2 = legal.Attributes.Append(XD.CreateAttribute("role"));
        XmlAttribute legal_ChildAtt3 = legal.Attributes.Append(XD.CreateAttribute("label"));
        XmlAttribute legal_ChildAtt4 = legal.Attributes.Append(XD.CreateAttribute("type"));

        XmlNode legal1 = legal.AppendChild(XD.CreateElement("LEGAL_DESCRIPTION"));
        XmlAttribute legal1_ChildAtt1 = legal1.Attributes.Append(XD.CreateAttribute("title"));
        XmlAttribute legal1_ChildAtt2 = legal1.Attributes.Append(XD.CreateAttribute("role"));
        XmlAttribute legal1_ChildAtt3 = legal1.Attributes.Append(XD.CreateAttribute("label"));
        XmlAttribute legal1_ChildAtt4 = legal1.Attributes.Append(XD.CreateAttribute("type"));

        XmlNode parcelid1 = legal1.AppendChild(XD.CreateElement("PARCEL_IDENTIFICATIONS"));

        XmlNode parcelid2 = parcelid1.AppendChild(XD.CreateElement("PARCEL_IDENTIFICATION"));

        XmlNode ParcelIdentificationType = parcelid2.AppendChild(XD.CreateElement("ParcelIdentificationType"));
        ParcelIdentificationType.InnerText = "ParcelIdentificationNumber";

        XmlNode parceldescr = parcelid2.AppendChild(XD.CreateElement("ParcelIdentificationTypeOtherDescription"));
        XmlAttribute legal1parceldescr_ChildAtt1 = parceldescr.Attributes.Append(XD.CreateAttribute("lang"));
        XmlAttribute egal1lparceldescr_ChildAtt2 = parceldescr.Attributes.Append(XD.CreateAttribute("SensitiveIndicator"));

        XmlNode Parcelidenti = parcelid2.AppendChild(XD.CreateElement("ParcelIdentifier"));
        XmlAttribute Parcelidenti_ChildAtt1 = Parcelidenti.Attributes.Append(XD.CreateAttribute("SensitiveIndicator"));
        XmlAttribute Parcelidenti_ChildAtt2 = Parcelidenti.Attributes.Append(XD.CreateAttribute("IdentifierEffectiveDate"));
        XmlAttribute Parcelidenti_ChildAtt3 = Parcelidenti.Attributes.Append(XD.CreateAttribute("IdentifierOwnerURI"));
        Parcelidenti.InnerText = parcelNumber.Trim();


        // Roles

        XmlNode ROLES1 = PARTY1.AppendChild(XD.CreateElement("ROLES"));
        XmlNode ROLE1 = ROLES1.AppendChild(XD.CreateElement("ROLE"));

        XmlNode PROPERTY_OWNER = ROLE1.AppendChild(XD.CreateElement("PROPERTY_OWNER"));
        XmlNode PROPERTY_OWNER_EXTENSION = PROPERTY_OWNER.AppendChild(XD.CreateElement("EXTENSION"));

        XmlNode ROLE_DETAIL1 = ROLE1.AppendChild(XD.CreateElement("ROLE_DETAIL"));
        XmlNode ROLE_DETAIL1_PartyRoleType = ROLE_DETAIL1.AppendChild(XD.CreateElement("PartyRoleType"));
        ROLE_DETAIL1_PartyRoleType.InnerText = "PropertyOwner";

        XmlNode ROLE_DETAIL1_EXTENSION = ROLE1.AppendChild(XD.CreateElement("EXTENSION"));


        //XmlNode parcel = ROLE_DETAIL1_EXTENSION.AppendChild(XD.CreateElement("PARCEL_IDENTIFICATION"));
        //XmlNode parceltype = parcel.AppendChild(XD.CreateElement("ParcelIdentificationType"));
        //parceltype.InnerText = "ParcelIdentificationNumber";

        //XmlNode parcelid = parcel.AppendChild(XD.CreateElement("ParcelIdentifier"));
        //parcelid.InnerText = txtparcel.Text.Trim();


        #endregion

        #region PARTY2
        XmlNode PARTY2 = PARTIES.AppendChild(XD.CreateElement("PARTY"));
        XmlAttribute DPARTY2ChildAtt = PARTY2.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        DPARTY2ChildAtt.InnerText = "2";

        XmlNode INDIVIDUAL2 = PARTY2.AppendChild(XD.CreateElement("INDIVIDUAL"));
        XmlNode NAME2 = INDIVIDUAL2.AppendChild(XD.CreateElement("NAME"));

        XmlNode EducationalAchievementsDescription2 = NAME2.AppendChild(XD.CreateElement("EducationalAchievementsDescription"));
        XmlNode FirstName2 = NAME2.AppendChild(XD.CreateElement("FirstName"));
        XmlNode MiddleName2 = NAME2.AppendChild(XD.CreateElement("MiddleName"));
        XmlNode LastName2 = NAME2.AppendChild(XD.CreateElement("LastName"));
        XmlNode FullName2 = NAME2.AppendChild(XD.CreateElement("FullName"));
        XmlNode PrefixName2 = NAME2.AppendChild(XD.CreateElement("PrefixName"));
        XmlNode SuffixName2 = NAME2.AppendChild(XD.CreateElement("SuffixName"));
        XmlNode EXTENSION2 = NAME2.AppendChild(XD.CreateElement("EXTENSION"));

        XmlNode IND2EXTENSION = INDIVIDUAL2.AppendChild(XD.CreateElement("EXTENSION"));


        //Adress

        XmlNode ADDRESSES2 = PARTY2.AppendChild(XD.CreateElement("ADDRESSES"));
        XmlNode ADDRESS2 = ADDRESSES2.AppendChild(XD.CreateElement("ADDRESS"));

        XmlAttribute ADDRESS2ChildAtt = ADDRESS2.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        ADDRESS2ChildAtt.InnerText = "1";

        XmlNode AddressType2 = ADDRESS2.AppendChild(XD.CreateElement("AddressType"));
        AddressType2.InnerText = "Primary";
        XmlNode AddressTypeOtherDescription2 = ADDRESS2.AppendChild(XD.CreateElement("AddressTypeOtherDescription"));
        XmlNode AddressUnitDesignatorType2 = ADDRESS2.AppendChild(XD.CreateElement("AddressUnitDesignatorType"));
        AddressUnitDesignatorType2.InnerText = "LOT";
        XmlNode AddressUnitDesignatorTypeOtherDescription2 = ADDRESS2.AppendChild(XD.CreateElement("AddressUnitDesignatorTypeOtherDescription"));
        XmlNode AddressUnitIdentifier2 = ADDRESS2.AppendChild(XD.CreateElement("AddressUnitIdentifier"));
        XmlNode CountryName2 = ADDRESS2.AppendChild(XD.CreateElement("CountryName"));
        XmlNode CountryCode2 = ADDRESS2.AppendChild(XD.CreateElement("CountryCode"));
        XmlNode StateName2 = ADDRESS2.AppendChild(XD.CreateElement("StateName"));
        XmlNode StateCode2 = ADDRESS2.AppendChild(XD.CreateElement("StateCode"));
        XmlNode CountyName2 = ADDRESS2.AppendChild(XD.CreateElement("CountyName"));
        XmlNode CountyCode2 = ADDRESS2.AppendChild(XD.CreateElement("CountyCode"));
        XmlNode AddressLineText22 = ADDRESS2.AppendChild(XD.CreateElement("AddressLineText"));
        XmlNode AddressAdditionalLineText2 = ADDRESS2.AppendChild(XD.CreateElement("AddressAdditionalLineText"));
        XmlNode CityName2 = ADDRESS2.AppendChild(XD.CreateElement("CityName"));
        XmlNode PlusFourZipCode2 = ADDRESS2.AppendChild(XD.CreateElement("PlusFourZipCode"));
        XmlNode PostalCode2 = ADDRESS2.AppendChild(XD.CreateElement("PostalCode"));



        // Roles

        XmlNode ROLES2 = PARTY2.AppendChild(XD.CreateElement("ROLES"));
        XmlNode ROLE2 = ROLES2.AppendChild(XD.CreateElement("ROLE"));

        XmlNode SUBMITTING_PARTY = ROLE2.AppendChild(XD.CreateElement("SUBMITTING_PARTY"));
        XmlNode SubmittingPartySequenceNumber = SUBMITTING_PARTY.AppendChild(XD.CreateElement("SubmittingPartySequenceNumber"));
        SubmittingPartySequenceNumber.InnerText = "1";
        XmlNode SubmittingPartyTransactionIdentifier = SUBMITTING_PARTY.AppendChild(XD.CreateElement("SubmittingPartyTransactionIdentifier"));
        XmlNode SubmittingPartyEXTENSION = SUBMITTING_PARTY.AppendChild(XD.CreateElement("EXTENSION"));
        XmlNode SubmittingPartyLoginAccountIdentifier = SubmittingPartyEXTENSION.AppendChild(XD.CreateElement("LoginAccountIdentifier"));
        XmlNode SubmittingPartyLoginAccountPassword = SubmittingPartyEXTENSION.AppendChild(XD.CreateElement("LoginAccountPassword"));
        XmlNode ROLE_DETAIL2 = ROLE2.AppendChild(XD.CreateElement("ROLE_DETAIL"));
        XmlNode ROLE_DETAIL2_PartyRoleType = ROLE_DETAIL2.AppendChild(XD.CreateElement("PartyRoleType"));
        ROLE_DETAIL2_PartyRoleType.InnerText = "SubmittingParty";

        XmlNode ROLE_DETAIL2_EXTENSION = ROLE2.AppendChild(XD.CreateElement("EXTENSION"));


        #endregion

        #region PARTY3
        XmlNode PARTY3 = PARTIES.AppendChild(XD.CreateElement("PARTY"));
        XmlAttribute DPARTY3ChildAtt = PARTY3.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        DPARTY3ChildAtt.InnerText = "3";

        XmlNode INDIVIDUAL3 = PARTY3.AppendChild(XD.CreateElement("INDIVIDUAL"));
        XmlNode NAME3 = INDIVIDUAL3.AppendChild(XD.CreateElement("NAME"));

        XmlNode EducationalAchievementsDescription3 = NAME3.AppendChild(XD.CreateElement("EducationalAchievementsDescription"));
        XmlNode FirstName3 = NAME3.AppendChild(XD.CreateElement("FirstName"));
        XmlNode MiddleName3 = NAME3.AppendChild(XD.CreateElement("MiddleName"));
        XmlNode LastName3 = NAME3.AppendChild(XD.CreateElement("LastName"));
        XmlNode FullName3 = NAME3.AppendChild(XD.CreateElement("FullName"));
        XmlNode PrefixName3 = NAME3.AppendChild(XD.CreateElement("PrefixName"));
        XmlNode SuffixName3 = NAME3.AppendChild(XD.CreateElement("SuffixName"));
        XmlNode EXTENSION3 = NAME3.AppendChild(XD.CreateElement("EXTENSION"));

        XmlNode IND3EXTENSION = INDIVIDUAL3.AppendChild(XD.CreateElement("EXTENSION"));

        //Adress

        XmlNode ADDRESSES3 = PARTY3.AppendChild(XD.CreateElement("ADDRESSES"));
        XmlNode ADDRESS3 = ADDRESSES3.AppendChild(XD.CreateElement("ADDRESS"));

        XmlAttribute ADDRESS3ChildAtt = ADDRESS3.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        ADDRESS3ChildAtt.InnerText = "1";

        XmlNode AddressType3 = ADDRESS3.AppendChild(XD.CreateElement("AddressType"));
        AddressType3.InnerText = "Primary";
        XmlNode AddressTypeOtherDescription3 = ADDRESS3.AppendChild(XD.CreateElement("AddressTypeOtherDescription"));
        XmlNode AddressUnitDesignatorType3 = ADDRESS3.AppendChild(XD.CreateElement("AddressUnitDesignatorType"));
        AddressUnitDesignatorType3.InnerText = "LOT";
        XmlNode AddressUnitDesignatorTypeOtherDescription3 = ADDRESS3.AppendChild(XD.CreateElement("AddressUnitDesignatorTypeOtherDescription"));
        XmlNode AddressUnitIdentifier3 = ADDRESS3.AppendChild(XD.CreateElement("AddressUnitIdentifier"));
        XmlNode CountryName3 = ADDRESS3.AppendChild(XD.CreateElement("CountryName"));
        XmlNode CountryCode3 = ADDRESS3.AppendChild(XD.CreateElement("CountryCode"));
        XmlNode StateName3 = ADDRESS3.AppendChild(XD.CreateElement("StateName"));
        XmlNode StateCode3 = ADDRESS3.AppendChild(XD.CreateElement("StateCode"));
        XmlNode CountyName3 = ADDRESS3.AppendChild(XD.CreateElement("CountyName"));
        XmlNode CountyCode3 = ADDRESS3.AppendChild(XD.CreateElement("CountyCode"));
        XmlNode AddressLineText32 = ADDRESS3.AppendChild(XD.CreateElement("AddressLineText"));
        XmlNode AddressAdditionalLineText3 = ADDRESS3.AppendChild(XD.CreateElement("AddressAdditionalLineText"));
        XmlNode CityName3 = ADDRESS3.AppendChild(XD.CreateElement("CityName"));
        XmlNode PlusFourZipCode3 = ADDRESS3.AppendChild(XD.CreateElement("PlusFourZipCode"));
        XmlNode PostalCode3 = ADDRESS3.AppendChild(XD.CreateElement("PostalCode"));

        // Roles

        XmlNode ROLES3 = PARTY3.AppendChild(XD.CreateElement("ROLES"));
        XmlNode ROLE3 = ROLES3.AppendChild(XD.CreateElement("ROLE"));
        XmlNode REQUESTING_PARTY = ROLE3.AppendChild(XD.CreateElement("REQUESTING_PARTY"));
        XmlNode InternalAccountIdentifier = REQUESTING_PARTY.AppendChild(XD.CreateElement("InternalAccountIdentifier"));
        XmlNode RequestedByName = REQUESTING_PARTY.AppendChild(XD.CreateElement("RequestedByName"));
        XmlNode RequestingPartyBranchIdentifier = REQUESTING_PARTY.AppendChild(XD.CreateElement("RequestingPartyBranchIdentifier"));
        XmlNode RequestingPartySequenceNumber = REQUESTING_PARTY.AppendChild(XD.CreateElement("RequestingPartySequenceNumber"));
        RequestingPartySequenceNumber.InnerText = "1";
        XmlNode RequestingPartyEXTENSION = REQUESTING_PARTY.AppendChild(XD.CreateElement("EXTENSION"));
        XmlNode RequestingPartyLoginAccountIdentifier = RequestingPartyEXTENSION.AppendChild(XD.CreateElement("LoginAccountIdentifier"));
        RequestingPartyLoginAccountIdentifier.InnerText = "StringInfo";
        XmlNode RequestingPartyLoginAccountPassword = RequestingPartyEXTENSION.AppendChild(XD.CreateElement("LoginAccountPassword"));
        RequestingPartyLoginAccountPassword.InnerText = "StringXML1@";

        XmlNode ROLE_DETAIL3 = ROLE3.AppendChild(XD.CreateElement("ROLE_DETAIL"));
        XmlNode ROLE_DETAIL3_PartyRoleType = ROLE_DETAIL3.AppendChild(XD.CreateElement("PartyRoleType"));
        ROLE_DETAIL3_PartyRoleType.InnerText = "SubmittingParty";

        XmlNode ROLE_DETAIL3_EXTENSION = ROLE3.AppendChild(XD.CreateElement("EXTENSION"));

        #endregion

        #region PARTY4

        XmlNode PARTY4 = PARTIES.AppendChild(XD.CreateElement("PARTY"));
        XmlAttribute DPARTY4ChildAtt = PARTY4.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        DPARTY4ChildAtt.InnerText = "4";

        XmlNode INDIVIDUAL4 = PARTY4.AppendChild(XD.CreateElement("INDIVIDUAL"));
        XmlNode NAME4 = INDIVIDUAL4.AppendChild(XD.CreateElement("NAME"));

        XmlNode EducationalAchievementsDescription4 = NAME4.AppendChild(XD.CreateElement("EducationalAchievementsDescription"));
        XmlNode FirstName4 = NAME4.AppendChild(XD.CreateElement("FirstName"));
        XmlNode MiddleName4 = NAME4.AppendChild(XD.CreateElement("MiddleName"));
        XmlNode LastName4 = NAME4.AppendChild(XD.CreateElement("LastName"));
        XmlNode FullName4 = NAME4.AppendChild(XD.CreateElement("FullName"));
        XmlNode PrefixName4 = NAME4.AppendChild(XD.CreateElement("PrefixName"));
        XmlNode SuffixName4 = NAME4.AppendChild(XD.CreateElement("SuffixName"));
        XmlNode EXTENSION4 = NAME4.AppendChild(XD.CreateElement("EXTENSION"));

        XmlNode IND4EXTENSION = INDIVIDUAL4.AppendChild(XD.CreateElement("EXTENSION"));

        //Adress

        XmlNode ADDRESSES4 = PARTY4.AppendChild(XD.CreateElement("ADDRESSES"));
        XmlNode ADDRESS4 = ADDRESSES4.AppendChild(XD.CreateElement("ADDRESS"));

        XmlAttribute ADDRESS4ChildAtt = ADDRESS4.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        ADDRESS4ChildAtt.InnerText = "1";

        XmlNode AddressType4 = ADDRESS4.AppendChild(XD.CreateElement("AddressType"));
        AddressType4.InnerText = "Primary";
        XmlNode AddressTypeOtherDescription4 = ADDRESS4.AppendChild(XD.CreateElement("AddressTypeOtherDescription"));
        XmlNode AddressUnitDesignatorType4 = ADDRESS4.AppendChild(XD.CreateElement("AddressUnitDesignatorType"));
        AddressUnitDesignatorType4.InnerText = "LOT";
        XmlNode AddressUnitDesignatorTypeOtherDescription4 = ADDRESS4.AppendChild(XD.CreateElement("AddressUnitDesignatorTypeOtherDescription"));
        XmlNode AddressUnitIdentifier4 = ADDRESS4.AppendChild(XD.CreateElement("AddressUnitIdentifier"));
        XmlNode CountryName4 = ADDRESS4.AppendChild(XD.CreateElement("CountryName"));
        XmlNode CountryCode4 = ADDRESS4.AppendChild(XD.CreateElement("CountryCode"));
        XmlNode StateName4 = ADDRESS4.AppendChild(XD.CreateElement("StateName"));
        XmlNode StateCode4 = ADDRESS4.AppendChild(XD.CreateElement("StateCode"));
        XmlNode CountyName4 = ADDRESS4.AppendChild(XD.CreateElement("CountyName"));
        XmlNode CountyCode4 = ADDRESS4.AppendChild(XD.CreateElement("CountyCode"));
        XmlNode AddressLineText42 = ADDRESS4.AppendChild(XD.CreateElement("AddressLineText"));
        XmlNode AddressAdditionalLineText4 = ADDRESS4.AppendChild(XD.CreateElement("AddressAdditionalLineText"));
        XmlNode CityName4 = ADDRESS4.AppendChild(XD.CreateElement("CityName"));
        XmlNode PlusFourZipCode4 = ADDRESS4.AppendChild(XD.CreateElement("PlusFourZipCode"));
        XmlNode PostalCode4 = ADDRESS4.AppendChild(XD.CreateElement("PostalCode"));


        // Roles

        XmlNode ROLES4 = PARTY4.AppendChild(XD.CreateElement("ROLES"));
        XmlNode ROLE4 = ROLES4.AppendChild(XD.CreateElement("ROLE"));
        XmlNode RETURN_TO = ROLE4.AppendChild(XD.CreateElement("RETURN_TO"));
        XmlNode PREFERRED_RESPONSES = RETURN_TO.AppendChild(XD.CreateElement("PREFERRED_RESPONSES"));

        XmlNode PREFERRED_RESPONSES1 = PREFERRED_RESPONSES.AppendChild(XD.CreateElement("PREFERRED_RESPONSE"));
        XmlAttribute PREFERRED_RESPONSES1ChildAtt = PREFERRED_RESPONSES1.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        PREFERRED_RESPONSES1ChildAtt.InnerText = "";

        XmlNode PreferredResponseFormatType = PREFERRED_RESPONSES1.AppendChild(XD.CreateElement("PreferredResponseFormatType"));
        PreferredResponseFormatType.InnerText = "XML";
        XmlNode PreferredResponseMethodType = PREFERRED_RESPONSES1.AppendChild(XD.CreateElement("PreferredResponseMethodType"));
        PreferredResponseMethodType.InnerText = "HTTP";

        XmlNode ROLE_DETAIL4 = ROLE4.AppendChild(XD.CreateElement("ROLE_DETAIL"));
        XmlNode ROLE_DETAIL4_PartyRoleType = ROLE_DETAIL4.AppendChild(XD.CreateElement("PartyRoleType"));
        ROLE_DETAIL4_PartyRoleType.InnerText = "RespondToParty";

        XmlNode ROLE_DETAIL4_EXTENSION = ROLE4.AppendChild(XD.CreateElement("EXTENSION"));

        #endregion

        #endregion

        #region Services
        XmlNode SERVICES = DEAL.AppendChild(XD.CreateElement("SERVICES"));
        XmlNode SERVICE = SERVICES.AppendChild(XD.CreateElement("SERVICE"));
        XmlNode SERVICE_PRODUCT = SERVICE.AppendChild(XD.CreateElement("SERVICE_PRODUCT"));
        XmlNode SERVICE_PRODUCT_REQUEST = SERVICE_PRODUCT.AppendChild(XD.CreateElement("SERVICE_PRODUCT_REQUEST"));

        XmlNode SERVICE_PRODUCT_DETAIL = SERVICE_PRODUCT_REQUEST.AppendChild(XD.CreateElement("SERVICE_PRODUCT_DETAIL"));
        XmlNode ServiceProductDescription = SERVICE_PRODUCT_DETAIL.AppendChild(XD.CreateElement("ServiceProductDescription"));
        ServiceProductDescription.InnerText = "Property Information";
        XmlNode ServiceProductIdentifier = SERVICE_PRODUCT_DETAIL.AppendChild(XD.CreateElement("ServiceProductIdentifier"));
        ServiceProductIdentifier.InnerText = "PIB3";
        XmlNode SERVICESEXTENSION = SERVICE_PRODUCT_DETAIL.AppendChild(XD.CreateElement("EXTENSION"));
        XmlNode ServiceProductOperationType = SERVICESEXTENSION.AppendChild(XD.CreateElement("ServiceProductOperationType"));
        ServiceProductOperationType.InnerText = "Create";
        XmlNode ServiceProductNotesDescription = SERVICESEXTENSION.AppendChild(XD.CreateElement("ServiceProductNotesDescription"));
        XmlNode ServiceProductReportReturnType = SERVICESEXTENSION.AppendChild(XD.CreateElement("ServiceProductReportReturnType"));
        XmlNode ServiceProductImageReturnType = SERVICESEXTENSION.AppendChild(XD.CreateElement("ServiceProductImageReturnType"));
        XmlNode ServiceProductEmailDeliveryAdrs = SERVICESEXTENSION.AppendChild(XD.CreateElement("ServiceProductEmailDeliveryAdrs"));
        XmlNode SubjectLienRecordedDateRangeStartDate = SERVICESEXTENSION.AppendChild(XD.CreateElement("SubjectLienRecordedDateRangeStartDate"));
        XmlNode SubjectLienRecordedDateRangeEndDate = SERVICESEXTENSION.AppendChild(XD.CreateElement("SubjectLienRecordedDateRangeEndDate"));
        XmlNode NumberSubjectPropertiesType = SERVICESEXTENSION.AppendChild(XD.CreateElement("NumberSubjectPropertiesType"));
        NumberSubjectPropertiesType.InnerText = "25";

        XmlNode SERVICE_PRODUCT_REQUEST_EXTENSION = SERVICE_PRODUCT_REQUEST.AppendChild(XD.CreateElement("EXTENSION"));

        XmlNode SERVICE_PRODUCT_NAMES = SERVICE_PRODUCT_REQUEST.AppendChild(XD.CreateElement("SERVICE_PRODUCT_NAMES"));
        XmlNode SERVICE_PRODUCT_NAME = SERVICE_PRODUCT_REQUEST.AppendChild(XD.CreateElement("SERVICE_PRODUCT_NAME"));
        XmlAttribute SERVICE_PRODUCT_NAMEchildatt = SERVICE_PRODUCT_NAME.Attributes.Append(XD.CreateAttribute("SequenceNumber"));
        SERVICE_PRODUCT_NAMEchildatt.InnerText = "";

        XmlNode SERVICE_PRODUCT_NAME_DETAIL = SERVICE_PRODUCT_NAME.AppendChild(XD.CreateElement("SERVICE_PRODUCT_NAME_DETAIL"));
        XmlNode ServiceProductNameDescription = SERVICE_PRODUCT_NAME_DETAIL.AppendChild(XD.CreateElement("ServiceProductNameDescription"));
        XmlNode ServiceProductNameIdentifier = SERVICE_PRODUCT_NAME_DETAIL.AppendChild(XD.CreateElement("ServiceProductNameIdentifier"));

        XmlNode SERVICE_PRODUCT_NAME_DETAIL_EXTENSION = SERVICE_PRODUCT_NAME.AppendChild(XD.CreateElement("EXTENSION"));

        XmlNode SERVICE_PRODUCT_NAME_EXTENSION = SERVICE_PRODUCT_NAME.AppendChild(XD.CreateElement("EXTENSION"));

        #endregion
        if (!Directory.Exists(strInput))
        {
            DirectoryInfo di = Directory.CreateDirectory(strInput);
        }

        string filename = strInput + orderNumber.Trim() + ".xml";
        XD.Save(filename);

        postXMLData("https://xmldata.datatree.com/XmlPost/PlaceOrder", filename, orderNumber);
        readxml(orderNumber, parcelNumber, ownerName, address, state, county);
    }
    public string postXMLData(string destinationUrl, string requestXml, string orderNumber)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
        string strXml;
        using (StreamReader sr = new StreamReader(requestXml))
        {
            strXml = sr.ReadToEnd();
        }
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(strXml);
        byte[] bytes;
        bytes = System.Text.Encoding.ASCII.GetBytes(doc.InnerXml);
        request.ContentType = "text/xml; encoding='utf-8'";
        request.ContentLength = bytes.Length;
        request.Method = "POST";

        Stream requestStream = request.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        HttpWebResponse response;
        response = (HttpWebResponse)request.GetResponse();
        if (response.StatusCode == HttpStatusCode.OK)
        {
            Stream responseStream = response.GetResponseStream();
            string responseStr = new StreamReader(responseStream).ReadToEnd();
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(responseStr);

            if (!Directory.Exists(strOutput))
            {
                DirectoryInfo di = Directory.CreateDirectory(strOutput);
            }

            string strXml1 = strOutput + orderNumber + ".xml";
            xdoc.Save(strXml1);


            XmlDocument doc1 = new XmlDocument();
            doc1.Load(strXml1);
            XmlNodeList node = doc1.GetElementsByTagName("StatusDescription");
            string successinnertext = "";
            foreach (XmlNode nd in node)
            {
                successinnertext = nd.InnerText;
            }

        }
        return null;
    }
    public string readxml(string orderNumber, string parcelNumber, string ownerName, string straddress, string strstate, string strcounty)
    {
        string strXmlread = strOutput + orderNumber.Trim() + ".xml";

        XmlDocument docread = new XmlDocument();
        docread.Load(strXmlread);
        XmlNodeList node = docread.GetElementsByTagName("StatusDescription");
        string successinnertext = "";
        foreach (XmlNode nd in node)
        {
            successinnertext = nd.InnerText;
            break;
        }

        XmlNodeList nodep1type = docread.GetElementsByTagName("PARTY");
        string paddress = "", ownername = "", parcelno = "", exemptiondet = "", totalassvalue = "", taxamountrate = "", cityname = "", countyname = "", statename = "";
        string legal = "", assessedyear = "", taxyear = "", propertytax = "", landvalue = "", improvementvalue = "", statusmsg = "", yearBuiltValue = "", alternateAPN = "", tra = "";

        XmlNodeList names = docread.GetElementsByTagName("FullName");
        foreach (XmlNode name in names)
        {
            ownername = name.InnerText;
            //txtownername.Text = name.InnerText;
            break;
        }


        XmlNodeList sales = docread.GetElementsByTagName("SALES_HISTORIES");

        foreach (XmlNode sale in sales)
        {
            foreach (XmlNode chd in sale.ChildNodes)
            {
                foreach (XmlNode exten in chd.ChildNodes)
                {
                    //string sst = exten["TRANSACTION_HISTORY"].InnerXml;
                }
            }
        }

        //address
        XmlNodeList addresses = docread.GetElementsByTagName("AddressLineText");

        foreach (XmlNode address in addresses)
        {

            paddress = address.InnerText;
            if (paddress == "")
            {
                for (int i = 0; i < addresses.Count; i++)
                {
                    if (paddress == "")
                    {
                        paddress = addresses[i].InnerText;
                    }
                }
            }


            XmlNodeList cities = docread.GetElementsByTagName("CityName");

            foreach (XmlNode city in cities)
            {

                cityname = city.InnerText;
                if (cityname == "")
                {
                    for (int i = 0; i < cities.Count; i++)
                    {
                        if (cityname == "")
                        {
                            cityname = cities[i].InnerText;
                            // break;
                        }
                    }
                }
                break;

            }


            XmlNodeList counties = docread.GetElementsByTagName("CountyName");

            foreach (XmlNode county in counties)
            {

                countyname = county.InnerText;
                if (countyname == "")
                {
                    for (int i = 0; i < counties.Count; i++)
                    {
                        if (countyname == "")
                        {
                            countyname = counties[i].InnerText;
                            //break;
                        }
                    }
                }
                // break;
            }

            XmlNodeList states = docread.GetElementsByTagName("StateCode");

            foreach (XmlNode state in states)
            {
                statename = state.InnerText;
                if (statename == "")
                {
                    for (int i = 0; i < states.Count; i++)
                    {
                        if (statename == "")
                        {
                            statename = states[i].InnerText;
                            // break;
                        }
                    }
                }
                break;
            }
            if (cityname.ToLower() == countyname.ToLower())
            {
                paddress = paddress + ",\r\n" + countyname + ",\r\n" + statename;
            }
            else
            {
                paddress = paddress + ",\r\n" + cityname + ",\r\n" + countyname + ",\r\n" + statename;
            }
            //txtpaddress.Text = paddress;
            break;

        }

        //parcelid
        XmlNodeList parcels = docread.GetElementsByTagName("ParcelIdentifier");

        foreach (XmlNode parcel in parcels)
        {

            parcelno = parcel.InnerText;
            // txtparcelid.Text = parcel.InnerText;
            break;
        }

        //exemptions
        XmlNodeList exemptions = docread.GetElementsByTagName("PROPERTY_TAX_EXEMPTIONS");

        foreach (XmlNode exemption in exemptions)
        {
            exemptiondet = exemption.InnerText;
            if (exemptiondet.Trim() == "")
            {
                for (int i = 0; i < exemptions.Count; i++)
                {
                    if (exemptiondet == "")
                    {
                        exemptiondet = exemptions[i].InnerText;
                        // break;
                    }
                }
            }
            break;
        }

        //totalassessedvalue
        XmlNodeList tavs = docread.GetElementsByTagName("PropertyTaxTotalAssessedValueAmount");

        foreach (XmlNode tav in tavs)
        {
            totalassvalue = tav.InnerText;
            if (totalassvalue.Trim() == "")
            {
                for (int i = 0; i < tavs.Count; i++)
                {
                    if (totalassvalue == "")
                    {
                        totalassvalue = tavs[i].InnerText;
                        //break;
                    }
                }
            }
            break;
        }

        //totaltaxvalue
        XmlNodeList taxamounts = docread.GetElementsByTagName("PropertyTaxTotalTaxAmount");
        foreach (XmlNode taxamount in taxamounts)
        {
            taxamountrate = taxamount.InnerText;
            if (taxamountrate.Trim() == "")
            {
                for (int i = 0; i < taxamounts.Count; i++)
                {
                    if (taxamountrate == "")
                    {
                        taxamountrate = taxamounts[i].InnerText;
                        //break;
                    }
                }
            }
            break;
        }


        //Legal Description
        XmlNodeList leagal = docread.GetElementsByTagName("UnparsedLegalDescription");
        foreach (XmlNode lea in leagal)
        {
            legal = lea.InnerText;
            if (legal.Trim() == "")
            {
                for (int i = 0; i < leagal.Count; i++)
                {
                    if (legal == "")
                    {
                        legal = leagal[i].InnerText;
                        //break;
                    }
                }
            }
            break;
        }


        //Assement Year
        XmlNodeList year = docread.GetElementsByTagName("PropertyTaxAssessmentEndYear");
        foreach (XmlNode ye in year)
        {
            assessedyear = ye.InnerText;
            if (assessedyear.Trim() == "")
            {
                for (int i = 0; i < year.Count; i++)
                {
                    if (assessedyear == "")
                    {
                        assessedyear = year[i].InnerText;
                        //break;
                    }
                }
            }
            break;
        }

        //Tax Year
        XmlNodeList taxy = docread.GetElementsByTagName("PropertyTaxYearIdentifier");
        foreach (XmlNode tax in taxy)
        {
            taxyear = tax.InnerText;
            if (taxyear.Trim() == "")
            {
                for (int i = 0; i < taxy.Count; i++)
                {
                    if (taxyear == "")
                    {
                        taxyear = taxy[i].InnerText;
                        // break;
                    }
                }
            }
            break;
        }


        //Property Tax
        XmlNodeList propertytax1 = docread.GetElementsByTagName("PropertyTaxTotalTaxAmount");
        foreach (XmlNode pro in propertytax1)
        {
            propertytax = pro.InnerText;
            if (propertytax.Trim() == "")
            {
                for (int i = 0; i < propertytax1.Count; i++)
                {
                    if (propertytax == "")
                    {
                        propertytax = propertytax1[i].InnerText;
                        //break;
                    }
                }
            }
            break;
        }


        //Land value
        XmlNodeList land = docread.GetElementsByTagName("PropertyTaxLandValueAmount");
        foreach (XmlNode la in land)
        {
            landvalue = la.InnerText;
            if (landvalue.Trim() == "")
            {
                for (int i = 0; i < land.Count; i++)
                {
                    if (landvalue == "")
                    {
                        landvalue = land[i].InnerText;
                        // break;
                    }
                }
            }
            break;
        }


        //Improvement value
        XmlNodeList improvement = docread.GetElementsByTagName("PropertyTaxImprovementValueAmount");
        foreach (XmlNode imp in improvement)
        {
            improvementvalue = imp.InnerText;
            if (improvementvalue.Trim() == "")
            {
                for (int i = 0; i < improvement.Count; i++)
                {
                    if (improvementvalue == "")
                    {
                        improvementvalue = improvement[i].InnerText;
                        // break;
                    }
                }
            }
            break;
        }

        //Status Code
        XmlNodeList statuscode = docread.GetElementsByTagName("StatusCode");
        foreach (XmlNode suc in statuscode)
        {
            statusmsg = suc.InnerText;
            break;
        }

        //Year Built...
        XmlNodeList yearBuilt = docread.GetElementsByTagName("PropertyStructureBuiltYear");
        foreach (XmlNode yrb in yearBuilt)
        {
            yearBuiltValue = yrb.InnerText;
            if (yearBuiltValue.Trim() == "")
            {
                for (int i = 0; i < yearBuilt.Count; i++)
                {
                    if (yearBuiltValue == "")
                    {
                        yearBuiltValue = yearBuilt[i].InnerText;
                        //break;
                    }
                }
            }
            break;
        }

        //alternateAPN
        XmlNodeList alterAPN = docread.GetElementsByTagName("ParcelIdentifier");
        foreach (XmlNode alt in alterAPN)
        {
            alternateAPN = alt.InnerText;
            if (alternateAPN.Trim() == "")
            {
                for (int i = 0; i < alterAPN.Count; i++)
                {
                    if (alternateAPN == "")
                    {
                        alternateAPN = alterAPN[i].InnerText;
                        //break;
                    }
                }
            }
            if (alternateAPN.Trim() != "")
            {
                string stralternateAPN = "";
                for (int i = 0; i < alterAPN.Count; i++)
                {
                    stralternateAPN = alterAPN[i].InnerText;
                    if (alternateAPN != stralternateAPN)
                    {
                        alternateAPN = stralternateAPN;
                    }
                }
            }
            break;
        }

        //for IN marion parcel id
        if (strstate == "IN" && strcounty == "Marion")
        {
            foreach (XmlNode parcel in parcels)
            {
                parcelno = parcel.InnerText;
            }
        }

        //Effect Year Built...
        XmlNodeList EffectyearBuilt = docread.GetElementsByTagName("PropertyEffectiveBuiltYear");
        foreach (XmlNode eyrb in EffectyearBuilt)
        {
            yearBuiltValue = eyrb.InnerText;
            if (yearBuiltValue.Trim() == "")
            {
                for (int i = 0; i < EffectyearBuilt.Count; i++)
                {
                    if (yearBuiltValue == "")
                    {
                        yearBuiltValue = EffectyearBuilt[i].InnerText;
                        //break;
                    }
                }
            }
            break;
        }

        //TRA
        XmlNodeList tRA = docread.GetElementsByTagName("PropertyTaxCountyRateAreaIdentifier");
        foreach (XmlNode tr in tRA)
        {
            tra = tr.InnerText;
            if (tra.Trim() == "")
            {
                for (int i = 0; i < tRA.Count; i++)
                {
                    if (tra == "")
                    {
                        tra = tRA[i].InnerText;
                        //break;
                    }
                }
            }
            break;
        }




        //Francis
        XmlNodeList multi = docread.GetElementsByTagName("StatusDescription");

        XmlNodeList parcelid = docread.GetElementsByTagName("ParcelIdentifier");

        XmlNodeList multinames = docread.GetElementsByTagName("FullName");

        XmlNodeList xAddress = docread.GetElementsByTagName("AddressLineText");


        List<string> multiName = new List<string>();
        List<string> multiaddress = new List<string>();
        for (int i = 0; i < multinames.Count; i++)
        {
            string owner_Name = multinames[i].InnerText;
            if (owner_Name != "")
            {
                multiName.Add(owner_Name);
            }
        }
        for (int i = 0; i < xAddress.Count; i++)
        {
            string address = xAddress[i].InnerText;
            if (address != "")
            {
                multiaddress.Add(address);
            }
        }
        int k = 0;
        for (int i = 0; i < multi.Count; i++)
        {
            if (multi[i].InnerText.Length > 0)
            {
                string multiParcel = multi[i].InnerText;

                DataSet dsbind = new DataSet();
                if (multiParcel == "MULTIPLE PROPERTIES FOUND")
                {

                    for (int j = 0; j < parcelid.Count; j++)
                    {
                        if (parcelid[j].InnerText.Length > 0)
                        {
                            string TitleFlex = multiaddress[k] + "~" + multiName[k] + "~" + countyname + "~" + cityname + "~" + statename;
                            //gc.insert_date(orderNumber, parcelid[j].InnerText, 262, TitleFlex, 1, DateTime.Now);
                            k++;
                            //DataSet ds = new DataSet();
                            //string multiquery = "insert into multiparcels (orderno, address, county, city, state, zipcode, parcelno,ownername)  values ('" + orderNumber + "','" + multiaddress[k] + "','" + countyname + "','" + cityname + "','" + statename + "',' ','" + parcelid[j].InnerText + "','" + multiName[k] + "')";
                            //ds = newcon.ExecuteQuery(multiquery);
                            //k++;
                            //string bindquery = "select orderno,parcelno,ownername,address,county,state from multiparcels where orderno = '" + orderNumber + "' group by parcelno";
                            //dsbind = newcon.ExecuteQuery(bindquery);

                            HttpContext.Current.Session["TitleFlex_Search"] = "Yes";
                        }
                    }
                }
                else
                {
                    if (multiName.Count != 0)
                    {

                        string TitleFlex = paddress.Replace("\r\n", " ") + "~" + ownername.Replace("\r\n", " ") + "~" + countyname + "~" + cityname + "~" + statename;
                        // gc.insert_date(orderNumber, parcelno, 262, TitleFlex, 1, DateTime.Now);

                        string TitleFlex_Property = ownername.Replace("\r\n", " ") + "~" + paddress.Replace("\r\n", " ") + "~" + legal.Replace("\r\n", " ") + "~" + yearBuiltValue + "~" + taxyear + "~" + assessedyear + "~" + landvalue + "~" + improvementvalue + "~" + totalassvalue + "~" + taxamountrate + "~" + exemptiondet + "~" + alternateAPN + "~" + yearBuiltValue + "~" + tra;
                        HttpContext.Current.Session["TitleFlex_Property"] = TitleFlex_Property;
                        //gc.insert_date(orderNumber, parcelno, 567, TitleFlex, 1, DateTime.Now);

                        DataSet ds = new DataSet();
                        string insert = "insert into titleflex (OrderNo, ParcelNo, OwnerName, Address, LegalDescription, YearBuilt, TaxYear,AssessedYear,LandValue,ImproveValue,TotalAssessed,TotalTax,Exemption,AlternateAPN,EffectiveYearBuilt,TRA)  values ('" + orderNumber + "', '" + parcelno + "','" + ownername.Replace("\r\n", " ") + "','" + paddress.Replace("\r\n", " ") + "' ,'" + legal.Replace("\r\n", " ") + "','" + yearBuiltValue + "','" + taxyear + "','" + assessedyear + "','" + landvalue + "','" + improvementvalue + "', '" + totalassvalue + "' , '" + taxamountrate + "' , '" + exemptiondet + "' ,'" + alternateAPN + "' ,'" + yearBuiltValue + "' ,'" + tra + "' )";
                        ds = con.ExecuteQuery(insert);
                        //Parcel empty
                        if (alternateAPN != "" && tra != "")
                        {
                            HttpContext.Current.Session["titleflex_alternateAPN"] = tra.Trim() + alternateAPN;
                        }

                        //OwnerName~Address~Legal Discription~Year Built~Tax Year~Assessed Year~Land Value~Improve Value~Total Assessed~Total Tax~Exemption~Alternate APN~Effetive Year Built~TRA
                    }
                }

            }
        }
        HttpContext.Current.Session["titleparcel"] = parcelno;
        return parcelno;
    }

    public string ReturnStType(string name)
    {
        MySqlConnection ftp = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString);
        ftp.Open();
        MySqlCommand cmd = new MySqlCommand("SELECT short_name from street_type WHERE name = '" + name.ToUpper() + "'", ftp);
        MySqlDataReader reader = cmd.ExecuteReader();
        String result = null;
        while (reader.Read())
        {
            result = reader["short_name"].ToString();
        }
        reader.Close();
        ftp.Close();
        return result;
    }
    public DataTable ReturnDtAPI(string qry)
    {
        string data = DtConvert.ReadDataFROMCloud(qry);
        IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(data);
        DataTable dt = DtConvert.ToDataTable(UserList);
        return dt;
    }
    public DataTable readdatafromcloud(string Query)
    {
        DataTable dt = ReturnDtAPI(Query);
        DataTable dTable = new DataTable();
        if (dt.Rows.Count > 0)
        {
            string data_text_id = dt.Rows[0]["Data_Field_Text_Id"].ToString();
            string order_no = dt.Rows[0]["order_no"].ToString();
            DataTable dtfield = ReturnDtAPI("select Data_Fields_Text from data_field_master where id='" + data_text_id + "'");
            string columnName = "order_no" + "~" + "parcel_no" + "~" + dtfield.Rows[0]["Data_Fields_Text"].ToString();
            string[] columnArray = columnName.Split('~');

            foreach (string cName in columnArray)
            {
                dTable.Columns.Add(cName);
            }
            DataRow dr = dTable.NewRow();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dTable.Rows.Add();
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string rowvals = order_no + "~" + dt.Rows[i]["parcel_no"].ToString() + "~" + dt.Rows[i]["Data_Field_value"].ToString();
                string[] rowValue = rowvals.Split('~');
                for (int k = 0; k < rowValue.Count(); k++)
                {
                    dTable.Rows[i][k] = rowValue[k];
                }
            }
        }

        return dTable;

    }
    public DataTable readdatafromcloudAB(string Query)
    {
        DataTable dt = ReturnDtAPIAB(Query);
        DataTable dTable = new DataTable();
        if (dt.Rows.Count > 0)
        {
            string data_text_id = dt.Rows[0]["Data_Field_Text_Id"].ToString();
            string order_no = dt.Rows[0]["order_no"].ToString();
            DataTable dtfield = ReturnDtAPIAB("select Data_Fields_Text from data_field_master where id='" + data_text_id + "'");
            string columnName = "order_no" + "~" + "parcel_no" + "~" + dtfield.Rows[0]["Data_Fields_Text"].ToString();
            string[] columnArray = columnName.Split('~');

            foreach (string cName in columnArray)
            {
                dTable.Columns.Add(cName);
            }
            DataRow dr = dTable.NewRow();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dTable.Rows.Add();
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string rowvals = order_no + "~" + dt.Rows[i]["parcel_no"].ToString() + "~" + dt.Rows[i]["Data_Field_value"].ToString();
                string[] rowValue = rowvals.Split('~');
                for (int k = 0; k < rowValue.Count(); k++)
                {
                    dTable.Rows[i][k] = rowValue[k];
                }
            }
        }

        return dTable;

    }
    //multi parcel
    public DataTable readdatafromcloudABM(string Query)
    {
        DataTable dt = ReturnDtAPIAB(Query);
        DataTable dTable = new DataTable();
        if (dt.Rows.Count > 0)
        {
            string data_text_id = dt.Rows[0]["Data_Field_Text_Id"].ToString();
            string order_no = dt.Rows[0]["order_no"].ToString();
            DataTable dtfield = ReturnDtAPIAB("select Data_Fields_Text from data_field_master where id='" + data_text_id + "'");
            string columnName = "Order_Number" + "~" + dtfield.Rows[0]["Data_Fields_Text"].ToString();
            string[] columnArray = columnName.Split('~');

            foreach (string cName in columnArray)
            {
                dTable.Columns.Add(cName);
            }
            DataRow dr = dTable.NewRow();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dTable.Rows.Add();
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string rowvals = order_no + "~" + dt.Rows[i]["Data_Field_value"].ToString();
                string[] rowValue = rowvals.Split('~');
                for (int k = 0; k < rowValue.Count(); k++)
                {
                    dTable.Rows[i][k] = rowValue[k];
                }
            }
        }

        return dTable;

    }
    public DataTable ReturnDtAPIAB(string qry)
    {
        string data = DtConvert.ReadDataFROMCloudAB(qry);
        IList<Testbind> UserList = JsonConvert.DeserializeObject<IList<Testbind>>(data);
        DataTable dt = DtConvert.ToDataTable(UserList);
        return dt;
    }
    public DataTable GridDisplay(string Query)
    {
        DataSet ds = db.ExecuteQuery(Query);
        DataTable dTable = new DataTable();
        if (ds.Tables[0].Rows.Count > 0)
        {
            string data_text_id = ds.Tables[0].Rows[0]["Data_Field_Text_Id"].ToString();
            string order_no = ds.Tables[0].Rows[0]["order_no"].ToString();
            DataSet dsField = db.ExecuteQuery("select Data_Fields_Text from data_field_master where id='" + data_text_id + "'");
            string columnName = "order_no" + "~" + "parcel_no" + "~" + dsField.Tables[0].Rows[0]["Data_Fields_Text"].ToString();
            string[] columnArray = columnName.Split('~');

            foreach (string cName in columnArray)
            {
                dTable.Columns.Add(cName);
            }
            DataRow dr = dTable.NewRow();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dTable.Rows.Add();
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string rowvals = order_no + "~" + ds.Tables[0].Rows[i]["parcel_no"].ToString() + "~" + ds.Tables[0].Rows[i]["Data_Field_value"].ToString();
                string[] rowValue = rowvals.Split('~');
                for (int k = 0; k < rowValue.Count(); k++)
                {
                    dTable.Rows[i][k] = rowValue[k];

                }
            }
        }

        return dTable;

    }
    public string ReturnTeam(string state, string county)
    {
        MySqlConnection ftp = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString);
        ftp.Open();
        MySqlCommand cmd = new MySqlCommand("select Team from state_county_master where state_name='" + state + "' and county_name='" + county + "'", ftp);
        MySqlDataReader reader = cmd.ExecuteReader();
        string team = null;
        while (reader.Read())
        {
            team = reader["Team"].ToString();
        }
        reader.Close();
        ftp.Close();
        return team;
    }

    public DataTable MultiGridDisplay(string orderno, string state, string county)
    {
        DataTable multiDT = new DataTable();
        string team = ReturnTeam(state, county);
        if (team == "Internal")
        {
            multiDT = readdatafromcloud("select * from data_value_master where order_no='" + orderno + "'");

        }
        else if (team == "External")
        {
            multiDT = readdatafromcloudABM("select * from data_value_master where order_no='" + orderno + "'");
        }
        return multiDT;

    }
    public class Testbind
    {
        public string Order_No { get; set; }
        public string Parcel_no { get; set; }
        public string Data_Field_Text_Id { get; set; }
        public string Data_Field_value { get; set; }
        public string Data_Fields_Text { get; set; }
    }

    public int InsertTaxData(string OrderNo, string TaxType, string TaxYear, string EndYear, string ParcelId, string delinquentstatus, string SpecilAsst, string txBill, string OrderComments,
                               string TaxPayStatus, int payfreq, int firstpaid, int firstdue, string TaxAmount, string TaxDiscountAmount,
                               string TaxAmountPaid, string RemainingBalance, string ExemptionStatus,
                               int secoundpaid, int secounddue, string TaxAmount1, string TaxDiscountAmount1, string TaxAmountPaid1,
                               string RemainingBalance1, string ExemptionStatus1, int thirdpaid, int thirddue, string TaxAmount2,
                               string TaxDiscountAmount2, string TaxAmountPaid2, string RemainingBalance2, string ExemptionStatus2,
                               int fourthpaid, int fourthdue, string TaxAmount3, string TaxDiscountAmount3, string TaxAmountPaid3,
                               string RemainingBalance3, string ExemptionStatus3, string Description, string SpecialAssessNo, string Noinstallment, string InstallmentPaid, string Amount,
                               string DelinquentTaxYear, string PayoffAmount, string PayoffGoodThruDate, string InitialInstallmentDueDate,
                               string NotApplicable, string DateofTaxSale, string LastDayToRedeem, string atype, string process, string Item_ID, string Last_Updated_By)

    {
        mParam = new MySqlParameter[55];
        mParam[0] = new MySqlParameter("?$Orderno", OrderNo.Trim());
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$TaxType", TaxType);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$TaxYear", TaxYear);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$EndYear", EndYear);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$ParcelId", ParcelId);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$DelinquentStatus", delinquentstatus);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$SpecialAssessment", SpecilAsst);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$TaxBill", txBill);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("?$OrderComments", OrderComments);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        mParam[9] = new MySqlParameter("?$TaxPayStatus", TaxPayStatus);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;

        mParam[10] = new MySqlParameter("?$payfreq", payfreq);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;

        mParam[11] = new MySqlParameter("?$firstpaid", firstpaid);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;

        mParam[12] = new MySqlParameter("?$firstdue", firstdue);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;

        mParam[13] = new MySqlParameter("?$TaxAmount", TaxAmount);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;

        mParam[14] = new MySqlParameter("?$TaxDiscountAmount", TaxDiscountAmount);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;

        mParam[15] = new MySqlParameter("?$TaxAmountPaid", TaxAmountPaid);
        mParam[15].MySqlDbType = MySqlDbType.VarChar;

        mParam[16] = new MySqlParameter("?$RemainingBalance", RemainingBalance);
        mParam[16].MySqlDbType = MySqlDbType.VarChar;

        mParam[17] = new MySqlParameter("?$ExemptionStatus", ExemptionStatus);
        mParam[17].MySqlDbType = MySqlDbType.VarChar;

        mParam[18] = new MySqlParameter("?$secoundpaid", secoundpaid);
        mParam[18].MySqlDbType = MySqlDbType.VarChar;

        mParam[19] = new MySqlParameter("?$secounddue", secounddue);
        mParam[19].MySqlDbType = MySqlDbType.VarChar;

        mParam[20] = new MySqlParameter("?$TaxAmount1", TaxAmount1);
        mParam[20].MySqlDbType = MySqlDbType.VarChar;

        mParam[21] = new MySqlParameter("?$TaxDiscountAmount1", TaxDiscountAmount1);
        mParam[21].MySqlDbType = MySqlDbType.VarChar;

        mParam[22] = new MySqlParameter("?$TaxAmountPaid1", TaxAmountPaid1);
        mParam[22].MySqlDbType = MySqlDbType.VarChar;

        mParam[23] = new MySqlParameter("?$RemainingBalance1", RemainingBalance1);
        mParam[23].MySqlDbType = MySqlDbType.VarChar;

        mParam[24] = new MySqlParameter("?$ExemptionStatus1", ExemptionStatus1);
        mParam[24].MySqlDbType = MySqlDbType.VarChar;

        mParam[25] = new MySqlParameter("?$thirdpaid", thirdpaid);
        mParam[25].MySqlDbType = MySqlDbType.VarChar;

        mParam[26] = new MySqlParameter("?$thirddue", thirddue);
        mParam[26].MySqlDbType = MySqlDbType.VarChar;

        mParam[27] = new MySqlParameter("?$TaxAmount2", TaxAmount2);
        mParam[27].MySqlDbType = MySqlDbType.VarChar;

        mParam[28] = new MySqlParameter("?$TaxDiscountAmount2", TaxDiscountAmount2);
        mParam[28].MySqlDbType = MySqlDbType.VarChar;

        mParam[29] = new MySqlParameter("?$TaxAmountPaid2", TaxAmountPaid2);
        mParam[29].MySqlDbType = MySqlDbType.VarChar;

        mParam[30] = new MySqlParameter("?$RemainingBalance2", RemainingBalance2);
        mParam[30].MySqlDbType = MySqlDbType.VarChar;

        mParam[31] = new MySqlParameter("?$ExemptionStatus2", ExemptionStatus2);
        mParam[31].MySqlDbType = MySqlDbType.VarChar;

        mParam[32] = new MySqlParameter("?$fourthpaid", fourthpaid);
        mParam[32].MySqlDbType = MySqlDbType.VarChar;

        mParam[33] = new MySqlParameter("?$fourthdue", fourthdue);
        mParam[33].MySqlDbType = MySqlDbType.VarChar;

        mParam[34] = new MySqlParameter("?$TaxAmount3", TaxAmount3);
        mParam[34].MySqlDbType = MySqlDbType.VarChar;

        mParam[35] = new MySqlParameter("?$TaxDiscountAmount3", TaxDiscountAmount3);
        mParam[35].MySqlDbType = MySqlDbType.VarChar;

        mParam[36] = new MySqlParameter("?$TaxAmountPaid3", TaxAmountPaid3);
        mParam[36].MySqlDbType = MySqlDbType.VarChar;

        mParam[37] = new MySqlParameter("?$RemainingBalance3", RemainingBalance3);
        mParam[37].MySqlDbType = MySqlDbType.VarChar;

        mParam[38] = new MySqlParameter("?$ExemptionStatus3", ExemptionStatus3);
        mParam[38].MySqlDbType = MySqlDbType.VarChar;

        mParam[39] = new MySqlParameter("?$Description", Description);
        mParam[39].MySqlDbType = MySqlDbType.VarChar;

        mParam[40] = new MySqlParameter("?$SpecialAssessNo", SpecialAssessNo);
        mParam[40].MySqlDbType = MySqlDbType.VarChar;

        mParam[41] = new MySqlParameter("?$Noinstallment", Noinstallment);
        mParam[41].MySqlDbType = MySqlDbType.VarChar;

        mParam[42] = new MySqlParameter("?$InstallmentPaid", InstallmentPaid);
        mParam[42].MySqlDbType = MySqlDbType.VarChar;

        mParam[43] = new MySqlParameter("?$Amount", Amount);
        mParam[43].MySqlDbType = MySqlDbType.VarChar;

        mParam[44] = new MySqlParameter("?$DelinquentTaxYear", DelinquentTaxYear);
        mParam[44].MySqlDbType = MySqlDbType.VarChar;

        mParam[45] = new MySqlParameter("?$PayoffAmount", PayoffAmount);
        mParam[45].MySqlDbType = MySqlDbType.VarChar;

        mParam[46] = new MySqlParameter("?$PayoffGoodThruDate", PayoffGoodThruDate);
        mParam[46].MySqlDbType = MySqlDbType.VarChar;

        mParam[47] = new MySqlParameter("?$InitialInstallmentDueDate", InitialInstallmentDueDate);
        mParam[47].MySqlDbType = MySqlDbType.VarChar;

        mParam[48] = new MySqlParameter("?$NotApplicable", NotApplicable);
        mParam[48].MySqlDbType = MySqlDbType.VarChar;

        mParam[49] = new MySqlParameter("?$DateofTaxSale", DateofTaxSale);
        mParam[49].MySqlDbType = MySqlDbType.VarChar;

        mParam[50] = new MySqlParameter("?$LastDayToRedeem", LastDayToRedeem);
        mParam[50].MySqlDbType = MySqlDbType.VarChar;

        mParam[51] = new MySqlParameter("?$Atype", atype);
        mParam[51].MySqlDbType = MySqlDbType.VarChar;

        mParam[52] = new MySqlParameter("?$Process", process);
        mParam[52].MySqlDbType = MySqlDbType.VarChar;

        mParam[53] = new MySqlParameter("?$ID", Item_ID);
        mParam[53].MySqlDbType = MySqlDbType.VarChar;

        mParam[54] = new MySqlParameter("?$Last_Updated_By", Last_Updated_By);
        mParam[54].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("sp_insert_tax", true, mParam);
    }

    public int DeleteGrid(string id, string processname, string gname)
    {
        try
        {
            mParam = new MySqlParameter[3];

            mParam[0] = new MySqlParameter("?$ID", id);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

            mParam[1] = new MySqlParameter("?$Processname", processname);
            mParam[1].MySqlDbType = MySqlDbType.VarChar;

            mParam[2] = new MySqlParameter("?$GName", gname);
            mParam[2].MySqlDbType = MySqlDbType.VarChar;

            return ExecuteSPNonQuery("sp_delete_grid", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable FetchSearch_Keying(string ordernumber)
    {
        DataTable dt = new DataTable();
        string query = "Sp_Search_keying";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$ordernumber", ordernumber);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }
    public DataTable FetchClient_Data(string ordernumber)
    {
        DataTable dt = new DataTable();
        string query = "Sp_Client_Tool_Data";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$ordernumber", ordernumber);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

    public DataTable FetchMismatch_Data(string ordernumber)
    {
        DataTable dt = new DataTable();
        string query = "Sp_mismatchdatadetails";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$ordernumber", ordernumber);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

    //Amrock
    //Amrock Production      


    public DataTable FetchOrderDetailsnew(string ordernumber)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetchorderdetails";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$orderno", ordernumber);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }
    public DataTable FetchOrderDetails()
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetchorderdetails";
        mDa = con.ExecuteSPAdapter1(query);
        mDa.Fill(dt);
        return dt;
    }

    public int insert_taxcertinfo(string orderno, string orderstatus, string comments, string createddate, string enteredby)
    {
        mParam = new MySqlParameter[5];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$orderstatus", orderstatus);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$comments", comments);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$createddate", createddate);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$enteredby", enteredby);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        return ExecuteSPNonQuery("Sp_insert_taxcert_info", true, mParam);
    }

    public DataTable Fetchtaxcertinfo(string orderno)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_taxcert_info";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

    public int updatetaxstatus(string orderno, string expecteddate, string followupdate, string description)
    {
        mParam = new MySqlParameter[4];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$expecteddate", expecteddate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$followupdate", followupdate);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$description", description);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_update_taxstatus", true, mParam);
    }

    public int updatedate(string orderno, string expecteddate, string followupdate)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$expecteddate", expecteddate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$followupdate", followupdate);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_updateddate", true, mParam);
    }

    public int DeleteGrid(string orderno)
    {
        try
        {
            mParam = new MySqlParameter[3];

            mParam[0] = new MySqlParameter("?$orderno", orderno);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

            return ExecuteSPNonQuery("Sp_Delete_taxcertinfo", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int insert_taxparcel(string orderno, string taxidnumber, string taxyear, string endyear,string status, string tbd, string estimate)
    {
        mParam = new MySqlParameter[7];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$taxidnumber", taxidnumber);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$taxyear", taxyear);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$endyear", endyear);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$status", status);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$tbd", tbd);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("$estimate", estimate);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        return ExecuteSPNonQuery("Sp_insert_taxparcel", true, mParam);
    }
    public void update_taxparcel(string id, string taxidnumber, string taxyear, string endyear,string taxid_input,string orderno, string tbd, string est)
    {
        mParam = new MySqlParameter[8];

        mParam[0] = new MySqlParameter("$Id", id);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$taxid", taxidnumber);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$taxyear", taxyear);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$endyear", endyear);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$taxid_input", taxid_input);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$orderno", orderno);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;        
        mParam[6] = new MySqlParameter("$tbd", tbd);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("$est", est);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;
        ExecuteSPNonQuery("Sp_update_tax_parcel", true, mParam);
    }

    public DataTable Fetchtaxparcel(string orderno)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_tax_parcel";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

        
    public DataTable Fetchtaxauthorities(string orderno, string taxid)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_tax_authorities";
        mParam = new MySqlParameter[2];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }
    public DataTable Fetchtaxparcel_edit(string orderno, string taxid)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_taxparcel_edit";
        mParam = new MySqlParameter[2];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }


    public int DeleteGridParcel(string Id)
    {
        try
        {
            mParam = new MySqlParameter[1];

            mParam[0] = new MySqlParameter("?$Id", Id);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

            return ExecuteSPNonQuery("Sp_Delete_taxparcel", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
           
    public DataSet edittaxauthorities(string Id)
    {
        mParam = new MySqlParameter[1];

        mParam[0] = new MySqlParameter("?$Id", Id);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;


        return con.ExecuteQuery("Sp_Select_taxauthorities", true, mParam);
    }



    public void DeleteGridOrders(string id)
    {
        try
        {
            mParam = new MySqlParameter[1];

            mParam[0] = new MySqlParameter("?$Id", id);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

           ExecuteSPNonQuery("sp_delete_orders", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int insert_Exemptions(string orderno, string taxidnumber, string agencyid, string exetype, string exeamount, string taxtype)
    {
        mParam = new MySqlParameter[6];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$taxid", taxidnumber);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$exemptiontype", exetype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$exemptionamount", exeamount);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$taxtype", taxtype);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        return ExecuteSPNonQuery("Sp_insert_exemption_taxauthority", true, mParam);
    }
    public int update_Exemptions(string id, string orderno, string taxidnumber, string agencyid, string exetype, string exeamount)
    {
        mParam = new MySqlParameter[6];

        mParam[0] = new MySqlParameter("$Id", id);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$orderno", orderno);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$taxid", taxidnumber);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$agencyid", agencyid);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$exemptiontype", exetype);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$exemptionamount", exeamount);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_update_exemption_taxauthority", true, mParam);
    }

    public DataTable FetchExemptionAll(string orderno, string taxid, string agencyid)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetchall_exemption_taxauthority";
        mParam = new MySqlParameter[3];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

    public int DeleteGridExemption(string id)
    {
        try
        {
            mParam = new MySqlParameter[1];

            mParam[0] = new MySqlParameter("?$id", id);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

            return ExecuteSPNonQuery("sp_delete_exemption_taxauthority", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int insert_SpecialAssessment(string orderno, string taxidnumber, string agencyid, string description, string specialassessmentno, string noofinstallment, string installmentpaid, string installmentremaining, string duedate, string amount, string remainingbalance, string goodthroughdate, string perdiem, string payee, string comments, string taxtype)
    {
        mParam = new MySqlParameter[16];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$taxid", taxidnumber);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$description", description);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$specialassessmentno", specialassessmentno);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$noofinstallment", noofinstallment);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("$installmentpaid", installmentpaid);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("$InstallmentsRemaining", installmentremaining);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;
        mParam[8] = new MySqlParameter("$DueDate", duedate);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;
        mParam[9] = new MySqlParameter("$amount", amount);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;
        mParam[10] = new MySqlParameter("$RemainingBalance", remainingbalance);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;
        mParam[11] = new MySqlParameter("$GoodThroughDate", goodthroughdate);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;
        mParam[12] = new MySqlParameter("$PerDiem", perdiem);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;
        mParam[13] = new MySqlParameter("$Payee", payee);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;
        mParam[14] = new MySqlParameter("$Comments", comments);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;
        mParam[15] = new MySqlParameter("$taxtype", taxtype);
        mParam[15].MySqlDbType = MySqlDbType.VarChar;
        return ExecuteSPNonQuery("sp_insert_specialassessment_authority", true, mParam);
    }
    public int update_SpecialAssessment(int id, string taxidnumber, string agencyid, string description, string specialassessmentno, string noofinstallment, string installmentpaid, string amount)
    {
        mParam = new MySqlParameter[8];

        mParam[0] = new MySqlParameter("$Id", id);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$taxid", taxidnumber);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$description", description);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$specialassessmentno", specialassessmentno);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$noofinstallment", noofinstallment);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("$installmentpaid", installmentpaid);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("$amount", amount);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_update_specialassessment", true, mParam);
    }

    public DataTable FetchSpecialAssessmentAll(string orderno, string taxid, string agencyid)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetchall_specialassessment";
        mParam = new MySqlParameter[3];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }
    public DataTable FetchSpecialAssessment(string id)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_specialassessment";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$Id", id);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }


    public int DeleteGridPriorDelinquent(string id)
    {
        try
        {
            mParam = new MySqlParameter[1];

            mParam[0] = new MySqlParameter("?$Id", id);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

            return ExecuteSPNonQuery("Sp_Delete_Priordelinquent", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public int DeleteGridSpecialAssessment(string id)
    {
        try
        {
            mParam = new MySqlParameter[1];

            mParam[0] = new MySqlParameter("?$Id", id);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

            return ExecuteSPNonQuery("Sp_Delete_Specialassessment", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int insert_DeliquentStatus(string orderno, string taxidnumber, string agencyid, string payee, string address, string city,
         string state, string zip, string deliquenttaxyear, string payoffamount, string comments, string goodthuruDate, string installmentduedate,
         string taxsalenotapplicable, string dateofTaxsale, string lastdaytoredeem, string BaseAmountDue, string RollOverDate, string PenaltyAmount, string PenaltyAmountFrequency, string AdditionalPenaltyAmount, string PerDiem, string PenaltyDueDate, string taxtype)
    {
        mParam = new MySqlParameter[24];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$taxid", taxidnumber);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$payee", payee);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$address", address);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$city", city);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("$state", state);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("$zip", zip);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;
        mParam[8] = new MySqlParameter("$deliquenttaxyear", deliquenttaxyear);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;
        mParam[9] = new MySqlParameter("$payoffamount", payoffamount);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;
        mParam[10] = new MySqlParameter("$comments", comments);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;
        mParam[11] = new MySqlParameter("$goodthuruDate", goodthuruDate);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;
        mParam[12] = new MySqlParameter("$installmentduedate", installmentduedate);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;
        mParam[13] = new MySqlParameter("$taxsalenotapplicable", taxsalenotapplicable);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;
        mParam[14] = new MySqlParameter("$dateofTaxsale", dateofTaxsale);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;
        mParam[15] = new MySqlParameter("$lastdaytoredeem", lastdaytoredeem);
        mParam[15].MySqlDbType = MySqlDbType.VarChar;
        mParam[16] = new MySqlParameter("$BaseAmountDue", BaseAmountDue);
        mParam[16].MySqlDbType = MySqlDbType.VarChar;
        mParam[17] = new MySqlParameter("$RollOverDate", RollOverDate);
        mParam[17].MySqlDbType = MySqlDbType.VarChar;
        mParam[18] = new MySqlParameter("$PenaltyAmount", PenaltyAmount);
        mParam[18].MySqlDbType = MySqlDbType.VarChar;
        mParam[19] = new MySqlParameter("$PenaltyAmountFrequency", PenaltyAmountFrequency);
        mParam[19].MySqlDbType = MySqlDbType.VarChar;
        mParam[20] = new MySqlParameter("$AdditionalPenaltyAmount", AdditionalPenaltyAmount);
        mParam[20].MySqlDbType = MySqlDbType.VarChar;
        mParam[21] = new MySqlParameter("$PerDiem", PerDiem);
        mParam[21].MySqlDbType = MySqlDbType.VarChar;
        mParam[22] = new MySqlParameter("$PenaltyDueDate", PenaltyDueDate);
        mParam[22].MySqlDbType = MySqlDbType.VarChar;
        mParam[23] = new MySqlParameter("$taxtype", taxtype);
        mParam[23].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_insert_Delinquent", true, mParam);
    }
    public int update_DeliquentStatus(int id, string taxidnumber, string agencyid, string payee, string address, string city,
        string state, string zip, string deliquenttaxyear, string payoffamount, string comments, string goodthuruDate, string installmentduedate,
        string taxsalenotapplicable, string dateofTaxsale, string lastdaytoredeem, string BaseAmountDue, string RollOverDate, string PenaltyAmount, string PenaltyAmountFrequency, string AdditionalPenaltyAmount, string PerDiem, string PenaltyDueDate)
    {
        mParam = new MySqlParameter[23];

        mParam[0] = new MySqlParameter("$Id", id);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$taxid", taxidnumber);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$payee", payee);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$address", address);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$city", city);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("$state", state);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("$zip", zip);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;
        mParam[8] = new MySqlParameter("$deliquenttaxyear", deliquenttaxyear);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;
        mParam[9] = new MySqlParameter("$payoffamount", payoffamount);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;
        mParam[10] = new MySqlParameter("$comments", comments);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;
        mParam[11] = new MySqlParameter("$goodthuruDate", goodthuruDate);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;
        mParam[12] = new MySqlParameter("$installmentduedate", installmentduedate);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;
        mParam[13] = new MySqlParameter("$taxsalenotapplicable", taxsalenotapplicable);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;
        mParam[14] = new MySqlParameter("$dateofTaxsale", dateofTaxsale);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;
        mParam[15] = new MySqlParameter("$lastdaytoredeem", lastdaytoredeem);
        mParam[15].MySqlDbType = MySqlDbType.VarChar;
        mParam[16] = new MySqlParameter("$BaseAmountDue", BaseAmountDue);
        mParam[16].MySqlDbType = MySqlDbType.VarChar;
        mParam[17] = new MySqlParameter("$RollOverDate", RollOverDate);
        mParam[17].MySqlDbType = MySqlDbType.VarChar;
        mParam[18] = new MySqlParameter("$PenaltyAmount", PenaltyAmount);
        mParam[18].MySqlDbType = MySqlDbType.VarChar;
        mParam[19] = new MySqlParameter("$PenaltyAmountFrequency", PenaltyAmountFrequency);
        mParam[19].MySqlDbType = MySqlDbType.VarChar;
        mParam[20] = new MySqlParameter("$AdditionalPenaltyAmount", AdditionalPenaltyAmount);
        mParam[20].MySqlDbType = MySqlDbType.VarChar;
        mParam[21] = new MySqlParameter("$PerDiem", PerDiem);
        mParam[21].MySqlDbType = MySqlDbType.VarChar;
        mParam[22] = new MySqlParameter("$PenaltyDueDate", PenaltyDueDate);
        mParam[22].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_update_delinquent", true, mParam);
    }
    //balaji
    public DataTable FetchDeliquentStatusAll(string orderno, string agencyid,string taxid)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetchall_delinquent";
        mParam = new MySqlParameter[3];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;              

        mParam[1] = new MySqlParameter("?$agencyid", agencyid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$taxid", taxid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }
    public DataTable FetchDeliquentStatus(string payeename)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_delinquent";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$payeename", payeename);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }
    public int DeleteGridDeliquentStatus(string id)
    {
        try
        {
            mParam = new MySqlParameter[1];

            mParam[0] = new MySqlParameter("?$Id", id);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

            return ExecuteSPNonQuery("Sp_Delete_deliquent", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //amrock
    public int update_tax_authorities_paymentdetails(string orderno, string taxid, string agencyid, string taxagencytype, string address, string instamount1, string instamount2, string instamount3, string instamount4, string instamountpaid1, string instamountpaid2, string instamountpaid3, string instamountpaid4, string instPaidDue1, string instPaidDue2, string instPaidDue3, string instPaidDue4, string remainingbalance1, string remainingbalance2, string remainingbalance3, string remainingbalance4, string installmentdate1, string installmentdate2, string installmentdate3, string installmentdate4, string delinquentdate1, string delinquentdate2, string delinquentdate3, string delinquentdate4, string discountamount1, string discountamount2, string discountamount3, string discountamount4, string discountdate1, string discountdate2, string discountdate3, string discountdate4, string exemptrelevy1, string exemptrelevy2, string exemptrelevy3, string exemptrelevy4, string nextbilldate1, string nextbilldate2, string taxbill, string paymentfrequency, string billingstartdate, string billingenddate, string futuretaxcal, string installmentcomments, string authoritystatus, string annualtaxamount)
    {        
        mParam = new MySqlParameter[51];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
                
        mParam[3] = new MySqlParameter("$taxagencytype", taxagencytype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("$address", address);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
               
        mParam[5] = new MySqlParameter("$instamount1", instamount1);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("$instamount2", instamount2);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("$instamount3", instamount3);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("$instamount4", instamount4);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        mParam[9] = new MySqlParameter("$instamountpaid1", instamountpaid1);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;

        mParam[10] = new MySqlParameter("$instamountpaid2", instamountpaid2);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;

        mParam[11] = new MySqlParameter("$instamountpaid3", instamountpaid3);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;

        mParam[12] = new MySqlParameter("$instamountpaid4", instamountpaid4);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;

        mParam[13] = new MySqlParameter("$instPaidDue1", instPaidDue1);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;

        mParam[14] = new MySqlParameter("$instPaidDue2", instPaidDue2);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;

        mParam[15] = new MySqlParameter("$instPaidDue3", instPaidDue3);
        mParam[15].MySqlDbType = MySqlDbType.VarChar;

        mParam[16] = new MySqlParameter("$instPaidDue4", instPaidDue4);
        mParam[16].MySqlDbType = MySqlDbType.VarChar;

        mParam[17] = new MySqlParameter("$remainingbalance1", remainingbalance1);
        mParam[17].MySqlDbType = MySqlDbType.VarChar;

        mParam[18] = new MySqlParameter("$remainingbalance2", remainingbalance2);
        mParam[18].MySqlDbType = MySqlDbType.VarChar;

        mParam[19] = new MySqlParameter("$remainingbalance3", remainingbalance3);
        mParam[19].MySqlDbType = MySqlDbType.VarChar;

        mParam[20] = new MySqlParameter("$remainingbalance4", remainingbalance4);
        mParam[20].MySqlDbType = MySqlDbType.VarChar;

        mParam[21] = new MySqlParameter("$installmentdate1", installmentdate1);
        mParam[21].MySqlDbType = MySqlDbType.VarChar;

        mParam[22] = new MySqlParameter("$installmentdate2", installmentdate2);
        mParam[22].MySqlDbType = MySqlDbType.VarChar;

        mParam[23] = new MySqlParameter("$installmentdate3", installmentdate3);
        mParam[23].MySqlDbType = MySqlDbType.VarChar;

        mParam[24] = new MySqlParameter("$installmentdate4", installmentdate4);
        mParam[24].MySqlDbType = MySqlDbType.VarChar;

        mParam[25] = new MySqlParameter("$delinquentdate1", delinquentdate1);
        mParam[25].MySqlDbType = MySqlDbType.VarChar;

        mParam[26] = new MySqlParameter("$delinquentdate2", delinquentdate2);
        mParam[26].MySqlDbType = MySqlDbType.VarChar;

        mParam[27] = new MySqlParameter("$delinquentdate3", delinquentdate3);
        mParam[27].MySqlDbType = MySqlDbType.VarChar;

        mParam[28] = new MySqlParameter("$delinquentdate4", delinquentdate4);
        mParam[28].MySqlDbType = MySqlDbType.VarChar;

        mParam[29] = new MySqlParameter("$discountamount1", discountamount1);
        mParam[29].MySqlDbType = MySqlDbType.VarChar;

        mParam[30] = new MySqlParameter("$discountamount2", discountamount2);
        mParam[30].MySqlDbType = MySqlDbType.VarChar;

        mParam[31] = new MySqlParameter("$discountamount3", discountamount3);
        mParam[31].MySqlDbType = MySqlDbType.VarChar;

        mParam[32] = new MySqlParameter("$discountamount4", discountamount4);
        mParam[32].MySqlDbType = MySqlDbType.VarChar;

        mParam[33] = new MySqlParameter("$discountdate1", discountdate1);
        mParam[33].MySqlDbType = MySqlDbType.VarChar;

        mParam[34] = new MySqlParameter("$discountdate2", discountdate2);
        mParam[34].MySqlDbType = MySqlDbType.VarChar;

        mParam[35] = new MySqlParameter("$discountdate3", discountdate3);
        mParam[35].MySqlDbType = MySqlDbType.VarChar;

        mParam[36] = new MySqlParameter("$discountdate4", discountdate4);
        mParam[36].MySqlDbType = MySqlDbType.VarChar;

        mParam[37] = new MySqlParameter("$exemptrelevy1", exemptrelevy1);
        mParam[37].MySqlDbType = MySqlDbType.VarChar;

        mParam[38] = new MySqlParameter("$exemptrelevy2", exemptrelevy2);
        mParam[38].MySqlDbType = MySqlDbType.VarChar;

        mParam[39] = new MySqlParameter("$exemptrelevy3", exemptrelevy3);
        mParam[39].MySqlDbType = MySqlDbType.VarChar;

        mParam[40] = new MySqlParameter("$exemptrelevy4", exemptrelevy4);
        mParam[40].MySqlDbType = MySqlDbType.VarChar;

        mParam[41] = new MySqlParameter("$nextbilldate1", nextbilldate1);
        mParam[41].MySqlDbType = MySqlDbType.VarChar;

        mParam[42] = new MySqlParameter("$nextbilldate2", nextbilldate2);
        mParam[42].MySqlDbType = MySqlDbType.VarChar;

        mParam[43] = new MySqlParameter("$taxbill", taxbill);
        mParam[43].MySqlDbType = MySqlDbType.VarChar;

        mParam[44] = new MySqlParameter("$paymentfrequency", paymentfrequency);
        mParam[44].MySqlDbType = MySqlDbType.VarChar;               

        mParam[45] = new MySqlParameter("$billingstartdate", billingstartdate);
        mParam[45].MySqlDbType = MySqlDbType.VarChar;

        mParam[46] = new MySqlParameter("$billingenddate", billingenddate);
        mParam[46].MySqlDbType = MySqlDbType.VarChar;
        
        mParam[47] = new MySqlParameter("$futuretaxcal", futuretaxcal);
        mParam[47].MySqlDbType = MySqlDbType.VarChar;

        mParam[48] = new MySqlParameter("$installmentcomments", installmentcomments);
        mParam[48].MySqlDbType = MySqlDbType.VarChar;

        mParam[49] = new MySqlParameter("$authoritystatus", authoritystatus);
        mParam[49].MySqlDbType = MySqlDbType.VarChar;

        mParam[50] = new MySqlParameter("$annualtaxamount", annualtaxamount);
        mParam[50].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_Update_taxparcelauthority", true, mParam);
    }



    public int Insert_tax_authorities_paymentdetails(string orderno, string taxid, string agencyid, string taxtype, string instamount1, string instamount2, string instamount3, string instamount4, string instamountpaid1, string instamountpaid2, string instamountpaid3, string instamountpaid4, string instPaidDue1, string instPaidDue2, string instPaidDue3, string instPaidDue4, string remainingbalance1, string remainingbalance2, string remainingbalance3, string remainingbalance4, string installmentdate1, string installmentdate2, string installmentdate3, string installmentdate4, string delinquentdate1, string delinquentdate2, string delinquentdate3, string delinquentdate4, string discountamount1, string discountamount2, string discountamount3, string discountamount4, string discountdate1, string discountdate2, string discountdate3, string discountdate4, string exemptrelevy1, string exemptrelevy2, string exemptrelevy3, string exemptrelevy4, string taxbill, string paymentfrequency, string billingstartdate, string billingenddate, string installmentcomments, string taxauthorityname, string annualtaxamount, string taxauthoritystatus,string futuretax, string billperiodstartdate, string billperiodenddate)
    {
        mParam = new MySqlParameter[51];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;              

        mParam[3] = new MySqlParameter("$taxagencytype", taxtype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;      

        mParam[4] = new MySqlParameter("$instamount1", instamount1);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("$instamount2", instamount2);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("$instamount3", instamount3);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("$instamount4", instamount4);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("$instamountpaid1", instamountpaid1);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        mParam[9] = new MySqlParameter("$instamountpaid2", instamountpaid2);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;

        mParam[10] = new MySqlParameter("$instamountpaid3", instamountpaid3);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;

        mParam[11] = new MySqlParameter("$instamountpaid4", instamountpaid4);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;

        mParam[12] = new MySqlParameter("$instPaidDue1", instPaidDue1);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;

        mParam[13] = new MySqlParameter("$instPaidDue2", instPaidDue2);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;

        mParam[14] = new MySqlParameter("$instPaidDue3", instPaidDue3);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;

        mParam[15] = new MySqlParameter("$instPaidDue4", instPaidDue4);
        mParam[15].MySqlDbType = MySqlDbType.VarChar;

        mParam[16] = new MySqlParameter("$remainingbalance1", remainingbalance1);
        mParam[16].MySqlDbType = MySqlDbType.VarChar;

        mParam[17] = new MySqlParameter("$remainingbalance2", remainingbalance2);
        mParam[17].MySqlDbType = MySqlDbType.VarChar;

        mParam[18] = new MySqlParameter("$remainingbalance3", remainingbalance3);
        mParam[18].MySqlDbType = MySqlDbType.VarChar;

        mParam[19] = new MySqlParameter("$remainingbalance4", remainingbalance4);
        mParam[19].MySqlDbType = MySqlDbType.VarChar;

        mParam[20] = new MySqlParameter("$installmentdate1", installmentdate1);
        mParam[20].MySqlDbType = MySqlDbType.VarChar;

        mParam[21] = new MySqlParameter("$installmentdate2", installmentdate2);
        mParam[21].MySqlDbType = MySqlDbType.VarChar;

        mParam[22] = new MySqlParameter("$installmentdate3", installmentdate3);
        mParam[22].MySqlDbType = MySqlDbType.VarChar;

        mParam[23] = new MySqlParameter("$installmentdate4", installmentdate4);
        mParam[23].MySqlDbType = MySqlDbType.VarChar;

        mParam[24] = new MySqlParameter("$delinquentdate1", delinquentdate1);
        mParam[24].MySqlDbType = MySqlDbType.VarChar;

        mParam[25] = new MySqlParameter("$delinquentdate2", delinquentdate2);
        mParam[25].MySqlDbType = MySqlDbType.VarChar;

        mParam[26] = new MySqlParameter("$delinquentdate3", delinquentdate3);
        mParam[26].MySqlDbType = MySqlDbType.VarChar;

        mParam[27] = new MySqlParameter("$delinquentdate4", delinquentdate4);
        mParam[27].MySqlDbType = MySqlDbType.VarChar;

        mParam[28] = new MySqlParameter("$discountamount1", discountamount1);
        mParam[28].MySqlDbType = MySqlDbType.VarChar;

        mParam[29] = new MySqlParameter("$discountamount2", discountamount2);
        mParam[29].MySqlDbType = MySqlDbType.VarChar;

        mParam[30] = new MySqlParameter("$discountamount3", discountamount3);
        mParam[30].MySqlDbType = MySqlDbType.VarChar;

        mParam[31] = new MySqlParameter("$discountamount4", discountamount4);
        mParam[31].MySqlDbType = MySqlDbType.VarChar;

        mParam[32] = new MySqlParameter("$discountdate1", discountdate1);
        mParam[32].MySqlDbType = MySqlDbType.VarChar;

        mParam[33] = new MySqlParameter("$discountdate2", discountdate2);
        mParam[33].MySqlDbType = MySqlDbType.VarChar;

        mParam[34] = new MySqlParameter("$discountdate3", discountdate3);
        mParam[34].MySqlDbType = MySqlDbType.VarChar;

        mParam[35] = new MySqlParameter("$discountdate4", discountdate4);
        mParam[35].MySqlDbType = MySqlDbType.VarChar;

        mParam[36] = new MySqlParameter("$exemptrelevy1", exemptrelevy1);
        mParam[36].MySqlDbType = MySqlDbType.VarChar;

        mParam[37] = new MySqlParameter("$exemptrelevy2", exemptrelevy2);
        mParam[37].MySqlDbType = MySqlDbType.VarChar;

        mParam[38] = new MySqlParameter("$exemptrelevy3", exemptrelevy3);
        mParam[38].MySqlDbType = MySqlDbType.VarChar;
    
        mParam[39] = new MySqlParameter("$exemptrelevy4", exemptrelevy4);
        mParam[39].MySqlDbType = MySqlDbType.VarChar;          

        mParam[40] = new MySqlParameter("$taxbill", taxbill);
        mParam[40].MySqlDbType = MySqlDbType.VarChar;

        mParam[41] = new MySqlParameter("$paymentfrequency", paymentfrequency);
        mParam[41].MySqlDbType = MySqlDbType.VarChar;

        mParam[42] = new MySqlParameter("$billingstartdate", billingstartdate);
        mParam[42].MySqlDbType = MySqlDbType.VarChar;

        mParam[43] = new MySqlParameter("$billingenddate", billingenddate);
        mParam[43].MySqlDbType = MySqlDbType.VarChar;

        mParam[44] = new MySqlParameter("$installmentcomments", installmentcomments);
        mParam[44].MySqlDbType = MySqlDbType.VarChar;

        mParam[45] = new MySqlParameter("$taxauthorityname", taxauthorityname);
        mParam[45].MySqlDbType = MySqlDbType.VarChar;

        mParam[46] = new MySqlParameter("$annualtaxamount", annualtaxamount);
        mParam[46].MySqlDbType = MySqlDbType.VarChar;

        mParam[47] = new MySqlParameter("$taxauthoritystatus", taxauthoritystatus);
        mParam[47].MySqlDbType = MySqlDbType.VarChar;

        mParam[48] = new MySqlParameter("$futuretaxoption", futuretax);
        mParam[48].MySqlDbType = MySqlDbType.VarChar;

        mParam[49] = new MySqlParameter("$BillingPeriodStartDate", billingstartdate);
        mParam[49].MySqlDbType = MySqlDbType.VarChar;

        mParam[50] = new MySqlParameter("$BillingPeriodEndDate", billperiodenddate);
        mParam[50].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_Insert_Taxauthoritynew", true, mParam);
    }

    public DataTable FetchTaxAuthorityDetails(string orderno, string taxid, string agencyid, string taxtype)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetchall_installment_details";
        mParam = new MySqlParameter[4];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$taxtype", taxtype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }


    public DataTable FetchTaxAuthorityDetailsFuture(string orderno, string taxid, string agencyid, string taxtype)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetchall_installment_details_future";
        mParam = new MySqlParameter[4];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$taxtype", taxtype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
                
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }





    public DataSet UpdateOrdersNew1(string query, string orderno, string borrower, string ordertype)
    {
        if (query == "sp_UpdateQC_New" || query == "sp_UpdateQC_User" || query == "sp_UpdateReview_New" || query == "sp_UpdateDU_New" || query == "sp_UpdateDU_User") mParam = new MySqlParameter[4];

        else mParam = new MySqlParameter[4];

        mParam[0] = new MySqlParameter("?$OrderNo", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$borrower", borrower);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$oType", ordertype);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
                
        mParam[3] = new MySqlParameter("?$username", SessionHandler.UserName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;       
                
        mParam[4] = new MySqlParameter("?$payfreq", myVariables.PayFreq);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        if (query == "sp_UpdateQC_New" || query == "sp_UpdateQC_User" || query == "sp_UpdateReview_New" || query == "sp_UpdateDU_New" || query == "sp_UpdateDU_User")
        {

        }

        return con.ExecuteQuery(query, true, mParam);
    }
    
        
    public int Inserttaxauthoritiesdetails(string orderno, string taxid, string AgencyId, string TaxAuthorityName, string TaxAgencyType, string TaxAgencyState, string Phone,string taxyearstartdate)
    {
        mParam = new MySqlParameter[8];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("$AgencyId", AgencyId);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("$TaxAuthorityName", TaxAuthorityName);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("$TaxAgencyType", TaxAgencyType);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("$TaxAgencyState", TaxAgencyState);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("$Phone", Phone);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("$taxyearstartdate", taxyearstartdate);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_Insert_Taxauthoritydetails", true, mParam);
    }

    public DataTable FetchTaxAuthoritywebsiteDetails(string orderno, string taxid, string agencyid)
    {
        DataTable dt1 = new DataTable();
        string query = "Sp_taxauthortywebsite";
        mParam = new MySqlParameter[3];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt1);
        return dt1;
    }

    public DataTable FetchTrackingDetails(string fdate, string tdate, string state, string username, string status)
    {
        DataTable dt = new DataTable();
        string query = "Sp_TrackingDetailsALL";
        mParam = new MySqlParameter[5];
        mParam[0] = new MySqlParameter("?$fdate", fdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$tdate", tdate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$state", state);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$username", username);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("?$status", status);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

    public DataSet GetOrderCountAll(string strfrmdate, string strtodate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", strfrmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strtodate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;


        return con.ExecuteQuery("sp_getOrdercountall", true, mParam);
    }




    public int insert_priordelinquent(string orderno, string taxidnumber, string agencyid, string delinquenttaxyear, string originalamountdue, string originaldelinquencydate, string amountpaid, string delinquencycomments, string taxtype)
    {
        mParam = new MySqlParameter[9];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$taxid", taxidnumber);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$delinquenttaxyear", delinquenttaxyear);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$originalamountdue", originalamountdue);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$originaldelinquencydate", originaldelinquencydate);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("$amountpaid", amountpaid);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("$delinquencycomments", delinquencycomments);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;
        mParam[8] = new MySqlParameter("$taxtype", taxtype);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_insert_pridelinq", true, mParam);
    }


    public int update_priordelinquent(int id, string taxidnumber, string agencyid, string delinquenttaxyear, string originalamountdue, string originaldelinquencydate, string amountpaid, string delinquencycomments)
    {
        mParam = new MySqlParameter[8];

        mParam[0] = new MySqlParameter("$Id", id);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$taxid", taxidnumber);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$delinquenttaxyear", delinquenttaxyear);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$originalamountdue", originalamountdue);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        mParam[5] = new MySqlParameter("$originaldelinquencydate", originaldelinquencydate);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;
        mParam[6] = new MySqlParameter("$amountpaid", amountpaid);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;
        mParam[7] = new MySqlParameter("$delinquencycomments", delinquencycomments);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        return ExecuteSPNonQuery("Sp_update_priordelinquent", true, mParam);
    }

    public DataTable FetchPriorDelinquentAll(string orderno, string taxid, string agencyid)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetchall_priordelinquent";
        mParam = new MySqlParameter[3];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }


    public DataTable FetchAUDetails(string orderno)
    {
        DataTable dt1 = new DataTable();
        string query = "Sp_taxaudetails";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt1);
        return dt1;
    }


    public DataSet inserttaxauthorities(string orderno, string taxid, string agencyid)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        
        mParam[1] = new MySqlParameter("$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        
        mParam[2] = new MySqlParameter("$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("Sp_inserttaxdetailsoutput", true, mParam);
    }


    public DataSet test(string orderno, string agencyid, string taxagencytype)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
              
        mParam[1] = new MySqlParameter("$agencyid", agencyid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("$taxtype", taxagencytype);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("Sp_test", true, mParam);
    }



    public int Updatetaxauthoritiesdetails(string orderno, string taxid, string AgencyId)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("$AgencyId", AgencyId);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;                           

        return ExecuteSPNonQuery("Sp_Update_Taxauthoritydetails", true, mParam);
    }


    public DataSet fetchwebsite(string orderno, string agencyid)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("$agencyid", agencyid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("Sp_Insert_Auwebsite", true, mParam);
    }


    public DataSet fetchwebsiteupdated(string orderno, string agencyid, string taxid)
    {
        mParam = new MySqlParameter[3];

        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$agencyid", agencyid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$taxid", taxid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("Sp_Update_Auwebsite", true, mParam);
    }


    public int DeleteGridpriordelinquent(string id)
    {
        try
        {
            mParam = new MySqlParameter[1];

            mParam[0] = new MySqlParameter("?$Id", id);
            mParam[0].MySqlDbType = MySqlDbType.VarChar;

            return ExecuteSPNonQuery("Sp_Delete_Priordelinquent", true, mParam);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable Fetchdeliquent(string orderno, string agencyid)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetchall_deliquent_details";
        mParam = new MySqlParameter[2];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;        
        mParam[1] = new MySqlParameter("?$agencyid", agencyid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

    public DataTable FetchdelinquentNew(string orderno, string taxid, string agencyid, string taxtype)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_delinquent_new";
        mParam = new MySqlParameter[4];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$taxtype", taxtype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

    public DataTable FetchExemptionNew(string orderno, string taxid, string agencyid, string taxtype)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_exemption_new";
        mParam = new MySqlParameter[4];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$taxtype", taxtype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }


    public DataTable FetchSpecialAssessmentNew(string orderno, string taxid, string agencyid, string taxtype)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_specialassessment_new";
        mParam = new MySqlParameter[4];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$taxtype", taxtype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

    public DataTable FetchPriorDeliquent(string orderno, string taxid, string agencyid, string taxtype)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_prior_new";
        mParam = new MySqlParameter[4];
        mParam[0] = new MySqlParameter("?$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("?$taxid", taxid);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("?$agencyid", agencyid);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("?$taxtype", taxtype);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }

    public int updatetaxtypedetails(string taxtype, string id)
    {
        mParam = new MySqlParameter[2];
        mParam[0] = new MySqlParameter("$taxtype", taxtype);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$id", id);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        
        return ExecuteSPNonQuery("Sp_update_taxtype", true, mParam);
    }

    public int insert_Addnotes(string ordno, string note, string note_type, string addeddate, string enterby)
    {
        mParam = new MySqlParameter[5];

        mParam[0] = new MySqlParameter("$orderno", ordno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[1] = new MySqlParameter("$note", note);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;
        mParam[2] = new MySqlParameter("$note_type", note_type);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;
        mParam[3] = new MySqlParameter("$added", addeddate);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;
        mParam[4] = new MySqlParameter("$enterby", enterby);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;
        return ExecuteSPNonQuery("Sp_insert_addnotes", true, mParam);
    }
    

    public DataTable Fetchaddnotes(string orderno)
    {
        DataTable dt = new DataTable();
        string query = "Sp_fetch_addnotes";
        mParam = new MySqlParameter[1];
        mParam[0] = new MySqlParameter("$orderno", orderno);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mDa = con.ExecuteSPAdapter(query, true, mParam);
        mDa.Fill(dt);
        return dt;
    }
}










