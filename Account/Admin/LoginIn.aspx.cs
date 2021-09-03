using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;

namespace Account
{
    public partial class LoginIn : System.Web.UI.Page
    {
        #region declare
        LoginIn_BL lgnBL = new LoginIn_BL();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                revEmail.ValidationExpression = @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsValidUser(txtemail.Text, EncryptPassword(txtPass.Text)))
            {
                Response.Redirect("");
                Session["UserName"] = txtemail.Text;
            }
            else
            {
                GlobalUI.MessageBox("Something Wrong!");
            }
        }

        private bool IsValidUser(string email, string passWord)
        {
            return lgnBL.IsValidUser(email, passWord);
        }

        private string EncryptPassword(string passWord)
        {
            return GlobalUI.EncryptPassword(passWord);
        }
    }
}