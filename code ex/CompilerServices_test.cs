using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Runtime.CompilerServices;

namespace ConsoleApp11
{
    class Class001
    {
        [Obsolete("OldMethod�� ���Ǿ����ϴ�. NewMethod�� �̿��ϼ���.")]
        public void OldMethod()
        {
            WriteLine("I'm old");
        }

        /// <summary>
        /// �� �Լ�
        /// </summary>
        public void NewMethod()
        {
            WriteLine("I'm new");
        }
    }
    public static class Trace
    {
        public static void WriteLine(string message,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            Console.WriteLine($"{file}(Line:{line}) {member}: {message}");
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    class History : Attribute
    {
        private string programmer;
        public string Programmer { get { return programmer; } }
        public double Version { get; set; }
        public string Changes { get; set; }

        public History(string programmer)
        {
            this.programmer = programmer;
            Version = 1.0;
            Changes = "First release";
        }
    }
    [History("Name", Version = 0.1, Changes = "20211209")]
    [History("Name", Version = 0.2, Changes = "20211210")]
    class C____
    {
        public void Func()
        {
            string _ = "Func()";
            WriteLine(_);
        }
    }
    class Class7
    {
        static void Main(string[] args)
        {
            //Class001 c = new Class001();
            //c.OldMethod();
            //c.NewMethod();

            //Trace.WriteLine("���α׷���");

            Type type = typeof(C____);
            Attribute[] attributes = Attribute.GetCustomAttributes(type);

            WriteLine("MyClass change history...");

            foreach (Attribute a in attributes)
            {
                History h = a as History;
                if (h != null)
                    WriteLine("Ver:{0}, Programmer:{1}, Changes:{2}", h.Version, h.Programmer, h.Changes);
            }
        }
    }
}