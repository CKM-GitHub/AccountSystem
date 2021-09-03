using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class FormEntry_BL
    {
        FormEntry_DL frmDL = new FormEntry_DL();

         public FormEntry_BL() { }

        public DataTable FrmsGridBind()
        {
            return frmDL.FrmsGridBind();
        }

        public bool IsFormExist(int frmID, string frmName, string frmDir)
        {
            return frmDL.IsFormExist(frmID, frmName, frmDir);
        }

        public bool SaveNewFrm(string frmName, string frmDir, string parent, string loginname)
        {
            return frmDL.SaveNewFrm(frmName, frmDir, parent, loginname);
        }

        public bool UpdateFrm(int frmID, string frmName, string frmDir, string parent, string loginname)
        {
            return frmDL.UpdateFrm(frmID, frmName, frmDir, parent,loginname);
        }

        public bool DeleteFrm(int frmID)
        {
            return frmDL.DeleteFrm(frmID);
        }
    }
}
