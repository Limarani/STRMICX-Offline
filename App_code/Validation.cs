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
    //balaji......
    public string checkParcel(string orderno)
    {
        string result = "";
        DataSet ds = dbconn.ExecuteQuery("select taxid from tbl_taxparcel where orderno='" + orderno + "' and (status = 'C' or status = 'M')");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string txid = ds.Tables[0].Rows[i]["taxid"].ToString();


                string query = "select agencyid as op,authoritystatus from tbl_taxauthorities2 where orderno = '" + orderno + "' and taxid = '" + txid + "'";
                DataSet opcou = dbconn.ExecuteQuery(query);


                if (opcou.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < opcou.Tables[0].Rows.Count; j++)
                    {
                        string output = opcou.Tables[0].Rows[j]["op"].ToString();
                        string aus = opcou.Tables[0].Rows[j]["authoritystatus"].ToString();

                        if (output == "")
                        {
                            result = "ParcelNumber: " + ds.Tables[0].Rows[i]["taxid"] + " must have one Agency";
                        }
                        else if (output != "" && aus == "")
                        {
                            result = "Cannot Complete Order";
                        }
                    }
                } 
                else
                {
                    result = result = "ParcelNumber: " + ds.Tables[0].Rows[i]["taxid"] + " must have one Agency";
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