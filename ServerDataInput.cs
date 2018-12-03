using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using DediServerCron.Data;
using LiteDB;
using Microsoft.WindowsAPICodePack.Dialogs;
using Timer = System.Timers.Timer;


namespace DediServerCron
{
    public partial class ServerDataInput : UserControl
    {
        private static string gametype;
        private static int testcnt = 1;
        public ServerDataInput()
        {
            InitializeComponent();
            StartTimer();
        }
        private static Timer loopingTimer;


        internal static Task StartTimer()
        {

            loopingTimer = new Timer()
            {
                Interval = 5000,
                AutoReset = true,
                Enabled = true
            };
            loopingTimer.Elapsed += OnTimerTicked;


            return Task.CompletedTask;
        }
        private static async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            string memsize = "GB";
            //var db = new LiteDatabase(@"Data\servers.db");
            // var servers = db.GetCollection<Servers>("servers");
            var result = DataStorage.GetDbInfo();//servers.FindAll();
            if (result == null) return;
            foreach (var t in result)
            {
                var curCnt = 0;
                var curproc = t.GameType; //+ ".exe";
                var confloc = t.ConfigFileLocation;
                var restart = t.AutoRestart;
                var batchLoc = t.BatchFileLocation;


                bool serverFound = false;
                Process[] workers = Process.GetProcessesByName(curproc); //("7DaysToDieServer.exe");
                foreach (Process worker in workers)
                {
                    var thePath = worker.MainModule.FileName;
                    string trimedLoc = batchLoc.Replace("startdedicated.bat", "");
                    string theloc = $"{trimedLoc}{curproc}.exe";
                    if (worker.ProcessName == curproc && thePath == theloc)
                    {

                        PerformanceCounter pc = new PerformanceCounter();
                        pc.CategoryName = "Process";
                        pc.CounterName = "Working Set - Private";
                        pc.InstanceName = worker.ProcessName;
                        long memconvert = Convert.ToInt64(pc.NextValue()) / 1000000;
                        if (memconvert < 1000)
                        {
                            memsize = "MB";
                        }
                        string themem = memconvert + memsize;
                        if (!Globals.SimpleView)
                        {
                            pictureBox1.ImageLocation = @"icons/online.png";
                            serverFound = true;
                            cpuLabel.Text = Utilities.GetCpuCounter(curproc) + @"%";
                            memLabel.Text = themem; //Utilities.GetServerName(curId,confloc);
                        }
                        else
                        {
                            SimpleViewCtrl.OnBox[curCnt].ImageLocation = @"icons/online.png";
                            serverFound = true;
                            SimpleViewCtrl.MemLabels[curCnt].Text = themem;
                            SimpleViewCtrl.CpuLabels[curCnt].Text = Utilities.GetCpuCounter(curproc) + @"%";
                        }

                        break;
                    }


                }



                if (!serverFound)
                {
                    if (!Globals.SimpleView)
                    {
                        pictureBox1.ImageLocation = @"icons/offline.png";

                    }
                    else
                    {
                        SimpleViewCtrl.OnBox[curCnt].ImageLocation = @"icons/offline.png";

                    }
                    if (!restart) continue;
                    Utilities.ExecuteBatch(batchLoc);
                    Task.Delay(5000);

                }
                curCnt = curCnt + 1;
            }

            await Task.CompletedTask;
        }
        private void ServerDataInput_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = @"icons/Offline.png";
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            
           Application.DoEvents();
            string memsize = "GB";
            //var db = new LiteDatabase(@"Data\servers.db");
           // var servers = db.GetCollection<Servers>("servers");
            var result = DataStorage.GetDbInfo();//servers.FindAll();
            if (result == null) return;
            foreach (var t in result)
            {
                var curCnt = 0;
                var curproc = t.GameType; //+ ".exe";
                var confloc = t.ConfigFileLocation;
                var restart = t.AutoRestart;
                var batchLoc = t.BatchFileLocation;


                bool serverFound = false;
                Process[] workers = Process.GetProcessesByName(curproc); //("7DaysToDieServer.exe");
                foreach (Process worker in workers)
                {
                    var thePath = worker.MainModule.FileName;
                    string trimedLoc = batchLoc.Replace("startdedicated.bat", "");
                    string theloc = $"{trimedLoc}{curproc}.exe";
                    if (worker.ProcessName == curproc && thePath==theloc)
                    {
                        
                        PerformanceCounter pc = new PerformanceCounter();
                        pc.CategoryName = "Process";
                        pc.CounterName = "Working Set - Private";
                        pc.InstanceName = worker.ProcessName;
                        long memconvert = Convert.ToInt64(pc.NextValue()) / 1000000;
                        if (memconvert < 1000)
                        {
                            memsize = "MB";
                        }
                        string themem = memconvert + memsize;
                        if (!Globals.SimpleView)
                        {
                            pictureBox1.ImageLocation = @"icons/online.png";
                            serverFound = true;
                            cpuLabel.Text = Utilities.GetCpuCounter(curproc) + @"%";
                            memLabel.Text = themem; //Utilities.GetServerName(curId,confloc);
                        }
                        else
                        {
                            SimpleViewCtrl.OnBox[curCnt].ImageLocation = @"icons/online.png";
                            serverFound = true;
                            SimpleViewCtrl.MemLabels[curCnt].Text = themem;
                            SimpleViewCtrl.CpuLabels[curCnt].Text = Utilities.GetCpuCounter(curproc) + @"%";
                        }

                        break;
                    }

                    
                }

                

                if (!serverFound)
                {
                    if (!Globals.SimpleView)
                    {
                        pictureBox1.ImageLocation = @"icons/offline.png";
                       
                    }
                    else
                    {
                        SimpleViewCtrl.OnBox[curCnt].ImageLocation = @"icons/offline.png";
                        
                    }
                    if (!restart) continue;
                    Utilities.ExecuteBatch(batchLoc);
                    Task.Delay(5000);
                    
                }
                curCnt = curCnt + 1;
            }

