using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;

namespace Account.Admin
{
    public partial class FormEntry : System.Web.UI.Page
    {
        #region declare
        FormEntry_BL frmBL = new FormEntry_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();

        string canEdit = "0", canDel = "0";

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
                FrmsGridBind();
            }
        }

        private void FrmsGridBind()
        {
            try
            {
                gdvFrms.DataSource = frmBL.FrmsGridBind();
                gdvFrms.DataBind();
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
                //int accID=hdfID.Value!=null?int.Parse(hdfID.Value.ToString()):0;
                int frmID = 0;
                if (!String.IsNullOrEmpty(hdfID.Value))
                    frmID = int.Parse(hdfID.Value.ToString());
                else frmID = 0;

                if (IsFormExist(frmID, txtFrmName.Text, txtFrmDir.Text))    //check entered form name is already inserted
                {
                    GlobalUI.MessageBox("This account is already stored!");
                }
                else
                {
                    string loginname = this.Page.User.Identity.Name;

                    switch (btnSave.Text)
                    {
                        case "Save":
                            isSaveOk = SaveNewFrm(txtFrmName.Text,txtFrmDir.Text, txtParent.Text,loginname);   //save new forms
                            if (isSaveOk)
                            {
                                GlobalUI.MessageBox("Save Successful!");
                            }
                            break;
                        case "Update":
                            isSaveOk = UpdateFrm(frmID,txtFrmName.Text,txtFrmDir.Text, txtParent.Text, loginname);   //update forms
                            if (isSaveOk)
                            {
                                GlobalUI.MessageBox("Update Successful!");
                            }
                            break;
                    }
                    Clear();
                    btnSave.Text = "Save";
                }
                FrmsGridBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void Clear()
        {
            txtFrmName.Text = "";
            txtFrmDir.Text = "";
            txtParent.Text = "";
            btnSave.Text = "Save";
        }

        private bool IsFormExist(int frmID, string frmName,string frmDir)
        {
            return frmBL.IsFormExist(frmID, frmName, frmDir);
        }

        private bool SaveNewFrm(string frmName, string frmDir, string parent,string loginname)
        {
            return frmBL.SaveNewFrm(frmName, frmDir, parent, loginname);
        }

        private bool UpdateFrm(int frmID, string frmName, string frmDir, string parent, string loginname)
        {
            return frmBL.UpdateFrm(frmID, frmName, frmDir, parent, loginname);
        }

        protected void gdvFrms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Control ctrl = e.CommandSource as Control;
                GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;

                switch (e.CommandName)
                {

                    case "Cmd_Edit":

                        Label lblid = row.FindControl("lblID") as Label;

                        Label lblFrmName = row.FindControl("lblFormName") as Label;
                        Label lblFrmDir = row.FindControl("lblFormDir") as Label;
                        Label lblParent = row.FindControl("lblParent") as Label;

                        hdfID.Value = lblid.Text;
                        txtFrmName.Text = lblFrmName.Text;
                        txtFrmDir.Text = lblFrmDir.Text;
                        txtParent.Text = lblParent.Text;

                        btnSave.Text = "Update";
                        break;

                    case "Cmd_Delete":

                        Label lblDelID = row.FindControl("lblID") as Label;

                        bool IsDelOk = DeleteFrm(int.Parse(lblDelID.Text));
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

                FrmsGridBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private bool DeleteFrm(int frmID)
        {
            return frmBL.DeleteFrm(frmID);
        }

        protected void gdvFrms_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvFrms.PageIndex = e.NewPageIndex;
            FrmsGridBind();
            Clear();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void gdvFrms_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    foreach (LinkButton button in e.Row.Cells[7].Controls.OfType<LinkButton>())
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