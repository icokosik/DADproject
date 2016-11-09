using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class UniqOperator : Operator
    {
        private List<Object> passedItems;

        public UniqOperator()
        {

        }

        public override bool checkInput(Tuple t)
        {
            throw new NotImplementedException();
        }
    }
}
