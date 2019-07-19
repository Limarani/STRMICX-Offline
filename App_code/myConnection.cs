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

/// <summary>
/// Summary description for myConnection
/// </summary>
public class myConnection
{
    string mysqlConnection = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
    MySqlConnection mConnection;
    MySqlDataAdapter mDa;
    MySqlCommand mCmd;
    MySqlDataReader mDr;
    MySqlParameter[] mParam;
    
    //public myConnection()
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //
    //}
    public string ConnectionString
    {
        //get { return ("server=192.168.10.6;database=tsit;uid=root;password=excel90();Pooling=false;default command timeout=99999;"); }
        get { return ( mysqlConnection = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString); }
    }
    private void openConnection()
    {
        mConnection = new MySqlConnection(mysqlConnection);
        if (mConnection.State == ConnectionState.Open)
        {
            mConnection.Close();
        }
        mConnection.Open();
    }
    public static void setError(string Message)
    {        
        myVariables.IsErr = true;
        myVariables.ErrMsg = Message;
    }
    public static void delError()
    {
        myVariables.IsErr = false;
        myVariables.ErrMsg = "";
    }
    public MySqlDataAdapter ExecuteSPAdapter(string query, bool isProcedure, MySqlParameter[] myParams)
    {
        delError();
        openConnection();
        mCmd = new MySqlCommand(query, mConnection);

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
            setError(mye.Number + " " + mye.Message);
            return mDa;
        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
    }
    public MySqlDataAdapter ExecuteSPAdapter(string query)
    {
        try
        {
            delError();
            mDa = new MySqlDataAdapter(query, mysqlConnection);
            return mDa;
        }
        catch (MySqlException mye)
        {
            setError(mye.Number + " " + mye.Message);
            return mDa;
        }
    }
    public DataSet ExecuteQuery(string query, bool isProcedure, MySqlParameter[] myParams)
    {
        delError();
        openConnection();
        mCmd = new MySqlCommand(query, mConnection);
        DataSet ds = new DataSet();
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
            mDa.Fill(ds);
            return ds;
        }
        catch (MySqlException mye)
        {
            setError(mye.Number + " " + mye.Message);
            return ds;
        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
    }
    public DataSet ExecuteQuery(string Query)
    {
        DataSet ds = new DataSet();
        delError();
        openConnection();
        try
        {
            mCmd = new MySqlCommand(Query, mConnection);
            ds = new DataSet();
            mDa = new MySqlDataAdapter(mCmd);
            mDa.Fill(ds);
            mConnection.Close();
            mConnection.Dispose();
            return ds;
        }
        catch (MySqlException mye)
        {
            setError(mye.Number + " " + mye.Message);
            return ds;
        }
     finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
    }
    public MySqlDataReader ExecuteSPReader(string query, bool isProcedure, MySqlParameter[] myParams)
    {
        delError();
        openConnection();
        mCmd = new MySqlCommand(query, mConnection);

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
            mDr = mCmd.ExecuteReader(CommandBehavior.CloseConnection);
            //mDr = mCmd.ExecuteReader();
            return mDr;
        }
        catch (MySqlException mye)
        {
            setError(mye.Number + " " + mye.Message);
            return mDr;
        }
        finally
        {
            //mConnection.Close();
            //mConnection.Dispose();
        }
    }
    public MySqlDataReader ExecuteStoredProcedure(string Query, bool isProcedure, MySqlParameter[] myParams)
    {
        delError();
        openConnection();
        mCmd = new MySqlCommand(Query, mConnection);

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
            mDr = mCmd.ExecuteReader();
            return mDr;
        }
        catch (MySqlException mye)
        {
            setError(mye.Number + " " + mye.Message);
            return mDr;
        }
    }

    public string ExecuteScalarst(string query)
    {
        string result = "";
        try
        {

            mConnection = new MySqlConnection(mysqlConnection);
            mCmd = new MySqlCommand(query, mConnection);
            mConnection.Open();
            result = mCmd.ExecuteScalar().ToString();
            mConnection.Close();
            mConnection.Dispose();
            return result;
        }
        catch (NullReferenceException)
        {
            result = "";
            return result;
        }
    }


    public MySqlDataReader ExecuteSPReader(string query)
    {
        delError();
        openConnection();
        mCmd = new MySqlCommand(query, mConnection);
        try
        {
            mDr = mCmd.ExecuteReader(CommandBehavior.CloseConnection);
            return mDr;
        }
        catch (MySqlException mye)
        {
            setError(mye.Number + " " + mye.Message);
            return mDr;
        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
    }  

    public int ExecuteSPNonQuery(string Query, bool isProcedure, MySqlParameter[] myParams)
    {
        int result;
        delError();
        openConnection();
        mCmd = new MySqlCommand(Query, mConnection);

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
            result = mCmd.ExecuteNonQuery();
        }
        catch (MySqlException mye)
        {
            if (mye.Number == 1062)
            {
                setError("Duplicate Entry: Name already found.");
            }
            else
            {
                setError(mye.Number + " " + mye.Message);
            }
            return -1;

        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
        return result;

    }
    public int ExecuteSPNonQuery(string Query)
    {
        int result;
        delError();
        openConnection();
        mCmd = new MySqlCommand(Query, mConnection);

        try
        {
            result = mCmd.ExecuteNonQuery();
        }
        catch (MySqlException mye)
        {
            if (mye.Number == 1062)
            {
                setError("Duplicate Entry: Name already found.");
            }
            else
            {
                setError(mye.Number + " " + mye.Message);
            }
            return -1;

        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
        return result;

    }   
    public int ExecuteSPScalar(string Query, bool isProcedure, MySqlParameter[] myParams)
    {
        int result;
        delError();
        openConnection();
        mCmd = new MySqlCommand(Query, mConnection);

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
            result = mCmd.ExecuteNonQuery();
        }
        catch (MySqlException mye)
        {
            if (mye.Number == 1062)
            {
                setError("Duplicate Entry: Name already found.");
            }
            else
            {
                setError(mye.Number + " " + mye.Message);
            }
            return -1;

        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
        return result;

    }
    public int ExecuteSPScalar(string Query)
    {
        delError();
        openConnection();
        mCmd = new MySqlCommand(Query, mConnection);

        try
        {
            return Convert.ToInt16(mCmd.ExecuteScalar());
        }
        catch (MySqlException mye)
        {

            setError(mye.Number + " " + mye.Message);
            return -1;

        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
    }
    public string ExecuteScalar(string Query)
    {
        delError();
        openConnection();
        mCmd = new MySqlCommand(Query, mConnection);

        try
        {
            return Convert.ToString(mCmd.ExecuteScalar());
        }
        catch (MySqlException mye)
        {

            setError(mye.Number + " " + mye.Message);
            return "";

        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
    }
    public string ExecuteScalar(string Query, bool isProcedure, MySqlParameter[] myParams)
    {
        string result;
        delError();
        openConnection();
        mCmd = new MySqlCommand(Query, mConnection);

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
            result = Convert.ToString(mCmd.ExecuteScalar());
        }
        catch (MySqlException mye)
        {
            if (mye.Number == 1062)
            {
                setError("Duplicate Entry: Name already found.");
            }
            else
            {
                setError(mye.Number + " " + mye.Message);
            }
            return "";

        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
        return result;

    }
    public DataSet ExecuteTables(string[] Querys)
    {
        DataSet ds = new DataSet();
        delError();
        openConnection();
        mCmd = new MySqlCommand();
        mCmd.Connection = mConnection;
        mDa = new MySqlDataAdapter();
        mDa.SelectCommand = mCmd;
        try
        {
            int count = 0;
            foreach (string query in Querys)
            {
                mDa.SelectCommand.CommandText = query;
                mDa.Fill(ds, "Table" + count);
                count++;
            }
            return ds;
        }
        catch (MySqlException mye)
        {
            setError(mye.Number + " " + mye.Message);
            return ds;
        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }

    }
    public DataSet ExecuteQuery1(string Query)
    {
        DataSet ds = new DataSet();
        delError();
        openConnection();
        try
        {
            mCmd = new MySqlCommand(Query, mConnection);
            ds = new DataSet();
            mDa = new MySqlDataAdapter(mCmd);
            mDa.Fill(ds);
            mConnection.Close();
            mConnection.Dispose();
            return ds;
        }
        catch (MySqlException mye)
        {
            setError(mye.Number + " " + mye.Message);
            return ds;
        }
    }
    public MySqlDataAdapter ExecuteSPAdapter1(string query)
    {
        delError();
        openConnection();
        mCmd = new MySqlCommand(query, mConnection);
        mCmd.CommandType = CommandType.StoredProcedure;
        try
        {
            mDa = new MySqlDataAdapter(mCmd);
            return mDa;
        }
        catch (MySqlException mye)
        {
            setError(mye.Number + " " + mye.Message);
            return mDa;
        }
        finally
        {
            mConnection.Close();
            mConnection.Dispose();
        }
    }

}
