using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class LoginIn_BL
    {
        LoginIn_DL lgnDL = new LoginIn_DL();
        public LoginIn_BL() { }

        public DataTable IsValidUser(string email, string passWord)
        {
            return lgnDL.IsValidUser(email, passWord);
        }

        public DataTable GetAllowedUrl(string roles)
        {
            return lgnDL.GetAllowedUrl(roles);
        }

        public DataTable GetRoles()
        {
            return lgnDL.GetRoles();
        }
    }
}
