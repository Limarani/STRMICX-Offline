using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Text;
using System.Collections;
using System.Security;
/// <summary>
/// Summary description for GlobalClass
/// </summary>
public class GlobalClass : myConnection
{
    myConnection con = new myConnection();

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
        if (setdate >= 000000 && setdate <= 070000) result = String.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddDays(-1));
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
            string query = "select *,cast(aes_decrypt(User_Password,'String') as char) as pwd from user_status where User_Id='" + SessionHandler.UserName + "'";
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

        string query = "select us.User_Name as Fullname,us.User_Id as Username,us.Admin as Admin,us.Keying as Key1,us.QC as QC,us.Review as Review,us.Pending as Pend,us.DU as DU,us.mailaway as mailaway,us.Parcelid as parcelid,us.Onhold as onhold,us.Order_type as type,us.State,us.SST,us.Priority,us.QA,us.Prior  from user_status us where us.User_Id ='" + UserName + "'  limit 1";
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
    public int InsertUser(string usr, string ful, string Ad, string Keya, string Qc, string Du, string pend, string mail, string parcelid, string onhold, string ordertype, string review, string priority, string qa, string prior)
    {
        mParam = new MySqlParameter[15];

        mParam[0] = new MySqlParameter("?$User_Name", ful);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;
        mParam[0].IsNullable = false;

        mParam[1] = new MySqlParameter("?$User_Id", usr);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        mParam[2] = new MySqlParameter("?$Admin", Ad);
        mParam[2].MySqlDbType = MySqlDbType.VarChar;

        mParam[3] = new MySqlParameter("?$Keying", Keya);
        mParam[3].MySqlDbType = MySqlDbType.VarChar;

        mParam[4] = new MySqlParameter("?$QC", Qc);
        mParam[4].MySqlDbType = MySqlDbType.VarChar;

        mParam[5] = new MySqlParameter("?$Pending", pend);
        mParam[5].MySqlDbType = MySqlDbType.VarChar;

        mParam[6] = new MySqlParameter("?$DU", Du);
        mParam[6].MySqlDbType = MySqlDbType.VarChar;

        mParam[7] = new MySqlParameter("?$mailaway", mail);
        mParam[7].MySqlDbType = MySqlDbType.VarChar;

        mParam[8] = new MySqlParameter("?$parcelid", parcelid);
        mParam[8].MySqlDbType = MySqlDbType.VarChar;

        mParam[9] = new MySqlParameter("?$onhold", onhold);
        mParam[9].MySqlDbType = MySqlDbType.VarChar;

        mParam[10] = new MySqlParameter("?$Order_type", ordertype);
        mParam[10].MySqlDbType = MySqlDbType.VarChar;

        mParam[11] = new MySqlParameter("?$review", review);
        mParam[11].MySqlDbType = MySqlDbType.VarChar;

        mParam[12] = new MySqlParameter("?$prio", priority);
        mParam[12].MySqlDbType = MySqlDbType.VarChar;

        mParam[13] = new MySqlParameter("?$qa", qa);
        mParam[13].MySqlDbType = MySqlDbType.VarChar;

        mParam[14] = new MySqlParameter("?$priority", prior);
        mParam[14].MySqlDbType = MySqlDbType.VarChar;

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

    public int ResetPassword(string usr)
    {
        string query = "update user_status set User_Password=aes_encrypt('String123$','String') where User_Id='" + usr + "' limit  1";
        return con.ExecuteSPNonQuery(query);
    }


    public int ChangePassword(string usr, string Pass)
    {
        string query = "update user_status set User_Password=aes_encrypt('" + Pass + "','String') where User_Id='" + usr + "' limit  1";
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
    public int InsertData_New(string OrderNo, string state, string county, string prior, string pdate, string expected)
    {
        mParam = new MySqlParameter[6];

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

    public void GetOrderDelete(string strorderno, string date)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Delete from record_status where order_no='" + strorderno + "' and pdate='" + date + "' and k1='0' and qc='0' and status='0'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderLock(string strorderno, string date)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set Lock1=1 where order_no='" + strorderno + "' and pdate='" + date + "'";
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

    public void GetOrderPriority(string strorderno, string date)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "update record_status set HP=1 where order_no='" + strorderno + "' and pdate='" + date + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void GetOrderReject(string strorderno, string date)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set k1='7',qc='7',status='7',Pend='0',Parcel='0',Tax='0' where Order_No ='" + strorderno + "' and pdate='" + date + "'";
            int result = con.ExecuteSPNonQuery(strquery);
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
    public void GetOrderYTS(string strorderno, string date)
    {
        string strquery = string.Empty;
        try
        {
            strquery = "Update record_status set k1='0',qc='0',status='0',pend='0',Parcel='0',Tax='0' where Order_No ='" + strorderno + "' and pdate='" + date + "'";
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

        return con.ExecuteQuery("sp_Production_Validation", true, mParam);
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

    public DataSet UpdateOrdersNew(string query, string orderno, string township, string borrower, string ist, string pdate, string oStatus, string ordertype, string error, string errorfield, string correct, string incorrect, string processname, string ist1, string qcopcomments, string errorcategory, string encount, string zipcode, string followupdate)
    {
        if (query == "sp_UpdateQC_New" || query == "sp_UpdateReview_New" || query == "sp_UpdateDU_New") mParam = new MySqlParameter[24];
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

        if (query == "sp_UpdateQC_New" || query == "sp_UpdateReview_New" || query == "sp_UpdateDU_New")
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
        }

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

        Int16 i = 1;
        string Status = "YTS";
        int hp;
        int lk;
        int rej;
        string test = "";
        string strcomments = "";

        string loc, k1, qc, hold, pend, tax, parcel, key_status, review, stat = "";
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
            //New
            dtRow[5] = reader["prior"];
            dtRow[6] = reader["Time_Zone"];
            dtRow[7] = reader["State"];
            dtRow[8] = reader["County"];
            dtRow[9] = reader["Township"];
            dtRow[10] = reader["webphone"];

            loc = reader["Lock1"].ToString();
            k1 = reader["k1"].ToString();
            qc = reader["qc"].ToString();
            review = reader["Review"].ToString();
            stat = reader["status"].ToString();
            pend = reader["pend"].ToString();
            tax = reader["tax"].ToString();
            parcel = reader["parcel"].ToString();
            key_status = reader["key_status"].ToString();

            if (loc == "1")
            { dtRow[11] = "Locked"; }
            else if (k1 == "0" && qc == "0")
            { dtRow[11] = "YTS"; }
            else if (k1 == "1" && qc == "0")
            { dtRow[11] = "Key Started"; }
            else if (k1 == "2" && qc == "0" && key_status != "Others")
            { dtRow[11] = "Key Completed"; }
            else if (k1 == "2" && qc == "0" && key_status == "Others")
            { dtRow[11] = "Others"; }
            else if (k1 == "2" && qc == "1")
            { dtRow[11] = "QC Started"; }
            else if (k1 == "5" && qc == "5" && stat == "5")
            { dtRow[11] = "Delivered"; }
            else if (k1 == "4" && qc == "4" && stat == "4")
            { dtRow[11] = "On Hold"; }
            else if (k1 == "7" && qc == "7" && stat == "7")
            { dtRow[11] = "Rejected"; }
            else if (k1 == "9" && qc == "9" && stat == "9" && review == "9")
            { dtRow[11] = "Order Missing"; }

            if (pend == "3")
            { dtRow[11] = "In Process"; }
            else if (pend == "1")
            { dtRow[11] = "In Process Started"; }

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
            { dtRow[11] = "Mail Away"; }
            else if (tax == "1")
            { dtRow[11] = "Mail Away Started"; }

            if (parcel == "3")
            { dtRow[11] = "ParcelID"; }
            else if (parcel == "1")
            { dtRow[11] = "ParcelID Started"; }

            dtRow[12] = reader["k1_op"];
            dtRow[13] = reader["Lastcomment"];
            dtRow[14] = reader["k1_st"];
            dtRow[15] = reader["k1_et"];

            if (dtRow[14].ToString() != "" && dtRow[15].ToString() != "")
            {
                DateTime StTime = DateTime.Parse(dtRow[14].ToString());
                DateTime EnTime = DateTime.Parse(dtRow[15].ToString());
                TimeSpan TimeDiff = EnTime.Subtract(StTime);
                dtRow[16] = TimeDiff;

            }
            dtRow[17] = reader["qc_op"];
            dtRow[18] = reader["qc_st"];
            dtRow[19] = reader["qc_et"];

            if (dtRow[18].ToString() != "" && dtRow[19].ToString() != "")
            {
                DateTime StTime = DateTime.Parse(dtRow[18].ToString());
                DateTime EnTime = DateTime.Parse(dtRow[19].ToString());
                TimeSpan TimeDiff = EnTime.Subtract(StTime);
                dtRow[20] = TimeDiff;
            }


            dtRow[21] = reader["uploadtime"];


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
            if (strtat == "0") dtRow[22] = "No";
            else if (strtat == "1") dtRow[22] = "Yes";
            dtRow[23] = reader["Delivered"];
            dtRow[24] = reader["id"];

            if (k1 == "5" && qc == "5" && stat == "5" && review == "5") dtRow[25] = "Yes";
            else dtRow[25] = "No";


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

        Int16 cnt = 1;
        string Status = "YTS";
        int hp;
        int lk;
        int rej;
        string test = "";
        string strcomments = "";

        string loc, k1, qc, hold, pend, tax, parcel, key_status, review, stat = "";
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
                dtRow[5] = ds.Tables[0].Rows[i]["prior"];
                dtRow[6] = ds.Tables[0].Rows[i]["Time_Zone"];
                dtRow[7] = ds.Tables[0].Rows[i]["State"];
                dtRow[8] = ds.Tables[0].Rows[i]["County"];
                dtRow[9] = ds.Tables[0].Rows[i]["Township"];
                dtRow[10] = ds.Tables[0].Rows[i]["webphone"];

                loc = ds.Tables[0].Rows[i]["Lock1"].ToString();
                k1 = ds.Tables[0].Rows[i]["k1"].ToString();
                qc = ds.Tables[0].Rows[i]["qc"].ToString();
                review = ds.Tables[0].Rows[i]["Review"].ToString();
                stat = ds.Tables[0].Rows[i]["status"].ToString();
                pend = ds.Tables[0].Rows[i]["pend"].ToString();
                tax = ds.Tables[0].Rows[i]["tax"].ToString();
                parcel = ds.Tables[0].Rows[i]["parcel"].ToString();
                key_status = ds.Tables[0].Rows[i]["key_status"].ToString();

                if (loc == "1")
                { dtRow[11] = "Locked"; }
                else if (k1 == "0" && qc == "0")
                { dtRow[11] = "YTS"; }
                else if (k1 == "1" && qc == "0")
                { dtRow[11] = "Key Started"; }
                else if (k1 == "2" && qc == "0" && key_status != "Others")
                { dtRow[11] = "Key Completed"; }
                else if (k1 == "2" && qc == "0" && key_status == "Others")
                { dtRow[11] = "Others"; }
                else if (k1 == "2" && qc == "1")
                { dtRow[11] = "QC Started"; }
                else if (k1 == "5" && qc == "5" && stat == "5")
                { dtRow[11] = "Delivered"; }
                else if (k1 == "4" && qc == "4" && stat == "4")
                { dtRow[11] = "On Hold"; }
                else if (k1 == "7" && qc == "7" && stat == "7")
                { dtRow[11] = "Rejected"; }
                else if (k1 == "9" && qc == "9" && stat == "9" && review == "9")
                { dtRow[11] = "Order Missing"; }

                if (pend == "3")
                { dtRow[11] = "In Process"; }
                else if (pend == "1")
                { dtRow[11] = "In Process Started"; }

                if (tax == "3")
                { dtRow[11] = "Mail Away"; }
                else if (tax == "1")
                { dtRow[11] = "Mail Away Started"; }

                if (parcel == "3")
                { dtRow[11] = "ParcelID"; }
                else if (parcel == "1")
                { dtRow[11] = "ParcelID Started"; }

                dtRow[12] = ds.Tables[0].Rows[i]["k1_op"];
                dtRow[13] = ds.Tables[0].Rows[i]["Lastcomment"];
                dtRow[14] = ds.Tables[0].Rows[i]["k1_st"];
                dtRow[15] = ds.Tables[0].Rows[i]["k1_et"];

                if (dtRow[14].ToString() != "" && dtRow[15].ToString() != "")
                {
                    DateTime StTime = DateTime.Parse(dtRow[14].ToString());
                    DateTime EnTime = DateTime.Parse(dtRow[15].ToString());
                    TimeSpan TimeDiff = EnTime.Subtract(StTime);
                    dtRow[16] = TimeDiff;

                }
                dtRow[17] = ds.Tables[0].Rows[i]["qc_op"];
                dtRow[18] = ds.Tables[0].Rows[i]["qc_st"];
                dtRow[19] = ds.Tables[0].Rows[i]["qc_et"];

                if (dtRow[18].ToString() != "" && dtRow[19].ToString() != "")
                {
                    DateTime StTime = DateTime.Parse(dtRow[18].ToString());
                    DateTime EnTime = DateTime.Parse(dtRow[19].ToString());
                    TimeSpan TimeDiff = EnTime.Subtract(StTime);
                    dtRow[20] = TimeDiff;

                }
                //dtRow[19] = ds.Tables[0].Rows[i]["DownloadtimeEST"];
                dtRow[21] = ds.Tables[0].Rows[i]["uploadtime"];

                string strtat = ds.Tables[0].Rows[i]["TAT_Rep"].ToString();
                if (strtat == "0") dtRow[22] = "No";
                else if (strtat == "1") dtRow[22] = "Yes";

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
                dtRow[23] = ds.Tables[0].Rows[i]["Delivered"];
                dtRow[24] = ds.Tables[0].Rows[i]["id"];

                if (k1 == "5" && qc == "5" && stat == "5" && review == "5") dtRow[25] = "Yes";
                else dtRow[25] = "No";

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

    public DataSet GetOrderCount(string strfrmdate, string strtodate)
    {
        mParam = new MySqlParameter[2];

        mParam[0] = new MySqlParameter("?$fdate", strfrmdate);
        mParam[0].MySqlDbType = MySqlDbType.VarChar;

        mParam[1] = new MySqlParameter("?$tdate", strtodate);
        mParam[1].MySqlDbType = MySqlDbType.VarChar;

        return con.ExecuteQuery("sp_getOrdercount", true, mParam);

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
            dcolumn.ColumnName = "05:30-06:30";
            dcolumn.Caption = "05:30-06:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "06:30-07:30";
            dcolumn.Caption = "06:30-07:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "07:30-08:30";
            dcolumn.Caption = "07:30-08:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "08:30-09:30";
            dcolumn.Caption = "08:30-09:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "09:30-10:30";
            dcolumn.Caption = "09:30-10:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "10:30-11:30";
            dcolumn.Caption = "10:30-11:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "11:30-12:30";
            dcolumn.Caption = "11:30-12:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "12:30-01:30";
            dcolumn.Caption = "12:30-13:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "01:30-02:30";
            dcolumn.Caption = "01:30-02:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "02:30-03:30";
            dcolumn.Caption = "02:30-03:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "03:30-04:30";
            dcolumn.Caption = "03:30-04:30";
            dcolumn.ReadOnly = true;
            dcolumn.Unique = false;
            dtTable.Columns.Add(dcolumn);

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "04:30-05:30";
            dcolumn.Caption = "04:30-05:30";
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
                    dtrow[1] = ds.Tables[0].Rows[i]["05:30 - 06:30"];
                    strdeliver += Convert.ToInt32(dtrow[1]);
                    if (dtrow[1].ToString() == "0") dtrow[1] = "";

                    dtrow[2] = ds.Tables[0].Rows[i]["06:30 - 07:30"];
                    strdeliver += Convert.ToInt32(dtrow[2]);
                    if (dtrow[2].ToString() == "0") dtrow[2] = "";

                    dtrow[3] = ds.Tables[0].Rows[i]["07:30 - 08:30"];
                    strdeliver += Convert.ToInt32(dtrow[3]);
                    if (dtrow[3].ToString() == "0") dtrow[3] = "";

                    dtrow[4] = ds.Tables[0].Rows[i]["08:30 - 09:30"];
                    strdeliver += Convert.ToInt32(dtrow[4]);
                    if (dtrow[4].ToString() == "0") dtrow[4] = "";

                    dtrow[5] = ds.Tables[0].Rows[i]["09:30 - 10:30"];
                    strdeliver += Convert.ToInt32(dtrow[5]);
                    if (dtrow[5].ToString() == "0") dtrow[5] = "";

                    dtrow[6] = ds.Tables[0].Rows[i]["10:30 - 11:30"];
                    strdeliver += Convert.ToInt32(dtrow[6]);
                    if (dtrow[6].ToString() == "0") dtrow[6] = "";

                    dtrow[7] = ds.Tables[0].Rows[i]["11:30 - 12:30"];
                    strdeliver += Convert.ToInt32(dtrow[7]);
                    if (dtrow[7].ToString() == "0") dtrow[7] = "";

                    dtrow[8] = ds.Tables[0].Rows[i]["12:30 - 01:30"];
                    strdeliver += Convert.ToInt32(dtrow[8]);
                    if (dtrow[8].ToString() == "0") dtrow[8] = "";

                    dtrow[9] = ds.Tables[0].Rows[i]["01:30 - 02:30"];
                    strdeliver += Convert.ToInt32(dtrow[9]);
                    if (dtrow[9].ToString() == "0") dtrow[9] = "";

                    dtrow[10] = ds.Tables[0].Rows[i]["02:30 - 03:30"];
                    strdeliver += Convert.ToInt32(dtrow[10]);
                    if (dtrow[10].ToString() == "0") dtrow[10] = "";

                    dtrow[11] = ds.Tables[0].Rows[i]["03:30 - 04:30"];
                    strdeliver += Convert.ToInt32(dtrow[11]);
                    if (dtrow[11].ToString() == "0") dtrow[11] = "";

                    dtrow[12] = ds.Tables[0].Rows[i]["04:30 - 05:30"];
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

            //dcolumn = new DataColumn();
            //dcolumn.DataType = System.Type.GetType("System.String");
            //dcolumn.ColumnName = "Process Type";
            //dcolumn.Caption = "Process Type";
            //dcolumn.ReadOnly = true;
            //dcolumn.Unique = false;
            //dtTable.Columns.Add(dcolumn);

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
                    //dtrow[4] = ds.Tables[0].Rows[i]["processtype"];
                    dtrow[4] = ds.Tables[0].Rows[i]["key_status"];
                    dtrow[5] = ds.Tables[0].Rows[i]["ProdQCTotalProcessTime"];
                    dtrow[6] = ds.Tables[1].Rows[i]["ProdQCTotalProcessTime"];
                    dtrow[7] = ds.Tables[2].Rows[i]["ProdQCTotalProcessTime"];
                    dtrow[8] = ds.Tables[0].Rows[i]["DeliveryDate"];
                    dtrow[9] = ds.Tables[0].Rows[i]["ProductionAttempts"];
                    dtrow[10] = ds.Tables[0].Rows[i]["QCAttempts"];

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
}

