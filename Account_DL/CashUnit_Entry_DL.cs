using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Account_DL
{
    public class CashUnit_Entry_DL
    {
        string connection = ConfigurationManager.ConnectionStrings["AccountConnectionString"].ConnectionString;
        ErrorLog_DL errDL = new ErrorLog_DL();

        public CashUnit_Entry_DL() { }

        public bool IsUnitExist(int unitID, string cashUnit)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_IsUnitExist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@unitID", unitID);
                cmd.Parameters.AddWithValue("@cashUnit", cashUnit);

                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)  
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

        public bool SaveNewCashUnit(string cashUnit, string loginname)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SaveNewCashUnit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@cashUnit", cashUnit);
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

        public DataTable CashUnitGridBind()
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_CashUnitGridBind", con);
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

        public bool UpdateNewCashUnit(int unitID, string cashUnit, string loginname)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_UpdateNewCashUnit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@unitID", unitID);
                cmd.Parameters.AddWithValue("@cashUnit", cashUnit);
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

        public bool DeleteCashUnit(int unitID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_DeleteCashUnit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@unitID", unitID);

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

    }
}
