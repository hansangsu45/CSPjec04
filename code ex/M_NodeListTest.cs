using static System.Console;

namespace MDicTest
{
    public class Node<T>
    {
        public T? data;
        public Node<T>? next;
        public Node<T>? prev;

        public Node() { }
        public Node(T data, Node<T> next, Node<T> prev)
        {
            this.data = data;
            this.next = next;
            this.prev = prev;
        }
    }

    public class NodeList<T>   //Circle
    {
        private Node<T> head;

        private int size;

        public int Count => size;

        public T this[int index]
        {
            get
            {
                if (size == 0) return default;

                Node<T> node = head;
                for (int i = 0; i < index; i++)
                {
                    node = node.next;
                }
                return node.data;
            }
        }

        public NodeList()
        {
            size = 0;
        }

        public void Add(T data)
        {
            if (size == 0)
            {
                head = new Node<T>(default, null, null);
            }
            else
            {
                Node<T> newNode = new Node<T>(data, null, null);

                if (size == 1)
                {
                    head.next = newNode;
                    head.prev = newNode;
                    newNode.prev = head;
                    newNode.next = head;
                }
                else
                {
                    Node<T> pre = head.prev;

                    head.prev = newNode;
                    newNode.prev = pre;
                    pre.next = newNode;
                    newNode.next = head;
                }
            }
            size++;
        }

        public void RemoveAt(int index)
        {
            if (size == 0) return;

            if (size == 1)
            {
                size = 0;
                head = null;
                return;
            }

            Node<T> node = head;
            for (int i = 0; i < index; i++)
            {
                node = node.next;
            }

            node.prev.next = node.next;
            node.next.prev = node.next;

            if (index == 0) head = head.next;

            size--;
        }

        public void Remove(T data)
        {
            if (size == 0) return;

            Node<T> node = head;
            for (int i = 0; i < size; i++)
            {
                if (node.data.Equals(data))
                {
                    RemoveAt(i);
                    return;
                }
                node = node.next;
            }
        }

        public bool Contain(T data)
        {
            if (size == 0) return false;

            Node<T> node = head;
            for (int i = 0; i < size; i++)
            {
                if (node.data.Equals(data))
                    return true;
                node = node.next;
            }
            return false;
        }
    }

    class Program
    {
        static void Main()
        {
            NodeList<uint> nli = new NodeList<uint>();
            nli.Add(3);
            nli.Add(6);
            nli.Add(14);

            WriteLine(nli.Count);
            WriteLine(nli.Contain(6));
            WriteLine(nli.Contain(50));
            WriteLine(nli[1]);
            WriteLine(nli[2]);

            nli.RemoveAt(1);
            WriteLine(nli.Count);
            WriteLine(nli[1]);

            nli.RemoveAt(0);
            WriteLine(nli[0]);

            for (ushort i = 100; i < 150; i += 5)
            {
                nli.Add(i);
            }

            for (int i = 0; i < nli.Count; i++)
            {
                Write(nli[i]);
            }
            WriteLine();

            WriteLine(nli.Count);
            nli.Remove(14);
            WriteLine(nli[0]);
            WriteLine(nli.Count);
        }
    }
}
