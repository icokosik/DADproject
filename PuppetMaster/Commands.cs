using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DADstorm
{
    class Commands
    {
        public static void start(List<String> x)
        {
            Console.WriteLine("Start: " + x[1].ToString());
            //temporary code - start
            string operatorID = x[1].ToString();
            string address = "";
            foreach (var y in Program.operatorsArray)
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
            foreach (var m in Program.machines)
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
            foreach (var m in Program.machines)
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
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            string address = "";
            Operator op;

            // na ktorej machine sa nachadza replica?
           

            String machine="";
            foreach (var i in Program.replicasArray)
            {
                if (i.operatorID.Equals(operatorID) && i.replicaID.Equals(Convert.ToInt32(replicaID)))
                {
                    machine = i.machineURL;
                }
            }

            string machinePort = "";
            foreach (var ii in Program.machines)
            {
                if (ii.machineURL.Equals(machine))
                    machinePort = Convert.ToString(ii.machineIDport);
            }

            address = "tcp://localhost:" + machinePort+"/kill";
            Console.WriteLine("connecting to killObject at Machine by URL: "+address);
            KillReplica killObject = (KillReplica)Activator.GetObject(
                                        typeof(KillReplica),
                                        address);

            killObject.replicaID = replicaID;
            killObject.deathInQueue=true;
        }


        public static void freeze(List<String> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            string address;
            Operator op;

            foreach (var i in Program.replicasArray)
            {
                if(i.operatorID.Equals(operatorID) && i.replicaID.Equals(Convert.ToInt32(replicaID)))
                {
                    address = "tcp://localhost:" + i.replicaIDport + "/op";
                    Console.WriteLine("i found replica on port: " + address);
                    op = (Operator)Activator.GetObject(
                                                typeof(Operator),
                                                address);
                    op.setFreeze(true);
                }
            }
        }
        public static void unfreeze(List<String> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            string address;
            Operator op;
            foreach (var i in Program.replicasArray)
            {
                if (i.operatorID.Equals(operatorID) && i.replicaID.Equals(Convert.ToInt32(replicaID)))
                {
                    address = "tcp://localhost:" + i.replicaIDport + "/op";
                    Console.WriteLine("i found replica on port: " + address);
                    op = (Operator)Activator.GetObject(
                                                typeof(Operator),
                                                address);
                    op.setFreeze(false);
                }
            }
        }

        public static void wait(List<String> x)
        {
            Thread.Sleep(Convert.ToInt32(x[1]));
        }

      
        public static void exit()
        {
            foreach (Process p in Process.GetProcessesByName("Operator"))
                p.Kill();
            foreach (Process p in Process.GetProcessesByName("Machine"))
                p.Kill();
            Program.running = false;
        }

       
    }
}
