<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MailAwayComments.aspx.cs" Inherits="Pages_MailAwayComments" Title="MailAway" Theme="Black" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link rel="shortcut icon" type="image/ico" href="../App_themes/Black/images/Firefox(1).ico" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager> 
        <table border="0" style="height:600px;width:100%; font-family:Calibri;">
            <tr>
                <td align="center">
                <table style="height:500px;width:950px;border-radius:10px;background-color: #f5f5f5;" class="tblproduction" >
                <tr>
                <td valign="top" style="padding-top:60px;">
                    <asp:Panel ID="PanelRights" runat="server" Height="50px" Width="250px" align="center">    
                    <div class="urbangreymenu">  
                            <h3 class="headerbar">Mail Away</h3>                           
                            <ul>
                                <li><asp:LinkButton ID="LnkRegularMail" runat="server" OnClick="LnkRegularMail_Click" cssclass="SelectedMenu" >Regular Mail</asp:LinkButton></li>
                                <li><asp:LinkButton Runat="server" OnClick="LnkUpsMail_Click" ID="LnkUpsMail" cssclass="TopMenu" >UPS Mail</asp:LinkButton></li>
                                <li><asp:LinkButton Runat="server" OnClick="LnkReturnUps_Click" ID="LnkReturnUps" cssclass="TopMenu" >Return UPS</asp:LinkButton></li>                                                                
                            </ul>
                     </div>
                    </asp:Panel>       
                </td>                
                <td valign="top" style="padding-top:60px;">
                    <div style="vertical-align: top; overflow: auto; width: 850px; padding: 0 0 0 50px; " id="divReport">                                        
                    <asp:Panel ID="PanelMailAway" runat="server" Width="830px">            
                        <table cellpadding="5" cellspacing="5" width="100%" >
                        <tr>
                            <td align="left" class="Txthead"><span>TSI Mail Away Comments</span> </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="5" cellspacing="8" width="100%">
                                <tr>
                                    <td colspan="2" align="center" class="Lblall">
                                        <asp:Label ID="LblType" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td align="center" class="Lblall">
                                    <span>Comments</span>
                                    </td>
                                </tr>
                                <tr><td align="left" class="Lblall" style="width: 50%"><span>Tax Type</span></td>
                                    <td>
                                        <%--<asp:TextBox ID="TxtTaxType" runat="server" CssClass="txtuser"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddltaxtype" runat="server" Height="20px" Width="200px" CssClass="txtuser">
                                        </asp:DropDownList>
                                    </td>
                                    <td rowspan="6" style="vertical-align: middle;">
                                        <asp:TextBox ID="TxtComments" TextMode="MultiLine" CssClass="txtuser" Height="150px" Width="500px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr><td align="left" class="Lblall" style="width: 50%"><span>Fee </span></td><td>$<asp:TextBox ID="TxtFee" runat="server" CssClass="txtuser"></asp:TextBox></td></tr>
                                <tr><td align="left" class="Lblall" style="width: 50%"><span>Mail Type</span></td>
                                    <td><%--<asp:TextBox ID="TxtMailType" runat="server" CssClass="txtuser"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddlmailtype" runat="server" Height="20px" Width="200px" CssClass="txtuser">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Regular Mail</asp:ListItem>
                                            <asp:ListItem>UPS</asp:ListItem>
                                            <asp:ListItem>UPS with Return UPS</asp:ListItem>
                                            <asp:ListItem>UPS with Self Addressed Stamp Envelope</asp:ListItem>
                                        </asp:DropDownList>
                                     </td>
                                </tr>
                                <tr><td align="left" class="Lblall" style="width: 50%"><span>Mailing Date</span></td><td><asp:TextBox ID="TxtMailDate" runat="server" CssClass="txtuser"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="MM-dd-yyyy" TargetControlID="TxtMailDate">
                                    </cc1:CalendarExtender>
                                </td></tr>
                                <tr><td align="left" class="Lblall" style="width: 50%"><span>Follow Up Date</span></td><td><asp:TextBox ID="TxtFollowUpDate" runat="server" CssClass="txtuser"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server"  Format="MM-dd-yyyy" TargetControlID="TxtFollowUpDate">
                                    </cc1:CalendarExtender>
                                </td></tr>
                                <tr><td align="left" class="Lblall" style="width: 50%;"><span>ETA</span></td><td><asp:TextBox ID="TxtETA" runat="server" CssClass="txtuser"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server"  Format="MM-dd-yyyy" TargetControlID="TxtETA">
                                    </cc1:CalendarExtender>
                                </td></tr>
                                <tr><td colspan="2"></td><td align="center">
                                    <asp:Button ID="btnsavedate" runat="server" Text="Save Dates" OnClick="btnsavedate_Click" CssClass="MenuFont" />
                                    <asp:Button ID="BtGetComments" runat="server" Text="Get Comments" OnClick="BtGetComments_Click" CssClass="MenuFont" />
                                    <asp:Button ID="BtClear" runat="server" Text="Clear" OnClick="BtClear_Click" CssClass="MenuFont" /></td>    
                                </tr>
                                 <tr><td colspan="2"><asp:Label ID="Lblerror" runat="server" Font-Names="Georgia" Font-Size="13px" ForeColor="red"></asp:Label></td></tr>
                        </table>
        </td>
                        </tr>
                        </table>
                    </asp:Panel>
                    </div>
                </td>
                        </tr>
                    </table>                
                </td>               
            </tr>            
        </table>
    </div>
    </form>
</body>
</html>
