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

            //mathias start

            int portnumber = 100;
            Process.Start("..\\..\\..\\Operator\\bin\\Debug\\Operator.exe", Convert.ToString(portnumber));
            //mathias end

            
            //TCP channel
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);



            Operator obj = (Operator)Activator.GetObject(
                typeof(Operator),
                "tcp://localhost:100/op");
            if (obj == null)
            {
                System.Console.WriteLine("Could not locate server");
            }
            else
            {
               // Console.WriteLine(obj.Hello());
            }


            Console.ReadLine();
        }

        public void LoadConfigFile()
        {

        }
    }
}
