using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class HomePage_BL
    {
        HomePage_DL homeDL = new HomePage_DL();

        public DataTable AccsGridBind(String date)
        {
            return homeDL.AccsGridBind(date);
        }

        public DataTable IsTotalAccBind(String date)
        {
            return homeDL.IsTotalAccBind(date);
        }
    }
}
