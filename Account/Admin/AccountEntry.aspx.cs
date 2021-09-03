using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;

namespace Account
{
    public partial class AccountEntry : System.Web.UI.Page
    {
        #region declare
            AccountEntry_BL accBL = new AccountEntry_BL();
            ErrorLog_BL errBL = new ErrorLog_BL();

            string canEdit = "0", canDel = "0";
            string accSpefCtrl = "0";

            Collection<string> roleIDItems = new Collection<string>();
            Collection<string> roleNameItems = new Collection<string>();

            string accAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();
        #endregion

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
                AccsGridBind();
                chkLstRolesBind();
            }
        }

        private void chkLstRolesBind()
        {
            chkLstRoles.DataSource = accBL.GetRoles();
            chkLstRoles.DataTextField = "RoleName";
            chkLstRoles.DataValueField = "RoleID";
            chkLstRoles.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSaveOk;
                //int accID=hdfID.Value!=null?int.Parse(hdfID.Value.ToString()):0;
                int accID = 0;
                if (!String.IsNullOrEmpty(hdfID.Value))
                    accID = int.Parse(hdfID.Value.ToString());
                else accID = 0;

                if (IsAccExist(accID, txtAccName.Text))    //check entered acc name is already inserted
                {
                    GlobalUI.MessageBox("This account is already stored!");
                }
                else
                {
                    string loginname = this.Page.User.Identity.Name;
                    int isCheck = 0;

                    switch (btnSave.Text)
                    {
                        case "Save":
                            if (chkCmpAcc.Checked == true)
                            {
                                isCheck = 1;
                            }

                            for (int index = 0; index < chkLstRoles.Items.Count; index++)
                            {
                                if (chkLstRoles.Items[index].Selected)
                                {
                                    roleIDItems.Add(chkLstRoles.Items[index].Value);
                                    roleNameItems.Add(chkLstRoles.Items[index].Text);
                                }
                            }

                            string roleID = String.Join(",", roleIDItems.ToArray());
                            string roleName = String.Join(",", roleNameItems.ToArray());

                            isSaveOk = SaveNewAcc(txtAccName.Text, txtAccNo.Text, isCheck, roleID, roleName, loginname);   //save new accs
                            if (isSaveOk)
                            {
                                GlobalUI.MessageBox("Save Successful!");
                            }
                            break;
                        case "Update":
                            if (chkCmpAcc.Checked == true)
                            {
                                isCheck = 1;
                            }

                            for (int index = 0; index < chkLstRoles.Items.Count; index++)
                            {
                                if (chkLstRoles.Items[index].Selected)
                                {
                                    roleIDItems.Add(chkLstRoles.Items[index].Value);
                                    roleNameItems.Add(chkLstRoles.Items[index].Text);
                                }
                            }

                            string roleID1 = String.Join(",", roleIDItems.ToArray());
                            string roleName1 = String.Join(",", roleNameItems.ToArray());

                            isSaveOk = UpdateNewAcc(accID, txtAccName.Text, txtAccNo.Text, isCheck, roleID1, roleName1, loginname);   //update accs
                            if (isSaveOk)
                            {
                                GlobalUI.MessageBox("Update Successful!");
                            }
                            break;
                    }
                    Clear();
                    btnSave.Text = "Save";
                }
                AccsGridBind();
            }
            catch (Exception ex)
            {
               errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void Clear()
        {
            txtAccName.Text = "";
            txtAccNo.Text = "";
            chkCmpAcc.Checked = false;
            chkLstRoles.ClearSelection();
        }

        private bool IsAccExist(int accID,string accName)
        {
            return accBL.IsAccExist(accID,accName);
        }

        private bool SaveNewAcc(string accName, string accNo, int isCheck, string roleID, string roleName, string loginname)
        {
            return accBL.SaveNewAcc(accName, accNo, isCheck, roleID, roleName, loginname);
        }

        private bool UpdateNewAcc(int accId, string accName, string accNo, int isCheck, string roleID, string roleName, string loginname)
        {
            return accBL.UpdateNewAcc(accId, accName, accNo, isCheck, roleID, roleName, loginname);
        }

        private void AccsGridBind()
        {
            try
            {
                DataTable dt = accBL.AccsGridBind();

                DataTable dtClone = dt.Clone();

                if (dt.Rows.Count > 0 && this.Page.User.Identity.Name != accAdmin && dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        String[] roles = GlobalUI.GetRoles();

                        accSpefCtrl = Common_Fun.AccSpectCtrl(dr["ID"].ToString(), roles[0]);

                        if (accSpefCtrl != "0")
                        {
                            dtClone.Rows.Add(dr.ItemArray);
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dtClone.Rows.Add(dr.ItemArray);
                    }
                }

                gdvAccs.DataSource = dtClone;
                gdvAccs.DataBind();              
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void gdvAccs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Control ctrl = e.CommandSource as Control;
                GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;

                switch (e.CommandName)
                {

                    case "Cmd_Edit":

                        Label lblid = row.FindControl("lblID") as Label;

                        Label lblAccName = row.FindControl("lblAccName") as Label;
                        Label lblIsRemainingTotal = row.FindControl("lblIsRemainingTotal") as Label;
                        Label lblRoleID = row.FindControl("lblRoleID") as Label;
                        
                        hdfID.Value = lblid.Text;
                        txtAccName.Text = lblAccName.Text;

                        if (lblIsRemainingTotal.Text == "√")
                        {
                            chkCmpAcc.Checked = true;
                        }
                        String[] arrRoleID = new String[10];

                        arrRoleID = lblRoleID.Text.Split(',');
                        int length = arrRoleID.Length;

                        for (int i = 0; i < arrRoleID.Length; i++)
                        {
                            for (int j = 0; j < chkLstRoles.Items.Count; j++)
                            {
                                if (chkLstRoles.Items[j].Value == arrRoleID[i])
                                {
                                    chkLstRoles.Items[j].Selected = true;
                                    break;
                                }
                            }
                        }

                        btnSave.Text = "Update";
                        break;

                    case "Cmd_Delete":

                        Label lblDel = row.FindControl("lblID") as Label;

                        bool IsDelOk = DeleteAcc(int.Parse(lblDel.Text));
                        if (IsDelOk)
                        {
                            GlobalUI.MessageBox("Delete Successful!");
                        }
                        else
                        {
                            GlobalUI.MessageBox("Delete was unsuccessful!");
                        }
                        break;
                }
               
                AccsGridBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private bool DeleteAcc(int accID)
        {
            return accBL.DeleteAcc(accID);
        }

        protected void gdvAccs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvAccs.PageIndex = e.NewPageIndex;
            AccsGridBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Text = "Save";
            chkCmpAcc.Checked = false;
        }

        protected void gdvAccs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    foreach (LinkButton button in e.Row.Cells[10].Controls.OfType<LinkButton>())
                    {
                        if (canEdit == "0" && this.Page.User.Identity.Name != accAdmin)
                        {
                            if (button.CommandName == "Cmd_Edit")
                            {
                                button.Visible = false;
                            }

                        }
                        if (canDel == "0" && this.Page.User.Identity.Name != accAdmin)
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