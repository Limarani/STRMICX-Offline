<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true" CodeFile="PostAuditError.aspx.cs" Inherits="Pages_NonAdminHome" Title="HOME" Theme="Black" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<table border="0" style="width:100%;height:600px;background-color:#f5f5f5;" align="center"> 
    <%--<tr>
        <td align="center">
            <table border="0" width="100%">
                <tr>
                    <td align="center" valign="bottom">
                        <asp:Image ID="Imgtitle" runat="server" ImageUrl="~/App_themes/Black/images/home1.png" />
                    </td>
                </tr>
           </table> 
        </td>
    </tr>--%>
    <tr><td class="Txthead" align="center"><asp:Label ID="Lblinfo" runat="server" Text=""></asp:Label></td></tr>
    <tr>
        <td align="center" style="width:75%;" valign="top">
            <asp:Panel ID="PanelGrid" runat="server" Width="85%" Height="300px" ScrollBars="auto"  align="center"> 
                 <asp:GridView ID="GridErrorDetails" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="Order_No" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                 <emptydatarowstyle backcolor="LightBlue" forecolor="Red" Font-Names="Georgia"/>
                 <emptydatatemplate><asp:image id="NoDataImage" runat="server" />No Data Found.</emptydatatemplate> 
                    <Columns>
                        <asp:BoundField DataField="Order_No" HeaderText="Order No" />
                        <asp:BoundField DataField="ErrorCategory1" HeaderText="Error Category" />
                        <asp:BoundField DataField="ErrorField1" HeaderText="Error Area" />
                        <asp:BoundField DataField="Incorrect1" HeaderText="Error Type" />
                        <asp:BoundField DataField="Correct1" HeaderText="Combined" />
                        <asp:BoundField DataField="Review_OP_Comments" HeaderText="Audit_OP Comments" />
                        <asp:BoundField DataField="Error_Comments1" HeaderText="Error Comments" />
                        <asp:BoundField DataField="Audit_AcceptReason" HeaderText="Accept Reason" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="Lnkbtnaccept" CommandName="Accept"  runat="server" OnClick="Lnkbtnaccept_Click">Accept</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>                       
                 </asp:GridView>                            
            </asp:Panel>
        </td>
    </tr>                               
</table>

<div class="page_dimmer" id="pagedimmer1" runat="server"></div>
<div class="Auditaccept_checklist" id="AcceptReason" runat="server" align="center">
    <table border="0" width="400px" height="200px">
        <tr>
            <td align="center" valign="top">
                <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                    <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                       <td align="center" style="height: 25px" colspan="2"> Do you want to Accept?</td>
                    </tr>
                    <tr style="height:20px;"></tr>
                    <tr>
                        <td class="Lblothers" align="right">
                            <asp:Label ID="lblreason" runat="server" Text="Reason :"></asp:Label></td>
                        <td align="left">
                            <asp:TextBox ID="txtreason" runat="server" TextMode="MultiLine" Width="300px" Height="50px" CssClass="txtuser"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="height:20px;"></tr>
                    <tr>
                        <td align="center" colspan="2" >
                            <asp:Button ID="btnok" runat="server" Width="150px" Text="Yes" CssClass="MenuFont" OnClick="btnok_Click" />
                            <asp:Button ID="btnno" runat="server" Width="150px" Text="No" CssClass="MenuFont" OnClick="btnno_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="lblerror" runat="server" Font-Size="Large" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>             
</div>
</asp:Content>

