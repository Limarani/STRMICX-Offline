

var objResultSet = "";
var test = "";
function jsonParse() {
    // var objResultSet;
    debugger

    try {
        if ($.trim($("[id*=txtorderno]").val()) == "") {
            alert("Enter OrderNo...");
            return;
        }

        var stat1 = document.getElementById("dropState");
        var strState1 = stat1.options[stat1.selectedIndex].text;


        if (strState1 == "--select--") {
            alert("Select State...");
            return;
        }
        var e1 = document.getElementById("ddlcounty");
        var strUser1 = e1.options[e1.selectedIndex].text;
        if (strUser1 == "--select--") {
            alert("Select County...");
            return;
        }

       // myFun();
        // var strXML = $("#hdnTypeXML").val();
        var obj = {};
        obj.StreetNo = $.trim($("[id*=txtstreetno]").val());
        obj.Direction = $.trim($("[id*=txtdirection]").val());
        obj.StreetName = $.trim($("[id*=txtstreetname]").val());
        obj.UnitNumber = $.trim($("[id*=txtunitnumber]").val());
        obj.StreetType = $.trim($("[id*=txtstreettype]").val());
        obj.Parcelid = $.trim($("[id*=txtparcelid]").val());
        obj.OrderNo = $.trim($("[id*=txtorderno]").val());
        obj.Address = $.trim($("[id*=txtaddress]").val());
        var stat = document.getElementById("dropState");
        var strState = stat.options[stat.selectedIndex].text;
        obj.State = strState;

        var e = document.getElementById("ddlcounty");
        var strUser = e.options[e.selectedIndex].text;
        obj.County = strUser;
        obj.OwnerName = $.trim($("[id*=txtownername]").val());
        obj.City = $.trim($("[id*=txtcity]").val());


        if (obj.Parcelid == "") {
            if (obj.County == "alameda" || obj.County == "contra") {
                var cit = $.trim($("[id*=txtcity]").val());
                if (cit == "") {
                    alert("Enter City");
                    return;
                }

            }
        }
        $("#Button1").prop("disabled", true);
        $.ajax({
            type: "POST",
            url: "../Pages/scrap.aspx/jsonData",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                objResultSet = response.d;
                var JsonDatas = $.parseJSON(objResultSet);
                var dataChk;
                try {
                    dataChk = JsonDatas[0].Result;
                }
                catch (err) {
                    alert("Data can't be scraped due to some delay. \nTry again.");
                    return;
                }

                if (dataChk == "MultiAddress") {
                    alert("As huge records have been returned. \nPlease proceed with manual search");
                    return;
                }
                else if (dataChk == "AddressGrid") {
                    gridShow(objResultSet, "AddressGrid");
                    fncBind4();
                    return;
                }
                else if (dataChk == "ServerError") {
                    alert("Unable to connect to the server \nPlease ensure the server is started");
                    return;
                }
                else if (dataChk == "ExceedError") {
                    alert("As huge records have been returned. \nPlease proceed with manual search");
                    return;
                }
                else if (dataChk == "timeOut") {
                    alert("System Timeout");
                    return;
                }
                else if (dataChk == "SearchError") {
                    alert("No data available for the search");
                    return;
                }
                else if (dataChk == "MultiOwner") {
                    gridShow(objResultSet, "MultiOwner");
                    fncBind();
                    //$("[id*=txtownername]").val("");
                    // $("#Button1").prop("disabled", false);       
                    return;
                }
                else if (dataChk == "DataError") {
                    alert("Data not available in page");
                    return;
                }
                else if (dataChk == "SiteError") {
                    alert("Site is currently down");
                    return;
                }
                else if (dataChk == "Normal") {
                    $("[id*=txtownername]").val("");
                    parserJson(objResultSet);
                }

                // parserJson(objResultSet);
            },
            failure: function (response) {
                alert("failure: " + response.d);
            },
            error: function (xhr, textStatus, error) {
                alert("Error: " + error);
            }
        });


    }
    catch (e) {
        alert(e.message);
    }
}


function myFun() 
{
    $.showLoading({ name: 'square-flip', allowHide: true });
}


$(document).ready(function () {
    $.showLoading({ name: 'square-flip', allowHide: true });
});



function fncBind() {
    $("[id*=Button2]").click();
}

function fncBind4() {
    $("[id*=Button4]").click();
}



