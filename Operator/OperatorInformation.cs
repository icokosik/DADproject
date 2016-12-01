using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    [Serializable]
    public class OperatorInformation
    {
        /**
         * Basic Operator information
         */
        public int port;
        public List<string> inputsource;
        public RoutingOption routing;
        public int id;
        public string name;
        public int repl_factor;
        public string address;
        public List<SourceOPs> outputs;
        public LoggingLevel logging;

        /**
         * FilterOperator
         */
        public int fieldnumber;
        public string value;
        public FilterCondition condition;
        /**
         * CustomOperator
         */
        public OperatorSpec type;
        public string dllLocation;
        public string className;
        public string method;
        /**
         * OutputOperator
         */
        public string path;

        public OperatorInformation() { }
        public OperatorInformation(int id, string name, List<string> inputsource, RoutingOption routing, string address,
            OperatorSpec type, int fieldnumber, string value, FilterCondition condition, string dllLocation,string className, string method, LoggingLevel logging) {
            this.id = id;
            this.name = name;
            this.inputsource = inputsource;
            this.routing = routing;
            this.address = address;
            this.type = type;
            this.outputs = new List<SourceOPs>();

            this.fieldnumber = fieldnumber;
            this.value = value;
            this.condition = condition;

            this.dllLocation = dllLocation;
            this.className = className;
            this.method = method;

            this.logging = logging;
        }
    }
}
