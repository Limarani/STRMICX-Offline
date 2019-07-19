using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

/// <summary>
/// Summary description for myVariables
/// </summary>
public class myVariables
{
    private static HttpSessionState session
    {
        get
        {
            return HttpContext.Current.Session;
        }
    }

    #region Variables Declaration
    
    public static string ErrMsg
    {
        get { return session["ErrMsg"] as string; }
        set { session["ErrMsg"] = value; }
    }

    private static bool _IsErr;

    public static bool IsErr
    {
        get { return _IsErr; }
        set { _IsErr = value; }
    }



    private static SortDirection _mSortDirection;

    public static SortDirection mSortDirection
    {
        get { return _mSortDirection; }
        set { _mSortDirection = value; }
    }

    private static string _Admin;
    public static string Admin
    {
        get
        {
            if (_Admin == null) { _Admin = "0"; }
            return _Admin;
        }
        set { _Admin = value; }
    }

    private static string _fullname;
    public static string Fullname
    {
        get
        {
            if (_fullname == null) { _fullname = "0"; }
            return _fullname;
        }
        set { _fullname = value; }
    }

    public static string scrape_status
    {
        get { return session["scrape_status"] as string; }
        set { session["scrape_status"] = value; }
    }

    private static string _Username;
    public static string Username
    {
        get
        {
            if (_Username == null) { _Username = "0"; }
            return _Username;
        }
        set { _Username = value; }
    }
    private static string _Key;
    public static string Key
    {
        get
        {
            if (_Key == null) { _Key = "0"; }
            return _Key;
        }
        set { _Key = value; }
    }
    private static string _QC;
    public static string QC
    {
        get
        {
            if (_QC == null) { _QC = "0"; }
            return _QC;
        }
        set { _QC = value; }
    }
    private static string _Review;
    public static string Review
    {
        get
        {
            if (_Review == null) { _Review = "0"; }
            return _Review;
        }
        set { _Review = value; }
    }
    private static string _Pend;
    public static string Pend
    {
        get
        {
            if (_Pend == null) { _Pend = "0"; }
            return _Pend;
        }
        set { _Pend = value; }
    }
    private static string _DU;
    public static string DU
    {
        get
        {
            if (_DU == null) { _DU = "0"; }
            return _DU;
        }
        set { _DU = value; }
    }
    private static string _Ordertype;
    public static string Ordertype
    {
        get
        {
            if (_Ordertype == null) { _Ordertype = "0"; }
            return _Ordertype;
        }
        set { _Ordertype = value; }
    }
    private static string _Mailaway;
    public static string Mailaway
    {
        get
        {
            if (_Mailaway == null) { _Mailaway = "0"; }
            return _Mailaway;
        }
        set { _Mailaway = value; }
    }
    private static string _Parcelid;
    public static string Parcelid
    {
        get
        {
            if (_Parcelid == null) { _Parcelid = "0"; }
            return _Parcelid;
        }
        set { _Parcelid = value; }
    }
    private static string _Onhold;
    public static string Onhold
    {
        get
        {
            if (_Onhold == null) { _Onhold = "0"; }
            return _Onhold;
        }
        set { _Onhold = value; }
    }
    private static string _UserCount;
    public static string UserCount
    {
        get
        {
            if (_UserCount == null) { _UserCount = "1"; }
            return _UserCount;
        }
        set { _UserCount = value; }
    }
    private static string _pdu;
    public static string pdu
    {
        get
        {
            if (_pdu == null) { _pdu = "0"; }
            return _pdu;
        }
        set { _pdu = value; }
    }
    private static string _pDate = "";
    public static string pDate
    {
        get { return _pDate; }
        set { _pDate = value; }
    }
    private static string _Userright;
    public static string Userright
    {
        get { return _Userright; }
        set { _Userright = value; }
    }
    private static string _pType;
    public static string pType
    {
        get { return _pType; }
        set { _pType = value; }
    }
    private static string _pType1;
    public static string pType1
    {
        get { return _pType1; }
        set { _pType1 = value; }
    }
    private static string _States;
    public static string States
    {
        get { return _States; }
        set { _States = value; }
    }
    private static string _SST;
    public static string SST
    {
        get { return _SST; }
        set { _SST = value; }
    }
    private static string _Priority;
    public static string Priority
    {
        get { return _Priority; }
        set { _Priority = value; }
    }
    private static string _AssPriority;
    public static string AssPriority
    {
        get { return _AssPriority; }
        set { _AssPriority = value; }
    }
    private static string _QA;
    public static string QA
    {
        get { return _QA; }
        set { _QA = value; }
    }


