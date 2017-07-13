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
using System.Windows.Forms.DataVisualization.Charting;

namespace Interface_Control
{
    public partial class Form1 : Form
    {
        MySqlHelper mySqlHelper;
        SFTP sftp;
        TableDataBase tableDataBase;
        Filter filter;
        SettingsConnect settingsConnect;
        Settings_functions_registers Settings_functions_registers;
        Instant_settings Instant_settings;
        Table_device Table_device;
        DB_Connect DB_Connect;
        int intervalTimerSecund;
        bool Indicator = false;

        public Form1()
        {
            InitializeComponent();
            mySqlHelper = new MySqlHelper();
            sftp = new SFTP();
        }

        private void ShowGraphic(Chart chart, DateTime[] x, double[] y, string titleX, string titleY)
        {
            chart.Series[0].Points.Clear();
            chart.Series[0].ChartType = SeriesChartType.Spline;
            chart.ChartAreas[0].AxisX.Title = titleX;
            chart.ChartAreas[0].AxisY.Title = titleY;
            chart.Series[0].Color = Color.Blue;
            //chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            //chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            for (int i = 0; i < x.Length; i++)
            {
                chart.Series[0].Points.AddXY(String.Format("{0:HH:mm\ndd/MM/yyyy}", x[i]), y[i]);
            }
        }

