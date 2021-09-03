using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Account_DL;

namespace Account_BL
{
    public class Json_User_BL
    {
        Json_User_DL jDL = new Json_User_DL();

        public System.Data.DataTable GetUser()
        {
            return jDL.GetUser();
        }
    }
}
