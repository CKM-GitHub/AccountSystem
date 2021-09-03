using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Account_DL;

namespace Account_BL
{
    public class UserEntry_BL
    {
        UserEntry_DL userDL=new UserEntry_DL();
        public UserEntry_BL() { }

        public bool IsUserExist(string user)
        {
            return userDL.IsUserExist(user);
        }

        public bool SaveNewUser(string userName, string passWord, int roleID,string loginname)
        {
            return userDL.SaveNewUser(userName, passWord, roleID, loginname);
        }

        public DataTable GetUserByUserID(int userId)
        {
            return userDL.GetUserByUserID(userId);
        }

        public bool deletebyUserId(int userId)
        {
           return userDL.deletebyUserId(userId);
        }

        public DataTable selectUser()
        {
            return userDL.selectUser();
        }

        public bool updatebyUserId(string username, string password,int roleID, int userid, string loginname)
        {
            return userDL.updatebyUserId(username, password, roleID, userid, loginname);
        }

        public DataTable ddlRolesBind()
        {
            return userDL.ddlRolesBind();
        }
    }
}
