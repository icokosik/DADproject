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
            this.input = Tuple.EMPTY;
        }

        public override bool checkInput()
        {
            throw new NotImplementedException();
        }

        public override Tuple execute()
        {
            Console.WriteLine(Path.GetFullPath(information.path));
            StreamWriter wr = new StreamWriter(Path.GetFullPath(information.path), true);
            wr.WriteLine(input.ToString());
            wr.Close();
            return Tuple.EMPTY;
        }
    }
}
