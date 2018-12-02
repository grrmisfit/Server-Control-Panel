using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DediServerCron.Data;
using LiteDB;
using Microsoft.WindowsAPICodePack.Dialogs;
using Message = Microsoft.WindowsAPICodePack.Shell.Interop.Message;


namespace DediServerCron
{
    public partial class ServerDataInput : UserControl
    {
        public static string gametype;
        public ServerDataInput()
        {
            InitializeComponent();
        }

        private void ServerDataInput_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = @"icons/Offline.png";
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string memsize = "GB";
            var db = new LiteDatabase(@"Data\servers.db");
            var servers = db.GetCollection<Servers>("servers");
            var result = servers.FindAll();
            if (result == null) return;
            foreach (var t in result)
            {
                var curId = t.Id;
                var curproc = t.GameType + ".exe";
                var confloc = t.ConfigFileLocation;
                var restart = t.AutoRestart;
                var batchLoc = t.BatchFileLocation;


                bool serverFound = false;
                Process[] workers = Process.GetProcessesByName(curproc); //("7DaysToDieServer.exe");
                foreach (Process worker in workers)
                {
                    var thePath = worker.MainModule.FileName;
                    if (worker.ProcessName == curproc && thePath==t.BatchFileLocation)
                    {
                        pictureBox1.ImageLocation = @"icons/online.png";
                        PerformanceCounter pc = new PerformanceCounter();
                        pc.CategoryName = "Process";
                        pc.CounterName = "Working Set - Private";
                        pc.InstanceName = worker.ProcessName;
                        long memconvert = Convert.ToInt64(pc.NextValue()) / 1000000;
                        if (memconvert < 1000)
                        {
                            memsize = "MB";
                        }
                        string blah = memconvert + memsize;
                        label1.Text = blah;
                        serverFound = true;

                        label6.Text = Utilities.GetServerName(curId,confloc);

                        break;
                    }
                }

                label2.Text = Utilities.GetCpuCounter(curproc).ToString()+ @"%";

                if (!serverFound)
                {
                    pictureBox1.ImageLocation = @"icons/offline.png";
                    if (!restart) continue;
                    Utilities.ExecuteBatch(batchLoc);
                    Task.Delay(5000);
                    timer1.Enabled = true;

                }
            }
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.SetToolTip(button1,
                "Stops the server process, it will be restarted on next check unless auto restart is off");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            string memsize = " gbs";
            if (timer1.Enabled) return;
            var db = new LiteDatabase(@"Data\servers.db");
            var servers = db.GetCollection<Servers>("servers");
            var result = servers.FindAll();
            if (result == null) return;
            foreach (var t in result)
            {
                var curId = t.Id;
                var curproc = t.GameType; //+ ".exe";
                var confloc = t.ConfigFileLocation;
                var batchLoc = t.BatchFileLocation;
                
                bool serverFound = false;
                Process[] workers = Process.GetProcessesByName(curproc ); //("7DaysToDieServer.exe");
                foreach (Process worker in workers)
                {

                    string fullPath = worker.MainModule.FileName;
                    string trimedLoc = batchLoc.Replace("startdedicated.bat", "");
                    string theloc = $"{trimedLoc}{curproc}.exe";
                    if (worker.ProcessName == curproc && fullPath == theloc)
                    {
                        pictureBox1.ImageLocation = @"icons/online.png";
                        PerformanceCounter pc = new PerformanceCounter();
                        pc.CategoryName = "Process";
                        pc.CounterName = "Working Set - Private";
                        pc.InstanceName = worker.ProcessName;
                        long memconvert = Convert.ToInt64(pc.NextValue()) / 1000000;
                        if (memconvert < 1000)
                        {
                            memsize = " mbs";
                        }
                        string blah = memconvert + memsize;
                        label6.Text = blah;
                        serverFound = true;
                        var curCpu = Utilities.GetCpuCounter(curproc);
                        label1.Text = curCpu  + @"%";
                        
                        break;
                    }
                }

                label2.Text = Utilities.GetCpuCounter(curproc).ToString();

                if (!serverFound)
                {
                    pictureBox1.ImageLocation = @"icons/offline.png";
                    label7.Text = @"Server Offline";
                    label6.Text = "";
                    button2.Enabled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var db = new LiteDatabase(@"Data\servers.db");
            var theserver = db.GetCollection<Servers>("servers");
            var form = new Form1();
            var curtab = form.tabControl1.SelectedTab.TabIndex +1;
            var results = theserver.FindById(curtab);

            if (results == null)
            {
                MessageBox.Show(@"No server is set to this tab, check settings and try again.", @"Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var curproc = results.GameType;
            var batchLoc = results.BatchFileLocation;
            Process[] servers = Process.GetProcessesByName(curproc); //("7DaysToDieServer.exe");
            foreach (var server in servers)
            {


                string fullPath = server.MainModule.FileName;
                string trimedLoc = batchLoc.Replace("startdedicated.bat", "");
                //if (server.ProcessName == curproc && fullPath == $"{trimedLoc}{curproc}.exe")

                    if (server.ProcessName == curproc && fullPath == $"{trimedLoc}{curproc}.exe")

                {
                    server.Kill();
                    server.WaitForExit();
                    server.Dispose();
                    label1.Text = "";
                    label2.Text = "";
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var curSel = comboBox1.Text;
           
            switch (curSel)
            {
                case "7Days":
                    gametype = "7DaysToDieServer";
                    break;
                case "Conan":
                    gametype = "ConanSandboxServer";
                    break;
                    default:
                        gametype = "none";
                        break;
            }
           /* var db = new LiteDatabase(@"Data\servers.db");
            var theserver = db.GetCollection<Servers>("servers");
            var form = new Form1();
            var curtab = form.tabControl1.SelectedTab.TabIndex;
            var results = theserver.FindById(curtab);

           // if (results == null)
           // {
           //     var server = new Servers
           // {
            //        GameType = gametype
           // };
            //    theserver.Insert(server);
            //}
            //var results = theserver.FindById(curtab);

           // if (results == null)
           // {
            //    MessageBox.Show(@"No server is set to this tab, check settings and try again.", @"Error!",
           //         MessageBoxButtons.OK, MessageBoxIcon.Error);
           //     return;
           /*/ }

        private void button7_Click(object sender, EventArgs e)
        {
            var theloc = "C:\\Users";


            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = theloc,
                IsFolderPicker = false
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var fileLoc = dialog.FileName;
                if (string.IsNullOrEmpty(fileLoc))
                    MessageBox.Show(@"Please choose a directory for the batch file!", @"Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                textBox1.Text = fileLoc;
            }
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {

                InitialDirectory = @"C:\\",
                IsFolderPicker = false,

            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var fileLoc = dialog.FileName;
                if (string.IsNullOrEmpty(fileLoc))
                    MessageBox.Show(@"Please choose a file for the server config file!", @"Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                textBox2.Text = fileLoc;

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == @"GameType")
            {
                MessageBox.Show(@"Please choose a game type!", @"Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox7.Text == "")
            {
                MessageBox.Show(@"Please fill out the server name field!", @"Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show(@"Please set the location of your batch file!", @"Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show(@"Please choose your server config file!", @"Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show(@"Please choose an auto restart option!", @"Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool restart = radioButton1.Checked;
            if (radioButton2.Checked)
            {
                restart = false;
            }
            var form = new Form1();
            var db = new LiteDatabase(@"Data\servers.db");
            var servers = db.GetCollection<Servers>("servers");
            var result = servers.FindById(form.tabControl1.SelectedIndex +1);
            if (result == null)
            {
                var newServer = new Servers
                {
                    Id = form.tabControl1.SelectedIndex + 1,
                    GameType = gametype,
                    ServerName = textBox7.Text,
                    BatchFileLocation = textBox1.Text,
                    ConfigFileLocation = textBox2.Text,
                    AutoRestart = restart

                };
                servers.Insert(newServer);
            }
            else
            {
                DialogResult choice =
                    MessageBox.Show(@"There is already a server saved to this Tab, do you want override settings?",
                        @"Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (choice == DialogResult.Yes)
                {
                    var newServer = new Servers
                    {
                        Id = form.tabControl1.SelectedIndex + 1,
                        GameType = comboBox1.SelectedText,
                        ServerName = textBox7.Text,
                        BatchFileLocation = textBox1.Text,
                        ConfigFileLocation = textBox2.Text,
                        AutoRestart = restart

                    };
                    servers.Insert(newServer);
                }
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form1 = new Form1();
            var form = new Form2();
            var db = new LiteDatabase(@"Data\servers.db");
            var servers = db.GetCollection<Servers>("servers");
            var result = servers.FindById(form1.tabControl1.SelectedIndex + 1);
            form.label2.Text = result.ServerName;
            form.label4.Text = result.BatchFileLocation;
            form.label6.Text = result.ConfigFileLocation;
            form.label8.Text = result.AutoRestart.ToString();


            form.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var db = new LiteDatabase(@"Data\servers.db");
            var theserver = db.GetCollection<Servers>("servers");
            var form = new Form1();
            var curtab = form.tabControl1.SelectedTab.TabIndex + 1;
            var results = theserver.FindById(curtab);
            
            if (results == null)
            {
                MessageBox.Show(@"No server is set to this tab, check settings and try again.", @"Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var game = results.GameType;
            var curproc = results.GameType;
            var batchLoc = results.BatchFileLocation;
            Process[] servers = Process.GetProcessesByName(curproc); //("7DaysToDieServer.exe");
            foreach (var server in servers)
            {


                string fullPath = server.MainModule.FileName;
                string trimedLoc = batchLoc.Replace("startdedicated.bat", "");
                if (server.ProcessName == curproc && fullPath == $"{trimedLoc}{curproc}.exe")

                {
                    MessageBox.Show(@"Server is already running! Use stop then try again!", @"Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (!File.Exists($"{batchLoc}"))
            {
                return;
            }
            Utilities.ExecuteBatch(batchLoc);
            Thread.Sleep(5000);
            timer1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            bool restart = radioButton1.Checked;
            if (radioButton2.Checked)
            {
                restart = false;
            }

            var form = new Form1();
            var db = new LiteDatabase(@"Data\servers.db");
            var servers = db.GetCollection<Servers>("servers");
            var result = servers.FindById(form.tabControl1.SelectedIndex + 1);

            if (result == null)
            {
                MessageBox.Show(@"No server set to this panel, please save a server first!", @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBox1.Text == @"GameType")
            {
                comboBox1.Text = result.GameType;
            }

            if (textBox7.Text == "")
            {
                textBox7.Text = result.ServerName;
            }

            if (textBox1.Text == "")
            {
                textBox1.Text = result.BatchFileLocation;
            }

            if (textBox2.Text == "")
            {
                textBox2.Text = result.ConfigFileLocation;
            }

            if (radioButton1.Checked)
            {
                result.AutoRestart = true;
            }

            var newServer = new Servers
            {
                Id = form.tabControl1.SelectedIndex + 1,
                GameType = gametype,
                ServerName = textBox7.Text,
                BatchFileLocation = textBox1.Text,
                ConfigFileLocation = textBox2.Text,
                AutoRestart = restart

            };
            servers.Update(newServer);

        }
    }
}
