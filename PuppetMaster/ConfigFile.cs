using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DADstorm
{
    class ConfigFile
    {
        public LoggingLevel logging = LoggingLevel.LIGHT;
        private List<OperatorInformation> operatorsArray = new List<OperatorInformation>();
        public List<ReplicasInOP> replicasArray=new List<ReplicasInOP>();
        public List<MachineWithReplicas> machines = new List<MachineWithReplicas>();
        List<string> allCMDinfile;
        public List<String> scripts=new List<string>();
        private static List<string> startOPs = new List<string>();

        public int port = 12000;
        public int portM = 13000;
        public static int operatorCount = 0;

        public ConfigFile()
        {
            loadConfigFile();
        }

        //CONFIG FILE
        public void loadConfigFile()
        {
            BasicFileReader configFile = new BasicFileReader(@"../../doc/dadstorm.config");
            allCMDinfile = configFile.returnFileArray();
            commandRecognizer();
        }

        public List<OperatorInformation> getOperatorsArray() {
            return operatorsArray;
        }

        public LoggingLevel returnLogging()
        {
            return logging;
        }

        public List<string> parseLineToArray(string text)
        {
            List<string> parseArray = new List<string>();
            string[] words = text.Split(' ');
            foreach (string s in words)
                parseArray.Add(s);
            return parseArray;
        }
        
        //Commands made by Config File ---> pick data from commands
        public void semantic(List<string> x)
        {
            string semantic_var = x[1].ToString();
        }
        public void loggingLevel(List<string> x)
        {
            string logginglevel_var = x[1].ToString();
            if (String.Equals(logginglevel_var, "full"))
                logging = LoggingLevel.FULL;
            else logging = LoggingLevel.LIGHT;
        }
   
        public void commandRecognizer()
        {
            //...every line of ArrayList
            foreach (string line in allCMDinfile)
            {
                List<string> x = parseLineToArray(line);

                string[] scriptWords = { "Start", "Interval", "Status", "Freeze", "Unfreeze","Crash", "Wait" };

                //header
                if (line.Contains("Semantics"))
                {
                    semantic(x);
                }
                if (line.Contains("LoggingLevel"))
                {
                    loggingLevel(x);
                }
                //operator_id
                if (line.Contains("input ops"))
                    operatorsSetUp(x);
                //script for PM
                foreach(var check in scriptWords)
                if (line.Contains(check))
                    scripts.Add(line);
            }
        }
        

        //Operator SET UP - from Config File Command
        public void operatorsSetUp(List<string> x)
        {
            string operator_name, routingfunction, value, dllLocation, className, method, inputText;
            operator_name = routingfunction = value = dllLocation = className = method = inputText = "";
            int field_number =0; 
            int repl_factor=0;
            int routingnumber = 0;
            FilterCondition condition=FilterCondition.EQUALS;
            RoutingOption routing=RoutingOption.PRIMARY;
            OperatorSpec type=OperatorSpec.COUNT;
            List<string> operator_source = new List<string>();
            List<string> address_array= new List<string>();
            
            //OPERATOR_ID input ops SOURCE_OP_ID1 | FILEPATH1,. . ., SOURCE_OP_IDn | FILEPATHn
            if (x.Count > 1)
                if (String.Equals(Convert.ToString(x[1]), "input"))
                {
                    operator_name = Convert.ToString(x[0]);
                    string input;
                    int counter = 3;
                    do
                    {
                        input = Convert.ToString(x[counter]);
                        string[] words2 = input.Split(',');
                        operator_source.Add(words2[0]);
                        counter++;
                    } while (Convert.ToString(x[counter]).Contains(","));


                    
                    //print
                    Console.WriteLine("\n----->operator ID: " + operator_name);
                    foreach (var u in operator_source)
                        Console.WriteLine("----->operator souce: " + u);
                }

            for (int i = 0; i < x.Count; i++)
            {
                //rep fact REPL_FACTOR routing primary|hashing|random
                if ((String.Equals(Convert.ToString(x[i]), "rep")) && (String.Equals(Convert.ToString(x[i + 1]), "fact")))
                {
                    repl_factor = Convert.ToInt32( x[i + 2]); //number of replicas
                    routingnumber = 0;
                    string routinginput = Convert.ToString(x[i + 4]);
                    char[] delimiterChars = { '(', ')' };
                    string[] words = routinginput.Split(delimiterChars);
                    
                    routingfunction = words[0];  //name of routing function

                    switch(routingfunction)
                    {
                        case "hashing":
                            routing = RoutingOption.HASHING;
                            routingnumber = Convert.ToInt32(words[1]);
                            break;
                        case "primary":
                            routing = RoutingOption.PRIMARY;
                            break;
                        case "random":
                            routing = RoutingOption.RANDOM;
                            break;
                        default:
                            break;
                    }
                    //print

                    Console.WriteLine("----->Repl factor: " + repl_factor + " Route-type: " + routingfunction + " Route-number: " + routingnumber);
                }

                //address URL1,. . .,URLn
                if ((String.Equals(Convert.ToString(x[i]), "address")))
                {
                    string input;
                    int counter = 0;
                    do
                    {
                        input = Convert.ToString(x[i + counter + 1]);
                        string[] words2 = input.Split(',');
                        address_array.Add(words2[0]);
                        String ip = words2[0].Split(':')[0]+ ":"+words2[0].Split(':')[1];

                        Boolean existsMachine = false;
                        foreach (var m in machines)
                        {
                            if (m.machineURL.Equals(ip))
                                existsMachine = true;
                        }

                        if (!existsMachine)
                        {
                            machines.Add(new MachineWithReplicas(ip, portM, operator_name));
                            portM++;
                        }

                        foreach (var z in machines)
                        {
                            if (z.machineURL.Equals(ip))
                                z.addReplica(new Replica(words2[0],counter,port));
                        }
                        
                       
                       // replicasArray.Add(new ReplicasInOP(ip,operator_name,words2[0],counter,port));
                        counter++;
                        port++;
                        
                    } while (Convert.ToString(x[i + counter]).Contains(","));
                    //print
                    foreach (var u in address_array)
                        Console.WriteLine("----->address: " + u);
                    

                }
                //operator spec OPERATOR_TYPE OPERATOR_PARAM1,. . ., OPERATOR_PARAMn
                if ((String.Equals(Convert.ToString(x[i]), "operator")) && (String.Equals(Convert.ToString(x[i + 1]), "spec")))
                {
                    string inputOperator = Convert.ToString(x[i + 2]); //name of input operator
                    Console.WriteLine("----->operator type: " + inputOperator);
                    string[] words3;

                    switch (inputOperator)
                    {
                        case "UNIQ":
                            inputText = Convert.ToString(x[i + 3]);
                            words3 = inputText.Split(',');
                            field_number = Convert.ToInt32(words3[0]) - 1;
                            type = OperatorSpec.UNIQ;
                            break;
                        case "COUNT":
                            type = OperatorSpec.COUNT;
                            break;
                        case "DUP":
                            type = OperatorSpec.DUP;
                            break;
                        case "FILTER":
                            inputText = Convert.ToString(x[i + 3]);
                            words3 = inputText.Split(',');
                            field_number = Convert.ToInt32(words3[0]) - 1;
                            switch (words3[1])
                            {
                                case "<":
                                    condition = FilterCondition.SMALLER;
                                    break;
                                case ">":
                                    condition = FilterCondition.GREATER;
                                    break;
                                case "=":
                                    condition = FilterCondition.EQUALS;
                                    break;
                                default: break;
                            }
                            value = words3[2].Trim('"');
                            type = OperatorSpec.FILTER;
                            break;
                        case "CUSTOM":
                            inputText = Convert.ToString(x[i + 3]);
                            words3 = inputText.Split(',');
                            dllLocation = words3[0];
                            className = words3[1];
                            method = words3[2];
                            type = OperatorSpec.CUSTOM;
                            break;

                    }
                }
            }

            // Add operator to list, one for each replica_fact.
            for (int i = 0; i < repl_factor; i++)
            {
                operatorsArray.Add(new OperatorInformation(operatorCount, operator_name, operator_source, routing, routingnumber, address_array[i], type, field_number, value, condition, dllLocation, className, method, logging));
                operatorCount++;
            }
            if(!containsInputOP(operator_source))
            {
                startOPs.Add(operator_name);
            }
        }

        private static bool containsInputOP(List<string> inputs)
        {
            foreach(string input in inputs)
            {
                if (Regex.IsMatch(input, "^OP\\d+$"))
                    return true;
            }
            return false;
        }

        public static List<string> getStartOPs()
        {
            return startOPs;
        }
    }
}
