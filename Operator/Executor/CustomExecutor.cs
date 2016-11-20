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
            this.input = Tuple.EMPTY;
        }

        public override bool checkInput()
        {
            throw new NotImplementedException();
        }

        public override Tuple execute()
        {
            Assembly assembly = Assembly.LoadFile(information.dllLocation);
            Type t = assembly.GetType(information.className);
            dynamic method = t.GetMethod(information.method);
            dynamic obj = Activator.CreateInstance(t);
            string output = method.Invoke(obj, new object[] { });
            return new Tuple(new List<string>() { output });
        }

    }
}
