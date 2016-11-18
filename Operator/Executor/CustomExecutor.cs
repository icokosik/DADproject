using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm.OperatorExecutor
{
    class CustomExecutor : Executor
    { 
        public CustomExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = Tuple.EMPTY;
        }

        public override bool checkInput()
        {
            throw new NotImplementedException();
        }

        public override Tuple execute()
        {
            throw new NotImplementedException();
        }

    }
}
