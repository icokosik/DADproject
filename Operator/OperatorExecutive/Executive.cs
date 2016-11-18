using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm.OperatorExecutive
{
    public interface Executive
    {
        bool checkInput();
        Tuple execute();
    }
}
