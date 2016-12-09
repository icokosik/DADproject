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
        public static bool running = true;
        public static LoggingLevel logging;
        public static List<OperatorInformation> operatorsArray;
        public static List<SourceOPs> sourceoperators;

        public static Random rand = new Random();

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
                LogService.log("PuppetMaster: Executing Machine " + x.machineURL, false);
                //starting machine process
                Process p = new Process();
                p.StartInfo.WorkingDirectory = "..\\..\\..\\Machine\\bin\\Debug";
                p.StartInfo.FileName = "Machine.exe";
                p.StartInfo.Arguments = Convert.ToString(x.machineIDport);
                p.Start();

                //send packet to machine process
                MachinePackage mp = new MachinePackage();
                mp.machines = machines;
                ThreadMachine m = new ThreadMachine(x.machineIDport, mp);
                Thread t1 = new Thread(new ThreadStart(m.start));
                t1.Start();
            }

            Thread.Sleep(2000);            
            // Delegate the startup of all Operators to ThreadOperators

            foreach (OperatorInformation info in operatorsArray)
            {
                Console.WriteLine("Starting new Operator in Thread with port: " + info.port);
                LogService.log("PuppetMaster: Starting new Operator in Thread with port: " + info.port, false);
                ThreadOperator op1 = new ThreadOperator(info, sourceoperators);
                Thread t1 = new Thread(new ThreadStart(op1.start));
                t1.Start();
            }

            setReplicaID();
            
            // Get the logging thread running
            ThreadLog log = new ThreadLog(logging);
            Thread t2 = new Thread(new ThreadStart(log.start));
            t2.Start();
            
            //commands from console
            consoleCommands();
        }

        public static void initPuppetMaster()
        {
            sourceoperators = new List<SourceOPs>();
            System.IO.File.Delete("..\\..\\doc\\log.txt");
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
                executeCommands(x);
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
                    outinfo.path = "..\\..\\doc\\output.txt";
                    outinfo.inputsource = new List<string>() { info.name };
                    outinfo.outputs = new List<SourceOPs>();
                    outputOps.Add(outinfo);
                    sourceoperators.Add(new SourceOPs(outinfo.name, outinfo.port));
                    int id = machines.Find(x => x.machineURL.Equals(ip)).replicas.Count;
                    machines.Find(x => x.machineURL.Equals(ip)).addReplica(new Replica(outinfo.address + "/op", id, outinfo.port));
                    portnumber++;
                    foreach(OperatorInformation info2 in operatorsArray.FindAll(x => x.name.Equals(info.name))) {
                        info2.outputs.Add(new SourceOPs(outinfo.name, outinfo.port));
                    }
                }
            }
            operatorsArray.AddRange(outputOps);
        }

        public void saveToLogFile(string logLine)
        {
            System.IO.File.AppendAllText("LoggingFile.txt", logLine + Environment.NewLine);
        }

        //COMMANDS
        public static void executeCommands(String inputString)
        {
            LogService.log("PuppetMaster: Trying to execute user command " + inputString, false);
            List<String> mainCMD = mainCMDparser(inputString);
            switch (Convert.ToString(mainCMD[0]))
            {
                case "Start":
                    Commands.start(mainCMD);
                    break;
                case "Interval":
                    Commands.interval(mainCMD);
                    break;
                case "Status":
                    Commands.status();
                    break;
                case "Crash":
                    Commands.crash(mainCMD);
                    break;
                case "Freeze":
                    Commands.freeze(mainCMD);
                    break;
                case "Unfreeze":
                    Commands.unfreeze(mainCMD);
                    break;
                case "Wait":
                    Commands.wait(mainCMD);
                    break;
                case "Run":
                    runScript();
                    break;
                case "Exit":
                    Commands.exit();
                    break;

                default:
                    Console.WriteLine("Command doesn´t exist");
                    break;
            }
        }
        public static void consoleCommands()
        {

            while (running)
            {
                Console.WriteLine("\n-->Commands:");
                Console.WriteLine("Run (runs script)");
                Console.WriteLine("Start -operator_id");
                Console.WriteLine("Interval operator id x ms");
                Console.WriteLine("Status");
                Console.WriteLine("Crash processname");
                Console.WriteLine("Freeze processname");
                Console.WriteLine("Unfreeze processname");
                Console.WriteLine("Wait x ms");

                String inputString = Console.ReadLine();
                executeCommands(inputString);
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

        public static void setReplicaID()
        {
            Operator op;
            String address = "";

            foreach (var i in replicasArray)
            {
                address = "tcp://localhost:" + i.replicaIDport + "/op";
                Console.WriteLine("i found replica on port: " + address);
                LogService.log("PuppetMaster: I found replica", true);
                op = (Operator)Activator.GetObject(
                                            typeof(Operator),
                                            address);
                op.setReplicaID(i.replicaID);
                op.showReplicaID();
            }

        }
    }
}