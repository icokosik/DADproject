﻿using System;
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
        public int port = 12000;
        public List<string> inputsource;
        public RoutingOption routing;
        public int id;
        public string name;
        string address;

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

        public OperatorInformation() { }
        public OperatorInformation(string name2, List<string> inputsource2, RoutingOption routing2, string address2, int fieldnumber2,
            string value2, FilterCondition condition2, string dllLocation2,string className2, string method2) {
            name = name2;
            inputsource = inputsource2;
            routing = routing2;
            address = address2;

            fieldnumber = fieldnumber2;
            value = value2;
            condition = condition2;

            dllLocation = dllLocation2;
            className = className2;
            method = method2;
        }

        public void setPort(int input) {
            port = input;
        }
        public int getPort()
        { return port; }

    }

}
