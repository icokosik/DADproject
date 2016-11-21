using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public OperatorInformation oi;
        public List<SourceOPs> sourceoperators;
        public ThreadOperator(int portnumber, OperatorInformation oi, List<SourceOPs> sourceoperators)
        {
            this.portnumber = portnumber;
            this.oi = oi;
            this.sourceoperators = sourceoperators;
        }

        public void start()
        {
            Process p = new Process();
            p.StartInfo.WorkingDirectory = "..\\..\\..\\Operator\\bin\\Debug";
            p.StartInfo.FileName = "Operator.exe";
            p.StartInfo.Arguments = Convert.ToString(portnumber);
            p.Start();

            //CLIENT
            string stringbuilder = "tcp://localhost:" + Convert.ToInt32(portnumber) + "/op";

            Operator obj = null;

                obj = (Operator)Activator.GetObject(
                              typeof(Operator),
                              stringbuilder);
            if (obj.test2 == true)
            {
                
            }
            
            if (obj == null)
            {
                Console.WriteLine("Could not locate server");
            }
            else
            {
                Console.WriteLine("OP is connected to PM, starting do UPLOAD sourceoperators and operatorinformation to Operator");
                //obj.setTestForIco("test1");
                //Console.WriteLine(obj.getTestForIco());

                obj.setOI(oi);
                obj.setSourceOPs(sourceoperators);
              //  Thread.Sleep(1000);
                //Console.WriteLine(obj.test());
              //  obj.input.showAll();


            }

        }
    }
}
