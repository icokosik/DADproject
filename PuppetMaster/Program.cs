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
        public static LoggingLevel logging;
        public static List<OperatorInformation> operatorsArray;
        public static List<SourceOPs> sourceoperators;
        public static int portnumber = 12000;

        static void Main(string[] args)
        {
            initPuppetMaster();
            setOperatorOutputs();
            DAGComputations();
            
            // Delegate the startup of all Operators to ThreadOperators
            foreach (OperatorInformation info in operatorsArray)
            {
                Console.WriteLine("Starting new Operator in Thread with port: " + info.port);
                ThreadOperator op1 = new ThreadOperator(info, sourceoperators);
                Thread t1 = new Thread(new ThreadStart(op1.start));
                t1.Start();
            }
            //logging = LoggingLevel.FULL;
            if (logging == LoggingLevel.FULL)
            {
                ThreadLog log = new ThreadLog();
                Thread t2 = new Thread(new ThreadStart(log.start));
                t2.Start();
            }
            
            Console.ReadLine();
        }

        public static void initPuppetMaster()
        {
            sourceoperators = new List<SourceOPs>();
            System.IO.File.Delete("LoggingFile.txt");
            loadConfigFile();

            // Add all Operators to the SourceOPs list. Also, set new port numbers (since everything is on local host, not on different ips)
            foreach (OperatorInformation info in operatorsArray)
            {
                info.port = portnumber;
                sourceoperators.Add(new SourceOPs(info.name, portnumber));
                portnumber++;
            }
        }

        public static void loadConfigFile()
        {
            ConfigFile config = new ConfigFile();
            logging = config.returnLogging();
            operatorsArray = config.getOperatorsArray();
        }

        /*
         * For every Operator, set itself as outputOperator at all of its inputOperators.
         */
        public static void setOperatorOutputs()
        {
            foreach (OperatorInformation info in operatorsArray)
            {
                foreach(string input in info.inputsource)
                {
                    foreach(OperatorInformation info2 in operatorsArray.FindAll(x => x.name.Equals(input)))
                    {
                        info2.outputs.Add(sourceoperators.Find(x => x.name.Equals(info.name) && x.portnumber == info.port));
                    }
                }
            }
        }

        public static void DAGComputations()
        {
            checkAcyclicGraph();
            chooseStartOP();
            addOutputOP();
        }

        private static void checkAcyclicGraph()
        {

        }

        private static void chooseStartOP()
        {
            List<string> startOPs = ConfigFile.getStartOPs();
            foreach(string starter in startOPs)
            {
                
            }
        }

        private static void addOutputOP()
        {

        }
        
        public void saveToLogFile(string logLine)
        {
            System.IO.File.AppendAllText("LoggingFile.txt", logLine + Environment.NewLine);            
        }
        
    }
    
}
