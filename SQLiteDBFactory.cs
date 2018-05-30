using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using System.IO;

namespace DasTec.Factory.SQLite
{
    public class SQLiteDBFactory
    {
        public SQLiteConnection _DBcon = new SQLiteConnection();
        public bool isOpen { get; set; }
        public string company { get; set; }
        public string financialYear { get; set; }
        
        public readonly string DBConnectionString = ConfigurationManager.ConnectionStrings["SQLiteDBConnection"].ToString();


        public Boolean isFirstCreatedDb = false;
        public SQLiteDBFactory()
        {
 
                 
                _DBcon.ConnectionString = DBConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
                _DBcon.ConnectionString = DBConnectionString.Replace("{ProgramDir}", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)); 

            isOpen = false;
        }
        public SQLiteDBFactory(string ConnectString)
        {
            ConnectString = ConnectString.Replace("{ProgramDir}", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            ConnectString = ConnectString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            _DBcon.ConnectionString = ConnectString;
            DBConnectionString = ConnectString;
            isOpen = false;
        }

        ~SQLiteDBFactory()
        {
            if (isOpen == true)
            {
                _DBcon.Close();
            }
            
            
        }

        public void Open()
        {
            if (isOpen == true)
            {
                _DBcon.Close();
            }
            _DBcon.Open();
            isOpen = true;
        }
        public void Close()
        {
            _DBcon.Close();
            isOpen = false;
        }
        public string ExeSQLSingleStringVal(string sql)
        {
            Open();
            SQLiteCommand command = new SQLiteCommand();
            string TempStr;
            command.CommandText = sql;
            command.Connection = _DBcon;
            try
            {
                TempStr = command.ExecuteScalar().ToString();
            }
            catch (Exception err)
            {
                Close();
                return "";
            }
            Close();
            return TempStr;
        }

        public int ExeDML(string sql)
        {
            int noOfRows;
            Open();
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = sql;
            command.Connection = _DBcon;
            noOfRows = command.ExecuteNonQuery();
            Close();
            return noOfRows;
        }

        public DataTable ExeSQL(string sql)
        {
            Open();
            DataTable RetVal = new DataTable();
            SQLiteDataAdapter a = new SQLiteDataAdapter(sql, _DBcon);
            a.Fill(RetVal);
            Close();
            return RetVal;
        }


        public int ReadyFirsDB()
        {
            String scriptPath = ConfigurationManager.AppSettings["DbScriptDir"].ToString();
            scriptPath = scriptPath.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            string sqldata = File.ReadAllText(scriptPath+"Schema.sql");
            return ExeDML(sqldata);
        }
    }
}
