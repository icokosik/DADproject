using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class LogService : MarshalByRefObject
    {
        public List<string> recievedLogData = new List<string>();
        public Object thisLock = new Object();

        
        public List<string> getLogData()
        {
            lock (thisLock)
            {
                return recievedLogData;
            }
        }
        //method is filled of string "replica_URL <list_of_tuple_fileds>"
        public void setLogData(string outputOp)
        {
            lock (thisLock)
            {
                this.recievedLogData.Add(outputOp);
            }
        }
        //clear list data
        public void Clear()
        {
            lock (thisLock)
            {
                this.recievedLogData.Clear();
            }
        }
        
    }
}
