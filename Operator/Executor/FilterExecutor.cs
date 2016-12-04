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
            this.input = new List<Tuple>();
        }

        public bool checkInput(Tuple t)
        {
            if (t.getSize() - 1 < information.fieldnumber)
                return false;
            return true;
        }

        public override List<Tuple> execute()
        {
            List<Tuple> result = new List<Tuple>();
            foreach (Tuple t in input)
            {
                switch (information.condition)
                {
                    case FilterCondition.EQUALS:
                        if (t.get(information.fieldnumber).Equals(information.value))
                        {
                            result.Add(t);
                        }
                        break;
                    case FilterCondition.GREATER:
                        if (greaterThan(t.get(information.fieldnumber), information.value))
                        {
                            result.Add(t);
                        }
                        break;
                    case FilterCondition.SMALLER:
                        if (smallerThan(t.get(information.fieldnumber), information.value))
                        {
                            result.Add(t);
                        }
                        break;
                }
            }
            return result;
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
