<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/STRMICX-Offline.master" CodeFile="STRMICXUsers.aspx.cs" Inherits="Pages_STRMICXUsers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0" style="background-color:white;">
        <tr>

            <td valign="top" align="center">
                <asp:Panel ID="PanelViewuser" runat="server" Width="90%" style="height:519px;">
                    <table border="0" width="100%" style="font-family: Calibri;">
                        <tr>
                            <td align="center" valign="middle" style="width: 100%;">
                                <table width="50%" border="1" style="border: solid 1px gray;">
                                    <tr>
                                        <td style="background-color: #B3DE97; width: 50px;" align="center">
                                            <asp:Label ID="lblonline" runat="server" Text="" Width="100px" Font-Bold="true" Style="font-family: Calibri;"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:LinkButton ID="Lnkonline" runat="server" Text="Online" Font-Names="Georgia" Font-Bold="true" Font-Overline="false" Style="font-family: Calibri;" OnClick="Lnkonline_Click"></asp:LinkButton>
                                        </td>
                                        <td style="background-color: #FAC8D3; width: 50px;" align="center">
                                            <asp:Label ID="lbloffline" runat="server" Width="100px" Font-Bold="true" Style="font-family: Calibri;"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:LinkButton ID="Lnkoffline" runat="server" Text="Offline" Font-Names="Georgia" Font-Bold="true" Font-Overline="false" Style="font-family: Calibri;" OnClick="Lnkoffline_Click"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:CheckBox ID="Chkrefresh" runat="server" CssClass="Autorefresh" Style="font-family: Calibri;" OnCheckedChanged="Chkrefresh_CheckedChanged" AutoPostBack="true" Text="Auto Refresh" />
                                            <asp:Timer ID="Refresh" runat="server" Interval="120000" Enabled="false" OnTick="Refresh_Tick"></asp:Timer>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" style="width: 80%;">
                                <asp:Panel ID="Panel1" runat="server" Width="100%" Height="500px" ScrollBars="Auto">
                                    <asp:GridView ID="Griduserdetails" runat="server" Width="100%" FooterStyle-CssClass="Gridfooterbar" ShowFooter="false" AutoGenerateColumns="false" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowDataBound="Griduserdetails_RowDataBound" Style="font-family: Calibri;">
                                        <Columns>
                                            <asp:BoundField DataField="User_name" HeaderText="Username" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                            <asp:BoundField DataField="Admin" HeaderText="Admin"></asp:BoundField>
                                            <asp:BoundField DataField="Keying" HeaderText="Production"></asp:BoundField>
                                            <asp:BoundField DataField="QC" HeaderText="QC"></asp:BoundField>
                                            <asp:BoundField DataField="DU" HeaderText="DU"></asp:BoundField>
                                            <asp:BoundField DataField="Review" HeaderText="Post Audit"></asp:BoundField>
                                            <asp:BoundField DataField="Inprocess" HeaderText="Inprocess"></asp:BoundField>
                                            <asp:BoundField DataField="mailaway" HeaderText="Mailaway"></asp:BoundField>
                                            <asp:BoundField DataField="Parcelid" HeaderText="ParcelID"></asp:BoundField>
                                            <asp:BoundField DataField="Onhold" HeaderText="OnHold"></asp:BoundField>
                                            <asp:BoundField DataField="Priority" HeaderText="Priority" Visible="false"></asp:BoundField>
                                            <asp:BoundField DataField="Order_type" HeaderText="Order Type"></asp:BoundField>
                                            <asp:BoundField DataField="System" HeaderText="IP Address" Visible="false"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <div class="footer" style="text-align: center;">
        <p style="background-color: #337ab7; margin-left: 2px; margin-bottom: 0px; color: white;">
            &copy; 2019. All rights reserved | Designed & Developed by String Information Services
        </p>
    </div>

</asp:Content>
