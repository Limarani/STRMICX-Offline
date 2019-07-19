using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for SessionHandler
/// </summary>
public static class SessionHandler
{
    #region Kanagarajan 

    private static string _userName = "UserName";
    public static string UserName
    {
        get
        {
            if (HttpContext.Current.Session[SessionHandler._userName] == null)
            {
                return "";
            }
            else
            {
                return (string)HttpContext.Current.Session[SessionHandler._userName];
            }
        }
        set
        {
            HttpContext.Current.Session[SessionHandler._userName] = value;
        }
    }

    private static string _theme = "Theme";
    public static string Theme
    {
        get
        {
            if (HttpContext.Current.Session[SessionHandler._theme] == null)
            {

                return "Black"; //Default
            }
            else
            {
                return (string)HttpContext.Current.Session[SessionHandler._theme];
            }
        }
        set
        {
            HttpContext.Current.Session[SessionHandler._theme] = value;
        }
    }

    private static string _IsAdmin = "0";
    public static bool IsAdmin
    {
        get
        {
                return Convert.ToBoolean(HttpContext.Current.Session[SessionHandler._IsAdmin]);
        }
        set
        {
            HttpContext.Current.Session[SessionHandler._IsAdmin] = value.ToString();
        }
    }
    private static string _IsprocessMenu = "Process";
    public static string IsprocessMenu
    {
        get
        {
            if (HttpContext.Current.Session[SessionHandler._IsprocessMenu] == null)
            {
                return "";
            }
            else
            {
                return (string)HttpContext.Current.Session[SessionHandler._IsprocessMenu];
            }
        }
        set
        {
            HttpContext.Current.Session[SessionHandler._IsprocessMenu] = value;
        }
    }
    private static string _IspendingMenu = "Pending";
    public static string IspendingMenu
    {
        get
        {
            if (HttpContext.Current.Session[SessionHandler._IspendingMenu] == null)
            {
                return "";
            }
            else
            {
                return (string)HttpContext.Current.Session[SessionHandler._IspendingMenu];
            }
        }
        set
        {
            HttpContext.Current.Session[SessionHandler._IspendingMenu] = value;
        }
    }
    private static string _AuditQA = "AuditQA";
    public static string AuditQA
    {
        get
        {
            if (HttpContext.Current.Session[SessionHandler._AuditQA] == null)
            {
                return "";
            }
            else
            {
                return (string)HttpContext.Current.Session[SessionHandler._AuditQA];
            }
        }
        set
        {
            HttpContext.Current.Session[SessionHandler._AuditQA] = value;
        }
    }


    private static string _OrderNo = "Order_No";
    public static string OrderNo
    {
        get
        {
            if (HttpContext.Current.Session[SessionHandler._OrderNo] == null)
            {
                return "";
            }
            else
            {
                return (string)HttpContext.Current.Session[SessionHandler._OrderNo];
            }
        }
        set
        {
            HttpContext.Current.Session[SessionHandler._OrderNo] = value;
        }
    }

    private static string _UserState = "User_State";
    public static string UserState
    {
        get
        {
            if (HttpContext.Current.Session[SessionHandler._UserState] == null)
            {
                return "";
            }
            else
            {
                return (string)HttpContext.Current.Session[SessionHandler._UserState];
            }
        }
        set
        {
            HttpContext.Current.Session[SessionHandler._UserState] = value;
        }
    }

    private static string _CheckPwd = "CheckPwd";
    public static bool CheckPwd
    {
        get
        {
            return Convert.ToBoolean(HttpContext.Current.Session[SessionHandler._CheckPwd]);
        }
        set
        {
            HttpContext.Current.Session[SessionHandler._CheckPwd] = value.ToString();
        }
    }
    #endregion
    
}
