using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;

namespace DADstorm.src
{
    class LoggingData: MarshalByRefObject
    {
        string senderName = "";
        string senderMessage = "";  //probably will be changed
        // var tuples = list of tuples

        public LoggingData(string senderName, string senderMessage) {
            this.senderName = senderName;
            this.senderMessage = senderMessage;
        }

        public string Hello() {
            return "Hello";
        }
    }

    
    class Logging
    {
          public Logging(LoggingLevel logg) {
          //  TcpChannel channel2 = new TcpChannel();
          //  ChannelServices.RegisterChannel(channel2, true);
            
        }

        public void run()
        {
            
            getData();
            //test -start 
            /*
             while (true)
             {
                 Thread.Sleep(500);
                 Console.WriteLine("+500 ms");
             }
             */
             //test -end
        }

        // logs will be downloading from nodes
        public void getData()
        {
            

            LoggingData obj = null;
            
                obj = (LoggingData)Activator.GetObject(
                    typeof(LoggingData),
                    "tcp://localhost:8086/loggingdata");
           
            if (obj == null)
            {
                Console.WriteLine("Could not locate server");
            }
            else
            {
                Console.WriteLine("test");
            }
        }


        //logs will be wroten into txt files (probably)
        public void saveData() { }
    }
    
}
