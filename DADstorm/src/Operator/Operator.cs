using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public abstract class Operator
    {

        private int id;
        private string name;
        private string inputSource;
        private RoutingOption routing;
        private int replicas;
        private List<string> addresses;

        public abstract bool checkInput(Tuple t);

        public void execute()
        {
            if (!connectionToInput())
            {
                connectToInput();
            }
        }

        public void connectToInput()
        {

        }

        public bool connectionToInput()
        {
            return false;
        }

    }
}
