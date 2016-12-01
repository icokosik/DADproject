using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class UniqExecutor : Executor
    {
        private List<string> passedItems;

        public UniqExecutor(OperatorInformation information)
        {
            this.information = information;
            this.passedItems = new List<string>();
        }

        public override bool checkInput()
        {
            if (input.getSize() - 1 < information.fieldnumber) return false;
            return true;
        }

        public override Tuple execute()
        {
            if (!checkInput()) throw new InvalidInputException();
            if (passedItems.Contains(input.getItems()[information.fieldnumber])) return Tuple.EMPTY;
            passedItems.Add(input.getItems()[information.fieldnumber]);
            return input;
        }
    }
}
