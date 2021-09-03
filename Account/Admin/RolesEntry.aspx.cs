using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Account_BL;
using System.Configuration;

namespace Account.Admin
{
    public partial class RolesEntry : System.Web.UI.Page
    {
        RolesEntry_BL roleBL = new RolesEntry_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();

        string canEdit = "0", canDel = "0";

        string AccountAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region getEditDelete Role

            string filePath = Page.AppRelativeVirtualPath;

            String[] roles = GlobalUI.GetRoles();

            string strr = Common_Fun.IsRoleCanEditDel(filePath, roles[0]);

            string[] ctrl = strr.Split(',');

            canEdit = ctrl[0];
            canDel = ctrl[1];

            #endregion

            if (!IsPostBack)
            {
                RolesGridBind();
            }
        }

        private void RolesGridBind()
        {
            try
            {
                gdvRoles.DataSource = roleBL.RolesGridBind();
                gdvRoles.DataBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSaveOk;
                int roleID = 0;
                if (!String.IsNullOrEmpty(hdfID.Value))
                    roleID = int.Parse(hdfID.Value.ToString());
                else roleID = 0;

                string userName = this.Page.User.Identity.Name;

                if (IsRolesExist(roleID, txtRoleName.Text))    //check entered role name is already inserted
                {
                    GlobalUI.MessageBox("This account is already stored!");
                }
                else
                {
                    switch (btnSave.Text)
                    {
                        case "Save":
                            isSaveOk = SaveNewRole(txtRoleName.Text, userName);   //save new roles
                            if (isSaveOk)
                            {
                                GlobalUI.MessageBox("Save Successful!");
                            }
                            break;
                        case "Update":

                            if (hdfRoleName.Value.ToString() != txtRoleName.Text)
                            {
                                isSaveOk = UpdateNewRole(roleID, txtRoleName.Text, userName);   //update roles
                                if (isSaveOk)
                                {
                                    DataTable dt = roleBL.GetForms();

                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        roleBL.UpdateRole(dt.Rows[i]["ID"].ToString(), hdfID.Value.ToString(),hdfRoleName.Value.ToString(), txtRoleName.Text, this.Page.User.Identity.Name);

                                        String frmDir = roleBL.GetFormDir(int.Parse(dt.Rows[i]["ID"].ToString()));
                                        string[] arrConfigurl = frmDir.Split('/');

                                        String[] arrRoles = roleBL.GetAllowRoles(int.Parse(dt.Rows[i]["ID"].ToString())).Split(',');

                                        //**dynamically change authentication in web.config under specific folder**
                                        //add allow roles and deny users *
                                        Common_Fun.AddRolesToConfig(arrConfigurl, arrRoles);
                                    }

                                    GlobalUI.MessageBox("Update Successful!");
                                }
                            }
                            break;
                    }
                    Clear();
                }
                RolesGridBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private bool UpdateNewRole(int roleID, string roleName, string userName)
        {
            return roleBL.UpdateNewRole(roleID, roleName,  userName);
        }

        private bool SaveNewRole(string roleName, string userName)
        {
            return roleBL.SaveNewRole(roleName,userName);
        }

        private bool IsRolesExist(int roleID, string roleName)
        {
            return roleBL.IsRolesExist(roleID, roleName);
        }

        private void Clear()
        {
            txtRoleName.Text = "";
            btnSave.Text = "Save";
        }

        protected void gdvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Control ctrl = e.CommandSource as Control;
                GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;

                if (e.CommandName == "Cmd_Edit")
                {
                    Label lblid = row.FindControl("lblID") as Label;
                    Label lblRoleName = row.FindControl("lblRoleName") as Label;
                    Label lblCanEdit = row.FindControl("lblCanEdit") as Label;
                    Label lblCanDelete = row.FindControl("lblCanDelete") as Label;

                    hdfID.Value = lblid.Text;
                    txtRoleName.Text = lblRoleName.Text;
                    hdfRoleName.Value = lblRoleName.Text;        
                    btnSave.Text = "Update";

                }
                if (e.CommandName == "Cmd_Delete")
                {
                    Label lblid = row.FindControl("lblID") as Label;
                    Label lblRoleName = row.FindControl("lblRoleName") as Label;

                    bool IsDelOk = DeleteRole(int.Parse(lblid.Text));
                    if (IsDelOk)
                    {
                        DataTable dt = roleBL.GetForms();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            roleBL.RemoveRole(dt.Rows[i]["ID"].ToString(), lblid.Text, lblRoleName.Text, this.Page.User.Identity.Name);

                            String frmDir = roleBL.GetFormDir(int.Parse(dt.Rows[i]["ID"].ToString()));
                            string[] arrConfigurl = frmDir.Split('/');

                            String[] arrRoles = roleBL.GetAllowRoles(int.Parse(dt.Rows[i]["ID"].ToString())).Split(',');

                            //**dynamically change authentication in web.config under specific folder**
                            //add allow roles and deny users *
                            Common_Fun.AddRolesToConfig(arrConfigurl, arrRoles);
                        }

                        GlobalUI.MessageBox("Delete Successful!");
                    }
                    else
                    {
                        GlobalUI.MessageBox("Delete was unsuccessful!");
                    }
                    Clear();
                    btnSave.Text = "Save";
                }
                RolesGridBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private bool DeleteRole(int roleID)
        {
            return roleBL.DeleteRole(roleID);
        }

        protected void gdvRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvRoles.PageIndex = e.NewPageIndex;
            RolesGridBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void gdvRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    foreach (LinkButton button in e.Row.Cells[7].Controls.OfType<LinkButton>())
                    {
                        if (canEdit == "0" && this.Page.User.Identity.Name!=AccountAdmin)
                        {
                            if (button.CommandName == "Cmd_Edit")
                            {
                                button.Visible = false;
                            }

                        }
                        if (canDel == "0" && this.Page.User.Identity.Name != AccountAdmin)
                        {
                            if (button.CommandName == "Cmd_Delete")
                            {
                                button.Visible = false;
                            }
                        }
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