function parserJson(JsonData) {

    //alert(json_obj.InterestDate.toString());

    debugger
    var CheckMatch = "";
    var JsonDatas = $.parseJSON(JsonData);
    for (var n = 0; n < JsonDatas.length; n++) {
        addressArray = [];
        var json_obj;
        try {
            json_obj = JsonDatas[n];
        }
        catch (err) {
            alert("Data can't be scraped due to some delay. \nTry again.");
            return;
        }
        var Parcel = json_obj.Parcel;
        //var parselNo = json_obj.Parcel.ParcelNumber;
        //var countyId = json_obj.Parcel.countyId;
        //var orderId = json_obj.Parcel.OrderId;
        //var address = json_obj.Parcel.Location + ", " + json_obj.Parcel.Town;
        //var address = json_obj.Parcel.Address;
        for (var i in Parcel) {
            addressDetails = {};
            addressDetails["orderid"] = Parcel[i].OrderId;
            addressDetails["address"] = Parcel[i].Address;
            addressDetails["parcel_no"] = Parcel[i].ParcelNumber;
            addressDetails["county_id"] = Parcel[i].countyId;
            addressDetails["OwnerInformation"] = Parcel[i].OwnerInformation;
            addressDetails["PropertyType"] = Parcel[i].PropertyType;
            addressDetails["MailingAddress"] = Parcel[i].MailingAddress;
            addressDetails["FolioNumber"] = Parcel[i].FolioNumber;
            addressDetails["AccountNumber"] = Parcel[i].AccountNumber;
            addressArray.push(addressDetails);
        }
        var addrDetails = addressArray;

        realInfoArray = [];
        var otherRealEstate = json_obj.otherRealInfo;
        for (var i in otherRealEstate) {
            otherRealInfo = {};
            otherRealInfo["Ryear"] = otherRealEstate[i].otherYear;
            otherRealInfo["RfaceValue"] = otherRealEstate[i].otherFaceValue;
            otherRealInfo["RcertificateValue"] = otherRealEstate[i].otherCertificateValue;
            otherRealInfo["Rstatus"] = otherRealEstate[i].otherStatus;
            otherRealInfo["RamtPaid"] = otherRealEstate[i].otherAmtPaid;
            realInfoArray.push(otherRealInfo);
        }
        var realInfoDetails = realInfoArray;


        delinquentDetailsArray = [];
        var paymentDetail = json_obj.PaymentDetail;
        for (var i in paymentDetail) {
            delinquentDetailsObject = {};
            delinquentDetailsObject["amount_paid"] = paymentDetail[i].AmountPaid;
            delinquentDetailsObject["payNo"] = parseInt(paymentDetail[i].Number);
            delinquentDetailsObject["due_charges"] = paymentDetail[i].DueCharges;
            delinquentDetailsObject["payment_posted"] = paymentDetail[i].PaymentPosted;
            delinquentDetailsObject["status"] = paymentDetail[i].Status;
            delinquentDetailsObject["installment"] = paymentDetail[i].Installment;
            delinquentDetailsObject["original_bill_amount"] = paymentDetail[i].OriginalBillAmount;
            delinquentDetailsObject["Ownername"] = paymentDetail[i].Ownername;
            delinquentDetailsObject["Folio"] = paymentDetail[i].Folio;
            delinquentDetailsObject["Paid"] = paymentDetail[i].Paid;
            delinquentDetailsObject["UnPaid"] = paymentDetail[i].UnPaid;
            delinquentDetailsObject["DelinquentDate"] = paymentDetail[i].DelinquentDate;
            //delinquentDetailsObject["TotalDue"] = paymentDetail[i].TotalDue;
            delinquentDetailsArray.push(delinquentDetailsObject);
        }
        var delinqDetails = delinquentDetailsArray;

        exemptionArray = [];
        var PropertyValue = json_obj.PropertyValue;
        for (var i in PropertyValue) {
            exemptionDetails = {};
            exemptionDetails["land"] = PropertyValue[i].Land;
            exemptionDetails["improvements"] = PropertyValue[i].Improvements;
            exemptionDetails["total_assess_value"] = PropertyValue[i].TotalAssessValue;
            exemptionDetails["net_assess_value"] = PropertyValue[i].NetAssessValue;
            exemptionDetails["exemp_value_new_contruction"] = PropertyValue[i].ExcemptionValue;
            exemptionDetails["construction_supp_value"] = parseInt(PropertyValue[i].ConstructionSuppValue);
            exemptionDetails["Type"] = PropertyValue[i].Type;
            exemptionDetails["Building_Value"] = PropertyValue[i].Building_Value;
            exemptionDetails["Year"] = PropertyValue[i].Year;
            exemptionDetails["ExtraFeaturevalue"] = PropertyValue[i].ExtraFeaturevalue;
            exemptionDetails["HouseholdPersonalProperty"] = PropertyValue[i].HouseholdPersonalProperty;
            exemptionDetails["BusinessPersonalProperty"] = PropertyValue[i].BusinessPersonalProperty;
            exemptionDetails["OtherExcemption"] = PropertyValue[i].OtherExcemption;
            exemptionDetails["MarketValue"] = PropertyValue[i].MarketValue;
            exemptionArray.push(exemptionDetails);
        }
        var exemptDetails = exemptionArray;

        payementArray = [];
        var Payment = json_obj.Payment;
        for (var i in Payment) {
            payementHistory = {};
            payementHistory["current_payments"] = Payment[i].CurrentCalendarYearPayment;
            payementHistory["last_payment_date"] = Payment[i].PaymentDate;
            payementHistory["prior_payments"] = Payment[i].PriorCalendarYearPayment;
            payementHistory["fiscal_tax_payments"] = Payment[i].FiscalTaxYearPayment;
            payementHistory["last_payment_amt"] = Payment[i].PaymentAmount;
            payementHistory["tax_before_payment"] = Payment[i].TaxBeforePayment;
            payementHistory["bill_type"] = Payment[i].BillType;
            payementHistory["ReceiptNumber"] = Payment[i].ReceiptNumber;
            payementHistory["FaceAmount"] = Payment[i].FaceAmount;
            payementHistory["Bid"] = Payment[i].Bid;
            payementHistory["PaidBy"] = Payment[i].PaidBy;
            payementHistory["EffectiveDate"] = Payment[i].EffectiveDate;
            payementHistory["Status"] = Payment[i].Status;
            payementHistory["Balance"] = Payment[i].Balance;
            payementHistory["BillNumber"] = Payment[i].BillNumber;
            payementHistory["DueDate"] = Payment[i].DueDate;
            payementArray.push(payementHistory);
        }
        var payDetails = payementArray;

        propertyDetailsArray = [];
        var RealProperty = json_obj.RealProperty;
        for (var z in RealProperty) {
            propertyDetails = {};
            propertyDetails["fiscal_value"] = RealProperty[z].FiscalYear;
            propertyDetails["improvements"] = RealProperty[z].Improvements;
            propertyDetails["gross_assessed"] = RealProperty[z].GrossAssessed;
            propertyDetails["total_ass_value"] = RealProperty[z].TotalAssessValue;
            propertyDetails["exempt"] = RealProperty[z].Exempt;
            propertyDetails["personal_property"] = RealProperty[z].PersonalProperty;
            propertyDetails["total_taxable_value"] = RealProperty[z].TotalTaxableValue;
            propertyDetails["land"] = RealProperty[z].Land;
            propertyDetails["tax_land_imp"] = RealProperty[z].Taxable;
            propertyDetails["allocation_assd"] = RealProperty[z].ElementAllocation;
            propertyDetails["Millage"] = RealProperty[z].Millage;
            propertyDetails["TaxingAuthority"] = RealProperty[z].TaxingAuthority;
            propertyDetails["LevyingAuthority"] = RealProperty[z].LevyingAuthority;
            propertyDetails["Rate"] = RealProperty[z].Rate;
            propertyDetails["Amount"] = RealProperty[z].Amount;
            propertyDetails["Type"] = RealProperty[z].Type;
            propertyDetails["AssessmentDescription"] = RealProperty[z].AssessmentDescription;
            propertyDetails["Units"] = RealProperty[z].Units;
            propertyDetails["Taxes"] = RealProperty[z].Taxes;
            propertyDetails["AssessmentTotal"] = RealProperty[z].AssessmentTotal;
            propertyDetails["OtherExempt"] = RealProperty[z].OtherExempt;
            propertyDetails["Credit"] = RealProperty[z].Credit;
            propertyDetails["Savings"] = RealProperty[z].Savings;
            propertyDetails["Taxingcode"] = RealProperty[z].Taxingcode;
            propertyDetails["Levyingcode"] = RealProperty[z].Levyingcode;
            propertyDetails["SpecialAssessment"] = RealProperty[z].SpecialAssessment;
            propertyDetailsArray.push(propertyDetails);
        }
        var propDetails = propertyDetailsArray;


        debugger
        taxpaymentsArray = [];
        var Tax = json_obj.Tax;
        for (var z in Tax) {
            taxpaymentsDetails = {};
            taxpaymentsDetails["taxes_assess"] = Tax[z].TaxesAssessed;
            taxpaymentsDetails["less_cap_reduction"] = Tax[z].LessCapReduction;
            taxpaymentsDetails["net_taxes"] = Tax[z].NetTaxes;
            taxpaymentsDetails["ExemptionAmount"] = Tax[z].ExemptionAmount;
            taxpaymentsDetails["CombinedTaxesAndAssessment"] = Tax[z].CombinedTaxesAndAssessment;
            taxpaymentsDetails["CheckPayable"] = Tax[z].CheckPayable;
            taxpaymentsDetails["GrossTax"] = Tax[z].GrossTax;
            taxpaymentsArray.push(taxpaymentsDetails);
        }
        var taxDetails = taxpaymentsArray;

        AmountDueArray = [];
        var AmountDue = json_obj.DelinquentDetails;
        for (var z in AmountDue) {
            AmountDueDetails = {};
            AmountDueDetails["tyear"] = AmountDue[z].Year;
            AmountDueDetails["chargecat"] = AmountDue[z].ChargeCategory;
            AmountDueDetails["district"] = AmountDue[z].District;
            AmountDueDetails["charge"] = AmountDue[z].Charge;
            AmountDueDetails["mindue"] = AmountDue[z].MinimumDue;
            AmountDueDetails["baldue"] = AmountDue[z].BalanceDue;
            AmountDueDetails["datedue"] = AmountDue[z].DateDue;
            AmountDueDetails["Period"] = AmountDue[z].Period;
            AmountDueDetails["AmountDue"] = AmountDue[z].AmountDue;
            AmountDueDetails["DefaultNumber"] = AmountDue[z].DefaultNumber;
            AmountDueArray.push(AmountDueDetails);
        }
        var AmntDueDetails = AmountDueArray;


        amgArray = [];
        var amg1 = json_obj.ParcelDetail;
        var amg11 = json_obj.ParcelDescription;

        if (Object.keys(amg1).length > 0) {
            for (var z in amg1) {
                amgDetail1 = {};
                amgDetail1["amgfound"] = parseInt(json_obj.AmgFound);
                amgDetail1["district_amgid"] = amg1[z].DistrictAMGID;
                amgDetail1["name"] = amg1[z].Name;
                amgDetail1["sttus"] = amg1[z].Status;
                amgDetail1["unbill_prinicipal"] = amg1[z].UnbilledPrincipal;
                if (Object.keys(amg11).length > 0) {
                    amgDetail1["legaldescription"] = amg11[z].LegalDescription;
                    amgDetail1["original_assesment"] = amg11[z].OriginalAssessment;
                    amgDetail1["payoff"] = amg11[z].Payoff;
                }
                amgArray.push(amgDetail1);
            }
        }
        else if (Object.keys(amg11).length > 0) {
            for (var z in amg11) {
                amgDetail1 = {};
                if (Object.keys(amg1).length > 0) {
                    amgDetail1["amgfound"] = parseInt(json_obj.AmgFound);
                    amgDetail1["district_amgid"] = amg1[z].DistrictAMGID;
                    amgDetail1["name"] = amg1[z].Name;
                    amgDetail1["sttus"] = amg1[z].Status;
                    amgDetail1["unbill_prinicipal"] = amg1[z].UnbilledPrincipal;
                }
                amgDetail1["legaldescription"] = amg11[z].LegalDescription;
                amgDetail1["original_assesment"] = amg11[z].OriginalAssessment;
                amgDetail1["payoff"] = amg11[z].Payoff;
                amgArray.push(amgDetail1);
            }
        }
        var AmgtDetails = amgArray;

        var TotalParcelDue = json_obj.ParcelDue;
        TotalParcelDueArray = [];
        for (var z in TotalParcelDue) {
            ParcelDueDetails = {};
            ParcelDueDetails["amgfound"] = parseInt(json_obj.AmgFound);
            ParcelDueDetails["due_status"] = TotalParcelDue[z].DueStatus;
            ParcelDueDetails["principal"] = TotalParcelDue[z].Principal;
            ParcelDueDetails["interest"] = TotalParcelDue[z].Interest;
            ParcelDueDetails["penalty"] = TotalParcelDue[z].Penalty;
            ParcelDueDetails["other"] = TotalParcelDue[z].Other;
            ParcelDueDetails["total_due"] = TotalParcelDue[z].TotalDue;
            ParcelDueDetails["BillYear"] = TotalParcelDue[z].BillYear;
            ParcelDueDetails["BillType"] = TotalParcelDue[z].BillType;
            ParcelDueDetails["BillNumber"] = TotalParcelDue[z].BillNumber;
            ParcelDueDetails["GrossTax"] = TotalParcelDue[z].GrossTax;
            ParcelDueDetails["Discount"] = TotalParcelDue[z].Discount;
            ParcelDueDetails["InstallmentAmount"] = TotalParcelDue[z].InstallmentAmount;
            ParcelDueDetails["Installement"] = TotalParcelDue[z].Installement;
            ParcelDueDetails["DueDate"] = TotalParcelDue[z].DueDate;
            ParcelDueDetails["AdValoremTax"] = TotalParcelDue[z].AdValoremTax;
            ParcelDueDetails["SpecialAssessments"] = TotalParcelDue[z].SpecialAssessments;
            ParcelDueDetails["Account"] = TotalParcelDue[z].Account;
            ParcelDueDetails["Folio"] = TotalParcelDue[z].Folio;
            ParcelDueDetails["Taxes"] = TotalParcelDue[z].Taxes;
            ParcelDueDetails["Fees"] = TotalParcelDue[z].Fees;
            ParcelDueDetails["Paid"] = TotalParcelDue[z].Paid;
            ParcelDueDetails["Type"] = TotalParcelDue[z].Type;
            ParcelDueDetails["PaidDate"] = TotalParcelDue[z].PaidDate;
            ParcelDueDetails["TaxableValue"] = TotalParcelDue[z].TaxableValue;
            ParcelDueDetails["DeedName"] = TotalParcelDue[z].DeedName;
            ParcelDueDetails["Period"] = TotalParcelDue[z].Period;
            TotalParcelDueArray.push(ParcelDueDetails);
        }

        var TotalParcelDueDetails = TotalParcelDueArray;

        var SummaryDetails = json_obj.ValueSummary;
        SummaryDetailsArray = [];
        for (var z in SummaryDetails) {
            SummaryObj = {};
            SummaryObj["TaxingDistrict"] = SummaryDetails[z].TaxingDistrict;
            SummaryObj["County"] = SummaryDetails[z].County;
            SummaryObj["SchoolBoard"] = SummaryDetails[z].SchoolBoard;
            SummaryObj["Municipal"] = SummaryDetails[z].Municipal;
            SummaryObj["Others"] = SummaryDetails[z].Others;
            SummaryObj["City"] = SummaryDetails[z].City;
            SummaryObj["Regional"] = SummaryDetails[z].Regional;
            SummaryDetailsArray.push(SummaryObj);
        }
        var TotalSummaryDetails = SummaryDetailsArray;

        var dataCheck = json_obj.hasOwnProperty('TaxCertificate');
        CertificateDetailsArray = [];
        if (dataCheck == true) {
            var CertificateDetails = json_obj.TaxCertificate;
            for (var z in CertificateDetails) {
                CertificateObj = {};
                CertificateObj["TaxYear"] = CertificateDetails[z].TaxYear;
                CertificateObj["Folio"] = CertificateDetails[z].Folio;
                CertificateObj["CertificateYear"] = CertificateDetails[z].CertificateYear;
                CertificateObj["CertificateNumber"] = CertificateDetails[z].CertificateNumber;
                CertificateObj["CertificateHolderName"] = CertificateDetails[z].CertificateHolderName;
                CertificateObj["TDAnumber"] = CertificateDetails[z].TDAnumber;
                CertificateDetailsArray.push(CertificateObj);
            }
        }
        var TaxCertificateDetails = CertificateDetailsArray;


        //var obj1 = $.parseJSON(JSON.stringify({ Address: addrDetails }));
        obj1 = {};
        obj1["Address"] = JSON.stringify(addrDetails);
        obj1["DelinArray"] = JSON.stringify(delinqDetails);
        obj1["Exemption"] = JSON.stringify(exemptDetails);
        obj1["Payment"] = JSON.stringify(payDetails);
        obj1["PropertyArray"] = JSON.stringify(propDetails);
        obj1["Taxpayments"] = JSON.stringify(taxDetails);
        obj1["AmountDueDetail"] = JSON.stringify(AmntDueDetails);
        obj1["AmgtDetail"] = JSON.stringify(AmgtDetails);
        obj1["DueDetail"] = JSON.stringify(TotalParcelDueDetails);
        obj1["SummaryDetail"] = JSON.stringify(TotalSummaryDetails);
        obj1["CertificateDetails"] = JSON.stringify(TaxCertificateDetails);
        obj1["RestateDetail"] = JSON.stringify(realInfoDetails);

        $.ajax({
            type: "POST",
            url: "../Pages/scrap.aspx/insertData",
            data: JSON.stringify(obj1),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                // alert(response.d);
                objResultSet = response.d;

                if (n == JsonDatas.length - 1) {
                    if (objResultSet == "1") {
                        alert("Data Inserted successfully");
                        fncBind();
                    }
                    else if (objResultSet == "0") {
                        alert("Data not available");
                    }
                    else if (objResultSet == "2") {
                        alert("Data not available check for the given input");
                    }
                }
            },
            failure: function (response) {
                alert("failure: " + response.d);
            },
            error: function (xhr, textStatus, error) {
                alert("Error: " + error);
            }
        });

    }

}





