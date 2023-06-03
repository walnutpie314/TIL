using Nakov.TurtleGraphics;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Turtle_ScreenSaver
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private Random rnd;
        private int exitCount;

        public Form1()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 200; 
            timer.Tick += Timer_Tick;

            rnd = new Random();

            exitCount = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int swidth = this.ClientSize.Width, sheight = this.ClientSize.Height;
            int r, g, b, angle, dist;
            float curX, curY;

            r = rnd.Next(0, 256);
            g = rnd.Next(0, 256);
            b = rnd.Next(0, 256);
            Turtle.PenColor = Color.FromArgb(r, g, b);

            angle = rnd.Next(0, 360);
            dist = rnd.Next(50, 200);
            Turtle.Rotate(angle);
            Turtle.Forward(dist);
            curX = Turtle.X;
            curY = Turtle.Y;

            if ((-swidth / 2 <= curX && curX <= swidth / 2) && (-sheight / 2 <= curY && curY <= sheight / 2))
            {

            }
            else
            {
                Turtle.PenUp();
                Turtle.MoveTo(0, 0);
                Turtle.PenDown();

                exitCount++;
                if (exitCount == 50)
                {
                    timer.Stop(); 
                }
            }
        }

         private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                Application.Exit();
            }
        }
    }
}

