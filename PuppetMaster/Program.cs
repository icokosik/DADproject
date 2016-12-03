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
        public static List<MachineWithReplicas> machines;

        public static List<String> scripts;
        public static int portnumber = 12000;

        //  public static List<Thread> threadsArray=new List<Thread>();
        static void Main(string[] args)
        {


            initPuppetMaster();
            setOperatorOutputs();

            //RUN MACHINES
            foreach (MachineWithReplicas x in machines)
            {
                Console.WriteLine("Executing Machine: " + x.machineURL);

                Process p = new Process();
                p.StartInfo.WorkingDirectory = "..\\..\\..\\Machine\\bin\\Debug";
                p.StartInfo.FileName = "Machine.exe";
                p.StartInfo.Arguments = Convert.ToString(x.machineIDport);
                p.Start();


                MachinePackage mp = new MachinePackage();
                mp.machines = machines;
                ThreadMachine m = new ThreadMachine(x.machineIDport, mp);
                Thread t1 = new Thread(new ThreadStart(m.start));
                t1.Start();
            }

            Thread.Sleep(2000);

            foreach (MachineWithReplicas x in machines)
            {


            }


            //RUN communications PM -> Machine
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
            System.IO.File.Delete("..\\..\\..\\Operator\\doc\\Output.txt");
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
            machines = config.machines;
            scripts = config.scripts;
        }

        public static void runScript()
        {
            foreach (var x in scripts)
                Commands(x);
        }

        /*
         * For every Operator, set itself as outputOperator at all of its inputOperators.
         */
        public static void setOperatorOutputs()
        {
            foreach (OperatorInformation info in operatorsArray)
            {
                foreach (string input in info.inputsource)
                {
                    foreach (OperatorInformation info2 in operatorsArray.FindAll(x => x.name.Equals(input)))
                    {
                        info2.outputs.Add(sourceoperators.Find(x => x.name.Equals(info.name) && x.portnumber == info.port));
                    }
                }
            }
            
            List<OperatorInformation> outputOps = new List<OperatorInformation>();
            foreach (OperatorInformation info in operatorsArray)
            {
                if (info.outputs.Count == 0 && operatorsArray.FindAll(x => x.name.Equals("Out" + info.name)).Count == 0)
                {
                    OperatorInformation outinfo = new OperatorInformation();
                    int operatorCount = ConfigFile.operatorCount;
                    outinfo.id = operatorCount;
                    outinfo.name = "Out" + info.name;
                    outinfo.type = OperatorSpec.OUT;
                    outinfo.port = portnumber;
                    string ip = info.address.Split(':')[0] + ":" + info.address.Split(':')[1];
                    outinfo.address = ip + ":" + (12500 + outinfo.id);
                    Console.WriteLine("NEW OUT AT!! " + outinfo.address);
                    outinfo.path = "..\\..\\doc\\output.txt";
                    outinfo.inputsource = new List<string>() { info.name };
                    outinfo.outputs = new List<SourceOPs>();
                    outputOps.Add(outinfo);
                    sourceoperators.Add(new SourceOPs(outinfo.name, outinfo.port));
                    int id = machines.Find(x => x.machineURL.Equals(ip)).replicas.Count;
                    machines.Find(x => x.machineURL.Equals(ip)).addReplica(new Replica(outinfo.address + "/op", id, outinfo.port));
                    portnumber++;
                    info.outputs.Add(new SourceOPs(outinfo.name, outinfo.port));
                }
            }
            operatorsArray.AddRange(outputOps);
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
            string address = "";
            foreach (var y in operatorsArray)
                if (y.name.Equals(operatorID))
                    address = "tcp://localhost:" + y.port + "/op";
            return address;
        }

        public static void start(List<String> x)
        {
            Console.WriteLine("Start: " + x[1].ToString());
            //temporary code - start
            string operatorID = x[1].ToString();
            string address = "";
            foreach (var y in operatorsArray)
            {
                Console.WriteLine("Operator: " + y.name);
                if (y.name.Equals(operatorID))
                {
                    address = "tcp://localhost:" + y.port + "/op";
                    Operator op = (Operator)Activator.GetObject(
                                                typeof(Operator),
                                                address);
                    op.setStart(true);
                }
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
            Int32 sleep_ms = Convert.ToInt32(x[2].ToString());
            string operatorID = x[1].ToString();
            string address = "";
            Operator op;
            foreach (var m in machines)
                if (m.operatorID.Equals(operatorID))
                {
                    foreach (var n in m.replicas)
                    {
                        address = "tcp://localhost:" + n.replicaIDport + "/op";
                        op = (Operator)Activator.GetObject(
                                                    typeof(Operator),
                                                    address);
                        op.setSleep(sleep_ms);
                    }
                }

        }

        public static void status()
        {
            String address;
            Operator op;
            foreach (var m in machines)
                foreach (var n in m.replicas)
                {
                    address = "tcp://localhost:" + n.replicaIDport + "/op";
                    op = (Operator)Activator.GetObject(
                                                typeof(Operator),
                                                address);
                    op.printStatus();
                }
        }

        public static void crash(List<String> x)
        {
            //mathias, help me please
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            string url = "";
            int i = 0;

            foreach (var m in machines)
                if (m.operatorID.Equals(operatorID))
                    i = m.replicas[Convert.ToInt32(replicaID)].replicaIDport;

            url = "tcp://localhost:" + Convert.ToString(i) + "/op";

            Operator op = (Operator)Activator.GetObject(
                                  typeof(Operator),
                                  url);
            op.crash();
        }


        public static void freeze(List<String> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            string address;
            Operator op;

            foreach (var m in machines)
                if (m.operatorID.Equals(operatorID))
                {
                    address = "tcp://localhost:" + m.replicas[Convert.ToInt32(replicaID)].replicaIDport + "/op";
                    op = (Operator)Activator.GetObject(
                                                typeof(Operator),
                                                address);
                    op.setFreeze(true);
                }

        }
        public static void unfreeze(List<String> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            string address;
            Operator op;

            foreach (var m in machines)
                if (m.operatorID.Equals(operatorID))
                {
                    address = "tcp://localhost:" + m.replicas[Convert.ToInt32(replicaID)].replicaIDport + "/op";
                    op = (Operator)Activator.GetObject(
                                                typeof(Operator),
                                                address);
                    op.setFreeze(false);
                }
        }

        public static void wait(List<String> x)
        {
            Thread.Sleep(Convert.ToInt32(x[1]));
        }
    }

}