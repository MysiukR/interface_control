using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System.Threading;

namespace Interface_Control
{
    class SFTP
    {
        const int port = 22;
       string host;// = "169.254.106.99";//"169.254.130.116";//"192.168.0.102";//"169.254.106.99";
       string username;// = "pi";
       string password;// = "raspberry";
        const string workingdirectory = "/home/pi/Dropbox-Uploader/Dropbox"; //"/home/pi/ModBus/Device";
        const string uploadfile = "device_2.txt";
        SftpClient client;
        FileStream file;
        
        private void readSettings()
        {
         
         host = Properties.Settings.Default.host;
         username = Properties.Settings.Default.username;
         password = Properties.Settings.Default.password;
        }

        public SFTP()
        {
            readSettings();
            client = new SftpClient(host, port, username, password);
            file = null;
        }

        
        public bool InitializeSFTP()
        {
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
                client.ChangeDirectory(workingdirectory);
                var listDirectory = client.ListDirectory(workingdirectory);
                foreach (var file in listDirectory)
                {
                    if (file.Name == uploadfile)
                    {
                        return true;
                    }
                   
                }

            }
            else return false;

            return false;
        }

        public void DownloadFile(string uploadfile,string pathFile)
        {
            try
            {
            
            file = new FileStream(pathFile, FileMode.Create);
            client.DownloadFile(uploadfile, file);
            file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }
        public void DeleteFile(string removefile)
        {
            try
            {
                client.DeleteFile(removefile);
        }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
}

        public void UploadFile(string upload_filename, string remove_filename)
        {
            const string workingdirectory ="/home/pi/Dropbox-Uploader/Dropbox";//"/home/pi/ModBus"; //Changing path to project on Raspberry Pi
            if (client.IsConnected)
            {
                client.ChangeDirectory(workingdirectory);

                try
                {
                    using (var file = File.OpenRead(upload_filename))
                    {
                        client.UploadFile(file, remove_filename);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("" + ex.Message);
                }
            }
        }
        public string[] setFile(string nameFile)
        {
            FileStream file = null;
            StreamReader readFile = null;
            List<string> list = new List<string>();
            List<string> bufferRead = new List<string>();
            try
            {
                file = new FileStream(nameFile, FileMode.Open);
                readFile = new StreamReader(file, Encoding.Default);
                bufferRead.Clear();

                while (!readFile.EndOfStream)
                {
                    bufferRead.Add(readFile.ReadLine());

                }
                readFile.Close();
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message);
            }
            return bufferRead.ToArray();
        }

        public string[] getFile(string nameFile)
        {
            FileStream file = null;
            StreamReader readFile = null;
            List<string> list = new List<string>();
            List<string> bufferRead = new List<string>();
            try
            {
                file = new FileStream(nameFile, FileMode.Open);
                readFile = new StreamReader(file, Encoding.Default);
                bufferRead.Clear();

                while (!readFile.EndOfStream)
                {
                    bufferRead.Add(readFile.ReadLine());

                }
                readFile.Close();
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message);
            }
            return bufferRead.ToArray();
        }
    }
}
