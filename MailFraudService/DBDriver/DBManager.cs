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
        string conString = "";
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
        public void DBConnect()
        {
            if (DB == null) { DB = new MySqlConnection(); }

            if (DB.State == System.Data.ConnectionState.Closed)
            {
                try {
                    // Get connectionstring from Web.Config
                    MySqlConnectionStringBuilder tBuild = new MySqlConnectionStringBuilder();

                    tBuild.Server = "localhost";//GetConfig("DBServer");
                    tBuild.Port = 3306; //Convert.ToUInt32(GetConfig("DBPort"));
                    tBuild.UserID = "root";// GetConfig("DBUser");
                    tBuild.Password = "Shlomo"; //GetConfig("DBPass");
                    tBuild.Database = "enron";// GetConfig("DBDatabase");

                    tBuild.Pooling = true;
                    tBuild.MinimumPoolSize = 30;

                    DB.ConnectionString = tBuild.ConnectionString;
                    DB.Open();
                }
                catch(Exception e)
                {

                }
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
            catch (Exception e)
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
            catch (Exception e) 
            {
                RS = null;
            }
        }
    }





    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "enron";
            uid = "root";
            password = "vcGN2108@";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {

            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert(string target,string values)
        {
            string query = "INSERT INTO {0} VALUES({1})";

            if(string.IsNullOrEmpty(target) || string.IsNullOrEmpty(values))
            {
                //error message
                Console.WriteLine("Arguments for Insert are not validated");
                return;
            }
            query = string.Format(query,target,values);
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update(string target,string values,string byCondition)
        {
            string query = "UPDATE {0} SET {1} WHERE {2}";

            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(values) || string.IsNullOrEmpty(byCondition))
            {
                //error message
                Console.WriteLine("Arguments for Update are not validated");
                return;
            }
            query = string.Format(query, target, values, byCondition);
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete(string target,string byCondition)
        {
            string query = "DELETE FROM {0} WHERE {1}";

            if (string.IsNullOrEmpty(target) ||  string.IsNullOrEmpty(byCondition))
            {
                //error message
                Console.WriteLine("Arguments for Delete are not validated");
                return;
            }
            query = string.Format(query, target, byCondition);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public DataTable Select(string target)
        {
            string query = "SELECT * FROM {0}" ;
            if (string.IsNullOrEmpty(target))
            {
                //error
                return null;
            }
            query = string.Format(query, target);
            //Create a list to store the result
            DataTable dt = null;

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list

                if (dataReader.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(dataReader, LoadOption.OverwriteChanges);
                }
                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return dt;
            }
            else
            {
                return dt;
            }
        }

        //Count statement
        public int Count(string tableName)
        {
            string query = "SELECT Count(*) FROM {0}";
            if (string.IsNullOrEmpty(tableName))
            {
                return -1;
            }

            query = string.Format(query, tableName);
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
    }
}
