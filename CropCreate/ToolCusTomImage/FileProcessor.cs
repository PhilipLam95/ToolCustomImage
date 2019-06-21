using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolCusTomImage;

namespace ToolCusTomImage
{

    public sealed class FileProcessor
    {
        private Int32 m_Year;
        private Thread m_Thread;

        public Boolean IsAlive()
        {
            return ((m_Thread != null) && m_Thread.IsAlive);
        }

        public Boolean FinishedLoading()
        {
            return ((m_Thread == null) || m_Thread.Join(10));
        }

        public FileProcessor(Int32 year)
        {
            m_Year = year;

            m_Thread = new Thread(Load);
            m_Thread.Name = "Background File Processor";
        }

        public void Start()
        {
            if (m_Thread != null)
                m_Thread.Start();
        }

        public void Stop()
        {
            if ((m_Thread != null) && m_Thread.IsAlive)
                m_Thread.Abort();
        }

        private void Load()
        {
            // Browse the Year folder...
            // Get and read all fines one by one...
        }
    }
    public static class FilesProcessor
    {
        private static List<FileProcessor> m_FileProcessors;

        public static void Start()
        {
            m_FileProcessors = new List<FileProcessor>();

                for (Int32 year = 2005; year < DateTime.Now.Year; ++year)
                    InstanciateFileProcessor(year);

                while (!FinishedLoading())
                    Application.DoEvents();
        }

        public static void Stop()
        {
            foreach (FileProcessor processor in m_FileProcessors)
                processor.Stop();
    
            m_FileProcessors.Clear();
            m_FileProcessors = null;
        }

        private static Boolean FinishedLoading()
        {
            foreach (FileProcessor processor in m_FileProcessors)
            {
                if (processor.IsAlive() && !processor.FinishedLoading())
                    return false;
            }

            return true;
        }

        private static void InstanciateFileProcessor(Int32 year)
        {
            FileProcessor processor = new FileProcessor(year);
            processor.Start();

            m_FileProcessors.Add(processor);
        }
    }



    
}
