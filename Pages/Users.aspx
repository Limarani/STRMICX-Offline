<%@ Page Language="C#" MasterPageFile="~/Master/STRMICX-Offline.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Pages_Users" Theme="Black" Title="USER MAINTANACE" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   
    <table width="100%" border="0" style="background-color: white; height: 584px;">
        <tr>
            <td align="center" style="padding-left: 145px">
                <asp:Panel ID="PanelViewuser" runat="server" Width="1100px" Style="height: 517px;">
                    <table border="0" width="100%" style="font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;">
                        <tr>
                            <td align="center" valign="middle" style="width: 100%;">
                                <table width="50%" border="1" style="border: solid 1px gray;">
                                    <tr>
                                        <td style="background-color: #5cb85c; width: 50px;" align="center">
                                            <asp:Label ID="lblonline" runat="server" Text="" Width="100px" Font-Bold="true" Style="font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:LinkButton ID="Lnkonline" runat="server" Text="Online" Font-Names="Georgia" Font-Bold="true" Font-Overline="false" Style="font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;" OnClick="Lnkonline_Click"></asp:LinkButton>
                                        </td>
                                        <td style="background-color: red; width: 50px;" align="center">
                                            <asp:Label ID="lbloffline" runat="server" Width="100px" Font-Bold="true" Style="font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:LinkButton ID="Lnkoffline" runat="server" Text="Offline" Font-Names="Georgia" Font-Bold="true" Font-Overline="false" Style="font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;" OnClick="Lnkoffline_Click"></asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:CheckBox ID="Chkrefresh" runat="server" CssClass="Autorefresh" Style="font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;" OnCheckedChanged="Chkrefresh_CheckedChanged" AutoPostBack="true" Text="Auto Refresh" />
                                            <asp:Timer ID="Refresh" runat="server" Interval="120000" Enabled="false" OnTick="Refresh_Tick"></asp:Timer>
                                        </td>
                                        <td align="center">
                                            <asp:LinkButton ID="Imguser" runat="server" OnClick="Imguser_Click">
                                                <asp:Image ID="Imguser1" runat="server" ImageUrl="~/App_themes/Black/images/user_add.png"
                                                    Height="30px" Width="30px" ToolTip="Create New User" />
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" style="width: 80%;">
                                <asp:Panel ID="Panel1" runat="server" Width="100%" Height="500px" ScrollBars="Auto">
                                    <asp:GridView ID="Griduserdetails" OnRowCommand="Griduserdetails_RowCommand" OnRowEditing="userGrid_RowEditing" OnRowDeleting="userGrid_RowDeleting" runat="server" Width="100%" FooterStyle-CssClass="Gridfooterbar" ShowFooter="false" AutoGenerateColumns="false" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowDataBound="Griduserdetails_RowDataBound" Style="font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Username">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="UserName" runat="server" Text='<%#Eval("User_Name")%>' CommandName="Edit" ForeColor="Navy" OnClick="LnkUserName_Click">


                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="Admin" HeaderText="Admin"></asp:BoundField>
                                            <asp:BoundField DataField="Keying" HeaderText="Production"></asp:BoundField>
                                            <asp:BoundField DataField="QC" HeaderText="QC"></asp:BoundField>
                                            <asp:BoundField DataField="DU" HeaderText="DU"></asp:BoundField>
                                            <asp:BoundField DataField="Review" HeaderText="Post Audit"></asp:BoundField>
                                            <asp:BoundField DataField="Inprocess" HeaderText="Inprocess"></asp:BoundField>
                                            <asp:BoundField DataField="mailaway" HeaderText="Mailaway"></asp:BoundField>
                                            <asp:BoundField DataField="Parcelid" HeaderText="ParcelID"></asp:BoundField>
                                            <asp:BoundField DataField="Onhold" HeaderText="OnHold"></asp:BoundField>
                                            <asp:BoundField DataField="Priority" HeaderText="Priority" Visible="false"></asp:BoundField>
                                            <asp:BoundField DataField="Order_type" HeaderText="Order Type"></asp:BoundField>
                                            <asp:BoundField DataField="System" HeaderText="IP Address" Visible="false"></asp:BoundField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ShowHeader="true"
                                                HeaderText="Delete User">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="BtDelete" runat="server" ToolTip="Delete User" CommandName="Delete"
                                                        ImageUrl="~/App_themes/Black/images/user_delete.png" Height="20px" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="81px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td align="center" style="padding-left: 184px">
                <asp:Panel ID="PanelEdit" runat="server" align="center">
                    <div class="col-md-5 offset-md-3">

                        <div class="card card-outline-secondary" style="border: thin solid #C0C0C0;">
                            <div class="card-header" style="background-color: #d9241b; height: 30px; color: #FFFFFF; padding-top: 6px;">
                                New User
                        <h3 class="mb-0"></h3>
                            </div>
                            <div class="card-body">
                                <div class="row RowMargin">
                                    <div class="col-md-3" style="padding-top: 6px; margin-left: 20px">
                                        <label>
                                            <b>User Name:</b>
                                        </label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtusername" runat="server" CssClass="form-control" Style="width: 255px;" autocomplete='off'></asp:TextBox>
                                    </div>
                                </div>
                                <div>
                                    <label style="color: red; font-weight: bold" id="lblerror"></label>
                                </div>
                                <div class="row RowMargin">
                                    <div class="col-md-12" style="padding-top: 13px">
                                        <div class="form-check-inline">
                                            <label class="form-check-label" for="ChkAdmin">
                                                <asp:CheckBox ID="ChkAdmin" class="form-check-input" runat="server" Text="Admin" />
                                            </label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                     <label class="form-check-label" for="ChkDU">
                                         <asp:CheckBox ID="ChkDU" class="form-check-input" runat="server" Text="DU" />
                                     </label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                     <label class="form-check-label" for="ChkPro">
                                         <asp:CheckBox ID="ChkPro" class="form-check-input" runat="server" Text="Production" />
                                     </label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <label class="form-check-label" for="ChkQC">
                                        <asp:CheckBox ID="ChkQC" class="form-check-input" runat="server" Text="QC" />
                                    </label>
                                        </div>
                                        <div class="form-check-inline">
                                        </div>
                                        <div class="form-check-inline">
                                        </div>
                                    </div>

                                    <div class="row RowMargin" style="width: 150px;">
                                        <div class="col-md-3" style="padding-top: 13px">
                                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-success"
                                                OnClick="btnsave_Click" OnClientClick="return functionlogouterror();" />
                                        </div>
                                        <div class="col-md-3" style="padding-top: 13px">
                                            <asp:Button ID="Btupdate" runat="server" Text="Update" CssClass="btn btn-success"
                                                OnClick="Btupdate_Click" />
                                        </div>
                                        <div class="col-md-3" style="padding-top: 13px">
                                            <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-success"
                                                OnClick="btncancel_Click" Style="margin-left: 50px;" />
                                        </div>
                                    </div>
                                    <div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <br />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="Hiddenid" runat="server" />
    <div class="footer" style="text-align: center;">
        <p style="background-color: #337ab7; margin-left: 2px; margin-bottom: 0px; color: white; font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;">
            &copy; 2019. All rights reserved | Designed & Developed by String Information Services
        </p>
    </div>
     <script type="text/javascript">
        function functionlogouterror() {
            var error;
            error = document.getElementById("txtusername").value;

            if (error == '') {
                document.getElementById("lblerror").innerHTML = "Please enter the Username";
                return false;
            }
        }
    </script>
</asp:Content>

