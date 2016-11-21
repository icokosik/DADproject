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
                    if (input.get(information.fieldnumber).Equals(information.value))
                    {
                        return input;
                    }
                    break;
                case FilterCondition.GREATER:
                    if (greaterThan(input.get(information.fieldnumber), information.value))
                    {
                        return input;
                    }
                    break;
                case FilterCondition.SMALLER:
                    if (smallerThan(input.get(information.fieldnumber), information.value))
                    {
                        return input;
                    }
                    break;
            }
            return Tuple.EMPTY;
        }

        public static bool greaterThan(string value1, string value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        public static bool smallerThan(string value1, string value2)
        {
            return greaterThan(value2, value1);
        }
    }
}
