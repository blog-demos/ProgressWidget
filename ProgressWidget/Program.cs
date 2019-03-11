using System;
using System.Threading;
using ProgressWidget.Core;

namespace ProgressWidget
{
    class Program
    {
        static void Main(string[] args)
        {
            DriveWidget widget = new DriveWidget();
            widget.Add(new TaskHandler((s) =>
            {
                Console.WriteLine("开始执行[1]");
                Thread.Sleep(1000);
                Console.WriteLine("[1]执行完成");
            }), 10);
            widget.Add(new TaskHandler((s) =>
            {
                Console.WriteLine("开始执行[2]");
                Thread.Sleep(2000);
                Console.WriteLine("[2]执行完成");
            }), 40);
            widget.Add(new TaskHandler((s) =>
            {
                Console.WriteLine("开始执行[3]");
                Thread.Sleep(1000);
                Console.WriteLine("[3]执行完成");
            }), 10);
            widget.Add(new MultipleTasksHandler((s, t, weight) =>
            {
                Console.WriteLine("开始执行[4]");
                for (int i = 0; i < 100; i++)
                {
                    t(weight / 100.0);
                    Thread.Sleep(100);
                }

                Console.WriteLine("[4]执行完成");
            }), 50);
            widget.OnProgressChanged += (s, p) =>
            {
                Console.WriteLine(String.Format("{0}%", p));
            };
            widget.OnProgressCompleted += (s, p) =>
            {
                Console.WriteLine(String.Format("已完成 {0}%", p));
            };
            widget.Start();

            Console.ReadKey();
        }
    }
}
