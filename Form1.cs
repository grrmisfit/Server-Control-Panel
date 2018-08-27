using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DediServerCron
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process[] workers = Process.GetProcesses(); //("7DaysToDieServer.exe");
            foreach (Process worker in workers)
            {
                if (worker.ProcessName == "7DaysToDieServer.exe")
                {
                    listBox1.Items.Add(worker.ProcessName);
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }
            }
        }
        }
    }
}
