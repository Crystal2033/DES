using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DES.SandBox
{
    public sealed class ThreadBarierTesting
    {
        public void test()
        {
            string myString = "Hello world, my dear Paul";
            bool isMainThreadSleep = true;

            Barrier barrier = new Barrier(3, (bar) =>
            {
                Console.WriteLine($"HEEEEEY {bar.CurrentPhaseNumber}");
            });

            MyThread mt1 = new MyThread(barrier);
            MyThread mt2 = new MyThread(barrier);
            MyThread mt3 = new MyThread(barrier);

            List<Task> myTasks = new List<Task>();
            myTasks.Add(Task.Run(() =>
            {
                mt1.Run(myString);
            }));
            myTasks.Add(Task.Run(() =>
            {
                mt2.Run(myString);
            }));
            myTasks.Add(Task.Run(() =>
            {
                mt3.Run(myString);
            }));

            Task.WaitAll(myTasks.ToArray());
            
            //ThreadPool.QueueUserWorkItem(mt1.Run, myString);
            //ThreadPool.QueueUserWorkItem(mt2.Run, myString);
            //ThreadPool.QueueUserWorkItem(mt3.Run, myString);

            //while (isMainThreadSleep)
            //{
            //    barrier.SignalAndWait();
            //}
            //barrier.Dispose();

        }

        private void threadWork(string workingString)
        {

        }
    }
}
