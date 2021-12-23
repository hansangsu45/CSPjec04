using System;
using static System.Console;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Counter
    {
        const int LOOP_COUNT = 1000;
        private int count;
        public int Count
        {
            get { return count; }
        }
        readonly object thisLock;
        public Counter()
        {
            count = 0;
            thisLock = new object();
        }

        public void Increment()
        {
            int loopCount = LOOP_COUNT;
            while (loopCount-- > 0)
            {
                lock (thisLock) //Monitor클래스도 있다
                {
                    count--;
                }
            }
        }
        public void Decrement()
        {
            int loopCount = LOOP_COUNT;
            while (loopCount-- > 0)
            {
                lock (thisLock)
                {
                    count--;
                }
            }
        }
    }

    class Program
    {
        static void DoSomething()
        {
            for (int i = 0; i < 5; i++)
            {
                WriteLine($"DoSomething: {i}");
                Thread.Sleep(50);
            }
        }

        static void Calc(object radius)
        {
            double r = (double)radius;
            double area = r * r * 3.141592;
            WriteLine("r = {0}, area = {1}", r, area);
        }

        static void Sum(int d1, int d2, int d3)
        {
            int sum = d1 + d2 + d3;
            WriteLine(sum);
        }

        private static void PrintThreadState(ThreadState state)
        {
            WriteLine("{0,-16} : {1}", state, (int)state);
        }

        static void SomeMethod(long num)
        {
            for (int i = 0; i < num; i++)
            {

            }
            if (num == 100000 - 1) WriteLine(num);
        }

        static int CalcSize(string data)
        {
            string s = data == null ? "" : data.ToString();
            Thread.Sleep(3000);
            return s.Length;
        }

        static string PopUp(string data)
        {
            string msg = data;
            string rt = "";
            WriteLine(data);
            Thread.Sleep(3000);
            rt = "yes";
            //rt = "no";
            return rt;
        }

        async static void MyMethodAsync(int count)
        {
            WriteLine("S");
            await Task.Run(async () =>
            {
                for (int i = 1; i <= count; i++)
                {
                    await Task.Delay(1000);
                }
            });
            WriteLine("E");
        }

        async static Task PrintAnswerToLife()
        {
            await Task.Delay(5000);
            int answer = 21 * 2;
            WriteLine(answer);
        }

        async static Task Go()
        {
            await PrintAnswerToLife();
            WriteLine("Done");
        }

        static void Main(string[] args)
        {
            /*Thread t1 = new Thread(new ThreadStart(DoSomething));
            //Thread t1 = new Thread(DoSomething);
            t1.Start();

            //Thread t2 = new Thread(new ParameterizedThreadStart(Calc));
            Thread t2 = new Thread(Calc);
            t2.Start(10.5);

            Thread t3 = new Thread(()=>Sum(10,20,30));
            t3.Start();
            t3.IsBackground = true;

            PrintThreadState(t2.ThreadState);

            for (int i = 0; i < 5; i++)
            {
                WriteLine($"Main: {i}");
                Thread.Sleep(100);
            }

            t1.Join();
            t2.Join();
            WriteLine("FINISH");
            PrintThreadState(t2.ThreadState);
            //t1.Abort();
            //t1.Interrupt();*/

            /*Counter counter = new Counter();
            Thread inc = new Thread(counter.Increment);
            Thread dec = new Thread(counter.Decrement);
            inc.Start();
            dec.Start();

            inc.Join();
            dec.Join();
            WriteLine(counter.Count);*/

            /*  Task t1 = new Task(() => WriteLine("345"));
              t1.Start();
              t1.Wait();

              Task t2 = Task.Run(() =>
              WriteLine("3086"));
              t2.Wait();

              Task t3 = Task.Factory.StartNew(()=> WriteLine("308346"));
              t3.Wait();*/

            /*var tk = Task<List<int>>.Run(() =>
            {
                Thread.Sleep(1000);

                List<int> list = new List<int>()
                {
                    3,4,5
                };
                return list;
            });
           
            WriteLine("Main");
            tk.Wait();
            WriteLine("Main2");

            List<int> list = tk.Result;
            foreach(var a in list)
            {
                WriteLine(a);
            }*/

            /*for (int i = 0; i < 100000; i++)
            {
                SomeMethod(i);
            }

            Parallel.For(0, 100000, i =>
            {
                SomeMethod(i);
            });*/

            //SomeMethod(99999);

            /*Task<int> task = Task.Factory.StartNew<int>(() =>
            {
                return CalcSize("Hello World");
            });

            WriteLine(1);
            WriteLine(task.Result);
            WriteLine(3);
            //Thread.Sleep(1000);
            //WriteLine(2);

            Task<string> task2 = Task.Factory.StartNew<string>( () => PopUp("허락하시겠습니까?") );
            string result = task2.Result;
            WriteLine(result);*/

            /* WriteLine(1);
             MyMethodAsync(3);
             WriteLine(2);
             ReadLine();*/

            Go();
            WriteLine("Main");

            ReadLine();



        }


    }

}
