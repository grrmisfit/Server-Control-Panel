using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DediServerCron.Data;
using LiteDB;

namespace DediServerCron
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            

            var servCount = DataStorage.GetDbInfo();//servers.FindAll();
                foreach (var t in servCount)
                {
                    var tabcnt = 0;

                    tabControl1.TabPages[tabcnt].Text = " " + t.ServerName;
                    tabcnt = tabcnt + 1;
                }

            Globals.SimpleView = false;

            // }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void fullViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Globals.SimpleView = false;
            var curTab = tabControl1.SelectedIndex;
            simpleViewCtrl1.Visible = false;
            tabControl1.Visible = true;
           /* switch (curTab)
            {
                case 0:
                    serverDataInput1.Visible = true;
                    break;
                case 1:
                    serverDataInput2.Visible = true;
                    break;
                case 2:
                    serverDataInput3.Visible = true;
                    break;
                case 3:
                    serverDataInput4.Visible = true;
                    break;
                case 4:
                    serverDataInput5.Visible = true;
                    break;
                case 5:
                    serverDataInput6.Visible = true;
                    break;

            }
            */
        
        }

        private void simpleViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Globals.SimpleView = true;
            tabControl1.Visible = false;
           
            simpleViewCtrl1.Visible = true;
            
            /* var curTab = tabControl1.SelectedIndex;
             switch (curTab)
             {
                 case 0:
                     serverDataInput1.Visible = false;
                     break;
                 case 1:
                     serverDataInput2.Visible = false;
                     break;
                 case 2:
                     serverDataInput3.Visible = false;
                     break;
                 case 3:
                     serverDataInput4.Visible = false;
                     break;
                 case 4:
                     serverDataInput5.Visible = false;
                     break;
                 case 5:
                     serverDataInput6.Visible = false;
                     break;
 
             }*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}
