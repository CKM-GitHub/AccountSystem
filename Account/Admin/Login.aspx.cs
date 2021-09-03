using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using Account_BL;
using System.Configuration;

namespace Account
{
    public partial class LoginIn : System.Web.UI.Page
    {
        #region declare
        LoginIn_BL lgnBL = new LoginIn_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();
        string AccountAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Page.User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/Admin/Login.aspx");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = 0;
                string roles = string.Empty;

                DataTable dt = new DataTable();
                dt = lgnBL.IsValidUser(txtName.Text, GlobalUI.EncryptPassword(txtPass.Text));

                userId = int.Parse(dt.Rows[0]["UserId"].ToString());
                roles = dt.Rows[0]["Roles"].ToString();

                switch (userId)
                {
                    case -1:
                        GlobalUI.MessageBox("Name and/or password is incorrect");
                        break;
                    default:
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, txtName.Text, DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), false, roles, FormsAuthentication.FormsCookiePath);
                        string hash = FormsAuthentication.Encrypt(ticket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                        Session["UserName"] = txtName.Text;
                        if (!String.IsNullOrWhiteSpace(roles)) { Session["Roles"] = roles; }
                        else { Response.Redirect("~/Account/Transaction_Report.aspx", false); }

                        if (ticket.IsPersistent)
                        {
                            cookie.Expires = ticket.Expiration;
                        }
                        Response.Cookies.Add(cookie);

                        DataTable dtAllMnus = lgnBL.GetAllowedUrl(roles); // get allowed urls to specific roles
                        bool home=false;

                        if (dtAllMnus.Rows.Count>0)
                        {
                            for (int i = 0; i < dtAllMnus.Rows.Count; i++)
                            {
                                if (dtAllMnus.Rows[i]["DirectoryPath"].ToString().Contains("Home"))
                                {
                                    home = true;
                                    Response.RedirectToRoute(dtAllMnus.Rows[i]["DirectoryPath"].ToString(), false);
                                    break;
                                }
                                else
                                {
                                    home = false;
                                }
                            }
                            if (!home)
                            {
                                Response.RedirectToRoute(dtAllMnus.Rows[0]["DirectoryPath"].ToString(), false);
                            }
                        }
                        else if (dtAllMnus.Rows.Count == 0 && txtName.Text == AccountAdmin)
                        {
                            DataTable dtRoles=lgnBL.GetRoles();
                            if (dtRoles.Rows.Count > 0)
                            {
                                Response.RedirectToRoute("~/Admin/Menu_Entry.aspx", false);
                            }
                            else
                            {
                                Response.RedirectToRoute("~/Admin/RolesEntry.aspx", false);
                            }
                        }
                        else
                        {
                            GlobalUI.MessageBox("Something wrong!");  //any form is not allowed to this roles. assign form(menu) to roles first.
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }
    }
}