using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    
    public class MachinePackage:MarshalByRefObject
    {
        public List<MachineWithReplicas> machines = new List<MachineWithReplicas>();
        public Boolean test = false;

        public void setMachines(List<MachineWithReplicas> x)
        {
            machines = x;
        }

    }
    
    [Serializable]
    public class MachineWithReplicas
    {
        public String machineURL;
        public int machineIDport;
        public String operatorID;

        public List<Replica> replicas = new List<Replica>();

        public MachineWithReplicas() { }
        public MachineWithReplicas(String machineURL,int machineIDport, String operatorID)
        {
            this.machineURL = machineURL;
            this.machineIDport = machineIDport;
            this.operatorID = operatorID;
        }
        public void addReplica(Replica y)
        {
           replicas.Add(y);
        }
    }
    [Serializable]
    public class Replica
    {
        public String replicaURL;
        public int replicaID;
        public int replicaIDport;

        public Replica(String replicaURL, int replicaID, int port)
        {
            this.replicaURL = replicaURL;
            this.replicaID = replicaID;
            this.replicaIDport = port;
        }
    }
}
