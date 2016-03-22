using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DBDriver
{
    public class DBManager
    {
        /**
            This class will implemets the conections to DB
        **/
        public MySqlDataReader RS;
        public MySqlConnection DB;
        public List<MySqlParameter> Params;

        // Initiator
        public DBManager()
        {
            RS = null;
            DBConnect();
            Params = new List<MySqlParameter>();
        }

        // Config & Connection
        private string GetConfig(string Name)
        {
            //return WebConfigurationManager.AppSettings[Name].ToString();
            return string.Empty;
        }
        private void DBConnect()
        {
            if (DB == null) { DB = new MySqlConnection(); }

            if (DB.State == System.Data.ConnectionState.Closed)
            {
                // Get connectionstring from Web.Config
                MySqlConnectionStringBuilder tBuild = new MySqlConnectionStringBuilder();

                tBuild.Server = "localhost";//GetConfig("DBServer");
                tBuild.Port = 3306;//Convert.ToUInt32(GetConfig("DBPort"));
                tBuild.UserID = "root";//GetConfig("DBUser");
                tBuild.Password = "vcGN2108@";//GetConfig("DBPass");
                tBuild.Database = "enron";//GetConfig("DBDatabase");

                tBuild.Pooling = true;
                tBuild.MinimumPoolSize = 4;

                DB.ConnectionString = tBuild.ConnectionString;
                DB.Open();
            }
        }

        // Execution
        public int DBExecute(string SQL, bool UseParams)
        {
            DBConnect();

            int y = 0;
            bool tDone;
            MySqlCommand tCmd = new MySqlCommand(SQL, DB);

            if (UseParams == true && Params != null)
            {
                for (int x = 0; x < Params.Count; x++)
                {
                    tCmd.Parameters.Add(Params[x]);
                }
            }

            // Prepare
            DBCloseRecordset();

            try
            {
                y = tCmd.ExecuteNonQuery();
                tDone = true;
            }
            catch (Exception)
            {
                // Error executing
                // Reconnect, and try again
                DB = null;
                DBConnect();

                tDone = false;
            }

            if (tDone == false)
            {
                // Try again
                y = tCmd.ExecuteNonQuery();
            }

            if (UseParams == true)
            { DBClearParams(); }

            return y;
        }
        public void DBExecuteRecordset(string SQL, bool UseParams)
        {
            DBConnect();

            int y = 0;
            bool tDone;
            MySqlCommand tCmd = new MySqlCommand(SQL, DB);

            if (UseParams == true && Params != null)
            {
                for (int x = 0; x < Params.Count; x++)
                {
                    tCmd.Parameters.Add(Params[x]);
                }
            }

            // Prepare
            DBCloseRecordset();

            try
            {
                RS = tCmd.ExecuteReader();
                tDone = true;
            }
            catch (Exception)
            {
                // Error executing
                // Reconnect, and try again
                DB = null;
                DBConnect();

                tDone = false;
            }

            if (tDone == false)
            {
                // Try again
                RS = tCmd.ExecuteReader();
            }

            if (UseParams == true)
            { DBClearParams(); }
        }
        public object DBExecuteRecordsetReturnOneField(string SQL, bool UseParams, string Field)
        {
            DBExecuteRecordset(SQL, UseParams);

            object tResult = null;
            if (RS.HasRows == true)
            {
                RS.Read();
                tResult = RS[Field];
            }

            DBCloseRecordset();

            return tResult;
        }

        public bool DBRowExists(string SQL, bool UseParams)
        {
            DBExecuteRecordset(SQL, UseParams);

            bool tResult = RS.HasRows;

            DBCloseRecordset();

            return tResult;
        }

        // Parameters
        public void DBClearParams()
        {
            Params = null;
        }
        public void DBAddParam(string Name, object Value, MySql.Data.MySqlClient.MySqlDbType Type)
        {
            if (Params == null) { Params = new List<MySqlParameter>(); }

            MySqlParameter t;

            t = new MySqlParameter(Name, Type);
            t.Value = Value;

            Params.Add(t);
        }

        public void DBCloseRecordset()
        {
            try
            { RS.Close(); }
            catch (Exception)
            { RS = null; }
        }
    }


}
