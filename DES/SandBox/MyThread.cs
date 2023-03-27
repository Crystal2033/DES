using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.SandBox
{
    public sealed class MyThread
    {
        private static int absID;
        private int id;
        Barrier _barrier;
        public MyThread(Barrier barrier)
        {
            absID++;
            id = absID;
            _barrier = barrier;
        }
        public void Run(object workingObj)
        {
            string workStr = workingObj as string;
            for(int i =0; i < 100; i++)
            {
                Console.WriteLine($"Thread {id} with text: {workStr} is {i + 1}");
                _barrier.SignalAndWait();
                //Console.WriteLine($"Unblocked {id}");
            }
            
        }
    }
}
