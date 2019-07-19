using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// 
/// </summary>
public class GridViewExportUtil
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    /// 
    public static void getdata(string fileName, GridView gv)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                Table table = new Table();
                
                //if (gv.HeaderRow != null)
                //{
                //    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                //    table.Rows.Add(gv.HeaderRow);
                //}
                
                foreach (GridViewRow row in gv.Rows)
                {
                    
                    CheckBox c1 = (CheckBox)row.FindControl("chkSelect");
                    if (c1.Checked == true)
                    {
                        GridViewExportUtil.PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                }
                table.RenderControl(htw);


                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }
    public static void getdata1(string fileName, GridView gv, Int16 prezerocol)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                Table table = new Table();
                if (gv.HeaderRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }
                foreach (GridViewRow row in gv.Rows)
                {
                    CheckBox c1 = (CheckBox)row.FindControl("chkSelect");
                    if (c1.Checked == true)
                    {
                        GridViewExportUtil.PrepareControlForExport(row);
                        row.Cells[prezerocol].Text = "&nbsp;" + row.Cells[prezerocol].Text;
                        table.Rows.Add(row);
                    }

                }
                table.RenderControl(htw);


                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }


    public static void Export(string fileName, GridView gv)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();
                TableRow tr = new TableRow();
                TableCell td = new TableCell();

                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    GridViewExportUtil.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }
    public static void Export1(string fileName, GridView gv, Int16 prezerocol)
            {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();

                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    GridViewExportUtil.PrepareControlForExport(row);
                    row.Cells[prezerocol].Text = "&nbsp;" + row.Cells[prezerocol].Text;
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }

    /// <summary>
    /// Replace any of the contained controls with literals
    /// </summary>
    /// <param name="control"></param>
    private static void PrepareControlForExport(Control control)
    {
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
              //  control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
            }

            if (current.HasControls())
            {
                GridViewExportUtil.PrepareControlForExport(current);
            }
        }
    }

    
}



/*
Aspx :

 

 <asp:DataGrid  ID="Grid1" runat="server" AutoGenerateColumns="false">
       <Columns>    
      <asp:TemplateColumn>
         <ItemTemplate>
           <asp:CheckBox ID="ch1" runat="server" />
         </ItemTemplate>
      </asp:TemplateColumn>     
      <asp:BoundColumn DataField="Ename" HeaderText="Employee Name" />
      <asp:BoundColumn DataField="Eno" HeaderText="Employee No" />
    </Columns>
    </asp:DataGrid>
   
    <asp:Button Text="Export to Excel" ID="btn1" runat="server" OnClick="btn1_Click" />

 
protected void ExportDataGrid(GridView gV, Label eLabel)
    {
        TableRow tr = new TableRow();
        tr.Cells = gV.HeaderRow.Cells;
    }
    
 

Aspx.cs:

 using System.Text;

 protected void btn1_Click(object sender, EventArgs e)
    {
        StringBuilder str1 = new StringBuilder();
        str1.Append("<html><body><table>");
        foreach(DataGridItem dt in Grid1.Items)
        {
            CheckBox ch = new CheckBox();
            ch = (CheckBox)dt.Cells[0].FindControl("ch");
            if (ch.Checked)
            {
                str1.Append("<tr>");
                str1.Append("<td>" + dt.Cells[1].Text + "</td>");
                str1.Append("</tr>");

                str1.Append("<tr>");
                str1.Append("<td>" + dt.Cells[2].Text + "</td>");
                str1.Append("</tr>");


            }
        }
        str1.Append("</html></body></table>");
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=export.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        Response.ContentEncoding = System.Text.Encoding.Default;
        Response.Write(str1.ToString());
        Response.Flush();
        Response.End();
    } 
   if (gv.HeaderRow != null)
        {
            for (integer i = 0; i < gv.HeaderRow.Cells.Count - 1;i++ )
            {
                sw.Write(gv.HeaderRow.Cells[i].Text);
            }
            sw.WriteLine();
        }
*/