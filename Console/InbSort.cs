using System;
using System.Collections.Generic;

/*namespace ConsoleApp3
{
    interface ILogger
    {
        void WriteLog(string log);
    }
    interface IItem
    {
        void Reinforce();
        void LevelUp();
    }


    class Program : ILogger
    {
        static void Main(string[] args)
        {
            
        }

        public void WriteLog(string log)
        {
            Console.WriteLine(log);
        }
    }
}
*/

/*interface IRunnable
{
    void Run();
}

interface IFlyable
{
    void Fly();
}

class FlyingCar : IRunnable, IFlyable
{
    public void Run()
    {
        Console.WriteLine("Run! Run!");
    }

    public void Fly()
    {
        Console.WriteLine("Fly! Fly!");
    }
}
class MainApp
{
    static void Main(string[] args)
    {
        FlyingCar car = new FlyingCar();
        car.Run();
        car.Fly();

        IRunnable runnable = car as IRunnable;
        runnable.Run();

        IFlyable flyable = car as IFlyable;
        flyable.Fly();
    }
}*/

class Dummy : IDisposable
{
    public void Dispose()
    {
        Console.WriteLine("Dispose() 메서드를 호출합니다.");
    }
}

class CP
{
    public string en;
    public string ko;

    public CP(string e = "", string k = "")
    {
        en = e;
        ko = k;
    }
}

public class App1
{
    static void Main(string[] args)
    {
        Dummy dummyA = new Dummy();
        dummyA.Dispose();

        using (Dummy dummyB = new Dummy())
        {

        }

        var a = new { Name = "홍길동", Age = 17 };
        Console.WriteLine(a.Name + a.Age);

        int[,,] array = new int[4, 3, 2];

        int[][] jagged = new int[3][];

        jagged[0] = new int[5] { 1, 2, 3, 4, 5 };
        jagged[1] = new int[] { 10, 20, 30 };
        jagged[2] = new int[] { 100, 200 };


        int[][] jagged2 = new int[2][] { new int[] { 1, 2 }, new int[4] { -1, -2, 0, 1000 } };

        foreach (int num in GetNumber())
        {
            Console.WriteLine(num);
        }

        Console.WriteLine("--------------------------------------------------------");

        List<CP> c1 = new List<CP>() { new CP("B"), new CP("H"), new CP("A") };
        List<CP> c2 = new List<CP>() { new CP("", "라"), new CP("", "나"), new CP("", "박") };

        foreach (CP c in c1) Console.Write(c.en + ", ");
        Console.WriteLine();
        foreach (CP c in c2) Console.Write(c.ko + ", ");

        c1.Sort((x, y) => x.en.CompareTo(y.en));
        c2.Sort((a, b) => a.ko.CompareTo(b.ko));

        Console.WriteLine();
        foreach (CP c in c1) Console.Write(c.en + ", ");
        Console.WriteLine();
        foreach (CP c in c2) Console.Write(c.ko + ", ");
    }

    static IEnumerable<int> GetNumber()
    {
        yield return 10;
        yield return 20;
        yield return 30;
    }

    /* class MyEnumerator
     {
         int[] numbers = { 1, 2, 3, 4 };
         public IEnumerator GetEnumerator()
         {
             yield return numbers[0];
             yield return numbers[1];
             yield return numbers[2];
             yield break;
             yield return numbers[3];
         }
     }
     class MainApp
     {

         static void Main(string[] args)
         {
             var obj = new MyEnumerator();
             foreach (int i in obj)
                 Console.WriteLine(i);
         }
     }*/
}