using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.IO;

namespace Interface_Control
{
    public partial class TableDataBase : Form
    {
        MySqlHelper mySqlHelper;
        public TableDataBase()
        {
            InitializeComponent();
            mySqlHelper = new MySqlHelper();
        }
        int count = 0;
        int order_value=0;
        private void TableDataBase_Load_1(object sender, EventArgs e)
        {
            
            string myConnectionString = "SERVER=localhost;" +
              "DATABASE = MODBUS;" +
                        "UID = root;" +
                            "PASSWORD = admin;"

            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SHOW TABLES;";
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            // MessageBox.Show("" + Properties.Settings.Default.DB_database);
            string db_names = Properties.Settings.Default.DB_database;
            CultureInfo culture = CultureInfo.CurrentCulture;//For big in small letters 
            db_names = db_names.ToLower(culture);// Call ToLower instance method with globalization parameter.
            while (Reader.Read())
            {
                string row = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                    row += Reader.GetValue(i).ToString();
                treeView1.Nodes.Add(Convert.ToString(row));
                if (db_names == row)
                {
                    order_value = count;
                }
                count++;
            }
            connection.Close();


            //MySqlConnection connection1 = new MySqlConnection(myConnectionString);
            //MySqlCommand command1 = connection1.CreateCommand();
            //command1.CommandText = "SHOW TABLES;";
            //MySqlDataReader Reader1;
            //connection1.Open();
            //Reader1 = command1.ExecuteReader();
            //while (Reader1.Read())
            //{
            //    string row1 = "";
            //    for (int i = 0; i < Reader1.FieldCount; i++)
            //        row1 += Reader1.GetValue(i).ToString();
            //    treeView1.Nodes[order_value].Nodes.Add(Convert.ToString(row1));
            //}
            //connection1.Close();

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
         //   MessageBox.Show(""+node.Text);
            try
            {
                int number_function;
                int device_number;
                using (TextReader reader = File.OpenText("data1.txt"))
                {
                    number_function = int.Parse(reader.ReadLine());
                    device_number = int.Parse(reader.ReadLine());
                }
                string commandSQL = "SELECT * FROM Data;";// + node.Text + ";";//TABLE_VALUES_REGISTERS_" + "1;";//device_number + ";";
                dataGridViewTableDB.DataSource = mySqlHelper.GetTable(commandSQL);
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            //   MessageBox.Show(""+node.Text);
            try
            {
                int number_function;
                int device_number;
                using (TextReader reader = File.OpenText("data1.txt"))
                {
                    number_function = int.Parse(reader.ReadLine());
                    device_number = int.Parse(reader.ReadLine());
                }
                string commandSQL = "SELECT * FROM " + node.Text + ";";//TABLE_VALUES_REGISTERS_" + "1;";//device_number + ";";
                dataGridViewTableDB.DataSource = mySqlHelper.GetTable(commandSQL);
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }
    }
}
