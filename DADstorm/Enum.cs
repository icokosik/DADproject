using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADStorm
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
