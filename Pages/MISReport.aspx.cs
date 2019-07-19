using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Pages_MISReport : System.Web.UI.Page
{
    #region Declaration
    MISReport ObjMisReport = new MISReport();
    GlobalClass gblcls = new GlobalClass();
    myConnection gl = new myConnection();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionHandler.UserName == "")
        {
            Response.Redirect("LoginPage.aspx");
        }
		if (SessionHandler.IsAdmin == false)
        {
            SessionHandler.UserName = "";
            Response.Redirect("Loginpage.aspx");
        }
        if (!Page.IsPostBack) txtDate.Text = gblcls.setdate();
        btnSubmit.Attributes.Add("onclick", "if(confirm('Do you Want To Submit it This Report To MIS For - " + txtDate.Text + "'))return true;else return false");
        btnSubmit.Visible = false;


    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            LblError.Text = "";
            btnSubmit.Visible = true;
            if (txtDate.Text.Trim() == "")
            {
                LblError.Text = "Please Select The Date..!";
                return;
            }
            LoadDownloadUploadPatter();
            LoadTAT();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region LoadDownloadUploadPatter
    private void LoadDownloadUploadPatter()
    {
        try
        {
            int review = 2;
            DataTable Dt = ObjMisReport.ConvertUploadtoDataview(review, ObjMisReport.GetReportDate(txtDate.Text), ObjMisReport.GetReportDate(txtDate.Text));
            GridView1.DataSource = ConvertThepatternToCustomFormat(Dt);
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region LoadTAT
    private void LoadTAT()
    {
        try
        {
            DataTable DtResult = new DataTable();
            string ReportType = "";
            DtResult = ObjMisReport.TATReport(ObjMisReport.GetReportDate(txtDate.Text));
            if (DtResult.Rows.Count > 0)
            {
                GridViewResult.DataSource = DtResult;
                GridViewResult.DataBind();
            }
            else
            {
                GridViewResult.DataSource = null;
                GridViewResult.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region SubmitDownloadUploadPattern
    private string SubmitDownloadUploadPattern()
    {
        try
        {


            string errMsg = "";
            string StrQuery = "";
            string ReportDate = ObjMisReport.GetReportDate(txtDate.Text);
            string PrID = ObjMisReport.GetMISProjectName();
            if (!isExistDUPToday())
            {

                StrQuery = "Insert Into downupload_pattern( fdate,  tdate,  project,  Hours,  download,  upload) Values";
                GridView GVDUP = new GridView();
                GVDUP = GridView1;

                for (int i = 1; i < 25; i++)
                {
                    string Hour = GVDUP.Columns[i].HeaderText.ToString();
                    Label Lbldownload = (Label)GVDUP.Rows[0].FindControl("hour" + (i - 1));
                    Label Lblupload = (Label)GVDUP.Rows[1].FindControl("hour" + (i - 1));
                    StrQuery += "('" + ObjMisReport.GetReportDateWithTime(txtDate.Text) + "','" + ObjMisReport.GetReportDateWithTime(txtDate.Text) + "','" + PrID + "','" + Hour + "','" + Lbldownload.Text + "','" + Lblupload.Text + "')";

                    if (i < 24)
                    {
                        StrQuery += ",";
                    }
                }

                int Result1 = ObjMisReport.ExecuteNonQuery(StrQuery, ObjMisReport.GetConString());

                if (Result1 > 0)
                {
                    errMsg = "Today Download Upload Pattern Successfully Submitted MIS..!";
                }


            }
            else
            {
                errMsg = "Today Download Upload Already Submitted..!";
            }
            return errMsg;



        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    #endregion

    #region SubmitTATData
    private string SubmitTATData()
    {
        try
        {
            string errMsg = "";

            if (!isExistTATToday())
            {
                string PrID = ObjMisReport.GetMISProjectName();
                string StrQuery = "";
                StrQuery = "Insert Into " + ObjMisReport.GetReportTable() + "(Date,UserName,KeyCount, AvgKeyTime, QCCount, AVGQCTime, ReviewCount, AvgReviewTime) Values";
                GridView GVResultTAT = new GridView();
                GVResultTAT = GridViewResult;
                int Counter = 1;
                foreach (GridViewRow R in GVResultTAT.Rows)
                {
                    StrQuery += "('" + ObjMisReport.GetReportDateymd(txtDate.Text) + "','" + R.Cells[0].Text + "','" + R.Cells[1].Text + "','" + R.Cells[2].Text + "','" + R.Cells[3].Text + "','" + R.Cells[4].Text + "','" + R.Cells[5].Text + "','" + R.Cells[6].Text + "')";

                    if (Counter < GVResultTAT.Rows.Count)
                    {
                        StrQuery += ",";
                    }
                    Counter += 1;
                }

                //foreach (GridViewRow R in GVResultTAT.Rows)
                //{
                //    StrQuery += "('" + ObjMisReport.GetReportDateymd(txtDate.Text) + "','" + PrID + "','" + R.Cells[0].Text + "','" + R.Cells[1].Text + "','" + R.Cells[2].Text + "','" + R.Cells[3].Text + "','" + R.Cells[4].Text + "','" + R.Cells[5].Text + "','" + R.Cells[6].Text + "','" + R.Cells[7].Text + "','" + R.Cells[8].Text + "')";

                //    if (Counter < GVResultTAT.Rows.Count)
                //    {
                //        StrQuery += ",";
                //    }
                //    Counter += 1;
                //}

                int Result1 = ObjMisReport.ExecuteNonQuery(StrQuery, ObjMisReport.GetConString());
                errMsg += "Today TAT Report Successfully Submitted MIS..!";
            }
            else
            {
                errMsg += "Today TAT Report Already Submitted...!";
            }

            return errMsg;
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    #endregion

    #region isExistTATToday
    private bool isExistTATToday()
    {
        try
        {
            string PrID = ObjMisReport.GetMISProjectName();
            bool Status = false;

            string StrQueryReport = "select count(id) as Cnt from " + ObjMisReport.GetReportTable() + " r where r.Date='" + ObjMisReport.GetReportDateymd(txtDate.Text) + "';";
            DataSet DSResult = ObjMisReport.ExecuteQuery(StrQueryReport, ObjMisReport.GetConString());

            if (Convert.ToInt32(DSResult.Tables[0].Rows[0]["Cnt"].ToString()) > 0)
            {
                Status = true;
            }
            else if (Convert.ToInt32(DSResult.Tables[0].Rows[0]["Cnt"].ToString()) == 0)
            {
                Status = false;

            }
            return Status;

        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    #endregion

    #region isExistDUPToday
    private bool isExistDUPToday()
    {
        try
        {
            string PrID = ObjMisReport.GetMISProjectName();
            bool Status = false;
            string StrQueryReport = "select count(d.id)  as Cnt from downupload_pattern d  where date(d.fdate)='" + ObjMisReport.GetReportDateymd(txtDate.Text) + "' and d.project='" + PrID + "';";
            DataSet DSResult = ObjMisReport.ExecuteQuery(StrQueryReport, ObjMisReport.GetConString());
            if (Convert.ToInt32(DSResult.Tables[0].Rows[0]["Cnt"].ToString()) > 0)
            {

                Status = true;
            }
            else if (Convert.ToInt32(DSResult.Tables[0].Rows[0]["Cnt"].ToString()) == 0)
            {
                Status = false;
            }
            return Status;

        }
        catch (Exception ex)
        {
            throw ex;

        }


    }
    #endregion

    #region ConvertThepatternToCustomFormat
    private DataTable ConvertThepatternToCustomFormat(DataTable DTResult)
    {
        try
        {
            string PrID = "4c6d10b81e0889340486e9a00b2c53cb";

            DataTable DtCustom = new DataTable();
            DtCustom.Columns.Add("Tittle", typeof(string));

            for (int i = 0; i < (DTResult.Rows.Count - 1); i++)
            {
                DtCustom.Columns.Add(DTResult.Rows[i][0].ToString(), typeof(string));
            }

            DataRow Dr = DtCustom.NewRow();
            Dr["Tittle"] = "Download";

            for (int i = 0; i < (DTResult.Rows.Count - 1); i++)
            {
                Dr[DTResult.Rows[i][0].ToString()] = DTResult.Rows[i][1].ToString();
            }
            DtCustom.Rows.Add(Dr);


            DataRow Dr1 = DtCustom.NewRow();
            Dr1["Tittle"] = "Upload";

            for (int i = 0; i < (DTResult.Rows.Count - 1); i++)
            {
                Dr1[DTResult.Rows[i][0].ToString()] = DTResult.Rows[i][2].ToString();
            }
            DtCustom.Rows.Add(Dr1);
            GridView1.DataSource = DtCustom;
            GridView1.DataBind();
            return DtCustom;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDate.Text.Trim() == "")
            {
                LblError.Text = "Please Select The Date..!";
                return;
            }
            //if (DDlProcess.SelectedValue.ToString().ToLower() == "Selet Process")
            //{
            //    LblError.Text = "Please Select The Process..!";
            //    return;
            //}
            string ErrMsg = SubmitDownloadUploadPattern();
            if (GridViewResult.Rows.Count > 0)
            {
                ErrMsg += ErrMsg != "" ? " And  " : "";
                ErrMsg += SubmitTATData();
            }
            LblError.Text = ErrMsg;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Page_preInit
    protected void Page_preInit(object sender, EventArgs e)
    {

        Page.Theme = SessionHandler.Theme;
    }


    #endregion
}
