using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Account_BL;
using System.Configuration;
using System.Globalization;

namespace Account
{
    public partial class HomePage : System.Web.UI.Page
    {
        #region declare
        HomePage_BL homeBL = new HomePage_BL();
        ErrorLog_BL errBL = new ErrorLog_BL();

        string accSpefCtrl = "0";

        string accAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();
        string minYear = ConfigurationManager.AppSettings["minYear"].ToString();

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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                String lstDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                if (!String.IsNullOrWhiteSpace(lstDate))
                {
                    //date = Convert.ToDateTime(lstDate);
                    //String endDate = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

                    AccsGridBind(lstDate);
                    IsTotalAccBind(lstDate);
                }

                rptMonthsDataBind();
                ddlYearBind();
            }
            else
            {
                String lstDate = "";
                if (ctrlMonth != 0)
                {
                    if (ddlYear.SelectedValue == "0")
                    {
                        ddlYear.SelectedIndex = 1;
                    }
                    lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    AccsGridBind(lstDate);
                    IsTotalAccBind(lstDate);
                }

                if (ctrlMonth == 0)
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        AccsGridBind(lstDate);
                        IsTotalAccBind(lstDate);
                    }
                }

                if (ctrlMonth == 0 && ddlYear.SelectedIndex == 0)
                {
                    lstDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    AccsGridBind(lstDate);
                    IsTotalAccBind(lstDate);
                }
            }
        }

        protected void gdvAccs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Control ctrl = e.CommandSource as Control;
            GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;


            switch (e.CommandName)
            {
                case "Cmd_Detail":

                    Label lblid = row.FindControl("lblID") as Label;

                    string data = GlobalUI.EncryptQueryString(string.Format("AccID={0}", lblid.Text));
                    Response.RedirectToRoute("TransReport", new { AccID = data });

                    break;
            }
        }

        private void AccsGridBind(String date)
        {
            try
            {
                DataTable dt = homeBL.AccsGridBind(date);

                if (dt.Rows.Count > 0 && this.Page.User.Identity.Name != accAdmin && dt != null)
                {
                    DataTable dtClone = dt.Clone();

                    foreach (DataRow dr in dt.Rows)
                    {
                        String[] roles = GlobalUI.GetRoles();

                        accSpefCtrl = Common_Fun.AccSpectCtrl(dr["ID"].ToString(), roles[0]);

                        if (accSpefCtrl != "0")
                        {
                            dtClone.Rows.Add(dr.ItemArray);
                        }
                    }

                    gdvAccs.DataSource = dtClone;
                    gdvAccs.DataBind();
                }
                else
                {
                    gdvAccs.DataSource = dt;
                    gdvAccs.DataBind();
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        private void IsTotalAccBind(String date)
        {
            try
            {
                Decimal totalOBLUSD = 0, totalOBLKs = 0, totalOBLYen = 0;

                DataTable dt = homeBL.IsTotalAccBind(date);

                if (dt.Rows.Count > 0 && this.Page.User.Identity.Name != accAdmin && dt != null)
                {
                    DataTable dtClone = dt.Clone();

                    foreach (DataRow dr in dt.Rows)
                    {
                        String[] roles = GlobalUI.GetRoles();

                        accSpefCtrl = Common_Fun.AccSpectCtrl(dr["ID"].ToString(), roles[0]);

                        if (accSpefCtrl != "0")
                        {
                            dtClone.Rows.Add(dr.ItemArray);
                        }
                    }

                    if (dtClone.Rows.Count > 0 && dtClone != null)
                    {
                        for (int i = 0; i < dtClone.Rows.Count; i++)
                        {
                            totalOBLUSD += Convert.ToDecimal(dtClone.Rows[i]["ProcessAmtUSD"].ToString());
                            totalOBLKs += Convert.ToDecimal(dtClone.Rows[i]["ProcessAmtKs"].ToString());
                            totalOBLYen += Convert.ToDecimal(dtClone.Rows[i]["ProcessAmtYen"].ToString());
                        }

                        gdvTotalOBL.DataSource = dtClone;
                        gdvTotalOBL.DataBind();

                        Label lblTotalUSD = gdvTotalOBL.FooterRow.Cells[1].FindControl("lblTotalUSD") as Label;
                        Label lblTotalKyat = gdvTotalOBL.FooterRow.Cells[1].FindControl("lblTotalKyat") as Label;
                        Label lblTotalYen1 = gdvTotalOBL.FooterRow.Cells[1].FindControl("lblTotalYen1") as Label;
                        Label lblTotalYen = gdvTotalOBL.FooterRow.Cells[1].FindControl("lblTotalYen") as Label;

                        lblTotalUSD.Text = totalOBLUSD.ToString("N2");
                        lblTotalKyat.Text = totalOBLKs.ToString("N2");
                        lblTotalYen1.Text = totalOBLYen.ToString("N2");
                        lblTotalYen.Text = totalOBLYen.ToString("N2");


                        if (!String.IsNullOrWhiteSpace(txtUSDtoKyat.Text) && !String.IsNullOrWhiteSpace(txtUSDtoYen.Text))
                        {
                            Decimal USDtoKyat = Convert.ToDecimal(txtUSDtoKyat.Text);
                            Decimal USDtoYen = Convert.ToDecimal(txtUSDtoYen.Text);

                            if (USDtoKyat != 0 && USDtoYen != 0)
                            {
                                Decimal Yen = ((totalOBLKs / USDtoKyat) + totalOBLUSD) * USDtoYen;

                                lblTotalYen.Text = Yen.ToString("N2");
                            }
                            else
                            {
                                lblTotalYen.Text = "0.00";
                            }

                        }
                        else
                        {
                            lblTotalYen.Text = "0.00";
                        }
                    }
                    else
                    {
                        gdvTotalOBL.DataSource = dtClone;
                        gdvTotalOBL.DataBind();
                    }
                }
                else
                {
                    if (dt.Rows.Count > 0 && dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            totalOBLUSD += Convert.ToDecimal(dt.Rows[i]["ProcessAmtUSD"].ToString());
                            totalOBLKs += Convert.ToDecimal(dt.Rows[i]["ProcessAmtKs"].ToString());
                            totalOBLYen += Convert.ToDecimal(dt.Rows[i]["ProcessAmtYen"].ToString());
                        }

                        gdvTotalOBL.DataSource = dt;
                        gdvTotalOBL.DataBind();

                        Label lblTotalUSD = gdvTotalOBL.FooterRow.Cells[1].FindControl("lblTotalUSD") as Label;
                        Label lblTotalKyat = gdvTotalOBL.FooterRow.Cells[1].FindControl("lblTotalKyat") as Label;
                        Label lblTotalYen1 = gdvTotalOBL.FooterRow.Cells[1].FindControl("lblTotalYen1") as Label;

                        Label lblTotalYen = gdvTotalOBL.FooterRow.Cells[1].FindControl("lblTotalYen") as Label;

                        lblTotalUSD.Text = totalOBLUSD.ToString("N2");
                        lblTotalKyat.Text = totalOBLKs.ToString("N2");
                        lblTotalYen1.Text = totalOBLYen.ToString("N2");

                        lblTotalYen.Text = totalOBLYen.ToString("N2");

                        if (!String.IsNullOrWhiteSpace(txtUSDtoKyat.Text) && !String.IsNullOrWhiteSpace(txtUSDtoYen.Text))
                        {
                            Decimal USDtoKyat = Convert.ToDecimal(txtUSDtoKyat.Text);
                            Decimal USDtoYen = Convert.ToDecimal(txtUSDtoYen.Text);

                            if (USDtoKyat != 0 && USDtoYen != 0)
                            {
                                Decimal Yen = ((totalOBLKs / USDtoKyat) + totalOBLUSD) * USDtoYen;

                                lblTotalYen.Text = Yen.ToString("N2");
                            }
                            else
                            {
                                lblTotalYen.Text = "0.00";
                            }
                        }
                        else
                        {
                            lblTotalYen.Text = "0.00";
                        }
                    }
                    else
                    {
                        gdvTotalOBL.DataSource = dt;
                        gdvTotalOBL.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
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

        protected void lnkBtnMonth_Click(object sender, EventArgs e)
        {
            try
            {
                String lstDate;
                LinkButton btn = (LinkButton)(sender);

                int month = int.Parse(btn.CommandArgument.ToString());

                if (ctrlMonth == month)
                {
                    btn.CssClass = "page_disabled";
                    ctrlMonth = 0;
                }
                else
                {
                    if (ddlYear.SelectedValue == "0")
                    {
                        ddlYear.SelectedIndex = 1;
                    }
                    lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), month, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    AccsGridBind(lstDate);
                    IsTotalAccBind(lstDate);
                    ctrlMonth = month;
                }

                if (ctrlMonth == 0)
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        AccsGridBind(lstDate);
                        IsTotalAccBind(lstDate);
                    }
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
                String lstDate = "";
                if (ctrlMonth != 0)
                {
                    if (ddlYear.SelectedValue == "0")
                    {
                        ddlYear.SelectedIndex = 1;
                    }
                    lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    AccsGridBind(lstDate);
                    IsTotalAccBind(lstDate);
                }

                if (ctrlMonth == 0)
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                        AccsGridBind(lstDate);
                        IsTotalAccBind(lstDate);
                    }
                }

                if (ctrlMonth == 0 && ddlYear.SelectedIndex == 0)
                {
                    lstDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    AccsGridBind(lstDate);
                    IsTotalAccBind(lstDate);
                }

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

        protected void gdvAccs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow gvRow = e.Row;
                if (gvRow.RowType == DataControlRowType.Header)
                {
                    if (gvRow.Cells[2].Text == "Account Name")
                    {
                        gvRow.Cells.Remove(gvRow.Cells[0]);
                        gvRow.Cells.Remove(gvRow.Cells[1]);
                        gvRow.Cells.Remove(gvRow.Cells[4]);
                        //gvRow.Cells.Remove(gvRow.Cells[3]);

                      

                        GridViewRow gvHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                        TableCell headerCellDetail = new TableCell()
                        {
                            Text = "",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2

                        };
                        TableCell headerCell0 = new TableCell()
                        {
                            Text = "Account Name",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2

                        };

                        TableCell headerCell1 = new TableCell()
                        {
                            Text = "Remaining Balance",
                            HorizontalAlign = HorizontalAlign.Center,
                            ColumnSpan = 3


                        };

                        TableCell headerCell2 = new TableCell()
                        {
                            Text = "¥",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2

                        };

                        gvHeader.Cells.Add(headerCellDetail);
                        gvHeader.Cells.Add(headerCell0);
                        gvHeader.Cells.Add(headerCell1);
                        gvHeader.Cells.Add(headerCell2);
                       
          

                        gdvAccs.Controls[0].Controls.AddAt(0, gvHeader);
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    Label lblOBLUSD = (Label)(e.Row.Cells[3].FindControl("lblOBLUSD"));
                    Label lblOBLKyat = (Label)(e.Row.Cells[4].FindControl("lblOBLKs"));
                    Label lblOBLYen = (Label)(e.Row.Cells[6].FindControl("lblOBLYen"));

                    if (!String.IsNullOrWhiteSpace(txtUSDtoKyat.Text) && !String.IsNullOrWhiteSpace(txtUSDtoYen.Text))
                    {
                        Decimal USD = Convert.ToDecimal(lblOBLUSD.Text);
                        Decimal Kyat = Convert.ToDecimal(lblOBLKyat.Text);

                        Decimal USDtoKyat = Convert.ToDecimal(txtUSDtoKyat.Text);
                        Decimal USDtoYen = Convert.ToDecimal(txtUSDtoYen.Text);

                        if (USDtoKyat != 0 && USDtoYen != 0)
                        {
                            Decimal Yen = ((Kyat / USDtoKyat) + USD) * USDtoYen;

                            lblOBLYen.Text = Yen.ToString("N2");
                        }
                        else
                        {
                            lblOBLYen.Text = "0.00";
                        }
                    }
                    else
                    {
                        lblOBLYen.Text = "0.00";
                    }
                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void gdvTotalOBL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow gvRow = e.Row;
                if (gvRow.RowType == DataControlRowType.Header)
                {
                    if (gvRow.Cells[1].Text == "Account Name")
                    {
                        gvRow.Cells.Remove(gvRow.Cells[0]);
                        gvRow.Cells.Remove(gvRow.Cells[0]);
                        gvRow.Cells.Remove(gvRow.Cells[2]);

                        GridViewRow gvHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                        TableCell headerCell0 = new TableCell()
                        {
                            Text = "Account Name",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2

                        };

                        TableCell headerCell1 = new TableCell()
                        {
                            Text = "Remaining Balance",
                            HorizontalAlign = HorizontalAlign.Center,
                            ColumnSpan = 3

                        };

                        TableCell headerCell2 = new TableCell()
                        {
                            Text = "¥",
                            HorizontalAlign = HorizontalAlign.Center,
                            RowSpan = 2

                        };

                        gvHeader.Cells.Add(headerCell0);
                        gvHeader.Cells.Add(headerCell1);
                        gvHeader.Cells.Add(headerCell2);

                        gdvTotalOBL.Controls[0].Controls.AddAt(0, gvHeader);
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                    Label lblOBLUSD = (Label)(e.Row.Cells[2].FindControl("lblOBLUSD"));
                    Label lblOBLKyat = (Label)(e.Row.Cells[3].FindControl("lblOBLKs"));
                    Label lblOBLYen = (Label)(e.Row.Cells[5].FindControl("lblOBLYen"));

                    if (!String.IsNullOrWhiteSpace(txtUSDtoKyat.Text) && !String.IsNullOrWhiteSpace(txtUSDtoYen.Text))
                    {
                        Decimal USD = Convert.ToDecimal(lblOBLUSD.Text);
                        Decimal Kyat = Convert.ToDecimal(lblOBLKyat.Text);

                        Decimal USDtoKyat = Convert.ToDecimal(txtUSDtoKyat.Text);
                        Decimal USDtoYen = Convert.ToDecimal(txtUSDtoYen.Text);

                        if (USDtoKyat != 0 && USDtoYen != 0)
                        {
                            Decimal Yen = ((Kyat / USDtoKyat) + USD) * USDtoYen;

                            lblOBLYen.Text = Yen.ToString("N2");
                        }
                        else
                        {
                            lblOBLYen.Text = "0.00";
                        }
                    }
                    else
                    {
                        lblOBLYen.Text = "0.00";
                    }
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

        protected void gdvTotalOBL_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells.Remove(e.Row.Cells[0]);
                    e.Row.Cells[0].Text = "Total";
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    //e.Row.Cells.Remove(e.Row.Cells[1]);   

                    Label lblTotalUSD = new Label();
                    lblTotalUSD.ID = "lblTotalUSD";

                    Label lblUnitUSD = new Label();
                    lblUnitUSD.ID = "lblUnitUSD";
                    lblUnitUSD.Text = "<span style='font-size:x-small'><br/> USD </span>";

                    Label lblTotalKyat = new Label();
                    lblTotalKyat.ID = "lblTotalKyat";

                    Label lblUnitKyat = new Label();
                    lblUnitKyat.ID = "lblUnitKyat";
                    lblUnitKyat.Text = "<span style='font-size:x-small'><br/> Kyat </span>";


                    Label lblTotalYen1 = new Label();
                    lblTotalYen1.ID = "lblTotalYen1";

                    Label lblUnitYen1 = new Label();
                    lblUnitYen1.ID = "lblUnitYen1";
                    lblUnitYen1.Text = "<span style='font-size:x-small'><br/> ¥ </span>";

                    Label lblTotalYen = new Label();
                    lblTotalYen.ID = "lblTotalYen";

                    Label lblUnitYen = new Label();
                    lblUnitYen.ID = "lblUnitYen";
                    lblUnitYen.Text = "<span style='font-size:x-small'><br/> ¥ </span>";

                    e.Row.Cells[1].Controls.Add(lblTotalUSD);
                    e.Row.Cells[1].Controls.Add(lblUnitUSD);

                    e.Row.Cells[2].Controls.Add(lblTotalKyat);
                    e.Row.Cells[2].Controls.Add(lblUnitKyat);

                    e.Row.Cells[3].Controls.Add(lblTotalYen1);
                    e.Row.Cells[3].Controls.Add(lblUnitYen1);


                    e.Row.Cells[4].Controls.Add(lblTotalYen);
                    e.Row.Cells[4].Controls.Add(lblUnitYen);

                }
            }
            catch (Exception ex)
            {
                errBL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        protected void txtUSDtoKyat_TextChanged(object sender, EventArgs e)
        {
            String lstDate;
            if (ctrlMonth == 0 && ddlYear.SelectedIndex == 0)
            {
                lstDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                AccsGridBind(lstDate);
                IsTotalAccBind(lstDate);
            }
            if (ctrlMonth != 0)
            {
                if (ddlYear.SelectedValue == "0")
                {
                    ddlYear.SelectedIndex = 1;
                }
                lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                AccsGridBind(lstDate);
                IsTotalAccBind(lstDate);
            }

            if (ctrlMonth == 0)
            {
                if (ddlYear.SelectedValue != "0")
                {
                    lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    AccsGridBind(lstDate);
                    IsTotalAccBind(lstDate);
                }
            }

        }

        protected void txtUSDtoYen_TextChanged(object sender, EventArgs e)
        {
            String lstDate;
            if (ctrlMonth == 0 && ddlYear.SelectedIndex == 0)
            {
                lstDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                AccsGridBind(lstDate);
                IsTotalAccBind(lstDate);
            }
            if (ctrlMonth != 0)
            {
                if (ddlYear.SelectedValue == "0")
                {
                    ddlYear.SelectedIndex = 1;
                }
                lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), ctrlMonth, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                AccsGridBind(lstDate);
                IsTotalAccBind(lstDate);
            }

            if (ctrlMonth == 0)
            {
                if (ddlYear.SelectedValue != "0")
                {
                    lstDate = new DateTime(int.Parse(ddlYear.SelectedItem.Text), 12, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
                    AccsGridBind(lstDate);
                    IsTotalAccBind(lstDate);
                }
            }
            txtUSDtoKyat.Focus();
        }
    }
}