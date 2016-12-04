using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class CustomExecutor : Executor
    { 
        public CustomExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = new List<Tuple>();
        }

        public override List<Tuple> execute()
        {
            Assembly assembly = Assembly.LoadFile(information.dllLocation);
            Type t = assembly.GetType(information.className);
            dynamic method = t.GetMethod(information.method);
            dynamic obj = Activator.CreateInstance(t);
            List<Tuple> output = method.Invoke(obj, new object[] { });
            return output;
        }

    }
}
