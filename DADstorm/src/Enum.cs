using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    enum RoutingOption
    {
        PRIMARY, HASHING, RANDOM
    };

    enum OperatorSpec
    {
        UNIQ, COUNT, DUP, FILTER, CUSTOM
    };
}
