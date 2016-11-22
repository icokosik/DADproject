using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
        private OperatorInformation information = new OperatorInformation();
        private List<SourceOPs> sourceoperators = new List<SourceOPs>();

        private ListOfTuples input = new ListOfTuples();
        private ListOfTuples output = new ListOfTuples();

        private string stringbuilder;
        private bool informationUploaded = false;
        private IExecutor executor;
        
        public Operator() : base() { }

        /**
         * Generate the output for each input Tuple and add it to the output list
         */
        public void execute()
        {
            foreach(Tuple t in input.tuplesArray)
            {
                setInput(t);
                output.addToList(this.executor.execute());
            }
        }

        /**
         * Set the proper Executor for this Operator, based on the information.type field
         */
        public void setExecutor()
        {
            switch(information.type)
            {
                case OperatorSpec.COUNT:
                    executor = new CountExecutor(information);
                    break;
                case OperatorSpec.CUSTOM:
                    executor = new CustomExecutor(information);
                    break;
                case OperatorSpec.DUP:
                    executor = new DupExecutor(information);
                    break;
                case OperatorSpec.FILTER:
                    executor = new FilterExecutor(information);
                    break;
                case OperatorSpec.UNIQ:
                    executor = new UniqExecutor(information);
                    break;
            }
        }
        
        /**
         * Connect to the input. This can either be a file or another operator.
         */
        public void connectToInput()
        {
            Console.WriteLine("Operator review... name: {0}, id: {1}, repl_factor: {2}, port:{3}, spec: {4}", information.name, information.id, information.repl_factor, information.port, information.type);
            
            // WE HAVE TO SOLVE PROBLEM WITH WAITING
            
            if (information.port == 12001)
                Thread.Sleep(2000);
            if (information.port == 12002)
                Thread.Sleep(6000);
            if (information.port == 12003)
                Thread.Sleep(10000);
            
            foreach (string operatorInput in information.inputsource)
            {
                if (Regex.IsMatch(operatorInput, "^OP\\d+$")) //operator in format OP1, OP2, ..., OPn
                {
                    connectToOperator(operatorInput);
                }
                else // input file
                {
                    connectToFile(operatorInput);
                }
            }
            createOutput();
        }

        /**
         * Connect to an Operator. Connect via Marshalling, than transfer its output to this input.
         */
        public void connectToOperator(string operatorInput)
        {
            int porttoconnect = getPortToInput(operatorInput);
            Console.WriteLine("Operator connecting to OP: " + operatorInput + " , by port: " + porttoconnect);

            //CLIENT
            stringbuilder = "tcp://localhost:" + Convert.ToInt32(porttoconnect) + "/op";
            Operator operatorImage = (Operator)Activator.GetObject(
                           typeof(Operator),
                           stringbuilder);

            if (operatorImage == null)
            {
                Console.WriteLine("Could not locate server ---> Can´t connect to :" + operatorInput);
            }
            else
            {
                input = operatorImage.getOutput();
                Console.WriteLine("Connected to INPUT: " + operatorInput);
                Thread.Sleep(3000);
            }
            informationUploaded = true;
        }

        // TODO: Implement different Routing options
        public int getPortToInput(string name)
        {
            switch (information.routing)
            {
                case RoutingOption.PRIMARY:
                    break;
                case RoutingOption.HASHING:
                    break;
                case RoutingOption.RANDOM:
                    break;
            }
            return getPrimary(name);
        }
        // TODO: Implement different Routing options
        // (this should only be called by RoutingOption.PRIMARY
        public int getPrimary(string name)
        {
            foreach (SourceOPs source in sourceoperators)
            {
                if (source.name == name)
                    return source.portnumber;
            }
            return -1;
        }

        /**
         * Connect to a file. Open a StreamReader on the file and parse its contents.
         */
        public void connectToFile(string path)
        {
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
                        string[] fields = line.Split(',');
                        foreach (string item in fields)
                        {
                            listItems.Add(item.Trim(' ').Trim('"'));
                        }
                        inputTuple = new Tuple(listItems);
                        input.addToList(inputTuple);
                    }
                }
            }
            informationUploaded = true;
            file.Close();
        }

        public void createOutput()
        {
            //output = input;
            Console.WriteLine("INPUTS:  --------");
            input.showAll();
            foreach(Tuple t in input.tuplesArray)
            {
                executor.setInput(t);
                Tuple result = executor.execute();
                if(result != Tuple.EMPTY)
                    output.addToList(executor.execute());
            }
            Console.WriteLine("OUTPUTS:   ----------");
            output.showAll();
        }

        /// <summary>
        /// Getters and Setters
        /// </summary>
        
        public ListOfTuples getInput()
        {
            return this.input;
        }

        public void setInput(Tuple input)
        {
            this.executor.setInput(input);
        }

        public ListOfTuples getOutput()
        {
            return this.output;
        }

        public OperatorInformation getInformation()
        {
            return this.executor.getInformation();
        }

        public void setInformation(OperatorInformation info)
        {
            information = info;
            informationUploaded = true;
        }

        public bool isInformationUploaded()
        {
            return informationUploaded;
        }

        public void setSourceOPs(List<SourceOPs> x)
        {
            sourceoperators = x;
        }

        public List<SourceOPs> getSourceOPs()
        {
            return sourceoperators;
        }
    }
}
