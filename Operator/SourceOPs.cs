using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
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
    }
}
