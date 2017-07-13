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


namespace Interface_Control
{
    public partial class Settings_functions_registers : Form
    {
        SFTP sftp;
        public Settings_functions_registers()
        {
            InitializeComponent();
            sftp = new SFTP();
        }

        private void SetPosition()
        {
            label5.Location = new Point(41, 62);
            numericUpDown4.Location = new Point(135, 60);
            numericUpDown5.Location = new Point(147, 119);
            label10.Location = new Point(192, 119);
            label12.Location = new Point(41, 92);
            label13.Location = new Point(208, 92);
            label7.Location = new Point(41, 120); 
            numericUpDown2.Location = new Point(212, 119);

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
                    int device_number = Convert.ToInt32(numericUpDown4.Value);
                    int register_number = Convert.ToInt32(numericUpDown5.Value);
                    double time = Convert.ToDouble(numericUpDown6.Value);
                    int value_register = Convert.ToInt32(textBox1.Text);

                    DialogResult result = MessageBox.Show("Ви обрали: " + "\nФункцію: 1" + "\nНомер приладу:" + device_number + "\nНомер регістру:" + register_number + "\nЗначення:" + value_register + "\nЧас затримки:" + time + "хв.\n", "Data_information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No)
                    { }
                    else { Close(); }
                    time = time * 60;
                    string text = "1" + "\n" + device_number + "\n" + register_number + "\n" + value_register + "\n" + time + "\n";
                    System.IO.File.WriteAllText(@"data1.txt", text);
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    int device_number = Convert.ToInt32(numericUpDown4.Value);
                    int register_number = Convert.ToInt32(numericUpDown5.Value);
                    double time = Convert.ToDouble(numericUpDown6.Value);

                    DialogResult result = MessageBox.Show("Ви обрали: " + "\nФункцію: 2" + "\nНомер приладу:" + device_number + "\nНомер регістру:" + register_number + "\nЧас затримки:" + time + "хв.\n", "Data_information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No)
                    { }
                    else { Close(); }
                    time = (time * 60) - 5;
                    string text = "2" + "\n" + device_number + "\n" + register_number + "\n" + time + "\n";
                    System.IO.File.WriteAllText(@"data1.txt", text);
                }
                if (comboBox1.SelectedIndex == 2)
                {
                    int device_number = Convert.ToInt32(numericUpDown4.Value);
                    int register_number_start = Convert.ToInt32(numericUpDown5.Value);
                    int register_number_finish = Convert.ToInt32(numericUpDown2.Value);
                    if (register_number_start > register_number_finish)
                    {
                        MessageBox.Show("Початкове значення повинно бути менше кінцевого!");
                    }
                    if (register_number_start == register_number_finish)
                    {
                        int device_number_1 = Convert.ToInt32(numericUpDown4.Value);
                        int register_number_1 = Convert.ToInt32(numericUpDown5.Value);
                        double time_1 = Convert.ToDouble(numericUpDown6.Value);
                        DialogResult result_1 = MessageBox.Show("Ви обрали: " + "\nФункцію: 2" + "\nНомер приладу:" + device_number_1 + "\nНомер регістру:" + register_number_1 + "\nЧас затримки:" + time_1 + "хв.\n", "Data_information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result_1 == DialogResult.No)
                        { }
                        else { Close(); }
                        time_1 = (time_1 * 60) - 5;
                        string text_1 = "2" + "\n" + device_number_1 + "\n" + register_number_1 + "\n" + time_1 + "\n";
                        System.IO.File.WriteAllText(@"data1.txt", text_1);
                    }
                    double time = Convert.ToDouble(numericUpDown6.Value);
                    DialogResult result = MessageBox.Show("Ви обрали: " + "\nФункцію: 3" + "\nНомер приладу:" + device_number + "\nПочатковий номер регістру:" + register_number_start + "\nКінцевий номер регістру:" + register_number_finish + "\nЧас затримки:" + time + "хв.\n", "Data_information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No)
                    { }
                    else { Close(); }
                    time = (time * 60);
                    string text = "3" + "\n" + device_number + "\n" + register_number_start + "\n" + register_number_finish + "\n" + time + "\n";
                    System.IO.File.WriteAllText(@"data1.txt", text);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            label12.Location = new Point(132, 99);
            numericUpDown5.Location = new Point(135, 115);
            if (comboBox1.Text == String.Empty)
            {
                label13.Visible = false;
                numericUpDown2.Visible = false;
                label10.Visible = false;
                label7.Text = "Номер регістра:";


            }
            if (comboBox1.SelectedIndex == 0)
            {
                // numericUpDown2.Visible = false;
                //label10.Visible = false;

                label13.Visible = false;
                numericUpDown2.Visible = false;
                label10.Visible = false;
                label7.Text = "Номер регістра:";
                textBox1.Visible = true;
                label11.Visible = true;


            }
            if (comboBox1.SelectedIndex == 1)
            {
                label13.Visible = false;
                numericUpDown2.Visible = false;
                label10.Visible = false;
                label7.Text = "Номер регістру:";
                textBox1.Visible = false;
                label11.Visible = false;

            }
            if (comboBox1.SelectedIndex == 2)
            {

                label13.Visible = true;
                numericUpDown2.Visible = true;
                label10.Visible = true;
                label7.Text = "Номери регістрів: з";
                SetPosition();
                textBox1.Visible = false;
                label11.Visible = false;

            }
        }

        private void Settings_functions_registers_Load(object sender, EventArgs e)
        {

            numericUpDown2.Visible = false;
            label10.Visible = false;
            label13.Visible = false;
            label12.Text = "Hexadecimal value is 0x001";
            label13.Text = "0x001";
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            int value_1 = Convert.ToInt32(numericUpDown5.Value);
            // Convert the decimal value to a hexadecimal value in string form.
            string hexOutput_1 = String.Format("{0:X}", value_1);
            label12.Text = "Hexadecimal value is 0x00" + hexOutput_1;
        }

        private void numericUpDown2_ValueChanged_1(object sender, EventArgs e)
        {
            label13.Visible = true;
            int value_2 = Convert.ToInt32(numericUpDown2.Value);
            // Convert the decimal value to a hexadecimal value in string form.
            string hexOutput_2 = String.Format("{0:X}", value_2);
            label13.Text = "0x00" + hexOutput_2;
        }
    }
}
