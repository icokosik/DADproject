using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    
    public class ReplicasInOP
    {
        public String machineURL;
        public String operatorID;
        public String replicaURL;
        public int replicaID;

        //temporary
        public int replicaIDport;
        public ReplicasInOP(String machineURL,String operatorID,String replicaURL,int replicaID,int port)
        {
            this.machineURL = machineURL;
            this.operatorID = operatorID;
            this.replicaURL = replicaURL;
            this.replicaID = replicaID;
            this.replicaIDport = port;
        }
        public void setPort(int port)
        {
            this.replicaIDport = port;
        }
    }
    [Serializable]
    public class SourceOPs
    {
        public string name;
        public int portnumber;
        public SourceOPs(string name, int portnumber)
        {
            this.name = name;
            this.portnumber = portnumber;
        }

        public override bool Equals(object obj)
        {
            if(obj.GetType() == typeof(SourceOPs))
            {
                SourceOPs that = (SourceOPs)obj;
                return this.name.Equals(that.name) && this.portnumber == that.portnumber;
            }
            return false;
        }
    }
}
