using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Account_DL
{
    public class Transaction_Report_DL
    {
        string connection = ConfigurationManager.ConnectionStrings["AccountConnectionString"].ConnectionString;

        ErrorLog_DL errDL = new ErrorLog_DL();

        public DataTable SearchReport(int accID, int TypeID, int stsID, string cashUnit, string fromDate, string toDate)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SearchTransReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@accID", accID);
                cmd.Parameters.AddWithValue("@TypeID", TypeID);
                cmd.Parameters.AddWithValue("@stsID", stsID);
                cmd.Parameters.AddWithValue("@cashUnit", cashUnit);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

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

        public DataTable getTransactionDetail(int transID)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_GetTransDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@transID", transID);

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

        public void UpdateAllTrans(int transID, string remainUSD, string remainKS,string remainYen)
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

        public void UpdateTrans(int transID, int accID, int typID, string particular, string remarks, int stsID, string amount, string unit, string updUser)
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

        public bool DelTrans(int transID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("Transaction_DeleteTest", con);
                //SqlCommand cmd = new SqlCommand("SP_DelTrans", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@transID", transID);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveUpdRemark(int transID, string updRemark)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_SaveUpdRemark", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@transID", transID);
                cmd.Parameters.AddWithValue("@updRemark", updRemark);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable ddlCashUnit_Bind()
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

        public DataTable GetLastOBL(int transID, int lstAccID, string lstDate, string created_Date)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_GetLastOBL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@transID", transID);
                cmd.Parameters.AddWithValue("@accID", lstAccID);
                cmd.Parameters.AddWithValue("@Date", lstDate);
                cmd.Parameters.AddWithValue("@created_Date", created_Date);

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

        public string GetLastTransDate(int accID)
        {
            try
            {
                string lastTransDate="";
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_GetLastTransDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@accID", accID);

                cmd.Connection.Open();
                DataTable dt = new DataTable();

                if (cmd.ExecuteScalar() != null)  //menus already assigned
                {
                    lastTransDate = cmd.ExecuteScalar().ToString();
                }
                else
                { }
                cmd.Connection.Close();

                return lastTransDate;
            }
            catch (Exception ex)
            {
                errDL.SaveErrLog(this.GetType().Name.Replace("_", "/"), ex.ToString());
                throw ex;
            }
        }

        public DataTable IsRoleCanEditDel(string file, string role)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_IsRoleCanEditDel", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@file", file);
                cmd.Parameters.AddWithValue("@role", role);                

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

        public DataTable AccSpectCtrl(string accID)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_AccSpectCtrl", con);
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

        public DataTable GetTransAttachs(int transID)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_GetTransAttachs", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@transID", transID);

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

        public void DeleteTransAttachment(int attachID)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_DeleteTransAttachment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@attachID", attachID);

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

        public DataTable updTransAttach(int oldTransID, int newTransID)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_updTransAttach", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@oldTransID", oldTransID);
                cmd.Parameters.AddWithValue("@newTransID", newTransID);

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

        public void DelTransAttachByTransID(int transID)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("SP_DelTransAttachByTransID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@transID", transID);

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
    }
}
