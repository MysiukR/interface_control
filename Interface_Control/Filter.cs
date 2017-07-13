using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace Interface_Control
{
    public partial class Filter : Form
    {
        MySqlHelper mySqlHelper;
        public Filter()
        {
            InitializeComponent();
            mySqlHelper = new MySqlHelper();
        }

        private string paramFilter { get; set; }

        private void setFilter(string commandSQL)
        {
            DataTable dt = mySqlHelper.GetTable(commandSQL);
            int M = dt.Columns.Count;
            int N = dt.Rows.Count;
            if (N == 0 && M == 0)
            {
                MessageBox.Show("Дані на цей день відсутні.");
            }
            else
            {
                string result = "";
                result = "\nДані з параметром " + paramFilter + ":\n"
                    + "День: " + String.Format("{0:dd/MM/yyyy}", dt.Rows[0][0]) + "\n";
                result += "Device_number: " + Math.Round(Convert.ToDouble(dt.Rows[0][1]), 1).ToString() + " \n";
                result += "Register_number: " + Math.Round(Convert.ToDouble(dt.Rows[0][2]), 1).ToString() + " \n";
                result += "Value_register: " + Math.Round(Convert.ToDouble(dt.Rows[0][3]), 1).ToString() + " \n";
                richTextBox1.AppendText(result);
            }
        }

        private void radioButtonMax_CheckedChanged_1(object sender, EventArgs e)
        {
            paramFilter = "MAX";
        }

        private void radioButtonMin_CheckedChanged_1(object sender, EventArgs e)
        {
            paramFilter = "MIN";
        }

        private void radioButtonAvg_CheckedChanged_1(object sender, EventArgs e)
        {
            paramFilter = "AVG";
        }

        private void Filter_Load_1(object sender, EventArgs e)
        {
            paramFilter = "MAX";
        }

        private void monthCalendar1_DateSelected_1(object sender, DateRangeEventArgs e)
        {
            try
            {

                int number_function;
                int device_number;
                using (TextReader reader = File.OpenText("data1.txt"))
                {
                    number_function = int.Parse(reader.ReadLine());
                    device_number = int.Parse(reader.ReadLine());
                }
                string SelectStart = e.Start.ToString("yyy-MM-dd");
                string SelectEnd = e.End.ToString("yyy-MM-dd");
                labelData.Text = SelectStart + "  ---  " + SelectEnd;
                string commandSQL = "SELECT Day_and_time, " + paramFilter + "(Device_number), " + paramFilter + "(Register_number), "
                    + paramFilter + "(Value_register) " + "FROM "+ Properties.Settings.Default.DB_table + "_" + device_number + " WHERE DATE(Day_and_time) BETWEEN '"
                    + SelectStart + "' AND '" + SelectEnd + "';";
                setFilter(commandSQL);
            }
            catch (Exception ex)
            {
                mySqlHelper = new MySqlHelper();
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonClear_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}
