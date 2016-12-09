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
            string operatorID = x[1].ToString();
            string address = "";
            foreach (var o in Program.replicasArray)
            {
                if (o.operatorID.Equals(operatorID))
                {
                    address = "tcp://localhost:" + o.replicaIDport + "/op";
                    Operator op = (Operator)Activator.GetObject(
                                                typeof(Operator),
                                                address);
                    op.setStart(true);
                }
            }
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

            String machine="";
            int counter = 0;
            int index = 0;

            foreach (var i in Program.replicasArray)
            {
                if (i.operatorID.Equals(operatorID) && i.replicaID.Equals(Convert.ToInt32(replicaID)))
                {
                    string address2 = "tcp://localhost:" + Convert.ToInt32(i.replicaIDport) + "/op";
                    Console.WriteLine(address2);

                    Operator op = (Operator)Activator.GetObject(
                                   typeof(Operator),
                                   address2);
                    op.eraseMe();

                    machine = i.machineURL;
                    index = counter;
                }
                counter++;
            }

            
            //remove crashed replica from array
            Console.WriteLine("index: "+ index);
            Program.replicasArray.RemoveAt(index);
            foreach (var m in Program.machines)
                if (m.operatorID.Equals(operatorID))
                {
                    m.RemoveReplica(Convert.ToInt32(replicaID));
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
