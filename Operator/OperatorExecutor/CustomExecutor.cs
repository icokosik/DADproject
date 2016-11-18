using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm.OperatorExecutor
{
    class CustomExecutor
    {
        private OperatorInformation information;
        private Tuple input;

        public CustomExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = Tuple.EMPTY;
        }

        public bool checkInput()
        {
            throw new NotImplementedException();
        }

        public Tuple execute()
        {
            throw new NotImplementedException();
        }

    }
}
