using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class FilterExecutor : Executor
    {
        public FilterExecutor(OperatorInformation information)
        {
            this.information = information;
            input = Tuple.EMPTY;
        }

        public void setInput(Tuple input)
        {
            this.input = input;
        }

        public override bool checkInput()
        {
            if (input.getSize() - 1 < information.fieldnumber) return false;
            return true;
        }

        public override Tuple execute()
        {
            switch (information.condition)
            {
                case FilterCondition.EQUALS:
                    if (input.get(information.fieldnumber).CompareTo(information.value) == 0)
                    {
                        return input;
                    }
                    break;
                case FilterCondition.GREATER:
                    if (input.get(information.fieldnumber).CompareTo(information.value) > 0)
                    {
                        return input;
                    }
                    break;
                case FilterCondition.SMALLER:
                    if (input.get(information.fieldnumber).CompareTo(information.value) < 0)
                    {
                        return input;
                    }
                    break;
            }
            return Tuple.EMPTY;
        }
    }
}
