using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;
using System.Globalization;
using System.Configuration;

namespace Account
{
    public partial class OpeningBalance : System.Web.UI.Page
    {
        #region declare
        OpeningBalance_BL openBL = new OpeningBalance_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();

        string accSpefCtrl = "0";

        string accAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
                ddlAccountName_Bind();
            }
            else
            {
                BindGridView();
            }
        }

        public void ddlAccountName_Bind()
        {
            try
            {
                AccountEntry_BL account = new AccountEntry_BL();
                DataTable dt = account.AccountSelect();

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
                ddlAccountName.DataSource = dtClone;
                ddlAccountName.DataTextField = "AccName";
                ddlAccountName.DataValueField = "ID";
                ddlAccountName.DataBind();
            }
            catch (Exception ex)
            {
               errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string loginname = this.Page.User.Identity.Name;

                string from = Request.Form[datetimepicker4.UniqueID];
                string to = Request.Form[datetimepicker5.UniqueID];

                bool isOK = true;

                if (!String.IsNullOrWhiteSpace(from) && !String.IsNullOrWhiteSpace(to))
                {
                    DateTime fromDate = DateConverter(from);
                    DateTime toDate = DateConverter(to);

                    if (fromDate.Date < toDate.Date)
                    {
                        isOK = true;
                    }
                    else
                    {
                        isOK = false;
                        GlobalUI.MessageBox("To Date can't be less than From Date!");
                        Clear();
                    }
                }

                if (isOK)
                {
                    bool isSaveOk = openBL.Save(ddlAccountName.SelectedItem.Value, txtbalanceUSD.Text, txtbalanceKs.Text,txtbalanceYen.Text, datetimepicker4.Text, datetimepicker5.Text, !String.IsNullOrWhiteSpace(txtRemarks.Text) ? "---" + txtRemarks.Text : txtRemarks.Text, loginname);   //save 
                    if (isSaveOk)
                    {
                        GlobalUI.MessageBox("Save Successful!");
                        BindGridView();
                        Clear();
                    }
                    else
                    {
                        GlobalUI.MessageBox("Save unsuccessful!");
                        Clear();
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
            txtbalanceUSD.Text = "";
            txtbalanceKs.Text = "";
            txtbalanceYen.Text = "";
            datetimepicker4.Text = "";
            datetimepicker5.Text = "";
            txtRemarks.Text = "";
            BindGridView();
        }

        private DateTime DateConverter(string todayDate)
        {
            
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.DateSeparator = "/";
            dtFormat.ShortDatePattern = "dd/MM/yyyy hh:mm tt";

            DateTime objdate = Convert.ToDateTime(todayDate, dtFormat);
            string date = objdate.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
            objdate = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

            return objdate;
          
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string accId = ddlAccountName.SelectedItem.Value;
                decimal balance = Convert.ToDecimal(txtbalanceUSD.Text);
                string fromdate = datetimepicker4.Text;
                string todate = datetimepicker5.Text;
                int obalid = Convert.ToInt32(txtid.Text);
                string loginname = this.Page.User.Identity.Name;

                openBL.updatebyUserId(accId, balance, fromdate, todate, obalid, loginname);
                GlobalUI.MessageBox("Update Successful!");
                BindGridView();
                txtbalanceUSD.Text = " ";
                datetimepicker4.Text = " ";
                datetimepicker5.Text = " ";
                btnCreate.Visible = true;
                btnUpdate.Visible = false;
            }
            catch (Exception ex)
            {
               errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void gdvOBal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Editting")
                {
                    int UserID = Convert.ToInt32((e.CommandArgument).ToString());
                    DataTable dtOBal = openBL.GetID(UserID);

                    txtid.Text = Convert.ToString(UserID);
                    txtbalanceUSD.Text = dtOBal.Rows[0]["OpeningBalance"].ToString();
                    datetimepicker4.Text = dtOBal.Rows[0]["FromDate"].ToString();
                    datetimepicker5.Text = dtOBal.Rows[0]["ToDate"].ToString();
                    ddlAccountName.SelectedItem.Value = dtOBal.Rows[0]["AccountName"].ToString();

                    btnCreate.Visible = false;
                    btnUpdate.Visible = true;
                }
                if (e.CommandName == "Deleting")
                {
                    int UserID = Convert.ToInt32((e.CommandArgument).ToString());
                    openBL.deletebyId(UserID);
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
               errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        public void BindGridView()
        {
            DataTable dt = openBL.selectOBal();

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

            gdvOBal.DataSource = dtClone.DefaultView;
            gdvOBal.DataBind();
        }

        protected void gdvOBal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow gvRow = e.Row;
                if (gvRow.RowType == DataControlRowType.Header)
                {
                    if (gvRow.Cells[1].Text == "Account Name")
                    {
                        gvRow.Cells.Remove(gvRow.Cells[1]);
                        //gvRow.Cells.Remove(gvRow.Cells[5]);
                        //gvRow.Cells.Remove(gvRow.Cells[5]);
                        //gvRow.Cells.Remove(gvRow.Cells[5]);
                        //gvRow.Cells.Remove(gvRow.Cells[5]);
                        //gvRow.Cells.Remove(gvRow.Cells[5]);

                        gvRow.Cells.Remove(gvRow.Cells[7]);
                        gvRow.Cells.Remove(gvRow.Cells[7]);
                        gvRow.Cells.Remove(gvRow.Cells[7]);
                        gvRow.Cells.Remove(gvRow.Cells[7]);
                        gvRow.Cells.Remove(gvRow.Cells[7]);
                       
                        GridViewRow gvHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                        TableCell headerCell0 = new TableCell()
                        {
                            Text = "Account Name",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2

                        };

                        TableCell headerCell1 = new TableCell()
                        {
                            Text = "Opening Balance",
                            HorizontalAlign = HorizontalAlign.Center,
                            ColumnSpan = 3
                        };
                        TableCell headerCell2 = new TableCell()
                        {
                            Text = "Closing Balance",
                            HorizontalAlign = HorizontalAlign.Center,
                            ColumnSpan = 3

                        };
                        TableCell headerCell3 = new TableCell()
                        {
                            Text = "Remarks",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2
                        };
                        //TableCell headerCell4= new TableCell()
                        //{
                        //    Text = "To Date",
                        //    HorizontalAlign = HorizontalAlign.Center,
                        //    RowSpan = 2

                        //};
                        TableCell headerCell5 = new TableCell()
                        {
                            Text = "Created Date",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2

                        };
                        TableCell headerCell6 = new TableCell()
                        {
                            Text = "Updated Date",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2

                        };
                        TableCell headerCell7 = new TableCell()
                        {
                            Text = "CreateBy",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2
                        };
                        
                        gvHeader.Cells.Add(headerCell0);
                        gvHeader.Cells.Add(headerCell1);
                        gvHeader.Cells.Add(headerCell2);
                        gvHeader.Cells.Add(headerCell3);
                        //gvHeader.Cells.Add(headerCell4);
                        gvHeader.Cells.Add(headerCell5);
                        gvHeader.Cells.Add(headerCell6);
                        gvHeader.Cells.Add(headerCell7);

                        gdvOBal.Controls[0].Controls.AddAt(0, gvHeader);
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
               errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void gdvOBal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvOBal.PageIndex = e.NewPageIndex;
            BindGridView();
        }
    }
}