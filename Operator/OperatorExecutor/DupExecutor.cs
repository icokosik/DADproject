using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class DupExecutor : Executor
    {
        private OperatorInformation information;
        private Tuple input;

        public DupExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = Tuple.EMPTY;
        }

        public bool checkInput()
        {
            return true;
        }

        public Tuple execute()
        {
            return input;
        }
    }
}
