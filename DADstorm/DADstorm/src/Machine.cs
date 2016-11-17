using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm.src
{
    //whole instance will be running in special Thread
    //machine will contain Operators
    class Machine
    {
        int numberOfOperators = 0;
        string ipAddress = "";
        //list of operators will be created here

        
        public Machine() { }
        public Machine(int numberOfOperators, string ipAddress)
        {
            this.numberOfOperators = numberOfOperators;
            this.ipAddress = ipAddress;
        }

        public void addOperator(int port)
        {

        }
    }
}
