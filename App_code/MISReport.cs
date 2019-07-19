using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for MISReport
/// </summary>
public class MISReport
{
    #region Declaration
    MySqlParameter[] mparam;
    myConnection db = new myConnection();
    MySqlConnection mConnection;
    MySqlDataAdapter mDa;
    MySqlCommand mCmd;
    MySqlDataReader mDr;
    #endregion

    #region TATReport
    public DataTable TATReport(string ReportDate)
    {
        try
        {
            DataSet DSresult = new DataSet();
            MySqlDataAdapter mda;
            mparam = new MySqlParameter[1];
            mparam[0] = new MySqlParameter("?$Date", ReportDate);
            mparam[0].MySqlDbType = MySqlDbType.VarChar;
            mda = db.ExecuteSPAdapter("SP_Get_MIS_Data", true, mparam);
            mda.Fill(DSresult, "Table0");
            if (DSresult.Tables.Count > 0)
                return DSresult.Tables[0];
            else
                return null;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region ConvertUploadtoDataview
    public DataTable ConvertUploadtoDataview(int review, string strfrmdate, string strtodate)
    {
        int reviews = review;

        string strfromtime = string.Empty;
        string strtotime = string.Empty;
        DataSet ds = new DataSet();
        DataTable dtTable = new DataTable();
        DataColumn dcolumn;
        try
        {

            dcolumn = new DataColumn();
            dcolumn.DataType = System.Type.GetType("System.String");
            dcolumn.ColumnName = "Timing";
            dcolumn.Caption = "Timing";
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

            mparam = new MySqlParameter[5];
            mparam[0] = new MySqlParameter("$review", reviews);
            mparam[0].MySqlDbType = MySqlDbType.Int32;

            mparam[1] = new MySqlParameter("$fromdate", strfrmdate);
            mparam[1].MySqlDbType = MySqlDbType.VarChar;

            mparam[2] = new MySqlParameter("$todate", strtodate);
            mparam[2].MySqlDbType = MySqlDbType.VarChar;


            int h = 17;
            int h1 = 18;
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

                    mparam[3] = new MySqlParameter("$fromtiming", strfromtime);
                    mparam[3].MySqlDbType = MySqlDbType.VarChar;

                    mparam[4] = new MySqlParameter("$totiming", strtotime = strfromtime == "23:00:00" ? "24:00:00" : strtotime);
                    mparam[4].MySqlDbType = MySqlDbType.VarChar;



                    ds = db.ExecuteQuery("sp_uploadpattern_MIS", true, mparam);

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

                    h = h + 1;
                    h1 = h1 + 1;
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


            return dtTable;

        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    #endregion

    #region GetReportDate()
    public string GetReportDate(string date)
    {
        try
        {
            DateTime ReportDate = Convert.ToDateTime(date);
            return ReportDate.ToString("MM/dd/yyyy");
            //2012-11-16
            //return date;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string GetReportDateWithTime(string Date)
    {
        try
        {
            DateTime DT = Convert.ToDateTime(Date);
            return DT.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("hh:mm:ss");

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string GetReportDateymd(string Date)
    {
        try
        {
            DateTime ReportDate = Convert.ToDateTime(Date);
            return ReportDate.ToString("yyyy-MM-dd");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region GetReportTable
    public string GetReportTable()
    {
        string Tblname = "report_tsitaxes";
        return Tblname;
    }
    #endregion

    #region GetConString
    public string GetConString()
    {
        //string Strcon = "server=192.168.10.13;database=stringreports;uid=root;password=excel90();pooling=false;";
        string Strcon = "server=10.0.1.17;database=stringreports;uid=root;password=string;pooling=false;";
        return Strcon;
    }
    #endregion

    #region GetMISProjectName
    //public string GetServernameValue(string ServerName)
    public string GetMISProjectName()
    {
        try
        {
            return "c1393d5b66fc6b3bfea9637afe0ee701";
        }
        catch (Exception ex)
        {
            throw ex;
        }



    }
    #endregion

    #region DatabaseSideCodings
    #region openConnection
    public MySqlConnection openConnection(string ConnectionString)
    {
        mConnection = new MySqlConnection(ConnectionString);
        if (mConnection.State == ConnectionState.Open)
        {
            mConnection.Close();
        }
        mConnection.Open();
        return mConnection;
    }
    #endregion
    #region closeConnection
    public void closeConnection(string ConnectionString)
    {
        mConnection = new MySqlConnection(ConnectionString);
        if (mConnection.State == ConnectionState.Open)
        {
            mConnection.Close();
        }
        mConnection.Dispose();
    }
    #endregion
    #region ExecuteQuery
    public DataSet ExecuteQuery(string Query, string ConnectionString)
    {
        DataSet ds;
        openConnection(ConnectionString);
        mCmd = new MySqlCommand(Query, mConnection);
        mCmd.CommandTimeout = 400;

        ds = new DataSet();
        mDa = new MySqlDataAdapter(mCmd);
        mDa.Fill(ds);

        mConnection.Close();
        mConnection.Dispose();
        return ds;

    }
    #endregion
    #region ExecuteNonQuery
    public int ExecuteNonQuery(string Query, string ConnectionString)
    {


        try
        {
            int result;
            openConnection(ConnectionString);
            mCmd = new MySqlCommand(Query, mConnection);
            mCmd.CommandTimeout = 400;
            result = mCmd.ExecuteNonQuery();
            return result;
            mConnection.Close();
            mConnection.Dispose();
        }
        catch (Exception ex)
        {
            throw ex;

        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }

    }
    #endregion
    #region ExecuteSPAdapter
    public MySqlDataAdapter ExecuteSPAdapter(string query, bool isProcedure, MySqlParameter[] myParams, string ConnectionString)
    {
        openConnection(ConnectionString);
        mCmd = new MySqlCommand(query, mConnection);
        mCmd.CommandTimeout = 400;
        if (isProcedure)
        {
            mCmd.CommandType = CommandType.StoredProcedure;
            if (myParams != null)
            {
                foreach (MySqlParameter param in myParams)
                {
                    mCmd.Parameters.Add(param);
                }
            }
        }
        try
        {
            mDa = new MySqlDataAdapter(mCmd);
            return mDa;
        }
        catch (MySqlException mye)
        {
            return mDa;
        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
    }
    #endregion
    #endregion
}
