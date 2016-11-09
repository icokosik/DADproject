using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public abstract class Operator
    {

        protected int id;
        protected string name;
        protected int inputSource;
        protected RoutingOption routing;
        protected int replicas;
        protected List<string> addresses;

        public abstract bool checkInput(Tuple t);
        public abstract Tuple execute();

        public Operator(int id, string name, int inputSource, RoutingOption routing, int replicas, List<string> addresses)
        {
            // TODO: check id != inputsource
            this.id = id;
            this.name = name;
            this.inputSource = inputSource;
            this.routing = routing;
            this.replicas = replicas;
            this.addresses = addresses;
        }

        // TODO: Diana
        public void connectToInput()
        {

        }

        public bool connectionToInput()
        {
            return false;
        }

    }
}
