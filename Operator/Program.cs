using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.IO;

namespace DADstorm
{
    
    public class Program
    {
        public static OperatorInformation information;
        

        static void Main(string[] args)
        {
            int puppetMasterPort = Int32.Parse(args[0]);
            getOperatorInformation(puppetMasterPort);            
            
            Console.ReadLine();
        }

        public static void getOperatorInformation(int port)
        {
            test();
            Console.WriteLine("Establishing connection with PuppetMaster at port " + port);

            //SERVER
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel,false);

            Operator operator1 = new Operator();
            RemotingServices.Marshal(operator1, "op",
                typeof(Operator));

            //do NOT delete "sleep" !!!
            Thread.Sleep(2000);
            information = operator1.getOI();

            Console.WriteLine("Operator review... name: {0}, id: {1}, repl_factor: {2}, port:{3}" ,information.name , information.id , information.repl_factor, information.port);

            operator1.connectToInput();

        }

        public static void test()
        {
            //TEST
            OperatorInformation info = new OperatorInformation();
            info.dllLocation = Path.GetFullPath("..\\..\\..\\CustomOperators\\bin\\Debug\\CustomOperators.dll");
            info.className = "CustomOperators.HelloWorld";
            info.method = "hello";
            Operator op = new Operator(new CustomExecutor(info));
            List<string> list = new List<string>() { "ha" };
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
