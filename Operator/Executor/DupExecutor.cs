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
            this.input = new List<Tuple>();
        }

        public override List<Tuple> execute()
        {
            return input;
        }
    }
}
