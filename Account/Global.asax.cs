using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.Globalization;
using System.Security.Principal;

namespace Account
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //file under folder Account
            RouteTable.Routes.MapPageRoute("~/Account/OpeningBalance.aspx", "oBL", "~/Account/OpeningBalance.aspx");
            RouteTable.Routes.MapPageRoute("~/Account/TransactionEntry.aspx", "transEnt", "~/Account/TransactionEntry.aspx");

            RouteTable.Routes.MapPageRoute("~/Account/Transaction_Report.aspx", "transRpt", "~/Account/Transaction_Report.aspx");
            RouteTable.Routes.MapPageRoute("TransReport", "transRpt/{AccID}", "~/Account/Transaction_Report.aspx");

            //file under folder Admin  
            RouteTable.Routes.MapPageRoute("Login", "lgn", "~/Admin/Login.aspx");
            RouteTable.Routes.MapPageRoute("~/Admin/UserEntry.aspx", "usrEnt", "~/Admin/UserEntry.aspx");
            RouteTable.Routes.MapPageRoute("~/Admin/RolesEntry.aspx", "roleEnt", "~/Admin/RolesEntry.aspx");
            RouteTable.Routes.MapPageRoute("~/Admin/Menu_Entry.aspx", "mnuEnt", "~/Admin/Menu_Entry.aspx");
            RouteTable.Routes.MapPageRoute("~/Admin/AccountEntry.aspx", "accEnt", "~/Admin/AccountEntry.aspx");
            RouteTable.Routes.MapPageRoute("~/Admin/TransactionType_Entry.aspx", "transTypEnt", "~/Admin/TransactionType_Entry.aspx");
            RouteTable.Routes.MapPageRoute("~/Admin/FormEntry.aspx", "frmEnt", "~/Admin/FormEntry.aspx");
            RouteTable.Routes.MapPageRoute("~/Admin/CashUnit_Entry.aspx", "cshUnitEnt", "~/Admin/CashUnit_Entry.aspx");
            RouteTable.Routes.MapPageRoute("attPop", "attPop", "~/Admin/Attachment_PopUp.aspx");


            RouteTable.Routes.MapPageRoute("~/Account/HomePage.aspx", "home", "~/Account/HomePage.aspx");

            RouteTable.Routes.MapPageRoute("~/Admin/Error_Log.aspx", "errLog", "~/Admin/Error_Log.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
                HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null || authCookie.Value == "")
                {
                    return;
                }
                FormsAuthenticationTicket authTicket = null;
                try
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    string[] roles = authTicket.UserData.Split(new char[] { ';' });
                    if (Context.User != null)
                    {
                        Context.User = new System.Security.Principal.GenericPrincipal(Context.User.Identity, roles);
                    }
                }
                catch
                {
                    return;
                }
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}