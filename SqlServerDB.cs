using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using LycheeGST_API.Models;

namespace LycheeGST_API.database_access_layer
{
    public class SqlServerDB
    {
        SqlConnection _DBcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public int ExeDML(string sql)
        {
            int noOfRows;
            _DBcon.Open();

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Connection = _DBcon;
            noOfRows = command.ExecuteNonQuery();
            _DBcon.Close();
            return noOfRows;
        }
        public string ExeSQLSingleStringVal(string sql)
        {
            _DBcon.Open();
            SqlCommand command = new SqlCommand();
            string TempStr;
            command.CommandText = sql;
            command.Connection = _DBcon;
            try
            {
                TempStr = command.ExecuteScalar().ToString();
            }
            catch (Exception err)
            {
                _DBcon.Close();
                return "";
            }
            _DBcon.Close();
            return TempStr;
        }
        public DataTable ExeSQL(string sql)
        {
            _DBcon.Open();

            DataTable RetVal = new DataTable();
            SqlDataAdapter a = new SqlDataAdapter(sql, _DBcon);
            a.Fill(RetVal);
            _DBcon.Close();
            return RetVal;
        }


    }
}