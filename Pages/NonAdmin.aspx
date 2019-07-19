<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true" CodeFile="NonAdmin.aspx.cs" Inherits="Pages_NonAdmin" Theme="black" Title="NonAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript" src="../Script/jquery-1.4.1.min.js"></script>
<script type="text/javascript" src="../Script/ScrollableGridPlugin.js"></script>
<script type = "text/javascript">
$(document).ready(function () {
    $('#<%=GridUser.ClientID %>').Scrollable({
        ScrollHeight: 600
    });
});
</script>


    <table width="1340px" style="background-color:#f5f5f5;font-family:Calibri;">
        <tr>
            <td align="center">
                <table>
                    <tr>
                    <%--<td class="Lblall">From:</td>
                    <td><asp:TextBox id="txtfrom" runat="server" CssClass="txtuser" Width="80px"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server" Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>
                    </td>
                    <td class="Lblall">To:</td>
                    <td><asp:TextBox id="txtto" runat="server" CssClass="txtuser" Width="80px"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server" Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>
                    </td>
                    <td><asp:Button ID="Btnshow" runat="server" Text="Show" CssClass="MenuFont"/></td>--%>
                    <td class="Lblall" align="center"  style="width:100px;">Order No</td>
                    <td align="center"><asp:TextBox ID="txtordersearch" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox></td>
                    <td align="center"><asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="MenuFont" OnClick="btnsearch_Click"/></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="5" align="center">
                <asp:Panel ID="PanelGrid" runat="server" Height="700px" Width="1200px" ScrollBars="auto">
                    <asp:GridView ID="GridUser" runat="server" AutoGenerateColumns="false" DataKeyNames="id" Width="3000px" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowCommand="GridUser_RowCommand" OnRowDataBound="GridUser_RowDataBound">                        
                        <Columns>
                            <asp:TemplateField HeaderText="SNo#">
                                <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>                                                                                    
                            <asp:TemplateField HeaderText="Order No">
                                <ItemTemplate>                                    
                                    <asp:LinkButton ID="Lnkorder" runat="server" Text='<%# Eval("Order No") %>' CommandName="Process" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Date" HeaderText="Date" />
                            <asp:BoundField DataField="PTY" HeaderText="PTY"/>
                            <asp:BoundField DataField="Zone" HeaderText="Zone" />
                            <asp:BoundField DataField="State" HeaderText="State" />
                            <asp:BoundField DataField="County" HeaderText="County" />
                            <asp:BoundField DataField="Township" HeaderText="Township" />
                            <asp:BoundField DataField="Type" HeaderText="Type" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="K1 Name" HeaderText="K1 Name" />
                            <asp:BoundField DataField="Comments" HeaderText="Comments" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="K1Start Time" HeaderText="Start Time" />
                            <asp:BoundField DataField="K1End Time" HeaderText="End Time" />
                            <asp:BoundField DataField="K1Time Taken" HeaderText="Time Taken" />
                            <asp:BoundField DataField="QC Name" HeaderText="QC Name" />
                            <asp:BoundField DataField="QCStart Time" HeaderText="Start Time" />
                            <asp:BoundField DataField="QCEnd Time" HeaderText="End Time" />
                            <asp:BoundField DataField="QCTime Taken" HeaderText="Time Taken" />
                            <asp:BoundField DataField="Upload Time" HeaderText="Upload Time" />
                            <asp:BoundField DataField="TAT" HeaderText="TAT" />                                                     
                            <asp:BoundField DataField="Delivered Date" HeaderText="Delivered Date" />
                            <asp:BoundField DataField="Post Audit" HeaderText="Post Audit" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>                
            </td>
        </tr>
        <tr><td colspan="5" align="center"><asp:Label ID="errorlabel" runat="server" Text="" Font-Names="Verdana" ForeColor="Red"></asp:Label></td></tr>
    </table>

</asp:Content>


