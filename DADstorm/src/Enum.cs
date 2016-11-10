﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public enum RoutingOption
    {
        PRIMARY, HASHING, RANDOM
    };

    public enum OperatorSpec
    {
        UNIQ, COUNT, DUP, FILTER, CUSTOM
    };

    public enum LoggingLevel
    {
        FULL, LIGHT
    };
}
