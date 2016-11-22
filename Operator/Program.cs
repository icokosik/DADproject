using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.IO;
using System.Net.NetworkInformation;

namespace DADstorm
{
    public class Program
    {
        private static int puppetMasterPort;

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
            operator1.connectToInput();
        }
    }
}
