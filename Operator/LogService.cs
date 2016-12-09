using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class LogService : MarshalByRefObject
    {
        public static LoggingLevel logging;
        public static Queue<string> queuedData = new Queue<string>();
        
        public static void log(string data, bool loglevel)
        {
            if((loglevel && (logging == LoggingLevel.FULL)) || !loglevel)
            {
                queuedData.Enqueue(data);
            }
        }
    }
}
