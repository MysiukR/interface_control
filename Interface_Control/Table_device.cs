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
    public partial class Table_device : Form
    {
        SFTP sftp;
        public Table_device()
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
                    // if (Value_register == 9999.9)
                    // { MessageBox.Show("До мережі не підключені вимірювальні прилади!"); }
                    // else
                    // {
                    registerData.Add(new Register(date, Device_number, Register_number, Value_register));
                    //}

                }
            }
            return registerData.ToArray();
        }
        private void Table_device_Load(object sender, EventArgs e)
        {
            SftpClient client;
            client = new SftpClient(Properties.Settings.Default.host, 22, Properties.Settings.Default.username, Properties.Settings.Default.password);

            StreamWriter write_request = new StreamWriter("Settings_register.txt");
            write_request.WriteLine("247");
            write_request.Close();

            FileStream upload_stream = new FileStream("Settings_register.txt", FileMode.Open);

            try
            {
                if (!client.IsConnected)
                    client.Connect();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (client.IsConnected)
            {
                client.ChangeDirectory("/home/pi/ModBus");
                client.UploadFile(upload_stream, "/home/pi/ModBus" + "/Settings_register.txt", null);
                client.Disconnect();
                upload_stream.Close();
            }
            else
            {
                upload_stream.Close();
            }

            string download_file_name = "Detecting_data.txt";

                try
                {
                    if (!client.IsConnected)
                        client.Connect();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (client.IsConnected)
                {
                    string working_directory = "/home/pi/ModBus";
                    string windows_file = "Download.txt";
                    bool exist = false;

                    try
                    {
                        client.ChangeDirectory(working_directory);

                        var listDirectory = client.ListDirectory(working_directory);

                        foreach (var file in listDirectory)
                        {
                            if (file.Name == download_file_name)
                            {
                                exist = true;
                                FileStream file_stream = new FileStream(windows_file, FileMode.Create);
                                client.DownloadFile(download_file_name, file_stream);
                                client.DeleteFile(download_file_name);
                                file_stream.Close();
                                MessageBox.Show("File saved in 'Download.txt'", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            else
                            {

                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    if (exist == false)
                    {
                        MessageBox.Show("Requested file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    client.Disconnect();
                }

                else
                {
                    MessageBox.Show("Connection failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
           
            dataGridView1.RowCount = 1;
            string[] x = sftp.getFile("Download.txt");
            Register[] registerData = ConverDataRegister(x);
            for (int i = 0; i < registerData.Length; i++)
            {
                dataGridView1.RowCount = dataGridView1.RowCount + 1;
                dataGridView1[0, i].Value = dataGridView1.RowCount - 1;
                dataGridView1[1, i].Value = Convert.ToString(registerData[i].Value_register.ToString());
                dataGridView1[2, i].Value = "9600";
                dataGridView1[3, i].Value = "Online";
            }
        }
    }
}
