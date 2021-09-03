using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Account_DL
{
     public class Transaction_DL
    {
         string connection = ConfigurationManager.ConnectionStrings["AccountConnectionString"].ConnectionString;
         ErrorLog_DL errDL = new ErrorLog_DL();

         public Transaction_DL() { }

         public DataTable AccNameDdlBind()
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
                 errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                 throw ex;
             }
         }

         public int SaveTransaction(int accID, int stsID, string amount, string cashUnit, int transTypeID, string remark, string particular, string date, string createdUser)
         {
             try
             {
                 DataTable dt = new DataTable();
                 SqlConnection con = new SqlConnection(connection);
                //SqlCommand cmd = new SqlCommand("Transaction_InsertTest", con);
                SqlCommand cmd = new SqlCommand("SP_SaveTransaction", con);
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.CommandTimeout = 0;

                 cmd.Parameters.AddWithValue("@accID", accID);
                 cmd.Parameters.AddWithValue("@stsID", stsID);
                 cmd.Parameters.AddWithValue("@amount", amount);
                 cmd.Parameters.AddWithValue("@cashUnit", cashUnit);
                 cmd.Parameters.AddWithValue("@transTypeID", transTypeID);
                 cmd.Parameters.AddWithValue("@remark", remark);
                 cmd.Parameters.AddWithValue("@particular", particular);
                 cmd.Parameters.AddWithValue("@date", date);
                 cmd.Parameters.AddWithValue("@createdUser", createdUser);

                 cmd.Connection.Open();
                 object obj = cmd.ExecuteScalar();
                 cmd.Connection.Close();

                 if (obj != null)  //return 1 for acc ID with oBL null
                 {
                     return Convert.ToInt32(obj);
                 }
                 else
                 {
                     return 0;
                 }
             }
             catch (Exception ex)
             {
                 errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                 return 0;
             }
         }

         public DataTable SelectAll(int accId, int stsID,string date)
         {
             try
             {
                 SqlConnection con = new SqlConnection(connection);
                 SqlCommand cmd = new SqlCommand("SP_TransactionReport", con);
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.CommandTimeout = 0;
                 cmd.Parameters.AddWithValue("@AccId", accId);
                 cmd.Parameters.AddWithValue("@stsID", stsID);
                 cmd.Parameters.AddWithValue("@date", date);
                 DataTable dt = new DataTable();
                 cmd.Connection.Open();
                 SqlDataAdapter sda = new SqlDataAdapter(cmd);
                 sda.Fill(dt);
                 cmd.Connection.Close();
                 return dt;
             }
             catch (Exception ex)
             {
                 errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                 throw ex;
             }
         }

         public DataTable stsNameDdlBind()
         {
             try
             {
                 SqlConnection con = new SqlConnection(connection);
                 SqlCommand cmd = new SqlCommand("SP_stsGridBind", con);
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
                 errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                 throw ex;
             }
         }

         public DataTable ddlCashUnitBind()
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
                 errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                 throw ex;
             }
         }

         public void SaveTransAttachment(int transID, string fileName)
         {
             try
             {
                 SqlConnection con = new SqlConnection(connection);
                 SqlCommand cmd = new SqlCommand("SP_SaveTransAttachment", con);
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.CommandTimeout = 0;

                 cmd.Parameters.AddWithValue("@transID", transID);
                 cmd.Parameters.AddWithValue("@fileName", fileName);

                 cmd.Connection.Open();
                 cmd.ExecuteNonQuery();
                 cmd.Connection.Close();
             }
             catch (Exception ex)
             {
                 errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                 throw ex;
             }
         }

        public DataTable SearchReport(int accID, int TypeID, int stsID, string cashUnit, string Date)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SearchTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@accID", accID);
                cmd.Parameters.AddWithValue("@TypeID", TypeID);
                cmd.Parameters.AddWithValue("@stsID", stsID);
                cmd.Parameters.AddWithValue("@cashUnit", cashUnit);
                cmd.Parameters.AddWithValue("@Date", Date);
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

        public DataTable SelectEditData(int accID)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SelectTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@accID", accID);
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

        public void UpdateTran(int transID, int accID, int typID, string particular, string remarks, int stsID, string amount, string unit, string updUser)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("Transaction_UpdateTest", con);
            //SqlCommand cmd = new SqlCommand("SP_UpdTrans", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            cmd.Parameters.AddWithValue("@transID", transID);
            cmd.Parameters.AddWithValue("@accID", accID);
            cmd.Parameters.AddWithValue("@typID", typID);
            cmd.Parameters.AddWithValue("@particular", particular);
            cmd.Parameters.AddWithValue("@remarks", remarks);
            cmd.Parameters.AddWithValue("@stsID", stsID);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@unit", unit);
            cmd.Parameters.AddWithValue("@updUser", updUser);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public void UpdateAllTran(int transID, string remainUSD, string remainKS, string remainYen)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("SP_UpdateAllTrans", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            cmd.Parameters.AddWithValue("@transID", transID);
            cmd.Parameters.AddWithValue("@remainUSD", remainUSD);
            cmd.Parameters.AddWithValue("@remainKS", remainKS);
            cmd.Parameters.AddWithValue("@remainYen", remainYen);


            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }
}
