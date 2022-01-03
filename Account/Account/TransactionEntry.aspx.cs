using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;
using System.Globalization;
using Account_DL;

namespace Account
{
    public partial class TransactionEntry : System.Web.UI.Page
    {
        Transaction_BL transBL = new Transaction_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();
        Transaction_DL transDL = new Transaction_DL();
        string accSpefCtrl = "0";

        string accAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();
        string attachFolderPath = ConfigurationManager.AppSettings["attachFolderPath"].ToString();
        string accID = ""; string canEdit = "0", canDel = "0";
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
        public string ID
        {
            get
            {
                if (Session["ID"] != null)
                {
                    return Convert.ToString(Session["ID"]);
                }
                else
                {
                    return null;
                }
            }
        }
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
                if (!IsPostBack)
                {
                    AccNameDdlBind();
                    stsNameDdlBind();
                    TransTypeBind();
                    ddlCashUnitBind();

                    Session.Remove("dtFileName");
                    Session.Remove("Report_dtFileName");

                    if (Request.QueryString["ID"] != null)
                    {
                        int accID = int.Parse(Request.QueryString["ID"]);
                        DataTable dtb = new DataTable();
                        dtb = transBL.SelectEditData(accID);
                        if (dtb.Rows.Count > 0)
                        {
                            btnSave.Text = "Update";
                            BindData(dtb);
                            txtDate.Enabled = false;
                           // btnAddAttach.Enabled = false;
                            ddlAccName.Enabled = false;
                            
                        }

                        BindData(dtb);
                        //Update();
                    }

                }

