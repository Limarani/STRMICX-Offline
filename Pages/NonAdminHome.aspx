<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" AutoEventWireup="true" CodeFile="NonAdminHome.aspx.cs" Inherits="Pages_NonAdminHome" Title="HOME" Theme="Black" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 
<table border="0" style="width:100%;height:600px;background-color:#f5f5f5;font-family:Calibri;" align="center"> 
    <tr>
        <td align="center">
            <table border="0" width="100%">
                <tr>
                    <td align="center" valign="bottom">
                        <asp:Image ID="Imgtitle" runat="server" ImageUrl="~/App_themes/Black/images/home1.png" />
                    </td>
                </tr>
           </table> 
        </td>
    </tr>
    <tr>
        <td align="center" style="width:75%;" valign="top">
            <asp:Panel ID="PanelGrid" runat="server" Width="75%" Height="300px" ScrollBars="auto"  align="center"> 
                 <asp:GridView ID="GridErrorDetails" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="Order_No" GridLines="None" CssClass="Gnowrap" AlternatingRowStyle-CssClass="alt" OnRowCommand="GridErrorDetails_RowCommand" OnRowEditing="GridErrorDetails_RowEditing">
                    <Columns>
                        <asp:BoundField DataField="Order_No" HeaderText="Order No" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Error" HeaderText="Process Error" />
                        <asp:BoundField DataField="ErrorField" HeaderText="Error Field" />
                        <asp:BoundField DataField="Incorrect" HeaderText="Incorrect" />
                        <asp:BoundField DataField="Correct" HeaderText="Correct" />
                        <asp:BoundField DataField="OP_Comments" HeaderText="Operator Comments" />
                        <asp:BoundField DataField="ErrorComments" HeaderText="Error Comments" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="Lnkbtnaccept" CommandName="Accept"  runat="server" OnClientClick="return confirm('Do you want to Accept?')" Enabled='<%# Eval("ErrorComments").ToString() != "Accepted" %>' >Accept</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>                       
                 </asp:GridView>                            
            </asp:Panel>
        </td>
    </tr>                               
</table>
</asp:Content>

