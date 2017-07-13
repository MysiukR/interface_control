using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface_Control
{
    public partial class DB_Connect : Form
    {       string DB_username;
            string DB_passwd;
            string DB_name;
            string DB_table;
        public DB_Connect()
        {
            InitializeComponent();
            DB_username = Properties.Settings.Default.DB_username;
            DB_passwd = Properties.Settings.Default.DB_passwd;
            DB_name = Properties.Settings.Default.DB_database;
            DB_table = Properties.Settings.Default.DB_table;
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            DB_username = textBox_DB_un.Text;
            DB_passwd = textBox_passwd.Text;
            DB_name = textBox_DB_name.Text;
            DB_table = textBox_DB_Table.Text;
            Properties.Settings.Default.DB_username = DB_username;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.DB_passwd = DB_passwd;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.DB_database = DB_name;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.DB_table = DB_table;
            Properties.Settings.Default.Save();
           // MessageBox.Show("Щоб зберегти налаштування перезавантажте програму!");
            Properties.Settings.Default.Reload();
            Close();
        }

        private void DB_Connect_Load(object sender, EventArgs e)
        {
           
            textBox_DB_un.Text = DB_username ;
           textBox_passwd.Text = DB_passwd ;
            textBox_passwd.PasswordChar = '*';
           textBox_DB_name.Text = DB_name;
           textBox_DB_Table.Text = DB_table;
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {  textBox_passwd.PasswordChar = '\0';}
            else {  textBox_passwd.PasswordChar = '*'; }
        }
    }
}
