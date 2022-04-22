using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Account_BL;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Drawing2D;


namespace Account
{
    public partial class Transaction_Report : System.Web.UI.Page
    {
        #region declare
        Transaction_Report_BL transBL = new Transaction_Report_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();

        string edtLgPath = ConfigurationManager.AppSettings["EditLogPath"].ToString();
        string delLgPath = ConfigurationManager.AppSettings["DeleteLogPath"].ToString();

        string minYear = ConfigurationManager.AppSettings["minYear"].ToString();

        string attachFolderPath = ConfigurationManager.AppSettings["attachFolderPath"].ToString();

        string exportFormatPath = ConfigurationManager.AppSettings["exportFormatPath"].ToString();
        string transRptExFmtFile = ConfigurationManager.AppSettings["transRptExFmtFile"].ToString();

        string accID = ""; string canEdit = "0", canDel = "0";

        string accSpefCtrl = "0";

        string accAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();
        string canExportUser = ConfigurationManager.AppSettings["canExportUsr"].ToString();

       
        public int accountID
        {
            set
            {
                ViewState["accountID"] = value;
            }
            get
            {
                if (ViewState["accountID"] != null)
                {
                    return (int)ViewState["accountID"];
                }
                else
                {
                    return 0;
                }
            }
        }
        public int search_no
        {
            set { ViewState["search_no"] = value; }
            get {
                if (ViewState["search_no"] != null)
                { return (int)ViewState["search_no"]; }
                else { return 0; }
            }
        }

        public string cashUnit
        {
            set
            {
                ViewState["cashUnit"] = value;
            }
            get
            {
                if (ViewState["cashUnit"] != null)
                {
                    return (string)ViewState["cashUnit"];
                }
                else
                {
                    return null;
                }
            }
        }

        public string updateAmt
        {
            set
            {
                ViewState["updateAmt"] = value;
            }
            get
            {
                if (ViewState["updateAmt"] != null)
                {
                    return (string)ViewState["updateAmt"];
                }
                else
                {
                    return null;
                }
            }
        }

        public int count
        {
            set
            {
                ViewState["count"] = value;
            }
            get
            {
                if (ViewState["count"] != null)
                {
                    return (int)ViewState["count"];
                }
                else
                {
                    return 0;
                }
            }
        }

        public string editLogStr
        {
            set
            {
                ViewState["editLogStr"] = value;
            }
            get
            {
                if (ViewState["editLogStr"] != null)
                {
                    return (string)ViewState["editLogStr"];
                }
                else
                {
                    return null;
                }
            }
        }

        public string updLogStr
        {
            set
            {
                ViewState["updLogStr"] = value;
            }
            get
            {
                if (ViewState["updLogStr"] != null)
                {
                    return (string)ViewState["updLogStr"];
                }
                else
                {
                    return null;
                }
            }
        }

        public string delLogStr
        {
            set
            {
                ViewState["delLogStr"] = value;
            }
            get
            {
                if (ViewState["delLogStr"] != null)
                {
                    return (string)ViewState["delLogStr"];
                }
                else
                {
                    return null;
                }
            }
        }

        public int ctrlAccID
        {
            set
            {
                ViewState["hdfAccID"] = value;
            }
            get
            {
                if (ViewState["hdfAccID"] != null)
                {
                    return (int)ViewState["hdfAccID"];
                }
                else
                {
                    return 0;
                }
            }
        }

        public string ctrlDate
        {
            set
            {
                ViewState["ctrlDate"] = value;
            }
            get
            {
                if (ViewState["ctrlDate"] != null)
                {
                    return ViewState["ctrlDate"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public int ctrlMonth
        {
            set
            {
                ViewState["ctrlMonth"] = value;
            }
            get
            {
                if (ViewState["ctrlMonth"] != null)
                {
                    return int.Parse(ViewState["ctrlMonth"].ToString());
                }
                else
                {
                    return 0;
                }
            }
        }

        public DataTable dtFileName
        {
            get
            {
                if (ViewState["dtFileName"] == null)
                {
                    DataTable dt = new DataTable("t1");
                    return dt;
                }
                else
                {
                    return ViewState["dtFileName"] as DataTable;
                }
            }
            set
            {
                ViewState["dtFileName"] = value;
            }
        }

        #endregion
 

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region getEditDelete Role

                string filePath = Page.AppRelativeVirtualPath;

                String[] roles = GlobalUI.GetRoles();

                string strr = Common_Fun.IsRoleCanEditDel(filePath, roles[0]);

                string[] ctrl = strr.Split(',');

                canEdit = ctrl[0];
                canDel = ctrl[1];

                #endregion

                if (canExportUser.Contains(Page.User.Identity.Name) || accAdmin.Contains(Page.User.Identity.Name))
                {
                    btnExport.Visible = true;
                    btnExportPDF.Visible = true;

                }
                if (!IsPostBack)
                {
                    Session.Remove("dtFileName");
                    ddlAccountName_Bind();
                   ddlStatus_Bind();
                    ddlCashUnit_Bind();
                    TransTypeBind();
                    rptMonthsDataBind();
                    ddlYearBind();
                    if (!String.IsNullOrWhiteSpace(Request.QueryString["ID"]))
                    {
                        ddlAccName.SelectedValue = Session["AccUpdate"].ToString();
                        ddlTransType.SelectedValue = Session["TypeUpdate"].ToString();
                        ddlYear.SelectedValue = Session["YearUpdate"].ToString();
                        ddlStatus.SelectedValue = Session["StatusUpdate"].ToString();
                        ddlCashUnit.SelectedValue = Session["CashUpdate"].ToString();
                        txtFromDate.Text = Session["DateFUpdate"].ToString();
                        txtToDate.Text = Session["DateTUpdate"].ToString();
                        pnlLast.Visible = true;
                        gdvTransReportSnd.Visible = false;
                        SearchMethod();
                        //SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue),
                        //int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
                        search_no = 1;
                        Session["AccUpdate"] = "";
                        Session["TypeUpdate"] = "";
                        Session["YearUpdate"] = "";
                        Session["CashUpdate"] = "";
                        Session["DateFUpdate"] = "";
                        Session["DateTUpdate"] = "";
                    }
                    if (!String.IsNullOrWhiteSpace(Request.QueryString["sav"]))
                    {
                        accID = GlobalUI.DecryptQueryString(Request.QueryString["sav"]);

                        ddlAccName.SelectedValue = accID;
                        //BindData();
                        Update();
                    }
                    else
                    {
                        string str;
                        string[] accIDArr = Request.RawUrl.Split('/');
                        str = accIDArr[accIDArr.Length - 1];

                        if (!String.IsNullOrWhiteSpace(str))
                        {
                            str = str.Replace('#', '/');
                            str = GlobalUI.DecryptQueryString(str);

                            if (str == "Errors")
                            {
                                switch (rdOpt.SelectedValue)
                                {
                                    case "1":
                                        pnlLast.Visible = true;
                                        gdvTransReportSnd.Visible = false;
                                       /// SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
                                        break;
                                    case "2":
                                        pnlLast.Visible = false;
                                        gdvTransReport.Visible = false;
                                      ///  SearchReport(rdOpt.SelectedValue, gdvTransReportSnd, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
                                        break;
                                }
                            }
                            else
                            {
                                string[] a = str.Split('=');
                                int accID = int.Parse(a[1]);

                                ddlAccName.SelectedValue = a[1];

                                accountID = accID;

                                DateTimeFormatInfo usDtfi = new CultureInfo("en-US", false).DateTimeFormat;

                                String lastTransDate = transBL.GetLastTransDate(accID);

                                if (!String.IsNullOrWhiteSpace(lastTransDate))
                                {
                                    DateTime date = Convert.ToDateTime(lastTransDate);
                                    //lastTransDate = date.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                                    String startDate = new DateTime(date.Year, date.Month, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                                    String endDate = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                                    txtFromDate.Text = startDate;
                                    txtToDate.Text = endDate;

                                 ///   SearchReport(rdOpt.SelectedValue, gdvTransReport, accID, 0, 0, "", startDate, endDate);
                                }
                                else
                                {
                                   /// SearchReport(rdOpt.SelectedValue, gdvTransReport, accID, 0, 0, "", "", "");
                                }
                            }
                        }
                    }

                }
                else 
                {
                    //if (Session["tempGdv"] != null)
                    //{
                    //    DataTable dtClone = Session["tempGdv"] as DataTable;
                    //    BindOption("1", dtClone, gdvTransReport);
                    //}
                   
                    try
                    {
                        Control postbackControlInstance = null;
                        string postbackControlName = Page.Request.Params.Get("__EVENTTARGET");
                        
                        //if ((IsPostBack == true && postbackControlName.Contains("lnkTransAttach")) || (IsPostBack == true && postbackControlName.Contains("gdvTransReport")))
                        //{
                        if (search_no != 0)
                        {

                            SearchMethod();
                            //SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);

                        }// SearchMethod();
                          //  UPanel.Update();
                        //}
                    }
                    catch ( Exception exA)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            SearchMethod();
            search_no = 1;

           // UPanel.Update();
        }

        private void SearchMethod()
        {
            try
            {
                String startDate = "", endDate = "";
                if (ctrlMonth != 0)
                {
                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                }
                else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "")
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    }
                }
                else if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    startDate = txtFromDate.Text;
                    endDate = txtToDate.Text;
                }

                switch (rdOpt.SelectedValue)
                {
                    case "1":
                        pnlLast.Visible = true;
                        gdvTransReportSnd.Visible = false;

                        SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                    case "2":
                        pnlLast.Visible = false;
                        gdvTransReport.Visible = false;

                        SearchReport(rdOpt.SelectedValue, gdvTransReportSnd, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            ddlAccName.SelectedIndex = 0;
            ddlTransType.SelectedIndex = 0;
            ddlCashUnit.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;

            rdOpt.SelectedValue = "1";

            ctrlMonth = 0;
            rptMonthsDataBind();

            for (int i = 0; i < ddlYear.Items.Count; i++)
            {
                if (ddlYear.Items[i].Value == "0")
                {
                    ddlYear.SelectedIndex = i;
                }
            }
           /// SearchReport(rdOpt.SelectedValue, gdvTransReport, 0, 0, 0, "", "", "");
        }

        protected void rdOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String startDate = "", endDate = "";
                if (ctrlMonth != 0)
                {
                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                }
                else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "")
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    }
                }
                else if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    startDate = txtFromDate.Text;
                    endDate = txtToDate.Text;
                }

