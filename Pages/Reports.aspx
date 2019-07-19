<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true"
    CodeFile="Reports.aspx.cs" Theme="Black" Inherits="Pages_Reports" Title="REPORTS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   
    <style type="text/css">
      .selected { color: red; }
    </style>
    <script type="text/javascript" src="../Script/sidemenu.js"></script>   
     <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
    
    
    <script language="javascript" type="text/javascript" src="../Script/datetimepicker.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script type="text/javascript">
        $('#navlist a').click(function (e) {
            e.preventDefault(); //prevent the link from being followed
            $('#navlist a').removeClass('selected');
            $(this).addClass('selected');
        });
    </script>
<script type="text/javascript">
    function txtFromToDate()
    {
    alert('Please Select From Date and To Date')
    }
    function txtFromDate() {
        alert('Please Select From Date')
    }
    function txtToDate() {
        alert('Please Select To Date')
    }
    function txtNoRecord()
    {
        alert('No Record Found')
    }
</script> 
    <table border="0" width="100%" style="background-color: #f5f5f5; height: 100%">
        
        <tr>
            <td valign="top" style="width: 300px; height: 100%">
                <table border="0" align="center">
                    <tr>
                        <td align="center">
                            <asp:CheckBox ID="Chkrefresh" runat="server" CssClass="Autorefresh" OnCheckedChanged="Chkrefresh_CheckedChanged"
                                AutoPostBack="true" Text="Auto Refresh" />
                            <asp:Timer id="Refresh" runat="server" Interval="120000" Enabled="false" OnTick="Refresh_Tick">
                            </asp:Timer>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-top: 10px; padding-left: 50px;">
                            <div class="urbangreymenu">
                                <h3 class="headerbar">
                                    EOD</h3>
                                <ul id="navlist">
                                    <li>
                                        <asp:LinkButton ID="Lnkeod1" runat="server"  OnClick="Lnkeod1_Click">Refinance</asp:LinkButton></li><li>
                                        <asp:LinkButton ID="Lnkeod2" runat="server"  OnClick="Lnkeod2_Click">Consolidated</asp:LinkButton></li><li>
                                        <asp:LinkButton ID="Lnkeod3" runat="server"  OnClick="Lnkeod3_Click">Purchase</asp:LinkButton></li>
										<li>
                                        <asp:LinkButton ID="LnkFpyReport" runat="server" class="activeBtn"   OnClick="LnkFpyReport_Click">FPY Report</asp:LinkButton></li>
										</ul>
                           
                                <h3 class="headerbar">
                                    MailAway</h3>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="Lnkmailaway" runat="server" OnClick="Lnkmailaway_Click">Mailaway Regular</asp:LinkButton></li><li>
                                        <asp:LinkButton ID="Lnkmailwayups" runat="server" OnClick="Lnkmailwayups_Click">Mailaway UPS</asp:LinkButton></li></ul>
                                <h3 class="headerbar">
                                    Consolidated</h3>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="Lnkbtnreglrcon" runat="server" OnClick="Lnkbtnreglrcon_Click">Regular Consolidated</asp:LinkButton></li><li>
                                        <asp:LinkButton ID="Lnkbtnupscon" runat="server" OnClick="Lnkbtnupscon_Click">UPS Consolidated</asp:LinkButton></li><li>
                                        <asp:LinkButton ID="Lnkbtnupstemp" runat="server" OnClick="Lnkbtnupstemp_Click">UPS Template</asp:LinkButton></li></ul>
                        
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="Lnkbtnlogchklst" runat="server" OnClick="Lnkbtnlogchklst_Click" Visible="false">Login Checklist</asp:LinkButton>

                                    </li>
                                    <li>
                                        <asp:LinkButton ID="Lnkbtnchklst" runat="server" OnClick="Lnkbtnchklst_Click" Visible="false">Checklist</asp:LinkButton>

                                    </li>
                                    <li>
                                        <asp:LinkButton ID="Lnkdeletelogin" runat="server" OnClick="Lnkdeletelogin_Click">Update Login</asp:LinkButton>

                                    </li>
                                        <li><asp:LinkButton ID="LnkKeyingUpdate" runat="server" OnClick="LnkKeyingUpdatedlogin_Click">Keying Mismatch Data</asp:LinkButton></li></ul>
                                <h3 class="headerbar">
                                    Mailaway Request</h3>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="LnkRegrequest" runat="server" OnClick="LnkRegrequest_Click">Regular Consolidated Request</asp:LinkButton></li></ul>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="Lnkupsrequest" runat="server" OnClick="Lnkupsrequest_Click">UPS Consolidated Request</asp:LinkButton></li></ul>
                                <h3 class="headerbar">
                                    Break Time</h3>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="Lnkbtnbreak" runat="server" Visible="false" OnClick="Lnkbtnbreak_Click">Dinner Break</asp:LinkButton></li><li>
                                        <asp:LinkButton ID="Lnkbtnunlock" runat="server" OnClick="Lnkbtnunlock_Click">Production Unlock</asp:LinkButton></li>
                                        <li>
                                        <asp:LinkButton ID="lnkbtnbrektot" runat="server" onclick="Lnk_break_rpt" >Break time</asp:LinkButton></li></ul>
                           
                                <h3 class="headerbar">
                                    Hourly Count</h3>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="Lnkhourcount" runat="server" OnClick="Lnkhourcount_Click">Hourly Count Report</asp:LinkButton></li>
