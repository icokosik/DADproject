using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CustomOperators;

namespace DADstorm
{
    class CustomExecutor : Executor
    {
        private List<List<string>> outputList = new List<List<string>>();
 
        public CustomExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = new List<Tuple>();
        }

        public override List<Tuple> execute()
        {
            Assembly assembly = Assembly.LoadFile(Path.GetFullPath(information.dllLocation));
            string type = information.dllLocation.Split('.')[0] + "." + information.className;
            Console.WriteLine(type);
            Type t = assembly.GetType(type);
            foreach (var m in t.GetMethods())
                Console.WriteLine(m);
            dynamic exec = t.GetMethod(information.method);
            dynamic instance = Activator.CreateInstance(t);
            foreach(Tuple tuple in input)
            {
                List<List<string>> output = exec.Invoke(instance, new object[] { tuple.getItems() });
                outputList.AddRange(output);
            }
            return transformOutput();
        }

        private List<Tuple> transformOutput()
        {
            List<Tuple> result = new List<Tuple>();
            foreach(List<string> l in outputList)
            {
                result.Add(new DADstorm.Tuple(l));
            }
            return result;
        }

    }
}
