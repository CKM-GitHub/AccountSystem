using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Account_DL
{
    public class AccountEntry_DL
    {
        string connection = ConfigurationManager.ConnectionStrings["AccountConnectionString"].ConnectionString;
        ErrorLog_DL errDL = new ErrorLog_DL();

        public AccountEntry_DL() { }

        public bool IsAccExist(int accID,string accName)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_IsAccExist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@accID", accID);
                cmd.Parameters.AddWithValue("@accName", accName);

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
                errDL.SaveErrLog("sql", ex.ToString());
                return false;
            }
        }

        public bool SaveNewAcc(string accName, string accNo, int isCheck, string roleID, string roleName, string loginname)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SaveNewAcc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@accName", accName);
                cmd.Parameters.AddWithValue("@accNo", accNo);
                cmd.Parameters.AddWithValue("@isCheck", isCheck);
                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                cmd.Parameters.AddWithValue("@loginname", loginname);

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

        public DataTable AccsGridBind()
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_AccsGridBind", con);
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

        public bool UpdateNewAcc(int accID, string accName, string accNo, int isCheck, string roleID, string roleName, string loginname)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_UpdateNewAcc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@accID", accID);
                cmd.Parameters.AddWithValue("@accName", accName);
                cmd.Parameters.AddWithValue("@accNo", accNo);
                cmd.Parameters.AddWithValue("@isCheck", isCheck);
                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                cmd.Parameters.AddWithValue("@loginname", loginname);

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

        public bool DeleteAcc(int accID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_DeleteAcc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@accID", accID);

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

        public DataTable AccountSelect()
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_AccountSelect", con);
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

        public DataTable GetRoles()
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
    }
}
