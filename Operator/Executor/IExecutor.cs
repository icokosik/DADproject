using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public interface IExecutor
    {
        bool checkInput();
        Tuple execute();

        Tuple getInput();
        void setInput(Tuple input);
        OperatorInformation getInformation();
    }
}
