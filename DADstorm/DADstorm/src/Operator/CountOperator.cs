using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class CountOperator : Operator
    {
        private int count = 0;

        public CountOperator(int id, string name, List<string> inputSource, RoutingOption routing, int replicas, List<string> addresses)
            : base(id, name, inputSource, routing, replicas, addresses)
        {

        }

        public override bool checkInput(Tuple t)
        {
            return true;
        }

        public override Tuple execute()
        {
            if(!checkInput(input)) throw new InvalidInputException();
            count++;

            return new Tuple(new List<string>
            {
               count.ToString()
            });
        }
    }
}
