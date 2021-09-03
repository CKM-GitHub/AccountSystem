using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using Account_BL;

namespace Account
{
    public partial class UserEntry : System.Web.UI.Page
    {
        #region declare
            UserEntry_BL userBL = new UserEntry_BL();
            ErrorLog_BL errBL = new ErrorLog_BL();

            string AccountAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();

            string canEdit = "0", canDel = "0";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region getEditDelete Role

            string filePath=Page.AppRelativeVirtualPath;

            String[] roles = GlobalUI.GetRoles();

            string strr = Common_Fun.IsRoleCanEditDel(filePath, roles[0]);

            string[] ctrl = strr.Split(',');

            canEdit = ctrl[0];
            canDel = ctrl[1];

            #endregion

            if (!IsPostBack) 
            {
                BindGridView();
                ddlRolesBind();
            }
        }

        private void ddlRolesBind()
        {
            ddlRoleName.DataSource = userBL.ddlRolesBind();
            ddlRoleName.DataTextField = "RoleName";
            ddlRoleName.DataValueField = "RoleID";
            ddlRoleName.DataBind();
        }
        
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (userBL.IsUserExist(txtUserName.Text))    //check entered username is already occupied by other user
                {
                    GlobalUI.MessageBox("User Name is already inserted!");
                }
                else
                {
                    string loginname = this.Page.User.Identity.Name;
                    bool isSaveOk = userBL.SaveNewUser(txtUserName.Text, GlobalUI.EncryptPassword(txtPass.Text), int.Parse(ddlRoleName.SelectedValue), loginname);   //save new users
                    if (isSaveOk)
                    {
                        GlobalUI.MessageBox("Save Successful!");
                        BindGridView();
                        Clear();
                    }
                    else
                    {
                        GlobalUI.MessageBox("Sorry,Save is unsuccessful!");
                    }
                }
            }
            catch (Exception ex)
            {
                 errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void Clear()
        {
            txtUserName.Text = "";
            txtPass.Text = "";
            btnUpdate.Visible = false;
            btnCreate.Visible = true;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUserName.Text;
                string password = txtPass.Text;
                int userid = Convert.ToInt32(txtid.Text);
                string loginname = this.Page.User.Identity.Name; 

                bool isUpdOK = userBL.updatebyUserId(username, GlobalUI.EncryptPassword(password), int.Parse(ddlRoleName.SelectedValue), userid, loginname);

                if (isUpdOK)
                {
                    GlobalUI.MessageBox("Update is successful!");
                }
                else
                {
                    GlobalUI.MessageBox("Sorry,Update is unsuccessful!");
                }

                BindGridView();
                Clear();
                btnCreate.Visible = true;
                btnUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            btnCreate.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void gdvUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Editting")
                {
                    int UserID = Convert.ToInt32((e.CommandArgument).ToString());
                    DataTable dtUser = userBL.GetUserByUserID(UserID);

                    txtid.Text = Convert.ToString(UserID);
                    txtUserName.Text = dtUser.Rows[0]["UserName"].ToString();
                    txtPass.Text = GlobalUI.DecryptPassword(dtUser.Rows[0]["Password"].ToString());

                    btnCreate.Visible = false;
                    btnUpdate.Visible = true;
                }
                if (e.CommandName == "Deleting")
                {
                    int UserID = Convert.ToInt32((e.CommandArgument).ToString());

                    Control ctrl = e.CommandSource as Control;
                    GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;

                    Label usrName = (Label)row.FindControl("lblUserName");

                    if (AccountAdmin == usrName.Text)
                    {
                        GlobalUI.MessageBox("This account can't be deleted!");
                    }
                    else
                    {

                        bool isDelOk = userBL.deletebyUserId(UserID);
                        if (isDelOk)
                        {
                            GlobalUI.MessageBox("Delete is successful!");
                        }
                        else
                        {
                            GlobalUI.MessageBox("Sorry,Delete is unsuccessful!");
                        }
                        BindGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        public void BindGridView()
        {
            try
            {
                DataTable dt = userBL.selectUser();
                gdvUser.DataSource = dt.DefaultView;
                gdvUser.DataBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void gdvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvUser.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void gdvUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    foreach (LinkButton button in e.Row.Cells[9].Controls.OfType<LinkButton>())
                    {
                        if (canEdit == "0" && this.Page.User.Identity.Name != AccountAdmin)
                        {
                            if (button.CommandName == "Editting")
                            {
                                button.Visible = false;
                            }

                        }
                        if (canDel == "0" && this.Page.User.Identity.Name != AccountAdmin)
                        {
                            if (button.CommandName == "Deleting")
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
    