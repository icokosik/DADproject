using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace DADstorm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PupperMaster");
            Operator op = new UniqOperator(0, "0", new List<string>(), RoutingOption.PRIMARY, 1, new List<string>(), 0);
            Console.WriteLine();
            Process.Start("..\\..\\..\\Operator\\bin\\Debug\\Operator.exe");
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);
        }
    }
}