    #region For Production
    public static string ecount
    {
        get { return session["ecount"] as string; }
        set { session["ecount"] = value; }
    }
    public static string Orderno
    {
        get { return session["Orderno"] as string; }
        set { session["Orderno"] = value; }
    }
    public static string Date
    {
        get { return session["Date"] as string; }
        set { session["Date"] = value; }
    }
    public static string State
    {
        get { return session["State"] as string; }
        set { session["State"] = value; }
    }
    public static string County
    {
        get { return session["County"] as string; }
        set { session["County"] = value; }
    }
    public static string WebPhone
    {
        get { return session["WebPhone"] as string; }
        set { session["WebPhone"] = value; }
    }
    public static string OrderTp
    {
        get { return session["OrderTp"] as string; }
        set { session["OrderTp"] = value; }
    }
    public static string TimeZone
    {
        get { return session["TimeZone"] as string; }
        set { session["TimeZone"] = value; }
    }
    public static string Lastcomment
    {
        get { return session["Lastcomment"] as string; }
        set { session["Lastcomment"] = value; }
    }
    public static string Borrower
    {
        get { return session["Borrower"] as string; }
        set { session["Borrower"] = value; }
    }
    public static string Zipcode
    {
        get { return session["Zipcode"] as string; }
        set { session["Zipcode"] = value; }
    }

    public static string serpro
    {
        get { return session["serpro"] as string; }
        set { session["serpro"] = value; }
    }

    public static string Followupdate
    {
        get { return session["Followupdate"] as string; }
        set { session["Followupdate"] = value; }
    }
    public static string Township
    {
        get { return session["Township"] as string; }
        set { session["Township"] = value; }
    }
    public static string KeyStatus
    {
        get { return session["KeyStatus"] as string; }
        set { session["KeyStatus"] = value; }
    }
    public static string QCStatus
    {
        get { return session["QCStatus"] as string; }
        set { session["QCStatus"] = value; }
    }
    public static string HP
    {
        get { return session["HP"] as string; }
        set { session["HP"] = value; }
    }
    public static string Prior
    {
        get { return session["Prior"] as string; }
        set { session["Prior"] = value; }
    }
    public static string ChecklistComments
    {
        get { return session["ChecklistComments"] as string; }
        set { session["ChecklistComments"] = value; }
    }
    
    public static string EntityComments
    {
        get { return session["EntityComments"] as string; }
        set { session["EntityComments"] = value; }
    }
    public static string Tax
    {
        get { return session["Tax"] as string; }
        set { session["Tax"] = value; }
    }
    public static string PayStatus
    {
        get { return session["PayStatus"] as string; }
        set { session["PayStatus"] = value; }
    }
    public static string PayFreq
    {
        get { return session["PayFreq"] as string; }
        set { session["PayFreq"] = value; }
    }

    public static string KeyStart
    {
        get { return session["DelayStart"] as string; }
        set { session["DelayStart"] = value; }
    }

    public static string Misc
    {
        get { return session["misc"] as string; }
        set { session["misc"] = value; }
    }

    public static string DelayStatus
    {
        get { return session["DelayStatus"] as string; }
        set { session["DelayStatus"] = value; }
    }
    public static string DelayComments
    {
        get { return session["DelayComments"] as string; }
        set { session["DelayComments"] = value; }
    }
    #endregion

    #endregion
}