<li><asp:LinkButton ID="lnkhourcounttotal" runat="server" 
                                        onclick="lnkhourcounttotal_Click">Hourly Count Total</asp:LinkButton></li>
</ul>

                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="height: 100%; vertical-align: top;">
                <table style="width: 100%; height: 100%" border="0">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblerror" runat="server" Text="" Font-Names="Verdana" ForeColor="Red"></asp:Label></td>
                    </tr>
<%--                      <tr>
                        <td align="center">
                            <asp:Label ID="lblrr" runat="server" Font-Names="Verdana" ForeColor="Red"></asp:Label></td>
                    </tr>--%>
                    <tr>
                        <td align="center" valign="top">
                            <asp:Panel ID="PanelKeyingList" runat="server" align="center">
                                <table cellspacing="5" cellpadding="5" width="480" border="0">
                                     <tr>
                                        <h3 id="KeyingUpdate" runat="server">Keying Mismatch Data</h3>
                                         
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="Lnkexport" runat="server" ImageUrl="~/App_themes/Black/images/Excel.png"
                                                Height="50px" Width="50px" ToolTip="Export" OnClick="Lnkexport_Click" />
                                        </td>
                                        <td class="Lblall" align="right">
                                            From</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtfrmdate" runat="server" CssClass="txtuser" Width="100px" autocomplete="off"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfrmdate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="Lblall" align="right">
                                            To</td>
                                        <td align="left">
                                            <asp:TextBox ID="txttodate" runat="server" CssClass="txtuser" Width="100px" autocomplete="off"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttodate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="Lblall" align="right">
                                            <asp:Label ID="lblusername" runat="server" Text="Username" CssClass="Lbldata"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="ddlusername" runat="server" CssClass="txtuser" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="PanelAssign" runat="server" Width="1050px" ScrollBars="auto" align="center" Height="1250px">
                                <asp:GridView ID="GridEOD" runat="server" Width="100%" FooterStyle-CssClass="Gridfooterbar"
                                    ShowFooter="false" AutoGenerateColumns="true" GridLines="None" CssClass="Gnowrap"
                                    AlternatingRowStyle-CssClass="alt"  >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          
                                    </Columns>

                                </asp:GridView>
                                 <asp:GridView ID="Gridtatreport" runat="server" Width="100%" FooterStyle-CssClass="Gridfooterbar"
                                    ShowFooter="false" AutoGenerateColumns="true" GridLines="None" CssClass="Gnowrap"
                                    AlternatingRowStyle-CssClass="alt" OnRowDataBound="Gridtatreport_RowDataBound">
                                       <Columns>
                                          <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                    </Columns>

                                    </asp:GridView> 
                                 <asp:GridView ID="GridTat" runat="server" Width="150%" FooterStyle-CssClass="Gridfooterbar" DataKeyNames="OrderNo"
                                ShowFooter="false" AutoGenerateColumns="false" GridLines="None" CssClass="Gnowrap"
                                    AlternatingRowStyle-CssClass="alt" OnRowDataBound="GridTat_RowDataBound">                                  
                                    <Columns>
                                       <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="pdate" HeaderText="Date" />
                                        <asp:BoundField DataField="OrderNo" HeaderText="OrderNo" />
                                        <asp:BoundField DataField="DownloadTime" HeaderText="DownloadTime" />
                                        <asp:BoundField DataField="UploadTime" HeaderText="UploadTime" />
                                        <asp:BoundField DataField="OrderType" HeaderText="OrderType" />
                                        <asp:BoundField DataField="Tat" HeaderText="TatAchieved" />
                                        <asp:BoundField DataField="pdate" HeaderText="pdate" />
                                        <asp:BoundField DataField="DeliveredDate" HeaderText="DeliveredDate" />
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="GridView1" runat="server" Width="100%" FooterStyle-CssClass="Gridfooterbar"
                                    ShowFooter="false" AutoGenerateColumns="true" GridLines="None" CssClass="Gnowrap"
                                    AlternatingRowStyle-CssClass="alt">
                                </asp:GridView>
                                <asp:GridView ID="GridView2" runat="server" Width="100%" FooterStyle-CssClass="Gridfooterbar"
                                    ShowFooter="false" AutoGenerateColumns="true" GridLines="None" CssClass="Gnowrap"
                                    AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="GridQuality" runat="server" Width="150%" FooterStyle-CssClass="Gridfooterbar" DataKeyNames="Order_No"
                                ShowFooter="false" AutoGenerateColumns="false" GridLines="None" CssClass="Gnowrap"
                                    AlternatingRowStyle-CssClass="alt" OnRowCommand="GridQuality_RowCommand" OnRowDeleting="GridQuality_RowDeleting" 
                                    OnRowEditing="GridQuality_RowEditing">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order No">
                                            <ItemTemplate>
                                                <asp:LinkButton CommandName="Edit" CssClass="linkbtn" ID="Editbtn" runat="server" Text='<%# Eval("Order_No") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Pdate" HeaderText="Date" />
                                        <asp:BoundField DataField="State" HeaderText="State" />
                                        <asp:BoundField DataField="County" HeaderText="County" />
                                        <asp:BoundField DataField="ErrorField" HeaderText="ErrorField" />
                                        <asp:BoundField DataField="Incorrect" HeaderText="Incorrect" />
                                        <asp:BoundField DataField="Correct" HeaderText="Correct" />
                                        <asp:BoundField DataField="ErrorComments" HeaderText="ErrorComments" />
                                        <asp:BoundField DataField="Review_OP" HeaderText="Review Person"/>
                                        <asp:BoundField DataField="AcceptedBy" HeaderText="AcceptedBy" />
                                        <asp:BoundField DataField="Cause" HeaderText="Cause" />
                                        <asp:BoundField DataField="Suggestion" HeaderText="Suggestion" />
                                    </Columns>
                                </asp:GridView>
                                
                            </asp:Panel>
                            <asp:Panel ID="PanelBreak" runat="server" ScrollBars="Auto" Height="480px" Width="1000px">
                   
                       
                        <asp:GridView ID="grd_break_total" runat="server" CssClass="Gnowrap" SkinID="Classic"
                            Width="910px" AutoGenerateColumns="false" OnRowDataBound="grd_break_total_RowDataBound">
                            <HeaderStyle Font-Names="Georgia" Font-Size="13px" ForeColor="white" Font-Bold="true"
                                Font-Underline="true" />
                            <RowStyle HorizontalAlign="Center" />
                            <EmptyDataRowStyle BackColor="LightBlue" ForeColor="Red" />
                            <EmptyDataTemplate>
                                <asp:Image ID="NoDataImage" ImageUrl="~/images/Image.jpg" AlternateText="No Image"
                                    runat="server" />
                                No Data Found.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <img alt = "" style="cursor: pointer" src="../images/plus.png" />
                                       
                                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="200px" DataField="Date" HeaderText="Date" />
                                                    <asp:BoundField ItemStyle-Width="200px" DataField="break_out" HeaderText="Start" />
                                                    <asp:BoundField ItemStyle-Width="200px" DataField="break_in" HeaderText="End" />
                                                    <asp:BoundField ItemStyle-Width="200px" DataField="Time Taken" HeaderText="Time Taken" />
                                                     <asp:BoundField ItemStyle-Width="200px" DataField="break_type" HeaderText="Break Type" />
                                                      <asp:BoundField ItemStyle-Width="200px" DataField="break_comments" HeaderText="Comments" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="150px" DataField="Name" HeaderText="Name" />
                                <asp:BoundField ItemStyle-Width="150px" DataField="Date" HeaderText="Date" />
                                <asp:BoundField ItemStyle-Width="150px" DataField="Time Taken" HeaderText="Time Taken" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>

                        
                            <asp:Panel ID="PanelThroughput" runat="server" Width="1050px" ScrollBars="auto" align="center" Height="1500px">
                                <% if (dsTER.Tables[0].Rows.Count > 0)
                                   {%>
                                <table id="tblThroughput" width="100%" class="Gnowrap">
                                    <thead>
                                        <tr>
                                            <th rowspan="2">Sl.No</th>
                                           <%
                                    int strcolcount = (dsTER.Tables[0].Columns.Count - 8) / 4;
                                    int colcnt = 0;
                                    for (int i = 0; i < strcolcount + 1; i++)
                                    {
                                           %>
                                           
                                              <%
                                    if (i == 0)
                                    { %>
                                                 
                                                 <th rowspan="2">
                                                     <% = dsTER.Tables[0].Columns[0].ToString()%>
                                                 </th>
                                                 <%}
                                                   else
                                                   {
                                                       if (i == 1) colcnt = i;
                                                       else colcnt = colcnt + 4;
                                                      %>
                                              <th colspan="4">
                                                <% = dsTER.Tables[0].Columns[colcnt].ToString()%>
                                              </th>
                                              <%}
                                            }%> 
                                             <th rowspan="2">Total Target</th>
                                             <th rowspan="2">Total Achieved</th>
                                             <th rowspan="2">Efficiency</th>
                                             <th rowspan="2">Total Production Achieved</th>
                                             <th rowspan="2">Avg Production time</th>
                                             <th rowspan="2">Total QC Achieved</th>
                                             <th rowspan="2">Avg QC Time</th> 
                                        </tr>
                                        <tr>
                                            <%for (int i = 0; i < dsTER.Tables[1].Rows.Count; i++)
                                              { %>
                                            <th>Target</th>
                                            <th>Production Achieved</th>
                                            <th>Target</th>
                                            <th>QC Achieved</th>
                                            <%} %>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% for (int i = 0; i < dsTER.Tables[0].Rows.Count; i++)
                                           { %>
                                        <tr>
                                            <td><%=(i + 1)%></td>
                                            <% for (int j = 0; j < dsTER.Tables[0].Columns.Count; j++)
                                               { %>
                                            
                                            <td><%=dsTER.Tables[0].Rows[i][j].ToString()%></td>
                                            
                                            <%} %>
                                        </tr>
                                        <%} %>
                                    </tbody>
                                </table>
                                <%} %>
                            </asp:Panel>
                            <asp:Panel ID="PanelCheque" runat="server" Width="1020px" Height="400px" ScrollBars="auto"
                                align="center">
                                <table width="100%">
                                    <tr>
                                        <td class="Txthead" align="center">
                                            Cheque Payable Details</td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 100px; width: 100%;">
                                            <table border="0" cellpadding="5" cellspacing="6" width="100%">
                                                <tr>
                                                    <td class="Lblothers">
                                                        Cheque Payable</td>
                                                    <td>
                                                        <asp:TextBox ID="txtchqpay" runat="server" CssClass="txtuser" Width="200px"></asp:TextBox><span
                                                            style="color: Red;">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Lblothers">
                                                        Request Type</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlrequesttype" runat="server" Width="200px" Height="20px" CssClass="txtuser">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="1">UPS</asp:ListItem>
                                                            <asp:ListItem Value="2">UPS/R</asp:ListItem>
                                                            <asp:ListItem Value="3">UPS/SASE</asp:ListItem>
                                                            <asp:ListItem Value="4">REGULAR</asp:ListItem>
                                                            <asp:ListItem Value="5">THANKS REQUEST</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Lblothers">
                                                        To Address</td>
                                                    <td>
                                                        <asp:TextBox ID="txtaddress" runat="server" CssClass="txtuser" Width="300px" Height="100px"
                                                            TextMode="MultiLine"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td class="Lblothers">
                                                        Charges</td>
                                                    <td>
                                                        <asp:TextBox ID="txtamount" runat="server" CssClass="txtuser" Width="300px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td class="Lblothers">
                                                        Tax Type</td>
                                                    <td>
                                                        <asp:TextBox ID="txttaxtype" runat="server" CssClass="txtuser" Width="300px" TextMode="MultiLine"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Button ID="btngetaddress" runat="server" Text="Get Address" CssClass="MenuFont"
                                                            OnClick="btngetaddress_Click" />
                                                        <asp:Button ID="btnnewcheque" runat="server" Text="New" CssClass="MenuFont" OnClick="btnnewcheque_Click" />
                                                        <asp:Button ID="btnsavecheque" runat="server" Text="Save" CssClass="MenuFont" OnClick="btnsavecheque_Click" />
                                                        <asp:Button ID="btnupdatecheque" runat="server" Text="Update" CssClass="MenuFont"
                                                            OnClick="btnupdatecheque_Click" />
                                                        <asp:Button ID="btndeletecheque" runat="server" Text="Delete" CssClass="MenuFont"
                                                            OnClick="btndeletecheque_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="PanelStateComments" runat="server" Width="1020px" Height="500px" ScrollBars="auto"
                                align="center">
                                <table width="90%">
                                    <tr>
                                        
                                        <td class="Txthead" align="center">                                            
                                            <asp:Label ID="LblTittle" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;">
                                            <table border="0" cellpadding="5" cellspacing="6" width="100%">
                                                <tr>
                                                    <td class="Lblothers">
                                                        <asp:Label ID="LblSubtittle" runat="server">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <div id="DivState" runat="server">
                                                                        <asp:TextBox ID="txtstate" runat="server" CssClass="txtuser" Width="200px"></asp:TextBox>
                                                                        <span style="color: Red;">*</span></div>
                                                                </td>
                                                              
                                                                <asp:HiddenField ID="HIDCommentsType" runat="server" />
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Lblothers">
                                                        <asp:Label ID="CommentTittle" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtstateaddress" runat="server" CssClass="txtuser" Height="300px" TextMode="MultiLine" Width="600px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2" style="height: 34px">
                                                        <asp:Button ID="btnstateaddress" runat="server" CssClass="MenuFont" OnClick="btnstateaddress_Click" Text="Show" />
                                                        <asp:Button ID="btnstatesave" runat="server" CssClass="MenuFont" OnClick="btnstatesave_Click" Text="Save" />
                                                        <asp:Button ID="btnstateupdate" runat="server" CssClass="MenuFont" OnClick="btnstateupdate_Click" Text="Update" />
                                                        <asp:Button ID="btnstatedelete" runat="server" CssClass="MenuFont" OnClick="btnstatedelete_Click" Text="Delete" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Label ID="errlable" runat="server" Font-Names="Verdana" ForeColor="Red" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                           <asp:Panel ID="PanelKeyingUpdate" runat="server" Width="1050px" ScrollBars="auto" align="center" Height="1500px">
                               <%-- <% if (dsTER.Tables[0].Rows.Count > 0)
                                   {%>
                                <table id="tblKeyingUpdate" width="100%" class="Gnowrap">
                                    <thead>
                                        <tr>
                                            <th rowspan="2">Sl.No</th>
                                           <%
                                    int strcolcount = (dsTER.Tables[0].Columns.Count - 8) / 4;
                                    int colcnt = 0;
                                    for (int i = 0; i < strcolcount + 1; i++)
                                    {
                                           %>
                                           
                                              <%
                                    if (i == 0)
                                    { %>
                                                 
                                                 <th rowspan="2">
                                                     <% = dsTER.Tables[0].Columns[0].ToString()%>
                                                 </th>
                                                 <%}
                                                   else
                                                   {
                                                       if (i == 1) colcnt = i;
                                                       else colcnt = colcnt + 4;
                                                      %>
                                              <th colspan="4">
                                                <% = dsTER.Tables[0].Columns[colcnt].ToString()%>
                                              </th>
                                              <%}
                                            }%> 
                                             <th rowspan="2">Total Target</th>
                                             <th rowspan="2">Total Achieved</th>
                                             <th rowspan="2">Efficiency</th>
                                             <th rowspan="2">Total Production Achieved</th>
                                             <th rowspan="2">Avg Production time</th>
                                             <th rowspan="2">Total QC Achieved</th>
                                             <th rowspan="2">Avg QC Time</th> 
                                        </tr>
                                        <tr>
                                            <%for (int i = 0; i < dsTER.Tables[1].Rows.Count; i++)
                                              { %>
                                            <th>Target</th>
                                            <th>Production Achieved</th>
                                            <th>Target</th>
                                            <th>QC Achieved</th>
                                            <%} %>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% for (int i = 0; i < dsTER.Tables[0].Rows.Count; i++)
                                           { %>
                                        <tr>
                                            <td><%=(i + 1)%></td>
                                            <% for (int j = 0; j < dsTER.Tables[0].Columns.Count; j++)
                                               { %>
                                            
                                            <td><%=dsTER.Tables[0].Rows[i][j].ToString()%></td>
                                            
                                            <%} %>
                                        </tr>
                                        <%} %>
                                    </tbody>
                                </table>
                                <%} %>--%>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
   
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $("[src*=plus]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "../images/minus.png");
    });
    $("[src*=minus]").live("click", function () {
        $(this).attr("src", "../images/plus.png");
        $(this).closest("tr").next().remove();
    });
