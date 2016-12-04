using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public interface IExecutor
    {
        List<Tuple> execute();

        List<Tuple> getInput();
        void setInput(List<Tuple> input);
        OperatorInformation getInformation();
        void setOriginOPType(OperatorSpec spec);
    }
}
