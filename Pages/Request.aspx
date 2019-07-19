<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Request.aspx.cs" Inherits="Pages_Request" Theme="Black" Title="Request Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link rel="shortcut icon" type="image/ico" href="../App_themes/Black/images/Firefox(1).ico" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="Tblmain">
            <div id="logo" style="float: left; width: 230px; height: 85px;">
            </div>
            <div id="templatemo_header" style="float: left;">
                <table border="0" style="width: 900px; height: 90px;">
                    <tr>
                        <td align="center" valign="top" style="padding-top: 30px;">
                            <asp:Image ID="Imgtitle" runat="server" ImageUrl="~/App_themes/Black/images/t1.png" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="templatemo_menu_wrapper">
            <div id="templatemo_menu">
                <table cellpadding="5px">
                    <tr>
                        <td style="font-family: Calibri; font-size: 15px; color: White; width: 300px;"
                            valign="bottom">
                        <asp:Label ID="Lblusername" runat="server"></asp:Label></td>                        
                        <td style="font-family: Calibri; font-size: 15px; color: White; width: 883px;"
                            align="right">
                            <asp:Label ID="Lbltime" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
            </div>
            <!-- end of menu -->
        </div>
        <!-- end of menu wrapper -->
        <div>
            <table id="MainTable" style="height: 500px; background-color: #f5f5f5;" width="1360px" border="0">
                <tr>
                    <td align="center">
                        <asp:Panel ID="Panel1" runat="server" Height="500px" Width="1200px" ScrollBars="Both">
                        <asp:GridView ID="GridRequest" runat="server" AutoGenerateColumns="false" Width="2000px" DataKeyNames="id" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowCommand="GridRequest_RowCommand" OnRowDataBound="GridRequest_RowDataBound">                                      
                           <emptydatarowstyle backcolor="LightBlue" forecolor="Red" Font-Names="Georgia"/>
                           <emptydatatemplate><asp:image id="NoDataImage" runat="server" />No Data Found.</emptydatatemplate>                         
                           <Columns>
                               <asp:TemplateField HeaderText="SNo#">
                                  <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                               </asp:TemplateField>                                                                                    
                               <asp:BoundField DataField="Order_no" HeaderText="Order No." />
                               <asp:BoundField DataField="pDate" HeaderText="Created Date" />
                               <asp:BoundField DataField="Mailaway_date" HeaderText="Mailaway Date" />
                               <asp:BoundField DataField="Req_Type" HeaderText="Request Type" />
                               <asp:BoundField DataField="Cheque_payable" HeaderText="Cheque Payable" />
                               <asp:BoundField DataField="Address" HeaderText="Address" />
                               <asp:BoundField DataField="Borrowername" HeaderText="Borrower Name" />
                               <asp:BoundField DataField="BorrowerAddress" HeaderText="Street" />
                               <asp:BoundField DataField="City" HeaderText="City" />
                               <asp:BoundField DataField="ParcelId" HeaderText="Parcel Id" />
                               <asp:BoundField DataField="Amount" HeaderText="Chanrges" />
                               <asp:BoundField DataField="TaxType" HeaderText="Tax Type" /> 
                               <asp:BoundField DataField="Status" HeaderText="Status" /> 
                               <asp:TemplateField>
                                    <ItemTemplate><asp:LinkButton id="Lnkstatus" CommandName="Received" runat="server" Text="Received"></asp:LinkButton></ItemTemplate>
                               </asp:TemplateField>
                               
                           </Columns>
                       </asp:GridView>
                       </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>        
    </form>
</body>
</html>
