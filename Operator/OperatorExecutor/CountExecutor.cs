using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class CountExecutor : Executor
    {
        private OperatorInformation information;
        private Tuple input;
        private int counter;

        public CountExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = Tuple.EMPTY;
            this.counter = 0;
        }

        public bool checkInput()
        {
            return true;
        }

        public Tuple execute()
        {
            if (!checkInput()) throw new InvalidInputException();
            counter++;
            return new Tuple(new List<string>
            {
               counter.ToString()
            });
        }

    }
}
