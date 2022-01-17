using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;

namespace Account.Admin
{
    public partial class Attachment_PopUp : System.Web.UI.Page
    {
        string attachFolderPath = ConfigurationManager.AppSettings["attachFolderPath"].ToString();
        Transaction_Report_BL transBL = new Transaction_Report_BL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Form"] != null)
                {
                    gdvSavedFile.Enabled = false;
                    panel2.Enabled = false;
                    attFile1.Enabled = false;
                    attFile2.Enabled = false;
                    attFile3.Enabled = false;
                    attFile4.Enabled = false;
                    attFile5.Enabled = false;
                    btnSaveFiles.Enabled = false;
                }
                else
                {
                    gdvSavedFile.Enabled = true;
                    panel2.Enabled = true;
                    attFile1.Enabled = true;
                    attFile2.Enabled = true;
                    attFile3.Enabled = true;
                    attFile4.Enabled = true;
                    attFile5.Enabled = true;
                    btnSaveFiles.Enabled = true;
                }

                if (Session["dtFileName"] != null)
                {
                    panel3.Visible = true;
                    gdvSavedFile.DataSource = Session["dtFileName"] as DataTable;
                    gdvSavedFile.DataBind();
                }
                if (Session["Report_dtFileName"] != null)
                {
                    panel3.Visible = true;

                    DataTable dt = Session["Report_dtFileName"] as DataTable;

                    if (dt.Rows.Count > 1)
                    {
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = "NOT(FileName =' ')";

                        gdvSavedFile.DataSource = dv;
                        gdvSavedFile.DataBind();
                    }
                }

            }
        }

        //first save files under folder name with session id
        //second after transaction save success move files under session id folder to new saved trans id folder       
        protected void btnSaveFiles_Click(object sender, EventArgs e)
        {
            if (Session["Report_dtFileName"] != null)
            {
                DataTable dt2 = Session["Report_dtFileName"] as DataTable;

                int transID = Convert.ToInt32(dt2.Rows[0]["TransID"].ToString());
                string AccID = dt2.Rows[0]["AccID"].ToString();

                //for creating folder with trans id
                string folderPath = Server.MapPath(attachFolderPath);
                folderPath += "MUssVBwgcG8=" + AccID + "\\" + transID.ToString() + "\\";

                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists. Create it.
                    Directory.CreateDirectory(folderPath);
                }                              

                //Save the File to the Directory (Folder).
                if (attFile1.HasFile)
                {

                    DataRow dr = dt2.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile1.FileName);
                    dt2.Rows.Add(dr);
                   
                        attFile1.SaveAs(folderPath + Path.GetFileName(attFile1.FileName));
                   
                }
                if (attFile2.HasFile)
                {
                    DataRow dr = dt2.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile2.FileName);
                    dt2.Rows.Add(dr);

                    attFile2.SaveAs(folderPath + Path.GetFileName(attFile2.FileName));
                }
                if (attFile3.HasFile)
                {
                    DataRow dr = dt2.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile3.FileName);
                    dt2.Rows.Add(dr);

                    attFile3.SaveAs(folderPath + Path.GetFileName(attFile3.FileName));
                }
                if (attFile4.HasFile)
                {
                    DataRow dr = dt2.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile4.FileName);
                    dt2.Rows.Add(dr);

                    attFile4.SaveAs(folderPath + Path.GetFileName(attFile4.FileName));
                }
                if (attFile5.HasFile)
                {
                    DataRow dr = dt2.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile5.FileName);
                    dt2.Rows.Add(dr);

                    attFile5.SaveAs(folderPath + Path.GetFileName(attFile5.FileName));
                }

                for (int i = 1; i < dt2.Rows.Count; i++)
                {
                    if(Session["Form"] != null)
                    {
                        if (!String.IsNullOrEmpty(dt2.Rows[i]["fileName"].ToString()))
                        {
                            transBL.SaveTransAttachment(transID, dt2.Rows[i]["fileName"].ToString());
                        }
                    }
                }

                panel3.Visible = true;

                DataView dv = dt2.DefaultView;
                dv.RowFilter = "NOT(FileName =' ')";

                gdvSavedFile.DataSource = dv;
                gdvSavedFile.DataBind();
                Session["Report_dtFileName"] = dt2;
                //Session.Remove("Report_dtFileName");
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.opener.__doPostBack('lnkTransAttach','');window.close()", true);
            }
            
            else
            {
                Session["ctrlAccID"] = Session["AttachAccID"];
                string acc = Session["ctrlAccID"] as string;

                ////for controlling same user acc usage from different browsers
                string sessionID = Session.SessionID;

                //for creating folder with session id
                string path = attachFolderPath + "MUssVBwgcG8=" + acc + "\\" + sessionID + "\\";
                string folderPath = Server.MapPath(attachFolderPath + "MUssVBwgcG8=" + acc + "\\" + sessionID + "\\");

                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists. Create it.
                    Directory.CreateDirectory(folderPath);
                }

                DataTable dt = Session["dtFileName"] == null ? new DataTable() : Session["dtFileName"] as DataTable;

                if (Session["dtFileName"] == null)
                {
                    DataColumn dc = new DataColumn("FileName", typeof(string));
                    dt.Columns.Add(dc);

                    DataColumn dc1 = new DataColumn("FolderPath", typeof(string));
                    dt.Columns.Add(dc1);

                    DataColumn dc2 = new DataColumn("FilePath", typeof(string));
                    dt.Columns.Add(dc2);

                    DataColumn dc3 = new DataColumn("ID", typeof(string));
                    dt.Columns.Add(dc3);
                }

                //Save the File to the Directory (Folder).
                if (attFile1.HasFile)
                {
                    DataRow dr = dt.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile1.FileName);
                    dr["FolderPath"] = path;
                    dr["FilePath"] = path + Path.GetFileName(attFile1.FileName);
                    dt.Rows.Add(dr);

                    attFile1.SaveAs(folderPath + Path.GetFileName(attFile1.FileName));
                }
                if (attFile2.HasFile)
                {
                    DataRow dr = dt.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile2.FileName);
                    dr["FolderPath"] = path;
                    dr["FilePath"] = path + Path.GetFileName(attFile2.FileName);
                    dt.Rows.Add(dr);

                    attFile2.SaveAs(folderPath + Path.GetFileName(attFile2.FileName));
                }
                if (attFile3.HasFile)
                {
                    DataRow dr = dt.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile3.FileName);
                    dr["FolderPath"] = path;
                    dr["FilePath"] = path + Path.GetFileName(attFile3.FileName);
                    dt.Rows.Add(dr);

                    attFile3.SaveAs(folderPath + Path.GetFileName(attFile3.FileName));
                }
                if (attFile4.HasFile)
                {
                    DataRow dr = dt.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile4.FileName);
                    dr["FolderPath"] = path;
                    dr["FilePath"] = path + Path.GetFileName(attFile4.FileName);
                    dt.Rows.Add(dr);

                    attFile4.SaveAs(folderPath + Path.GetFileName(attFile4.FileName));
                }
                if (attFile5.HasFile)
                {
                    DataRow dr = dt.NewRow();
                    dr["FileName"] = Path.GetFileName(attFile5.FileName);
                    dr["FolderPath"] = path;
                    dr["FilePath"] = path + Path.GetFileName(attFile5.FileName);
                    dt.Rows.Add(dr);

                    attFile5.SaveAs(folderPath + Path.GetFileName(attFile5.FileName));
                }
                Session["dtFileName"] = dt;

                panel3.Visible = true;

                gdvSavedFile.DataSource = dt;
                gdvSavedFile.DataBind();

                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.opener.__doPostBack('lnkTransAttach','');window.close()", true);
            }
        }

        protected void btnCancelModal_Click(object sender, EventArgs e)
        {
            if (Session["Form"] != null)
            {
                Session.Remove("Report_dtFileName");
            }
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.opener.__doPostBack('lnkTransAttach','');window.close()", true);
        }

        protected void gdvSavedFile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Session["Report_dtFileName"] != null)
            {
                DataTable dtDelete = new DataTable();
                DataTable dt = Session["Report_dtFileName"] as DataTable;

                DataView dv = dt.DefaultView;
                dv.RowFilter = "NOT(FileName =' ')";

                gdvSavedFile.DataSource = dv;
                gdvSavedFile.DataBind();

                int i = Convert.ToInt32(e.CommandArgument);
                int transID = Convert.ToInt32(dt.Rows[0]["TransID"].ToString());
                int AccID = Convert.ToInt32(dt.Rows[0]["AccID"].ToString());

                int count = gdvSavedFile.Rows.Count;

                GridViewRow row = gdvSavedFile.Rows[i];

                int attachID = 0;
                string folderPath = "";
                foreach (HyperLink l in row.Cells[1].Controls.OfType<HyperLink>())
                {
                    attachID = Convert.ToInt32(l.Text);
                }

                foreach (HyperLink l in row.Cells[0].Controls.OfType<HyperLink>())
                {
                    string fileName = l.Text;
                    if (Session["Form"] != null)
                    {
                        string crrdate = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_sstt", CultureInfo.GetCultureInfo("en-US"));

                        string delFileName = crrdate + "_DELETE_" + l.Text;
                        string filePath = l.NavigateUrl;
                        folderPath = filePath.Replace(fileName, "");

                        string delFolder = folderPath + "\\DEL\\";
                        delFolder = Server.MapPath(delFolder);

                        folderPath = Server.MapPath(folderPath);

                        //if (Directory.Exists(folderPath)) //physical delete
                        //{
                        //    File.Delete(Server.MapPath(filePath));
                        //}

                        if (!Directory.Exists(delFolder))
                        {
                            Directory.CreateDirectory(delFolder);
                        }

                        if (Directory.Exists(folderPath))
                        {
                            string oldFile = folderPath + fileName;
                            string newFile = delFolder + delFileName;


                            File.Move(oldFile, newFile);
                            File.Delete(oldFile);
                        }
                    }
                    else
                    {

                        if (Session["Delete_dtFileName"] != null)
                        {
                            dtDelete = Session["Delete_dtFileName"] as DataTable;
                        }
                        if (!dtDelete.Columns.Contains("FileName"))
                        {
                            DataColumn dc = new DataColumn("FileName", typeof(System.String));
                            dtDelete.Columns.Add(dc);
                        }
                        if (!dtDelete.Columns.Contains("FolderPath"))
                        {
                            DataColumn dc1 = new DataColumn("FolderPath", typeof(string));
                            dtDelete.Columns.Add(dc1);
                        }
                        if (!dtDelete.Columns.Contains("FilePath"))
                        {
                            DataColumn dc2 = new DataColumn("FilePath", typeof(string));
                            dtDelete.Columns.Add(dc2);
                        }
                        if (!dtDelete.Columns.Contains("AttachID"))
                        {
                            DataColumn dc1 = new DataColumn("AttachID", typeof(System.String));
                            dtDelete.Columns.Add(dc1);
                        }


                    }
                    for (int j = i; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["FileName"].ToString() == fileName)
                        {
                            if (Session["Form"] != null)
                            {
                                transBL.DeleteTransAttachment(attachID);
                            }
                            else
                            {
                                DataRow dr = dtDelete.NewRow();
                                dr["FileName"] = dt.Rows[j]["FileName"].ToString();
                                dr["AttachID"] = dt.Rows[j]["ID"].ToString();
                                dr["FilePath"] = dt.Rows[j]["FilePath"].ToString();
                                dr["FolderPath"] = dt.Rows[j]["FolderPath"].ToString();
                                dtDelete.Rows.Add(dr);
                                Session["Delete_dtFileName"] = dtDelete;
                            }
                            dt.Rows[j].Delete();
                            dt.AcceptChanges();
                        }
                    }

                    //if (Directory.Exists(folderPath))
                    //{
                    //    string[] files = Directory.GetFiles(folderPath);

                    //    if (files.Length == 0)
                    //    {
                    //        Directory.Delete(folderPath);
                    //    }
                    //}
                }

                dv = dt.DefaultView;
                dv.RowFilter = "NOT(FileName =' ')";

                gdvSavedFile.DataSource = dv;
                gdvSavedFile.DataBind();

                Session["Report_dtFileName"] = dt;

            }
            else
            {
                gdvSavedFile.DataSource = Session["dtFileName"] as DataTable;
                gdvSavedFile.DataBind();

                DataTable dt = Session["dtFileName"] as DataTable;

                int i = Convert.ToInt32(e.CommandArgument);

                int count = gdvSavedFile.Rows.Count;

                GridViewRow row = gdvSavedFile.Rows[i];

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

                gdvSavedFile.DataSource = dt;
                gdvSavedFile.DataBind();

                Session["dtFileName"] = dt;
            }
        }
    }
}