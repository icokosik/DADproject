using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class OperatorInformation
    {
        /**
         * Basic Operator information
         */
        public int id;
        public string name;
        public OperatorSpec type;
        public List<string> inputsource;
        public RoutingOption routing;
        public Tuple input;
        /**
         * FilterOperator
         */
        public int fieldnumber;
        public string value;
        public FilterCondition condition;
        /**
         * CustomOperator
         */
        public string dllLocation;
        public string className;
        public string method;
    }
}