</script>
<div class="page_dimmer" id="pagedimmer" runat="server"></div>
<div class="Taxinfo_msgbx" id="DivCommentsUpdate" runat="server" align="center">
    <table border="0" width="700px" height="150px">
        <tr>
            <td align="center" style="width:100%">
                <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                    <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                       <td colspan="3" align="center" style="height: 25px">
                        <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Update Comments"></asp:Label></td>
                    </tr>
                     <tr>
                        <td class="Lblothers">Order No </td>
                        <td class="Lblothers">:</td>
                        <td><asp:Label ID="LblOrderno" runat="server" Text="Username" CssClass="Lbldata"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="Lblothers">Cause </td>
                        <td class="Lblothers">:</td>
                        <td><asp:TextBox ID="txtcause" runat="server" CssClass="txtuser" Width="100%" Height="50px" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="Lblothers">Suggestion </td>
                        <td class="Lblothers">:</td>
                        <td><asp:TextBox ID="txtsuggestion" runat="server" CssClass="txtuser" Width="100%" Height="50px" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btnupdatecomments" runat="server" Text="Update Comments" CssClass="MenuFont" OnClick="btnupdatecomments_Click" Width="150px" />
                            <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="MenuFont" OnClick="btncancel_Click" Width="150px"/>
                        </td>
                    </tr>
                </table>    
            </td>
        </tr>
    </table>             
</div> 
     
   <%-- <table style="width: 100%; height: 100%" border="0">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" Text="" Font-Names="Verdana" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center" valign="top">
                            <asp:Panel ID="Panel1" runat="server" align="center">
                                <table cellspacing="5" cellpadding="5" width="480" border="0">
                                   
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_themes/Black/images/Excel.png"
                                                Height="50px" Width="50px" ToolTip="Export" OnClick="Lnkexport_Click" />
                                        </td>
                                        <td class="Lblall" align="right">
                                            From</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtfrmdate1" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtfrmdate1"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="Lblall" align="right">
                                            To</td>
                                        <td align="left">
                                            <asp:TextBox ID="txttodate1" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txttodate1"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="Lblall" align="right">
                                            <asp:Label ID="Label3" runat="server" Text="Username" CssClass="Lbldata"></asp:Label></td>
                                       
                                    </tr>
                                </table>
                            </asp:Panel>
                            
                          
                        </td>
                    </tr>
                </table>--%>
      
</asp:Content>
