using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolCusTomImage.Presentation_Layer.IMAGE;
using ToolCusTomImage;

namespace ToolCusTomImage.Common
{
    class ResizeImage
    {


        public static int Count;
        public static Thread Thrd;



        //public void callThread_ResizeImage(int numberthread, string name,int numberThreads)
        //{
        //    Count = 0;
        //    Thrd = new Thread(Crop_Canvasize.Canvasize_Imagẹ̣̣);
        //    Thrd.Name = name+"_"+ numberthread.ToString();
        //    Thrd.Start(numberthread);
        //    ToolCusTomImage.CallThread callthread = new ToolCusTomImage.CallThread(1000);
        //    callthread.AddThreads(numberThreads);
        //    Thread.Sleep(100);
        //}

        /// <summary>
        /// Auto lấy số thread để xử lý file (ưu tiên khi nhiều file)
        /// </summary>
        /// <param name="numberFile"> folder directory</param>
        /// <param name="fileOnThread"> </param>
        public static int autoGetNumberFileOneThread(int numberFile, int fileOnThread )
        {
            return numberFile > fileOnThread ? (int)Math.Ceiling(float.Parse(numberFile.ToString()) / fileOnThread) : 1;
        }



        // Entry point of thread.
        //void Run()
        //{
        //    Console.WriteLine(Thrd.Name + " starting.");
        //    do
        //    {
        //        Thread.Sleep(500);
        //        Console.WriteLine("In " + Thrd.Name +
        //        ", Count is " + Count);
        //        Count++;
        //    } while (Count < 10);
        //    Console.WriteLine(Thrd.Name + " terminating.");
        //}

    }
}
