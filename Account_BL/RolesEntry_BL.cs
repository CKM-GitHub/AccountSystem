using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Account_DL;

namespace Account_BL
{
    public class RolesEntry_BL
    {
         RolesEntry_DL roleDL=new RolesEntry_DL();
         public RolesEntry_BL() { }

         public DataTable RolesGridBind()
         {
            return roleDL.RolesGridBind();
         }

         public bool UpdateNewRole(int roleID, string roleName,string userName)
         {
             return roleDL.UpdateNewRole(roleID, roleName, userName);
         }

         public bool SaveNewRole(string roleName,string userName)
         {
             return roleDL.SaveNewRole(roleName,  userName);
         }

         public bool IsRolesExist(int roleID, string roleName)
         {
             return roleDL.IsRolesExist(roleID, roleName);
         }

         public bool DeleteRole(int roleID)
         {
             return roleDL.DeleteRole(roleID);
         }

         public DataTable GetForms()
         {
             return roleDL.GetForms();
         }

         public void RemoveRole(string ID, string roleID, string roleName, string userName)
         {
             roleDL.RemoveRole( ID,  roleID,  roleName,  userName);
         }

         public void UpdateRole(string ID, string roleID,string oldRole, string newRole, string userName)
         {
             roleDL.UpdateRole( ID,  roleID, oldRole,  newRole,  userName);
         }

         public string GetFormDir(int formID)
         {
             return roleDL.GetFormDir(formID);
         }

         public string GetAllowRoles(int formID)
         {
             return roleDL.GetAllowRoles(formID);
         }
    }
}
