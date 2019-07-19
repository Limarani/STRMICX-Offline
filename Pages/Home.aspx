<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Pages_Home" Theme="Black" Title="HOME" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" style="width: 100%; height: 600px; background-color: #F1FBFF;font-family:Calibri;" align="center">
        <tr>
            <td style="width: 300px; padding-top: 10px;" align="center" valign="top">
                <table>
                    <tr>
                        <td align="center">
                            <asp:CheckBox ID="Chkrefresh" runat="server" CssClass="Autorefresh" OnCheckedChanged="Chkrefresh_CheckedChanged" style="font-family:Calibri" AutoPostBack="true" Text="Auto Refresh" />
                            <asp:Timer ID="Refresh" runat="server" Interval="120000" Enabled="false" OnTick="Refresh_Tick"></asp:Timer>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="urbangreymenu">
                                <h3 class="headerbar" style="font-family:Calibri">Today&#39;s Target Status</h3>
                                <ul>
                                    <%--<li><asp:LinkButton ID="today" runat="server" OnClick="today_Click">Today</asp:LinkButton></li>--%>
                                    <li>
                                        <asp:LinkButton ID="lastweek" runat="server" OnClick="lastweek_Click" Visible="false">Last Week</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="last30days" runat="server" OnClick="last30days_Click" Visible="false">Last 30 Days</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="last90days" runat="server" OnClick="last90days_Click" Visible="false">Last 90 Days</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="lnkusertarget" runat="server" OnClick="lnkusertarget_Click" style="font-family:Calibri;">Set User Target</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="lnkadminerror" runat="server" OnClick="lnkadminerror_Click" Visible="false">Admin Error</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="Lnkposterror" runat="server" OnClick="Lnkposterror_Click" Visible="false">Post Audit Error</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="LnkScraping" runat="server" OnClick="lnkscraping_Click" style="font-family:Calibri;">STARS</asp:LinkButton>
                                    </li>

                                    <%-- <li>
                                        <asp:LinkButton ID="rrr" runat="server"  Text="Test" OnClick="rrr_Click" >Test</asp:LinkButton>
                                    </li>--%>
                                </ul>
                                <%--<h3 class="headerbar">Quality</h3> 
                    <ul>
                        <li><asp:LinkButton ID="lnkfpyreport" runat="server" OnClick="lnkfpyreport_Click">FPY Report</asp:LinkButton></li>
                        <li><asp:LinkButton ID="lnkpostaudit" runat="server" OnClick="lnkpostaudit_Click">Post Audit Report</asp:LinkButton></li>
                    </ul> --%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divscrape" runat ="server" visible ="false"  class="urbangreymenu"> 
                               <%-- <ul>
                                    <li>
                                        <asp:LinkButton ID="LnkScraping" runat="server" Text="Tax Automation" OnClick="lnkscraping_Click">Scraping</asp:LinkButton>
                                    </li>
                                </ul>--%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="Txthead" align="left">
                            <asp:Label ID="Lblutil" runat="server" Text="" Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="color: Red" align="center">
                            <h3 class="headerbar">Aging orders</h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="color: Red" align="center" >
                            <asp:Panel ID="pnlInproc" runat="server" Width="200px" Height="200px" ScrollBars="Auto">
                                <asp:GridView ID="grdinproc" runat="server">
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="left" valign="top">
                <table border="0">
                    <tr>
                        <td class="Txthead" align="center">
                            <asp:Label ID="Lblinfo" style="font-family:Calibri" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" Width="900px" Height="730px" ScrollBars="auto" align="center">
                                <asp:GridView ID="Gridutilization" runat="server" Width="850px" FooterStyle-CssClass="Gridfooterbar" ShowFooter="true" AutoGenerateColumns="false" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowDataBound="Gridutilization_RowDataBound" AllowSorting="true" OnSorting="Gridutilization_Sorting" style="font-family:Calibri">
                                    <EmptyDataRowStyle BackColor="LightBlue" ForeColor="Red" Font-Names="Georgia" />
                                    <EmptyDataTemplate>
                                        <asp:Image ID="NoDataImage" runat="server" />No Data Found.
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno." HeaderStyle-Width="45px">
                                            <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-ForeColor="white" ItemStyle-HorizontalAlign="Left"></asp:BoundField>

                                        <asp:BoundField DataField="Target" HeaderText="Target" SortExpression="Target" HeaderStyle-ForeColor="white"></asp:BoundField>

                                        <asp:BoundField DataField="Processed" HeaderText="Processed" SortExpression="Processed" HeaderStyle-ForeColor="white"></asp:BoundField>

                                        <asp:BoundField DataField="Completed" HeaderText="Completed" SortExpression="Completed" HeaderStyle-ForeColor="white"></asp:BoundField>

                                        <asp:BoundField DataField="AuditType" HeaderText="AuditType" SortExpression="AuditType" HeaderStyle-ForeColor="white" Visible="false"></asp:BoundField>
                                        
                                        <asp:BoundField DataField="QualityAchived" HeaderText="QualityAchived" SortExpression="QualityAchived" HeaderStyle-ForeColor="white" Visible="false"></asp:BoundField>

                                        <asp:BoundField DataField="Key_Ptime" HeaderText="Keying AvgTime" SortExpression="Key_Ptime" HeaderStyle-ForeColor="white"></asp:BoundField>

                                        <asp:BoundField DataField="QC_Ptime" HeaderText="QC AvgTime" SortExpression="QC_Ptime" HeaderStyle-ForeColor="white"></asp:BoundField>

                                        <asp:BoundField DataField="Efficiency" HeaderText="Target (%)" SortExpression="Efficiency" HeaderStyle-ForeColor="white"></asp:BoundField>

                                        <asp:BoundField DataField="Utilization" HeaderText="Utilization" SortExpression="Utilization" HeaderStyle-ForeColor="white" Visible="false"></asp:BoundField>

                                        <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" HeaderStyle-ForeColor="white" Visible="false"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Panel ID="pnlTax" runat="server" Width="800px" Height="500px" ScrollBars="auto" align="center">
                                <asp:GridView ID="grdTax" runat="server" Width="650px" CssClass="Gnowrap" style="font-family:Calibri" Visible="false">
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <div class="page_dimmer" id="pagedimmer1" runat="server"></div>
    <div class="Logout_msgbx2" id="Targetuser" runat="server" align="center">
        <table border="0" width="100%" height="800">
            <tr>
                <td align="center" valign="top">
                    <table border="0" cellpadding="3" cellspacing="4" width="100%">
                        <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                            <td align="center" style="height: 25px">User Today Target</td>
                        </tr>
                        <tr>
                            <td align="center"style="font-family:Calibri">
                                <asp:Panel ID="PanelGrid" runat="server" Width="800px" Height="448px" ScrollBars="auto" align="center">
                                    <asp:GridView ID="Gridusername" runat="server" Width="700px" AutoGenerateColumns="false" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="User_Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" />
                                            <asp:BoundField DataField="Type" HeaderText="Audit Type" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                                            <asp:TemplateField HeaderText="Target">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txttargetvalue" runat="server" BorderStyle="None"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtfilter" runat="server" TargetControlID="txttargetvalue" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="Gridusername1" runat="server" Width="700px" AutoGenerateColumns="false" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="User_Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" />
                                            <asp:BoundField DataField="Type" HeaderText="Audit Type" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                                            <asp:TemplateField HeaderText="Target">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txttargetvalue1" runat="server" BorderStyle="None" Text='<%# Eval("Target")%>'></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtfilter1" runat="server" TargetControlID="txttargetvalue1" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comments">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcomments" runat="server" BorderStyle="None" Text='<%# Eval("Comments")%>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" />
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="Comments" HeaderText="Comments" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />--%>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnsettarget" runat="server" Width="150px" Text="Set Target" CssClass="MenuFont" style="font-family:Calibri;" OnClick="btnsettarget_Click" />
                                <asp:Button ID="btnclose" runat="server" Width="150px" Text="Close" CssClass="MenuFont" OnClick="btnclose_Click" style="font-family:Calibri;" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
