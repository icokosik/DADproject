using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace DADstorm
{
    public class Program
    {
        private static int puppetMasterPort;
        private static string operatorID;

        static void Main(string[] args)
        {
            puppetMasterPort = Int32.Parse(args[0]);
            initOperator(puppetMasterPort);          
            
            Console.ReadLine();
        }

        /**
         * Connect to the PuppetMaster to get the OperatorInformation
         * Set the OperatorInformation insie Operator and get the right Executor
         */
        public static void initOperator(int port)
        {
            Console.WriteLine("Establishing connection with PuppetMaster at port " + port);

            //SERVER
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel,false);

            Operator operator1 = new Operator();
            RemotingServices.Marshal(operator1, "op",
                typeof(Operator));

            //trying to connect
            do
            {
                Console.WriteLine("___ NOT Connected to PM");
                Thread.Sleep(300);
            } while (!operator1.isInformationUploaded());
            Console.WriteLine("___ FINALLY Connected to PM");
            
            operator1.setExecutor();
            operator1.setOutputOperator();
            operator1.setInitialized(true);

            operator1.connectToInput();

            //connectToOutput() is now PM command
            
        }



        // Prepared commands for Machines and Operators ---> they will be located here until we create "Machine"
        public void status()
        {
            //STATUS of all nodes
        }
        public void crash(List<String> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
          //  crashCMD(operatorID, replicaID);
        }
        public void freeze(List<String> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
          //  freezeCMD(operatorID, replicaID);
        }
        public void wait(List<String> x)
        {
            Int32 wait_ms = Convert.ToInt32(x[1].ToString());
         //   waitCMD(wait_ms);
        }
        public void unfreeze(List<String> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
           // unfreezeCMD(operatorID, replicaID);
        }

        
    }

   
}
