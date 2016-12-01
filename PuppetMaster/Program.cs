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

        /// <summary>
        ///replicasArray with wrong replicaID for now... 
        /// </summary>
        public static List<ReplicasInOP> replicasArray;
        public static List<String> scripts;
        public static int portnumber = 12000;

      //  public static List<Thread> threadsArray=new List<Thread>();
        static void Main(string[] args)
        {
            initPuppetMaster();
            setOperatorOutputs();
            
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

            //commands from config file
            //runScript();
            //commands from console
            consoleCommands();
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
            replicasArray = config.replicasArray;
            operatorsArray = config.getOperatorsArray();
            scripts = config.scripts;
        }

        public static void runScript()
        {
            foreach(var x in scripts)
                Commands(x);
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
        
        public void saveToLogFile(string logLine)
        {
            System.IO.File.AppendAllText("LoggingFile.txt", logLine + Environment.NewLine);            
        }

        //COMMANDS
        public static void Commands(String inputString)
        {
            List<String> mainCMD = mainCMDparser(inputString);
            switch (Convert.ToString(mainCMD[0]))
            {
                case "Start":
                    start(mainCMD);
                    break;
                case "Interval":
                    interval(mainCMD);
                    break;
                case "Status":
                    status();
                    break;
                case "Crash":
                    crash(mainCMD);
                    break;
                case "Freeze":
                    freeze(mainCMD);
                    break;
                case "Unfreeze":
                    unfreeze(mainCMD);
                    break;
                case "Wait":
                    wait(mainCMD);
                    break;

                default:
                    Console.WriteLine("Command doesn´t exist");
                    break;
            }
        }
        public static void consoleCommands()
        {
            while (true)
            {
                Console.WriteLine("\n-->Commands:");
                Console.WriteLine("Start -operator_id");
                Console.WriteLine("Interval operator id x ms");
                Console.WriteLine("Status");
                Console.WriteLine("Crash processname");
                Console.WriteLine("Freeze processname");
                Console.WriteLine("Unfreeze processname");
                Console.WriteLine("Wait x ms");
                Console.WriteLine("Load (config file)");

                String inputString = Console.ReadLine();
                Commands(inputString);
            }
        }

        public static List<String> mainCMDparser(string s)
        {
            List<String> array = new List<String>();
            string[] words = s.Split(' ');
            foreach (string word in words)
                array.Add(word);
            return array;
        }


        public static String returnAddressOfOperator(List<String> x)
        {
            string operatorID = x[1].ToString();
            string address="";
            foreach (var y in operatorsArray)
                if (y.name.Equals(operatorID))
                    address = "tcp://localhost:" + y.port + "/op";
            return address;
        }

        public static void start(List<String> x)
        {
            //temporary code - start
            string operatorID = x[1].ToString();
            string address = "";
            foreach (var y in operatorsArray)
                if (y.name.Equals(operatorID))
                {
                    address = "tcp://localhost:" + y.port + "/op";
                    Operator op = (Operator)Activator.GetObject(
                                                typeof(Operator),
                                                address);
                    op.setStart(true);
                }
            //temporary code - finish


            /*
            Operator op = (Operator)Activator.GetObject(
                             typeof(Operator),
                             returnAddressOfOperator(x));
            op.createOutput();
            */
        }

        public static void interval(List<String> x)
        {
            //tell operator with ID The operator should sleep x milliseconds between two consecutive events.
            // createOutput() method ...
            Int32 sleep_ms = Convert.ToInt32(x[2].ToString());

            //temporary code   - start
            string operatorID = x[1].ToString();
            string address = "";
            foreach (var y in operatorsArray)
                if (y.name.Equals(operatorID))
                {
                    address = "tcp://localhost:" + y.port + "/op";
                    Operator op = (Operator)Activator.GetObject(
                                                typeof(Operator),
                                                address);
                    op.setSleep(sleep_ms);
                }
            //temporary code   - finish

            /*
            Operator op = (Operator)Activator.GetObject(
                             typeof(Operator),
                             returnAddressOfOperator(x));
            op.setSleep(sleep_ms);
            */
        }

        public static void status()
        {
            String address;
            Operator op;
            foreach (var y in operatorsArray)
            {
                address = "tcp://localhost:" + y.port + "/op";
                op = (Operator)Activator.GetObject(
                                            typeof(Operator),
                                            address);
                op.printStatus();
            }
        }

        public static void crash(List<String> x)
        {
            //we need to have replicas inside of machine first (i don´t want to write code we will not use)
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();

            // i will use ___.close() method 
        }


        public static void freeze(List<String> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            Operator op;
            //EXACT URL for now
            String address = "tcp://localhost:" + 12003 + "/op";
           
                    op = (Operator)Activator.GetObject(
                                           typeof(Operator),
                                           address);
                    op.setFreeze(true);
               
        }
        public static void unfreeze(List<String> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            Operator op;

            //EXACT URL for now
            String address = "tcp://localhost:" + 12003 + "/op";
            
                    op = (Operator)Activator.GetObject(
                                           typeof(Operator),
                                           address);
            op.setFreeze(false);
        }

        public static void wait(List<String> x)
        {
            Thread.Sleep(Convert.ToInt32(x[1]));
        }
    }

}
