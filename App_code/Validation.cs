using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Validation
/// </summary>
public class Validation
{
    DBConnection dbconn = new DBConnection();
    public Validation()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //must have 1 parcel
    public string checkParcel(string orderno)
    {
        string result = "";
        DataSet ds = dbconn.ExecuteQuery("select taxid from tbl_taxparcel where orderno='" + orderno + "'");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            int count = dbconn.ExecuteSPNonQuery("select count(agnecyid) from tbl_taxauthorities2 where orderno='" + orderno + "' and taxid='" + ds.Tables[0].Rows[i]["taxid"] + "'");
            if (count == 0)
            {
                result = "ParcelNumber: " + ds.Tables[0].Rows[i]["taxid"] + "";
            }
        }

        return result;

    }

}