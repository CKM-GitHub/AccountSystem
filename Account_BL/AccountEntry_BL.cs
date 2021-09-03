using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class AccountEntry_BL
    {
        AccountEntry_DL accDL = new AccountEntry_DL();
        public AccountEntry_BL() { }

        public bool IsAccExist(int accID,string accName)
        {
            return accDL.IsAccExist(accID,accName);
        }

        public bool SaveNewAcc(string accName, string accNo, int isCheck, string roleID, string roleName, string loginname)
        {
            return accDL.SaveNewAcc(accName, accNo, isCheck, roleID, roleName, loginname);
        }

        public DataTable AccsGridBind()
        {
            return accDL.AccsGridBind();
        }

        public bool UpdateNewAcc(int accID, string accName, string accNo, int isCheck, string roleID, string roleName, string loginname)
        {
            return accDL.UpdateNewAcc(accID, accName, accNo, isCheck, roleID, roleName, loginname);
        }

        public bool DeleteAcc(int accID)
        {
            return accDL.DeleteAcc(accID);
        }

        public DataTable AccountSelect()
        {
            return accDL.AccountSelect();
        }

        public DataTable GetRoles()
        {
            return accDL.GetRoles();
        }
    }
}
