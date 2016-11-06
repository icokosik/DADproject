using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADStorm
{
    class Operator
    {

        private int id;
        private string inputSource;
        private RoutingOption routing;
        private int replicas;
        private List<string> addresses;
        private OperatorSpec operatorspec;

        public Operator(int id, string inputSource, RoutingOption routing, int replicas, List<string> addresses, OperatorSpec operatorspec)
        {
            this.id = id;
            this.inputSource = inputSource;
            this.routing = routing;
            this.replicas = replicas;
            this.addresses = addresses;
            this.operatorspec = operatorspec;
        }

        public void execute()
        {
            if (!connectionToInput())
            {
                connectToInput();
            }
            executeSomeThings();
        }

        public void connectToInput()
        {

        }

        public bool connectionToInput()
        {
            return false;
        }

        public void executeSomeThings()
        {

        }


    }
}
