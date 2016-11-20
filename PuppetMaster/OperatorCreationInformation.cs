using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class OperatorCreationInformation
    {
        /**
         * Basic Operator information
         */
        public int port;
        public OperatorSpec type;
        public List<string> inputsource;
        public RoutingOption routing;
        public int id;
        public string name;
        public int repl_factor;
        List<string> address_array;

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

        public OperatorCreationInformation() { }
        public OperatorCreationInformation(string name2, List<string> inputsource2, int repl_factor2, RoutingOption routing2, List<string> address_array2,
            OperatorSpec type2, int fieldnumber2, string value2, FilterCondition condition2, string dllLocation2, string className2, string method2)
        {
            name = name2;
            inputsource = inputsource2;
            repl_factor = repl_factor2;
            routing = routing2;
            address_array = address_array2;
            type = type2;

            fieldnumber = fieldnumber2;
            value = value2;
            condition = condition2;

            dllLocation = dllLocation2;
            className = className2;
            method = method2;
        }

        public void setPort(int input)
        {
            port = input;
        }
        public int getPort()
        { return port; }
    }
}
