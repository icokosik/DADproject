using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace DADstorm
{
    
    public class Program
    {
        public static OperatorInformation information;

        static void Main(string[] args)
        {
            int puppetMasterPort = Int32.Parse(args[0]);
            getOperatorInformation(puppetMasterPort);
            CheckOperatorInformation();
            
            
            Console.ReadLine();
        }

        public static void getOperatorInformation(int port)
        {
            test();
            Console.WriteLine("Establishing connection with PuppetMaster on port " + port);
            TcpChannel channel = new TcpChannel(port);
            
             RemotingConfiguration.RegisterWellKnownServiceType(
             typeof(Operator),
                            "op",
                            WellKnownObjectMode.Singleton);

            // Receive OperatorInformation
            information = new OperatorInformation();
        }

        public static void CheckOperatorInformation()
        {
            switch(information.type)
            {
                case OperatorSpec.COUNT:
                    break;
                case OperatorSpec.DUP:
                    break;
                case OperatorSpec.FILTER:
                    break;
                case OperatorSpec.UNIQ:
                    break;
                case OperatorSpec.CUSTOM:
                    break;
            }
        }

        public static void test()
        {
            //TEST
            OperatorInformation info = new OperatorInformation();
            info.fieldnumber = 0;
            info.value = "hai";
            info.condition = FilterCondition.GREATER;
            Operator op = new Operator(new FilterExecutor(info));
            List<string> list = new List<string>();
            list.Add("ha");
            Tuple input = new Tuple(list);
            op.setInput(input);
            Console.WriteLine(op.execute().ToString());
            //ENDTEST
        }
    }








    // new class  ---- remoting
    public class SharedClass : MarshalByRefObject
    {
        public string x = "text";
        public SharedClass() : base()
        {
                showTestInConsole();
        }
        public string Hello()
        {
            return "Hello World!";
        }
        public void setText(string x)
        { this.x = x; }
        public string getText()
        { return x; }
        public void showTestInConsole()
        { Console.WriteLine(x); }
    }
}
