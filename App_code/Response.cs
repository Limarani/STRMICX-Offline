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


    public string GetJsonData(string orderno)
    {
        JsonOutputTag JsonOutput = new JsonOutputTag();
        JsonOutput.Response = GetResponseTagData(orderno);
        JsonOutput.Response.OrderDetail = GetOrderDetailTagData(orderno);
        JsonOutput.Response.OrderDetail.TaxDetail = GetTaxDetailTagData(orderno);

        JsonOutput.Response.OrderDetail.TaxDetail.TaxParcel = GetTaxParcelTagData(orderno);
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
            Response.MessageTypeCode = ds.Tables[0].Rows[i]["message_type_code"].ToString();
        }

        return Response;
    }

    public OrderDetailTag GetOrderDetailTagData(string orderno)
    {
        OrderDetailTag orderDetail = new OrderDetailTag();

        DataSet ds = dbconn.ExecuteQuery("select * from tbl_order_details where OrderDetailId='" + orderno + "'");
        //read data from dataset 
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            orderDetail.OrderDetailId = ds.Tables[0].Rows[i]["OrderDetailId"].ToString();
            orderDetail.NoteText = "";
        }
        return orderDetail;
    }

    public TaxDetailTag GetTaxDetailTagData(string orderno)
    {
        TaxDetailTag taxDetailTag = new TaxDetailTag();
        //read data from dataset 
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_order_details where OrderDetailId='" + orderno + "'");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            taxDetailTag.ExpectedDate = ds.Tables[0].Rows[i]["expecteddate"].ToString();
            taxDetailTag.FollowUpDate = ds.Tables[0].Rows[i]["followupdate"].ToString();
            taxDetailTag.StatusCode = "";
            taxDetailTag.WasAssessedAsLand = "";
            taxDetailTag.WasAssessedAsHomestead = "";
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
            TaxParcel.TaxYear = ds.Tables[0].Rows[i]["taxyear"].ToString();
            TaxParcel.EndYear = ds.Tables[0].Rows[i]["endyear"].ToString();
            TaxParcel.TaxId = ds.Tables[0].Rows[i]["taxid"].ToString();
            TaxParcel.IsToBeDetermined = "";
            TaxParcel.IsEstimate = "";
            TaxParcel.TaxAgencyDetails = GetTaxAgencyDetailsTagData(orderno);
            TaxParcel.SpecialAssessments = GetSpecialAssessmentsList(orderno);
            TaxParcelList.Add(TaxParcel);
        }
        return TaxParcelList;
    }



    public List<TaxAgencyDetailsTag> GetTaxAgencyDetailsTagData(string orderno)
    {
        TaxAgencyDetailsTag TaxAgency = new TaxAgencyDetailsTag();
        List<TaxAgencyDetailsTag> TaxAgencyDetails = new List<TaxAgencyDetailsTag>() { };
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_taxauthorities1 where Orderno='" + orderno + "'");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            TaxAgency.AgencyId = ds.Tables[0].Rows[i]["AgencyId"].ToString();
            TaxAgency.TaxAgencyType = ds.Tables[0].Rows[i]["TaxAgencyType"].ToString();
            TaxAgency.IsDelinquent = ds.Tables[0].Rows[i]["IsDelinquent"].ToString();
            TaxAgency.DelinquentAgencyId = ds.Tables[0].Rows[i]["DelinquencyAgencyId"].ToString();
            TaxAgency.BillingFrequency = ds.Tables[0].Rows[i]["paymentfrequency"].ToString();
            TaxAgency.NextBillDate1 = ds.Tables[0].Rows[i]["BillingDate1"].ToString();
            TaxAgency.NextBillDate2 = ds.Tables[0].Rows[i]["BillingDate2"].ToString();
            TaxAgency.DelinquentDate1 = ds.Tables[0].Rows[i]["DelinquencyDate1"].ToString();
            TaxAgency.DelinquentDate2 = ds.Tables[0].Rows[i]["DelinquencyDate2"].ToString();
            TaxAgency.DelinquentDate3 = ds.Tables[0].Rows[i]["DelinquencyDate3"].ToString();
            TaxAgency.DelinquentDate4 = ds.Tables[0].Rows[i]["DelinquencyDate4"].ToString();
            TaxAgency.DiscountAmount1 = ds.Tables[0].Rows[i]["Discountamount1"].ToString();
            TaxAgency.DiscountAmount2 = ds.Tables[0].Rows[i]["Discountamount2"].ToString();
            TaxAgency.DiscountAmount3 = ds.Tables[0].Rows[i]["Discountamount3"].ToString();
            TaxAgency.DiscountAmount4 = ds.Tables[0].Rows[i]["Discountamount4"].ToString();
            TaxAgency.DiscountDate1 = ds.Tables[0].Rows[i]["DiscountDate1"].ToString();
            TaxAgency.DiscountDate2 = ds.Tables[0].Rows[i]["DiscountDate2"].ToString();
            TaxAgency.DiscountDate3 = ds.Tables[0].Rows[i]["DiscountDate3"].ToString();
            TaxAgency.DiscountDate4 = ds.Tables[0].Rows[i]["DiscountDate4"].ToString();
            TaxAgency.InstallmentDate1 = ds.Tables[0].Rows[i]["Installmentdate1"].ToString();
            TaxAgency.InstallmentDate2 = ds.Tables[0].Rows[i]["Installmentdate2"].ToString();
            TaxAgency.InstallmentDate3 = ds.Tables[0].Rows[i]["Installmentdate3"].ToString();
            TaxAgency.InstallmentDate4 = ds.Tables[0].Rows[i]["Installmentdate4"].ToString();
            TaxAgency.InstallmentAmount1 = ds.Tables[0].Rows[i]["Instamount1"].ToString();
            TaxAgency.InstallmentAmount2 = ds.Tables[0].Rows[i]["Instamount2"].ToString();
            TaxAgency.InstallmentAmount3 = ds.Tables[0].Rows[i]["Instamount3"].ToString();
            TaxAgency.InstallmentAmount4 = ds.Tables[0].Rows[i]["Instamount4"].ToString();
            TaxAgency.InstallmentAmountPaid1 = ds.Tables[0].Rows[i]["Instamountpaid1"].ToString();
            TaxAgency.InstallmentAmountPaid2 = ds.Tables[0].Rows[i]["Instamountpaid2"].ToString();
            TaxAgency.InstallmentAmountPaid3 = ds.Tables[0].Rows[i]["Instamountpaid3"].ToString();
            TaxAgency.InstallmentAmountPaid4 = ds.Tables[0].Rows[i]["Instamountpaid4"].ToString();
            TaxAgency.Installment1PaidOrDue = ds.Tables[0].Rows[i]["InstPaidDue1"].ToString();
            TaxAgency.Installment2PaidOrDue = ds.Tables[0].Rows[i]["InstPaidDue2"].ToString();
            TaxAgency.Installment3PaidOrDue = ds.Tables[0].Rows[i]["InstPaidDue3"].ToString();
            TaxAgency.Installment4PaidOrDue = ds.Tables[0].Rows[i]["InstPaidDue4"].ToString();
            TaxAgency.RemainingBalance1 = ds.Tables[0].Rows[i]["Remainingbalance1"].ToString();
            TaxAgency.RemainingBalance2 = ds.Tables[0].Rows[i]["Remainingbalance2"].ToString();
            TaxAgency.RemainingBalance3 = ds.Tables[0].Rows[i]["Remainingbalance3"].ToString();
            TaxAgency.RemainingBalance4 = ds.Tables[0].Rows[i]["Remainingbalance4"].ToString();
            TaxAgency.TaxBill = ds.Tables[0].Rows[i]["taxbill"].ToString();
            TaxAgency.TaxRate = "";
            TaxAgency.TaxableValue = "";
            TaxAgency.Comments = ds.Tables[0].Rows[i]["installmentcomments"].ToString();
            TaxAgency.IsInstallmentExempt1 = ds.Tables[0].Rows[i]["ExemptRelevy1"].ToString();
            TaxAgency.IsInstallmentExempt2 = ds.Tables[0].Rows[i]["ExemptRelevy2"].ToString();
            TaxAgency.IsInstallmentExempt3 = ds.Tables[0].Rows[i]["ExemptRelevy3"].ToString();
            TaxAgency.IsInstallmentExempt4 = ds.Tables[0].Rows[i]["ExemptRelevy4"].ToString();
            TaxAgency.IsHomesteadExempt = "";
            TaxAgency.BillingPeriodStartDate = ds.Tables[0].Rows[i]["BillingStartDate"].ToString();
            TaxAgency.BillingPeriodEndDate = ds.Tables[0].Rows[i]["BillingEndDate"].ToString();
            TaxAgency.FutureTaxOption = ds.Tables[0].Rows[i]["FutureTaxOption"].ToString();
            TaxAgency.WasFullyAssessedLastYear = "";
            TaxAgency.AssessedValue = "";
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
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            TaxExemptions.ExemptionTypeCode = ds.Tables[0].Rows[i]["exemptiontype"].ToString();
            TaxExemptions.ExemptionAmount = ds.Tables[0].Rows[i]["exemptionamount"].ToString();
            TaxExemptionList.Add(TaxExemptions);
        }
        return TaxExemptionList;
    }

    public List<DelinquentTaxesTag> GetDelinquentTaxesList(string orderno)
    {
        DelinquentTaxesTag DelinquentTaxes = new DelinquentTaxesTag();
        List<DelinquentTaxesTag> DelinquentTaxesList = new List<DelinquentTaxesTag> { };
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_deliquent where orderno='" + orderno + "'");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            DelinquentTaxes.IsCurrentDelinquency = ds.Tables[0].Rows[i]["IsCurrentDelinquency"].ToString();
            DelinquentTaxes.IsFullyPaidOff = ds.Tables[0].Rows[i]["IsFullyPaidOff"].ToString();
            DelinquentTaxes.AdditionalPenaltyAmount = ds.Tables[0].Rows[i]["AdditionalPenaltyAmount"].ToString();
            DelinquentTaxes.AmountGoodThruDate = ds.Tables[0].Rows[i]["goodthuruDate"].ToString();
            DelinquentTaxes.InitialInstallmentDueDate = ds.Tables[0].Rows[i]["InstallmentDueDate"].ToString();
            DelinquentTaxes.BaseAmountDue = ds.Tables[0].Rows[i]["BaseAmountDue"].ToString();
            DelinquentTaxes.PayOffAmount = ds.Tables[0].Rows[i]["PayOffAmount"].ToString();
            DelinquentTaxes.PenaltyAmount = ds.Tables[0].Rows[i]["PenaltyAmount"].ToString();
            DelinquentTaxes.PenaltyAmountFrequency = ds.Tables[0].Rows[i]["PenaltyAmountFrequency"].ToString();
            DelinquentTaxes.PenaltyDueDate = ds.Tables[0].Rows[i]["PenaltyDueDate"].ToString();
            DelinquentTaxes.PercentOfPenaltyAmount = ds.Tables[0].Rows[i]["PercentofPenaltyAmount"].ToString();
            DelinquentTaxes.RollOverDate = ds.Tables[0].Rows[i]["RollOverDate"].ToString();
            DelinquentTaxes.PerDiem = ds.Tables[0].Rows[i]["PerDiem"].ToString();
            DelinquentTaxes.TaxYear = ds.Tables[0].Rows[i]["deliquenttaxyear"].ToString();
            DelinquentTaxes.Comments = ds.Tables[0].Rows[i]["Comments"].ToString();
            DelinquentTaxes.PayeeName = ds.Tables[0].Rows[i]["Payee"].ToString();
            DelinquentTaxes.AmountPaid = ds.Tables[0].Rows[i]["AmountPaid"].ToString();
            DelinquentTaxes.LatestPaymentDateTime = ds.Tables[0].Rows[i]["LatestPaymentDateTime"].ToString();
            DelinquentTaxes.TaxSaleDate = ds.Tables[0].Rows[i]["DateofTaxSale"].ToString();
            DelinquentTaxes.LastDayToRedeemDate = ds.Tables[0].Rows[i]["LastdaytoRedeem"].ToString();
            DelinquentTaxes.TaxSalePending = ds.Tables[0].Rows[i]["TaxSaleNotApplicable"].ToString();
            DelinquentTaxesList.Add(DelinquentTaxes);
        }
        return DelinquentTaxesList;
    }

    public List<SpecialAssessmentsTag> GetSpecialAssessmentsList(string orderno)
    {
        List<SpecialAssessmentsTag> SpecialAssessmentsList = new List<SpecialAssessmentsTag>() { };
        SpecialAssessmentsTag SpecialAssessments = new SpecialAssessmentsTag();
        DataSet ds = dbconn.ExecuteQuery("select * from tbl_specialassessment_authority where orderno='" + orderno + "'");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            SpecialAssessments.Number = ds.Tables[0].Rows[i]["Number"].ToString();
            SpecialAssessments.DueDate = ds.Tables[0].Rows[i]["DueDate"].ToString();
            SpecialAssessments.Description = ds.Tables[0].Rows[i]["description"].ToString();
            SpecialAssessments.Amount = ds.Tables[0].Rows[i]["amount"].ToString();
            SpecialAssessments.NumberOfInstallments = ds.Tables[0].Rows[i]["noofinstallment"].ToString();
            SpecialAssessments.InstallmentsPaid = ds.Tables[0].Rows[i]["installmentpaid"].ToString();
            SpecialAssessments.InstallmentsRemaining = ds.Tables[0].Rows[i]["InstallmentsRemaining"].ToString();
            SpecialAssessments.RemainingBalance = ds.Tables[0].Rows[i]["RemainingBalance"].ToString();
            SpecialAssessments.PerDiem = ds.Tables[0].Rows[i]["PerDiem"].ToString();
            SpecialAssessments.Payee = ds.Tables[0].Rows[i]["Payee"].ToString();
            SpecialAssessments.GoodThroughDate = ds.Tables[0].Rows[i]["GoodThroughDate"].ToString();
            SpecialAssessments.Comments = ds.Tables[0].Rows[i]["Comments"].ToString();
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
            Taxvendor.VendorResponse = ds.Tables[0].Rows[i]["vendor_response"].ToString();

            Taxvendor.TaxVendorService.TaxVendorServiceId = ds.Tables[0].Rows[i]["service_id"].ToString();
            Taxvendor.TaxVendorService.ServiceCode = ds.Tables[0].Rows[i]["service_code"].ToString();
            Taxvendor.TaxVendorService.ActualCost = ds.Tables[0].Rows[i]["projected_cost"].ToString();
            Taxvendor.TaxVendorService.StatusCode = ""; //Completed
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
    public string OrderDetailId;
    public string NoteText;
    public TaxDetailTag TaxDetail;
    public TaxVendorTag TaxVendor;
}

