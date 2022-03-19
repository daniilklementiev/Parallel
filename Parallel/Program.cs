using System;
using System.Xml;

namespace Parallel
{
    class Program
    {
        public DateTime dt = DateTime.Now;

        static void Main(string[] args)
        {
            Thread t;
            t = new Thread(ThreadProc);
            t.Start();
            t.Join();

            CancellationTokenSource cts = new CancellationTokenSource();
            t = new Thread(ThreadProc3);
            t.Start(cts.Token);

            t = new Thread(ThreadProc2);
            t.Start(2);
            t.Join();



            t = new Thread(ThreadProc2);
            t.Start(new Multidata { X = 10, Y = 20 });
            t.Join();
            Console.WriteLine("Hello Threads");

            cts.Cancel();
        }

        private static void ThreadProc()
        {
            Console.WriteLine("Thread proc 1");
        }

        private static void ThreadProc2(object? par)
        {
            Console.WriteLine("Thread proc " + par);
        }

        private static void ThreadProc3(object? par)
        {
            if (par == null) return;
            CancellationToken token = (CancellationToken)par;
            Console.WriteLine("ThreadProc3 Start");
            for (int i = 0; i < 100; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("ThreadProc3 cancelled");
                    return;
                }
                Thread.Sleep(10);
            }
            Console.WriteLine("ThreadProc3 Stop");

        }
    }

    class Multidata
    {
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString()
        {
            return $"X = {X}; Y = {Y}";
        }
    }
}