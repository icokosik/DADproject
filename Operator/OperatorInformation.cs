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
<<<<<<< Updated upstream
        public int id;
        public string name;
        public OperatorSpec type;
        public List<string> inputsource;
        public RoutingOption routing;
=======
        public static int id;
        public static string name;
        public List<string> inputsource;
        public static int repl_factor;
        public RoutingOption routing;
        List<string> address_array;
        public OperatorSpec type;


        public Tuple input;
>>>>>>> Stashed changes
        /**
         * FilterOperator
         */
        public int fieldnumber;
        public string value;
        public FilterCondition condition;
        /**
         * CustomOperator
         */
<<<<<<< Updated upstream
        public string dllLocation;
        public string className;
        public string method;
=======
        public static string dllLocation;
        public static string className;
        public static string method;

        public OperatorInformation() { }
        public OperatorInformation(string name2, List<string> inputsource,int repl_factor2,RoutingOption routing, List<string> address_array,OperatorSpec type,int fieldnumber2,string value2, FilterCondition condition2, string dllLocation2,string className2, string method2) {
            name = name2;
            this.inputsource = inputsource;
            repl_factor = repl_factor2;
            this.routing = routing;
            this.address_array = address_array;
            this.type = type;

            fieldnumber = fieldnumber2;
            value = value2;
            condition = condition2;

            dllLocation = dllLocation2;
            className = className2;
            method = method2;
        }

>>>>>>> Stashed changes
    }

}
