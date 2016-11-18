using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public interface Executor
    {
        bool checkInput();
        Tuple execute();
    }
}
