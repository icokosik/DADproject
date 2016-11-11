using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DADstorm
{
    public abstract class Operator
    {

        protected int id;
        protected string name;
        protected List<string> inputSource;
        protected RoutingOption routing;
        protected int replicas;
        protected List<string> addresses;
        protected Tuple input;

        public abstract bool checkInput();
        public abstract Tuple execute();

        public Operator(int id, string name, List<string> inputSource, RoutingOption routing, int replicas, List<string> addresses)
        {
            // TODO: check id != inputsource
            this.id = id;
            this.name = name;
            this.inputSource = inputSource;
            this.routing = routing;
            this.replicas = replicas;
            this.addresses = addresses;
        }

        public Operator()
        {
            // TODO: Complete member initialization
        }

        public void setInput(Tuple input)
        {
            this.input = input;
        }

        // TODO: Diana
        public void connectToInput()
        {
            List<List<Object>> listItems = new List<List<Object>>(); 
            

            //inputSource.Add("D:\\followers.dat");
            foreach(string tmp in inputSource)
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
                                List<Object> tmp2 = new List<Object>();
                                foreach(string item in fields)
                                {
                                    tmp2.Add(item);

                                } 
                                listItems.Add(tmp2);
                            }
                        }
                    }
                    file.Close();
                    //input = new Tuple(listItems);
                }
            }
 
        }

        public bool connectionToInput()
        {
            return false;
        }

    }
}
