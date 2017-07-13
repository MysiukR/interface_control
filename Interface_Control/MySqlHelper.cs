using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace Interface_Control
{
    class MySqlHelper
    {
        string MySQL_host = "localhost";
        string MySQL_port = "3306";
        string MySQL_uid = Properties.Settings.Default.DB_username;
        string MySQL_pw = Properties.Settings.Default.DB_passwd;
        string NameDataBase =Properties.Settings.Default.DB_database;
        string NameTable = Properties.Settings.Default.DB_table;
        public bool IsDataBase;

        MySqlConnection Connection;
        MySqlCommand Query;
        MySqlDataReader Reader;
        DataTable dataTable;

        private void readSettings()
        {
            MySQL_uid = Properties.Settings.Default.DB_username;
            MySQL_pw = Properties.Settings.Default.DB_passwd;
            NameDataBase =  Properties.Settings.Default.DB_database;
            NameTable = Properties.Settings.Default.DB_table;
        }

        public MySqlHelper()
        {    
            MySQL_host = "localhost";
            MySQL_port = "3306";
            readSettings();
    
            IsDataBase = false;

            Connection = new MySqlConnection("Data Source=" + MySQL_host + ";Port=" + MySQL_port + ";User Id=" + MySQL_uid + ";Password=" + MySQL_pw + ";");
            Query = new MySqlCommand();
            Query.Connection = Connection;
        }

        public void CreateTable(int Device_number)
        {
            try
            {
                Connection.Open();

                Query.CommandText = "CREATE DATABASE IF NOT EXISTS `" + NameDataBase + "`;";
                Query.ExecuteNonQuery();

                Query.CommandText = "USE " + NameDataBase + ";";
                Query.ExecuteNonQuery();

                Query.CommandText = "CREATE TABLE IF NOT EXISTS `" + NameTable + "_" + Device_number + "` (`ID` int(10) AUTO_INCREMENT,"
                        + "`Day_and_time` DATETIME NOT NULL, `Device_number` FLOAT NOT NULL,"
                        + "`Register_number` FLOAT NOT NULL, `Value_register` FLOAT NOT NULL,"
                        + " PRIMARY KEY(ID));";
                Query.ExecuteNonQuery();

                IsDataBase = true;


                Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                Connection.Close();
            }
        }

        public void ClearDataBase(int Device_number)
        {
            try
            {
                Connection.Open();

                Query.CommandText = "DROP TABLE `" + NameTable + "_" + Device_number + "`;";
                Query.ExecuteNonQuery();

                Connection.Close();

                CreateTable(Device_number);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                Connection.Close();
            }
        }

        public DataTable GetTable(string commandText)
        {
            dataTable = new DataTable();
            try
            {
                Connection.Open();
                Query.CommandText = "USE " + NameDataBase + ";";
                Query.ExecuteNonQuery();

                Query.CommandText = commandText;
                Reader = Query.ExecuteReader();
                if (Reader.HasRows)
                    dataTable.Load(Reader);

                Connection.Close();
            }
            catch (MySqlException ex)
            {
                Connection.Close();
                MessageBox.Show(ex.Message);
            }
            return dataTable;
        }

        public void WriteData(DateTime date, double Device_number, double Register_number, double Value_register)
        {
            try
            {
                Connection.Open();
                Query.CommandText = "USE " + NameDataBase + ";";
                Query.ExecuteNonQuery();

                Query.CommandText = "INSERT INTO " + NameTable + "_" + Device_number + "(Day_and_time, device_number, register_number, Value_register) VALUES (?day, ?Device_number, ?Register_number, ?Value_register);";
                Query.Parameters.Add("?day", MySqlDbType.DateTime).Value = date;//day
                Query.Parameters.Add("?Device_number", MySqlDbType.Float).Value = Device_number;
                Query.Parameters.Add("?Register_number", MySqlDbType.Float).Value = Register_number;
                Query.Parameters.Add("?Value_register", MySqlDbType.Float).Value = Value_register;
                Query.ExecuteNonQuery();
                Query.Parameters.Clear();
                Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                Connection.Close();
            }
        }

    }
}
