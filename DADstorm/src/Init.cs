using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;

namespace DADstorm.src
{
    class Init
    {
        // list of machines
        List<Machine> machines = new List<Machine>();

        public Init()
        {
            start();
        }
        public void start()
        {
        }

        // "Client" side of TCP communication - Machine will be recieved, and added to "list of machines"
        public void addMachine()
        {
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);
            Machine obj = (Machine)Activator.GetObject(
                typeof(Machine),
                "tcp://localhost:10/MachineRemote");
            if (obj == null)
            {
                System.Console.WriteLine("Could not locate server");
            }
            else
            {
                machines.Add(obj);
            }
        }
    }
}
