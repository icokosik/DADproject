using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public abstract class Executor : IExecutor
    {
        protected Tuple input;
        protected OperatorInformation information;

        public abstract bool checkInput();
        public abstract Tuple execute();

        public Tuple getInput()
        {
            return input;
        }

        public void setInput(Tuple input)
        {
            this.input = input;
        }

        public OperatorInformation getInformation()
        {
            return information;
        }
    }
}