                switch (rdOpt.SelectedValue)
                {
                    case "1":
                        pnlLast.Visible = true;
                        gdvTransReportSnd.Visible = false;

                    ///    SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                    case "2":
                        pnlLast.Visible = false;
                        gdvTransReport.Visible = false;

                   ///     SearchReport(rdOpt.SelectedValue, gdvTransReportSnd, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                }
            }
            catch (Exception ex)
            {
                
               errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        #region 1st Opt gdv (default Opt)

        protected void gdvTransReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow gvRow = e.Row;
                if (gvRow.RowType == DataControlRowType.Header)
                {
                    gvRow.Cells.Remove(gvRow.Cells[0]);
                    gvRow.Cells.Remove(gvRow.Cells[0]);
                    gvRow.Cells.Remove(gvRow.Cells[0]);
                    gvRow.Cells.Remove(gvRow.Cells[0]);

                    //gvRow.Cells.Remove(gvRow.Cells[6]);
                    //gvRow.Cells.Remove(gvRow.Cells[6]);
                    //gvRow.Cells.Remove(gvRow.Cells[6]);
                    //gvRow.Cells.Remove(gvRow.Cells[6]);
                    //gvRow.Cells.Remove(gvRow.Cells[6]);
                    //gvRow.Cells.Remove(gvRow.Cells[6]);

                    gvRow.Cells.Remove(gvRow.Cells[9]);
                    gvRow.Cells.Remove(gvRow.Cells[9]);
                    gvRow.Cells.Remove(gvRow.Cells[9]);
                    gvRow.Cells.Remove(gvRow.Cells[9]);
                    gvRow.Cells.Remove(gvRow.Cells[9]);
                    gvRow.Cells.Remove(gvRow.Cells[9]);

                    GridViewRow gvHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell headerCell0 = new TableCell()
                    {
                        Text = "Date",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2

                    };
                    TableCell headerCell1 = new TableCell()
                    {
                        Text = "Acc Name",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2

                    };
                    TableCell headerCell2 = new TableCell()
                    {
                        Text = "Type",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2

                    };
                    TableCell headerCell3 = new TableCell()
                    {
                        Text = "Particular",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2

                    };
                    TableCell headerCell4 = new TableCell()
                    {
                        Text = "Expense",
                        HorizontalAlign = HorizontalAlign.Center,
                        ColumnSpan = 3

                    };
                    TableCell headerCell5 = new TableCell()
                    {
                        Text = "Income",
                        HorizontalAlign = HorizontalAlign.Center,
                        ColumnSpan = 3
                    };

                    TableCell headerCell6 = new TableCell()
                    {
                        Text = "Remaining Balance",
                        HorizontalAlign = HorizontalAlign.Center,
                        ColumnSpan = 3
                    };
                    TableCell headerCell7 = new TableCell()
                    {
                        Text = "Remark",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2
                    };
                    TableCell headerCell8 = new TableCell()
                    {
                        Text = "Attachments",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2
                    };
                    TableCell headerCell9 = new TableCell()
                    {
                        Text = "CreateBy",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2
                    };

                    TableCell headerCell10 = new TableCell()
                    {
                        Text = "UpdateBy ",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2
                    };

                    gvHeader.Cells.Add(headerCell0);
                    gvHeader.Cells.Add(headerCell1);
                    gvHeader.Cells.Add(headerCell2);
                    gvHeader.Cells.Add(headerCell3);
                    gvHeader.Cells.Add(headerCell4);
                    gvHeader.Cells.Add(headerCell5);
                    gvHeader.Cells.Add(headerCell6);
                    gvHeader.Cells.Add(headerCell7);
                    gvHeader.Cells.Add(headerCell8);
                    gvHeader.Cells.Add(headerCell9);
                    gvHeader.Cells.Add(headerCell10);

                    if (canEdit == "1" || accAdmin == this.Page.User.Identity.Name)
                    {
                        TableCell headerCell11 = new TableCell()
                        {
                            Text = " ",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2
                        };
                        gvHeader.Cells.Add(headerCell11);

                    }

                    if (canDel == "1" || accAdmin == this.Page.User.Identity.Name)
                    {
                        TableCell headerCell12 = new TableCell()
                        {
                            Text = " ",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2
                        };
                        gvHeader.Cells.Add(headerCell12);
                    }
                    //gvHeader.Cells.Add(headerCell13);
                    //gvHeader.Cells.Add(headerCell14);
                    //gvHeader.Cells.Add(headerCell15);
                    //gvHeader.Cells.Add(headerCell16);
                    gdvTransReport.Controls[0].Controls.AddAt(0, gvHeader);

                }
                

                if (e.Row.RowType == DataControlRowType.DataRow && gdvTransReport.EditIndex == e.Row.RowIndex)
                {
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlEditTransTyp");
                    DataTable dt = transBL.TransTypeBind();

                    ddl.DataSource = dt;
                    ddl.DataTextField = "Type";
                    ddl.DataValueField = "Id";
                    ddl.DataBind();

                    DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddlEditAcc");
                    AccountEntry_BL account = new AccountEntry_BL();
                    DataTable dt2 = account.AccountSelect();

                    DataTable dtClone = dt2.Clone();

                    if (dt2.Rows.Count > 0 && this.Page.User.Identity.Name != accAdmin && dt2 != null)
                    {
                        foreach (DataRow dr in dt2.Rows)
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
                        foreach (DataRow dr in dt2.Rows)
                        {
                            dtClone.Rows.Add(dr.ItemArray);
                        }
                    }

                    ddl2.DataSource = dtClone;
                    ddl2.DataTextField = "AccName";
                    ddl2.DataValueField = "ID";
                    ddl2.DataBind();
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;

                    e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[14].HorizontalAlign = HorizontalAlign.Center;

                    e.Row.Cells[15].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[16].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[17].HorizontalAlign = HorizontalAlign.Center;


                    foreach (LinkButton button in e.Row.Cells[14].Controls.OfType<LinkButton>())
                    {
                        if (button.ID == "lnkTransAttach")
                        {
                            if (String.IsNullOrWhiteSpace(button.Text))
                            {
                                button.Text = "<span class='glyphicon glyphicon-paperclip'></span>";
                            }
                        }
                    }

                    if (canEdit == "0" && this.Page.User.Identity.Name != accAdmin)
                    {
                        e.Row.Cells[17].Visible = false;

                    }
                    if (canDel == "0" && this.Page.User.Identity.Name != accAdmin)
                    {
                        e.Row.Cells[18].Visible = false;
                    }

                    //foreach (LinkButton button in e.Row.Cells[17].Controls.OfType<LinkButton>())
                    //{
                    //    if (button.CommandName == "Update")
                    //    {
                    //        button.Attributes["onclick"] = "if(!confirm('Are you sure you want to update?')){ return false; };";
                    //    }
                    //}

                    foreach (LinkButton button in e.Row.Cells[18].Controls.OfType<LinkButton>())
                    {
                        if (button.CommandName == "Delete_Trans")
                        {
                            button.Attributes["onclick"] = "if(!confirm('Are you sure you want to delete?')){ return false; };";
                        }
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow && gdvTransReport.EditIndex != e.Row.RowIndex)
                    {

                        if (((Label)(e.Row.Cells[3].NamingContainer.FindControl("lblRemarks"))).Text == "YwBsAG8ATwBCAEwA")
                        {
                            e.Row.Cells[14].Visible = false;
                            e.Row.Cells[17].Visible = false;
                            e.Row.Cells[18].Visible = false;
                            ((Label)(e.Row.Cells[3].NamingContainer.FindControl("lblRemarks"))).Text = "";
                        }
                    }

                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void gdvTransReport_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells.Remove(e.Row.Cells[0]);
                    e.Row.Cells[0].Text = "Total";
                    e.Row.Cells[0].ColumnSpan = 4;
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                }
                //if (e.Row.RowType == DataControlRowType.Header)
                //{ //Render the header
                //    e.Row.SetRenderMethodDelegate(new RenderMethod(RenderHeader));
                //}
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        //protected void gdvTransReport_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    try
        //    {

        //        GridViewRow row = gdvTransReport.Rows[e.NewEditIndex];
        //        HiddenField hdfID = (HiddenField)row.FindControl("hdfID");
        //        HiddenField hdfAccID = (HiddenField)row.FindControl("hdfAccID");
        //        ctrlAccID = int.Parse(hdfAccID.Value);
        //        ctrlDate = ((Label)row.FindControl("lblEntryDate")).Text;

        //        Label lblTransTyID = (Label)row.FindControl("lblTransTyID");
        //        Label lblAccName = (Label)row.FindControl("lblAccName");

        //        DataTable dt = transBL.TransTypeBind();
        //        if (dt.Rows.Count == 0)
        //        {
        //            GlobalUI.MessageBox("Please insert new transaction types for update case");
        //        }
        //        else
        //        {
        //            #region old edit data log

        //            editLogStr = "\r\n EditDate: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        //            editLogStr += " LoginUserName: " + this.Page.User.Identity.Name + "\r\n Old Trans";
        //            editLogStr += "\r\n TransID: " + hdfID.Value;
        //            editLogStr += "\r\n Date: " + ((Label)row.FindControl("lblEntryDate")).Text;
        //            editLogStr += " Acc Name: " + ((Label)row.FindControl("lblAccName")).Text;
        //            editLogStr += " Trans Typ: " + ((Label)row.FindControl("lblTransType")).Text;
        //            editLogStr += " Particular: " + ((Label)row.FindControl("lblParticular")).Text;
        //            editLogStr += " Expense(USD): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblExpenseS")).Text) ? ((Label)row.FindControl("lblExpenseS")).Text : "0");
        //            editLogStr += " Expense(Kyat): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblExpenseKs")).Text) ? ((Label)row.FindControl("lblExpenseKs")).Text : "0");
        //            editLogStr += " Expense(￥): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblExpenseYen")).Text) ? ((Label)row.FindControl("lblExpenseYen")).Text : "0");

        //            editLogStr += " Income(USD): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblIncomeS")).Text) ? ((Label)row.FindControl("lblIncomeS")).Text : "0");
        //            editLogStr += " Income(Kyat): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblIncomeKs")).Text) ? ((Label)row.FindControl("lblIncomeKs")).Text : "0");
        //            editLogStr += " Income(￥): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblIncomeYen")).Text) ? ((Label)row.FindControl("lblIncomeYen")).Text : "0");

        //            editLogStr += " RemainBal(USD): " + ((Label)row.FindControl("lblRemainUSD")).Text;
        //            editLogStr += " RemainBal(Kyat): " + ((Label)row.FindControl("lblRemainKs")).Text;
        //            editLogStr += " RemainBal(￥): " + ((Label)row.FindControl("lblRemainYen")).Text;

        //            editLogStr += " Remark: " + ((Label)row.FindControl("lblRemarks")).Text;
        //            editLogStr += " CreatedBy: " + ((Label)row.FindControl("lblCreateBy")).Text;
        //            editLogStr += " UpdatedBy: " + ((Label)row.FindControl("lblUpdateBy")).Text;

        //            #endregion

        //            gdvTransReport.EditIndex = e.NewEditIndex;
        //            string startDate = "", endDate = "";
        //            if (ctrlMonth != 0)
        //            {
        //                if (ddlYear.SelectedValue == "0")
        //                {
        //                    ddlYear.SelectedIndex = 1;
        //                }

        //                startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        //                endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

        //            }
        //            else
        //            {
        //                if (txtFromDate.Text == "" && txtToDate.Text == "")
        //                {
        //                    if (ddlYear.SelectedValue != "0")
        //                    {
        //                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        //                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        //                    }
        //                }
        //                else
        //                {
        //                    startDate = txtFromDate.Text;
        //                    endDate = txtToDate.Text;
        //                }
        //            }

        //            SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);

        //            row = gdvTransReport.Rows[e.NewEditIndex];
        //            DropDownList ddl = (DropDownList)row.FindControl("ddlEditTransTyp");

        //            ddl.DataSource = dt;
        //            ddl.DataTextField = "Type";
        //            ddl.DataValueField = "Id";
        //            ddl.DataBind();

        //            ddl.SelectedValue = lblTransTyID.Text;

        //            row = gdvTransReport.Rows[e.NewEditIndex];
        //            DropDownList ddl2 = (DropDownList)row.FindControl("ddlEditAcc");

        //            AccountEntry_BL account = new AccountEntry_BL();
        //            DataTable dt2 = account.AccountSelect();

        //            DataTable dtClone = dt2.Clone();

        //            if (dt2.Rows.Count > 0 && this.Page.User.Identity.Name != accAdmin && dt2 != null)
        //            {
        //                foreach (DataRow dr in dt2.Rows)
        //                {
        //                    String[] roles = GlobalUI.GetRoles();

        //                    accSpefCtrl = Common_Fun.AccSpectCtrl(dr["ID"].ToString(), roles[0]);

        //                    if (accSpefCtrl != "0")
        //                    {
        //                        dtClone.Rows.Add(dr.ItemArray);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (DataRow dr in dt2.Rows)
        //                {
        //                    dtClone.Rows.Add(dr.ItemArray);
        //                }
        //            }

        //            ddl2.DataSource = dtClone;
        //            ddl2.DataTextField = "AccName";
        //            ddl2.DataValueField = "ID";
        //            ddl2.DataBind();

        //            ddl2.SelectedValue = hdfAccID.Value;
        //        }
        //    }
        //    catch (Exception ex) 
        //    {
        //        errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
        //    }
        //}

        //protected void gdvTransReport_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    try
        //    {
        //        gdvTransReport.EditIndex = -1;
        //        string startDate = "", endDate = "";
        //        if (ctrlMonth != 0)
        //        {
        //            if (ddlYear.SelectedValue == "0")
        //            {
        //                ddlYear.SelectedIndex = 1;
        //            }

        //            startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        //            endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        //        }
        //        else
        //        {
        //            if (txtFromDate.Text == "" && txtToDate.Text == "")
        //            {
        //                if (ddlYear.SelectedValue != "0")
        //                {
        //                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        //                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        //                }
        //            }
        //            else
        //            {
        //                startDate = txtFromDate.Text;
        //                endDate = txtToDate.Text;
        //            }
        //        }
        //        SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);

        //    }
        //    catch (Exception ex)
        //    {

        //       errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
        //    }
        //}

        //protected void gdvTransReport_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    try
        //    {

        //        GridViewRow row = gdvTransReport.Rows[e.RowIndex];

        //        HiddenField hdfID = (HiddenField)row.FindControl("hdfID");
        //        HiddenField hdfAccID = (HiddenField)row.FindControl("hdfAccID");
        //        HiddenField hdfStsID = (HiddenField)row.FindControl("hdfStsID");

        //        TextBox txtExpenseUSD = (TextBox)row.FindControl("txtExpenseUSD");
        //        TextBox txtExpenseKs = (TextBox)row.FindControl("txtExpenseKs");
        //        TextBox txtExpenseYen = (TextBox)row.FindControl("txtExpenseYen");

        //        TextBox txtIncomeUSD = (TextBox)row.FindControl("txtIncomeUSD");
        //        TextBox txtIncomeKs = (TextBox)row.FindControl("txtIncomeKs");
        //        TextBox txtIncomeYen = (TextBox)row.FindControl("txtIncomeYen");

        //        TextBox[] arrtext = { txtExpenseUSD, txtExpenseKs, txtExpenseYen, txtIncomeUSD, txtIncomeKs, txtIncomeYen };
        //        int stsID = 0;
        //        for (int i = 0; i < arrtext.Length; i++)
        //        {
        //            if (!String.IsNullOrWhiteSpace(arrtext[i].Text))
        //            {
        //                count += 1;
        //                if (count > 1)
        //                {
        //                    count = 0;
        //                    break;
        //                }
        //                if (count == 1)
        //                {
        //                    updateAmt = arrtext[i].Text;
        //                    //cashUnit = arrtext[i].ID.Contains("USD") ? gdvTransReport.HeaderRow.Cells[0].Text : gdvTransReport.HeaderRow.Cells[1].Text;
        //                    // cashUnit = arrtext[i].ID.Contains("USD") ? gdvTransReport.HeaderRow.Cells[0].Text : gdvTransReport.HeaderRow.Cells[1].Text;
        //                    if (arrtext[i].ID.Contains("USD"))
        //                    {
        //                        cashUnit = gdvTransReport.HeaderRow.Cells[0].Text;
        //                    }
        //                    else if (arrtext[i].ID.Contains("Ks"))
        //                    {
        //                        cashUnit = gdvTransReport.HeaderRow.Cells[1].Text;
        //                    }
        //                    else
        //                    {
        //                        cashUnit = gdvTransReport.HeaderRow.Cells[2].Text;
        //                    }
        //                    stsID = arrtext[i].ID.Contains("Expense") ? 1 : 2;
        //                }
        //            }
        //        }

        //        TextBox txtEdtDate = (TextBox)row.FindControl("txtEditDate");

        //        DropDownList ddl = (DropDownList)row.FindControl("ddlEditTransTyp");
        //        DropDownList ddl2 = (DropDownList)row.FindControl("ddlEditAcc");

        //        TextBox txtParticular = (TextBox)row.FindControl("txtParticular");
        //        TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");

        //        if (count == 1)
        //        {

        //            #region updated data log
        //            updLogStr = "\r\n New Trans";
        //            updLogStr += "\r\n TransID: " + hdfID.Value;
        //            updLogStr += "\r\n Date: " + ((TextBox)row.FindControl("txtEditDate")).Text;
        //            updLogStr += " Acc Name: " + ddl2.SelectedItem.Text;
        //            updLogStr += " Trans Typ: " + ddl.SelectedItem.Text;
        //            updLogStr += " Particular: " + txtParticular.Text;
        //            updLogStr += " Expense(USD): " + (!String.IsNullOrWhiteSpace(txtExpenseUSD.Text) ? txtExpenseUSD.Text : "0");
        //            updLogStr += " Expense(Kyat): " + (!String.IsNullOrWhiteSpace(txtExpenseKs.Text) ? txtExpenseKs.Text : "0");
        //            updLogStr += " Expense(￥): " + (!String.IsNullOrWhiteSpace(txtExpenseYen.Text) ? txtExpenseYen.Text : "0");
        //            updLogStr += " Income(USD): " + (!String.IsNullOrWhiteSpace(txtIncomeUSD.Text) ? txtIncomeUSD.Text : "0");
        //            updLogStr += " Income(Kyat): " + (!String.IsNullOrWhiteSpace(txtIncomeKs.Text) ? txtIncomeKs.Text : "0");
        //            updLogStr += " Income(￥): " + (!String.IsNullOrWhiteSpace(txtIncomeYen.Text) ? txtIncomeYen.Text : "0");
        //            updLogStr += " RemainBal(USD): " + ((Label)row.FindControl("lblRemainUSD")).Text;
        //            updLogStr += " RemainBal(Kyat): " + ((Label)row.FindControl("lblRemainKs")).Text;
        //            updLogStr += " RemainBal(￥): " + ((Label)row.FindControl("lblRemainYen")).Text;
        //            updLogStr += " Remark: " + txtRemarks.Text;
        //            updLogStr += " CreatedBy: " + ((Label)row.FindControl("lblCreateBy")).Text;
        //            updLogStr += " UpdatedBy: " + ((Label)row.FindControl("lblUpdateBy")).Text;

        //            // ConsoleWriteLine_Tofile(edtLgPath, editLogStr + updLogStr);
        //            #endregion

        //            if (ctrlAccID == int.Parse(ddl2.SelectedValue))
        //            {
        //                //if (ctrlDate == txtEdtDate.Text)
        //                //{
        //                transBL.UpdateTrans(int.Parse(hdfID.Value), int.Parse(hdfAccID.Value), int.Parse(ddl.SelectedValue), txtParticular.Text, txtRemarks.Text, stsID, updateAmt, cashUnit, this.Page.User.Identity.Name);
        //                // }
        //                //else
        //                //{
        //                //bool isDelOK = transBL.DelTrans(int.Parse(hdfID.Value));
        //                //if (isDelOK)
        //                //{
        //                //    //Update();
        //                //    Transaction_BL transBL1 = new Transaction_BL();
        //                //    int newTrans = transBL1.SaveTransaction(int.Parse(ddl2.SelectedValue), stsID, updateAmt, cashUnit, int.Parse(ddl.SelectedValue), txtRemarks.Text, txtParticular.Text, txtEdtDate.Text, this.Page.User.Identity.Name);

        //                //    UpdateTransAttach(hdfID.Value, newTrans, ddl2.SelectedValue);
        //                //   // Update();
        //                //}
        //                // }
        //            }
        //            else
        //            {
        //                bool isDelOK = transBL.DelTrans(int.Parse(hdfID.Value));

        //                if (isDelOK)
        //                {
        //                    // Update();
        //                    Transaction_BL transBL1 = new Transaction_BL();
        //                    int newTrans = transBL1.SaveTransaction(int.Parse(ddl2.SelectedValue), stsID, updateAmt, cashUnit, int.Parse(ddl.SelectedValue), txtRemarks.Text, txtParticular.Text, txtEdtDate.Text, this.Page.User.Identity.Name);

        //                    UpdateTransAttach(hdfID.Value, newTrans, ddl2.SelectedValue);
        //                    //Update();
        //                }
        //            }
        //            count = 0;
        //            gdvTransReport.EditIndex = -1;
        //            //Update();
        //            BindData();

        //            GlobalUI.MessageBox("Update Successful");
        //        }
        //        else
        //        {
        //            GlobalUI.MessageBox("Please enter only expense or income!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobalUI.MessageBox("Update Unsuccessful");
        //        errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
        //    }
        //}

        private void UpdateTransAttach(string oldTransID, int newTransID, string accID)
        {
            try
            {
                DataTable dt = transBL.updTransAttach(int.Parse(oldTransID), newTransID);

                string newFolder = Server.MapPath(attachFolderPath + "MUssVBwgcG8=" + accID + "\\" + newTransID.ToString() + "\\");
                string oldFolder = Server.MapPath(attachFolderPath + "MUssVBwgcG8=" + ctrlAccID + "\\" + oldTransID + "\\");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!String.IsNullOrWhiteSpace(dt.Rows[i]["FileName"].ToString()))
                    {
                        string fileName = dt.Rows[i]["FileName"] as string;

                        if (!Directory.Exists(newFolder))
                        {
                            Directory.CreateDirectory(newFolder);
                        }

                        //move files from sessionid folder to new saved transid folder
                        if (Directory.Exists(oldFolder))
                        {
                            string newFile = newFolder + fileName;
                            string oldFile = oldFolder + fileName;

                            File.Move(oldFile, newFile);
                            File.Delete(oldFile);

                            foreach (string folder in Directory.GetDirectories(oldFolder))
                            {
                                string source = Path.Combine(oldFolder, Path.GetFileName(folder));
                                string dest = Path.Combine(newFolder, Path.GetFileName(folder));
                                Directory.Move(source, dest);
                            }
                        }
                    }
                }

                if (Directory.Exists(oldFolder))
                {
                    string[] files = Directory.GetFiles(oldFolder);

                    if (files.Length == 0)
                    {
                        Directory.Delete(oldFolder);
                    }
                }
            }
            catch (Exception ex)
            {
                
               errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        //protected void gdvTransReport_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow row = gdvTransReport.Rows[e.RowIndex];

        //        HiddenField hdfID = (HiddenField)row.FindControl("hdfID");
        //        HiddenField hdfAccID = (HiddenField)row.FindControl("hdfAccID");

        //        bool isDelOK = transBL.DelTrans(int.Parse(hdfID.Value));

        //        string newFolder = Server.MapPath(attachFolderPath + "MUssVBwgcG8=" + hdfAccID.Value.ToString() + "\\" + hdfID.Value.ToString() + "\\");

        //        string delFolder = newFolder + "DEL\\";

        //        string crrdate = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_sstt", CultureInfo.GetCultureInfo("en-US"));

        //        DataTable dt = transBL.GetTransAttachs(int.Parse(hdfID.Value));

        //        if (!Directory.Exists(delFolder))
        //        {
        //            Directory.CreateDirectory(delFolder);
        //        }

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (Directory.Exists(newFolder))
        //            {
        //                string oldFile = newFolder + dt.Rows[i]["FileName"];
        //                string newFile = delFolder + crrdate + "_DELETE_" + dt.Rows[i]["FileName"];

        //                File.Move(oldFile, newFile);
        //                File.Delete(oldFile);
        //            }
        //        }
        //        transBL.DelTransAttachByTransID(int.Parse(hdfID.Value));

        //        if (isDelOK)
        //        {
        //            #region del data log

        //            delLogStr = "\r\n DeletedDate: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        //            delLogStr += " LoginUserName: " + this.Page.User.Identity.Name;
        //            delLogStr += "\r\n TransID: " + hdfID.Value;
        //            delLogStr += "\r\n Date: " + ((Label)row.FindControl("lblEntryDate")).Text;
        //            delLogStr += " Acc Name: " + ((Label)row.FindControl("lblAccName")).Text;
        //            delLogStr += " Trans Typ: " + ((Label)row.FindControl("lblTransType")).Text;
        //            delLogStr += " Particular: " + ((Label)row.FindControl("lblParticular")).Text;
        //            delLogStr += " Expense(USD): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblExpenseS")).Text) ? ((Label)row.FindControl("lblExpenseS")).Text : "0");
        //            delLogStr += " Expense(Kyat): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblExpenseKs")).Text) ? ((Label)row.FindControl("lblExpenseKs")).Text : "0");
        //            delLogStr += " Expense(￥): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblExpenseYen")).Text) ? ((Label)row.FindControl("lblExpenseYen")).Text : "0");
        //            delLogStr += " Income(USD): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblIncomeS")).Text) ? ((Label)row.FindControl("lblIncomeS")).Text : "0");
        //            delLogStr += " Income(Kyat): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblIncomeKs")).Text) ? ((Label)row.FindControl("lblIncomeKs")).Text : "0");
        //            delLogStr += " Income(￥): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblIncomeYen")).Text) ? ((Label)row.FindControl("lblIncomeYen")).Text : "0");
        //            delLogStr += " RemainBal(USD): " + ((Label)row.FindControl("lblRemainUSD")).Text;
        //            delLogStr += " RemainBal(Kyat): " + ((Label)row.FindControl("lblRemainKs")).Text;
        //            delLogStr += " RemainBal(￥): " + ((Label)row.FindControl("lblRemainYen")).Text;

        //            delLogStr += " Remark: " + ((Label)row.FindControl("lblRemarks")).Text;
        //            delLogStr += " CreatedBy: " + ((Label)row.FindControl("lblCreateBy")).Text;
        //            delLogStr += " UpdatedBy: " + ((Label)row.FindControl("lblUpdateBy")).Text;

        //            ConsoleWriteLine_Tofile(delLgPath, delLogStr);

        //            #endregion

        //            BindData();

        //            //Update();

        //            GlobalUI.MessageBox("Delete Successful!");

        //            //SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), 0, int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
        //        }
        //        else
        //        {
        //            GlobalUI.MessageBox("Delete Unsuccessful!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //       errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
        //    }
        //}

        #endregion

        #region 2nd Opt gdv

        protected void gdvTransReportSnd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow gvRow = e.Row;
                if (gvRow.RowType == DataControlRowType.Header)
                {
                    gvRow.Cells.Remove(gvRow.Cells[0]);
                    gvRow.Cells.Remove(gvRow.Cells[0]);
                    gvRow.Cells.Remove(gvRow.Cells[0]);
                    gvRow.Cells.Remove(gvRow.Cells[0]);
                    //gvRow.Cells.Remove(gvRow.Cells[6]);
                    //gvRow.Cells.Remove(gvRow.Cells[6]);
                    //gvRow.Cells.Remove(gvRow.Cells[6]);


                    gvRow.Cells.Remove(gvRow.Cells[9]);
                    gvRow.Cells.Remove(gvRow.Cells[9]);
                    gvRow.Cells.Remove(gvRow.Cells[9]);

                    GridViewRow gvHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell headerCell0 = new TableCell()
                    {
                        Text = "Date",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2

                    };
                    TableCell headerCell1 = new TableCell()
                    {
                        Text = "Acc Name",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2

                    };
                    TableCell headerCell2 = new TableCell()
                    {
                        Text = "Type",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2

                    };
                    TableCell headerCell3 = new TableCell()
                    {
                        Text = "Particular",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2

                    };
                    TableCell headerCell4 = new TableCell()
                    {
                        Text = "Expense",
                        HorizontalAlign = HorizontalAlign.Center,
                        ColumnSpan = 3

                    };
                    TableCell headerCell5 = new TableCell()
                    {
                        Text = "Income",
                        HorizontalAlign = HorizontalAlign.Center,
                        ColumnSpan = 3
                    };

                    TableCell headerCell6 = new TableCell()
                    {
                        Text = "Remaining Balance",
                        HorizontalAlign = HorizontalAlign.Center,
                        ColumnSpan = 3
                    };
                    TableCell headerCell7 = new TableCell()
                    {
                        Text = "Remark",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2
                    };
                    TableCell headerCell8 = new TableCell()
                    {
                        Text = "CreateBy",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2
                    };

                    TableCell headerCell9 = new TableCell()
                    {
                        Text = "UpdateBy ",
                        HorizontalAlign = HorizontalAlign.Center,
                        RowSpan = 2
                    };

                    gvHeader.Cells.Add(headerCell0);
                    gvHeader.Cells.Add(headerCell1);
                    gvHeader.Cells.Add(headerCell2);
                    gvHeader.Cells.Add(headerCell3);
                    gvHeader.Cells.Add(headerCell4);
                    gvHeader.Cells.Add(headerCell5);
                    gvHeader.Cells.Add(headerCell6);
                    gvHeader.Cells.Add(headerCell7);
                    gvHeader.Cells.Add(headerCell8);
                    gvHeader.Cells.Add(headerCell9);

                    gdvTransReportSnd.Controls[0].Controls.AddAt(0, gvHeader);


                    //gvRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                }

                if (e.Row.RowType == DataControlRowType.DataRow && gdvTransReportSnd.EditIndex == e.Row.RowIndex)
                {
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlEditTransTyp");
                    DataTable dt = transBL.TransTypeBind();

                    ddl.DataSource = dt;
                    ddl.DataTextField = "Type";
                    ddl.DataValueField = "Id";
                    ddl.DataBind();

                    DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddlEditAcc");
                    AccountEntry_BL account = new AccountEntry_BL();
                    DataTable dt2 = account.AccountSelect();

                    ddl2.DataSource = dt2;
                    ddl2.DataTextField = "AccName";
                    ddl2.DataValueField = "ID";
                    ddl2.DataBind();
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;

                    e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Center;

                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void gdvTransReportSnd_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells.Remove(e.Row.Cells[0]);
                    e.Row.Cells[0].Text = "Total";
                    e.Row.Cells[0].ColumnSpan = 4;
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                    e.Row.Cells.Remove(e.Row.Cells[5]);
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        #endregion

        #region binding datas

        private void ddlCashUnit_Bind()
        {
            DataTable dt = transBL.ddlCashUnit_Bind();
            ddlCashUnit.DataSource = dt;
            ddlCashUnit.DataTextField = "CashUnit";
            ddlCashUnit.DataValueField = "ID";
            ddlCashUnit.DataBind();
            ddlCashUnit.Items.Insert(0, "- Select One -");
            ddlCashUnit.Items[0].Value = "0";
        }

        private void ddlStatus_Bind()
        {
            ddlStatus.DataSource = transBL.stsNameDdlBind();
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataValueField = "stsID";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, "- Select One -");
            ddlStatus.Items[0].Value = "0";
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

                ddlAccName.DataSource = dtClone;
                ddlAccName.DataTextField = "AccName";
                ddlAccName.DataValueField = "ID";
                ddlAccName.DataBind();
                ddlAccName.Items.Insert(0, "- Select One -");
                ddlAccName.Items[0].Value = "0";
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
                DataTable dt = transBL.TransTypeBind();
                ddlTransType.DataSource = dt;
                ddlTransType.DataTextField = "Type";
                ddlTransType.DataValueField = "Id";
                ddlTransType.DataBind();
                ddlTransType.Items.Insert(0, "- Select One -");
                ddlTransType.Items[0].Value = "0";
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void ddlYearBind()
        {
            //Fill Years
            for (int i = int.Parse(System.DateTime.Now.Year.ToString()); i >= int.Parse(minYear); i--)
            {
                ddlYear.Items.Add(i.ToString());
            }

            ddlYear.Items.Insert(0, "- Select -");
            ddlYear.Items[0].Value = "0";

            //for (int i = 0; i < ddlYear.Items.Count; i++)
            //{
            //    if (ddlYear.Items[i].Text == System.DateTime.Now.Year.ToString())
            //    {
            //        ddlYear.SelectedIndex = i;
            //    }
            //}
        }

        private void rptMonthsDataBind()
        {
            try
            {
                DataTable dt = new DataTable();

                DataColumn dc = new DataColumn("MonthName", typeof(System.String));
                dt.Columns.Add(dc);

                for (int i = 1; i <= 12; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["MonthName"] = i;
                    dt.Rows.Add(dr);
                }

                rptMonths.DataSource = dt;
                rptMonths.DataBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        #endregion

        #region methods

        private void Update()
        {

            try
            {
                DataTable dt = transBL.SearchReport(0, 0, 0, "", "", "");

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    int lstAccID = int.Parse(dt.Rows[i]["ACCID"].ToString());

                    DateTime date = Convert.ToDateTime(dt.Rows[i]["Date"].ToString());
                    string lstDate = date.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                    DataTable dt2 = transBL.GetLastOBL(int.Parse(dt.Rows[i]["TransID"].ToString()), lstAccID, lstDate, dt.Rows[i]["Created_Date"].ToString());

                    if (dt2.Rows.Count != 0 || dt2 != null)
                    {
                        decimal incUSD, incKs, incYen;
                        decimal expUSD, expKs, expYen ;

                        incUSD = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["IncomeUSD"].ToString()) ? "0" : dt.Rows[i]["IncomeUSD"].ToString());
                        expUSD = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["ExpenseUSD"].ToString()) ? "0" : dt.Rows[i]["ExpenseUSD"].ToString());

                        incKs = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["IncomeKs"].ToString()) ? "0" : dt.Rows[i]["IncomeKs"].ToString());
                        expKs = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["ExpenseKs"].ToString()) ? "0" : dt.Rows[i]["ExpenseKs"].ToString());

