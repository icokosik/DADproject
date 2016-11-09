using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class CountOperator : Operator
    {

        public CountOperator(int id, string name, int inputSource, RoutingOption routing, int replicas, List<string> addresses)
            : base(id, name, inputSource, routing, replicas, addresses)
        {

        }

        public override bool checkInput(Tuple t)
        {
            throw new NotImplementedException();
        }

        public override Tuple execute()
        {
            throw new NotImplementedException();
        }
    }
}