        private void UpdateGraphic_Date(string commandSQL, int row_date, int row_other_line)
        {
            DataTable dt = mySqlHelper.GetTable(commandSQL);

            int M = dt.Columns.Count;
            int N = dt.Rows.Count;

            DateTime[] x = new DateTime[N];
            double[] y = new double[N];

            for (int i = 0; i < N; i++)
            {
                x[i] = Convert.ToDateTime(dt.Rows[i][row_date]);
                y[i] = Convert.ToInt32(dt.Rows[i][row_other_line]);
            }

            ShowGraphic(chart1, x, y, "Дата: " + String.Format("{0:dd/MM/yyyy}", x[0]), "");
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

                    Value_register = Value_register / 10;

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

        public int GetFile()
        {
            int number_function;
            int device_number;
            using (TextReader reader = File.OpenText("data1.txt"))
            {
                number_function = int.Parse(reader.ReadLine());
                device_number = int.Parse(reader.ReadLine());
            }
            return device_number;
        }

        private void Process()
        {  
            string pathFile = "data.txt";
            label5.Text = "Online";
            label5.ForeColor = Color.Green;
            //sftp.DownloadFile("Register_data.txt",pathFile);
            string nameFile = "device_" + GetFile() + ".txt";
            sftp.DownloadFile(nameFile, pathFile);
            string[] x = sftp.getFile(pathFile);
            sftp.DeleteFile(nameFile);
            Register[] registerData = ConverDataRegister(x);

            if (registerData.Length == 0)
                return;
            //for (int i = 0; i < registerData.Length; i++)
            //{
            //    richTextBox1.Text ="Пристрій №"+ registerData[i].Device_number.ToString() + " Регістр №" + registerData[i].Register_number.ToString() + " Значення: "+registerData[i].Value_register.ToString() + " Дата: " + registerData[i].date.ToString() + "\n";
            //}
            //label_t_air.Text = registerData[registerData.Length - 1].Device_number.ToString();
            //label_t_ground.Text = registerData[registerData.Length - 1].Register_number.ToString();
            //label_humidity.Text = registerData[registerData.Length - 1].Value_register.ToString();
            //label_press.Text = registerData[registerData.Length - 1].date.ToString();
            toolStripStatusLabel2.Text = "Дані з приладу завантажено...";
            FileStream download_stream = new FileStream("Data_result.txt", FileMode.Append);
            StreamWriter data_result = new StreamWriter(download_stream);
            for (int i = 0; i < registerData.Length; i++)
            {
                data_result.WriteLine(registerData[i].date + " " + registerData[i].Device_number + " " + registerData[i].Register_number + " " + registerData[i].Value_register + "\n");
            }
            data_result.Close();
            for (int i = 0; i < registerData.Length; i++)
            {
                mySqlHelper.WriteData(registerData[i].date, registerData[i].Device_number, registerData[i].Register_number, registerData[i].Value_register);
            }

            toolStripStatusLabel2.Text = "Дані з приладу збережено в базу даних. Зібрано " + registerData.Length + " даних.";

            DateTime dt = registerData[registerData.Length - 1].date;
            string strDate = dt.ToString("yyy-MM-dd");
            ///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!/registerData[registerData.Length - 1].Device_number;
            string commandSQL = "SELECT * FROM "+ Properties.Settings.Default.DB_table + "_" + GetFile() + " WHERE DATE(Day_and_time) = '" + strDate + "';";
            UpdateGraphic_Date(commandSQL, 1, 4);
            
        }

        private void monthCalendar1_DateSelected_1(object sender, DateRangeEventArgs e)
        {
            try
            {
                // int first_number;
                // int two_number;
                // int three_number;
                // using (TextReader reader = File.OpenText("data1.txt"))
                //{//255 first or 2
                //     first_number = int.Parse(reader.ReadLine());
                //     two_number = int.Parse(reader.ReadLine());
                //     three_number = int.Parse(reader.ReadLine());
                // }
                if (radioButton1.Checked == true)
                {
                    int device_number = Convert.ToInt32(numericUpDown4.Value);
                    string SelectStart = e.Start.ToString("yyy-MM-dd");
                    string SelectEnd = e.End.ToString("yyy-MM-dd");
                    //щоб побудувало графік змінити треба назву таблиці
                    string commandSQL = "SELECT * FROM "+ Properties.Settings.Default.DB_table + "_" + device_number + " WHERE DATE(Day_and_time) BETWEEN '" + SelectStart + "' AND '" + SelectEnd + "';";
                    UpdateGraphic_Date(commandSQL, 1, 4);
                }
                else if (radioButton2.Checked == true)
                {
                    string start_time = Convert.ToString(numericUpDown2.Value) + ":00";
                    string finish_time = Convert.ToString(numericUpDown3.Value) + ":00";
                    int device_number = Convert.ToInt32(numericUpDown4.Value);
                    string SelectStart = e.Start.ToString("yyy-MM-dd") + " " + start_time;
                    string SelectEnd = e.End.ToString("yyy-MM-dd") + " " + finish_time;
                    string commandSQL = "SELECT * FROM "+ Properties.Settings.Default.DB_table + "_" + device_number + " WHERE Day_and_time BETWEEN '" + SelectStart + "' AND '" + SelectEnd + "';";
                    UpdateGraphic_Date(commandSQL, 1, 4);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
        }
        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                intervalTimerSecund = Convert.ToInt32(numericUpDown1.Value * 60);
                timer1.Stop();
                timer1.Interval = intervalTimerSecund * 1000;
                timer1.Start();
                toolStripProgressBar1.Maximum = intervalTimerSecund;
                toolStripProgressBar1.Value = 0;
                timer2.Start();
            }
            else
            {
                intervalTimerSecund = Convert.ToInt32(numericUpDown1.Value * 60);
                timer1.Interval = intervalTimerSecund * 1000;
            }
        }

        private void backgroundWorker_DoWork_1(object sender, DoWorkEventArgs e)
        {
            timer2.Stop();
            timer1.Stop();
            // toolStripStatusLabel2.Text = "З'єднання з сервером...";
            e.Result = sftp.InitializeSFTP();
            
        }

        private void backgroundWorker_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker.CancelAsync();
            if ((bool)e.Result)
            {
                toolStripStatusLabel2.Text = "Завантаження даних...";
                try
                {
                    Process();
                }
                catch (Exception ex)
                {
                   
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                label5.Text = "Offline";
                label5.ForeColor = Color.Red;
                toolStripStatusLabel2.Text = "З'єднання з сервером відсутнє.";
            }

            timer1.Start();
            toolStripProgressBar1.Maximum = intervalTimerSecund;
            toolStripProgressBar1.Value = 0;
            timer2.Start();

        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            toolStripProgressBar1.Value++;

            if (toolStripProgressBar1.Value == 3)
                toolStripStatusLabel1.Text = "";
        }

        private void Form1_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Maximum = 100;
            toolStripProgressBar1.Value = 100;
            backgroundWorker.RunWorkerAsync();
            timer2.Stop();
            timer1.Stop();
        }

