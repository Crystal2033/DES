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
        public void run(object workingObj)
        {
            string workStr = workingObj as string;
            for(int i =0; i < 10; i++)
            {
                Console.WriteLine($"Thread {id} with text: {workStr}");
                _barrier.SignalAndWait();
                Console.WriteLine($"Unblocked {id}");
            }

        }
    }
}
