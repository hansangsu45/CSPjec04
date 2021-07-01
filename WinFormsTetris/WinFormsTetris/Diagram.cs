using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsTetris
{
    class Diagram       //테트리스 떨어지는 블럭, 다이아그램
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Turn { get; set; }
        public int BlockNum { get; set; }

        public Diagram()
        {
            Reset();
        }

        public void Reset()   // 다이아그램 생성
        {
            Random rand = new Random();
            X = GameRule.SX;
            Y = GameRule.SY;
            Turn = rand.Next() % 4;
            BlockNum = rand.Next() % 7;
        }

        public void MoveLeft()  { X--; }
        public void MoveRight() { X++; }
        public void MoveDown()  { Y++; }
        public void MoveTurn()  { Turn = (Turn + 1) % 4; }

        public void MoveBack(int x, int y, int t)
        {
            X += x;
            Y += y;
            Turn = t;
        }
    }
}
