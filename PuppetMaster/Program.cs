using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace DADstorm
{
    class Program
    {
        public static LoggingLevel logging = LoggingLevel.LIGHT;
        public static ConfigFile config;
       public static List<OperatorInformation> operatorsArray;
        public static List<SourceOPs> sourceoperators;
     //   public static List<OperatorCreationInformation> operatorsArray;
        public static int portnumber = 12000;

        static void Main(string[] args)
        {
            sourceoperators = new List<SourceOPs>();

            System.IO.File.Delete("LoggingFile.txt");

            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);
            loadConfigFile();

            //add to PM SourceOPs list
            foreach (var x in operatorsArray)
            {
                sourceoperators.Add(new SourceOPs(x.name, portnumber));
                portnumber++;
            }

            foreach (var x in operatorsArray)
            {
                foreach(var y in sourceoperators)
                    if(x.name==y.name)
                    {
                        Console.WriteLine("Starting new Operator in Thread with port: " + y.portnumber);
                        x.setPort(y.portnumber);
                        ThreadOperator op1 = new ThreadOperator(y.portnumber,x,sourceoperators);
                        Thread t1 = new Thread(new ThreadStart(op1.start));
                        t1.Start();
                    }
            }
           
            if (logging == LoggingLevel.FULL)
            {
                ThreadLog log = new ThreadLog();
                Thread t2 = new Thread(new ThreadStart(log.start));
                t2.Start();
            }
          
            Console.WriteLine(Console.ReadLine());
            Console.ReadLine();
        }

        public static void loadConfigFile()
        {
            config = new ConfigFile();
            logging = config.returnLogging();
            operatorsArray = config.operatorsArray;
        }


        public void saveToLogFile(string logLine)
        {
            System.IO.File.AppendAllText("LoggingFile.txt", logLine + Environment.NewLine);            
        }
        
    }
    
}
