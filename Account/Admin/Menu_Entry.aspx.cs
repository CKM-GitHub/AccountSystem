using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Account_BL;
using System.Web.Configuration;
using System.IO;
using System.Data;

namespace Account.Admin
{
    public partial class Menu_Entry : System.Web.UI.Page
    {
        #region  declare
        string AccountAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();

        Menu_Entry_BL frmEtryBL = new Menu_Entry_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();
        Collection<int> fromIDItems = new Collection<int>();

        Collection<string> roleIDItems = new Collection<string>();
        Collection<string> roleNameItems = new Collection<string>();

        string canEditBtn = "0", canDelBtn = "0";

        public bool canRead
        {
            set
            {
                ViewState["canRead"] = value;
            }
            get
            {
                if (ViewState["canRead"] != null)
                {
                    return Convert.ToBoolean(ViewState["canRead"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }

        public bool canEdit
        {
            set
            {
                ViewState["canEdit"] = value;
            }
            get
            {
                if (ViewState["canEdit"] != null)
                {
                    return Convert.ToBoolean(ViewState["canEdit"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }

        public bool canDel
        {
            set
            {
                ViewState["canDel"] = value;
            }
            get
            {
                if (ViewState["canDel"] != null)
                {
                    return Convert.ToBoolean(ViewState["canDel"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region getEditDelete Role

            string filePath = Page.AppRelativeVirtualPath;

            String[] roles = GlobalUI.GetRoles();

            string strr = Common_Fun.IsRoleCanEditDel(filePath, roles[0]);

            string[] ctrl = strr.Split(',');

            canEditBtn = ctrl[0];
            canDelBtn = ctrl[1];

            #endregion

            if (!IsPostBack)
            {
                ddlRolesBind();
                MenusGridBind();
            }
        }

        private void ddlRolesBind()
        {
            ddlRoles.DataSource = frmEtryBL.GetRoles();
            ddlRoles.DataTextField = "RoleName";
            ddlRoles.DataValueField = "RoleID";
            ddlRoles.DataBind();
        }      
           
        //private void AddRolesToConfig(string[] arrConfigurl, string[] arrRoleName)
        //{
        //    Configuration config = (Configuration)WebConfigurationManager.OpenWebConfiguration("~/" + arrConfigurl[1]+ "/");
        //    AuthorizationSection section = (AuthorizationSection)config.GetSection("system.web/authorization");

        //    ConfigurationLocationCollection locations = config.Locations;

        //    foreach (ConfigurationLocation l in locations)
        //    {
        //        if (l.Path == arrConfigurl[2]) //This is case Sensitive
        //        {
        //            Configuration adminConfig = (Configuration)l.OpenConfiguration();
        //            AuthorizationSection admin_section = (AuthorizationSection)adminConfig.GetSection("system.web/authorization");

        //            string webConfigUrl = Server.MapPath("~/" + arrConfigurl[1] + "/Web.config"); 

        //            //Remove all Current roles to folder name contained in arrConfigurl[1] location.
        //            if (arrRoleName[0] == "delete")
        //            {
        //                admin_section.Rules.Clear();
        //                SetDefaultConfig(admin_section, webConfigUrl);
        //                break;
        //            }

        //            admin_section.Rules.Clear();
        //            ////Add New Roles to filename contained in arrConfigurl[2] location.
        //            AuthorizationRule adminAuth = new AuthorizationRule(AuthorizationRuleAction.Allow);

        //            //for assigned roles count more than 1
        //            for (int i = 0; i < arrRoleName.Length; i++)
        //            {
        //                adminAuth.Roles.Add(arrRoleName[i]);
        //            }
        //            admin_section.Rules.Add(adminAuth);
        //            adminAuth = null;

        //            SetDefaultConfig(admin_section, webConfigUrl);
        //        }
        //    }
        //}

        //private void SetDefaultConfig(AuthorizationSection admin_section, string webConfigUrl)
        //{

        //    //add default allowed user AccountAdmin
        //    AuthorizationRule adminAllow = new AuthorizationRule(AuthorizationRuleAction.Allow);
        //    adminAllow.Users.Add(AccountAdmin);
        //    admin_section.Rules.Add(adminAllow);

        //    // add deny users
        //    AuthorizationRule adminDeny = new AuthorizationRule(AuthorizationRuleAction.Deny);
        //    adminDeny.Users.Add("*");
        //    admin_section.Rules.Add(adminDeny);

        //    // Remove readonly attribute off of the file before execution
        //    if (File.Exists(webConfigUrl))
        //    {
        //        FileAttributes attributes = File.GetAttributes(webConfigUrl);

        //        if (attributes.HasFlag(FileAttributes.ReadOnly))
        //            File.SetAttributes(webConfigUrl, FileAttributes.Normal);
        //    }

        //    admin_section.CurrentConfiguration.Save();
        //}
              
        private void MenusGridBind()
        {
            DataTable dt = frmEtryBL.MenusGridBind(ddlRoles.SelectedItem.Text);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!dt.Rows[i]["canEdit"].ToString().Contains(ddlRoles.SelectedItem.Text))
                {
                    dt.Rows[i]["canEdit"] = "";
                }
                if (!dt.Rows[i]["canDel"].ToString().Contains(ddlRoles.SelectedItem.Text))
                {
                    dt.Rows[i]["canDel"] = "";
                }
                if (!dt.Rows[i]["RoleName"].ToString().Contains(ddlRoles.SelectedItem.Text))
                {
                    dt.Rows[i]["RoleName"] = "";
                }
            }

            gdvMenus.DataSource = dt;
            gdvMenus.DataBind();
        }   

        protected void gdvMenus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvMenus.PageIndex = e.NewPageIndex;
            gdvMenus.EditIndex = -1;
            MenusGridBind();
        }      

        protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            gdvMenus.EditIndex = -1;
            MenusGridBind();
        }

        protected void gdvMenus_RowEditing(object sender, GridViewEditEventArgs e)
        {
            canRead = false; canEdit = false; canDel = false;

            GridViewRow row = gdvMenus.Rows[e.NewEditIndex];

            Label lblCanRead = (Label)row.FindControl("lblRoles");
            Label lblCanEdt = (Label)row.FindControl("lblCanEdt");
            Label lblCanDel = (Label)row.FindControl("lblCanDel");

            if (lblCanRead.Text == "√")
            {
                canRead = true;
            }

            if (lblCanEdt.Text == "√")
            {
                canEdit = true;
            }

            if (lblCanDel.Text == "√")
            {
                canDel = true;
            }

            gdvMenus.EditIndex = e.NewEditIndex;
            MenusGridBind();
        }

        protected void gdvMenus_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            bool isSaveOk = false;            

            GridViewRow row = gdvMenus.Rows[e.RowIndex];

            Label frmID = (Label)row.FindControl("lblFormID");
            int ID=int.Parse(frmID.Text);

            CheckBox edtChkCanRead = (CheckBox)row.FindControl("edtChkCanRead");
            CheckBox edtChkCanEdt = (CheckBox)row.FindControl("edtChkCanEdt");
            CheckBox edtChkCanDel = (CheckBox)row.FindControl("edtChkCanDel");

            string flagEdit = !edtChkCanEdt.Checked ? "" : ddlRoles.SelectedItem.Text;
            string flagDel = !edtChkCanDel.Checked ? "" : ddlRoles.SelectedItem.Text; 

            string userName = this.Page.User.Identity.Name;

                bool isSaveSuccess;
                if (canRead != edtChkCanRead.Checked)
                {
                    if (edtChkCanRead.Checked == true)
                    {
                        isSaveOk = frmEtryBL.SaveNewMenu(ID, ddlRoles.SelectedValue, ddlRoles.SelectedItem.Text, userName);   //save new form name/dir
                    }
                    else
                    {
                        isSaveOk = frmEtryBL.RemoveRole(ID, ddlRoles.SelectedValue, ddlRoles.SelectedItem.Text, userName);
                    }
                }
                if (canEdit != edtChkCanEdt.Checked)
                {
                    if (!String.IsNullOrWhiteSpace(flagEdit))
                    {
                        frmEtryBL.SaveEdit(ID, ddlRoles.SelectedItem.Text, userName);
                    }
                    else
                    {
                        frmEtryBL.RemoveEdit(ID, ddlRoles.SelectedItem.Text, userName);
                    }
                }
                if (canDel != edtChkCanDel.Checked)
                {
                    if (!String.IsNullOrWhiteSpace(flagDel))
                    {
                        frmEtryBL.SaveDel(ID, ddlRoles.SelectedItem.Text, userName);
                    }
                    else
                    {
                        frmEtryBL.RemoveDel(ID, ddlRoles.SelectedItem.Text, userName);
                    }
                }
                    if (isSaveOk)
                    {
                        String frmDir = frmEtryBL.GetFormDir(ID);
                        string[] arrConfigurl = frmDir.Split('/');

                        String[] arrRoles = frmEtryBL.GetAllowRoles(ID).Split(',');

                        //**dynamically change authentication in web.config under specific folder**
                        //add allow roles and deny users *
                        Common_Fun.AddRolesToConfig(arrConfigurl, arrRoles);

                        isSaveSuccess = true;
                    }
                    else
                    {
                        isSaveSuccess = false;
                    }
                    if (isSaveSuccess)
                    {
                        GlobalUI.MessageBox("Successful!");
                    }
                   
            gdvMenus.EditIndex = -1;
            MenusGridBind();

            canRead = false; canEdit = false; canDel = false;
        }

        protected void gdvMenus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gdvMenus.EditIndex = -1;
            MenusGridBind();
        }

        protected void edtChkCanRead_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            CheckBox chkRead = (CheckBox)chk.Parent.FindControl("edtChkCanRead");
            CheckBox chkEdt = (CheckBox)chk.Parent.FindControl("edtChkCanEdt");
            CheckBox chkDel = (CheckBox)chk.Parent.FindControl("edtChkCanDel");           

            if (chk.Checked == true)
            {
                chkRead.Checked = true;               
            }
            else
            {
                chkEdt.Checked = false;
                chkDel.Checked = false;
            }
        }

        protected void edtChkCanEdt_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            if (chk.Checked == true)
            {
                CheckBox chk2 = (CheckBox)chk.Parent.FindControl("edtChkCanRead");
                chk2.Checked = true;
            }            
        }

        protected void edtChkCanDel_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            if (chk.Checked == true)
            {
                CheckBox chk2 = (CheckBox)chk.Parent.FindControl("edtChkCanRead");
                chk2.Checked = true;
            }
        }

        protected void gdvMenus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (canEditBtn == "0" && this.Page.User.Identity.Name!=AccountAdmin)
                    {
                        e.Row.Cells[13].Visible = false;

                    }                    
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }
    }
}