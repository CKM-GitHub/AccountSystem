using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;
using Microsoft.Reporting.WebForms;

namespace Account
{
    public partial class TransactionReport : System.Web.UI.Page
    {
        Transaction_BL transBL = new Transaction_BL();
        int account; int stsID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlAccountName_Bind();
                stsNameDdlBind();
            }
        }

        public void ddlAccountName_Bind()
        {
            AccountEntry_BL account = new AccountEntry_BL();
            DataTable dt = account.AccountSelect();
            ddlAccountName.DataSource = dt;
            ddlAccountName.DataTextField = "AccName";
            ddlAccountName.DataValueField = "ID";
            ddlAccountName.DataBind();
            ddlAccountName.Items.Insert(0, "- Select One -");
        }

        private void stsNameDdlBind()
        {
            ddlStatus.DataSource = transBL.stsNameDdlBind();
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataValueField = "stsID";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, "- Select One -");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.Visible = true;
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Account/Report1.rdlc");
                DataTable dt = Getdata();

                ReportDataSource datasource = new ReportDataSource("TransactionDataSet", dt);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.DataBind();
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            { 
                throw ex; 
            }
        }

        protected DataTable Getdata()
        {
            if(ddlAccountName.SelectedItem.Value != "- Select One -")
                account = Convert.ToInt32(ddlAccountName.SelectedItem.Value);
            else 
                account=0;

            if (ddlStatus.SelectedItem.Value != "- Select One -")
                stsID = Convert.ToInt32(ddlStatus.SelectedItem.Value);
            else
                stsID = 0;

            DataTable dt = transBL.Reportdata(account,stsID, txtdate.Text);
            return dt;
        }
    }
}