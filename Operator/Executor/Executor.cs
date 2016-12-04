using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public abstract class Executor : IExecutor
    {
        protected List<Tuple> input;
        protected OperatorInformation information;
        protected OperatorSpec originOPType;
        
        public abstract List<Tuple> execute();

        public List<Tuple> getInput()
        {
            return input;
        }

        public void setInput(List<Tuple> input)
        {
            this.input = input;
        }

        public OperatorInformation getInformation()
        {
            return information;
        }

        public void setOriginOPType(OperatorSpec spec)
        {
            this.originOPType = spec;
        }
    }
}
