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
            Process p = new Process();
            p.StartInfo.WorkingDirectory = "..\\..\\..\\Operator\\bin\\Debug";
            p.StartInfo.FileName = "Operator.exe";
            p.StartInfo.Arguments = Convert.ToString(portnumber);
            p.Start();
            
            // Connect to Operator
            string address = "tcp://localhost:" + Convert.ToInt32(portnumber) + "/op";
            Operator op = (Operator)Activator.GetObject(
                              typeof(Operator),
                              address);
            
            /**
             * Should maybe block until Operator is fully initialized and has his Marshalling setup
             */

            if (op == null)
            {
                Console.WriteLine("Could not locate server");
            }
            else
            {
                Console.WriteLine("OP{0} is connected to PM, starting upload sourceoperators and operatorinformation to Operator", information.id);
                op.setInformation(information);
                Console.WriteLine("Finished uploading to OP{0}", information.id);
            }
        }
    }
}
