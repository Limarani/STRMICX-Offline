using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Response
/// </summary>
public class Response
{
    DBConnection dbconn = new DBConnection();
    int? value = null;
    decimal? decimalnull = null;
    DateTime? NullDate = null;
    bool? boolnull = null;
    string stringnull = null;
    string blankDate = "1900-01-01";
    string blankDateTime = "1900-01-01T00:00:00Z";
    TaxAgencyDetailsTag TaxAgency = new TaxAgencyDetailsTag();
    public string GetJsonData(string orderno)
    {
        JsonOutputTag JsonOutput = new JsonOutputTag();
        JsonOutput.Response = GetResponseTagData(orderno);
        JsonOutput.Response.OrderDetail = GetOrderDetailTagData(orderno);
        JsonOutput.Response.OrderDetail.TaxDetail = GetTaxDetailTagData(orderno);

        JsonOutput.Response.OrderDetail.TaxDetail.TaxParcels = GetTaxParcelTagData(orderno);
        JsonOutput.Response.OrderDetail.TaxVendor = GetTaxVendorTagData(orderno);
        string JsonResult = JsonConvert.SerializeObject(JsonOutput);
        return JsonResult;
    }


    public ResponseTag GetResponseTagData(string orderno)
    {
        ResponseTag Response = new ResponseTag();
        DataSet ds = dbconn.ExecuteQuery("select message_type_code from tbl_order_details where OrderDetailId='" + orderno + "'");
        //read data from dataset 
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            //Response.MessageTypeCode = ds.Tables[0].Rows[i]["message_type_code"] != DBNull.Value ? ds.Tables[0].Rows[i]["message_type_code"].ToString() : null;
            Response.MessageTypeCode = "COMPLETE";
        }

