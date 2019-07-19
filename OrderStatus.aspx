<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true" CodeFile="OrderStatus.aspx.cs" Inherits="Pages_OrderStatus" Theme="Black" Title="ORDERSTATUS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="../Script/jquery-1.4.1.min.js"></script>
<script type="text/javascript" src="../Script/ScrollableGridPlugin.js"></script>
<script type = "text/javascript">
$(document).ready(function () {
    $('#<%=GridUser.ClientID %>').Scrollable({
        ScrollHeight: 800
    });
//    $('#<%=GridUserUtilization.ClientID %>').Scrollable({
//        ScrollHeight: 800
//    });
});
</script>

  <table border="0" width="100%" style="background-color:#f5f5f5;">
        <tr>
           <td style="width: 125px; height: 621px" align="right" valign="top">
               <asp:Panel ID="Sidepanel" runat="server">
                    <table style="height:600px;">
                        <tr><td class="Txthead" align="Left"><asp:Label ID="lblstate" runat="server" Text="ME-Cumberland" ></asp:Label></td></tr>
                        <tr>
                            <td style="width: 128px" align="center">
                                <asp:Label ID="lbltotalOrders" runat="server" Text="Total Orders : " Font-Bold="true" Font-Size="17px" ForeColor="black"></asp:Label>
                                <asp:Label ID="lbltotalcount" runat="server" Font-Size="17px" ForeColor="black" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr><td class="Txthead" align="Left">All Records</td></tr>
                        <tr>
                            <td style="width: 128px">
                                <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" AutoGenerateRows="false" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" Width="125px" Font-Names="Verdana" Font-Size="Smaller">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Fields>
                                        <asp:TemplateField HeaderText="Working">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnwrkng" runat="server" ForeColor="Black" 
                                                    OnClick="lnkbtnwrkng_Click" 
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Working") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mailaway">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnmnway" runat="server" ForeColor="Black" 
                                                    OnClick="lnkbtnmnway_Click" 
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Mailaway") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Inprocess">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtninproc" runat="server" ForeColor="Black" 
                                                    OnClick="lnkbtninproc_Click" 
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Inprocess") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ParcelID">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnpid" runat="server" ForeColor="Black" 
                                                    OnClick="lnkbtnpid_Click" 
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.ParcelID") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OnHold">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnonhold" runat="server" ForeColor="Black" 
                                                    OnClick="lnkbtnonhold_Click" 
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Onhold") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtntotal" runat="server" ForeColor="Black" 
                                                    OnClick="lnkbtntotal_Click" 
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Total") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField HeaderText="KeyCompletedTotal">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnKeytotal" runat="server" ForeColor="Black" 
                                                    OnClick="lnkbtnKeytotal_Click" 
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.KEYCOMPLETEDALL") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Fields>
                                </asp:DetailsView>
                            </td>
                         </tr>
                         <tr><td class="Txthead" align="Left">Date Wise Records</td></tr>
                         <tr>
                            <td style="width: 128px;">
                                <asp:DetailsView ID="DetailsView2" runat="server" Height="50px" CssClass="Gnowrap" AutoGenerateRows="false" AlternatingRowStyle-CssClass="alt" Width="125px" Font-Names="Verdana" Font-Size="Smaller">
                                    <Fields>
                                       <asp:TemplateField HeaderText="Total">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtntotal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.total") %>' ForeColor="Black" OnClick="ftlnkbtntotal_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="YTS">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtnyts" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.YTS") %>' ForeColor="Black" OnClick="ftlnkbtnyts_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Working">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtnwrkng" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Working") %>' ForeColor="Black" OnClick="ftlnkbtnwrkng_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="KeyCompleted">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtnkeycmd" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KeyCompleted") %>' ForeColor="Black" OnClick="ftlnkbtnkeycmd_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Mailaway">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtnmnway" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Mainaway") %>' ForeColor="Black" OnClick="ftlnkbtnmnway_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Inprocess">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtninproc" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Inprocess") %>' ForeColor="Black" OnClick="ftlnkbtninproc_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>                                         
                                         <asp:TemplateField HeaderText="ParcelID">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtnpid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ParcelID") %>' ForeColor="Black" OnClick="ftlnkbtnpid_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="OnHold">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtnonhold" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OnHold") %>' ForeColor="Black" OnClick="ftlnkbtnonhold_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="HP">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="lnkbtnhp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.HP") %>' ForeColor="Black" OnClick="lnkbtnhp_Click"> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Followup">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="lnkbtnfollowup" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Followupdate") %>' ForeColor="Black" OnClick="lnkbtnfollowup_Click"> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Closing Date">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="lnkbtnclosedate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CloseDate") %>' ForeColor="Black" OnClick="lnkbtnclosedate_Click"> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="OrderMissing">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="lnkbtnmissing" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrderMissing") %>' ForeColor="Black" OnClick="lnkbtnmissing_Click"> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="PostAudit">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="lnkbtnpostaudit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PostAudit") %>' ForeColor="Black" OnClick="lnkbtnpostaudit_Click"> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>                              
                                         <asp:TemplateField HeaderText="Rejected">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtnrej" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Rejected") %>' ForeColor="Black" OnClick="ftlnkbtnrej_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Others">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtnothers" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Others") %>' ForeColor="Black" OnClick="ftlnkbtnothers_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>  
                                         <asp:TemplateField HeaderText="Delivered">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="ftlnkbtncmd" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Completed") %>' ForeColor="Black" OnClick="ftlnkbtncmd_Click">> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="TotalDelivered">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="lnkbtncmdall" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CompletedAll") %>' ForeColor="Black" OnClick="lnkbtncmdall_Click"> </asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>                              
                                    </Fields>
                                </asp:DetailsView>
                             </td>
                         </tr>
                    </table>
                </asp:Panel>
            </td>
            <td style="height:621px;" align="center" valign="top">
                <table class="Table1" border="0" width="100%" style="height:100%;">
                    <tr>
                        <td align="center" style="height:100%;width:100%;">
                            <asp:Panel ID="PanelOrderList" runat="server" align="center" Width="100%">
                                <table border="0" cellspacing="5" cellpadding="5" width="100%">                                                                                                       
                                    <tr>
                                        <td><asp:CheckBox ID="Chkrefresh" runat="server" CssClass="Autorefresh" OnCheckedChanged="Chkrefresh_CheckedChanged" AutoPostBack="true" Text="Auto Refresh"/>
                                          <asp:Timer id="Refresh" runat="server" Interval="120000" Enabled ="false" OnTick="Refresh_Tick"></asp:Timer>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="Lnkexport" runat="server" ImageUrl="~/App_themes/Black/images/Excel.png" Height="50px" Width="50px" ToolTip="Export" OnClick="Lnkexport_Click" />
                                        </td>
                                        <td class="Lblall" align="right">From</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtfrmdate" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfrmdate" Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="Lblall" align="right">To</td>
                                        <td align="left">
                                            <asp:TextBox ID="txttodate" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnordershow" runat="server" Text="Show" CssClass="MenuFont" OnClick="btnordershow_Click"/></td>
                                        <td align="center"><asp:Button ID="btnutilshow" runat="server" Text="Show Utilization" Width="125px" CssClass="MenuFont" OnClick="btnutilshow_Click"/></td>
                                        <td class="Lblall" align="center"  style="width:100px;">OrderNo</td>
                                        <td align="center"><asp:TextBox ID="txtordersearch" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox></td>
                                        <td align="center"><asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="MenuFont" OnClick="btnsearch_Click"/></td>
                                        <%--<td class="Lblall" align="center"  style="width:100px;">UserName</td>
                                        <td>
                                            <asp:DropDownList ID="ddlusername" runat="server" CssClass="txtuser" Width="100px" OnSelectedIndexChanged="ddlusername_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td> --%>
                                        <td align="center"><asp:Button ID="btnpostaudit" runat="server" Text="Post Audit" CssClass="MenuFont" /></td>
                                        <td align="center"><asp:Button ID="btnabstract" runat="server" Text="View ParcelID" CssClass="MenuFont" /></td>
                                    </tr>  
                                  </table>
                              </asp:Panel> 
                            <asp:Panel ID="PanelGrid" runat="server" Height="900px" ScrollBars="auto" Width="1150px"  align="center">
                                 <asp:GridView ID="GridUser" runat="server" AutoGenerateColumns="false" DataKeyNames="id" Width="4500px" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowCommand="GridUser_RowCommand" OnRowDataBound="GridUser_RowDataBound">
                                      <Columns>
                                          <asp:TemplateField HeaderText="SNo#">
                                          <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                          </asp:TemplateField>                                                                                    
                                          <asp:TemplateField HeaderText="Order No">
                                          <ItemTemplate>
                                            <asp:Image ID="Imglocked" runat="server" ImageUrl="~/App_themes/Black/images/file-locked.ico" />
                                            <asp:LinkButton ID="Lnkorder" runat="server" Text='<%# Eval("Order No") %>' CommandName="Process" ></asp:LinkButton>
                                          </ItemTemplate>
                                          </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="Lnkdate" runat="server" Text='<%# Eval("Date") %>' CommandName="DateProcess" ></asp:LinkButton>
                                            </ItemTemplate>
                                          </asp:TemplateField>
                                          <asp:BoundField DataField="Download Time" HeaderText="Download Time" />
                                          <asp:BoundField DataField="AssignedDate" HeaderText="Assigned Date" />
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
                                    <asp:GridView ID="GridUserUtilization" runat="server" AutoGenerateColumns="true" Width="2500px" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                                    </asp:GridView>                                            
                                  </asp:Panel>  
                        </td>
                     </tr>                                              
                     <tr><td colspan="5" align="center"><asp:Label ID="errorlabel" runat="server" Text="" Font-Names="Verdana" ForeColor="Red"></asp:Label></td></tr>
                  </table>
              </td>
          </tr>
       </table>  
       <div class="page_dimmer" id="pagedimmer" runat="server"></div>
        <div class="Logout_msgbx1" id="commentsdetails" runat="server" align="center">
            <table border="0" width="800px" >
                <tr>
                    <td align="center">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="800px">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                               <td align="center" style="height: 25px"> Status Comments</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txtstatecomments" runat="server" TextMode="MultiLine" Height="250px" CssClass="txtuser1"
                                            Width="800px" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="center" ><asp:Button ID="btnclose" runat="server" Width="150px" Text="Close" CssClass="MenuFont" OnClick="btnclose_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>             
        </div>   
</asp:Content>