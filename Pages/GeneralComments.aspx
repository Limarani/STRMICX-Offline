<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true" CodeFile="GeneralComments.aspx.cs" Inherits="Pages_GEneralComments" Title="GENERAL COMMENTS" Theme="Black" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" width="100%" style="background-color:#f5f5f5;font-family:Calibri;">   
        <tr>
            <td align="center">
                <table border="0" width="80%" style="height:200px">
                    <tr>
                        <td align="center">
                            <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                                <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                   <td align="center" style="height: 25px">Web Tool Installment Comments</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtstatecomments" runat="server" TextMode="MultiLine" Height="505px" CssClass="txtuser1" Width="100%" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" ><asp:Button ID="btnclose" runat="server" Width="150px" Text="Close" CssClass="MenuFont" OnClick="btnclose_Click" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table> 
            </td>
        </tr>
    </table>
</asp:Content>

