using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;

namespace Account.Setup
{
    public partial class TransactionType_Entry : System.Web.UI.Page
    {
        #region declare
        TransactionType_Entry_BL transBL = new TransactionType_Entry_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();

        string canEdit = "0", canDel = "0";

        string AccountAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();

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
                TransTypeBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSaveOk;

                int typeID = 0;
                if (!String.IsNullOrEmpty(hdfID.Value))
                    typeID = int.Parse(hdfID.Value.ToString());
                else typeID = 0;

                if (IsCategoryExist(typeID, txtTransType.Text))    //check entered trans name is already inserted
                {
                    GlobalUI.MessageBox("This transaction type is already stored!");
                }
                else
                {
                    string loginname = this.Page.User.Identity.Name;

                    switch (btnSave.Text)
                    {
                        case "Save":
                            isSaveOk = SaveNewTransType(txtTransType.Text, loginname);   //save new trans
                            if (isSaveOk)
                            {
                                GlobalUI.MessageBox("Save Successful!");
                            }
                            break;
                        case "Update":
                            isSaveOk = UpdateTransType(typeID, txtTransType.Text, loginname);   //update trans
                            if (isSaveOk)
                            {
                                GlobalUI.MessageBox("Update Successful!");
                            }
                            break;
                    }
                    Clear();
                    btnSave.Text = "Save";
                }
                TransTypeBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void TransTypeBind()
        {
            try
            {
                gdvTransType.DataSource = transBL.TransTypeBind();
                gdvTransType.DataBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void Clear()
        {
            txtTransType.Text = "";
            btnSave.Text = "Save";
        }

        private bool UpdateTransType(int typeID, string transType, string loginname)
        {
            return transBL.UpdateTransType(typeID,transType, loginname);
        }

        private bool SaveNewTransType(string transType, string loginname)
        {
            return transBL.SaveNewTransType(transType, loginname);
        }

        private bool IsCategoryExist(int typeID, string transType)
        {
            return transBL.IsCategoryExist(typeID, transType);
        }

        protected void gdvTransType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Control ctrl = e.CommandSource as Control;
                GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;

                if (e.CommandName == "Cmd_Edit")
                {
                    Label lblid = row.FindControl("lblID") as Label;
                    Label lblType = row.FindControl("lblType") as Label;

                    hdfID.Value = lblid.Text;
                    txtTransType.Text = lblType.Text;

                    btnSave.Text = "Update";
                }
                if (e.CommandName == "Cmd_Delete")
                {
                    Label lblid = row.FindControl("lblID") as Label;

                    delTransType(int.Parse(lblid.Text));
                    GlobalUI.MessageBox("Delete Successful!");
                    TransTypeBind();
                }
            }
            catch (Exception ex)
            {                
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void delTransType(int transID)
        {
             transBL.delTransType(transID);
        }

        protected void gdvTransType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvTransType.PageIndex = e.NewPageIndex;
            TransTypeBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void gdvTransType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    foreach (LinkButton button in e.Row.Cells[5].Controls.OfType<LinkButton>())
                    {
                        if (canEdit == "0" && this.Page.User.Identity.Name != AccountAdmin)
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