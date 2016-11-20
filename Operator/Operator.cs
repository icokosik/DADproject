using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DADstorm
{
    public class Operator : MarshalByRefObject
    {
        public string testforico = "Test 1";
        private IExecutor executor;

        public Operator(IExecutor executor)
        {
            this.executor = executor;
        }
        public Operator()
        { }

        public Tuple execute()
        {
            return this.executor.execute();
        }

        public void setInput(Tuple input)
        {
            this.executor.setInput(input);
        }

        // TODO: Diana
        public void connectToInput()
        {
            List<string> listItems = new List<string>(); 
            

            //inputSource.Add("D:\\followers.dat");
            foreach(string tmp in this.getInformation().inputsource)
            {
                if (Regex.IsMatch(tmp, "^OP\\d+$")) //operator in format OP1, OP2, ..., OPn
                {
                    
                }
                else if (Regex.IsMatch(tmp, @"^\d+$")) //operator as number 
                {
                   
                }
                else // input file
                {

                    string line;
                    //path
                    System.IO.StreamReader file = new System.IO.StreamReader(tmp);
                    //
                    while ((line = file.ReadLine()) != null)
                    {
                        //if line is NOT empty
                        if (line.Length != 0)
                        {
                            //if line is NOT comment
                            if ((!String.Equals(line[0].ToString(), "%")))
                            {
                                string[] fields=line.Split(',');
                                foreach(string item in fields)
                                {
                                    listItems.Add(item);
                                }
                                executor.setInput(new Tuple(listItems));
                                //execute operator
                            }
                        }
                    }
                    file.Close();
                }
            }
 
        }

        public bool connectionToInput()
        {
            return false;
        }

        public Tuple getInput()
        {
            return this.executor.getInput();
        }

        public OperatorInformation getInformation()
        {
            return this.executor.getInformation();
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
