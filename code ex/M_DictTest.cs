using System;
using static System.Console;

//Simple
namespace MDicTest
{
    public class Node<V>
    {
        public V? data;
        public Node<V>? nextData;

        public Node() { }
        public Node(V v)
        {
            data = v;
        }
    }

    public class SDict<K, V>
    {
        private readonly int maxLength = 5000;

        public Node<V>[]? values;

        public SDict()
        {
            values = new Node<V>[maxLength];
        }

        public V this[K k]
        {
            get
            {
                return values[GetHashCode(k)].data;
            }
            set
            {
                Add(k, value);
            }
        }

        public int GetHashCode(K k) => Math.Abs(k.GetHashCode()) % maxLength;

        public void Add(K k, V v)
        {
            int hashCode = GetHashCode(k);
            if (values[hashCode] == null)
            {
                values[hashCode] = new Node<V>(v);
            }
            else
            {
                values[hashCode].data = v;
            }
        }

        public V Get(K k)
        {
            return values[GetHashCode(k)].data;
        }

        public bool ContainKey(K k)
        {
            int code = GetHashCode(k);
            return values[code] != null;
        }

        public void Remove(K k)
        {
            values[GetHashCode(k)] = null;
        }
    }

    class Program
    {
        static void Main()
        {
            SDict<string, float> dic = new SDict<string, float>();
            dic.Add("test 1", 0.4f);
            dic.Add("Test 2", 0.9f);
            WriteLine(dic.Get("test 1"));
            WriteLine(dic.ContainKey("yyy"));
            WriteLine(dic.ContainKey("Test 2"));
            dic.Remove("Test 2");
            WriteLine(dic.ContainKey("Test 2"));
            dic.Add("test 1", 4);
            WriteLine(dic.Get("test 1"));
            WriteLine(dic["test 1"]);
            dic["test 1"] = 5.5f;
            WriteLine(dic["test 1"]);
        }
    }
}