function gridShow(JsonData, data) {

    debugger
    var JsonDatas = $.parseJSON(JsonData);
    var dataChk;
    try {
        dataChk = JsonDatas[0].Parcel;
    }
    catch (err) {
        alert("Data can't be scraped due to some delay. \nTry again.");
        return;
    }

    try {

        //        $('#tblshow').show();
        //        $('#tblshow').DataTable({
        //            destroy: true,
        //            retrieve: true,
        //            data: dataChk,
        //            columns: [
        //                        { data: 'PropertyAddress' },
        //                        { data: 'MailingAddress' },
        //                        { data: 'ParcelID' },
        //                        { data: 'OwnerName' }
        //                    ]

        //        })
        obj1 = {};
        var result = "";
        obj1["ownerData"] = JSON.stringify(dataChk);
        obj1["Search"] = data;
        $.ajax({
            type: "POST",
            url: "../Pages/scrap.aspx/gridData",
            data: JSON.stringify(obj1),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                result = response.d;
                //                $("#content").show();
                //                for (var i = 0; i < result.Item1.length; i++) {

                //                    $("#tblsettle").append("<tr><td><input type=\"checkbox\" class=\"select\" id=\"select\" name=\"CheckBox\" /></td><td>" + result.Item1[i].PropertyAddress + "</td><td>" + result.Item1[i].MailingAddress + "</td><td>" + result.Item1[i].ParcelID + "</td><td>" + result.Item1[i].OwnerName + "</td></tr>");

                //                }

            },
            failure: function (response) {
                alert("failure: " + response.d);
            },
            error: function (xhr, textStatus, error) {

                alert("Error: " + xhr.responseText);
            }
        });



    }
    catch (e) {
        alert(e.message);
    }
}




