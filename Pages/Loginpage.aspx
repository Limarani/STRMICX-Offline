<%@ Page Language="C#" MasterPageFile="~/Master/TSI3.master" AutoEventWireup="true" CodeFile="Loginpage.aspx.cs" Inherits="Pages_Loginpage" Title="LOGIN" Theme="Black" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" style="height:500px;background:f5f5f5;" border="0">
<tr>
<td align="left" valign="middle">
    <table border="0" style="height:600px;width:800px;">
        <tr><td style="height:100px;"><asp:Image ID="Imglogo" runat="server" ImageUrl="~/App_themes/logo.png" /></td></tr>        
        <tr><td align="right"  valign="top" style="padding-top:150px;"><asp:Image ID="Imgtitle" runat="server" ImageUrl="~/App_themes/Black/images/t1.png" /></td></tr>
    </table>    
</td>
<td style="width:500px;" valign="middle" align="center">
    <table cellpadding="5" cellspacing="5" border="0" class="auto-style1" >
    <tr><td colspan="2" class="templatemo_menu_wrapper1" style="height:20px;color:White;font-size:22px;font-family:Calibri;width:50px;" align="center">Sign In</td></tr>
    <tr>
        <td class="auto-style4" style="font-family:Helvetica, Calibri; font-size:13px">Username</td>
        <td class="auto-style2" style="font-family:Helvetica, Calibri; font-size:13px"><asp:TextBox ID="txtusername" runat="server" CssClass="txtuser" Width="270px"></asp:TextBox>
        </td>
    </tr>    
    <tr>
        <td class="auto-style5" style="font-family:Helvetica, Calibri; font-size:13px">Password</td>
        <td class="auto-style3" style="font-family:Helvetica, Calibri; font-size:13px"><asp:TextBox ID="txtpassword" runat="server" CssClass="txtuser" AutoPostBack="true" Width="270px" TextMode="Password" OnTextChanged="txtpassword_TextChanged"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right"><asp:Button ID="Lnksignin" runat="server" Text="Login" CssClass="MenuFont" style="font-family:Helvetica, Calibri; font-size:13px" OnClick="Lnksignin_Click"/></td>
    </tr>
    <tr><td colspan="2"></td></tr>
</table>
<div><asp:Label ID="Label1" runat="server" CssClass="Lblinfo" style="font-family:Helvetica, Calibri; font-size:13px" ForeColor="red" ></asp:Label></div>
</td>
</tr>
</table>
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            width: 346px;
        }
        .auto-style2 {
            height: 48px;
        }
        .auto-style3 {
            height: 40px;
        }
        .auto-style4 {
            height: 48px;
            width: 76px;
        }
        .auto-style5 {
            height: 40px;
            width: 76px;
        }
    </style>
</asp:Content>


