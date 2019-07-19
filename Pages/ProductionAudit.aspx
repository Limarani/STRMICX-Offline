<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductionAudit.aspx.cs" Inherits="Pages_Production"
    Title="PRODUCTION" Theme="Black" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link rel="shortcut icon" type="image/ico" href="../App_themes/Black/images/Firefox(1).ico" />
</head>
<body> 
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager> 
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
                        <td style="font-family:Calibri; font-size: 15px; color: White; width: 300px;"
                            valign="bottom">
                            <asp:Label ID="Lblusername" runat="server"></asp:Label></td>
                        <td>
                            <asp:Button ID="LogoutBtn" runat="server" CssClass="MenuFont" Text="Logout" Font-Overline="false"
                                OnClick="LogoutBtn_Click"></asp:Button></td>
                        <td style="font-family: Calibri; font-size: 15px; color: White; width: 883px;"
                            align="right">
                            <asp:Label ID="Lbltime" runat="server" Text="" Visible="false"></asp:Label></td>
                    </tr>
                </table>
            </div>
            <!-- end of menu -->
        </div>
        <!-- end of menu wrapper -->
        <div>
            <table style="width:100%;background-color:#f1f1f1;border-bottom:outset 1px gray;">
                <tr>
                    <td align="center" valign="top" style="width:90%" >
                        <asp:GridView ID="Gridutilization" runat="server" Width="600px" GridLines="None" CssClass="Gnowrap"></asp:GridView>
                    </td>
                    <td style="width:15%;font-size:18px;font-weight:bold;color:Black;" align="center">
                        <asp:Label ID="lblbreaktotal" runat="server" Text="Break Time "></asp:Label>
                        <asp:Label ID="lblbreak" runat="server" Text="00:00:00"></asp:Label>
                    </td>
                </tr>
            </table>  
        </div>
        <div>
            <table id="MainTable" style="height: 500px; background-color: #f5f5f5;" width="1327px"
                class="tblproduction">
                <tr>
                    <td align="center">
                        <asp:Panel ID="PanelOrderallotment" runat="server">
                            <table style="background-color: #f1f1f1" width="1150" cellpadding="5" border="0" cellspacing="10" class="Tblprod">
                                <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                    <td colspan="7" align="center" style="height: 25px">
                                        <asp:Label ID="Lblprocessname" runat="server" Font-Size="Large" Text="PRODUCTION"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td rowspan="11" valign="top">
                                        <asp:Panel ID="Panel1" runat="server" Width="100%" BackColor="#f1f0ef" ScrollBars="Auto">
                                            <asp:GridView ID="Gridnextorder" runat="server" GridLines="None" CssClass="Gnowrap"></asp:GridView>
                                        </asp:Panel>
                                    </td>
                                    <td class="Lblothers" style="height: 35px;">
                                        Order No</td>
                                    <td>
                                        :</td>
                                    <td style="width: 247px">
                                        <asp:Label ID="Lblorderno" runat="server" Text="1234567" CssClass="lblorderno"></asp:Label></td>
                                    <td class="Lblothers" style="width: 50px;">
                                        Date</td>
                                    <td style="width: 5px;">
                                        :</td>
                                    <td style="width: 100px">
                                        <asp:Label ID="Lbldate" runat="server" Text="05-Dec-2012" CssClass="Lbldata"></asp:Label></td>
                                    
                                </tr>
                                <tr>
                                    <td class="Lblothers" style="height: 35px;">
                                        ProcessTime</td>
                                    <td>
                                        :</td>
                                    <td class="Lblothers" style="width: 247px">

                                        <script type="text/javascript" language="javascript" src="../Script/TimerClock.js"></script>

                                    </td>
                                    <td colspan="2" class="Lblothers">
                                        History
                                    </td>
                                    <td align="right"><asp:Label ID="lblkeycomplete" runat="server" Text="" Font-Size="18px" Font-Names="Georgia" ForeColor="blueViolet" ></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">
                                        State with TZ</td>
                                    <td>
                                        :</td>
                                    <td style="width: 247px">
                                        <asp:Label ID="Lblstate" runat="server" Text="FL - EST" CssClass="Lbldata"></asp:Label></td>
                                    <td colspan="3" rowspan="5" align="center">
                                        <asp:TextBox ID="txtcommentshistory" runat="server" TextMode="MultiLine" Height="225px" Width="550px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">
                                        County</td>
                                    <td>
                                        :</td>
                                    <td style="width: 247px">
                                        <asp:Label ID="Lblcouny" runat="server" Text="County" CssClass="Lbldata"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">
                                        OrderType</td>
                                    <td>
                                        :</td>
                                    <td style="width: 247px">
                                        <%--<asp:DropDownList ID="ddlordertype" runat="server" Height="20px" Width="200px" CssClass="txtuser"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlordertype_SelectedIndexChanged">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Website</asp:ListItem>
                                            <asp:ListItem>Phone</asp:ListItem>
                                            <asp:ListItem>Mailaway</asp:ListItem>
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtordertype" runat="server" Height="20px" Width="200px" CssClass="txtuser" OnTextChanged="txtordertype_TextChanged"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers" style="height: 41px">
                                        Borrower</td>
                                    <td style="height: 41px">
                                        :</td>
                                    <td style="height: 41px; width: 247px;">
                                        <asp:TextBox ID="txtBorrower" runat="server" Height="20px" Width="200px" CssClass="txtuser"></asp:TextBox>
                                        <%--<cc1:FilteredTextBoxExtender ID="txtfilter" runat="server" FilterType="UppercaseLetters" TargetControlID="txtBorrower"></cc1:FilteredTextBoxExtender>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">
                                        Township</td>
                                    <td>
                                        :</td>
                                    <td style="width: 247px">
                                        <asp:TextBox ID="txttownship" runat="server" Height="20px" Width="200px" CssClass="txtuser"></asp:TextBox></td>
                                    
                                </tr>
                                <tr>
                                    <td class="Lblothers">References</td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlreferences" runat="server" Height="20px" Width="200px" CssClass="txtuser" OnSelectedIndexChanged="ddlreferences_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="3" rowspan="3" align="left">
                                        <asp:TextBox ID="txtexcomments" runat="server" TextMode="MultiLine" Height="100px" Width="400px" ReadOnly="True" ForeColor="Red" Font-Size="20px" Font-Names="Times New Roman"></asp:TextBox>
                                        <asp:CheckBox ID="chkclientcmt" runat="server" Text="Client Comments" Font-Names="Times New Roman" Font-Size="18px" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="Lblothers">Zip Code</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtzipcode" runat="server" Height="20px" Width="200px" CssClass="txtuser"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">
                                        Status</td>
                                    <td>
                                        :</td>
                                    <td style="width: 247px">
                                        <asp:DropDownList ID="ddlstatus" runat="server" Height="20px" Width="120px" CssClass="txtuser"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:LinkButton ID="Lnkcomments" runat="server" Text="GetComments" ></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">
                                        Email Type</td>
                                    <td>
                                        :</td>
                                    <td style="width: 247px">
                                        <asp:DropDownList ID="ddlemailtype" runat="server" Height="20px" Width="200px" CssClass="txtuser"
                                            AutoPostBack="true">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>County</asp:ListItem>
                                            <asp:ListItem>Escrow</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="Lblothers">
                                        County Name</td>
                                    <td>
                                        :</td>
                                    <td style="width: 247px">
                                        <asp:TextBox ID="txtcountyname" runat="server" Height="20px" Width="100px" CssClass="txtuser"></asp:TextBox>
                                        <asp:Button ID="btnsendmail" runat="server" Text="Send Email" CssClass="MenuFont" OnClick="btnsendmail_Click" />
                                        <asp:Button ID="btnstatecomments" runat="server" Text="State Comments" CssClass="MenuFont" OnClick="btnstatecomments_Click" />
                                    </td>
                                        
                                </tr>
                                <tr>
                                    <td colspan="7" style="widows:100%;">
                                        <asp:Panel ID="PanelQc" runat="server" BackColor="#f1f0ef" BorderColor="Gray" BorderStyle="Dashed" BorderWidth="1px" Width="100%">
                                            <table style="height: 50px;width:100%;">
                                                <tr>
                                                    <td class="Lblothers"  style="width:75px;">
                                                        Error :</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlerror" CssClass="txtuser" runat="server" AutoPostBack="true" Width="85px" OnSelectedIndexChanged="ddlerror_SelectedIndexChanged" >
                                                            <asp:ListItem>No Error</asp:ListItem>
                                                            <asp:ListItem>Error</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    
                                                    <td class="Lblothers" style="width:200px;">
                                                        Error Category :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlerrorcat" CssClass="txtuser" runat="server" Width="180px" OnSelectedIndexChanged="ddlerrorcat_SelectedIndexChanged" AutoPostBack="true" >
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="Lblothers" style="width:150px;">
                                                        Error Area :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlerrorarea" CssClass="txtuser" runat="server" Width="180px" OnSelectedIndexChanged="ddlerrorarea_SelectedIndexChanged" AutoPostBack="true" >
                                                        </asp:DropDownList>
                                                    </td>
                                                    
                                                                    
                                                    <%--<td style="width: 4px">
                                                        <asp:Panel ID="Paneliferror" runat="server" Width="100%" >
                                                            <table>
                                                                <tr>
                                                                    <td class="Lblothers">
                                                                        ErrorField :
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlerrorfield" CssClass="txtuser" runat="server" Width="180px" >
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="Lblothers">
                                                                        Incorrect :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtincorrect" runat="server" CssClass="txtuser" Width="180px"></asp:TextBox></td>
                                                                    <td class="Lblothers">
                                                                        Correct :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtcorrect" runat="server" CssClass="txtuser" Width="180px"></asp:TextBox></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>--%>
                                                </tr>
                                                <tr style="height:10px;"></tr>
                                                <tr>
                                                    <td class="Lblothers" style="width:135px;">
                                                        Error Type :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlerrortype" CssClass="txtuser" runat="server" Width="180px" OnSelectedIndexChanged="ddlerrortype_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="Lblothers" style="width:125px;">
                                                        Combined :
                                                    </td>
                                                    <td colspan="3" align="left">
                                                        <asp:DropDownList ID="ddlcombined" CssClass="txtuser" runat="server" Width="500px" >
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnadderror" runat="server" Text="Add Error" CssClass="MenuFont" OnClick="btnadderror_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7" style="widows:100%;">
                                        <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Auto">
                                            <asp:GridView ID="Griderrors" runat="server" GridLines="None" AutoGenerateColumns="false" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" Width="100%" OnRowDeleting="Griderrors_RowDeleting" OnRowCommand="Griderrors_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sno.">                        
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>                        
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Order_No" HeaderText="Order No" />
                                                    <asp:BoundField DataField="Pdate" HeaderText="Date" />
                                                    <asp:BoundField DataField="K1_OP" HeaderText="Key Person" />
                                                    <asp:BoundField DataField="QC_OP" HeaderText="QC Person" />
                                                    <asp:BoundField DataField="Error" HeaderText="Error" />
                                                    <asp:BoundField DataField="Error_Category" HeaderText="Error Category" />
                                                    <asp:BoundField DataField="Error_Area" HeaderText="Error Area" />
                                                    <asp:BoundField DataField="Error_Type" HeaderText="Error Type" />
                                                    <asp:TemplateField HeaderText="Combined">     
                                                        <ItemTemplate>
                                                            <asp:Label ID="GridLblerror" runat="server" Text='<%# Eval("Combined") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Combined" HeaderText="Combined" />--%>
                                                    
                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>                                
                                                            <asp:ImageButton ID="lnkbtndelete" runat="server" CommandName="Delete" ImageUrl="~/App_themes/Black/images/imagesde.png" Height="20px" Width="20px" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?')"/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td colspan="7" align="center" valign="middle">
                                        <asp:Panel ID="PanelLink" runat="server" BackColor="#f5f5f5" BorderColor="Gray" BorderStyle="Dashed" BorderWidth="1px" Width="100%">
                                            <table style="height: 50px;width:100%;">
                                                <tr>
                                                    <td class="Lblothers">Assessor </td>
                                                    <td class="Lblothers">:</td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtassessor" runat="server" CssClass="txtuser" Height="20px" Width="600px"></asp:TextBox>
                                                    </td>
                                                    <td class="Lblothers">Assessor# </td>
                                                    <td class="Lblothers">:</td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtassphone" runat="server" CssClass="txtuser" Height="20px" Width="200px"></asp:TextBox>
                                                    </td>
                                                    <td rowspan="4" align="center" valign="middle">
                                                        <asp:Button ID="btnlinksave" runat="server" Text="Save" CssClass="MenuFont" OnClick="btnlinksave_Click" />
                                                        <br />
                                                        <asp:Button ID="btnlinkupdate" runat="server" Text="Update" CssClass="MenuFont" OnClick="btnlinkupdate_Click" />
                                                        <asp:CheckBox ID="chksavelink" runat="server" Text="Save Link" />
                                                    </td>
                                                </tr>
                                                <tr style="height:10px;"></tr>
                                                <tr>
                                                    <td class="Lblothers">Treasurer </td>
                                                    <td class="Lblothers">:</td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtTreasurer" runat="server" CssClass="txtuser" Height="20px" Width="600px"></asp:TextBox>
                                                    </td>
                                                    <td class="Lblothers">TaxCollector# </td>
                                                    <td class="Lblothers">:</td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtTreasphone" runat="server" CssClass="txtuser" Height="20px" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="height:10px;"></tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="Lblothers" style="height: 35px;" colspan="2">
                                        <asp:Label ID="lblqcerrorcmts" runat="server" Text="Error Comments  :"></asp:Label></td>
                                     <td colspan="5" align="left"><asp:TextBox ID="txtqcerrorcmts" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox></td>
                                </tr>
                                
                                <tr>
                                    <td class="Lblothers" style="height: 35px;">
                                        Comments</td>
                                    <td>
                                        :</td>
                                    <td colspan="5" align="left">
                                        <%--<asp:TextBox ID="txtcomments" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox>--%>
                                        <asp:Label ID="lblprdcomments" runat="server" Text="" Width="800px" Height="20px" Font-Size="Large"></asp:Label>
                                        <asp:DropDownList ID="ddlprdcomments" CssClass="txtuser" runat="server" Width="600px" Height="20px" OnSelectedIndexChanged="ddlprdcomments_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList><asp:CheckBox ID="chktaxinfo" runat="server" Text="Send Tax Mail" OnCheckedChanged="chktaxinfo_CheckedChanged" AutoPostBack="true" />
                                        <span style="color: Red;">*</span>
                                        <asp:TextBox ID="txtaddcomments" runat="server" CssClass="txtuser" Height="20px" Width="600px"></asp:TextBox>
                                        <asp:Button ID="btnaddcomments" runat="server" Text="Add" CssClass="MenuFont" OnClick="btnaddcomments_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7" align="center" style="height: 35px;">
                                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="MenuFont" OnClick="btnsave_Click" />
                                        <asp:Button ID="Btnmoveqc" runat="server" Text="MoveQC" CssClass="MenuFont" OnClick="Btnmoveqc_Click" />
                                        <asp:Button ID="btnMovecall" runat="server" Text="Move To Call" Width="120px" CssClass="MenuFont" OnClick="btnMovecall_Click" />
                                        <asp:Button ID="btnrequest" runat="server" Text="View" CssClass="MenuFont" OnClick="btnrequest_Click" /> 
                                        <asp:Button ID="btngetcomments" runat="server" Text="Get Comments" Width="120px" CssClass="MenuFont" OnClick="btngetcomments_Click" />    
                                        <asp:Button ID="btnreferences" runat="server" Text="Get Reference" CssClass="MenuFont" OnClick="btnreferences_Click" />  
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7" align="center" class="LiteralError" style="padding-left: 20px;">
                                        <asp:Label ID="Lblerror" runat="server" ForeColor="red" Font-Size="14px"></asp:Label></td>
                                </tr>
                            </table>
                        </asp:Panel>                                                                                                                  
                        <asp:Panel ID="PanelStatus" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblinfo" runat="server" ForeColor="red" Font-Size="25px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div id="templatemo_footer_wrapper">
            <div id="templatemo_footer">
                Copy Right © String 2012. All Rights Reserved.Powered By : SST
            </div>
            <!-- end of footer -->
        </div>
        
        <div class="page_dimmer" id="pagedimmer" runat="server"></div>
        
        <div class="Logout_msgbx" id="ReportPanel" runat="server" align="center">
            <table border="0" width="800px" height="550px">
            <tr>
            <td align="center">
                <table border="0" cellpadding="3px" cellspacing="4px" width="500px">
                <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                   <td colspan="4" align="center" style="height: 25px">
                   <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Mail Away"></asp:Label></td>
                </tr>
                <tr>
                   <td class="Lblothers">Date</td>                   
                   <td>
                   <asp:TextBox ID="txtdate" runat="server" CssClass="txtuser" Width="85px"></asp:TextBox>
                   <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdate" Format="MM/dd/yyyy">
                   </cc1:CalendarExtender>
                   </td>                   
                </tr>
                
                <tr>
                   <td class="Lblothers">Cheque Payable</td>                   
                   <td>
                    <asp:TextBox ID="txtchqpay" runat="server" CssClass="txtuser" Width="200px"></asp:TextBox>
                    <asp:Button ID="btngetaddress" runat="server" Width="100px" Text="Get Address" CssClass="MenuFont" OnClick="btngetaddress_Click" />
                   </td>                                       
                </tr>
                
                <tr>
                   <td class="Lblothers">Request Type</td>                   
                   <td>
                   <asp:DropDownList ID="ddlrequesttype" runat="server" Width="200px" Height="20px" CssClass="txtuser">
                   <asp:ListItem></asp:ListItem>
                   <asp:ListItem Value="1">UPS</asp:ListItem>
                   <asp:ListItem Value="2">UPS/R</asp:ListItem>
                   <asp:ListItem Value="3">UPS/SASE</asp:ListItem>
                   <asp:ListItem Value="4">REGULAR</asp:ListItem>
                   <asp:ListItem Value="5">THANKS REQUEST</asp:ListItem>
                   </asp:DropDownList>
                   </td>                     
                </tr>
                
                <tr>
                    <td class="Lblothers">To Address</td>                      
                    <td><asp:TextBox ID="txtaddress" runat="server" CssClass="txtuser" Width="300px"  Height="100px" TextMode="MultiLine"></asp:TextBox></td>   
                </tr>
                
                <tr>                   
                   <td class="Lblothers">Borrower Name</td>                   
                   <td><asp:TextBox ID="txtbrrname" runat="server" CssClass="txtuser" Width="300px"></asp:TextBox></td>                       
                </tr>
                
                <tr>
                    <td class="Lblothers">Street</td>                                        
                    <td><asp:TextBox ID="txtbrraddress" runat="server" CssClass="txtuser" Width="300px" Height="50px" TextMode="MultiLine"></asp:TextBox></td>                                                             
                </tr>
                
                <tr>
                    <td class="Lblothers">City</td>                    
                    <td><asp:TextBox ID="txtcity" runat="server" CssClass="txtuser" Width="300px" Height="50px" TextMode="MultiLine"></asp:TextBox></td>                                          
                </tr>              
                                
                <tr>
                   <td class="Lblothers">ParcelID</td>                   
                   <td><asp:TextBox ID="txtparcelid" runat="server" CssClass="txtuser" Width="300px"></asp:TextBox></td>                                      
                </tr>   
                
                <tr>
                   <td class="Lblothers">Charges</td>                   
                   <td><asp:TextBox ID="txtamount" runat="server" CssClass="txtuser" Width="300px"></asp:TextBox></td>                   
                </tr>
                
                <tr>                   
                   <td class="Lblothers">Tax Type</td>                   
                   <td><asp:TextBox ID="txttaxtype" runat="server" CssClass="txtuser" Width="300px" TextMode="MultiLine"></asp:TextBox>
                    <%--<asp:DropDownList ID="ddltaxtype" runat="server" Height="20px" Width="200px" CssClass="txtuser">
                    </asp:DropDownList>--%>
                   </td>                   
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                   <td align="center" colspan="4"><asp:Button ID="btncreatetreq" runat="server" Width="150px" Text="Create Request" CssClass="MenuFont" OnClick="btncreatetreq_Click" />
                   <asp:Button ID="btncancel" runat="server" Width="100px" Text="Cancel" CssClass="MenuFont" OnClick="btncancel_Click" />
                   </td>
                </tr>
                <tr>
                    <td align="center" colspan="2"><asp:Label ID="Lblsuccess" runat="server" ForeColor="red" Font-Size="14px" Text="Error"></asp:Label></td>
                </tr>
             </table>    
            </td>
            </tr>
            </table>             
        </div>
        
        
        <div class="page_dimmer" id="pagedimmer1" runat="server"></div>
        <div class="Logout_msgbx1" id="statecomments" runat="server" align="center">
            <table border="0" width="800px" height="200px">
                <tr>
                    <td align="center">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="800px">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                               <td align="center" style="height: 25px"> State Comments</td>
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
        
        <div class="Logout_msgbx2" id="getcomments" runat="server" align="center">
            <table border="0" width="100%px" height="800px">
                <tr>
                    <td align="center" valign="top">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                               <td align="center" style="height: 25px"> Web Tool Installment Comments</td>
                            </tr>
                            <tr>
                                <td align="center"><asp:TextBox ID="txtgetcomments" runat="server" TextMode="MultiLine" Height="400px" CssClass="txtuser1" Width="800px" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="center" ><asp:Button ID="btngetclose" runat="server" Width="150px" Text="Close" CssClass="MenuFont" OnClick="btngetclose_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>             
        </div>
        
        <div class="Logout_checklist" id="LogoutReason" runat="server" align="center">
            <table border="0" width="400px" height="200px">
                <tr>
                    <td align="center" valign="top">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                               <td align="center" style="height: 25px" colspan="2"> Do you want to Logout?</td>
                            </tr>
                            <tr style="height:25px;"></tr>
                            <tr>
                                <td class="Lblothers" align="right">Logout Reason  :</td>
                                <td align="left">
                                    <asp:DropDownList ID="ddllogout" runat="server" Height="20px" Width="200px" CssClass="txtuser" OnSelectedIndexChanged="ddllogout_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:TextBox ID="txtlogreason" runat="server" CssClass="txtuser1" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height:25px;"></tr>
                            <tr>
                                <td align="center" colspan="2" >
                                    <asp:Button ID="btnok" runat="server" Width="150px" Text="Ok" CssClass="MenuFont" OnClick="btnok_Click" />
                                    <asp:Button ID="btnlogoutclose" runat="server" Width="150px" Text="Cancel" CssClass="MenuFont" OnClick="btnlogoutclose_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>             
        </div>
        
        <div class="Taxinfo_msgbx" id="Taxinfomail" runat="server" align="center">
            <asp:Panel ID="Paneltaxinfo" runat="server" BackColor="#f5f5f5" Width="700px">
                <table border="0" width="700px">
                    <tr>
                        <td align="center" valign="top">
                            <table border="0" width="700px">
                                <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                   <td align="center" style="height: 25px" colspan="3">Tax Information</td>
                                </tr>
                                <tr style="height:20px;"></tr>
                                <tr>
                                    <td class="Lblothers">To id </td>
                                    <td class="Lblothers">:</td>
                                    <td><asp:TextBox ID="txttaxtoid" runat="server" CssClass="txtuser" Width="300px"></asp:TextBox></td>
                                </tr>
                                <tr>                   
                                    <td class="Lblothers">Tax Type </td>
                                    <td class="Lblothers">:</td>
                                    <td><asp:DropDownList ID="ddltaxtype1" runat="server" Height="20px" Width="300px" CssClass="txtuser"></asp:DropDownList></td>                   
                                </tr>
                                <tr>
                                    <td class="Lblothers">Address </td>                      
                                    <td class="Lblothers">:</td>
                                    <td><asp:TextBox ID="txttaxadd" runat="server" CssClass="txtuser" Width="300px"  Height="100px" TextMode="MultiLine"></asp:TextBox></td>   
                                </tr>
                                <tr>
                                    <td class="Lblothers">Parcel No </td>
                                    <td class="Lblothers">:</td>
                                    <td><asp:TextBox ID="txtparcelno" runat="server" CssClass="txtuser" Width="300px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center"><asp:Label ID="lbltaxerror" runat="server" ForeColor="red" Font-Size="Large"></asp:Label></td>
                                </tr>
                                <tr style="height:20px;"></tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Button ID="btnsendtaxmail" runat="server" Width="100px" Text="Send Mail" CssClass="MenuFont" OnClick="btnsendtaxmail_Click" />
                                        <asp:Button ID="btntaxcancel" runat="server" Width="100px" Text="Cancel" CssClass="MenuFont" OnClick="btntaxcancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        
        <input type="hidden" id="theInput" value="<%=Session["TimePro"]%>" />
        
        <asp:HiddenField ID="Hlogout" runat="server" />
        
        <script type="text/javascript" language="javascript">
            show_TickerTime();
        </script>
        
        <script type="text/javascript" language="javascript">
         
            function openNewWin(url) 
            {
                    var x = window.open(url, 'mynewwin', 'width=600,height=600,toolbar=1');
                    x.focus();
            }                                       
            
            function disp_prompt()
            {
                var fname=prompt("Please Enter the Logout Reason:","");
                document.getElementById("<%=Hlogout.ClientID %>").value= fname;
            }
                   
        </script>
        

    </form>
</body>
</html>
