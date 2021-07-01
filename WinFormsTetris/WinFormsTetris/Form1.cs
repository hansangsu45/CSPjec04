using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Media;

//테트리스 색 바꿈, 블럭 배치할 수록 내려오는 속도 증가, 점수와 최고점수 추가, 최고점수와 최고점수 달성 날짜 저장
//배경 넣음, 블럭 배치 효과음, 다음 블럭 미리보기, 다시하기, 블럭 위치,회전 되돌리기
namespace WinFormsTetris
{
    public partial class Form1 : Form
    {
        Game game;
        int bx;
        int by;
        int bwidth;
        int bheight;

        int maxRewCnt = 200;
        int rewCnt;

        public static int score = 0;
        int bestScore = 0;
        string savePath;
        string savedStr;

        Random rand = new Random();
        Dictionary<int, Color> colorDic = new Dictionary<int, Color>();
        int colorIndex, beforeIdx;
        bool checkCurBlockLine = false;

        Bitmap img;
        SoundPlayer sp;
        

        public Form1()  //생성자
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //Start()
        {
            game = Game.Singleton;
            bx = GameRule.BX;
            by = GameRule.BY;
            bwidth = GameRule.B_WIDTH;
            bheight = GameRule.B_HEIGHT;
            this.SetClientSizeCore(GameRule.BX * GameRule.B_WIDTH + 210, GameRule.BY * GameRule.B_HEIGHT);

            InitData();

            DoubleBuffered = true;

        }

        private void InitData()
        {
            savePath = string.Concat(Application.StartupPath, "/", "SaveFile.txt");
            DataLoad();

            rewCnt = maxRewCnt;
            colorDic.Add(0, Color.Red);
            colorDic.Add(1, Color.Blue);
            colorDic.Add(2, Color.Green);
            colorDic.Add(3, Color.LightPink);
            colorDic.Add(4, Color.LightSkyBlue);
            colorDic.Add(5, Color.Purple);
            colorDic.Add(6, Color.Yellow);

            ScoreTxt.Location = new Point(500, 65);
            ScoreTxt.Text = $"현재점수: {score}";
            BestTxt.Location = new Point(500, 1);
            BestTxt.Text = $"최고점수: {bestScore}";
            DateTxt.Location = new Point(490, 23);

            RewCnt.Text = $"되돌리기 남은 횟수: {rewCnt}";

            ReTxt.Location = new Point(520, 350);
            RewindTxt.Location = new Point(520, 400);
            RewCnt.Location = new Point(500, 428);

            img = new Bitmap("TetBg.jpg");  //Application.StartupPath + /TetBg.jpg
            sp = new SoundPlayer(Application.StartupPath + "/BlockEffect1.wav");
        }

        private void DataLoad()
        {
            if (File.Exists(savePath))
            {
                string code = File.ReadAllText(savePath);
                byte[] bytes = Convert.FromBase64String(code);
                string[] strs = Encoding.UTF8.GetString(bytes).Split('|');
                bestScore = int.Parse(strs[0]);
                DateTxt.Text = strs[1];
            }
        }

        private void Save()
        {
            savedStr = bestScore.ToString() + "|" + DateTxt.Text;
            byte[] bytes = Encoding.UTF8.GetBytes(savedStr);
            string code = Convert.ToBase64String(bytes);
            File.WriteAllText(savePath, code);
        }

        private void Form1_Paint(object sender, PaintEventArgs e) //Update()
        {
            e.Graphics.DrawImage(img, 0, 0);

            DoubleBuffered = true;
            DrawGraduation(e.Graphics);
            DrawDiagram(e.Graphics);
            DrawDiagramNext(e.Graphics);
            DrawBoard(e.Graphics);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right: MoveRight(); return;
                case Keys.Left: MoveLeft(); return;
                case Keys.Up: MoveTurn(); return;
                case Keys.Down: MoveDown(); return;
                case Keys.Space: MoveSSDown(); return;
                case Keys.G: MoveBack(); return;
                case Keys.R: GameReplay(); return; 
            }
        }

        private void GameReplay()
        {
            game.ReStart();
            Restart();
            Invalidate();
            game.now.X = GameRule.SX;
            game.now.Y = GameRule.SY;
        }

        private void timer1_Tick(object sender, EventArgs e)  //Update()
        {
            MoveDown();
        }

