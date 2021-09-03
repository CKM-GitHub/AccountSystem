using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Account_DL;

namespace Account_BL
{
    public class Menu_Entry_BL
    {
        Menu_Entry_DL mnuEtryDL = new Menu_Entry_DL();

        public bool UpdateNewMenu(int menuID, string menuName, string parentMenu, string menuDir, string roleID, string roleName, string username)
        {
            return mnuEtryDL.UpdateNewMenu(menuID, menuName, parentMenu, menuDir, roleID, roleName, username);
        }

        public bool SaveNewMenu(int formID, string roleID, string roleName, string userName)
        {            
            return mnuEtryDL.SaveNewMenu(formID, roleID, roleName, userName);

        }

        public DataTable MenusGridBind(string roles)
        {
            return mnuEtryDL.MenusGridBind(roles);
        }

        public bool IsMenusExist(int menuID, int formID, int roleID)
        {
            return mnuEtryDL.IsMenusExist(menuID, formID, roleID);
        }

        public DataTable GetRoles()
        {
            return mnuEtryDL.ddlRolesBind();
        }

        public bool DeleteMenu(int menuID)
        {
            return mnuEtryDL.DeleteMenu(menuID);
        }

        public DataTable GetForms()
        {
            return mnuEtryDL.GetForms();
        }

        public string GetFormDir(int frmID)
        {
            return mnuEtryDL.GetForms(frmID);
        }

        public string GetAllowRoles(int frmID)
        {
            return mnuEtryDL.GetAllowRoles(frmID);
        }

        public bool RemoveRole(int ID, string roleID, string roleName, string userName)
        {
            return mnuEtryDL.RemoveRole(ID, roleID, roleName, userName);
        }

        public void SaveEdit(int ID, string roleName, string userName)
        {
            mnuEtryDL.SaveEdit(ID, roleName, userName);
        }

        public void RemoveEdit(int ID, string roleName, string userName)
        {
            mnuEtryDL.RemoveEdit(ID, roleName, userName);
        }

        public void SaveDel(int ID, string roleName, string userName)
        {
            mnuEtryDL.SaveDel(ID, roleName, userName);
        }

        public void RemoveDel(int ID, string roleName, string userName)
        {
            mnuEtryDL.RemoveDel(ID, roleName, userName);
        }
    }
}
