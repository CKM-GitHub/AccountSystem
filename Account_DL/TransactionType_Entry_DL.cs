using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Account_DL
{
    public class TransactionType_Entry_DL
    {
        string connection = ConfigurationManager.ConnectionStrings["AccountConnectionString"].ConnectionString;
        ErrorLog_DL errDL = new ErrorLog_DL();

        public bool IsCategoryExist(int typeID, string transType)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_IsCategoryExist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@typeID", typeID);
                cmd.Parameters.AddWithValue("@transType", transType);

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

        public bool SaveNewTransType(string transType, string loginname)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SaveNewTransType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@transType", transType);
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

        public bool UpdateTransType(int typeID, string transType, string loginname)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_UpdateTransType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@typeID", typeID);
                cmd.Parameters.AddWithValue("@transType", transType);
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

        public DataTable TransTypeBind()
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_TransTypeBind", con);
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

        public void delTransType(int transID)
        {
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("SP_DelTransTyp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            cmd.Parameters.AddWithValue("@transID", transID);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }
    }
}