        private void setFilterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            filter = new Filter();
            filter.ShowDialog();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            int device_number=GetFile();
            mySqlHelper.CreateTable(device_number);
            intervalTimerSecund = Convert.ToInt32(numericUpDown1.Value * 60);

            timer1.Interval = intervalTimerSecund * 1000;
        

             
               
        }

        private void HelpToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("База даних для роботи з регістрами мікроконтролера.\n   2016 рік\nРозробники: Сасовець Ігор і Мисюк Роман\nEmail: is487@ukr.net,mysyukr@ukr.net");
        }

        private void ShowDataBaseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            tableDataBase = new TableDataBase();
            tableDataBase.ShowDialog();
        }

        private void ClearDataBaseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int device_number=GetFile();
            mySqlHelper.ClearDataBase(device_number);
            toolStripStatusLabel2.Text = "База даних порожня...";
        }

   

        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Indicator = true;
            settingsConnect = new SettingsConnect();
            settingsConnect.ShowDialog();

            toolStripProgressBar1.Maximum = 100;
            toolStripProgressBar1.Value = 100;
            backgroundWorker.RunWorkerAsync();

           
                ////Date
                //string upload_filename_date = "Date.txt";
                //string remove_filename_date = "date.txt";
                //DateTime date1 = DateTime.Now;
                //int year_now = date1.Year - 2000;
                //int second_now = date1.Second + 5;
                //string text_1 = "0" + date1.Month + "" + date1.Day + "" + date1.Hour + "" + date1.Minute + "" + year_now + "." + second_now;
                //System.IO.File.WriteAllText(@"Date.txt", text_1);
                //sftp.UploadFile(upload_filename_date, remove_filename_date);
                ////


        }

        private void Settings_function_register_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Indicator == false)
            {
                MessageBox.Show("Немає з'єднання! \n Спершу встановіть з'єднання!");
            }
            else
            {
                Settings_functions_registers = new Settings_functions_registers();
                Settings_functions_registers.ShowDialog();
     
                //Thread.Sleep(3000);
                //sftp.DeleteFile(remove_filename_1);
                string upload_filename = "data1.txt";
                string remove_filename = "Settings_register.txt";
                sftp.UploadFile(upload_filename, remove_filename);
                //Date
                string upload_filename_date = "Date.txt";
                string remove_filename_date = "date.txt";
                DateTime date1 = DateTime.Now;
                string year_now = Convert.ToString(date1.Year - 2000);
                string second_now = Convert.ToString(date1.Second + 5);
                string month_now = Convert.ToString(date1.Month);
                string day_now = Convert.ToString(date1.Day);
                string hour_now = Convert.ToString(date1.Hour);
                string min_now = Convert.ToString(date1.Minute);
                if (Convert.ToInt32(month_now) < 10) { month_now = "0" + month_now; }
                if (Convert.ToInt32(day_now) < 10) { day_now = "0" + day_now; }
                if (Convert.ToInt32(hour_now) < 10) { hour_now = "0" + hour_now; }
                if (Convert.ToInt32(min_now) < 10) { min_now = "0" + min_now; }
                string text_1 = "" + month_now + "" + day_now + "" + hour_now + "" + min_now + "" + year_now + "." + second_now;
                System.IO.File.WriteAllText(@"Date.txt", text_1);
                sftp.UploadFile(upload_filename_date, remove_filename_date);
                //
            }
        }

        private void Connect_with_DB_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB_Connect = new DB_Connect();
            DB_Connect.ShowDialog();
        }

        private void Instant_reading_register_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Indicator == false)
            {
                MessageBox.Show("Немає з'єднання! \n Спершу встановіть з'єднання!");
            }
            else
            {
         
                Instant_settings = new Instant_settings();
                Instant_settings.ShowDialog();
                string upload_filename = "data1.txt";
                string remove_filename = "Settings_register.txt";
                sftp.UploadFile(upload_filename, remove_filename);
                Thread.Sleep(10000);
                sftp.DownloadFile("Instant_data.txt", "data2.txt");
                string[] x = sftp.getFile("data2.txt");
                Register[] instant_data = ConverDataRegister(x);
                if (instant_data.Length == 0)
                    return;
                // MessageBox::Show(""+)
                //label5.Text = instant_data[instant_data.Length - 1].Value_register.ToString();
                MessageBox.Show("Пристрій: "+instant_data[instant_data.Length - 1].Device_number+";\nРегістр:"+ instant_data[instant_data.Length - 1].Register_number+";\nЗначення: " + instant_data[instant_data.Length - 1].Value_register);
                sftp.DeleteFile("Instant_data.txt");
                //Date
                string upload_filename_date = "Date.txt";
                string remove_filename_date = "date.txt";
                DateTime date1 = DateTime.Now;
                string year_now = Convert.ToString(date1.Year - 2000);
                string second_now = Convert.ToString(date1.Second + 5);
                string month_now = Convert.ToString(date1.Month);
                string day_now = Convert.ToString(date1.Day);
                string hour_now = Convert.ToString(date1.Hour);
                string min_now = Convert.ToString(date1.Minute);
                if (Convert.ToInt32(month_now) < 10) { month_now = "0" + month_now; }
                if (Convert.ToInt32(day_now) < 10) { day_now = "0" + day_now; }
                if (Convert.ToInt32(hour_now) < 10) { hour_now = "0" + hour_now; }
                if (Convert.ToInt32(min_now) < 10) { min_now = "0" + min_now; }
                string text_1 = "" + month_now + "" + day_now + "" + hour_now + "" + min_now + "" + year_now + "." + second_now;
                System.IO.File.WriteAllText(@"Date.txt", text_1);
                sftp.UploadFile(upload_filename_date, remove_filename_date);
                //
            }
            //if (Indicator == false)
            //{
            //    MessageBox.Show("Немає з'єднання! \n Спершу встановіть з'єднання!");
            //}
            //else
            //{
            //    Instant_settings = new Instant_settings();
            //    Instant_settings.ShowDialog();
            //    //string upload_filename = "data1.txt";
            //    //string remove_filename = "Settings_register.txt";
            //    //sftp.UploadFile(upload_filename, remove_filename);
            //    //Thread.Sleep(10000);
            //    //sftp.DownloadFile("Instant_data.txt", "data2.txt");
            //    //string[] x = sftp.getFile("data2.txt");
            //    //Register[] instant_data = ConverDataRegister(x);
            //    //if (instant_data.Length == 0)
            //    //    return;
            //    //label1.Text = instant_data[instant_data.Length - 1].Value_register.ToString();
            //    //sftp.DeleteFile("Instant_data.txt");
            //}
        }

        private void Save_graphics_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.SaveImage("Image.jpg", ChartImageFormat.Jpeg);
        }

        private void Show_device_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (Indicator == false)
            {
                MessageBox.Show("Немає з'єднання! \n Спершу встановіть з'єднання!");
            }
            else
            {
                Table_device = new Table_device();
                Table_device.ShowDialog();
                //Date
                string upload_filename_date = "Date.txt";
                string remove_filename_date = "date.txt";
                DateTime date1 = DateTime.Now;
                string year_now = Convert.ToString(date1.Year - 2000);
                string second_now = Convert.ToString(date1.Second + 5);
                string month_now = Convert.ToString(date1.Month);
                string day_now = Convert.ToString(date1.Day);
                string hour_now = Convert.ToString(date1.Hour);
                string min_now = Convert.ToString(date1.Minute);
                if (Convert.ToInt32(month_now) < 10) { month_now = "0" + month_now; }
                if (Convert.ToInt32(day_now) < 10) { day_now = "0" + day_now; }
                if (Convert.ToInt32(hour_now) < 10) { hour_now = "0" + hour_now; }
                if (Convert.ToInt32(min_now) < 10) { min_now = "0" + min_now; }
                string text_1 = "" + month_now + "" + day_now + "" + hour_now + "" + min_now + "" + year_now + "." + second_now;
                System.IO.File.WriteAllText(@"Date.txt", text_1);
                sftp.UploadFile(upload_filename_date, remove_filename_date);
                //
            }
        }

        private void Exit_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
