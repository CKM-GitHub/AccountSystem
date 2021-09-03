using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using Account_BL;
using System.Web.Security;
using System.Configuration;

namespace Account
{
    public partial class Account : System.Web.UI.MasterPage
    {
        #region declare
        ErrorLog_BL errBL = new ErrorLog_BL();
        AccountMaster_BL accMasBL = new AccountMaster_BL();

        string accAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                Response.RedirectToRoute("Login", false);
            }
            else
            {
                lblUser.Text = this.Page.User.Identity.Name;

                DataTable dtParentMnu = new DataTable();

                String[] roles = GlobalUI.GetRoles();

                if (roles != null || roles.Length>0) 
                {
                    if ( accAdmin == this.Page.User.Identity.Name)
                    {
                        dtParentMnu = accMasBL.GetParentMnuAccAdmin();
                    }
                    else
                    {
                         dtParentMnu = accMasBL.GetMenu(roles[0]); //get parentmenu of specific roles including null (means file without parentmenu)
                    }

                    if (dtParentMnu.Rows.Count > 0) //may be parentmenu not null only or parentmenu not null and parentmenu null
                    {
                        for (int i = 0; i < dtParentMnu.Rows.Count; i++)
                        {
                            if (dtParentMnu.Rows[i]["ParentMenu"] != null && !String.IsNullOrWhiteSpace(dtParentMnu.Rows[i]["ParentMenu"].ToString()))
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");  //create parent menu
                                li.Attributes.Add("class", "dropdown");
                                navbar.Controls.Add(li);  //add parent menu to nav bar

                                HtmlGenericControl anchor = new HtmlGenericControl("a");
                                anchor.Attributes.Add("href", "#");
                                anchor.Attributes.Add("class", "dropdown-toggle");
                                anchor.Attributes.Add("data-toggle", "dropdown");
                                anchor.InnerText = Convert.ToString(dtParentMnu.Rows[i]["ParentMenu"] + "▼");
                                li.Controls.Add(anchor);

                                DataTable dtSubMenu = new DataTable();

                                if ( accAdmin == this.Page.User.Identity.Name)
                                {
                                    dtSubMenu = accMasBL.GetSubMenuAccAdmin(dtParentMnu.Rows[i]["ParentMenu"].ToString()); //get submenus of parent menu for default acc admin
                                }
                                else
                                {
                                    dtSubMenu = accMasBL.GetSubMenu(dtParentMnu.Rows[i]["ParentMenu"].ToString(), roles[0]); //get submenus of parent menu for specific roles
                                }
                                HtmlGenericControl ul = new HtmlGenericControl("ul");
                                ul.Attributes.Add("class", "dropdown-menu");

                                AddMenu(ul, dtSubMenu); //add child menus to parent menu

                                li.Controls.Add(ul);
                               
                            }
                            else // parentmenu null case
                            {
                                DataTable dt = new DataTable();
                                if (accAdmin == this.Page.User.Identity.Name)
                                {
                                    dt = accMasBL.GetMenuNullParentAccAdmin();
                                }
                                else
                                {
                                     dt = accMasBL.GetMenuNullParent(roles[0]);
                                }
                                AddMenu(navbar, dt); //add menu to navbar
                            }
                        }
                    }
                    else // roles is only assigned to parentmenu null case
                    {
                         DataTable dt = new DataTable();

                         if (accAdmin == this.Page.User.Identity.Name)
                         {
                             dt = accMasBL.GetMenuNullParentAccAdmin();
                         }
                         else
                         {

                             dt = accMasBL.GetMenuNullParent(roles[0]);
                         }
                        AddMenu(navbar, dt); //add menu to navbar
                    }
                }
            }
        }

        private void AddMenu(HtmlGenericControl parent, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["DirectoryPath"].ToString().Contains("Home"))
                {
                    homepg.HRef = homepg.GetRouteUrl(dt.Rows[i]["DirectoryPath"].ToString(), 0);
                }
                else
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    parent.Controls.Add(li);

                    HtmlGenericControl anchor = new HtmlGenericControl("a");
                    anchor.Attributes.Add("href", anchor.GetRouteUrl(dt.Rows[i]["DirectoryPath"].ToString(), 0));
                    anchor.InnerText = Convert.ToString(dt.Rows[i]["FormName"]);

                    li.Controls.Add(anchor);
                }
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Remove("UserName");
                Session.Remove("Roles");
                Response.RedirectToRoute("Login", false);
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }
    }
}