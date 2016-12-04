using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace DADstorm
{
    class Program
    {
        public static List<Process2> processes = new List<Process2>();
        public static List<MachineWithReplicas> machines;
        public static String machineURL;
        public static List<Replica> replicas;
        public static MachinePackage m1;

        public static int puppetMasterPort;
        static void Main(string[] args)
        {
            puppetMasterPort = Int32.Parse(args[0]);
            Console.WriteLine("I´m Machine at port: " + puppetMasterPort);

            initMachine(puppetMasterPort);
            runOperators();

            KillReplica killObject = new KillReplica();
            RemotingServices.Marshal(killObject, "kill",
                typeof(KillReplica));

            killLoop(killObject);

            Console.ReadLine();
        }

        static void initMachine(int port)
        {
            Console.WriteLine("Connecting to PM at port: "+port);
            //SERVER
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel, false);
            m1 = new MachinePackage();
            RemotingServices.Marshal(m1, "machine",
                typeof(MachinePackage));
            
            //trying to connect
            do
            {
                Console.WriteLine("... trying to connect to PM");
                Thread.Sleep(300);
            } while (!m1.test);
            Console.WriteLine("... CONNECTED to PM");

            machines = m1.machines;

        }

        static void runOperators()
        {
            foreach(MachineWithReplicas x in machines)
                if(x.machineIDport.Equals(puppetMasterPort))
                {
                    MachineWithReplicas y = x;
                    foreach(var z in y.replicas)
                    {
                        Console.WriteLine("Replica at port: " + z.replicaIDport);

                        //Process2 p = new Process2();
                        Process2 p = new Process2();
                        processes.Add(p);
                        Console.WriteLine("Replica ID: " + z.replicaID);
                        p.replicaID = z.replicaID;

                        p.StartInfo.WorkingDirectory = "..\\..\\..\\Operator\\bin\\Debug";
                        p.StartInfo.FileName = "Operator.exe";
                        p.StartInfo.Arguments = Convert.ToString(z.replicaIDport);
                        p.Start();
                        processes.Add(p);

                       
                    }
                }
        }

        public static void killLoop(KillReplica k)
        {
            while (true)
            {
                if(k.deathInQueue)
                {
                    killReplica(Convert.ToInt32(k.replicaID));
                    k.deathInQueue = false;
                }
                Thread.Sleep(300);
            }
            
        }

        

        public static void killReplica(int repID)
        {
            int t = 0;
            int index = 0;

            foreach (var p in processes)
            {
                if (p.replicaID == repID)
                    index = t;
                t++;
                    }

            processes[index].Kill();
          
        }
    }
    class Process2:Process
    {
        public int replicaID=0;
        public Process2() : base() { }
    }
 
}
