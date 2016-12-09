using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DADstorm
{
    class ThreadLog
    {
        private LoggingLevel logging;

        public ThreadLog(LoggingLevel logging)
        {
            this.logging = logging;
        }

        public void start() 
        {
            LogService.logging = logging;
            
            // Get up to 30 lines, log them to a file and wait a while
            while(true)
            {
                List<string> toBeLogged = new List<string>();
                while (LogService.queuedData.Count > 0 && toBeLogged.Count < 3)
                {
                    toBeLogged.Add(LogService.queuedData.Dequeue());
                }
                saveToFile(toBeLogged);
                Thread.Sleep(2000);
            }
        }

        public void saveToFile(List<string> logLines)
        {
            foreach(string line in logLines)
            {
                string text = line + Environment.NewLine;
                System.IO.File.AppendAllText("..\\..\\doc\\log.txt", text);
            }
        }
    }
}
