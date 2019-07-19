<%@ Page Language="C#" MasterPageFile="~/Master/STRMICX-Offline.master" AutoEventWireup="true"
    CodeFile="AssignJob.aspx.cs" Inherits="Pages_AssignJob" Title="ASSIGNJOB" Theme="Black" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function SelectAll(id) {
            var grid = document.getElementById("<%= Gridstatecount.ClientID %>");
            var cell;

            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        function Uncheck(chk1, chk2, chk3) {
            if (document.getElementById(chk1).checked == true) {
                document.getElementById(chk2).checked = false;
                document.getElementById(chk3).checked = false;
            }
        }
        function Uncheck1(chkkey, chkdu) {
            if (document.getElementById(chkkey).checked == true) {
                document.getElementById(chkdu).checked = false;
            }
        }
    </script>
    <table border="0" width="100%" style="background-color: white; font-family: Calibri;">
        <tr>
            <td valign="top" style="width: 300px">
                <table border="0" align="center">
                    <tr>
                        <td align="center">
                            <asp:CheckBox ID="Chkrefresh" runat="server" CssClass="Autorefresh" Style="font-family: Calibri;" OnCheckedChanged="Chkrefresh_CheckedChanged"
                                AutoPostBack="true" Text="Auto Refresh" />
                            <asp:Timer ID="Refresh" runat="server" Interval="120000" Enabled="false" OnTick="Refresh_Tick">
                            </asp:Timer>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-left: 15px;">
                            <div class="urbangreymenu">
                                <h3 class="headerbar" style="font-family: Calibri;">Assign Job</h3>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="Lnksettings" runat="server" OnClick="Lnksettings_Click" Style="font-family: Calibri;">User Setting</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkUpload" runat="server" OnClick="LnkUpload_Click" Visible="false">Assign Panel</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkUplodExcel" runat="server" OnClick="LnkUplodExcel_Click" Visible="false">Assign Excel Panel</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkPriority" runat="server" OnClick="LnkPriority_Click" Visible="false">Priority Panel</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkHPriority" runat="server" OnClick="LnkHPriority_Click" Visible="false">High Priority Panel</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkReset" runat="server" OnClick="LnkReset_Click" Style="font-family: Calibri;">Reset Panel</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkTracking" runat="server" OnClick="LnkTracking_Click" Visible="false">Tracking No Update</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkStatuschange" runat="server" Style="font-family: Calibri;" OnClick="LnkStatuschange_Click">Status Change</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkAssignOrder" runat="server" Style="font-family: Calibri;" OnClick="LnkAssignOrder_Click">Order Assign</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="Lnkordertype" runat="server" OnClick="Lnkordertype_Click" Visible="false">Update Order Type</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkClearDatabase" runat="server" OnClick="LnkClearDatabase_Click" Visible="false">Clear Database</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LnkClearyts" runat="server" Style="font-family: Calibri;" OnClick="LnkClearyts_Click" Visible="false">Clear YTS</asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <%--<td valign="top" >
            <table class="tvlright" style="height:600px;"><tr><td>&nbsp;</td></tr></table>
        </td>   --%>
            <td>
                <table style="width: 580px; height: 525px;" border="0">
                    <tr>
                        <td align="center">
                            <asp:Panel ID="PanelAssign" runat="server">
                                <table border="0" cellspacing="5" cellpadding="5">
                                    <tr style="font-family: Calibri;">
                                        <td class="Txthead" align="left">Assign Panel
                                        </td>
                                        <td style="width: 10px;" class="Lblall" align="right">Date
                                        </td>
                                        <td align="left" style="width: 100px;">
                                            <asp:TextBox ID="txtdate" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtAssign" runat="server" TextMode="MultiLine" CssClass="txtuser"
                                                Height="150px" Width="900px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Lblall" style="font-size: 14px; font-family: Calibri;">Pasting Format : Order No.| State | County | Priority
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btntransmit" runat="server" Text="Transmit" CssClass="MenuFont" OnClick="btntransmit_Click" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Panel ID="PanelGrid" runat="server" Width="800px" Height="300px" ScrollBars="auto"
                                                align="center">
                                                <asp:GridView ID="GridOrders" runat="server" Width="500px" AutoGenerateColumns="false"
                                                    GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No.">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Orderno" HeaderText="Order NO." />
                                                        <asp:BoundField DataField="County" HeaderText="County" />
                                                        <asp:BoundField DataField="State" HeaderText="State" />
                                                        <asp:BoundField DataField="Priority" HeaderText="Priority" />
                                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                                                        <asp:BoundField DataField="ExpectedClosing" HeaderText="Expected Closing" />
                                                        <asp:BoundField DataField="serprovied" HeaderText="Service" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Button ID="Btnupload" runat="server" Text="Upload" CssClass="MenuFont" Visible="True" OnClick="Btnupload_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblerrmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelExcelAssign" runat="server">
                                <table border="0" cellspacing="5" cellpadding="5">
                                    <tr>
                                        <td class="Txthead" align="left">Assign Excel Panel
                                        </td>
                                        <td style="width: 10px;" class="Lblall" align="right">Date
                                        </td>
                                        <td align="left" style="width: 100px;">
                                            <asp:TextBox ID="txtedate" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtedate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txteAssign" runat="server" TextMode="MultiLine" CssClass="txtuser"
                                                Height="150px" Width="900px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Lblall" style="font-size: 14px;">Pasting Format : Order No.| State | County | Priority
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnetransmit" runat="server" Text="Transmit" CssClass="MenuFont"
                                                OnClick="btnetransmit_Click" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Panel ID="Panel2" runat="server" Width="800px" Height="300px" ScrollBars="auto"
                                                align="center">
                                                <asp:GridView ID="GridExcelOrders" runat="server" Width="500px" AutoGenerateColumns="false"
                                                    GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No.">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Orderno" HeaderText="Order NO." />
                                                        <asp:BoundField DataField="County" HeaderText="County" />
                                                        <asp:BoundField DataField="State" HeaderText="State" />
                                                        <asp:BoundField DataField="Priority" HeaderText="Priority" />
                                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Button ID="btneupload" runat="server" Text="Upload" CssClass="MenuFont" OnClick="btneupload_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lbleerrmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelReset" runat="server" Width="400px" Visible="False" Height="600px">
                                <table border="0" width="400" cellpadding="5" cellspacing="5" class="Table2">
                                    <tr>
                                        <td colspan="2" class="templatemo_menu_wrapper1" style="height: 20px; color: White; font-size: 16px; font-weight: bold;"
                                            align="center">Reset Orders
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Lblall">
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_date"
                                                Format="MM/dd/yyyy">
                                            </cc1:CalendarExtender>
                                            <asp:Label ID="lbldate" runat="server" Text="Date"></asp:Label>
                                            <asp:TextBox ID="txt_date" runat="server" Text="" CssClass="txtuser" Width="100px"></asp:TextBox>
                                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Width="29px"
                                                CssClass="MenuFont" />
                                            <br />
                                            <br />
                                            <asp:ListBox ID="lstbx" runat="server" Width="200px" Height="400px" CssClass="txtuser"
                                                AutoPostBack="True" OnSelectedIndexChanged="lstbx_SelectedIndexChanged" Font-Names="Verdana"
                                                SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                        <td style="width: 204px">
                                            <asp:Panel ID="pnlreset" runat="server" Height="100px" Width="200px" ToolTip="Select Reset Option"
                                                CssClass="borderonly">
                                                <table style="height: 100; width: 200">
                                                    <tr>
                                                        <td align="center" style="width: 196px; height: 40px;">
                                                            <asp:Label ID="lblTitle" runat="server" Text="Select Reset Option" Font-Bold="True"></asp:Label><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 196px">&nbsp;<asp:ListBox ID="lstStatus" runat="server" Height="56px" Width="181px" CssClass="txtuser"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <br />
                                            <br />
                                            <table class="borderonly" align="center" border="0">
                                                <tr>
                                                    <td style="width: 78px">
                                                        <asp:Button ID="btnreject" runat="server" Text="Reject" CssClass="MenuFont" Width="100px"
                                                            Font-Bold="False" OnClick="btnreject_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="MenuFont" Width="100px"
                                                            OnClick="btndelete_Click" Font-Bold="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 78px">
                                                        <asp:Button ID="btnlock" runat="server" Text="Lock" CssClass="MenuFont" Width="100px"
                                                            OnClick="btnlock_Click" Font-Bold="False" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnprior" runat="server" Text="Priority" CssClass="MenuFont" Width="100px"
                                                            OnClick="btnprior_Click" Font-Bold="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 78px" valign="top">
                                                        <asp:Button ID="btnreset" runat="server" Text="YTS" CssClass="MenuFont" Width="100px"
                                                            OnClick="btnreset_Click" Font-Bold="False" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btninproces" runat="server" Text="Inprocess" CssClass="MenuFont"
                                                            Width="100px" Font-Bold="False" OnClick="btninproces_Click" />&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 78px" align="center" valign="top">
                                                        <asp:Button ID="btnshow" runat="server" Text="Others to YTS" CssClass="MenuFont"
                                                            Width="100px" Font-Bold="False" OnClick="btnshow_Click" />
                                                    </td>
                                                    <td style="width: 78px" align="center" valign="top">
                                                        <asp:Button ID="btdelyts" runat="server" Text="Change Date" CssClass="MenuFont" Width="100px"
                                                            Font-Bold="False" OnClick="btdelyts_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 78px" align="center" valign="top">
                                                        <asp:Button ID="btndelmissing" runat="server" Text="Order Missing" CssClass="MenuFont"
                                                            Width="100px" Font-Bold="False" OnClick="btndelmissing_Click" OnClientClick="return confirm('Do you Want Delete Missing in Atlas Orders?')" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Label ID="lblSearch" runat="server" Text="Search" Font-Names="Verdana"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center" style="height: 26px">
                                                        <asp:TextBox ID="txtSearch" runat="server" Font-Names="Verdana" CssClass="txtuser"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="MenuFont" OnClick="btnSearch_Click"
                                                            Font-Bold="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center" >
                                                        <asp:Label ID="lblErrorSearch" runat="server" Text="" class="LiteralError"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center" style="height: 15px">
                                            <asp:Label ID="lblerrormsg" runat="server" Font-Names="Verdana" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelPriority" runat="server">
                                <table border="0" cellspacing="5" cellpadding="5">
                                    <tr>
                                        <td colspan="3" class="Txthead" align="left">Priority Panel
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtpriority" runat="server" TextMode="MultiLine" CssClass="txtuser"
                                                Height="150px" Width="900px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Lblall" style="font-size: 14px; width: 400px;">Pasting Format : Order No.| Priority
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btnpriortransmit" runat="server" Text="Transmit" CssClass="MenuFont"
                                                OnClick="btnpriortransmit_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Panel ID="PanelGridview" runat="server" Width="800px" Height="300px" ScrollBars="auto"
                                                align="center">
                                                <asp:GridView ID="grdpriority" runat="server" Width="500px" AutoGenerateColumns="True"
                                                    GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Button ID="btnpriorityupdate" runat="server" Text="Update" CssClass="MenuFont"
                                                OnClick="btnpriorityupdate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblerror" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelOrdertype" runat="server">
                                <table border="0" cellspacing="5" cellpadding="5">
                                    <tr>
                                        <td colspan="3" class="Txthead" align="left">Update Order Type
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtordertype" runat="server" TextMode="MultiLine" CssClass="txtuser"
                                                Height="150px" Width="900px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Lblall" style="font-size: 14px; width: 400px;">Pasting Format : State | County | Ordertype
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btntypetransmit" runat="server" Text="Transmit" CssClass="MenuFont"
                                                OnClick="btntypetransmit_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Panel ID="Panel4" runat="server" Width="800px" Height="300px" ScrollBars="auto"
                                                align="center">
                                                <asp:GridView ID="GridOrdertype" runat="server" Width="500px" AutoGenerateColumns="True"
                                                    GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Button ID="btntypeupdate" runat="server" Text="Update" CssClass="MenuFont" OnClick="btntypeupdate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lbltypeerror" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelHighPriority" runat="server">
                                <table border="0" cellspacing="5" cellpadding="5">
                                    <tr>
                                        <td colspan="3" class="Txthead" align="left">High Priority Panel
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txthp" runat="server" TextMode="MultiLine" CssClass="txtuser" Height="150px"
                                                Width="900px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Lblall" style="font-size: 14px; width: 400px;">Pasting Format : Order No.
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btnhpriotrans" runat="server" Text="Transmit" CssClass="MenuFont"
                                                OnClick="btnhpriotrans_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Panel ID="PanelHpGridview" runat="server" Width="800px" Height="300px" ScrollBars="auto"
                                                align="center">
                                                <asp:GridView ID="grdhpriority" runat="server" Width="500px" AutoGenerateColumns="True"
                                                    GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Button ID="btnhprioupdate" runat="server" Text="Update" CssClass="MenuFont"
                                                OnClick="btnhprioupdate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblhperror" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelTracking" runat="server" Height="600px">
                                <table border="0" cellspacing="5" cellpadding="5">
                                    <tr>
                                        <td colspan="3" class="Txthead" align="left">Tracking No Update Panel
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txttracking" runat="server" TextMode="MultiLine" CssClass="txtuser"
                                                Height="150px" Width="900px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Lblall" style="font-size: 14px; width: 400px;">Pasting Format : Order No.| Tracking No. | Cheque No.
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btntracking" runat="server" Text="Update" CssClass="MenuFont" OnClick="btntracking_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lbltrackingerror" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <%-- <asp:Panel ID="PanelStatusChange" runat="server" Height="600px">
                            <table border="0" cellspacing="5" cellpadding="5">                                
                                <tr>
                                    <td colspan="2" class="Txthead" align="left">Move To YTS</td>
                                    <td align="right" style="font-size:18px;font-weight:bold;">Username : 
                                        <asp:DropDownList ID="ddlusername" runat="server" CssClass="txtuser" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtstatuschange" runat="server" TextMode="MultiLine" CssClass="txtuser" Height="150px" Width="900px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  class="Lblall" style="font-size:14px;width:400px;">Pasting Format : Order No.</td>
                                    <td align="left"><asp:Button ID="btnstatuschange" runat="server" Text="Update" CssClass="MenuFont" OnClick="btnstatuschange_Click"/></td>
                                    <td><asp:Button ID="btnassignorder" runat="server" Text="Assign User" CssClass="MenuFont"  OnClick="btnassignorder_Click" /></td>
                                </tr>                                                                
                                
                                <tr><td colspan="3" align="center"><asp:Label ID="lblstatuserror" runat="server" Text="" ForeColor="Red"></asp:Label></td></tr>
                            </table>
                        </asp:Panel>--%>

                            <asp:Panel ID="PanelStatusChange" runat="server" Width="90%">
                                <table border="0" cellspacing="5" cellpadding="5" width="100%">
                                    <tr>
                                        <td class="Txthead" colspan="3" align="center"></td>
                                    </tr>
                                    <tr>
                                        <td class="Txthead" colspan="3" align="center">Status Change Panel
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%;" align="right">
                                            <asp:TextBox ID="txtmovestatus" runat="server" TextMode="MultiLine" CssClass="txtuser"
                                                Height="400px" Width="200px" OnTextChanged="txtmovestatus_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 20%; padding-top: 30px;" valign="top" align="center">
                                            <table width="80%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:RadioButtonList ID="rdbtnstatuschange" runat="server" Width="100%" CssClass="Lblall1">
                                                            <asp:ListItem Value="0">InProcess</asp:ListItem>
                                                            <asp:ListItem Value="1">ParcelID</asp:ListItem>
                                                            <asp:ListItem Value="2">Mailaway</asp:ListItem>
                                                            <asp:ListItem Value="3">OnHold</asp:ListItem>
                                                            <asp:ListItem Value="4">Rejected</asp:ListItem>
                                                            <asp:ListItem Value="5">YTS</asp:ListItem>
                                                            <asp:ListItem Value="6">DU</asp:ListItem>
                                                            <asp:ListItem Value="7">Move QC</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnmove" runat="server" Text=">>" CssClass="MenuFont" Width="150px"
                                                            Font-Bold="true" OnClick="btnmove_Click" />
                                                    </td>
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlusername" runat="server" CssClass="txtuser" Width="150px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnassignorder" runat="server" Text="Assign Order" CssClass="MenuFont"
                                                            Width="150px" Font-Bold="true" OnClick="btnassignorder_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 35%;" align="left">
                                            <asp:TextBox ID="txtstatuschange" runat="server" TextMode="MultiLine" CssClass="txtuser"
                                                Height="400px" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblstatuserror" runat="server" Font-Names="Verdana" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td colspan="3"></td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelAssignOrder" runat="server" Width="90%">
                                <table border="0" cellspacing="5" cellpadding="5" width="100%">

                                    <tr>
                                        <td class="Txthead" colspan="3" align="left" style="text-decoration: underline; font-size: 20px; top: -30px; color: black">Order Assign To User
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtassignorder" runat="server" TextMode="MultiLine" class="form-control"
                                                Height="400px" Width="200px" OnTextChanged="txtassignorder_TextChanged" AutoPostBack="true" Style="resize: none;"></asp:TextBox>
                                        </td>
                                        <td valign="top" align="center">
                                            <table>
                                                <tr>
                                                    <td style="font-family:Calibri;">
                                                        <asp:DropDownList ID="ddlusernameassign" runat="server" class="form-control" onchange="txtdeliquentsta1()">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnmoveassignorder" runat="server" Text="Assign QC" CssClass="btn btn-success" Width="85px" OnClick="btnmoveassignorder_Click" Style="margin-top: 12px;font-family:Calibri;" />
                                                        <asp:Button ID="btnassignorderuser" runat="server" Text="Assign Production/DU" CssClass="btn btn-success" Width="185px" OnClick="btnassignorderuser_Click" Style="margin-top: 12px;font-family:Calibri;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr style="height: 10px;">
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblstatusassignorder" runat="server" Font-Names="Verdana" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>

                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelClearDb" runat="server" Width="65%" Height="600px">
                                <table width="100%">
                                    <tr style="height: 100px;">
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 35%; font-family: Calibri; font-size: 18px; font-weight: bold;">Please Enter the Password:
                                        </td>
                                        <td style="width: 50%" align="left">
                                            <asp:TextBox ID="txtpassword" runat="server" CssClass="txtuser" Width="65%" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 50px;">
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnok" runat="server" Text="Ok" CssClass="MenuFont" Width="100px"
                                                OnClick="btnok_Click" />
                                        </td>
                                    </tr>
                                    <tr style="height: 50px;">
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lbldberror" runat="server" ForeColor="Red" Font-Size="18px" Width="100%"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelDeleteDb" runat="server" Width="100%" Height="600px">
                                <table width="80%" align="left" style="padding-left: 100px;">
                                    <tr style="height: 20px;">
                                        <td colspan="6"></td>
                                    </tr>
                                    <tr>
                                        <td class="Lblalldb" align="right">From
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtfrmdate" runat="server" CssClass="txtuser" Width="150px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtfrmdate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="Lblalldb" align="right">To
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txttodate" runat="server" CssClass="txtuser" Width="150px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txttodate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnordershow" runat="server" Text="Show" CssClass="MenuFont" OnClick="btnordershow_Click"
                                                Width="100px" />
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnshowdelete" runat="server" Text="Delete" CssClass="MenuFont" OnClick="btnshowdelete_Click"
                                                Width="100px" />
                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                        <td colspan="6"></td>
                                    </tr>
                                    <tr style="height: 100px;">
                                        <td colspan="6" valign="middle">
                                            <asp:Panel ID="Panelcleardata" runat="server" Width="100%" Height="600px" ScrollBars="Auto">
                                                <asp:GridView ID="GridClearDb" runat="server" Width="90%" FooterStyle-CssClass="Gridfooterbar"
                                                    ShowFooter="false" AutoGenerateColumns="true" GridLines="None" CssClass="Gnowrap"
                                                    AlternatingRowStyle-CssClass="alt">
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                        <td colspan="6"></td>
                                    </tr>
                                    <tr>
                                        <asp:Label ID="lblerrordb" runat="server" ForeColor="Red" Font-Size="18px" Width="100%"></asp:Label>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="pnlClrYts" runat="server" Width="65%" Height="600px">
                                <table width="100%">
                                    <tr style="height: 100px;">
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 35%; font-family: Calibri; font-size: 18px; font-weight: bold;">Please Enter the Password:
                                        </td>
                                        <td style="width: 50%" align="left">
                                            <asp:TextBox ID="txtClrYts" runat="server" CssClass="txtuser" Width="65%" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 50px;">
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnClrYts" runat="server" Text="Ok" CssClass="MenuFont"
                                                Width="100px" OnClick="btnClrYts_Click" />
                                        </td>
                                    </tr>
                                    <tr style="height: 50px;">
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblYtsError" runat="server" ForeColor="Red" Font-Size="18px" Width="100%"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelDeleteyts" runat="server" Width="100%" Height="600px">
                                <table width="80%" align="left" style="padding-left: 100px;">
                                    <tr style="height: 20px;">
                                        <td colspan="6"></td>
                                    </tr>
                                    <tr>
                                        <td class="Lblallyts" align="right">Date
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtdateyts" runat="server" CssClass="txtuser" Width="150px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtenderyts" runat="server" TargetControlID="txtdateyts"
                                                Format="MM/dd/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td align="left" colspan="3">
                                            <asp:Button ID="btndeleteyts" runat="server" Text="Delete" CssClass="MenuFont" Width="100px"
                                                OnClick="btndeleteyts_Click" />
                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                        <td colspan="6"></td>
                                    </tr>
                                    <tr style="height: 20px;">
                                        <td colspan="6"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" align="center">
                                            <asp:Label ID="lblerroryts" runat="server" ForeColor="Red" Font-Size="18px" Width="100%"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="PanelSettings" runat="server" Width="100%" Height="100%">
                                <table border="0">
                                    <tr>
                                        <td align="center" style="font-family:Calibri">
                                            <asp:ListBox ID="Lstuser" class="form-control" AutoPostBack="true" runat="server" Width="200px"
                                                Height="240px" OnSelectedIndexChanged="Lstuser_SelectedIndexChanged" SelectionMode="Multiple" style="background-color:white;color:black;overflow:auto;"></asp:ListBox>
                                        </td>
                                        <td align="center">
                                            <asp:Panel ID="Sidepanel" runat="server" Height="600px" Width="200px" ScrollBars="Vertical" Visible="false">
                                                <table style="height: 100%;" width="100%">
                                                    <tr>
                                                        <td style="width: 100%; height: 100%;">
                                                            <asp:GridView ID="Gridstatecount" runat="server" AutoGenerateColumns="true" Width="100%"
                                                                GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowDataBound="Gridstatecount_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkselect" ToolTip="Click here to select/deselect all rows" runat="server" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td align="center" valign="top">
                                            <table border="0">
                                                <tr>
                                                    <td class="Txthead" colspan="3" align="center" style="text-decoration: underline; font-size: 20px; top: -12px; color: black;font-family:Calibri">User Maintenance
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 90px;">
                                                        <table border="0">
                                                            <tr>
                                                                <td class="Lblall">FullName
                                                                </td>
                                                                <td style="font-family:Calibri">
                                                                    <asp:TextBox ID="txtfulname" runat="server" CssClass="txtuser form-control" Width="200px"></asp:TextBox>                                                    
                                                                </td>
                                                                <td>
                                                                    <span style="color: Red;margin-left:-16px;">*</span>
                                                                </td>
                                                                <td class="Lblall">UserName
                                                                </td>
                                                                <td style="font-family:Calibri">
                                                                    <asp:TextBox ID="txtusername" runat="server" CssClass="txtuser form-control" Width="200px" style="margin-left:10px;"></asp:TextBox>                                                                    
                                                                </td>
                                                                <td>
                                                                    <span style="color: Red;margin-left:5px;">*</span>
                                                                </td>
                                                            </tr>
<%--                       
                                                            <tr style="visibility: hidden;">
                                                                <td class="Lblall">Order Type
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlotype" runat="server" CssClass="txtuser" Width="200px" Height="21px">
                                                                    </asp:DropDownList>
                                                                    <span style="color: Red;">*</span>
                                                                </td>
                                                            </tr>
                                                            <tr style="visibility: hidden;">
                                                                <td class="Lblall">States
                                                                </td>
                                                                <td>
                                                                    <asp:Panel ID="Panel1" runat="server" Width="75%" Height="80px" ScrollBars="Auto">
                                                                        <asp:Label ID="lblstates" runat="server" Text="" Height="50px"></asp:Label>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td colspan="2" align="center">
                                                                    <table cellpadding="3" cellspacing="3" width="300" border="0">
                                                                        <tr>
                                                                            <td class="Lblall" style="padding-left: 20px;">
                                                                                <asp:CheckBox ID="Chkprio" runat="server" Text="High Priority" Visible="false"></asp:CheckBox>
                                                                            </td>
                                                                            <td class="Lblall" style="padding-left: 20px; visibility: hidden;">
                                                                                <asp:CheckBox ID="Chkadmin" runat="server" Text="Admin" Visible="false"></asp:CheckBox>&nbsp;<asp:CheckBox
                                                                                    ID="chkbxqa" runat="server" Text="QA"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Lblall" style="padding-left: 20px;">
                                                                                <asp:CheckBox ID="chkpriority" runat="server" Text="Priority" Visible="false" />
                                                                            </td>

                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Lblall" style="font-family:Calibri;font-size:15px;">
                                                                                <asp:CheckBox ID="Chkkey" runat="server" Text="Production"/>
                                                                            </td>
                                                                            <td class="Lblall" style="padding-left: 20px; width: 119px;font-family:Calibri;font-size:15px;">
                                                                                <asp:CheckBox ID="ChkQC" runat="server" Text="QC"/>
                                                                            </td>
                                                                            <td class="Lblall" style="font-family:Calibri;font-size:15px;">
                                                                                <asp:CheckBox ID="Chkdu" runat="server" Text="DU"/>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Lblall" style="padding-left: 20px; width: 119px; visibility: hidden;">
                                                                                <asp:CheckBox ID="ChkOnhold" runat="server" Text="OnHold" />
                                                                            </td>
                                                                            <td class="Lblall" style="padding-left: 20px; visibility: hidden;">
                                                                                <asp:CheckBox ID="Chkpending" runat="server" Text="Inprocess" />
                                                                            </td>
                                                                        </tr>
                                                                       <%-- <tr>
                                                                            <td class="Lblall" style="padding-left: 20px; width: 119px; visibility: hidden;">
                                                                                <asp:CheckBox ID="Chkparcelid" runat="server" Text="ParcelId" />
                                                                            </td>
                                                                            <td class="Lblall" style="padding-left: 20px; visibility: hidden;">
                                                                                <asp:CheckBox ID="Chkmail" runat="server" Text="Mailaway" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Lblall" style="padding-left: 20px; width: 119px; visibility: hidden;">
                                                                                <asp:CheckBox ID="Chkreview" runat="server" Text="Post Audit" />
                                                                            </td>
                                                                        </tr>--%>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="6">
                                                                    <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnsave_Click" Visible="false"/>
                                                                    <asp:Button ID="BtnUpdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="BtnUpdate_Click" />
                                                                    <%--<asp:Button ID="Btnclear" runat="server" Text="Clear" CssClass="MenuFont" OnClick="Btnclear_Click"/>--%>
                                                                    <asp:Button ID="Btnnewuser" runat="server" Text="New User" CssClass="btn btn-success" OnClick="Btnnewuser_Click" />
                                                                    <asp:Button ID="Button1" runat="server" Text="ResetPwd" CssClass="btn btn-success" OnClick="Btnreset_Click"
                                                                        OnClientClick="return confirm('Do you want to reset the password?');" Width="130px" Visible="false"/>
                                                                    <asp:Button ID="Button2" runat="server" Text="Delete" CssClass="btn btn-success" OnClick="Btndelete_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" class="LiteralError" align="center">
                                                                    <asp:Literal ID="LiteralErr" runat="server"></asp:Literal>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table style="border: solid 1px Gray; width: 425px; border-radius: 10px;margin-top:35px;" cellspacing="5">
                                                            <tr>
                                                                <td align="center" style="font-family:Calibri; font-size: 16px;">Reset Break
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txt_reason" runat="server" TextMode="MultiLine" Width="414px" style="margin-left:4px;resize:none;height:115px;"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Button ID="btn_reset_brk" runat="server" Text="Reset" CssClass="btn btn-info" OnClick="btn_reset_brk_Click" style="height:35px;margin-bottom:5px;"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbl_brk_error" runat="server" Text="" CssClass="ErrorMsg"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <asp:Label ID="errorlabel" runat="server" Text="" Font-Names="Verdana" ForeColor="Red"></asp:Label>
        </tr>
    </table>
    <div class="footer" style="text-align: center;">
        <p style="background-color: #337ab7; margin-left: 2px; margin-bottom: 0px; color: white;height:26px;">
            &copy; 2019. All rights reserved | Designed & Developed by String Information Services
        </p>
    </div>

 
</asp:Content>
