﻿using System;
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
        public static OperatorInformation information;

        static void Main(string[] args)
        {
            int puppetMasterPort = Int32.Parse(args[0]);
            getOperatorInformation(puppetMasterPort);
            CheckOperatorInformation();
            Console.ReadLine();
        }

        public static void getOperatorInformation(int port)
        {
            Console.WriteLine("Establishing connection with PuppetMaster on port " + port);
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel, true);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Operator),
                "op",
                WellKnownObjectMode.Singleton);

            // Receive OperatorInformation
            information = new OperatorInformation();
        }

        public static void CheckOperatorInformation()
        {
            switch(information.type)
            {
                case OperatorSpec.COUNT:
                    break;
                case OperatorSpec.DUP:
                    break;
                case OperatorSpec.FILTER:
                    break;
                case OperatorSpec.UNIQ:
                    break;
                case OperatorSpec.CUSTOM:
                    break;
            }
        }
    }
}
