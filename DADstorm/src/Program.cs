using DADstorm.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DADstorm
{
    class Program
    {
        static void Main(string[] args)
        {
            // PuppetMaster in new Thread
            PuppetMaster puppetMaster = new PuppetMaster();
            Thread puppetMasterProcess = new Thread(new ThreadStart(puppetMaster.start));
            puppetMasterProcess.Start();

            // new object, where ALL MACHINES WILL BE CREATED
            Init initMachines = new Init();
            Thread initProcess = new Thread(new ThreadStart(initMachines.start));
            initProcess.Start();
        }
    }
}
