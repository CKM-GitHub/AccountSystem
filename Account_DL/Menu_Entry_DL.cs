using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Account_DL
{
    public class Menu_Entry_DL
    {
        string connection = ConfigurationManager.ConnectionStrings["AccountConnectionString"].ConnectionString;
        ErrorLog_DL errDL = new ErrorLog_DL();

        public bool UpdateNewMenu(int menuID, string menuName, string parentMenu, string menuDir, string roleID, string roleName, string username)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_UpdateNewMenu", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@menuID", menuID);
                cmd.Parameters.AddWithValue("@menuName", menuName);
                cmd.Parameters.AddWithValue("@menuDir", menuDir);
                cmd.Parameters.AddWithValue("@parentmenu", parentMenu);
                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                cmd.Parameters.AddWithValue("@username", username);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                return false;
            }
        }

        public bool SaveNewMenu(int formID, string roleID, string roleName, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SaveNewMenu", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@formID", formID);
                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@roleName", roleName);               
                cmd.Parameters.AddWithValue("@userName", userName);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                return false;
            }
        }

        public DataTable MenusGridBind(string roles)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_MenusGridBind", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                //cmd.Parameters.AddWithValue("@roles", roles);

                cmd.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                cmd.Connection.Close();

                return dt;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                throw ex;
            }
        }

        public bool IsMenusExist(int menuID, int formID, int roleID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_IsMenusExist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@menuID", menuID);
                cmd.Parameters.AddWithValue("@formID", formID);
                cmd.Parameters.AddWithValue("@roleID", roleID);

                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)  //menus already assigned
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                return false;
            }
        }

        public DataTable ddlRolesBind()
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_RolesGridBind", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                cmd.Connection.Close();

                return dt;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                throw ex;
            }
        }

        public bool DeleteMenu(int menuID)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_DeleteMenu", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@menuID", menuID);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog("sql", ex.ToString());
                return false;
            }
        }

        public DataTable GetForms()
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_FrmsGridBind", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                cmd.Connection.Close();

                return dt;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog("sql", ex.ToString());
                throw ex;
            }
        }

        public string GetForms(int frmID)
        {
            try
            {
                String frmDir = "";
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_GetForms", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@frmID", frmID);

                cmd.Connection.Open();

                if (cmd.ExecuteScalar() != null)  //menus already assigned
                {
                    frmDir = cmd.ExecuteScalar().ToString();
                }
                else
                {  }
                cmd.Connection.Close();

                return frmDir;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                return "";
            }
        }

        public string GetAllowRoles(int frmID)
        {
            try
            {
            String roles = "";
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_GetAllowRoles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@frmID", frmID);

                cmd.Connection.Open();

                if (cmd.ExecuteScalar() != null)  //menus already assigned
                {
                    roles = cmd.ExecuteScalar().ToString();
                }
                else
                {  }
                cmd.Connection.Close();

                return roles;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                return "";
            }
        }

        public bool RemoveRole(int ID, string roleID, string roleName, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_RemoveRoles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@formID", ID);
                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                cmd.Parameters.AddWithValue("@userName", userName);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                return false;
            }
        }

        public void SaveEdit(int formID, string roleName, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SaveEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@formID", formID);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                cmd.Parameters.AddWithValue("@userName", userName);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        public void RemoveEdit(int formID, string roleName, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_RemoveEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@formID", formID);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                cmd.Parameters.AddWithValue("@userName", userName);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        public void SaveDel(int ID, string roleName, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SaveDel", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@formID", ID);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                cmd.Parameters.AddWithValue("@userName", userName);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }

        public void RemoveDel(int ID, string roleName, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_RemoveDel", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@formID", ID);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                cmd.Parameters.AddWithValue("@userName", userName);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
            }
        }
    }
}
