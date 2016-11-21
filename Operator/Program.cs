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
using System.Net.NetworkInformation;

namespace DADstorm
{
    
    public class Program
    {
        public static OperatorInformation information;
        public static  int puppetMasterPort;

        static void Main(string[] args)
        {
            puppetMasterPort = Int32.Parse(args[0]);
            getOperatorInformation(puppetMasterPort);            
            
            Console.ReadLine();
        }

        public static void getOperatorInformation(int port)
        {
            //test();
            Console.WriteLine("Establishing connection with PuppetMaster at port " + port);

            //SERVER
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel,false);

            Operator operator1 = new Operator();
            RemotingServices.Marshal(operator1, "op",
                typeof(Operator));

          
            //trying to connect
            bool isOIuploaded = false;
            do
            {
                if (operator1.isConnected)
                {
                    Console.WriteLine("___ FINALLY Connected to PM");
                    isOIuploaded = true;
                }
                else
                {
                    Console.WriteLine("___ NOT Connected to PM");
                    Thread.Sleep(300);
                }
            } while (isOIuploaded == false);


            information = operator1.getOI();
            
            Console.WriteLine("Operator review... name: {0}, id: {1}, repl_factor: {2}, port:{3}, spec: {4}" ,information.name , information.id , information.repl_factor, information.port, information.type);
            operator1.setExecutor();
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
            //ENDTEST
        }


        public static bool isConnected(string stringbuilder)
        {

            Uri url = new Uri(stringbuilder);
            string pingurl = string.Format("{0}", url.Host);
            string host = pingurl;

            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }
            return result;
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
