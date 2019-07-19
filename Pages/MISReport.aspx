<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MISReport.aspx.cs" Inherits="Pages_MISReport"
    Title="MIS Updation" MasterPageFile="~/Master/TSI1.master" Theme="Black" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" style="background-color: White; height: 600px; overflow: scroll;font-family:Calibri;">
        <tr>
            <td valign="top" align="center" style="padding-top: 20px; ">
                <table border="0">
                    <tr>
                        <td align="center" valign="top">
                            <table border="0">
                                <tr>
                                    <td style="vertical-align: top;padding-left: 400px;">
                                        <table style="vertical-align: top;">
                                            <tr>
                                                <td style="width: 10px;" class="Lblall" align="right">
                                                    <b>Date</b>
                                                </td>
                                                <td align="left">
                                                    &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtDate" runat="server" CssClass="txtuser" Width="100px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdate"
                                                        Format="dd-MMM-yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btntransmit" runat="server" Text="Show" CssClass="MenuFont"
                                                        OnClick="btnShow_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblError" Style="font-size: 13px; color: Red; font-weight: bolder;"
                                            runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 50px;">
                                        <asp:GridView ID="GridView1" runat="server" Width="899px" Font-Names="Verdana" Font-Size="Smaller"
                                            AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="small"
                                            HeaderStyle-ForeColor="white" RowStyle-Font-Size="Small" HeaderStyle-BackColor="#50504e"
                                            AlternatingRowStyle-BackColor="#e5e4e3" RowStyle-Font-Bold="true" RowStyle-HorizontalAlign="Center">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Hour">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Tittle" Text='<%#Eval("tittle") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="17:00 - 18:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour0" Text='<%#Eval("17:00 - 18:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="18:00 - 19:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour1" Text='<%#Eval("18:00 - 19:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="19:00 - 20:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour2" Text='<%#Eval("19:00 - 20:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="20:00 - 21:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour3" Text='<%#Eval("20:00 - 21:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="21:00 - 22:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour4" Text='<%#Eval("21:00 - 22:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="22:00 - 23:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour5" Text='<%#Eval("22:00 - 23:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="23:00 - 24:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour6" Text='<%#Eval("23:00 - 24:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="00:00 - 01:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour7" Text='<%#Eval("00:00 - 01:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="01:00 - 02:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour8" Text='<%#Eval("01:00 - 02:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="02:00 - 03:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour9" Text='<%#Eval("02:00 - 03:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="03:00 - 04:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour10" Text='<%#Eval("03:00 - 04:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="04:00 - 05:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour11" Text='<%#Eval("04:00 - 05:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="05:00 - 06:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour12" Text='<%#Eval("05:00 - 06:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="06:00 - 07:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour13" Text='<%#Eval("06:00 - 07:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="07:00 - 08:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour14" Text='<%#Eval("07:00 - 08:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="08:00 - 09:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour15" Text='<%#Eval("08:00 - 09:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="09:00 - 10:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour16" Text='<%#Eval("09:00 - 10:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="10:00 - 11:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour17" Text='<%#Eval("10:00 - 11:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="11:00 - 12:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour18" Text='<%#Eval("11:00 - 12:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="12:00 - 13:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour19" Text='<%#Eval("12:00 - 13:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="13:00 - 14:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour20" Text='<%#Eval("13:00 - 14:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="14:00 - 15:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour21" Text='<%#Eval("14:00 - 15:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="15:00 - 16:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour22" Text='<%#Eval("15:00 - 16:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="16:00 - 17:00">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hour23" Text='<%#Eval("16:00 - 17:00") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 100px; text-align: center;">
                                        <br />
                                        <asp:GridView ID="GridViewResult" runat="server" Width="899px" Font-Names="Verdana"
                                            Font-Size="Smaller" AutoGenerateColumns="false" SkinID="GridUser1" EmptyDataText="No Records Found"
                                            HeaderStyle-BackColor="#50504e" HeaderStyle-ForeColor="white" HeaderStyle-Font-Bold="true"
                                            HeaderStyle-Font-Size="small" RowStyle-Font-Size="Small" RowStyle-HorizontalAlign="Center">
                                            <Columns>
                                                <asp:BoundField DataField="Name" HeaderText="Name">
                                                    <ItemStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Keycount" HeaderText="Key Count" HtmlEncode="False">
                                                    <ItemStyle Width="30px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AvgkeyTime" HeaderText="Avg Key Time">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="QCCount" HeaderText="QC Count">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AvgqcTime" HeaderText="Avg Qc Time">
                                                    <ItemStyle Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ReviewCount" HeaderText="Review Count">
                                                    <ItemStyle Width="80px" />
                                                    <HeaderStyle Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AvgReviewTime" HeaderText="Avg Review Time">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 500px;">
                                        <br />
                                        <asp:LinkButton ID="btnSubmit" runat="server" Text=" Submit " CssClass="MenuFont"
                                            Width="60px" OnClick="btnSubmit_Click" Style="text-align: center; padding-top: 3px;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
    
    function Clear()
    {       
            document.getElementById("ctl00_ContentPlaceHolder1_DivError").innerHTML='';
    }
    
      function ValidatePass()
      {
          var OldPass=document.getElementById("ctl00_ContentPlaceHolder1_txtOldPassword").value;      
          var NewPassWord=document.getElementById("ctl00_ContentPlaceHolder1_txtNewPassword").value;
          var ConfirmPassWord=document.getElementById("ctl00_ContentPlaceHolder1_txtConformPassword").value;  
          
  
            if(OldPass == "") {                  
              document.getElementById("ctl00_ContentPlaceHolder1_DivError").innerHTML='Error: Old Password Can Not Be Null!';              
              document.getElementById("ctl00_ContentPlaceHolder1_txtOldPassword").focus();
              return false;
            }
            if(NewPassWord == "") {      
              document.getElementById("ctl00_ContentPlaceHolder1_DivError").innerHTML='Error: New Password Can Not Be Null!';              
              document.getElementById("ctl00_ContentPlaceHolder1_txtNewPassword").focus()
              return false;
            }
            if(ConfirmPassWord == "") {      
              
              document.getElementById("ctl00_ContentPlaceHolder1_DivError").innerHTML='Error: Confirm Password Can Not Be Null!';              
              
              document.getElementById("ctl00_ContentPlaceHolder1_txtConformPassword").focus();  
              return false;
            }
            
         
           
             if(NewPassWord!= "")
             {
             
                    if(NewPassWord.length < 8)
                       {
                        
                        
                        document.getElementById("ctl00_ContentPlaceHolder1_DivError").innerHTML='Error:New Password must contain more than eight characters!';              
                        document.getElementById("ctl00_ContentPlaceHolder1_txtNewPassword").focus();
                        return false;
                      }
                  
                     re = /[0-9]/;
                      if(!re.test(NewPassWord))
                       {
                        
                        document.getElementById("ctl00_ContentPlaceHolder1_DivError").innerHTML='Error:New password must contain at least one number (0-9)!';              
                        document.getElementById("ctl00_ContentPlaceHolder1_txtNewPassword").focus();
                        return false;
                       }
             
              }
      
                  if(NewPassWord!= "")
                  {
                  
                            var iChars = "!`@#$%^&*()+=-[]\\\';,./{}|\":<>?~_";                               
                            var flag=false;
                            for (var i = 0; i < NewPassWord.length; i++)
                            {      
                                if (iChars.indexOf(NewPassWord.charAt(i)) != -1)
                                {                                   
                                flag=true;
                                } 
                                
                            }
                            
                            
                            if(flag==false)
                            {
                                 
                                 document.getElementById("ctl00_ContentPlaceHolder1_DivError").innerHTML='New Password Must Contains atleast one Special Charector!';              
                                 document.getElementById("ctl00_ContentPlaceHolder1_txtNewPassword").focus();
                                 return false;
                            }
                            
                            
                  }
                  
                  if(NewPassWord!="" && OldPass!="" )
                  {                     
                     if(NewPassWord!=ConfirmPassWord )
                     {
                      document.getElementById("ctl00_ContentPlaceHolder1_DivError").innerHTML='Error:New Password and Confirm Password did not match.,Please enter both as same!';              
                      document.getElementById("ctl00_ContentPlaceHolder1_txtConformPassword").focus();
                      return false;
                     }
                     else
                     {
                      return true;   
                     }
                  }
                  return true;
      }
       
    </script>

</asp:Content>
