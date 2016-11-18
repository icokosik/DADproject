using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class UniqExecutor : Executor
    {
        private OperatorInformation information;
        private Tuple input;
        private List<string> passedItems;

        public UniqExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = Tuple.EMPTY;
            this.passedItems = new List<string>();
        }

        public bool checkInput()
        {
            if (input.getSize() - 1 < information.fieldnumber) return false;
            return true;
        }

        public Tuple execute()
        {
            if (!checkInput()) throw new InvalidInputException();
            if (passedItems.Contains(input.getItems()[information.fieldnumber])) return null;
            passedItems.Add(input.getItems()[information.fieldnumber]);
            return input;
        }
    }
}
