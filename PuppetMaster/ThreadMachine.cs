using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class ThreadMachine
    {
        public int portnumber;
        public MachinePackage machines;
        public List<MachineWithReplicas> machines2 = new List<MachineWithReplicas>();

        public ThreadMachine(int portnumber, MachinePackage machines)
        {
            this.portnumber = portnumber;
            this.machines = machines;
            this.machines2 = machines.machines;
        }
        public void start()
        {
            string address = "tcp://localhost:" + Convert.ToInt32(portnumber) + "/machine";
            MachinePackage op = (MachinePackage)Activator.GetObject(
                              typeof(MachinePackage),
                              address);
            op.setMachines(machines2);

            if (op == null)
                Console.WriteLine("Could not locate server");
            else
            {
                op.test = true;
            }
        }
    }
}
