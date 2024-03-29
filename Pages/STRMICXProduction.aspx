﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="STRMICXProduction.aspx.cs" Inherits="Pages_STRMICXProduction" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PRODUCTION</title>
    <meta charset="utf-8" />

    <link rel="icon" href="../images/favicon.ico" type="image/x-icon" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link href="../Script/Bootstrap.min.css" rel="stylesheet" />
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css" />
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="../Script/Jquery-1.8.3-jquery.min.js"></script>
    <script type="text/javascript">

        function InstallmentRemaining() {
            var noIns = document.getElementById('txtnoinstall').value;
            var InsPaid = document.getElementById('txtinstallpaid').value;

            if (noIns != "" && InsPaid != "") {
                var blnce = noIns - InsPaid;
                document.getElementById('txtInstallRemain').value = blnce;
            }
        }

        function setTwoNumberDecimal(el) {
            el.value = parseFloat(el.value).toFixed(2);
            if (el.value == "NaN") {
                el.value = "0.00";
            }
        };


        function futureYear(txtDate) {
            var current = txtDate.value;
            var today = new Date();
            var yyyy = today.getFullYear();
            if (current > yyyy) {
                document.getElementById('txtdelitaxyear').value = "";
                setTimeout(function () { document.getElementById("txtdelitaxyear").focus(); }, 1);
                //document.getElementById('txtdelitaxyear').focus();
                alert("Future year not allowed");
                return;
            }
        }

        function functionTaxBill(txtDate) {

            var ddlTaxBill = document.getElementById("paymentfrequency");
            var txBill = ddlTaxBill.options[ddlTaxBill.selectedIndex].innerHTML;

            var today = new Date();
            var dd = today.getDate();

            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }

            if (mm < 10) {
                mm = '0' + mm;
            }

            today = mm + '/' + dd + '/' + yyyy;
            if (txBill == "Annual") {
                var dt = document.getElementById("delinq1").value;
                if (Date.parse(dt) >= Date.parse(today)) {
                    document.getElementById("taxbill").selectedIndex = 1;

                }
                else if (Date.parse(dt) <= Date.parse(today)) {
                    document.getElementById("taxbill").selectedIndex = 2;
                }
                else {
                    document.getElementById("taxbill").selectedIndex = 0;
                }
            }
            else if (txBill == "SemiAnnual") {
                var dt = document.getElementById("delinq2").value;
                if (Date.parse(dt) >= Date.parse(today)) {
                    document.getElementById("taxbill").selectedIndex = 1;

                }
                else if (Date.parse(dt) <= Date.parse(today)) {
                    document.getElementById("taxbill").selectedIndex = 2;
                }
                else {
                    document.getElementById("taxbill").selectedIndex = 0;
                }

            }
            else if (txBill == "TriAnnual") {
                var dt = document.getElementById("delinq3").value;
                if (Date.parse(dt) >= Date.parse(today)) {
                    document.getElementById("taxbill").selectedIndex = 1;

                }
                else if (Date.parse(dt) <= Date.parse(today)) {
                    document.getElementById("taxbill").selectedIndex = 2;
                }
                else {
                    document.getElementById("taxbill").selectedIndex = 0;
                }
            }
            else if (txBill == "Quarterly") {
                var dt = document.getElementById("delinq4").value;
                if (Date.parse(dt) >= Date.parse(today)) {
                    document.getElementById("taxbill").selectedIndex = 1;

                }
                else if (Date.parse(dt) <= Date.parse(today)) {
                    document.getElementById("taxbill").selectedIndex = 2;
                }
                else {
                    document.getElementById("taxbill").selectedIndex = 0;
                }
            }


        }

        function samedate1(val) {
            var inst1 = document.getElementById("instdate1").value;
            var delinq1 = document.getElementById("delinq1").value;

            if (delinq1 == inst1) {
                document.getElementById('delinq1').value = "";
                setTimeout(function () { document.getElementById("delinq1").focus(); }, 1);
                //document.getElementById("delinq1").focus();
                alert('Delinquent date and installment date should not be same...');
                return;
            }
        }

        function samedate2(val) {
            var inst2 = document.getElementById("instdate2").value;
            var delinq2 = document.getElementById("delinq2").value;

            if (delinq2 == inst2) {
                document.getElementById('delinq2').value = "";
                setTimeout(function () { document.getElementById("delinq2").focus(); }, 1);
                //document.getElementById("delinq2").focus();
                alert('Delinquent date and installment date should not be same...');
                return;
            }
        }

        function samedate3(val) {
            var inst3 = document.getElementById("instdate3").value;
            var delinq3 = document.getElementById("delinq3").value;

            if (delinq3 == inst3) {
                document.getElementById('delinq3').value = "";
                setTimeout(function () { document.getElementById("delinq3").focus(); }, 1);
                //document.getElementById("delinq3").focus();
                alert('Delinquent date and installment date should not be same...');
                return;
            }
        }

        function samedate4(val) {
            var inst4 = document.getElementById("instdate4").value;
            var delinq4 = document.getElementById("delinq4").value;

            if (delinq4 == inst4) {
                document.getElementById('delinq4').value = "";
                setTimeout(function () { document.getElementById("delinq4").focus(); }, 1);
                //document.getElementById("delinq4").focus();
                alert('Delinquent date and installment date should not be same...');
                return;
            }
        }

        function futuresamedate1(val) {
            var inst1 = document.getElementById("txtmaninstdate1").value;
            var delinq1 = document.getElementById("txtmandeliqdate1").value;

            if (delinq1 == inst1) {
                document.getElementById('txtmandeliqdate1').value = "";
                setTimeout(function () { document.getElementById("txtmandeliqdate1").focus(); }, 1);
                //document.getElementById("txtmandeliqdate1").focus();
                alert('Delinquent date and installment date should not be same...');
                return;
            }
        }

        function futuresamedate2(val) {
            var inst2 = document.getElementById("txtmaninstdate2").value;
            var delinq2 = document.getElementById("txtmandeliqdate2").value;

            if (delinq2 == inst2) {
                document.getElementById('txtmandeliqdate2').value = "";
                setTimeout(function () { document.getElementById("txtmandeliqdate2").focus(); }, 1);
                //document.getElementById("txtmandeliqdate2").focus();
                alert('Delinquent date and installment date should not be same...');
                return;
            }
        }

        function futuresamedate3(val) {
            var inst3 = document.getElementById("txtmaninstdate3").value;
            var delinq3 = document.getElementById("txtmandeliqdate3").value;

            if (delinq3 == inst3) {
                document.getElementById('txtmandeliqdate3').value = "";
                setTimeout(function () { document.getElementById("txtmandeliqdate3").focus(); }, 1);
                //document.getElementById("txtmandeliqdate3").focus();
                alert('Delinquent date and installment date should not be same...');
                return;
            }
        }

        function futuresamedate4(val) {
            var inst4 = document.getElementById("txtmaninstdate4").value;
            var delinq4 = document.getElementById("txtmandeliqdate4").value;

            if (delinq4 == inst4) {
                document.getElementById('txtmandeliqdate4').value = "";
                setTimeout(function () { document.getElementById("txtmandeliqdate4").focus(); }, 1);
                //document.getElementById("txtmandeliqdate4").focus();
                alert('Delinquent date and installment date should not be same...');
                return;
            }
        }

        function discountdate1(val) {
            var delinq1 = document.getElementById("delinq1").value;
            var disc1 = document.getElementById("discdate1").value;
            if (disc1 == delinq1) {
                document.getElementById('discdate1').value = "";
                setTimeout(function () { document.getElementById("discdate1").focus(); }, 1);
                //document.getElementById("discdate1").focus();
                alert('Discount Date and Delinquent Date should not be same...');
                return;
            }
        }

        function discountdate2(val) {
            var delinq2 = document.getElementById("delinq2").value;
            var disc2 = document.getElementById("discdate2").value;
            if (disc2 == delinq2) {
                document.getElementById('discdate2').value = "";
                setTimeout(function () { document.getElementById("discdate2").focus(); }, 1);
                //document.getElementById("discdate2").focus();
                alert('Discount Date and Delinquent Date should not be same...');
                return;
            }
        }

        function discountdate3(val) {
            var delinq3 = document.getElementById("delinq3").value;
            var disc3 = document.getElementById("discdate3").value;
            if (disc3 == delinq3) {
                document.getElementById('discdate3').value = "";
                setTimeout(function () { document.getElementById("discdate2").focus(); }, 1);
                //document.getElementById("discdate3").focus();
                alert('Discount Date and Delinquent Date should not be same...');
                return;
            }
        }

        function discountdate4(val) {
            var delinq4 = document.getElementById("delinq4").value;
            var disc4 = document.getElementById("discdate4").value;
            if (disc4 == delinq4) {
                document.getElementById('discdate4').value = "";
                setTimeout(function () { document.getElementById("discdate4").focus(); }, 1);
                //document.getElementById("discdate4").focus();
                alert('Discount Date and Delinquent Date should not be same...');
                return;
            }
        }

        function futurdiscountdate1(val) {
            var futdelinq1 = document.getElementById("txtmandeliqdate1").value;
            var futdisc1 = document.getElementById("txtmandisdate1").value;
            if (futdisc1 == futdelinq1) {
                document.getElementById('txtmandisdate1').value = "";
                setTimeout(function () { document.getElementById("txtmandisdate1").focus(); }, 1);
                //document.getElementById("txtmandisdate1").focus();
                alert('Discount Date and Delinquent Date should not be same...');
                return;
            }
        }

        function futurdiscountdate2(val) {
            var futdelinq2 = document.getElementById("txtmandeliqdate2").value;
            var futdisc2 = document.getElementById("txtmandisdate2").value;
            if (futdisc2 == futdelinq2) {
                document.getElementById('txtmandisdate2').value = "";
                setTimeout(function () { document.getElementById("txtmandisdate2").focus(); }, 1);
                //document.getElementById("txtmandisdate2").focus();
                alert('Discount Date and Delinquent Date should not be same...');
                return;
            }
        }

        function futurdiscountdate3(val) {
            var futdelinq3 = document.getElementById("txtmandeliqdate3").value;
            var futdisc3 = document.getElementById("txtmandisdate3").value;
            if (futdisc3 == futdelinq3) {
                document.getElementById('txtmandisdate3').value = "";
                setTimeout(function () { document.getElementById("txtmandisdate3").focus(); }, 1);
                //document.getElementById("txtmandisdate3").focus();
                alert('Discount Date and Delinquent Date should not be same...');
                return;
            }
        }

        function futurdiscountdate4(val) {
            var futdelinq4 = document.getElementById("txtmandeliqdate4").value;
            var futdisc4 = document.getElementById("txtmandisdate4").value;
            if (futdisc4 == futdelinq4) {
                document.getElementById('txtmandisdate4').value = "";
                setTimeout(function () { document.getElementById("txtmandisdate4").focus(); }, 1);
                //document.getElementById("txtmandisdate4").focus();
                alert('Discount Date and Delinquent Date should not be same...');
                return;
            }
        }

        function functionpayemtfrequency(ddlPay) {
            // var payfre = document.getElementById("paymentfrequency").value;
            var payfre = ddlPay.options[ddlPay.selectedIndex].innerHTML;
            //alert(payfre);
            var Ann = "", Semi = "", Tri = "", Qua = "";
            if (payfre == "Annual") {
                Ann = "Annual";
            }
            if (payfre == "SemiAnnual") {
                Semi = "SemiAnnual";
            }
            if (payfre == "Quarterly") {
                Qua = "Quarterly";
            }
            if (payfre == "TriAnnual") {
                Tri = "TriAnnual";
            }
            if (Ann == "Annual") {
                document.getElementById("instamount1").disabled = false;
                document.getElementById("instamountpaid1").disabled = false;
                document.getElementById("instpaiddue1").disabled = false;
                document.getElementById("remainingbalance1").disabled = false;
                document.getElementById("instdate1").disabled = false;
                document.getElementById("delinq1").disabled = false;
                document.getElementById("discamt1").disabled = false;
                document.getElementById("discdate1").disabled = false;
                document.getElementById("exemptrelevy1").disabled = false;

                document.getElementById("instamount2").disabled = true;
                document.getElementById("instamount2").value = "";
                document.getElementById("instamountpaid2").disabled = true;
                document.getElementById("instamountpaid2").value = "";
                document.getElementById("instpaiddue2").disabled = true;
                document.getElementById("instpaiddue2").selectedIndex = 0;
                document.getElementById("remainingbalance2").disabled = true;
                document.getElementById("remainingbalance2").value = "";
                document.getElementById("instdate2").disabled = true;
                document.getElementById("instdate2").value = "";
                document.getElementById("delinq2").disabled = true;
                document.getElementById("delinq2").value = "";
                document.getElementById("discamt2").disabled = true;
                document.getElementById("discamt2").value = "";
                document.getElementById("discdate2").disabled = true;
                document.getElementById("discdate2").value = "";
                document.getElementById("exemptrelevy2").disabled = true;
                document.getElementById("exemptrelevy2").checked = false;

                document.getElementById("instamount3").disabled = true;
                document.getElementById("instamount3").value = "";
                document.getElementById("instamountpaid3").disabled = true;
                document.getElementById("instamountpaid3").value = "";
                document.getElementById("instpaiddue3").disabled = true;
                document.getElementById("instpaiddue3").selectedIndex = 0;
                document.getElementById("remainingbalance3").disabled = true;
                document.getElementById("remainingbalance3").value = "";
                document.getElementById("instdate3").disabled = true;
                document.getElementById("instdate3").value = "";
                document.getElementById("delinq3").disabled = true;
                document.getElementById("delinq3").value = "";
                document.getElementById("discamt3").disabled = true;
                document.getElementById("discamt3").value = "";
                document.getElementById("discdate3").disabled = true;
                document.getElementById("discdate3").value = "";
                document.getElementById("exemptrelevy3").disabled = true;
                document.getElementById("exemptrelevy3").checked = false;

                document.getElementById("instamount4").disabled = true;
                document.getElementById("instamount4").value = "";
                document.getElementById("instamountpaid4").disabled = true;
                document.getElementById("instamountpaid4").value = "";
                document.getElementById("instpaiddue4").disabled = true;
                document.getElementById("instpaiddue4").selectedIndex = 0;
                document.getElementById("remainingbalance4").disabled = true;
                document.getElementById("remainingbalance4").value = "";
                document.getElementById("instdate4").disabled = true;
                document.getElementById("instdate4").value = "";
                document.getElementById("delinq4").disabled = true;
                document.getElementById("delinq4").value = "";
                document.getElementById("discamt4").disabled = true;
                document.getElementById("discamt4").value = "";
                document.getElementById("discdate4").disabled = true;
                document.getElementById("discdate4").value = "";
                document.getElementById("exemptrelevy4").disabled = true;
                document.getElementById("exemptrelevy4").checked = false;

                //document.getElementById("instamount1").value = "0.00";
                //document.getElementById("instamountpaid1").value = "0.00";
                //document.getElementById("remainingbalance1").value = "0.00";
            }

            if (Semi == "SemiAnnual") {

                document.getElementById("instamount1").disabled = false;
                document.getElementById("instamountpaid1").disabled = false;
                document.getElementById("instpaiddue1").disabled = false;
                document.getElementById("remainingbalance1").disabled = false;
                document.getElementById("instdate1").disabled = false;
                document.getElementById("delinq1").disabled = false;
                document.getElementById("discamt1").disabled = false;
                document.getElementById("discdate1").disabled = false;
                document.getElementById("exemptrelevy1").disabled = false;

                document.getElementById("instamount2").disabled = false;
                document.getElementById("instamountpaid2").disabled = false;
                document.getElementById("instpaiddue2").disabled = false;
                document.getElementById("remainingbalance2").disabled = false;
                document.getElementById("instdate2").disabled = false;
                document.getElementById("delinq2").disabled = false;
                document.getElementById("discamt2").disabled = false;
                document.getElementById("discdate2").disabled = false;
                document.getElementById("exemptrelevy2").disabled = false;

                document.getElementById("instamount3").disabled = true;
                document.getElementById("instamount3").value = "";
                document.getElementById("instamountpaid3").disabled = true;
                document.getElementById("instamountpaid3").value = "";
                document.getElementById("instpaiddue3").disabled = true;
                document.getElementById("instpaiddue3").selectedIndex = 0;
                document.getElementById("remainingbalance3").disabled = true;
                document.getElementById("remainingbalance3").value = "";
                document.getElementById("instdate3").disabled = true;
                document.getElementById("instdate3").value = "";
                document.getElementById("delinq3").disabled = true;
                document.getElementById("delinq3").value = "";
                document.getElementById("discamt3").disabled = true;
                document.getElementById("discamt3").value = "";
                document.getElementById("discdate3").disabled = true;
                document.getElementById("discdate3").value = "";
                document.getElementById("exemptrelevy3").disabled = true;
                document.getElementById("exemptrelevy3").checked = false;

                document.getElementById("instamount4").disabled = true;
                document.getElementById("instamount4").value = "";
                document.getElementById("instamountpaid4").disabled = true;
                document.getElementById("instamountpaid4").value = "";
                document.getElementById("instpaiddue4").disabled = true;
                document.getElementById("instpaiddue4").selectedIndex = 0;
                document.getElementById("remainingbalance4").disabled = true;
                document.getElementById("remainingbalance4").value = "";
                document.getElementById("instdate4").disabled = true;
                document.getElementById("instdate4").value = "";
                document.getElementById("delinq4").disabled = true;
                document.getElementById("delinq4").value = "";
                document.getElementById("discamt4").disabled = true;
                document.getElementById("discamt4").value = "";
                document.getElementById("discdate4").disabled = true;
                document.getElementById("discdate4").value = "";
                document.getElementById("exemptrelevy4").disabled = true;
                document.getElementById("exemptrelevy4").checked = false;

                //document.getElementById("instamount1").value = "0.00";
                //document.getElementById("instamountpaid1").value = "0.00";
                //document.getElementById("remainingbalance1").value = "0.00";
                //document.getElementById("instamount2").value = "0.00";
                //document.getElementById("instamountpaid2").value = "0.00";
                //document.getElementById("remainingbalance2").value = "0.00";
            }

            if (Tri == "TriAnnual") {
                document.getElementById("instamount1").disabled = false;
                document.getElementById("instamountpaid1").disabled = false;
                document.getElementById("instpaiddue1").disabled = false;
                document.getElementById("remainingbalance1").disabled = false;
                document.getElementById("instdate1").disabled = false;
                document.getElementById("delinq1").disabled = false;
                document.getElementById("discamt1").disabled = false;
                document.getElementById("discdate1").disabled = false;
                document.getElementById("exemptrelevy1").disabled = false;

                document.getElementById("instamount2").disabled = false;
                document.getElementById("instamountpaid2").disabled = false;
                document.getElementById("instpaiddue2").disabled = false;
                document.getElementById("remainingbalance2").disabled = false;
                document.getElementById("instdate2").disabled = false;
                document.getElementById("delinq2").disabled = false;
                document.getElementById("discamt2").disabled = false;
                document.getElementById("discdate2").disabled = false;
                document.getElementById("exemptrelevy2").disabled = false;

                document.getElementById("instamount3").disabled = false;
                document.getElementById("instamountpaid3").disabled = false;
                document.getElementById("instpaiddue3").disabled = false;
                document.getElementById("remainingbalance3").disabled = false;
                document.getElementById("instdate3").disabled = false;
                document.getElementById("delinq3").disabled = false;
                document.getElementById("discamt3").disabled = false;
                document.getElementById("discdate3").disabled = false;
                document.getElementById("exemptrelevy3").disabled = false;

                document.getElementById("instamount4").disabled = true;
                document.getElementById("instamount4").value = "";
                document.getElementById("instamountpaid4").disabled = true;
                document.getElementById("instamountpaid4").value = "";
                document.getElementById("instpaiddue4").disabled = true;
                document.getElementById("instpaiddue4").selectedIndex = 0;
                document.getElementById("remainingbalance4").disabled = true;
                document.getElementById("remainingbalance4").value = "";
                document.getElementById("instdate4").disabled = true;
                document.getElementById("instdate4").value = "";
                document.getElementById("delinq4").disabled = true;
                document.getElementById("delinq4").value = "";
                document.getElementById("discamt4").disabled = true;
                document.getElementById("discamt4").value = "";
                document.getElementById("discdate4").disabled = true;
                document.getElementById("discdate4").value = "";
                document.getElementById("exemptrelevy4").disabled = true;
                document.getElementById("exemptrelevy4").checked = false;

                //document.getElementById("instamount1").value = "0.00";
                //document.getElementById("instamountpaid1").value = "0.00";
                //document.getElementById("remainingbalance1").value = "0.00";
                //document.getElementById("instamount2").value = "0.00";
                //document.getElementById("instamountpaid2").value = "0.00";
                //document.getElementById("remainingbalance2").value = "0.00";
                //document.getElementById("instamount3").value = "0.00";
                //document.getElementById("instamountpaid3").value = "0.00";
                //document.getElementById("remainingbalance3").value = "0.00";
            }

            if (Qua == "Quarterly") {
                document.getElementById("instamount1").disabled = false;
                document.getElementById("instamountpaid1").disabled = false;
                document.getElementById("instpaiddue1").disabled = false;
                document.getElementById("remainingbalance1").disabled = false;
                document.getElementById("instdate1").disabled = false;
                document.getElementById("delinq1").disabled = false;
                document.getElementById("discamt1").disabled = false;
                document.getElementById("discdate1").disabled = false;
                document.getElementById("exemptrelevy1").disabled = false;

                document.getElementById("instamount2").disabled = false;
                document.getElementById("instamountpaid2").disabled = false;
                document.getElementById("instpaiddue2").disabled = false;
                document.getElementById("remainingbalance2").disabled = false;
                document.getElementById("instdate2").disabled = false;
                document.getElementById("delinq2").disabled = false;
                document.getElementById("discamt2").disabled = false;
                document.getElementById("discdate2").disabled = false;
                document.getElementById("exemptrelevy2").disabled = false;

                document.getElementById("instamount3").disabled = false;
                document.getElementById("instamountpaid3").disabled = false;
                document.getElementById("instpaiddue3").disabled = false;
                document.getElementById("remainingbalance3").disabled = false;
                document.getElementById("instdate3").disabled = false;
                document.getElementById("delinq3").disabled = false;
                document.getElementById("discamt3").disabled = false;
                document.getElementById("discdate3").disabled = false;
                document.getElementById("exemptrelevy3").disabled = false;

                document.getElementById("instamount4").disabled = false;
                document.getElementById("instamountpaid4").disabled = false;
                document.getElementById("instpaiddue4").disabled = false;
                document.getElementById("remainingbalance4").disabled = false;
                document.getElementById("instdate4").disabled = false;
                document.getElementById("delinq4").disabled = false;
                document.getElementById("discamt4").disabled = false;
                document.getElementById("discdate4").disabled = false;
                document.getElementById("exemptrelevy4").disabled = false;

                //document.getElementById("instamount1").value = "0.00";
                //document.getElementById("instamountpaid1").value = "0.00";
                //document.getElementById("remainingbalance1").value = "0.00";
                //document.getElementById("instamount2").value = "0.00";
                //document.getElementById("instamountpaid2").value = "0.00";
                //document.getElementById("remainingbalance2").value = "0.00";
                //document.getElementById("instamount3").value = "0.00";
                //document.getElementById("instamountpaid3").value = "0.00";
                //document.getElementById("remainingbalance3").value = "0.00";
                //document.getElementById("instamount4").value = "0.00";
                //document.getElementById("instamountpaid4").value = "0.00";
                //document.getElementById("remainingbalance4").value = "0.00";
            }
        }


        //paymentfrequency futuretax
        function functionpayemtfrequency1(ddlPay1) {
            // var payfre = document.getElementById("paymentfrequency").value;
            var payfre = ddlPay1.options[ddlPay1.selectedIndex].innerHTML;
            //alert(payfre);
            var Ann = "", Semi = "", Tri = "", Qua = "";
            if (payfre == "Annual") {
                Ann = "Annual";
            }
            if (payfre == "SemiAnnual") {
                Semi = "SemiAnnual";
            }
            if (payfre == "Quarterly") {
                Qua = "Quarterly";
            }
            if (payfre == "TriAnnual") {
                Tri = "TriAnnual";
            }
            if (Ann == "Annual") {
                document.getElementById("instmanamount1").disabled = false;
                document.getElementById("instmanamtpaid1").disabled = false;
                document.getElementById("ddlmaninstpaiddue1").disabled = false;
                document.getElementById("txtmanurembal1").disabled = false;
                document.getElementById("txtmaninstdate1").disabled = false;
                document.getElementById("txtmandeliqdate1").disabled = false;
                document.getElementById("txtmandisamount1").disabled = false;
                document.getElementById("txtmandisdate1").disabled = false;
                document.getElementById("chkexrelmanu1").disabled = false;

                document.getElementById("instmanamount2").disabled = true;
                document.getElementById("instmanamount2").value = "";
                document.getElementById("instmanamtpaid2").disabled = true;
                document.getElementById("instmanamtpaid2").value = "";
                document.getElementById("ddlmaninstpaiddue2").disabled = true;
                document.getElementById("ddlmaninstpaiddue2").selectedIndex = 0;
                document.getElementById("txtmanurembal2").disabled = true;
                document.getElementById("txtmanurembal2").value = "";
                document.getElementById("txtmaninstdate2").disabled = true;
                document.getElementById("txtmaninstdate2").value = "";
                document.getElementById("txtmandeliqdate2").disabled = true;
                document.getElementById("txtmandeliqdate2").value = "";
                document.getElementById("txtmandisamount2").disabled = true;
                document.getElementById("txtmandisamount2").value = "";
                document.getElementById("txtmandisdate2").disabled = true;
                document.getElementById("txtmandisdate2").value = "";
                document.getElementById("chkexrelmanu2").disabled = true;
                document.getElementById("chkexrelmanu2").checked = false;

                document.getElementById("instmanamount3").disabled = true;
                document.getElementById("instmanamount3").value = "";
                document.getElementById("instmanamtpaid3").disabled = true;
                document.getElementById("instmanamtpaid3").value = "";
                document.getElementById("ddlmaninstpaiddue3").disabled = true;
                document.getElementById("ddlmaninstpaiddue3").selectedIndex = 0;
                document.getElementById("txtmanurembal3").disabled = true;
                document.getElementById("txtmanurembal3").value = "";
                document.getElementById("txtmaninstdate3").disabled = true;
                document.getElementById("txtmaninstdate3").value = "";
                document.getElementById("txtmandeliqdate3").disabled = true;
                document.getElementById("txtmandeliqdate3").value = "";
                document.getElementById("txtmandisamount3").disabled = true;
                document.getElementById("txtmandisamount3").value = "";
                document.getElementById("txtmandisdate3").disabled = true;
                document.getElementById("txtmandisdate3").value = "";
                document.getElementById("chkexrelmanu3").disabled = true;
                document.getElementById("chkexrelmanu3").checked = false;

                document.getElementById("instmanamount4").disabled = true;
                document.getElementById("instmanamount4").value = "";
                document.getElementById("instmanamtpaid4").disabled = true;
                document.getElementById("instmanamtpaid4").value = "";
                document.getElementById("ddlmaninstpaiddue4").disabled = true;
                document.getElementById("ddlmaninstpaiddue4").selectedIndex = 0;
                document.getElementById("txtmanurembal4").disabled = true;
                document.getElementById("txtmanurembal4").value = "";
                document.getElementById("txtmaninstdate4").disabled = true;
                document.getElementById("txtmaninstdate4").value = "";
                document.getElementById("txtmandeliqdate4").disabled = true;
                document.getElementById("txtmandeliqdate4").value = "";
                document.getElementById("txtmandisamount4").disabled = true;
                document.getElementById("txtmandisamount4").value = "";
                document.getElementById("txtmandisdate4").disabled = true;
                document.getElementById("txtmandisdate4").value = "";
                document.getElementById("chkexrelmanu4").disabled = true;
                document.getElementById("chkexrelmanu4").checked = false;
            }

            if (Semi == "SemiAnnual") {

                document.getElementById("instmanamount1").disabled = false;
                document.getElementById("instmanamtpaid1").disabled = false;
                document.getElementById("ddlmaninstpaiddue1").disabled = false;
                document.getElementById("txtmanurembal1").disabled = false;
                document.getElementById("txtmaninstdate1").disabled = false;
                document.getElementById("txtmandeliqdate1").disabled = false;
                document.getElementById("txtmandisamount1").disabled = false;
                document.getElementById("txtmandisdate1").disabled = false;
                document.getElementById("chkexrelmanu1").disabled = false;

                document.getElementById("instmanamount2").disabled = false;
                document.getElementById("instmanamtpaid2").disabled = false;
                document.getElementById("ddlmaninstpaiddue2").disabled = false;
                document.getElementById("txtmanurembal2").disabled = false;
                document.getElementById("txtmaninstdate2").disabled = false;
                document.getElementById("txtmandeliqdate2").disabled = false;
                document.getElementById("txtmandisamount2").disabled = false;
                document.getElementById("txtmandisdate2").disabled = false;
                document.getElementById("chkexrelmanu2").disabled = false;

                document.getElementById("instmanamount3").disabled = true;
                document.getElementById("instmanamount3").value = "";
                document.getElementById("instmanamtpaid3").disabled = true;
                document.getElementById("instmanamtpaid3").value = "";
                document.getElementById("ddlmaninstpaiddue3").disabled = true;
                document.getElementById("ddlmaninstpaiddue3").selectedIndex = 0;
                document.getElementById("txtmanurembal3").disabled = true;
                document.getElementById("txtmanurembal3").value = "";
                document.getElementById("txtmaninstdate3").disabled = true;
                document.getElementById("txtmaninstdate3").value = "";
                document.getElementById("txtmandeliqdate3").disabled = true;
                document.getElementById("txtmandeliqdate3").value = "";
                document.getElementById("txtmandisamount3").disabled = true;
                document.getElementById("txtmandisamount3").value = "";
                document.getElementById("txtmandisdate3").disabled = true;
                document.getElementById("txtmandisdate3").value = "";
                document.getElementById("chkexrelmanu3").disabled = true;
                document.getElementById("chkexrelmanu3").checked = false;

                document.getElementById("instmanamount4").disabled = true;
                document.getElementById("instmanamount4").value = "";
                document.getElementById("instmanamtpaid4").disabled = true;
                document.getElementById("instmanamtpaid4").value = "";
                document.getElementById("ddlmaninstpaiddue4").disabled = true;
                document.getElementById("ddlmaninstpaiddue4").selectedIndex = 0;
                document.getElementById("txtmanurembal4").disabled = true;
                document.getElementById("txtmanurembal4").value = "";
                document.getElementById("txtmaninstdate4").disabled = true;
                document.getElementById("txtmaninstdate4").value = "";
                document.getElementById("txtmandeliqdate4").disabled = true;
                document.getElementById("txtmandeliqdate4").value = "";
                document.getElementById("txtmandisamount4").disabled = true;
                document.getElementById("txtmandisamount4").value = "";
                document.getElementById("txtmandisdate4").disabled = true;
                document.getElementById("txtmandisdate4").value = "";
                document.getElementById("chkexrelmanu4").disabled = true;
                document.getElementById("chkexrelmanu4").checked = false;
            }

            if (Tri == "TriAnnual") {
                document.getElementById("instmanamount1").disabled = false;
                document.getElementById("instmanamtpaid1").disabled = false;
                document.getElementById("ddlmaninstpaiddue1").disabled = false;
                document.getElementById("txtmanurembal1").disabled = false;
                document.getElementById("txtmaninstdate1").disabled = false;
                document.getElementById("txtmandeliqdate1").disabled = false;
                document.getElementById("txtmandisamount1").disabled = false;
                document.getElementById("txtmandisdate1").disabled = false;
                document.getElementById("chkexrelmanu1").disabled = false;

                document.getElementById("instmanamount2").disabled = false;
                document.getElementById("instmanamtpaid2").disabled = false;
                document.getElementById("ddlmaninstpaiddue2").disabled = false;
                document.getElementById("txtmanurembal2").disabled = false;
                document.getElementById("txtmaninstdate2").disabled = false;
                document.getElementById("txtmandeliqdate2").disabled = false;
                document.getElementById("txtmandisamount2").disabled = false;
                document.getElementById("txtmandisdate2").disabled = false;
                document.getElementById("chkexrelmanu2").disabled = false;

                document.getElementById("instmanamount3").disabled = false;
                document.getElementById("instmanamtpaid3").disabled = false;
                document.getElementById("ddlmaninstpaiddue3").disabled = false;
                document.getElementById("txtmanurembal3").disabled = false;
                document.getElementById("txtmaninstdate3").disabled = false;
                document.getElementById("txtmandeliqdate3").disabled = false;
                document.getElementById("txtmandisamount3").disabled = false;
                document.getElementById("txtmandisdate3").disabled = false;
                document.getElementById("chkexrelmanu3").disabled = false;

                document.getElementById("instmanamount4").disabled = true;
                document.getElementById("instmanamount4").value = "";
                document.getElementById("instmanamtpaid4").disabled = true;
                document.getElementById("instmanamtpaid4").value = "";
                document.getElementById("ddlmaninstpaiddue4").disabled = true;
                document.getElementById("ddlmaninstpaiddue4").selectedIndex = 0;
                document.getElementById("txtmanurembal4").disabled = true;
                document.getElementById("txtmanurembal4").value = "";
                document.getElementById("txtmaninstdate4").disabled = true;
                document.getElementById("txtmaninstdate4").value = "";
                document.getElementById("txtmandeliqdate4").disabled = true;
                document.getElementById("txtmandeliqdate4").value = "";
                document.getElementById("txtmandisamount4").disabled = true;
                document.getElementById("txtmandisamount4").value = "";
                document.getElementById("txtmandisdate4").disabled = true;
                document.getElementById("txtmandisdate4").value = "";
                document.getElementById("chkexrelmanu4").disabled = true;
                document.getElementById("chkexrelmanu4").checked = false;
            }

            if (Qua == "Quarterly") {
                document.getElementById("instmanamount1").disabled = false;
                document.getElementById("instmanamtpaid1").disabled = false;
                document.getElementById("ddlmaninstpaiddue1").disabled = false;
                document.getElementById("txtmanurembal1").disabled = false;
                document.getElementById("txtmaninstdate1").disabled = false;
                document.getElementById("txtmandeliqdate1").disabled = false;
                document.getElementById("txtmandisamount1").disabled = false;
                document.getElementById("txtmandisdate1").disabled = false;
                document.getElementById("chkexrelmanu1").disabled = false;

                document.getElementById("instmanamount2").disabled = false;
                document.getElementById("instmanamtpaid2").disabled = false;
                document.getElementById("ddlmaninstpaiddue2").disabled = false;
                document.getElementById("txtmanurembal2").disabled = false;
                document.getElementById("txtmaninstdate2").disabled = false;
                document.getElementById("txtmandeliqdate2").disabled = false;
                document.getElementById("txtmandisamount2").disabled = false;
                document.getElementById("txtmandisdate2").disabled = false;
                document.getElementById("chkexrelmanu2").disabled = false;

                document.getElementById("instmanamount3").disabled = false;
                document.getElementById("instmanamtpaid3").disabled = false;
                document.getElementById("ddlmaninstpaiddue3").disabled = false;
                document.getElementById("txtmanurembal3").disabled = false;
                document.getElementById("txtmaninstdate3").disabled = false;
                document.getElementById("txtmandeliqdate3").disabled = false;
                document.getElementById("txtmandisamount3").disabled = false;
                document.getElementById("txtmandisdate3").disabled = false;
                document.getElementById("chkexrelmanu3").disabled = false;

                document.getElementById("instmanamount4").disabled = false;
                document.getElementById("instmanamtpaid4").disabled = false;
                document.getElementById("ddlmaninstpaiddue4").disabled = false;
                document.getElementById("txtmanurembal4").disabled = false;
                document.getElementById("txtmaninstdate4").disabled = false;
                document.getElementById("txtmandeliqdate4").disabled = false;
                document.getElementById("txtmandisamount4").disabled = false;
                document.getElementById("txtmandisdate4").disabled = false;
                document.getElementById("chkexrelmanu4").disabled = false;
            }
        }


        function validateCheckBoxes() {
            var isValid = false;
            var gridView = document.getElementById('<%= gvtaxauthorities.ClientID %>');
            for (var i = 1; i < gridView.rows.length; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            isValid = true;
                            return true;
                        }
                    }
                }
            }
            document.getElementById("lblagency").innerHTML = "Please select atleast one Authority.";
            return false;
        }


        function functionInsttax() {
            var Installmenterror1 = document.getElementById("instdate1").value;
            var Installmenterror2 = document.getElementById("instdate2").value;
            var Installmenterror3 = document.getElementById("instdate3").value;
            var Installmenterror4 = document.getElementById("instdate4").value;

            var instamountpaidnew1 = document.getElementById("instamountpaid1").value;
            var remainingbalancenew1 = document.getElementById("remainingbalance1").value;
            var exemptrelevynew1 = document.getElementById("exemptrelevy1").checked;

            var instamountpaidnew2 = document.getElementById("instamountpaid2").value;
            var remainingbalancenew2 = document.getElementById("remainingbalance2").value;
            var exemptrelevynew2 = document.getElementById("exemptrelevy2").checked;

            var instamountpaidnew3 = document.getElementById("instamountpaid3").value;
            var remainingbalancenew3 = document.getElementById("remainingbalance3").value;
            var exemptrelevynew3 = document.getElementById("exemptrelevy3").checked;

            var instamountpaidnew4 = document.getElementById("instamountpaid4").value;
            var remainingbalancenew4 = document.getElementById("remainingbalance4").value;
            var exemptrelevynew4 = document.getElementById("exemptrelevy4").checked;


            var instamountnew1 = document.getElementById("instamount1").value;
            var instamountnew2 = document.getElementById("instamount2").value;
            var instamountnew3 = document.getElementById("instamount3").value;
            var instamountnew4 = document.getElementById("instamount4").value;

            var delinquentdate1 = document.getElementById("delinq1").value;
            var delinquentdate2 = document.getElementById("delinq2").value;
            var delinquentdate3 = document.getElementById("delinq3").value;
            var delinquentdate4 = document.getElementById("delinq4").value;

            var payfre = document.getElementById("paymentfrequency").value;

            var Ann = "", Semi = "", Tri = "", Qua = "";
            if (payfre == "1") {
                Ann = "Annual";
            }
            if (payfre == "2") {
                Semi = "SemiAnnual";
            }
            if (payfre == "4") {
                Qua = "Quarterly";
            }
            if (payfre == "3") {
                Tri = "TriAnnual";
            }
            if (Ann == "Annual") {
                if (Installmenterror1 == "") {
                    setTimeout(function () { document.getElementById("instdate1").focus(); }, 1);
                    //document.getElementById("instdate1").focus();
                    alert("Installment Date1 should be required");
                    return false;
                }
                else if ((instamountnew1 == "")) {
                    setTimeout(function () { document.getElementById("instamount1").focus(); }, 1);
                    //document.getElementById("instamount1").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew1 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid1").focus(); }, 1);
                    //document.getElementById("instamountpaid1").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate1 == "") {
                    setTimeout(function () { document.getElementById("delinq1").focus(); }, 1);
                    alert("Delinquent Date1 Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew1 == "0.00" && remainingbalancenew1 == "0.00" && exemptrelevynew1 == false) {
                    setTimeout(function () { document.getElementById("instamount1").focus(); }, 1);
                    alert("Installmentamount Paid and remainingbalance Cannot Be Zero");
                    return false;
                }
            }

            if (Semi == "SemiAnnual") {
                if (Installmenterror1 == "") {
                    setTimeout(function () { document.getElementById("instdate1").focus(); }, 1);
                    //document.getElementById("instdate1").focus();
                    alert("Installment1 Date1 should be required");
                    return false;
                }
                else if (Installmenterror2 == "") {
                    setTimeout(function () { document.getElementById("instdate2").focus(); }, 1);
                    //document.getElementById("instdate2").focus();
                    alert("Installment2 Date2 should be required");
                    return false;
                }
                else if ((instamountnew1 == "")) {
                    setTimeout(function () { document.getElementById("instamount1").focus(); }, 1);
                    //document.getElementById("instamount1").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew2 == "") {
                    setTimeout(function () { document.getElementById("instamount2").focus(); }, 1);
                    //document.getElementById("instamount2").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }

                else if (instamountpaidnew1 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid1").focus(); }, 1);
                    //document.getElementById("instamountpaid1").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew2 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid2").focus(); }, 1);
                    //document.getElementById("instamountpaid2").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate1 == "") {
                    setTimeout(function () { document.getElementById("delinq1").focus(); }, 1);
                    alert("Delinquent Date1 Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate2 == "") {
                    setTimeout(function () { document.getElementById("delinq2").focus(); }, 1);
                    alert("Delinquent Date2 Cannot Be Empty");
                    return false;
                }
                else if ((instamountpaidnew1 == "0.00" && remainingbalancenew1 == "0.00" && exemptrelevynew1 == false) || (instamountpaidnew2 == "0.00" && remainingbalancenew2 == "0.00" && exemptrelevynew2 == false)) {
                    setTimeout(function () { document.getElementById("instamount2").focus(); }, 1);
                    alert("Installmentamount Paid and remainingbalance Cannot Be Zero");
                    return false;
                }
            }

            if (Tri == "TriAnnual") {
                if (Installmenterror1 == "") {
                    setTimeout(function () { document.getElementById("instdate1").focus(); }, 1);
                    //document.getElementById("instdate1").focus();
                    alert("Installment Date1 should be required");
                    return false;
                }
                else if (Installmenterror2 == "") {
                    setTimeout(function () { document.getElementById("instdate2").focus(); }, 1);
                    //document.getElementById("instdate2").focus();
                    alert("Installment Date2 should be required");
                    return false;
                }
                else if (Installmenterror3 == "") {
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById("instdate3").focus();
                    alert("Installment Date3 should be required");
                    return false;
                }
                else if ((instamountnew1 == "")) {
                    setTimeout(function () { document.getElementById("instamount1").focus(); }, 1);
                    //document.getElementById("instamount1").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew2 == "") {
                    setTimeout(function () { document.getElementById("instamount2").focus(); }, 1);
                    //document.getElementById("instamount2").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew3 == "") {
                    setTimeout(function () { document.getElementById("instamount3").focus(); }, 1);
                    //document.getElementById("instamount3").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew1 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid1").focus(); }, 1);
                    //document.getElementById("instamountpaid1").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew2 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid2").focus(); }, 1);
                    //document.getElementById("instamountpaid2").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew3 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid3").focus(); }, 1);
                    //document.getElementById("instamountpaid3").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate1 == "") {
                    setTimeout(function () { document.getElementById("delinq1").focus(); }, 1);
                    alert("Delinquent Date1 Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate2 == "") {
                    setTimeout(function () { document.getElementById("delinq2").focus(); }, 1);
                    alert("Delinquent Date2 Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate3 == "") {
                    setTimeout(function () { document.getElementById("delinq3").focus(); }, 1);
                    alert("Delinquent Date3 Cannot Be Empty");
                    return false;
                }
                else if ((instamountpaidnew1 == "0.00" && remainingbalancenew1 == "0.00" && exemptrelevynew1 == false) || (instamountpaidnew2 == "0.00" && remainingbalancenew2 == "0.00" && exemptrelevynew2 == false) || (instamountpaidnew3 == "0.00" && remainingbalancenew3 == "0.00" && exemptrelevynew3 == false)) {
                    setTimeout(function () { document.getElementById("instamount3").focus(); }, 1);
                    alert("Installmentamount Paid and remainingbalance Cannot Be Zero");
                    return false;
                }
            }

            if (Qua == "Quarterly") {
                if (Installmenterror1 == "") {
                    setTimeout(function () { document.getElementById("instdate1").focus(); }, 1);
                    //document.getElementById("instdate1").focus();
                    alert("Installment Date1 should be required");
                    return false;
                }
                else if (Installmenterror2 == "") {
                    setTimeout(function () { document.getElementById("instdate2").focus(); }, 1);
                    //document.getElementById("instdate2").focus();
                    alert("Installment Date2 should be required");
                    return false;
                }
                else if (Installmenterror3 == "") {
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById("instdate3").focus();
                    alert("Installment Date3 should be required");
                    return false;
                }
                else if (Installmenterror4 == "") {
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById("instdate4").focus();
                    alert("Installment Date4 should be required");
                    return false;
                }
                else if ((instamountnew1 == "")) {
                    setTimeout(function () { document.getElementById("instamount1").focus(); }, 1);
                    //document.getElementById("instamount1").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew2 == "") {
                    setTimeout(function () { document.getElementById("instamount2").focus(); }, 1);
                    //document.getElementById("instamount2").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew3 == "") {
                    setTimeout(function () { document.getElementById("instamount3").focus(); }, 1);
                    //document.getElementById("instamount3").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew4 == "") {
                    setTimeout(function () { document.getElementById("instamount4").focus(); }, 1);
                    //document.getElementById("instamount4").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew1 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid1").focus(); }, 1);
                    //document.getElementById("instamountpaid1").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew2 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid2").focus(); }, 1);
                    //document.getElementById("instamountpaid2").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew3 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid3").focus(); }, 1);
                    //document.getElementById("instamountpaid3").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew4 == "") {
                    setTimeout(function () { document.getElementById("instamountpaid4").focus(); }, 1);
                    //document.getElementById("instamountpaid4").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate1 == "") {
                    setTimeout(function () { document.getElementById("delinq1").focus(); }, 1);
                    alert("Delinquent Date1 Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate2 == "") {
                    setTimeout(function () { document.getElementById("delinq2").focus(); }, 1);
                    alert("Delinquent Date2 Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate3 == "") {
                    setTimeout(function () { document.getElementById("delinq3").focus(); }, 1);
                    alert("Delinquent Date3 Cannot Be Empty");
                    return false;
                }
                else if (delinquentdate4 == "") {
                    setTimeout(function () { document.getElementById("delinq4").focus(); }, 1);
                    alert("Delinquent Date4 Cannot Be Empty");
                    return false;
                }
                else if ((instamountpaidnew1 == "0.00" && remainingbalancenew1 == "0.00" && exemptrelevynew1 == false) || (instamountpaidnew2 == "0.00" && remainingbalancenew2 == "0.00" && exemptrelevynew2 == false) || (instamountpaidnew3 == "0.00" && remainingbalancenew3 == "0.00" && exemptrelevynew3 == false) || (instamountpaidnew4 == "0.00" && remainingbalancenew4 == "0.00" && exemptrelevynew4 == false)) {
                    setTimeout(function () { document.getElementById("instamount4").focus(); }, 1);
                    alert("Installmentamount Paid and remainingbalance Cannot Be Zero");
                    return false;
                }
            }

            var Insterror;
            Insterror = document.getElementById("nextbilldate1").value;
            if (Insterror == "") {
                document.getElementById('nextbilldate1').style.borderColor = "#ff0000";
                document.getElementById("lblnextbilldate1").style.color = "#ff0000";
            }
            else if (Insterror != "") {
                document.getElementById('nextbilldate1').style.borderColor = "green";
                document.getElementById('lblnextbilldate1').style.color = "green";
            }
            else {
                document.getElementById('nextbilldate1').style.borderColor = "green";
                document.getElementById('lblnextbilldate1').style.color = "green";
            }

            if (Insterror == '') {
                return false;
            }
            else if (Insterror != '') {
                return true;
            }
        }

        function samedatenextbill(val) {
            var inst1 = document.getElementById("instdate1").value;
            var nextbilldate = document.getElementById("nextbilldate1").value;

            if (nextbilldate == inst1) {
                document.getElementById('nextbilldate1').value = "";
                setTimeout(function () { document.getElementById("nextbilldate1").focus(); }, 1);
                //document.getElementById("nextbilldate1").focus();
                alert('Next bill date1 and installment date should not be same...');
                return;
            }

            var inst2 = document.getElementById("instdate2").value;
            if (nextbilldate == inst2) {
                document.getElementById('nextbilldate1').value = "";
                setTimeout(function () { document.getElementById("nextbilldate1").focus(); }, 1);
                //document.getElementById("nextbilldate1").focus();
                alert('Next bill date1 and installment date should not be same...');
                return;
            }

            var inst3 = document.getElementById("instdate3").value;
            if (nextbilldate == inst3) {
                document.getElementById('nextbilldate1').value = "";
                setTimeout(function () { document.getElementById("nextbilldate1").focus(); }, 1);
                //document.getElementById("nextbilldate1").focus();
                alert('Next bill date1 and installment date should not be same...');
                return;
            }

            var inst4 = document.getElementById("instdate4").value;
            if (nextbilldate == inst4) {
                document.getElementById('nextbilldate1').value = "";
                setTimeout(function () { document.getElementById("nextbilldate1").focus(); }, 1);
                //document.getElementById("nextbilldate1").focus();
                alert('Next bill date1 and installment date should not be same...');
                return;
            }
        }

        function functionfutinst() {
            var Instfuterror, Instfuterror1;
            Instfuterror = document.getElementById("txtmanubillstartdate").value;
            Instfuterror1 = document.getElementById("txtmanubillenddate").value;

            if (Instfuterror == "") {
                setTimeout(function () { document.getElementById("txtmanubillstartdate").focus(); }, 1);
                //document.getElementById("txtmanubillstartdate").focus();
                alert("Please enter Billing Start Date...");
                return false;
            }

            if (Instfuterror1 == "") {
                setTimeout(function () { document.getElementById("txtmanubillenddate").focus(); }, 1);
                //document.getElementById("txtmanubillenddate").focus();
                alert("Please enter Billing End Date...");
                return false;
            }

            var Installmenterror1 = document.getElementById("txtmaninstdate1").value;
            var Installmenterror2 = document.getElementById("txtmaninstdate2").value;
            var Installmenterror3 = document.getElementById("txtmaninstdate3").value;
            var Installmenterror4 = document.getElementById("txtmaninstdate4").value;

            var instamountpaidnew1 = document.getElementById("instmanamtpaid1").value;
            var remainingbalancenew1 = document.getElementById("txtmanurembal1").value;
            var exemptrelevynew1 = document.getElementById("chkexrelmanu1").checked;

            var instamountpaidnew2 = document.getElementById("instmanamtpaid2").value;
            var remainingbalancenew2 = document.getElementById("txtmanurembal2").value;
            var exemptrelevynew2 = document.getElementById("chkexrelmanu2").checked;

            var instamountpaidnew3 = document.getElementById("instmanamtpaid3").value;
            var remainingbalancenew3 = document.getElementById("txtmanurembal3").value;
            var exemptrelevynew3 = document.getElementById("chkexrelmanu3").checked;

            var instamountpaidnew4 = document.getElementById("instmanamtpaid4").value;
            var remainingbalancenew4 = document.getElementById("txtmanurembal4").value;
            var exemptrelevynew4 = document.getElementById("chkexrelmanu4").checked;

            var instamountnew1 = document.getElementById("instmanamount1").value;
            var instamountnew2 = document.getElementById("instmanamount2").value;
            var instamountnew3 = document.getElementById("instmanamount3").value;
            var instamountnew4 = document.getElementById("instmanamount4").value;

            var payfre = document.getElementById("paymentfrequency").value;

            var Ann = "", Semi = "", Tri = "", Qua = "";
            if (payfre == "1") {
                Ann = "Annual";
            }
            if (payfre == "2") {
                Semi = "SemiAnnual";
            }
            if (payfre == "4") {
                Qua = "Quarterly";
            }
            if (payfre == "3") {
                Tri = "TriAnnual";
            }
            if (Ann == "Annual") {
                if (Installmenterror1 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate1").focus(); }, 1);
                    //document.getElementById("txtmaninstdate1").focus();
                    alert("Installment Date1 should be required");
                    return false;
                }
                else if ((instamountnew1 == "")) {
                    setTimeout(function () { document.getElementById("instmanamount1").focus(); }, 1);
                    //document.getElementById("instmanamount1").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew1 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid1").focus(); }, 1);
                    //document.getElementById("instmanamtpaid1").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew1 == "0.00" && remainingbalancenew1 == "0.00" && exemptrelevynew1 == false) {
                    setTimeout(function () { document.getElementById("instmanamount1").focus(); }, 1);
                    alert("Installmentamount Paid and remainingbalance Cannot Be Zero");
                    return false;
                }
            }

            if (Semi == "SemiAnnual") {
                if (Installmenterror1 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate1").focus(); }, 1);
                    //document.getElementById("txtmaninstdate1").focus();
                    alert("Installment1 Date1 should be required");
                    return false;
                }
                else if (Installmenterror2 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate2").focus(); }, 1);
                    //document.getElementById("txtmaninstdate2").focus();
                    alert("Installment2 Date2 should be required");
                    return false;
                }
                else if ((instamountnew1 == "")) {
                    setTimeout(function () { document.getElementById("instmanamount1").focus(); }, 1);
                    //document.getElementById("instmanamount1").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew2 == "") {
                    setTimeout(function () { document.getElementById("instmanamount2").focus(); }, 1);
                    //document.getElementById("instmanamount2").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }

                else if (instamountpaidnew1 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid1").focus(); }, 1);
                    //document.getElementById("instmanamtpaid1").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew2 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid2").focus(); }, 1);
                    //document.getElementById("instmanamtpaid2").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }

                else if ((instamountpaidnew1 == "0.00" && remainingbalancenew1 == "0.00" && exemptrelevynew1 == false) || (instamountpaidnew2 == "0.00" && remainingbalancenew2 == "0.00" && exemptrelevynew2 == false)) {
                    setTimeout(function () { document.getElementById("instmanamount2").focus(); }, 1);
                    alert("Installmentamount Paid and remainingbalance Cannot Be Zero");
                    return false;
                }
            }

            if (Tri == "TriAnnual") {
                if (Installmenterror1 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate1").focus(); }, 1);
                    //document.getElementById("txtmaninstdate1").focus();
                    alert("Installment Date1 should be required");
                    return false;
                }
                else if (Installmenterror2 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate2").focus(); }, 1);
                    //document.getElementById("txtmaninstdate2").focus();
                    alert("Installment Date2 should be required");
                    return false;
                }
                else if (Installmenterror3 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById("txtmaninstdate3").focus();
                    alert("Installment Date3 should be required");
                    return false;
                }
                else if ((instamountnew1 == "")) {
                    setTimeout(function () { document.getElementById("instmanamount1").focus(); }, 1);
                    //document.getElementById("instmanamount1").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew2 == "") {
                    setTimeout(function () { document.getElementById("instmanamount2").focus(); }, 1);
                    //document.getElementById("instmanamount2").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew3 == "") {
                    setTimeout(function () { document.getElementById("instmanamount3").focus(); }, 1);
                    //document.getElementById("instmanamount3").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew1 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid1").focus(); }, 1);
                    //document.getElementById("instmanamtpaid1").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew2 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid2").focus(); }, 1);
                    //document.getElementById("instmanamtpaid2").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew3 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid3").focus(); }, 1);
                    //document.getElementById("instmanamtpaid3").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if ((instamountpaidnew1 == "0.00" && remainingbalancenew1 == "0.00" && exemptrelevynew1 == false) || (instamountpaidnew2 == "0.00" && remainingbalancenew2 == "0.00" && exemptrelevynew2 == false) || (instamountpaidnew3 == "0.00" && remainingbalancenew3 == "0.00" && exemptrelevynew3 == false)) {
                    setTimeout(function () { document.getElementById("instmanamount3").focus(); }, 1);
                    alert("Installmentamount Paid and remainingbalance Cannot Be Zero");
                    return false;
                }
            }

            if (Qua == "Quarterly") {
                if (Installmenterror1 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate1").focus(); }, 1);
                    //document.getElementById("txtmaninstdate1").focus();
                    alert("Installment Date1 should be required");
                    return false;
                }
                else if (Installmenterror2 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate2").focus(); }, 1);
                    //document.getElementById("txtmaninstdate2").focus();
                    alert("Installment Date2 should be required");
                    return false;
                }
                else if (Installmenterror3 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById("txtmaninstdate3").focus();
                    alert("Installment Date3 should be required");
                    return false;
                }
                else if (Installmenterror4 == "") {
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById("txtmaninstdate4").focus();
                    alert("Installment Date4 should be required");
                    return false;
                }
                else if ((instamountnew1 == "")) {
                    setTimeout(function () { document.getElementById("instmanamount1").focus(); }, 1);
                    //document.getElementById("instmanamount1").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew2 == "") {
                    setTimeout(function () { document.getElementById("instmanamount2").focus(); }, 1);
                    //document.getElementById("instmanamount2").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew3 == "") {
                    setTimeout(function () { document.getElementById("instmanamount3").focus(); }, 1);
                    //document.getElementById("instmanamount3").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountnew4 == "") {
                    setTimeout(function () { document.getElementById("instmanamount4").focus(); }, 1);
                    //document.getElementById("instmanamount4").focus();
                    alert("Installment Amount Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew1 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid1").focus(); }, 1);
                    //document.getElementById("instmanamtpaid1").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew2 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid2").focus(); }, 1);
                    //document.getElementById("instmanamtpaid2").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew3 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid3").focus(); }, 1);
                    //document.getElementById("instmanamtpaid3").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if (instamountpaidnew4 == "") {
                    setTimeout(function () { document.getElementById("instmanamtpaid4").focus(); }, 1);
                    //document.getElementById("instmanamtpaid4").focus();
                    alert("Installmentamount Paid Cannot Be Empty");
                    return false;
                }
                else if ((instamountpaidnew1 == "0.00" && remainingbalancenew1 == "0.00" && exemptrelevynew1 == false) || (instamountpaidnew2 == "0.00" && remainingbalancenew2 == "0.00" && exemptrelevynew2 == false) || (instamountpaidnew3 == "0.00" && remainingbalancenew3 == "0.00" && exemptrelevynew3 == false) || (instamountpaidnew4 == "0.00" && remainingbalancenew4 == "0.00" && exemptrelevynew4 == false)) {
                    setTimeout(function () { document.getElementById("instmanamount4").focus(); }, 1);
                    alert("Installmentamount Paid and remaining Balance Cannot Be Zero");
                    return false;
                }
            }
        }

        function userValid() {
            var error;
            error = document.getElementById("ddlordstatus").value;

            if (error == '--Select Status--' || error == undefined) {
                document.getElementById("lbltaxcerterror").innerHTML = "Please Enter The Fields";
                return false;
            }
        }

        //balaji
        function completeorder() {
            var comerror;
            comerror = document.getElementById("ddlstatus").value;

            if (comerror == '--Select--' || comerror == undefined) {
                alert("Please choose any of the status");
                return false;
            }


            var delistatus = document.getElementById("txtdeliquent").value;
            var delirows = gvDeliquentStatus.rows;

            if (delistatus == "Yes") {
                if (delirows.length == 1) {
                    setTimeout(function () { document.getElementById("txtdeliPayee").focus(); }, 1);
                    //document.getElementById('txtdeliPayee').focus();
                    alert("Deliquent Status is required");
                    return false;
                }
            }

            if (delistatus == "Select") {
                alert("Please Choose Any One Option in Deliquency");
                return false;
            }

            var exestatus = document.getElementById("txtexemption").value;
            var exerows = gvExemption.rows;


            if (exestatus == "Yes") {
                if (exerows.length == 1) {
                    setTimeout(function () { document.getElementById("txtexetype").focus(); }, 1);
                    //document.getElementById('txtexetype').focus();
                    alert("Exemption Status is required");
                    return false;
                }
            }

            if (exestatus == "Select") {
                alert("Please Choose Any One Option in Exemption");
                return false;
            }

            var primarystatus = document.getElementById("txtResidence").value;

            if (primarystatus == "Select") {
                alert("Please Choose Any One Option in primary residence");
                return false;
            }


            var specialstatus = document.getElementById("SecialAssmnt").value;
            var specrows = gvSpecial.rows;

            if (specialstatus == "Yes") {
                if (specrows.length == 1) {
                    setTimeout(function () { document.getElementById("txtInstallRemain").focus(); }, 1);
                    //document.getElementById('txtInstallRemain').focus();
                    alert("Special Assessment is required");
                    return false;
                }
            }

            if (specialstatus == "Select") {
                alert("Please Choose Any One Option in Special Assessment");
                return false;
            }

            var clientmame = document.getElementById("lblclientName").value;
            if (clientmame == "ORMS") {
                var priordeli = document.getElementById("pastDeliquent").value;
                var priorrows = GrdPriordelinquent.rows;


                if (priordeli == "Yes") {
                    if (priorrows.length == 1) {
                        setTimeout(function () { document.getElementById("txtpriodeli").focus(); }, 1);
                        //document.getElementById('txtpriodeli').focus();
                        alert("Prior Delinquent is required");
                        return false;
                    }
                }

                if (priordeli == "Select") {
                    alert("Please Choose Any One Option in Prior Delinquent");
                    return false;
                }
            }
        }


        function editfunction() {
            document.getElementById("date1").disabled = false;
            document.getElementById("date2").disabled = false;

            document.getElementById("btneditdates").style.visibility = 'hidden';
            document.getElementById("btnTaxOrderStatus").style.visibility = 'hidden';

            document.getElementById("btnsavedates").style.visibility = "visible";
            document.getElementById("btnsavedates").style.display = 'inline-block';
            document.getElementById("btncanceldates").style.visibility = "visible";
            document.getElementById("btncanceldates").style.display = 'inline-block';
        }

        function Cancelfunction() {
            document.getElementById("btneditdates").style.visibility = "visible";
            document.getElementById("btnTaxOrderStatus").style.visibility = "visible";
            document.getElementById("btnsavedates").style.visibility = "hidden";
            document.getElementById("btncanceldates").style.visibility = "hidden";

            document.getElementById("date1").disabled = true;
            document.getElementById("date2").disabled = true;
        }

        function functionlogouterror() {
            var Logouterror;
            Logouterror = document.getElementById("txtreason").value;

            if (Logouterror == '') {
                document.getElementById("lbllogouterror").innerHTML = "Please Enter The Valid Reason";
                return false;
            }
        }

        function functionaddnoteerror() {
            var addnoteerror;
            addnoteerror = document.getElementById("txtnotes").value;

            if (addnoteerror == '') {
                document.getElementById("lbladdnoteserror").innerHTML = "Please Enter The Notes";
                return false;
            }
        }

        function TaxparcelFunction() {
            var taxparcelerror, taxyearerror, taxdroperror;
            taxparcelerror = document.getElementById("txtdrop").value;
            taxyearerror = document.getElementById("txtTaxYear").value;
            taxdroperror = document.getElementById("txtTaxNo").value;

            var firstLetter = $("#txtTaxYear").val().charAt(0);

            if (taxparcelerror == "") {
                document.getElementById('txtdrop').style.borderColor = "#ff0000";
                document.getElementById("lbldrop").style.color = "#ff0000";
            }
            else if (taxparcelerror != "") {
                document.getElementById('txtdrop').style.borderColor = "green";
                document.getElementById('lbldrop').style.color = "green";
            }
            else {
                document.getElementById('txtdrop').style.borderColor = "green";
                document.getElementById('lbldrop').style.color = "green";
            }


            if (taxparcelerror == "--Select--") {
                document.getElementById('txtdrop').style.borderColor = "#ff0000";
                document.getElementById("lbldrop").style.color = "#ff0000";
            }
            else if (taxparcelerror != "--Select--") {
                document.getElementById('txtdrop').style.borderColor = "green";
                document.getElementById('lbldrop').style.color = "green";
            }
            else {
                document.getElementById('txtdrop').style.borderColor = "green";
                document.getElementById('lbldrop').style.color = "green";
            }


            if (taxyearerror == "") {
                document.getElementById('txtTaxYear').style.borderColor = "#ff0000";
                document.getElementById("lblTaxYear").style.color = "#ff0000";
                var x = document.getElementById("txtTaxYear").pattern;
            }
            else if (taxyearerror != "") {
                document.getElementById('txtTaxYear').style.borderColor = "green";
                document.getElementById('lblTaxYear').style.color = "green";
                var x = document.getElementById("txtTaxYear").pattern;
            }
            else {
                document.getElementById('txtTaxYear').style.borderColor = "green";
                document.getElementById('lblTaxYear').style.color = "green";
                var x = document.getElementById("txtTaxYear").pattern;
            }


            //if (taxdroperror == "--Select--") {
            //    document.getElementById('txtdrop').style.borderColor = "#ff0000";
            //    document.getElementById("lbldrop").style.color = "#ff0000";
            //}
            //else if (taxdroperror != "--Select--") {
            //    document.getElementById('txtTaxNo').style.borderColor = "green";
            //    document.getElementById('lbldrop').style.color = "green";
            //}
            //else {
            //    document.getElementById('txtTaxNo').style.borderColor = "green";
            //    document.getElementById('lbldrop').style.color = "green";
            //}                     
            if (taxyearerror == "" || taxparcelerror == "" && document.getElementById("chkTBD").checked == false) {
                return false;
            }
            else if (taxyearerror != "" || taxparcelerror != "") {
                return true;
            }
            else if (document.getElementById("chkTBD").checked = true && taxparcelerror == "") {
                return true;
            }
        }

        function functionDelinquent() {
            var txtdeliPayeeerror,
                txtdelitAddresserror,
                txtdelitCityerror,
                txtdelitStateerror,
                txtdelitziperror,
                txtdelitaxyearerror,
                txtpayoffamounterror,
                txtpayoffgooderror,
                txtinitialinstallerror,
                txtdelitcommenterror,
                txtnotapplicableerror, txtdatetaxsaleerror, txtlastdayrederror;
            txtdeliPayeeerror = document.getElementById("txtdeliPayee").value;
            txtdelitAddresserror = document.getElementById("txtdelitAddress").value;
            txtdelitCityerror = document.getElementById("txtdelitCity").value;
            txtdelitStateerror = document.getElementById("txtdelitState").value;
            txtdelitziperror = document.getElementById("txtdelitzip").value;
            txtdelitaxyearerror = document.getElementById("txtdelitaxyear").value;
            txtpayoffamounterror = document.getElementById("txtpayoffamount").value;
            //txtdelitcommenterror = document.getElementById("txtdelitcomment").value;
            txtpayoffgooderror = document.getElementById("txtpayoffgood").value;
            txtinitialinstallerror = document.getElementById("txtinitialinstall").value;
            txtnotapplicableerror = document.getElementById("txtnotapplicable").value;
            txtdatetaxsaleerror = document.getElementById("txtdatetaxsale").value;
            txtlastdayrederror = document.getElementById("txtlastdayred").value;


            if (txtdeliPayeeerror == "") {
                document.getElementById('txtdeliPayee').style.borderColor = "#ff0000";
                document.getElementById("lbldeliPayee").style.color = "#ff0000";
            }
            else if (txtdeliPayeeerror != "") {
                document.getElementById('txtdeliPayee').style.borderColor = "green";
                document.getElementById('lbldeliPayee').style.color = "green";
            }
            else {
                document.getElementById('txtdeliPayee').style.borderColor = "green";
                document.getElementById('lbldeliPayee').style.color = "green";
            }

            if (txtdelitAddresserror == "") {
                document.getElementById('txtdelitAddress').style.borderColor = "#ff0000";
                document.getElementById('lbldelitAddress').style.color = "#ff0000";
            }
            else if (txtdelitAddresserror != "") {
                document.getElementById('txtdelitAddress').style.borderColor = "green";
                document.getElementById('lbldelitAddress').style.color = "green";
            }
            else {
                document.getElementById('txtdelitAddress').style.borderColor = "green";
                document.getElementById('lbldelitAddress').style.color = "green";
            }

            if (txtdelitCityerror == "") {
                document.getElementById('txtdelitCity').style.borderColor = "#ff0000";
                document.getElementById('lbldelitCity').style.color = "#ff0000";
            }
            else if (txtdelitCityerror != "") {
                document.getElementById('txtdelitCity').style.borderColor = "green";
                document.getElementById('lbldelitCity').style.color = "green";
            }
            else {
                document.getElementById('txtdelitCity').style.borderColor = "green";
                document.getElementById('lbldelitCity').style.color = "green";
            }

            if (txtdelitStateerror == "") {
                document.getElementById('txtdelitState').style.borderColor = "#ff0000";
                document.getElementById('lbldelitState').style.color = "#ff0000";
            }
            else if (txtdelitStateerror != "") {
                document.getElementById('txtdelitState').style.borderColor = "green";
                document.getElementById('lbldelitState').style.color = "green";
            }
            else {
                document.getElementById('txtdelitState').style.borderColor = "green";
                document.getElementById('lbldelitState').style.color = "green";
            }

            if (txtdelitziperror == "") {
                document.getElementById('txtdelitzip').style.borderColor = "#ff0000";
                document.getElementById('lbldelitzip').style.color = "#ff0000";
            }
            else if (txtdelitziperror != "") {
                document.getElementById('txtdelitzip').style.borderColor = "green";
                document.getElementById('lbldelitzip').style.color = "green";
            }
            else {
                document.getElementById('txtdelitzip').style.borderColor = "green";
                document.getElementById('lbldelitzip').style.color = "green";
            }

            if (txtdelitaxyearerror == "") {
                document.getElementById('txtdelitaxyear').style.borderColor = "#ff0000";
                document.getElementById('lbldelitaxyear').style.color = "#ff0000";
            }
            else if (txtdelitaxyearerror != "") {
                document.getElementById('txtdelitaxyear').style.borderColor = "green";
                document.getElementById('lbldelitaxyear').style.color = "green";
            }
            else {
                document.getElementById('txtdelitaxyear').style.borderColor = "green";
                document.getElementById('lbldelitaxyear').style.color = "green";
            }

            if (txtpayoffamounterror == "") {
                document.getElementById('txtpayoffamount').style.borderColor = "#ff0000";
                document.getElementById('lblpayoffamount').style.color = "#ff0000";
            }
            else if (txtpayoffamounterror != "") {
                document.getElementById('txtpayoffamount').style.borderColor = "green";
                document.getElementById('lblpayoffamount').style.color = "green";
            }
            else {
                document.getElementById('txtpayoffamount').style.borderColor = "green";
                document.getElementById('lblpayoffamount').style.color = "green";
            }

            if (txtpayoffgooderror == "") {
                document.getElementById('txtpayoffgood').style.borderColor = "#ff0000";
                document.getElementById('lblpayoffgood').style.color = "#ff0000";
            }
            else if (txtpayoffgooderror != "") {
                document.getElementById('txtpayoffgood').style.borderColor = "green";
                document.getElementById('lblpayoffgood').style.color = "green";
            }
            else {
                document.getElementById('txtpayoffgood').style.borderColor = "green";
                document.getElementById('lblpayoffgood').style.color = "green";
            }

            if (txtinitialinstallerror == "") {
                document.getElementById('txtinitialinstall').style.borderColor = "#ff0000";
                document.getElementById('lblinitialinstall').style.color = "#ff0000";
            }
            else if (txtinitialinstallerror != "") {
                document.getElementById('txtinitialinstall').style.borderColor = "green";
                document.getElementById('lblinitialinstall').style.color = "green";
            }
            else {
                document.getElementById('txtinitialinstall').style.borderColor = "green";
                document.getElementById('lblinitialinstall').style.color = "green";
            }

            if (txtnotapplicableerror == "Select") {
                document.getElementById('txtnotapplicable').style.borderColor = "#ff0000";
                document.getElementById('lblnotapplicable').style.color = "#ff0000";

                document.getElementById('txtdatetaxsale').style.borderColor = "";
                document.getElementById("lbldatetaxsale").style.color = "";
                document.getElementById('txtlastdayred').style.borderColor = "";
                document.getElementById("lbllastdayred").style.color = "";
            }
            else if (txtnotapplicableerror != "Select") {
                document.getElementById('txtnotapplicable').style.borderColor = "green";
                document.getElementById('lblnotapplicable').style.color = "green";
            }
            else {
                document.getElementById('txtnotapplicable').style.borderColor = "green";
                document.getElementById('lblnotapplicable').style.color = "green";
            }

            if (txtnotapplicableerror == "No") {
                document.getElementById('txtdatetaxsale').style.borderColor = "#ff0000";
                document.getElementById("lbldatetaxsale").style.color = "#ff0000";
                document.getElementById('txtlastdayred').style.borderColor = "#ff0000";
                document.getElementById("lbllastdayred").style.color = "#ff0000";
            }
            else if (txtnotapplicableerror != "No") {
                document.getElementById('txtdatetaxsale').style.borderColor = "";
                document.getElementById('lbldatetaxsale').style.color = "";
                document.getElementById('txtlastdayred').style.borderColor = "";
                document.getElementById('lbllastdayred').style.color = "";
            }

            if (txtdatetaxsaleerror != "") {
                document.getElementById('txtdatetaxsale').style.borderColor = "green";
                document.getElementById('lbldatetaxsale').style.color = "green";
            }

            if (txtlastdayrederror != "") {
                document.getElementById('txtlastdayred').style.borderColor = "green";
                document.getElementById('lbllastdayred').style.color = "green";
            }

            if (txtdeliPayeeerror == "" || txtdelitAddresserror == "" || txtdelitaxyearerror == "" || txtpayoffamounterror == "" || txtpayoffgooderror == "" || txtinitialinstallerror == "" || txtnotapplicableerror == "Select" || txtnotapplicableerror == "No" || txtdatetaxsaleerror == "" && txtlastdayrederror == "") {
                if (txtnotapplicableerror == "No" || txtnotapplicableerror == "Select") {
                    if ((txtdatetaxsaleerror == "" || txtlastdayrederror == "")) {
                        return false;
                    }
                }
            }
            else if (txtdeliPayeeerror != "" || txtdelitAddresserror != "" || txtdelitaxyearerror != "" || txtpayoffamounterror != "" || txtpayoffgooderror != "" || txtinitialinstallerror != "" || txtnotapplicableerror != "Select" || txtnotapplicableerror == "No" || txtdatetaxsaleerror != "" && txtlastdayrederror != "") {
                if (txtnotapplicableerror == "Yes") {
                    if ((txtdatetaxsaleerror != "" || txtlastdayrederror != "")) {
                        return true;
                    }
                }
            }
        }

        function functionExemption() {
            var Exemerror;
            Exemerror = document.getElementById("txtexetype").value;

            if (Exemerror == "Select") {
                document.getElementById('txtexetype').style.borderColor = "#ff0000";
                document.getElementById("lblexetype").style.color = "#ff0000";
            }
            else if (Exemerror != "") {
                document.getElementById('txtexetype').style.borderColor = "green";
                document.getElementById('lblexetype').style.color = "green";
            }
            else {
                document.getElementById('txtexetype').style.borderColor = "green";
                document.getElementById('lblexetype').style.color = "green";
            }

            if (Exemerror == 'Select') {
                return false;
            }
            else if (Exemerror != 'Select') {
                return true;
            }
        }

        function functionSpecial() {
            var Specialerror;
            Specialerror = document.getElementById("txtInstallRemain").value;

            if (Specialerror == "") {
                document.getElementById('txtInstallRemain').style.borderColor = "#ff0000";
                document.getElementById("lblinstremaining").style.color = "#ff0000";
            }
            else if (Specialerror != "") {
                document.getElementById('txtInstallRemain').style.borderColor = "green";
                document.getElementById('lblinstremaining').style.color = "green";
            }
            else {
                document.getElementById('txtInstallRemain').style.borderColor = "green";
                document.getElementById('lblinstremaining').style.color = "green";
            }

            if (Specialerror == '') {
                return false;
            }
            else if (Specialerror != '') {
                return true;
            }
        }

        function functionPrior() {
            var Priorerror, txtpriorigamtdueerror, txtprideliqdateerror, txtpriamtpaiderror;
            Priorerror = document.getElementById("txtpriodeli").value;
            txtpriorigamtdueerror = document.getElementById("txtpriorigamtdue").value;
            txtprideliqdateerror = document.getElementById("txtprideliqdate").value;
            txtpriamtpaiderror = document.getElementById("txtpriamtpaid").value;

            if (Priorerror == "") {
                document.getElementById('txtpriodeli').style.borderColor = "#ff0000";
                document.getElementById("lblpriodeli").style.color = "#ff0000";
            }
            else if (Priorerror != "") {
                document.getElementById('txtpriodeli').style.borderColor = "green";
                document.getElementById('lblpriodeli').style.color = "green";
            }
            else {
                document.getElementById('txtpriodeli').style.borderColor = "green";
                document.getElementById('lblpriodeli').style.color = "green";
            }

            if (txtpriorigamtdueerror == "") {
                document.getElementById('txtpriorigamtdue').style.borderColor = "#ff0000";
                document.getElementById("lblpriorigamtdue").style.color = "#ff0000";
            }
            else if (txtpriorigamtdueerror != "") {
                document.getElementById('txtpriorigamtdue').style.borderColor = "green";
                document.getElementById('lblpriorigamtdue').style.color = "green";
            }
            else {
                document.getElementById('txtpriorigamtdue').style.borderColor = "green";
                document.getElementById('lblpriorigamtdue').style.color = "green";
            }

            if (txtprideliqdateerror == "") {
                document.getElementById('txtprideliqdate').style.borderColor = "#ff0000";
                document.getElementById("lblprideliqdate").style.color = "#ff0000";
            }
            else if (txtprideliqdateerror != "") {
                document.getElementById('txtprideliqdate').style.borderColor = "green";
                document.getElementById('lblprideliqdate').style.color = "green";
            }
            else {
                document.getElementById('txtprideliqdate').style.borderColor = "green";
                document.getElementById('lblprideliqdate').style.color = "green";
            }

            if (txtpriamtpaiderror == "") {
                document.getElementById('txtpriamtpaid').style.borderColor = "#ff0000";
                document.getElementById("lblpriamtpaid").style.color = "#ff0000";
            }
            else if (txtpriamtpaiderror != "") {
                document.getElementById('txtpriamtpaid').style.borderColor = "green";
                document.getElementById('lblpriamtpaid').style.color = "green";
            }
            else {
                document.getElementById('txtpriamtpaid').style.borderColor = "green";
                document.getElementById('lblpriamtpaid').style.color = "green";
            }

            if (Priorerror == '' || txtpriorigamtdueerror == "" || txtprideliqdateerror == "" || txtpriamtpaiderror == "") {
                return false;
            }
            else if (Priorerror != '' || txtpriorigamtdueerror != "" || txtprideliqdateerror != "" || txtpriamtpaiderror != "") {
                return true;
            }
        }

        function checkinstdate1() {
            var startDate = document.getElementById("instdate1").value;
            var endDate = document.getElementById("instdate2").value;
            var startDate1 = document.getElementById("instdate3").value;
            var endDate1 = document.getElementById("instdate4").value;
            if (startDate == "") {
                if (endDate != "") {
                    document.getElementById("instdate2").value = "";
                    setTimeout(function () { document.getElementById("instdate2").focus(); }, 1);
                    //document.getElementById('instdate2').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (startDate1 != "") {
                    document.getElementById("instdate3").value = "";
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById('instdate3').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
            }

            if (endDate == "") {
                if (startDate1 != "") {
                    document.getElementById("instdate3").value = "";
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById('instdate3').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
            }
            if (startDate1 == "") {
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date3 Should not be Empty");
                    return;
                }
            }
            //if (startDate != "") {
            //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
            //        alert("Installment Date2 should be greater than Installment Date1");
            //        document.getElementById("instdate2").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date1");
            //        document.getElementById("instdate3").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date1");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (endDate != "") {
            //    if ((Date.parse(endDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date2");
            //        document.getElementById("instdate3").value = "";
            //    }

            //    if ((Date.parse(endDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date2");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (startDate1 != "") {
            //    if ((Date.parse(startDate1) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date3");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
        }

        function checkinstdate2() {
            var startDate = document.getElementById("instdate1").value;
            var endDate = document.getElementById("instdate2").value;
            var startDate1 = document.getElementById("instdate3").value;
            var endDate1 = document.getElementById("instdate4").value;
            if (startDate == "") {
                if (endDate != "") {
                    document.getElementById("instdate2").value = "";
                    setTimeout(function () { document.getElementById("instdate2").focus(); }, 1);
                    //document.getElementById('instdate2').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (startDate1 != "") {
                    document.getElementById("instdate3").value = "";
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById('instdate3').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
            }
            if (endDate == "") {
                if (startDate1 != "") {
                    document.getElementById("instdate3").value = "";
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById('instdate3').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
            }
            if (startDate1 == "") {
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date3 Should not be Empty");
                    return;
                }
            }
            //if (startDate != "") {
            //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
            //        alert("Installment Date2 should be greater than Installment Date1");
            //        document.getElementById("instdate2").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date1");
            //        document.getElementById("instdate3").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date1");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (endDate != "") {
            //    if ((Date.parse(endDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date2");
            //        document.getElementById("instdate3").value = "";
            //    }

            //    if ((Date.parse(endDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date2");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (startDate1 != "") {
            //    if ((Date.parse(startDate1) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date3");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
        }

        function checkinstdate3() {
            var startDate = document.getElementById("instdate1").value;
            var endDate = document.getElementById("instdate2").value;
            var startDate1 = document.getElementById("instdate3").value;
            var endDate1 = document.getElementById("instdate4").value;
            if (startDate == "") {
                if (endDate != "") {
                    document.getElementById("instdate2").value = "";
                    setTimeout(function () { document.getElementById("instdate2").focus(); }, 1);
                    //document.getElementById('instdate2').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (startDate1 != "") {
                    document.getElementById("instdate3").value = "";
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById('instdate3').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
            }
            if (endDate == "") {
                if (startDate1 != "") {
                    document.getElementById("instdate3").value = "";
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById('instdate3').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
            }
            if (startDate1 == "") {
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date3 Should not be Empty");
                    return;
                }
            }
            //if (startDate != "") {
            //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
            //        alert("Installment Date2 should be greater than Installment Date1");
            //        document.getElementById("instdate2").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date1");
            //        document.getElementById("instdate3").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date1");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (endDate != "") {
            //    if ((Date.parse(endDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date2");
            //        document.getElementById("instdate3").value = "";
            //    }

            //    if ((Date.parse(endDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date2");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (startDate1 != "") {
            //    if ((Date.parse(startDate1) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date3");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
        }

        function checkinstdate4() {
            var startDate = document.getElementById("instdate1").value;
            var endDate = document.getElementById("instdate2").value;
            var startDate1 = document.getElementById("instdate3").value;
            var endDate1 = document.getElementById("instdate4").value;
            if (startDate == "") {
                if (endDate != "") {
                    document.getElementById("instdate2").value = "";
                    setTimeout(function () { document.getElementById("instdate2").focus(); }, 1);
                    //document.getElementById('instdate2').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (startDate1 != "") {
                    document.getElementById("instdate3").value = "";
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById('instdate3').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
            }
            if (endDate == "") {
                if (startDate1 != "") {
                    document.getElementById("instdate3").value = "";
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById('instdate3').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
            }
            if (startDate1 == "") {
                if (endDate1 != "") {
                    document.getElementById("instdate4").value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById('instdate4').focus();
                    alert("Installment Date3 Should not be Empty");
                    return;
                }
            }
            //if (startDate != "") {
            //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
            //        alert("Installment Date2 should be greater than Installment Date1");
            //        document.getElementById("instdate2").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date1");
            //        document.getElementById("instdate3").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date1");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (endDate != "") {
            //    if ((Date.parse(endDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date2");
            //        document.getElementById("instdate3").value = "";
            //    }

            //    if ((Date.parse(endDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date2");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (startDate1 != "") {
            //    if ((Date.parse(startDate1) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date3");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
        }

        //$("#instdate3").change(function () {
        //    var startDate = document.getElementById("instdate2").value;
        //    var endDate = document.getElementById("instdate3").value;

        //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
        //        alert("Installment Date3 should be greater than Installment Date2");
        //        document.getElementById("instdate3").value = "";
        //    }
        //});
        //$("#instdate4").change(function () {
        //    var startDate = document.getElementById("instdate3").value;
        //    var endDate = document.getElementById("instdate4").value;

        //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
        //        alert("Installment Date4 should be greater than Installment Date3");
        //        document.getElementById("instdate4").value = "";
        //    }
        //});

        //Future Inst Date
        function Instcheckinstdate1() {
            var fstartDate = document.getElementById("txtmaninstdate1").value;
            var fendDate = document.getElementById("txtmaninstdate2").value;
            var fstartDate1 = document.getElementById("txtmaninstdate3").value;
            var fendDate1 = document.getElementById("txtmaninstdate4").value;
            if (fstartDate == "") {
                if (fendDate != "") {
                    document.getElementById("txtmaninstdate2").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate2").focus(); }, 1);
                    //document.getElementById('txtmaninstdate2').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (fstartDate1 != "") {
                    document.getElementById("txtmaninstdate3").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById('txtmaninstdate3').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
            }

            if (fendDate == "") {
                if (fstartDate1 != "") {
                    document.getElementById("txtmaninstdate3").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById('txtmaninstdate3').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
            }
            if (fstartDate1 == "") {
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date3 Should not be Empty");
                    return;
                }
            }
            //if (startDate != "") {
            //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
            //        alert("Installment Date2 should be greater than Installment Date1");
            //        document.getElementById("instdate2").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date1");
            //        document.getElementById("instdate3").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date1");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (endDate != "") {
            //    if ((Date.parse(endDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date2");
            //        document.getElementById("instdate3").value = "";
            //    }

            //    if ((Date.parse(endDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date2");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (startDate1 != "") {
            //    if ((Date.parse(startDate1) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date3");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
        }

        function Instcheckinstdate2() {
            var fstartDate = document.getElementById("txtmaninstdate1").value;
            var fendDate = document.getElementById("txtmaninstdate2").value;
            var fstartDate1 = document.getElementById("txtmaninstdate3").value;
            var fendDate1 = document.getElementById("txtmaninstdate4").value;
            if (fstartDate == "") {
                if (fendDate != "") {
                    document.getElementById("txtmaninstdate2").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate2").focus(); }, 1);
                    //document.getElementById('txtmaninstdate2').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (fstartDate1 != "") {
                    document.getElementById("txtmaninstdate3").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById('txtmaninstdate3').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
            }
            if (fendDate == "") {
                if (fstartDate1 != "") {
                    document.getElementById("txtmaninstdate3").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById('txtmaninstdate3').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
            }
            if (fstartDate1 == "") {
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date3 Should not be Empty");
                    return;
                }
            }
            //if (startDate != "") {
            //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
            //        alert("Installment Date2 should be greater than Installment Date1");
            //        document.getElementById("instdate2").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date1");
            //        document.getElementById("instdate3").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date1");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (endDate != "") {
            //    if ((Date.parse(endDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date2");
            //        document.getElementById("instdate3").value = "";
            //    }

            //    if ((Date.parse(endDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date2");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (startDate1 != "") {
            //    if ((Date.parse(startDate1) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date3");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
        }

        function checkinstdate3() {
            var fstartDate = document.getElementById("txtmaninstdate1").value;
            var fendDate = document.getElementById("txtmaninstdate2").value;
            var fstartDate1 = document.getElementById("txtmaninstdate3").value;
            var fendDate1 = document.getElementById("txtmaninstdate4").value;
            if (fstartDate == "") {
                if (fendDate != "") {
                    document.getElementById("txtmaninstdate2").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate2").focus(); }, 1);
                    //document.getElementById('txtmaninstdate2').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (fstartDate1 != "") {
                    document.getElementById("txtmaninstdate3").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById('txtmaninstdate3').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
            }
            if (fendDate == "") {
                if (fstartDate1 != "") {
                    document.getElementById("txtmaninstdate3").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById('txtmaninstdate3').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
            }
            if (fstartDate1 == "") {
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date3 Should not be Empty");
                    return;
                }
            }
            //if (startDate != "") {
            //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
            //        alert("Installment Date2 should be greater than Installment Date1");
            //        document.getElementById("instdate2").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date1");
            //        document.getElementById("instdate3").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date1");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (endDate != "") {
            //    if ((Date.parse(endDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date2");
            //        document.getElementById("instdate3").value = "";
            //    }

            //    if ((Date.parse(endDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date2");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (startDate1 != "") {
            //    if ((Date.parse(startDate1) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date3");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
        }

        function checkinstdate4() {
            var fstartDate = document.getElementById("txtmaninstdate1").value;
            var fendDate = document.getElementById("txtmaninstdate2").value;
            var fstartDate1 = document.getElementById("txtmaninstdate3").value;
            var fendDate1 = document.getElementById("txtmaninstdate4").value;
            if (fstartDate == "") {
                if (fendDate != "") {
                    document.getElementById("txtmaninstdate2").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate2").focus(); }, 1);
                    //document.getElementById('txtmaninstdate2').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (fstartDate1 != "") {
                    document.getElementById("txtmaninstdate3").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById('txtmaninstdate3').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date1 Should not be Empty");
                    return;
                }
            }
            if (fendDate == "") {
                if (fstartDate1 != "") {
                    document.getElementById("txtmaninstdate3").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById('txtmaninstdate3').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date2 Should not be Empty");
                    return;
                }
            }
            if (fstartDate1 == "") {
                if (fendDate1 != "") {
                    document.getElementById("txtmaninstdate4").value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById('txtmaninstdate4').focus();
                    alert("Installment Date3 Should not be Empty");
                    return;
                }
            }
            //if (startDate != "") {
            //    if ((Date.parse(startDate) >= Date.parse(endDate))) {
            //        alert("Installment Date2 should be greater than Installment Date1");
            //        document.getElementById("instdate2").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date1");
            //        document.getElementById("instdate3").value = "";
            //    }
            //    if ((Date.parse(startDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date1");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (endDate != "") {
            //    if ((Date.parse(endDate) >= Date.parse(startDate1))) {
            //        alert("Installment Date3 should be greater than Installment Date2");
            //        document.getElementById("instdate3").value = "";
            //    }

            //    if ((Date.parse(endDate) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date2");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
            //if (startDate1 != "") {
            //    if ((Date.parse(startDate1) >= Date.parse(endDate1))) {
            //        alert("Installment Date4 should be greater than Installment Date3");
            //        document.getElementById("instdate4").value = "";
            //    }
            //}
        }

        //Inst.Amount Paid
        function myFunctionAmtPaid1() {
            document.getElementById("instamountpaid1").value = document.getElementById("e").innerText;
        }
        function myFunctionAmtPaid2() {
            document.getElementById("instamountpaid2").value = document.getElementById("f").innerText;
        }
        function myFunctionAmtPaid3() {
            document.getElementById("instamountpaid3").value = document.getElementById("g").innerText;
        }
        function myFunctionAmtPaid4() {
            document.getElementById("instamountpaid4").value = document.getElementById("h").innerText;
        }

        //Future Amount Paid
        function FuturemyFunctionAmtPaid1() {
            document.getElementById("instmanamtpaid1").value = document.getElementById("fe").innerText;
        }
        function FuturemyFunctionAmtPaid2() {
            document.getElementById("instmanamtpaid2").value = document.getElementById("ff").innerText;
        }
        function FuturemyFunctionAmtPaid3() {
            document.getElementById("instmanamtpaid3").value = document.getElementById("fg").innerText;
        }
        function FuturemyFunctionAmtPaid4() {
            document.getElementById("instmanamtpaid4").value = document.getElementById("fh").innerText;
        }

        function formatMoneyAmtPaid1(n, c, instamountpaid1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instamountpaid1 = instamountpaid1 == undefined ? "." : instamountpaid1,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instamountpaid1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyAmtPaid2(n, c, instamountpaid2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instamountpaid2 = instamountpaid2 == undefined ? "." : instamountpaid2,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instamountpaid2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyAmtPaid3(n, c, instamountpaid3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instamountpaid3 = instamountpaid3 == undefined ? "." : instamountpaid3,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instamountpaid3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyAmtPaid4(n, c, instamountpaid4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instamountpaid4 = instamountpaid4 == undefined ? "." : instamountpaid4,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instamountpaid4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        //Future Amtpaid
        function futureformatMoneyAmtPaid1(n, c, instmanamtpaid1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instmanamtpaid1 = instmanamtpaid1 == undefined ? "." : instmanamtpaid1,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instmanamtpaid1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoneyAmtPaid2(n, c, instmanamtpaid2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instmanamtpaid2 = instmanamtpaid2 == undefined ? "." : instmanamtpaid2,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instmanamtpaid2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoneyAmtPaid3(n, c, instmanamtpaid3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instmanamtpaid3 = instmanamtpaid3 == undefined ? "." : instmanamtpaid3,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instmanamtpaid3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoneyAmtPaid4(n, c, instmanamtpaid4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instmanamtpaid4 = instmanamtpaid4 == undefined ? "." : instmanamtpaid4,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instmanamtpaid4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function AmtPaid1() {
            document.getElementById("e").innerText = formatMoneyAmtPaid1(document.getElementById("instamountpaid1").value);
        }
        function AmtPaid2() {
            document.getElementById("f").innerText = formatMoneyAmtPaid2(document.getElementById("instamountpaid2").value);
        }
        function AmtPaid3() {
            document.getElementById("g").innerText = formatMoneyAmtPaid3(document.getElementById("instamountpaid3").value);
        }
        function AmtPaid4() {
            document.getElementById("h").innerText = formatMoneyAmtPaid4(document.getElementById("instamountpaid4").value);
        }

        //Future AmtPaid
        function FutureAmtPaid1() {
            document.getElementById("fe").innerText = futureformatMoneyAmtPaid1(document.getElementById("instmanamtpaid1").value);
        }
        function FutureAmtPaid2() {
            document.getElementById("ff").innerText = futureformatMoneyAmtPaid2(document.getElementById("instmanamtpaid2").value);
        }
        function FutureAmtPaid3() {
            document.getElementById("fg").innerText = futureformatMoneyAmtPaid3(document.getElementById("instmanamtpaid3").value);
        }
        function FutureAmtPaid4() {
            document.getElementById("fh").innerText = futureformatMoneyAmtPaid4(document.getElementById("instmanamtpaid4").value);
        }

        //Remaining Balance
        function mytest1() {
            AmtPaid1();
            myFunctionAmtPaid1();
            document.getElementById('remainingbalance1').disabled = false;
            var instAmt1 = document.getElementById("instamount1").value;
            var instPaid1 = document.getElementById("instamountpaid1").value;
            var paidDue1 = document.getElementById("instpaiddue1");
            Money1 = parseFloat(instAmt1.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid1.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit1").value = formatMoney1(result);
            var myHidden = document.getElementById("hdntxtbxTaksit1").value;
            document.getElementById("remainingbalance1").value = formatMoney1(result);

            instAmt1 = instAmt1.replace(',', '');
            instPaid1 = instPaid1.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue1.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue1.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue1.value = "Paid";
            }
            var firstChar = myHidden.substr(0, 1);
            if (firstChar == '-') {
                document.getElementById("remainingbalance1").readOnly = false;
            }
            else {
                document.getElementById("remainingbalance1").readOnly = false;
            }
        }

        function mytest2() {
            AmtPaid2();
            myFunctionAmtPaid2();
            document.getElementById('remainingbalance2').disabled = false;
            var instAmt2 = document.getElementById("instamount2").value;
            var instPaid2 = document.getElementById("instamountpaid2").value;
            var paidDue2 = document.getElementById("instpaiddue2");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit2").value = formatMoney1(result);
            var myHidden2 = document.getElementById("hdntxtbxTaksit2").value;
            document.getElementById("remainingbalance2").value = myHidden2;

            instAmt2 = instAmt2.replace(',', '');
            instPaid2 = instPaid2.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue2.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue2.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue2.value = "Paid";
            }
            var firstChar2 = myHidden2.substr(0, 1);
            if (firstChar2 == '-') {
                document.getElementById("remainingbalance2").readOnly = false;
            }
            else {
                document.getElementById("remainingbalance2").readOnly = false;
            }
        }

        function mytest3() {
            AmtPaid3();
            myFunctionAmtPaid3();
            document.getElementById('remainingbalance3').disabled = false;
            var instAmt3 = document.getElementById("instamount3").value;
            var instPaid3 = document.getElementById("instamountpaid3").value;
            var paidDue3 = document.getElementById("instpaiddue3");
            Money1 = parseFloat(instAmt3.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid3.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit3").value = formatMoney1(result);
            var myHidden3 = document.getElementById("hdntxtbxTaksit3").value;
            document.getElementById("remainingbalance3").value = myHidden3;

            instAmt3 = instAmt3.replace(',', '');
            instPaid3 = instPaid3.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue3.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue3.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue3.value = "Paid";
            }
            var firstChar3 = myHidden3.substr(0, 1);
            if (firstChar3 == '-') {
                document.getElementById("remainingbalance3").readOnly = false;
            }
            else {
                document.getElementById("remainingbalance3").readOnly = false;
            }
        }

        function mytest4() {
            AmtPaid4();
            myFunctionAmtPaid4();
            document.getElementById('remainingbalance4').disabled = false;
            var instAmt4 = document.getElementById("instamount4").value;
            var instPaid4 = document.getElementById("instamountpaid4").value;
            var paidDue4 = document.getElementById("instpaiddue4");
            Money1 = parseFloat(instAmt4.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid4.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit4").value = formatMoney1(result);
            var myHidden4 = document.getElementById("hdntxtbxTaksit4").value;
            document.getElementById("remainingbalance4").value = myHidden4;

            instAmt4 = instAmt4.replace(',', '');
            instPaid4 = instPaid4.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue4.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue4.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue4.value = "Paid";
            }
            var firstChar4 = myHidden4.substr(0, 1);
            if (firstChar4 == '-') {
                document.getElementById("remainingbalance4").readOnly = false;
            }
            else {
                document.getElementById("remainingbalance4").readOnly = false;
            }
        }

        //Future Remaining Balance
        function Futuremytest1() {
            FutureAmtPaid1();
            FuturemyFunctionAmtPaid1();
            document.getElementById('txtmanurembal1').disabled = false;
            var instAmt1 = document.getElementById("instmanamount1").value;
            var instPaid1 = document.getElementById("instmanamtpaid1").value;
            var paidDue1 = document.getElementById("ddlmaninstpaiddue1");
            Money1 = parseFloat(instAmt1.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid1.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("futurehdntxtbxTaksit1").value = futureformatMoney1(result);
            var myHidden = document.getElementById("futurehdntxtbxTaksit1").value;
            document.getElementById("txtmanurembal1").value = formatMoney1(result);

            instAmt1 = instAmt1.replace(',', '');
            instPaid1 = instPaid1.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue1.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue1.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue1.value = "Paid";
            }
            var firstChar = myHidden.substr(0, 1);
            if (firstChar == '-') {
                document.getElementById("txtmanurembal1").readOnly = false;
            }
            else {
                document.getElementById("txtmanurembal1").readOnly = false;
            }
        }

        function Futuremytest2() {
            FutureAmtPaid2();
            FuturemyFunctionAmtPaid2();
            document.getElementById('txtmanurembal2').disabled = false;
            var instAmt2 = document.getElementById("instmanamount2").value;
            var instPaid2 = document.getElementById("instmanamtpaid2").value;
            var paidDue2 = document.getElementById("ddlmaninstpaiddue2");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("futurehdntxtbxTaksit2").value = futureformatMoney1(result);
            var myHidden2 = document.getElementById("futurehdntxtbxTaksit2").value;
            document.getElementById("txtmanurembal2").value = myHidden2;

            instAmt2 = instAmt2.replace(',', '');
            instPaid2 = instPaid2.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue2.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue2.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue2.value = "Paid";
            }
            var firstChar2 = myHidden2.substr(0, 1);
            if (firstChar2 == '-') {
                document.getElementById("txtmanurembal2").readOnly = false;
            }
            else {
                document.getElementById("txtmanurembal2").readOnly = false;
            }
        }

        function Futuremytest3() {
            FutureAmtPaid3();
            FuturemyFunctionAmtPaid3();
            document.getElementById('txtmanurembal3').disabled = false;
            var instAmt3 = document.getElementById("instmanamount3").value;
            var instPaid3 = document.getElementById("instmanamtpaid3").value;
            var paidDue3 = document.getElementById("ddlmaninstpaiddue3");
            Money1 = parseFloat(instAmt3.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid3.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("futurehdntxtbxTaksit3").value = futureformatMoney1(result);
            var myHidden3 = document.getElementById("futurehdntxtbxTaksit3").value;
            document.getElementById("txtmanurembal3").value = myHidden3;
            instAmt3 = instAmt3.replace(',', '');
            instPaid3 = instPaid3.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue3.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue3.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue3.value = "Paid";
            }
            var firstChar3 = myHidden3.substr(0, 1);
            if (firstChar3 == '-') {
                document.getElementById("txtmanurembal3").readOnly = false;
            }
            else {
                document.getElementById("txtmanurembal3").readOnly = false;
            }
        }

        function Futuremytest4() {
            FutureAmtPaid4();
            FuturemyFunctionAmtPaid4();
            document.getElementById('txtmanurembal4').disabled = false;
            var instAmt4 = document.getElementById("instmanamount4").value;
            var instPaid4 = document.getElementById("instmanamtpaid4").value;
            var paidDue4 = document.getElementById("ddlmaninstpaiddue4");
            Money1 = parseFloat(instAmt4.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid4.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("futurehdntxtbxTaksit4").value = futureformatMoney1(result);
            var myHidden4 = document.getElementById("futurehdntxtbxTaksit4").value;
            document.getElementById("txtmanurembal4").value = myHidden4;

            instAmt4 = instAmt4.replace(',', '');
            instPaid4 = instPaid4.replace(',', '');
            if (parseFloat(instAmt4) == parseFloat(instPaid4)) {
                paidDue4.value = "Paid";
            } else if (parseFloat(instAmt4) > parseFloat(instPaid4)) {
                paidDue4.value = "Due";
            }
            else if (parseFloat(instAmt4) < parseFloat(instPaid4)) {
                paidDue4.value = "Paid";
            }
            var firstChar4 = myHidden4.substr(0, 1);
            if (firstChar4 == '-') {
                document.getElementById("txtmanurembal4").readOnly = false;
            }
            else {
                document.getElementById("txtmanurembal4").readOnly = false;
            }
        }

        function greateramount1(val) {
            var instamunt1 = document.getElementById("instamount1").value;
            var discountamnt1 = document.getElementById("discamt1").value;

            var regex = /[.,\s]/g;
            var result = instamunt1.replace(regex, '');
            result = (result - (result % 100)) / 100;

            var regex1 = /[.,\s]/g;
            var result1 = discountamnt1.replace(regex1, '');

            var lastDigit1 = discountamnt1.toString().slice(-3);

            if (lastDigit1 == ".00") {
                result1 = (result1 - (result1 % 100)) / 100;
            }

            if (result1 > result) {
                document.getElementById('discamt1').value = "";
                setTimeout(function () { document.getElementById("discamt1").focus(); }, 1);
                //document.getElementById('discamt1').focus();
                alert("Discount amount cannot be greater than the installment amount...");
                return;
            }
        }

        function greateramount2(val) {
            var instamunt2 = document.getElementById("instamount2").value;
            var discountamnt2 = document.getElementById("discamt2").value;

            var regex1 = /[.,\s]/g;
            var result1 = instamunt2.replace(regex1, '');
            result1 = (result1 - (result1 % 100)) / 100;

            var regex11 = /[.,\s]/g;
            var result11 = discountamnt2.replace(regex11, '');

            var lastDigit2 = discountamnt2.toString().slice(-3);

            if (lastDigit2 == ".00") {
                result11 = (result11 - (result11 % 100)) / 100;
            }

            if (result11 > result1) {
                document.getElementById('discamt2').value = "";
                setTimeout(function () { document.getElementById("discamt2").focus(); }, 1);
                //document.getElementById('discamt2').focus();
                alert("Discount amount cannot be greater than the installment amount...");
                return;
            }
        }

        function greateramount3(val) {
            var instamunt3 = document.getElementById("instamount3").value;
            var discountamnt3 = document.getElementById("discamt3").value;

            var regex11 = /[.,\s]/g;
            var result11 = instamunt3.replace(regex11, '');
            result11 = (result11 - (result11 % 100)) / 100;

            var regex111 = /[.,\s]/g;
            var result111 = discountamnt3.replace(regex111, '');

            var lastDigit3 = discountamnt3.toString().slice(-3);

            if (lastDigit3 == ".00") {
                result111 = (result111 - (result111 % 100)) / 100;
            }

            if (result111 > result11) {
                document.getElementById('discamt3').value = "";
                setTimeout(function () { document.getElementById("discamt3").focus(); }, 1);
                //document.getElementById('discamt3').focus();
                alert("Discount amount cannot be greater than the installment amount...");
                return;
            }
        }

        function greateramount4(val) {
            var instamunt4 = document.getElementById("instamount4").value;
            var discountamnt4 = document.getElementById("discamt4").value;

            var regex111 = /[.,\s]/g;
            var result111 = instamunt4.replace(regex111, '');
            result111 = (result111 - (result111 % 100)) / 100;

            var regex1111 = /[.,\s]/g;
            var result1111 = discountamnt4.replace(regex1111, '');

            var lastDigit4 = discountamnt4.toString().slice(-3);

            if (lastDigit4 == ".00") {
                result1111 = (result1111 - (result1111 % 100)) / 100;
            }

            if (result1111 > result111) {
                document.getElementById('discamt4').value = "";
                setTimeout(function () { document.getElementById("discamt4").focus(); }, 1);
                //document.getElementById('discamt4').focus();
                alert("Discount amount cannot be greater than the installment amount...");
                return;
            }
        }

        function mydiscountamount1() {
            Discount1();
            myFunctionDiscount1();
        }
        function mydiscountamount2() {
            Discount2();
            myFunctionDiscount2();
        }
        function mydiscountamount3() {
            Discount3();
            myFunctionDiscount3();
        }
        function mydiscountamount4() {
            Discount4();
            myFunctionDiscount4();
        }

        function formatMoneyDiscount1(n, c, discamt1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              discamt1 = discamt1 == undefined ? "." : discamt1,
               n = n.replace(/,/g, ''),
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? discamt1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };


        function formatMoneyDiscount2(n, c, discamt2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              discamt2 = discamt2 == undefined ? "." : discamt2,
               n = n.replace(/,/g, ''),
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? discamt2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyDiscount3(n, c, discamt3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              discamt3 = discamt3 == undefined ? "." : discamt3,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? discamt3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyDiscount4(n, c, discamt4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              discamt4 = discamt4 == undefined ? "." : discamt4,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? discamt4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function Discount1() {
            document.getElementById("a").innerText = formatMoneyDiscount1(document.getElementById("discamt1").value);
        }
        function Discount2() {
            document.getElementById("b").innerText = formatMoneyDiscount2(document.getElementById("discamt2").value);
        }
        function Discount3() {
            document.getElementById("c").innerText = formatMoneyDiscount3(document.getElementById("discamt3").value);
        }
        function Discount4() {
            document.getElementById("d").innerText = formatMoneyDiscount4(document.getElementById("discamt4").value);
        }

        function myFunctionDiscount1() {
            document.getElementById("discamt1").value = document.getElementById("a").innerText;
        }
        function myFunctionDiscount2() {
            document.getElementById("discamt2").value = document.getElementById("b").innerText;
        }
        function myFunctionDiscount3() {
            document.getElementById("discamt3").value = document.getElementById("c").innerText;
        }
        function myFunctionDiscount4() {
            document.getElementById("discamt4").value = document.getElementById("d").innerText;
        }
        //Future Discount
        function futgreateramount1(val) {
            var futinstamunt1 = document.getElementById("instmanamount1").value;
            var futdiscountamnt1 = document.getElementById("txtmandisamount1").value;

            var regex = /[.,\s]/g;
            var result = futinstamunt1.replace(regex, '');
            result = (result - (result % 100)) / 100;

            var regex1 = /[.,\s]/g;
            var result1 = futdiscountamnt1.replace(regex1, '');

            var lastDigit1 = futdiscountamnt1.toString().slice(-3);

            if (lastDigit1 == ".00") {
                result1 = (result1 - (result1 % 100)) / 100;
            }

            if (result1 > result) {
                document.getElementById('txtmandisamount1').value = "";
                setTimeout(function () { document.getElementById("txtmandisamount1").focus(); }, 1);
                //document.getElementById('txtmandisamount1').focus();
                alert("Discount amount cannot be greater than the installment amount...");
                return;
            }
        }

        function futgreateramount2(val) {
            var futinstamunt2 = document.getElementById("instmanamount2").value;
            var futdiscountamnt2 = document.getElementById("txtmandisamount2").value;

            var regex = /[.,\s]/g;
            var result = futinstamunt2.replace(regex, '');
            result = (result - (result % 100)) / 100;

            var regex1 = /[.,\s]/g;
            var result1 = futdiscountamnt2.replace(regex1, '');

            var lastDigit2 = futdiscountamnt2.toString().slice(-3);

            if (lastDigit2 == ".00") {
                result1 = (result1 - (result1 % 100)) / 100;
            }

            if (result1 > result) {
                document.getElementById('txtmandisamount2').value = "";
                setTimeout(function () { document.getElementById("txtmandisamount2").focus(); }, 1);
                //document.getElementById('txtmandisamount2').focus();
                alert("Discount amount cannot be greater than the installment amount...");
                return;
            }
        }

        function futgreateramount3(val) {
            var futinstamunt3 = document.getElementById("instmanamount3").value;
            var futdiscountamnt3 = document.getElementById("txtmandisamount3").value;

            var regex = /[.,\s]/g;
            var result = futinstamunt3.replace(regex, '');
            result = (result - (result % 100)) / 100;

            var regex1 = /[.,\s]/g;
            var result1 = futdiscountamnt3.replace(regex1, '');

            var lastDigit3 = futdiscountamnt3.toString().slice(-3);

            if (lastDigit3 == ".00") {
                result1 = (result1 - (result1 % 100)) / 100;
            }

            if (result1 > result) {
                document.getElementById('txtmandisamount3').value = "";
                setTimeout(function () { document.getElementById("txtmandisamount3").focus(); }, 1);
                //document.getElementById('txtmandisamount3').focus();
                alert("Discount amount cannot be greater than the installment amount...");
                return;
            }
        }

        function futgreateramount4(val) {
            var futinstamunt4 = document.getElementById("instmanamount4").value;
            var futdiscountamnt4 = document.getElementById("txtmandisamount4").value;

            var regex = /[.,\s]/g;
            var result = futinstamunt4.replace(regex, '');
            result = (result - (result % 100)) / 100;

            var regex1 = /[.,\s]/g;
            var result1 = futdiscountamnt4.replace(regex1, '');

            var lastDigit4 = futdiscountamnt4.toString().slice(-3);

            if (lastDigit4 == ".00") {
                result1 = (result1 - (result1 % 100)) / 100;
            }

            if (result1 > result) {
                document.getElementById('txtmandisamount4').value = "";
                setTimeout(function () { document.getElementById("txtmandisamount4").focus(); }, 1);
                //document.getElementById('txtmandisamount4').focus();
                alert("Discount amount cannot be greater than the installment amount...");
                return;
            }
        }

        function futuremydiscountamount1() {
            futureDiscount1();
            futuremyFunctionDiscount1();
        }
        function futuremydiscountamount2() {
            futureDiscount2();
            futuremyFunctionDiscount2();
        }
        function futuremydiscountamount3() {
            futureDiscount3();
            futuremyFunctionDiscount3();
        }
        function futuremydiscountamount4() {
            futureDiscount4();
            futuremyFunctionDiscount4();
        }

        function futureformatMoneyDiscount1(n, c, txtmandisamount1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtmandisamount1 = txtmandisamount1 == undefined ? "." : txtmandisamount1,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtmandisamount1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoneyDiscount2(n, c, txtmandisamount2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtmandisamount2 = txtmandisamount2 == undefined ? "." : txtmandisamount2,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtmandisamount2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoneyDiscount3(n, c, txtmandisamount3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtmandisamount3 = txtmandisamount3 == undefined ? "." : txtmandisamount3,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtmandisamount3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoneyDiscount4(n, c, txtmandisamount4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtmandisamount4 = txtmandisamount4 == undefined ? "." : txtmandisamount4,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtmandisamount4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function futureDiscount1() {
            document.getElementById("fa").innerText = futureformatMoneyDiscount1(document.getElementById("txtmandisamount1").value);
        }
        function futureDiscount2() {
            document.getElementById("fb").innerText = futureformatMoneyDiscount2(document.getElementById("txtmandisamount2").value);
        }
        function futureDiscount3() {
            document.getElementById("fc").innerText = futureformatMoneyDiscount3(document.getElementById("txtmandisamount3").value);
        }
        function futureDiscount4() {
            document.getElementById("fd").innerText = futureformatMoneyDiscount4(document.getElementById("txtmandisamount4").value);
        }

        function futuremyFunctionDiscount1() {
            document.getElementById("txtmandisamount1").value = document.getElementById("fa").innerText;
        }
        function futuremyFunctionDiscount2() {
            document.getElementById("txtmandisamount2").value = document.getElementById("fb").innerText;
        }
        function futuremyFunctionDiscount3() {
            document.getElementById("txtmandisamount3").value = document.getElementById("fc").innerText;
        }
        function futuremyFunctionDiscount4() {
            document.getElementById("txtmandisamount4").value = document.getElementById("fd").innerText;
        }
    </script>

    <script type="text/javascript">
        //function mytxtamount1() {
        //    hello1();
        //    myFunction1();
        //    document.getElementById('remainingbalance1').disabled = false;
        //    var instAmt1 = document.getElementById("instamount1").value;
        //    var instPaid1 = document.getElementById("instamountpaid1").value;
        //    Money1 = isNaN(parseFloat(instAmt1.replace(/[^0-9\.]+/g, "")));
        //    Money2 = isNaN(parseFloat(instPaid1.replace(/[^0-9\.]+/g, "")));
        //    if (Money1 == true && Money2 == false) {
        //        Money11 = '0.00';
        //        Money21 = instPaid1;
        //        Money21 = parseFloat(Money21.replace(',', ''));
        //        result = parseFloat(Money11 - Money21).toFixed(2);
        //    }
        //    else if (Money1 == false && Money2 == true) {
        //        Money11 = instAmt1;
        //        Money21 = '0.00';
        //        Money11 = parseFloat(Money11.replace(',', ''));
        //        result = parseFloat(Money11 - Money21).toFixed(2);
        //    }
        //    else if (Money1 == true && Money2 == true) {
        //        Money11 = '0.00';
        //        Money21 = '0.00';
        //        result = '0.00';
        //    }
        //    else if (Money1 == false && Money2 == false) {
        //        Money11 = parseFloat(instAmt1.replace(/[^0-9\.]+/g, ""));
        //        Money21 = parseFloat(instPaid1.replace(/[^0-9\.]+/g, ""));
        //        result = (Money11 - Money21).toFixed(2);
        //    }
        //    document.getElementById("remainingbalance1").value = formatMoney1(result);
        //}

        //function mytxtamountTotalAnnual()
        //{
        //    totalAnnual();
        //    myFunctiontotal();
        //}

        //function totalAnnual() {
        //    document.getElementById("TA").innerText = formatMoneyTotalAnnual(document.getElementById("txtAnnualTaxAmount").value);

        //}
        //function myFunctiontotal() {
        //    document.getElementById("txtAnnualTaxAmount").value = document.getElementById("TA").innerText;
        //}

        //function formatMoneyTotalAnnual(n, c, txtAnnualTaxAmount, t) {
        //    var c = isNaN(c = Math.abs(c)) ? 2 : c,
        //      txtAnnualTaxAmount = txtAnnualTaxAmount == undefined ? "." : txtAnnualTaxAmount,
        //      //n = n.replace(/,/g, ''),
        //      t = t == undefined ? "," : t,
        //      s = n < 0 ? "-" : "",
        //      i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        //      j = (j = i.length) > 3 ? j % 3 : 0;

        //    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtAnnualTaxAmount + Math.abs(n - i).toFixed(c).slice(2) : "");
        //};

        function mytxtamount1() {
            hello1();
            myFunction1();
            document.getElementById('remainingbalance1').disabled = false;

            var instAmt11 = document.getElementById("instamount1").value;
            var instAmt22 = document.getElementById("instamount2").value;
            var instAmt33 = document.getElementById("instamount3").value;
            var instAmt44 = document.getElementById("instamount4").value;

            if (instAmt11 == "") {
                instAmt11 = '0.00';
            }
            if (instAmt22 == "") {
                instAmt22 = '0.00';
            }

            if (instAmt33 == "") {
                instAmt33 = '0.00';
            }
            if (instAmt44 == "") {
                instAmt44 = '0.00';
            }
            var res1 = parseFloat(instAmt11.replace(/,/g, ''));
            var res2 = parseFloat(instAmt22.replace(/,/g, ''));
            var res3 = parseFloat(instAmt33.replace(/,/g, ''));
            var res4 = parseFloat(instAmt44.replace(/,/g, ''));
            document.getElementById("txtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            //document.getElementById('txtstrRemaingBlnce1').removeAttribute('readonly');
            var instAmt1 = document.getElementById("instamount1").value;
            var instPaid1 = document.getElementById("instamountpaid1").value;

            var paidDue1 = document.getElementById("instpaiddue1");
            Money1 = parseFloat(instAmt1.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid1.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);




            document.getElementById("hdntxtbxTaksit1").value = formatMoney1(result);
            var myHidden = document.getElementById("hdntxtbxTaksit1").value;
            document.getElementById("remainingbalance1").value = myHidden;

            instAmt1 = instAmt1.replace(',', '');
            instPaid1 = instPaid1.replace(',', '');

            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue1.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue1.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue1.value = "Paid";
            }
            var firstChar = myHidden.substr(0, 1);
            if (firstChar == '-') {
                document.getElementById("remainingbalance1").readOnly = false;
            }
            else {
                document.getElementById("remainingbalance1").readOnly = false;
            }
        }

        function mytxtamount2() {
            hello2();
            myFunction2();
            document.getElementById('remainingbalance2').disabled = false;

            var instAmt11 = document.getElementById("instamount1").value;
            var instAmt22 = document.getElementById("instamount2").value;
            var instAmt33 = document.getElementById("instamount3").value;
            var instAmt44 = document.getElementById("instamount4").value;

            if (instAmt11 == "") {
                instAmt11 = '0.00';
            }
            if (instAmt22 == "") {
                instAmt22 = '0.00';
            }

            if (instAmt33 == "") {
                instAmt33 = '0.00';
            }
            if (instAmt44 == "") {
                instAmt44 = '0.00';
            }
            var res1 = parseFloat(instAmt11.replace(/,/g, ''));
            var res2 = parseFloat(instAmt22.replace(/,/g, ''));
            var res3 = parseFloat(instAmt33.replace(/,/g, ''));
            var res4 = parseFloat(instAmt44.replace(/,/g, ''));
            document.getElementById("txtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            var instAmt2 = document.getElementById("instamount2").value;
            var instPaid2 = document.getElementById("instamountpaid2").value;
            var paidDue2 = document.getElementById("instpaiddue2");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit2").value = formatMoney1(result);
            var myHidden2 = document.getElementById("hdntxtbxTaksit2").value;
            document.getElementById("remainingbalance2").value = myHidden2;

            instAmt2 = instAmt2.replace(',', '');
            instPaid2 = instPaid2.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue2.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue2.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue2.value = "Paid";
            }
            var firstChar2 = myHidden2.substr(0, 1);
            if (firstChar2 == '-') {
                document.getElementById("remainingbalance2").readOnly = false;
            }
            else {
                document.getElementById("remainingbalance2").readOnly = false;
            }


        }

        function mytxtamount3() {
            hello3();
            myFunction3();
            document.getElementById('remainingbalance3').disabled = false;

            var instAmt11 = document.getElementById("instamount1").value;
            var instAmt22 = document.getElementById("instamount2").value;
            var instAmt33 = document.getElementById("instamount3").value;
            var instAmt44 = document.getElementById("instamount4").value;

            if (instAmt11 == "") {
                instAmt11 = '0.00';
            }
            if (instAmt22 == "") {
                instAmt22 = '0.00';
            }

            if (instAmt33 == "") {
                instAmt33 = '0.00';
            }
            if (instAmt44 == "") {
                instAmt44 = '0.00';
            }
            var res1 = parseFloat(instAmt11.replace(/,/g, ''));
            var res2 = parseFloat(instAmt22.replace(/,/g, ''));
            var res3 = parseFloat(instAmt33.replace(/,/g, ''));
            var res4 = parseFloat(instAmt44.replace(/,/g, ''));
            document.getElementById("txtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            var instAmt3 = document.getElementById("instamount3").value;
            var instPaid3 = document.getElementById("instamountpaid3").value;
            var paidDue3 = document.getElementById("instpaiddue3");
            Money1 = parseFloat(instAmt3.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid3.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit3").value = formatMoney1(result);
            var myHidden3 = document.getElementById("hdntxtbxTaksit3").value;
            document.getElementById("remainingbalance3").value = myHidden3;
            //var instAmt11 = document.getElementById("instamount1").value;
            //var instAmt22 = document.getElementById("instamount2").value;
            //var instAmt33 = document.getElementById("instamount3").value;
            //var instAmt44 = document.getElementById("instamount4").value;

            //if (instAmt11 == "") {
            //    instAmt11 = '0.00';
            //}
            //if (instAmt22 == "") {
            //    instAmt22 = '0.00';
            //}

            //if (instAmt33 == "") {
            //    instAmt33 = '0.00';
            //}
            //if (instAmt44 == "") {
            //    instAmt44 = '0.00';
            //}
            //var res1 = parseFloat(instAmt11.replace(',', ''));
            //var res2 = parseFloat(instAmt22.replace(',', ''));
            //var res3 = parseFloat(instAmt33.replace(',', ''));
            //var res4 = parseFloat(instAmt44.replace(',', ''));
            //document.getElementById("txtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            //Money1 = isNaN(parseFloat(instAmt3.replace(/[^0-9\.]+/g, "")));
            //Money2 = isNaN(parseFloat(instPaid3.replace(/[^0-9\.]+/g, "")));
            //if (Money1 == true && Money2 == false) {
            //    Money11 = '0.00';
            //    Money21 = instPaid3;
            //    Money21 = parseFloat(Money21.replace(',', ''));
            //    result = parseFloat(Money11 - Money21).toFixed(2);
            //}
            //else if (Money1 == false && Money2 == true) {
            //    Money11 = instAmt3;
            //    Money21 = '0.00';
            //    Money11 = parseFloat(Money11.replace(',', ''));
            //    result = parseFloat(Money11 - Money21).toFixed(2);
            //}
            //else if (Money1 == true && Money2 == true) {
            //    Money11 = '0.00';
            //    Money21 = '0.00';
            //    result = '0.00';
            //}
            //else if (Money1 == false && Money2 == false) {
            //    //Money11 = parseFloat(Money11.replace(',', ''));
            //    //Money21 = parseFloat(Money21.replace(',', ''));
            //    Money11 = parseFloat(instAmt3.replace(/[^0-9\.]+/g, ""));
            //    Money21 = parseFloat(instPaid3.replace(/[^0-9\.]+/g, ""));
            //    result = (Money11 - Money21).toFixed(2);
            //}

            //document.getElementById("remainingbalance3").value = formatMoney1(result);

            instAmt3 = instAmt3.replace(',', '');
            instPaid3 = instPaid3.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue3.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue3.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue3.value = "Paid";
            }
            var firstChar3 = myHidden3.substr(0, 1);
            if (firstChar3 == '-') {
                document.getElementById("remainingbalance3").readOnly = false;
            }
            else {
                document.getElementById("remainingbalance3").readOnly = false;
            }

        }

        function mytxtamount4() {
            hello4();
            myFunction4();
            document.getElementById('remainingbalance4').disabled = false;

            var instAmt11 = document.getElementById("instamount1").value;
            var instAmt22 = document.getElementById("instamount2").value;
            var instAmt33 = document.getElementById("instamount3").value;
            var instAmt44 = document.getElementById("instamount4").value;

            if (instAmt11 == "") {
                instAmt11 = '0.00';
            }
            if (instAmt22 == "") {
                instAmt22 = '0.00';
            }

            if (instAmt33 == "") {
                instAmt33 = '0.00';
            }
            if (instAmt44 == "") {
                instAmt44 = '0.00';
            }
            var res1 = parseFloat(instAmt11.replace(/,/g, ''));
            var res2 = parseFloat(instAmt22.replace(/,/g, ''));
            var res3 = parseFloat(instAmt33.replace(/,/g, ''));
            var res4 = parseFloat(instAmt44.replace(/,/g, ''));
            document.getElementById("txtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            var instAmt4 = document.getElementById("instamount4").value;
            var instPaid4 = document.getElementById("instamountpaid4").value;
            var paidDue4 = document.getElementById("instpaiddue4");

            Money1 = parseFloat(instAmt4.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid4.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit4").value = formatMoney1(result);
            var myHidden4 = document.getElementById("hdntxtbxTaksit4").value;
            document.getElementById("remainingbalance4").value = myHidden4;

            //var instAmt11 = document.getElementById("instamount1").value;
            //var instAmt22 = document.getElementById("instamount2").value;
            //var instAmt33 = document.getElementById("instamount3").value;
            //var instAmt44 = document.getElementById("instamount4").value;

            //if (instAmt11 == "") {
            //    instAmt11 = '0.00';
            //}
            //if (instAmt22 == "") {
            //    instAmt22 = '0.00';
            //}

            //if (instAmt33 == "") {
            //    instAmt33 = '0.00';
            //}
            //if (instAmt44 == "") {
            //    instAmt44 = '0.00';
            //}
            //var res1 = parseFloat(instAmt11.replace(',', ''));
            //var res2 = parseFloat(instAmt22.replace(',', ''));
            //var res3 = parseFloat(instAmt33.replace(',', ''));
            //var res4 = parseFloat(instAmt44.replace(',', ''));
            //document.getElementById("txtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            //Money1 = isNaN(parseFloat(instAmt4.replace(/[^0-9\.]+/g, "")));
            //Money2 = isNaN(parseFloat(instPaid4.replace(/[^0-9\.]+/g, "")));

            //if (Money1 == true && Money2 == false) {
            //    Money11 = '0.00';
            //    Money21 = instPaid4;
            //    Money21 = parseFloat(Money21.replace(',', ''));
            //    result = parseFloat(Money11 - Money21).toFixed(2);
            //}
            //else if (Money1 == false && Money2 == true) {
            //    Money11 = instAmt4;
            //    Money21 = '0.00';
            //    Money11 = parseFloat(Money11.replace(',', ''));
            //    result = parseFloat(Money11 - Money21).toFixed(2);
            //}
            //else if (Money1 == true && Money2 == true) {
            //    Money11 = '0.00';
            //    Money21 = '0.00';
            //    result = '0.00';
            //}
            //else if (Money1 == false && Money2 == false) {
            //    //Money11 = parseFloat(Money11.replace(',', ''));
            //    //Money21 = parseFloat(Money21.replace(',', ''));
            //    Money11 = parseFloat(instAmt4.replace(/[^0-9\.]+/g, ""));
            //    Money21 = parseFloat(instPaid4.replace(/[^0-9\.]+/g, ""));
            //    result = (Money11 - Money21).toFixed(2);
            //}
            //document.getElementById("remainingbalance4").value = formatMoney1(result);

            instAmt4 = instAmt4.replace(',', '');
            instPaid4 = instPaid4.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue4.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue4.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue4.value = "Paid";
            }
            var firstChar4 = myHidden4.substr(0, 1);
            if (firstChar4 == '-') {
                document.getElementById("remainingbalance4").readOnly = false;
            }
            else {
                document.getElementById("remainingbalance4").readOnly = false;
            }
        }

        //Future mytxtamont
        function Futuremytxtamount1() {
            Futurehello1();
            FuturemyFunction1();
            document.getElementById('txtmanurembal1').disabled = false;

            var instAmt11 = document.getElementById("instmanamount1").value;
            var instAmt22 = document.getElementById("instmanamount2").value;
            var instAmt33 = document.getElementById("instmanamount3").value;
            var instAmt44 = document.getElementById("instmanamount4").value;

            if (instAmt11 == "") {
                instAmt11 = '0.00';
            }
            if (instAmt22 == "") {
                instAmt22 = '0.00';
            }

            if (instAmt33 == "") {
                instAmt33 = '0.00';
            }
            if (instAmt44 == "") {
                instAmt44 = '0.00';
            }
            var res1 = parseFloat(instAmt11.replace(/,/g, ''));
            var res2 = parseFloat(instAmt22.replace(/,/g, ''));
            var res3 = parseFloat(instAmt33.replace(/,/g, ''));
            var res4 = parseFloat(instAmt44.replace(/,/g, ''));
            document.getElementById("futuretxtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            //document.getElementById('txtstrRemaingBlnce1').removeAttribute('readonly');
            var instAmt1 = document.getElementById("instmanamount1").value;
            var instPaid1 = document.getElementById("instmanamtpaid1").value;
            var paidDue1 = document.getElementById("ddlmaninstpaiddue1");
            Money1 = parseFloat(instAmt1.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid1.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);

            document.getElementById("futurehdntxtbxTaksit1").value = futureformatMoney1(result);
            var myHidden = document.getElementById("futurehdntxtbxTaksit1").value;
            document.getElementById("txtmanurembal1").value = myHidden;

            instAmt1 = instAmt1.replace(',', '');
            instPaid1 = instPaid1.replace(',', '');

            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue1.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue1.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue1.value = "Paid";
            }
            var firstChar = myHidden.substr(0, 1);
            if (firstChar == '-') {
                document.getElementById("txtmanurembal1").readOnly = false;
            }
            else {
                document.getElementById("txtmanurembal1").readOnly = false;
            }
        }

        function Futuremytxtamount2() {
            Futurehello2();
            FuturemyFunction2();
            document.getElementById('txtmanurembal2').disabled = false;

            var instAmt11 = document.getElementById("instmanamount1").value;
            var instAmt22 = document.getElementById("instmanamount2").value;
            var instAmt33 = document.getElementById("instmanamount3").value;
            var instAmt44 = document.getElementById("instmanamount4").value;

            if (instAmt11 == "") {
                instAmt11 = '0.00';
            }
            if (instAmt22 == "") {
                instAmt22 = '0.00';
            }

            if (instAmt33 == "") {
                instAmt33 = '0.00';
            }
            if (instAmt44 == "") {
                instAmt44 = '0.00';
            }
            var res1 = parseFloat(instAmt11.replace(/,/g, ''));
            var res2 = parseFloat(instAmt22.replace(/,/g, ''));
            var res3 = parseFloat(instAmt33.replace(/,/g, ''));
            var res4 = parseFloat(instAmt44.replace(/,/g, ''));
            document.getElementById("futuretxtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            var instAmt2 = document.getElementById("instmanamount2").value;
            var instPaid2 = document.getElementById("instmanamtpaid2").value;
            var paidDue2 = document.getElementById("ddlmaninstpaiddue2");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("futurehdntxtbxTaksit2").value = futureformatMoney1(result);
            var myHidden2 = document.getElementById("futurehdntxtbxTaksit2").value;
            document.getElementById("txtmanurembal2").value = myHidden2;

            instAmt2 = instAmt2.replace(',', '');
            instPaid2 = instPaid2.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue2.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue2.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue2.value = "Paid";
            }
            var firstChar2 = myHidden2.substr(0, 1);
            if (firstChar2 == '-') {
                document.getElementById("txtmanurembal2").readOnly = false;
            }
            else {
                document.getElementById("txtmanurembal2").readOnly = false;
            }
        }

        function Futuremytxtamount3() {
            Futurehello3();
            FuturemyFunction3();
            document.getElementById('txtmanurembal3').disabled = false;

            var instAmt11 = document.getElementById("instmanamount1").value;
            var instAmt22 = document.getElementById("instmanamount2").value;
            var instAmt33 = document.getElementById("instmanamount3").value;
            var instAmt44 = document.getElementById("instmanamount4").value;

            if (instAmt11 == "") {
                instAmt11 = '0.00';
            }
            if (instAmt22 == "") {
                instAmt22 = '0.00';
            }

            if (instAmt33 == "") {
                instAmt33 = '0.00';
            }
            if (instAmt44 == "") {
                instAmt44 = '0.00';
            }
            var res1 = parseFloat(instAmt11.replace(/,/g, ''));
            var res2 = parseFloat(instAmt22.replace(/,/g, ''));
            var res3 = parseFloat(instAmt33.replace(/,/g, ''));
            var res4 = parseFloat(instAmt44.replace(/,/g, ''));
            document.getElementById("futuretxtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            var instAmt3 = document.getElementById("instmanamount3").value;
            var instPaid3 = document.getElementById("instmanamtpaid3").value;
            var paidDue3 = document.getElementById("ddlmaninstpaiddue3");
            Money1 = parseFloat(instAmt3.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid3.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("futurehdntxtbxTaksit3").value = futureformatMoney1(result);
            var myHidden3 = document.getElementById("futurehdntxtbxTaksit3").value;
            document.getElementById("txtmanurembal3").value = myHidden3;
            //var instAmt11 = document.getElementById("instamount1").value;
            //var instAmt22 = document.getElementById("instamount2").value;
            //var instAmt33 = document.getElementById("instamount3").value;
            //var instAmt44 = document.getElementById("instamount4").value;

            //if (instAmt11 == "") {
            //    instAmt11 = '0.00';
            //}
            //if (instAmt22 == "") {
            //    instAmt22 = '0.00';
            //}

            //if (instAmt33 == "") {
            //    instAmt33 = '0.00';
            //}
            //if (instAmt44 == "") {
            //    instAmt44 = '0.00';
            //}
            //var res1 = parseFloat(instAmt11.replace(',', ''));
            //var res2 = parseFloat(instAmt22.replace(',', ''));
            //var res3 = parseFloat(instAmt33.replace(',', ''));
            //var res4 = parseFloat(instAmt44.replace(',', ''));
            //document.getElementById("txtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            //Money1 = isNaN(parseFloat(instAmt3.replace(/[^0-9\.]+/g, "")));
            //Money2 = isNaN(parseFloat(instPaid3.replace(/[^0-9\.]+/g, "")));
            //if (Money1 == true && Money2 == false) {
            //    Money11 = '0.00';
            //    Money21 = instPaid3;
            //    Money21 = parseFloat(Money21.replace(',', ''));
            //    result = parseFloat(Money11 - Money21).toFixed(2);
            //}
            //else if (Money1 == false && Money2 == true) {
            //    Money11 = instAmt3;
            //    Money21 = '0.00';
            //    Money11 = parseFloat(Money11.replace(',', ''));
            //    result = parseFloat(Money11 - Money21).toFixed(2);
            //}
            //else if (Money1 == true && Money2 == true) {
            //    Money11 = '0.00';
            //    Money21 = '0.00';
            //    result = '0.00';
            //}
            //else if (Money1 == false && Money2 == false) {
            //    //Money11 = parseFloat(Money11.replace(',', ''));
            //    //Money21 = parseFloat(Money21.replace(',', ''));
            //    Money11 = parseFloat(instAmt3.replace(/[^0-9\.]+/g, ""));
            //    Money21 = parseFloat(instPaid3.replace(/[^0-9\.]+/g, ""));
            //    result = (Money11 - Money21).toFixed(2);
            //}

            //document.getElementById("remainingbalance3").value = formatMoney1(result);

            instAmt3 = instAmt3.replace(',', '');
            instPaid3 = instPaid3.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue3.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue3.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue3.value = "Paid";
            }
            var firstChar3 = myHidden3.substr(0, 1);
            if (firstChar3 == '-') {
                document.getElementById("txtmanurembal3").readOnly = false;
            }
            else {
                document.getElementById("txtmanurembal3").readOnly = false;
            }

        }

        function Futuremytxtamount4() {
            Futurehello4();
            FuturemyFunction4();
            document.getElementById('txtmanurembal4').disabled = false;

            var instAmt11 = document.getElementById("instmanamount1").value;
            var instAmt22 = document.getElementById("instmanamount2").value;
            var instAmt33 = document.getElementById("instmanamount3").value;
            var instAmt44 = document.getElementById("instmanamount4").value;

            if (instAmt11 == "") {
                instAmt11 = '0.00';
            }
            if (instAmt22 == "") {
                instAmt22 = '0.00';
            }

            if (instAmt33 == "") {
                instAmt33 = '0.00';
            }
            if (instAmt44 == "") {
                instAmt44 = '0.00';
            }
            var res1 = parseFloat(instAmt11.replace(/,/g, ''));
            var res2 = parseFloat(instAmt22.replace(/,/g, ''));
            var res3 = parseFloat(instAmt33.replace(/,/g, ''));
            var res4 = parseFloat(instAmt44.replace(/,/g, ''));
            document.getElementById("futuretxtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            var instAmt4 = document.getElementById("instmanamount4").value;
            var instPaid4 = document.getElementById("instmanamtpaid4").value;
            var paidDue4 = document.getElementById("ddlmaninstpaiddue4");

            Money1 = parseFloat(instAmt4.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid4.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("futurehdntxtbxTaksit4").value = futureformatMoney1(result);
            var myHidden4 = document.getElementById("futurehdntxtbxTaksit4").value;
            document.getElementById("txtmanurembal4").value = myHidden4;

            //var instAmt11 = document.getElementById("instamount1").value;
            //var instAmt22 = document.getElementById("instamount2").value;
            //var instAmt33 = document.getElementById("instamount3").value;
            //var instAmt44 = document.getElementById("instamount4").value;

            //if (instAmt11 == "") {
            //    instAmt11 = '0.00';
            //}
            //if (instAmt22 == "") {
            //    instAmt22 = '0.00';
            //}

            //if (instAmt33 == "") {
            //    instAmt33 = '0.00';
            //}
            //if (instAmt44 == "") {
            //    instAmt44 = '0.00';
            //}
            //var res1 = parseFloat(instAmt11.replace(',', ''));
            //var res2 = parseFloat(instAmt22.replace(',', ''));
            //var res3 = parseFloat(instAmt33.replace(',', ''));
            //var res4 = parseFloat(instAmt44.replace(',', ''));
            //document.getElementById("txtAnnualTaxAmount").innerHTML = parseFloat(res1 + res2 + res3 + res4).toFixed(2);

            //Money1 = isNaN(parseFloat(instAmt4.replace(/[^0-9\.]+/g, "")));
            //Money2 = isNaN(parseFloat(instPaid4.replace(/[^0-9\.]+/g, "")));

            //if (Money1 == true && Money2 == false) {
            //    Money11 = '0.00';
            //    Money21 = instPaid4;
            //    Money21 = parseFloat(Money21.replace(',', ''));
            //    result = parseFloat(Money11 - Money21).toFixed(2);
            //}
            //else if (Money1 == false && Money2 == true) {
            //    Money11 = instAmt4;
            //    Money21 = '0.00';
            //    Money11 = parseFloat(Money11.replace(',', ''));
            //    result = parseFloat(Money11 - Money21).toFixed(2);
            //}
            //else if (Money1 == true && Money2 == true) {
            //    Money11 = '0.00';
            //    Money21 = '0.00';
            //    result = '0.00';
            //}
            //else if (Money1 == false && Money2 == false) {
            //    //Money11 = parseFloat(Money11.replace(',', ''));
            //    //Money21 = parseFloat(Money21.replace(',', ''));
            //    Money11 = parseFloat(instAmt4.replace(/[^0-9\.]+/g, ""));
            //    Money21 = parseFloat(instPaid4.replace(/[^0-9\.]+/g, ""));
            //    result = (Money11 - Money21).toFixed(2);
            //}
            //document.getElementById("remainingbalance4").value = formatMoney1(result);

            instAmt4 = instAmt4.replace(',', '');
            instPaid4 = instPaid4.replace(',', '');
            if (parseFloat(Money1) == parseFloat(Money2)) {
                paidDue4.value = "Paid";
            } else if (parseFloat(Money1) > parseFloat(Money2)) {
                paidDue4.value = "Due";
            }
            else if (parseFloat(Money1) < parseFloat(Money2)) {
                paidDue4.value = "Paid";
            }
            var firstChar4 = myHidden4.substr(0, 1);
            if (firstChar4 == '-') {
                document.getElementById("txtmanurembal4").readOnly = false;
            }
            else {
                document.getElementById("txtmanurembal4").readOnly = false;
            }
        }

        function myremamount1() {
            RemBalance1();
            myFunctionRemBalance1();
        }
        function myremamount2() {
            RemBalance2();
            myFunctionRemBalance2();
        }
        function myremamount3() {
            RemBalance3();
            myFunctionRemBalance3();
        }
        function myremamount4() {
            RemBalance4();
            myFunctionRemBalance4();
        }

        //Future myreamount
        function futuremyremamount1() {
            futureRemBalance1();
            futuremyFunctionRemBalance1();
        }
        function futuremyremamount2() {
            futureRemBalance2();
            futuremyFunctionRemBalance2();
        }
        function futuremyremamount3() {
            futureRemBalance3();
            futuremyFunctionRemBalance3();
        }
        function futuremyremamount4() {
            futureRemBalance4();
            futuremyFunctionRemBalance4();
        }


        function myFunction1() {
            document.getElementById("instamount1").value = document.getElementById("w").innerText;
        }
        function myFunction2() {
            document.getElementById("instamount2").value = document.getElementById("x").innerText;
        }
        function myFunction3() {
            document.getElementById("instamount3").value = document.getElementById("y").innerText;
        }
        function myFunction4() {
            document.getElementById("instamount4").value = document.getElementById("z").innerText;
        }

        //Future Myfunction
        function FuturemyFunction1() {
            document.getElementById("instmanamount1").value = document.getElementById("fw").innerText;
        }
        function FuturemyFunction2() {
            document.getElementById("instmanamount2").value = document.getElementById("fx").innerText;
        }
        function FuturemyFunction3() {
            document.getElementById("instmanamount3").value = document.getElementById("fy").innerText;
        }
        function FuturemyFunction4() {
            document.getElementById("instmanamount4").value = document.getElementById("fz").innerText;
        }

        function formatMoney1(n, c, instamount1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instamount1 = instamount1 == undefined ? "." : instamount1,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instamount1 + Math.abs(n - i).toFixed(c).slice(2) : "");
        };
        function formatMoney2(n, c, instamount2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instamount2 = instamount2 == undefined ? "." : instamount2,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instamount2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoney3(n, c, instamount3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instamount3 = instamount3 == undefined ? "." : instamount3,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instamount3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoney4(n, c, instamount4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instamount4 = instamount4 == undefined ? "." : instamount4,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instamount4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        //Future FormatMoney
        function futureformatMoney1(n, c, instmanamount1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instmanamount1 = instmanamount1 == undefined ? "." : instmanamount1,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instmanamount1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoney2(n, c, instmanamount2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instmanamount2 = instmanamount2 == undefined ? "." : instmanamount2,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instmanamount2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoney3(n, c, instmanamount3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instmanamount3 = instmanamount3 == undefined ? "." : instmanamount3,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instmanamount3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoney4(n, c, instmanamount4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              instmanamount4 = instmanamount4 == undefined ? "." : instmanamount4,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? instmanamount4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function hello1() {
            document.getElementById("w").innerText = formatMoney1(document.getElementById("instamount1").value);

        }
        function hello2() {
            document.getElementById("x").innerText = formatMoney2(document.getElementById("instamount2").value);

        }
        function hello3() {
            document.getElementById("y").innerText = formatMoney3(document.getElementById("instamount3").value);

        }
        function hello4() {
            document.getElementById("z").innerText = formatMoney4(document.getElementById("instamount4").value);

        }

        //Future Hello
        function Futurehello1() {
            document.getElementById("fw").innerText = futureformatMoney1(document.getElementById("instmanamount1").value);
        }
        function Futurehello2() {
            document.getElementById("fx").innerText = futureformatMoney2(document.getElementById("instmanamount2").value);
        }
        function Futurehello3() {
            document.getElementById("fy").innerText = futureformatMoney3(document.getElementById("instmanamount3").value);
        }
        function Futurehello4() {
            document.getElementById("fz").innerText = futureformatMoney4(document.getElementById("instmanamount4").value);
        }
    </script>

    <script type="text/javascript">
        //Remaining Balance
        function myFunctionRemBalance1() {
            document.getElementById("remainingbalance1").value = document.getElementById("i").innerText;
        }
        function myFunctionRemBalance2() {
            document.getElementById("remainingbalance2").value = document.getElementById("j").innerText;
        }
        function myFunctionRemBalance3() {
            document.getElementById("remainingbalance3").value = document.getElementById("k").innerText;
        }
        function myFunctionRemBalance4() {
            document.getElementById("remainingbalance4").value = document.getElementById("l").innerText;
        }

        //Future myFunctionRemBalance
        function futuremyFunctionRemBalance1() {
            document.getElementById("txtmanurembal1").value = document.getElementById("fi").innerText;
        }
        function futuremyFunctionRemBalance2() {
            document.getElementById("txtmanurembal2").value = document.getElementById("fj").innerText;
        }
        function futuremyFunctionRemBalance3() {
            document.getElementById("txtmanurembal3").value = document.getElementById("fk").innerText;
        }
        function futuremyFunctionRemBalance4() {
            document.getElementById("txtmanurembal4").value = document.getElementById("fl").innerText;
        }

        function formatMoneyRemBalance1(n, c, remainingbalance1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              remainingbalance1 = remainingbalance1 == undefined ? "." : remainingbalance1,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? remainingbalance1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyRemBalance2(n, c, remainingbalance2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              remainingbalance2 = remainingbalance2 == undefined ? "." : remainingbalance2,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? remainingbalance2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyRemBalance3(n, c, remainingbalance3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              remainingbalance3 = remainingbalance3 == undefined ? "." : remainingbalance3,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? remainingbalance3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyRemBalance4(n, c, remainingbalance4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              remainingbalance4 = remainingbalance4 == undefined ? "." : txtstrRemaingBlnce4,
              n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? remainingbalance4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        //Future formatMoneyRemBalance
        function futureformatMoneyRemBalance1(n, c, txtmanurembal1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtmanurembal1 = txtmanurembal1 == undefined ? "." : txtmanurembal1,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtmanurembal1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoneyRemBalance2(n, c, txtmanurembal2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtmanurembal2 = txtmanurembal2 == undefined ? "." : txtmanurembal2,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtmanurembal2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoneyRemBalance3(n, c, txtmanurembal3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtmanurembal3 = txtmanurembal3 == undefined ? "." : txtmanurembal3,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtmanurembal3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function futureformatMoneyRemBalance4(n, c, txtmanurembal4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtmanurembal4 = txtmanurembal4 == undefined ? "." : txtmanurembal4,
               n = n.replace(/,/g, ''),
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtmanurembal4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function RemBalance1() {
            document.getElementById("i").innerText = formatMoneyRemBalance1(document.getElementById("remainingbalance1").value);
        }
        function RemBalance2() {
            document.getElementById("j").innerText = formatMoneyRemBalance2(document.getElementById("remainingbalance2").value);
        }
        function RemBalance3() {
            document.getElementById("k").innerText = formatMoneyRemBalance3(document.getElementById("remainingbalance3").value);
        }
        function RemBalance4() {
            document.getElementById("l").innerText = formatMoneyRemBalance4(document.getElementById("remainingbalance4").value);
        }

        //Future RemBalance1
        function futureRemBalance1() {
            document.getElementById("fi").innerText = futureformatMoneyRemBalance1(document.getElementById("txtmanurembal1").value);
        }
        function futureRemBalance2() {
            document.getElementById("fj").innerText = futureformatMoneyRemBalance2(document.getElementById("txtmanurembal2").value);
        }
        function futureRemBalance3() {
            document.getElementById("fk").innerText = futureformatMoneyRemBalance3(document.getElementById("txtmanurembal3").value);
        }
        function futureRemBalance4() {
            document.getElementById("fl").innerText = futureformatMoneyRemBalance4(document.getElementById("txtmanurembal4").value);
        }
    </script>

    <script type="text/javascript">
        //$(function () {
        //    $("#txtdate1,#txtdate2,#instdate1,#instdate2,#instdate3,#instdate4,#delinq1,#delinq2,#delinq3,#delinq4,#discdate1,#discdate2,#discdate3,#discdate4,#nextbilldate1,#txtbillstartdate,#txtbillenddate,#txtpayoffgood,#txtinitialinstall,#txtdatetaxsale,#txtlastdayred").datepicker({
        //        changeMonth: true,
        //        changeYear: true,
        //        format: "mm/dd/yyyy",
        //        language: "tr"
        //    });
        //});

        function checkReqFields1(year, element, ev) {
            var startyear = document.getElementById("txtTaxYear").value;
            var errors = {
                txtTaxYear: '',
                txtEndYear: '',
            };

            if (year != "") {
                if (year < startyear) {
                    document.getElementById(element.id).value = '';
                    setTimeout(function () { document.getElementById(element.id).focus();; }, 1);
                    //document.getElementById(element.id).focus();
                    alert("End Year should be greater than Tax year");
                    return;
                }
            }
        }
        function checkReqFields(year, element, ev) {
            var startyear = document.getElementById("txtEndYear").value;
            var errors = {
                txtTaxYear: '',
                txtEndYear: '',
            };

            if (startyear != "") {
                if (year > startyear) {
                    document.getElementById(element.id).value = '';
                    setTimeout(function () { document.getElementById("txtEndYear").focus(); }, 1);
                    //document.getElementById("txtEndYear").focus();
                    alert("End Year should be greater than Tax year");
                    return;
                }
            }
        }

    </script>

    <script type="text/javascript">
        function getTarget(event) {
            var e = event || window.event;

            if (e.target)
                return e.target;
            else
                return e.srcElement;
        }
        function showError(node, message) {
            alert(message);
            // focus & select element with error (Mozilla, Safari & Chrome require brief delay before moving focus) 
            setTimeout(function () { node.focus(); node.select(); }, 1);
        }

        function isvalidateDate(node) {

            var errorMessage = "Date needs to be in date format, such as MM/DD/YYYY.";
            if (node != "") {

                if (node.length !== 10) {
                    return errorMessage;
                }
                if (node.substring(2, 3) !== '/' || node.substring(5, 6) !== '/') {
                    return errorMessage;
                }
                //if (node.value.substring(2, 0) == '2' && node.value.substring(5, 3) >= 30) {
                //    return errorMessage;
                //}           
                // try parsing as date using JavaScript Date constructor 
                var dateValue = new Date(node.replace(/-/g, "/"));
                if (isFinite(dateValue)) {
                    // if two-digit year, guess at correct century                   
                    if (node.match(/\D\d{1,2}$/) && dateValue.getFullYear() < (new Date().getFullYear() - 96)) {
                        dateValue.setFullYear(dateValue.getFullYear() + 100);
                    }
                    // format as mm/dd/yyyy 
                    node = (dateValue.getMonth() + 1) + "/" + dateValue.getVarDate() + "/" + dateValue.getFullYear();
                    return "";
                }
                else {
                    return errorMessage;
                    node = "";
                }
            }
            return "";
        }
        function checkDate(val, event) {
            //alert(val.value);
            var node = getTarget(event);

            if (node) {
                var result = isvalidateDate(node.value);
                if (result != "") {
                    showError(node.value, result);
                    //val.value = "";
                    setTimeout(function () { node.focus(); node.select(); }, 1);
                } // endif 
            } // endif 


            return stopPropagation(event);
        }


        //installmentdetails
        function checkINSTDEDate1() {
            var errormsg = "Delinquent date must be less than 12 months from the installment date...";
            var inst1 = document.getElementById("instdate1").value;
            var delinq1 = document.getElementById("delinq1").value;

            var a = new Date(inst1);
            var b = new Date(delinq1);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();


            if (b > a) {

                if (months >= 12) {
                    document.getElementById('delinq1').value = "";
                    setTimeout(function () { document.getElementById("delinq1").focus(); }, 1);
                    //document.getElementById("delinq1").focus();
                    alert(errormsg);
                    return;
                }
            }
            else if (b < a) {
                document.getElementById('delinq1').value = "";
                setTimeout(function () { document.getElementById("delinq1").focus(); }, 1);
                //document.getElementById("delinq1").focus();
                alert('Delinquent date must be after the installment date...');
                return;
            }
        }

        function checkINSTDEDate2() {
            var errormsg = "Delinquent date must be less than 12 months from the installment date...";
            var inst2 = document.getElementById("instdate2").value;
            var delinq2 = document.getElementById("delinq2").value;

            var a = new Date(inst2);
            var b = new Date(delinq2);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            if (b > a) {

                if (months >= 12) {
                    document.getElementById('delinq2').value = "";
                    setTimeout(function () { document.getElementById("delinq2").focus(); }, 1);
                    //document.getElementById("delinq2").focus();
                    alert(errormsg);
                    return;
                }
            }
            else if (b < a) {
                document.getElementById('delinq2').value = "";
                setTimeout(function () { document.getElementById("delinq2").focus(); }, 1);
                //document.getElementById("delinq2").focus();
                alert('Delinquent date must be after the installment date...');
                return;
            }
        }

        function checkINSTDEDate3() {
            var errormsg = "Delinquent date must be less than 12 months from the installment date...";
            var inst3 = document.getElementById("instdate3").value;
            var delinq3 = document.getElementById("delinq3").value;

            var a = new Date(inst3);
            var b = new Date(delinq3);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            if (b > a) {

                if (months >= 12) {
                    document.getElementById('delinq3').value = "";
                    setTimeout(function () { document.getElementById("delinq3").focus(); }, 1);
                    //document.getElementById("delinq3").focus();
                    alert(errormsg);
                    return;
                }
            }
            else if (b < a) {
                document.getElementById('delinq3').value = "";
                setTimeout(function () { document.getElementById("delinq3").focus(); }, 1);
                //document.getElementById("delinq3").focus();
                alert('Delinquent date must be after the installment date...');
                return;
            }
        }

        function checkINSTDEDate4() {
            var errormsg = "Delinquent date must be less than 12 months from the installment date...";
            var inst4 = document.getElementById("instdate4").value;
            var delinq4 = document.getElementById("delinq4").value;

            var a = new Date(inst4);
            var b = new Date(delinq4);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            if (b > a) {

                if (months >= 12) {
                    document.getElementById('delinq4').value = "";
                    setTimeout(function () { document.getElementById("delinq4").focus(); }, 1);
                    //document.getElementById("delinq4").focus();
                    alert(errormsg);
                    return;
                }
            }
            else if (b < a) {
                document.getElementById('delinq4').value = "";
                setTimeout(function () { document.getElementById("delinq4").focus(); }, 1);
                //document.getElementById("delinq4").focus();
                alert('Delinquent date must be after the installment date...');
                return;
            }
        }



        function checkmanualDEinstdate2() {
            var errormsg = "Installment dates must be within 12 months of each other...";
            var inst1 = document.getElementById("instdate1").value;
            var inst2 = document.getElementById("instdate2").value;

            var a = new Date(inst1);
            var b = new Date(inst2);


            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            if (b > a) {
                if (months >= 12) {
                    document.getElementById('instdate2').value = "";
                    setTimeout(function () { document.getElementById("instdate2").focus(); }, 1);
                    //document.getElementById("instdate2").focus();
                    alert(errormsg);
                    return;
                }
            }
            if (b < a) {
                document.getElementById('instdate2').value = "";
                setTimeout(function () { document.getElementById("instdate2").focus(); }, 1);
                //document.getElementById("instdate2").focus();
                alert('Installment date2 must be after Installment date1...');
                return;
            }
        }

        function checkmanualDEinstdate3() {
            var errormsg = "Installment dates must be within 12 months of each other...";
            var inst1 = document.getElementById("instdate1").value;
            var inst2 = document.getElementById("instdate2").value;
            var inst3 = document.getElementById("instdate3").value;

            var a = new Date(inst1);
            var b = new Date(inst2);
            var c = new Date(inst3);

            //var months = (c.getFullYear() - b.getFullYear()) * 12;
            //months += c.getMonth() - b.getMonth();

            var months = (c.getFullYear() - a.getFullYear()) * 12;
            months += c.getMonth() + a.getMonth();

            if (c > a) {
                if (months >= 12) {
                    document.getElementById('instdate3').value = "";
                    setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                    //document.getElementById("instdate3").focus();
                    alert(errormsg);
                    return;
                }
            }

            if (c < b || c < a) {
                document.getElementById('instdate3').value = "";
                setTimeout(function () { document.getElementById("instdate3").focus(); }, 1);
                //document.getElementById("instdate3").focus();
                alert('Installment date3 must be after Installment date2...');
                return;
            }
        }

        function checkmanualDEinstdate4() {
            var errormsg = "Installment dates must be within 12 months of each other...";
            var inst1 = document.getElementById("instdate1").value;
            var inst2 = document.getElementById("instdate2").value;
            var inst3 = document.getElementById("instdate3").value;
            var inst4 = document.getElementById("instdate4").value;

            var a = new Date(inst1);
            var b = new Date(inst2);
            var c = new Date(inst3);
            var d = new Date(inst4);

            var months = (d.getFullYear() - a.getFullYear()) * 12;
            months += d.getMonth() - a.getMonth();

            //var months = (d.getFullYear() - c.getFullYear() - b.getFullYear() - a.getFullYear()) * 12;
            //months += d.getMonth() - c.getMonth() - b.getMonth() - a.getMonth();
            if (d > a) {
                if (months >= 12) {
                    document.getElementById('instdate4').value = "";
                    setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                    //document.getElementById("instdate4").focus();
                    alert(errormsg);
                    return;
                }
            }

            if (d < c || d < b || d < a) {
                document.getElementById('instdate4').value = "";
                setTimeout(function () { document.getElementById("instdate4").focus(); }, 1);
                //document.getElementById("instdate4").focus();
                alert('Installment date4 must be after Installment date3...');
                return;
            }
        }

        //Discount Dates
        function checkDISDate1() {
            var errormsg = "Discount date must be less than 12 months from the installment date...";
            var errormsg1 = "Discount date must be before the delinquent date...";
            var errormsg2 = "Discount date must be between the delinquent date and installment date...";
            var inst1 = document.getElementById("instdate1").value;
            var delinq1 = document.getElementById("delinq1").value;
            var disc1 = document.getElementById("discdate1").value;

            var a = new Date(delinq1);
            var b = new Date(disc1);
            var c = new Date(inst1);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            var months11 = (c.getFullYear() - b.getFullYear()) * 12;
            months11 += c.getMonth() - b.getMonth();

            if (b > a) {
                document.getElementById('discdate1').value = "";
                setTimeout(function () { document.getElementById("discdate1").focus(); }, 1);
                //document.getElementById("discdate1").focus();
                alert(errormsg1);
                return;
            }
            if (c > b) {

                if (months11 >= 12) {
                    document.getElementById('discdate1').value = "";
                    setTimeout(function () { document.getElementById("discdate1").focus(); }, 1);
                    //document.getElementById("discdate1").focus();
                    alert(errormsg);
                    return;
                }
            }
        }

        function checkDISDate2() {
            var errormsg = "Discount date must be less than 12 months from the installment date...";
            var errormsg2 = "Discount date must be before the delinquent date...";
            var inst2 = document.getElementById("instdate2").value;
            var delinq2 = document.getElementById("delinq2").value;
            var disc2 = document.getElementById("discdate2").value;

            var a = new Date(delinq2);
            var b = new Date(disc2);
            var c = new Date(inst2);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            var months11 = (c.getFullYear() - b.getFullYear()) * 12;
            months11 += c.getMonth() - b.getMonth();

            if (b > a) {
                document.getElementById('discdate2').value = "";
                setTimeout(function () { document.getElementById("discdate2").focus(); }, 1);
                //document.getElementById("discdate2").focus();
                alert(errormsg2);
                return;
            }
            if (c > b) {

                if (months11 >= 12) {
                    document.getElementById('discdate2').value = "";
                    setTimeout(function () { document.getElementById("discdate2").focus(); }, 1);
                    //document.getElementById("discdate2").focus();
                    alert(errormsg);
                    return;
                }
            }
        }

        function checkDISDate3() {
            var errormsg = "Discount date must be less than 12 months from the installment date...";
            var errormsg3 = "Discount date must be before the delinquent date...";
            var inst3 = document.getElementById("instdate3").value;
            var delinq3 = document.getElementById("delinq3").value;
            var disc3 = document.getElementById("discdate3").value;

            var a = new Date(delinq3);
            var b = new Date(disc3);
            var c = new Date(inst3);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            var months11 = (c.getFullYear() - b.getFullYear()) * 12;
            months11 += c.getMonth() - b.getMonth();

            if (b > a) {
                document.getElementById('discdate3').value = "";
                setTimeout(function () { document.getElementById("discdate3").focus(); }, 1);
                //document.getElementById("discdate3").focus();
                alert(errormsg3);
                return;
            }
            if (c > b) {

                if (months11 >= 12) {
                    document.getElementById('discdate3').value = "";
                    setTimeout(function () { document.getElementById("discdate3").focus(); }, 1);
                    //document.getElementById("discdate3").focus();
                    alert(errormsg);
                    return;
                }
            }
        }

        function checkDISDate4() {
            var errormsg = "Discount date must be less than 12 months from the installment date...";
            var errormsg4 = "Discount date must be before the delinquent date...";
            var inst4 = document.getElementById("instdate4").value;
            var delinq4 = document.getElementById("delinq4").value;
            var disc4 = document.getElementById("discdate4").value;

            var a = new Date(delinq4);
            var b = new Date(disc4);
            var c = new Date(inst4);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            var months11 = (c.getFullYear() - b.getFullYear()) * 12;
            months11 += c.getMonth() - b.getMonth();

            if (b > a) {
                document.getElementById('discdate4').value = "";
                setTimeout(function () { document.getElementById("discdate4").focus(); }, 1);
                //document.getElementById("discdate4").focus();
                alert(errormsg4);
                return;
            }
            if (c > b) {

                if (months11 >= 12) {
                    document.getElementById('discdate4').value = "";
                    setTimeout(function () { document.getElementById("discdate4").focus(); }, 1);
                    //document.getElementById("discdate4").focus();
                    alert(errormsg);
                    return;
                }
            }
        }

        //future tax
        function checkDate1() {
            var errormsg = "Delinquent date must be less than 12 months from the installment date...";
            var inst1 = document.getElementById("txtmaninstdate1").value;
            var delinq1 = document.getElementById("txtmandeliqdate1").value;

            var a = new Date(inst1);
            var b = new Date(delinq1);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            if (b > a) {

                if (months >= 12) {
                    document.getElementById('txtmandeliqdate1').value = "";
                    setTimeout(function () { document.getElementById("txtmandeliqdate1").focus(); }, 1);
                    //document.getElementById("txtmandeliqdate1").focus();
                    alert(errormsg);
                    return;
                }
            }
            else if (b < a) {
                document.getElementById('txtmandeliqdate1').value = "";
                setTimeout(function () { document.getElementById("txtmandeliqdate1").focus(); }, 1);
                //document.getElementById("txtmandeliqdate1").focus();
                alert('Delinquent date must be after the installment date...');
                return;
            }
        }

        function checkDate2() {
            var errormsg = "Delinquent date must be less than 12 months from the installment date...";
            var inst2 = document.getElementById("txtmaninstdate2").value;
            var delinq2 = document.getElementById("txtmandeliqdate2").value;

            var a = new Date(inst2);
            var b = new Date(delinq2);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            if (b > a) {

                if (months >= 12) {
                    document.getElementById('txtmandeliqdate2').value = "";
                    setTimeout(function () { document.getElementById("txtmandeliqdate2").focus(); }, 1);
                    //document.getElementById("txtmandeliqdate2").focus();
                    alert(errormsg);
                    return;
                }
            }
            else if (b < a) {
                document.getElementById('txtmandeliqdate2').value = "";
                setTimeout(function () { document.getElementById("txtmandeliqdate2").focus(); }, 1);
                //document.getElementById("txtmandeliqdate2").focus();
                alert('Delinquent date must be after the installment date...');
                return;
            }
        }

        function checkDate3() {
            var errormsg = "Delinquent date must be less than 12 months from the installment date...";
            var inst3 = document.getElementById("txtmaninstdate3").value;
            var delinq3 = document.getElementById("txtmandeliqdate3").value;

            var a = new Date(inst3);
            var b = new Date(delinq3);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            if (b > a) {

                if (months >= 12) {
                    document.getElementById('txtmandeliqdate3').value = "";
                    setTimeout(function () { document.getElementById("txtmandeliqdate3").focus(); }, 1);
                    //document.getElementById("txtmandeliqdate3").focus();
                    alert(errormsg);
                    return;
                }
            }
            else if (b < a) {
                document.getElementById('txtmandeliqdate3').value = "";
                setTimeout(function () { document.getElementById("txtmandeliqdate3").focus(); }, 1);
                //document.getElementById("txtmandeliqdate3").focus();
                alert('Delinquent date must be after the installment date...');
                return;
            }
        }

        function checkDate4() {
            var errormsg = "Installment dates must be within 12 months of each other...";
            var inst4 = document.getElementById("txtmaninstdate4").value;
            var delinq4 = document.getElementById("txtmandeliqdate4").value;

            var a = new Date(inst3);
            var b = new Date(delinq3);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            if (b > a) {

                if (months >= 12) {
                    document.getElementById('txtmandeliqdate4').value = "";
                    setTimeout(function () { document.getElementById("txtmandeliqdate3").focus(); }, 1);
                    //document.getElementById("txtmandeliqdate4").focus();
                    alert(errormsg);
                    return;
                }
            }
            else if (b < a) {
                document.getElementById('txtmandeliqdate4').value = "";
                setTimeout(function () { document.getElementById("txtmandeliqdate3").focus(); }, 1);
                //document.getElementById("txtmandeliqdate4").focus();
                alert('Delinquent date must be after the installment date...');
                return;
            }
        }

        function checkNextBillDate() {
            var inst = document.getElementById("instdate1").value;
            var nextbill = document.getElementById("nextbilldate1").value;

            var a = new Date(inst);
            var b = new Date(nextbill);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();


            if (b < a) {
                document.getElementById('nextbilldate1').value = "";
                setTimeout(function () { document.getElementById("nextbilldate1").focus(); }, 1);
                //document.getElementById("nextbilldate1").focus();
                alert('Next bill date1 must be after the installment date...');
                return;
            }
        }


        function checkmanualinstdate2() {
            var errormsg = "Installment dates must be within 12 months of each other...";
            var inst1 = document.getElementById("txtmaninstdate1").value;
            var inst2 = document.getElementById("txtmaninstdate2").value;

            var a = new Date(inst1);
            var b = new Date(inst2);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            if (b > a) {
                if (months >= 12) {
                    document.getElementById('txtmaninstdate2').value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate2").focus(); }, 1);
                    //document.getElementById("txtmaninstdate2").focus();
                    alert(errormsg);
                    return;
                }
            }

            if (b < a) {
                document.getElementById('txtmaninstdate2').value = "";
                setTimeout(function () { document.getElementById("txtmaninstdate2").focus(); }, 1);
                //document.getElementById("txtmaninstdate2").focus();
                alert('Installment date2 must be after Installment date1...');
                return;
            }
        }


        function checkmanualinstdate3() {
            var errormsg = "Installment dates must be within 12 months of each other...";
            var inst1 = document.getElementById("txtmaninstdate1").value;
            var inst2 = document.getElementById("txtmaninstdate2").value;
            var inst3 = document.getElementById("txtmaninstdate3").value;

            var a = new Date(inst1);
            var b = new Date(inst2);
            var c = new Date(inst3);

            var months = (c.getFullYear() - a.getFullYear()) * 12;
            months += c.getMonth() - a.getMonth();

            //var months = (c.getFullYear() - b.getFullYear() - a.getFullYear()) * 12;
            //months += c.getMonth() - b.getMonth() - a.getMonth();

            if (c > a) {
                if (months >= 12) {
                    document.getElementById('txtmaninstdate3').value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                    //document.getElementById("txtmaninstdate3").focus();
                    alert(errormsg);
                    return;
                }
            }

            if (c < b || c < a) {
                document.getElementById('txtmaninstdate3').value = "";
                setTimeout(function () { document.getElementById("txtmaninstdate3").focus(); }, 1);
                //document.getElementById("txtmaninstdate3").focus();
                alert('Installment date3 must be after Installment date2...');
                return;
            }
        }

        function checkmanualinstdate4() {
            var errormsg = "Installment dates must be within 12 months of each other...";
            var inst1 = document.getElementById("txtmaninstdate1").value;
            var inst2 = document.getElementById("txtmaninstdate2").value;
            var inst3 = document.getElementById("txtmaninstdate3").value;
            var inst4 = document.getElementById("txtmaninstdate4").value;

            var a = new Date(inst1);
            var b = new Date(inst2);
            var c = new Date(inst3);
            var d = new Date(inst4);

            var months = (d.getFullYear() - a.getFullYear()) * 12;
            months += d.getMonth() - a.getMonth();

            //var months = (d.getFullYear() - c.getFullYear() - b.getFullYear() - a.getFullYear()) * 12;
            //months += d.getMonth() - c.getMonth() - b.getMonth() - a.getMonth();
            if (d > a) {
                if (months >= 12) {
                    document.getElementById('txtmaninstdate4').value = "";
                    setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                    //document.getElementById("txtmaninstdate4").focus();
                    alert(errormsg);
                    return;
                }
            }

            if (d < c || d < b || d < a) {
                document.getElementById('txtmaninstdate4').value = "";
                setTimeout(function () { document.getElementById("txtmaninstdate4").focus(); }, 1);
                //document.getElementById("txtmaninstdate4").focus();
                alert('Installment date4 must be after Installment date3...');
                return;
            }
        }

        //Future Discount Date
        function checkfutINSTDEDate1() {
            var errormsg = "Discount date must be less than 12 months from the installment date..."
            var futerrormsg1 = "Discount date must be before the delinquent date...";
            var inst1 = document.getElementById("txtmaninstdate1").value;
            var futdelinq1 = document.getElementById("txtmandeliqdate1").value;
            var futdisc1 = document.getElementById("txtmandisdate1").value;

            var a = new Date(futdelinq1);
            var b = new Date(futdisc1);
            var c = new Date(inst1);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            var months11 = (c.getFullYear() - b.getFullYear()) * 12;
            months11 += c.getMonth() - b.getMonth();

            if (b > a) {
                document.getElementById('txtmandisdate1').value = "";
                setTimeout(function () { document.getElementById("txtmandisdate1").focus(); }, 1);
                //document.getElementById("txtmandisdate1").focus();
                alert(futerrormsg1);
                return;
            }
            if (c > b) {

                if (months11 >= 12) {
                    document.getElementById('txtmandisdate1').value = "";
                    setTimeout(function () { document.getElementById("txtmandisdate1").focus(); }, 1);
                    //document.getElementById("txtmandisdate1").focus();
                    alert(errormsg);
                    return;
                }
            }
        }

        function checkfutINSTDEDate2() {
            var errormsg = "Discount date must be less than 12 months from the installment date..."
            var futerrormsg2 = "Discount date must be before the delinquent date..."
            var inst2 = document.getElementById("txtmaninstdate2").value;
            var futdelinq2 = document.getElementById("txtmandeliqdate2").value;
            var futdisc2 = document.getElementById("txtmandisdate2").value;

            var a = new Date(futdelinq2);
            var b = new Date(futdisc2);
            var c = new Date(inst2);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            var months11 = (c.getFullYear() - b.getFullYear()) * 12;
            months11 += c.getMonth() - b.getMonth();

            if (b > a) {
                document.getElementById('txtmandisdate2').value = "";
                setTimeout(function () { document.getElementById("txtmandisdate2").focus(); }, 1);
                //document.getElementById("txtmandisdate2").focus();
                alert(futerrormsg2);
                return;
            }
            if (c > b) {

                if (months11 >= 12) {
                    document.getElementById('txtmandisdate2').value = "";
                    setTimeout(function () { document.getElementById("txtmandisdate2").focus(); }, 1);
                    //document.getElementById("txtmandisdate2").focus();
                    alert(errormsg);
                    return;
                }
            }
        }

        function checkfutINSTDEDate3() {
            var errormsg = "Discount date must be less than 12 months from the installment date..."
            var futerrormsg3 = "Discount date must be before the delinquent date..."
            var inst3 = document.getElementById("txtmaninstdate3").value;
            var futdelinq3 = document.getElementById("txtmandeliqdate3").value;
            var futdisc3 = document.getElementById("txtmandisdate3").value;

            var a = new Date(futdelinq3);
            var b = new Date(futdisc3);
            var c = new Date(inst3);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            var months11 = (c.getFullYear() - b.getFullYear()) * 12;
            months11 += c.getMonth() - b.getMonth();

            if (b > a) {
                document.getElementById('txtmandisdate3').value = "";
                setTimeout(function () { document.getElementById("txtmandisdate3").focus(); }, 1);
                alert(futerrormsg3);
                return;
                //document.getElementById("txtmandisdate3").focus();
            }
            if (c > b) {

                if (months11 >= 12) {
                    document.getElementById('txtmandisdate3').value = "";
                    setTimeout(function () { document.getElementById("txtmandisdate3").focus(); }, 1);
                    //document.getElementById("txtmandisdate3").focus();
                    alert(errormsg);
                    return;
                }
            }
        }

        function checkfutINSTDEDate4() {
            var errormsg = "Discount date must be less than 12 months from the installment date..."
            var futerrormsg4 = "Discount date must be before the delinquent date..."
            var inst4 = document.getElementById("txtmaninstdate4").value;
            var futdelinq4 = document.getElementById("txtmandeliqdate4").value;
            var futdisc4 = document.getElementById("txtmandisdate4").value;

            var a = new Date(futdelinq4);
            var b = new Date(futdisc4);
            var c = new Date(inst4);

            var months = (b.getFullYear() - a.getFullYear()) * 12;
            months += b.getMonth() - a.getMonth();

            var months11 = (c.getFullYear() - b.getFullYear()) * 12;
            months11 += c.getMonth() - b.getMonth();

            if (b > a) {
                document.getElementById('txtmandisdate4').value = "";
                setTimeout(function () { document.getElementById("txtmandisdate4").focus(); }, 1);
                //document.getElementById("txtmandisdate4").focus();
                alert(futerrormsg4);
                return;
            }
            if (c > b) {

                if (months11 >= 12) {
                    document.getElementById('txtmandisdate4').value = "";
                    setTimeout(function () { document.getElementById("txtmandisdate4").focus(); }, 1);
                    //document.getElementById("txtmandisdate4").focus();
                    alert(errormsg);
                    return;
                }
            }
        }

        function dateValidate(txtpayoffgood) {
            var pickeddate = new Date(txtpayoffgood.value);
            var todayDate = new Date();
            if (pickeddate > todayDate) {
                document.getElementById('txtpayoffgood').style.borderColor = "green";
                document.getElementById('lblpayoffgood').style.color = "green";
                return true;
            }
            else {
                document.getElementById('txtpayoffgood').style.borderColor = "#ff0000";
                document.getElementById('lblpayoffgood').style.color = "#ff0000";
                document.getElementById('txtpayoffgood').value = "";
                setTimeout(function () { document.getElementById("txtpayoffgood").focus(); }, 1);
                //document.getElementById("txtpayoffgood").focus();
                alert("Date should be greater than current date");
                return true;
            }
        }

        function dateValidateFutue(txtpayoffgood) {


            var today = new Date();
            var dd = today.getDate();

            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }

            if (mm < 10) {
                mm = '0' + mm;
            }

            today = mm + '/' + dd + '/' + yyyy;

            if (txtpayoffgood.value == "") {
                return true;
            }

            if (Date.parse(txtpayoffgood.value) <= Date.parse(today)) {
                document.getElementById('txtinitialinstall').style.borderColor = "green";
                document.getElementById('lblinitialinstall').style.color = "green";
                return true;
            }
            else {
                document.getElementById('txtinitialinstall').style.borderColor = "#ff0000";
                document.getElementById('lblinitialinstall').style.color = "#ff0000";
                document.getElementById('txtinitialinstall').value = "";
                setTimeout(function () { document.getElementById("txtinitialinstall").focus(); }, 1);
                //document.getElementById("txtinitialinstall").focus();
                alert("Future date not allowed..");
                return true;
            }
        }

        function dateValidateFutuetaxsale(txtpayoffgood) {


            var today = new Date();
            var dd = today.getDate();

            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }

            if (mm < 10) {
                mm = '0' + mm;
            }

            today = mm + '/' + dd + '/' + yyyy;

            if (txtpayoffgood.value == "") {
                return true;
            }

            if (Date.parse(txtpayoffgood.value) <= Date.parse(today)) {
                document.getElementById('txtdatetaxsale').style.borderColor = "green";
                document.getElementById('lbldatetaxsale').style.color = "green";
                return true;
            }
            else {
                document.getElementById('txtdatetaxsale').style.borderColor = "#ff0000";
                document.getElementById('lbldatetaxsale').style.color = "#ff0000";
                document.getElementById('txtdatetaxsale').value = "";
                setTimeout(function () { document.getElementById("txtdatetaxsale").focus(); }, 1);
                //document.getElementById("txtdatetaxsale").focus();
                alert("Future date not allowed..");
                return true;
            }
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

        var isShift = false;
        var seperator = "/";


        function DateFormat(txt, keyCode) {
            //if (keyCode == 16)
            //   isShift = true;
            if (((keyCode >= 46 && keyCode <= 57) || keyCode == 8 || keyCode == 127 || keyCode == 224 || keyCode == 22 || keyCode == 17 || keyCode == 2 ||

                 keyCode <= 37 || keyCode <= 39 || keyCode == 86 || keyCode == 67 ||

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


            if (val.length == 10) {
                var splits = val.split("/");
                var dt = new Date(splits[0] + "/" + splits[1] + "/" + splits[2]);
                //Validation for Dates
                if (dt.getMonth() + 1 == splits[0] && dt.getDate() == splits[1] && dt.getFullYear() == splits[2]) {

                    txt.style.color = "Black";
                }
                else {



                    txt.style.color = "red";
                    alert("Invalid Date Format");
                    txt.value = '';
                    return;

                }


            }
            else if (val.length < 10) {

            }

        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

    <script>
        //Exemption and Special Assessment
        function txtexeSpecial() {
            var exestatus = document.getElementById("txtexemption").value;
            if (exestatus == "Yes") {
                document.getElementById("tblExestatus").style.visibility = "visible";
                document.getElementById("tblExestatus").style.display = "block";
                document.getElementById('gvExemption').style.display = "block";
            }
            else if (exestatus == "No") {
                document.getElementById("tblExestatus").style.visibility = "hidden";
                document.getElementById("tblExestatus").style.display = "none";
                document.getElementById('gvExemption').style.display = "none";
            }
            else {
                document.getElementById("tblExestatus").style.visibility = "hidden";
                document.getElementById("tblExestatus").style.display = "none";
                document.getElementById('gvExemption').style.display = "none";
            }

            var specialstatus = document.getElementById("SecialAssmnt").value;
            if (specialstatus == "Yes") {
                document.getElementById("tblSpecialstatus").style.visibility = "visible";
                document.getElementById("tblSpecialstatus").style.display = "block";
                document.getElementById('gvSpecial').style.display = "block";
            }
            else if (specialstatus == "No") {
                document.getElementById("tblSpecialstatus").style.visibility = "hidden";
                document.getElementById("tblSpecialstatus").style.display = "none";
                document.getElementById('gvSpecial').style.display = "none";
            }
            else {
                document.getElementById("tblSpecialstatus").style.visibility = "hidden";
                document.getElementById("tblSpecialstatus").style.display = "none";
                document.getElementById('gvSpecial').style.display = "none";
            }


            var delistatus = document.getElementById("txtdeliquent").value;
            if (delistatus == "Yes") {
                document.getElementById("tblDeliquentStatus").style.visibility = "visible";
                document.getElementById("tblDeliquentStatus").style.display = "block";
                document.getElementById('gvDeliquentStatus').style.display = "block";
            }
            else if (delistatus == "No") {
                document.getElementById("tblDeliquentStatus").style.visibility = "hidden";
                document.getElementById("tblDeliquentStatus").style.display = "none";
                document.getElementById('gvDeliquentStatus').style.display = "none";
            }
            else {
                document.getElementById("tblDeliquentStatus").style.visibility = "hidden";
                document.getElementById("tblDeliquentStatus").style.display = "none";
                document.getElementById('gvDeliquentStatus').style.display = "none";
            }

            //var paststatus = document.getElementById("pastDeliquent").value;
            //if (paststatus == "Yes") {
            //    document.getElementById("tblPastDeliquent").style.visibility = "visible";
            //    document.getElementById("tblPastDeliquent").style.display = "block";
            //    document.getElementById('GrdPriordelinquent').style.display = "block";
            //}
            //else if (paststatus == "No") {
            //    document.getElementById("tblPastDeliquent").style.visibility = "hidden";
            //    document.getElementById("tblPastDeliquent").style.display = "none";
            //    document.getElementById('GrdPriordelinquent').style.display = "none";
            //}
            //else {
            //    document.getElementById("tblPastDeliquent").style.visibility = "hidden";
            //    document.getElementById("tblPastDeliquent").style.display = "none";
            //    document.getElementById('GrdPriordelinquent').style.display = "none";
            //}

            var ddlselect = document.getElementById("txtnotapplicable").value;
            if (ddlselect == "Yes") {
                document.getElementById("txtdatetaxsale").disabled = true;
                document.getElementById("txtlastdayred").disabled = true;
                document.getElementById("txtdatetaxsale").value = "";
                document.getElementById("txtlastdayred").value = "";
            }
            else if (ddlselect == "No") {
                document.getElementById("txtdatetaxsale").disabled = false;
                document.getElementById("txtlastdayred").disabled = false;
            }
            else {
                document.getElementById("txtdatetaxsale").value = "";
                document.getElementById("txtlastdayred").value = "";
            }

            var ddlselect = document.getElementById("txtnotapplicable").value;
            if (ddlselect == "Yes") {
                document.getElementById("txtdatetaxsale").disabled = true;
                document.getElementById("txtlastdayred").disabled = true;
                document.getElementById("txtdatetaxsale").value = "";
                document.getElementById("txtlastdayred").value = "";
            }
            else if (ddlselect == "No") {
                document.getElementById("txtdatetaxsale").disabled = false;
                document.getElementById("txtlastdayred").disabled = false;
            }
            else {
                document.getElementById("txtdatetaxsale").value = "";
                document.getElementById("txtlastdayred").value = "";
            }

        }

        //Deliquent Status
        function txtVisible1() {
            document.getElementById("tbldelistatus").style.visibility = "visible";
            document.getElementById("tbldelistatus").style.display = "block";
        }

        function txtdeliquentsta1() {
            var delistatus = document.getElementById("txtdeliquent").value;
            var oRows = gvDeliquentStatus.rows;

            if (delistatus == "Yes") {
                document.getElementById("tblDeliquentStatus").style.visibility = "visible";
                document.getElementById("tblDeliquentStatus").style.display = "block";
                document.getElementById('tblDeliquentStatus').focus();
                document.getElementById('txtdeliPayee').focus();
            }
            else if (delistatus == "No") {
                document.getElementById("tblDeliquentStatus").style.visibility = "hidden";
                document.getElementById("tblDeliquentStatus").style.display = "none";
                document.getElementById("txtdelitaxyear").value = '';
                document.getElementById('tblDeliquentStatus').focus();

                if (oRows.length > 1) {
                    $('#delinquentstatus').modal('show');
                }
            }
            else {
                document.getElementById("tblDeliquentStatus").style.visibility = "hidden";
                document.getElementById("tblDeliquentStatus").style.display = "none";
                document.getElementById('tblDeliquentStatus').focus();
            }
        }


        //function installment details 
        function txtinstdetails() {
            var inststatus = document.getElementById("ddlfuturetaxcalc").value;

            if (inststatus == "Manual") {
                document.getElementById("PnlTax1").style.visibility = "visible";
                document.getElementById("PnlTax1").style.display = "block";

            }
            else if (inststatus == "No") {
                document.getElementById("PnlTax1").style.visibility = "hidden";
                document.getElementById("PnlTax1").style.display = "none";
            }
            else {
                document.getElementById("PnlTax1").style.visibility = "hidden";
                document.getElementById("PnlTax1").style.display = "none";
            }
        }

        function txtexeVisible1() {
            document.getElementById("tblExestatus").style.visibility = "visible";
            document.getElementById("tblExestatus").style.display = "block";
        }

        function txtexemption1() {
            var exestatus = document.getElementById("txtexemption").value;
            var exerow = gvExemption.rows;

            if (exestatus == "Yes") {
                document.getElementById("tblExestatus").style.visibility = "visible";
                document.getElementById("tblExestatus").style.display = "block";
                document.getElementById('gvExemption').style.display = "block";
                document.getElementById('tblExestatus').focus();
                document.getElementById('txtexetype').focus();
            }
            else if (exestatus == "No") {
                document.getElementById("tblExestatus").style.visibility = "hidden";
                document.getElementById("tblExestatus").style.display = "none";
                document.getElementById('gvExemption').style.display = "none";
                document.getElementById('tblExestatus').focus();
                if (exerow.length > 1) {
                    $('#exemptionstatus').modal('show');
                }
            }
            else {
                document.getElementById("tblExestatus").style.visibility = "hidden";
                document.getElementById("tblExestatus").style.display = "none";
                document.getElementById('gvExemption').style.display = "none";
                document.getElementById('tblExestatus').focus();
            }
        }

        //Deliquent tax year
        function IsValidLengthTax3(year, element, ev) {
            var id1 = year.length;
            if (id1 != 0) {
                if (id1 < 4) {
                    document.getElementById("txtdelitaxyear").value = '';
                    setTimeout(function () { document.getElementById("txtdelitaxyear").focus(); }, 1);
                    //document.getElementById("txtdelitaxyear").focus();
                    alert("Delinquent Status Year should be 4 Numeric Characters");
                    return;
                }
            }
        }

        function IsValidLengthTaxYear(year, element, ev) {
            var id1 = year.length;
            if (id1 != 0) {
                if (id1 < 4) {
                    document.getElementById("txtTaxYear").value = '';
                    setTimeout(function () { document.getElementById("txtTaxYear").focus(); }, 1);
                    //document.getElementById("txtTaxYear").focus();
                    alert("Tax Year should be 4 Numeric Characters");
                    return;
                }
            }
        }

        function IsValidLengthEndYear(year, element, ev) {
            var id1 = year.length;
            if (id1 != 0) {
                if (id1 < 4) {
                    document.getElementById("txtEndYear").value = '';
                    setTimeout(function () { document.getElementById("txtEndYear").focus(); }, 1);
                    //document.getElementById("txtEndYear").focus();
                    alert("End Year should be 4 Numeric Characters");
                    return;
                }
            }
        }

        function IsValidLengthState(year, element, ev) {
            var id1 = year.length;
            if (id1 != 0) {
                if (id1 < 2) {
                    document.getElementById("txtdelitState").value = '';
                    setTimeout(function () { document.getElementById("txtdelitState").focus(); }, 1);
                    //document.getElementById("txtdelitState").focus();
                    alert("State should be 2 Characters");
                    return;
                }
            }
        }

        function IsValidLengthZip(year, element, ev) {
            var id1 = year.length;
            if (id1 != 0) {
                if (id1 < 5) {
                    document.getElementById("txtdelitzip").value = '';
                    setTimeout(function () { document.getElementById("txtdelitzip").focus(); }, 1);
                    //document.getElementById("txtdelitzip").focus();
                    alert("Zip should be 5 Numeric Characters");
                    return;
                }
            }
        }
        //Prior Deliquent tax year
        function IsValidLengthTaxPrior(year, element, ev) {
            var priorid = year.length;
            if (priorid != 0) {
                if (priorid < 4) {
                    document.getElementById("txtpriodeli").value = '';
                    setTimeout(function () { document.getElementById("txtpriodeli").focus(); }, 1);
                    //document.getElementById("txtpriodeli").focus();
                    alert("Prior Deliquent Year should be 4 Numeric Characters");
                    return;
                }
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function isNumberKey1(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        //PayoffAmount
        function mypay() {
            PayAmount1();
            myFunctionPayAmount1();
        }

        function PayAmount1() {
            document.getElementById("o").innerText = formatMoneyPayAmount1(document.getElementById("txtpayoffamount").value);
        }

        function formatMoneyPayAmount1(n, c, txtpayoffamount, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtpayoffamount = txtpayoffamount == undefined ? "." : txtpayoffamount,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtpayoffamount + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function myFunctionPayAmount1() {
            document.getElementById("txtpayoffamount").value = document.getElementById("o").innerText;
        }

        //Base Amount
        function Baseduepay() {
            BasedueAmount();
            BaseduePayAmount();
        }

        function BasedueAmount() {
            document.getElementById("BA").innerText = formatMoneyBaseAmount(document.getElementById("txtbaseamntdue").value);
        }

        function formatMoneyBaseAmount(n, c, txtbaseamntdue, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtbaseamntdue = txtbaseamntdue == undefined ? "." : txtbaseamntdue,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtbaseamntdue + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function BaseduePayAmount() {
            document.getElementById("txtbaseamntdue").value = document.getElementById("BA").innerText;
        }

        //Per Penalty
        function Perpenpay() {
            PerpenAmount();
            PerpePayAmount();
        }

        function PerpenAmount() {
            document.getElementById("PP").innerText = formatMoneyPerPen(document.getElementById("txtpenlatyamt").value);
        }

        function formatMoneyPerPen(n, c, txtpenlatyamt, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtpenlatyamt = txtpenlatyamt == undefined ? "." : txtpenlatyamt,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtpenlatyamt + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function PerpePayAmount() {
            document.getElementById("txtpenlatyamt").value = document.getElementById("PP").innerText;
        }

        //Add Pen Penalty
        function Addpenpay() {
            AddpenAmount();
            AddpePayAmount();
        }

        function AddpenAmount() {
            document.getElementById("AP").innerText = formatMoneyAddPen(document.getElementById("txtaddpenAmnt").value);
        }

        function formatMoneyAddPen(n, c, txtaddpenAmnt, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtaddpenAmnt = txtaddpenAmnt == undefined ? "." : txtaddpenAmnt,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtaddpenAmnt + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function AddpePayAmount() {
            document.getElementById("txtaddpenAmnt").value = document.getElementById("AP").innerText;
        }

        //Exemption Amount
        function myExe() {
            ExemptionAmount1();
            myFunctionExemptionAmount1();
        }

        function ExemptionAmount1() {
            document.getElementById("ee").innerText = formatMoneyExeAmount1(document.getElementById("txtexeamount").value);
        }

        function formatMoneyExeAmount1(n, c, txtexeamount, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtexeamount = txtexeamount == undefined ? "." : txtexeamount,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtexeamount + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function myFunctionExemptionAmount1() {
            document.getElementById("txtexeamount").value = document.getElementById("ee").innerText;
        }


        //Tax Sale
        function applicable() {
            var ddlselect = document.getElementById("txtnotapplicable").value;
            if (ddlselect == "Yes" || ddlselect == "Select") {
                document.getElementById("txtdatetaxsale").disabled = true;
                document.getElementById("txtlastdayred").disabled = true;
                document.getElementById("txtdatetaxsale").value = "";
                document.getElementById("txtlastdayred").value = "";
            }
            else if (ddlselect == "No") {
                document.getElementById("txtdatetaxsale").disabled = false;
                document.getElementById("txtlastdayred").disabled = false;
            }
            //else {
            //    document.getElementById("txtdatetaxsale").value = "";
            //    document.getElementById("txtlastdayred").value = "";
            //}
        }

        //Special Assessment
        function CheckFirstChar(key, txt) {
            if (key == 32 && txt.value.length <= 0) {
                return false;
            }
            else if (txt.value.length > 0) {
                if (txt.value.charCodeAt(0) == 32) {
                    txt.value = txt.value.substring(1, txt.value.length);
                    return true;
                }
            }
            return true;
        }

        //Special Status
        function txtSpeVisible1() {
            document.getElementById("tblSpecialstatus").style.visibility = "visible";
            document.getElementById("tblSpecialstatus").style.display = "block";
        }

        function txtSpecial1() {
            var specialstatus = document.getElementById("SecialAssmnt").value;
            var SpRows = gvSpecial.rows;
            if (specialstatus == "Yes") {
                document.getElementById("tblSpecialstatus").style.visibility = "visible";
                document.getElementById("tblSpecialstatus").style.display = "block";
                document.getElementById('tblSpecialstatus').focus();
                document.getElementById('txtdescription').focus();
                //document.getElementById('gvSpecial').style.display = "block";
            }
            else if (specialstatus == "No") {
                document.getElementById("tblSpecialstatus").style.visibility = "hidden";
                document.getElementById("tblSpecialstatus").style.display = "none";
                document.getElementById('tblSpecialstatus').focus();
                //document.getElementById('gvSpecial').style.display = "none";
                if (SpRows.length > 1) {
                    $('#specialstatus').modal('show');
                }
            }
            else {
                document.getElementById("tblSpecialstatus").style.visibility = "hidden";
                document.getElementById("tblSpecialstatus").style.display = "none";
                document.getElementById('tblSpecialstatus').focus();
                //document.getElementById('gvSpecial').style.display = "none";
            }
        }

        function txtPastDel() {
            document.getElementById("tblPastDeliquent").style.visibility = "visible";
            document.getElementById("tblPastDeliquent").style.display = "block";
        }

        //Past Deliquent
        function txtpastDeliquent() {

            var paststatus = document.getElementById("pastDeliquent").value;
            var oRows = GrdPriordelinquent.rows;
            if (paststatus == "Yes") {
                document.getElementById("tblPastDeliquent").style.visibility = "visible";
                document.getElementById("tblPastDeliquent").style.display = "block";
                document.getElementById('tblPastDeliquent').focus();
            }
            else if (paststatus == "No") {
                document.getElementById("tblPastDeliquent").style.visibility = "hidden";
                document.getElementById("tblPastDeliquent").style.display = "none";
                document.getElementById('tblPastDeliquent').focus();
                if (oRows.length > 1) {
                    $('#priordeliqstatus').modal('show');
                }
            }
            else {
                document.getElementById("tblPastDeliquent").style.visibility = "hidden";
                document.getElementById("tblPastDeliquent").style.display = "none";
                document.getElementById('tblPastDeliquent').focus();
                //document.getElementById('GrdPriordelinquent').style.display = "none";
            }
        }

        //Exemption Amount
        function mySpe() {
            SpeAmount1();
            SpemyFunctionAmount1();
        }

        function SpeAmount1() {
            document.getElementById("s").innerText = formatMoneySpeAmount1(document.getElementById("txtInstallRemain").value);
        }

        function formatMoneySpeAmount1(n, c, txtInstallRemain, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtInstallRemain = txtInstallRemain == undefined ? "." : txtInstallRemain,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtInstallRemain + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function SpemyFunctionAmount1() {
            document.getElementById("txtInstallRemain").value = document.getElementById("s").innerText;
        }

        //Remain
        function myRemian() {
            RemianAmount1();
            SpemyFunctionRemian1();
        }

        function RemianAmount1() {
            document.getElementById("sr").innerText = formatMoneySpeRemain1(document.getElementById("txtamountspecial").value);
        }

        function formatMoneySpeRemain1(n, c, txtamountspecial, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtamountspecial = txtamountspecial == undefined ? "." : txtamountspecial,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtamountspecial + Math.abs(n - i).toFixed(c).slice(2) : "");
        };

        function SpemyFunctionRemian1() {
            document.getElementById("txtamountspecial").value = document.getElementById("sr").innerText;
        }

        //Prior Amount Paid
        function mypriamtpaid() {
            Priamountpaid();
            PriFunctionpaid();
        }

        function Priamountpaid() {
            document.getElementById("OD").innerText = formatMoneypaidamount(document.getElementById("txtpriamtpaid").value);
        }

        function formatMoneypaidamount(n, c, txtpriamtpaid, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtpriamtpaid = txtpriamtpaid == undefined ? "." : txtpriamtpaid,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtpriamtpaid + Math.abs(n - i).toFixed(c).slice(2) : "");
        };

        function PriFunctionpaid() {
            document.getElementById("txtpriamtpaid").value = document.getElementById("OD").innerText;
        }

        //Prior Amount Due
        function OriDue() {
            OriginalAmountDue();
            OriginFunctionDue();
        }

        function OriginalAmountDue() {
            document.getElementById("OA").innerText = formatMoneydueamount(document.getElementById("txtpriorigamtdue").value);
        }

        function formatMoneydueamount(n, c, txtpriorigamtdue, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtpriorigamtdue = txtpriorigamtdue == undefined ? "." : txtpriorigamtdue,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtpriorigamtdue + Math.abs(n - i).toFixed(c).slice(2) : "");
        };

        function OriginFunctionDue() {
            document.getElementById("txtpriorigamtdue").value = document.getElementById("OA").innerText;
        }

        //Remaing Balance
        function myRemianBal() {
            RemianBalAmount1();
            SpemyFunctionRemianBal1();
        }

        function RemianBalAmount1() {
            document.getElementById("rr").innerText = formatMoneySpeRemainBal1(document.getElementById("txtsperembal").value);
        }

        function formatMoneySpeRemainBal1(n, c, txtsperembal, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtsperembal = txtsperembal == undefined ? "." : txtsperembal,
                n = n.replace(/,/g, ''),
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtsperembal + Math.abs(n - i).toFixed(c).slice(2) : "");
        };

        function SpemyFunctionRemianBal1() {
            document.getElementById("txtsperembal").value = document.getElementById("rr").innerText;
        }
    </script>

    <script type="text/javascript">
        $("[src*=Arrow]").live("click", function () {
            $(this).closest("tr").after("<tr><td style='width: 10px;'></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../images/STRArrow.png");          
            document.getElementById("edit-save").disabled = true;
        });
        //$("[src*=minus]").live("click", function () {
        //    $(this).attr("src", "../images/STRplus.jpg");
        //    $(this).closest("tr").next().remove();
        //});
        $(document).ready(function () { // on document ready
            $("[src*=Arrow]").click();
        })
        function openModal() {
            //$('[id*=myModalOK]').modal('show');
        }

        function openModal1() {
            $('[id*=AddTaxStatus]').modal('show');
        }

        function AuthorityopenModal() {
            $('[id*=ModalAuthorityStatus]').modal('show');
        }

    </script>


    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <style type="text/css">
        .colorbold {
            color: red;
        }

        .googlefonts {
            /*font-family:Calibri;*/
            /*font-family: Calibri;*/
            font-weight: 400;
            /*color:black;*/
            width: 100px;
            word-break: break-all;
            /*margin-left: -5px;*/
        }

        .modal-header {
            border-bottom: 1px solid #fff;
        }

        .table table-bordered tr {
            line-height: 10px;
        }

        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td {
            padding: 1px;
        }

        .panel-heading {
            height: 28px;
        }

        .panel-title {
            margin-top: -5px;
        }

        .overlay {
            background-color: #EFEFEF;
            position: fixed;
            width: 100%;
            height: 100%;
            z-index: 1000;
            top: 0px;
            left: 0px;
            opacity: .5;
            filter: alpha(opacity=50);
        }

        #overlay {
            position: fixed;
            display: none;
            width: 100%;
            height: 100%;
            z-index: 1000;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5);
            cursor: pointer;
        }

        .stopwatch .controls {
            font-size: 12px;
        }

            /* I'd rather stick to CSS rather than JS  for styling */

            .stopwatch .controls button {
                padding: 5px 15px;
                background: #EEE;
                border: 3px solid #06C;
                border-radius: 5px;
            }

        .stopwatch .time {
            /*font-size: 150%;*/
        }

        .align {
            display: inline;
            float: left;
            width: 50%;
        }

        input[type=number]::-webkit-inner-spin-button,
        input[type=number]::-webkit-outer-spin-button {
            -webkit-appearance: none;
            margin: 0;
        }

        .pad {
            padding-left: 14px;
        }

        .CheckBold {
            font-weight: 600;
        }

        .editabledropdown {
            border-radius: 6px;
            border: solid 1px #ccc;
            padding: 5px;
            margin: 0px;
        }

        .taxing {
            height: 30px;
            padding-top: 2px;
            padding-bottom: 2px;
        }
    </style>

    <script type="text/javascript">

        //Code To avoid First Spaces

        function AvoidSpace(event) {
            var k = event ? event.which : window.event.keyCode;
            var startPos = event.currentTarget.selectionStart;
            if (k == 32 && startPos == 0) return false;
        }

        function off() {
            document.getElementById("overlay").style.display = "none";
        }

        function toggleChevron(e) {
            $(e.target)
                .prev('.panel-heading')
                .find("i.indicator")
                .toggleClass('glyphicon-chevron-down glyphicon-chevron-right');

            $(e.target).parent().toggleClass('panel-default panel-default');
        }
        $('#accordion').on('hidden.bs.collapse shown.bs.collapse', toggleChevron);
    </script>
    <style type="text/css">
        /*.panel-heading {
             padding: 0px;
        }*/
        strong {
            font-size: 15px;
        }

        body {
            margin: 0;
            padding: 0;
            font-family: Calibri;
            line-height: 1.5em;
        }

        #header {
            background: #fff;
            height: 20px;
        }

            #header h1 {
                margin: 0;
            }

        main {
            padding-bottom: 10010px;
            margin-bottom: -10005px;
            float: left;
            width: 100%;
        }

        #nav {
            /*padding-bottom: 10010px;
            margin-bottom: -10000px;
            float: left;*/
            width: 230px;
            margin-left: -100%;
            background: #eee;
        }

        #footer {
            clear: left;
            width: 100%;
            background: #ccc;
            text-align: center;
        }

        #wrapper {
            overflow: hidden;
        }

        .innertube {
            margin: 15px; /* Padding for content */
            margin-top: 0;
        }

        p {
            color: #555;
        }

        nav ul {
            list-style-type: none;
            margin: 0;
            padding: 0;
        }

            nav ul a {
                color: darkgreen;
                text-decoration: none;
            }

        .sidebar {
            height: 100%;
            width: 0;
            position: fixed;
            z-index: 1;
            top: 0;
            left: 0;
            background-color: #111;
            overflow-x: hidden;
            transition: 0.5s;
            padding-top: 60px;
        }

            .sidebar a {
                padding: 8px 8px 8px 32px;
                text-decoration: none;
                font-size: 25px;
                color: #818181;
                display: block;
                transition: 0.3s;
            }

                .sidebar a:hover {
                    color: #f1f1f1;
                }

            .sidebar .closebtn {
                position: absolute;
                top: 0;
                right: 25px;
                font-size: 36px;
                margin-left: 50px;
            }

        .openbtn {
            font-size: 20px;
            cursor: pointer;
            background-color: #111;
            color: white;
            padding: 10px 15px;
            border: none;
        }

            .openbtn:hover {
                background-color: #444;
            }

        #main {
            transition: margin-left .5s;
            padding: 16px;
        }
        /* On smaller screens, where height is less than 450px, change the style of the sidenav (less padding and a smaller font size) */
        @media screen and (max-height: 450px) {
            .sidebar {
                padding-top: 15px;
            }

                .sidebar a {
                    font-size: 18px;
                }
        }

        .panel.with-nav-tabs .panel-heading {
            padding: 5px 5px 0 5px;
            border-top: 1px solid #e5e5e5;
        }

        .panel.with-nav-tabs .nav-tabs {
            border-bottom: none;
        }

        .panel.with-nav-tabs .nav-justified {
            margin-bottom: -1px;
        }

        /********************************************************************/
        /*** PANEL PRIMARY ***/
        .with-nav-tabs.panel-primary .nav-tabs > li > a,
        .with-nav-tabs.panel-primary .nav-tabs > li > a:hover,
        .with-nav-tabs.panel-primary .nav-tabs > li > a:focus {
            color: #fff;
        }

            .with-nav-tabs.panel-primary .nav-tabs > .open > a,
            .with-nav-tabs.panel-primary .nav-tabs > .open > a:hover,
            .with-nav-tabs.panel-primary .nav-tabs > .open > a:focus,
            .with-nav-tabs.panel-primary .nav-tabs > li > a:hover,
            .with-nav-tabs.panel-primary .nav-tabs > li > a:focus {
                color: #fff;
                background-color: #3071a9;
                border-color: transparent;
            }

        .with-nav-tabs.panel-primary .nav-tabs > li.active > a,
        .with-nav-tabs.panel-primary .nav-tabs > li.active > a:hover,
        .with-nav-tabs.panel-primary .nav-tabs > li.active > a:focus {
            color: #428bca;
            background-color: #fff;
            border-color: #428bca;
            border-bottom-color: transparent;
        }

        .with-nav-tabs.panel-primary .nav-tabs > li.dropdown .dropdown-menu {
            background-color: #428bca;
            border-color: #3071a9;
        }

            .with-nav-tabs.panel-primary .nav-tabs > li.dropdown .dropdown-menu > li > a {
                color: #fff;
            }

                .with-nav-tabs.panel-primary .nav-tabs > li.dropdown .dropdown-menu > li > a:hover,
                .with-nav-tabs.panel-primary .nav-tabs > li.dropdown .dropdown-menu > li > a:focus {
                    background-color: #3071a9;
                }

            .with-nav-tabs.panel-primary .nav-tabs > li.dropdown .dropdown-menu > .active > a,
            .with-nav-tabs.panel-primary .nav-tabs > li.dropdown .dropdown-menu > .active > a:hover,
            .with-nav-tabs.panel-primary .nav-tabs > li.dropdown .dropdown-menu > .active > a:focus {
                background-color: #4a9fe9;
            }

        .font {
            font-family: Roboto;
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0 1.4em 1.4em 1.4em !important;
            margin: 1.5em 1.5em 1.5em 1.5em !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            font-size: 1.2em !important;
            font-weight: bold !important;
            width: auto;
            padding: 0 10px;
            border-bottom: none;
        }
    </style>
    <script type="text/javascript">
        var bodyText = ["The smaller your reality, the more convinced you are that you know everything.", "If the facts don't fit the theory, change the facts.", "The past has no power over the present moment.", "This, too, will pass.", "</p><p>You will not be punished for your anger, you will be punished by your anger.", "Peace comes from within. Do not seek it without.", "<h3>Heading</h3><p>The most important moment of your life is now. The most important person in your life is the one you are with now, and the most important activity in your life is the one you are involved with now."]
        function generateText(sentenceCount) {
            for (var i = 0; i < sentenceCount; i++)
                document.write(bodyText[Math.floor(Math.random() * 7)] + " ")
        }
    </script>

    <link href="../Script/BT-1.6.4.bootstrap-datepicker.css" rel="stylesheet" />
    <script src="../Script/JS-1.6.4.bootstrap-datepicker.js"></script>
    <script src="../datepicker.js"></script>

    <%--Modal Popup & Editable Dropdown--%>
    <script src="../Libs/jquery.min.js"></script>
    <script type="text/javascript" src="../Editabble Drodown/jquery-1.10.2.js"></script>
    <script src="../Libs/bootstrap.min.js"></script>
    <link href="../Libs/font.css" rel="stylesheet" />
    <link rel="stylesheet" href="../Editabble Drodown/jquery-ui.css" />
    <link rel="stylesheet" href="../Editabble Drodown/style.css" />
    <script type="text/javascript" src="../Editabble Drodown/jquery-ui.js"></script>

    <script src="http://code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        var dateToday = new Date();
        $(function () {
            $("#date1").datepicker({
                defaultDate: "-7D",
                changeMonth: true,
                numberOfMonths: 1,
                beforeShowDay: $.datepicker.noWeekends,
                minDate: dateToday,

            });
        });
        var dateToday = new Date();
        $(function () {
            $("#date2").datepicker({
                defaultDate: "-7D",
                changeMonth: true,
                numberOfMonths: 1,
                beforeShowDay: $.datepicker.noWeekends,
                minDate: dateToday
            });
        });
    </script>
    <style>
        .custom-combobox {
            position: relative;
            display: inline-block;
        }

        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
        }

        .custom-combobox-input {
            margin: 0;
            padding: 5px 10px;
        }

        .ui-state-default,
        .ui-widget-content .ui-state-default,
        .ui-widget-header .ui-state-default {
            border: 1px solid #421D1D;
            background: #ffffff url("images/ui-bg_glass_75_e6e6e6_1x400.png") 50% 50% repeat-x !important;
            font-weight: normal;
            color: #555555;
        }
        /* Corner radius */
        .ui-corner-all,
        .ui-corner-top,
        .ui-corner-left,
        .ui-corner-tl {
            border-top-left-radius: 0px !important;
        }

        .ui-corner-all,
        .ui-corner-top,
        .ui-corner-right,
        .ui-corner-tr {
            border-top-right-radius: 0px !important;
        }

        .ui-corner-all,
        .ui-corner-bottom,
        .ui-corner-left,
        .ui-corner-bl {
            border-bottom-left-radius: 0px !important;
        }

        .ui-corner-all,
        .ui-corner-bottom,
        .ui-corner-right,
        .ui-corner-br {
            border-bottom-right-radius: 0px !important;
        }

        .header {
            width: -webkit-fill-available;
            background-color: #f1f1f1;
            padding: 20px 10px;
        }


        .select-editable {
            position: relative;
            background-color: white;
        }

            .select-editable select {
                top: 0px;
                left: 0px;
                font-size: 14px;
            }

            .select-editable input {
                position: absolute;
                top: 0px;
                left: 0px;
                width: 207px;
                font-size: 14px;
            }

                .select-editable select:focus, .select-editable input:focus {
                    outline: none;
                }

        .auto-style1 {
            display: block;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            border: 1px solid #ccc;
            padding: 6px 12px;
            background-color: #fff;
            background-image: none;
        }

        .tdalign {
            text-align: left;
        }
    </style>
    <script src="../SelectDist/easytimer.min.js"></script>
    <script>
        var timer = new Timer();
        timer.start();
        timer.addEventListener('secondsUpdated', function (e) {
            $('#basicUsage').html(timer.getTimeValues().toString());
        });
    </script>

    <script>
        function TaxtypeModal() {
            $('[id*=Modaltaxtype]').modal('show');
        }
    </script>
    <script type="text/javascript">
        function onlyAlphabets(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 32)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }
        }

    </script>

    <script>
        function openWindow(url) {
            var tet = url;
            var a = "";
            var aindex = 0;
            var bindex = 29;
            if (tet.includes("Pages")) {

                tet = tet.replace(tet.substring(aindex, bindex), "");
                a = tet;
                a = a.match(/^https?:/) ? a : '//' + a;
                window.open(a, '_blank');
                return;
            }
            else {

                url = url.match(/^https?:/) ? url : '//' + url;
                window.open(url, '_blank');
                return;
            }
        }
    </script>

    <script type="text/javascript">
        function isNumber(ev) {
            if (ev.type === "paste" || ev.type === "drop") {
                var textContent = (ev.type === "paste" ? ev.clipboardData : ev.dataTransfer).getData('text');
                return !isNaN(textContent) && textContent.indexOf(".") === -1;
            } else if (ev.type === "keydown") {
                if (ev.ctrlKey || ev.metaKey) {
                    return true
                };
                var keysToAllow = [8, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57];
                return keysToAllow.indexOf(ev.keyCode) > -1;
            } else {
                return true
            }
        }
    </script>

</head>
<body>
    <div class="sign-up-row widget-shadow" style="width: 100%;" align="center">
        <!--<h3 class="title2"  style="margin-right:80px">Create Input Fields</h3>-->

        <form id="myForm" name="myForm" style="margin-top: 1px;" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <table class="header" style="width: 100%; table-layout: auto">
                <tr style="width: 250px;">
                    <td style="color: red; font-weight: bold; width: 150px;">
                        <img src="../images/logo.png" style="width: 90px;" /></td>

                    <td style="width: 470px;"></td>
                    <td style="text-align: center; font-weight: bold; font-size: x-large; color: #280277;">
                        <img src="../images/favicon-32x32.png" style="width: 30px;" />
                        &nbsp;&nbsp;&nbsp;&nbsp;  <b style="margin-left: -30px; white-space: nowrap;">STRMICX</b></td>
                    <td style="width: 315px;">
                        <div style="text-align: end; margin-top: -13px;">
                            <img src="../images/Prcess image.png" alt="image" style="width: 15px" />
                            <asp:Label ID="processtatus" runat="server" Style="color: black; font-weight: 500"></asp:Label>
                        </div>
                    </td>

                    <td class="Lblothers" style="color: black; font-weight: 500; width: 150px; text-align: center">
                        <div style="margin-top: -10px;">
                            <img alt="image" src="../images/Timer.png" style="width: 15px; margin-top: -4px;" />
                            <asp:UpdatePanel ID="up_Timer" runat="server" RenderMode="Inline" UpdateMode="Always">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick" />
                                    <asp:Label ID="lit_Timer" runat="server"></asp:Label><br />
                                    <asp:HiddenField ID="hid_Ticker" runat="server" Value="0" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>

                    <td>
                        <div class="header-right" style="margin-left: 12px; margin-top: -15px;">
                            <div class="profile_details">
                                <ul style="padding-left: 6px;">
                                    <li class="dropdown profile_details_drop">
                                        <a href="" class="dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                            <div class="profile_img">
                                                <div class="user-name">
                                                    <p class="glyphicon glyphicon-user" style="margin-left: -21px;"></p>
                                                    <asp:Label ID="Lblusername" runat="server" Style="color: black; font-weight: 500"></asp:Label>
                                                    <p class="glyphicon glyphicon-chevron-down" style="margin-left: 5px;"></p>
                                                </div>
                                                <i class="fa fa-angle-down lnr"></i>
                                                <i class="fa fa-angle-up lnr"></i>
                                                <div class="clearfix"></div>
                                            </div>
                                        </a>
                                        <ul class="dropdown-menu drp-mnu" style="width: 77px; left: -116px;">
                                            <li>
                                                <a href="#" data-toggle="modal" data-target="#ModallogoutReason"><i class="glyphicon glyphicon-log-out nav_icon" style="left: -7px;"></i>Logout</a>
                                            </li>
                                        </ul>

                                    </li>
                                </ul>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div class="panel-group" id="accordion" style="margin-top: -35px; margin-left: 15px; margin-right: 15px;">
                <div class="panel panel-default" style="border-color: #280277;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#collapse1" style="color: #FFFFFF; background-color: #280277; border-color: #280277">
                        <h4 class="panel-title">
                            <strong style="text-decoration: underline; cursor: pointer">Order Details Information</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                        </h4>
                    </div>
                    <div id="collapse1" class="panel-collapse collapse in">
                        <div class="panel-body">

                            <table style="width: 1283px;">
                                <tbody>
                                    <tr>
                                        <td class="colorbold" style="width: 100px;"><b class="CheckBold">Order Id</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblord" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold" style="width: 115px;"><b class="CheckBold">Transaction Type</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lbltransactiontype" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold" style="width: 100px;"><b class="CheckBold">Loan Number</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblloannumber" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold" style="width: 115px;"><b class="CheckBold">Estimated Value</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblestimatedvalue" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colorbold"><b class="CheckBold">Purchase Price</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblpurchaseprice" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold"><b class="CheckBold">Borrower Name</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblborrowername" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold"><b class="CheckBold">Seller Name</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblsellername" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold"><b class="CheckBold">Year Built</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblyearbuilt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colorbold"><b class="CheckBold">County</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblcounty" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold"><b class="CheckBold">City</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblcity" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold"><b class="CheckBold">State</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblstate" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold"><b class="CheckBold">Zip Code</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label ID="lblzipcode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colorbold"><b class="CheckBold">Street Address</b></td>
                                        <td class="colorbold">:</td>
                                        <td colspan="4">
                                            <asp:Label ID="lblstreetaddress1" runat="server"></asp:Label>
                                        </td>
                                        <td class="colorbold"><b class="CheckBold">Client Name</b></td>
                                        <td class="colorbold">:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lblclientName"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="colorbold"><b class="CheckBold">Brief Legal</b></td>
                                        <td class="colorbold">:</td>
                                        <td colspan="5">
                                            <asp:Label ID="lblbrieflegal" runat="server"></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <br />

                <div class="panel panel-default" style="border-color: #280277;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#collapse3" style="color: #FFFFFF; background-color: #280277; border-color: #280277">
                        <h4 class="panel-title">
                            <strong style="cursor: pointer; text-decoration: underline;">Tax Parcels And Agencies</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                        </h4>
                    </div>
                    <div id="collapse3" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <div class="tab-content">
                                <div class="tab-pane fade in active" id="tab1primary">

                                    <table style="width: 1290px;">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <b>
                                                        <label style="white-space: nowrap;" class="CheckBold" id="lbldrop">Tax ID Number:</label>
                                                    </b>
                                                </td>
                                                <td>
                                                    <div class="ui-widget">
                                                        <div class="select-editable">
                                                            <select runat="server" id="txtTaxNo" onchange="this.nextElementSibling.value=this.value" style="width: 225px; height: 27px; background-color: #d3d3d35c">
                                                            </select>
                                                            <input type="text" runat="server" id="txtdrop" name="format" placeholder="Tax ID" value="--Select--" autocomplete="off" onchange="return TaxparcelFunction()" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);" />
                                                        </div>
                                                    </div>
                                                    <%--<cc1:ComboBox ID="txtTaxNo" runat="server" AutoCompleteMode="SuggestAppend"></cc1:ComboBox>--%>
                                                </td>
                                                <td>
                                                    <b>
                                                        <label id="lblTaxYear" style="white-space: nowrap;" class="CheckBold">Tax Year:</label>
                                                    </b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTaxYear" runat="server" class="form-control" placeholder="YYYY" pattern="\d{4}" title="Please enter exactly 4 digits" MaxLength="4" onkeypress="return isNumberKey(event)" onblur="checkReqFields(this.value,this,event);IsValidLengthTaxYear(this.value,this,event);" onpaste="return isNumber(event)" autocomplete='off' Style="width: 95px;" onchange="return TaxparcelFunction()" />
                                                </td>
                                                <td>
                                                    <b style="white-space: nowrap;" class="CheckBold">End Year(If Applicable):</b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEndYear" runat="server" class="form-control" placeholder="YYYY" MaxLength="4" onkeypress="return isNumberKey(event);" pattern="\d{4}" title="Please enter exactly 4 digits" onblur="checkReqFields1(this.value,this,event);IsValidLengthEndYear(this.value,this,event);" onpaste="return isNumber(event)" autocomplete='off' Style="width: 95px;" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <b style="white-space: nowrap;" class="CheckBold">TBD:</b>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkTBD" runat="server" />
                                                </td>
                                                <td>
                                                    <b style="white-space: nowrap;" class="CheckBold">Estimate:</b>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkEst" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btntaxparcels" class="btn btn-success" runat="server" Text="Add Tax Parcel"
                                                        AutoPostBack="false" OnClick="btntaxparcel_Click" OnClientClick="return TaxparcelFunction()" Style="margin-left: 10px;" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <br />
                                <div id="taxPar" runat="server" style="height: auto; overflow: auto; margin-top: -15px;">
                                    <asp:Panel ID="gvTax" runat="server">
                                        <asp:GridView ID="gvTaxParcel" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" CssClass="Grid"
                                            DataKeyNames="taxid" Visible="false" Width="100%" OnRowDataBound="OnRowDataBound"
                                            OnRowEditing="gvTaxParcel_RowEditing" OnRowUpdating="gvTaxParcel_RowUpdating"
                                            OnRowCommand="btnTaxParcelModal_RowCommand" EmptyDataText="No Data Found" EmptyDataRowStyle-HorizontalAlign="Center"
                                            OnRowCancelingEdit="gvTaxParcel_RowCancelingEdit">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <img style="width: 30px; text-align: center;" id="edit-save" src="../images/STRArrow.png" />
                                                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                                            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="Grid" Width="90%" GridLines="None"
                                                                OnRowCommand="btnOrders_RowCommand" DataKeyNames="Id">
                                                                <Columns>
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="Id" HeaderText="Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />

                                                                    <asp:TemplateField HeaderText="Agency Id" ItemStyle-Width="30%">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkOrder" runat="server" ForeColor="Blue"
                                                                                DataField="agencyid" HeaderText="Tax Id" ItemStyle-Width="20%" OnClick="lnkgvOrders_Click"
                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.agencyid") %>' autopostback="false"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:BoundField ItemStyle-Width="30%" DataField="TaxAuthorityName" HeaderText="Authority Name" />
                                                                    <%--<asp:BoundField ItemStyle-Width="30%" DataField="TaxAgencyType" HeaderText="Tax Type" />--%>

                                                                    <asp:TemplateField HeaderText="Tax Type" ItemStyle-Width="30%">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkAgnecy" runat="server" ForeColor="Blue"
                                                                                DataField="TaxAgencyType" HeaderText="Tax Type" ItemStyle-Width="20%" OnClick="lnkAgnecy_Click"
                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.TaxAgencyType") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="TaxAgencyState" HeaderText="State" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="Phone" HeaderText="Phone" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="taxid" HeaderText="Tax ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="Id" HeaderText="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="TaxYearStartDate" HeaderText="Tax Year Start Date" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="PreferredContactMethod" HeaderText="PreferredContactMethod" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="JobTitle" HeaderText="JobTitle" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="City" HeaderText="City" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="County" HeaderText="County" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="State" HeaderText="State" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="ContactType" HeaderText="ContactType" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="Zip" HeaderText="Zip" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="Address1" HeaderText="Address" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />


                                                                    <asp:TemplateField HeaderText="Actions">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton runat="server" ID="btnSubDelete" CommandName="DeleteOrders" OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');" ItemStyle-Width="10%" Style="height: 30px; margin-left: 18px;" ToolTip="Delete" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' class="glyphicon glyphicon-trash" CssClass=""></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle BackColor="#f3f2ea" />
                                                                <HeaderStyle BackColor="#f94848" ForeColor="white" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField ItemStyle-Width="50%" DataField="Id" HeaderText="Tax ID" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                <asp:BoundField ItemStyle-Width="30%" DataField="taxid" HeaderText="Tax ID Number" ReadOnly="true" />
                                                <asp:BoundField ItemStyle-Width="30%" DataField="taxyear" HeaderText="Tax Year" ReadOnly="true" />
                                                <asp:BoundField ItemStyle-Width="20%" DataField="endyear" HeaderText="End Year" ReadOnly="true" />
                                                <asp:BoundField ItemStyle-Width="50%" DataField="tbd" HeaderText="TBD" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                <asp:BoundField ItemStyle-Width="50%" DataField="estimate" HeaderText="Estimate" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="HdnId" runat="server" Value='<%# Bind("Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LnkEdit" runat="server" class="glyphicon glyphicon-edit" CommandName="Edit" Style="height: 30px; margin-left: 5px; margin-bottom: -10px;" ToolTip="Edit" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="btn_Update" runat="server" class="glyphicon glyphicon-ok" Style="height: 30px; margin-left: 10px;" ToolTip="Update" CommandName="Update" OnClientClick="return TaxparcelFunction()" />
                                                        <asp:LinkButton ID="btn_Cancel" runat="server" class="glyphicon glyphicon-remove" Style="height: 30px; margin-left: 10px;" ToolTip="Cancel" CommandName="Cancel" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnParDelete" runat="server" class="glyphicon glyphicon-trash" OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');" CommandName="SelectParDelete" Style="height: 30px; margin-left: 10px; margin-bottom: -10px;" ToolTip="Delete" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnAddAuthor" runat="server" CommandName="SelectAddAuthor" Style="width: 120px; font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif; white-space: nowrap; cursor: pointer" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>'><i title="Add Authorities"><img src="../images/Blue User.png"/ style="width:15px;margin-top:-5px;"></i>+ Authorities</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#f94848" ForeColor="white" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <br />

                <asp:Panel ID="PnlTax" class="panel panel-default" runat="server" Visible="false" Style="border-color: #280277;">

                    <div class="panel-heading" data-toggle="collapse" data-target="#collapse4" style="color: #FFFFFF; background-color: #280277; border-color: #280277;">
                        <h4 class="panel-title">
                            <strong style="cursor: pointer; text-decoration: underline;">Tax Authorities</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                        </h4>
                    </div>
                    <br />
                    <br />
                    <div id="collapse4" style="margin-top: -3px;" class="panel-collapse collapse in">
                        <div style="margin-top: -45px;">
                            <div class="tab-content">
                                <div class="tab-pane fade in active" id="tab2primary">
                                    <br />
                                    <div class="col-xs-8">
                                        <div class="table-responsive">
                                            <table style="width: 845px;">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 130px;">
                                                            <b class="CheckBold">Tax Id</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="LblTaxID" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td style="width: 90px;">
                                                            <b class="CheckBold">Agencyid</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td style="width: 105px;">
                                                            <asp:Label ID="LblAgencyID" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td style="width: 110px;">
                                                            <b class="CheckBold">Authority Name</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtAuthorityname" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b class="CheckBold" title="Prefered Contact Method">Contact Method</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtprefcontactmethod" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b class="CheckBold">Email</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtemail" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b class="CheckBold">Phone</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtphone" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td><b class="CheckBold">Job Title</b></td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="Lbljobtitle" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td><b class="CheckBold">City</b></td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="LblCity1" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td><b class="CheckBold">County</b></td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtCounty" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b class="CheckBold">Tax Agency Type</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td style="width: 145px; white-space: nowrap">
                                                            <asp:Label ID="txtTaxType" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b class="CheckBold">Fax</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtfax" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b class="CheckBold">State</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtState" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b class="CheckBold">Tax Year Start Date</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtstartyeardate" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b class="CheckBold">ContactType</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtcontactType" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b class="CheckBold">Zip</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="txtZip" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <b class="CheckBold">Address</b>
                                                        </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="Lbladdress" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b class="CheckBold">OperationHours</b> </td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="lblOperation" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b class="CheckBold" style="white-space: nowrap;">No of Phone Calls</b></td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:Label ID="lblphNos" runat="server" Style="font-size: 14px;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="col-xs-4">
                                        <div class="table-responsive">
                                            <asp:Panel ID="Panel1" Height="144px" runat="server">
                                                <asp:GridView ID="gridwebsite" runat="server" AutoGenerateColumns="false" GridLines="None">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="WebSite" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <%-- <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Bind("url") %>' onclick ="openWindow(this.href); return false;">--%>
                                                                <%--<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Bind("url", "javascript:openWindow(&#039;{0}&#039;);") %>'>--%>
                                                                <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%# Bind("url") %>'>
                                                                    <asp:Label ID="lnkwebsite3" runat="server" Text='<%# Bind("url") %>'></asp:Label>
                                                                </asp:HyperLink>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="Description" HeaderText="Description" ReadOnly="true">
                                                            <ItemStyle Width="30%" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#f3f2ea" />
                                                    <HeaderStyle BackColor="#f94848" ForeColor="white" />
                                                    <EmptyDataTemplate>
                                                        <div style="color: red; font-weight: bold;" align="center">No records found.</div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="panel-body" style="margin-top: 130px;">
                            <div style="border-top: 1px solid #e5e5e5;"></div>
                            <h4 class="panel-title" style="margin-top: 0px;">
                                <strong style="margin-bottom: -30px; text-decoration: underline;">Miscellaneous Info</strong>
                            </h4>

                            <div style="border-top: 1px solid #e5e5e5; margin-top: 6px;">
                            </div>
                            <div class="tab-content">
                                <div class="tab-pane fade in active">
                                    <fieldset>
                                        <div class="col-md-3">
                                            <div class="col-md-12">
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="txtmisc" runat="server" class="form-control" Style="margin-left: -40px; width: 1280px; height: 130px; resize: none; background-color: white; cursor: default" TextMode="MultiLine" ReadOnly />
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="panel-body" style="margin-top: -55px;">
                            <%--<div style="border-top: 1px solid #e5e5e5; margin-top: 8px;"></div>--%>
                            <%-- <h4 class="panel-title" style="margin-top: -27px;">
                                <strong style="margin-bottom: -30px; text-decoration: underline;">Tax Authority Questions</strong>
                            </h4>--%>

                            <%-- <div style="border-top: 1px solid #e5e5e5; margin-top: -24px;">
                                <br />

                            </div>--%>
                            <div class="tab-content">
                                <div class="tab-pane fade in active">
                                    <br />
                                    <fieldset>
                                        <div class="col-md-12">
                                            <div class="col-md-2 Label">
                                                <b style="margin-left: -67px; color: red; font-size: 18px;" class="CheckBold">Annual Tax Amount: </b>

                                            </div>
                                            <div class="col-md-2">
                                                <b style="margin-left: -170px; color: black; font-size: 18px;" class="CheckBold">
                                                    <asp:Label ID="txtAnnualTaxAmount" runat="server" /></b>
                                            </div>
                                        </div>
                                        <br />

                                        <table class="table table-striped table-hover">
                                            <thead style="background-color: #f94848; color: #fff;">
                                                <tr>
                                                    <th>Installment 1</th>
                                                    <th>Installment 2</th>
                                                    <th>Installment 3</th>
                                                    <th>Installment 4</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 55px;" class="CheckBold">Inst.Amount:</label>
                                                            <input type="text" id="instamount1" runat="server" class="form-control taxing" placeholder="Tax Amount" style="width: 150px;" onkeyup="hello1();" onfocusout="myFunction1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytxtamount1()" autocomplete='off' tabindex="1" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 55px;" class="CheckBold">Inst.Amount:</label>
                                                            <input type="text" id="instamount2" runat="server" class="form-control taxing" placeholder="Tax Amount" style="width: 150px;" onkeyup="hello2();" onfocusout="myFunction2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytxtamount2()" autocomplete='off' tabindex="10" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 55px;" class="CheckBold">Inst.Amount:</label>
                                                            <input type="text" id="instamount3" runat="server" class="form-control taxing" placeholder="Tax Amount" style="width: 150px;" onkeyup="hello3();" onfocusout="myFunction3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytxtamount3()" autocomplete='off' tabindex="19" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 55px;" class="CheckBold">Inst.Amount:</label>
                                                            <input type="text" id="instamount4" runat="server" class="form-control taxing" placeholder="Tax Amount" style="width: 150px;" onkeyup="hello4();" onfocusout="myFunction4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytxtamount4()" autocomplete='off' tabindex="27" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 23px;" class="CheckBold">Inst.Amount Paid:</label>
                                                            <input type="text" id="instamountpaid1" runat="server" class="form-control taxing" placeholder="Amount Paid" style="width: 150px;" onkeyup="AmtPaid1();" onfocusout="myFunctionAmtPaid1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytest1();" autocomplete='off' tabindex="2" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 23px;" class="CheckBold">Inst.Amount Paid:</label>
                                                            <input type="text" id="instamountpaid2" runat="server" class="form-control taxing" placeholder="Amount Paid" style="width: 150px;" onkeyup="AmtPaid2();" onfocusout="myFunctionAmtPaid2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytest2();" autocomplete='off' tabindex="11" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 23px;" class="CheckBold">Inst.Amount Paid:</label>
                                                            <input type="text" id="instamountpaid3" runat="server" class="form-control taxing" placeholder="Amount Paid" style="width: 150px;" onkeyup="AmtPaid3();" onfocusout="myFunctionAmtPaid3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytest3();" autocomplete='off' tabindex="19" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 23px;" class="CheckBold">Inst.Amount Paid:</label>
                                                            <input type="text" id="instamountpaid4" runat="server" class="form-control taxing" placeholder="Amount Paid" style="width: 150px;" onkeyup="AmtPaid4();" onfocusout="myFunctionAmtPaid4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytest4();" autocomplete='off' tabindex="28" />
                                                        </div>
                                                    </td>
                                                    <td hidden>
                                                        <p id="w"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="a"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="e"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="i"></p>
                                                    </td>

                                                    <td hidden>
                                                        <p id="x"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="b"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="f"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="j"></p>
                                                    </td>

                                                    <td hidden>
                                                        <p id="c"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="y"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="g"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="k"></p>
                                                    </td>

                                                    <td hidden>
                                                        <p id="z"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="d"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="h"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="l"></p>
                                                    </td>
                                                    <td hidden>
                                                        <p id="TA"></p>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Inst.Paid/Due?</label>
                                                            <select id="instpaiddue1" runat="server" class="form-control taxing" style="width: 150px;" tabindex="3">
                                                                <option>Select</option>
                                                                <option>Paid</option>
                                                                <option>Due</option>
                                                            </select>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Inst.Paid/Due?</label>
                                                            <select id="instpaiddue2" runat="server" class="form-control taxing" style="width: 150px;" tabindex="12">
                                                                <option>Select</option>
                                                                <option>Paid</option>
                                                                <option>Due</option>
                                                            </select>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Inst.Paid/Due?</label>
                                                            <select id="instpaiddue3" runat="server" class="form-control taxing" style="width: 150px;" tabindex="20">
                                                                <option>Select</option>
                                                                <option>Paid</option>
                                                                <option>Due</option>
                                                            </select>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Inst.Paid/Due?</label>
                                                            <select id="instpaiddue4" runat="server" class="form-control taxing" style="width: 150px;" tabindex="29">
                                                                <option>Select</option>
                                                                <option>Paid</option>
                                                                <option>Due</option>
                                                            </select>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hdntxtbxTaksit1" runat="server" Value=""></asp:HiddenField>
                                                        <asp:HiddenField ID="hdntxtbxTaksit2" runat="server" Value=""></asp:HiddenField>
                                                        <asp:HiddenField ID="hdntxtbxTaksit3" runat="server" Value=""></asp:HiddenField>
                                                        <asp:HiddenField ID="hdntxtbxTaksit4" runat="server" Value=""></asp:HiddenField>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 12px;" class="CheckBold">Remaining Balance:</label>
                                                            <input type="text" id="remainingbalance1" class="form-control taxing" runat="server" placeholder="Remaining Balance" style="width: 150px;" onkeyup="RemBalance1();" onfocusout="myFunctionRemBalance1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="myremamount1()" autocomplete="off" tabindex="4" onchange="setTwoNumberDecimal(this)" />
                                                            <%--<input type="text" id="remainingbalance1" class="form-control taxing" runat="server" placeholder="Remaining Balance" style="width: 150px;" onkeypress="return isNumberKey(event)" onkeyup="RemBalance1(event);" autocomplete="off" tabindex="4" onchange="setTwoNumberDecimal(this)" />--%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 12px;" class="CheckBold">Remaining Balance:</label>
                                                            <input type="text" id="remainingbalance2" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeyup="RemBalance2();" onfocusout="myFunctionRemBalance2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="myremamount2()" autocomplete="off" tabindex="13" onchange="setTwoNumberDecimal(this)" />
                                                            <%--<input type="text" id="remainingbalance2" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeypress="return isNumberKey(event)" onkeyup="RemBalance2(event);" autocomplete="off" tabindex="13" onchange="setTwoNumberDecimal(this)" />--%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 12px;" class="CheckBold">Remaining Balance:</label>
                                                            <input type="text" id="remainingbalance3" class="form-control taxing" runat="server" placeholder="Remaining Balance" style="width: 150px;" onkeyup="RemBalance3();" onfocusout="myFunctionRemBalance3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="myremamount3()" autocomplete="off" tabindex="21" onchange="setTwoNumberDecimal(this)" />
                                                            <%--<input type="text" id="remainingbalance3" class="form-control taxing" runat="server" placeholder="Remaining Balance" style="width: 150px;" onkeypress="return isNumberKey(event)" onkeyup="RemBalance3(event);" autocomplete="off" tabindex="21" onchange="setTwoNumberDecimal(this)" />--%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 12px;" class="CheckBold">Remaining Balance:</label>
                                                            <input type="text" id="remainingbalance4" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeyup="RemBalance4();" onfocusout="myFunctionRemBalance4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="myremamount4()" autocomplete="off" tabindex="30" onchange="setTwoNumberDecimal(this)" />
                                                            <%--<input type="text" id="remainingbalance4" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeypress="return isNumberKey(event)" onkeyup="RemBalance4(event);" autocomplete="off" tabindex="30" onchange="setTwoNumberDecimal(this)" />--%>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 28px;" class="CheckBold">Installment Date:</label>
                                                            <input type="text" id="instdate1" name="instdate1" class="form-control taxing" runat="server" style="width: 150px;" maxlength="10" onchange="checkinstdate1();" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" onkeypress="return isNumberKey1(event)" placeholder="MM/DD/YYYY" autocomplete="off" tabindex="5" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 28px;" class="CheckBold">Installment Date:</label>
                                                            <input type="text" id="instdate2" runat="server" class="form-control taxing" style="width: 150px;" maxlength="10" onchange="checkinstdate2();" placeholder="MM/DD/YYYY" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkmanualDEinstdate2(); checkDate(this,event)" autocomplete="off" tabindex="14" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 28px;" class="CheckBold">Installment Date:</label>
                                                            <input type="text" id="instdate3" runat="server" class="form-control taxing" style="width: 150px;" maxlength="10" onchange="checkinstdate3();" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkmanualDEinstdate3(); checkDate(this,event)" placeholder="MM/DD/YYYY" autocomplete="off" tabindex="22" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 28px;" class="CheckBold">Installment Date:</label>
                                                            <input type="text" id="instdate4" runat="server" class="form-control taxing" style="width: 150px;" maxlength="10" onchange="checkinstdate4();" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkmanualDEinstdate4(); checkDate(this,event)" placeholder="MM/DD/YYYY" autocomplete="off" tabindex="31" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 29px;" class="CheckBold">Delinquent Date:</label>
                                                            <input type="text" id="delinq1" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkINSTDEDate1(); checkDate(this,event)" autocomplete="off" tabindex="6" onchange="functionTaxBill(this);samedate1(this.value);" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 29px;" class="CheckBold">Delinquent Date:</label>
                                                            <input type="text" id="delinq2" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkINSTDEDate2(); checkDate(this,event)" autocomplete="off" tabindex="15" onchange="functionTaxBill(this);samedate2(this.value);" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 29px;" class="CheckBold">Delinquent Date:</label>
                                                            <input type="text" id="delinq3" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkINSTDEDate3(); checkDate(this,event)" autocomplete="off" tabindex="23" onchange="functionTaxBill(this);samedate3(this.value);" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 29px;" class="CheckBold">Delinquent Date:</label>
                                                            <input type="text" id="delinq4" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkINSTDEDate4(); checkDate(this,event)" autocomplete="off" tabindex="32" onchange="functionTaxBill(this);samedate4(this.value);" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 21px;" class="CheckBold">Discount Amount:</label>
                                                            <input type="text" id="discamt1" runat="server" class="form-control taxing" placeholder="Discount Amount" style="width: 150px;" onkeyup="Discount1();" onfocusout="myFunctionDiscount1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mydiscountamount1();" onchange="greateramount1(this.value);" autocomplete="off" tabindex="7" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 21px;" class="CheckBold">Discount Amount:</label>
                                                            <input type="text" id="discamt2" runat="server" class="form-control taxing" placeholder="Discount Amount" style="width: 150px;" onkeyup="Discount2();" onfocusout="myFunctionDiscount2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mydiscountamount2();" onchange="greateramount2(this.value)" autocomplete="off" tabindex="16" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 21px;" class="CheckBold">Discount Amount:</label>
                                                            <input type="text" id="discamt3" runat="server" class="form-control taxing" placeholder="Discount Amount" style="width: 150px;" onkeyup="Discount3();" onfocusout="myFunctionDiscount3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mydiscountamount3()" onchange="greateramount3(this.value)" autocomplete="off" tabindex="24" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 21px;" class="CheckBold">Discount Amount:</label>
                                                            <input type="text" id="discamt4" runat="server" class="form-control taxing" placeholder="Discount Amount" style="width: 150px;" onkeyup="Discount4();" onfocusout="myFunctionDiscount4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mydiscountamount4();" onchange="greateramount4(this.value)" autocomplete="off" tabindex="33" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Discount Date:</label>
                                                            <input type="text" id="discdate1" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkDISDate1(); return checkDate(this,event)" autocomplete="off" tabindex="8" onchange="discountdate1(this.value);" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Discount Date:</label>
                                                            <input type="text" id="discdate2" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkDISDate2(); return checkDate(this,event)" autocomplete="off" tabindex="17" onchange="discountdate2(this.value);" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Discount Date:</label>
                                                            <input type="text" id="discdate3" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkDISDate3(); return checkDate(this,event)" autocomplete="off" tabindex="25" onchange="discountdate3(this.value);" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group" style="margin-bottom: 0px;">
                                                            <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Discount Date:</label>
                                                            <input type="text" id="discdate4" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkDISDate4(); return checkDate(this,event)" autocomplete="off" tabindex="34" onchange="discountdate4(this.value);" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="CheckBold">Exempt/Relevy?</label>
                                                        <input type="checkbox" id="exemptrelevy1" runat="server" style="width: 17px; height: 17px; margin-left: 32px;" tabindex="9" />
                                                    </td>
                                                    <td>
                                                        <label class="CheckBold">Exempt/Relevy?</label>
                                                        <input type="checkbox" id="exemptrelevy2" runat="server" style="width: 17px; height: 17px; margin-left: 32px;" tabindex="18" />
                                                    </td>
                                                    <td>
                                                        <label class="CheckBold">Exempt/Relevy?</label>
                                                        <input type="checkbox" id="exemptrelevy3" runat="server" style="width: 17px; height: 17px; margin-left: 32px;" tabindex="26" />
                                                    </td>
                                                    <td>
                                                        <label class="CheckBold">Exempt/Relevy?</label>
                                                        <input type="checkbox" id="exemptrelevy4" runat="server" style="width: 17px; height: 17px; margin-left: 32px;" tabindex="35" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <table style="width: 1283px;">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <b>
                                                            <label id="lblnextbilldate1" style="white-space: nowrap;" class="CheckBold">Next Bill Date1:</label>
                                                        </b>
                                                    </td>
                                                    <td>
                                                        <input type="text" id="nextbilldate1" runat="server" class="form-control" style="width: 170px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkNextBillDate(); return checkDate(this,event)" autocomplete="off" onchange="samedatenextbill(this.value);return functionInsttax();" tabindex="37" />
                                                    </td>
                                                    <td>
                                                        <b style="white-space: nowrap" class="CheckBold">Next Bill Date2:</b>
                                                    </td>
                                                    <td>
                                                        <input type="text" id="nextbilldate2" runat="server" class="form-control" style="width: 167px; margin-bottom: 5px" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete="off" tabindex="38" />
                                                    </td>
                                                    <td>
                                                        <b style="white-space: nowrap;" id="dd" runat="server" class="CheckBold">Future Tax Calculation:</b>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlfuturetaxcalc" runat="server" Style="width: 179px;" class="form-control" OnSelectedIndexChanged="ddlfuturetaxcalc_SelectedIndexChanged" AutoPostBack="True">
                                                            <asp:ListItem>Select</asp:ListItem>
                                                            <asp:ListItem>Manual</asp:ListItem>
                                                            <asp:ListItem>SameAsCurrent</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>


                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b style="white-space: nowrap;" class="CheckBold">Payment Frequency:</b>
                                                    </td>
                                                    <td>
                                                        <select class="form-control" id="paymentfrequency" runat="server" style="width: 170px; margin-top: 10px;" onchange="functionpayemtfrequency(this)" tabindex="39">
                                                            <option value="1">Annual</option>
                                                            <option value="2">SemiAnnual</option>
                                                            <option value="3">TriAnnual</option>
                                                            <option value="4">Quarterly</option>
                                                        </select>
                                                        <%--   <asp:DropDownList ID="paymentfrequency" runat="server"  style="width: 170px;" TabIndex="39" OnSelectedIndexChanged="paymentfrequency_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Value="1">Annual</asp:ListItem>
                                                            <asp:ListItem Value="2">Semi-Annual</asp:ListItem>
                                                            <asp:ListItem Value="3">TriAnnual</asp:ListItem>
                                                            <asp:ListItem Value="4">Quarterly</asp:ListItem>
                                                        </asp:DropDownList>--%>
                                                    </td>
                                                    <td>
                                                        <b style="white-space: nowrap" class="CheckBold" runat="server" id="lblbillingstartdate">Billing Start Date:</b>
                                                    </td>
                                                    <td>
                                                        <input type="text" id="txtbillstartdate" runat="server" class="form-control" style="width: 166px; margin-bottom: 10px" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete="off" tabindex="40" />
                                                    </td>
                                                    <td>
                                                        <b style="white-space: nowrap" runat="server" id="lblbillingenddate" class="CheckBold">Billing End Date:</b>
                                                    </td>
                                                    <td>
                                                        <input type="text" id="txtbillenddate" runat="server" class="form-control" style="width: 180px; margin-bottom: 5px" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete="off" tabindex="41" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b style="white-space: nowrap" class="CheckBold">Tax Bill:</b>
                                                    </td>
                                                    <td>
                                                        <select class="form-control" id="taxbill" runat="server" style="width: 170px;" tabindex="42">
                                                            <option value="0">Select Bill</option>
                                                            <option value="1">Current</option>
                                                            <option value="2">Previous</option>
                                                        </select>
                                                    </td>

                                                    <td>
                                                        <b class="CheckBold">Installment Comments:</b>
                                                    </td>
                                                    <td colspan="3">
                                                        <textarea id="instcomm" runat="server" placeholder="Installment Comments" class="form-control" style="resize: none; width: 636px;" autocomplete='off' tabindex="43" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);"></textarea>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnTaxParcelSave" runat="server" Text="Update" CssClass="btn btn-success" Style="margin-left: -90px;" AutoPostBack="true" OnClick="btnTaxParcelSave_Click" OnClientClick="return functionInsttax();" TabIndex="44" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="PnlTax1" class="panel panel-default" runat="server" Style="margin-top: 15px; border-color: #280277;">
                    <div id="collapsefut" style="margin-top: -3px;" class="panel-collapse collapse in">

                        <div class="panel-body" style="margin-top: -20px;">
                            <div class="tab-content">
                                <div class="tab-pane fade in active" id="tab2primary1">
                                    <div style="border-top: 1px solid #e5e5e5; margin-top: 15px;"></div>
                                    <br />
                                    <div class="tab-content">
                                        <div class="tab-pane fade in active" id="tab3primary1" style="margin-top: -8px;">
                                            <div class="col-md-12">
                                                <div class="col-md-2 Label">
                                                    <b style="margin-left: -67px; color: red; font-size: 18px;" class="CheckBold">Annual Tax Amount: </b>

                                                </div>
                                                <div class="col-md-2">
                                                    <b style="margin-left: -170px; color: black; font-size: 18px;" class="CheckBold">
                                                        <asp:Label ID="futuretxtAnnualTaxAmount" runat="server" /></b>
                                                </div>
                                            </div>
                                            <br />

                                            <table class="table table-striped table-hover">
                                                <thead style="background-color: #f94848; color: #fff;">
                                                    <tr>
                                                        <th>Installment 1</th>
                                                        <th>Installment 2</th>
                                                        <th>Installment 3</th>
                                                        <th>Installment 4</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 55px;" class="CheckBold">Inst.Amount:</label>
                                                                <input type="text" id="instmanamount1" runat="server" class="form-control taxing" placeholder="Tax Amount" style="width: 150px;" onkeyup="Futurehello1();" onfocusout="FuturemyFunction1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Futuremytxtamount1()" autocomplete='off' tabindex="53" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 55px;" class="CheckBold">Inst.Amount:</label>
                                                                <input type="text" id="instmanamount2" class="form-control taxing" runat="server" placeholder="Tax Amount" style="width: 150px;" onkeyup="Futurehello2();" onfocusout="FuturemyFunction2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Futuremytxtamount2()" autocomplete='off' tabindex="62" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 55px;" class="CheckBold">Inst.Amount:</label>
                                                                <input type="text" id="instmanamount3" class="form-control taxing" runat="server" placeholder="Tax Amount" style="width: 150px;" onkeyup="Futurehello3();" onfocusout="FuturemyFunction3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Futuremytxtamount3()" autocomplete='off' tabindex="71" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 55px;" class="CheckBold">Inst.Amount:</label>
                                                                <input type="text" id="instmanamount4" class="form-control taxing" runat="server" placeholder="Tax Amount" style="width: 150px;" onkeyup="Futurehello4();" onfocusout="FuturemyFunction4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Futuremytxtamount4()" autocomplete='off' tabindex="80" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 23px;" class="CheckBold">Inst.Amount Paid:</label>
                                                                <input type="text" id="instmanamtpaid1" runat="server" class="form-control taxing" placeholder="Amount Paid" style="width: 150px;" onkeyup="FutureAmtPaid1();" onfocusout="FuturemyFunctionAmtPaid1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Futuremytest1();" autocomplete='off' tabindex="54" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 23px;" class="CheckBold">Inst.Amount Paid:</label>
                                                                <input type="text" id="instmanamtpaid2" runat="server" class="form-control taxing" placeholder="Amount Paid" style="width: 150px;" onkeyup="FutureAmtPaid2();" onfocusout="FuturemyFunctionAmtPaid2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Futuremytest2();" autocomplete='off' tabindex="63" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 23px;" class="CheckBold">Inst.Amount Paid:</label>
                                                                <input type="text" id="instmanamtpaid3" runat="server" class="form-control taxing" placeholder="Amount Paid" style="width: 150px;" onkeyup="FutureAmtPaid3();" onfocusout="FuturemyFunctionAmtPaid3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Futuremytest3();" autocomplete='off' tabindex="72" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 23px;" class="CheckBold">Inst.Amount Paid:</label>
                                                                <input type="text" id="instmanamtpaid4" runat="server" class="form-control taxing" placeholder="Amount Paid" style="width: 150px;" onkeyup="FutureAmtPaid4();" onfocusout="FuturemyFunctionAmtPaid4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Futuremytest4();" autocomplete='off' tabindex="81" />
                                                            </div>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fw"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fa"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fe"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fi"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fx"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fb"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="ff"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fj"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fc"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fy"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fg"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="k"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fz"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fd"></p>
                                                        </td>

                                                        <td hidden>
                                                            <p id="fh"></p>
                                                        </td>
                                                        <td hidden>
                                                            <p id="fl"></p>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Inst.Paid/Due?</label>
                                                                <select id="ddlmaninstpaiddue1" runat="server" class="form-control taxing" style="width: 150px;" tabindex="55">
                                                                    <option>Select</option>
                                                                    <option>Paid</option>
                                                                    <option>Due</option>
                                                                </select>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Inst.Paid/Due?</label>
                                                                <select id="ddlmaninstpaiddue2" runat="server" class="form-control taxing" style="width: 150px;" tabindex="64">
                                                                    <option>Select</option>
                                                                    <option>Paid</option>
                                                                    <option>Due</option>
                                                                </select>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Inst.Paid/Due?</label>
                                                                <select id="ddlmaninstpaiddue3" runat="server" class="form-control taxing" style="width: 150px;" tabindex="73">
                                                                    <option>Select</option>
                                                                    <option>Paid</option>
                                                                    <option>Due</option>
                                                                </select>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Inst.Paid/Due?</label>
                                                                <select id="ddlmaninstpaiddue4" runat="server" class="form-control taxing" style="width: 150px;" tabindex="82">
                                                                    <option>Select</option>
                                                                    <option>Paid</option>
                                                                    <option>Due</option>
                                                                </select>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <asp:HiddenField ID="futurehdntxtbxTaksit1" runat="server" Value=""></asp:HiddenField>
                                                        <asp:HiddenField ID="futurehdntxtbxTaksit2" runat="server" Value=""></asp:HiddenField>
                                                        <asp:HiddenField ID="futurehdntxtbxTaksit3" runat="server" Value=""></asp:HiddenField>
                                                        <asp:HiddenField ID="futurehdntxtbxTaksit4" runat="server" Value=""></asp:HiddenField>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 12px;" class="CheckBold">Remaining Balance:</label>
                                                                <input type="text" id="txtmanurembal1" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeyup="futureRemBalance1();" onfocusout="futuremyFunctionRemBalance1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="futuremyremamount1()" autocomplete="off" tabindex="56" onchange="setTwoNumberDecimal(this)" />
                                                                <%--<input type="text" id="txtmanurembal1" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeypress="return isNumberKey(event)" onkeyup="futureRemBalance1(event);" autocomplete="off" tabindex="56" onchange="setTwoNumberDecimal(this)" />--%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 12px;" class="CheckBold">Remaining Balance:</label>
                                                                <input type="text" id="txtmanurembal2" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeyup="futureRemBalance2();" onfocusout="futuremyFunctionRemBalance2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="futuremyremamount2()" autocomplete="off" tabindex="65" onchange="setTwoNumberDecimal(this)" />
                                                                <%--<input type="text" id="txtmanurembal2" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeypress="return isNumberKey(event)" onkeyup="futureRemBalance2(event);" autocomplete="off" tabindex="65" onchange="setTwoNumberDecimal(this)" />--%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 12px;" class="CheckBold">Remaining Balance:</label>
                                                                <input type="text" id="txtmanurembal3" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeyup="futureRemBalance3(event);" onfocusout="futuremyFunctionRemBalance3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="futuremyremamount3()" autocomplete="off" tabindex="74" onchange="setTwoNumberDecimal(this)" />
                                                                <%--<input type="text" id="txtmanurembal3" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeypress="return isNumberKey(event)" onkeyup="futureRemBalance3(event);" autocomplete="off" tabindex="74" onchange="setTwoNumberDecimal(this)" />--%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 12px;" class="CheckBold">Remaining Balance:</label>
                                                                <input type="text" id="txtmanurembal4" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeyup="futureRemBalance4(event);" onfocusout="futuremyFunctionRemBalance4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="futuremyremamount4()" autocomplete="off" tabindex="83" onchange="setTwoNumberDecimal(this)" />
                                                                <%--<input type="text" id="txtmanurembal4" runat="server" class="form-control taxing" placeholder="Remaining Balance" style="width: 150px;" onkeypress="return isNumberKey(event)" onkeyup="futureRemBalance4(event);" autocomplete="off" tabindex="83" onchange="setTwoNumberDecimal(this)" />--%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 28px;" class="CheckBold">Installment Date:</label>
                                                                <input type="text" id="txtmaninstdate1" name="instdate1" runat="server" class="form-control taxing" style="width: 150px;" maxlength="10" onchange="Instcheckinstdate1();" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" placeholder="MM/DD/YYYY" autocomplete="off" tabindex="57" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 28px;" class="CheckBold">Installment Date:</label>
                                                                <input type="text" id="txtmaninstdate2" runat="server" class="form-control taxing" style="width: 150px;" maxlength="10" onchange="Instcheckinstdate2();" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkmanualinstdate2(); return checkDate(this,event)" placeholder="MM/DD/YYYY" autocomplete="off" tabindex="66" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 28px;" class="CheckBold">Installment Date:</label>
                                                                <input type="text" id="txtmaninstdate3" runat="server" class="form-control taxing" style="width: 150px;" maxlength="10" onchange="Instcheckinstdate3();" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkmanualinstdate3(); return checkDate(this,event)" placeholder="MM/DD/YYYY" autocomplete="off" tabindex="75" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 28px;" class="CheckBold">Installment Date:</label>
                                                                <input type="text" id="txtmaninstdate4" runat="server" class="form-control taxing" style="width: 150px;" maxlength="10" onchange="Instcheckinstdate4();" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkmanualinstdate4(); return checkDate(this,event)" placeholder="MM/DD/YYYY" autocomplete="off" tabindex="84" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 29px;" class="CheckBold">Delinquent Date:</label>
                                                                <input type="text" id="txtmandeliqdate1" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkDate1(); return checkDate(this,event);" autocomplete="off" tabindex="58" onchange="futuresamedate1(this.value);" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 29px;" class="CheckBold">Delinquent Date:</label>
                                                                <input type="text" id="txtmandeliqdate2" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkDate2(); return checkDate(this,event)" autocomplete="off" tabindex="67" onchange="futuresamedate2(this.value);" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 29px;" class="CheckBold">Delinquent Date:</label>
                                                                <input type="text" id="txtmandeliqdate3" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkDate3(); return checkDate(this,event)" autocomplete="off" tabindex="76" onchange="futuresamedate3(this.value);" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 29px;" class="CheckBold">Delinquent Date:</label>
                                                                <input type="text" id="txtmandeliqdate4" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkDate4(); return checkDate(this,event)" autocomplete="off" tabindex="85" onchange="futuresamedate4(this.value);" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 21px;" class="CheckBold">Discount Amount:</label>
                                                                <input type="text" id="txtmandisamount1" runat="server" class="form-control taxing" placeholder="Discount Amount" style="width: 150px;" onkeyup="futureDiscount1();" onfocusout="futuremyFunctionDiscount1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="futuremydiscountamount1();" onchange="futgreateramount1(this.value)" autocomplete="off" tabindex="59" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 21px;" class="CheckBold">Discount Amount:</label>
                                                                <input type="text" id="txtmandisamount2" runat="server" class="form-control taxing" placeholder="Discount Amount" style="width: 150px;" onkeyup="futureDiscount2();" onfocusout="futuremyFunctionDiscount2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="futuremydiscountamount2();" onchange="futgreateramount2(this.value)" autocomplete="off" tabindex="68" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 21px;" class="CheckBold">Discount Amount:</label>
                                                                <input type="text" id="txtmandisamount3" runat="server" class="form-control taxing" placeholder="Discount Amount" style="width: 150px;" onkeyup="futureDiscount3();" onfocusout="futuremyFunctionDiscount3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="futuremydiscountamount3()" onchange="futgreateramount3(this.value)" autocomplete="off" tabindex="77" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 21px;" class="CheckBold">Discount Amount:</label>
                                                                <input type="text" id="txtmandisamount4" runat="server" class="form-control taxing" placeholder="Discount Amount" style="width: 150px;" onkeyup="futureDiscount4();" onfocusout="futuremyFunctionDiscount4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="futuremydiscountamount4();" onchange="futgreateramount4(this.value)" autocomplete="off" tabindex="86" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Discount Date:</label>
                                                                <input type="text" id="txtmandisdate1" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkfutINSTDEDate1(); return checkDate(this,event)" onchange="futurdiscountdate1(this.value);" autocomplete="off" tabindex="60" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Discount Date:</label>
                                                                <input type="text" id="txtmandisdate2" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkfutINSTDEDate2(); return checkDate(this,event)" onchange="futurdiscountdate2(this.value);" autocomplete="off" tabindex="69" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Discount Date:</label>
                                                                <input type="text" id="txtmandisdate3" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkfutINSTDEDate3(); return checkDate(this,event)" onchange="futurdiscountdate3(this.value);" autocomplete="off" tabindex="78" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group" style="margin-bottom: 0px;">
                                                                <label style="text-align: right; clear: both; float: left; margin-right: 44px;" class="CheckBold">Discount Date:</label>
                                                                <input type="text" id="txtmandisdate4" runat="server" class="form-control taxing" style="width: 150px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="checkfutINSTDEDate4(); return checkDate(this,event)" onchange="futurdiscountdate4(this.value);" autocomplete="off" tabindex="87" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label class="CheckBold">Exempt/Relevy?</label>
                                                            <input type="checkbox" id="chkexrelmanu1" runat="server" style="width: 17px; height: 17px; margin-left: 32px;" tabindex="61" />
                                                        </td>
                                                        <td>
                                                            <label class="CheckBold">Exempt/Relevy?</label>
                                                            <input type="checkbox" id="chkexrelmanu2" runat="server" style="width: 17px; height: 17px; margin-left: 32px;" tabindex="70" />
                                                        </td>
                                                        <td>
                                                            <label class="CheckBold">Exempt/Relevy?</label>
                                                            <input type="checkbox" id="chkexrelmanu3" runat="server" style="width: 17px; height: 17px; margin-left: 32px;" tabindex="79" />
                                                        </td>
                                                        <td>
                                                            <label class="CheckBold">Exempt/Relevy?</label>
                                                            <input type="checkbox" id="chkexrelmanu4" runat="server" style="width: 17px; height: 17px; margin-left: 32px;" tabindex="88" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <table style="width: 1283px;">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <b style="white-space: nowrap" class="CheckBold">Tax Bill:</b>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="ddlmanutaxbill" runat="server" class="form-control" style="width: 166px; margin-bottom: 5px;" autocomplete="off" tabindex="89" />
                                                            <%--  <select class="form-control" id="ddlmanutaxbill" runat="server" style="width: 170px; margin-bottom: 5px;" tabindex="89">
                                                                <option value="0">Select Bill</option>
                                                                <option value="1">Current</option>
                                                                <option value="2">Previous</option>
                                                            </select>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b style="white-space: nowrap;" class="CheckBold">Payment Frequency:</b>
                                                        </td>
                                                        <td>
                                                            <select class="form-control" id="ddlpayfreqmanu" runat="server" onchange="functionpayemtfrequency1(this)" style="width: 170px; margin-bottom: 5px;" tabindex="90">
                                                                <option value="1">Annual</option>
                                                                <option value="2">SemiAnnual</option>
                                                                <option value="3">TriAnnual</option>
                                                                <option value="4">Quarterly</option>
                                                            </select>
                                                        </td>
                                                        <td>
                                                            <b style="white-space: nowrap" class="CheckBold">Billing Start Date: </b>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtmanubillstartdate" runat="server" class="form-control" style="width: 166px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete="off" tabindex="91" />
                                                        </td>
                                                        <td>
                                                            <b style="white-space: nowrap" class="CheckBold">Billing End Date:</b>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtmanubillenddate" runat="server" class="form-control" style="width: 180px;" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete="off" tabindex="92" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b class="CheckBold">Installment Comments:</b>
                                                        </td>
                                                        <td colspan="3">
                                                            <textarea id="txtinstcommentsmanual" placeholder="Installment Comments" runat="server" class="form-control" rows="2" style="resize: none;" autocomplete='off' tabindex="93" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);"></textarea>
                                                        </td>
                                                        <td>
                                                            <%--<asp:Button ID="btntaxparcelsavemanual" runat="server" Text="Save" OnClick="btntaxparcelsavemanual_Click" CssClass="btn btn-success" Style="margin-left: 25px;" AutoPostBack="true" " />--%>

                                                            <asp:Button ID="btnsavetaxauthorities" runat="server" Text="Save" CssClass="btn btn-success" Style="margin-left: 25px;" AutoPostBack="true" OnClick="btnsavetaxauthorities_Click" OnClientClick="return functionfutinst();" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="deliexemspecial" TabIndex="45" class="panel panel-default" runat="server" Style="border-color: #280277; margin-top: 12px;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#collapse10" style="color: #FFFFFF; background-color: #280277; border-color: #280277;">
                        <h4 class="panel-title">
                            <i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                            <strong style="float: left">
                                <label class="colorbold" style="padding-left: 58px; color: #FFFFFF"><b class="CheckBold">Tax Id:</b></label>
                                <asp:Label ID="LblTaxId1" runat="server" Style="font-size: 14px; margin-bottom: 10px; font-weight: bold" />
                            </strong>
                            <strong style="cursor: pointer; text-decoration: underline;">Delinquency and Exemption Status</strong>
                            <strong style="float: right">
                                <label class="colorbold" style="color: #FFFFFF"><b class="CheckBold">Agency Id:</b></label>
                                <asp:Label ID="LblAgencyId1" runat="server" Style="font-size: 14px; font-weight: bold; margin-right: 60px;" />
                            </strong>
                        </h4>
                    </div>
                    <div id="collapse10" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <table style="width: 1200px">
                                <tbody>
                                    <tr>
                                        <td style="padding-left: 13px;"><b class="CheckBold">Are there any delinquencies?:</b></td>
                                        <td>
                                            <asp:DropDownList ID="txtdeliquent" runat="server" Style="margin-bottom: 5px;" class="form-control" onchange="txtdeliquentsta1()">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem>No</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold">Is this property taxed as the primary residence? :</b></td>
                                        <td>
                                            <asp:DropDownList ID="txtResidence" runat="server" class="form-control" Style="width: 100px;">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem>No</asp:ListItem>
                                                <asp:ListItem>Not Applicable</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 13px;"><b class="CheckBold">Are there any additional Exemptions?:</b></td>
                                        <td>
                                            <asp:DropDownList ID="txtexemption" runat="server" Style="margin-bottom: 5px;" AutoPostBack="false" class="form-control" onchange="txtexemption1()">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem>No</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold">Special Assessment?:</b></td>
                                        <td>
                                            <asp:DropDownList ID="SecialAssmnt" runat="server" AutoPostBack="false" class="form-control" onchange="txtSpecial1()" Style="width: 100px;">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem>No</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 13px;" runat="server">
                                            <asp:Label runat="server" ID="Prior">
                                                <b class="CheckBold">Have any property taxes been delinquent in the past 24 months?:</b>
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="pastDeliquent" runat="server" AutoPostBack="false" class="form-control" onchange="txtpastDeliquent()" Visible="false">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem>No</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </asp:Panel>

                <br />
                <div class="modal fade" id="delinquentstatus" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content" style="height: auto">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Delinquent</h4>
                                <div style="border-top: 1px solid #e5e5e5;"></div>
                            </div>
                            <div class="modal-body">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Are You Sure Want To Delete Delinquent Detials?
                                            </td>
                                            <td>
                                                <div class="col-xs-12">
                                                    <asp:Button ID="btndelidelinquent" runat="server" Text="OK" class="btn btn-success" Style="height: 28px; padding-top: 3px;" OnClick="btndelidelinquent_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal fade" id="exemptionstatus" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content" style="height: auto">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Exemption</h4>
                                <div style="border-top: 1px solid #e5e5e5;"></div>
                            </div>
                            <div class="modal-body">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Are You Sure Want To Delete Exemption Detials?
                                            </td>
                                            <td>
                                                <div class="col-xs-12">
                                                    <asp:Button ID="btnexemption" runat="server" Text="OK" class="btn btn-success" Style="height: 28px; padding-top: 3px;" OnClick="btnexemption_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal fade" id="specialstatus" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content" style="height: auto">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Special Assessment</h4>
                                <div style="border-top: 1px solid #e5e5e5;"></div>
                            </div>
                            <div class="modal-body">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Are You Sure Want To Delete Special Assessment Detials?
                                            </td>
                                            <td>
                                                <div class="col-xs-12">
                                                    <asp:Button ID="btnspecdet" runat="server" Text="OK" class="btn btn-success" Style="height: 28px; padding-top: 3px;" OnClick="btnspecdet_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal fade" id="priordeliqstatus" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content" style="height: auto">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Prior Delinquent</h4>
                                <div style="border-top: 1px solid #e5e5e5;"></div>
                            </div>
                            <div class="modal-body">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Are You Sure Want To Delete Prior Delinquent Detials?
                                            </td>
                                            <td>
                                                <div class="col-xs-12">
                                                    <asp:Button ID="btnprideliqstatus" runat="server" Text="OK" class="btn btn-success" Style="height: 28px; padding-top: 3px;" OnClick="btnprideliqstatus_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <%--Delinquent Status--%>
                <asp:Panel ID="tblDeliquentStatus" TabIndex="46" class="panel panel-default" runat="server" Style="visibility: hidden; display: none; border-color: #280277;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#collapseDeliquent" style="color: #FFFFFF; background-color: #280277; border-color: #280277;">
                        <h4 class="panel-title">
                            <strong style="cursor: pointer; text-decoration: underline;">Delinquent Status</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                        </h4>
                    </div>
                    <div id="collapseDeliquent" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <table style="width: 1295px" border="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <b>
                                                <label id="lbldeliPayee" class="CheckBold">Payee Name:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdeliPayee" runat="server" Style="margin-bottom: 5px;" class="form-control" placeholder="Payee Name" autocomplete='off' onchange="return functionDelinquent()" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);">
                                            </asp:TextBox>
                                        </td>

                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label id="lbldelitAddress" class="CheckBold">Address:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdelitAddress" runat="server" class="form-control" placeholder="Address" autocomplete='off' onchange="return functionDelinquent()" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);">
                                            </asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label id="lbldelitCity" class="CheckBold">City:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdelitCity" runat="server" class="form-control" placeholder="City" autocomplete='off' onchange="return functionDelinquent()" onkeypress="return onlyAlphabets(event,this);" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);">
                                            </asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b class="CheckBold">
                                                <label id="lbldelitState" class="CheckBold">State:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdelitState" runat="server" class="form-control" placeholder="State" autocomplete='off' Style="text-transform: uppercase" MaxLength="2" onchange="return functionDelinquent()" onkeypress="return onlyAlphabets(event,this);" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);" onblur="IsValidLengthState(this.value,this,event);">
                                            </asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <b>
                                                <label class="CheckBold" id="lbldelitzip">Zip:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <%--<asp:TextBox ID="txtdelitzip" runat="server" Style="margin-bottom: 5px;" class="form-control" placeholder="Zip" autocomplete='nope' onchange="return functionDelinquent()">
                                            </asp:TextBox>--%>
                                            <asp:TextBox ID="txtdelitzip" runat="server" class="form-control" placeholder="Zip Code" onkeypress="return isNumberKey(event)" autocomplete='off' MaxLength="5" Style="margin-bottom: 5px;" onchange="return functionDelinquent();" onblur="IsValidLengthZip(this.value,this,event);"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <label class="CheckBold" id="lblbaseamntdue">Base Amount Due:</label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtbaseamntdue" class="form-control" placeholder="Base Amount Due" onkeyup="BasedueAmount();" onfocusout="BaseduePayAmount();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Baseduepay();" autocomplete='off'></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b class="CheckBold">
                                                <label class="CheckBold" id="lbldelitaxyear">Delinquent Tax Year:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdelitaxyear" runat="server" class="form-control" placeholder="YYYY" MaxLength="4" pattern="\d{4}" title="Please enter exactly 4 digits" onkeypress="return isNumberKey(event)" onfocusout="return futureYear(this)" onblur="IsValidLengthTax3(this.value,this,event);" autocomplete='off' onchange="return functionDelinquent();" onpaste="return isNumber(event)">
                                            </asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b class="CheckBold">
                                                <label class="CheckBold" id="lblrolloverdate" style="white-space: nowrap">Roll Over Date:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtrolloverdate" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" autocomplete='off'></asp:TextBox>
                                        </td>
                                        <td hidden>
                                            <p id="BA"></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <label class="CheckBold" id="lblpenlatyamt">% of Penalty Amount:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpenlatyamt" runat="server" class="form-control" Style="margin-bottom: 5px;" placeholder="% Penalty Amount" onkeyup="PerpenAmount();" onfocusout="PerpePayAmount();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Perpenpay();" autocomplete='off'></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label class="CheckBold" id="lblpencalfre" style="white-space: nowrap" title="Penalty Amount Calc Frequency">Penalty Amt Calc Fre:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtpencalfre" runat="server" class="form-control" placeholder="Penalty Amt Calc Fre">
                                                <asp:ListItem>--Select--</asp:ListItem>
                                                <asp:ListItem>Daily</asp:ListItem>
                                                <asp:ListItem>Weekly</asp:ListItem>
                                                <asp:ListItem>Monthly</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label class="CheckBold" id="lbladdpenAmnt" style="white-space: nowrap">Additional Penalty Amount:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtaddpenAmnt" runat="server" class="form-control" placeholder="Additional Penalty Amount" onkeyup="AddpenAmount();" onfocusout="AddpePayAmount();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="Addpenpay();" autocomplete='off'></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label class="CheckBold" id="lblPerdiem">Perdiem:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPerdiem" runat="server" class="form-control" placeholder="Perdiem" autocomplete='off'></asp:TextBox>
                                        </td>
                                        <td hidden>
                                            <p id="PP"></p>
                                        </td>
                                        <td hidden>
                                            <p id="AP"></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <label class="CheckBold" id="lblpenamtdue" style="white-space: nowrap">Penalties Amt Due Date:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpenamtdue" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" Style="margin-bottom: 5px;" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" autocomplete='off'></asp:TextBox>
                                        </td>

                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label class="CheckBold" id="lblpayoffamount">Payoff Amount:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpayoffamount" runat="server" class="form-control" placeholder="Payoff Amount" onkeyup="PayAmount1();" onfocusout="myFunctionPayAmount1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mypay();" autocomplete='off' onchange="return functionDelinquent()"></asp:TextBox>
                                        </td>

                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label class="CheckBold" id="lblpayoffgood">Payoff Good thru Date:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpayoffgood" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="isvalidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete='off' onchange="dateValidate(this);return functionDelinquent();"></asp:TextBox>
                                        </td>

                                        <td style="padding-left: 31px;">
                                            <b title="Initial Installment Due Date">
                                                <label class="CheckBold" id="lblinitialinstall">Inst Due Date:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtinitialinstall" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="isvalidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" autocomplete='off' onblur="return checkDate(this,event)" onchange="return functionDelinquent();" onfocusout="return dateValidateFutue(this);">
                                            </asp:TextBox>
                                        </td>


                                    </tr>

                                    <tr>
                                        <td>
                                            <b title="TaxSale-Not Applicable">
                                                <label class="CheckBold" id="lblnotapplicable">Tax Sale Info: N/A:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtnotapplicable" runat="server" class="form-control" onchange="applicable();return functionDelinquent()" Style="margin-bottom: 5px;">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem>No</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b title="Date of Tax Sale">
                                                <label id="lbldatetaxsale" class="CheckBold">Tax Sale Date:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdatetaxsale" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete='off' onchange="return functionDelinquent()" onfocusout="return dateValidateFutuetaxsale(this)">
                                            </asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label id="lbllastdayred" class="CheckBold">Last Day To Redeem:</label>
                                            </b>

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtlastdayred" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete='off' onchange="return functionDelinquent()">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b class="CheckBold">
                                                <label class="CheckBold" id="lbldelitcomment">Comments:</label>
                                            </b>
                                        </td>
                                        <td colspan="5">
                                            <textarea id="txtdelitcomment" runat="server" rows="2" class="form-control" placeholder="Comments" style="resize: none;" autocomplete='off' onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);"></textarea>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <asp:Button runat="server" ID="DeliquentStatusAdd" Text="Add" class="btn btn-success" Style="height: 30px;" OnClick="btnDeliquentStatusAdd_Click" OnClientClick="return functionDelinquent()" />
                                        </td>
                                        <td hidden>
                                            <p id="o"></p>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <asp:GridView ID="gvDeliquentStatus" runat="server" AutoGenerateColumns="false" Width="90%" GridLines="None"
                            DataKeyNames="Id" OnRowCommand="btnDeliquentStatus_RowCommand" OnRowEditing="gvDeliquentStatus_RowEditing"
                            OnRowUpdating="gvDeliquentStatus_RowUpdating"
                            EmptyDataRowStyle-HorizontalAlign="Center"
                            OnRowCancelingEdit="gvDeliquentStatus_RowCancelingEdit">
                            <Columns>
                                <asp:BoundField ItemStyle-Width="50%" DataField="Id" HeaderText="Tax ID" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="30%" DataField="payee" HeaderText="Payee Name" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="30%" DataField="address" HeaderText="Address" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="20%" DataField="city" HeaderText="City" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="40%" DataField="state" HeaderText="State" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="40%" DataField="zip" HeaderText="Zip Code" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="20%" DataField="deliquenttaxyear" HeaderText="Delinquent Year" ReadOnly="true" ItemStyle-CssClass="tdalign" />
                                <asp:BoundField ItemStyle-Width="20%" DataField="payoffamount" HeaderText="Pay-off Amount" ReadOnly="true" ItemStyle-CssClass="tdalign" />
                                <asp:BoundField ItemStyle-Width="40%" DataField="comments" HeaderText="Comments" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="20%" DataField="goodthuruDate" HeaderText="Good-thru Date" ReadOnly="true" ItemStyle-CssClass="tdalign" />
                                <asp:BoundField ItemStyle-Width="25%" DataField="installmentduedate" HeaderText="Installment Date" ReadOnly="true" ItemStyle-CssClass="tdalign" />
                                <asp:BoundField ItemStyle-Width="40%" DataField="taxsalenotapplicable" HeaderText="TaxSale.NotApp" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="40%" DataField="dateofTaxsale" HeaderText="DtOfTaxsale" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="40%" DataField="lastdaytoredeem" HeaderText="LastDayRedeem" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="HdnSpecialId" runat="server" Value='<%# Bind("Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LnkEdit" runat="server" class="glyphicon glyphicon-edit" autopostback="false" CommandName="Edit" Style="height: 30px; margin-left: 8px; margin-bottom: -10px;" ToolTip="Edit" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LnkUpdate" runat="server" class="glyphicon glyphicon-ok" Style="height: 30px; margin-left: 8px;" autopostback="false" ToolTip="Update" CommandName="Update" OnClientClick="return functionDelinquent()" />
                                        <asp:LinkButton ID="Cancel" runat="server" class="glyphicon glyphicon-remove" autopostback="false" Style="height: 30px; margin-left: 10px;" ToolTip="Cancel" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="35%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_DeleteSpecial" runat="server" Style="margin-left: 12px;" autopostback="false" class="glyphicon glyphicon-trash" OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');" CommandName="DeleteDelinquent" ToolTip="Delete" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <AlternatingRowStyle BackColor="#f3f2ea" />
                            <HeaderStyle BackColor="#f94848" ForeColor="white" />
                            <EmptyDataTemplate>
                                <div id="delirecord" style="color: red; font-weight: bold;" align="center">No records found.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <br />
                    </div>
                </asp:Panel>

                <%--Excemption Status--%>
                <asp:Panel ID="tblExestatus" TabIndex="47" class="panel panel-default" runat="server" Style="visibility: hidden; display: none; border-color: #280277; margin-top: 12px;">
                    <div id="exefocus" class="panel-heading" data-toggle="collapse" data-target="#collapseExemption" style="color: #FFFFFF; background-color: #280277; border-color: #280277;">
                        <h4 class="panel-title">
                            <strong style="cursor: pointer; text-decoration: underline;">Exemption Status</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                        </h4>
                    </div>
                    <div id="collapseExemption" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <table style="width: 1000px" border="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <b>
                                                <label class="CheckBold" id="lblexetype">Exemption Type:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="txtexetype" runat="server" class="form-control" Style="width: auto" onchange="return functionExemption()">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem>Veteran</asp:ListItem>
                                                <asp:ListItem>Disability</asp:ListItem>
                                                <asp:ListItem>Senior</asp:ListItem>
                                                <asp:ListItem>Agricultural</asp:ListItem>
                                                <asp:ListItem>Widowed</asp:ListItem>
                                                <asp:ListItem>Income Based</asp:ListItem>
                                                <asp:ListItem>Homestead Cap</asp:ListItem>
                                                <asp:ListItem>Abatement</asp:ListItem>
                                                <asp:ListItem>Other</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td style="padding-left: 25px;"><b class="CheckBold">Exemption Amount(if Applicable):</b></td>
                                        <td>
                                            <asp:TextBox ID="txtexeamount" runat="server" class="form-control" placeholder="Exemption Amount" onkeyup="ExemptionAmount1();" onfocusout="myFunctionExemptionAmount1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" Style="width: 150px;" onblur="myExe();" autocomplete='off' value="0.00" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="ExemptionAdd" Text="Add" class="btn btn-success" Style="height: 28px; padding-top: 3px;" OnClick="btnExemptionAdd_Click" OnClientClick="return functionExemption();" />
                                        </td>
                                        <td hidden>
                                            <p id="ee"></p>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <br />
                            <asp:GridView ID="gvExemption" runat="server" AutoGenerateColumns="false" Width="70%" GridLines="None"
                                DataKeyNames="Id" OnRowEditing="gvExemption_RowEditing" OnRowUpdating="gvExemption_RowUpdating"
                                OnRowCommand="btngvExemption_RowCommand" EmptyDataRowStyle-HorizontalAlign="Center"
                                OnRowCancelingEdit="gvExemption_RowCancelingEdit">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="50%" DataField="Id" HeaderText="Tax ID" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField ItemStyle-Width="50%" DataField="exemptiontype" HeaderText="Exemption Type" ReadOnly="true" ItemStyle-CssClass="tdalign" />
                                    <asp:BoundField ItemStyle-Width="40%" DataField="exemptionamount" HeaderText="Exemption Amount" ReadOnly="true" ItemStyle-CssClass="tdalign" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="HdnExemptionId" runat="server" Value='<%# Bind("Id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LnkEdit" runat="server" class="glyphicon glyphicon-edit" CommandName="Edit" autopostback="false" Style="height: 30px; margin-left: 5px; margin-bottom: -10px;" ToolTip="Edit" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="LnkUpdate" runat="server" class="glyphicon glyphicon-ok" autopostback="false" Style="height: 30px; margin-left: 5px;" ToolTip="Update" CommandName="Update" OnClientClick="return functionExemption();" />
                                            <asp:LinkButton ID="Cancel" runat="server" class="glyphicon glyphicon-remove" autopostback="false" Style="height: 30px; margin-left: 10px;" ToolTip="Cancel" CommandName="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_DeleteExemption" runat="server" class="glyphicon glyphicon-trash" autopostback="false" OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');" CommandName="DeleteExemption" Style="height: 30px;" ToolTip="Delete" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle BackColor="#f3f2ea" />
                                <HeaderStyle BackColor="#f94848" ForeColor="white" />
                                <EmptyDataTemplate>
                                    <div style="margin-left: 392px; color: red; font-weight: bold;" align="center">No records found.</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>

                <%--Special Assessment Status--%>
                <asp:Panel ID="tblSpecialstatus" TabIndex="48" class="panel panel-default" runat="server" Style="visibility: hidden; display: none; border-color: #280277; margin-top: 12px;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#collapseSpecial" style="color: #FFFFFF; background-color: #280277; border-color: #280277;">
                        <h4 class="panel-title">
                            <strong style="cursor: pointer; text-decoration: underline;">Special Assessment</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                        </h4>
                    </div>
                    <div id="collapseSpecial" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <table style="width: 1300px;">
                                <tbody>
                                    <tr>
                                        <td>
                                            <b>
                                                <label class="CheckBold" id="lbldescription">Description:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdescription" runat="server" class="form-control" Style="margin-bottom: 5px;" placeholder="Description" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);" autocomplete='off' onchange="return functionSpecial()"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold" title="Special Assessment No">AssessNo:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtspecialassno" runat="server" class="form-control" placeholder="Special Assessment No" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);" autocomplete='off'></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold" title="Number Of Installments">No Of Inst:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtnoinstall" runat="server" class="form-control" placeholder="Number Of Installments" onkeypress="return isNumberKey(event)" autocomplete='off' onfocusout="InstallmentRemaining();"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold" title="Installment Paid">Inst Paid:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtinstallpaid" runat="server" class="form-control" placeholder="Installment Paid" onkeypress="return isNumberKey(event)" autocomplete='off' onfocusout="InstallmentRemaining();"></asp:TextBox>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td>
                                            <b>
                                                <label class="CheckBold" id="lblinstremaining">Remaining:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <%-- <asp:TextBox ID="txtInstallRemain" runat="server" class="form-control" placeholder="Inst Remaining" Style="margin-bottom: 5px;" onkeyup="SpeAmount1();" onfocusout="SpemyFunctionAmount1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mySpe();" autocomplete='off'></asp:TextBox>--%>
                                            <asp:TextBox ID="txtInstallRemain" runat="server" class="form-control" placeholder="Inst Remaining" onkeypress="return isNumberKey(event)" autocomplete='off' onchange="return functionSpecial()" Style="margin-bottom: 5px;"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold">Due Date:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtduedate" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete='off'></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold">Amount:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtamountspecial" runat="server" class="form-control" placeholder="Amount" onkeyup="RemianAmount1();" onfocusout="SpemyFunctionRemian1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="myRemian();" autocomplete='off' value="0.00"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold">Remaining Balance:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtsperembal" runat="server" class="form-control" onkeyup="RemianBalAmount1();" placeholder="Remaining Balance" onfocusout="SpemyFunctionRemianBal1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="myRemianBal();" autocomplete='off' value="0.00"></asp:TextBox>
                                        </td>
                                        <td hidden>
                                            <p id="s"></p>
                                        </td>
                                        <td hidden>
                                            <p id="sr"></p>
                                        </td>
                                        <td hidden>
                                            <p id="rr"></p>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td><b class="CheckBold">Date:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtspecdate" runat="server" class="form-control" placeholder="MM/DD/YYYY" Style="margin-bottom: 5px;" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete='off'></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold">Per Diem:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtspecperdiem" runat="server" class="form-control" placeholder="Per Diem" onkeypress="return isNumberKey(event)" onkeydown="return CheckFirstChar(event.keyCode, this);" autocomplete='off' onpaste="return isNumber(event)"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;"><b class="CheckBold">Payee:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtspecpayee" runat="server" placeholder="Payee" class="form-control" autocomplete='off' onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td><b class="CheckBold">Comments:</b></td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtspeccomments" runat="server" class="form-control" placeholder="Comments" autocomplete='off' onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <asp:Button ID="SpecialAdd" runat="server" Text="Add" class="btn btn-success" Style="height: 32px; padding-top: 3px;" OnClick="btnSpecialAdd_Click" OnClientClick="return functionSpecial()" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <asp:GridView ID="gvSpecial" runat="server" AutoGenerateColumns="false" Width="90%" GridLines="None"
                            DataKeyNames="Id" OnRowCommand="btnSpecialAssessment_RowCommand" OnRowEditing="gvSpecialAssessment_RowEditing"
                            OnRowUpdating="gvSpecialAssessment_RowUpdating"
                            EmptyDataText="No Data Found" EmptyDataRowStyle-HorizontalAlign="Center"
                            OnRowCancelingEdit="gvSpecialAssessment_RowCancelingEdit">
                            <Columns>

                                <asp:BoundField ItemStyle-Width="50%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="Id" HeaderText="Tax ID" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="30%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="description" HeaderText="Description" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="30%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="InstallmentsRemaining" HeaderText="Installment Remaining" ReadOnly="true" ItemStyle-CssClass="tdalign" />
                                <asp:BoundField ItemStyle-Width="30%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="specialassessmentno" HeaderText="Special Assessment No" ReadOnly="true" ItemStyle-CssClass="tdalign" />
                                <asp:BoundField ItemStyle-Width="20%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="noofinstallment" HeaderText="No.Of.Installments" ReadOnly="true" ItemStyle-CssClass="tdalign" />
                                <asp:BoundField ItemStyle-Width="40%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="installmentpaid" HeaderText="Installments Paid" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField ItemStyle-Width="40%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="amount" HeaderText="Amount" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="HdnSpecialId" runat="server" Value='<%# Bind("Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LnkEdit" runat="server" class="glyphicon glyphicon-edit" autopostback="false" CommandName="Edit" Style="height: 30px; margin-left: 8px; margin-bottom: -10px;" ToolTip="Edit" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LnkUpdate" runat="server" class="glyphicon glyphicon-ok" autopostback="false" Style="height: 30px; margin-left: 8px;" ToolTip="Update" CommandName="Update" OnClientClick="return functionSpecial()" />
                                        <asp:LinkButton ID="Cancel" runat="server" class="glyphicon glyphicon-remove" autopostback="false" Style="height: 30px; margin-left: 10px;" ToolTip="Cancel" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="35%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_DeleteSpecial" runat="server" Style="margin-left: 12px;" autopostback="false" class="glyphicon glyphicon-trash" OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');" CommandName="DeleteSpecial" ToolTip="Delete" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <AlternatingRowStyle BackColor="#f3f2ea" />
                            <HeaderStyle BackColor="#f94848" ForeColor="white" />
                            <EmptyDataTemplate>
                                <div style="color: red; font-weight: bold;" align="center">No records found.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <br />
                    </div>
                </asp:Panel>

                <asp:Panel ID="tblPastDeliquent" TabIndex="49" class="panel panel-default" runat="server" Style="visibility: hidden; display: none; border-color: #280277; margin-top: 12px;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#collapsePast" style="color: #FFFFFF; background-color: #280277; border-color: #280277;">
                        <h4 class="panel-title">
                            <strong style="cursor: pointer; text-decoration: underline;">Prior Delinquent Tax</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                        </h4>
                    </div>
                    <div id="collapsePast" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <table style="width: 1300px;">
                                <tbody>
                                    <tr style="height: 50px;">
                                        <td>
                                            <b>
                                                <label class="CheckBold" id="lblpriodeli">Delinquent Tax Year:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpriodeli" runat="server" class="form-control" placeholder="YYYY" MaxLength="4" Style="margin-bottom: 5px;" onkeypress="return isNumberKey(event)" onblur="IsValidLengthTaxPrior(this.value,this,event);" onchange="return functionPrior()" autocomplete='off' pattern="\d{4}" title="Please enter exactly 4 digits" onpaste="return isNumber(event)"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label class="CheckBold" id="lblpriorigamtdue">Original Amount Due:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpriorigamtdue" placeholder="Original Amount Due" runat="server" class="form-control" onkeyup="OriginalAmountDue();" onfocusout="OriginFunctionDue();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="OriDue();" onchange="return functionPrior()" autocomplete='off'></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b>
                                                <label class="CheckBold" id="lblprideliqdate">Originally Delinquency Date:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtprideliqdate" runat="server" placeholder="MM/DD/YYYY" class="form-control" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" autocomplete='off' onchange="return functionPrior()"></asp:TextBox>
                                        </td>
                                        <td hidden>
                                            <p id="OD"></p>
                                        </td>
                                        <td hidden>
                                            <p id="OA"></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <label class="CheckBold" id="lblpriamtpaid">Amount Paid:</label>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpriamtpaid" runat="server" placeholder="Amount Paid" class="form-control" onkeyup="Priamountpaid();" onfocusout="PriFunctionpaid();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocusin="if (this.value=='0.00') this.value='';" onblur="mypriamtpaid();" autocomplete='off' onchange="return functionPrior()"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <b class="CheckBold">Delinquency Comments:</b>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtprideliqcommts" runat="server" placeholder="Delinquency Comments" Rows="2" cols="20" class="form-control" Style="resize: none" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);"></asp:TextBox>
                                        </td>
                                        <td style="padding-left: 31px;">
                                            <asp:Button ID="btnpriordelinquenttax" runat="server" Text="Add" class="btn btn-success" Style="height: 32px; padding-top: 3px;" OnClick="btnpriordelinquenttax_Click" OnClientClick="return functionPrior()" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <asp:GridView ID="GrdPriordelinquent" runat="server" AutoGenerateColumns="false" Width="90%" GridLines="None"
                        DataKeyNames="Id" OnRowCommand="GrdPriordelinquent_RowCommand" OnRowEditing="GrdPriordelinquent_RowEditing"
                        OnRowUpdating="GrdPriordelinquent_RowUpdating"
                        EmptyDataText="No Data Found" EmptyDataRowStyle-HorizontalAlign="Center"
                        OnRowCancelingEdit="GrdPriordelinquent_RowCancelingEdit">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="Id" HeaderText="Tax ID" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                            <asp:BoundField ItemStyle-Width="30%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="delinquenttaxyear" HeaderText="delinquenttaxyear" ReadOnly="true" />
                            <asp:BoundField ItemStyle-Width="30%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="originalamountdue" HeaderText="originalamountdue" ReadOnly="true" />
                            <asp:BoundField ItemStyle-Width="20%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="originaldelinquencydate" HeaderText="originaldelinquencydate" ReadOnly="true" />
                            <asp:BoundField ItemStyle-Width="40%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="amountpaid" HeaderText="amountpaid" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                            <asp:BoundField ItemStyle-Width="40%" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="false" DataField="delinquencycomments" HeaderText="delinquencycomments" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="HdnSpecialId" runat="server" Value='<%# Bind("Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LnkEdit" runat="server" class="glyphicon glyphicon-edit" autopostback="false" CommandName="Edit" Style="height: 30px; margin-left: 8px; margin-bottom: -10px;" ToolTip="Edit" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LnkUpdate" runat="server" class="glyphicon glyphicon-ok" autopostback="false" Style="height: 30px; margin-left: 8px;" ToolTip="Update" CommandName="Update" OnClientClick="return functionPrior()" />
                                    <asp:LinkButton ID="Cancel" runat="server" class="glyphicon glyphicon-remove" autopostback="false" Style="height: 30px; margin-left: 10px;" ToolTip="Cancel" CommandName="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" ItemStyle-Width="35%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btn_DeleteSpecial" runat="server" Style="margin-left: 12px;" autopostback="false" class="glyphicon glyphicon-trash" OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');" CommandName="DeleteSpecial" ToolTip="Delete" CommandArgument='<%# DataBinder.Eval(Container,"DataItemIndex") %>' CssClass=""></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BackColor="#f3f2ea" />
                        <HeaderStyle BackColor="#f94848" ForeColor="white" />
                        <EmptyDataTemplate>
                            <div style="color: red; font-weight: bold;" align="center">No records found.</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>

                <%--Tax Cert Info Modal--%>
                <div class="modal fade" id="AddTaxStatus" role="dialog">
                    <div class="modal-dialog" style="width: 765px;">
                        <div class="modal-content" style="height: 200px;">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Order Status</h4>
                                <div style="border-top: 1px solid #e5e5e5;"></div>
                            </div>
                            <div class="modal-body">
                                <table style="width: 700px;">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <b style="white-space: nowrap;" class="CheckBold">Order Status: </b>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlordstatus" runat="server" class="form-control" Style="width: 180px;">
                                                    <asp:ListItem>--Select Status--</asp:ListItem>
                                                    <asp:ListItem>ORDERED</asp:ListItem>
                                                    <asp:ListItem>ATTEMPTED</asp:ListItem>
                                                    <asp:ListItem>REORDERED</asp:ListItem>
                                                    <asp:ListItem>PROBLEM</asp:ListItem>
                                                    <asp:ListItem>DECLINED</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <b style="white-space: nowrap;" class="CheckBold">Comments: </b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtComments" runat="server" class="form-control" Style="width: 300px;" autocomplete="off" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="4">
                                                <div class="col-xs-12" style="left: 250px; padding-top: 10px">
                                                    <asp:Label ID="lbltaxcerterror" runat="server" Class="CheckBold" Style="color: red; white-space: nowrap" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <div class="col-xs-12" style="left: 300px">

                                                    <asp:Button ID="btnModalSave" runat="server" Text="Save" class="btn btn-success" Style="height: 28px; padding-top: 3px;" OnClick="MdlOrderStatus_Click" OnClientClick="return userValid();" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal fade" role="dialog" id="AddNotes">
                    <div class="modal-dialog">
                        <div class="modal-content" style="height: 280px;">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Add Note</h4>
                                <div style="border-top: 1px solid #e5e5e5;"></div>
                            </div>
                            <div class="modal-body" style="padding-top: 0px;">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <label class="CheckBold">Note Text:</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <textarea id="txtnotes" runat="server" rows="5" style="resize: none; width: 550px;" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);"></textarea>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div>
                                    <asp:Label runat="server" Class="CheckBold" Style="color: red; white-space: nowrap" ID="lbladdnoteserror" />
                                </div>
                                <br />
                                <div>
                                    <div class="col-xs-12">
                                        <asp:Button ID="btnaddnotes" runat="server" Text="Submit" class="btn btn-success" Style="height: 28px; padding-top: 3px;" OnClick="btnaddnotes_Click" OnClientClick="return functionaddnoteerror();" />
                                        <button type="button" class="btn btn-info" data-dismiss="modal" style="height: 28px; padding-top: 3px;">Cancel</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:Panel ID="PanelAuthorityInfo" class="panel panel-default" runat="server" Visible="false">
                    <div class="modal fade" id="ModalAuthorityStatus" role="dialog" runat="server">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content" style="width: 100%; height: 400px; margin-left: 0px;">
                                <div class="modal-header">
                                    <button type="button" id="test" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Tax Authority</h4>
                                    <div style="border-top: 1px solid #e5e5e5;"></div>
                                </div>
                                <div class="modal-body">

                                    <div class="panel-body">
                                        <div class="tab-content">
                                            <div class="tab-pane fade in active" id="tab1primary151">
                                                <div class="col-md-12">
                                                    <div class="col-md-6">
                                                        <div class="col-md-12">
                                                            <asp:GridView ID="gvtaxauthorities" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                                Width="225%" GridLines="None" Style="white-space: nowrap;">
                                                                <Columns>
                                                                    <asp:BoundField ItemStyle-Width="30%" DataField="AgencyId" HeaderText="Agency Id" />
                                                                    <asp:BoundField ItemStyle-Width="30%" DataField="TaxAuthorityName" HeaderText="Tax Authority Name" />
                                                                    <asp:BoundField ItemStyle-Width="30%" DataField="TaxAgencyType" HeaderText="Tax Type" />
                                                                    <asp:BoundField ItemStyle-Width="30%" DataField="TaxAgencyState" HeaderText="State" />
                                                                    <asp:BoundField ItemStyle-Width="30%" DataField="Phone" HeaderText="Phone" />
                                                                    <asp:BoundField ItemStyle-Width="10%" DataField="TaxYearStartDate" HeaderText="Tax Year Start Date" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:TemplateField HeaderText="Select Authority">
                                                                        <EditItemTemplate>
                                                                            <asp:CheckBox ID="chkauthority" runat="server" Style="margin-left: 55px;" />
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkauthority" runat="server" Style="margin-left: 55px;" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle BackColor="#f3f2ea" />
                                                                <HeaderStyle BackColor="#f94848" ForeColor="white" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <br />

                                                    <div class="col-md-8">
                                                        <asp:Button ID="btnaddauthority" runat="server" Text="Add" OnClick="btnaddauthority_Click" class="btn btn-success" Style="height: 28px; padding-top: 3px; margin-right: -295px; margin-top: 18px;" OnClientClick="return validateCheckBoxes()" />
                                                    </div>
                                                </div>
                                                <div>
                                                    <asp:Label runat="server" ID="lblagency" CssClass="CheckBold" ForeColor="Red"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="PanelTaxtype" class="panel panel-default" runat="server" Visible="false">
                    <div class="modal fade" id="Modaltaxtype" role="dialog">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content" style="width: 490px;">
                                <div class="modal-header">
                                    <button type="button" id="test1" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Tax Type</h4>
                                    <div style="border-top: 1px solid #e5e5e5;"></div>
                                </div>
                                <div class="modal-body">
                                    <div class="panel-body">
                                        <div class="tab-content">
                                            <div class="tab-pane fade in active" id="tabtaxtype">
                                                <asp:DropDownList ID="drotxttype" runat="server" CssClass="form-control" Style="width: 300px;">
                                                    <asp:ListItem>County</asp:ListItem>
                                                    <asp:ListItem>City</asp:ListItem>
                                                    <asp:ListItem>Town</asp:ListItem>
                                                    <asp:ListItem>Township/County</asp:ListItem>
                                                    <asp:ListItem>Incorp Village</asp:ListItem>
                                                    <asp:ListItem>CityAndSchool</asp:ListItem>
                                                    <asp:ListItem>School District</asp:ListItem>
                                                    <asp:ListItem>JuniorColleges</asp:ListItem>
                                                    <asp:ListItem>Irrigation District</asp:ListItem>
                                                    <asp:ListItem>Utility District</asp:ListItem>
                                                    <asp:ListItem>CapitalImprovementDistrict</asp:ListItem>
                                                    <asp:ListItem>Waste Fee District</asp:ListItem>
                                                    <asp:ListItem>Water/Sewer Rental</asp:ListItem>
                                                    <asp:ListItem>Subdivision Maint. Escrowed</asp:ListItem>
                                                    <asp:ListItem>Subdivision Maint. Non-Escrowed</asp:ListItem>
                                                    <asp:ListItem>BondAuthority</asp:ListItem>
                                                    <asp:ListItem>Misc. Charges District</asp:ListItem>
                                                    <asp:ListItem>Borough</asp:ListItem>
                                                    <asp:ListItem>AssessmentDistrict</asp:ListItem>
                                                    <asp:ListItem>CentralAppraisalTaxingAuthority</asp:ListItem>
                                                    <asp:ListItem>CentralCollectionTaxingAuthority</asp:ListItem>
                                                    <asp:ListItem>Unsecured County Taxes</asp:ListItem>
                                                    <asp:ListItem>MobileHomeAuthority</asp:ListItem>
                                                    <asp:ListItem>CountyCollectedByOtherTaxingAuthority</asp:ListItem>
                                                    <asp:ListItem>OtherTaxes</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div>
                                                <asp:Button ID="btntaxtypeupdate" OnClick="btntaxtypeupdate_Click" runat="server" Text="Update" class="btn btn-success" Style="height: 28px; padding-top: 3px; margin-top: 25px;" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <%--Logout Reason Modal--%>
                <div class="modal fade" id="ModallogoutReason" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Logout Reason</h4>
                                <div style="border-top: 1px solid #e5e5e5;"></div>
                            </div>
                            <div class="modal-body">
                                <div class="panel-body">
                                    <div class="tab-content">
                                        <div class="tab-pane fade in active" id="tab1primary153">
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtreason" Style="resize: none; width: 480px;" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <br />
                                                <br />
                                                <br />
                                                <div>
                                                    <asp:Label runat="server" Class="CheckBold" Style="color: red; white-space: nowrap" ID="lbllogouterror" />
                                                </div>
                                                <div class="col-md-12" style="margin-top: 5px;">
                                                    <asp:Button ID="logoutreason" runat="server" Text="Logout" class="btn btn-success" OnClick="logoutreason_Click" OnClientClick="return functionlogouterror();" />
                                                    <button type="button" class="btn btn-default btn-ok" data-dismiss="modal">Cancel</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Add Notes--%>
                <div class="panel panel-default" tabindex="50" style="border-color: #280277; margin-top: 12px;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#collapsenotes" style="color: #FFFFFF; background-color: #280277; border-color: #280277;">
                        <h4 class="panel-title">
                            <strong style="cursor: pointer; text-decoration: underline;">Notes</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                        </h4>
                    </div>
                    <br />
                    <div id="collapsenotes" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <i class="glyphicon glyphicon-file" style="color: #337ab7"></i>
                                            <a data-toggle="modal" href="#AddNotes" style="font-weight: 800;">Add Notes</a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div id="Addnotesdiv" runat="server" style="height: auto; width: 100%; overflow: auto; margin-top: -15px;">
                                <asp:Panel ID="Panel2" runat="server">
                                    <table class="table table-striped table-hover">
                                        <thead style="background-color: #f94848; color: #fff;">
                                            <tr>
                                                <asp:GridView ID="GvAddNotes" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                    Width="98%" GridLines="None">
                                                    <Columns>
                                                        <asp:BoundField ItemStyle-Width="35%" DataField="note" HeaderText="Note" />
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="note_type" HeaderText="Note Type" />
                                                        <asp:BoundField ItemStyle-Width="25%" DataField="added" HeaderText="Added" />
                                                        <asp:BoundField ItemStyle-Width="25%" DataField="enterby" HeaderText="Entered By" />
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#f3f2ea" />
                                                    <HeaderStyle BackColor="#f94848" ForeColor="white" />
                                                </asp:GridView>
                                            </tr>
                                        </thead>
                                    </table>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <div class="panel panel-default" tabindex="51" style="border-color: #280277; margin-top: 12px;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#collapse2" style="color: #FFFFFF; background-color: #280277; border-color: #280277;">
                        <h4 class="panel-title">
                            <strong style="cursor: pointer; text-decoration: underline;">Tax Cert Info</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                        </h4>
                    </div>
                    <div id="collapse2" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <table style="width: 1083px;">
                                <tbody>
                                    <tr>
                                        <td class="colorbold">
                                            <label for="txtdate1"><b class="CheckBold">Expected Date:</b></label>
                                        </td>
                                        <td style="width: 215px">
                                            <input type="text" id="date1" name="txtdate1" runat="server" class="form-control" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" style="height: 35px; width: 160px; background-color: white;" autocomplete="off" />
                                        </td>
                                        <td class="colorbold">
                                            <label for="txtdate2"><b class="CheckBold">Followup Date:</b></label>
                                        </td>
                                        <td style="width: 190px">
                                            <input runat="server" id="date2" name="txtdate2" class="form-control" placeholder="MM/DD/YYYY" maxlength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(this,event)" style="height: 35px; width: 160px; background-color: white;" autocomplete="off" />
                                        </td>
                                        <td colspan="4">
                                            <button type="button" id="btneditdates" runat="server" class="btn btn-success" onclick="editfunction()">Edit</button>
                                            <button type="button" id="btnTaxOrderStatus" runat="server" class="btn btn-success" data-toggle="modal" data-target="#AddTaxStatus">Add Tax Status</button>
                                            <button type="button" id="btnsavedates" runat="server" class="btn btn-success" style="visibility: hidden; display: none" onserverclick="btnEditDatesSave_Click">Save</button>
                                            <button type="button" id="btncanceldates" runat="server" style="visibility: hidden; display: none" class="btn btn-success" onclick="Cancelfunction()">Cancel</button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>

                            <div id="TaxStatus" runat="server" style="height: auto; width: 100%; overflow: auto; margin-top: -15px;">
                                <asp:Panel ID="PnlTaxStatus" runat="server">
                                    <table class="table table-striped table-hover">
                                        <thead style="background-color: #f94848; color: #fff;">
                                            <tr>
                                                <asp:GridView ID="GvTaxStatus" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                    Width="100%" GridLines="None">
                                                    <Columns>
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="orderstatus" HeaderText="Order Status" />
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="createddate" HeaderText="Created Date" />
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="comments" HeaderText="Comments" />
                                                        <asp:BoundField ItemStyle-Width="30%" DataField="enteredby" HeaderText="Entered By" />
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#f3f2ea" />
                                                    <HeaderStyle BackColor="#f94848" ForeColor="white" />
                                                </asp:GridView>
                                            </tr>
                                        </thead>
                                    </table>
                                </asp:Panel>
                            </div>


                            <div class="panel panel-default" style="border-color: #280277; margin-top: 12px;">
                                <div class="panel-heading" data-toggle="collapse" data-target="#collapsecomplete" style="color: #FFFFFF; background-color: #280277; border-color: #280277;">
                                    <h4 class="panel-title">
                                        <strong style="cursor: pointer; text-decoration: underline;">Complete</strong><i class="indicator glyphicon glyphicon-chevron-down pull-left"></i>
                                    </h4>
                                </div>
                                <div id="collapsecomplete" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <table style="width: 780px;">
                                            <tbody>
                                                <tr>
                                                    <td class="colorbold" style="width: 135px;">
                                                        <b class="CheckBold">Order Comments:</b>
                                                    </td>
                                                    <td style="width: 330px">
                                                        <textarea runat="server" rows="2" id="txttotalcomments" class="form-control" style="resize: none" tabindex="51" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);"></textarea>
                                                    </td>
                                                    <td class="colorbold" style="width: 75px;">
                                                        <b class="CheckBold" style="margin-left: 10px;">Status:</b>
                                                    </td>
                                                    <td style="width: 190px">
                                                        <select class="form-control" id="ddlstatus" runat="server" style="width: 180px;" tabindex="52">
                                                            <option>--Select--</option>
                                                            <option>Completed</option>
                                                            <option>In Process</option>
                                                            <option>Mail Away</option>
                                                            <option>On Hold</option>
                                                            <option>Others</option>
                                                            <option>ParcelID</option>
                                                            <option>Rejected</option>
                                                        </select>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:Button ID="btnsaverecordnew" class="btn btn-success" runat="server" Text="Complete" OnClick="btnsaverecordnew_Click" OnClientClick="return completeorder();" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </form>

        <footer id="footer">
            <div class="innertube" style="margin-bottom: 0px">
                <p style="margin-bottom: 0px; font-family: Roboto,-apple-system,BlinkMacSystemFont,Segoe UI,Oxygen,Ubuntu,Cantarell,Fira San,Droid Sans,Helvetica Neue,sans-serif;">
                    2019. All rights reserved | Designed & Developed by<a href="http://stringinfo.com" target="_blank" style="color: red; clear: both;"> String Information Services</a>
                </p>
            </div>
        </footer>
</body>
</html>