public class TaxDetailTag
{
    public string ExpectedDate;
    public string FollowUpDate;
    public string StatusCode;
    public string WasAssessedAsLand;
    public string WasAssessedAsHomestead;
    public List<TaxParcelTag> TaxParcel;
}

public class TaxParcelTag
{
    public string TaxYear;
    public string EndYear;
    public string TaxId;
    public string IsToBeDetermined;
    public string IsEstimate;

    public List<TaxAgencyDetailsTag> TaxAgencyDetails;
    public List<SpecialAssessmentsTag> SpecialAssessments;
}

public class TaxAgencyDetailsTag
{
    public string AgencyId;
    public string TaxAgencyType;
    public string IsDelinquent;
    public string DelinquentAgencyId;
    public string BillingFrequency;
    public string NextBillDate1;
    public string NextBillDate2;
    public string DelinquentDate1;
    public string DelinquentDate2;
    public string DelinquentDate3;
    public string DelinquentDate4;
    public string DiscountAmount1;
    public string DiscountAmount2;
    public string DiscountAmount3;
    public string DiscountAmount4;
    public string DiscountDate1;
    public string DiscountDate2;
    public string DiscountDate3;
    public string DiscountDate4;
    public string InstallmentDate1;
    public string InstallmentDate2;
    public string InstallmentDate3;
    public string InstallmentDate4;
    public string InstallmentAmount1;
    public string InstallmentAmount2;
    public string InstallmentAmount3;
    public string InstallmentAmount4;
    public string InstallmentAmountPaid1;
    public string InstallmentAmountPaid2;
    public string InstallmentAmountPaid3;
    public string InstallmentAmountPaid4;
    public string Installment1PaidOrDue;
    public string Installment2PaidOrDue;
    public string Installment3PaidOrDue;
    public string Installment4PaidOrDue;
    public string RemainingBalance1;
    public string RemainingBalance2;
    public string RemainingBalance3;
    public string RemainingBalance4;
    public string TaxBill;
    public string TaxRate;
    public string TaxableValue;
    public string Comments;
    public string IsInstallmentExempt1;
    public string IsInstallmentExempt2;
    public string IsInstallmentExempt3;
    public string IsInstallmentExempt4;
    public string IsHomesteadExempt;
    public string BillingPeriodStartDate;
    public string BillingPeriodEndDate;
    public string FutureTaxOption;
    public string WasFullyAssessedLastYear;
    public string AssessedValue;

