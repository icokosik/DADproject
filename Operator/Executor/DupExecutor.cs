using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class DupExecutor : Executor
    {
        public DupExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = Tuple.EMPTY;
        }

        public override bool checkInput()
        {
            return true;
        }

        public override Tuple execute()
        {
            return input;
        }
    }
}