                gdvAttachFiles.DataSource = Session["dtFileName"] as DataTable;
                gdvAttachFiles.DataBind();
            }
            catch (Exception ex)
            {

                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }
        private void BindData(DataTable dt)
        {
            if (ddlAccName.Items.FindByText(dt.Rows[0]["AccountName"].ToString()) != null)
            {
                ddlAccName.ClearSelection();
                ddlAccName.Items.FindByText(dt.Rows[0]["AccountName"].ToString()).Selected = true;
            }
            string sname = dt.Rows[0]["StatusID"].ToString();

            if (ddlStatus.Items.FindByValue(dt.Rows[0]["StatusID"].ToString()) != null)
            {
                ddlStatus.ClearSelection();
                ddlStatus.Items.FindByValue(dt.Rows[0]["StatusID"].ToString()).Selected = true;
            }
            
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["Date"].ToString()))
            {
                
                txtDate.Text = String.Format("{0:dd-MM-yyyy HH:mm:ss}", dt.Rows[0]["Date"]); 
                
            }
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["ExpenseUSD"].ToString()))
            {
                txtAmount.Text = dt.Rows[0]["ExpenseUSD"].ToString();
            }
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["ExpenseKs"].ToString()))
            {
                txtAmount.Text = dt.Rows[0]["ExpenseKs"].ToString();
            }
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["ExpenseYen"].ToString()))
            {
                txtAmount.Text = dt.Rows[0]["ExpenseYen"].ToString();
            }
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["IncomeUSD"].ToString()))
            {
                txtAmount.Text = dt.Rows[0]["IncomeUSD"].ToString();
            }
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["IncomeKs"].ToString()))
            {
                txtAmount.Text = dt.Rows[0]["IncomeKs"].ToString();
            }
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["IncomeYen"].ToString()))
            {
                txtAmount.Text = dt.Rows[0]["IncomeYen"].ToString();
            }
            if (ddlCashUnit.Items.FindByText(dt.Rows[0]["Unit"].ToString()) != null)
            {
                ddlCashUnit.ClearSelection();
                ddlCashUnit.Items.FindByText(dt.Rows[0]["Unit"].ToString()).Selected = true;
            }
            if (ddlTransType.Items.FindByValue(dt.Rows[0]["TransType"].ToString()) != null)
            {
                ddlTransType.ClearSelection();
                ddlTransType.Items.FindByValue(dt.Rows[0]["TransType"].ToString()).Selected = true;
            }
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["Particular"].ToString()))
            {
                txtParticular.Text = dt.Rows[0]["Particular"].ToString();
            }
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["Remarks"].ToString()))
            {
                txtRemark.Text = dt.Rows[0]["Remarks"].ToString();
            }
            if (!String.IsNullOrWhiteSpace(dt.Rows[0]["FileName"].ToString()))
            {

                string filePath = attachFolderPath + "MUssVBwgcG8=" + dt.Rows[0]["ACCID"].ToString() + "\\" + dt.Rows[0]["TransID"].ToString() + "\\";
                DataColumn dc1 = new DataColumn("FolderPath", typeof(string));
                dt.Columns.Add(dc1);

                DataColumn dc2 = new DataColumn("FilePath", typeof(string));
                dt.Columns.Add(dc2);
                dt.Rows[0]["FolderPath"] = filePath;
                dt.Rows[0]["FilePath"] = filePath + dt.Rows[0]["FileName"].ToString();
                
                gdvAttachFiles.DataSource = dt;
                gdvAttachFiles.DataBind();
                Session["dtFileName"] = dt;
               
            }
        }
        public DataTable SelectEditData(int accID)
        {
            return transDL.SelectEditData(accID);
           
        }

        private void ddlCashUnitBind()
        {
            try
            {
                ddlCashUnit.DataSource = transBL.ddlCashUnitBind();
                ddlCashUnit.DataTextField = "CashUnit";
                ddlCashUnit.DataValueField = "ID";
                ddlCashUnit.DataBind();
                ddlCashUnit.ClearSelection();
                ddlCashUnit.Items.Insert(0, "--Select--");
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
                ddlTransType.DataSource = transBL.TransTypeBind();
                ddlTransType.DataTextField = "Type";
                ddlTransType.DataValueField = "Id";
                ddlTransType.DataBind();
                ddlTransType.ClearSelection();
                ddlTransType.Items.Insert(0, "--Select--");
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void stsNameDdlBind()
        {
            try
            {
                ddlStatus.DataSource = transBL.stsNameDdlBind();
                ddlStatus.DataTextField = "Status";
                ddlStatus.DataValueField = "stsID";
                ddlStatus.DataBind();
                ddlStatus.ClearSelection();
                ddlStatus.Items.Insert(0, "--Select--");
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void AccNameDdlBind()
        {
            try
            {
                DataTable dt = transBL.AccNameDdlBind();

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
                ddlAccName.ClearSelection();
                ddlAccName.Items.Insert(0, "--Select--");
                Session["AttachAccID"] = ddlAccName.SelectedValue;
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
                #region update
                if (btnSave.Text == "Update")
                {
                    try
                {
                        string updatedUser = this.Page.User.Identity.Name;
                        int accID = int.Parse(Request.QueryString["ID"]);
                        transBL.UpdateTran(int.Parse(accID.ToString()), int.Parse(ddlAccName.SelectedItem.Value), int.Parse(ddlTransType.SelectedItem.Value), txtParticular.Text, txtRemark.Text, int.Parse(ddlStatus.SelectedItem.Value), txtAmount.Text.Replace(",", ""), ddlCashUnit.SelectedItem.Text, updatedUser); 
                        //TransRpt_Update();

                        GlobalUI.MessageBox("Update Successful");
                        Response.Redirect("~/Account/Transaction_Report.aspx?ID="+ accID  , true); 
                        Clear();

                }
                catch (Exception ex)
                {
                    GlobalUI.MessageBox("Update Unsuccessful");
                    errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                }
            }
               
                #endregion
                #region save
                else
                {
                    string createdUser = this.Page.User.Identity.Name;
                    int transID = SaveTransaction(int.Parse(ddlAccName.SelectedItem.Value), int.Parse(ddlStatus.SelectedValue), txtAmount.Text.Replace(",", ""), ddlCashUnit.SelectedItem.Text, int.Parse(ddlTransType.SelectedValue), txtRemark.Text, txtParticular.Text, txtDate.Text, createdUser);   //save new accs
                    if (transID != 0)
                    {
                        string AttachAccID = Session["AttachAccID"] == null ? "" : Session["AttachAccID"] as string;
                        string sessionID = Session.SessionID;

                        if (ddlAccName.SelectedValue == AttachAccID)
                        {
                            if (Session["dtFileName"] != null)
                            {
                                DataTable dt = Session["dtFileName"] as DataTable;

                                if (dt.Rows.Count > 0)
                                {
                                    string newFolder = Server.MapPath(attachFolderPath + "MUssVBwgcG8=" + ddlAccName.SelectedValue + "\\" + transID.ToString() + "\\");
                                    string oldFolder = Server.MapPath(attachFolderPath + "MUssVBwgcG8=" + ddlAccName.SelectedValue + "\\" + sessionID + "\\");

                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (!String.IsNullOrWhiteSpace(dt.Rows[i]["FileName"].ToString()))
                                        {
                                            string fileName = dt.Rows[i]["FileName"] as string;

                                            SaveTransAttachment(transID, fileName);

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
                                            }
                                        }
                                    }

                                    if (Directory.Exists(oldFolder))
                                    {
                                        Directory.Delete(oldFolder, true);
                                    }

                                    Session.Clear();
                                }
                            }
                        }

                        String url = Request.ApplicationPath;

                        string accID = GlobalUI.EncryptQueryString(ddlAccName.SelectedValue);

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Save Successful'); window.location='Account/Transaction_Report.aspx?sav="+accID+"';", true);

                        //TransRpt_Update();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Save Successful');", true);
                        Clear();
                    }
                    else
                    {
                        GlobalUI.MessageBox("Save Unsuccessful!");
                        Clear();
                    }
                }
                

                #endregion
                
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void TransRpt_Update()
        {
            try
            {
                Transaction_Report_BL transBL = new Transaction_Report_BL();
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
                        decimal expUSD, expKs, expYen;

                        incUSD = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["IncomeUSD"].ToString()) ? "0" : dt.Rows[i]["IncomeUSD"].ToString());
                        expUSD = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["ExpenseUSD"].ToString()) ? "0" : dt.Rows[i]["ExpenseUSD"].ToString());

                        incKs = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["IncomeKs"].ToString()) ? "0" : dt.Rows[i]["IncomeKs"].ToString());
                        expKs = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["ExpenseKs"].ToString()) ? "0" : dt.Rows[i]["ExpenseKs"].ToString());

                        incYen = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["IncomeYen"].ToString()) ? "0" : dt.Rows[i]["IncomeYen"].ToString());
                        expYen = Convert.ToDecimal(String.IsNullOrWhiteSpace(dt.Rows[i]["ExpenseYen"].ToString()) ? "0" : dt.Rows[i]["ExpenseYen"].ToString());

                        dt.Rows[i]["OpeningBalanceUSD"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtUSD"].ToString()) + incUSD - expUSD;
                        dt.Rows[i]["OpeningBalanceKs"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtKs"].ToString()) + incKs - expKs;
                        dt.Rows[i]["OpeningBalanceYen"] = Convert.ToDecimal(dt2.Rows[0]["ResultAmtYen"].ToString()) + incYen - expYen;

                        //transBL.updateAllTranabc(int.Parse(dt.Rows[i]["TransID"].ToString()), dt.Rows[i]["OpeningBalanceUSD"].ToString(), dt.Rows[i]["OpeningBalanceKs"].ToString(), dt.Rows[i]["OpeningBalanceYen"].ToString());
                        
                    }                
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void SaveTransAttachment(int transID, string fileName)
        {
            transBL.SaveTransAttachment(transID, fileName);
        }

        private void Clear()
        {
            ddlTransType.ClearSelection();
            ddlCashUnit.ClearSelection();
            ddlStatus.ClearSelection();
            ddlAccName.ClearSelection();

            ddlCashUnit.Items.Insert(0, "--Select--");
            ddlStatus.Items.Insert(0, "--Select--");
            ddlTransType.Items.Insert(0, "--Select--");
            ddlAccName.Items.Insert(0, "--Select--");
            txtDate.Text = "";
            txtAmount.Text = "";
            txtRemark.Text = "";
            txtParticular.Text = "";
            gdvAttachFiles.DataSource = null;
            gdvAttachFiles.DataBind();
        }

        private int SaveTransaction(int accID, int stsID, string amount, string cashUnit, int transTypeID, string remark, string particular, string date, string createdUser)
        {
            return transBL.SaveTransaction(accID, stsID, amount, cashUnit, transTypeID, remark, particular, date, createdUser);
        }

        protected void ddlAccName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //for rebinding datatable to session dtfilename 
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("FileName", typeof(string));
            dt.Columns.Add(dc);

            DataColumn dc1 = new DataColumn("FolderPath", typeof(string));
            dt.Columns.Add(dc1);

            DataColumn dc2 = new DataColumn("FilePath", typeof(string));
            dt.Columns.Add(dc2);

            DataColumn dc3 = new DataColumn("ID", typeof(string));
            dt.Columns.Add(dc3);

            string ctrlAccID = Session["AttachAccID"] == null ? "" : Session["AttachAccID"] as string;

            //when changing ddlaccname delete selected old acc's uploaded files 
            if (ctrlAccID != ddlAccName.SelectedValue)
            {
                string sessionID = Session.SessionID;

                string oldFolder = attachFolderPath + "MUssVBwgcG8=" + ctrlAccID + "\\" + sessionID + "\\";
                string newFolder = attachFolderPath + "MUssVBwgcG8=" + ddlAccName.SelectedValue + "\\" + sessionID + "\\";

                string oldAccfolderPath = Server.MapPath(oldFolder);
                string newAccPath = Server.MapPath(newFolder);

                if (!Directory.Exists(newAccPath))
                {
                    Directory.CreateDirectory(newAccPath);
                }

                //move files from sessionid folder to new saved transid folder
                if (Directory.Exists(oldAccfolderPath))
                {
                    foreach (string file in Directory.GetFiles(oldAccfolderPath))
                    {
                        string newFile = newAccPath + Path.GetFileName(file);
                        string oldFile = oldAccfolderPath + Path.GetFileName(file);

                        DataRow dr = dt.NewRow();
                        dr["FileName"] = Path.GetFileName(file);
                        dr["FolderPath"] = newFolder;
                        dr["FilePath"] = newFolder + Path.GetFileName(file);
                        dt.Rows.Add(dr);

                        File.Move(oldFile, newFile);
                        File.Delete(oldFile);
                    }
                }

                //Check whether Directory (Folder) exists.
                if (Directory.Exists(oldAccfolderPath))
                {
                    Directory.Delete(oldAccfolderPath);
                }

                //Session.Remove("dtFileName");
            }
            Session["AttachAccID"] = ddlAccName.SelectedValue;

            Session["dtFileName"] = dt;
            gdvAttachFiles.DataSource = Session["dtFileName"] as DataTable;
            gdvAttachFiles.DataBind();

            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.opener.__doPostBack();window.close()", true);
        }

        protected void gdvSavedFile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            gdvAttachFiles.DataSource = Session["dtFileName"] as DataTable;
            gdvAttachFiles.DataBind();

            DataTable dt = Session["dtFileName"] as DataTable;

            int i = Convert.ToInt32(e.CommandArgument);

            int count = gdvAttachFiles.Rows.Count;

            GridViewRow row = gdvAttachFiles.Rows[i];

            foreach (HyperLink l in row.Cells[0].Controls.OfType<HyperLink>())
            {
                string fileName = l.Text;
                string filePath = l.NavigateUrl;
                string folderPath = filePath.Replace(fileName, "");
                folderPath = Server.MapPath(folderPath);

                if (Directory.Exists(folderPath))
                {
                    File.Delete(Server.MapPath(filePath));
                }

                if (dt.Rows[i]["FileName"].ToString() == fileName)
                {
                    dt.Rows[i].Delete();
                    dt.AcceptChanges();
                }

                if (Directory.Exists(folderPath))
                {
                    string[] files = Directory.GetFiles(folderPath);

                    if (files.Length == 0)
                    {
                        Directory.Delete(folderPath);
                    }
                }
            }

            gdvAttachFiles.DataSource = dt;
            gdvAttachFiles.DataBind();

            Session["dtFileName"] = dt;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnAddAttach_Click(object sender, EventArgs e)
        {

            if (Request.QueryString["ID"] != null)
            {
                try
                {
                   
                    int transID = int.Parse(Request.QueryString["ID"]);
                    DataTable dtb = new DataTable();
                    dtb = transBL.SelectEditData(transID);
                    string AccID = "";
                    if (!String.IsNullOrWhiteSpace(dtb.Rows[0]["ACCID"].ToString()))
                    {
                        AccID = dtb.Rows[0]["ACCID"].ToString();
                    }

                    string filePath = attachFolderPath + "MUssVBwgcG8=" + AccID + "\\" + transID.ToString() + "\\";

                    BindModalGridView(AccID, transID, filePath);
                    //   BindData(dtb);
                    GetLinkButton(AccID);
                    ClientScript.RegisterStartupScript(this.GetType(), "popup_window", "<script>ShowAtta_PopUp('this')</script>");
                }
                catch (Exception ex)
                {
                    errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                }
            }
            else
            {
                Session.Remove("Report_dtFileName");
                ClientScript.RegisterStartupScript(this.GetType(), "popup_window", "<script>ShowAtta_PopUp('this')</script>");
            }
        }
        protected void GetLinkButton(string AccID)
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
                // int trID = int.Parse(Request.QueryString["ID"]);
                DataTable dtl = transBL.GetTransAttachs(transID);
                DataColumn dc1 = new DataColumn("FolderPath", typeof(string));
                dtl.Columns.Add(dc1);

                DataColumn dc2 = new DataColumn("FilePath", typeof(string));
                dtl.Columns.Add(dc2);

                if (!dtl.Columns.Contains("TransID"))
                {
                    DataColumn dc3 = new DataColumn("TransID", typeof(string));
                    dtl.Columns.Add(dc3);
                }

                DataColumn dc4 = new DataColumn("AccID", typeof(string));
                dtl.Columns.Add(dc4);

                DataRow dr = dtl.NewRow();
                dr["TransID"] = transID;
                dtl.Rows.InsertAt(dr, 0);

                dtl.Rows[0]["AccID"] = AccID;

                if (dtl.Rows.Count > 1)
                {
                    for (int i = 1; i < dtl.Rows.Count; i++)
                    {
                        if (!String.IsNullOrWhiteSpace(dtl.Rows[i]["FileName"].ToString()))
                        {
                            dtl.Rows[i]["FolderPath"] = filePath;
                            dtl.Rows[i]["FilePath"] = filePath + dtl.Rows[i]["FileName"].ToString();
                        }
                    }
                }

                Session["dtFileName"] = dtl;
                //  UPanel.Update();
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

    }
}