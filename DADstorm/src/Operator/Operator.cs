using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DADstorm
{
    public abstract class Operator
    {

        protected int id;
        protected string name;
        protected List<string> inputSource;
        protected RoutingOption routing;
        protected int replicas;
        protected List<string> addresses;
        protected Tuple input;

        public abstract bool checkInput(Tuple t);
        public abstract Tuple execute();

        public Operator(int id, string name, List<string> inputSource, RoutingOption routing, int replicas, List<string> addresses)
        {
            // TODO: check id != inputsource
            this.id = id;
            this.name = name;
            this.inputSource = inputSource;
            this.routing = routing;
            this.replicas = replicas;
            this.addresses = addresses;
        }

        public void setInput(Tuple input)
        {
            this.input = input;
        }

        // TODO: Diana
        public void connectToInput()
        {
            foreach(string tmp in inputSource)
            {
                if (Regex.IsMatch(tmp, "^OP\\d+$")) //operator in format OP1, OP2, ..., OPn
                {
                   
                }
                else if (Regex.IsMatch(tmp, @"^\d+$")) //operator as number 
                {
                   
                }
                else // input file
                {
 
                }
            }
 
        }

        public bool connectionToInput()
        {
            return false;
        }

    }
}
