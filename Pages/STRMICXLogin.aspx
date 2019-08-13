<%@ Page Language="C#" AutoEventWireup="true" CodeFile="STRMICXLogin.aspx.cs" Inherits="Pages_STRLogin" %>

<!DOCTYPE html>
<html>
<head>
    <title>STRMICX</title>
    <meta charset="utf-8" />

    <link rel="icon" href="../images/favicon.ico" type="image/x-icon" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="Creative Login Form Responsive Widget,Login form widgets, Sign up Web forms , Login signup Responsive web form,Flat Pricing table,Flat Drop downs,Registration Forms,News letter Forms,Elements" />

    <script src="../Script/jquery.date-dropdowns.min.js"></script>
    <script src="../Script/jquery-1.11.1.min.js"></script>
    <link href="../Loginscipts/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <script src="../Script/Jquery-1.8.3-jquery.min.js"></script>
    <link href="../Script/Bootstrap.min.css" rel="stylesheet" />
    <link href="../Script/CustomizedStyle.css" rel="stylesheet" />
    <script src="../Script/jquery-1.4.1.min.js"></script>
    <link href="../Script/Loginstyle.css" rel="stylesheet" />
    <link href="../Script/jquery.Wload.css" rel="stylesheet" />
    <link href="../Script/font-awesome.min.css" rel="stylesheet" />
    <script src="../Script/jquery.min.js"></script>
    <link href="../Script/font-awesome.css" rel="stylesheet" />
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script>
        function AvoidSpace(event) {
            var k = event ? event.which : window.event.keyCode;
            var startPos = event.currentTarget.selectionStart;
            if (k == 32 && startPos == 0) return false;
        }
    </script>

    <style>
        .btn-success:hover, .btn-success:focus, .btn-success.focus, .btn-success:active, .btn-success.active, .open > .dropdown-toggle.btn-success {
            color: #fff;
            background-color: #E74225 !important;
            border-color: #E74225 !important;
        }

        .btn {
            display: inline-block;
            padding: 6px 53px;
            margin-bottom: 0;
            font-size: 14px;
            font-weight: normal;
            line-height: 1.42857143;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
        }
    </style>
</head>
<body style="height:400px;">

    <div class="header-w3l">
        <img src="../images/logo.png" class="img-responsive" style="width:121px; margin-top: 10px;" />
        <h1 style="width:3%;float:left; margin-left:42.5%;">

            <img src="../images/favicon-32x32.png" style="width:40px; height:40px;" />
            </h1>
            <h1 style="Margin-left:11px;float:left;">
                <span style="font-weight:bold; color:#2e302d;"></span><label style="color:#280277; font-weight:bold;font-family:Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;">STRMICX</label>
            </h1>
</div>


    <form runat="server">
       
        <div class="sub-main-w3" id="login">
            <h2 style="font-family:Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;">Login Here</h2>
            <div id="primary-menu">
                <div class="pom-agile">
                    <span class="fa fa-user" aria-hidden="true"></span>
                    <asp:TextBox ID="txtusername" runat="server" CssClass="txtuser" placeholder="Username" oninvalid="this.setCustomValidity('Please enter valid Employee Id/Username')" oninput="setCustomValidity('')" required=required></asp:TextBox>
                   <%-- <input placeholder="Employee Code" class="user" type="text" min="5" maxlength="8"  oninvalid="this.setCustomValidity('Please enter valid id')" onkeypress="return AvoidSpace(event)" oninput="setCustomValidity('')" required autofocus>--%>
                </div>
                <div class="pom-agile">
                    <span class="fa fa-key" aria-hidden="true"></span>
                    <asp:TextBox ID="txtpassword" runat="server" CssClass="txtuser" AutoPostBack="true" placeholder="Password" TextMode="Password" OnTextChanged="txtpassword_TextChanged" oninvalid="this.setCustomValidity('Please enter valid Password')" onkeypress="return AvoidSpace(event)" oninput="setCustomValidity('')" required=required></asp:TextBox>

                    <%--<input placeholder="Password" style="width:73%" id="passwordfield" name="Password" value="password" type="password" min="8" maxlength="20" ng-model="password" oninvalid="this.setCustomValidity('Please enter password')" oninput="setCustomValidity('')" required autofocus />--%>
                  
                    <%--<i class="" data-toggle="tooltip" title="Show Password" data-placement="right" id="logineye" style="left:10px;"><img src="../images/show password icon.jpg" /></i>--%>
                </div>
                <br />
               <%-- <div>
                    <div ng-if="msgP === 'Password Expired!'">
                        <div class="alert alert-danger"> <div OnClick="PasswordShow()" value="Show Hide DIV"><a href="#">Go to Change Password? </a></div></div>
                    </div>
                </div>--%>
                <div>
                    <div><asp:Label ID="Label1" runat="server" CssClass="Lblinfo" ForeColor="red" ></asp:Label></div>
                   <%-- <div class="alert alert-danger">{{msg}}</div>--%>
                </div>
                <br />
               <%-- <div class="sub-w3l">
                    <div class="sub-agile">
                    </div>
                    <a href="#" OnClick="Forgot()">Forgot Password?</a>
                    <div class="clear"></div>
                </div>--%>
                <div>
                   <%-- <button id="btnLogin" value="Login" type="submit" class="btn btn-success" OnClick="LoginCheck()">Login</button>--%>
                  <asp:Button ID="Lnksignin" runat="server" Text="Login" class="btn btn-success" OnClick="Lnksignin_Click"/>
                </div>
                <br />
            </div>
        </div>

