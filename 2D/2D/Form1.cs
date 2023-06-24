using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2D
{
    public partial class Form1 : Form
    {
        int PlayerSpeed = 5; //플레이어 스피드
        bool goleft, goright; //좌우 이동

        private float turretAngle; // 총알 각도
        private bool goUp, goDown; // 위아래 이동
        private bool isFiring; // 발사 여부

        private float gravity = 9.8f; // 중력 가속도    
        private float time = 0f; // 경과 시간

        private float horizontalVelocity; // 수평 속도
        private float verticalVelocity; // 수직 속도

        private List<Point> trajectoryPoints; // 포물선 경로 좌표 리스트
        private Point currentPosition; // 현재 위치


        public Form1()
        {
            InitializeComponent();
            trajectoryPoints = new List<Point>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            if(e.KeyCode == Keys.Right)
            {
                goright = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                if (!isFiring)
                {
                    isFiring = true;
                    time = 0f;
                    trajectoryPoints.Clear();
                }
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }


        private void timer1_Tick(object sender, EventArgs e) //게임 메인 타이머
        {
            if(goleft)
            {
                Player.Left -= PlayerSpeed;
            }
            if(goright)
            {
                Player.Left += PlayerSpeed;
            }
            if (goUp)
            {
                turretAngle -= 1f; // 각도 증가
                if (turretAngle < 0f)
                    turretAngle += 360f; // 각도가 음수가 되면 360도를 더해주어 양수로 조정
            }
            else if (goDown)
            {
                turretAngle += 1f; // 각도 감소
                if (turretAngle >= 360f)
                    turretAngle -= 360f; // 각도가 360도 이상이면 360도를 빼주어 0~359도 범위로 조정
            }

            if (isFiring)
            {
                trajectoryPoints.Clear();

                // 초기 설정
                int initialSpeed = 10;
                int launchAngle = 45;

                // 발사각도를 라디안으로 변환
                float launchAngleRad = launchAngle * (float)Math.PI / 180f;

                // 초기 속도를 수평과 수직 속도로 분해
                float horizontalVelocity = initialSpeed * (float)Math.Cos(launchAngleRad);
                float verticalVelocity = initialSpeed * (float)Math.Sin(launchAngleRad);

                // 초기 위치
                float startX = Player.Left + Player.Width / 2f;
                float startY = Player.Top + Player.Height / 2f;
                float time = 0f;

                while (startY <= ClientSize.Height)
                {
                    // 수평 방향으로 이동
                    float x = startX + horizontalVelocity * time;

                    // 수직 방향으로 이동
                    float y = startY - verticalVelocity * time + 0.5f * gravity * time * time;

                    trajectoryPoints.Add(new Point((int)x, (int)y));

                    // 시간 증가
                    time += 0.1f;
                }
            }

            Refresh(); // 그래픽 업데이트
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 그래픽 업데이트 시 총알의 각도를 적용하여 그리기
            float startX = Player.Left + Player.Width / 2f;
            float startY = Player.Top + Player.Height / 2f;
            float endX = startX + (float)Math.Cos(turretAngle * Math.PI / 180f) * 50f;
            float endY = startY - (float)Math.Sin(turretAngle * Math.PI / 180f) * 50f;
            e.Graphics.DrawLine(Pens.Black, startX, startY, endX, endY);

            // 그래픽 업데이트 시 포물선 경로 그리기
            foreach (var point in trajectoryPoints)
            {
                e.Graphics.FillEllipse(Brushes.Red, point.X, point.Y, 5, 5);
            }
        }
    }
}
