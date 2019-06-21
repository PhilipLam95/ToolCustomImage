using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolCusTomImage
{
    public class CallThread
    {

        public int _numThreads = 0;
        public int _spinTime;

        public CallThread(int SpinTime)
        {
            this._spinTime = SpinTime;
        }

        //Call Addthreads(int numThreads) before executing a thread(s).

        public void AddThreads(int numThreads)
        {
            _numThreads += numThreads;
        }


        //Call RemoveThread() after each one has completed.
        public void RemoveThread()
        {
            if (_numThreads > 0)
            {
                _numThreads--;
            }
        }

        //Use Wait() at the point that you want to wait for all the threads to complete before continuing
        public void Wait()
        {
            while (_numThreads != 0)
            {
                System.Threading.Thread.Sleep(_spinTime);
            }
        }
    }
}