<%--        <div id="passowrdchange" class="sub-main-w3" ng-show="passwordchange">
            <div class="primary-menu">
                <div class="pom-agile">
                    <input type="password" style="width: 70%;" placeholder="Enter Old Password" id="passwordchange" ng-model="oldpassword" oninput="setCustomValidity('')" oninvalid="this.setCustomValidity('Please Enter old password')" required />
                    <i class="" data-toggle="tooltip" title="Show Password" data-placement="right" id="old" style="left:10px;"><img src="images/show password icon.jpg" /></i>
                </div>
                <div class="clearfix"> </div>
            </div>
            <div class="sign-u">
                <div class="pom-agile">
                    <input type="password" id="newpassword" style="width: 70%;" placeholder="Enter New Password" pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$" ng-keydown="removeText($event)" ng-model="newpassword" min="8" maxlength="20" oninput="setCustomValidity('')" oninvalid="this.setCustomValidity('Password should be at least 8 characters and contain 1no & 1specialchar')" required />
                    <i class="" data-toggle="tooltip" title="Show Password" data-placement="right" id="new" style="left:10px;"><img src="images/show password icon.jpg" /></i>
                </div>
                <div class="clearfix"> </div>
            </div>
            <div class="sign-u">
                <div class="pom-agile">
                    <input type="password" id="confirmpassword" style="width: 70%;" placeholder="Enter Confirm Password" oninvalid="this.setCustomValidity('Please Enter confirm password')" oninput="setCustomValidity('')" min="8" maxlength="20" ng-keypress="getkeys($event)" ng-model="confirmpassword" required>
                    <i class="" data-toggle="tooltip" title="Show Password" data-placement="right" id="confirm" style="left:10px;"><img src="images/show password icon.jpg" /></i>
                </div>
                <div class="clearfix"> </div>
            </div>
            <div class="sign-u">
                <div class="sign-up1"></div>
                <div class="sign-up2">
                    <span style="color:red;background-color:#f2dede">{{result}}</span>
                </div>
            </div>
            <br />
            <div ng-hide="!msgsucess">
                <div ng-if="msgsucess === 'Password Changed Sucessfully!'">
                    <div class="alert alert-danger"> {{msgsucess}} <div ng-click="GotoLogin()" value="Show Hide DIV"><a href="#">Go to Login? </a></div></div>
                </div>
            </div>
            <div ng-hide="!msg">
                <div class="alert alert-danger">{{msg}}</div>
            </div>
            <div class="right-w3l">
                <button id="btnSubmit" type="submit" class="btn btn-success" ng-click="btnchangepassword()">Submit</button>
                <button id="Clear" type="submit" class="btn btn-success" ng-click="cleardata()">Reset</button>
                <!--<input type="submit" class="right-w3l" value="Submit" ng-click="btnchangepassword()">
                <input type="button" class="button" value="Clear" ng-click="cleardata()">-->
            </div>
            <br /><br />
            <div class="clearfix"> </div>
        </div>

        <div class="sub-main-w3" id="forgetpassword" ng-show="forgetpassword">
            <h2>Forgot Password</h2>
            <div id="primary-menu1">
                <div class="pom-agile">
                    <span class="fa fa-user" aria-hidden="true"></span>
                    <input placeholder="Employee Code" class="user" type="text" ng-model="empid1" name="empid1" min="5" maxlength="8" number oninvalid="this.setCustomValidity('Please enter valid id')" onkeypress="return AvoidSpace(event)" oninput="setCustomValidity('')" ng-required="true" autofocus>
                </div>
                <div class="pom-agile">
                    <span class="fa fa-envelope" aria-hidden="true"></span>
                    <input placeholder="Email ID" pattern="[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,3}$" name="email" class="pass" type="email" ng-model="email" oninvalid="this.setCustomValidity('Please enter email id')" oninput="setCustomValidity('')" ng-required="true" autofocus>

                </div>
                <br />
                <div class="pom-agile">
                    <span class="fa fa-calendar" aria-hidden="true"></span>
                    <input type="text" class="pom-agile-dob" ng-model="dob1" id="example2" placeholder="Date Of Birth" required>
                </div>
                <div class="sub-w3l">
                    <div ng-hide="!msgsent">
                        <div>
                            <div class="alert alert-danger">{{msgsent}}</div>
                        </div>
                    </div>
                    <div ng-hide="!DOB">
                        <div>
                            <div class="alert alert-danger">{{DOB}}</div>
                        </div>
                    </div>
                    <a href="#" ng-click="GotoLogin()" style="margin-top:10px;">Go to Login</a>
                    <div class="clear"></div>
                </div>
                <div class="center-w3l">
                    <button id="btnSend" type="submit" class="btn btn-success" ng-click="ForgotCheck()">Submit</button>
                    <!--<input type="submit" class="button" value="submit" id="btnSend" ng-click="ForgotCheck()">-->
                </div>
            </div>
        </div>--%>
    </form>
    <div class="footer" style="font-family:"Roboto", -apple-system, BlinkMacSystemFont, "Segoe UI", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", sans-serif;">
        <p>
            &copy; 2019. All rights reserved | Designed & Developed by String Information Services
        </p>
    </div>

    <script type="text/javascript">
        $(function () {
            $("#example2").dateDropdowns({
                submitFieldName: 'example2',
                submitFormat: "dd/mm/yyyy"
            });
            // Set all hidden fields to type text for the demo
            $('input[type="hidden"]').attr('type', 'text').attr('readonly', 'readonly');
        });

        $("#passwordfield").on("keyup", function () {
            if ($(this).val())
                $("#logineye").show();
            else
                $("#logineye").hide();
        });

        $("#logineye").mousedown(function () {
            $("#passwordfield").attr('type', 'text');
        }).mouseup(function () {
            $("#passwordfield").attr('type', 'password');
        }).mouseout(function () {
            $("#passwordfield").attr('type', 'password');
        });

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        $("#passwordchange").on("keyup", function () {
            if ($(this).val())
                $("#old").show();
            else
                $("#old").hide();
        });

        $("#old").mousedown(function () {
            $("#passwordchange").attr('type', 'text');
        }).mouseup(function () {
            $("#passwordchange").attr('type', 'password');
        }).mouseout(function () {
            $("#passwordchange").attr('type', 'password');
        });

        //New password
        $("#newpassword").on("keyup", function () {
            if ($(this).val())
                $("#new").show();
            else
                $("#new").hide();
        });

        $("#new").mousedown(function () {
            $("#newpassword").attr('type', 'text');
        }).mouseup(function () {
            $("#newpassword").attr('type', 'password');
        }).mouseout(function () {
            $("#newpassword").attr('type', 'password');
        });

        //Confirm password
        $("#confirmpassword").on("keyup", function () {
            if ($(this).val())
                $("#confirm").show();
            else
                $("#confirm").hide();
        });

        $("#confirm").mousedown(function () {
            $("#confirmpassword").attr('type', 'text');
        }).mouseup(function () {
            $("#confirmpassword").attr('type', 'password');
        }).mouseout(function () {
            $("#confirmpassword").attr('type', 'password');
        });

    </script>
</body>
</html>