            testcnt = testcnt + 1;
        }

        //private void button1_MouseHover(object sender, EventArgs e)
        //{
        //    ToolTip toolTip1 = new ToolTip();
        //    toolTip1.SetToolTip(button1,
         //       "Stops the server process, it will be restarted on next check unless auto restart is off");
       // }

        private  void timer2_Tick(object sender, EventArgs e)
        {
            
            string memsize = " gb";
            if (timer1.Enabled) return;
           // var db = new LiteDatabase(@"Data\servers.db");
           // var servers = db.GetCollection<Servers>("servers");
            var result = DataStorage.GetDbInfo();//servers.FindAll();
            if (result == null) return;
            foreach (var t in result)
            {
                var curCnt = 0;
                var curproc = t.GameType; //+ ".exe";
                //var confloc = t.ConfigFileLocation;
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
                        
                        PerformanceCounter pc = new PerformanceCounter();
                        pc.CategoryName = "Process";
                        pc.CounterName = "Working Set - Private";
                        pc.InstanceName = worker.ProcessName;
                        long memconvert = Convert.ToInt64(pc.NextValue()) / 1000000;
                        if (memconvert < 1000)
                        {
                            memsize = " mb";
                        }
                        string themem = memconvert + memsize;
                        if (!Globals.SimpleView)
                        {
                            pictureBox1.ImageLocation = @"icons/online.png";
                            
                            cpuLabel.Text = Utilities.GetCpuCounter(curproc) + @"%";
                            memLabel.Text = themem;
                        }
                        else
                        {
                            SimpleViewCtrl.OnBox[curCnt].ImageLocation = @"icons/online.png";
                            
                            SimpleViewCtrl.MemLabels[curCnt].Text = themem;
                            SimpleViewCtrl.CpuLabels[curCnt].Text = Utilities.GetCpuCounter(curproc) + @"%";
                        }
                        
                        serverFound = true;
                        break;
                    }
                }

                

                if (!serverFound && !Globals.SimpleView)
                {
                    pictureBox1.ImageLocation = @"icons/offline.png";
                    label7.Text = @"Server Offline";
                    memLabel.Text = "";
                    button2.Enabled = true;
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var db = new LiteDatabase(@"Data\servers.db");
           // var theserver = db.GetCollection<Servers>("servers");
            var form = new Form1();
            var curtab = form.tabControl1.SelectedTab.TabIndex +1;
            var results = DataStorage.GetDbInfobyId(curtab); //theserver.FindById(curtab);

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
                
                if (server.ProcessName == curproc && fullPath == $"{trimedLoc}{curproc}.exe")

                {
                    server.Kill();
                    server.WaitForExit();
                    server.Dispose();
                    label1.Text = "";
                    cpuLabel.Text = "";
                    break;
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
            if (results.Paused)
            {
                var update = new Servers
                {
                    Paused = false
                };
                theserver.Update(update);
            }
            
            button2.Enabled = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var form = new Form1();
            var curtab = form.tabControl1.SelectedTab.TabIndex + 1;
            var results = DataStorage.GetDbInfobyId(curtab); //theserver.FindById(curtab);

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

                if (server.ProcessName == curproc && fullPath == $"{trimedLoc}{curproc}.exe")

                {
                    server.Kill();
                    server.WaitForExit();
                    server.Dispose();
                    label1.Text = "";
                    cpuLabel.Text = "";
                    var servUpdate = DataStorage.GetDb();
                    var update = new Servers
                    {
                        Paused = true
                    };
                    servUpdate.Update(update);

                    results.Paused = true;
                    break;
                }
            }
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
