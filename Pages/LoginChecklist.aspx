<%@ Page Language="C#" MasterPageFile="~/Master/TSI2.master" AutoEventWireup="true" CodeFile="LoginChecklist.aspx.cs" Inherits="Pages_LoginChecklist" Title="LOGIN CHECKLIST" Theme="Black" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="LoginPanel" runat="server">
    <div class="Login_checklist" id="ChecklistLogin" runat="server" align="center">
            <table border="0" width="700px" height="200px">
                <tr>
                    <td align="center" style="width:100%">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                               <td colspan="3" align="center" style="height: 25px">
                               <asp:Label ID="Label2" runat="server" Font-Size="Large" style="font-family:Calibri" Text="Login CheckList"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td style="height: 26px;font-family:Calibri"><asp:CheckBox ID="chkattendance" runat="server" Text="Attendance" /></td>                     
                                <td style="height: 26px;font-family:Calibri"><asp:CheckBox ID="chkbiometric" runat="server" Text="Biometric Access" /></td>                                       
                            </tr>
                            <tr>   
                                <td>&nbsp;</td>                             
                               <td style="height: 26px;font-family:Calibri"><asp:CheckBox ID="chkmobile" runat="server" Text="Mobile Restriction" /></td>   
                               <td style="height: 26px;font-family:Calibri"><asp:CheckBox ID="chkidcard" runat="server" Text="ID Card & Dress Code" /></td> 
                            </tr> 
                            <tr>   
                                <td>&nbsp;</td>                
                                <td style="height: 26px;font-family:Calibri"><asp:CheckBox ID="chkheadset" runat="server" Text="Headset allotment" /></td>                     
                                <td style="height: 26px;font-family:Calibri"><asp:CheckBox ID="chkhardware" runat="server" Text="Login and check if all hardware work properly" /></td>                     
                            </tr>
                            
                            <tr><td colspan="3">&nbsp;</td></tr>
                            <tr>
                               <td align="center" colspan="3"><asp:Button ID="btncreatetlogin" runat="server" Width="150px" Text="Save Login Details" style="font-family:Calibri" CssClass="MenuFont" OnClick="btncreatetlogin_Click" />
                               <asp:Button ID="btncancel" runat="server" Width="100px" Text="Cancel" CssClass="MenuFont" style="font-family:Calibri" OnClick="btncancel_Click" />
                               </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3" style="font-family:Calibri"><asp:Label ID="lblerror" runat="server" ForeColor="red" Font-Size="14px"></asp:Label></td>
                            </tr>
                        </table>    
                    </td>
                </tr>
            </table>             
        </div>
        </asp:Panel>
        
        <asp:Panel ID="LogoutPanel" runat="server">
        <div class="Logout_checklist" id="ChecklistLogout" runat="server" align="center">
            <table border="0" width="700px" height="150px">
                <tr>
                    <td align="center" style="width:100%">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                               <td colspan="4" align="center" style="height: 25px">
                               <asp:Label ID="Label1" runat="server" Font-Size="Large" style="font-family:Calibri" Text="Logout CheckList"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td style="height: 26px;font-family:Calibri"><asp:CheckBox ID="chkplaceclean" runat="server" Text="Keeping the work place clean" /></td>                     
                                <td style="height: 26px;font-family:Calibri"><asp:CheckBox ID="chkheadsethandovr" runat="server" Text="Headset handover" /></td>   
                                <td style="height: 26px;font-family:Calibri"><asp:CheckBox ID="chkswitchoff" runat="server" Text="Switching off systems" /></td>                                       
                            </tr>                            
                            <tr><td colspan="4">&nbsp;</td></tr>
                            <tr>
                               <td align="center" colspan="4"><asp:Button ID="btncreatetlogout" runat="server" Width="150px" Text="Save Logout Details" CssClass="MenuFont" style="font-family:Calibri" OnClick="btncreatelogout_Click" />
                               <asp:Button ID="btnlogcancel" runat="server" Width="150px" Text="Cancel" CssClass="MenuFont" style="font-family:Calibri" OnClick="btnlogcancel_Click" />
                               </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4"><asp:Label ID="lbllogerror" runat="server" ForeColor="red" style="font-family:Calibri" Font-Size="14px"></asp:Label></td>
                            </tr>
                        </table>    
                    </td>
                </tr>
            </table>             
        </div>  
        </asp:Panel>     
</asp:Content>

