using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class FilterOperator : Operator
    {
        private int fieldnumber;
        private FilterCondition condition;
        private string value;

        public FilterOperator(int id, string name, List<string> inputSource, RoutingOption routing, int replicas, List<string> addresses,
            int fieldnumber, FilterCondition condition, string value)
            : base(id, name, inputSource, routing, replicas, addresses)
        {
            this.fieldnumber = fieldnumber;
            this.condition = condition;
            this.value = value;
        }

        public override bool checkInput()
        {
            if (input.getSize() - 1 < fieldnumber) return false;
            return true;
        }

        public override Tuple execute()
        {
            switch(condition)
            {
                case FilterCondition.EQUALS:
                    if(input.get(fieldnumber).CompareTo(value) == 0)
                    {
                        return input;
                    }
                    break;
                case FilterCondition.GREATER:
                    if(input.get(fieldnumber).CompareTo(value) > 0)
                    {
                        return input;
                    }
                    break;
                case FilterCondition.SMALLER:
                    if(input.get(fieldnumber).CompareTo(value) < 0)
                    {
                        return input;
                    }
                    break;
            }
            return Tuple.EMPTY;
        }
    }
}
