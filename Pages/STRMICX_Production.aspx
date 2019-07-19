<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="STRMICX_Production.aspx.cs"
    Inherits="Pages_STRMICX_Production" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Production</title>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <style type="text/css">
        .glyphicon {
            font-size: 20px;
        }
    </style>
    <style type="text/css">
        .Initial {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            /*background: url("../Images/bg.jpg") no-repeat right top;*/
            background-color: white;
            color: Black;
            font-weight: bold;
        }

            .Initial:hover {
                color: red;
                /*background: url("../Images/") no-repeat right top;*/
                background-color: #c18383;
            }

        .Clicked {
            float: left;
            display: block;
            /*background: url("../Images/L3.png") no-repeat right top;*/
            background-color: brown;
            padding: 4px 18px 4px 18px;
            color: Black;
            font-weight: bold;
            color: White;
        }
    </style>
    <style>
        .container {
            /*background-color:#f3f0f0;
           background-color:#DBDBDB;*/
            /*background-color: #62d4e6;*/
            background-color:#c1d7dc;
            /* background: url("../Images/14.jpg"); */
            height: 850px;
            width: 1347px;
            /*border:outset;
            border-color:black;*/
        }

        .tab .nav-tabs {
            border: none;
            padding: 0 0 10px;
            position: relative;
            perspective: 17em;
        }

            .tab .nav-tabs li {
                transform-origin: 0 0;
                margin-right: 10px;
                box-shadow: 0 0 3px rgba(0,0,0,0.2);
                animation: swing 1.5s ease-in-out infinite alternate;
            }

                .tab .nav-tabs li:nth-child(1) {
                    animation-delay: 0.9s;
                }

                .tab .nav-tabs li:nth-child(2) {
                    animation-delay: 0.6s;
                }

                .tab .nav-tabs li:nth-child(3) {
                    animation-delay: 0.3s;
                }

                .tab .nav-tabs li:nth-child(4) {
                    animation-delay: 0.0s;
                }

                .tab .nav-tabs li a {
                    outline: none;
                    padding: 10px 30px;
                    background: #FFF;
                    color: #808080;
                    text-transform: uppercase;
                    border: 1px solid transparent;
                    /*transition: 0.3s;*/
                }

        .nav-tabs li.active a,
        .nav-tabs li.active a:focus,
        .nav-tabs li.active a:hover {
            background: #e77d20;
            color: #fff;
            border: 1px solid #fff;
        }

        .tab .tab-content {
            padding: 30px 20px;
            background: rgba(0,0,0,0.2);
            box-shadow: 0 0 10px rgba(0,0,0,0.4);
            width: 150%;
        }

            .tab .tab-content p {
                text-transform: capitalize;
                color: #808080;
                font-size: 17px;
                height: 450px;
            }

        @keyframes swing {
            from {
                transform: rotateX(10deg) rotateZ(0deg);
            }

            to {
                transform: rotateX(-5deg) rotateZ(0deg);
            }
        }

        @media only screen and (max-width: 640px) {
            .tab .nav-tabs li {
                width: 100%;
            }
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#myModal").on('show.bs.modal', function (event) {
                var button = $(event.relatedTarget);  // Button that triggered the modal
                var titleData = button.data('title'); // Extract value from data-* attributes
                $(this).find('.modal-title').text(titleData + ' Order');
            });
        });
    </script>
    <script type="text/javascript">
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function isAmount(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 46) {
                var inputValue = $("#input").val()
                if (inputValue.indexOf('.') < 1) {
                    return true;
                }
                return false;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function isDate(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;


            if (charCode == 46) {
                var inputValue = $("#input").val()
                if (inputValue.indexOf('.') < 1) {
                    return true;
                }
                return false;
            }
            if (charCode != 45 && charCode != 47 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            return true;
        }
        function parseDate(evt) {
            var m = evt.match(/^(\d{1,2})-(\d{1,2})-(\d{4})$/);
            return (m) ? new Date(m[3], m[2] - 1, m[1]) : null;
        }
    </script>
    <script type="text/javascript">
        function checkEnterDate(e) {


            var key;
            if (window.event) {
                key = window.event.keyCode; //IE
            }
            else {
                key = e.which; //firefox
            }
            if (key == 127 && key == 224 && key != 45 && key != 47 && key > 31 && (key < 48 || key > 57) && (window.event.ctrlKey && (key != 118 || key != 86))) {
                return false;
            }
            //if (key == 86) {
            //    event.returnValue = false;
            //}
            return true;
        }
    </script>
    <script type="text/javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        $(function () {
            $(".numeric").bind("keypress", function (e) {
                var keyCode = e.which ? e.which : e.keyCode
                var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
                //$(".error").css("display", ret ? "none" : "inline");
                return ret;
            });
            $(".numeric").bind("paste", function (e) {
                var keyCode = e.which ? e.which : e.keyCode
               (event.keyCode == 86 && event.ctrlKey === true)
                return true;
            });
            $(".numeric").bind("drop", function (e) {
                return false;
            });
        });
    </script>
    <%-- <script src="//oss.maxcdn.com/momentjs/2.8.2/moment.min.js"></script>--%>
    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>
    <%--   <script>
        $(function () {
            $("#newModalForm").validate({
                rules: {
                    pName: {
                        required: true,

                    },
                    action: "required"
                },
                messages: {
                    pName: {
                        required: "Please enter some data",

                    },
                    action: "Please provide some data"
                }
            });
        });
    </script>--%>
    <script type="text/javascript">

        var isShift = false;

        var seperator = "/";

        function DateFormat(txt, keyCode) {

            if (keyCode == 16)

                isShift = true;

            //Validate that its Numeric

            //if (keyCode != 45 && keyCode != 47 && keyCode > 31 && (keyCode < 48 || keyCode > 57)) {
            //    return false;
            //}

            if (((keyCode >= 48 && keyCode <= 57) || keyCode == 8 || keyCode == 127 || keyCode == 224 || keyCode == 22 || keyCode == 17 || keyCode == 2 ||

                 keyCode <= 37 || keyCode <= 39 || keyCode == 86 ||

                 (keyCode >= 96 && keyCode <= 105)) && isShift == false) {

                if ((txt.value.length == 2 || txt.value.length == 5) && keyCode != 8) {

                    txt.value += seperator;

                }

                return true;

            }

            else {

                return false;

            }

        }



        function ValidateDate(txt, keyCode) {

            if (keyCode == 16)

                isShift = false;

            var val = txt.value;

            var lblmesg = document.getElementById("<%=lblMesg.ClientID%>");

            if (val.length == 10) {

                var splits = val.split("/");

                var dt = new Date(splits[0] + "/" + splits[1] + "/" + splits[2]);


                //Validation for Dates

                if (dt.getMonth() + 1 == splits[0] && dt.getDate() == splits[1] && dt.getFullYear() == splits[2]) {
                    // lblmesg.style.color="green";
                    // lblmesg.innerHTML = "Valid Date";            
                    txt.style.color = "Black";
                }
                else {

                    // lblmesg.style.color="red";

                    //lblmesg.innerHTML = "Invalid Date";

                    txt.style.color = "red";

                    return;

                }

                //Range Validation

                //if(txt.id.indexOf("txtRange") != -1)

                //    RangeValidation(dt);



                ////BirthDate Validation

                //if(txt.id.indexOf("txtBirthDate") != -1)               

                //    BirthDateValidation(dt)

            }
            else if (val.length < 10) {

                // lblmesg.style.color="blue";

                //  lblmesg.innerHTML = "Required dd/mm/yy format. Slashes will come up automatically.";


            }

        }

    </script>
    <script type="text/javascript">
        //Gets called after the page is completely loaded
        $(document).ready(function () {
            $('input#submit').click(function (event) {
                $("div#errMsg").html("");
                var email = $("textarea#txtchaingrantor").val();
                var confEmail = $("textarea#txtchaingrantee").val();
                var errMsg = "<ul>";
                if (email == "")
                    errMsg = errMsg + "<li>Please enter Grantor address</li>";
                if (confEmail == "")
                    errMsg = errMsg + "<li>Please enter Grantee email</li>";

                errMsg = errMsg + "</ul>";
                $("div#errMsg").css("background-color", "red");
                $("div#errMsg").html(errMsg);

            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#txtlientype").change(function (evt) {

                $("#txtliendocumentype").attr("disabled", true);
                $("#txtliendate").attr("disabled", true);
                $("#txtlienrecordeddate").attr("disabled", true);
                $("#txtlienbook").attr("disabled", true);
                $("#txtlienpage").attr("disabled", true);
                $("#txtlienliber").attr("disabled", true);
                $("#txtlienvolume").attr("disabled", true);
                $("#txtlieninstrument").attr("disabled", true);
                $("#txtlienamount").attr("disabled", true);
                $("#txtlienholder").attr("disabled", true);
                $("#txtlienagainst").attr("disabled", true);
                $("#txtlienstate").attr("disabled", true);
                $("#txtlienstatedistrict").attr("disabled", true);
                $("#txtliencounty").attr("disabled", true);
                $("#txtlieninfavourof").attr("disabled", true);
                $("#txtliencourtdistrict").attr("disabled", true);
                $("#txtliencourttype").attr("disabled", true);
                $("#txtlientrustee").attr("disabled", true);
                $("#txtlienassignor").attr("disabled", true);
                $("#txtlienassignee").attr("disabled", true);
                $("#txtlienassignbook").attr("disabled", true);
                $("#txtlienassignpage").attr("disabled", true);
                $("#txtlienassignliber").attr("disabled", true);
                $("#txtlienassignvolume").attr("disabled", true);
                $("#txtlienassigninstrument").attr("disabled", true);
                $("#txtliendocumentname").attr("disabled", true);
                $("#txtlienpurpose").attr("disabled", true);
                $("#txtliengrantor").attr("disabled", true);
                $("#txtliengrantee").attr("disabled", true);
                $("#txtlienendorsements").attr("disabled", true);
                $("#txtliencasenumber").attr("disabled", true);
                $("#txtlientaxyear").attr("disabled", true);
                $("#txtlieninstallmentno").attr("disabled", true);
                $("#txtlieninstallmentamount").attr("disabled", true);
                $("#txtlienmaturitydate").attr("disabled", true);

                if ($(this).val() == "1085") {

                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtliencourtdistrict").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);

                }
                else if ($(this).val() == "1143") {

                    $("#txtlienassignor").attr("disabled", false);
                    $("#txtlienassignee").attr("disabled", false);

                }
                else if ($(this).val() == "1147") {

                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);

                }
                else if ($(this).val() == "1145") {
                    $("#txtliendocumentname").attr("disabled", false);

                }
                else if ($(this).val() == "1144") {


                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);

                }
                else if ($(this).val() == "1102") {

                    $("#txtliendocumentname").attr("disabled", false);

                }
                else if ($(this).val() == "1130") {

                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlientrustee").attr("disabled", false);

                }
                else if ($(this).val() == "1104") {
                    $("#txtlieninfavourof").attr("disabled", false);
                }
                else if ($(this).val() == "1103") {
                    $("#txtlienpurpose").attr("disabled", false);
                }
                else if ($(this).val() == "1123") {

                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);

                }
                else if ($(this).val() == "1162") {
                    $("#txtlienassignor").attr("disabled", false);
                    $("#txtlienassignee").attr("disabled", false);
                }
                else if ($(this).val() == "1068") {
                    //Enable the textbox
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    // This stops the Postback                 
                }
                else if ($(this).val() == "1023") {
                    //Enable the textbox
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencourtdistrict").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    // This stops the Postback                 
                }
                else if ($(this).val() == "1030") {
                    //Enable the textbox
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencourtdistrict").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    // This stops the Postback                 
                }
                else if ($(this).val() == "1020") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1024") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1153") {
                    $("#txtlienpurpose").attr("disabled", false);

                }
                else if ($(this).val() == "1017") {
                    $("#txtlienassignor").attr("disabled", false);
                    $("#txtlienassignee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1139") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1036") {

                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                }
                else if ($(this).val() == "1151") {
                    $("#txtlienpurpose").attr("disabled", false);

                }
                else if ($(this).val() == "1025") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1037") {
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);

                }
                else if ($(this).val() == "1094") {

                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1026") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlientrustee").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);

                }
                else if ($(this).val() == "1018") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlientrustee").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtlienmaturitydate").attr("disabled", false);
                }
                else if ($(this).val() == "1021") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1038") {
                    $("#txtliendocumentname").attr("disabled", false);
                }
                else if ($(this).val() == "1022") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1052") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienholder").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                }
                else if ($(this).val() == "1050") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                }
                else if ($(this).val() == "1039") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1154") {
                    $("#txtlienpurpose").attr("disabled", false);
                }
                else if ($(this).val() == "1150") {
                    $("#txtlienpurpose").attr("disabled", false);
                }
                else if ($(this).val() == "1002") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencourtdistrict").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1160") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencourtdistrict").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1141") {
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienholder").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                }
                else if ($(this).val() == "1016") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencourtdistrict").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1149") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1009") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1152") {
                    $("#txtlienpurpose").attr("disabled", false);
                }
                else if ($(this).val() == "1046") {
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtliengrantor").attr("disabled", false);
                }
                else if ($(this).val() == "1028") {

                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1049") {
                    $("#txtliengrantor").attr("disabled", false);

                }
                else if ($(this).val() == "1057") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                }
                else if ($(this).val() == "1034") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1031") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1040") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1142") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                }
                else if ($(this).val() == "1032") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1033") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1140") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                }
                else if ($(this).val() == "1053") {
                    $("#txtliengrantor").attr("disabled", false);
                    $("#txtliengrantee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1041") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                }
                else if ($(this).val() == "1137") {
                    $("#txtlienholder").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                }
                else if ($(this).val() == "1035") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1048") {
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1019") {
                    $("#txtlientrustee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1001") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtlienmaturitydate").attr("disabled", false);
                }
                else if ($(this).val() == "1164") {
                    $("#txtlienpurpose").attr("disabled", false);
                }
                else if ($(this).val() == "1060") {
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1080") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1089") {
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1054") {
                    $("#txtlienassignor").attr("disabled", false);
                    $("#txtlienassignee").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1127") {

                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1112") {
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1132") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1118") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtliengrantor").attr("disabled", false);
                    $("#txtlienpurpose").attr("disabled", false);
                }
                else if ($(this).val() == "1108") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);

                }
                else if ($(this).val() == "1061") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencourttype").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);

                }
                else if ($(this).val() == "1093") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                }
                else if ($(this).val() == "1064") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                }
                else if ($(this).val() == "1096") {
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1067") {
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlientrustee").attr("disabled", false);
                }
                else if ($(this).val() == "1073") {
                    $("#txtliengrantor").attr("disabled", false);
                    $("#txtliengrantee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtliencourttype").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                }
                else if ($(this).val() == "1072") {
                    $("#txtliengrantor").attr("disabled", false);
                    $("#txtliengrantee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtliencourttype").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                }
                else if ($(this).val() == "1161") {

                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1056") {

                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1086") {

                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1133") {
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1083") {

                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtliengrantor").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1122") {

                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1155") {
                    $("#txtlienassignee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1124") {

                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1110") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);

                }
                else if ($(this).val() == "1076") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1126") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                }
                else if ($(this).val() == "1125") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                }
                else if ($(this).val() == "1070") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1111") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1063") {
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1065") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencourttype").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                }
                else if ($(this).val() == "1131") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtliencourttype").attr("disabled", false);
                    $("#txtliencounty").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                }
                else if ($(this).val() == "1135") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliengrantor").attr("disabled", false);
                    $("#txtliengrantee").attr("disabled", false);

                }
                else if ($(this).val() == "1097") {
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1109") {
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1120") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1078") {
                    $("#txtliendocumentname").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1079") {
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                }
                else if ($(this).val() == "1115") {
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1157") {
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1116") {
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1158") {
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1055") {
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1117") {
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1066") {
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                }
                else if ($(this).val() == "1092") {
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                }
                else if ($(this).val() == "1088") {
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlientrustee").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                }
                else if ($(this).val() == "1159") {
                    $("#txtliengrantor").attr("disabled", false);
                    $("#txtliengrantee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1082") {
                    $("#txtliengrantor").attr("disabled", false);
                    $("#txtliengrantee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1134") {
                    $("#txtliengrantor").attr("disabled", false);
                    $("#txtliengrantee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1099") {
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1074") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1075") {
                    $("#txtlienpurpose").attr("disabled", false);
                }
                else if ($(this).val() == "1107") {
                    $("#txtlienassignor").attr("disabled", false);
                    $("#txtlienassignee").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1106") {
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                }
                else if ($(this).val() == "1071") {
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1129") {
                    $("#txtlienpurpose").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                }
                else if ($(this).val() == "1087") {
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                }
                else if ($(this).val() == "1095") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliendocumentname").attr("disabled", false);
                }
                else if ($(this).val() == "1069") {
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1119") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencourttype").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                }
                else if ($(this).val() == "1136") {
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencourttype").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                }
                else if ($(this).val() == "1114") {
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                }
                else if ($(this).val() == "1156") {
                    $("#txtlieninfavourof").attr("disabled", false);
                    $("#txtlienagainst").attr("disabled", false);
                    $("#txtlienamount").attr("disabled", false);
                    $("#txtliendate").attr("disabled", false);
                    $("#txtlienrecordeddate").attr("disabled", false);
                    $("#txtlienbook").attr("disabled", false);
                    $("#txtlienpage").attr("disabled", false);
                    $("#txtlienliber").attr("disabled", false);
                    $("#txtlienvolume").attr("disabled", false);
                    $("#txtlieninstrument").attr("disabled", false);
                    $("#txtliencasenumber").attr("disabled", false);
                }
                else if ($(this).val() == "1193") {
                    $("#txtlienassignor").attr("disabled", false);
                    $("#txtlienassignee").attr("disabled", false);
                }
                evt.preventDefault();
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $("#txtotherdeedtype").attr("disabled", true);
            $("#txtdeedtype").change(function () {
                $("div#lblMesg").html("");
                $("#txtotherdeedtype").attr("disabled", true);

                if ($(this).val() == "Other") {
                    $("#txtotherdeedtype").attr("disabled", false);
                }
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $("#ddlstatus").change(function () {
                $("#btnsave").attr("disabled", false);
                $("#btnsave").attr("class", "btn btn-primary");
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('[name=txtnum]').on('keypress input', function (evt) {
                var value = $(this).val();
                value = value.replace(/[^0-9]/g, ''); //removes any non-number char
                $(this).val(value);
            });
            $('[name=txtamount]').on('keypress input', function (evt) {
                var value = $(this).val();
                value = value.replace(/[^0-9\.]/g, ''); // removes any non-number char with out .
                $(this).val(value);
            });
            $('[name=txtdate]').on('keypress input', function (evt) {
                var value = $(this).val();
                value = value.replace(/[^0-9\/]/g, ''); // removes any non-number char with out /
                $(this).val(value);
            });
        });
    </script>
    <script type="text/javascript">

        //function checkDate(event) {

        //    var ExpiryDate = getTarget(event);
        //    // check date and print message 
        //    if (validateformat(ExpiryDate)) {
        //        //alert('OK');
        //    }
        //    else {
        //        alert('Invalid date format! Date format, such as MM/DD/YYYY. ');
        //    }
        //   // return stopPropagation(event);
        //}
        function checkDate(event) {

            var node = getTarget(event);

            if (node) {
                var result = isvalidateDate(node);
                if (result != "") {
                    showError(node, result);
                } // endif 
            } // endif 

            return stopPropagation(event);
        }
        function isvalidateDate(node) {

            var errorMessage = "Date needs to be in date format, such as 01/31/2001.";

            if (node.value != "") {

                if (node.value.length !== 10) {
                    return errorMessage;
                }
                if (node.value.substring(2, 3) !== '/' || node.value.substring(5, 6) !== '/') {
                    return errorMessage;
                }
                //if (node.value.substring(2, 0) == '2' && node.value.substring(5, 3) >= 30) {
                //    return errorMessage;
                //}           
                // try parsing as date using JavaScript Date constructor 
                var dateValue = new Date(node.value.replace(/-/g, "/"));
                if (isFinite(dateValue)) {
                    // if two-digit year, guess at correct century                   
                    if (node.value.match(/\D\d{1,2}$/) && dateValue.getFullYear() < (new Date().getFullYear() - 96)) {
                        dateValue.setFullYear(dateValue.getFullYear() + 100);
                    }
                    // format as mm/dd/yyyy 
                    node.value = (dateValue.getMonth() + 1) + "/" + dateValue.getVarDate() + "/" + dateValue.getFullYear();
                    return "";
                }
                else {
                    return errorMessage;
                }
            }
            return "";
        }
        function showError(node, message) {
            alert(message);
            // focus & select element with error (Mozilla, Safari & Chrome require brief delay before moving focus) 
            setTimeout(function () { node.focus(); node.select(); }, 1);
        }

        function supressEnterKeySubmit(event) {
            // prevent enter key from submitting form 
            var event = event || window.event;
            if (event.keyCode == 13) {
                return false;
            }
        }
        function getTarget(event) {
            var e = event || window.event;

            if (e.target)
                return e.target;
            else
                return e.srcElement;
        }
        function stopPropagation(event) {

            if (event.stopPropagation)
                event.stopPropagation();

            if (event.preventDefault)
                event.preventDefault();

            if (event.cancelBubble)
                event.cancelBubble = true;

            if (event.returnValue)
                event.returnValue = false;
            return false;
        }

        function validateformat(ExpiryDate) {
            var objDate,  // date object initialized from the ExpiryDate string 
                mSeconds, // ExpiryDate in milliseconds 
                day,      // day 
                month,    // month 
                year;     // year 
            // date length should be 10 characters (no more no less) 

            if (ExpiryDate.value.length != 0) {
                if (ExpiryDate.value.length !== 10) {
                    return false;
                }
                // third and sixth character should be '/' 
                if (ExpiryDate.value.substring(2, 3) !== '/' || ExpiryDate.value.substring(5, 6) !== '/') {
                    return false;
                }
                // extract month, day and year from the ExpiryDate (expected format is mm/dd/yyyy) 
                // subtraction will cast variables to integer implicitly (needed 
                // for !== comparing) 
                month = ExpiryDate.value.substring(0, 2) - 1; // because months in JS start from 0 
                day = ExpiryDate.value.substring(3, 5) - 0;
                year = ExpiryDate.value.substring(6, 10) - 0;
                // test year range 
                if (year < 1000 || year > 3000) {
                    return false;
                }
                // convert ExpiryDate to milliseconds 
                mSeconds = (new Date(year, month, day)).getTime();
                // initialize Date() object from calculated milliseconds 
                objDate = new Date();
                objDate.setTime(mSeconds);
                // compare input date and parts from Date() object 
                // if difference exists then date isn't valid 
                if (objDate.getFullYear() !== year ||
                    objDate.getMonth() !== month ||
                    objDate.getDate() !== day) {
                    return false;
                }
            }
            // otherwise return true 
            return true;
        }

    </script>
    <script type="text/javascript">
        function callConfirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure, want to do next order ?")) {
                alert("Thank You!"); //Click OK
                confirm_value.value = "Yes";

            }
            else {
                alert("See you later."); //Click CANCEL
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function showModal() {
            $("#myModalnextorder").modal('show');
        }
        function showModalError() {
            $("#myModalError").modal('show');
        }
        function showModalTax() {
            $("#myModalTax").modal('show');
        }
        function showModalActionResult() {
            $("#myModalActionResult").modal('show');
        }

        //$(function () {
        //    $("#btnShow").click(function () {
        //        showModal();
        //    });
        //});
    </script>
    <script type="text/javascript">
        function addCommas(clientID) {

            var nStr = document.getElementById(clientID.id).value;

            nStr += '';
            x = nStr.split('.');

            if (!x[0]) {
                x[0] = "0";
            }

            x1 = x[0];
            if (!x[1]) {
                x[1] = "00";
            }

            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;

            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }

            document.getElementById(clientID.id).value = x1 + x2;
            return true;
        }
        function removeCommas(clientID) {
            var nStr = document.getElementById(clientID.id).value;
            nStr = nStr.replace(/,/g, '');
            document.getElementById(clientID.id).value = nStr;

            $(clientID).select();
            return true;
        }
    </script>
</head>
<body style="background-color: #23a22700">

    <form id="form1" runat="server" style="border: 1px solid red">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%-- <asp:Content ID="Content1" ContentPlaceHolderID="MenuContentPlaceHolder" runat="Server">
        </asp:Content>
        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
        </asp:Content>
        <asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        </asp:Content>--%>
        <div>
            <div class="container" runat="server">
                <table style="width: 1320px;">
                    <tr>
                        <td>
                            <h4><span class="label label-default">Order No:</span>
                                <asp:Label ID="lblOrderNo" runat="server" Font-Names="Verdana" ForeColor="White" Font-Bold="True" class="label label-primary label-bs">0</asp:Label>
                                <span>|</span> <span class="label label-default">Name:</span>
                                <asp:Label ID="lblProUsrNme" runat="server" Font-Names="Verdana" ForeColor="White" class="label label-primary label-bs">0</asp:Label>
                                <span>|</span> <span class="label label-default">Date:</span>
                                <asp:Label ID="lblCreatedDate" runat="server" Font-Names="Verdana" ForeColor="White" class="label label-primary label-bs">0</asp:Label>
                                <b><span class="label label-default">Process Time:</span> </b>
                                <b><span class="label label-primary label-bs">
                                    <script type="text/javascript" lang="javascript" src="../Scripts/TimerClock.js"></script>
                                </span></b>
                                <span><b>|</b></span> <b><span class="label label-default">State:</span> </b>
                                <asp:Label ID="lblState" runat="server" Font-Names="Verdana" Font-Size="Small" ForeColor="White" class="label label-primary label-bs"
                                    Font-Bold="true">0</asp:Label>
                                <span><b>|</b></span> <b><span class="label label-default">County:</span> </b>
                                <asp:Label ID="lblCounty" runat="server" Font-Names="Verdana" Font-Size="Small" ForeColor="White" class="label label-primary label-bs">0</asp:Label>
                                <%--<span><b>|</b></span>--%> <b><span class="label label-default">Process:</span></b>
                                <asp:Label ID="lblProcess" runat="server" Font-Names="Verdana" Font-Size="Small" class="label label-primary label-bs"
                                    ForeColor="White" Font-Bold="True">0</asp:Label>
                                <%-- --%>          
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4>


                                <b><span class="label label-default">Product Type:</span> </b>
                                <asp:Label ID="txtProType" runat="server" Font-Names="Verdana" Font-Size="Small" class="label label-primary label-bs"
                                    ForeColor="White" Font-Bold="True">0</asp:Label>

                                <b><span class="label label-default">Order Type:</span> </b><span><b>
                                    <asp:Label ID="ddlOrderType" runat="server" Font-Names="Verdana" Font-Size="Small" class="label label-danger label-xsmall"
                                        ForeColor="White" Font-Bold="True">0</asp:Label>
                                </b></span>
                                <b><span class="label label-default">Client Type:</span></b> <span><b>
                                    <asp:Label ID="lblprotiertype" runat="server" Font-Names="Verdana" Font-Size="Small" class="label label-danger label-xsmall"
                                        ForeColor="White" Font-Bold="True">0</asp:Label>
                                </b></span>
                                <b><span class="label label-default">Client Code:</span></b> <span><b>
                                    <asp:Label ID="lblClntAccNo" runat="server" Font-Names="Verdana" Font-Size="Small" class="label label-danger label-xsmall"
                                        ForeColor="White" Font-Bold="True">0</asp:Label>
                                </b></span>
                                <b><span class="label label-default">Loan Amount:</span></b> <span><b>
                                    <asp:Label ID="lblloanamount" runat="server" Font-Names="Verdana" Font-Size="Small" class="label label-danger label-xsmall"
                                        ForeColor="White" Font-Bold="True">0</asp:Label>
                                </b></span>
                                <b><span class="label label-default">Fees:</span></b> <span><b>
                                    <asp:Label ID="lblfees" runat="server" Font-Names="Verdana" Font-Size="Small" class="label label-danger label-xsmall"
                                        ForeColor="White" Font-Bold="True">0</asp:Label>
                                </b></span>
                            </h4>
                        </td>

                    </tr>
                </table>
                <%--<div id="errMsg"></div>--%>
                <table width="100%" align="center" id="divresware" runat="server" visible="true" style="padding-left: -10px;">
                    <tr>
                        <td>
                            <asp:Button Text="General" BorderStyle="Ridge" ID="Tab1" class="nav nav-tabs" CssClass="Initial" runat="server"
                                OnClick="Tab1_Click" />
                            <asp:Button Text="Deed" BorderStyle="Ridge" ID="Tab2" CssClass="Initial" runat="server"
                                OnClick="Tab2_Click" />
                            <asp:Button Text="Taxes" BorderStyle="Ridge" ID="Tab3" CssClass="Initial" runat="server"
                                OnClick="Tab3_Click" />
                            <asp:Button Text="Lien/Requirement" BorderStyle="Ridge" ID="Tab4" CssClass="Initial" runat="server"
                                OnClick="Tab4_Click" />
                            <div id="lblMesg" runat="server" class="btn-default" style="font-weight: bold; color: red;"></div>
                            <asp:MultiView ID="MainView" runat="server">
                                <%-- <asp:View ID="View1" runat="server">
                                    <table style="width: 100%; height: 450px; overflow: scroll; border-width: 1px; border-color: #666; border-style: solid; padding-top: 100px;">
                                        <tr>
                                            <td style="padding-left: 5px;">
                                                <label for="txtgeneffdate">Commit Effective Date:</label>

                                                <input runat="server" class="form-control" name="txtgeneffdate" id="txtgeneffdate" type="text" style="height: 30px; width: 160px;" maxlength="10" placeholder="MM/DD/YYYY" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                            </td>
                                            <td>
                                                <label for="txtgeninterest">Interest</label>
                                                <select runat="server" id="txtgeninterest" name="txtgeninterest" class="form-control" style="height: 30px; width: 150px;">
                                                    <option value="FEE SIMPLE">FEE SIMPLE</option>

                                                </select>
                                            </td>
                                            <td>
                                                <label for="txtgenvesting">Vesting:</label>
                                                <textarea runat="server" class="form-control" id="txtgenvesting" rows="1" style="width: 950px;"></textarea>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding-left: 5px;">
                                                <label for="txtgenlegal">Legal</label>
                                                <textarea runat="server" class="form-control" rows="8" id="txtgenlegal" value="legal"></textarea>

                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding-left: 500px; padding-top: 10px;">
                                                <asp:Button runat="server" ID="btnsavegeneral" class="btn btn-warning" Text="Add" OnClick="btnsavegeneral_onclick" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding-top: 5px;">
                                                <div style="padding-top: 10px; padding-left: 5px; height: 250px;">
                                                    <asp:Panel ID="PanelGeneral" runat="server" ScrollBars="Both" Height="200">
                                                        <asp:GridView ID="gridgeneral" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="id" CssClass="table table-bordered table-striped" OnRowCancelingEdit="gridgeneral_RowCancelingEdit" OnRowCommand="gridgeneral_RowCommand" OnRowDeleting="gridgeneral_RowDeleting" OnRowEditing="gridgeneral_RowEditing" OnRowUpdating="gridgeneral_RowUpdating">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl.No">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex+1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CommEffdate" HeaderText="EffectiveDate" />
                                                                <asp:BoundField DataField="Interest" HeaderText="Interest" />
                                                                <asp:BoundField DataField="Vesting" HeaderText="Vesting" />
                                                                <asp:BoundField DataField="Legal" HeaderText="Legal" />
                                                                <asp:TemplateField HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LnkEdit" runat="server" aria-hidden="true" ToolTip="Edit" class="glyphicon glyphicon-edit" Text="" CommandArgument='<%# Eval("id") %>'
                                                                            OnClientClick="javascript : return confirm('Are you sure, want to edit this Row?');"
                                                                            CommandName="Edit"></asp:LinkButton>
                                                                      
                                                                        <asp:LinkButton ID="LnkCancel" Text="" runat="server" ToolTip="Cancel" class="glyphicon glyphicon-remove" CommandName="Cancel" Visible="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LnkDelete" runat="server" ToolTip="Delete" class="glyphicon glyphicon-trash" CommandArgument='<%# Eval("id") %>'
                                                                            OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');"
                                                                            CommandName="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>

                                    </table>

                                </asp:View>
                                <asp:View ID="View2" runat="server">
                                    <table style="width: 100%; height: 450px; overflow: scroll; border-width: 1px; border-color: #666; border-style: solid">
                                        <tr>
                                            <td colspan="3" style="padding-left: 5px;">

                                                <label for="ex2">Deed Type         </label>
                                                <select runat="server" id="txtdeedtype" name="txtdeedtype" class="form-control" style="height: 30px;">
                                                </select>
                                            </td>
                                            <td colspan="5">
                                                <label for="txtotherdeedtype">Other Deed Type:</label>
                                                <input runat="server" class="form-control" id="txtotherdeedtype" style="width: 500px;" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="padding-left: 5px;">
                                                <label for="txtchaingrantor">Grantor:</label>
                                                <textarea runat="server" class="form-control" rows="1" id="txtchaingrantor"></textarea></td>
                                            <td colspan="4">
                                                <label for="txtchaingrantee">Grantees</label>
                                                <textarea runat="server" class="form-control" rows="1" id="txtchaingrantee"></textarea>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-left: 5px;">
                                                <label for="txtchainbook">Book:</label>
                                                <input runat="server" class="form-control" id="txtchainbook" type="text" name="txtnum" style="width: 150px; height: 30px;" />
                                            </td>
                                            <td>
                                                <label for="txtchainpage">Page:</label>
                                                <input runat="server" class="form-control" id="txtchainpage" type="text" name="txtnum" style="width: 150px; height: 30px;" />
                                            </td>
                                            <td>
                                                <label for="txtchaininst">Instrument:</label>
                                                <input runat="server" class="form-control" id="txtchaininst" type="text" style="width: 150px; height: 30px;" />
                                            </td>
                                            <td>
                                                <label for="txtchainddate">Dated Date:</label>
                                                <input runat="server" class="form-control" name="txtchainddate" id="txtchainddate" type="text" style="width: 150px; height: 30px;" maxlength="10" placeholder="MM/DD/YYYY" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                            </td>
                                            <td>
                                                <label for="txtchainrdate">Recorded  Date:</label>
                                               
                                                <input runat="server" class="form-control" name="txtchainrdate" id="txtchainrdate" type="text" style="width: 150px; height: 30px;" maxlength="10" placeholder="MM/DD/YYYY" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                            </td>
                                            <td>
                                                <label for="txtchaindConsideration">Consideration:</label>
                                                <input runat="server" class="form-control" name="txtchaindConsideration" id="txtchaindConsideration" type="text" style="width: 150px; height: 30px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                            </td>
                                            <td>
                                                <label for="txtchainnotes">Notes</label>
                                                <textarea runat="server" class="form-control" id="txtchainnotes" style="width: 350px; height: 30px;"></textarea></td>
                                            <td style="padding-top: 20px;">
                                                <label for="btnchainadd"></label>
                                                <asp:Button runat="server" ID="btnchainadd" class="btn btn-warning" Text="Add" OnClick="btnchainadd_Click" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="8" style="padding-left: 5px;">
                                                <div style="padding-top: 10px; height: 400px;">
                                                    <asp:Panel ID="Pnlchain" runat="server" ScrollBars="Both" Height="380">
                                                        <asp:GridView ID="Gvchain" runat="server" AutoGenerateColumns="false" DataKeyNames="id , Priority"
                                                            OnPreRender="Gvchain_Prerender" OnRowDeleting="Gvchain_Deleting" OnRowEditing="Gvchain_RowEditing" OnRowUpdating="Gvchain_RowUpdating"
                                                            OnRowCommand="Gvchain_RowCommand" EmptyDataText="No Data Found" EmptyDataRowStyle-HorizontalAlign="Center"
                                                            CssClass="table table-bordered table-striped" OnRowCancelingEdit="Gvchain_RowCancelingEdit" GridLines="Vertical">
                                                            <AlternatingRowStyle BackColor="#dcedf2" />
                                                            <Columns>
                                                     

                                                                <asp:TemplateField HeaderText="Sl.No">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex+1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DeedType" HeaderText="DeedType" />
                                                                <asp:BoundField DataField="Grantors" HeaderText="Grantors" />
                                                                <asp:BoundField DataField="Grantees" HeaderText="Grantees" />
                                                                <asp:BoundField DataField="Book" HeaderText="Book" />
                                                                <asp:BoundField DataField="Page" HeaderText="Page" />
                                                                <asp:BoundField DataField="Instrument" HeaderText="Instrument" />
                                                                <asp:BoundField DataField="Dated" HeaderText="Dated" />
                                                                <asp:BoundField DataField="Recorded" HeaderText="Recorded" />
                                                                <asp:BoundField DataField="Consideration" HeaderText="Consideration" />
                                                                <asp:BoundField DataField="Notes" HeaderText="Notes" />
                                                                <asp:TemplateField HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LnkEdit" runat="server" ToolTip="Edit" aria-hidden="true" class="glyphicon glyphicon-edit" Text="" CommandArgument='<%# Eval("id") %>'
                                                                            OnClientClick="javascript : return confirm('Are you sure, want to edit this Row?');"
                                                                            CommandName="Edit"></asp:LinkButton>
                                                                        <asp:LinkButton ID="LnkCancel" Text="" ToolTip="Cancel" runat="server" CommandName="Cancel" class="glyphicon glyphicon-remove" Visible="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LnkDelete" runat="server" ToolTip="Delete" class="glyphicon glyphicon-trash" CommandArgument='<%# Eval("id") %>'
                                                                            OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');"
                                                                            CommandName="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Up/Down">
                                                                    <ItemTemplate>
                                                                   
                                                                        <asp:Button ID="btnUp" CommandName="Up" ToolTip="UP" Text="&#x25B2;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                        <asp:Button ID="btnDown" CommandName="Down" ToolTip="Down" Text="&#x25BC;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>--%>
                                <asp:View ID="View3" runat="server">
                                    <table style="width: 100%; height: 450px; overflow: scroll; border-width: 1px; border-color: #666; border-style: solid; padding-top: 10px;">

                                        <tr>
                                            <td>

                                                <div style="padding-top: 5px; padding-left: 5px;">
                                                    <table>
                                                        <tr>
                                                            <td>

                                                                <div class="col-md-4 Label" style="white-space: nowrap">
                                                                    <b style="margin-left: -7px;">Tax Type</b>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:DropDownList ID="txttaxtype" runat="server" class="form-control" Style="width: 150px; height: 30px; margin-left: 48px;">
                                                                    </asp:DropDownList>

                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="col-md-4 Label" style="white-space: nowrap;">
                                                                    <b style="margin-left: 33px;">Tax Year</b>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <input runat="server" class="form-control" id="txttaxyear" style="width: 150px; height: 30px; margin-left: 64px;" maxlength="4" />

                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="col-md-4 Label" style="white-space: nowrap;">
                                                                    <b style="margin-left: -6px;">End Year</b>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <input runat="server" class="form-control" id="txtendyear" style="width: 150px; height: 30px; margin-left: 28px;" maxlength="4" />
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="col-md-4 Label" style="white-space: nowrap;">
                                                                    <b>Parcel Id</b>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <input runat="server" class="form-control" id="txtparcelid" style="width: 150px; height: 30px; margin-left: 19px;" maxlength="4" />

                                                                </div>
                                                                <br />
                                                            </td>



                                                        </tr>
                                                        <br />
                                                        <tr>
                                                            <br />
                                                            <td>
                                                                <br />
                                                                <div class="col-md-6 Label" style="white-space: nowrap">
                                                                    <b style="margin-left: -8px;">Remaining Balance</b>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <input runat="server" class="form-control" id="txtrembal" style="width: 150px; height: 30px; margin-left: -7px;" maxlength="4" />

                                                                </div>
                                                            </td>
                                                            <td>
                                                                <br />
                                                                <div class="col-md-6 Label" style="white-space: nowrap">
                                                                    <b style="margin-left: 34px;">Exemption Status</b>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <input runat="server" class="form-control" id="txtexempstatus" style="width: 150px; height: 30px; margin-left: 4px;" maxlength="4" />

                                                                </div>
                                                            </td>
                                                            <td>
                                                                <br />
                                                                <div class="col-md-6 Label" style="white-space: nowrap">
                                                                    <b style="margin-left: -7px;">Delinquent Status</b>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <input runat="server" class="form-control" id="txtdelistatus" style="width: 150px; height: 30px; margin-left: -22px;" maxlength="4" />
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <br />
                                                                <div class="col-md-6 Label" style="white-space: nowrap">
                                                                    <b style="margin-left: -1px;">Order comments</b>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <input runat="server" class="form-control" id="txtordercomm" style="width: 150px; height: 30px; margin-left: -29px;" maxlength="4" />

                                                                </div>
                                                            </td>



                                                        </tr>
                                                        <%--<tr>
                                                            <td>
                                                                <label for="txtrembal">Remaining Balance</label>
                                                                <input runat="server" class="form-control" id="txtrembal" style="width: 150px; height: 30px;" maxlength="4" />
                                                            </td>
                                                            <td>
                                                                <label for="txtexempstatus">Exemption Status</label>
                                                                <input runat="server" class="form-control" id="txtexempstatus" style="width: 150px; height: 30px;" maxlength="4" />
                                                            </td>
                                                            <td>
                                                                <label for="txtdelistatus">Delinquent Status</label>
                                                                <input runat="server" class="form-control" id="txtdelistatus" style="width: 150px; height: 30px;" maxlength="4" />
                                                            </td>
                                                            <td>
                                                                <label for="txtordercomm">Order comments</label>
                                                                <input runat="server" class="form-control" id="txtordercomm" style="width: 150px; height: 30px;" maxlength="4" />
                                                            </td>
                                                          
                                                        </tr>--%>
                                                        <tr>

                                                            <td>
                                                                <br />
                                                                <div class="col-md-6 Label">
                                                                    <b style="margin-left: -9px;">Description</b>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <input runat="server" class="form-control" id="txtdescription" style="width: 150px; height: 30px; margin-left: -7px;" maxlength="4" />
                                                                </div>

                                                            </td>

                                                            <td>
                                                                <br />
                                                                <div class="col-md-6 Label" style="white-space: nowrap">
                                                                    <b style="margin-left: 33px;">Special AssessNo</b>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <input runat="server" class="form-control" id="txtspecassno" style="width: 150px; height: 30px; margin-left: 5px;" maxlength="4" />
                                                                </div>

                                                            </td>
                                                            <td>
                                                                <br />
                                                                <div class="col-md-6 Label" style="white-space: nowrap">
                                                                    <b style="margin-left: -7px;">Installments Paid</b>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <input runat="server" class="form-control" id="txtinstallpaid" style="width: 150px; height: 30px; margin-left: -21px;" maxlength="4" />
                                                                </div>

                                                            </td>
                                                            <td>
                                                                <br />
                                                                <div class="col-md-6 Label">
                                                                    <b>Amount</b>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <input runat="server" class="form-control" id="txtAmount" style="width: 150px; height: 30px; margin-left: -30px;" maxlength="4" />
                                                                </div>

                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <br />
                                                                <div class="col-md-6 Label" style="white-space: nowrap">
                                                                    <b style="margin-left: -6px;">Payment Frequency</b>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList runat="server" ID="txtpaymentfrequency" name="txtpaymentfrequency" class="form-control" Style="width: 150px; height: 30px; margin-left: -7px;" OnSelectedIndexChanged="txtpaymentfrequency_SelectedIndexChanged" AutoPostBack="true">

                                                                        <asp:ListItem>Annual</asp:ListItem>
                                                                        <asp:ListItem>Semiannual</asp:ListItem>
                                                                        <asp:ListItem>Quarterly</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>


                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="padding-top: 10px; padding-left: 5px;">
                                                    <table id="tabletax1" runat="server">
                                                        <tr>
                                                            <td>

                                                                <label for="txttaxstatus1" style="padding-left:5px;">Tax Status</label>
                                                                <input runat="server" class="form-control" id="txttaxstatus1" type="text" placeholder="Tax Status" style="width: 150px; height: 30px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                 
                                                            <td>
                                                                <label for="txttaxamount1" style="padding-left:5px;">Tax Amount</label>
                                                                <input runat="server" class="form-control" id="txttaxamount1" type="text" placeholder="Tax Amount" style="width: 150px; height: 30px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                            <td>
                                                                <label for="txtdisamount1" style="padding-left:5px;">Discount Amount</label>
                                                                <input runat="server" class="form-control" id="txtdisamount1" type="text" placeholder="Discount Amount" style="width: 150px; height: 30px;" />
                                                            </td>
                                                            <td>
                                                                <label for="txtamountpaid1" style="padding-left:5px;">Amount Paid</label>
                                                                <input runat="server" class="form-control" id="txtamountpaid1" type="text" placeholder="Amount Paid" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                                <label for="txtrembalance1" style="padding-left:5px;">Remaining Balance</label>
                                                                <input runat="server" class="form-control" id="txtrembalance1" type="text" placeholder="Remaining Balance" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                                <label for="txtexempstatus1" style="padding-left:5px;">Exemption Status</label>
                                                                <input runat="server" class="form-control" id="txtexempstatus1" type="text" placeholder="Exemption Status" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>

                                                            <td style="padding-left: 50px; padding-top: 20px;">&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                    <table id="tabletax2" runat="server" visible="false">
                                                        <tr>
                                                            <td>

<%--                                                            <label for="txttaxstatus2">Tax Status</label>--%>
                                                                <input runat="server" class="form-control" id="txttaxstatus2" type="text" placeholder="Tax Status" style="width: 150px; height: 30px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                            <td>
                                                                <%--<label for="txttaxamount2">Tax Amount</label>--%>
                                                                <input runat="server" class="form-control" id="txttaxamount2" type="text" placeholder="Tax Amount" style="width: 150px; height: 30px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                            <td>
                                                                <%--<label for="txtdisamount2">Discount Amount</label>--%>
                                                                <input runat="server" class="form-control" id="txtdisamount2" type="text" placeholder="Discount Amount" style="width: 150px; height: 30px;" />
                                                            </td>
                                                            <td>
                                                                <%--<label for="txtamountpaid2">Amount Paid</label>--%>
                                                                <input runat="server" class="form-control" id="txtamountpaid2" type="text" placeholder="Amount Paid" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                               <%-- <label for="txtrembalance2">Remaining Balance</label>--%>
                                                                <input runat="server" class="form-control" id="txtrembalance2" type="text" placeholder="Remaining Balance" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                                <%--<label for="txtexempstatus2">Exemption Status</label>--%>
                                                                <input runat="server" class="form-control" id="txtexempstatus2" type="text" placeholder="Exemption Status" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table id="tabletax3" runat="server" visible="false">
                                                        <tr>
                                                              <td>

                                                               <%-- <label for="txttaxstatus3">Tax Status</label>--%>
                                                                <input runat="server" class="form-control" id="txttaxstatus3" type="text" placeholder="Tax Status" style="width: 150px; height: 30px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                            <td>
                                                              <%--  <label for="txttaxamount3">Tax Amount</label>--%>
                                                                <input runat="server" class="form-control" id="txttaxamount3" type="text" placeholder="Tax Amount" style="width: 150px; height: 30px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                            <td>
                                                                <%--<label for="txtdisamount3">Discount Amount</label>--%>
                                                                <input runat="server" class="form-control" id="txtdisamount3" type="text" placeholder="Discount Amount" style="width: 150px; height: 30px;" />
                                                            </td>
                                                            <td>
                                                                <%--<label for="txtamountpaid3">Amount Paid</label>--%>
                                                                <input runat="server" class="form-control" id="txtamountpaid3" type="text" placeholder="Amount Paid" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                               <%-- <label for="txtrembalance3">Remaining Balance</label>--%>
                                                                <input runat="server" class="form-control" id="txtrembalance3" type="text" placeholder="Remaining Balance" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                               <%-- <label for="txtexempstatus3">Exemption Status</label>--%>
                                                                <input runat="server" class="form-control" id="txtexempstatus3" type="text" placeholder="Exemption Status" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tabletax4" runat="server" visible="false">
                                                        <tr>
                                                              <td>

                                                              
                                                                <input runat="server" class="form-control" id="txttaxstatus4" type="text" placeholder="Tax Status" style="width: 150px; height: 30px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                            <td>
                                                               
                                                                <input runat="server" class="form-control" id="txttaxamount4" type="text" placeholder="Tax Amount" style="width: 150px; height: 30px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                            <td>
                                                               
                                                                <input runat="server" class="form-control" id="txtdisamount4" type="text" placeholder="Discount Amount" style="width: 150px; height: 30px;" />
                                                            </td>
                                                            <td>
                                                               
                                                                <input runat="server" class="form-control" id="txtamountpaid4" type="text" placeholder="Amount Paid" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                               
                                                                <input runat="server" class="form-control" id="txtrembalance4" type="text" placeholder="Remaining Balance" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                                
                                                                <input runat="server" class="form-control" id="txtexempstatus4" type="text" placeholder="Exemption Status" style="width: 150px; height: 30px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <br />

                                                    <table id="tbldeliquentstatus" runat="server" cellspacing="1" cellpadding="1">
                                                           <thead style="background-color:#d9241b; color:#fff;">
                                                               <tr>
                                                               <th style="padding-left:7px;">Delinquent Tax Year</th>
                                                                   <th style="padding-left:25px;">Payoff Amount</th>
                                                                   <th style="padding-left:21px;">Payoff Good Thru Date</th>
                                                                   <th style="padding-left:23px;">Initial Installment Due Date</th>
                                                                   <th style="padding-left:22px;">Not Applicable</th>
                                                                   <th style="padding-left:23px;">Date of Tax Sale</th>
                                                                   <th style="padding-left:21px;">Last Day To Redeem</th>
                                                               </tr>
                                                                   </thead>
                                                        <tbody>
                                                        <tr style="padding:25px;">
                                                            
                                                            <td>
                                                                <input  runat="server" class="form-control" id="txtdelitaxstatus" type="text" placeholder="Tax Status" style="width: 150px; height: 30px;border-spacing: 50px 0;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                            <td>
                                                                <input runat="server" class="form-control" id="txtpayoffamount" type="text" placeholder="Tax Amount" style="width: 150px; height: 30px;border-spacing: 50px 0;margin-left:17px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                            </td>
                                                            <td>
                                                                <input runat="server" class="form-control" id="txtpayoffgood" type="text" placeholder="Discount Amount" style="width: 170px; height: 30px;border-spacing: 50px 0;margin-left:19px;" />
                                                            </td>
                                                            <td>
                                                                <input runat="server" class="form-control" id="txtinitialinstall" type="text" placeholder="Amount Paid" style="width: 204px; height: 30px;border-spacing: 50px 0;margin-left:18px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                                <input runat="server" class="form-control" id="txtnotapplicable" type="text" placeholder="Remaining Balance" style="width: 150px; height: 30px;border-spacing: 50px 0;margin-left:17px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                                <input runat="server" class="form-control" id="txtdatetaxsale" type="text" placeholder="Exemption Status" style="width: 150px; height: 30px;border-spacing: 50px 0;margin-left:18px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                            <td>
                                                                <input runat="server" class="form-control" id="txtlastdayred" type="text" placeholder="Exemption Status" style="width: 150px; height: 30px;border-spacing: 50px 0;margin-left:18px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                            </td>
                                                        </tr>
                                                            </tbody>
                                                    </table>
                                                    <table id="tblnew" runat="server">
                                                        <tr>
                                                            <td>

                                                                <input runat="server" class="form-control" name="txttaxAssYear" id="txttaxAssYear" type="text" style="width: 150px; height: 30px;" visible="false" />
                                                            </td>
                                                            <td>

                                                                <input runat="server" class="form-control" name="txttaxproptype" id="txttaxproptype" type="text" style="width: 150px; height: 30px;" visible="false" />
                                                            </td>
                                                            <td>

                                                                <input runat="server" class="form-control" name="txttaxtornsabs" id="txttaxtornsabs" type="text" style="width: 150px; height: 30px;" visible="false" />
                                                            </td>
                                                            <td>

                                                                <input runat="server" class="form-control" name="txttaxscndinsYear" id="txttaxscndinsYear" type="text" style="width: 150px; height: 30px;" visible="false" />
                                                            </td>
                                                            <td>

                                                                <input runat="server" class="form-control" name="txttaxthrdinsYear" id="txttaxthrdinsYear" type="text" style="width: 150px; height: 30px;" visible="false" />
                                                            </td>
                                                            <td>

                                                                <input runat="server" class="form-control" name="txttaxfrthinsYear" id="txttaxfrthinsYear" type="text" style="width: 150px; height: 30px;" visible="false" />
                                                            </td>
                                                            <td style="padding-left: 50px; padding-top: 20px;" rowspan="2">
                                                                <asp:Button runat="server" ID="btntaxadd" class="btn btn-warning" Text="Add" OnClick="btntaxadd_Click" />

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">

                                                                <input runat="server" class="form-control" name="txttaxprdelcmnts" id="txttaxprdelcmnts" type="text" style="width: 500px; height: 30px;" visible="false" />
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="padding-left: 5px; padding-top: 10px;">
                                                    <asp:Panel ID="PanelTax" runat="server" ScrollBars="Both" Height="180">
                                                        <asp:GridView ID="Gvtax" runat="server" AutoGenerateColumns="false" DataKeyNames="id,Priority"
                                                            OnPreRender="Gvtax_PreRender" OnRowDeleting="Gvtax_RowDeleting" OnRowEditing="Gvtax_RowEditing" OnRowUpdating="Gvtax_RowUpdating"
                                                            OnRowCommand="Gvtax_RowCommand" EmptyDataText="No Data Found" EmptyDataRowStyle-HorizontalAlign="Center"
                                                            CssClass="table table-bordered table-striped" Width="100%" OnRowCancelingEdit="Gvtax_RowCancelingEdit">
                                                            <AlternatingRowStyle BackColor="#dcedf2" />
                                                            <Columns>
                                                                <%-- <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="ChkSelectAll" runat="server" ToolTip="Select All" 
                                                                    AutoPostBack="true" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkItem" runat="server" />
                                                                <asp:HiddenField ID="HIDEdit" runat="server" Value='<%# Eval("I_ID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="Sl.No">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex+1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="TaxTypeName" HeaderText="TaxTypeName" />
                                                                <asp:BoundField DataField="Year" HeaderText="Year" />
                                                                <asp:BoundField DataField="TaxingEntity" HeaderText="TaxingEntity" />
                                                                <%--<asp:BoundField DataField="TaxingEntityPhone" HeaderText="Phone" />
                                                        <asp:BoundField DataField="TaxingEntityStreet1" HeaderText="Street1" />
                                                        <asp:BoundField DataField="TaxingEntityStreet2" HeaderText="Street2" />
                                                        <asp:BoundField DataField="TaxingEntityCity" HeaderText="City" />
                                                        <asp:BoundField DataField="TaxingEntityState" HeaderText="State" />
                                                        <asp:BoundField DataField="TaxingEntityZipCode" HeaderText="ZipCode" />--%>
                                                                <asp:BoundField DataField="TotalAnnualTax" HeaderText="TotalAnnualTax" />
                                                                <asp:BoundField DataField="TaxIDNumber" HeaderText="TaxIDNumber" />
                                                                <%--   <asp:BoundField DataField="TaxIDNumberFurtherDescribed" HeaderText="FurtherDescribed" />
                                                        <asp:BoundField DataField="StateIDNumber" HeaderText="StateIDNumber" />
                                                        <asp:BoundField DataField="Land" HeaderText="Land" />
                                                        <asp:BoundField DataField="Improvements" HeaderText="Improvements" />
                                                        <asp:BoundField DataField="ExemptionMortgage" HeaderText="ExemMortgage" />
                                                        <asp:BoundField DataField="ExemptionHomeowners" HeaderText="ExemHomeowners" />
                                                        <asp:BoundField DataField="ExemptionHomesteadSupplemental" HeaderText="ExemSupplemental" />
                                                        <asp:BoundField DataField="ExemptionAdditional" HeaderText="ExemAdditional" />--%>
                                                                <asp:BoundField DataField="Notes" HeaderText="Notes" />
                                                                <asp:BoundField DataField="PaymentFrequencyTypeName" HeaderText="PaymentFrequency" />
                                                                <%-- <asp:BoundField DataField="FirstInstallment" HeaderText="FirstInstallment" />
                                                        <asp:BoundField DataField="FirstDueDate" HeaderText="FirstDueDate" />
                                                        <asp:BoundField DataField="FirstDelinquentDate" HeaderText="FirstDelinquentDate" />
                                                        <asp:BoundField DataField="FirstTaxesOutDate" HeaderText="FirstTaxesOutDate" />
                                                        <asp:BoundField DataField="FirstDiscountDate" HeaderText="FirstDiscountDate" />
                                                        <asp:BoundField DataField="FirstGoodthroughDate" HeaderText="FirstGoodthroughDate" /> 
                                                        <asp:BoundField DataField="FirstPartiallyPaidAmount" HeaderText="FirstPartiallyPaidAmount" />
                                                        <asp:BoundField DataField="FirstPartiallyPaid" HeaderText="FirstPartiallyPaid" />
                                                        <asp:BoundField DataField="FirstPaid" HeaderText="FirstPaid" />
                                                        <asp:BoundField DataField="FirstDue" HeaderText="FirstDue" />
                                                        <asp:BoundField DataField="FirstDelinquent" HeaderText="FirstDelinquent" />--%>

                                                                <%--<asp:BoundField DataField="SecondInstallment" HeaderText="SecondInstallment" />
                                                        <asp:BoundField DataField="SecondDueDate" HeaderText="SecondDueDate" />
                                                        <asp:BoundField DataField="SecondDelinquentDate" HeaderText="SecondDelinquentDate" />
                                                        <asp:BoundField DataField="SecondTaxesOutDate" HeaderText="SecondTaxesOutDate" />
                                                        <asp:BoundField DataField="SecondDiscountDate" HeaderText="SecondDiscountDate" />
                                                        <asp:BoundField DataField="SecondGoodthroughDate" HeaderText="SecondGoodthroughDate" />                                                   
                                                        <asp:BoundField DataField="SecondPartiallyPaidAmount" HeaderText="SecondPartiallyPaidAmount" />
                                                        <asp:BoundField DataField="SecondPaid" HeaderText="SecondPaid" />
                                                        <asp:BoundField DataField="SecondDue" HeaderText="SecondDue" />
                                                        <asp:BoundField DataField="SecondDelinquent" HeaderText="SecondDelinquent" />
                                                         <asp:BoundField DataField="SecondPartiallyPaid" HeaderText="SecondPartiallyPaid" />

                                                        <asp:BoundField DataField="ThirdInstallment" HeaderText="ThirdInstallment" />
                                                        <asp:BoundField DataField="ThirdDueDate" HeaderText="ThirdDueDate" />
                                                        <asp:BoundField DataField="ThirdDelinquentDate" HeaderText="ThirdDelinquentDate" />
                                                        <asp:BoundField DataField="ThirdTaxesOutDate" HeaderText="ThirdTaxesOutDate" />
                                                        <asp:BoundField DataField="ThirdDiscountDate" HeaderText="ThirdDiscountDate" />
                                                        <asp:BoundField DataField="ThirdGoodthroughDate" HeaderText="ThirdGoodthroughDate" />                                                       
                                                        <asp:BoundField DataField="ThirdPartiallyPaidAmount" HeaderText="ThirdPartiallyPaidAmount" />
                                                        <asp:BoundField DataField="ThirdPaid" HeaderText="ThirdPaid" />
                                                        <asp:BoundField DataField="ThirdDue" HeaderText="ThirdDue" />
                                                        <asp:BoundField DataField="ThirdDelinquent" HeaderText="ThirdDelinquent" />
                                                        <asp:BoundField DataField="ThirdPartiallyPaid" HeaderText="ThirdPartiallyPaid" />

                                                        <asp:BoundField DataField="FourthInstallment" HeaderText="FourthInstallment" />
                                                        <asp:BoundField DataField="FourthDueDate" HeaderText="FourthDueDate" />
                                                        <asp:BoundField DataField="FourthDelinquentDate" HeaderText="FourthDelinquentDate" />
                                                        <asp:BoundField DataField="FourthTaxesOutDate" HeaderText="FourthTaxesOutDate" />
                                                        <asp:BoundField DataField="FourthDiscountDate" HeaderText="FourthDiscountDate" />
                                                        <asp:BoundField DataField="FourthGoodthroughDate" HeaderText="FourthGoodthroughDate" />                                
                                                        <asp:BoundField DataField="FourthPartiallyPaidAmount" HeaderText="FourthPartiallyPaidAmount" />
                                                         <asp:BoundField DataField="FourthPartiallyPaid" HeaderText="FourthPartiallyPaid" />
                                                        <asp:BoundField DataField="FourthPaid" HeaderText="FourthPaid" />
                                                        <asp:BoundField DataField="FourthDue" HeaderText="FourthDue" />
                                                        <asp:BoundField DataField="FourthDelinquent" HeaderText="FourthDelinquent" />--%>
                                                                <asp:TemplateField HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LnkEdit" runat="server" ToolTip="Edit" class="glyphicon glyphicon-edit" Text="" CommandArgument='<%# Eval("id") %>'
                                                                            OnClientClick="javascript : return confirm('Are you sure, want to edit this Row?');"
                                                                            CommandName="Edit"></asp:LinkButton>
                                                                        <asp:LinkButton ID="LnkCancel" Text="" ToolTip="Cancel" runat="server" class="glyphicon glyphicon-remove" CommandName="Cancel" Visible="false" />
                                                                    </ItemTemplate>
                                                                    <%--<EditItemTemplate>
                               
                                
                            </EditItemTemplate>--%>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LnkDelete" runat="server" ToolTip="Delete" class="glyphicon glyphicon-trash" CommandArgument='<%# Eval("id") %>'
                                                                            OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');"
                                                                            CommandName="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Up/Down">
                                                                    <ItemTemplate>
                                                                        <%--<asp:Button ID="btnUp" CommandName="Up" ToolTip="UP" Text="&uArr;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
<asp:Button ID="btnDown" CommandName="Down" ToolTip="Down" Text="&dArr;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                                                        <asp:Button ID="btnUp" CommandName="Up" ToolTip="UP" Text="&#x25B2;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                        <asp:Button ID="btnDown" CommandName="Down" ToolTip="Down" Text="&#x25BC;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </asp:View>
                                <%--<asp:View ID="View4" runat="server">
                                    <div>
                                        <table style="width: 100%; height: 450px; overflow: scroll; padding-top: 10px; border-width: 1px; border-color: #666; border-style: solid;">
                                            <tr>
                                                <td>
                                                    <div style="padding-top: -2px; padding-left: 5PX;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <label>Lien/Requirement</label>
                                                                    <select runat="server" id="txtlienreq" class="form-control" style="width: 130px; height: 30px; padding-top: -100PX;" data-toggle="tooltip" data-placement="top" title="Lien/Requirement!" disabled="disabled">
                                                                        <option value="Lien">Lien</option>
                                                                      
                                                                    </select>
                                                                </td>
                                                                <td>
                                                                    <label>TypeName</label>
                                                                   
                                                                    <asp:DropDownList ID="txtlientype" runat="server" class="form-control" Style="width: 350px; height: 30px;"></asp:DropDownList>

                                                                </td>
                                                                <td>
                                                                    <label>DocumentType:</label>
                                                                    <input runat="server" class="form-control" id="txtliendocumentype" style="width: 300px; height: 30px;" data-toggle="tooltip" data-placement="top" />
                                                                </td>
                                                                <td>
                                                                    <label>Date</label>
                                                                    <input runat="server" class="form-control" id="txtliendate" style="width: 130px; height: 25px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxephone">RecordDate:</label>
                                                                    <input runat="server" class="form-control" id="txtlienrecordeddate" type="text" style="width: 130px; height: 25px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxestreet1">Book:</label>
                                                                    <input runat="server" class="form-control" id="txtlienbook" type="text" style="width: 100px; height: 25px;" data-toggle="tooltip" data-placement="top" />
                                                                </td>
                                                                <td>
                                                                    <label for="txtlienbook">Page:</label>
                                                                    <input runat="server" class="form-control" id="txtlienpage" type="text" style="width: 100px; height: 25px;" data-toggle="tooltip" data-placement="top" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxecity">Liber:</label>
                                                                    <input runat="server" class="form-control" id="txtlienliber" type="text" style="width: 120px; height: 25px;" data-toggle="tooltip" data-placement="top" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="padding-top: -2px; padding-left: 5PX;">
                                                        <table>
                                                            <tr>

                                                                <td>
                                                                    <label for="txttaxestate">Volume:</label>
                                                                    <input runat="server" class="form-control" id="txtlienvolume" type="text" style="width: 100px; height: 25px;" visible="false" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxeszip">Instrument:</label>
                                                                    <input runat="server" class="form-control" id="txtlieninstrument" type="text" style="width: 150px; height: 25px;" visible="false" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxtotanntax">Amount:</label>
                                                                    <input runat="server" class="form-control" id="txtlienamount" type="text" style="width: 150px; height: 25px;" onfocus="removeCommas(this)" onblur="addCommas(this)" visible="false"/>
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxidnumber">Holder:</label>
                                                                    <input runat="server" class="form-control" id="txtlienholder" type="text" style="width: 300px; height: 25px;" visible="false"/>
                                                                </td>
                                                                <td>
                                                                    <label>Against:</label>
                                                                    <input runat="server" class="form-control" id="txtlienagainst" type="text" style="width: 300px; height: 25px;" visible="false"/>
                                                                </td>

                                                                <td>
                                                                    <label for="txttaxLand">State:</label>
                                                                    <input runat="server" class="form-control" id="txtlienstate" type="text" style="width: 100px; height: 25px;" visible="false"/>
                                                                </td>
                                                                <td>
                                                                    <label for="txttaximprovement">StateDistrict:</label>
                                                                    <input runat="server" class="form-control" id="txtlienstatedistrict" type="text" style="width: 100px; height: 25px;" visible="false"/>
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexmortgage">County:</label>
                                                                    <input runat="server" class="form-control" id="txtliencounty" type="text" style="width: 160px; height: 25px;" visible="false"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="padding-top: -2px; padding-left: 5PX;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <label for="txttaxexhome">InFavorOf:</label>
                                                                    <input runat="server" class="form-control" id="txtlieninfavourof" type="text" style="width: 250px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexhomesup">CourtDistrict:</label>
                                                                    <input runat="server" class="form-control" id="txtliencourtdistrict" type="text" style="width: 140px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexhomeadd">CourtType:</label>
                                                                    <input runat="server" class="form-control" id="txtliencourttype" type="text" style="width: 140px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxnotes">Trustee:</label>
                                                                    <input runat="server" class="form-control" id="txtlientrustee" type="text" style="width: 290px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaximprovement">Assignor:</label>
                                                                    <input runat="server" class="form-control" id="txtlienassignor" type="text" style="width: 290px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexmortgage">Assignee:</label>
                                                                    <input runat="server" class="form-control" id="txtlienassignee" type="text" style="width: 250px; height: 25px;" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <div style="padding-top: -2px; padding-left: 5PX;">
                                                        <table>
                                                            <tr>

                                                                <td>
                                                                    <label for="txttaxexhome">AssigBook:</label>
                                                                    <input runat="server" class="form-control" id="txtlienassignbook" type="text" style="width: 100px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexhomesup">AssigPage:</label>
                                                                    <input runat="server" class="form-control" id="txtlienassignpage" type="text" style="width: 100px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexhomeadd">AssigLiber:</label>
                                                                    <input runat="server" class="form-control" id="txtlienassignliber" type="text" style="width: 100px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxnotes">AssigVolume:</label>
                                                                    <input runat="server" class="form-control" id="txtlienassignvolume" type="text" style="width: 100px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaximprovement">AssigInstrument:</label>
                                                                    <input runat="server" class="form-control" id="txtlienassigninstrument" type="text" style="width: 150px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexmortgage">DocumentName:</label>
                                                                    <input runat="server" class="form-control" id="txtliendocumentname" type="text" style="width: 220px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexhome">Purpose:</label>
                                                                    <input runat="server" class="form-control" id="txtlienpurpose" type="text" style="width: 200px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexhomesup">Grantor:</label>
                                                                    <input runat="server" class="form-control" id="txtliengrantor" type="text" style="width: 390px; height: 25px;" />
                                                                </td>


                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="padding-top: -2px; padding-left: 5PX;">
                                                        <table>
                                                            <tr>

                                                                <td>
                                                                    <label for="txttaxexhomeadd">Grantee:</label>
                                                                    <input runat="server" class="form-control" id="txtliengrantee" type="text" style="width: 450px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxnotes">Endorsements:</label>
                                                                    <input runat="server" class="form-control" id="txtlienendorsements" type="text" style="width: 100px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaximprovement">CaseNumber:</label>
                                                                    <input runat="server" class="form-control" id="txtliencasenumber" type="text" style="width: 150px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label>TaxYears:</label>
                                                                    <input runat="server" class="form-control" id="txtlientaxyear" type="text" style="width: 100px; height: 25px; padding-top: 5px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexhome">InstallmentNumber:</label>
                                                                    <input runat="server" class="form-control" id="txtlieninstallmentno" type="text" style="width: 150px; height: 25px;" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexhomesup">InstallmentAmount:</label>
                                                                    <input runat="server" class="form-control" id="txtlieninstallmentamount" type="text" style="width: 150px; height: 25px;" onfocus="removeCommas(this)" onblur="addCommas(this)" />
                                                                </td>
                                                                <td>
                                                                    <label for="txttaxexhomeadd">MaturityDate:</label>
                                                                    <input runat="server" class="form-control" id="txtlienmaturitydate" type="text" style="width: 100px; height: 25px;" maxlength="10" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                                                                </td>
     
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <label for="ddllienOpnClsd">Open/Closed:</label>
                                                                    <select runat="server" id="ddllienOpnClsd" class="form-control" style="width: 130px; height: 30px;">
                                                                        <option value="Select">Select</option>
                                                                        <option value="CLOSED">CLOSED</option>
                                                                        <option value="OPEN">OPEN</option>
                                                                    </select>
                                                                </td>
                                                                <td>
                                                                    <label for="ddllienSubLienyesno">Sub. Lien(Yes/No):</label>
                                                                    <select runat="server" id="ddllienSubLienyesno" class="form-control" style="width: 130px; height: 30px;">
                                                                        <option value="Select">Select</option>
                                                                        <option value="YES">YES</option>
                                                                        <option value="NO">NO</option>
                                                                    </select>
                                                                </td>
                                                                <td colspan="3">
                                                                    <label for="txtliennotes">Notes:</label>
                                                                 
                                                                    <asp:TextBox runat="server" ID="txtliennotes" Style="width: 600px; height: 40px;" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <label for="ddllienDocSubs">Deed Divisions:</label>
                                                                    <select runat="server" id="ddllienDocSubs" class="form-control" style="width: 130px; height: 30px;">
                                                                        <option value="Select">Select</option>
                                                                        <option value="Security">Security</option>
                                                                        <option value="Related">Related</option>
                                                                        <option value="InVoluntary">InVoluntary</option>
                                                                        <option value="Others">Others</option>
                                                                    </select>
                                                                </td>
                                                                <td style="padding-top: 20px; padding-left: 20px;">
                                                                    <asp:Button ID="btnlienadd" runat="server" class="btn btn-warning" OnClick="btnlienadd_Click" Text="Add" />
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                
                                                    <div style="padding-top: 10px; padding-left: 5px;">
                                                        <asp:Panel ID="PanelLien" runat="server" ScrollBars="Both" Height="300">
                                                            <asp:GridView ID="GvLien" runat="server" AutoGenerateColumns="false" DataKeyNames="id,Priority"
                                                                OnPreRender="GvLien_PreRender" OnRowDeleting="GvLien_RowDeleting" OnRowEditing="GvLien_RowEditing" OnRowUpdating="GvLien_RowUpdating"
                                                                OnRowCommand="GvLien_RowCommand" EmptyDataText="No Data Found" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                CssClass="table table-bordered table-striped" Width="100%" OnRowCancelingEdit="GvLien_RowCancelingEdit">
                                                                <AlternatingRowStyle BackColor="#dcedf2" />
                                                                <Columns>
                                                                
                                                                    <asp:TemplateField HeaderText="Sl.No">
                                                                        <ItemTemplate>
                                                                            <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Lien_Requirement" HeaderText="Lien_Requirement" />
                                                                    <asp:BoundField DataField="TypeName" HeaderText="TypeName" />
                                                                    <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" />
                                                                    <asp:BoundField DataField="Date" HeaderText="Date" />
                                                                    <asp:BoundField DataField="RecordedDate" HeaderText="RecordedDate" />
                                                                    <asp:BoundField DataField="Book" HeaderText="Book" />
                                                                    <asp:BoundField DataField="Page" HeaderText="Page" />
                                                                    <asp:BoundField DataField="Liber" HeaderText="Liber" />
                                                                    <asp:BoundField DataField="Volume" HeaderText="Volume" />
                                                                    <asp:BoundField DataField="Instrument" HeaderText="Instrument" />

                                                                    <asp:TemplateField HeaderText="Edit">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LnkEdit" runat="server" ToolTip="Edit" class="glyphicon glyphicon-edit" Text="" CommandArgument='<%# Eval("id") %>'
                                                                                OnClientClick="javascript : return confirm('Are you sure, want to edit this Row?');"
                                                                                CommandName="Edit"></asp:LinkButton>
                                                                            <asp:LinkButton ID="LnkCancel" Text="" ToolTip="Cancel" class="glyphicon glyphicon-remove" runat="server" CommandName="Cancel" Visible="false" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LnkDelete" runat="server" ToolTip="Delete" class="glyphicon glyphicon-trash" CommandArgument='<%# Eval("id") %>'
                                                                                OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');"
                                                                                CommandName="Delete"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Up/Down">
                                                                        <ItemTemplate>
                                                                    
                                                                            <asp:Button ID="btnUp" CommandName="Up" ToolTip="UP" Text="&#x25B2;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                            <asp:Button ID="btnDown" CommandName="Down" ToolTip="Down" Text="&#x25BC;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>



                                </asp:View>--%>
                            </asp:MultiView>
                        </td>
                    </tr>
                </table>
                <%-- <table id="tblgeneralkey" runat="server" visible="false" style="width: 80%; border-width: 1px; border-color: #666; border-style: solid; padding-top: 100px;">
                    <tr>
                        <td>
                            <div class="form-group">
                                <label class="control-label">Buyer Name:</label>
                                <input type="text" class="form-control" id="txtBrwrName" runat="server" style="width: 250px;" />
                            </div>
                        </td>

                        <td>
                            <div class="form-group">
                                <label class="control-label">Seller Name:</label>
                                <input type="text" class="form-control" id="txtsellerName" runat="server" style="width: 250px;" />
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <label class="control-label">Property Address:</label>
                                <input type="text" class="form-control" id="txtPropAdrs" runat="server" style="width: 400px;" />
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <label class="control-label">Page Count:</label>
                                <input type="text" class="form-control" id="txtpagecount" runat="server" style="width: 100px;" />
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <label class="control-label">Tax:</label>
                                <asp:DropDownList ID="ddltax" runat="server" class="form-control" AutoPostBack="true" Style="width: 100px;" OnSelectedIndexChanged="ddltax_SelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                    <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                    <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="form-group" runat="server" id="divhideforabsdate">
                                <label for="txtgensearchdate">Search Date:</label>
                                <input runat="server" class="form-control" name="txtgensearchdate" id="txtgensearchdate" type="text" style="height: 30px; width: 160px;" maxlength="10" placeholder="MM/DD/YYYY" onkeypress="return  checkEnterDate(event)" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" />
                            </div>
                        </td>
                        <td>
                            <div class="form-group" runat="server" id="divhideforabsname">
                                <label for="txtgennamesrchd">Name(s) Searched:</label>
                                <input runat="server" class="form-control" id="txtgennamesrchd" type="text" name="txtgennamesrchd" style="width: 150px; height: 30px;" />
                            </div>
                        </td>
                        <td>
                            <div class="form-group" runat="server" id="divhideforabsadrs">
                                <label for="txtgenadrsprclsrchd">Address/Parcel Searched:</label>
                                <input runat="server" class="form-control" id="txtgenadrsprclsrchd" type="text" name="txtgenadrsprclsrchd" style="width: 200px; height: 30px;" />
                            </div>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>--%>
                <%--    <asp:Panel ID="panelpackage" runat="server" Visible="false">
                    <table style="width: 700px; height: 300px">
                        <tr>
                            <td>
                                <asp:Label ID="lblfilepath" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>

                                <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" class="btn-bs-file btn btn-primary" />
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="AddPackage" CssClass="sbutton" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ListBox ID="lstpackage" runat="server" Height="118px" Width="506px" CssClass="textbox"></asp:ListBox></td>
                        </tr>
                        <tr>

                            <td colspan="2">
                                <asp:TextBox ID="Richpackage" runat="server" TextMode="MultiLine" Width="497px" Height="54px" CssClass="textbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnupload" runat="server" Text="Upload" class="btn btn-info" OnClick="btnupload_Click" />
                            </td>
                        </tr>
                    </table>


                </asp:Panel>--%>
                <div style="vertical-align: middle; align-items: center; padding-left: 500PX; padding-top: 20px;">
                    <table>
                        <%--<tr>
                            <td>
                                <h2>
                                    <button type="button" id="btnordersave" runat="server" class="btn btn-success" data-toggle="modal" data-target="#myModal" data-title="Save">Savee</button>
                                    <h4 class="modal-title">Modal Window</h4>
                                </h2>
                            </td>
                        </tr>--%>
                    </table>
                </div>
                <br />
                
                <%-- <div class="modal-footer">

                                <asp:Button runat="server" class="btn btn-primary" ID="btnsave" Text="Save" OnClick="btnsave_Click" />
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                            </div>--%>
            </div>

        </div>
        </div>
        <div class="modal-body">

                    <div class="form-group">
                        <label class="control-label">Comments:</label>
                        <input type="text" class="form-control" id="txtcommnets" runat="server" />
                    </div>
                    <div class="form-group">
                        <label for="message-text" class="control-label">Status:</label>
                        <select class="form-control" id="ddlstatus" runat="server"></select>
                    </div>
                </div>
                <div id="myModallogout" class="modal fade">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Logout Message</h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <label class="control-label">Comments:</label>
                                    <input type="text" class="form-control" id="txtlogoutcmts" name="txtlogoutcmts" runat="server" />
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="lbllogouterror" runat="server" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:Label>

                            </div>
                            <%--<div class="modal-footer">

                                <asp:Button runat="server" class="btn btn-primary" ID="btnlogoutsave" Text="Save" OnClick="btnlogoutsave_Click" />
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                            </div>--%>
                        </div>
                    </div>
                </div>
        <div id="myModalresware" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Search Data Resware</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="control-label">Do you want upload data to resware?</label>
                        </div>
                    </div>
                    <%--<div class="modal-footer">
                                <asp:Button runat="server" class="btn btn-primary" ID="btnreswareok" Text="Confirm" OnClick="btnreswareok_Click" />
                                <asp:Button runat="server" class="btn btn-danger" ID="btnreswarecancel" Text="Cancel" OnClick="btnreswarecancel_Click" />
                            </div>--%>
                </div>
            </div>
        </div>
        <%--  show code--%>
        <div class="modal fade" id="myModaldatares">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Resware Search Data Result</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblMessage" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <%--  <button type="button" class="btn btn-default" data-dismiss="modal">
                                Close</button>--%>
                        <%--<asp:Button ID="btnclosedatares" runat="server" class="btn btn-primary" Text="Close" OnClick="btnclosedatares_Click" />--%>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

        <%-- next order --%>
        <div class="modal fade" id="myModalnextorder" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Next Order</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="Label1" runat="server">Do you want to proceed next order?</asp:Label>
                    </div>
                    <%-- <div class="modal-footer">
                                <asp:Button ID="btnnextok" class="btn btn-primary" runat="server" Text="OK" OnClick="btnnextok_Click" />
                                <asp:Button ID="btnnextcancel" class="btn btn-danger" runat="server" Text="Cancel" OnClick="btnnextcancel_Click" />
                            </div>--%>
                </div>
            </div>
        </div>

        <%-- Error --%>
        <%-- <div class="modal fade" id="myModalError" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Error</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="lblProdErr" runat="server"></asp:Label>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnerrorok" class="btn btn-primary" runat="server" Text="OK" />
                              
                            </div>
                        </div>
                    </div>
                </div>--%>

        <%-- TaxMpdel --%>
        <div class="modal fade" id="myModalTax" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Tax Available</h4>
                    </div>
                    <div class="modal-body">
                        <table style="width: 500px; height: 300px">

                            <tr>
                                <td>Inquriy Subject:<asp:TextBox ID="txtinquriysub" runat="server" Height="50px" TextMode="SingleLine"
                                    Width="550px" class="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Inquriy Body:&nbsp;&nbsp;
                                                <asp:TextBox ID="txtinquriybody" runat="server" Height="50px" TextMode="MultiLine"
                                                    Width="550px" Style="margin-right: 5px" class="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="visibility: hidden;">
                                <td>Status:&nbsp;&nbsp;<asp:TextBox ID="txtinquriystatus" runat="server" TextMode="MultiLine" Width="550px" Height="50px" Font-Bold="True" class="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%-- <div class="modal-footer">
                                <asp:Button ID="btnsendinquriy" runat="server" Text="Upload Inquriy"
                                    OnClick="btnsendinquriy_Click" class="btn btn-info" />
                              
                            </div>--%>
                </div>
            </div>
        </div>

        <%-- Action Event Result --%>
        <div class="modal fade" id="myModalActionResult">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Resware Action Event Result</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblactionmsg" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal">
                            Close</button>

                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        </div>
        </div>
        <input type="hidden" id="theInput" value="<%=Session["TimePro"]%>" />
        <button type="button" style="display: none;" id="btnShowPopup" class="btn btn-primary btn-lg"
            data-toggle="modal" data-target="#myModaldatares">
            Launch demo modal</button>
        <%--<script type="text/javascript" lang="javascript">
            show_TickerTime();
        </script>--%>
        <script type="text/javascript">
            function ShowPopup() {
                $("#btnShowPopup").click();
            }
        </script>
    </form>
</body>
</html>