        private void DrawBoard(Graphics graphics)
        {
            for (int xx = 0; xx < bx; xx++)
            {
                for (int yy = 0; yy < by; yy++)
                {
                    if (game[xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle(xx * bwidth + 2, yy * bheight + 2, bwidth - 4, bheight - 4);
                        graphics.DrawRectangle(Pens.White, now_rt);
                        Fill(beforeIdx, graphics, now_rt);
                        //graphics.FillRectangle(Brushes.Black, now_rt);
                    }
                }
            }
        }

        private void Fill(int i, Graphics graphics, Rectangle now_rt)
        {
            if (i == 0) graphics.FillRectangle(Brushes.Red, now_rt);
            else if (i == 1) graphics.FillRectangle(Brushes.Blue, now_rt);
            else if (i == 2) graphics.FillRectangle(Brushes.Green, now_rt);
            else if (i == 3) graphics.FillRectangle(Brushes.LightPink, now_rt);
            else if (i == 4) graphics.FillRectangle(Brushes.LightSkyBlue, now_rt);
            else if (i == 5) graphics.FillRectangle(Brushes.Purple, now_rt);
            else if (i == 6) graphics.FillRectangle(Brushes.Yellow, now_rt);
        }

        private void DrawDiagram(Graphics graphics)
        {
            int bn = game.BlockNum;
            int tn = game.Turn;
            Point now = game.NowPosition;

            if (!checkCurBlockLine)
            {
                colorIndex = rand.Next(0, 7);
                checkCurBlockLine = true;
            }
            Pen dpen = new Pen(colorDic[colorIndex], 3);

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        graphics.DrawRectangle(dpen, now_rt);
                    }
                }
            }
        }
        private void DrawDiagramNext(Graphics graphics)
        {
            int bn = game.next.BlockNum;
            int tn = game.next.Turn;
            Point next = new Point(12, 3);

            Pen dpen = new Pen(Color.Black, 3);

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle((next.X + xx) * bwidth + 2, (next.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        graphics.DrawRectangle(dpen, now_rt);
                    }
                }
            }
        }
        private void DrawGraduation(Graphics graphics)
        {
            DrawHorizons(graphics);
            DrawVerticals(graphics);
        }

        private void DrawVerticals(Graphics graphics)
        {
            Point st = new Point();
            Point et = new Point();

            for (int cx = 0; cx <= bx; cx++)
            {
                st.X = cx * bwidth;
                st.Y = 0;
                et.X = st.X;
                et.Y = by * bheight;
                graphics.DrawLine(Pens.White, st, et);
            }
        }

        private void DrawHorizons(Graphics graphics)
        {
            Point st = new Point();
            Point et = new Point();

            for (int cy = 0; cy < by; cy++)
            {
                st.X = 0;
                st.Y = cy * bheight;
                et.X = bx * bwidth;
                et.Y = cy * bheight;
                graphics.DrawLine(Pens.White, st, et);
            }
        }

        private void MoveTurn()
        {
            if (game.MoveTurn())
            {
                Region rg = MakeRegion();
                Invalidate(rg);
            }
        }

        private void MoveLeft()
        {
            if (game.MoveLeft())
            {
                Region rg = MakeRegion(1, 0);
                Invalidate(rg);
            }
        }

        private void MoveRight()
        {
            if (game.MoveRight())
            {
                Region rg = MakeRegion(-1, 0);
                Invalidate(rg);
            }
        }

        private void MoveDown()
        {
            if (game.MoveDown())
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
            }
            else
            {
                EndingCheck();
            }
        }

        private void MoveSSDown()
        {
            while (game.MoveDown())
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
            }
            EndingCheck();
        }

        private void MoveBack()
        {
            if (rewCnt <= 0) return;

            if(game.MoveBack())
            {
                Region rg = MakeRegion(game.fDirX, game.fDirY);
                Invalidate(rg);
                rewCnt--;
                RewCnt.Text = $"되돌리기 남은 횟수: {rewCnt}";
            }
        }

        private void Restart()
        {
            score = 0;
            ScoreTxt.Text = $"현재점수: {score}";
            timer1.Interval = 1000;
            timer1.Enabled = true;

            rewCnt = maxRewCnt;
            RewCnt.Text = $"되돌리기 남은 횟수: {rewCnt}";
        }

        private void EndingCheck()
        {
            if (game.Next())
            {
                Invalidate(); //전체 영역 갱신
                checkCurBlockLine = false;
                game.ClearData();
                //score += 10;
                ScoreTxt.Text = $"현재점수: {score}";
                if(timer1.Interval>=400)
                   timer1.Interval -= 10;
                beforeIdx = colorIndex;
                sp.Play();
            }
            else
            {
                timer1.Enabled = false;
                
                if(score>bestScore)
                {
                    bestScore=score;
                    BestTxt.Text = $"최고점수: {bestScore}";
                    DateTxt.Text = DateTime.Now.ToString();
                }
                Save();

                if (DialogResult.Yes == MessageBox.Show($"계속 하실건가요?\n나의 점수: [{score}]", "계속 진행 확인 창", MessageBoxButtons.YesNo))
                {
                    game.ReStart();
                    Restart();
                    Invalidate();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private Region MakeRegion(int cx, int cy) //갱신할 영역 계산
        {
            Point now = game.NowPosition;

            int bn = game.BlockNum;
            int tn = game.Turn;
            Region region = new Region();
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        Rectangle rect2 = new Rectangle((now.X + cx + xx) * bwidth, (now.Y + cy + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        Region rg2 = new Region(rect2);
                        region.Union(rg1);
                        region.Union(rg2);
                    }
                }
            }
            return region;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private Region MakeRegion() 
        {
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;
            int oldtn = (tn + 3) % 4;

            Region region = new Region();
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                    if (BlockValue.bvals[bn, oldtn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                }
            }
            return region;
        }

    }
}
