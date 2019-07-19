<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductionNew.aspx.cs" Inherits="Pages_ProductionNew"
    Title="PRODUCTION" Theme="Black" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap-theme.min.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

    <link rel="stylesheet" href="Bootstrapp.css" />
    <link rel="shortcut icon" type="image/ico" href="../App_themes/Black/images/Firefox(1).ico" />
    <script language="javascript" type="text/javascript" src="../Script/datetimepicker.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <link rel="stylesheet" href="../designcss.css" />

    <script type="text/javascript">
        window.onload = function () {
            //document.getElementById('txtstrRemaingBlnce1').disabled = false;
            //document.getElementById('txtstrRemaingBlnce2').disabled = false;
            //document.getElementById('txtstrRemaingBlnce3').disabled = false;
            //document.getElementById('txtstrRemaingBlnce4').disabled = false;
        }
    </script>

    <script type="text/javascript">
        window.pressed = function () {
            var a = document.getElementById('FileUpload1');
            if (a.value == "") {
                LblUpload.innerHTML = "Choose file";
            }
            else {
                LblUpload.innerHTML = "";
            }
        };
    </script>


    <script type="text/javascript">

        function checkReqFields1(year, element, ev) {
            var startyear = document.getElementById("txtstrTaxYear").value;
            var errors = {
                txtstrTaxYear: '',
                txtstrEndYear: '',
            };

            if (year != "") {
                if (year < startyear) {
                    alert("End Year should be greater than Start year");
                    document.getElementById(element.id).value = '';
                    return;
                }
            }
        }
        function checkReqFields(year, element, ev) {
            var startyear = document.getElementById("txtstrEndYear").value;
            var errors = {
                txtstrTaxYear: '',
                txtstrEndYear: '',
            };

            if (startyear != "") {
                if (year > startyear) {
                    alert("End Year should be greater than Start year");
                    document.getElementById(element.id).value = '';
                    document.getElementById("txtstrEndYear").value = '';
                    return;
                }
            }
        }

    </script>

    <script type="text/javascript">
        function txtVisible() {
            document.getElementById("tblspecialassess123").style.visibility = "visible";
            document.getElementById("tblspecialassess123").style.display = "block";
            document.getElementById("tblspecialassessment").style.visibility = "visible";
            document.getElementById("tblspecialassessment").style.display = "block";
        }
        function txtVisible1() {
            document.getElementById("tbldelistatus123").style.visibility = "visible";
            document.getElementById("tbldelistatus123").style.display = "block";
            document.getElementById("tbldeliquentstatus").style.visibility = "visible";
            document.getElementById("tbldeliquentstatus").style.display = "block";
        }
        //Remaining Balance
        function myFunctionRemBalance1() {
            document.getElementById("txtstrRemaingBlnce1").value = document.getElementById("i").innerText;
        }
        function myFunctionRemBalance2() {
            document.getElementById("txtstrRemaingBlnce2").value = document.getElementById("j").innerText;
        }
        function myFunctionRemBalance3() {
            document.getElementById("txtstrRemaingBlnce3").value = document.getElementById("k").innerText;
        }
        function myFunctionRemBalance4() {
            document.getElementById("txtstrRemaingBlnce4").value = document.getElementById("l").innerText;
        }
        function formatMoneyRemBalance1(n, c, txtstrRemaingBlnce1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrRemaingBlnce1 = txtstrRemaingBlnce1 == undefined ? "." : txtstrRemaingBlnce1,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrRemaingBlnce1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyRemBalance2(n, c, txtstrRemaingBlnce2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrRemaingBlnce2 = txtstrRemaingBlnce2 == undefined ? "." : txtstrRemaingBlnce2,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrRemaingBlnce2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyRemBalance3(n, c, txtstrRemaingBlnce3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrRemaingBlnce3 = txtstrRemaingBlnce3 == undefined ? "." : txtstrRemaingBlnce3,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrRemaingBlnce3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyRemBalance4(n, c, txtstrRemaingBlnce4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrRemaingBlnce4 = txtstrRemaingBlnce4 == undefined ? "." : txtstrRemaingBlnce4,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrRemaingBlnce4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function RemBalance1() {
            document.getElementById("i").innerText = formatMoneyRemBalance1(document.getElementById("txtstrRemaingBlnce1").value);
        }
        function RemBalance2() {
            document.getElementById("j").innerText = formatMoneyRemBalance2(document.getElementById("txtstrRemaingBlnce2").value);
        }
        function RemBalance3() {
            document.getElementById("k").innerText = formatMoneyRemBalance3(document.getElementById("txtstrRemaingBlnce3").value);
        }
        function RemBalance4() {
            document.getElementById("l").innerText = formatMoneyRemBalance4(document.getElementById("txtstrRemaingBlnce4").value);
        }
        function IsValidLengthTax3(year, element, ev) {
            var id1 = year.length;
            if (id1 != 0) {
                if (id1 < 4) {
                    alert("Deliquent Status Year should be 4 Numeric Characters");
                    document.getElementById("txtdelitaxstatus").value = '';
                    return;
                }
            }
        }

        function TaxType() {
            alert("Please Choose Tax Type");
        }

        function dialog123(TaxType) {
            $("#myModal").modal();
        }

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
    </script>
    <script type="text/javascript">
       
    </script>
    <script type="text/javascript">
        function mytxtamount1() {
            hello1();
            myFunction1();
            document.getElementById('txtstrRemaingBlnce1').disabled = false;
            //document.getElementById('txtstrRemaingBlnce1').removeAttribute('readonly');
            var instAmt1 = document.getElementById("txtstrTaxAmount1").value;
            var instPaid1 = document.getElementById("txtstrAmountPaid1").value;
            var paidDue1 = document.getElementById("txtstrTaxStatus1");
            Money1 = parseFloat(instAmt1.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid1.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);

            document.getElementById("hdntxtbxTaksit1").value = formatMoney1(result);
            var myHidden = document.getElementById("hdntxtbxTaksit1").value;
            document.getElementById("txtstrRemaingBlnce1").value = myHidden;

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
                document.getElementById("txtstrRemaingBlnce1").readOnly = false;
            }
            else {
                document.getElementById("txtstrRemaingBlnce1").readOnly = false;
            }
        }
        function mytxtamount2() {
            hello2();
            myFunction2();
            document.getElementById('txtstrRemaingBlnce2').disabled = false;
            var instAmt2 = document.getElementById("txtstrTaxAmount2").value;
            var instPaid2 = document.getElementById("txtstrAmountPaid2").value;
            var paidDue2 = document.getElementById("txtstrTaxStatus2");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit2").value = formatMoney1(result);
            var myHidden2 = document.getElementById("hdntxtbxTaksit2").value;
            document.getElementById("txtstrRemaingBlnce2").value = myHidden2;


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
                document.getElementById("txtstrRemaingBlnce2").readOnly = false;
            }
            else {
                document.getElementById("txtstrRemaingBlnce2").readOnly = false;
            }

        }
        function mytxtamount3() {
            hello3();
            myFunction3();
            document.getElementById('txtstrRemaingBlnce3').disabled = false;
            var instAmt2 = document.getElementById("txtstrTaxAmount3").value;
            var instPaid2 = document.getElementById("txtstrAmountPaid3").value;
            var paidDue2 = document.getElementById("txtstrTaxStatus3");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit3").value = formatMoney1(result);
            var myHidden3 = document.getElementById("hdntxtbxTaksit3").value;
            document.getElementById("txtstrRemaingBlnce3").value = myHidden3;


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
            var firstChar3 = myHidden3.substr(0, 1);
            if (firstChar3 == '-') {
                document.getElementById("txtstrRemaingBlnce3").readOnly = false;
            }
            else {
                document.getElementById("txtstrRemaingBlnce3").readOnly = false;
            }

        }
        function mytxtamount4() {
            hello4();
            myFunction4();
            document.getElementById('txtstrRemaingBlnce4').disabled = false;
            var instAmt2 = document.getElementById("txtstrTaxAmount4").value;
            var instPaid2 = document.getElementById("txtstrAmountPaid4").value;
            var paidDue2 = document.getElementById("txtstrTaxStatus4");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit4").value = formatMoney1(result);
            var myHidden4 = document.getElementById("hdntxtbxTaksit4").value;
            document.getElementById("txtstrRemaingBlnce4").value = myHidden4;

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
            var firstChar4 = myHidden4.substr(0, 1);
            if (firstChar4 == '-') {
                document.getElementById("txtstrRemaingBlnce4").readOnly = false;
            }
            else {
                document.getElementById("txtstrRemaingBlnce4").readOnly = false;
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
        function mypay() {
            PayAmount1();
            myFunctionPayAmount1();
        }
        function myamountspecial() {
            Amount1();
            myFunctionAmount1();
        }

        function mytest1() {
            AmtPaid1();
            myFunctionAmtPaid1();
            document.getElementById('txtstrRemaingBlnce1').disabled = false;
            var instAmt1 = document.getElementById("txtstrTaxAmount1").value;
            var instPaid1 = document.getElementById("txtstrAmountPaid1").value;
            var paidDue1 = document.getElementById("txtstrTaxStatus1");
            Money1 = parseFloat(instAmt1.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid1.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit1").value = formatMoney1(result);
            var myHidden = document.getElementById("hdntxtbxTaksit1").value;
            document.getElementById("txtstrRemaingBlnce1").value = myHidden;

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
                document.getElementById("txtstrRemaingBlnce1").readOnly = false;
            }
            else {
                document.getElementById("txtstrRemaingBlnce1").readOnly = false;
            }

        }

        function mytest2() {
            AmtPaid2();
            myFunctionAmtPaid2();
            document.getElementById('txtstrRemaingBlnce2').disabled = false;
            var instAmt2 = document.getElementById("txtstrTaxAmount2").value;
            var instPaid2 = document.getElementById("txtstrAmountPaid2").value;
            var paidDue2 = document.getElementById("txtstrTaxStatus2");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit2").value = formatMoney1(result);
            var myHidden2 = document.getElementById("hdntxtbxTaksit2").value;
            document.getElementById("txtstrRemaingBlnce2").value = myHidden2;

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
                document.getElementById("txtstrRemaingBlnce2").readOnly = false;
            }
            else {
                document.getElementById("txtstrRemaingBlnce2").readOnly = false;
            }
        }

        function mytest3() {
            AmtPaid3();
            myFunctionAmtPaid3();
            document.getElementById('txtstrRemaingBlnce3').disabled = false;
            var instAmt2 = document.getElementById("txtstrTaxAmount3").value;
            var instPaid2 = document.getElementById("txtstrAmountPaid3").value;
            var paidDue2 = document.getElementById("txtstrTaxStatus3");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit3").value = formatMoney1(result);
            var myHidden3 = document.getElementById("hdntxtbxTaksit3").value;
            document.getElementById("txtstrRemaingBlnce3").value = myHidden3;
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
            var firstChar3 = myHidden3.substr(0, 1);
            if (firstChar3 == '-') {
                document.getElementById("txtstrRemaingBlnce3").readOnly = false;
            }
            else {
                document.getElementById("txtstrRemaingBlnce3").readOnly = false;
            }

        }

        function mytest4() {
            AmtPaid4();
            myFunctionAmtPaid4();
            document.getElementById('txtstrRemaingBlnce4').disabled = false;
            var instAmt2 = document.getElementById("txtstrTaxAmount4").value;
            var instPaid2 = document.getElementById("txtstrAmountPaid4").value;
            var paidDue2 = document.getElementById("txtstrTaxStatus4");
            Money1 = parseFloat(instAmt2.replace(/[^0-9\.]+/g, ""));
            Money2 = parseFloat(instPaid2.replace(/[^0-9\.]+/g, ""));
            result = (Money1 - Money2).toFixed(2);
            document.getElementById("hdntxtbxTaksit4").value = formatMoney1(result);
            var myHidden4 = document.getElementById("hdntxtbxTaksit4").value;
            document.getElementById("txtstrRemaingBlnce4").value = myHidden4;

            instAmt2 = instAmt2.replace(',', '');
            instPaid2 = instPaid2.replace(',', '');
            if (parseFloat(instAmt2) == parseFloat(instPaid2)) {
                paidDue2.value = "Paid";
            } else if (parseFloat(instAmt2) > parseFloat(instPaid2)) {
                paidDue2.value = "Due";
            }
            else if (parseFloat(instAmt2) < parseFloat(instPaid2)) {
                paidDue2.value = "Paid";
            }
            var firstChar4 = myHidden4.substr(0, 1);
            if (firstChar4 == '-') {
                document.getElementById("txtstrRemaingBlnce4").readOnly = false;
            }
            else {
                document.getElementById("txtstrRemaingBlnce4").readOnly = false;
            }

        }

    </script>
    <script type="text/javascript">
        function applicable() {
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
        //PayOff Amount
        function myFunctionPayAmount1() {
            document.getElementById("txtpayoffamount").value = document.getElementById("o").innerText;
        }

        function formatMoneyPayAmount1(n, c, txtpayoffamount, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
                txtpayoffamount = txtpayoffamount == undefined ? "." : txtpayoffamount,
                t = t == undefined ? "," : t,
                s = n < 0 ? "-" : "",
                i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtpayoffamount + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function PayAmount1() {
            document.getElementById("o").innerText = formatMoneyPayAmount1(document.getElementById("txtpayoffamount").value);
        }
        function IsAlphaNumeric(e) {
            // alert(e.keyCode);
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <=

122) || (keyCode == 32));
            //document.getElementById("error").style.display = ret ? "none" : "inline";
            return ret;
        }
        //Amount Special Assessment Section
        function myFunctionAmount1() {
            document.getElementById("txtamountspecial").value = document.getElementById("s").innerText;
        }
        function formatMoneyAmount1(n, c, txtamountspecial, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtamountspecial = txtamountspecial == undefined ? "." : txtamountspecial,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtamountspecial + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function Amount1() {
            document.getElementById("s").innerText = formatMoneyAmount1(document.getElementById("txtamountspecial").value);
        }
        //Discount Amount
        function myFunctionDiscount1() {
            document.getElementById("txtstrDiscountAmount1").value = document.getElementById("a").innerText;
        }
        function myFunctionDiscount2() {
            document.getElementById("txtstrDiscountAmount2").value = document.getElementById("b").innerText;
        }
        function myFunctionDiscount3() {
            document.getElementById("txtstrDiscountAmount3").value = document.getElementById("c").innerText;
        }
        function myFunctionDiscount4() {
            document.getElementById("txtstrDiscountAmount4").value = document.getElementById("d").innerText;
        }
        function formatMoneyDiscount1(n, c, txtstrDiscountAmount1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrDiscountAmount1 = txtstrDiscountAmount1 == undefined ? "." : txtstrDiscountAmount1,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrDiscountAmount1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyDiscount2(n, c, txtstrDiscountAmount2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrDiscountAmount2 = txtstrDiscountAmount2 == undefined ? "." : txtstrDiscountAmount2,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrDiscountAmount2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyDiscount3(n, c, txtstrDiscountAmount3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrDiscountAmount3 = txtstrDiscountAmount3 == undefined ? "." : txtstrDiscountAmount3,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrDiscountAmount3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyDiscount4(n, c, txtstrDiscountAmount4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrDiscountAmount4 = txtstrDiscountAmount4 == undefined ? "." : txtstrDiscountAmount4,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrDiscountAmount4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function Discount1() {
            document.getElementById("a").innerText = formatMoneyDiscount1(document.getElementById("txtstrDiscountAmount1").value);
        }
        function Discount2() {
            document.getElementById("b").innerText = formatMoneyDiscount2(document.getElementById("txtstrDiscountAmount2").value);
        }
        function Discount3() {
            document.getElementById("c").innerText = formatMoneyDiscount3(document.getElementById("txtstrDiscountAmount3").value);
        }
        function Discount4() {
            document.getElementById("d").innerText = formatMoneyDiscount4(document.getElementById("txtstrDiscountAmount4").value);
        }
        //Inst.Amount Paid

        function myFunctionAmtPaid1() {
            document.getElementById("txtstrAmountPaid1").value = document.getElementById("e").innerText;
        }
        function myFunctionAmtPaid2() {
            document.getElementById("txtstrAmountPaid2").value = document.getElementById("f").innerText;
        }
        function myFunctionAmtPaid3() {
            document.getElementById("txtstrAmountPaid3").value = document.getElementById("g").innerText;
        }
        function myFunctionAmtPaid4() {
            document.getElementById("txtstrAmountPaid4").value = document.getElementById("h").innerText;
        }
        function formatMoneyAmtPaid1(n, c, txtstrAmountPaid1, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrAmountPaid1 = txtstrAmountPaid1 == undefined ? "." : txtstrAmountPaid1,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrAmountPaid1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyAmtPaid2(n, c, txtstrAmountPaid2, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrAmountPaid2 = txtstrAmountPaid2 == undefined ? "." : txtstrAmountPaid2,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrAmountPaid2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyAmtPaid3(n, c, txtstrAmountPaid3, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrAmountPaid3 = txtstrAmountPaid3 == undefined ? "." : txtstrAmountPaid3,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrAmountPaid3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoneyAmtPaid4(n, c, txtstrAmountPaid4, t) {
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrAmountPaid4 = txtstrAmountPaid4 == undefined ? "." : txtstrAmountPaid4,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrAmountPaid4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function AmtPaid1() {
            document.getElementById("e").innerText = formatMoneyAmtPaid1(document.getElementById("txtstrAmountPaid1").value);
        }
        function AmtPaid2() {
            document.getElementById("f").innerText = formatMoneyAmtPaid2(document.getElementById("txtstrAmountPaid2").value);
        }
        function AmtPaid3() {
            document.getElementById("g").innerText = formatMoneyAmtPaid3(document.getElementById("txtstrAmountPaid3").value);
        }
        function AmtPaid4() {
            document.getElementById("h").innerText = formatMoneyAmtPaid4(document.getElementById("txtstrAmountPaid4").value);
        }
        //Remaining Balance
        //function myFunctionRemBalance1() {
        //    document.getElementById("txtstrRemaingBlnce1").value = document.getElementById("i").innerText;
        //}
        //function myFunctionRemBalance2() {
        //    document.getElementById("txtstrRemaingBlnce2").value = document.getElementById("j").innerText;
        //}
        //function myFunctionRemBalance3() {
        //    document.getElementById("txtstrRemaingBlnce3").value = document.getElementById("k").innerText;
        //}
        //function myFunctionRemBalance4() {
        //    document.getElementById("txtstrRemaingBlnce4").value = document.getElementById("l").innerText;
        //}
        //function formatMoneyRemBalance1(n, c, txtstrRemaingBlnce1, t) {
        //    var c = isNaN(c = Math.abs(c)) ? 2 : c,
        //      txtstrRemaingBlnce1 = txtstrRemaingBlnce1 == undefined ? "." : txtstrRemaingBlnce1,
        //      t = t == undefined ? "," : t,
        //      s = n < 0 ? "-" : "",
        //      i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        //      j = (j = i.length) > 3 ? j % 3 : 0;

        //    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrRemaingBlnce1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        //};
        //function formatMoneyRemBalance2(n, c, txtstrRemaingBlnce2, t) {
        //    var c = isNaN(c = Math.abs(c)) ? 2 : c,
        //      txtstrRemaingBlnce2 = txtstrRemaingBlnce2 == undefined ? "." : txtstrRemaingBlnce2,
        //      t = t == undefined ? "," : t,
        //      s = n < 0 ? "-" : "",
        //      i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        //      j = (j = i.length) > 3 ? j % 3 : 0;

        //    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrRemaingBlnce2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        //};
        //function formatMoneyRemBalance3(n, c, txtstrRemaingBlnce3, t) {
        //    var c = isNaN(c = Math.abs(c)) ? 2 : c,
        //      txtstrRemaingBlnce3 = txtstrRemaingBlnce3 == undefined ? "." : txtstrRemaingBlnce3,
        //      t = t == undefined ? "," : t,
        //      s = n < 0 ? "-" : "",
        //      i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        //      j = (j = i.length) > 3 ? j % 3 : 0;

        //    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrRemaingBlnce3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        //};
        //function formatMoneyRemBalance4(n, c, txtstrRemaingBlnce4, t) {
        //    var c = isNaN(c = Math.abs(c)) ? 2 : c,
        //      txtstrRemaingBlnce4 = txtstrRemaingBlnce4 == undefined ? "." : txtstrRemaingBlnce4,
        //      t = t == undefined ? "," : t,
        //      s = n < 0 ? "-" : "",
        //      i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        //      j = (j = i.length) > 3 ? j % 3 : 0;

        //    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrRemaingBlnce4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        //};

        //function RemBalance1() {
        //    document.getElementById("i").innerText = formatMoneyRemBalance1(document.getElementById("txtstrRemaingBlnce1").value);
        //}
        //function RemBalance2() {
        //    document.getElementById("j").innerText = formatMoneyRemBalance2(document.getElementById("txtstrRemaingBlnce2").value);
        //}
        //function RemBalance3() {
        //    document.getElementById("k").innerText = formatMoneyRemBalance3(document.getElementById("txtstrRemaingBlnce3").value);
        //}
        //function RemBalance4() {
        //    document.getElementById("l").innerText = formatMoneyRemBalance4(document.getElementById("txtstrRemaingBlnce4").value);
        //}
    </script>
    <script type="text/javascript">
        function myFunction1() {
            document.getElementById("txtstrTaxAmount1").value = document.getElementById("w").innerText;
        }
        function myFunction2() {
            document.getElementById("txtstrTaxAmount2").value = document.getElementById("x").innerText;
        }
        function myFunction3() {
            document.getElementById("txtstrTaxAmount3").value = document.getElementById("y").innerText;
        }
        function myFunction4() {
            document.getElementById("txtstrTaxAmount4").value = document.getElementById("z").innerText;
        }
        function formatMoney1(n, c, txtstrTaxAmount1, t) {
            //var txtstrTaxAmount1 = document.getElementById("txtstrTaxAmount1").value;
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrTaxAmount1 = txtstrTaxAmount1 == undefined ? "." : txtstrTaxAmount1,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrTaxAmount1 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoney2(n, c, txtstrTaxAmount2, t) {
            //var txtstrTaxAmount1 = document.getElementById("txtstrTaxAmount1").value;
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrTaxAmount2 = txtstrTaxAmount2 == undefined ? "." : txtstrTaxAmount2,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrTaxAmount2 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoney3(n, c, txtstrTaxAmount3, t) {
            //var txtstrTaxAmount1 = document.getElementById("txtstrTaxAmount1").value;
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrTaxAmount3 = txtstrTaxAmount3 == undefined ? "." : txtstrTaxAmount3,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrTaxAmount3 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };
        function formatMoney4(n, c, txtstrTaxAmount4, t) {
            //var txtstrTaxAmount1 = document.getElementById("txtstrTaxAmount1").value;
            var c = isNaN(c = Math.abs(c)) ? 2 : c,
              txtstrTaxAmount4 = txtstrTaxAmount4 == undefined ? "." : txtstrTaxAmount4,
              t = t == undefined ? "," : t,
              s = n < 0 ? "-" : "",
              i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
              j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? txtstrTaxAmount4 + Math.abs(n - i).toFixed(c).slice(2) : "");

        };

        function hello1() {
            document.getElementById("w").innerText = formatMoney1(document.getElementById("txtstrTaxAmount1").value);

        }
        function hello2() {
            document.getElementById("x").innerText = formatMoney2(document.getElementById("txtstrTaxAmount2").value);

        }
        function hello3() {
            document.getElementById("y").innerText = formatMoney3(document.getElementById("txtstrTaxAmount3").value);

        }
        function hello4() {
            document.getElementById("z").innerText = formatMoney4(document.getElementById("txtstrTaxAmount4").value);

        }
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

            <%--var lblmesg = document.getElementById("<%=lblMesg.ClientID%>");--%>
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
                    //alert("Invalid Date Format   " +    txt.id);
                    alert("Invalid Date Format");
                    txt.value = '';
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
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        //-->
    </script>
    <script type="text/javascript">

        function txtpayment() {
            var delistatus = document.getElementById("txtdeliquent").value;
            var payfreq = document.getElementById("txtstrPaymentFrequency").value;
            var SpecialAsst = document.getElementById("txtSpcelAsst").value;
            var ddlselect = document.getElementById("txtnotapplicable").value;

            if (payfreq == "Annual") {
                document.getElementById("tblTaxBill").style.visibility = "visible";
                document.getElementById("tblTaxBill").style.display = "block";
                document.getElementById("tblpayfrenq").style.visibility = "visible";
                document.getElementById("tblpayfrenq").style.display = "block";
                document.getElementById("tabletax1").style.visibility = "visible";
                document.getElementById("tabletax1").style.display = "block";
                document.getElementById("tabletax2").style.visibility = "hidden";
                document.getElementById("tabletax2").style.display = "none";
                document.getElementById("tabletax3").style.visibility = "hidden";
                document.getElementById("tabletax3").style.display = "none";
                document.getElementById("tabletax4").style.visibility = "hidden";
                document.getElementById("tabletax4").style.display = "none";
                //document.getElementById("txtstrExmpStatus1").value = "No";


                if (delistatus == "Yes") {
                    document.getElementById("tbldeliquentstatus").style.visibility = "visible";
                    document.getElementById("tbldeliquentstatus").style.display = "block";
                    document.getElementById("tbldelistatus123").style.visibility = "visible";
                    document.getElementById("tbldelistatus123").style.display = "block";
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
                else {
                    document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                    document.getElementById("tbldeliquentstatus").style.display = "none";
                    document.getElementById("tbldelistatus123").style.visibility = "hidden";
                    document.getElementById("tbldelistatus123").style.display = "none";
                }

                if (SpecialAsst == "Yes") {
                    document.getElementById("tblspecialassessment").style.visibility = "visible";
                    document.getElementById("tblspecialassessment").style.display = "block";
                    document.getElementById("tblspecialassess123").style.visibility = "visible";
                    document.getElementById("tblspecialassess123").style.display = "block";
                }
                else {
                    document.getElementById("tblspecialassessment").style.visibility = "hidden";
                    document.getElementById("tblspecialassessment").style.display = "none";
                    document.getElementById("tblspecialassess123").style.visibility = "hidden";
                    document.getElementById("tblspecialassess123").style.display = "none";
                }
            }
            else if (payfreq == "Semiannual") {
                document.getElementById("tblTaxBill").style.visibility = "visible";
                document.getElementById("tblTaxBill").style.display = "block";
                document.getElementById("tblpayfrenq").style.visibility = "visible";
                document.getElementById("tblpayfrenq").style.display = "block";
                document.getElementById("tabletax1").style.visibility = "visible";
                document.getElementById("tabletax1").style.display = "block";
                document.getElementById("tabletax2").style.visibility = "visible";
                document.getElementById("tabletax2").style.display = "block";
                document.getElementById("tabletax3").style.visibility = "hidden";
                document.getElementById("tabletax3").style.display = "none";
                document.getElementById("tabletax4").style.visibility = "hidden";
                document.getElementById("tabletax4").style.display = "none";

                document.getElementById("tblspecialassess123").style.visibility = "hidden";
                document.getElementById("tblspecialassess123").style.display = "none";

                document.getElementById("tbldelistatus123").style.visibility = "hidden";
                document.getElementById("tbldelistatus123").style.display = "none";

                if (delistatus == "Yes") {
                    document.getElementById("tbldelistatus123").style.visibility = "visible";
                    document.getElementById("tbldelistatus123").style.display = "block";
                    document.getElementById("tbldeliquentstatus").style.visibility = "visible";
                    document.getElementById("tbldeliquentstatus").style.display = "block";
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
                else {
                    document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                    document.getElementById("tbldeliquentstatus").style.display = "none";
                    document.getElementById("tbldelistatus123").style.visibility = "hidden";
                    document.getElementById("tbldelistatus123").style.display = "none";
                }

                if (SpecialAsst == "Yes") {
                    document.getElementById("tblspecialassessment").style.visibility = "visible";
                    document.getElementById("tblspecialassessment").style.display = "block";
                    document.getElementById("tblspecialassess123").style.visibility = "visible";
                    document.getElementById("tblspecialassess123").style.display = "block";
                }
                else {
                    document.getElementById("tblspecialassessment").style.visibility = "hidden";
                    document.getElementById("tblspecialassessment").style.display = "none";
                    document.getElementById("tblspecialassess123").style.visibility = "hidden";
                    document.getElementById("tblspecialassess123").style.display = "none";
                }
            }
            else if (payfreq == "Quarterly") {
                document.getElementById("tblTaxBill").style.visibility = "visible";
                document.getElementById("tblTaxBill").style.display = "block";
                document.getElementById("tblpayfrenq").style.visibility = "visible";
                document.getElementById("tblpayfrenq").style.display = "block";
                document.getElementById("tabletax1").style.visibility = "visible";
                document.getElementById("tabletax1").style.display = "block";
                document.getElementById("tabletax2").style.visibility = "visible";
                document.getElementById("tabletax2").style.display = "block";
                document.getElementById("tabletax3").style.visibility = "visible";
                document.getElementById("tabletax3").style.display = "block";
                document.getElementById("tabletax4").style.visibility = "visible";
                document.getElementById("tabletax4").style.display = "block";

                document.getElementById("tblspecialassess123").style.visibility = "hidden";
                document.getElementById("tblspecialassess123").style.display = "none";

                document.getElementById("tbldelistatus123").style.visibility = "hidden";
                document.getElementById("tbldelistatus123").style.display = "none";

                if (delistatus == "Yes") {
                    document.getElementById("tbldeliquentstatus").style.visibility = "visible";
                    document.getElementById("tbldeliquentstatus").style.display = "block";
                    document.getElementById("tbldelistatus123").style.visibility = "visible";
                    document.getElementById("tbldelistatus123").style.display = "block";
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
                else {
                    document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                    document.getElementById("tbldeliquentstatus").style.display = "none";
                    document.getElementById("tbldelistatus123").style.visibility = "hidden";
                    document.getElementById("tbldelistatus123").style.display = "none";
                }

                if (SpecialAsst == "Yes") {
                    document.getElementById("tblspecialassessment").style.visibility = "visible";
                    document.getElementById("tblspecialassessment").style.display = "block";
                    document.getElementById("tblspecialassess123").style.visibility = "visible";
                    document.getElementById("tblspecialassess123").style.display = "block";
                }
                else {
                    document.getElementById("tblspecialassessment").style.visibility = "hidden";
                    document.getElementById("tblspecialassessment").style.display = "none";
                    document.getElementById("tblspecialassess123").style.visibility = "hidden";
                    document.getElementById("tblspecialassess123").style.display = "none";
                }
            }
            else {
                document.getElementById("tblTaxBill").style.visibility = "hidden";
                document.getElementById("tblTaxBill").style.display = "none";
                document.getElementById("tblpayfrenq").style.visibility = "hidden";
                document.getElementById("tblpayfrenq").style.display = "none";
                document.getElementById("tabletax1").style.visibility = "hidden";
                document.getElementById("tabletax1").style.display = "none";
                document.getElementById("tabletax2").style.visibility = "hidden";
                document.getElementById("tabletax2").style.display = "none";
                document.getElementById("tabletax3").style.visibility = "hidden";
                document.getElementById("tabletax3").style.display = "none";
                document.getElementById("tabletax4").style.visibility = "hidden";
                document.getElementById("tabletax4").style.display = "none";

                document.getElementById("tblspecialassess123").style.visibility = "hidden";
                document.getElementById("tblspecialassess123").style.display = "none";

                document.getElementById("tbldelistatus123").style.visibility = "hidden";
                document.getElementById("tbldelistatus123").style.display = "none";

                if (delistatus == "Yes") {
                    document.getElementById("tbldeliquentstatus").style.visibility = "visible";
                    document.getElementById("tbldeliquentstatus").style.display = "block";
                    document.getElementById("tbldelistatus123").style.visibility = "visible";
                    document.getElementById("tbldelistatus123").style.display = "block";
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
                else {
                    document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                    document.getElementById("tbldeliquentstatus").style.display = "none";
                    document.getElementById("tbldelistatus123").style.visibility = "hidden";
                    document.getElementById("tbldelistatus123").style.display = "none";
                }

                if (SpecialAsst == "Yes") {
                    document.getElementById("tblspecialassessment").style.visibility = "visible";
                    document.getElementById("tblspecialassessment").style.display = "block";
                    document.getElementById("tblspecialassess123").style.visibility = "visible";
                    document.getElementById("tblspecialassess123").style.display = "block";
                }
                else {
                    document.getElementById("tblspecialassessment").style.visibility = "hidden";
                    document.getElementById("tblspecialassessment").style.display = "none";
                    document.getElementById("tblspecialassess123").style.visibility = "hidden";
                    document.getElementById("tblspecialassess123").style.display = "none";
                }
            }
        }

        function txtdeliquentsta1() {
            var delistatus = document.getElementById("txtdeliquent").value;
            var payfreq = document.getElementById("txtstrPaymentFrequency").value;

            if (delistatus == "Yes") {
                document.getElementById("tbldelistatus123").style.visibility = "visible";
                document.getElementById("tbldelistatus123").style.display = "block";
                document.getElementById("tbldeliquentstatus").style.visibility = "visible";
                document.getElementById("tbldeliquentstatus").style.display = "block";
                document.getElementById("txtpayoffamount").value = '0.00';

                if (payfreq == "Annual") {
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "hidden";
                    document.getElementById("tabletax2").style.display = "none";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                    //document.getElementById("txtstrExmpStatus1").value = "No";
                }
                else if (payfreq == "Semiannual") {
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                }
                else if (payfreq == "Quarterly") {
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "visible";
                    document.getElementById("tabletax3").style.display = "block";
                    document.getElementById("tabletax4").style.visibility = "visible";
                    document.getElementById("tabletax4").style.display = "block";
                }

            }
            else if (delistatus == "No") {
                document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                document.getElementById("tbldeliquentstatus").style.display = "none";
                document.getElementById("tbldelistatus123").style.visibility = "hidden";
                document.getElementById("tbldelistatus123").style.display = "none";
                document.getElementById("txtdelitaxstatus").value = '';
                document.getElementById("txtpayoffamount").value = '0.00';
                document.getElementById("txtpayoffgood").value = '';
                document.getElementById("txtinitialinstall").value = '';
                document.getElementById("txtnotapplicable").value = 'Select';
                document.getElementById("txtdatetaxsale").value = '';
                document.getElementById("txtlastdayred").value = '';
                if (payfreq == "Annual") {
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "hidden";
                    document.getElementById("tabletax2").style.display = "none";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                    //document.getElementById("txtstrExmpStatus1").value = "No";
                }
                else if (payfreq == "Semiannual") {
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                }
                else if (payfreq == "Quarterly") {
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "visible";
                    document.getElementById("tabletax3").style.display = "block";
                    document.getElementById("tabletax4").style.visibility = "visible";
                    document.getElementById("tabletax4").style.display = "block";
                }
            }
            else {
                document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                document.getElementById("tbldeliquentstatus").style.display = "none";
                document.getElementById("tbldelistatus123").style.visibility = "hidden";
                document.getElementById("tbldelistatus123").style.display = "none";

                document.getElementById("txtdelitaxstatus").value = '';
                document.getElementById("txtpayoffamount").value = '0.00';
                document.getElementById("txtpayoffgood").value = '';
                document.getElementById("txtinitialinstall").value = '';
                document.getElementById("txtnotapplicable").value = 'Select';
                document.getElementById("txtdatetaxsale").value = '';
                document.getElementById("txtlastdayred").value = '';

                if (payfreq == "Annual") {

                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "hidden";
                    document.getElementById("tabletax2").style.display = "none";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                    //document.getElementById("txtstrExmpStatus1").value = "No";
                }
                else if (payfreq == "Semiannual") {
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                }
                else if (payfreq == "Quarterly") {
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "visible";
                    document.getElementById("tabletax3").style.display = "block";
                    document.getElementById("tabletax4").style.visibility = "visible";
                    document.getElementById("tabletax4").style.display = "block";
                }

            }
        }

        function txtSpcelAsststa1() {
            var SpecialAsst = document.getElementById("txtSpcelAsst").value;
            var payfreq = document.getElementById("txtstrPaymentFrequency").value;

            if (SpecialAsst == "Yes") {
                document.getElementById("tblspecialassess123").style.visibility = "visible";
                document.getElementById("tblspecialassess123").style.display = "block";
                document.getElementById("tblspecialassessment").style.visibility = "visible";
                document.getElementById("tblspecialassessment").style.display = "block";
                document.getElementById("txtamountspecial").value = "0.00";

                if (payfreq == "Annual") {
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "hidden";
                    document.getElementById("tabletax2").style.display = "none";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                    //document.getElementById("txtstrExmpStatus1").value = "No";
                }
                else if (payfreq == "Semiannual") {
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                }
                else if (payfreq == "Quarterly") {
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "visible";
                    document.getElementById("tabletax3").style.display = "block";
                    document.getElementById("tabletax4").style.visibility = "visible";
                    document.getElementById("tabletax4").style.display = "block";
                }

            }
            else if (SpecialAsst == "No") {
                document.getElementById("tblspecialassessment").style.visibility = "hidden";
                document.getElementById("tblspecialassessment").style.display = "none";
                document.getElementById("tblspecialassess123").style.visibility = "hidden";
                document.getElementById("tblspecialassess123").style.display = "none";

                document.getElementById("txtdescription").value = '';
                document.getElementById("txtspecialassno").value = '';
                document.getElementById("txtnoinstall").value = '';
                document.getElementById("txtinstallpaid").value = '';
                document.getElementById("txtamountspecial").value = '0.00';
                if (payfreq == "Annual") {
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "hidden";
                    document.getElementById("tabletax2").style.display = "none";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                }
                else if (payfreq == "Semiannual") {

                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                }
                else if (payfreq == "Quarterly") {

                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "visible";
                    document.getElementById("tabletax3").style.display = "block";
                    document.getElementById("tabletax4").style.visibility = "visible";
                    document.getElementById("tabletax4").style.display = "block";
                }
            }
            else {

                document.getElementById("tblspecialassessment").style.visibility = "hidden";
                document.getElementById("tblspecialassessment").style.display = "none";
                document.getElementById("tblspecialassess123").style.visibility = "hidden";
                document.getElementById("tblspecialassess123").style.display = "none";


                document.getElementById("txtdescription").value = '';
                document.getElementById("txtspecialassno").value = '';
                document.getElementById("txtnoinstall").value = '';
                document.getElementById("txtinstallpaid").value = '';
                document.getElementById("txtamountspecial").value = '0.00';


                if (payfreq == "Annual") {

                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "hidden";
                    document.getElementById("tabletax2").style.display = "none";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                }
                else if (payfreq == "Semiannual") {
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "hidden";
                    document.getElementById("tabletax3").style.display = "none";
                    document.getElementById("tabletax4").style.visibility = "hidden";
                    document.getElementById("tabletax4").style.display = "none";
                }
                else if (payfreq == "Quarterly") {

                    document.getElementById("tblTaxBill").style.visibility = "visible";
                    document.getElementById("tblTaxBill").style.display = "block";
                    document.getElementById("tblpayfrenq").style.visibility = "visible";
                    document.getElementById("tblpayfrenq").style.display = "block";
                    document.getElementById("tabletax1").style.visibility = "visible";
                    document.getElementById("tabletax1").style.display = "block";
                    document.getElementById("tabletax2").style.visibility = "visible";
                    document.getElementById("tabletax2").style.display = "block";
                    document.getElementById("tabletax3").style.visibility = "visible";
                    document.getElementById("tabletax3").style.display = "block";
                    document.getElementById("tabletax4").style.visibility = "visible";
                    document.getElementById("tabletax4").style.display = "block";
                }
            }
        }

        function txtpaymentfrequency() {
            var delistatus1 = document.getElementById("txtdeliquent").value;
            var payfreq1 = document.getElementById("txtstrPaymentFrequency").value;
            var SpecialAsst1 = document.getElementById("txtSpcelAsst").value;

            if (payfreq1 == "Annual") {
                document.getElementById('txtstrRemaingBlnce1').disabled = false;
                document.getElementById("tblTaxBill").style.visibility = "visible";
                document.getElementById("tblTaxBill").style.display = "block";
                document.getElementById("tblpayfrenq").style.visibility = "visible";
                document.getElementById("tblpayfrenq").style.display = "block";
                document.getElementById("tabletax1").style.visibility = "visible";
                document.getElementById("tabletax1").style.display = "block";
                document.getElementById("tabletax2").style.visibility = "hidden";
                document.getElementById("tabletax2").style.display = "none";
                document.getElementById("tabletax3").style.visibility = "hidden";
                document.getElementById("tabletax3").style.display = "none";
                document.getElementById("tabletax4").style.visibility = "hidden";
                document.getElementById("tabletax4").style.display = "none";
                //Clear Fields
                document.getElementById("txtstrTaxStatus1").value = "Select";
                document.getElementById("txtstrTaxAmount1").value = "0.00";
                document.getElementById("txtstrDiscountAmount1").value = "0.00";
                document.getElementById("txtstrAmountPaid1").value = "0.00";
                document.getElementById("txtstrRemaingBlnce1").value = "0.00";
                document.getElementById("txtstrExmpStatus1").value = "Select";

                document.getElementById("txtstrTaxStatus2").value = "Select";
                document.getElementById("txtstrTaxAmount2").value = "0.00";
                document.getElementById("txtstrDiscountAmount2").value = "0.00";
                document.getElementById("txtstrAmountPaid2").value = "0.00";
                document.getElementById("txtstrRemaingBlnce2").value = "0.00";
                document.getElementById("txtstrExmpStatus2").value = "Select";

                document.getElementById("txtstrTaxStatus3").value = "Select";
                document.getElementById("txtstrTaxAmount3").value = "0.00";
                document.getElementById("txtstrDiscountAmount3").value = "0.00";
                document.getElementById("txtstrAmountPaid3").value = "0.00";
                document.getElementById("txtstrRemaingBlnce3").value = "0.00";
                document.getElementById("txtstrExmpStatus3").value = "Select";

                document.getElementById("txtstrTaxStatus4").value = "Select";
                document.getElementById("txtstrTaxAmount4").value = "0.00";
                document.getElementById("txtstrDiscountAmount4").value = "0.00";
                document.getElementById("txtstrAmountPaid4").value = "0.00";
                document.getElementById("txtstrRemaingBlnce4").value = "0.00";
                //document.getElementById("txtstrExmpStatus4").value = "Select";
                document.getElementById("txtstrExmpStatus1").value = "No";
                document.getElementById("txtstrExmpStatus2").value = "No";
                document.getElementById("txtstrExmpStatus3").value = "No";
                document.getElementById("txtstrExmpStatus4").value = "No";

                if (delistatus1 == "Yes") {
                    document.getElementById("tbldeliquentstatus").style.visibility = "visible";
                    document.getElementById("tbldeliquentstatus").style.display = "block";
                    document.getElementById("tbldelistatus123").style.visibility = "visible";
                    document.getElementById("tbldelistatus123").style.display = "block";
                }
                else {
                    document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                    document.getElementById("tbldeliquentstatus").style.display = "none";
                    document.getElementById("tbldelistatus123").style.visibility = "hidden";
                    document.getElementById("tbldelistatus123").style.display = "none";
                }

                if (SpecialAsst1 == "Yes") {
                    document.getElementById("tblspecialassessment").style.visibility = "visible";
                    document.getElementById("tblspecialassessment").style.display = "block";
                    document.getElementById("tblspecialassess123").style.visibility = "visible";
                    document.getElementById("tblspecialassess123").style.display = "block";
                }
                else {
                    document.getElementById("tblspecialassessment").style.visibility = "hidden";
                    document.getElementById("tblspecialassessment").style.display = "none";
                    document.getElementById("tblspecialassess123").style.visibility = "hidden";
                    document.getElementById("tblspecialassess123").style.display = "none";
                }
            }
            else if (payfreq1 == "Semiannual") {
                document.getElementById('txtstrRemaingBlnce1').disabled = false;
                document.getElementById('txtstrRemaingBlnce2').disabled = false;
                document.getElementById("tblpayfrenq").style.visibility = "visible";
                document.getElementById("tblpayfrenq").style.display = "block";
                document.getElementById("tblTaxBill").style.visibility = "visible";
                document.getElementById("tblTaxBill").style.display = "block";
                document.getElementById("tabletax1").style.visibility = "visible";
                document.getElementById("tabletax1").style.display = "block";
                document.getElementById("tabletax2").style.visibility = "visible";
                document.getElementById("tabletax2").style.display = "block";
                document.getElementById("tabletax3").style.visibility = "hidden";
                document.getElementById("tabletax3").style.display = "none";
                document.getElementById("tabletax4").style.visibility = "hidden";
                document.getElementById("tabletax4").style.display = "none";
                //Clear Fields
                document.getElementById("txtstrTaxStatus1").value = "Select";
                document.getElementById("txtstrTaxAmount1").value = "0.00";
                document.getElementById("txtstrDiscountAmount1").value = "0.00";
                document.getElementById("txtstrAmountPaid1").value = "0.00";
                document.getElementById("txtstrRemaingBlnce1").value = "0.00";
                document.getElementById("txtstrExmpStatus1").value = "Select";

                document.getElementById("txtstrTaxStatus2").value = "Select";
                document.getElementById("txtstrTaxAmount2").value = "0.00";
                document.getElementById("txtstrDiscountAmount2").value = "0.00";
                document.getElementById("txtstrAmountPaid2").value = "0.00";
                document.getElementById("txtstrRemaingBlnce2").value = "0.00";
                document.getElementById("txtstrExmpStatus2").value = "Select";

                document.getElementById("txtstrTaxStatus3").value = "Select";
                document.getElementById("txtstrTaxAmount3").value = "0.00";
                document.getElementById("txtstrDiscountAmount3").value = "0.00";
                document.getElementById("txtstrAmountPaid3").value = "0.00";
                document.getElementById("txtstrRemaingBlnce3").value = "0.00";
                document.getElementById("txtstrExmpStatus3").value = "Select";

                document.getElementById("txtstrTaxStatus4").value = "Select";
                document.getElementById("txtstrTaxAmount4").value = "0.00";
                document.getElementById("txtstrDiscountAmount4").value = "0.00";
                document.getElementById("txtstrAmountPaid4").value = "0.00";
                document.getElementById("txtstrRemaingBlnce4").value = "0.00";
                document.getElementById("txtstrExmpStatus4").value = "Select";

                document.getElementById("txtstrExmpStatus1").value = "No";
                document.getElementById("txtstrExmpStatus2").value = "No";
                document.getElementById("txtstrExmpStatus3").value = "No";
                document.getElementById("txtstrExmpStatus4").value = "No";

                if (delistatus1 == "Yes") {
                    document.getElementById("tbldelistatus123").style.visibility = "visible";
                    document.getElementById("tbldelistatus123").style.display = "block";
                    document.getElementById("tbldeliquentstatus").style.visibility = "visible";
                    document.getElementById("tbldeliquentstatus").style.display = "block";
                }
                else {
                    document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                    document.getElementById("tbldeliquentstatus").style.display = "none";
                    document.getElementById("tbldelistatus123").style.visibility = "hidden";
                    document.getElementById("tbldelistatus123").style.display = "none";
                }

                if (SpecialAsst1 == "Yes") {
                    document.getElementById("tblspecialassessment").style.visibility = "visible";
                    document.getElementById("tblspecialassessment").style.display = "block";
                    document.getElementById("tblspecialassess123").style.visibility = "visible";
                    document.getElementById("tblspecialassess123").style.display = "block";
                }
                else {
                    document.getElementById("tblspecialassessment").style.visibility = "hidden";
                    document.getElementById("tblspecialassessment").style.display = "none";
                    document.getElementById("tblspecialassess123").style.visibility = "hidden";
                    document.getElementById("tblspecialassess123").style.display = "none";
                }

            }
            else if (payfreq1 == "Quarterly") {
                document.getElementById('txtstrRemaingBlnce1').disabled = false;
                document.getElementById('txtstrRemaingBlnce2').disabled = false;
                document.getElementById('txtstrRemaingBlnce3').disabled = false;
                document.getElementById('txtstrRemaingBlnce4').disabled = false;
                document.getElementById("tblTaxBill").style.visibility = "visible";
                document.getElementById("tblTaxBill").style.display = "block";
                document.getElementById("tblpayfrenq").style.visibility = "visible";
                document.getElementById("tblpayfrenq").style.display = "block";
                document.getElementById("tabletax1").style.visibility = "visible";
                document.getElementById("tabletax1").style.display = "block";
                document.getElementById("tabletax2").style.visibility = "visible";
                document.getElementById("tabletax2").style.display = "block";
                document.getElementById("tabletax3").style.visibility = "visible";
                document.getElementById("tabletax3").style.display = "block";
                document.getElementById("tabletax4").style.visibility = "visible";
                document.getElementById("tabletax4").style.display = "block";
                //Clear Fields
                document.getElementById("txtstrTaxStatus1").value = "Select";
                document.getElementById("txtstrTaxAmount1").value = "0.00";
                document.getElementById("txtstrDiscountAmount1").value = "0.00";
                document.getElementById("txtstrAmountPaid1").value = "0.00";
                document.getElementById("txtstrRemaingBlnce1").value = "0.00";
                document.getElementById("txtstrExmpStatus1").value = "Select";

                document.getElementById("txtstrTaxStatus2").value = "Select";
                document.getElementById("txtstrTaxAmount2").value = "0.00";
                document.getElementById("txtstrDiscountAmount2").value = "0.00";
                document.getElementById("txtstrAmountPaid2").value = "0.00";
                document.getElementById("txtstrRemaingBlnce2").value = "0.00";
                document.getElementById("txtstrExmpStatus2").value = "Select";

                document.getElementById("txtstrTaxStatus3").value = "Select";
                document.getElementById("txtstrTaxAmount3").value = "0.00";
                document.getElementById("txtstrDiscountAmount3").value = "0.00";
                document.getElementById("txtstrAmountPaid3").value = "0.00";
                document.getElementById("txtstrRemaingBlnce3").value = "0.00";
                document.getElementById("txtstrExmpStatus3").value = "Select";

                document.getElementById("txtstrTaxStatus4").value = "Select";
                document.getElementById("txtstrTaxAmount4").value = "0.00";
                document.getElementById("txtstrDiscountAmount4").value = "0.00";
                document.getElementById("txtstrAmountPaid4").value = "0.00";
                document.getElementById("txtstrRemaingBlnce4").value = "0.00";
                document.getElementById("txtstrExmpStatus4").value = "Select";

                document.getElementById("txtstrExmpStatus1").value = "No";
                document.getElementById("txtstrExmpStatus2").value = "No";
                document.getElementById("txtstrExmpStatus3").value = "No";
                document.getElementById("txtstrExmpStatus4").value = "No";
                if (delistatus1 == "Yes") {
                    document.getElementById("tbldeliquentstatus").style.visibility = "visible";
                    document.getElementById("tbldeliquentstatus").style.display = "block";
                    document.getElementById("tbldelistatus123").style.visibility = "visible";
                    document.getElementById("tbldelistatus123").style.display = "block";
                }
                else {
                    document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                    document.getElementById("tbldeliquentstatus").style.display = "none";
                    document.getElementById("tbldelistatus123").style.visibility = "hidden";
                    document.getElementById("tbldelistatus123").style.display = "none";
                }

                if (SpecialAsst1 == "Yes") {
                    document.getElementById("tblspecialassessment").style.visibility = "visible";
                    document.getElementById("tblspecialassessment").style.display = "block";
                    document.getElementById("tblspecialassess123").style.visibility = "visible";
                    document.getElementById("tblspecialassess123").style.display = "block";
                }
                else {
                    document.getElementById("tblspecialassessment").style.visibility = "hidden";
                    document.getElementById("tblspecialassessment").style.display = "none";
                    document.getElementById("tblspecialassess123").style.visibility = "hidden";
                    document.getElementById("tblspecialassess123").style.display = "none";
                }
            }
            else {
                document.getElementById("tblTaxBill").style.visibility = "hidden";
                document.getElementById("tblTaxBill").style.display = "none";
                document.getElementById("tblpayfrenq").style.visibility = "hidden";
                document.getElementById("tblpayfrenq").style.display = "none";
                document.getElementById("tabletax1").style.visibility = "hidden";
                document.getElementById("tabletax1").style.display = "none";
                document.getElementById("tabletax2").style.visibility = "hidden";
                document.getElementById("tabletax2").style.display = "none";
                document.getElementById("tabletax3").style.visibility = "hidden";
                document.getElementById("tabletax3").style.display = "none";
                document.getElementById("tabletax4").style.visibility = "hidden";
                document.getElementById("tabletax4").style.display = "none";

                document.getElementById("tblspecialassess123").style.visibility = "hidden";
                document.getElementById("tblspecialassess123").style.display = "none";

                document.getElementById("tbldelistatus123").style.visibility = "hidden";
                document.getElementById("tbldelistatus123").style.display = "none";

                if (delistatus1 == "Yes") {
                    document.getElementById("tbldeliquentstatus").style.visibility = "visible";
                    document.getElementById("tbldeliquentstatus").style.display = "block";
                    document.getElementById("tbldelistatus123").style.visibility = "visible";
                    document.getElementById("tbldelistatus123").style.display = "block";

                }
                else {
                    document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                    document.getElementById("tbldeliquentstatus").style.display = "none";
                    document.getElementById("tbldelistatus123").style.visibility = "hidden";
                    document.getElementById("tbldelistatus123").style.display = "none";
                }

                if (SpecialAsst1 == "Yes") {
                    document.getElementById("tblspecialassessment").style.visibility = "visible";
                    document.getElementById("tblspecialassessment").style.display = "block";
                    document.getElementById("tblspecialassess123").style.visibility = "visible";
                    document.getElementById("tblspecialassess123").style.display = "block";
                }
                else {
                    document.getElementById("tblspecialassessment").style.visibility = "hidden";
                    document.getElementById("tblspecialassessment").style.display = "none";
                    document.getElementById("tblspecialassess123").style.visibility = "hidden";
                    document.getElementById("tblspecialassess123").style.display = "none";
                }
                //document.getElementById("tblpayfrenq").style.visibility = "hidden";
                //document.getElementById("tblpayfrenq").style.display = "none";
                //document.getElementById("tabletax1").style.visibility = "hidden";
                //document.getElementById("tabletax1").style.display = "none";
                //document.getElementById("tabletax2").style.visibility = "hidden";
                //document.getElementById("tabletax2").style.display = "none";
                //document.getElementById("tabletax3").style.visibility = "hidden";
                //document.getElementById("tabletax3").style.display = "none";
                //document.getElementById("tabletax4").style.visibility = "hidden";
                //document.getElementById("tabletax4").style.display = "none";
                //document.getElementById("tbldeliquentstatus").style.visibility = "hidden";
                //document.getElementById("tbldeliquentstatus").style.display = "none";
                //document.getElementById("tbldelistatus123").style.visibility = "hidden";
                //document.getElementById("tbldelistatus123").style.display = "none";

                //document.getElementById("tblspecialassessment").style.visibility = "hidden";
                //document.getElementById("tblspecialassessment").style.display = "none";
                //document.getElementById("tblspecialassess123").style.visibility = "hidden";
                //document.getElementById("tblspecialassess123").style.display = "none";
            }
        }
    </script>
    <script type="text/javascript">
        $(function () {
            blinkeffect('#lblserpro');
        })
        function blinkeffect(selector) {
            $(selector).fadeOut('slow', function () {
                $(this).fadeIn('slow', function () {
                    blinkeffect(this);
                });
            });
        }
        function Confim() {
            var result = window.confirm('Are you sure?');
            if (result == true)
                return true;
            else
                return false;
        }

    </script>

    <script type="text/javascript">
        $("#cancel").click(function () {
            document.getElementById("fileupload1").value = "";
        })
    </script>

    <style type="text/css">
        .table1,
        .th1,
        .td1,
        .td1 {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .font1 {
            font-family: -apple-system,BlinkMacSystemFont,"Segoe UI",Calibri,"Helvetica Neue",Arial,Calibri,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
            font-weight: 400;
        }


        .Mismatch_Popup {
            position: absolute;
            height: auto;
            width: 850px;
            /*left: 30%;*/
            top: 3%;
            right: 18%;
            border: solid 2px #ffffff;
            z-index: 50;
            color: Black;
            /*font-weight: bolder;*/
            background-color: #f5f5f5;
            -moz-border-radius: 20px;
            -webkit-border-radius: 20px;
        }
    </style>
    <style type="text/css">
        .txtuser {
        }

        .font {
            font-family: -apple-system,BlinkMacSystemFont,"Segoe UI",Calibri,"Helvetica Neue",Arial,Calibri,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
            font-weight: 600;
        }
    </style>
    <style type="text/css">
        body {
            font-family: Calibri;
            font-size: 10pt;
        }

        .table {
            border: 1px solid #ccc;
            border-collapse: collapse;
            width: 200px;
        }

            .table th {
                background-color: #F7F7F7;
                color: #333;
                font-weight: bold;
            }

            .table th, .table td {
                padding: 5px;
                border: 1px solid #ccc;
            }

        .auto-style4 {
            width: 228px;
        }

        .auto-style5 {
            width: 95%;
        }

        .auto-style6 {
            float: left;
            width: 200px;
            height: 59px;
        }

        th {
            white-space: nowrap;
            font-weight: bold;
            text-align: center;
        }
    </style>
    <style type="text/css">
        .bs-example {
            margin: 20px;
        }
    </style>
    <style type="text/css">
        .panel-heading .accordion-toggle:after {
            font-family: 'Glyphicons Halflings';
            content: "\2212";
            float: right;
            color: grey;
        }

        .panel-heading .accordion-toggle.collapsed:after {
            content: "\002B";
        }

        .auto-style7 {
            width: 434px;
            height: 78px;
        }

        .auto-style8 {
            height: 91px;
            width: 470px;
        }

        .auto-style9 {
            height: 25px;
            width: 324px;
        }

        .auto-style10 {
            width: 324px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="font-family: Helvetica, Calibri">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="page_dimmer" id="pagedimmer" runat="server">
        </div>

        <div class="msg_box_container" id="Other_breakMsgbx" runat="server" align="center">
            <table width="600px" style="display: none;">
                <tr>
                    <td colspan="3" class="PagedimmerMsg" align="center" style="height: 36px">
                        <asp:Label ID="lbltext" runat="server" ForeColor="black" Font-Bold="true"
                            Font-Size="Larger" Text="Break Comments"></asp:Label>
                    </td>
                </tr>
            </table>
            <div id="div_brk_start" runat="server">
                <table width="600px">
                    <tr>
                        <td align="right">
                            <asp:Label ID="Lblcomments" runat="server" Text="Select Break Type:" CssClass="PagedimmerMsg"
                                ForeColor="black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="drp_break" runat="server" Width="150px" ForeColor="Black">
                                <asp:ListItem Value="1">Tea/Coffee</asp:ListItem>
                                <asp:ListItem Value="2">Lunch/Dinner</asp:ListItem>
                                <asp:ListItem Value="3">Short Break</asp:ListItem>
                                <asp:ListItem Value="4">Meeting</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_brk_start" runat="server" Text="Start" CssClass="MenuFont" OnClick="btn_brk_start_Click" />
                            <asp:Button ID="btn_brk_cancel" CssClass="MenuFont" runat="server" Text="Cancel"
                                OnClick="btn_brk_cancel_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="div_brk_cmd" runat="server">
                <table width="600px">
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="lbl_brk_name" runat="server" ForeColor="#862d2d" Font-Size="19px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:UpdatePanel ID="up_Timer" runat="server" RenderMode="Inline" UpdateMode="Always">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="Timer3" EventName="Tick" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Timer ID="Timer3" runat="server" Interval="1000" OnTick="Timer1_Tickb" />
                                    <%--  <asp:Literal ID="lit_Timer" runat="server" /><br />--%>

                                    <asp:Label ID="lit_Timer" runat="server" Font-Size="18px"></asp:Label><br />
                                    <asp:HiddenField ID="hid_Ticker" runat="server" Value="0" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <div id="div_brk_countdown" runat="server">
                    <table width="600px">
                        <tr>
                            <%-- <td>
                        <span id="time"></span>
                        <input type="button" value="start" onclick="start();"/>
                        </td>--%>
                            <td align="center">
                                <asp:Button ID="btn_brk_stop" runat="server" Text="Stop" CssClass="RvButton" OnClick="btn_brk_stop_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Label ID="lblothererror" runat="server" ForeColor="#862d2d" Font-Size="19px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_brk_stop" runat="server">
                    <table width="600px">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label2" runat="server" Text="Enter Delay Reson:" CssClass="PagedimmerMsg"
                                    ForeColor="black"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_break_reason" runat="server" Font-Names="Georgia" TextMode="MultiLine"
                                    placeholder="Maximum 100 Characters" Height="50px" Width="400px"></asp:TextBox>
                                <br />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_break_reason" ForeColor="#862d2d" Font-Size="17px"
                                    ErrorMessage=" Please limit to 100 characters or less." ValidationExpression="[\s\S]{1,100}" ValidationGroup="g2"></asp:RegularExpressionValidator>

                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="height: 33px">
                                <asp:Button ID="btn_brk_reson" runat="server" Text="Save" CssClass="RvButton" OnClick="btn_brk_reson_Click" ValidationGroup="g2" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="lbl_brk_cmd_err" runat="server" CssClass="ErrorLabel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

        <div id="Tblmain" style="height: 45px">
            <%--<div id="logo" style="float: left; width: 230px; height: 85px;">
            </div>--%>
            <div id="templatemo_header" style="float: left; height: 45px;">
                <table border="0" class="auto-style6" style="width: 1330px;">
                    <tr>
                        <td style="font-size: 15px; color: White; width: 300px; padding-bottom: 20px;"
                            valign="bottom" class="font">
                            <asp:Label ID="Lblusername" runat="server"></asp:Label>
                        </td>
                        <td align="center" valign="top">
                            <asp:Image ID="Imgtitle" runat="server" ImageUrl="~/App_themes/Black/images/t1.png" Style="height: 49px;" />
                        </td>

                        <td class="navbar-right" style="padding-top: 7px; width: 117px;">
                            <asp:Button ID="LogoutBtn" runat="server" CssClass="MenuFont" Text="Logout" Font-Overline="false"
                                OnClick="LogoutBtn_Click"></asp:Button>
                        </td>
                        <td style="padding-bottom: 2px; width: 80px; padding-left: 15px;">
                            <asp:Button ID="lnkOthers" runat="server" CssClass="MenuFont" Text="Break" Font-Overline="false"
                                OnClick="lnkOthers_Click"></asp:Button>
                        </td>
                        <%-- <td style="font-family: Comic Sans MS; font-size: 15px; color: White; width: 883px;"
                            align="right">
                            <asp:Label ID="Lbltime" runat="server" Text="" Visible="false"></asp:Label>
                        </td>--%>
                    </tr>
                </table>
            </div>
        </div>
        <%--        <div id="templatemo_menu_wrapper">
            <div id="templatemo_menu">
                <table cellpadding="5px">
                    <tr>
                        <td style="font-family: Calibri; font-size: 15px; color: White; width: 300px;"
                            valign="bottom">
                            <asp:Label ID="Lblusername" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="LogoutBtn" runat="server" CssClass="MenuFont" Text="Logout" Font-Overline="false"
                                OnClick="LogoutBtn_Click"></asp:Button>
                        </td>
                        <td style="font-family: Comic Sans MS; font-size: 15px; color: White; width: 883px;"
                            align="right">
                            <asp:Label ID="Lbltime" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </div>
            <!-- end of menu -->
        </div>--%>
        <!-- end of menu wrapper -->
        <div>
            <table style="width: 100%; border-bottom: outset 1px gray;">
                <tr>
                    <td align="center" valign="top" style="width: 200px">
                        <asp:GridView ID="Gridutilization" runat="server" Width="600px" GridLines="None"
                            CssClass="Gnowrap">
                        </asp:GridView>
                    </td>
                    <td style="width: 15%; font-size: 16px; color: Black; padding-right: 13px;" align="right" class="font">
                        <asp:Label ID="lblbreaktotal" runat="server" Text="Break Time:"></asp:Label>
                        <asp:Label ID="lblbreak" runat="server" Text="00:00:00"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>


        <div class="container" style="padding-top: 12px; width: 1330px;">
            <div class="col-lg-12 well" style="background-color: white; width: 1302px;">
                <div class="row">
                    <div class="col-sm-12">
                        <table id="MainTable" style="height: 500px; background-color: white; width: 100%" class="tblproduction">
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="PanelOrderallotment" runat="server">

                                        <table style="background-color: #f1f1f1; border-collapse: collapse; border-spacing: 0 1em;" width="1260px" cellpadding="5" border="0" cellspacing="10">
                                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                                <td colspan="5" align="center" style="height: 25px" class="font">
                                                    <asp:Label ID="Lblprocessname" runat="server" Font-Size="Large" Text="PRODUCTION"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr align="center">
                                                <td colspan="4" align="center" style="background-color: white;">
                                                    <%-- <asp:TextBox ID="txtcommentshistory" runat="server" TextMode="MultiLine" Height="200px"      Width="550px" ReadOnly="True"></asp:TextBox>--%>
                                                    <div id="scrapgrd" runat="server" style="overflow: scroll; height: auto; width: 1260px;">
                                                        <table style="background-color: white;" border="1">
                                                            <tr>
                                                                <td align="left" class="font">
                                                                    <asp:HyperLink class="btn btn-default btn-sm" ID="hyperlink1" Font-Bold="true" Font-Size="Large" NavigateUrl="~/Pages/Taxdetails.aspx" Target="_blank" runat="server" Style="margin-left: 40px; margin-bottom: 6px; margin-top: 5px; font-weight: 500; height: 35px; background-color: lightgray">
                                                                        <span class="glyphicon glyphicon-list-alt font" style="padding-right:6px;"></span>Full Tax
                                                                    </asp:HyperLink>

                                                                    <asp:HyperLink ID="hyperlink2" class="btn btn-default btn-sm" Font-Bold="true" Font-Size="Large" NavigateUrl="~/Pages/TaxPDF.aspx" Target="_blank" runat="server" Style="margin-left: 955px; margin-bottom: 6px; margin-top: 5px; font-weight: 500; height: 35px; background-color: lightgray">
                                                                        <span class="fa" style="padding-right:6px;font-size:20px;font-weight:600;color:red">&#xf1c1;</span>Tax Pdf
                                                                    </asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="padding-left: 375px;" class="font">
                                                                    <asp:DetailsView ID="DetailsView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Both">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
                                                                        <EditRowStyle BackColor="#999999" />
                                                                        <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
                                                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                    </asp:DetailsView>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="font">
                                                                    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="Gnowrap" Style="white-space: nowrap">
                                                                    </asp:GridView>
                                                                    <%--<asp:DataList ID ="datalist1" runat="server" >                                              

                                                            <HeaderTemplate>

                                                                <table>

                                                                    <tr>
                                                                        <th style="width: 150px" align="left">Installment 1</th>

                                                                        <th style="width: 150px" align="left">Installment 2</th>

                                                                        <th style="width: 150px" align="left">Installment 3</th>

                                                                        <th style="width: 150px" align="left">Installment 4</th>
                                                                    </tr>

                                                                </table>

                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 150px" align="left">
                                                                            <asp:Label ID="lblbaseamount" Text='<%# DataBinder.Eval(Container.DataItem, "INST 1") %>' runat="server" /></td>

                                                                        <td style="width: 150px" align="left">
                                                                            <asp:Label ID="lblpaidamount" Text='<%# DataBinder.Eval(Container.DataItem, "INST 2") %>' runat="server" /></td>

                                                                        <td style="width: 150px" align="left">
                                                                            <asp:Label ID="lblstatus" Text='<%# DataBinder.Eval(Container.DataItem, "INST 3") %>' runat="server" /></td>

                                                                        <td style="width: 150px" align="left">
                                                                            <asp:Label ID="lblpaiddate" Text='<%# DataBinder.Eval(Container.DataItem, "INST 4") %>' runat="server" /></td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:DataList>--%>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="font">
                                                                    <asp:GridView ID="GridView2" runat="server" GridLines="None" CssClass="Gnowrap">
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <asp:GridView ID="GridView3" runat="server" CssClass="Gnowrap" GridLines="None">
                                                            </asp:GridView>
                                                        </table>
                                                    </div>
                                                    <div id="titlebtn" runat="server" visible="false" style="height: auto; width: 1260px;">
                                                        <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-info" Text="Get Titleflex Data" Width="154px" OnClick="Button1_Click" Style="margin-top: 6px; font-size: 14px; background-color: #07446f" />
                                                        <asp:Label ID="lblStatus" Font-Size="Larger" ForeColor="Red" runat="server" Text=""></asp:Label>
                                                    </div>
                                                    <div id="divtitle" runat="server" visible="false" style="overflow: scroll; height: auto; width: 1260px;">

                                                        <table>
                                                            <tr>
                                                                <td style="color: #171616; font-size: large; padding-top: 6px; text-decoration: underline" align="center"></td>
                                                            </tr>
                                                            <tr>

                                                                <td class="font">
                                                                    <asp:GridView ID="GridView4" runat="server" GridLines="None" CssClass="Gnowrapp" Style="white-space: nowrap; margin-bottom: 7px;">
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="GridView5" runat="server" GridLines="None" CssClass="Gnowrapp">
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>

                                        </table>
                                        <br />
                                        <table class="container" style="width: 1260px;">
                                            <tbody class="col-lg-12 well" style="width: 1238px; left: 13px;">
                                                <tr class="row" style="height: 35px;">
                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 33px;">Process Time:
                                                    </td>
                                                    <td class="Lblothers font" style="width: 150px; padding-left: 10px;">
                                                        <script type="text/javascript" language="javascript" src="../Script/TimerClock.js"></script>
                                                    </td>

                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 33px;">Order No:
                                                    </td>
                                                    <td style="width: 247px;" class="font">
                                                        <%--<asp:Label ID="Lblorderno" runat="server" Text="1234567" CssClass="lblorderno"></asp:Label>--%>
                                                        <asp:LinkButton ID="Lblorderno" runat="server" Text="1234567" CssClass="lblorderno"
                                                            Font-Underline="false" OnClick="Lblorderno_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>

                                                <tr class="row font" style="height: 35px;">
                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 33px;">State with TZ:
                                                    </td>
                                                    <td style="width: 247px; font-weight: 400" class="font">
                                                        <asp:Label ID="Lblstate" runat="server" Text="FL - EST" CssClass="Lbldata"></asp:Label>
                                                    </td>

                                                    <td class="form-group form-inline font" style="font-weight: 600; width: 129px;">State Comments:
                                                    </td>
                                                    <td style="width: 150px; padding-left: 6px; font-weight: 400" class="font">
                                                        <asp:Label ID="lblkeycomplete" runat="server" Text="" Font-Size="16px"
                                                            ForeColor="black"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr class="row" style="height: 35px;">

                                                    <td class="form-group form-inline font" style="font-weight: 600; width: 11%">Date:
                                                    </td>
                                                    <td style="width: 382px; font-weight: 400" class="font">
                                                        <asp:Label ID="Lbldate" runat="server" Text="05-Dec-2012" CssClass="Lbldata"></asp:Label>
                                                        <asp:LinkButton ID="LnkbtnFLcalc" runat="server" Text="FL Calc" OnClick="LnkbtnFLcalc_Click"></asp:LinkButton>
                                                    </td>

                                                    <td class="form-group form-inline font" style="padding-left: 0px; font-weight: 600">County:
                                                    </td>
                                                    <td style="width: 247px; font-weight: 400">
                                                        <asp:Label ID="Lblcouny" runat="server" Text="County" CssClass="Lbldata font" Style="font-weight: 400"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr class="row" style="height: 35px;">
                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 50px;">OrderType:
                                                    </td>
                                                    <td style="width: 200px; padding-left: 11px; padding-right: 0px;" class="col-md-4 form-group form-inline">
                                                        <asp:TextBox ID="txtordertype" runat="server" placeholder="Enter OrderType..." class="form-control Lbldata"
                                                            OnTextChanged="txtordertype_TextChanged" MaxLength="1" Style="height: 27px; width: 182px;" AutoPostBack="true"></asp:TextBox>
                                                        <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars"
                                                ValidChars="pPmMwW" TargetControlID="txtordertype" FilterType="Custom">
                                            </cc1:FilteredTextBoxExtender>--%>
                                                    </td>

                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 33px;">Followup Date: 
                                                    </td>
                                                    <td class="col-md-4 form-group form-inline" style="height: 20px; width: 247px; padding-left: 10px">
                                                        <asp:TextBox ID="txtfollowupdate" runat="server" class="form-control Lbldata" OnClick="javascript:NewCal('txtfollowupdate','ddmmmyyyy',true,24)" placeholder="Enter Followup Date..." Style="height: 27px"></asp:TextBox>
                                                    </td>


                                                </tr>

                                                <tr class="row" style="height: 35px;">
                                                    <td class="form-group form-inline font" style="font-weight: 600">Borrower:
                                                    </td>
                                                    <td style="height: 20px; width: 247px; padding-left: 10px;" class="col-md-4 form-group form-inline">
                                                        <asp:TextBox ID="txtBorrower" runat="server" placeholder="Enter Borrower..." class="form-control Lbldata" Style="height: 27px; width: 182px;"></asp:TextBox>
                                                    </td>

                                                    <td class="form-group form-inline font" style="font-weight: 600">Zip Code: 
                                                    </td>
                                                    <td class="col-md-4 form-group form-inline" style="padding-left: 10px;">
                                                        <asp:TextBox ID="txtzipcode" runat="server" class="form-control Lbldata" MaxLength="5" placeholder="Enter Zipcode..." Style="height: 32px;"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="txtzipcode">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>

                                                <tr class="row" style="height: 45px;">
                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 33px;">Status: 
                                                    </td>
                                                    <td class="col-md-4 form-group form-inline" style="padding-left: 10px;">
                                                        <asp:DropDownList ID="ddlstatus" runat="server" AutoPostBack="true" class="form-control Lbldata" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged" Style="width: 182px; height: 35px;">
                                                            <asp:ListItem>--Select Status--</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:LinkButton ID="Lnkcomments" runat="server" Text="GetComments"></asp:LinkButton>
                                                    </td>

                                                    <td class="form-group form-inline font" style="font-weight: 600">Township: 
                                                    </td>
                                                    <td class="col-md-4 form-group form-inline" style="width: 247px; padding-left: 10px;">
                                                        <asp:TextBox ID="txttownship" runat="server" class="form-control Lbldata" placeholder="Enter Township..." Style="height: 27px"></asp:TextBox>
                                                    </td>

                                                    <td style="width: 70px;">&nbsp;</td>


                                                </tr>
                                            </tbody>
                                        </table>

                                        <div class="panel-group" id="accordion1">
                                            <div class="panel panel-default" style="width: 1242px;">
                                                <div class="panel-heading" style="width: 1240px">
                                                    <h4 class="panel-title">
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne1">State Comments
                                                        </a>
                                                    </h4>
                                                </div>
                                                <div id="collapseOne1" class="panel-collapse collapse in">
                                                    <div class="panel-body">
                                                        <table class="container" style="margin-top: 13px;">
                                                            <tbody class="col-lg-12 well" style="width: 1207px;">
                                                                <tr>
                                                                    <td class="font form-group form-inline">
                                                                        <asp:TextBox ID="txtcommentshistory" runat="server" TextMode="MultiLine" Height="235px"
                                                                            Style="width: 1164px;" ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <table class="container" style="margin-top: 13px;">
                                                            <tbody class="col-lg-12 well" style="width: 1207px;">
                                                                <tr>
                                                                    <td class="font form-group form-inline">
                                                                        <asp:TextBox ID="txtexcomments" runat="server" TextMode="MultiLine" Height="70px"
                                                                            Width="1164px" ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--   StrMICX Production--%>
                                        <div class="panel-group" id="accordion2" runat="server">
                                            <div class="panel panel-default" style="width: 1242px;">
                                                <div class="panel-heading" style="width: 1240px">
                                                    <h4 class="panel-title">
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseOne2">Production
                                                        </a>
                                                    </h4>
                                                </div>
                                                <div style="height: auto;">
                                                    <asp:Label ID="lblErrorMessage" runat="server" Font-Names="Verdana" Font-Size="Small" ForeColor="Red" class="label label-primary label-bs" />
                                                </div>
                                                <div id="collapseOne2" class="panel-collapse collapse in">
                                                    <div class="panel-body">
                                                        <table class="container" style="margin-top: 13px;">
                                                            <tbody class="col-lg-12 well" style="width: 1207px;">
                                                                <tr>
                                                                    <td class="form-group form-inline font" style="font-weight: 600; width: 70px; white-space: nowrap">Tax ID
                                                                    </td>
                                                                    <td class="col-md-4 form-group form-inline" style="width: 216px; padding-left: 7px;">
                                                                        <asp:TextBox ID="txtstrParcelId" runat="server" class="form-control Lbldata" placeholder="Enter Tax Id" Style="height: 35px" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);" autocomplete='off'></asp:TextBox>
                                                                    </td>
                                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 33px;">Tax Type
                                                                    </td>
                                                                    <td class="col-md-4 form-group form-inline" style="padding-left: 7px; width: 236px;">
                                                                        <asp:DropDownList ID="txtstrTaxType" runat="server" class="form-control Lbldata" Style="width: 170px; height: 35px;">
                                                                        </asp:DropDownList>
                                                                    </td>

                                                                    <td class="form-group form-inline font" style="font-weight: 600; width: 79px; white-space: nowrap">Tax Year 
                                                                    </td>
                                                                    <td class="col-md-4 form-group form-inline" style="width: 201px; padding-left: 9px;">
                                                                        <asp:TextBox ID="txtstrTaxYear" runat="server" class="form-control Lbldata" MaxLength="4" placeholder="YYYY" Style="height: 35px" onkeypress="return isNumberKey(event)" onblur="checkReqFields(this.value,this,event);" autocomplete='off'></asp:TextBox>
                                                                        <%--        <asp:RegularExpressionValidator runat="server" ID="valNumbersOnly" ControlToValidate="txtstrTaxYear" Display="Dynamic" ErrorMessage="Please enter a numbers only." ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)">
                                                                        </asp:RegularExpressionValidator>--%>
                                                                    </td>

                                                                    <td class="form-group form-inline font" style="font-weight: 600; width: 85px; padding-left: 9px; white-space: nowrap">End Year 
                                                                    </td>
                                                                    <td class="col-md-4 form-group form-inline" style="width: 225px; padding-left: 10px;">
                                                                        <asp:TextBox ID="txtstrEndYear" runat="server" class="form-control Lbldata" placeholder="YYYY" MaxLength="4" Style="height: 35px" onkeypress="return isNumberKey(event);" onblur="checkReqFields1(this.value,this,event);" autocomplete='off'></asp:TextBox>
                                                                        <%-- <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtstrEndYear" Display="Dynamic" ErrorMessage="Please enter a numbers only" ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)">
                                                                        </asp:RegularExpressionValidator>--%>
                                                                    </td>

                                                                </tr>

                                                                <tr>
                                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 33px; padding-top: 25px; white-space: nowrap">Payment Frequency
                                                                    </td>

                                                                    <td class="col-md-4 form-group form-inline" style="padding-left: 5px; width: 236px;">
                                                                        <asp:DropDownList ID="txtstrPaymentFrequency" runat="server" name="txtstrPaymentFrequency" class="form-control Lbldata" Style="width: 170px; height: 35px; margin-top: 20px;" onchange="txtpaymentfrequency()">
                                                                            <asp:ListItem>Select</asp:ListItem>
                                                                            <asp:ListItem>Annual</asp:ListItem>
                                                                            <asp:ListItem>Semiannual</asp:ListItem>
                                                                            <asp:ListItem>Quarterly</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>

                                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 33px; padding-top: 25px; white-space: nowrap">Delinquent Status
                                                                    </td>

                                                                    <td>
                                                                        <asp:DropDownList ID="txtdeliquent" runat="server" class="form-control" Style="width: 168px; height: 35px; margin-bottom: -27px; margin-left: 9px;" onchange="txtdeliquentsta1()">
                                                                            <asp:ListItem>Select</asp:ListItem>
                                                                            <asp:ListItem>Yes</asp:ListItem>
                                                                            <asp:ListItem>No</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>

                                                                    <td class="form-group form-inline font" style="font-weight: 600; height: 33px; padding-top: 25px; white-space: nowrap; padding-left: 2px;">Special Assessment
                                                                    </td>

                                                                    <td>
                                                                        <asp:DropDownList ID="txtSpcelAsst" runat="server" class="form-control" Style="width: 171px; height: 35px; margin-bottom: -27px; margin-left: 9px;" onchange="txtSpcelAsststa1()">
                                                                            <asp:ListItem>Select</asp:ListItem>
                                                                            <asp:ListItem>Yes</asp:ListItem>
                                                                            <asp:ListItem>No</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>

                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <div class="panel-group" id="tblpayfrenq" runat="server" style="visibility: hidden; display: none">
                                                            <div class="panel panel-default" style="width: 1222px;">
                                                                <div class="panel-heading" style="width: 1221px">
                                                                    <h4 class="panel-title">
                                                                        <a>Payment Frequency Section
                                                                        </a>
                                                                    </h4>
                                                                </div>
                                                            </div>
                                                            <table id="tabletax1" runat="server" style="visibility: hidden; display: none">
                                                                <thead style="background-color: #d9241b; color: #fff;">
                                                                    <tr>
                                                                        <td>
                                                                            <br />
                                                                        </td>

                                                                    </tr>
                                                                    <tr>

                                                                        <th style="padding-left: 59px;" class="font"><font color="#000">Inst. Amount </font></th>
                                                                        <th style="padding-left: 60px;" class="font"><font color="#000">Inst. Amount Paid</font></th>
                                                                        <th style="padding-left: 90px;" class="font"><font color="#000">Inst. Paid/Due?</font></th>
                                                                        <th style="padding-left: 52px;" class="font"><font color="#000">Remaining Balance</font></th>
                                                                        <th style="padding-left: 44px;" class="font"><font color="#000">Discount Amount</font></th>
                                                                        <th style="padding-left: 80px;" class="font"><font color="#000">Exempt/Relevy?</font></th>

                                                                    </tr>

                                                                </thead>
                                                                <tbody>
                                                                    <tr>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrTaxAmount1" runat="server" class="form-control" placeholder="Tax Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 24px; width: 160px;" onkeyup="hello1();" onfocusout="myFunction1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytxtamount1()" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrAmountPaid1" runat="server" class="form-control" placeholder="Amount Paid" Style="height: 35px; border-spacing: 10px 0; margin-left: 30px; width: 160px;" onkeyup="AmtPaid1();" onfocusout="myFunctionAmtPaid1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytest1();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:DropDownList ID="txtstrTaxStatus1" runat="server" Style="height: 35px; border-spacing: 10px 0; margin-left: 60px; width: 160px;">
                                                                                <%-- <asp:ListItem>Select</asp:ListItem>--%>
                                                                                <asp:ListItem>Paid</asp:ListItem>
                                                                                <asp:ListItem>Due</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>

                                                                        <td>
                                                                            <asp:HiddenField ID="hdntxtbxTaksit1" runat="server" Value=""></asp:HiddenField>
                                                                            <asp:HiddenField ID="hdntxtbxTaksit2" runat="server" Value=""></asp:HiddenField>
                                                                            <asp:HiddenField ID="hdntxtbxTaksit3" runat="server" Value=""></asp:HiddenField>
                                                                            <asp:HiddenField ID="hdntxtbxTaksit4" runat="server" Value=""></asp:HiddenField>
                                                                            <asp:TextBox ID="txtstrRemaingBlnce1" runat="server" class="form-control" placeholder="Remaining Balance" Style="height: 35px; border-spacing: 10px 0; margin-left: 32px; width: 160px;" onkeypress="return isNumberKey(event)" onkeyup="RemBalance1(event);"></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrDiscountAmount1" runat="server" class="form-control" placeholder="Discount Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 23px; width: 160px;" onkeyup="Discount1();" onfocusout="myFunctionDiscount1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mydiscountamount1();" autocomplete='off'></asp:TextBox>

                                                                        </td>

                                                                        <td>
                                                                            <asp:DropDownList ID="txtstrExmpStatus1" runat="server" Style="height: 35px; border-spacing: 10px 0; margin-left: 60px; width: 160px;">
                                                                                <%--<asp:ListItem>Select</asp:ListItem>--%>
                                                                                <asp:ListItem>Yes</asp:ListItem>
                                                                                <asp:ListItem>No</asp:ListItem>
                                                                            </asp:DropDownList>
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
                                                                    </tr>

                                                                </tbody>

                                                            </table>

                                                            <table id="tabletax2" runat="server" style="visibility: hidden; display: none">
                                                                <thead style="background-color: #d9241b; color: #fff;">
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtstrTaxAmount2" runat="server" class="form-control" placeholder="Tax Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 24px; width: 160px;" onkeyup="hello2();" onfocusout="myFunction2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytxtamount2();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrAmountPaid2" runat="server" class="form-control" placeholder="Amount Paid" Style="height: 35px; border-spacing: 10px 0; margin-left: 30px; width: 160px;" onkeyup="AmtPaid2();" onfocusout="myFunctionAmtPaid2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytest2();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:DropDownList ID="txtstrTaxStatus2" runat="server" Style="height: 35px; border-spacing: 10px 0; margin-left: 60px; width: 160px;">
                                                                                <%-- <asp:ListItem>Select</asp:ListItem>--%>
                                                                                <asp:ListItem>Paid</asp:ListItem>
                                                                                <asp:ListItem>Due</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrRemaingBlnce2" runat="server" class="form-control" placeholder="Remaining Balance" Style="height: 35px; border-spacing: 10px 0; margin-left: 32px; width: 160px;" onkeypress="return isNumberKey(event)" onkeyup="RemBalance2();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrDiscountAmount2" runat="server" class="form-control" placeholder="Discount Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 23px; width: 160px;" onkeyup="Discount2();" onfocusout="myFunctionDiscount2();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mydiscountamount2();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:DropDownList ID="txtstrExmpStatus2" runat="server" Style="height: 35px; border-spacing: 10px 0; margin-left: 60px; width: 160px;">
                                                                                <%--  <asp:ListItem>Select</asp:ListItem>--%>
                                                                                <asp:ListItem>Yes</asp:ListItem>
                                                                                <asp:ListItem>No</asp:ListItem>
                                                                            </asp:DropDownList>
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
                                                                    </tr>
                                                                </tbody>

                                                            </table>

                                                            <table id="tabletax3" runat="server" style="visibility: hidden; display: none">
                                                                <thead style="background-color: #d9241b; color: #fff;">
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtstrTaxAmount3" runat="server" class="form-control" placeholder="Tax Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 24px; width: 160px;" onkeyup="hello3();" onfocusout="myFunction3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytxtamount3();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrAmountPaid3" runat="server" class="form-control" placeholder="Amount Paid" Style="height: 35px; border-spacing: 10px 0; margin-left: 30px; width: 160px;" onkeyup="AmtPaid3();" onfocusout="myFunctionAmtPaid3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytest3();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:DropDownList ID="txtstrTaxStatus3" runat="server" Style="height: 35px; border-spacing: 10px 0; margin-left: 60px; width: 160px;">
                                                                                <%--  <asp:ListItem>Select</asp:ListItem>--%>
                                                                                <asp:ListItem>Paid</asp:ListItem>
                                                                                <asp:ListItem>Due</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrRemaingBlnce3" runat="server" class="form-control" placeholder="Remaining Balance" Style="height: 35px; border-spacing: 10px 0; margin-left: 32px; width: 160px;" onkeypress="return isNumberKey(event)" onkeyup="RemBalance3();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrDiscountAmount3" runat="server" class="form-control" placeholder="Discount Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 23px; width: 160px;" onkeyup="Discount3();" onfocusout="myFunctionDiscount3();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mydiscountamount3()" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:DropDownList ID="txtstrExmpStatus3" runat="server" Style="height: 35px; border-spacing: 10px 0; margin-left: 60px; width: 160px;">
                                                                                <%--  <asp:ListItem>Select</asp:ListItem>--%>
                                                                                <asp:ListItem>Yes</asp:ListItem>
                                                                                <asp:ListItem>No</asp:ListItem>
                                                                            </asp:DropDownList>
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
                                                                    </tr>
                                                                </tbody>
                                                            </table>

                                                            <table id="tabletax4" runat="server" style="visibility: hidden; display: none">
                                                                <thead style="background-color: #d9241b; color: #fff;">
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtstrTaxAmount4" runat="server" class="form-control" placeholder="Tax Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 24px; width: 160px;" onkeyup="hello4();" onfocusout="myFunction4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytxtamount4();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrAmountPaid4" runat="server" class="form-control" placeholder="Amount Paid" Style="height: 35px; border-spacing: 10px 0; margin-left: 30px; width: 160px;" onkeyup="AmtPaid4();" onfocusout="myFunctionAmtPaid4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mytest4();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:DropDownList ID="txtstrTaxStatus4" runat="server" Style="height: 35px; border-spacing: 10px 0; margin-left: 60px; width: 160px;">
                                                                                <%-- <asp:ListItem>Select</asp:ListItem>--%>
                                                                                <asp:ListItem>Paid</asp:ListItem>
                                                                                <asp:ListItem>Due</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrRemaingBlnce4" runat="server" class="form-control" placeholder="Remaining Balance" Style="height: 35px; border-spacing: 10px 0; margin-left: 32px; width: 160px;" onkeypress="return isNumberKey(event)" onkeyup="RemBalance4();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtstrDiscountAmount4" runat="server" class="form-control" placeholder="Discount Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 23px; width: 160px;" onkeyup="Discount4();" onfocusout="myFunctionDiscount4();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="mydiscountamount4();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:DropDownList ID="txtstrExmpStatus4" runat="server" Style="height: 35px; border-spacing: 10px 0; margin-left: 60px; width: 160px;">
                                                                                <%-- <asp:ListItem>Select</asp:ListItem>--%>
                                                                                <asp:ListItem>Yes</asp:ListItem>
                                                                                <asp:ListItem>No</asp:ListItem>
                                                                            </asp:DropDownList>
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
                                                                    </tr>
                                                                </tbody>

                                                            </table>
                                                        </div>
                                                        <br />
                                                        <div id="tblTaxBill" runat="server" style="visibility: hidden; display: none">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td class="form-group form-inline font" style="font-weight: 600; height: 33px;">Tax Bill
                                                                        </td>
                                                                        <td class="col-md-4 form-group form-inline" style="padding-left: 10px; width: 236px;">
                                                                            <asp:DropDownList runat="server" ID="txtTaxBill" class="form-control Lbldata" Style="width: 182px; height: 35px;">
                                                                                <asp:ListItem>Select Tax Bill</asp:ListItem>
                                                                                <asp:ListItem>Current</asp:ListItem>
                                                                                <asp:ListItem>Previous</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <br />
                                                        <div id="lblMesg" runat="server" class="btn-default" style="font-weight: bold; color: red;"></div>

                                                        <div class="panel-group" id="tbldelistatus123" runat="server" style="visibility: hidden; display: none;">
                                                            <div class="panel panel-default" style="width: 1222px;">
                                                                <div class="panel-heading" style="width: 1221px">
                                                                    <h4 class="panel-title">
                                                                        <a>Delinquent Status Section
                                                                        </a>
                                                                    </h4>
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <table id="tbldeliquentstatus" runat="server" cellspacing="1" cellpadding="1" style="visibility: hidden; display: none;">
                                                                <thead style="background-color: #d9241b; color: #fff;">
                                                                    <tr>
                                                                        <th style="padding-left: 5px;" class="font"><font color="#000">Delinquent Tax Year</font></th>
                                                                        <th style="padding-left: 25px;" class="font"><font color="#000">Payoff Amount</font></th>
                                                                        <th style="padding-left: 21px;" class="font"><font color="#000">Payoff Good Thru Date</font></th>
                                                                        <th style="padding-left: 15px;" class="font"><font color="#000">Initial Installment Due Date</font></th>
                                                                        <th style="padding-left: 8px;" class="font"><font color="#000">TaxSale-Not Applicable</font></th>
                                                                        <th style="padding-left: 23px;" class="font"><font color="#000">Date of Tax Sale</font></th>
                                                                        <th style="padding-left: 21px;" class="font"><font color="#000">Last Day To Redeem</font></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr style="padding: 25px;">
                                                                        <td>
                                                                            <asp:TextBox ID="txtdelitaxstatus" runat="server" class="form-control" placeholder="YYYY" MaxLength="4" onkeypress="return isNumberKey(event)" Style="height: 35px; border-spacing: 10px 0; width: 138px;" onblur="IsValidLengthTax3(this.value,this,event);" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtpayoffamount" runat="server" class="form-control" placeholder="Payoff Amount" onkeyup="PayAmount1();" onfocusout="myFunctionPayAmount1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" Style="height: 35px; border-spacing: 10px 0; margin-left: 14px; width: 160px;" onblur="mypay();" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtpayoffgood" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" Style="height: 35px; border-spacing: 10px 0; margin-left: 16px; width: 160px;" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtinitialinstall" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" Style="height: 35px; border-spacing: 10px 0; margin-left: 20px; width: 165px;" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:DropDownList ID="txtnotapplicable" runat="server" class="form-control" Style="width: 145px; height: 35px; margin-left: 10px;" onchange="applicable()">
                                                                                <asp:ListItem>Select</asp:ListItem>
                                                                                <asp:ListItem>Yes</asp:ListItem>
                                                                                <asp:ListItem>No</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <%-- <td>
                                                                           <asp:TextBox ID="txtpayoffamount1" runat="server" value="0.00" class="form-control" placeholder="Payoff Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 14px; width: 160px;"></asp:TextBox></td>--%>

                                                                        <td>
                                                                            <asp:TextBox ID="txtdatetaxsale" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" Style="height: 35px; border-spacing: 10px 0; margin-left: 14px; width: 160px;" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtlastdayred" runat="server" class="form-control" placeholder="MM/DD/YYYY" MaxLength="10" onkeyup="ValidateDate(this, event.keyCode)" onkeydown="return DateFormat(this, event.keyCode)" onblur="return checkDate(event)" Style="height: 35px; border-spacing: 10px 0; margin-left: 13px; width: 160px;" autocomplete='off'></asp:TextBox>
                                                                        </td>

                                                                        <%--<td>
                                                                            <asp:Button runat="server" ID="MultiDeliquent" Text="➕" class="btn btn-sm btn-success" OnClick="MultiDeliquent_Click" Style="margin-left: 5px;" OnClientClick="return validate();" />
                                                                        </td>--%>
                                                                        <td hidden>
                                                                            <p id="o"></p>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>


                                                            <%-- <asp:Panel runat="server" ID="pnlMultiDel" Width="100%" BackColor="#d8d8d7" ScrollBars="Auto" Style="margin-top: -2px;">
                                                                <asp:GridView ID="MultiDel" runat="server" GridLines="None" CssClass="Gnowrap"
                                                                    AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" OnRowEditing="MultiDel_RowEditing" OnRowDeleting="MultiDel_RowDeleting" OnRowCommand="MultiDel_RowCommand"
                                                                    Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Sl.No">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="DelinquentTaxYear" HeaderText="Delinquent Tax Year" ReadOnly="true" />
                                                                        <asp:BoundField DataField="PayoffAmount" HeaderText="Payoff Amount" ReadOnly="true" />
                                                                        <asp:BoundField DataField="PayoffGoodThruDate" HeaderText="Payoff Good Thru Date" ReadOnly="true" />
                                                                        <asp:BoundField DataField="InitialInstallmentDueDate" HeaderText="Initial Installment Due Date" ReadOnly="true" />
                                                                        <asp:BoundField DataField="NotApplicable" HeaderText="TaxSale-Not Applicable" ReadOnly="true" />
                                                                        <asp:BoundField DataField="DateofTaxSale" HeaderText="Date of Tax Sale" ReadOnly="true" />
                                                                        <asp:BoundField DataField="LastDayToRedeem" HeaderText="Last Day To Redeem" ReadOnly="true" />

                                                                        <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="MultiDelEdit" runat="server" ToolTip="Edit" class="glyphicon glyphicon-edit" Text=""
                                                                                    OnClientClick="javascript : return confirm('Are you sure, want to edit this Row?');"
                                                                                    CommandName="Edit"></asp:LinkButton>
                                                                                <asp:LinkButton ID="MultiDelCancel" Text="" ToolTip="Cancel" runat="server" class="glyphicon glyphicon-remove" CommandName="Cancel" Visible="false" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="MultiDelDelete" runat="server" ToolTip="Delete" class="glyphicon glyphicon-trash"
                                                                                    OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');"
                                                                                    CommandName="Delete"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>--%>
                                                        </div>
                                                        <br />

                                                        <div class="panel-group" id="tblspecialassess123" runat="server" style="visibility: hidden; display: none;">
                                                            <div class="panel panel-default" style="width: 1222px;">
                                                                <div class="panel-heading" style="width: 1221px">
                                                                    <h4 class="panel-title">
                                                                        <a>Special Assessment Section
                                                                        </a>
                                                                    </h4>
                                                                </div>
                                                            </div>
                                                            <br />

                                                            <table id="tblspecialassessment" runat="server" cellspacing="1" cellpadding="1" style="visibility: hidden; display: none; margin-left: 175px;">
                                                                <thead style="background-color: #d9241b; color: #fff;">
                                                                    <tr>
                                                                        <th style="padding-left: 20px" class="font"><font color="#000">Description</font></th>
                                                                        <th style="padding-left: 22px;" class="font"><font color="#000">Special Assessment No</font></th>
                                                                        <th style="padding-left: 21px;" class="font"><font color="#000">Number Of Installments</font></th>
                                                                        <th style="padding-left: 30px;" class="font"><font color="#000">Installment Paid</font></th>
                                                                        <th style="padding-left: 50px;" class="font"><font color="#000">Amount</font></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr style="padding: 25px;">
                                                                        <td>
                                                                            <asp:TextBox ID="txtdescription" runat="server" class="form-control" placeholder="Description" Style="height: 35px; border-spacing: 10px 0; margin-left: -10px; width: 160px;" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);" autocomplete='off'></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtspecialassno" runat="server" class="form-control" placeholder="Special AssessNo" Style="height: 35px; border-spacing: 10px 0; margin-left: 14px; width: 160px;" onkeyup="CheckFirstChar(event.keyCode, this);" onkeydown="return CheckFirstChar(event.keyCode, this);" autocomplete='off'></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtnoinstall" runat="server" class="form-control" placeholder="No.Installments" Style="height: 35px; border-spacing: 10px 0; margin-left: 16px; width: 160px;" onkeypress="return isNumberKey(event)" autocomplete='off'></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtinstallpaid" runat="server" class="form-control" placeholder="Installment Paid" Style="height: 35px; border-spacing: 10px 0; margin-left: 15px; width: 160px;" onkeypress="return isNumberKey(event)" autocomplete='off'></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtamountspecial" runat="server" class="form-control" placeholder="Amount" Style="height: 35px; border-spacing: 10px 0; margin-left: 15px; width: 160px;" onkeyup="Amount1();" onfocusout="myFunctionAmount1();if (this.value=='0.00') this.value='0.00';if (this.value=='') this.value='0.00';" onfocus="this.value='0.00'" onfocusin="if (this.value=='0.00') this.value='';" onblur="myamountspecial();" autocomplete='off'></asp:TextBox>

                                                                        </td>
                                                                        <%-- <td>
                                                                            <asp:Button runat="server" ID="btnSpecialAssesment" Text="➕" class="btn btn-sm btn-success" OnClick="MultiSpecial_Click" Style="margin-left: 5px;" OnClientClick="return validate1();" />
                                                                        </td>--%>
                                                                        <td hidden>
                                                                            <p id="s"></p>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                            <br />
                                                            <%-- <asp:Panel runat="server" ID="PanelSpecial" Width="100%" BackColor="#d8d8d7" ScrollBars="Auto" Style="margin-top: -2px;">
                                                                <asp:GridView ID="MultiSpecial" runat="server" GridLines="None" CssClass="Gnowrap"
                                                                    AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" OnRowEditing="MultiSpecial_RowEditing"
                                                                    OnRowDeleting="MultiSpecial_RowDeleting" OnRowCommand="MultiSpecial_RowCommand"
                                                                    Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Sl.No">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="true" />
                                                                        <asp:BoundField DataField="Special Assessment No" HeaderText="Special Assessment No" ReadOnly="true" />
                                                                        <asp:BoundField DataField="Number Of Installments" HeaderText="Number Of Installments" ReadOnly="true" />
                                                                        <asp:BoundField DataField="Installment Paid" HeaderText="Installment Paid" ReadOnly="true" />
                                                                        <asp:BoundField DataField="Amount" HeaderText="Amount" ReadOnly="true" />

                                                                        <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="MultiSpecialEdit" runat="server" ToolTip="Edit" class="glyphicon glyphicon-edit" Text=""
                                                                                    OnClientClick="javascript : return confirm('Are you sure, want to edit this Row?');"
                                                                                    CommandName="Edit"></asp:LinkButton>
                                                                                <asp:LinkButton ID="MultiSpecialCancel" Text="" ToolTip="Cancel" runat="server" class="glyphicon glyphicon-remove" CommandName="Cancel" Visible="false" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="MultiSpecialDelete" runat="server" ToolTip="Delete" class="glyphicon glyphicon-trash"
                                                                                    OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');"
                                                                                    CommandName="Delete"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                            --%>
                                                        </div>

                                                        <br />


                                                        <asp:Button ID="btnadd" runat="server" Text="Add" class="btn btn-sm btn-success" Style="margin-top: -50px;" Width="50px" OnClick="btnadd_Click" />
                                                        <br />
                                                        <br />


                                                        <asp:Panel runat="server" ID="pnlNested" Width="100%" BackColor="#d8d8d7" ScrollBars="Auto" Style="margin-top: -2px;">
                                                            <asp:GridView ID="gvtaxdetails" runat="server" GridLines="None" CssClass="Gnowrap"
                                                                AutoGenerateColumns="false" OnRowEditing="gvtaxdetails_RowEditing" OnRowUpdating="gvtaxdetails_RowUpdating"
                                                                OnRowCommand="gvtaxdetails_RowCommand" OnRowDeleting="gvtaxdetails_RowDeleting" EmptyDataText="No Data Found" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                Width="100%" OnRowCancelingEdit="gvtaxdetails_RowCancelingEdit">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sl.No">
                                                                        <ItemTemplate>
                                                                            <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="ParcelId" HeaderText="Tax Id" ReadOnly="true" />
                                                                    <asp:BoundField DataField="TaxType" HeaderText="Tax Type" ReadOnly="true" />
                                                                    <asp:BoundField DataField="TaxYear" HeaderText="Tax Year" ReadOnly="true" />
                                                                    <asp:BoundField DataField="EndYear" HeaderText="End Year" ReadOnly="true" />
                                                                    <asp:TemplateField HeaderText="Edit">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LnkEdit" runat="server" ToolTip="Edit" class="glyphicon glyphicon-edit" Text="" CommandArgument='<%# Eval("searchkey_ID") %>'
                                                                                OnClientClick="javascript : return confirm('Are you sure, want to edit this Row?');"
                                                                                CommandName="Edit"></asp:LinkButton>
                                                                            <asp:LinkButton ID="LnkCancel" Text="" ToolTip="Cancel" runat="server" class="glyphicon glyphicon-remove" CommandName="Cancel" Visible="false" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LnkDelete" runat="server" ToolTip="Delete" class="glyphicon glyphicon-trash" CommandArgument='<%# Eval("searchkey_ID") %>'
                                                                                OnClientClick="javascript : return confirm('Are you sure, want to delete this Row?');"
                                                                                CommandName="Delete"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>

                                                    </div>



                                                </div>
                                            </div>
                                            <%--   StrMICX Production--%>
                                        </div>

                                        <%-- Keying QC --%>
                                        <div class="panel-group" id="accordion3" runat="server">
                                            <div class="panel panel-default" style="width: 1242px;">
                                                <div class="panel-heading" style="width: 1240px">
                                                    <h4 class="panel-title">
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion3" href="#collapseOne3">Compare Keying Data
                                                        </a>
                                                    </h4>
                                                </div>
                                                <div id="collapseOne3" class="panel-collapse collapse in">
                                                    <div class="panel-body">
                                                        <table class="container" style="margin-top: 13px;">
                                                            <tbody class="col-lg-12 well" style="width: 1207px;">
                                                                <tr>


                                                                    <td>
                                                                        <asp:FileUpload ID="FileUpload1" runat="server" Style="margin-left: 315px;" onchange="pressed()" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button runat="server" ID="cancel" Text="Remove" OnClick="cancel_Click" />

                                                                        <%--  <asp:Button ID="btnUp123" runat="server" Text="Upload" Style="margin-left: -92px;"
                                                                            OnClick="btnUploadss_Click" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button runat="server" ID="Compatebtn" Text="Comparing with KeyingQC Data" Style="width: 190px; margin-left: 40px;" class="btn btn-sm btn-success" OnClick="Compatebtn_Click" />
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                                <asp:Label ID="LblUpload" runat="server" Text=" " />
                                                            </tbody>
                                                        </table>

                                                        <!-- Modal Popup -->

                                                        <div class="Mismatch_Popup" id="MismatchPopup" runat="server" align="center">
                                                            <div>

                                                                <table>
                                                                    <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                                                        <td align="center" style="height: 25px; width: 795px; font-weight: bold;" colspan="2">Comparison Report
                                                                        </td>
                                                                        <td align="right" colspan="2">
                                                                            <asp:Button ID="btnMismatchclose" runat="server" Width="70px" Text="❌" CssClass="btn btn-sm" OnClick="CloseButton_Click" Style="background: url(images/templatemo_menu_repeat.jpg); color: white; font-size: 14px; width: 27px; padding-left: 0px;" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <table style="width: 790px;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="mismatchLabel"></asp:Label>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td class="form-group form-inline font" style="font-weight: 600; width: 79px; white-space: nowrap; padding-left: 75px;">Order No: 
                                                                        </td>
                                                                        <td class="form-group form-inline">
                                                                            <asp:Label runat="server" ID="PopOrderno" style="padding-left:6px;"></asp:Label>
                                                                        </td>

                                                                        <%--<td class="form-group form-inline font" style="font-weight: 600; width: 79px; white-space: nowrap; padding-left: 32px;">Tax Id: 
                                                                        </td>
                                                                        <td class="form-group form-inline" style="width: 130px;">
                                                                            <asp:Label runat="server" ID="PopTaxId"></asp:Label>
                                                                        </td>

                                                                        <td class="form-group form-inline font" style="font-weight: 600; width: 79px; white-space: nowrap; padding-left: 36px;">Tax Type: 
                                                                        </td>
                                                                        <td class="form-group form-inline" style="width: 160px;">
                                                                            <asp:Label runat="server" ID="PopTaxType" style="padding-left:5px;"></asp:Label>
                                                                        </td>--%>

                                                                        <td class="form-group form-inline font" style="font-weight: 600; width: 70px; white-space: nowrap">No Of Mismatches: 
                                                                        </td>
                                                                        <td class="form-group form-inline" style="padding-left: 10px;">
                                                                            <asp:Label runat="server" ID="PopMismatch"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <br />
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="GridView_Mismatch" runat="server" AutoGenerateColumns="false" Style="width: 775px; white-space: nowrap;" class="font1">
                                                                            <Columns>
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="Parcel_id" HeaderText="Tax Id"></asp:BoundField>
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="TaxType" HeaderText="Tax Type" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="Field_id" HeaderText="Field Label" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="Mismatch_Keying_Data" HeaderText="Keying Data" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="Mismatch_ClientTool_Data" HeaderText="Client Tool Data" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                    </div>




                    <div class="panel-group" id="accordion">
                        <div class="panel panel-default" style="width: 1242px">
                            <div class="panel-heading" style="width: 1240px">
                                <h4 class="panel-title">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">County/Escrow
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <table class="container">
                                        <tbody class="col-lg-12 well" style="width: 1206px;">
                                            <tr>
                                                <td class="form-group form-inline font" style="font-weight: 600; height: 33px; padding-left: 53px;">InProcess Attempts:</td>

                                                <td style="width: 247px">
                                                    <asp:Label ID="lblInprocnt" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Red" Visible="False"></asp:Label>
                                                </td>
                                                <td class="Lblothers">&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td style="width: 247px; height: 62px;">&nbsp;</td>
                                            </tr>

                                            <tr>
                                                <td class="form-group form-inline font" style="font-weight: 600; height: 33px; padding-left: 53px;">Email Type: 
                                                </td>
                                                <td class="col-md-4 form-group form-inline">
                                                    <asp:DropDownList ID="ddlemailtype" runat="server" class="form-control" Style="width: 215px;">
                                                        <asp:ListItem>--Select Type--</asp:ListItem>
                                                        <asp:ListItem>County</asp:ListItem>
                                                        <asp:ListItem>Escrow</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="form-group form-inline font" style="font-weight: 600; height: 33px;">County Name:
                                                </td>
                                                <td class="col-md-4 form-group form-inline" style="padding-left: 10px; width: 180px;">
                                                    <asp:TextBox ID="txtcountyname" runat="server" class="form-control" placeholder="Enter County Name..." Style="width: 200px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsendmail" runat="server" CssClass="btn" OnClick="btnsendmail_Click" Text="Send Email" Style="background: gray; color: white" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>



                    <asp:Panel ID="PanelCompleted" runat="server" Width="98%" BackColor="#f5f5f5" BorderColor="Gray"
                        BorderStyle="Ridge" BorderWidth="1px">
                        <table width="100%">
                            <tr style="height: 35px; font-weight: 600;" class="font">
                                <td colspan="0.5">
                                    <asp:Label ID="lblParcelFormat" runat="server" Text="Parcel ID Format"></asp:Label>
                                </td>
                                <td colspan="0.5" style="margin-left: 40px">
                                    <asp:TextBox ID="txtParcelFormat" runat="server" onkeypress="checkNum"
                                        MaxLength="1" Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <td colspan="0.5">
                                    <asp:Label ID="lblimprovements" runat="server" Text="Improvements"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtimprovements" runat="server" MaxLength="1" Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <td colspan="0.5">
                                    <asp:Label ID="lblFireDistricttax" runat="server" Text="Fire District tax"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtFireDistricttax" runat="server" MaxLength="1" Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <td colspan="0.5">
                                    <asp:Label ID="lblDelinquenttax" runat="server" Text="Delinquent tax"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtDelinquenttax" runat="server" MaxLength="1" Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 35px; font-weight: 600;" class="font">
                                <td colspan="0.5">
                                    <asp:Label ID="lblSupplementalbills" runat="server" Text="Supplemental bills"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtSupplementalbills" runat="server" MaxLength="1"
                                        Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <td colspan="0.5">
                                    <asp:Label ID="lblExemption" runat="server" Text="Exemption"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtExemption" runat="server" MaxLength="1" Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <td colspan="0.5">
                                    <asp:Label ID="lblInstallmentDate" runat="server" Text="Installment Date"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtInstallmentDate" runat="server" MaxLength="1" Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <td colspan="0.5">
                                    <asp:Label ID="lblNextBilldate" runat="server" Text="Next Bill date"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtNextBilldate" runat="server" MaxLength="1" Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 35px; font-weight: 600;" class="font">
                                <td colspan="0.5">
                                    <asp:Label ID="lblTaxAuthority" runat="server" Text="Tax Authority"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtTaxAuthority" runat="server" MaxLength="1" Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <td colspan="0.5">
                                    <asp:Label ID="lblNoofparcelconfirmation" runat="server" Text="No of parcel confirmation"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtNoofparcelconfirmation" runat="server" MaxLength="1"
                                        Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <td colspan="0.5">
                                    <asp:Label ID="lblPaymentstatus" runat="server" Text="Payment status"></asp:Label>
                                </td>
                                <td colspan="0.5">
                                    <asp:TextBox ID="txtPaymentstatus" runat="server" MaxLength="1" Width="28px" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PanelInproHold" runat="server" Width="98%" BackColor="#f5f5f5" BorderColor="Gray"
                        BorderStyle="Ridge" BorderWidth="1px">
                        <table width="100%">
                            <tr style="height: 35px" class="font">
                                <td colspan="1" style="width: 290px;">
                                    <asp:CheckBox ID="chktranstype1" runat="server" Text="Transaction Type" class="font" Style="padding-left: 10px;" />
                                </td>
                                <td colspan="1">
                                    <asp:CheckBox ID="chkorderdoc1" runat="server" Text="Order documents(Legal,Abstract)" class="font" Style="padding-left: 10px;" />
                                </td>
                                <td colspan="1">
                                    <asp:CheckBox ID="chkparcelid1" runat="server" Text="Parcel ID formats" class="font" Style="padding-left: 10px;" />
                                </td>
                                <td colspan="1">
                                    <asp:CheckBox ID="chkcomments1" runat="server" Text="Comments" class="font" Style="padding-left: 10px;" />
                                </td>
                            </tr>
                            <tr style="height: 35px" class="font">
                                <td class="Lblothers" style="height: 14px">
                                    <asp:Label ID="lblmultiple1" runat="server" Text="Multiple parcels" class="font"></asp:Label>
                                </td>
                                <td style="height: 14px">
                                    <asp:DropDownList ID="ddlmultiple1" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="Lblothers" style="height: 14px">
                                    <asp:Label ID="lblcorrect1" runat="server" Text="Correct authorities" class="font"></asp:Label>
                                </td>
                                <td style="height: 14px">
                                    <asp:DropDownList ID="ddlcorrect1" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="Lblothers" style="height: 14px">
                                    <asp:Label ID="lblfollowupdate1" runat="server" Text="Follow up date" class="font"></asp:Label>
                                </td>
                                <td style="height: 14px">
                                    <asp:DropDownList ID="ddlfollowupdate1" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="Lblothers" style="height: 14px">
                                    <asp:Label ID="lbletadate1" runat="server" Text="ETA Date" class="font"></asp:Label>
                                </td>
                                <td style="height: 14px">
                                    <asp:DropDownList ID="ddletadate1" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 35px" class="font">
                                <td class="Lblothers">
                                    <asp:Label ID="lblfollow1" runat="server" Text="Follow the Miscellaneous info notes" class="font"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlfollow1" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PanelMailaway" runat="server" Width="98%" Height="89px" BackColor="#f5f5f5" BorderColor="Gray"
                        BorderStyle="Ridge" BorderWidth="1px">
                        <table width="100%">
                            <tr style="height: 25px">
                                <td colspan="2" class="Lblothers">
                                    <asp:CheckBox ID="chktranstype2" runat="server" Text="Transaction Type" />
                                </td>
                                <td colspan="2" class="Lblothers">
                                    <asp:CheckBox ID="chkorderdoc2" runat="server" Text="Order documents(Legal,Abstract)" />
                                </td>
                                <td colspan="2" class="Lblothers">
                                    <asp:CheckBox ID="chkparcelid2" runat="server" Text="Parcel ID formats" />
                                </td>
                                <td colspan="2" class="Lblothers">
                                    <asp:CheckBox ID="chkreqtype2" runat="server" Text="Request Type" />
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td colspan="2" class="Lblothers">
                                    <asp:CheckBox ID="chkcomments2" runat="server" Text="Comments" />
                                </td>
                                <td class="Lblothers">
                                    <asp:Label ID="lblmultiple2" runat="server" Text="Multiple parcels"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlmultiple2" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="Lblothers">
                                    <asp:Label ID="lblcorrect2" runat="server" Text="Correct authorities"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcorrect2" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="Lblothers">
                                    <asp:Label ID="lblfollowupdate2" runat="server" Text="Follow up date"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlfollowupdate2" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td class="Lblothers">
                                    <asp:Label ID="lblfollow2" runat="server" Text="Follow the Miscellaneous info notes"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlfollow2" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="Lblothers">
                                    <asp:Label ID="lbletadate2" runat="server" Text="ETA Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddletadate2" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PanelParcelID" runat="server" Width="98%" BackColor="#f5f5f5" BorderColor="Gray"
                        BorderStyle="Ridge" BorderWidth="1px" Style="height: 47px;" CssClass="font">
                        <table width="100%" style="margin-top: 7px;">
                            <tr style="height: 25px">
                                <td>
                                    <asp:CheckBox ID="chktranstype3" runat="server" Text="Transaction Type" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkorderdoc3" runat="server" Text="Order documents(Legal,Abstract)" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkcomments3" runat="server" Text="Comments" />
                                </td>
                                <td class="Lblothers">
                                    <asp:Label ID="lblfollowupdate3" runat="server" Text="Follow up date"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlfollowupdate3" CssClass="txtuser" runat="server" Width="85px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>YES</asp:ListItem>
                                        <asp:ListItem>NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PanelRejected" runat="server" Width="35%" BackColor="#f5f5f5" BorderColor="Gray"
                        BorderStyle="Ridge" BorderWidth="1px" Style="height: 47px;" CssClass="font">
                        <table width="100%" style="margin-top: 7px;">
                            <tr style="height: 25px">
                                <td>
                                    <asp:CheckBox ID="chktranstype4" runat="server" Text="Transaction Type" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkcomments4" runat="server" Text="Comments" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>


                    <table class="container" style="margin-top: 13px;">
                        <tbody class="col-lg-12 well" style="width: 1240px; height: auto;">
                            <tr>
                                <td class="font" style="width: 37px; font-weight: 600;">History :
                                </td>
                                <td class="font">
                                    <asp:TextBox ID="txtcmdhistory" runat="server" TextMode="MultiLine" Height="60px"
                                        Style="width: 1070px;" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 70px;">
                                <td class="font" style="width: 37px; font-weight: 600;">Misc :
                                </td>
                                <td class="col-md-4 form-group form-inline" style="width: 247px; padding-left: 0px;">
                                    <asp:TextBox ID="txtmisc" runat="server" class="form-control Lbldata" placeholder="Enter Misc..." Style="height: 35px; width: 1070px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="font">
                                    <asp:Panel ID="PanelQc" runat="server" BackColor="#f5f5f5" BorderColor="Gray" BorderStyle="Ridge"
                                        BorderWidth="1px" Width="100%" Style="margin-top: 10px;">
                                        <table style="height: 50px; width: 100%;" class="font">
                                            <tr>
                                                <td class="Lblothers" style="width: 75px;">Error :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlerror" CssClass="txtuser" runat="server" AutoPostBack="true"
                                                        Width="85px" OnSelectedIndexChanged="ddlerror_SelectedIndexChanged">
                                                        <asp:ListItem></asp:ListItem>
                                                        <asp:ListItem>No Error</asp:ListItem>
                                                        <asp:ListItem>Error</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="font" style="width: 200px;">Error Category :
                                                </td>
                                                <td class="font">
                                                    <asp:DropDownList ID="ddlerrorcat" CssClass="txtuser" runat="server" Width="180px"
                                                        OnSelectedIndexChanged="ddlerrorcat_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="font" style="width: 150px;">Error Area :
                                                </td>
                                                <td class="font">
                                                    <asp:DropDownList ID="ddlerrorarea" CssClass="txtuser" runat="server" Width="227px"
                                                        OnSelectedIndexChanged="ddlerrorarea_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="font" style="width: 87px; padding-left: 10px;">Error Type :
                                                </td>
                                                <td class="font">
                                                    <asp:DropDownList ID="ddlerrortype" CssClass="txtuser" runat="server" Width="180px"
                                                        OnSelectedIndexChanged="ddlerrortype_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="font" style="width: 125px;">Combined :
                                                </td>
                                                <td colspan="3" align="left" class="font">
                                                    <asp:DropDownList ID="ddlcombined" CssClass="txtuser" runat="server" Width="617px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="center" valign="middle" class="font">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="font" style="height: 35px; width: 111px;">
                                    <asp:Label ID="lblqcerrorcmts" runat="server" Text="Error Comments  :"></asp:Label>
                                </td>
                                <td class="col-md-4 form-group form-inline font">
                                    <asp:TextBox ID="txtqcerrorcmts" runat="server" class="form-control txtuser" Height="35px" placeholder="Enter Error Comments..."
                                        Width="1058px"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <table class="container">
                        <tbody class="col-lg-12 well" style="width: 1240px;">
                            <tr>
                                <td class="form-group form-inline font" style="font-weight: 600; height: 33px; width: 100px;">Comments:
                                </td>
                                <td class="col-md-4 form-group form-inline font" style="display: inline;">
                                    <%--<asp:TextBox ID="txtcomments" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox>--%>
                                    <asp:Label ID="lblprdcomments" runat="server" Text="" Width="800px" Height="20px"
                                        Font-Size="Large"></asp:Label>
                                    <asp:DropDownList ID="ddlprdcomments" class="form-control" runat="server" Width="900px"
                                        Height="35px" OnSelectedIndexChanged="ddlprdcomments_SelectedIndexChanged" AutoPostBack="true" Style="margin-bottom: 6px;">
                                    </asp:DropDownList>
                                    <asp:Button ID="chktaxinfo" runat="server" CssClass="btn" Text="Send Tax email" OnClick="chktaxinfo_Click" AutoPostBack="true" Style="background: grey; color: white" />
                                    <%--<asp:CheckBox ID="chktaxinfo" runat="server" CssClass="font" Text="Send Tax email" OnCheckedChanged="chktaxinfo_CheckedChanged" AutoPostBack="true" style="padding-left:6px;"/>--%>
                                                        &nbsp;<asp:TextBox ID="txtaddcomments" runat="server" class="form-control" Height="35px" placeholder="Adding Comments..." Width="900px"></asp:TextBox>
                                    <asp:Button ID="btnaddcomments" runat="server" Text="Add" CssClass="btn" OnClick="btnaddcomments_Click" Style="background: grey; color: white" />
                                    <br />
                                    <br />
                                </td>

                            </tr>

                            <tr>
                                <td colspan="5" align="center" style="height: 35px;">
                                    <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-sm btn-success" OnClick="btnsave_Click" Width="50px" />
                                    <asp:Button ID="btnMovecall" runat="server" Text="Move To Call" Width="120px" class="btn btn-sm"
                                        OnClick="btnMovecall_Click" />
                                    <asp:Button ID="btnrequest" runat="server" Text="View" class="btn btn-sm" OnClick="btnrequest_Click" />
                                    <asp:Button ID="btngetcomments" runat="server" Text="Get Comments" Width="120px"
                                        class="btn btn-sm btn-info" />
                                    <asp:Button ID="btnreferences" runat="server" Text="Get Reference" class="btn btn-sm"
                                        OnClick="btnreferences_Click" Visible="False" />
                                    <asp:Button ID="Btnmoveqc" runat="server" class="btn btn-sm" OnClick="Btnmoveqc_Click" Text="MoveQC" Visible="False" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="center" class="LiteralError" style="padding-top: 6px;">
                                    <asp:Label ID="Lblerror" runat="server" ForeColor="red" Font-Size="14px"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    </asp:Panel>

                                    <asp:Panel ID="PanelStatus" runat="server" CssClass="font">
                                        <table>
                                            <tr>
                                                <td class="font">
                                                    <asp:Label ID="lblinfo" runat="server" ForeColor="red" Font-Size="25px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                    </td>
                            </tr>
                        </table>
                </div>
            </div>
        </div>
        </div>

        <div id="templatemo_footer_wrapper">
            <div id="templatemo_footer" class="font">
                Copy Right ɠString 2012. All Rights Reserved.Powered By : SST
            </div>
            <!-- end of footer -->
        </div>

        <div class="Logout_msgbx" id="ReportPanel" runat="server" align="center">
            <table border="0" style="height: 400px; width: 400px;">
                <tr>
                    <td align="center">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="500px">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                <td colspan="4" align="center" style="height: 25px">
                                    <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Mail Away"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">Date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdate" runat="server" CssClass="txtuser" Width="85px" ForeColor="Black"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdate"
                                        Format="MM/dd/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">Cheque Payable:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtchqpay" runat="server" CssClass="txtuser" Width="200px" ForeColor="Black"></asp:TextBox>
                                    <asp:Button ID="btngetaddress" runat="server" Width="100px" Text="Get Address" CssClass="btn btn-sm" Style="background: grey"
                                        OnClick="btngetaddress_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">Request Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlrequesttype" runat="server" Width="200px" Height="20px"
                                        CssClass="txtuser" ForeColor="Black">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="1">UPS</asp:ListItem>
                                        <asp:ListItem Value="2">UPS/R</asp:ListItem>
                                        <asp:ListItem Value="3">UPS/SASE</asp:ListItem>
                                        <asp:ListItem Value="4">REGULAR</asp:ListItem>
                                        <asp:ListItem Value="5">THANKS REQUEST</asp:ListItem>
                                        <asp:ListItem Value="6">USPS</asp:ListItem>
                                        <asp:ListItem Value="7">FAX REQUEST</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">To Address:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtaddress" runat="server" CssClass="txtuser" Width="300px" Height="100px"
                                        TextMode="MultiLine" ForeColor="Black"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">Borrower Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbrrname" runat="server" CssClass="txtuser font" Width="300px" ForeColor="Black"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">Street:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbrraddress" runat="server" CssClass="txtuser" Width="300px" Height="50px"
                                        TextMode="MultiLine" ForeColor="Black"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">
                                    <asp:Label ID="lblcity" runat="server" Text="City" CssClass="Lbldata"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcity" runat="server" CssClass="txtuser" Width="300px" Height="50px"
                                        TextMode="MultiLine" ForeColor="Black"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">ParcelID:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtparcelid" runat="server" CssClass="txtuser" Width="300px" ForeColor="Black"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">Charges:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtamount" runat="server" CssClass="txtuser" Width="300px" ForeColor="Black"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers">Tax Type:
                                </td>
                                <td>
                                    <asp:TextBox ID="txttaxtype" runat="server" CssClass="txtuser" Width="300px" TextMode="MultiLine" ForeColor="Black"></asp:TextBox>
                                    <%--<asp:TextBox ID="txtcomments" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Button ID="btncreatetreq" runat="server" Width="150px" Text="Create Request"
                                        CssClass="btn btn-sm" OnClick="btncreatetreq_Click" Style="background: grey" />
                                    <asp:Button ID="btncancel" runat="server" Width="100px" Text="Cancel" CssClass="btn btn-sm" Style="background: grey"
                                        OnClick="btncancel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Lblsuccess" runat="server" ForeColor="red" Font-Size="14px" Text="Error"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="Logout_msgbx1" id="statecomments" runat="server" align="center">
            <table border="0" width="800px" style="height: 200px">
                <tr>
                    <td align="center">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="800px">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                <td align="center" style="height: 25px">Order Details
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtstatecomments" runat="server" TextMode="MultiLine" Height="250px"
                                        CssClass="txtuser1" Width="800px" ForeColor="Black"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnclose" runat="server" Width="150px" Text="Split Details" CssClass="MenuFont"
                                        OnClick="btnclose_Click" />
                                    <asp:Button ID="btnsplitclose" runat="server" Width="150px" Text="Close" CssClass="MenuFont"
                                        OnClick="btnsplitclose_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="LblSpliterror" runat="server" ForeColor="red" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="FLcalc_msgbx" id="getcomments" runat="server" align="center">
            <table border="0" width="75%px" style="height: 300px">
                <tr>
                    <td align="center" valign="top" style="color: black">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                <td align="center" style="height: 25px" colspan="2">FL Calcuator
                                </td>
                            </tr>
                        Enter the amount
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtflamount" runat="server" CssClass="txtuser1" Width="150px" ForeColor="Black"></asp:TextBox>
                        <asp:LinkButton ID="Lnkcalculate" runat="server" Text="Calculate" OnClick="Lnkcalculate_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtgetcomments" runat="server" TextMode="MultiLine" Height="250px"
                            CssClass="txtuser1" Width="100%" ReadOnly="True" ForeColor="Black"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btngetclose" runat="server" Width="150px" Text="Close" CssClass="MenuFont"
                            OnClick="btngetclose_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblFLerror" runat="server" ForeColor="red" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
            </table>
            </td>
                </tr>
            </table>
        </div>
        <div class="Logout_checklist" id="LogoutReason" runat="server" align="center">
            <table border="0" width="400px" style="height: 200px">
                <tr>
                    <td align="center" valign="top">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                <td align="center" style="height: 25px" colspan="2">Do you want to Logout?
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers" align="left">Logout Reason :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddllogout" runat="server" Height="20px" Width="200px" CssClass="txtuser"
                                        OnSelectedIndexChanged="ddllogout_SelectedIndexChanged"
                                        AutoPostBack="false" Visible="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:TextBox ID="txtlogreason" runat="server" CssClass="txtuser" Width="311px" ValidationGroup="log"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="reqLog" ControlToValidate="txtlogreason" ErrorMessage="*" ForeColor="Red" ValidationGroup="log" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnok" runat="server" Text="Ok" CssClass="btn btn-sm"
                                        OnClick="btnok_Click" ValidationGroup="log" Style="margin-top: 7px; background: grey" />
                                    <asp:Button ID="btnlogoutclose" runat="server" Width="70px" Text="Cancel" CssClass="btn btn-sm"
                                        OnClick="btnlogoutclose_Click" Style="margin-top: 7px; background: grey" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="Exemptions_msgbox" id="DelayReason" runat="server" align="center">
            <table border="0" width="600px" style="height: 150px">
                <tr>
                    <td align="center" valign="top">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="100%">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                <td align="center" style="height: 25px" colspan="2">Delay Reason
                                </td>
                            </tr>
                            <tr>
                                <td class="Lblothers" align="center" colspan="2">Your target time exceeded for this Order. Please select your Delay Reason.
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:DropDownList ID="ddldelayreason" runat="server" Height="20px" Width="300px"
                                        CssClass="txtuser">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lbldelayerror" runat="server" ForeColor="red" Font-Size="Large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btndelayreason" runat="server" Width="150px" Text="Ok" CssClass="MenuFont"
                                        OnClick="btndelayreason_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="Exemptions_msgbox" id="exemptionsmsgbox" runat="server" align="center"
            visible="false">
            <table border="0" width="400px" style="height: 200px">
                <tr>
                    <td align="center" valign="top">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="600px">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                <%--<asp:TextBox ID="txtcomments" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox>--%>
                                <td align="center" style="height: 50px">
                                    <asp:Label ID="lblexemhead" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px"></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnExemptionsOk" runat="server" Width="150px" Text="OK" CssClass="MenuFont"
                                        OnClick="btnExemptionsOk_Click" />
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
                            <table border="0" style="width: 485px;">
                                <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                    <td align="center" style="height: 25px" colspan="3">Tax Information
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">Email ID</td>
                                    <td class="Lblothers">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttaxtoid" CssClass="font" runat="server" Width="330px" ForeColor="Black" Style="margin-top: 6px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">Tax Type
                                    </td>
                                    <td class="Lblothers">:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddltaxtype1" runat="server" Height="30px" Width="330px" CssClass="table font" ForeColor="Black" Style="margin-bottom: 0px; margin-top: 6px;">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">Address
                                    </td>
                                    <td class="Lblothers">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttaxadd" runat="server" CssClass="txtuser font" Width="330px" Height="100px"
                                            TextMode="MultiLine" ForeColor="Black"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">Parcel No
                                    </td>
                                    <td class="Lblothers">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtparcelno" runat="server" CssClass="txtuser font" Width="330px" ForeColor="Black"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Label ID="lbltaxerror" runat="server" ForeColor="red" Font-Size="Large" CssClass="font"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Button ID="btnsendtaxmail" runat="server" Width="100px" Text="Send Email" CssClass="btn btn-sm" Style="background: gray; margin-top: 6px;"
                                            OnClick="btnsendtaxmail_Click" />
                                        <asp:Button ID="btntaxcancel" runat="server" Width="100px" Text="Cancel" CssClass="btn btn-sm" Style="background: gray; margin-top: 6px;"
                                            OnClick="btntaxcancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div class="Taxinfo_msgbx" id="ParcelInformation" runat="server" align="center">
            <asp:Panel ID="PanelParcelInfo" runat="server" BackColor="#f5f5f5" Width="700px">
                <table border="0" width="700px">
                    <tr>
                        <td align="center" valign="top">
                            <table border="0" width="700px">
                                <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">
                                    <td align="center" style="height: 25px" colspan="3">Parcel Inforamtion
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">No of Parcels
                                    </td>
                                    <td class="Lblothers">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtnoofparcels" runat="server" CssClass="txtuser" Width="300px" ForeColor="Black"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">Parcel
                                    </td>
                                    <td class="Lblothers">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_parcels" runat="server" CssClass="txtuser" Width="300px" Height="100px"
                                            TextMode="MultiLine" ForeColor="Black"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Lblothers">Parcel No
                                    </td>
                                    <td class="Lblothers">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tst_parcelno" runat="server" CssClass="txtuser" Width="300px" ForeColor="Black"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Label ID="lblparcelerror" runat="server" ForeColor="red" Font-Size="Large"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Button ID="btnparcelmail" runat="server" Width="100px" Text="Send Mail" CssClass="MenuFont"
                                            OnClick="btnparcelmail_Click" />
                                        <asp:Button ID="btnparcelcancel" runat="server" Width="100px" Text="Cancel" CssClass="MenuFont"
                                            OnClick="btnparcelcancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <%--<asp:TextBox ID="txtcomments" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox>--%><%--<asp:TextBox ID="txtcomments" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox>--%><%--<asp:TextBox ID="txtcomments" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox>--%>
        <div class="Logout_checklist" id="divpopup" runat="server" align="center">
            <table width="700px" align="center">
                <tr>
                    <td align="center">County Parcel
                    </td>
                    <td align="center">Title Source Parcel
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCntyPrcl" runat="server" Height="30px" Width="200px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTleSrcePrcl" runat="server" Height="30px" Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblCheck" runat="server" ForeColor="Red" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnCheck" runat="server" Text="Check" OnClick="btnCheck_Click" CssClass="MenuFont" />
                        <asp:Button ID="btnTleCntyClse" runat="server" CssClass="MenuFont" OnClick="btnTleCntyClse_Click"
                            Text="Close" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="Logout_checklist_popup" id="divPrclFormat" runat="server" align="center">
            <asp:Panel ID="pnlPrclFrmt" runat="server" Width="800px" Height="300px" ScrollBars="Auto">
                <table align="center" border="0" width="800px">
                    <tr>
                        <td align="left" style="color: Green; font-family: Times New Roman;">For the following types of parcel discrepancies please email us for corrections:
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="left" style="font-family: Times New Roman; font-size: small;">1. Incorrect parcels: Parcels listed in the Title Source system that are for a property
                        other than our subject property.
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-family: Times New Roman; font-size: small;">2. When an account number is listed instead of the parcel / geographic ID number.
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-family: Times New Roman; font-size: small;">3. Pennsylvania parcel numbers that are not in the correct format.
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-family: Times New Roman; font-size: small;">4. Additional numbers are missing from the parcel (example: TSI System shows: 01977-0014
                        County Site shows: 3-01977-1105)
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px;"></td>
                    </tr>
                    <tr>
                        <td align="left" style="color: Red; font-family: Times New Roman;">If a discrepancy is one of the following please leave the parcel number as it is
                        shown:
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="left" style="font-family: Times New Roman; font-size: small;">1. A parcel shows leading zeros (example: TSI System shows: 00610922 County Site
                        shows: 610922).
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-family: Times New Roman; font-size: small;">2. The parcel numbers match but are not in the same format (example: TSI System
                        shows: 108250710 County Site shows: 108-25-0710) ***EXCLUDING PA ORDERS***
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-family: Times New Roman; font-size: small;">3. When Title Source shows and additional number (example: TSI System shows: 204-01-838
                        9 County Site shows: 204-01-838).
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 40px;"></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnClsePrclFrmt" runat="server" Text="Close" CssClass="MenuFont"
                                OnClick="btnClsePrclFrmt_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <%--<asp:TextBox ID="txtcomments" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox>--%>
        <div class="Exemptions_msgbox" id="Divqccheck" runat="server" align="center"
            visible="false">
            <table border="0" width="400px" style="height: 200px">
                <tr>
                    <td align="center" valign="top">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="600px">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">

                                <td align="center" style="height: 50px">
                                    <asp:Label ID="lblQCCheck" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px">The tax amount mismatched!!!
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnQCCheck" runat="server" Width="150px" Text="OK"
                                        CssClass="MenuFont" OnClick="btnQCCheck_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

        <div class="Exemptions_msgbox_ca" id="DivCAstate" runat="server" align="center">
            <table border="0" width="200px" style="height: 150px">
                <tr>
                    <td align="center" valign="top">
                        <table border="0" cellpadding="3px" cellspacing="4px" width="200px">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">

                                <%-- <td align="center" style="height: 50px">
                                    <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                </td>--%>
                            </tr>
                            <tr>
                                <td style="height: 25px; padding-left: 40px;">"TRA # updated?"
                                </td>
                            </tr>
                            <tr>
                                <td align="center">

                                    <%--<asp:Button ID="btnsavepop" runat="server" Width="150px" Text="OK"
                                        CssClass="MenuFont"/>--%>
                                    <asp:Button ID="btnokpop" runat="server" Width="150px" Text="OK"
                                        CssClass="MenuFont" OnClick="btnokpop_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

        <div class="Exemptions_msgbox_scrap" id="ScrapDiv" runat="server" align="center">
            <table border="0" class="auto-style8">
                <tr>
                    <td align="center" valign="top">
                        <table border="0" cellpadding="3px" cellspacing="4px" class="auto-style7">
                            <tr class="templatemo_menu_wrapper1" style="height: 25px; color: White;" align="center">

                                <%-- <td align="center" style="height: 50px">
                                    <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                </td>--%>
                            </tr>
                            <tr>
                                <td style="padding-left: 40px;" class="auto-style9">&quot;Information retrieved, did you see the data??&quot;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="auto-style10">

                                    <%--<asp:Button ID="btnsavepop" runat="server" Width="150px" Text="OK"
                                        CssClass="MenuFont"/>--%>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="BtnScrapMsg" runat="server" Width="67px" Text="Yes"
                                        CssClass="MenuFont" OnClick="BtnScrapMsg_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="BtnScrapMsgNo" runat="server" Width="67px" Text="No"
                                        CssClass="MenuFont" OnClick="BtnScrapMsgNo_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <input type="hidden" id="theInput" value="<%=Session["TimePro"]%>" />
        <asp:HiddenField ID="Hlogout" runat="server" />
        <script type="text/javascript" language="javascript">
            show_TickerTime();
        </script>
        <script type="text/javascript" language="javascript">

            function openNewWin(url) {
                var x = window.open(url, 'mynewwin', 'width=600,height=600,toolbar=1');
                x.focus();
            }

            function disp_prompt() {
                var fname = prompt("Please Enter the Logout Reason:", "");
                document.getElementById("<%=Hlogout.ClientID %>").value = fname;
            }

        </script>

        <script type="text/javascript">
            function validate() {
                var empid = document.getElementById("txtEmpId").value;
                var empname = document.getElementById("txtEmpName").value;
                var empsalary = document.getElementById("txtEmpSalary").value;
                var dept = document.getElementById("txtDept").value;
                if (empid == "" || empname == "" || empsalary == "" || dept == "") {
                    alert("Fill All the Fields");
                    return false;
                }
                else {
                    return true;
                }
            }
        </script>

        <div id="DivEmail" runat="server">
            <table>
                <tr>
                    <td>
                        <b><span style="font-size: 9pt; color: gray">Thanks and Regards,<u></u><u></u></span></b>
                    </td>
                    <tr>
                        <td>
                            <b><span style="font-size: 9pt; color: gray">Service Delivery Team &nbsp;</span></b><b></b><b></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <u></u><span style="width: 333px; min-height: 11px">
                                <img src="cid:REDLINE" height="3" width="333"></span><u></u><b><span style="font-size: 11pt; font-family: Calibri,Calibri; color: gray"><u></u>&nbsp;<u></u></span></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td valign="top" style="padding-top: 15px;">
                                        <u></u>
                                        <img src="cid:LOGO" align="left" height="43" hspace="12" width="106"><u></u><span
                                            style="font-size: 8pt; color: rgb(78,75,76)"><u></u><u></u></span>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span style="font-size: 8pt; color: rgb(235,28,35)">(v):</span><span style="font-size: 8pt; color: rgb(78,75,76)">&nbsp;202-470-0648/49</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-size: 8pt; color: rgb(235,28,35)">(e):</span><span style="font-size: 8pt; color: Blue">&nbsp; <u>tsi@stringinfo.com</u></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 21px">
                                                                <span style="font-size: 8pt; color: rgb(235,28,35)">(w):</span><span style="font-size: 8pt; color: Blue">&nbsp; <u>www.stringinfo.com</u></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 21px">
                                                                <span style="font-size: 8pt; color: rgb(78,75,76)">&nbsp;String Information Services&nbsp;&nbsp;</span><a
                                                                    href="http://www.linkedin.com/company/50494?trk=tyah&amp;trkInfo=tas:String%20,idx:"
                                                                    style="color: rgb(17,85,204)" target="_blank" title="This external link will open in a new window"><b><span
                                                                        style="font-size: 8pt; color: rgb(15,36,62); text-decoration: none"><img src="cid:LN"
                                                                            border="0" height="14" width="14"></span></b></a><span style="font-size: 8pt">&nbsp;</span><a
                                                                                href="https://twitter.com/Stringre" style="color: rgb(17,85,204)" target="_blank"
                                                                                title="This external link will open in a new window"><b><span style="font-size: 8pt; color: rgb(15,36,62); text-decoration: none"><img src="cid:TWTR" border="0" height="14"
                                                                                    width="14"></span></b></a><span style="font-size: 11pt; font-family: Calibri,Calibri">&nbsp;
                                                                                                &nbsp; &nbsp;</span><span style="font-family: Calibri,Calibri; font-size: 11pt">&nbsp;&nbsp;</span>
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
        </div>
        <div id="DivTaxEmail" runat="server">
            <table>
                <tr>
                    <td>
                        <b><span style="font-size: 9pt; color: gray">Thanks and Regards,<u></u><u></u></span></b>
                    </td>
                    <tr>
                        <td>
                            <b><span style="font-size: 9pt; color: gray">Service Delivery Team &nbsp;</span></b><b></b><b></b>
                        </td>
                        <tr>
                            <td>
                                <u></u><span style="width: 333px; min-height: 11px">
                                    <img src="cid:REDLINE" height="3" width="333"></span><u></u><b><span style="font-size: 11pt; font-family: Calibri,Calibri; color: gray"><u></u>&nbsp;<u></u></span></b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td valign="top" style="padding-top: 15px;">
                                            <u></u>
                                            <img src="cid:LOGO" align="left" height="43" hspace="12" width="106"><u></u><span
                                                style="font-size: 8pt; color: rgb(78,75,76)"><u></u><u></u></span>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span style="font-size: 8pt; color: rgb(235,28,35)">(v):</span><span style="font-size: 8pt; color: rgb(78,75,76)">&nbsp;202-470-0648/49</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span style="font-size: 8pt; color: rgb(235,28,35)">(e):</span><span style="font-size: 8pt; color: Blue">&nbsp; <u>taxcerts@stringinformation.com</u></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 21px">
                                                                    <span style="font-size: 8pt; color: rgb(235,28,35)">(w):</span><span style="font-size: 8pt; color: Blue">&nbsp; <u>www.stringinfo.com</u></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 21px">
                                                                    <span style="font-size: 8pt; color: rgb(78,75,76)">&nbsp;String Information Services&nbsp;&nbsp;</span><a
                                                                        href="http://www.linkedin.com/company/50494?trk=tyah&amp;trkInfo=tas:String%20,idx:"
                                                                        style="color: rgb(17,85,204)" target="_blank" title="This external link will open in a new window"><b><span
                                                                            style="font-size: 8pt; color: rgb(15,36,62); text-decoration: none"><img src="cid:LN"
                                                                                border="0" height="14" width="14"></span></b></a><span style="font-size: 8pt">&nbsp;</span><a
                                                                                    href="https://twitter.com/Stringre" style="color: rgb(17,85,204)" target="_blank"
                                                                                    title="This external link will open in a new window"><b><span style="font-size: 8pt; color: rgb(15,36,62); text-decoration: none"><img src="cid:TWTR" border="0" height="14"
                                                                                        width="14"></span></b></a><span style="font-size: 11pt; font-family: Calibri,Calibri">&nbsp;
                                                                                                    &nbsp; &nbsp;</span><span style="font-family: Calibri,Calibri; font-size: 11pt">&nbsp;&nbsp;</span>
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
        </div>
        <%--<asp:TextBox ID="txtcomments" runat="server" CssClass="txtuser" Height="20px" Width="800px"></asp:TextBox>--%>
        <asp:DropDownList ID="ddlreferences" runat="server" Height="16px" Width="78px"
            OnSelectedIndexChanged="ddlreferences_SelectedIndexChanged" AutoPostBack="true" Visible="False">
        </asp:DropDownList>
        <asp:Label ID="lblserpro" runat="server" Font-Bold="True"
            Font-Size="Larger" ForeColor="Red"
            Style="text-decoration: blink; width: 135px; height: 19px; font-weight: 500;" Visible="False"></asp:Label>
        <asp:TextBox ID="txttaxajuri" runat="server" Height="16px"
            Width="65px" Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtentitycount" runat="server" Height="20px" Width="66px" AutoPostBack="true" OnTextChanged="txtentitycount_TextChanged" Visible="False"></asp:TextBox>
        <asp:Label ID="lblTax" runat="server" Text="" Visible="false"></asp:Label>

        <asp:CheckBox ID="chkclientcmt" runat="server" Text="Client Comments" Font-Names="Times New Roman"
            Font-Size="18px" Visible="False" />
        <asp:TextBox ID="txtassessor" runat="server" Height="20px" Width="26px" Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtassphone" runat="server" Height="20px" Width="19px" Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtTreasphone" runat="server" Height="20px" Width="16px" Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtTreasurer" runat="server" Height="20px" Width="28px" Visible="False"></asp:TextBox>
        <asp:Button ID="btnlinksave" runat="server" Text="Save" CssClass="MenuFont" OnClick="btnlinksave_Click" Visible="False" />
        <asp:CheckBox ID="chksavelink" runat="server" Text="Save Link" Visible="False" />
        <asp:Button ID="btnlinkupdate" runat="server" Text="Update" CssClass="MenuFont" OnClick="btnlinkupdate_Click" Visible="False" />
        <asp:CheckBox ID="chkparcelmail" runat="server" Text="Parcel Mail" OnCheckedChanged="chkparcelmail_CheckedChanged"
            AutoPostBack="true" Visible="False" />
        <asp:Button ID="btnPrclShow" runat="server" Text="Parcel Format" CssClass="MenuFont"
            Font-Bold="True" ForeColor="Red" OnClick="btnPrclShow_Click" Visible="False" />
        <asp:Button ID="btnPrclFrmt" runat="server" Text="Parcel #" CssClass="MenuFont" Font-Bold="True" Visible="false"
            ForeColor="White" OnClick="btnPrclFrmt_Click" />
        <div runat="server" style="display: none;">
            <table>
                <tr>
                    <td rowspan="2" valign="top" class="auto-style4">
                        <asp:Panel ID="Panel1" runat="server" Width="100%" BackColor="#f1f0ef" ScrollBars="Auto">
                            <asp:GridView ID="Gridnextorder" runat="server" GridLines="None" CssClass="Gnowrap">
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Panel ID="Panelprocesstime" runat="server" Width="100%" BackColor="#f1f0ef"
                            ScrollBars="Auto">
                            <table style="border: solid 0px gray;" class="auto-style5">
                                <tr style="height: 25px;">
                                    <td class="Lblothers">
                                        <asp:Label ID="lblordertype" runat="server" Text="" Width="100%" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height: 25px;">
                                    <td>
                                        <asp:Label ID="lblprocesstime" runat="server" Width="100%" Height="25px" Visible="False"></asp:Label>
                                        <asp:Timer ID="Timer1" runat="server" Enabled="false" OnTick="Timer1_Tick">
                                        </asp:Timer>
                                        <asp:Timer ID="Timer2" runat="server" Enabled="false" OnTick="Timer2_Tick">
                                        </asp:Timer>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" Width="100%" BackColor="#f1f0ef" ScrollBars="Auto">
                            <asp:GridView ID="GridEntity" runat="server" GridLines="None" CssClass="Gnowrap"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sno.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Frequency">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlPymntFrqncy" runat="server" BorderStyle="None" BackColor="FloralWhite"
                                                Width="100px" Height="20px" AutoPostBack="True" OnSelectedIndexChanged="ddlPymntFrqncy_SelectedIndexChanged">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Annual</asp:ListItem>
                                                <asp:ListItem>Semi-Annual</asp:ListItem>
                                                <asp:ListItem>Instalments</asp:ListItem>
                                                <asp:ListItem>Quartely</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Panel runat="server" ID="pnlNested" Width="100%" BackColor="#d8d8d7" ScrollBars="Auto">
                                                <asp:GridView ID="gvNested" runat="server" GridLines="None" CssClass="Gnowrap"
                                                    AutoGenerateColumns="false">
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="Tax Amount">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTaxAmnt" runat="server" BorderStyle="None" Text='<%# Eval("TaxAmount")%>'
                                                                    BackColor="FloralWhite" Width="100%" Height="20px" AutoPostBack="True"
                                                                    OnTextChanged="txtTaxAmnt_TextChanged"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Payment Status">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlPymntSts" runat="server" BorderStyle="None" BackColor="FloralWhite"
                                                                    Width="100px" Height="20px">
                                                                    <asp:ListItem></asp:ListItem>
                                                                    <asp:ListItem>Paid</asp:ListItem>
                                                                    <asp:ListItem>Due</asp:ListItem>
                                                                    <asp:ListItem>Delinquent</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Label ID="lblhp" runat="server" Text="" Width="40%" Visible="false"></asp:Label>
                        <asp:Label ID="lblprior" runat="server" Text="" Width="40%" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
