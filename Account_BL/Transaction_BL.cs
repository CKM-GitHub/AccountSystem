using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class Transaction_BL
    {
        Transaction_DL transDL = new Transaction_DL();
        Transaction_Report_DL trarRDL = new Transaction_Report_DL();

        public DataTable AccNameDdlBind()
        {
            return transDL.AccNameDdlBind();
        }

        public int SaveTransaction(int accID, int stsID, string amount, string cashUnit, int transTypeID, string remark, string particular, string date, string createdUser)
        {
            return transDL.SaveTransaction(accID, stsID, amount, cashUnit, transTypeID, remark, particular, date, createdUser);
        }

        public DataTable Reportdata(int accId,int stsID, string date)
        {
            return transDL.SelectAll(accId, stsID,date);
        }

        public DataTable stsNameDdlBind()
        {
            return transDL.stsNameDdlBind();
        }

        public DataTable TransTypeBind()
        {
            return transDL.TransTypeBind();
        }

        public DataTable ddlCashUnitBind()
        {
            return transDL.ddlCashUnitBind();
        }

        public void SaveTransAttachment(int transID, string fileName)
        {
            transDL.SaveTransAttachment(transID, fileName);
        }

        public DataTable SelectEditData(int accID)
        {
            return transDL.SelectEditData(accID);
        }
        public DataTable SearchReport(int accID, int TypeID, int stsID, string cashUnit, string Date)
        {
            return transDL.SearchReport(accID, TypeID, stsID, cashUnit, Date);
        }

        public void updateAllTranabc(int transID, string remainUSD, string remainKS, string remainYen)
        {
            transDL.UpdateAllTran(transID, remainUSD, remainKS, remainYen);
        }

        public bool UpdateTran(int transID, int accID, int typID,string date, string particular, string remarks, int stsID, string amount, string unit, string updUser)
        {
            return transDL.UpdateTran(transID, accID, typID, date, particular, remarks, stsID, amount, unit, updUser);
        }
        public void DeleteTransAttachment(int attachID)
        {
            trarRDL.DeleteTransAttachment(attachID);
        }
        public DataTable GetTransAttachs(int transID)
        {
            return trarRDL.GetTransAttachs(transID);
        }
        public DataTable updTransAttach(int oldTransID, int newTransID)
        {
            return trarRDL.updTransAttach(oldTransID, newTransID);
        }
    }
}
