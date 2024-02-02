using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace _3rdEyE.ManagingTools
{
    public class OracleGateInOut_DBHelper
    {
        public static string ConStr;

        public OracleGateInOut_DBHelper()
        {
            ConStr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleGateInOut"].ConnectionString;
        }


        //22 - 12 - 2020 kayesh ===> Execute Sql Non Query Command With Rollback Function
        //file version v.0.0.1
        public static bool DbSaveChanges(List<string> SqlCommandList)
        {
            if (string.IsNullOrWhiteSpace(ConStr))
            {
                new OracleGateInOut_DBHelper();
            }
            bool result = true;
            OracleConnection con = new OracleConnection(ConStr);
            OracleCommand cmd = new OracleCommand();
            OracleTransaction trn;
            con.Open();
            trn = con.BeginTransaction();
            try
            {
                foreach (string sql_command in SqlCommandList)
                {
                    cmd.CommandText = sql_command;
                    cmd.Connection = con;
                    cmd.Transaction = trn;
                    cmd.ExecuteNonQuery();
                }
                trn.Commit();
                result = true;
            }
            catch (OracleException ex)
            {
                string exceptions = ex.Message;
                trn.Rollback();
                result = false;
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        //example insert into (my_id,my_name) values('1','kayesh') RETURNING my_id INTO :my_id_param
        //return value data type is : Int64 or BigInt
        public Int64 DbSaveChangesOut(string Sql_Command)
        {
            if (string.IsNullOrWhiteSpace(ConStr))
            {
                new OracleGateInOut_DBHelper();
            }
            Int64 result = 0;
            OracleConnection con = new OracleConnection(ConStr);
            OracleCommand cmd = new OracleCommand();
            OracleTransaction trn;
            con.Open();
            trn = con.BeginTransaction();
            try
            {
                cmd.CommandText = Sql_Command;
                cmd.Connection = con;
                cmd.Transaction = trn;
                OracleParameter outputParameter = new OracleParameter("my_id_param", OracleDbType.Decimal);
                outputParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParameter);
                cmd.ExecuteNonQuery();
                result = Convert.ToInt64(outputParameter.Value.ToString());
                trn.Commit();
            }
            catch (OracleException ex)
            {
                string exceptions = ex.Message;
                trn.Rollback();
                result = 0;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public static DataTable ExecuteSelectCommand(string select_Command)
        {
            if (string.IsNullOrWhiteSpace(ConStr))
            {
                new OracleGateInOut_DBHelper();
            }
            OracleConnection con = new OracleConnection(ConStr);
            DataTable dataTable = new DataTable();
            try
            {
                con.Open();
                OracleCommand cmd = new OracleCommand(select_Command);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                {
                    dataAdapter.SelectCommand = cmd;
                    dataAdapter.Fill(dataTable);
                }
            }
            catch (OracleException ex)
            {
                string exceptions = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return dataTable;
        }
    }
}