using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_ScrapingPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["order"] != null)
            {
                imagedisplay(Session["order"].ToString());
            }

        }

    }

    public void imagedisplay(string ordernumber)
    {
        try
        {
            Response.Redirect("http://173.192.83.98/restservice/pdf/" + ordernumber + ".pdf");
        }
        catch
        {

        }


    }
}