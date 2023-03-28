using DES.ThreadingWork;
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
            bool isEnd = false;
            List<Task> myTasks = new List<Task>();
            MyThread[] myThreads = new MyThread[ThreadsInfo.VALUE_OF_THREAD];

            Barrier barrier = new Barrier(ThreadsInfo.VALUE_OF_THREAD, (bar) =>
            {
                Console.WriteLine($"HEEEEEY {bar.CurrentPhaseNumber}");
                if(bar.CurrentPhaseNumber == 10)
                {
                    for(int i = 0; i < myThreads.Length; i++)
                    {
                        myThreads[i].IsEnd = true;
                    }
                }
            });

            

            for(int i = 0; i < myThreads.Length; i++)
            {
                myThreads[i] = new MyThread(barrier);
                myTasks.Add(Task.Run(() =>
                {
                    myThreads[i].Run(myString);
                }));
            }

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
