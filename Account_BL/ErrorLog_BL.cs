using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class ErrorLog_BL
    {
        ErrorLog_DL errDL = new ErrorLog_DL();

        public void SaveErrLog(string name,String err)
        {
            errDL.SaveErrLog(name,err);
        }

        public DataTable ErrorGridBind()
        {
           return errDL.ErrorGridBind();
        }
    }
}
