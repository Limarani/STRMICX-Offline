<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostAudit.aspx.cs" Inherits="Pages_PostAudit" Theme="Black" Title="POSTAUDIT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script type="text/javascript" language="Javascript" src="../Script/Checkbox.js"></script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>

<body>
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager> 
        <div id="Tblmain">
            <div id="logo" style="float: left; width: 230px; height: 85px;">
            </div>
            <div id="templatemo_header" style="float: left;">
                <table border="0" style="width: 900px; height: 90px;">
                    <tr>
                        <td align="center" valign="top" style="padding-top: 30px;">
                            <asp:Image ID="Imgtitle" runat="server" ImageUrl="~/App_themes/Black/images/t1.png" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        
        <div id="templatemo_menu_wrapper">
            <div id="templatemo_menu">
                <table cellpadding="5px">
                    <tr>
                        <td style="font-family: Calibri; font-size: 15px; color: White; width: 300px;"
                            valign="bottom">
                            <asp:Label ID="Lblusername" runat="server"></asp:Label></td>
                        <td>
                            <asp:Button ID="LogoutBtn" runat="server" CssClass="MenuFont" Text="Close" Font-Overline="false" OnClick="Btnclose_Click"></asp:Button></td>
                        <td style="font-family: Calibri; font-size: 15px; color: White; width: 883px;" align="right">
                            <asp:Label ID="Lbltime" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- end of menu -->
        </div>
     
        <div>
            <table id="MainTable" style="height: 500px; background-color: #f5f5f5;" width="100%" class="tblproduction">
                <tr>
                    <td align="center">                                                                                                            
                         <table class="Table1" border="0" width="100%" style="height:100%;">
                            <tr>
                                <td align="center" style="height: 100%">
                                    <asp:Panel ID="PanelOrderList" runat="server" align="center" Width="1100px">
                                        <table border="0" cellspacing="5" cellpadding="5" width="100%">                                                                                                       
                                            <tr>
                                                <td class="Lblall" align="right">From</td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtfrmdate" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfrmdate" Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="Lblall" align="right">To</td>
                                                <td align="left">
                                                    <asp:TextBox ID="txttodate" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="Lblall" align="right">Order Type</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlotype" runat="server" CssClass="txtuser" Width="200px" Height="21px">                                 
                                                        <asp:ListItem></asp:ListItem>
                                                        <asp:ListItem Value ="1">Phone</asp:ListItem>
                                                        <asp:ListItem Value ="2">Website</asp:ListItem>
                                                        <asp:ListItem Value ="3">Mailaway</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Lblall" align="right">Username</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlusername" runat="server" CssClass="txtuser" Width="200px" Height="21px">                                 
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center"><asp:Button ID="btnordershow" runat="server" Text="Show" CssClass="MenuFont" OnClick="btnordershow_Click"/></td>
                                                <td align="center"><asp:Button ID="btnmvereview" runat="server" Text="Move to Post Audit" CssClass="MenuFont" OnClick="btnmvereview_Click" Width="150px"/></td>
                                             </tr>  
                                         </table>
                                    </asp:Panel> 
                                    <asp:Panel ID="PanelGrid" runat="server" Height="600px" ScrollBars="auto" Width="1100px"  align="center">
                                         <asp:GridView ID="GridUser" runat="server" AutoGenerateColumns="false" Width="1500px" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowCreated="GridUser_RowCreated">
                                            <Columns>
                                                  <asp:TemplateField>
                                                  <HeaderStyle HorizontalAlign="Center" />
                                                  <HeaderTemplate>
                                                    <asp:CheckBox ID="chkselectall" ToolTip="Click here to select/deselect all rows" runat="server" />
                                                  </HeaderTemplate>
                                                  <ItemTemplate>
                                                    <asp:CheckBox ID="chkselect" runat="server" />
                                                  </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="SNo#">
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:BoundField DataField="Order No" HeaderText="Order No" />
                                                  <asp:BoundField DataField="Date" HeaderText="Date" />
                                                  <asp:BoundField DataField="PTY" HeaderText="PTY" />
                                                  <asp:BoundField DataField="State" HeaderText="State" />
                                                  <asp:BoundField DataField="County" HeaderText="County" />
                                                  <asp:BoundField DataField="Township" HeaderText="Township" />
                                                  <asp:BoundField DataField="Type" HeaderText="Type" />
                                                  <asp:BoundField DataField="Status" HeaderText="Status" />
                                                  <asp:BoundField DataField="K1 Name" HeaderText="K1 Name" />
                                                  <asp:BoundField DataField="QC Name" HeaderText="QC Name" />
                                                  <asp:BoundField DataField="Comments" HeaderText="Comments" ItemStyle-HorizontalAlign="Left" />
                                                  <asp:BoundField DataField="Download Time" HeaderText="Download Time" />
                                                  <asp:BoundField DataField="Upload Time" HeaderText="Upload Time" />
                                                  <asp:BoundField DataField="TAT" HeaderText="TAT" />
<asp:BoundField DataField="AssignedDate" HeaderText="AssignedDate" />                                                     
                                            </Columns>
                                          </asp:GridView>                                        
                                   </asp:Panel>  
                                </td>
                             </tr>                                              
                            <tr><td colspan="5" align="center"><asp:Label ID="errorlabel" runat="server" Text="" Font-Names="Verdana" ForeColor="Red"></asp:Label></td></tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="templatemo_footer_wrapper">
            <div id="templatemo_footer">
                Copy Right © String 2012. All Rights Reserved.Powered By : SST
            </div>
            <!-- end of footer -->
        </div>
    </form>
</body>
</html>
