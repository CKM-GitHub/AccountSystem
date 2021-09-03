using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Account_DL;

namespace Account_BL
{
    public class OpeningBalance_BL
    {
        OpeningBalance_DL openDL=new OpeningBalance_DL();
        public OpeningBalance_BL() { }

        public bool Save(string accID, string balanceUSD,string balanceKs,string balanceYen,string fromdate, string todate, string remarks,string loginname)
        {
            return openDL.Save(accID, balanceUSD, balanceKs, balanceYen, fromdate, todate, remarks, loginname);
        }

        public DataTable GetID(int userId)
        {
            return openDL.GetID(userId);
        }
        public void deletebyId(int userId)
        {
            openDL.deletebyId(userId);
        }

        public DataTable selectOBal()
        {
            return openDL.selectOBal();
        }

        public void updatebyUserId(string accId, decimal balance, string fromdate, string todate, int obalid, string loginname)
        {
            openDL.updatebyUserId(accId, balance, fromdate, todate, obalid, loginname);
        }
    }
}
