using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class OutputExecutor : Executor
    {

        public OutputExecutor(OperatorInformation information)
        {
            this.information = information;
            this.input = new List<Tuple>();
        }

        public override List<Tuple> execute()
        {
            List<string> output = new List<string>();
            switch(this.originOPType)
            {
                case OperatorSpec.COUNT:
                    output.Add(input.Last().ToString());
                    break;
                default:
                    foreach(Tuple t in input)
                    {
                        output.Add(t.ToString());
                    }
                    break;
            }
            StreamWriter wr = new StreamWriter(Path.GetFullPath(information.path), true);
            foreach(string line in output)
                wr.WriteLine(line);
            wr.Close();
            return new List<Tuple>();
        }
    }
}
