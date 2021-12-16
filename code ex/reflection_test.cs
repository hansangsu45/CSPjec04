using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Reflection;

namespace ConsoleApp11
{
    class Class55
    {
        int num = 0;

        public Class55() { num = 1; }
    }

    class Class6
    {
        //Class55 a = new Class55();

        class Profile
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public void Print()
            {
                WriteLine($"{Name},{Phone}");
            }
        }
        static void PrintInterfaces(Type type)
        {
            Console.WriteLine("-------- Interfaces -------- ");

            Type[] interfaces = type.GetInterfaces();
            foreach (Type i in interfaces)
                Console.WriteLine("Name:{0}", i.Name);

            Console.WriteLine();
        }

        static void PrintFields(Type type)
        {
            Console.WriteLine("-------- Fields -------- ");

            FieldInfo[] fields = type.GetFields(
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                String accessLevel = "protected";
                if (field.IsPublic) accessLevel = "public";
                else if (field.IsPrivate) accessLevel = "private";

                WriteLine("Access:{0}, Type:{1}, Name:{2}",
                    accessLevel, field.FieldType.Name, field.Name);
            }

            WriteLine();
        }

        static void PrintMethods(Type type)
        {
            Console.WriteLine("-------- Methods -------- ");

            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo method in methods)
            {
                Console.Write("Type:{0}, Name:{1}, Parameter:",
                    method.ReturnType.Name, method.Name);

                ParameterInfo[] args = method.GetParameters();
                for (int i = 0; i < args.Length; i++)
                {
                    Console.Write("{0}", args[i].ParameterType.Name);
                    if (i < args.Length - 1)
                        Console.Write(", ");
                }
                WriteLine();
            }
            Console.WriteLine();
        }

        static void PrintProperties(Type type)
        {
            Console.WriteLine("-------- Properties -------- ");

            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
                WriteLine("Type:{0}, Name:{1}",
                  property.PropertyType.Name, property.Name);

            Console.WriteLine();
        }
        static void Main()
        {
            string a = "aaa";
            Type t = a.GetType();

            PrintInterfaces(t);
            PrintFields(t);
            PrintProperties(t);
            PrintMethods(t);

            Type type = typeof(Profile);
            Profile profile = (Profile)Activator.CreateInstance(type);
            profile.Name = "Peter";
            profile.Phone = "010-1234-5678";

            MethodInfo method = type.GetMethod("Print");
            method.Invoke(profile, null);
        }
    }
}

/*
    // GetType() ------------------------------------------------
    int a = 0;
    Type type = a.GetType();
    FieldInfo[] fields = type.GetFields();

    foreach (FieldInfo field in fields)
    {
        Console.WriteLine($"Type:{field.FieldType.Name} Name:{field.Name}");
    }

    //Activator.CreateInstance ---------------------------------

    //object num = Activator.CreateInstance(typeof(int));
    //List<int> list = Activator.CreateInstance<List<int>>();


    //---------------------------------------------------------
    class Profile
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public void Print()
        {
            Console.WriteLine($"{Name}, {Phone}");
        }
    }
    static void Main()
    {
        Type type = typeof(Profile);

        Profile profile = (Profile)Activator.CreateInstance(type);
        profile.Name = "Peter";
        profile.Phone = "010-1234-5678";

        MethodInfo method = type.GetMethod("Print");

        method.Invoke(profile, null); //매개변수가 없으므로 null
    } 
*/
