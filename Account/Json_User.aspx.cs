using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Account_BL;
using Newtonsoft.Json;

namespace Account
{
    public partial class Json_User : System.Web.UI.Page
    {
         Json_User_BL jBL=new Json_User_BL();
            protected void Page_Load(object sender, EventArgs e)
            {
                DataTable dt = new DataTable();
                dt = jBL.GetUser();

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Password"] = GlobalUI.DecryptPassword(dr["Password"].ToString());
                }

                string str = DataTableToJSONWithStringBuilder(dt);
                Response.Write(str);
            }

            public string DataTableToJSONWithStringBuilder(DataTable table)
            {
                string JSONString = JsonConvert.SerializeObject(table);
                return JSONString;
            }
    }
}
