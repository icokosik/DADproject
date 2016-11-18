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
        public static int id;
        public static string name;
        public static List<string> inputsource;
        public static RoutingOption routing;
        public static int replicas;
        public static Tuple input;
        /**
         * FilterOperator
         */
        public static int fieldnumber;
        public static string value;
        public static FilterCondition condition;
        /**
         * CustomOperator
         */
        public static string dllLocation;
        public static string className;
        public static string method;
    }
}
