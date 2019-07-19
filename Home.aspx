<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Pages_Home" Theme="Black" Title="HOME" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table border="0" style="width:1340px;height:600px;background-color:#f5f5f5;" align="center">                
    <tr>
    <td style="width:300px;padding-top:10px;" align="center" valign="top">
        <table>            
            <tr>
                <td>
                    <div class="urbangreymenu">   
                    <h3 class="headerbar">Utilization</h3>                         
                    <ul>
                        <%--<li><asp:LinkButton ID="today" runat="server" OnClick="today_Click">Today</asp:LinkButton></li>--%>
                        <li><asp:LinkButton ID="lastweek" runat="server" OnClick="lastweek_Click">Last Week</asp:LinkButton></li>
                        <li><asp:LinkButton ID="last30days" runat="server" OnClick="last30days_Click">Last 30 Days</asp:LinkButton></li>                                                                
                        <li><asp:LinkButton ID="last90days" runat="server" OnClick="last90days_Click">Last 90 Days</asp:LinkButton></li>                                                                
                    </ul>
                    <%--<h3 class="headerbar">Quality</h3> 
                    <ul>
                        <li><asp:LinkButton ID="lnkfpyreport" runat="server" OnClick="lnkfpyreport_Click">FPY Report</asp:LinkButton></li>
                        <li><asp:LinkButton ID="lnkpostaudit" runat="server" OnClick="lnkpostaudit_Click">Post Audit Report</asp:LinkButton></li>
                    </ul> --%>
                    </div>   
                </td>
            </tr>
        </table>        
    </td>    
    <td align="Left" valign="top">
       <table border="0">
        <tr><td class="Txthead" align="center"><asp:Label ID="Lblinfo" runat="server" Text=""></asp:Label></td></tr>
        <tr>
            <td>
                <cc1:CollapsiblePanelExtender ID="CollapeExeProduction" runat="server" 
                    TargetControlID="PanelGrid"
                    ExpandControlID="TitlePanel" 
                    CollapseControlID="TitlePanel" 
                    Collapsed="True"
                    TextLabelID="Label1" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="Image1" 
                    ExpandedImage="../App_themes/Black/images/collapse_blue.jpg" 
                    CollapsedImage="../App_themes/Black/images/expand_blue.jpg"
                    SuppressPostBack="true">
                    
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="TitlePanel" runat="server" Width="900px"> 
                   <table width="100%">
                    <tr>
                        <td style="width:100%;" align="left">
                            <div class="header_00"><span></span>PRODUCTION / DIRECT UPLOAD
                            </div>
                        </td>
                    </tr>
                   </table>
                </asp:Panel>
                
                <asp:Panel ID="PanelGrid" runat="server" Width="900px" Height="730px" ScrollBars="auto" align="center">
                    <asp:GridView ID="Gridutilization" runat="server" Width="850px" FooterStyle-CssClass="Gridfooterbar" ShowFooter="true" AutoGenerateColumns="false"  GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt"  OnRowDataBound="Gridutilization_RowDataBound" AllowSorting="true" OnSorting="Gridutilization_Sorting">
                        <emptydatarowstyle backcolor="LightBlue" forecolor="Red" Font-Names="Georgia"/>
                        <emptydatatemplate><asp:image id="NoDataImage" runat="server" />No Data Found.</emptydatatemplate> 
                       <Columns>
                        <asp:TemplateField HeaderText="Sno." HeaderStyle-Width="45px">                        
                            <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>                            
                        </asp:TemplateField>             
                        
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-ForeColor="white" ItemStyle-HorizontalAlign="Left">                                                
                        </asp:BoundField>      
                        
                        <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Website" HeaderText="Website" SortExpression="Website" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Mailaway" HeaderText="Mailaway" SortExpression="Mailaway" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="TotalOrders" HeaderText="TotalOrders" SortExpression="TotalOrders" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="PhoneTime" HeaderText="PhoneTime" SortExpression="PhoneTime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="WebsiteTime" HeaderText="WebsiteTime" SortExpression="WebsiteTime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="MailawayTime" HeaderText="MailawayTime" SortExpression="MailawayTime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Totaltime" HeaderText="Totaltime" SortExpression="Totaltime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="AvgTime" HeaderText="AvgTime" SortExpression="AvgTime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                                               
                        </Columns>           
                     </asp:GridView>
                </asp:Panel>
                
                <cc1:CollapsiblePanelExtender ID="CollapeExeQC" runat="server" 
                    TargetControlID="PanelGridQc"
                    ExpandControlID="TitlePanelQc" 
                    CollapseControlID="TitlePanelQc" 
                    Collapsed="True"
                    TextLabelID="Label1" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="Image1" 
                    ExpandedImage="../App_themes/Black/images/collapse_blue.jpg" 
                    CollapsedImage="../App_themes/Black/images/expand_blue.jpg"
                    SuppressPostBack="true">
                    
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="TitlePanelQc" runat="server" Width="900px"> 
                   <table width="100%">
                    <tr>
                        <td style="width:100%;" align="left">
                            <div class="header_00"><span></span>QC</div>
                        </td>
                    </tr>
                   </table>
                </asp:Panel>
                
                <asp:Panel ID="PanelGridQc" runat="server" Width="900px" Height="730px" ScrollBars="auto" align="center">
                     <asp:GridView ID="GridutilizationQC" runat="server" Width="850px" FooterStyle-CssClass="Gridfooterbar" ShowFooter="true" AutoGenerateColumns="false"  GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowDataBound="GridutilizationQC_RowDataBound" AllowSorting="true" OnSorting="GridutilizationQC_Sorting">
                        <emptydatarowstyle backcolor="LightBlue" forecolor="Red" Font-Names="Georgia"/>
                        <emptydatatemplate><asp:image id="NoDataImage" runat="server" />No Data Found.</emptydatatemplate> 
                       <Columns>
                        <asp:TemplateField HeaderText="Sno." HeaderStyle-Width="45px">                        
                            <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>                            
                        </asp:TemplateField>             
                        
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-ForeColor="white" ItemStyle-HorizontalAlign="Left">                                                
                        </asp:BoundField>      
                        
                        <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Website" HeaderText="Website" SortExpression="Website" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Mailaway" HeaderText="Mailaway" SortExpression="Mailaway" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="TotalOrders" HeaderText="TotalOrders" SortExpression="TotalOrders" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="PhoneTime" HeaderText="PhoneTime" SortExpression="PhoneTime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="WebsiteTime" HeaderText="WebsiteTime" SortExpression="WebsiteTime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="MailawayTime" HeaderText="MailawayTime" SortExpression="MailawayTime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Totaltime" HeaderText="Totaltime" SortExpression="Totaltime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="AvgTime" HeaderText="AvgTime" SortExpression="AvgTime" HeaderStyle-ForeColor="white">                                                
                        </asp:BoundField>
                                            
                        </Columns>           
                     </asp:GridView>
                </asp:Panel>
                
                 <cc1:CollapsiblePanelExtender ID="CollapeExeFPY" runat="server" 
                    TargetControlID="PanelQulaityGrid"
                    ExpandControlID="TitlePanelFPY" 
                    CollapseControlID="TitlePanelFPY" 
                    Collapsed="True"
                    TextLabelID="Label1" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="Image1" 
                    ExpandedImage="../App_themes/Black/images/collapse_blue.jpg" 
                    CollapsedImage="../App_themes/Black/images/expand_blue.jpg"
                    SuppressPostBack="true">
                    
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="TitlePanelFPY" runat="server" Width="900px"> 
                   <table width="100%">
                    <tr>
                        <td style="width:100%;" align="left">
                            <div class="header_00"><span></span>FPY REPORT</div>
                        </td>
                    </tr>
                   </table>
                </asp:Panel>
                
                <asp:Panel ID="PanelQulaityGrid" runat="server" Width="900px" Height="730px" ScrollBars="auto" align="center">
                    <asp:GridView ID="QualityGrid" runat="server" Width="850px" FooterStyle-CssClass="Gridfooterbar" ShowFooter="true" AutoGenerateColumns="false"  GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowDataBound="QualityGrid_RowDataBound"  >
                        <emptydatarowstyle backcolor="LightBlue" forecolor="Red" Font-Names="Georgia"/>
                        <emptydatatemplate><asp:image id="NoDataImage" runat="server" />No Data Found.</emptydatatemplate> 
                        <Columns>
                            <asp:TemplateField HeaderText="Sno." HeaderStyle-Width="45px">                        
                                <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>                            
                            </asp:TemplateField>             
                            
                            <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-ForeColor="white" ItemStyle-HorizontalAlign="Left">                                                
                            </asp:BoundField>      
                            
                            <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Website" HeaderText="Website" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Mailaway" HeaderText="Mailaway" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="TotalOrders" HeaderText="TotalOrders" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Error" HeaderText="No.of Defects" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Quality" HeaderText="Quality" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                                            
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                
                
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" 
                    TargetControlID="PanelQulaityGridPostAudit"
                    ExpandControlID="TitlePanelPostAudit" 
                    CollapseControlID="TitlePanelPostAudit" 
                    Collapsed="True"
                    TextLabelID="Label1" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="Image1" 
                    ExpandedImage="../App_themes/Black/images/collapse_blue.jpg" 
                    CollapsedImage="../App_themes/Black/images/expand_blue.jpg"
                    SuppressPostBack="true">
                    
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="TitlePanelPostAudit" runat="server" Width="900px"> 
                   <table width="100%">
                    <tr>
                        <td style="width:100%;" align="left">
                            <div class="header_00"><span></span>POST AUDIT REPORT</div>
                        </td>
                    </tr>
                   </table>
                </asp:Panel>
                
                <asp:Panel ID="PanelQulaityGridPostAudit" runat="server" Width="900px" Height="730px" ScrollBars="auto" align="center">
                    <asp:GridView ID="QualityGridPostAudit" runat="server" Width="850px" FooterStyle-CssClass="Gridfooterbar" ShowFooter="true" AutoGenerateColumns="false"  GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowDataBound="QualityGridPostAudit_RowDataBound"  >
                        <emptydatarowstyle backcolor="LightBlue" forecolor="Red" Font-Names="Georgia"/>
                        <emptydatatemplate><asp:image id="NoDataImage" runat="server" />No Data Found.</emptydatatemplate> 
                        <Columns>
                            <asp:TemplateField HeaderText="Sno." HeaderStyle-Width="45px">                        
                                <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>                            
                            </asp:TemplateField>             
                            
                            <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-ForeColor="white" ItemStyle-HorizontalAlign="Left">                                                
                            </asp:BoundField>      
                            
                            <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Website" HeaderText="Website" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Mailaway" HeaderText="Mailaway" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="TotalOrders" HeaderText="TotalOrders" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Error" HeaderText="No.of Defects" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Quality" HeaderText="Quality" HeaderStyle-ForeColor="white">                                                
                            </asp:BoundField>
                                            
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>       
       </table> 
    </td>
    </tr>                
</table>
</asp:Content>
