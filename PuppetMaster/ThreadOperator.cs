using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DADstorm
{
    class ThreadOperator
    {
        int portnumber;
        public OperatorInformation information;
        public List<SourceOPs> sourceoperators;

        Process p = new Process();
        
        public ThreadOperator(OperatorInformation information, List<SourceOPs> sourceoperators)
        {
            this.information = information;
            this.portnumber = information.port;
            this.sourceoperators = sourceoperators;
        }

        /**
         * Create a new Process of type Operator, connect to it via Marshalling and send it all information
         */
        public void start()
        {
            string address = "tcp://localhost:" + Convert.ToInt32(portnumber) + "/op";
            Console.WriteLine("Trying to connect to address: " + address);
            LogService.log("ThreadOperator: Trying to connect to address: " + address, false);
            Operator op = (Operator)Activator.GetObject(
                              typeof(Operator),
                              address);

            if (op == null)
            {
                Console.WriteLine("Could not locate server");
                LogService.log("ThreadOperator: Could not locate server", false);
            }
            else
            {
                Console.WriteLine("{0} is connected to PM, starting upload sourceoperators and operatorinformation to Operator", information.name);
                LogService.log("ThreadOperator: " + information.name + " is connected to PM, starting upload sourceoperators and operatorinformation to Operator", false);
                op.setInformation(information);
                lock (this)
                {
                    op.setReplicaIDport(portnumber);
                    op.isreplicaIDuploaded = true;
                }
                Console.WriteLine("Finished uploading to OP{0}", information.id);
                LogService.log("ThreadOperator: Finished uploading to OP" + information.id, false);
            }
        }
        public void close()
        {
            p.CloseMainWindow();
            p.Close();
        }

       
    }
}