        return Response;
    }

    public OrderDetailTag GetOrderDetailTagData(string orderno)
    {
        OrderDetailTag orderDetail = new OrderDetailTag();

        DataSet ds = dbconn.ExecuteQuery("select * from tbl_order_details where OrderDetailId='" + orderno + "'");
        DataSet dsNotes = dbconn.ExecuteQuery("select * from tbl_notes where Orderno='" + orderno + "'");
        //read data from dataset 
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            orderDetail.OrderDetailId = Convert.ToInt64(ds.Tables[0].Rows[i]["OrderDetailId"]);

        }
        for (int i = 0; i < dsNotes.Tables[0].Rows.Count; i++)
        {
            string note = dsNotes.Tables[0].Rows[i]["note"].ToString();
            orderDetail.NoteText = orderDetail.NoteText + "\n" + note;
        }

        return orderDetail;
    }

    public TaxDetailTag GetTaxDetailTagData(string orderno)
    {
        TaxDetailTag taxDetailTag = new TaxDetailTag();
        //read data from dataset 
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_order_details where OrderDetailId='" + orderno + "'");
        DataSet dsStatus = dbconn.ExecuteQuery("select id,orderstatus from tbl_taxcert_info where Orderno='" + orderno + "' order by id desc limit 1");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            taxDetailTag.ExpectedDate = ds.Tables[0].Rows[i]["expecteddate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["expecteddate"]).Date.ToString() : blankDate.ToString();
            taxDetailTag.FollowUpDate = ds.Tables[0].Rows[i]["followupdate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["followupdate"]).Date.ToString() : blankDateTime.ToString();

            taxDetailTag.WasAssessedAsLand = false;//not sending
            taxDetailTag.WasAssessedAsHomestead = false;
        }

        for (int i = 0; i < dsStatus.Tables[0].Rows.Count; i++)
        {
            taxDetailTag.StatusCode = dsStatus.Tables[0].Rows[i]["orderstatus"].ToString();
        }
        return taxDetailTag;
    }


    public List<TaxParcelTag> GetTaxParcelTagData(string orderno)
    {
        List<TaxParcelTag> TaxParcelList = new List<TaxParcelTag> { };
        TaxParcelTag TaxParcel = new TaxParcelTag();
        //read data from dataset 
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_taxparcel where orderno='" + orderno + "'");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            TaxParcel.TaxYear = ds.Tables[0].Rows[i]["taxyear"] != DBNull.Value ? ds.Tables[0].Rows[i]["taxyear"].ToString() : null;
            TaxParcel.EndYear = ds.Tables[0].Rows[i]["endyear"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["endyear"]) : value;
            TaxParcel.TaxId = ds.Tables[0].Rows[i]["taxid"] != DBNull.Value ? ds.Tables[0].Rows[i]["taxid"].ToString() : null;
            TaxParcel.IsToBeDetermined = ds.Tables[0].Rows[i]["tbd"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["tbd"]) : false;
            TaxParcel.IsEstimate = ds.Tables[0].Rows[i]["estimate"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["estimate"]) : false;
            TaxParcel.TaxAgencyDetails = GetTaxAgencyDetailsTagData(orderno);
            TaxParcel.SpecialAssessments = GetSpecialAssessmentsList(orderno);
            TaxParcelList.Add(TaxParcel);
        }
        return TaxParcelList;
    }



    public List<TaxAgencyDetailsTag> GetTaxAgencyDetailsTagData(string orderno)
    {

        List<TaxAgencyDetailsTag> TaxAgencyDetails = new List<TaxAgencyDetailsTag>() { };
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_taxauthorities2 where Orderno='" + orderno + "'");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            TaxAgency.AgencyId = ds.Tables[0].Rows[i]["AgencyId"].ToString();
            TaxAgency.TaxAgencyType = ds.Tables[0].Rows[i]["TaxAgencyType"].ToString();
            if (ds.Tables[0].Rows[i]["IsDelinquent"].ToString() == "Yes")
            {
                TaxAgency.IsDelinquent = true;
            }
            else
            {
                TaxAgency.IsDelinquent = false;
            }
            //TaxAgency.IsDelinquent = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsDelinquent"]);
            TaxAgency.DelinquentAgencyId = ds.Tables[0].Rows[i]["DelinquencyAgencyId"].ToString();
            TaxAgency.BillingFrequency = ds.Tables[0].Rows[i]["taxfrequency"].ToString();
            TaxAgency.NextBillDate1 = ds.Tables[0].Rows[i]["nextbilldate1"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["nextbilldate1"]) : null;
            TaxAgency.NextBillDate2 = ds.Tables[0].Rows[i]["nextbilldate2"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["nextbilldate2"]) : null;
            TaxAgency.DelinquentDate1 = ds.Tables[0].Rows[i]["DelinquentDate1"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DelinquentDate1"]) : null;
            TaxAgency.DelinquentDate2 = ds.Tables[0].Rows[i]["DelinquentDate2"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DelinquentDate2"]) : null;
            TaxAgency.DelinquentDate3 = ds.Tables[0].Rows[i]["DelinquentDate3"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DelinquentDate3"]) : null;
            TaxAgency.DelinquentDate4 = ds.Tables[0].Rows[i]["DelinquentDate4"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DelinquentDate4"]) : null;
            TaxAgency.DiscountAmount1 = ds.Tables[0].Rows[i]["Discountamount1"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Discountamount1"]) : decimalnull;
            TaxAgency.DiscountAmount2 = ds.Tables[0].Rows[i]["Discountamount2"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Discountamount2"]) : decimalnull;
            TaxAgency.DiscountAmount3 = ds.Tables[0].Rows[i]["Discountamount3"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Discountamount3"]) : decimalnull;
            TaxAgency.DiscountAmount4 = ds.Tables[0].Rows[i]["Discountamount4"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Discountamount4"]) : decimalnull;
            TaxAgency.DiscountDate1 = ds.Tables[0].Rows[i]["DiscountDate1"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DiscountDate1"]) : null;
            TaxAgency.DiscountDate2 = ds.Tables[0].Rows[i]["DiscountDate2"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DiscountDate2"]) : null;
            TaxAgency.DiscountDate3 = ds.Tables[0].Rows[i]["DiscountDate3"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DiscountDate3"]) : null;
            TaxAgency.DiscountDate4 = ds.Tables[0].Rows[i]["DiscountDate4"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DiscountDate2"]) : null;
            TaxAgency.InstallmentDate1 = ds.Tables[0].Rows[i]["duedate1"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["duedate1"]) : null;
            TaxAgency.InstallmentDate2 = ds.Tables[0].Rows[i]["duedate2"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["duedate2"]) : null;
            TaxAgency.InstallmentDate3 = ds.Tables[0].Rows[i]["duedate3"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["duedate3"]) : null;
            TaxAgency.InstallmentDate4 = ds.Tables[0].Rows[i]["duedate4"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["duedate4"]) : null;
            TaxAgency.InstallmentAmount1 = Convert.ToDecimal(ds.Tables[0].Rows[i]["Instamount1"]);
            TaxAgency.InstallmentAmount2 = ds.Tables[0].Rows[i]["Instamount2"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Instamount2"]) : decimalnull;
            TaxAgency.InstallmentAmount3 = ds.Tables[0].Rows[i]["Instamount3"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Instamount3"]) : decimalnull;
            TaxAgency.InstallmentAmount4 = ds.Tables[0].Rows[i]["Instamount4"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Instamount4"]) : decimalnull;
            TaxAgency.InstallmentAmountPaid1 = Convert.ToDecimal(ds.Tables[0].Rows[i]["Instamountpaid1"]);
            TaxAgency.InstallmentAmountPaid2 = ds.Tables[0].Rows[i]["Instamountpaid2"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Instamountpaid2"]) : decimalnull;
            TaxAgency.InstallmentAmountPaid3 = ds.Tables[0].Rows[i]["Instamountpaid3"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Instamountpaid3"]) : decimalnull;
            TaxAgency.InstallmentAmountPaid4 = ds.Tables[0].Rows[i]["Instamountpaid4"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Instamountpaid4"]) : decimalnull;
            TaxAgency.Installment1PaidOrDue = ds.Tables[0].Rows[i]["InstPaidDue1"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["InstPaidDue1"]) : null;
            TaxAgency.Installment2PaidOrDue = ds.Tables[0].Rows[i]["InstPaidDue2"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["InstPaidDue2"]) : null;
            TaxAgency.Installment3PaidOrDue = ds.Tables[0].Rows[i]["InstPaidDue3"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["InstPaidDue3"]) : null;
            TaxAgency.Installment4PaidOrDue = ds.Tables[0].Rows[i]["InstPaidDue4"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["InstPaidDue4"]) : null;
            TaxAgency.RemainingBalance1 = ds.Tables[0].Rows[i]["Remainingbalance1"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Remainingbalance1"]) : decimalnull;
            TaxAgency.RemainingBalance2 = ds.Tables[0].Rows[i]["Remainingbalance2"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Remainingbalance2"]) : decimalnull;
            TaxAgency.RemainingBalance3 = ds.Tables[0].Rows[i]["Remainingbalance3"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Remainingbalance3"]) : decimalnull;
            TaxAgency.RemainingBalance4 = ds.Tables[0].Rows[i]["Remainingbalance4"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Remainingbalance4"]) : decimalnull;
            TaxAgency.TaxBill = ds.Tables[0].Rows[i]["taxbill"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["taxbill"]) : null;
            TaxAgency.TaxRate = "0.00";//need to clarify
            TaxAgency.TaxableValue = 0;//need to clarify
            TaxAgency.Comments = ds.Tables[0].Rows[i]["installmentcomments"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["installmentcomments"]) : null;

            if (ds.Tables[0].Rows[i]["ExemptRelevy1"].ToString() == "Yes")
            {
                TaxAgency.IsInstallmentExempt1 = true;
            }
            else
            {
                TaxAgency.IsInstallmentExempt1 = false;
            }

            if (ds.Tables[0].Rows[i]["ExemptRelevy2"].ToString() == "Yes")
            {
                TaxAgency.IsInstallmentExempt2 = true;
            }
            else
            {
                TaxAgency.IsInstallmentExempt2 = false;
            }

            if (ds.Tables[0].Rows[i]["ExemptRelevy3"].ToString() == "Yes")
            {
                TaxAgency.IsInstallmentExempt3 = true;
            }
            else
            {
                TaxAgency.IsInstallmentExempt3 = false;
            }

            if (ds.Tables[0].Rows[i]["ExemptRelevy4"].ToString() == "Yes")
            {
                TaxAgency.IsInstallmentExempt4 = true;
            }
            else
            {
                TaxAgency.IsInstallmentExempt4 = false;
            }

            //TaxAgency.IsInstallmentExempt1 = ds.Tables[0].Rows[i]["ExemptRelevy1"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["ExemptRelevy1"]) : boolnull;
            //TaxAgency.IsInstallmentExempt2 = ds.Tables[0].Rows[i]["ExemptRelevy2"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["ExemptRelevy2"]) : boolnull;
            //TaxAgency.IsInstallmentExempt3 = ds.Tables[0].Rows[i]["ExemptRelevy3"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["ExemptRelevy3"]) : boolnull;
            //TaxAgency.IsInstallmentExempt4 = ds.Tables[0].Rows[i]["ExemptRelevy4"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["ExemptRelevy4"]) : boolnull;
            TaxAgency.IsHomesteadExempt = false;//need to calrify
            TaxAgency.BillingPeriodStartDate = ds.Tables[0].Rows[i]["BillingPeriodStartDate"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["BillingPeriodStartDate"]) : null;
            TaxAgency.BillingPeriodEndDate = ds.Tables[0].Rows[i]["BillingPeriodEndDate"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["BillingPeriodEndDate"]) : null;
            TaxAgency.FutureTaxOption = ds.Tables[0].Rows[i]["FutureTaxOption"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["FutureTaxOption"]) : "";
            TaxAgency.WasFullyAssessedLastYear = ds.Tables[0].Rows[i]["primaryresidence"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["primaryresidence"]) : false;
            TaxAgency.AssessedValue = 0; //need to clarify
            TaxAgency.TaxExemptions = GetTaxExemptionList(orderno);
            TaxAgency.DelinquentTaxes = GetDelinquentTaxesList(orderno);
            TaxAgencyDetails.Add(TaxAgency);
        }
        return TaxAgencyDetails;

    }


    public List<TaxExemptionsTag> GetTaxExemptionList(string orderno)
    {
        TaxExemptionsTag TaxExemptions = new TaxExemptionsTag();
        List<TaxExemptionsTag> TaxExemptionList = new List<TaxExemptionsTag> { };
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_exemption_taxauthority where orderno='" + orderno + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                TaxExemptions.ExemptionTypeCode = ds.Tables[0].Rows[i]["exemptiontype"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["exemptiontype"]) : null;
                TaxExemptions.ExemptionAmount = ds.Tables[0].Rows[i]["exemptionamount"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["exemptionamount"]) : 0;
                TaxExemptionList.Add(TaxExemptions);
            }
        }
        else
        {
            TaxExemptions.ExemptionTypeCode = null; //non empty string???
            TaxExemptions.ExemptionAmount = 0;
            TaxExemptionList.Add(TaxExemptions);
        }
        return TaxExemptionList;
    }
    public List<DelinquentTaxesTag> GetDelinquentTaxesList(string orderno)
    {
        DelinquentTaxesTag DelinquentTaxes = new DelinquentTaxesTag();
        List<DelinquentTaxesTag> DelinquentTaxesList = new List<DelinquentTaxesTag> { };
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_deliquent where orderno='" + orderno + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DelinquentTaxes.IsCurrentDelinquency = TaxAgency.IsDelinquent;
                DelinquentTaxes.IsFullyPaidOff = ds.Tables[0].Rows[i]["IsFullyPaidOff"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[i]["IsFullyPaidOff"]) : false;
                DelinquentTaxes.AdditionalPenaltyAmount = ds.Tables[0].Rows[i]["AdditionalPenaltyAmount"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["AdditionalPenaltyAmount"]) : 0;
                DelinquentTaxes.AmountGoodThruDate = ds.Tables[0].Rows[i]["goodthuruDate"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["goodthuruDate"]) : null;
                DelinquentTaxes.InitialInstallmentDueDate = ds.Tables[0].Rows[i]["InstallmentDueDate"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["InstallmentDueDate"]) : null;
                DelinquentTaxes.BaseAmountDue = ds.Tables[0].Rows[i]["BaseAmountDue"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["BaseAmountDue"]) : 0;
                DelinquentTaxes.PayOffAmount = ds.Tables[0].Rows[i]["PayOffAmount"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["PayOffAmount"]) : 0;
                DelinquentTaxes.PenaltyAmount = ds.Tables[0].Rows[i]["PenaltyAmount"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["PenaltyAmount"]) : 0;
                DelinquentTaxes.PenaltyAmountFrequency = ds.Tables[0].Rows[i]["PenaltyAmountFrequency"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["PenaltyAmountFrequency"]) : null;
                DelinquentTaxes.PenaltyDueDate = ds.Tables[0].Rows[i]["PenaltyDueDate"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["PenaltyDueDate"]) : null;
                DelinquentTaxes.PercentOfPenaltyAmount = ds.Tables[0].Rows[i]["PercentofPenaltyAmount"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["PercentofPenaltyAmount"]) : 0;
                DelinquentTaxes.RollOverDate = ds.Tables[0].Rows[i]["RollOverDate"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["RollOverDate"]) : null;
                DelinquentTaxes.PerDiem = ds.Tables[0].Rows[i]["PerDiem"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["PerDiem"]) : 0;
                DelinquentTaxes.TaxYear = ds.Tables[0].Rows[i]["deliquenttaxyear"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["deliquenttaxyear"]) : null;
                DelinquentTaxes.Comments = ds.Tables[0].Rows[i]["Comments"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Comments"]) : null;
                DelinquentTaxes.PayeeName = ds.Tables[0].Rows[i]["Payee"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Payee"]) : null;
                DelinquentTaxes.AmountPaid = ds.Tables[0].Rows[i]["AmountPaid"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["AmountPaid"]) : 0;
                DelinquentTaxes.LatestPaymentDateTime = ds.Tables[0].Rows[i]["LatestPaymentDateTime"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["LatestPaymentDateTime"]) : null;
                DelinquentTaxes.TaxSaleDate = ds.Tables[0].Rows[i]["DateofTaxSale"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DateofTaxSale"]) : null;
                DelinquentTaxes.LastDayToRedeemDate = ds.Tables[0].Rows[i]["LastdaytoRedeem"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["LastdaytoRedeem"]) : null;

                if (ds.Tables[0].Rows[i]["TaxSaleNotApplicable"].ToString() == "Yes")
                {
                    DelinquentTaxes.TaxSalePending = false;
                }
                else
                {
                    DelinquentTaxes.TaxSalePending = true;
                }

                DelinquentTaxesList.Add(DelinquentTaxes);
            }
        }
        else
        {
            DelinquentTaxes.IsCurrentDelinquency = false;
            DelinquentTaxes.IsFullyPaidOff = false;
            DelinquentTaxes.AdditionalPenaltyAmount = 0;
            DelinquentTaxes.AmountGoodThruDate = null;
            DelinquentTaxes.InitialInstallmentDueDate = null;
            DelinquentTaxes.BaseAmountDue = 0;
            DelinquentTaxes.PayOffAmount = 0;
            DelinquentTaxes.PenaltyAmount = 0;
            DelinquentTaxes.PenaltyAmountFrequency = "";
            DelinquentTaxes.PenaltyDueDate = null;
            DelinquentTaxes.PercentOfPenaltyAmount = 0;
            DelinquentTaxes.RollOverDate = null;
            DelinquentTaxes.PerDiem = 0;
            DelinquentTaxes.TaxYear = "";
            DelinquentTaxes.Comments = "";
            DelinquentTaxes.PayeeName = "";
            DelinquentTaxes.AmountPaid = null;
            DelinquentTaxes.LatestPaymentDateTime = null;
            DelinquentTaxes.TaxSaleDate = null;
            DelinquentTaxes.LastDayToRedeemDate = null;
            DelinquentTaxesList.Add(DelinquentTaxes);
        }

        return DelinquentTaxesList;
    }

    public List<SpecialAssessmentsTag> GetSpecialAssessmentsList(string orderno)
    {
        List<SpecialAssessmentsTag> SpecialAssessmentsList = new List<SpecialAssessmentsTag>() { };
        SpecialAssessmentsTag SpecialAssessments = new SpecialAssessmentsTag();
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_specialassessment_authority where orderno='" + orderno + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                SpecialAssessments.Number = ds.Tables[0].Rows[i]["specialassessmentno"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["specialassessmentno"]) : null;
                SpecialAssessments.DueDate = ds.Tables[0].Rows[i]["DueDate"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["DueDate"]) : null;
                SpecialAssessments.Description = ds.Tables[0].Rows[i]["description"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["description"]) : null;
                SpecialAssessments.Amount = ds.Tables[0].Rows[i]["amount"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["amount"]) : 0;
                SpecialAssessments.NumberOfInstallments = ds.Tables[0].Rows[i]["noofinstallment"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["noofinstallment"]) : value;
                SpecialAssessments.InstallmentsPaid = ds.Tables[0].Rows[i]["installmentpaid"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["installmentpaid"]) : value;
                SpecialAssessments.InstallmentsRemaining = ds.Tables[0].Rows[i]["InstallmentsRemaining"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["InstallmentsRemaining"]) : value;
                SpecialAssessments.RemainingBalance = ds.Tables[0].Rows[i]["RemainingBalance"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["RemainingBalance"]) : 0;
                SpecialAssessments.PerDiem = ds.Tables[0].Rows[i]["PerDiem"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["PerDiem"]) : 0;
                SpecialAssessments.Payee = ds.Tables[0].Rows[i]["Payee"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Payee"]) : null;
                SpecialAssessments.GoodThroughDate = ds.Tables[0].Rows[i]["GoodThroughDate"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["GoodThroughDate"]) : null;
                SpecialAssessments.Comments = ds.Tables[0].Rows[i]["Comments"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Comments"]) : null;
                SpecialAssessmentsList.Add(SpecialAssessments);
            }
        }
        else
        {
            SpecialAssessments.Number = "";
            SpecialAssessments.DueDate = null;
            SpecialAssessments.Description = "";
            SpecialAssessments.Amount = null;
            SpecialAssessments.NumberOfInstallments = null;
            SpecialAssessments.InstallmentsPaid = null;
            SpecialAssessments.InstallmentsRemaining = null;
            SpecialAssessments.RemainingBalance = null;
            SpecialAssessments.PerDiem = null;
            SpecialAssessments.Payee = "";
            SpecialAssessments.GoodThroughDate = null;
            SpecialAssessments.Comments = "";
            SpecialAssessmentsList.Add(SpecialAssessments);
        }
        return SpecialAssessmentsList;
    }
    public TaxVendorTag GetTaxVendorTagData(string orderno)
    {
        TaxVendorTag Taxvendor = new TaxVendorTag();
        DataSet ds = dbconn.ExecuteQuery("select * from tax_vendor where order_no='" + orderno + "'");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            Taxvendor.VendorNumber = "88998152";
            Taxvendor.VendorResponse = ds.Tables[0].Rows[i]["vendor_response"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["vendor_response"]) : null;

            Taxvendor.TaxVendorService.TaxVendorServiceId = ds.Tables[0].Rows[i]["service_id"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["service_id"]) : 0;
            Taxvendor.TaxVendorService.ServiceCode = ds.Tables[0].Rows[i]["service_code"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["service_code"]) : null;
            Taxvendor.TaxVendorService.ActualCost = ds.Tables[0].Rows[i]["projected_cost"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["projected_cost"]) : 0;
            Taxvendor.TaxVendorService.StatusCode = null; //Completed
        }
        return Taxvendor;


    }

}

class JsonOutputTag
{
    public ResponseTag Response;

}
public class ResponseTag
{
    public string MessageTypeCode;
    public OrderDetailTag OrderDetail;
}
public class OrderDetailTag
{
    public Int64 OrderDetailId;
    public string NoteText;
    public TaxDetailTag TaxDetail;
    public TaxVendorTag TaxVendor;
}

public class TaxDetailTag
{
    public string ExpectedDate;
    public string FollowUpDate;
    public string StatusCode;
    public bool? WasAssessedAsLand;
    public bool WasAssessedAsHomestead;
    public List<TaxParcelTag> TaxParcels;
}

public class TaxParcelTag
{
    public string TaxYear;
    public int? EndYear;
    public string TaxId;
    public bool IsToBeDetermined;
    public bool IsEstimate;

    public List<TaxAgencyDetailsTag> TaxAgencyDetails;
    public List<SpecialAssessmentsTag> SpecialAssessments;
}

public class TaxAgencyDetailsTag
{
    public string AgencyId;
    public string TaxAgencyType;
    public bool IsDelinquent;
    public string DelinquentAgencyId;
    public string BillingFrequency;
    public string NextBillDate1;
    public string NextBillDate2;
    public string DelinquentDate1;
    public string DelinquentDate2;
    public string DelinquentDate3;
    public string DelinquentDate4;
    public Decimal? DiscountAmount1;
    public Decimal? DiscountAmount2;
    public Decimal? DiscountAmount3;
    public Decimal? DiscountAmount4;
    public string DiscountDate1;
    public string DiscountDate2;
    public string DiscountDate3;
    public string DiscountDate4;
    public string InstallmentDate1;
    public string InstallmentDate2;
    public string InstallmentDate3;
    public string InstallmentDate4;
    public Decimal InstallmentAmount1;
    public Decimal? InstallmentAmount2;
    public Decimal? InstallmentAmount3;
    public Decimal? InstallmentAmount4;
    public Decimal InstallmentAmountPaid1;
    public Decimal? InstallmentAmountPaid2;
    public Decimal? InstallmentAmountPaid3;
    public Decimal? InstallmentAmountPaid4;
    public string Installment1PaidOrDue;
    public string Installment2PaidOrDue;
    public string Installment3PaidOrDue;
    public string Installment4PaidOrDue;
    public Decimal? RemainingBalance1;
    public Decimal? RemainingBalance2;
    public Decimal? RemainingBalance3;
    public Decimal? RemainingBalance4;
    public string TaxBill;
    public string TaxRate;
    public Decimal? TaxableValue;
    public string Comments;
    public bool? IsInstallmentExempt1;
    public bool? IsInstallmentExempt2;
    public bool? IsInstallmentExempt3;
    public bool? IsInstallmentExempt4;
    public bool? IsHomesteadExempt;
    public string BillingPeriodStartDate;
    public string BillingPeriodEndDate;
    public string FutureTaxOption;
    public bool? WasFullyAssessedLastYear;
    public decimal? AssessedValue;

    public List<TaxExemptionsTag> TaxExemptions;
    public List<DelinquentTaxesTag> DelinquentTaxes;


}

public class TaxExemptionsTag
{
    public string ExemptionTypeCode;
    public decimal ExemptionAmount;
}

public class DelinquentTaxesTag
{
    public bool? IsCurrentDelinquency;
    public bool? IsFullyPaidOff;
    public Decimal? AdditionalPenaltyAmount;
    public string AmountGoodThruDate;
    public string InitialInstallmentDueDate;
    public Decimal? BaseAmountDue;
    public Decimal? PayOffAmount;
    public Decimal? PenaltyAmount;
    public string PenaltyAmountFrequency;
    public string PenaltyDueDate;
    public Decimal? PercentOfPenaltyAmount;
    public string RollOverDate;
    public Decimal? PerDiem;
    public string TaxYear;
    public string Comments;
    public string PayeeName;
    public Decimal? AmountPaid;
    public string LatestPaymentDateTime;
    public string TaxSaleDate;
    public string LastDayToRedeemDate;
    public bool? TaxSalePending;
}

public class SpecialAssessmentsTag
{
    public string Number;
    public string DueDate;
    public string Description;
    public Decimal? Amount;
    public int? NumberOfInstallments;
    public int? InstallmentsPaid;
    public int? InstallmentsRemaining;
    public Decimal? RemainingBalance;
    public Decimal? PerDiem;
    public string Payee;
    public string GoodThroughDate;
    public string Comments;
}

public class TaxVendorTag
{
    public string VendorNumber;
    public string VendorResponse;
    public TaxVendorServiceTag TaxVendorService = new TaxVendorServiceTag();
}
public class TaxVendorServiceTag
{
    public Int32 TaxVendorServiceId;
    public string ServiceCode;
    public decimal ActualCost;
    public string StatusCode;

}