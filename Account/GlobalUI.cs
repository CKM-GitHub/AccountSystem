using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;
using System.Text;
using System.Web.Security;
using System.Globalization;

namespace Account
{
    public class GlobalUI
    {
        //alert box
        public static void MessageBox(string message)
        {
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page
            Page page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(GlobalUI), "alert", script);
            }
        }

        //dateconverter
        public static DateTime DateConverter(string todayDate)
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.DateSeparator = "/";
            dtFormat.ShortDatePattern = "dd/MM/yyyy hh:mm tt";

            DateTime objdate = Convert.ToDateTime(todayDate, dtFormat);
            string date = objdate.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
            objdate = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

            return objdate;
        }

        //encrypt password
        public static string EncryptPassword(string txtPassword)
        {
            byte[] passBytes = System.Text.Encoding.Unicode.GetBytes(txtPassword);
            string encryptPassword = Convert.ToBase64String(passBytes);
            return encryptPassword;
        }

        //decrypt password
        public static string DecryptPassword(string encryptedPassword)
        {
            byte[] passByteData = Convert.FromBase64String(encryptedPassword);
            string originalPassword = System.Text.Encoding.Unicode.GetString(passByteData);
            return originalPassword;
        }

        //Encrypt paramater 
        public static string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Encrypt(strQueryString, "r0b1nr0y");
        }

        //Decrypt paramater 
        public static string DecryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Decrypt(strQueryString, "r0b1nr0y");
        }

        public static string[] GetRoles()
        {
            string[] roles=null;
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id =
                            (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData;
                        roles = userData.Split(',');
                    }
                }
            }
            return roles;
        }
    }
}