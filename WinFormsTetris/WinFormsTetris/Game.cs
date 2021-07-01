using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinFormsTetris
{
    class FormerPosition
    {
        public int x;
        public int y;
        public int t;

        public FormerPosition(int x, int y, int t)
        {
            this.x = x;
            this.y = y;
            this.t = t;
        }
    }

    class Game
    {        
        Board gboard = Board.GameBoard;
        public Diagram now;
        public Diagram next;

        public List<FormerPosition> fPosList = new List<FormerPosition>();

        public int fDirX=0;
        public int fDirY = 0;

        public Point NowPosition
        {
            get { return new Point(now.X, now.Y); }
        }
        public int BlockNum
        {
            get { return now.BlockNum; }
        }
        public int Turn
        {
            get { return now.Turn; }
        }

        public static Game Singleton { get; }

        public int this[int x, int y]
        {
            get { return gboard[x, y]; }
        }

        static Game()
        {
            Singleton = new Game();
        }
        Game()
        {
            now = new Diagram();
            next = new Diagram();
            next.Reset();
            now.Reset();
        }

        public bool MoveLeft()
        {
            if (gboard.MoveEnable(now.BlockNum, Turn, now.X - 1, now.Y))
            {
                fPosList.Add(new FormerPosition(1, 0, now.Turn));
                now.MoveLeft();
                return true;
            }
            return false;
        }

        public bool MoveRight()
        {
            if (gboard.MoveEnable(now.BlockNum, Turn, now.X + 1, now.Y))
            {
                fPosList.Add(new FormerPosition(-1, 0, now.Turn));
                now.MoveRight();
                return true;
            }
            return false;
        }

        public bool MoveDown()
        {
            if (gboard.MoveEnable(now.BlockNum, Turn, now.X, now.Y + 1))
            {
                fPosList.Add(new FormerPosition(0, -1, now.Turn));
                now.MoveDown();
                return true;
            }
            gboard.Store(now.BlockNum, Turn, now.X, now.Y); //보드 정보 갱신
            return false;
        }

        public bool MoveTurn()
        {
            if (gboard.MoveEnable(now.BlockNum, (Turn + 1) % 4, now.X, now.Y))
            {
                now.MoveTurn();
                fPosList.Add(new FormerPosition(0, 0, now.Turn));
                return true;
            }
            return false;
        }

        public bool MoveBack()
        {
            if (fPosList.Count == 0) return false;

            FormerPosition fp = fPosList[fPosList.Count - 1];

            if (now.Y + fp.y <= GameRule.SY) return false;

            if(gboard.MoveEnable(now.BlockNum,Turn,now.X+fp.x,now.Y+fp.y))
            {
                fDirX = fp.x;
                fDirY = fp.y;
                now.MoveBack(fDirX, fDirY, fp.t);
                fPosList.RemoveAt(fPosList.Count - 1);
                return true;
            }

            gboard.Store(now.BlockNum, Turn, now.X, now.Y);

            return false;
        }

        public bool Next()
        {
            now.Turn = next.Turn;
            now.BlockNum = next.BlockNum;

            now.X = GameRule.SX;
            now.Y = GameRule.SY;

            next.Reset();
           
            return gboard.MoveEnable(now.BlockNum, Turn, now.X, now.Y);
        }

        public void ReStart()
        {
            gboard.ClearBoard();
            fPosList.Clear();
        }

        public void ClearData() => fPosList.Clear();
    }
}
