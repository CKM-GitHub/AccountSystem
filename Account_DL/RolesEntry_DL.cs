using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Account_DL
{
   public class RolesEntry_DL
    {
        string connection = ConfigurationManager.ConnectionStrings["AccountConnectionString"].ConnectionString;
        ErrorLog_DL errDL = new ErrorLog_DL();

        public RolesEntry_DL() { }

        public DataTable RolesGridBind()
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

        public bool UpdateNewRole(int roleID, string roleName, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_UpdateNewRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

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

        public bool SaveNewRole(string roleName, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SaveNewRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

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

        public bool IsRolesExist(int roleID, string roleName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_IsRolesExist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@roleName", roleName);

                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)  //acc already inserted
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

        public bool DeleteRole(int roleID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_DeleteRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@roleID", roleID);

                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();

                if (obj != null)  //acc already inserted
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
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

        public void RemoveRole(string ID, string roleID, string roleName, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_RemoveAllOptRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@formID", ID);
                cmd.Parameters.AddWithValue("@roleID", roleID);
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

        public void UpdateRole(string ID, string roleID, string oldRole, string newRole, string userName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_UpdateRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@formID", ID);
                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@oldRole", oldRole);
                cmd.Parameters.AddWithValue("@newRole", newRole);
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

        public string GetFormDir(int formID)
        {
            try
            {
                String frmDir = "";
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_GetForms", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@frmID", formID);

                cmd.Connection.Open();

                if (cmd.ExecuteScalar() != null)  //menus already assigned
                {
                    frmDir = cmd.ExecuteScalar().ToString();
                }
                else
                { }
                cmd.Connection.Close();

                return frmDir;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                return "";
            }
        }

        public string GetAllowRoles(int formID)
        {
            try
            {
                String roles = "";
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_GetAllowRoles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@frmID", formID);

                cmd.Connection.Open();

                if (cmd.ExecuteScalar() != null)  //menus already assigned
                {
                    roles = cmd.ExecuteScalar().ToString();
                }
                else
                { }
                cmd.Connection.Close();

                return roles;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                return "";
            }
        }
    }
}
