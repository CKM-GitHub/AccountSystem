using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class TransactionType_Entry_BL
    {
        TransactionType_Entry_DL transDL = new TransactionType_Entry_DL();

        public bool SaveNewTransType(string transType, string loginname)
        {
            return transDL.SaveNewTransType(transType, loginname);
        }

        public bool UpdateTransType(int typeID, string transType, string loginname)
        {
            return transDL.UpdateTransType(typeID, transType, loginname);
        }

        public bool IsCategoryExist(int typeID, string transType)
        {
            return transDL.IsCategoryExist(typeID, transType);
        }

        public DataTable TransTypeBind()
        {
            return transDL.TransTypeBind();
        }

        public void delTransType(int transID)
        {
            transDL.delTransType(transID);
        }
    }
}
