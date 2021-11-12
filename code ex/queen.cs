using System;
using static System.Console;
using System.Collections.Generic;

namespace ConsoleApp1Sh //퀸 n개 안겹치게 배치
{

    class Program
    {
        public List<List<char>> board;
        public List<bool> lineQ = new List<bool>();

        public class Trace
        {
            public int i;
            public int j;
            public List<List<char>> board = new List<List<char>>();

            public Trace(int i, int j)
            {
                this.i = i;
                this.j = j;
            }

            public void AddBoard(List<List<char>> li, int n)
            {
                for (int i = 0; i < n; i++)
                {
                    List<char> list = new List<char>();
                    for (int j = 0; j < n; j++)
                    {
                        list.Add(li[i][j]);
                    }
                    board.Add(list);
                }
            }

        }

        public Stack<Trace> traceStack = new Stack<Trace>();

        public void CreateBoard(int n)
        {
            board = new List<List<char>>();

            for (int i = 0; i < n; i++)
            {
                List<char> list = new List<char>();
                for (int j = 0; j < n; j++)
                {
                    list.Add('X');
                }
                board.Add(list);

                lineQ.Add(false);
            }
        }

        public void PrintBoard()
        {
            Write("N 입력: ");
            int n = int.Parse(ReadLine());
            CreateBoard(n);

            int k, t;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (board[i][j] == 'X' && !lineQ[i])
                    {

                        lineQ[i] = true;
                        Trace tr = new Trace(i, j);

                        board[i][j] = 'Q';
                        tr.AddBoard(board, n);

                        for (k = 0; k < n; k++)
                        {
                            if (board[k][j] == 'X')
                            {
                                board[k][j] = 'Z';  //세로 차지
                            }
                        }

                        for (k = 0; k < n; k++)  //대각선 차지
                        {
                            for (t = 0; t < n; t++)
                            {
                                if (board[k][t] == 'X' && MathF.Abs(k - i) == MathF.Abs(t - j))
                                {
                                    board[k][t] = 'Z';
                                }
                            }
                        }

                        traceStack.Push(tr);

                        break;
                    }
                }
                if (!lineQ[i] && traceStack.Count > 0)
                {
                    Trace tr = traceStack.Pop();
                    lineQ[i - 1] = false;
                    i = tr.i - 1;

                    board = tr.board;

                    board[tr.i][tr.j] = 'Z';
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (board[i][j] == 'Z')
                    {
                        board[i][j] = 'X';
                    }
                }
            }

            board.ForEach(x = > { x.ForEach(x = > Write(x + " ")); WriteLine(); });
        }

        static void Main(string[] args)
        {
            Program p = new Program();

            p.PrintBoard();
        }
    }
}

