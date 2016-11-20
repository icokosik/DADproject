using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void start() 
        {
            TcpChannel channel = new TcpChannel(10001);

            RemotingConfiguration.RegisterWellKnownServiceType(
            typeof(LogService),
                           "log",
                           WellKnownObjectMode.Singleton);

            LogService obj = (LogService)Activator.GetObject(
                         typeof(LogService),
                         "tcp://localhost:10001/log");

            if (obj == null)
            {
                Console.WriteLine("Could not locate server");
            }
            else
            {
                Console.WriteLine("LogThread connected...");
            }

            //waiting for data from OP 
            while(true)
            {
                lock (obj.thisLock) //critical section
                {
                    if (obj.getLogData() != null)
                    {
                        saveToFile(obj.getLogData());
                        obj.Clear();
                    }
                } Thread.Sleep(10);               
            }
        }

        public void saveToFile(List<string> logLines)
        {
            foreach(string line in logLines)
            {
                string text = "tuple " + line + Environment.NewLine;
                System.IO.File.AppendAllText("LoggingFile.txt", text);
            }
        }
    }
}
