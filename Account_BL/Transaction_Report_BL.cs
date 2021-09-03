using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class Transaction_Report_BL
    {
        Transaction_Report_DL transDL = new Transaction_Report_DL();

        public DataTable SearchReport(int accID, int TypeID, int stsID, string cashUnit, string fromDate, string toDate)
        {
            return transDL.SearchReport(accID, TypeID, stsID, cashUnit,fromDate, toDate);
        }

        public DataTable TransTypeBind()
        {
            return transDL.TransTypeBind();
        }

        public object getTransactionDetail(int transID)
        {
            return transDL.getTransactionDetail(transID);

        }

        public void UpdateAllTrans(int transID, string remainUSD, string remainKS,string remainYen)
        {
            transDL.UpdateAllTrans(transID, remainUSD, remainKS,remainYen);
        }

        public void UpdateTrans(int transID, int accID, int typID, string particular, string remarks, int stsID, string amount, string unit, string updUser)
        {
            transDL.UpdateTrans(transID, accID, typID, particular, remarks, stsID, amount, unit, updUser);
        }

        public bool DelTrans(int transID)
        {
            return transDL.DelTrans(transID);
        }

        public bool SaveUpdRemark(int transID,string updRemark)
        {
            return transDL.SaveUpdRemark(transID, updRemark);
        }

        public DataTable ddlCashUnit_Bind()
        {
            return transDL.ddlCashUnit_Bind();
        }

        public object stsNameDdlBind()
        {
            return transDL.stsNameDdlBind();
        }

        public DataTable GetLastOBL(int transID, int lstAccID, string lstDate, string created_Date)
        {
            return transDL.GetLastOBL(transID, lstAccID, lstDate, created_Date);
        }

        public string GetLastTransDate(int accID)
        {
            return transDL.GetLastTransDate(accID);
        }

        public DataTable IsRoleCanEditDel(string file, string role)
        {
            return transDL.IsRoleCanEditDel(file, role);
        }

        public DataTable AccSpectCtrl(string accID)
        {
            return transDL.AccSpectCtrl(accID);
        }

        public DataTable GetTransAttachs(int transID)
        {
            return transDL.GetTransAttachs(transID);
        }

        public void SaveTransAttachment(int transID, string fileName)
        {
            transDL.SaveTransAttachment(transID, fileName);
        }

        public void DeleteTransAttachment(int attachID)
        {
            transDL.DeleteTransAttachment(attachID);
        }

        public DataTable updTransAttach(int oldTransID, int newTransID)
        {
            return transDL.updTransAttach(oldTransID, newTransID);
        }

        public void DelTransAttachByTransID(int transID)
        {
            transDL.DelTransAttachByTransID(transID);
        }
    }
}
