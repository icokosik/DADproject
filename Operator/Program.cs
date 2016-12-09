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
        public static String machineURL;

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
            Console.WriteLine("Operator: Establishing connection with PuppetMaster at port " + port);
            LogService.log("Operator: Establishing connection with PuppetMaster at port " + port, false);

            //SERVER
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel,false);

            Operator operator1 = new Operator();
            RemotingServices.Marshal(operator1, "op",
                typeof(Operator));

            //trying to connect
            if(!operator1.isInformationUploaded())
                Console.WriteLine("___ OP is NOT Connected to PM");
            do
            {
                Thread.Sleep(300);
            } while (!operator1.isInformationUploaded());
            Console.WriteLine("___ OP is FINALLY Connected to PM");
            LogService.log("Operator: Connected to PM", false);
            
            operator1.setExecutor();
            operator1.setOutputOperator();
            operator1.setInitialized(true);

            operator1.connectToInput();
        }
    }
}
