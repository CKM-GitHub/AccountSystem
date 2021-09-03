using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
   public class AccountMaster_BL
    {
       AccountMaster_DL accMasDL = new AccountMaster_DL();

        public DataTable GetMenu(string role)
        {
            return accMasDL.GetMenu(role);
        }

        public DataTable GetSubMenu(string parentMenu,string roles)
        {
            return accMasDL.GetSubMenu(parentMenu,roles);
        }

        public DataTable GetMenuNullParent(string role)
        {
            return accMasDL.GetMenuNullParent(role);
        }

        public DataTable GetParentMnuAccAdmin()
        {
            return accMasDL.GetParentMnuAccAdmin();
        }

        public DataTable GetMenuNullParentAccAdmin()
        {
            return accMasDL.GetMenuNullParentAccAdmin();
        }

        public DataTable GetSubMenuAccAdmin(string parentMnu)
        {
            return accMasDL.GetSubMenuAccAdmin(parentMnu);
        }
    }
}
