using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace DADstorm
{

    public class Operator : MarshalByRefObject
    {
        private OperatorInformation information = new OperatorInformation();
        private List<SourceOPs> inputRequesters = new List<SourceOPs>();
        private List<SourceOPs> outputOperators = new List<SourceOPs>();

        private ListOfTuples input = new ListOfTuples();
        private ListOfTuples output = new ListOfTuples();
        
        private ListOfTuples test = new ListOfTuples();
        private bool initialized = false;
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
        
        public void setOutputOperator()
        {
            // For every Operator type requesting, select the right instance
            List<SourceOPs> tempList = new List<SourceOPs>(information.outputs);
            while (tempList.Count > 0)
            {
                SourceOPs s = tempList[0];
                List<SourceOPs> candidates = tempList.FindAll(x => x.name == s.name);

                switch (information.routing)
                {
                    case RoutingOption.PRIMARY:
                        Console.WriteLine(getPrimary(candidates).name + " on port " + getPrimary(candidates).portnumber);
                        outputOperators.Add(getPrimary(candidates));
                        break;
                    case RoutingOption.HASHING:
                        outputOperators.Add(getHashing(candidates));
                        break;
                    case RoutingOption.RANDOM:
                        outputOperators.Add(getRandom(candidates));
                        break;
                }
                tempList.RemoveAll(x => x.name.Equals(s.name));
                Console.WriteLine("OUTPUT OPERATORS");
                foreach(SourceOPs op in outputOperators)
                {
                    Console.WriteLine(op.name + " on port ", op.portnumber);
                }
            }
        }
        
        public SourceOPs getPrimary(List<SourceOPs> list)
        {
            Console.WriteLine(list[0].name + " on port " + list[0].portnumber);
            return list[0];
        }

        public SourceOPs getHashing(List<SourceOPs> list)
        {
            return getPrimary(list);
        }

        public SourceOPs getRandom(List<SourceOPs> list)
        {
            Random rand = new Random();
            return list[rand.Next(list.Count)];
        }
        
        /**
         * Connect to the input. This can either be a file or another operator.
         */
        public void connectToInput()
        {
            Console.WriteLine("Operator review... name: {0}, id: {1}, port:{2}, spec: {3}", information.name, information.id, information.port, information.type);
            
            // Collect all input Tuples from all input sources
            foreach (string operatorInput in information.inputsource)
            {
                if (isOperator(operatorInput)) //operator in format OP1, OP2, ..., OPn
                {
                    // Wait until signalled that the input is uploaded by the inputOperator
                    EventWaitHandle inputReadySignaller = new EventWaitHandle(false, EventResetMode.AutoReset, operatorInput + information.name + information.port);
                    inputReadySignaller.WaitOne();
                }
                else // input file
                {
                    input.addToList(connectToFile(operatorInput));
                }
            }
            informationUploaded = true;
        }

        public bool isOperator(string operatorInput)
        {
            return Regex.IsMatch(operatorInput, "^OP\\d+$");
        }

        public void uploadToOutputs()
        {
            foreach(SourceOPs outputOp in outputOperators)
            {
                string address = "tcp://localhost:" + Convert.ToInt32(outputOp.portnumber) + "/op";
                Operator outputImage = (Operator)Activator.GetObject(
                               typeof(Operator),
                               address);
                if (outputImage == null)
                    Console.WriteLine("Could not locate server ---> Can´t connect to :" + outputOp.name);
                else
                {
                    while(!outputImage.isInitialized())
                    {
                        Thread.Sleep(200);
                    }
                    outputImage.addToInputTuples(getOutput());
                    EventWaitHandle uploadReadySignal = new EventWaitHandle(false, EventResetMode.AutoReset, this.information.name + outputOp.name + outputOp.portnumber);
                    uploadReadySignal.Set();
                }
            }
        }

        /**
         * Connect to a file. Open a StreamReader on the file and parse its contents.
         */
        public ListOfTuples connectToFile(string path)
        {
            string line;
            Tuple inputTuple;
            List<string> listItems;
            ListOfTuples result = new ListOfTuples();
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
                        result.addToList(inputTuple);
                    }
                }
            }
            file.Close();
            return result;
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
                    output.addToList(result);
            }
            Console.WriteLine("OUTPUTS:   ----------");
            output.showAll();
            
            if (information.logging == LoggingLevel.FULL)
            {
               connectToLogService().setLogData(information.address,output.getDataForLog());
            }
        }

        public LogService connectToLogService()
        {
            string stringbuilder = "tcp://localhost:10001/log";

            LogService objLog = (LogService)Activator.GetObject(
                          typeof(LogService),
                          stringbuilder);

            return objLog;
        }

        public void addToInputTuples(ListOfTuples tuples)
        {
            input.addToList(tuples);
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

        public bool isInitialized()
        {
            return initialized;
        }

        public void setInitialized(bool v)
        {
            initialized = v;
        }
    }
}
