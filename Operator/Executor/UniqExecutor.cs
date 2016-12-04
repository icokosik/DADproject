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
            this.input = new List<Tuple>();
            this.passedItems = new List<string>();
        }

        public bool checkInput(Tuple t)
        {
            if (t.getSize() - 1 < information.fieldnumber) return false;
            return true;
        }

        public override List<Tuple> execute()
        {
            List<Tuple> result = new List<Tuple>();
            foreach(Tuple t in input)
            {
                if (!checkInput(t)) throw new InvalidInputException();
                if (!passedItems.Contains(t.getItems()[information.fieldnumber]))
                {
                    passedItems.Add(t.getItems()[information.fieldnumber]);
                    result.Add(t);
                }
            }
            return result;
        }
    }
}
