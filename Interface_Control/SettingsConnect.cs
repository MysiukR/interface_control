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
    public partial class SettingsConnect : Form
    {
         string host;
         string userName;
         string password;
      
        public SettingsConnect()
        {
            InitializeComponent();
            host = Properties.Settings.Default.host;
            userName = Properties.Settings.Default.username;
            password = Properties.Settings.Default.password;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            host = textBoxHost.Text;
            userName = textBoxUserName.Text;
            password = textBoxPassword.Text;
            Properties.Settings.Default.host = host;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.username = userName;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.password = password;
            Properties.Settings.Default.Save();
           // MessageBox.Show("Щоб зберегти налаштування перезавантажте програму!");
            Properties.Settings.Default.Reload();
            Close();
            
        }

        private void SettingsConnect_Load_1(object sender, EventArgs e)
        {
            textBoxHost.Text = host;
            textBoxUserName.Text = userName;
            textBoxPassword.Text = password;
            
        }
    }
}
