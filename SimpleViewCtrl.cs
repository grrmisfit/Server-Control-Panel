using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DediServerCron
{
    public partial class SimpleViewCtrl : UserControl
    {
        public static Label[] CpuLabels = new Label[6];
        public static Label[] MemLabels = new Label[6];
        public static PictureBox[] OnBox = new PictureBox[6];
        public SimpleViewCtrl()
        {
            InitializeComponent();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SimpleViewCtrl_Load(object sender, EventArgs e)
        {
            CpuLabels[0] = cpu1;
            CpuLabels[1] = cpu2;
            CpuLabels[2] = cpu3;
            CpuLabels[3] = cpu4;
            CpuLabels[4] = cpu5;
            CpuLabels[5] = cpu6;

            MemLabels[0] = mem1;
            MemLabels[1] = mem2;
            MemLabels[2] = mem3;
            MemLabels[3] = mem4;
            MemLabels[4] = mem5;
            MemLabels[5] = mem6;

            OnBox[0] = onbox1;
            OnBox[1] = onbox2;
            OnBox[2] = onbox3;
            OnBox[3] = onbox4;
            OnBox[4] = onbox5;
            OnBox[5] = onbox6;
        }
    }
}
