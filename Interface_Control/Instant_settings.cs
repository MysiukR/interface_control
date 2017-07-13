using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Renci.SshNet;


namespace Interface_Control
{
    public partial class Instant_settings : Form
    {
        SFTP sftp;
        public Instant_settings()
        {
            InitializeComponent();
            sftp = new SFTP();
        }
        private Register[] ConverDataRegister(string[] buffFile)
        {
            List<Register> registerData = new List<Register>();
            registerData.Clear();
            DateTime date;
            double Device_number;
            double Register_number;
            double Value_register;

            for (int i = 0; i < buffFile.Length; i++)
            {
                int count = 0;
                int countNoNull = 0;
                string[] temp0 = buffFile[i].Split('|');
                for (int j = 1; j < temp0.Length; j++)
                {
                    if (String.IsNullOrEmpty(temp0[j]))
                        count++;
                    else countNoNull++;
                }

                if (count > 0 || countNoNull < 4)
                    continue;
                else
                {
                    Device_number = Convert.ToDouble(buffFile[i].Split('|')[1], CultureInfo.InvariantCulture);
                    Register_number = Convert.ToDouble(buffFile[i].Split('|')[2], CultureInfo.InvariantCulture);
                    Value_register = Convert.ToDouble(buffFile[i].Split('|')[3], CultureInfo.InvariantCulture);

                    string temp = buffFile[i].Split('|')[4];
                    int[] dateTemp = new int[temp.Split('-').Length];
                    for (int j = 0; j < dateTemp.Length; j++)
                    {
                        dateTemp[j] = Convert.ToInt32(temp.Split('-')[j]);
                    }
                    date = new DateTime(dateTemp[0], dateTemp[1], dateTemp[2], dateTemp[3], dateTemp[4], dateTemp[5]);
                     if (Value_register == 9999.9)
                     { MessageBox.Show("До мережі не підключені вимірювальні прилади!"); }
                     else
                     {
                    registerData.Add(new Register(date, Device_number, Register_number, Value_register));
                    }

                }
            }
            return registerData.ToArray();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text == String.Empty)
            {
                MessageBox.Show("Please select IP-adress,username and password");

            }
            else
            {

                if (comboBox1.SelectedIndex == 0)
                {

                    int device_number = Convert.ToInt32(numericUpDown1.Value);
                    int register_number = Convert.ToInt32(numericUpDown2.Value);
                    int value_register = Convert.ToInt32(textBox1.Text);

                    DialogResult result = MessageBox.Show("Ви обрали: " + "\nФункцію: 1" + "\nНомер приладу:" + device_number + "\nНомер регістру:" + register_number + "\nЗначення:" + value_register + "\n", "Data_information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No)
                    { }
                    else { Close(); }

                    string text = "255\n" + "1" + "\n" + device_number + "\n" + register_number + "\n" + value_register + "\n";
                    System.IO.File.WriteAllText(@"data1.txt", text);
                }
                if (comboBox1.SelectedIndex == 1)
                {

                    int device_number = Convert.ToInt32(numericUpDown1.Value);
                    int register_number = Convert.ToInt32(numericUpDown2.Value);


                    DialogResult result = MessageBox.Show("Ви обрали: " + "\nФункцію: 2" + "\nНомер приладу:" + device_number + "\nНомер регістру:" + register_number + "\n", "Data_information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No)
                    { }
                    else { Close(); }

                    string text = "255\n" + "2" + "\n" + device_number + "\n" + register_number + "\n";
                    System.IO.File.WriteAllText(@"data1.txt", text);
                }
            }
            string upload_filename = "data1.txt";
            string remove_filename = "Settings_register.txt";
            sftp.UploadFile(upload_filename, remove_filename);
            //  string upload_filename = "data1.txt";
            //  string remove_filename = "Settings_register.txt";
            //  sftp.UploadFile(upload_filename, remove_filename);
            //Thread.Sleep(19000);
            //sftp.DownloadFile("Instant_data.txt", "data2.txt");
            //string[] x = sftp.getFile("data2.txt");
            //Register[] instant_data = ConverDataRegister(x);
            //if (instant_data.Length == 0)
            //    return;
            //label5.Text = instant_data[instant_data.Length - 1].Value_register.ToString();
            //sftp.DeleteFile("Instant_data.txt");
        }

        private void Instant_settings_Load(object sender, EventArgs e)
        {
            textBox1.Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == String.Empty)
            {
                textBox1.Visible = false;
            }
            else
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    textBox1.Visible = true;
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    textBox1.Visible = false;
                }
            }
        }
    }
}