                        incYen = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["IncomeYen"].ToString()) ? "0" : dt.Rows[i]["IncomeYen"].ToString());
                        expYen = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["ExpenseYen"].ToString()) ? "0" : dt.Rows[i]["ExpenseYen"].ToString());


                        dt.Rows[i]["OpeningBalanceUSD"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtUSD"].ToString()) + incUSD - expUSD;
                        dt.Rows[i]["OpeningBalanceKs"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtKs"].ToString()) + incKs - expKs;
                        dt.Rows[i]["OpeningBalanceYen"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtYen"].ToString()) + incYen - expYen;

                        transBL.UpdateAllTrans(int.Parse(dt.Rows[i]["TransID"].ToString()), dt.Rows[i]["OpeningBalanceUSD"].ToString(), dt.Rows[i]["OpeningBalanceKs"].ToString(), dt.Rows[i]["OpeningBalanceYen"].ToString());
                    }
                }

                String startDate = "", endDate = "";

                if (ctrlMonth != 0)
                {
                    if (ddlYear.SelectedValue == "0")
                    {
                        ddlYear.SelectedIndex = 1;
                    }
                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                }
                else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "" && accID == "")
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    }
                }
                else if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    startDate = txtFromDate.Text;
                    endDate = txtToDate.Text;
                }

                SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
                //SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), 0, 0, "", startDate, endDate);
              //  SearchMethod();
            }
            catch (Exception ex)
            {
                
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }

        }

        private void BindData()
        {

            try
            {
                //DataTable dt = transBL.SearchReport(0, 0, 0, "", "", "");

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{

                //    int lstAccID = int.Parse(dt.Rows[i]["ACCID"].ToString());

                //    DateTime date = Convert.ToDateTime(dt.Rows[i]["Date"].ToString());
                //    string lstDate = date.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                //    DataTable dt2 = transBL.GetLastOBL(int.Parse(dt.Rows[i]["TransID"].ToString()), lstAccID, lstDate, dt.Rows[i]["Created_Date"].ToString());

                //    if (dt2.Rows.Count != 0 || dt2 != null)
                //    {
                //        decimal incUSD, incKs, incYen;
                //        decimal expUSD, expKs, expYen;

                //        incUSD = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["IncomeUSD"].ToString()) ? "0" : dt.Rows[i]["IncomeUSD"].ToString());
                //        expUSD = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["ExpenseUSD"].ToString()) ? "0" : dt.Rows[i]["ExpenseUSD"].ToString());

                //        incKs = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["IncomeKs"].ToString()) ? "0" : dt.Rows[i]["IncomeKs"].ToString());
                //        expKs = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["ExpenseKs"].ToString()) ? "0" : dt.Rows[i]["ExpenseKs"].ToString());

                //        incYen = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["IncomeYen"].ToString()) ? "0" : dt.Rows[i]["IncomeYen"].ToString());
                //        expYen = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["ExpenseYen"].ToString()) ? "0" : dt.Rows[i]["ExpenseYen"].ToString());


                //        dt.Rows[i]["OpeningBalanceUSD"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtUSD"].ToString()) + incUSD - expUSD;
                //        dt.Rows[i]["OpeningBalanceKs"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtKs"].ToString()) + incKs - expKs;
                //        dt.Rows[i]["OpeningBalanceYen"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtYen"].ToString()) + incYen - expYen;

                //       transBL.UpdateAllTrans(int.Parse(dt.Rows[i]["TransID"].ToString()), dt.Rows[i]["OpeningBalanceUSD"].ToString(), dt.Rows[i]["OpeningBalanceKs"].ToString(), dt.Rows[i]["OpeningBalanceYen"].ToString());
                //    }
                //}

                String startDate = "", endDate = "";

                if (ctrlMonth != 0)
                {
                    if (ddlYear.SelectedValue == "0")
                    {
                        ddlYear.SelectedIndex = 1;
                    }
                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                }
                else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "" && accID == "")
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    }
                }
                else if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    startDate = txtFromDate.Text;
                    endDate = txtToDate.Text;
                }

                SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
                //SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), 0, 0, "", startDate, endDate);
                //  SearchMethod();
            }
            catch (Exception ex)
            {

                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }

        }

        private void ConsoleWriteLine_Tofile(string path, string text)
        {
            String url = Request.ApplicationPath;

            StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.GetEncoding("Shift_Jis"));
            sw.AutoFlush = true;
            Console.SetOut(sw);
            Console.WriteLine(text);
            sw.Close();
            sw.Dispose();
        }

        private void SearchReport(string opt, GridView gdv, int accID, int typeID, int stsID, string cashUnit, string fromDate, string ToDate)
        {
            try
            {
                gdv.Visible = true;

                if (!String.IsNullOrWhiteSpace(fromDate) && !String.IsNullOrWhiteSpace(ToDate))
                {
                    DateTime from = GlobalUI.DateConverter(fromDate);
                    DateTime to = GlobalUI.DateConverter(ToDate);

                    //DateTime to = DateTime.Now.Date;
                    if (from.Date > to.Date)
                    {
                        GlobalUI.MessageBox("To Date can't be less than From Date!");
                    }
                    else
                    {
                        ReportGridBind(opt, gdv, accID, typeID, stsID, cashUnit, fromDate, ToDate);
                    }
                }
                else
                {
                    ReportGridBind(opt, gdv, accID, typeID, stsID, cashUnit, fromDate, ToDate);
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void ReportGridBind(string opt, GridView gdv, int accID, int typeID, int stsID, string cashUnit, string fromDate, string ToDate)
        {
            try
            {
                lblLstOBL.Text = "";

                if (cashUnit == "- Select One -") cashUnit = "";

                DataTable dt = transBL.SearchReport(accID, typeID, stsID, cashUnit, fromDate, ToDate);

                DataTable dtClone = dt.Clone();

                if (dt.Rows.Count > 0 && this.Page.User.Identity.Name != accAdmin && dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        String[] roles = GlobalUI.GetRoles();

                        accSpefCtrl = Common_Fun.AccSpectCtrl(dr["ACCID"].ToString(), roles[0]);

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

                BindOption(opt, dtClone, gdv);
            }
            catch (Exception ex)
            {
                
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }

        }

        private void BindOption(string opt, DataTable dtClone, GridView gdv)
        {
            try
            {
                gdv.Visible = true;
                decimal lstResultAmtUSD = 0, lstResultAmtKs = 0, lstResultAmtYen = 0;
                decimal totalExpenseUSD = 0, totalExpenseKs = 0,totalExpenseYen = 0, totalIncomeUSD = 0, totalIncomeKs = 0 ,totalIncomeYen=0;
                decimal totalOBLUSD = 0, totalOBLKs = 0, totalOBLYen = 0;

                string tempAccID1 = "", tempAccID2 = "";

                if (opt == "1")



                {
                    if (dtClone.Rows.Count > 0 && dtClone != null)
                    {
                        //DataTable dtTemp = dtClone.Clone();
                        //foreach (DataRow dr in dtClone.Rows)
                        //{
                        //    dtTemp.Rows.Add(dr.ItemArray);
                        //}

                        //Session["tempGdv"] = dtTemp;

                        DateTime date = Convert.ToDateTime(dtClone.Rows[0]["Date"].ToString());
                        string prevDate = date.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                        for (int i = 0; i < dtClone.Rows.Count; i++)
                        {
                            if (tempAccID2 != dtClone.Rows[i]["ACCID"].ToString())
                            {
                                tempAccID2 = dtClone.Rows[i]["ACCID"].ToString();

                                int lstAccID = int.Parse(dtClone.Rows[i]["ACCID"].ToString());

                                DateTime date1 = Convert.ToDateTime(dtClone.Rows[i]["Date"].ToString());
                                string lstDate = date1.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                                DataTable dt2 = transBL.GetLastOBL(int.Parse(dtClone.Rows[i]["TransID"].ToString()), lstAccID, lstDate, dtClone.Rows[i]["Created_Date"].ToString());

                                //for adding closing from balance dynamically
                                string closeDate = "";
                                if (!String.IsNullOrWhiteSpace(dt2.Rows[0]["Date"].ToString()))
                                {
                                    DateTime dd = Convert.ToDateTime(dt2.Rows[0]["Date"].ToString());
                                    closeDate = dd.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-US"));
                                }

                                if (dt2.Rows[0]["ResultAmtUSD"].ToString() != "0.00" || dt2.Rows[0]["ResultAmtKs"].ToString() != "0.00" || dt2.Rows[0]["ResultAmtYen"].ToString() != "0.00")
                                {
                                    DataRow dr = dtClone.NewRow();

                                    dr["AccountName"] = dt2.Rows[0]["AccName"].ToString();

                                    dr["Particular"] = "Closing Balance from:" + closeDate;
                                    dr["IncomeUSD"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtUSD"]).ToString();
                                    dr["IncomeKs"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtKs"]).ToString();
                                    dr["IncomeYen"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtYen"]).ToString();

                                    dr["OpeningBalanceUSD"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtUSD"]).ToString();
                                    dr["OpeningBalanceKs"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtKs"]).ToString();
                                    dr["OpeningBalanceYen"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtYen"]).ToString();
                                    dr["Remarks"] = "YwBsAG8ATwBCAEwA";  //just temp val for controlling dynamically inserted closing bal row

                                    dtClone.Rows.InsertAt(dr, i);
                                }
                            }
                        }
                       
                        for (int i = 0; i < dtClone.Rows.Count; i++)
                        {
                            DataTable dt2 = new DataTable();

                            int lstAccID = int.Parse(dtClone.Rows[i]["ACCID"].ToString());

                            // decimal lstResultAmtUSD = 0, lstResultAmtKs = 0, lstResultAmtYen = 0;

                            if (i != 0)
                            {
                                DateTime date1 = Convert.ToDateTime(dtClone.Rows[i]["Date"].ToString());
                                string lstDate = date1.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                                lstResultAmtUSD = Convert.ToDecimal(dtClone.Rows[i - 1]["OpeningBalanceUSD"].ToString());
                                lstResultAmtKs = Convert.ToDecimal(dtClone.Rows[i - 1]["OpeningBalanceKs"].ToString());
                                lstResultAmtYen = Convert.ToDecimal(dtClone.Rows[i - 1]["OpeningBalanceYen"].ToString());
                                //lstResultAmtUSD = Convert.ToDecimal(dtClone.Rows[i]["ResultAmtUSD"].ToString());
                                //lstResultAmtKs = Convert.ToDecimal(dtClone.Rows[i]["ResultAmtKs"].ToString());
                                //lstResultAmtYen = Convert.ToDecimal(dtClone.Rows[i]["ResultAmtYen"].ToString());
                            }
                            //if (!String.IsNullOrWhiteSpace(dtClone.Rows[i]["Date"].ToString()))
                            //{
                            //    DateTime date1 = Convert.ToDateTime(dtClone.Rows[i]["Date"].ToString());
                            //    string lstDate = date1.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                            //    dt2 = transBL.GetLastOBL(int.Parse(dtClone.Rows[i]["TransID"].ToString()), lstAccID, lstDate, dtClone.Rows[i]["Created_Date"].ToString());
                            //    if (dt2.Rows.Count != 0 || dt2 != null)
                            //    {
                            //        lstResultAmtUSD = Convert.ToDecimal(dt2.Rows[0]["ResultAmtUSD"].ToString());
                            //        lstResultAmtKs = Convert.ToDecimal(dt2.Rows[0]["ResultAmtKs"].ToString());
                            //        lstResultAmtYen = Convert.ToDecimal(dt2.Rows[0]["ResultAmtYen"].ToString());
                            //    }

                            //    //lstResultAmtUSD = Convert.ToDecimal(dtClone.Rows[i - 1]["OpeningBalanceUSD"].ToString());
                            //    //lstResultAmtKs = Convert.ToDecimal(dtClone.Rows[i - 1]["OpeningBalanceKs"].ToString());
                            //    //lstResultAmtYen = Convert.ToDecimal(dtClone.Rows[i - 1]["OpeningBalanceYen"].ToString());
                            //}

                            //for binding totals in footer 
                            totalExpenseUSD += dtClone.Rows[i]["ExpenseUSD"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["ExpenseUSD"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["ExpenseUSD"].ToString()) : 0;
                            totalExpenseKs += dtClone.Rows[i]["ExpenseKs"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["ExpenseKs"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["ExpenseKs"].ToString()) : 0;
                            totalExpenseYen += dtClone.Rows[i]["ExpenseYen"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["ExpenseYen"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["ExpenseYen"].ToString()) : 0;

                            totalIncomeUSD += dtClone.Rows[i]["IncomeUSD"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["IncomeUSD"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["IncomeUSD"].ToString()) : 0;
                            totalIncomeKs += dtClone.Rows[i]["IncomeKs"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["IncomeKs"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["IncomeKs"].ToString()) : 0;
                            totalIncomeYen += dtClone.Rows[i]["IncomeYen"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["IncomeYen"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["IncomeYen"].ToString()) : 0;

                            if (tempAccID1 != dtClone.Rows[i]["ACCID"].ToString())
                            {
                                tempAccID1 = dtClone.Rows[i]["ACCID"].ToString();
                                //totalOBLUSD += Convert.ToDecimal(dt.Rows[i]["OpeningBalanceUSD"].ToString());
                                //totalOBLKs += Convert.ToDecimal(dt.Rows[i]["OpeningBalanceKs"].ToString());
                            }

                            //dynamic calc for OBLS  
                            decimal incUSD, incKs,incYen;
                            decimal expUSD, expKs, expYen;

                            incUSD = Convert.ToDecimal(String.IsNullOrWhiteSpace(dtClone.Rows[i]["IncomeUSD"].ToString()) ? "0" : dtClone.Rows[i]["IncomeUSD"].ToString());
                            expUSD = Convert.ToDecimal(String.IsNullOrWhiteSpace(dtClone.Rows[i]["ExpenseUSD"].ToString()) ? "0" : dtClone.Rows[i]["ExpenseUSD"].ToString());

                            incKs = Convert.ToDecimal(String.IsNullOrWhiteSpace(dtClone.Rows[i]["IncomeKs"].ToString()) ? "0" : dtClone.Rows[i]["IncomeKs"].ToString());
                            expKs = Convert.ToDecimal(String.IsNullOrWhiteSpace(dtClone.Rows[i]["ExpenseKs"].ToString()) ? "0" : dtClone.Rows[i]["ExpenseKs"].ToString());

                            incYen = Convert.ToDecimal(String.IsNullOrWhiteSpace(dtClone.Rows[i]["IncomeYen"].ToString()) ? "0" : dtClone.Rows[i]["IncomeYen"].ToString());
                            expYen = Convert.ToDecimal(String.IsNullOrWhiteSpace(dtClone.Rows[i]["ExpenseYen"].ToString()) ? "0" : dtClone.Rows[i]["ExpenseYen"].ToString());

                            dtClone.Rows[i]["OpeningBalanceUSD"] = lstResultAmtUSD + incUSD - expUSD;
                            dtClone.Rows[i]["OpeningBalanceKs"] = lstResultAmtKs + incKs - expKs;
                            dtClone.Rows[i]["OpeningBalanceYen"] = lstResultAmtYen + incYen - expYen;
                            //dtClone.Rows[i]["OpeningBalanceUSD"] = lstResultAmtUSD ;
                            //dtClone.Rows[i]["OpeningBalanceKs"] = lstResultAmtKs  ;
                            //dtClone.Rows[i]["OpeningBalanceYen"] = lstResultAmtYen;

                        }

                        gdv.DataSource = dtClone;
                        gdv.DataBind();

                        tempAccID1 = "";
                        tempAccID2 = "";

                        totalOBLUSD += totalIncomeUSD - totalExpenseUSD;
                        totalOBLKs += totalIncomeKs - totalExpenseKs;
                        totalOBLYen += totalIncomeYen - totalExpenseYen;

                        gdv.FooterRow.Cells[1].Text = totalExpenseUSD.ToString("N2") + "<span style='font-size:x-small'><br/> USD </span>";
                        gdv.FooterRow.Cells[2].Text = totalExpenseKs.ToString("N2") + "<span style='font-size:x-small'><br/> Kyat </span>";
                        gdv.FooterRow.Cells[3].Text = totalExpenseYen.ToString("N2") + "<span style='font-size:x-small'><br/> ￥ </span>";

                        gdv.FooterRow.Cells[4].Text = totalIncomeUSD.ToString("N2") + "<span style='font-size:x-small'><br/> USD </span>";
                        gdv.FooterRow.Cells[5].Text = totalIncomeKs.ToString("N2") + "<span style='font-size:x-small'><br/> Kyat </span>";
                        gdv.FooterRow.Cells[6].Text = totalIncomeYen.ToString("N2") + "<span style='font-size:x-small'><br/> ￥ </span>";

                        gdv.FooterRow.Cells[7].Text = totalOBLUSD.ToString("N2") + "<span style='font-size:x-small'><br/> USD </span>";
                        gdv.FooterRow.Cells[8].Text = totalOBLKs.ToString("N2") + "<span style='font-size:x-small'><br/> Kyat </span>";
                        gdv.FooterRow.Cells[9].Text = totalOBLYen.ToString("N2") + "<span style='font-size:x-small'><br/> ￥ </span>";
                    }
                    else
                    {
                        gdv.DataSource = dtClone;
                        gdv.DataBind();
                    }
                }
                else if (opt == "2")
                {
                    if (dtClone.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtClone.Rows.Count; i++)
                        {
                            totalExpenseUSD += dtClone.Rows[i]["ExpenseUSD"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["ExpenseUSD"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["ExpenseUSD"].ToString()) : 0;
                            totalExpenseKs += dtClone.Rows[i]["ExpenseKs"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["ExpenseKs"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["ExpenseKs"].ToString()) : 0;
                            totalExpenseYen += dtClone.Rows[i]["ExpenseYen"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["ExpenseYen"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["ExpenseYen"].ToString()) : 0;


                            totalIncomeUSD += dtClone.Rows[i]["IncomeUSD"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["IncomeUSD"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["IncomeUSD"].ToString()) : 0;
                            totalIncomeKs += dtClone.Rows[i]["IncomeKs"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["IncomeKs"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["IncomeKs"].ToString()) : 0;
                            totalIncomeYen += dtClone.Rows[i]["IncomeYen"] != null && !String.IsNullOrWhiteSpace(dtClone.Rows[i]["IncomeYen"].ToString()) ? Convert.ToDecimal(dtClone.Rows[i]["IncomeYen"].ToString()) : 0;

                            if (tempAccID1 != dtClone.Rows[i]["ACCID"].ToString())
                            {
                                tempAccID1 = dtClone.Rows[i]["ACCID"].ToString();
                                totalOBLUSD += Convert.ToDecimal(dtClone.Rows[i]["OpeningBalanceUSD"].ToString());
                                totalOBLKs += Convert.ToDecimal(dtClone.Rows[i]["OpeningBalanceKs"].ToString());
                                totalOBLYen += Convert.ToDecimal(dtClone.Rows[i]["OpeningBalanceYen"].ToString());
                            }
                        }

                        gdv.DataSource = dtClone;
                        gdv.DataBind();

                        totalOBLUSD += totalIncomeUSD - totalExpenseUSD;
                        totalOBLKs += totalIncomeKs - totalExpenseKs;
                        totalOBLYen += totalIncomeYen - totalExpenseYen;

                        gdv.FooterRow.Cells[1].Text = totalExpenseUSD.ToString("N2") + "<span style='font-size:x-small'><br/> USD </span>";
                        gdv.FooterRow.Cells[2].Text = totalExpenseKs.ToString("N2") + "<span style='font-size:x-small'><br/> Kyat </span>";
                        gdv.FooterRow.Cells[3].Text = totalExpenseYen.ToString("N2") + "<span style='font-size:x-small'><br/> ￥ </span>";

                        gdv.FooterRow.Cells[4].Text = totalIncomeUSD.ToString("N2") + "<span style='font-size:x-small'><br/> USD </span>";
                        gdv.FooterRow.Cells[5].Text = totalIncomeKs.ToString("N2") + "<span style='font-size:x-small'><br/> Kyat </span>";
                        gdv.FooterRow.Cells[6].Text = totalIncomeYen.ToString("N2") + "<span style='font-size:x-small'><br/> ￥ </span>";

                        gdv.FooterRow.Cells[7].Text = totalOBLUSD.ToString("N2") + "<span style='font-size:x-small'><br/> USD </span>";
                        gdv.FooterRow.Cells[8].Text = totalOBLKs.ToString("N2") + "<span style='font-size:x-small'><br/> Kyat </span>";
                        gdv.FooterRow.Cells[9].Text = totalOBLYen.ToString("N2") + "<span style='font-size:x-small'><br/> ￥ </span>";
                    }
                    else
                    {
                        gdv.DataSource = dtClone;
                        gdv.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                
               errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        #endregion

        protected void lnkBtnMonth_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";

                LinkButton btn = (LinkButton)(sender);

                int month = int.Parse(btn.CommandArgument.ToString());

                if (ddlYear.SelectedValue == "0")
                {
                    ddlYear.SelectedIndex = 1;
                }

                String startDate = new DateTime(int.Parse(ddlYear.SelectedValue), month, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                String endDate = new DateTime(int.Parse(ddlYear.SelectedValue), month, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                ctrlMonth = month;

                switch (rdOpt.SelectedValue)
                {
                    case "1":
                        pnlLast.Visible = true;
                        gdvTransReportSnd.Visible = false;
                        SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                    case "2":
                        pnlLast.Visible = false;
                        gdvTransReport.Visible = false;
                        SearchReport(rdOpt.SelectedValue, gdvTransReportSnd, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                }

                rptMonthsDataBind();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String startDate = "", endDate = "";
                if (ctrlMonth != 0)
                {
                    if (ddlYear.SelectedValue == "0")
                    {
                        ddlYear.SelectedIndex = 1;
                    }

                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                }
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    txtFromDate.Text = "";
                    txtToDate.Text = "";
                }
                if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "")
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    }
                }


                switch (rdOpt.SelectedValue)
                {
                    case "1":
                        pnlLast.Visible = true;
                        gdvTransReportSnd.Visible = false;

                     ///   SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);

                        break;
                    case "2":
                        pnlLast.Visible = false;
                        gdvTransReport.Visible = false;

                  ///      SearchReport(rdOpt.SelectedValue, gdvTransReportSnd, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);

                        break;
                }
               // UPanel.Update();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void rptMonths_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            try
            {
                DataList dL = (DataList)sender;
                LinkButton lnk = (LinkButton)e.Item.FindControl("lnkBtnMonth");

                if (ctrlMonth == int.Parse(lnk.CommandArgument.ToString()))
                {
                    lnk.CssClass = "page_enabled";
                }
                else if (ctrlMonth == 0)
                {
                    //    if (int.Parse(lnk.CommandArgument.ToString()) == 1)
                    //    {
                    //        lnk.CssClass = "page_enabled";
                    //    }
                    //}
                }
                else
                {
                    lnk.CssClass = "page_disabled";
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ctrlMonth != 0)
                {
                    ctrlMonth = 0;
                    rptMonthsDataBind();
                }
                ddlYear.SelectedIndex = 0;
                UpTo.Update();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ctrlMonth != 0)
                {
                    ctrlMonth = 0;
                    rptMonthsDataBind();
                }
                ddlYear.SelectedIndex = 0;
                UpFrom.Update();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void lnkTransAttach_Click(object sender, EventArgs e)
        {
            try
            {
                
                LinkButton l = (LinkButton)sender;
                int transID = Convert.ToInt32(l.CommandArgument);

                string AccID = l.CommandName;

                string filePath = attachFolderPath + "MUssVBwgcG8=" + AccID + "\\" + transID.ToString() + "\\";

                BindModalGridView(AccID, transID, filePath);
                GetLinkButton(AccID);
                Session["Form"] = "Mainreport";
                ClientScript.RegisterStartupScript(this.GetType(), "popup_window", "<script>ShowAtta_PopUp('this')</script>");
               
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected  void  GetLinkButton(string AccID)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }

        }

        private void BindModalGridView(string AccID, int transID, string filePath)
        {
            try
            {
                DataTable dt = transBL.GetTransAttachs(transID);
                DataColumn dc1 = new DataColumn("FolderPath", typeof(string));
                dt.Columns.Add(dc1);

                DataColumn dc2 = new DataColumn("FilePath", typeof(string));
                dt.Columns.Add(dc2);

                if (!dt.Columns.Contains("TransID"))
                {
                    DataColumn dc3 = new DataColumn("TransID", typeof(string));
                    dt.Columns.Add(dc3);
                }

                DataColumn dc4 = new DataColumn("AccID", typeof(string));
                dt.Columns.Add(dc4);

                DataRow dr = dt.NewRow();
                dr["TransID"] = transID;
                dt.Rows.InsertAt(dr, 0);

                dt.Rows[0]["AccID"] = AccID;

                if (dt.Rows.Count > 1)
                {
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        if (!String.IsNullOrWhiteSpace(dt.Rows[i]["FileName"].ToString()))
                        {
                            dt.Rows[i]["FolderPath"] = filePath;
                            dt.Rows[i]["FilePath"] = filePath + dt.Rows[i]["FileName"].ToString();
                        }
                    }
                }

                Session["Report_dtFileName"] = dt;
              //  UPanel.Update();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (gdvTransReport.Rows.Count > 0 && search_no !=0)
                {
                    string excelFmtPath = Server.MapPath(exportFormatPath);
                    string fmtFile = Server.MapPath(exportFormatPath + transRptExFmtFile);

                    var workbook = new XLWorkbook(fmtFile);
                    var ws = workbook.Worksheet(1);

                    //String startDate = "", endDate = "";
                    //if (ctrlMonth != 0)
                    //{
                    //    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    //    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    //}
                    //else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "")
                    //{
                    //    if (ddlYear.SelectedValue != "0")
                    //    {
                    //        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    //        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    //    }
                    //}
                    //else if (txtFromDate.Text != "" && txtToDate.Text != "")
                    //{
                    //    startDate = txtFromDate.Text;
                    //    endDate = txtToDate.Text;
                    //}
                    //if (ddlCashUnit.SelectedItem.Text == "- Select One -") cashUnit = "";
                    //DataTable dt = new DataTable();
                    //switch (rdOpt.SelectedValue)
                    //{
                    //    case "1":
                    //        dt = transBL.SearchReport(int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), cashUnit, startDate, endDate);
                    //        break;
                    //    case "2":
                    //        dt = transBL.SearchReport(int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), cashUnit, startDate, endDate);
                    //        break;
                    //}

                    //DataTable dtClone = dt.Clone();

                    //if (dt.Rows.Count > 0 && this.Page.User.Identity.Name != accAdmin && dt != null)
                    //{
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        String[] roles = GlobalUI.GetRoles();

                    //        accSpefCtrl = Common_Fun.AccSpectCtrl(dr["ACCID"].ToString(), roles[0]);

                    //        if (accSpefCtrl != "0")
                    //        {
                    //            dtClone.Rows.Add(dr.ItemArray);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        dtClone.Rows.Add(dr.ItemArray);
                    //    }
                    //}

                    //if(rdOpt.SelectedValue == "1")
                    //    BindOption("1", dtClone, gdvTransReport);
                    //else
                    //    BindOption("2", dtClone, gdvTransReport);
                    DataTable dtClone = ExportData();

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        // Adding DataRows.
                        for (int i = 0; i < dtClone.Rows.Count; i++)
                        {
                            //ws.Cell("A" + (i + 3)).Value = "'" + Convert.ToDateTime(dtClone.Rows[i]["Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-US"));
                            ws.Cell("A" + (i + 3)).Value = "'" + dtClone.Rows[i]["F_Date"].ToString();
                            ws.Cell("A" + (i + 3)).Style.Font.FontName = "Arial";

                            //ws.Cell("B" + (i + 3)).Value = dtClone.Rows[i]["AccountName"].ToString();
                            //ws.Cell("C" + (i + 3)).Value = dtClone.Rows[i]["Type"].ToString();
                            //ws.Cell("D" + (i + 3)).Value = dtClone.Rows[i]["Particular"].ToString();
                            //ws.Cell("E" + (i + 3)).Value = dtClone.Rows[i]["ExpenseUSD"].ToString();
                            //ws.Cell("F" + (i + 3)).Value = dtClone.Rows[i]["ExpenseKs"].ToString();
                            //ws.Cell("G" + (i + 3)).Value = dtClone.Rows[i]["IncomeUSD"].ToString();
                            //ws.Cell("H" + (i + 3)).Value = dtClone.Rows[i]["IncomeKs"].ToString();
                            //ws.Cell("I" + (i + 3)).Value = dtClone.Rows[i]["ResultAmtUSD"].ToString();
                            //ws.Cell("J" + (i + 3)).Value = dtClone.Rows[i]["ResultAmtKs"].ToString();
                            //ws.Cell("K" + (i + 3)).Value = dtClone.Rows[i]["Remarks"].ToString();
                            //ws.Cell("L" + (i + 3)).Value = dtClone.Rows[i]["CreatedByUser"].ToString();
                            //ws.Cell("M" + (i + 3)).Value = dtClone.Rows[i]["UpdatedByUser"].ToString();



                            ws.Cell("B" + (i + 3)).Value = dtClone.Rows[i]["AccountName"].ToString();
                            ws.Cell("C" + (i + 3)).Value = dtClone.Rows[i]["Type"].ToString();
                            ws.Cell("D" + (i + 3)).Value = dtClone.Rows[i]["Particular"].ToString();
                            ws.Cell("E" + (i + 3)).Value = dtClone.Rows[i]["ExpenseUSD"].ToString();
                            ws.Cell("F" + (i + 3)).Value = dtClone.Rows[i]["ExpenseKs"].ToString();
                            ws.Cell("G" + (i + 3)).Value = dtClone.Rows[i]["ExpenseYen"].ToString();
                            ws.Cell("H" + (i + 3)).Value = dtClone.Rows[i]["IncomeUSD"].ToString();
                            ws.Cell("I" + (i + 3)).Value = dtClone.Rows[i]["IncomeKs"].ToString();
                            ws.Cell("J" + (i + 3)).Value = dtClone.Rows[i]["IncomeYen"].ToString();
                            //if (dtClone.Rows[i]["Remarks"].ToString() != "YwBsAG8ATwBCAEwA" || dtClone.Rows[i]["Particular"].ToString() != "Closing Balance from:")
                            //{
                            //ws.Cell("K" + (i + 3)).Value = dtClone.Rows[i]["ResultAmtUSD"].ToString();
                            //ws.Cell("L" + (i + 3)).Value = dtClone.Rows[i]["ResultAmtKs"].ToString();
                            //ws.Cell("M" + (i + 3)).Value = dtClone.Rows[i]["ResultAmtYen"].ToString();
                            //}
                            //else
                            //{
                                ws.Cell("K" + (i + 3)).Value = dtClone.Rows[i]["OpeningBalanceUSD"].ToString();
                                ws.Cell("L" + (i + 3)).Value = dtClone.Rows[i]["OpeningBalanceKs"].ToString();
                                ws.Cell("M" + (i + 3)).Value = dtClone.Rows[i]["OpeningBalanceYen"].ToString();
                           // }
                            if (dtClone.Rows[i]["Remarks"].ToString() != "YwBsAG8ATwBCAEwA")
                            {
                                ws.Cell("N" + (i + 3)).Value = dtClone.Rows[i]["Remarks"].ToString();
                            }
                            else
                            {
                               
                                dtClone.Columns["Remarks"].ReadOnly = false; 
                                dtClone.Rows[i]["Remarks"] = "";
                                ws.Cell("N" + (i + 3)).Value = dtClone.Rows[i]["Remarks"].ToString();
                            }
                            ws.Cell("O" + (i + 3)).Value = dtClone.Rows[i]["CreatedByUser"].ToString();
                            ws.Cell("P" + (i + 3)).Value = dtClone.Rows[i]["UpdatedByUser"].ToString();
                        }

                        #region for adding total

                        int lastRow = dtClone.Rows.Count + 3;

                        var row = ws.Row(lastRow);
                        row.Height = 25;

                        ws.Cell("A" + lastRow).Value = "Total";
                        ws.Range("A" + lastRow + ":D" + lastRow).Merge();
                        ws.Range("A" + lastRow + ":D" + lastRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Range("A" + lastRow + ":D" + lastRow).Style.Font.FontName = "Arial";

                        int index = gdvTransReport.FooterRow.Cells[1].Text.IndexOf("<");
                        if (index > 0)
                            ws.Cell("E" + lastRow).Value = gdvTransReport.FooterRow.Cells[1].Text.Substring(0, index);

                        int index1 = gdvTransReport.FooterRow.Cells[2].Text.IndexOf("<");
                        if (index1 > 0)
                            ws.Cell("F" + lastRow).Value = gdvTransReport.FooterRow.Cells[2].Text.Substring(0, index1);

                        int index2 = gdvTransReport.FooterRow.Cells[3].Text.IndexOf("<");
                        if (index2 > 0)
                            ws.Cell("G" + lastRow).Value = gdvTransReport.FooterRow.Cells[3].Text.Substring(0, index2);

                        int index3 = gdvTransReport.FooterRow.Cells[4].Text.IndexOf("<");
                        if (index3 > 0)
                            ws.Cell("H" + lastRow).Value = gdvTransReport.FooterRow.Cells[4].Text.Substring(0, index3);

                        int index4 = gdvTransReport.FooterRow.Cells[5].Text.IndexOf("<");
                        if (index4 > 0)
                            ws.Cell("I" + lastRow).Value = gdvTransReport.FooterRow.Cells[5].Text.Substring(0, index4);

                        int index5 = gdvTransReport.FooterRow.Cells[6].Text.IndexOf("<");
                        if (index5 > 0)
                            ws.Cell("J" + lastRow).Value = gdvTransReport.FooterRow.Cells[6].Text.Substring(0, index5);


                        int index6 = gdvTransReport.FooterRow.Cells[7].Text.IndexOf("<");
                        if (index6 > 0)
                            ws.Cell("K" + lastRow).Value = gdvTransReport.FooterRow.Cells[7].Text.Substring(0, index6);


                        int index7 = gdvTransReport.FooterRow.Cells[8].Text.IndexOf("<");
                        if (index7> 0)
                            ws.Cell("L" + lastRow).Value = gdvTransReport.FooterRow.Cells[8].Text.Substring(0, index7);

                        int index8 = gdvTransReport.FooterRow.Cells[9].Text.IndexOf("<");
                        if (index8 > 0)
                            ws.Cell("M" + lastRow).Value = gdvTransReport.FooterRow.Cells[9].Text.Substring(0, index8);
                        #endregion
                    }

                    string fileName = "Transaction_Report_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt", CultureInfo.GetCultureInfo("en-us")) + ".xlsx";

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                  //  UPanel.Update();
                }
                else
                {
                    GlobalUI.MessageBox("There is no transactions");
                }
            }
            catch (Exception ex)
            {
                string exMsg = ex.Message;
                if (!ex.Message.Contains("unable to evaluate"))
                {
                    errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                }
            }
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void ddlAccName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String startDate = "", endDate = "";
                if (ctrlMonth != 0)
                {
                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                }
                else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "")
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    }
                }
                else if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    startDate = txtFromDate.Text;
                    endDate = txtToDate.Text;
                }

                switch (rdOpt.SelectedValue)
                {
                    case "1":
                        pnlLast.Visible = true;
                        gdvTransReportSnd.Visible = false;

                      //  SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                    case "2":
                        pnlLast.Visible = false;
                        gdvTransReport.Visible = false;

                    //    SearchReport(rdOpt.SelectedValue, gdvTransReportSnd, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                }
               //
                //UPanel.Update();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String startDate = "", endDate = "";
                if (ctrlMonth != 0)
                {
                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                }
                else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "")
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    }
                }
                else if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    startDate = txtFromDate.Text;
                    endDate = txtToDate.Text;
                }

                switch (rdOpt.SelectedValue)
                {
                    case "1":
                        pnlLast.Visible = true;
                        gdvTransReportSnd.Visible = false;

                    ///    SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                    case "2":
                        pnlLast.Visible = false;
                        gdvTransReport.Visible = false;

                   ///     SearchReport(rdOpt.SelectedValue, gdvTransReportSnd, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                }
               // UPanel.Update();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String startDate = "", endDate = "";
                if (ctrlMonth != 0)
                {
                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                }
                else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "")
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    }
                }
                else if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    startDate = txtFromDate.Text;
                    endDate = txtToDate.Text;
                }

                switch (rdOpt.SelectedValue)
                {
                    case "1":
                        pnlLast.Visible = true;
                        gdvTransReportSnd.Visible = false;

                    ///    SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                    case "2":
                        pnlLast.Visible = false;
                        gdvTransReport.Visible = false;

                     ///   SearchReport(rdOpt.SelectedValue, gdvTransReportSnd, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                }
               // UPanel.Update();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void ddlCashUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String startDate = "", endDate = "";
                if (ctrlMonth != 0)
                {
                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                }
                else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "")
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    }
                }
                else if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    startDate = txtFromDate.Text;
                    endDate = txtToDate.Text;
                }

                switch (rdOpt.SelectedValue)
                {
                    case "1":
                        pnlLast.Visible = true;
                        gdvTransReportSnd.Visible = false;

                   ///     SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                    case "2":
                        pnlLast.Visible = false;
                        gdvTransReport.Visible = false;

                   ///     SearchReport(rdOpt.SelectedValue, gdvTransReportSnd, int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, startDate, endDate);
                        break;
                }
               // UPanel.Update();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        public DataTable ExportData()
        {
            String startDate = "", endDate = "";
            if (ctrlMonth != 0)
            {
                startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
            }
            else if (ctrlMonth == 0 && txtFromDate.Text == "" && txtToDate.Text == "")
            {
                if (ddlYear.SelectedValue != "0")
                {
                    startDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 1, 1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    endDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                }
            }
            else if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                startDate = txtFromDate.Text;
                endDate = txtToDate.Text;
            }
            if (ddlCashUnit.SelectedItem.Text == "- Select One -") cashUnit = "";
            DataTable dt = new DataTable();
            switch (rdOpt.SelectedValue)
            {
                case "1":
                    dt = transBL.SearchReport(int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), cashUnit, startDate, endDate);
                    break;
                case "2":
                    dt = transBL.SearchReport(int.Parse(ddlAccName.SelectedValue), int.Parse(ddlTransType.SelectedValue), int.Parse(ddlStatus.SelectedValue), cashUnit, startDate, endDate);
                    break;
            }

               DataTable dtClone = dt.Clone();
           

            if (dt.Rows.Count > 0 && this.Page.User.Identity.Name != accAdmin && dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    String[] roles = GlobalUI.GetRoles();

                    accSpefCtrl = Common_Fun.AccSpectCtrl(dr["ACCID"].ToString(), roles[0]);

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
            if (rdOpt.SelectedValue == "1")
                BindOption("1", dtClone, gdvTransReport);
            else
                BindOption("2", dtClone, gdvTransReport);


            return dtClone;
        }

        protected void lnkeditupdate_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gdvTransReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var index = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Edit")
                {
                    Session["AccUpdate"] = ddlAccName.SelectedValue.ToString();
                    Session["TypeUpdate"] = ddlTransType.SelectedValue.ToString();
                    Session["YearUpdate"] = ddlYear.SelectedValue.ToString();
                    Session["StatusUpdate"] = ddlStatus.SelectedValue.ToString();
                    Session["CashUpdate"] = ddlCashUnit.SelectedValue.ToString();
                    Session["DateFUpdate"] = txtFromDate.Text.ToString();
                    Session["DateTUpdate"] = txtToDate.Text.ToString();
                    GridViewRow row = gdvTransReport.Rows[index];
                    HiddenField hdfID = (HiddenField)row.FindControl("hdfID");
                    Response.Redirect("~/Account/TransactionEntry.aspx?ID=" + hdfID.Value, true);
                   
                }
                if (e.CommandName == "Delete_Trans")
                {                  
                    DeleteDataRow(index);
                      
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DeleteDataRow(int index)
        {
            try
            {
                GridViewRow row = gdvTransReport.Rows[index];

                HiddenField hdfID = (HiddenField)row.FindControl("hdfID");
                HiddenField hdfAccID = (HiddenField)row.FindControl("hdfAccID");

                bool isDelOK = transBL.DelTrans(int.Parse(hdfID.Value));

                string newFolder = Server.MapPath(attachFolderPath + "MUssVBwgcG8=" + hdfAccID.Value.ToString() + "\\" + hdfID.Value.ToString() + "\\");

                string delFolder = newFolder + "DEL\\";

                string crrdate = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_sstt", CultureInfo.GetCultureInfo("en-US"));

                DataTable dt = transBL.GetTransAttachs(int.Parse(hdfID.Value));

                if (!Directory.Exists(delFolder))
                {
                    Directory.CreateDirectory(delFolder);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Directory.Exists(newFolder))
                    {
                        string oldFile = newFolder + dt.Rows[i]["FileName"];
                        string newFile = delFolder + crrdate + "_DELETE_" + dt.Rows[i]["FileName"];

                        File.Move(oldFile, newFile);
                        File.Delete(oldFile);
                    }
                }
                transBL.DelTransAttachByTransID(int.Parse(hdfID.Value));

                if (isDelOK)
                {
                    #region del data log

                    delLogStr = "\r\n DeletedDate: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    delLogStr += " LoginUserName: " + this.Page.User.Identity.Name;
                    delLogStr += "\r\n TransID: " + hdfID.Value;
                    delLogStr += "\r\n Date: " + ((Label)row.FindControl("lblEntryDate")).Text;
                    delLogStr += " Acc Name: " + ((Label)row.FindControl("lblAccName")).Text;
                    delLogStr += " Trans Typ: " + ((Label)row.FindControl("lblTransType")).Text;
                    delLogStr += " Particular: " + ((Label)row.FindControl("lblParticular")).Text;
                    delLogStr += " Expense(USD): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblExpenseS")).Text) ? ((Label)row.FindControl("lblExpenseS")).Text : "0");
                    delLogStr += " Expense(Kyat): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblExpenseKs")).Text) ? ((Label)row.FindControl("lblExpenseKs")).Text : "0");
                    delLogStr += " Expense(￥): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblExpenseYen")).Text) ? ((Label)row.FindControl("lblExpenseYen")).Text : "0");
                    delLogStr += " Income(USD): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblIncomeS")).Text) ? ((Label)row.FindControl("lblIncomeS")).Text : "0");
                    delLogStr += " Income(Kyat): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblIncomeKs")).Text) ? ((Label)row.FindControl("lblIncomeKs")).Text : "0");
                    delLogStr += " Income(￥): " + (!string.IsNullOrWhiteSpace(((Label)row.FindControl("lblIncomeYen")).Text) ? ((Label)row.FindControl("lblIncomeYen")).Text : "0");
                    delLogStr += " RemainBal(USD): " + ((Label)row.FindControl("lblRemainUSD")).Text;
                    delLogStr += " RemainBal(Kyat): " + ((Label)row.FindControl("lblRemainKs")).Text;
                    delLogStr += " RemainBal(￥): " + ((Label)row.FindControl("lblRemainYen")).Text;

                    delLogStr += " Remark: " + ((Label)row.FindControl("lblRemarks")).Text;
                    delLogStr += " CreatedBy: " + ((Label)row.FindControl("lblCreateBy")).Text;
                    delLogStr += " UpdatedBy: " + ((Label)row.FindControl("lblUpdateBy")).Text;

                    ConsoleWriteLine_Tofile(delLgPath, delLogStr);

                    #endregion

                    BindData();

                    //Update();

                  GlobalUI.MessageBox("Delete Successful!");

                    //SearchReport(rdOpt.SelectedValue, gdvTransReport, int.Parse(ddlAccName.SelectedValue), 0, int.Parse(ddlStatus.SelectedValue), ddlCashUnit.SelectedItem.Text, txtFromDate.Text, txtToDate.Text);
                }
                else
                {
                    GlobalUI.MessageBox("Delete Unsuccessful!");
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try  
            {
                if (gdvTransReport.Rows.Count > 0 && search_no!=0)
                {
                    
                    //Start PDF File

                    DataTable dtClone = ExportData();

                    //Matrix transf = new Matrix();
                    //transf.RotateAt(90,new System.Drawing.PointF(350f,500f), MatrixOrder.Append);
                    //transf.Rotate(30, MatrixOrder.Append);


                    MemoryStream ms = new MemoryStream();
                  //  Document doc = new Document(iTextSharp.text.PageSize.LETTER.Rotate(),5,5,5,5);
                    Document doc = new Document(PageSize.A4.Rotate(), 5, 5, 5, 5);
                    
                    PdfWriter pw = PdfWriter.GetInstance(doc, ms);
                    
                   
                   


                    Font smallfont = new Font(BaseFont.CreateFont(Server.MapPath("~/fonts/TimesNewRomanPSMT.otf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
                    smallfont.IsBold();
                    smallfont.Size = 11;

                    Font smallfont1 = new Font(BaseFont.CreateFont(Server.MapPath("~/fonts/TimesNewRomanPSMT.otf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
                    smallfont1.Size = 11;
                    smallfont1.IsBold();
                    smallfont1.Color = BaseColor.WHITE;


                    Font smallfontg = new Font(BaseFont.CreateFont(Server.MapPath("~/fonts/TimesNewRomanPSMT.otf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
                    smallfontg.Size = 11;
                    smallfontg.IsBold();
                    smallfontg.Color = BaseColor.GRAY;

                    doc.Open();
                   

                    PdfPTable tableh1;


                    tableh1 = new PdfPTable(13);
                    tableh1.WidthPercentage = 100;
                    //tableh1.DefaultCell.Rotation = 90;

                    Paragraph p1 = new Paragraph("Particular", smallfontg);
                    PdfPCell cell1 = new PdfPCell(p1);
                    cell1.BackgroundColor = BaseColor.GRAY;
                    cell1.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    cell1.BorderColorBottom = BaseColor.GRAY;
                   

                    Paragraph pExpense = new Paragraph("Expense", smallfont1);
                    PdfPCell cell2 = new PdfPCell(pExpense);
                    cell2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    cell2.Colspan = 3;
                    cell2.BackgroundColor = BaseColor.GRAY;
                   

                    Paragraph pIncome = new Paragraph("Income", smallfont1);
                    PdfPCell cell3 = new PdfPCell(pIncome);
                    cell3.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    cell3.Colspan = 3;
                    cell3.BackgroundColor = BaseColor.GRAY;
                   

                    Paragraph pRbalace = new Paragraph("Remaining Balance", smallfont1);
                    PdfPCell cell4 = new PdfPCell(pRbalace);
                    cell4.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    cell4.Colspan = 3;
                    cell4.BackgroundColor = BaseColor.GRAY;
                  


                    tableh1.AddCell(cell1);
                    tableh1.AddCell(cell1);
                    tableh1.AddCell(cell1);
                    tableh1.AddCell(cell2);
                    tableh1.AddCell(cell3);
                    tableh1.AddCell(cell4);
                    tableh1.AddCell(cell1);


                    PdfPTable tableh2;
                    tableh2 = new PdfPTable(13);
                    tableh2.WidthPercentage = 100;


                    Paragraph p211 = new Paragraph("Date", smallfont1);
                    PdfPCell cell211 = new PdfPCell(p211);
                    cell211.BackgroundColor = BaseColor.GRAY;
                  

                    cell211.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    cell211.BorderColorTop = BaseColor.GRAY;
                 

                    Paragraph p212 = new Paragraph("Acc Name", smallfont1);
                    PdfPCell cell212 = new PdfPCell(p212);
                    cell212.BackgroundColor = BaseColor.GRAY;
                   

                    cell212.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    cell212.BorderColorTop = BaseColor.GRAY;
                    

                    Paragraph p21 = new Paragraph("Particular", smallfont1);
                    PdfPCell cell21 = new PdfPCell(p21);
                    cell21.BackgroundColor = BaseColor.GRAY;
                    cell21.BorderColorTop = BaseColor.GRAY;
                    cell21.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                  

                    Paragraph p22 = new Paragraph("USD", smallfont1);
                    PdfPCell cell22 = new PdfPCell(p22);
                    cell22.BackgroundColor = BaseColor.GRAY;
                    cell22.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                   

                    Paragraph p23 = new Paragraph("Kyat", smallfont1);
                    PdfPCell cell23 = new PdfPCell(p23);
                    cell23.BackgroundColor = BaseColor.GRAY;
                    cell23.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                   

                    Paragraph p24 = new Paragraph("¥", smallfont1);
                    PdfPCell cell24 = new PdfPCell(p24);
                    cell24.BackgroundColor = BaseColor.GRAY;
                    cell24.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                   


                    Paragraph p25 = new Paragraph("Remark", smallfont1);
                    PdfPCell cell25 = new PdfPCell(p25);

                    cell25.BackgroundColor = BaseColor.GRAY;
                    cell25.BorderColorTop = BaseColor.GRAY;
                    cell25.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                   



                    tableh2.AddCell(cell211);
                    tableh2.AddCell(cell212);
                    tableh2.AddCell(cell21);
                    tableh2.AddCell(cell22);
                    tableh2.AddCell(cell23);
                    tableh2.AddCell(cell24);
                    tableh2.AddCell(cell22);
                    tableh2.AddCell(cell23);
                    tableh2.AddCell(cell24);
                    tableh2.AddCell(cell22);
                    tableh2.AddCell(cell23);
                    tableh2.AddCell(cell24);
                    tableh2.AddCell(cell25);






                    PdfPTable tablebody;
                    PdfPCell cell;

                    tablebody = new PdfPTable(13);
                    tablebody.WidthPercentage = 100;
                    PdfPTable tablefoot = new PdfPTable(13);
                    tablefoot.WidthPercentage = 100;

                    cell = new PdfPCell();

                    cell.BackgroundColor = new BaseColor(0xC0, 0xC0, 0xC0);
                    int pdfRowIndex = 1;

                    for (int i = 0; i < dtClone.Rows.Count; i++)
                    {


                        tablebody.AddCell(new Phrase(dtClone.Rows[i]["F_Date"].ToString(), smallfont));
                        tablebody.AddCell(new Phrase(dtClone.Rows[i]["AccountName"].ToString(), smallfont));
                        tablebody.AddCell(new Phrase(dtClone.Rows[i]["Particular"].ToString(), smallfont));

                        tablebody.AddCell(new Phrase(dtClone.Rows[i]["ExpenseUSD"].ToString(), smallfont));

                        tablebody.AddCell(new Phrase(dtClone.Rows[i]["ExpenseKs"].ToString(), smallfont));

                        tablebody.AddCell(new Phrase(dtClone.Rows[i]["ExpenseYen"].ToString(), smallfont));



                        tablebody.AddCell(new Phrase(dtClone.Rows[i]["IncomeUSD"].ToString(), smallfont));
                        tablebody.AddCell(new Phrase(dtClone.Rows[i]["IncomeKs"].ToString(), smallfont));
                        tablebody.AddCell(new Phrase(dtClone.Rows[i]["IncomeYen"].ToString(), smallfont));

                       






                        if (dtClone.Rows[i]["Remarks"].ToString() != "YwBsAG8ATwBCAEwA" || dtClone.Rows[i]["Particular"].ToString() != "Closing Balance from:")
                        {
                            
                            tablebody.AddCell(new Phrase(dtClone.Rows[i]["ResultAmtUSD"].ToString(), smallfont));
                            tablebody.AddCell(new Phrase(dtClone.Rows[i]["ResultAmtKs"].ToString(), smallfont));
                            tablebody.AddCell(new Phrase(dtClone.Rows[i]["ResultAmtYen"].ToString(), smallfont));
                        }
                        else
                        {
                            


                            tablebody.AddCell(new Phrase(dtClone.Rows[i]["OpeningBalanceUSD"].ToString(), smallfont));
                            tablebody.AddCell(new Phrase(dtClone.Rows[i]["OpeningBalanceKs"].ToString(), smallfont));
                            tablebody.AddCell(new Phrase(dtClone.Rows[i]["OpeningBalanceYen"].ToString(), smallfont));
                        }

                        if (dtClone.Rows[i]["Remarks"].ToString() != "YwBsAG8ATwBCAEwA")
                        {
                            tablebody.AddCell(new Phrase(dtClone.Rows[i]["Remarks"].ToString(), smallfont));
                        }
                        else
                        {
                            dtClone.Columns["Remarks"].ReadOnly = false;
                            dtClone.Rows[i]["Remarks"] = "";
                            tablebody.AddCell(new Phrase(dtClone.Rows[i]["Remarks"].ToString(), smallfont));

                        }

                          pdfRowIndex++;

                         
                      

                        #region //additional region

                        var j = (dtClone.Rows.Count - 1);
                        if (i == j)
                        {

                            Paragraph ptotal = new Paragraph("Total", smallfont);
                            PdfPCell celltotal = new PdfPCell(ptotal);
                            celltotal.Colspan = 3;
                            celltotal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            tablefoot.AddCell(celltotal);
                            int index = gdvTransReport.FooterRow.Cells[1].Text.IndexOf("<");
                            if (index > 0)
                                tablefoot.AddCell(new Phrase(gdvTransReport.FooterRow.Cells[1].Text.Substring(0, index), smallfont));

                            int index1 = gdvTransReport.FooterRow.Cells[2].Text.IndexOf("<");
                            if (index1 > 0)
                                tablefoot.AddCell(new Phrase(gdvTransReport.FooterRow.Cells[2].Text.Substring(0, index1), smallfont));

                            int index2 = gdvTransReport.FooterRow.Cells[3].Text.IndexOf("<");
                            if (index2 > 0)
                                tablefoot.AddCell(new Phrase(gdvTransReport.FooterRow.Cells[3].Text.Substring(0, index2), smallfont));

                            int index3 = gdvTransReport.FooterRow.Cells[4].Text.IndexOf("<");
                            if (index3 > 0)
                                tablefoot.AddCell(new Phrase(gdvTransReport.FooterRow.Cells[4].Text.Substring(0, index3), smallfont));

                            int index4 = gdvTransReport.FooterRow.Cells[5].Text.IndexOf("<");
                            if (index4 > 0)
                                tablefoot.AddCell(new Phrase(gdvTransReport.FooterRow.Cells[5].Text.Substring(0, index4), smallfont));


                            int index5 = gdvTransReport.FooterRow.Cells[6].Text.IndexOf("<");
                            if (index5 > 0)
                                tablefoot.AddCell(new Phrase(gdvTransReport.FooterRow.Cells[6].Text.Substring(0, index5), smallfont));

                            int index6 = gdvTransReport.FooterRow.Cells[7].Text.IndexOf("<");
                          
                            if (index6 > 0)
                           
                               tablefoot.AddCell(new Phrase(gdvTransReport.FooterRow.Cells[7].Text.Substring(0, index6), smallfont));

                            int index7 = gdvTransReport.FooterRow.Cells[8].Text.IndexOf("<");
                          
                            if (index7 > 0)
                           
                                tablefoot.AddCell(new Phrase(gdvTransReport.FooterRow.Cells[8].Text.Substring(0, index7), smallfont));

                            int index8 = gdvTransReport.FooterRow.Cells[9].Text.IndexOf("<");
                          
                            if (index8 > 0)
                           
                                tablefoot.AddCell(new Phrase(gdvTransReport.FooterRow.Cells[9].Text.Substring(0, index8), smallfont));
                            tablefoot.AddCell("");

                        }
                        #endregion
                    }

                    //pw.DirectContent.Transform(transf);
                    //transf.Invert();
                    doc.Add(tableh1);
                    doc.Add(tableh2);

                    doc.Add(tablebody);
                    doc.Add(tablefoot);


                    doc.Close();
                  
                   

                    pw.Close();
                    string FileName = "Transaction_Report_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt", CultureInfo.GetCultureInfo("en-us")) + ".pdf";
                   
                    
                    Byte[] fbyte = ms.ToArray();

                    Response.Clear();
                    Response.ContentType = "application/pdf";


                    Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.ContentType = "application/pdf";
                    Response.Buffer = true;
                    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                    Response.BinaryWrite(fbyte);
                    Response.End();
                    Response.Close();




                }
                else
                {
                    GlobalUI.MessageBox("There is no transactions");
                }
            }
            catch(Exception ex)
            {
                string exMsg = ex.Message;
                if (!ex.Message.Contains("unable to evaluate"))
                {
                    errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                }
            }

        }
    }
}