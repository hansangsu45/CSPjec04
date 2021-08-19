using System;
using System.Collections.Generic;
using static System.Console;

namespace ConsoleApp11
{
    class Program
    {
        static void Main(string[] args)
        {
            //WriteLine("Hello World!");
            //string s = ReadLine();

            //WriteLine(int.MaxValue + ", " + int.MinValue);
            //int a = 10, b = 5;
            //WriteLine((float)(a + b) / 2);
            //WriteLine((int)'s');
            //WriteLine((char)100);

            //int a = 100;
            //WriteLine(Convert.ToString(a,2));
            //WriteLine(Convert.ToString(a, 8));
            //WriteLine(Convert.ToString(a, 16));

            //int a = 84, b = 63, c = 70;
            //int d = a + b + c;
            //WriteLine(d);
            //WriteLine(((double)d / 3).ToString("0.00"));
            //WriteLine(Math.Round( (double)d / 3, 2));

            //TimeSpan ts = DateTime.Now - new DateTime(2000, 1, 1);
            //WriteLine((int)ts.TotalDays);

            //string s = "of the people, by the people, for the people";
            //string[] _s = s.Split(',');
            //foreach (string a in _s) WriteLine(a);

            /*int a;
            while(true)
            {
                string s = ReadLine();
                if (int.TryParse(s, out a)) break;
                else WriteLine("다시 입력");
            }
            WriteLine(a);*/

            //TimeSpan ts = TimeSpan.FromDays(365);
            //WriteLine(ts.TotalSeconds);

            /*for(int i=1; i<=100; i++)
            {
                //if (i % 2 == 0) continue;
                //Write(i + " ");

                if (i % 2 != 0) Write(i + " ");
            }*/

            /*int a = -20;

            if (a < 0) WriteLine("음수");
            else if (a > 0) WriteLine("양수");
            else WriteLine(0);*/

            /*while (true)
            {
                ConsoleKey k = ReadKey().Key;

                switch(k)
                {
                    case ConsoleKey.LeftArrow:
                        WriteLine("L");
                        break;
                    case ConsoleKey.RightArrow:
                        WriteLine("R");
                        break;
                    case ConsoleKey.UpArrow:
                        WriteLine("U");
                        break;
                    case ConsoleKey.DownArrow:
                        WriteLine("D");
                        break;
                }
            }*/

            /*string[] fruits = { "사과", "배", "포도", "수박", "딸기" };

            for(int i=0; i<fruits.Length; i++)
                WriteLine(fruits[i]);
            
            Array.Reverse(fruits);

            foreach (string s in fruits)
                WriteLine(s);*/

            /* string st = "고구마 토마토";
             char[] c=st.ToCharArray();

             Array.Reverse(c);

             string s2 = new string(c);
             WriteLine(s2);*/

            /*int num = Abs_V(-1);
            WriteLine(num);

            int a = -2;
            Abs_R(ref a);
            WriteLine(a);*/

            /*int s;
            bool b =Sum(2, -3, out s);
            WriteLine(s+" "+b);

            b = Sum(4, 5, out s);
            WriteLine(s + " " + b);*/

            //Sum(1);
            //Sum(1, 2);

            //string st = MaxString("abc", "owefho", "4g9hhowownog", "wfe4");
            //WriteLine(st);

            /*bool b = IsValid("NickName");
            bool b2 = IsValid("abc123");
            WriteLine(b + " " + b2);*/

            //WriteLine(MyMath.Abs(52));
            //WriteLine(MyMath.Abs(-273));

            /*Test a = new Test();
            //Test b = a;  //얕은 복사
            Test b = new Test();
            b.value = a.value;  //깊은 복사

            a.value = 10;
            b.value = 20;

            WriteLine(a.value + " " + b.value);

            a.Copy(b);*/

            //SquareCalculator square = new SquareCalculator();
            //WriteLine(square[10]);
        }

        /*public static int Abs_V(int n) => (int)MathF.Abs(n);
        *//*{
            if (n < 0) return -n;
            return n;
        }*//*

        public static void Abs_R(ref int n) 
        { 
            if (n < 0) n *= -1; 
        }*/

        /*static bool Sum(int a, int b, out int sum)
        {
            sum = a + b;
            return sum > 0;
        }*/

        //static void Sum(int a, int b=2) => WriteLine(a + b);

        /*static string MaxString(params string[] ss)
        {
            int n = -1;
            string t="";

            for(int i=0; i<ss.Length; i++)
            {
                if(ss[i].Length>n)
                {
                    n = ss[i].Length;
                    t = ss[i];
                }
            }

            return t;
        }*/

        /*static bool IsValid(string nick)
        {
            for(int i=0; i<nick.Length; i++)
            {
                if ((nick[i] >= '0' && nick[i] <= '9') || (nick[i] >= 'a' && nick[i] < 'z'))
                    continue;
                return false;
            }
            return true;
        }*/


        /*class MyMath
        {
            public static int Abs(int x)
            {
                if (x < 0) x = - x;
                return x;
            }
        }*/

        /*class Test
        {
            public int value = 0;

            public void Copy(Test t)
            {
                value = t.value;
            }
        }*/

        /*class MyPlayer
        {
            *//*private int level;

            public int Level
            {
                get
                {
                    return level;
                }
                set
                {
                    level = value;
                }
            }*//*

            public int Level { get; set; }
        }*/

        /*class SquareCalculator
        {
            public int this[int n]
            {
                get { return n * n; }
            }
        }*/
    }
}
