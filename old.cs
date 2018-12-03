using System;
using System.Windows.Forms;
using DediServerCron.Data;
using LiteDB;

namespace DediServerCron
{
    public partial class old : Form
    {
        public old()
        {
            InitializeComponent();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            
          //  button1.MouseHover += button1_MouseHover;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            using (var db = new LiteDatabase(@"Data\servers.db"))
            {

                var servers = db.GetCollection<Servers>("servers");
    
                var servCount = servers.FindAll();
                foreach (var t in servCount)
                {
                    var tabcnt = 0;
                    
                    tabControl1.TabPages[tabcnt].Text = t.ServerName;
                    tabcnt = tabcnt + 1;
                }


            }
        }

        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            var db = new LiteDatabase(@"Data\servers.db");
            var servers = db.GetCollection<Servers>("servers");
            var result = servers.FindAll();
            

        }
        private void button1_MouseHover(object sender, EventArgs e)
        {
           // ToolTip toolTip1 = new ToolTip();
           // toolTip1.SetToolTip(button1,
            //    "Stops the server process, it will be restarted on next check unless auto restart is off");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          
        }
        
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

        }

      
        private void button19_Click(object sender, EventArgs e)
        {
            

           


        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
           
           
           
           
        }
    

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void serverDataInput1_Load(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
                    
            
        
    
        

    

