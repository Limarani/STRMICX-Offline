<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ParcelAbstract.aspx.cs" Inherits="Pages_PostAudit" Theme="Black" Title="ABSTRACT CHECK" %>

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
                            <tr><td align="center" valign="middle"><asp:Button ID="btnordershow" runat="server" Text="View Titlesource" CssClass="MenuFont" OnClick="btnordershow_Click" Width="150px"/></td></tr>
                            <tr>
                                <td align="center" style="height: 100%;">
                                 <asp:GridView ID="GridUser" runat="server" AutoGenerateColumns="true" Width="300px" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowCreated="GridUser_RowCreated">
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
                                    </Columns>
                                  </asp:GridView>
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
        
        <script type="text/javascript" src="../Script/jquery-1.4.1.min.js"></script>
        <script type="text/javascript" src="../Script/ScrollableGridPlugin.js"></script>
        <script type = "text/javascript">
            $(document).ready(function () {
                $('#<%=GridUser.ClientID %>').Scrollable({ScrollHeight: 500});
            });
        </script>
        

    </form>
</body>
</html>
