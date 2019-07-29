<%@ Page Language="C#" MasterPageFile="~/Master/STRMICX-Offline.master" AutoEventWireup="true" CodeFile="STRMICXOrderStatus.aspx.cs" Inherits="Pages_STRMICXOrderStatus" %>

<%--<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true" CodeFile="OrderStatus.aspx.cs" Inherits="Pages_OrderStatus" Theme="Black" Title="ORDER STATUS" %>--%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <script type="text/javascript" src="../Script/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="../Script/ScrollableGridPlugin.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>--%>

    <%--<link href="../Script/Bootstrap.min.css" rel="stylesheet" />
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css" />
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="../Script/Jquery-1.8.3-jquery.min.js"></script>--%>

    <link href='/path/to/font-awesome.css' rel='stylesheet'/>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            $('#<%=GridUser.ClientID %>').Scrollable({
                ScrollHeight: 800
            });
            //    $('#<%=GridUserUtilization.ClientID %>').Scrollable({
            //        ScrollHeight: 800
            //    });
        });
    </script>--%>
    <script type="text/javascript">

        function checkAll(objRef) {

            var GridView = objRef.parentNode.parentNode.parentNode;

            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {

                //Get the Cell To find out ColumnIndex

                var row = inputList[i].parentNode.parentNode;

                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                    if (objRef.checked) {

                        inputList[i].checked = true;
                    }

                    else {

                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function Assign() {
            $('[id*=ModalManualAssign]').modal('show');
        }
    </script>

    <style type="text/css">
        .check {
            text-align: center;
        }
    </style>

    <div class="sign-up-row widget-shadow" style="width: 100%;" align="center">

        <div class="panel panel-default" style="margin-bottom: 0px;">

            <div id="collapse2" class="panel-collapse collapse in">
                <div class="panel-body" style="background-color: #e6dddd2b; padding-bottom: 5px; padding-left: 5px; padding-right: 5px; padding-top: 5px;">
                    <div class="tab-content">
                        <div class="tab-pane fade in active" id="tab1primary">
                            <table class="Table1" border="0" width="100%" style="height: 100%;">
                                <tr>
                                    <td align="center" style="height: 100%; width: 100%;">
                                        <asp:Panel ID="PanelOrderList" runat="server" align="center" Width="100%">
                                            <table cellspacing="5" cellpadding="5" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="Chkrefresh" Style="font-family: Calibri;" runat="server" CssClass="Autorefresh" OnCheckedChanged="Chkrefresh_CheckedChanged" AutoPostBack="true" Text="Auto Refresh" Visible="false" />
                                                        <asp:Timer ID="Refresh" runat="server" Interval="120000" Enabled="false" OnTick="Refresh_Tick"></asp:Timer>
                                                    </td>
                                                    <%--<td>
                                                        <asp:ImageButton ID="Lnkexport" runat="server" ImageUrl="~/App_themes/Black/images/Excel.png" Height="50px" Width="50px" ToolTip="Export" OnClick="Lnkexport_Click" />
                                                    </td>--%>
                                                    <td class="Lblall" align="left" style="font-weight: bold; width: 59px;">From &nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                    <td style="width: 141px;">
                                                        <asp:TextBox ID="txtfrmdate" runat="server" CssClass="txtuser form-control" Width="110px" autocomplete="off"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfrmdate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                                    </td>
                                                    <td class="Lblall" style="font-weight: bold; width: 39px;">To &nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                    <td style="width: 136px;">
                                                        <asp:TextBox ID="txttodate" runat="server" CssClass="txtuser form-control" Width="110px" autocomplete="off"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                                    </td>

                                                    <td class="Lblall" style="width: 96px; font-weight: bold;">Order Type
                                                    </td>
                                                    <td style="width: 207px;">
                                                        <asp:DropDownList runat="server" ID="Ordertypelist" CausesValidation="false" CssClass="form-control" Style="width: 186px;">
                                                            <asp:ListItem>Select OrderType</asp:ListItem>
                                                            <asp:ListItem>Refinance</asp:ListItem>
                                                            <asp:ListItem>Purchase</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <%--<asp:TextBox ID="txtordersearch" runat="server" CssClass="txtuser form-control" Width="112px"></asp:TextBox></td>--%>

                                                    <td class="Lblall" style="width: 99px; font-weight: bold;">State
                                                    </td>
                                                    <td style="width: 127px;">
                                                        <asp:DropDownList runat="server" ID="Statelist" CausesValidation="false" CssClass="form-control" Style="width: 153px; margin-left: -44px;">
                                                            <asp:ListItem>Select State</asp:ListItem>
                                                            <asp:ListItem>CA</asp:ListItem>
                                                            <asp:ListItem>LM</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td class="Lblall" style="width: 91px; font-weight: bold;">User Name
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="Usernamelist" CausesValidation="false" CssClass="form-control" Style="width: 195px;">
                                                            <asp:ListItem>Select Username</asp:ListItem>
                                                            <asp:ListItem>Rasheed</asp:ListItem>
                                                            <asp:ListItem>Baskar</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnordershow" runat="server" Text="Go" class="btn btn-success" Enabled="True" CausesValidation="false" OnClick="btnordershow_Click" />
                                                    </td>
                                                    <%-- <td align="center">
                                                        <asp:Button ID="btnutilshow" runat="server" Text="Show Utilization" Width="125px" class="btn btn-success" OnClick="btnutilshow_Click" />
                                                    </td>--%>

                                                    <%--<asp:Button ID="btnsearch" runat="server" Text="Search" class="btn btn-success" OnClick="btnsearch_Click" /></td>--%>
                                                    <%--<td class="Lblall" align="center"  style="width:100px;">UserName</td>
                                        <td>
                                            <asp:DropDownList ID="ddlusername" runat="server" CssClass="txtuser" Width="100px" OnSelectedIndexChanged="ddlusername_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td> --%>
                                                    <%--<td align="center">
                                                        <asp:Button ID="btnpostaudit" runat="server" Text="Post Audit" class="btn btn-success" Visible="False" /></td>
                                                    <td align="center">
                                                        <asp:Button ID="btnabstract" runat="server" Text="View Parcel ID" class="btn btn-success" /></td>--%>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="center">
                                        <asp:Label ID="errorlabel" runat="server" Text="" Font-Names="Verdana" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <br />
        <table border="0" width="100%" style="background-color: white; font-family: Calibri;">
            <tr>
                <td style="height: 321px;" align="center" valign="top">
                    <asp:Panel runat="server" ID="Counts" Style="margin-top: -10px;">
                        <div class="panel panel-default" style="width: 1294px;">
                            <div id="collapse3" class="panel-collapse collapse in">
                                <div class="panel-body" style="background-color: #e6dddd2b; height: 30px; padding-top: 4px; padding-bottom: 30px; padding-left: 0px; padding-right: 0px;">
                                    <div class="tab-content">
                                        <div class="tab-pane fade in active" id="tab12primary">

                                            <table style="width: 1267px;">
                                                <tbody>
                                                    <tr>
                                                        <td class="Lblall" style="font-weight: bold; width: 59px; white-space: nowrap;">Quick Search:</td>
                                                        <td style="width: 176px;">
                                                            <asp:TextBox runat="server" ID="quicksearch" Style="height: 28px; width: 151px; margin-left: 8px;" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                        <td class="Lblall" style="font-weight: bold; width: 60px; white-space: nowrap;">WIP:</td>
                                                        <td style="width: 14px;">
                                                            <asp:LinkButton ID="wiporders" runat="server" OnClick="wiporders_Click" Style="color: red; font-weight: bold; margin-left: -24px"></asp:LinkButton>
                                                        </td>
                                                        <td class="Lblall" style="font-weight: bold; width: 60px; white-space: nowrap;">YTS:</td>
                                                        <td style="width: 10px;">
                                                            <asp:LinkButton ID="ytsorders" runat="server" OnClick="ytsorders_Click" Style="color: red; font-weight: bold; margin-left: -24px"></asp:LinkButton>
                                                        </td>
                                                        <td class="Lblall" style="font-weight: bold; width: 59px; white-space: nowrap;">Mailaway:</td>
                                                        <td style="width: 40px;">
                                                            <asp:LinkButton ID="mailwayorders" runat="server" OnClick="mailwayorders_Click" Style="color: red; font-weight: bold; margin-left: 6px"></asp:LinkButton>
                                                        </td>
                                                        <td class="Lblall" style="font-weight: bold; width: 59px; white-space: nowrap;">In-Process:</td>
                                                        <td style="width: 40px;">
                                                            <asp:LinkButton ID="inprocessorders" runat="server" OnClick="inprocessorders_Click" Style="color: red; font-weight: bold; margin-left: 6px"></asp:LinkButton>
                                                        </td>
                                                        <td class="Lblall" style="font-weight: bold; width: 59px; white-space: nowrap;">Delivered:</td>
                                                        <td style="width: 50px;">
                                                            <asp:LinkButton ID="deliveredorders" runat="server" OnClick="deliveredorders_Click" Style="color: red; font-weight: bold; margin-left: 6px"></asp:LinkButton>
                                                        </td>
                                                        <td class="Lblall" style="font-weight: bold; width: 59px; white-space: nowrap;">Rejected:</td>
                                                        <td style="width: 42px;">
                                                            <asp:LinkButton ID="rejectedorders" runat="server" OnClick="rejectedorders_Click" Style="color: red; font-weight: bold; margin-left: 6px"></asp:LinkButton>
                                                        </td>
                                                        <td class="Lblall" style="font-weight: bold; width: 72px; white-space: nowrap;">Total Count:</td>
                                                        <td>
                                                            <asp:Label runat="server" ID="totalorders" Style="color: red; font-weight: bold; margin-left: 10px;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PanelGrid" runat="server" Height="453px" ScrollBars="auto" align="center" Style="margin-top: -19px; overflow: auto">

                        <asp:GridView ID="GridUser" ShowHeaderWhenEmpty="true" CssClass="Grid" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
                            Width="1293" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px" HeaderStyle-BorderColor="White"
                            OnRowCommand="GridUser_RowCommand" OnRowDataBound="GridUser_RowDataBound" Style="text-align: center">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="30px" HeaderStyle-CssClass="check" ItemStyle-Width="30px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" TextAlign="Left" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chktrackdetails" runat="server" align="Center" Style="margin-left: 5px;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="30px" ItemStyle-Width="30px" HeaderStyle-CssClass="check">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LnkId" runat="server" Text='<%# Container.DataItemIndex + 1 %>' OnClick="LnkId_Click" autopostback="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="130px" ItemStyle-Width="30px" HeaderStyle-CssClass="check">
                                    <ItemTemplate>
                                       <%-- <asp:Image ID="Imglocked" runat="server" ImageUrl="~/App_themes/Black/images/lockimg.png" />--%>
                                        <asp:LinkButton ID="Lnkorder" runat="server" Text='<%# Eval("Order_No") %>' CommandName="Process"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="check">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Lnkdate" runat="server" Text='<%# Eval("PDate") %>' CommandName="DateProcess"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DownloadTime" HeaderText="In Time" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="UploadTime" HeaderText="Out Date" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="TAT" HeaderText="TAT (Hrs)" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="State" HeaderText="State" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="County" HeaderText="County" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="OrderType" HeaderText="OrderType" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="orderstatus" HeaderText="Status" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="HP" HeaderText="Priority" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="K1_OP" HeaderText="Key OP" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="QC_OP" HeaderText="QC OP" HeaderStyle-CssClass="check" />
                                <asp:BoundField DataField="Comments" HeaderText="Comments" HeaderStyle-CssClass="check" />
                            </Columns>
                            <AlternatingRowStyle BackColor="#f3f2ea" />
                            <HeaderStyle BackColor="#d9241b" ForeColor="white" BorderColor="#d9241b" />
                        </asp:GridView>
                        <asp:GridView ID="GridUserUtilization" runat="server" AutoGenerateColumns="true" Width="2500px" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt">
                        </asp:GridView>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="PanelReset" runat="server" align="center" Style="margin-bottom: 10px;">
                        <button type="button" class="btn btn-default ng-pristine ng-untouched ng-valid ng-empty" data-toggle="modal" data-target="#Assign" title="Assign" id="Assign" runat="server" onserverclick="Assign_Click">
                            <span class="glyphicon glyphicon-user" style="font-size: 20px;"></span>
                            <br />
                            Assign
                        </button>
                        <%--<button type="button" class="btn btn-default" id="btnhold1" data-toggle="modal" data-target="HoldModal" title="Hold" runat="server" onserverclick="Hold_Click">
                            <span class="fa fa-hand-paper-o" style="font-size: 20px;"></span>
                            <br />
                            Hold
                        </button>
                        <button type="button" class="btn btn-default" id="Button1" data-toggle="modal" data-target="HoldModal" title="UnHold" runat="server" onserverclick="UnHold_Click">
                            <span class="fa fa-hand-lizard-o" style="font-size: 20px;"></span>
                            <br />
                            UnHold
                        </button>--%>
                        <button type="button" class="btn btn-default" id="btnhold" data-toggle="modal" data-target="HoldModal" title="Reject" runat="server" onserverclick="Reject_Click">
                            <span class="glyphicon glyphicon-remove" style="font-size: 20px;"></span>
                            <br />
                            Reject
                        </button>
                        <button type="button" class="btn btn-default" data-toggle="modal" data-target="#exampleModal1" title="Delete" runat="server" onserverclick="Delete_Click">
                            <span class="glyphicon glyphicon-trash" style="font-size: 20px"></span>
                            <br />
                            Delete
                        </button>
                        <button type="button" id="btnreject" class="btn btn-default" data-toggle="modal" data-target="#exampleModal2" title="Lock" runat="server" onserverclick="Lock_Click">
                            <span class="glyphicon glyphicon-lock" style="font-size: 20px"></span>
                            <br />
                            Lock
                        </button>
                        <button type="button" id="btnunlock" class="btn btn-default" data-toggle="modal" data-target="#exampleModal2" title="UnLock" runat="server" onserverclick="UnLock_Click">
                            <%--<span class="glyphicon glyphicon-fa-unlock" style="font-size: 20px"></span>--%>
                             <i class="fa fa-unlock" style="font-size: 25px"></i>
                            <br />
                            UnLock
                        </button>
                        <button type="button" class="btn btn-default" data-toggle="modal" data-target="#Accept" title="Priority" runat="server" onserverclick="Priority_Click">
                            <span class="glyphicon glyphicon-exclamation-sign" style="font-size: 20px"></span>
                            <br />
                            Priority
                        </button>
                        <button type="button" class="btn btn-default" data-toggle="modal" data-target="#Accept" title="DePriority" runat="server" onserverclick="DePriority_Click">
                            <span class="glyphicon glyphicon-info-sign" style="font-size: 20px"></span>
                            <br />
                            DePriority
                        </button>
                        <button type="button" class="btn btn-default ng-pristine ng-untouched ng-valid ng-empty" data-toggle="modal" data-target="#ManualAssign" title="YTS" runat="server" onserverclick="YTS_Click">
                            <span class="glyphicon glyphicon-play-circle" style="font-size: 20px;"></span>
                            <br />
                            YTS
                        </button>
                    </asp:Panel>
                </td>
            </tr>
        </table>

        <div class="page_dimmer" id="pagedimmer" runat="server"></div>
        <div class="Logout_checklist" id="LogoutReason" runat="server" align="center" visible="false">
            <table border="0" width="400px" style="height: 200px">
                <tr>
                    <td align="center" valign="top">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                <td align="center" style="height: 25px" colspan="2">Do you want to proceed manual?
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers" align="left">Reason to proceed Manual:
                                </td>
                                <td align="left">
                                    <%-- <asp:DropDownList ID="ddllogout" runat="server" Height="20px" Width="200px" CssClass="txtuser"
                                        OnSelectedIndexChanged="ddllogout_SelectedIndexChanged"
                                        AutoPostBack="false" Visible="False">
                                    </asp:DropDownList>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:TextBox ID="txtlogreason" runat="server" CssClass="txtuser" Width="300px" ValidationGroup="log"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="reqLog" ControlToValidate="txtlogreason" ErrorMessage="*" ForeColor="Red" ValidationGroup="log" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnok" runat="server" Width="150px" Text="Ok"
                                        OnClick="btnok_Click" ValidationGroup="log" class="btn btn-success" />
                                    <asp:Button ID="btnlogoutclose" runat="server" Width="150px" Text="Cancel"
                                        OnClick="btnlogoutclose_Click" class="btn btn-success" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="Logout_msgbx1" id="commentsdetails" runat="server" align="center" style="font-family: Calibri;">
            <table border="0" width="800px">
                <tr>
                    <td align="center">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="800px">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                <td align="center" style="height: 25px">Status Comments</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtstatecomments" runat="server" TextMode="MultiLine" Height="250px" CssClass="txtuser1"
                                        Width="800px" ReadOnly="True" ForeColor="Black"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnclose" runat="server" Width="150px" Text="Close" class="btn btn-success" OnClick="btnclose_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:Panel ID="PanelManualInfo" class="panel panel-default" runat="server" Visible="false">
        <div class="modal fade" id="ModalManualAssign" role="dialog" runat="server">
            <div class="modal-dialog modal-lg">
                <div class="modal-content" style="width: 100%; height: 400px; margin-left: 0px;">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" style="margin-left: 156px; margin-bottom: -27px;">Order Details</h4>
                        <h4 class="modal-title" style="margin-left: 635px;">User Details</h4>
                    </div>
                    <div class="modal-body">
                        <div class="panel-body" style="margin-top: -17px;">
                            <div class="tab-content">
                                <div class="tab-pane fade in active" id="tab1primary151">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="col-md-12">
                                                <asp:GridView ID="gvorderdetails" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                    Width="125%" GridLines="None" Style="white-space: nowrap; overflow: auto;">
                                                    <Columns>
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="Order_No" HeaderText="Order_No" />
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="State" HeaderText="State" />
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="County" HeaderText="County" />
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="Status" HeaderText="Status" />
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="HP" HeaderText="Priority" />
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="Key OP" HeaderText="K1_OP" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                        <asp:TemplateField HeaderText="Select Orders">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkorders" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#f3f2ea" />
                                                    <HeaderStyle BackColor="#d9241b" ForeColor="white" BorderColor="#fff000" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <table>
                                            <tr>
                                                <td style="width: 35%;" align="right">
                                                    <asp:ListBox ID="lstuserdetails" runat="server" Style="width: 200px; height: 250px; margin-left: -74px" CssClass="txtuser"
                                                        Font-Names="Verdana" OnSelectedIndexChanged="lstuserdetails_SelectedIndexChanged"></asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />

                                        <asp:Button ID="btnassign" runat="server" OnClick="btnassign_Click" autopostback="true" Text="Key-Assign" Stype="margin-left: 412px" class="btn btn-success" />
                                        <asp:Button ID="btnqcassign" runat="server" OnClick="btnqcassign_Click" autopostback="true" Text="QC-Assign" Stype="margin-left: 412px" class="btn btn-success" />
                                        <asp:Label ID="lblerror" runat="server"></asp:Label>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="footer" style="text-align: center;">
        <p style="background-color: #337ab7; margin-left: 2px; margin-bottom: 0px; color: white; font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;">
            &copy; 2019. All rights reserved | Designed & Developed by String Information Services
        </p>
    </div>
</asp:Content>
