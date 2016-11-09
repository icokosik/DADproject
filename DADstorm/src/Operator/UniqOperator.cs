using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class UniqOperator : Operator
    {

        private Tuple input;
        private List<Object> passedItems;

        public UniqOperator(int id, string name, int inputSource, RoutingOption routing, int replicas, List<string> addresses)
            : base(id, name, inputSource, routing, replicas, addresses)
        {
            passedItems = new List<Object>();
        }

        public override bool checkInput(Tuple t)
        {
            return true;
        }

        public override Tuple execute()
        {
            checkInput(input);
            if (passedItems.Contains(input)) return null;
            passedItems.Add(input);
            return input;
        }

        public void setInput(Tuple input)
        {
            this.input = input;
        }
    }
}
