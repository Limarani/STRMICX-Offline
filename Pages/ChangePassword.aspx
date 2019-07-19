<%@ Page Language="C#" MasterPageFile="~/Master/TSI1.master" Theme="Black" Title="CHANGEPWD" AutoEventWireup="false"
    CodeFile="ChangePassword.aspx.cs" Inherits="Pages_ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0" style="background-color: White;height:600px;font-family:Calibri;">
        <tr>
            <td valign="top" align="center" style="padding-top:80px;">
                <table border="0">
                    <tr>
                        <td align="center" valign="top">
                            <table border="0">
                                <tr>
                                    <td>
                                        <div class="header_02">
                                            <span></span>Change Password</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="98%" style="background-color: White">
                                            <tr>
                                                <td>
                                                    <b style="color: Red">
                                                        <div id="DivError" runat="server">
                                                            
                                                        </div>
                                                    </b>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 100px;" valign="top">
                                        <table border="0" cellpadding="5" cellspacing="6">
                                            <tr>
                                                <td class="Lblall">
                                                    Old Password</td>
                                                <td>
                                                    <asp:TextBox ID="txtOldPassword" runat="server" CssClass="txtuser" Width="200px"
                                                        onkeypress="javascript:Clear()" TextMode="Password"></asp:TextBox>
                                                    <span style="color: Red;">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Lblall">
                                                    New Password</td>
                                                <td>
                                                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="txtuser" Width="200px" onkeypress="javascript:Clear()" TextMode="Password"></asp:TextBox>
                                                    <span style="color: Red;">*</span></td>
                                            </tr>
                                            <tr>
                                                <td class="Lblall">
                                                    ConfirmPassword</td>
                                                <td>
                                                    <asp:TextBox ID="txtConformPassword" runat="server" CssClass="txtuser" Width="200px" onkeypress="javascript:Clear()" TextMode="Password"></asp:TextBox>
                                                    <span style="color: Red;">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="MenuFont" OnClientClick="return ValidatePass();"
                                                        OnClick="btnsave_Click2" />
                                                    <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="MenuFont" OnClick="BtnCancel_Click" />
                                                </td>
                                            </tr>                                            
                                        </table>
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
