using System;

namespace _LinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            _LinkedList<string> linkedList = new _LinkedList<string>();
            linkedList.AddFirst("300");
            linkedList.AddBefore(linkedList.head, "100");
            linkedList.AddLast("400");
            linkedList.AddAfter(linkedList.Find("100"),"200");
            string str = "500";
            if (!linkedList.Contains(str))
            {
                linkedList.AddLast(str);
            }
            linkedList.AddAfter(linkedList.Last, "600");
            linkedList.AddBefore(linkedList.First, "000");
            Console.Write($"List elements: ");
            foreach (var item in linkedList)
            {
                Console.Write($"{item} ");
            }
            int count = linkedList.count;
            Console.WriteLine($"\nList elements count is {count}");
            linkedList.Remove("000");
            linkedList.RemoveFirst();
            linkedList.RemoveLast();
            Console.Write($"List elements: ");
            foreach (var item in linkedList)
            {
                Console.Write($"{item} ");
            }
            count = linkedList.count;
            Console.WriteLine($"\nList elements count is {count}");
            
            Console.ReadKey();
        }
    }
}
