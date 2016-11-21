using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DADstorm
{
    public class Operator : MarshalByRefObject
    {
        public OperatorInformation information = new OperatorInformation();
        public List<SourceOPs> sourceoperators = new List<SourceOPs>();

        public ListOfTuples input = new ListOfTuples();
        public ListOfTuples output = new ListOfTuples();

        public bool isConnected = false;
        public string testforico = "Test 1";
        public bool test2 = true;
        private IExecutor executor;

        public Operator(IExecutor executor)
        {
            this.executor = executor;
        }
        public Operator()
        {
        }

        public Tuple execute()
        {
            return this.executor.execute();
        }

        public void setInput(Tuple input)
        {
            this.executor.setInput(input);
        }

        public int getPortToInput(string name)
        {
            int porttoinput = 0;
            foreach (var x in sourceoperators)
            {
                if (x.name == name)
                    porttoinput = x.portnumber;
            }
            return porttoinput;
        }
        // TODO: Diana
        public void connectToInput()
        {
            int porttoconnect = 0;

            // WE HAVE TO SOLVE PROBLEM WITH WAITING
            /*
            if (information.port == 12001)
                Thread.Sleep(2000);
            if (information.port == 12002)
                Thread.Sleep(6000);
            if (information.port == 12003)
                Thread.Sleep(10000);
*/

            //inputSource.Add("D:\\followers.dat");
            foreach (string tmp in information.inputsource)
            {
                

                if (Regex.IsMatch(tmp, "^OP\\d+$")) //operator in format OP1, OP2, ..., OPn
                {
                    porttoconnect = getPortToInput(tmp);
                    Console.WriteLine("Operator connecting to OP: " + tmp + " , by port: " + porttoconnect);
                    //CLIENT
                    string stringbuilder = "tcp://localhost:" + Convert.ToInt32(porttoconnect) + "/op";

                    Operator operatorImage = (Operator)Activator.GetObject(
                                   typeof(Operator),
                                   stringbuilder);
                    input = operatorImage.output;
              
                    if (operatorImage == null)
                    {
                        Console.WriteLine("Could not locate server ---> Can´t connect to :"+tmp);
                    }
                    else
                    {
                        Console.WriteLine("Connected to INPUT: "+tmp +" test : "+ input.returnHello());
                        Thread.Sleep(3000);
                        
                    }

                    isConnected = true;
                    
                }
                else if (Regex.IsMatch(tmp, @"^\d+$")) //operator as number 
                {
                   
                }
                else // input file
                {
                    string path = "";
                    foreach (var x in information.inputsource)
                    {
                        if(!(Regex.IsMatch(tmp, "^OP\\d+$")))
                        {
                            path = x;
                            Console.WriteLine("I found path : " + path);

                        }
                    }

                    string line;
                    Tuple inputTuple;
                    List<string> listItems;

                    System.IO.StreamReader file = new System.IO.StreamReader("..\\..\\doc\\" + path);
                    while ((line = file.ReadLine()) != null)
                    {
                          
                        if (line.Length != 0)
                        {
                            listItems = new List<string>();
                            if ((!String.Equals(line[0].ToString(), "%")))
                            {
                                string[] fields=line.Split(',');
                                foreach(string item in fields)
                                {
                                    listItems.Add(item);
                                }
                                inputTuple = new Tuple(listItems);
                                input.addToList(inputTuple);
                            }
                           
                        }
                        
                    }
                    isConnected = true;

                    file.Close();
                  //  Console.WriteLine("SHOW TUPLE :" + input.ToString());
                }
            }

            createOutput();
            input.showAll();

        }

        public void createOutput()
        {
            //HAS TO BE CHANGED !!! 
            output = input;
        }

        public bool connectionToInput()
        {
            return isConnected;
        }

        public Tuple getInput()
        {
            return this.executor.getInput();
        }

        public OperatorInformation getInformation()
        {
            return this.executor.getInformation();
        }








        //Igor _ code
        public void setOI(OperatorInformation x)
        {
            information = x;
        }
        public OperatorInformation getOI()
        { return information; }

        public void setSourceOPs(List<SourceOPs> x)
        {
            sourceoperators = x;
        }
        public List<SourceOPs> getSourceOPs()
        { return sourceoperators; }

        public string test()
        {
            string x = "NO";
            x = information.test();
            return x;
        }
        
        
        // IGOR´s TEST
        public string getTestForIco()
        {
            Console.WriteLine(testforico);
            return testforico;
        }
        public void setTestForIco(string x)
        {
            this.testforico = x;
        }


    }
}
