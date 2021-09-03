using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Account_BL;
using System.Web.Configuration;
using System.IO;
using System.Data;

namespace Account
{
    public class Common_Fun
    {
        public Common_Fun()
        {

        }

        public static string IsRoleCanEditDel(string file, string roles)
        {
            Transaction_Report_BL transBL = new Transaction_Report_BL();
            string canEdit = "0", canDel = "0";

            DataTable dt= transBL.IsRoleCanEditDel(file, roles);

            if (dt.Rows.Count > 0 && !String.IsNullOrWhiteSpace(roles))
            {
                if (dt.Rows[0]["canEdit"].ToString().Contains(roles))
                {
                    canEdit = "1";
                }

                if (dt.Rows[0]["canDel"].ToString().Contains(roles))
                {
                    canDel = "1";
                }
            }
            
            return canEdit+","+canDel;


        }

        public static void AddRolesToConfig(string[] arrConfigurl, string[] arrRoleName)
        {
            Configuration config = (Configuration)WebConfigurationManager.OpenWebConfiguration("~/" + arrConfigurl[1] + "/");
            AuthorizationSection section = (AuthorizationSection)config.GetSection("system.web/authorization");

            ConfigurationLocationCollection locations = config.Locations;

            foreach (ConfigurationLocation l in locations)
            {
                if (l.Path == arrConfigurl[2]) //This is case Sensitive
                {
                    Configuration adminConfig = (Configuration)l.OpenConfiguration();
                    AuthorizationSection admin_section = (AuthorizationSection)adminConfig.GetSection("system.web/authorization");

                    string webConfigUrl = HttpContext.Current.Server.MapPath("~/" + arrConfigurl[1] + "/Web.config"); 

                    //Remove all Current roles to folder name contained in arrConfigurl[1] location.
                    if (String.IsNullOrWhiteSpace(arrRoleName[0]) )
                    {
                        admin_section.Rules.Clear();
                        SetDefaultConfig(admin_section, webConfigUrl);
                        break;
                    }

                    admin_section.Rules.Clear();
                    ////Add New Roles to filename contained in arrConfigurl[2] location.
                    AuthorizationRule adminAuth = new AuthorizationRule(AuthorizationRuleAction.Allow);

                    //for assigned roles count more than 1
                    for (int i = 0; i < arrRoleName.Length; i++)
                    {
                        adminAuth.Roles.Add(arrRoleName[i]);
                    }
                    admin_section.Rules.Add(adminAuth);
                    adminAuth = null;

                    SetDefaultConfig(admin_section, webConfigUrl);
                }
            }
        }

        public static void SetDefaultConfig(AuthorizationSection admin_section, string webConfigUrl)
        {
            string AccountAdmin = ConfigurationManager.AppSettings["AccountAdmin"].ToString();

            //add default allowed user AccountAdmin
            AuthorizationRule adminAllow = new AuthorizationRule(AuthorizationRuleAction.Allow);
            adminAllow.Users.Add(AccountAdmin);
            admin_section.Rules.Add(adminAllow);

            // add deny users
            AuthorizationRule adminDeny = new AuthorizationRule(AuthorizationRuleAction.Deny);
            adminDeny.Users.Add("*");
            admin_section.Rules.Add(adminDeny);

            // Remove readonly attribute off of the file before execution
            if (File.Exists(webConfigUrl))
            {
                FileAttributes attributes = File.GetAttributes(webConfigUrl);

                if (attributes.HasFlag(FileAttributes.ReadOnly))
                    File.SetAttributes(webConfigUrl, FileAttributes.Normal);
            }

            admin_section.CurrentConfiguration.Save();
        }

        public static string AccSpectCtrl(string accID,string roles)
        {
            Transaction_Report_BL transBL = new Transaction_Report_BL();
            string accSpefCtrl = "0";

            DataTable dt = transBL.AccSpectCtrl(accID);

            if (dt.Rows.Count > 0 && !String.IsNullOrWhiteSpace(roles))
            {
                if (dt.Rows[0]["RoleName"].ToString().Contains(roles))
                {
                    accSpefCtrl = "1";
                }               
            }

            return accSpefCtrl;
        }
    }
}