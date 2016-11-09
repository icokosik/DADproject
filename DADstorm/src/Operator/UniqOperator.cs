using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class UniqOperator : Operator
    {
        private int fieldNumber;
        private List<Object> passedItems;

        public UniqOperator(int id, string name, int inputSource, RoutingOption routing, int replicas, List<string> addresses, int fieldNumber)
            : base(id, name, inputSource, routing, replicas, addresses)
        {
            this.fieldNumber = fieldNumber;
            passedItems = new List<Object>();
        }

        public override bool checkInput(Tuple t)
        {
            if (t.getSize()-1 < fieldNumber) return false;
            return true;
        }

        public override Tuple execute()
        {
            if (!checkInput(input)) throw new InvalidInputException();
            if (passedItems.Contains(input.getItems()[fieldNumber])) return null;
            passedItems.Add(input.getItems()[fieldNumber]);
            return input;
        }
    }
}
