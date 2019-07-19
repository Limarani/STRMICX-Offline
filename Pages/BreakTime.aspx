<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true" CodeFile="BreakTime.aspx.cs" Inherits="Pages_BreakTime" Title="Break Time" Theme="Black" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" width="100%" style="background-color:#f5f5f5;font-family:Calibri;">
        <tr>
           <td style="width: 150px; height: 621px" align="right" valign="top">
             <table>
                <tr>
                    <td>
                        <asp:ListBox ID="Lstuser" CssClass="txtuser" runat="server" Width="200px" Height="600px" OnSelectedIndexChanged="Lstuser_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                    </td>
                </tr>
              </table>
           </td>
           <td style="height:621px;" align="center" valign="top">
                <table class="Table1" border="0" width="100%">
                   <tr>
                      <td>
                        <table class="Table1" border="0" width="800">
                            <tr style="height:20px;"></tr>
                            <tr>
                                <td align="center" style="width:200px;"><asp:CheckBox ID="Chkrefresh" runat="server" CssClass="Autorefresh" OnCheckedChanged="Chkrefresh_CheckedChanged" AutoPostBack="true" Text="Auto Refresh"/>
                                  <asp:Timer id="Refresh" runat="server" Interval="120000" Enabled ="false" OnTick="Refresh_Tick"></asp:Timer>
                                </td>
                                <td colspan="2" style="width:90%;" align="center">
                                    <div class="header_22"><span></span>Break Time Update</div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Panel ID="PanelAssign" runat="server" Width="1020px" Height="450px" ScrollBars="auto"  align="center"> 
                                        <asp:GridView ID="GridBreakDetails" runat="server" Width="75%" FooterStyle-CssClass="Gridfooterbar" ShowFooter="false" AutoGenerateColumns="true" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowDataBound="GridBreakDetails_RowDataBound" >                                                                                                             
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sno.">                        
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>                            
                                                </asp:TemplateField>                                     
                                            </Columns>                        
                                        </asp:GridView>                            
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr style="height:20px;"></tr>
                            <%--<tr>
                                <td align="center" style="width:15%; height: 26px;">
                                    <asp:Button ID="btnbreak" runat="server" Text="Break" CssClass="MenuFont" OnClick="btnbreak_Click" Visible="false"/>
                                </td>
                                <td align="center" style="width:15%; height: 26px;">
                                    <asp:Button ID="btndinner" runat="server" Text="Dinner" CssClass="MenuFont" OnClick="btndinner_Click" Visible="false"/>
                                </td>
                                <td align="center" style="width:15%; height: 26px;">
                                    <asp:Button ID="btnmeeting" runat="server" Text="Meeting" CssClass="MenuFont" OnClick="btnmeeting_Click" Visible="false"/>
                                </td>
                                <td align="center" style="width:15%; height: 26px;">
                                    <asp:Button ID="btntraining" runat="server" Text="Training" CssClass="MenuFont" OnClick="btntraining_Click" Visible="false"/>
                                </td>
                            </tr>--%>
                        </table>
                      </td>
                   </tr>                                       
                   <tr><td align="center"><asp:Label ID="errorlabel" runat="server" Font-Names="Verdana" ForeColor="Red"></asp:Label></td></tr>
                </table>
           </td>
        </tr>
     </table> 
</asp:Content>

