using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class hello : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string str1 = "";
        //GlobalClass gl = new GlobalClass();
        //myConnection con = new myConnection();
        if (!this.IsPostBack)
        {          
            string constr = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            //MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString);
            string query = "SELECT TaxType,TaxYear,ParcelId FROM tbl_search_tax_key;";
            query += "SELECT TaxType,TaxYear,ParcelId FROM tbl_search_tax_key1";
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            hfServerValue.Value = ds.ToString();
                              ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "ss('"+ hfServerValue.Value + "')", true);

                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                
                                gvEmployee.DataSource = ds.Tables[i];
                                gvEmployee.DataBind();
                              
                            }
                          

                        }
                    }
                }
            }
        }
    }
}