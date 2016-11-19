using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace DADstorm
{
    class Program
    {
        public static LoggingLevel logging = LoggingLevel.LIGHT;
        public static ConfigFile config;
        public static List<OperatorInformation> operatorsArray;
        public static int portnumber = 12000;

        static void Main(string[] args)
        {
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);

            loadConfigFile();
            
            foreach (var x in operatorsArray)
            {
                Console.WriteLine("running operator id: "+portnumber);
                x.setPort(portnumber);
                ThreadOperator op1 = new ThreadOperator(portnumber);
                Thread t1 = new Thread(new ThreadStart(op1.start));
                t1.Start();
                portnumber++;
            }

            Console.WriteLine(Console.ReadLine());
            Console.ReadLine();
        }

        public static void loadConfigFile()
        {
            config = new ConfigFile();
            logging = config.returnLogging() ;
            operatorsArray = config.returnOperatorsArray();
        }
        
    }
    
}
