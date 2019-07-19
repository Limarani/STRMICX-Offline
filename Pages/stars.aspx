<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stars.aspx.cs" Inherits="Pages_scrap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script src="../jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../dist/jquery.loading.min.js" type="text/javascript"></script>
    <script src="../dist/loading.min.js"></script>
    <%--<script src="../JsonParser.js"></script>--%>
    <link href="../App_themes/Black/Black.css" rel="stylesheet" />   
    <script src="../bootstrap.min.js" type="text/javascript"></script>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Gridstyle.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.5/css/bootstrap.min.css" integrity="sha384-AysaV+vQoT3kOAXZkl02PThvDr8HYKPZhNT5h/CXfBThSRXQ6jW5DO2ekP5ViFdi" crossorigin="anonymous" />
    <link href="../dist/loading.min.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .Logout_checklist {
            position: fixed;
            height: 233px;
            width: 649px;
            left: 30%;
            top: 30%;
            right: 20%;
            border: solid 2px #ffffff;
            z-index: 50;
            color: Black;
            font-weight: bolder;
            background-color: #2aadb7;
            -moz-border-radius: 20px;
            -webkit-border-radius: 20px;
        }

        .multiownergrid {
            position: fixed;
            height: 550px;
            width: 1100px;
            left: 5%;
            top: 5%;
            right: 5%;
            border: solid 2px #ffffff;
            z-index: 50;
            color: Black;
            font-weight: bolder;
            background-color: #2aadb7;
            -moz-border-radius: 20px;
            -webkit-border-radius: 20px;
        }
    </style>


    <script type="text/javascript">
        $(document).on("click", "[Id*=btn]", function () {
            $("#Id").html($(".Id", $(this).closest("tr")).html());
            $("#Order_No").html($(".Order_No", $(this).closest("tr")).html());
            $("#State").html($(".State", $(this).closest("tr")).html());
            $("#dialog").dialog({
                title: "View Details",
                buttons: {
                    Ok: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
            return false;
        });
    </script>

    <script type="text/javascript">
        function check() {
            if (document.getElementById("btn1").checked = true) {
                $('#btn1').prop('disabled', false);

            } else {
                $('#btn1').prop('enabled', true);
            }
        };
        $(function () {
            $("#chkRow").click(function () {
                if ($(this).is(":checked")) {
                    $("#example").show();
                } else {
                    $("#example").hide();
                }
            });
        });
    </script>
    <%--<script type="text/javascript">
    function myfunction() {
        alert("Hello");
    }
</script>--%>
    <script type="text/javascript">
        // for check all checkbox  
        function CheckAll(Checkbox) {
            var GridVwHeaderCheckbox = document.getElementById("<%=Grid99.ClientID %>");
            for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>

    <style type="text/css">
        .LogoutMsgBox {
            position: fixed;
            height: 200px;
            width: 400px;
            left: 35%;
            top: 30%;
            right: 20%;
            border: solid 2px #FFFFFF;
            z-index: 50;
            color: #FFFFFF;
            font-weight: bolder; /*background: rgba(0, 0, 0, 1);*/
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            background-color: #1e90ff;
        }

        .page_dimmer {
            position: fixed;
            top: -200px;
            left: 0px;
            height: 135%;
            width: 100%;
            /*filter: alpha(opacity=50); */
            -moz-opacity: .50; /*opacity: .50; */
            z-index: 50;
        }


        .container {
            margin: 4px auto 70px auto;
        }

        .style1 {
            height: 29px;
        }

        .style2 {
            height: 30px;
        }

        .style6 {
            height: 35px;
            width: 231px;
        }

        .style8 {
            height: 30px;
            width: 235px;
        }

        .style10 {
            height: 28px;
        }

        .style13 {
            height: 33px;
            width: 175px;
        }

        .style18 {
            height: 30px;
            width: 200px;
            font-weight: bold;
        }

        .style30 {
            height: 28px;
            width: 172px;
        }

        .style31 {
            height: 29px;
            width: 172px;
        }

        .style32 {
            height: 40px;
            width: 174px;
        }

        .style33 {
            height: 40px;
        }

        .style34 {
            height: 33px;
        }

        .style35 {
            height: 43px;
            width: 172px;
        }

        .style36 {
            height: 43px;
        }

        .style37 {
            height: 47px;
            width: 172px;
        }

        .style38 {
            height: 47px;
        }

        .style39 {
            height: 30px;
            width: 231px;
            font-weight: bold;
        }

        .reg_name {
            max-width: 50px;
        }

        .th {
            text-align: center;
        }

        .auto-style3 {
            text-align: center;
            z-index: 101;
            left: 131px;
            position: absolute;
            top: 62px;
            width: 1083px;
            right: -123px;
            height: 78px;
            margin-left: 0px;
            bottom: 391px;
            overflow: scroll;
        }

        .auto-style4 {
            margin-left: 511;
            margin-top: 164;
        }
    </style>
    <title>STARS</title>
</head>
<body>

    <%-- <div id="templatemo_header" align="center">
        <table border="0" style="height: 90px;">
            <tr>
                <td align="center" valign="top" style="padding-top: 20px;">
                    <asp:Image ID="Imgtitle" runat="server" ImageUrl="~/App_themes/Black/images/t1.png" />
                </td>
            </tr>
        </table>
    </div>--%>

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
            <div style="float: right; padding-top: 10px;">
                <asp:LinkButton ID="LnkLogout" runat="server" CssClass="Lnklogout" Text="Logout" OnClick="LnkLogout_Click"></asp:LinkButton>
            </div>
        </div>
        <div id="templatemo_menu_wrapper">
            <div id="templatemo_menu">
                <table cellpadding="5px" border="0">
                    <tr>
                        <td style="font-family: Calibri; font-size: 15px; color: White; width: 250px;" valign="bottom">
                            <asp:Label ID="Lblusername" runat="server"></asp:Label>
                        </td>
                        <td style="font-family: Calibri; font-size: 15px; color: White;" align="right">
                            <asp:Label ID="Lbltime" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:LinkButton ID="Lnk" runat="server" Text="Back" style="font-family:Calibri;" OnClick="scrap">Back</asp:LinkButton>
        <div style="width: 98%; height: 449px; overflow: scroll; margin-left: 60px;font-family:Calibri;">
            <asp:GridView ID="Grid99" runat="server" AutoGenerateColumns="False" CellPadding="4" GridLines="Horizontal" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" style="font-family:Calibri;" BorderWidth="1px" ForeColor="Black">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SL NO" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-CssClass="Id" />--%>
                    <asp:BoundField DataField="order_no" HeaderText="ORDER NO" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"></asp:BoundField>                                      
                    <asp:BoundField DataField="county" HeaderText="COUNTY" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="180px"></asp:BoundField>
                    <asp:BoundField DataField="state" HeaderText="STATE" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80px" ></asp:BoundField>
                    <asp:BoundField DataField="borrowername" HeaderText="OWNER NAME" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="320px"></asp:BoundField>
                    <asp:BoundField DataField="address" HeaderText="PROPERTY ADDRESS" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="490px"></asp:BoundField>
                    <%--  <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button Text="View" ID="btn" runat="server" BackColor="AliceBlue" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <SelectedRowStyle BackColor="#CC3333" ForeColor="White" Font-Bold="True" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#333333" BorderStyle="Solid" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#242121" />
            </asp:GridView>
        </div>
        <div align="center">
            <asp:Button ID="btnGetRecord" Text="Go" BackColor="#5D7B9D" class="demo"
                OnClick="btnGetRecord_Click" runat="server" Font-Bold="true" Height="40px"
                Width="143px" Style="text-align: center" />
        </div>

        <div class="container">
            <!--Page Dimmer-->
            <div class="page_dimmer" id="pagedimmer" runat="server" visible="false">
            </div>
            <%--<div style="height: 559px; margin-top: 10px">
            </div>--%>
            <div>

                <div class="multiownergrid" id="divmultiowner" runat="server" style="height: 500px; overflow: scroll; margin-left: 116px;">

                    <asp:GridView ID="GridView3" runat="server" BackColor="White" BorderColor="#CCCCCC"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" Width="1025px" CssClass="Grid" Height="400px"
                        AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkselect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div align="center">
                        <asp:Button runat="server" ID="btnscrap" Text="Go" OnClick="btnscrap_Click" class="demo" />

                        &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnclose" Text="Close" OnClick="btnclose_Click" />
                    </div>
                </div>
            </div>

        </div>
        <div class="Logout_checklist" id="captcharead" runat="server" align="center">
            <asp:Image ID="Image1" runat="server" Height="57px" Width="300px" />
            <asp:Button ID="BtnNew" runat="server" CssClass="btn btn-primary" OnClick="BtnNew_Click"
                Text="New" Width="74px" Height="36px" />
            <br />
            <asp:TextBox ID="TextBox3" runat="server" Height="31px" Width="358px"></asp:TextBox>
            <br />
            <asp:Button ID="Button5" runat="server" Height="31px" Text="Submit" Width="81px"
                CssClass="btn btn-primary demo6" OnClick="Button5_Click" />
            <br />
            <br />
        </div>
        <script type="text/javascript">
            $(".demo1").click(function () {
                $.showLoading({ allowHide: true });
            });
            $(".demo2").click(function () {
                $.showLoading({ name: 'jump-pulse', allowHide: true });
            });
            $(".demo3").click(function () {
                $.showLoading({ name: 'circle-turn', allowHide: true });
            });
            $(".demo4").click(function () {
                $.showLoading({ name: 'circle-turn-scale', allowHide: true });
            });
            $(".demo5").click(function () {
                $.showLoading({ name: 'circle-fade', allowHide: true });
            });
            $(".demo").click(function () {
                $.showLoading({ name: 'square-flip', allowHide: true });
            });
            $(".demo7").click(function () {
                $.showLoading({ name: 'line-scale', allowHide: true });
            });
            $(".demo66").click(function () {
                if (confirm("Are you Sure???") == true) {
                    $.showLoading({ name: 'square-flip', allowHide: true });
                } else {
                    return false;
                }

            });

        </script>
        <script type="text/javascript">

            var _gaq = _gaq || [];
            _gaq.push(['_setAccount', 'UA-36251023-1']);
            _gaq.push(['_setDomainName', 'jqueryscript.net']);
            _gaq.push(['_trackPageview']);

            (function () {
                var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
            })();
        </script>
    </form>
</body>
</html>
