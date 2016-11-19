using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class ThreadOperator
    {
        int portnumber;

        public ThreadOperator(int portnumber)
        {
            this.portnumber = portnumber;
        }

        public void start()
        {
            Process.Start("..\\..\\..\\Operator\\bin\\Debug\\Operator.exe", Convert.ToString(portnumber));
            string stringbuilder = "tcp://localhost:" + Convert.ToInt32(portnumber) + "/op";


           
            Operator obj = (Operator)Activator.GetObject(
                          typeof(Operator),
                          stringbuilder);
            if (obj == null)
            {
                Console.WriteLine("Could not locate server");
            }
            else
            {
                Console.WriteLine("Connected...");
              //Console.WriteLine(obj.getTestForIco());
            }
            

        }
    }
}
