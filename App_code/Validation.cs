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
    //balaji
    public string checkParcel(string orderno)
    {
        string result = "";
        DataSet ds = dbconn.ExecuteQuery("select taxid from tbl_taxparcel where orderno='" + orderno + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string txid = ds.Tables[0].Rows[i]["taxid"].ToString();


                string query = "select count(agencyid) as op,authoritystatus from tbl_taxauthorities2 where orderno = '" + orderno + "' and taxid = '" + txid + "' and authoritystatus = '" + "2" + "'";                
                DataSet opcou = dbconn.ExecuteQuery(query);
                string output = opcou.Tables[0].Rows[0]["op"].ToString();
                string aus = opcou.Tables[0].Rows[0]["authoritystatus"].ToString();             

                if (output == "0")
                {
                    result = "ParcelNumber: " + ds.Tables[0].Rows[i]["taxid"] + " must have one Agency";
                    return result;
                }                
            }
        }
        else
        {
            return "OrderNumber must have one Taxid";
        }
        return result;
    }
}