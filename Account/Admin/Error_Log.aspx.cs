using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;

namespace Account.Admin
{
    public partial class Error_Log : System.Web.UI.Page
    {
        ErrorLog_BL errBL = new ErrorLog_BL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ErrorGridBind();
            }
        }

        private void ErrorGridBind()
        {
            gdvErrorLog.DataSource=errBL.ErrorGridBind();
            gdvErrorLog.DataBind();
        }

        protected void gdvErrorLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvErrorLog.PageIndex = e.NewPageIndex;
            ErrorGridBind();
        }
    }
}