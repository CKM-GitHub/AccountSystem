using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Account_BL;

namespace OpeningBalance_Console
{
    public class Program
    {
        #region declare
        string connection = ConfigurationManager.ConnectionStrings["AccountConnectionString"].ConnectionString;

        ErrorLog_BL errBL = new ErrorLog_BL();
        #endregion

        static void Main(string[] args)
        {
            new Program().SaveOBL();
        }

        public void SaveOBL()
        {
            try
            {
                DataTable dt = GetCurMonthOBLst();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool isSaveOK = SaveOBL(int.Parse(dt.Rows[i]["AccountsID"].ToString()));

                    if (isSaveOK)
                    {
                        Console.WriteLine("Save OK");
                    }
                    else
                    {
                        Console.WriteLine("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                SaveErrLog("console",ex.ToString());
            }
        }

        private bool SaveOBL(int accID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SaveOBL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@accID", accID);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                SaveErrLog("console_sql", ex.ToString());
                return false;
            }
        }

        private void SaveErrLog(string name, string err)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("SP_SaveErrLog", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@error", err);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private DataTable GetCurMonthOBLst()
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_GetCurMonthOBLst", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                cmd.Connection.Close();

                return dt;
            }
            catch (Exception ex)
            {
                SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                throw ex;
            }
        }
    }
}