function parserJsonMultiple() {

    var jsonReturn = $("[id*=hdnField1]").val()
    debugger;

    var CheckMatch = "";
    var JsonDatas = $.parseJSON(jsonReturn);
    for (var n = 0; n < JsonDatas.length; n++) {
        addressArray = [];
        var json_obj = JsonDatas[n];
        var Parcel = json_obj.Parcel;
        //var parselNo = json_obj.Parcel.ParcelNumber;
        //var countyId = json_obj.Parcel.countyId;
        //var orderId = json_obj.Parcel.OrderId;
        //var address = json_obj.Parcel.Location + ", " + json_obj.Parcel.Town;
        //var address = json_obj.Parcel.Address;
        for (var i in Parcel) {
            addressDetails = {};
            addressDetails["orderid"] = Parcel[i].OrderId;
            addressDetails["address"] = Parcel[i].Address;
            addressDetails["parcel_no"] = Parcel[i].ParcelNumber;
            addressDetails["county_id"] = Parcel[i].countyId;
            addressDetails["OwnerInformation"] = Parcel[i].OwnerInformation;
            addressDetails["PropertyType"] = Parcel[i].PropertyType;
            addressDetails["MailingAddress"] = Parcel[i].MailingAddress;
            addressDetails["FolioNumber"] = Parcel[i].FolioNumber;
            addressDetails["AccountNumber"] = Parcel[i].AccountNumber;
            addressArray.push(addressDetails);
        }
        var addrDetails = addressArray;

        realInfoArray = [];
        var otherRealEstate = json_obj.otherRealInfo;
        for (var i in otherRealEstate) {
            otherRealInfo = {};
            otherRealInfo["Ryear"] = otherRealEstate[i].otherYear;
            otherRealInfo["RfaceValue"] = otherRealEstate[i].otherFaceValue;
            otherRealInfo["RcertificateValue"] = otherRealEstate[i].otherCertificateValue;
            otherRealInfo["Rstatus"] = otherRealEstate[i].otherStatus;
            otherRealInfo["RamtPaid"] = otherRealEstate[i].otherAmtPaid;
            realInfoArray.push(otherRealInfo);
        }
        var realInfoDetails = realInfoArray;



        delinquentDetailsArray = [];
        var paymentDetail = json_obj.PaymentDetail;
        for (var i in paymentDetail) {
            delinquentDetailsObject = {};
            delinquentDetailsObject["amount_paid"] = paymentDetail[i].AmountPaid;
            delinquentDetailsObject["payNo"] = parseInt(paymentDetail[i].Number);
            delinquentDetailsObject["due_charges"] = paymentDetail[i].DueCharges;
            delinquentDetailsObject["payment_posted"] = paymentDetail[i].PaymentPosted;
            delinquentDetailsObject["status"] = paymentDetail[i].Status;
            delinquentDetailsObject["installment"] = paymentDetail[i].Installment;
            delinquentDetailsObject["original_bill_amount"] = paymentDetail[i].OriginalBillAmount;
            delinquentDetailsObject["Ownername"] = paymentDetail[i].Ownername;
            delinquentDetailsObject["Folio"] = paymentDetail[i].Folio;
            delinquentDetailsObject["Paid"] = paymentDetail[i].Paid;
            delinquentDetailsObject["UnPaid"] = paymentDetail[i].UnPaid;
            delinquentDetailsObject["DelinquentDate"] = paymentDetail[i].DelinquentDate;
            //delinquentDetailsObject["TotalDue"] = paymentDetail[i].TotalDue;
            delinquentDetailsArray.push(delinquentDetailsObject);
        }
        var delinqDetails = delinquentDetailsArray;

        exemptionArray = [];
        var PropertyValue = json_obj.PropertyValue;
        for (var i in PropertyValue) {
            exemptionDetails = {};
            exemptionDetails["land"] = PropertyValue[i].Land;
            exemptionDetails["improvements"] = PropertyValue[i].Improvements;
            exemptionDetails["total_assess_value"] = PropertyValue[i].TotalAssessValue;
            exemptionDetails["net_assess_value"] = PropertyValue[i].NetAssessValue;
            exemptionDetails["exemp_value_new_contruction"] = PropertyValue[i].ExcemptionValue;
            exemptionDetails["construction_supp_value"] = parseInt(PropertyValue[i].ConstructionSuppValue);
            exemptionDetails["Type"] = PropertyValue[i].Type;
            exemptionDetails["Building_Value"] = PropertyValue[i].Building_Value;
            exemptionDetails["Year"] = PropertyValue[i].Year;
            exemptionDetails["ExtraFeaturevalue"] = PropertyValue[i].ExtraFeaturevalue;
            exemptionDetails["HouseholdPersonalProperty"] = PropertyValue[i].HouseholdPersonalProperty;
            exemptionDetails["BusinessPersonalProperty"] = PropertyValue[i].BusinessPersonalProperty;
            exemptionDetails["OtherExcemption"] = PropertyValue[i].OtherExcemption;
            exemptionDetails["MarketValue"] = PropertyValue[i].MarketValue;
            exemptionArray.push(exemptionDetails);
        }
        var exemptDetails = exemptionArray;

        payementArray = [];
        var Payment = json_obj.Payment;
        for (var i in Payment) {
            payementHistory = {};
            payementHistory["current_payments"] = Payment[i].CurrentCalendarYearPayment;
            payementHistory["last_payment_date"] = Payment[i].PaymentDate;
            payementHistory["prior_payments"] = Payment[i].PriorCalendarYearPayment;
            payementHistory["fiscal_tax_payments"] = Payment[i].FiscalTaxYearPayment;
            payementHistory["last_payment_amt"] = Payment[i].PaymentAmount;
            payementHistory["tax_before_payment"] = Payment[i].TaxBeforePayment;
            payementHistory["bill_type"] = Payment[i].BillType;
            payementHistory["ReceiptNumber"] = Payment[i].ReceiptNumber;
            payementHistory["FaceAmount"] = Payment[i].FaceAmount;
            payementHistory["Bid"] = Payment[i].Bid;
            payementHistory["PaidBy"] = Payment[i].PaidBy;
            payementHistory["EffectiveDate"] = Payment[i].EffectiveDate;
            payementHistory["Status"] = Payment[i].Status;
            payementHistory["Balance"] = Payment[i].Balance;
            payementHistory["BillNumber"] = Payment[i].BillNumber;
            payementArray.push(payementHistory);
        }
        var payDetails = payementArray;

        propertyDetailsArray = [];
        var RealProperty = json_obj.RealProperty;
        for (var z in RealProperty) {
            propertyDetails = {};
            propertyDetails["fiscal_value"] = RealProperty[z].FiscalYear;
            propertyDetails["improvements"] = RealProperty[z].Improvements;
            propertyDetails["gross_assessed"] = RealProperty[z].GrossAssessed;
            propertyDetails["total_ass_value"] = RealProperty[z].TotalAssessValue;
            propertyDetails["exempt"] = RealProperty[z].Exempt;
            propertyDetails["personal_property"] = RealProperty[z].PersonalProperty;
            propertyDetails["total_taxable_value"] = RealProperty[z].TotalTaxableValue;
            propertyDetails["land"] = RealProperty[z].Land;
            propertyDetails["tax_land_imp"] = RealProperty[z].Taxable;
            propertyDetails["allocation_assd"] = RealProperty[z].ElementAllocation;
            propertyDetails["Millage"] = RealProperty[z].Millage;
            propertyDetails["TaxingAuthority"] = RealProperty[z].TaxingAuthority;
            propertyDetails["LevyingAuthority"] = RealProperty[z].LevyingAuthority;
            propertyDetails["Rate"] = RealProperty[z].Rate;
            propertyDetails["Amount"] = RealProperty[z].Amount;
            propertyDetails["Type"] = RealProperty[z].Type;
            propertyDetails["AssessmentDescription"] = RealProperty[z].AssessmentDescription;
            propertyDetails["Units"] = RealProperty[z].Units;
            propertyDetails["Taxes"] = RealProperty[z].Taxes;
            propertyDetails["AssessmentTotal"] = RealProperty[z].AssessmentTotal;
            propertyDetails["OtherExempt"] = RealProperty[z].OtherExempt;
            propertyDetails["Credit"] = RealProperty[z].Credit;
            propertyDetails["Savings"] = RealProperty[z].Savings;
            propertyDetails["Taxingcode"] = RealProperty[z].Taxingcode;
            propertyDetails["Levyingcode"] = RealProperty[z].Levyingcode;
            propertyDetails["SpecialAssessment"] = RealProperty[z].SpecialAssessment;
            propertyDetailsArray.push(propertyDetails);
        }
        var propDetails = propertyDetailsArray;


        debugger
        taxpaymentsArray = [];
        var Tax = json_obj.Tax;
        for (var z in Tax) {
            taxpaymentsDetails = {};
            taxpaymentsDetails["taxes_assess"] = Tax[z].TaxesAssessed;
            taxpaymentsDetails["less_cap_reduction"] = Tax[z].LessCapReduction;
            taxpaymentsDetails["net_taxes"] = Tax[z].NetTaxes;
            taxpaymentsDetails["ExemptionAmount"] = Tax[z].ExemptionAmount;
            taxpaymentsDetails["CombinedTaxesAndAssessment"] = Tax[z].CombinedTaxesAndAssessment;
            taxpaymentsDetails["CheckPayable"] = Tax[z].CheckPayable;
            taxpaymentsDetails["GrossTax"] = Tax[z].GrossTax;
            taxpaymentsArray.push(taxpaymentsDetails);
        }
        var taxDetails = taxpaymentsArray;

        AmountDueArray = [];
        var AmountDue = json_obj.DelinquentDetails;
        for (var z in AmountDue) {
            AmountDueDetails = {};
            AmountDueDetails["tyear"] = AmountDue[z].Year;
            AmountDueDetails["chargecat"] = AmountDue[z].ChargeCategory;
            AmountDueDetails["district"] = AmountDue[z].District;
            AmountDueDetails["charge"] = AmountDue[z].Charge;
            AmountDueDetails["mindue"] = AmountDue[z].MinimumDue;
            AmountDueDetails["baldue"] = AmountDue[z].BalanceDue;
            AmountDueDetails["datedue"] = AmountDue[z].DateDue;
            AmountDueDetails["Period"] = AmountDue[z].Period;
            AmountDueArray.push(AmountDueDetails);
        }
        var AmntDueDetails = AmountDueArray;


        amgArray = [];
        var amg1 = json_obj.ParcelDetail;
        var amg11 = json_obj.ParcelDescription;

        if (Object.keys(amg1).length > 0) {
            for (var z in amg1) {
                amgDetail1 = {};
                amgDetail1["amgfound"] = parseInt(json_obj.AmgFound);
                amgDetail1["district_amgid"] = amg1[z].DistrictAMGID;
                amgDetail1["name"] = amg1[z].Name;
                amgDetail1["sttus"] = amg1[z].Status;
                amgDetail1["unbill_prinicipal"] = amg1[z].UnbilledPrincipal;
                if (Object.keys(amg11).length > 0) {
                    amgDetail1["legaldescription"] = amg11[z].LegalDescription;
                    amgDetail1["original_assesment"] = amg11[z].OriginalAssessment;
                    amgDetail1["payoff"] = amg11[z].Payoff;
                }
                amgArray.push(amgDetail1);
            }
        }
        else if (Object.keys(amg11).length > 0) {
            for (var z in amg11) {
                amgDetail1 = {};
                if (Object.keys(amg1).length > 0) {
                    amgDetail1["amgfound"] = parseInt(json_obj.AmgFound);
                    amgDetail1["district_amgid"] = amg1[z].DistrictAMGID;
                    amgDetail1["name"] = amg1[z].Name;
                    amgDetail1["sttus"] = amg1[z].Status;
                    amgDetail1["unbill_prinicipal"] = amg1[z].UnbilledPrincipal;
                }
                amgDetail1["legaldescription"] = amg11[z].LegalDescription;
                amgDetail1["original_assesment"] = amg11[z].OriginalAssessment;
                amgDetail1["payoff"] = amg11[z].Payoff;
                amgArray.push(amgDetail1);
            }
        }
        var AmgtDetails = amgArray;

        var TotalParcelDue = json_obj.ParcelDue;
        TotalParcelDueArray = [];
        for (var z in TotalParcelDue) {
            ParcelDueDetails = {};
            ParcelDueDetails["amgfound"] = parseInt(json_obj.AmgFound);
            ParcelDueDetails["due_status"] = TotalParcelDue[z].DueStatus;
            ParcelDueDetails["principal"] = TotalParcelDue[z].Principal;
            ParcelDueDetails["interest"] = TotalParcelDue[z].Interest;
            ParcelDueDetails["penalty"] = TotalParcelDue[z].Penalty;
            ParcelDueDetails["other"] = TotalParcelDue[z].Other;
            ParcelDueDetails["total_due"] = TotalParcelDue[z].TotalDue;
            ParcelDueDetails["BillYear"] = TotalParcelDue[z].BillYear;
            ParcelDueDetails["BillType"] = TotalParcelDue[z].BillType;
            ParcelDueDetails["BillNumber"] = TotalParcelDue[z].BillNumber;
            ParcelDueDetails["GrossTax"] = TotalParcelDue[z].GrossTax;
            ParcelDueDetails["Discount"] = TotalParcelDue[z].Discount;
            ParcelDueDetails["InstallmentAmount"] = TotalParcelDue[z].InstallmentAmount;
            ParcelDueDetails["Installement"] = TotalParcelDue[z].Installement;
            ParcelDueDetails["DueDate"] = TotalParcelDue[z].DueDate;
            ParcelDueDetails["AdValoremTax"] = TotalParcelDue[z].AdValoremTax;
            ParcelDueDetails["SpecialAssessments"] = TotalParcelDue[z].SpecialAssessments;
            ParcelDueDetails["Account"] = TotalParcelDue[z].Account;
            ParcelDueDetails["Folio"] = TotalParcelDue[z].Folio;
            ParcelDueDetails["Taxes"] = TotalParcelDue[z].Taxes;
            ParcelDueDetails["Fees"] = TotalParcelDue[z].Fees;
            ParcelDueDetails["Paid"] = TotalParcelDue[z].Paid;
            ParcelDueDetails["Type"] = TotalParcelDue[z].Type;
            ParcelDueDetails["PaidDate"] = TotalParcelDue[z].PaidDate;
            ParcelDueDetails["TaxableValue"] = TotalParcelDue[z].TaxableValue;
            ParcelDueDetails["DeedName"] = TotalParcelDue[z].DeedName;
            ParcelDueDetails["Period"] = TotalParcelDue[z].Period;
            TotalParcelDueArray.push(ParcelDueDetails);
        }

        var TotalParcelDueDetails = TotalParcelDueArray;

        var SummaryDetails = json_obj.ValueSummary;
        SummaryDetailsArray = [];
        for (var z in SummaryDetails) {
            SummaryObj = {};
            SummaryObj["TaxingDistrict"] = SummaryDetails[z].TaxingDistrict;
            SummaryObj["County"] = SummaryDetails[z].County;
            SummaryObj["SchoolBoard"] = SummaryDetails[z].SchoolBoard;
            SummaryObj["Municipal"] = SummaryDetails[z].Municipal;
            SummaryObj["Others"] = SummaryDetails[z].Others;
            SummaryObj["City"] = SummaryDetails[z].City;
            SummaryObj["Regional"] = SummaryDetails[z].Regional;
            SummaryDetailsArray.push(SummaryObj);
        }
        var TotalSummaryDetails = SummaryDetailsArray;

        var dataCheck = json_obj.hasOwnProperty('TaxCertificate');
        CertificateDetailsArray = [];
        if (dataCheck == true) {
            var CertificateDetails = json_obj.TaxCertificate;
            for (var z in CertificateDetails) {
                CertificateObj = {};
                CertificateObj["TaxYear"] = CertificateDetails[z].TaxYear;
                CertificateObj["Folio"] = CertificateDetails[z].Folio;
                CertificateObj["CertificateYear"] = CertificateDetails[z].CertificateYear;
                CertificateObj["CertificateNumber"] = CertificateDetails[z].CertificateNumber;
                CertificateObj["CertificateHolderName"] = CertificateDetails[z].CertificateHolderName;
                CertificateObj["TDAnumber"] = CertificateDetails[z].TDAnumber;
                CertificateDetailsArray.push(CertificateObj);
            }
        }
        var TaxCertificateDetails = CertificateDetailsArray;


        //var obj1 = $.parseJSON(JSON.stringify({ Address: addrDetails }));
        obj1 = {};
        obj1["Address"] = JSON.stringify(addrDetails);
        obj1["DelinArray"] = JSON.stringify(delinqDetails);
        obj1["Exemption"] = JSON.stringify(exemptDetails);
        obj1["Payment"] = JSON.stringify(payDetails);
        obj1["PropertyArray"] = JSON.stringify(propDetails);
        obj1["Taxpayments"] = JSON.stringify(taxDetails);
        obj1["AmountDueDetail"] = JSON.stringify(AmntDueDetails);
        obj1["AmgtDetail"] = JSON.stringify(AmgtDetails);
        obj1["DueDetail"] = JSON.stringify(TotalParcelDueDetails);
        obj1["SummaryDetail"] = JSON.stringify(TotalSummaryDetails);
        obj1["CertificateDetails"] = JSON.stringify(TaxCertificateDetails);
        obj1["RestateDetail"] = JSON.stringify(realInfoDetails);



        $.ajax({
            type: "POST",
            url: "../Pages/scrap.aspx/insertData",
            data: JSON.stringify(obj1),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                // alert(response.d);
                objResultSet = response.d;

                if (n == JsonDatas.length - 1) {
                    if (objResultSet == "1") {
                        alert("Data Inserted successfully");
                        fncBind();
                    }
                    else if (objResultSet == "0") {
                        alert("Data not available");
                    }
                    else if (objResultSet == "2") {
                        alert("Data not available check for the given input");
                    }
                }
            },
            failure: function (response) {
                alert("failure: " + response.d);
            },
            error: function (xhr, textStatus, error) {
                alert("Error: " + error);
            }
        });

    }

}





