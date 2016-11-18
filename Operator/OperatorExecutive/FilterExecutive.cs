using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm.OperatorExecutive
{
    class FilterExecutive : Executive
    {
        private OperatorInformation information;
        private Tuple input;

        public bool checkInput()
        {
            if (input.getSize() - 1 < OperatorInformation.fieldnumber) return false;
            return true;
        }

        public Tuple execute()
        {
            switch (OperatorInformation.condition)
            {
                case FilterCondition.EQUALS:
                    if (input.get(OperatorInformation.fieldnumber).CompareTo(OperatorInformation.value) == 0)
                    {
                        return input;
                    }
                    break;
                case FilterCondition.GREATER:
                    if (input.get(OperatorInformation.fieldnumber).CompareTo(OperatorInformation.value) > 0)
                    {
                        return input;
                    }
                    break;
                case FilterCondition.SMALLER:
                    if (input.get(OperatorInformation.fieldnumber).CompareTo(OperatorInformation.value) < 0)
                    {
                        return input;
                    }
                    break;
            }
            return Tuple.EMPTY;
        }
    }
}
