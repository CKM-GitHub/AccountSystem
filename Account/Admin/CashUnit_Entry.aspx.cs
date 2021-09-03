using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;

namespace Account.Admin
{
    public partial class CashUnit_Entry : System.Web.UI.Page
    {
        #region declare
        CashUnit_Entry_BL cashBL = new CashUnit_Entry_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CashUnitGridBind();
            }
        }

        private void CashUnitGridBind()
        {
            try
            {
                gdvCashUnits.DataSource = cashBL.CashUnitGridBind();
                gdvCashUnits.DataBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void gdvCashUnits_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        Label lblAccNo = row.FindControl("lblAccNo") as Label;

                        hdfID.Value = lblid.Text;
                        txtCashUnit.Text = lblAccName.Text;

                        btnSave.Text = "Update";
                        break;

                    case "Cmd_Delete":

                        Label lblDel = row.FindControl("lblID") as Label;

                        bool IsDelOk = DeleteCashUnit(int.Parse(lblDel.Text));
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

                CashUnitGridBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private bool DeleteCashUnit(int unitID)
        {
            return cashBL.DeleteCashUnit(unitID);
        }

        protected void gdvCashUnits_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSaveOk;
                int unitID = 0;
                if (!String.IsNullOrEmpty(hdfID.Value))
                    unitID = int.Parse(hdfID.Value.ToString());
                else unitID = 0;

                if (cashBL.IsUnitExist(unitID, txtCashUnit.Text))    //check entered acc name is already inserted
                {
                    GlobalUI.MessageBox("This cash unit is already stored!");
                }
                else
                {
                    string loginname = this.Page.User.Identity.Name;

                    switch (btnSave.Text)
                    {
                        case "Save":
                            isSaveOk = SaveNewCashUnit(txtCashUnit.Text, loginname);   //save new accs
                            if (isSaveOk)
                            {
                                GlobalUI.MessageBox("Save Successful!");
                            }
                            break;
                        case "Update":
                            isSaveOk = UpdateNewCashUnit(unitID, txtCashUnit.Text, loginname);   //update accs
                            if (isSaveOk)
                            {
                                GlobalUI.MessageBox("Update Successful!");
                            }
                            break;
                    }
                    Clear();
                    btnSave.Text = "Save";
                }
                CashUnitGridBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private bool UpdateNewCashUnit(int unitID, string cashUnit, string loginname)
        {
            return cashBL.UpdateNewCashUnit(unitID, cashUnit, loginname);
        }

        private bool SaveNewCashUnit(string accName, string loginname)
        {
            return cashBL.SaveNewCashUnit(accName, loginname);
        }

        private void Clear()
        {
            txtCashUnit.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtCashUnit.Text = "";
            btnSave.Text = "Save";
        }
    }
}