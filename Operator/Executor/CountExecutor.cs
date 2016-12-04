using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class CountExecutor : Executor
    {
        private int counter;

        public CountExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = new List<Tuple>();
            this.counter = 0;
        }

        public override List<Tuple> execute()
        {
            return new List<Tuple>() { new Tuple(new List<string>() { "" + input.Count }) };
        }
    }
}
