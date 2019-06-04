using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessMonitor
{
    public partial class Form1 : Form
    {
        private Single _X;
        private Single _sngY;
        private Single _cpuY;
        private Single _proY;
        private Single _memY;
        Pen penCPU = new Pen(Color.Red);
        Pen penProc = new Pen(Color.Green);
        Pen penMem = new Pen(Color.Blue);
        Pen penFore = new Pen(SystemColors.ActiveCaption);
        Pen penBack = new Pen(SystemColors.ControlLight);

        const Single INCREMENT = 1;

        private PerformanceCounter prozessor;
        private PerformanceCounter processe;
        private PerformanceCounter speicher;

        public Form1()
        {
            InitializeComponent();

            prozessor = new PerformanceCounter();
            prozessor.CategoryName = "Prozessor";
            prozessor.MachineName = ".";
            prozessor.InstanceName = "_Total";
            prozessor.CounterName = "Prozessorzeit (%)";

            processe = new PerformanceCounter();
            processe.CategoryName = "System";
            processe.MachineName = ".";
            processe.CounterName = "Processes";

            speicher = new PerformanceCounter();
            speicher.CategoryName = "Memory";
            speicher.MachineName = ".";
            speicher.CounterName = "% Committed Bytes In Use";

            toolTipTrackbar.SetToolTip(trackBar1, timer1.Interval.ToString() + "ms");

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "&Start")
            {
                buttonStart.Text = "&Stop";
                timer1.Enabled = true;
            }
            else
            {
                buttonStart.Text = "&Start";
                timer1.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Single cpu = prozessor.NextValue() / 100;
            Single pro = processe.NextValue();
            Single mem = speicher.NextValue();

            Zeichnen(cpu, pro, mem);

            labelCPU.Text = cpu.ToString("p");
            labelProcesse.Text = pro.ToString();
            labelSpeicher.Text = mem.ToString("f2") + " %";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int value = (int)trackBar1.Value;
            timer1.Interval = value * 100;
            toolTipTrackbar.SetToolTip(trackBar1, timer1.Interval.ToString() + "ms");

        }

        private void Zeichnen(Single cpuWert, Single proWert, Single memWert)
        {
            Graphics graphics = panel1.CreateGraphics();
            Single height = panel1.Height;
            Single cpuY;
            Single proY;
            Single memY;

            graphics.DrawLine(penBack, _X, 0, _X, height);

            cpuY = height - (cpuWert * height) - 1;
            graphics.DrawLine(penCPU, _X - INCREMENT, _cpuY, _X, cpuY);
            _cpuY = cpuY;

            proY = height - (float)(proWert / 3.5);
            graphics.DrawLine(penProc, _X - INCREMENT, _proY, _X, proY);
            _proY = proY;

            memY = height - (memWert);
            graphics.DrawLine(penMem, _X - INCREMENT, _memY, _X, memY);
            _memY = memY;

            _X += INCREMENT;

            if (_X > panel1.Width)
            {
                _X = 0;
                graphics.Clear(SystemColors.ControlLight);
            }

            graphics.DrawLine(penFore, _X, 0, _X, height);

        }
    }
}
