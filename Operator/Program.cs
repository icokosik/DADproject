using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace DADstorm
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Operator");
            Console.WriteLine("arg: " + args[0]);
            Console.WriteLine("Establishing connection with PuppetMaster...");
            int puppetMasterPort = Int32.Parse(args[0]);
            TcpChannel channel = new TcpChannel(puppetMasterPort);
            ChannelServices.RegisterChannel(channel, true);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Operator),
                "op",
                WellKnownObjectMode.Singleton);

            Console.ReadLine();
        }
    }
}
