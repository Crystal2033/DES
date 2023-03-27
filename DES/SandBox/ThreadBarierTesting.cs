using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.SandBox
{
    public sealed class ThreadBarierTesting
    {
        public void test()
        {
            string myString = "Hello world, my dear Paul";

            Barrier barrier = new Barrier(3, (data) =>
            {
                Console.WriteLine($"HEEEEEY {data.CurrentPhaseNumber}");
                
            });

            MyThread mt1 = new MyThread(barrier);
            MyThread mt2 = new MyThread(barrier);
            MyThread mt3 = new MyThread(barrier);
            

            ThreadPool.QueueUserWorkItem(mt1.run, myString);
            ThreadPool.QueueUserWorkItem(mt2.run, myString);
            ThreadPool.QueueUserWorkItem(mt3.run, myString); 

            Console.ReadLine();

            

        }

        private void threadWork(string workingString)
        {

        }
    }
}
