using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class CashUnit_Entry_BL
    {
         CashUnit_Entry_DL cashDL = new CashUnit_Entry_DL();

        public CashUnit_Entry_BL() { }

        public bool IsUnitExist(int accID,string accName)
        {
            return cashDL.IsUnitExist(accID, accName);
        }

        public bool SaveNewCashUnit(string accName, string loginname)
        {
            return cashDL.SaveNewCashUnit(accName, loginname);
        }

        public DataTable CashUnitGridBind()
        {
            return cashDL.CashUnitGridBind();
        }

        public bool UpdateNewCashUnit(int unitID, string cashUnit, string loginname)
        {
            return cashDL.UpdateNewCashUnit(unitID, cashUnit, loginname);
        }

        public bool DeleteCashUnit(int accID)
        {
            return cashDL.DeleteCashUnit(accID);
        }     
    }
}