    public List<TaxExemptionsTag> TaxExemptions;
    public List<DelinquentTaxesTag> DelinquentTaxes;


}

public class TaxExemptionsTag
{
    public string ExemptionTypeCode;
    public string ExemptionAmount;
}

public class DelinquentTaxesTag
{
    public string IsCurrentDelinquency;
    public string IsFullyPaidOff;
    public string AdditionalPenaltyAmount;
    public string AmountGoodThruDate;
    public string InitialInstallmentDueDate;
    public string BaseAmountDue;
    public string PayOffAmount;
    public string PenaltyAmount;
    public string PenaltyAmountFrequency;
    public string PenaltyDueDate;
    public string PercentOfPenaltyAmount;
    public string RollOverDate;
    public string PerDiem;
    public string TaxYear;
    public string Comments;
    public string PayeeName;
    public string AmountPaid;
    public string LatestPaymentDateTime;
    public string TaxSaleDate;
    public string LastDayToRedeemDate;
    public string TaxSalePending;
}

public class SpecialAssessmentsTag
{
    public string Number;
    public string DueDate;
    public string Description;
    public string Amount;
    public string NumberOfInstallments;
    public string InstallmentsPaid;
    public string InstallmentsRemaining;
    public string RemainingBalance;
    public string PerDiem;
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
    public string TaxVendorServiceId;
    public string ServiceCode;
    public string ActualCost;
    public string StatusCode;

}
