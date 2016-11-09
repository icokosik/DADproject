using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class PuppetMaster
    {
        ArrayList mainCMD;
        ArrayList allCMDinfile;
        public PuppetMaster()
        {
            String inputString;
            while (true)
            {
                Console.WriteLine("\n-->Commands:");
                Console.WriteLine("Start -operator_id");
                Console.WriteLine("Interval operator id x ms");
                Console.WriteLine("Status");
                Console.WriteLine("Crash processname");
                Console.WriteLine("Freeze processname");
                Console.WriteLine("Unfreeze processname");
                Console.WriteLine("Wait x ms");
                Console.WriteLine("Load (config file)");

                inputString = Console.ReadLine();
                mainCMD = mainCMDparser(inputString);
                switch (Convert.ToString(mainCMD[0]))
                {
                    case "Start":
                        start(mainCMD);
                        break;
                    case "Interval":
                        interval(mainCMD);
                        break;
                    case "Status":
                        status();
                        break;
                    case "Crash":
                        crash(mainCMD);
                        break;
                    case "Freeze":
                        freeze(mainCMD);
                        break;
                    case "Unfreeze":
                        unfreeze(mainCMD);
                        break;
                    case "Wait":
                        wait(mainCMD);
                        break;
                    case "Load":
                        loadConfigFile();
                        break;
                    case "Test":
                        test();
                        break;

                    default:
                        Console.WriteLine("Command doesn´t exist");
                        break;
                }
            }
        }
        public ArrayList mainCMDparser(string s)
        {
            ArrayList array = new ArrayList();
            string[] words = s.Split(' ');
            foreach (string word in words)
                array.Add(word);
            return array;
        }



        //PuppetMaster Commands !!!!!!!! IMPORTANT - waiting for the rest of program
        public void startCMD(string operatorID) {}
        public void intervalCMD(string operatorID,Int32 sleep_ms) { }
        public void statusCMD() { }
        public void crashCMD(string operatorID,string replicaID) { }
        public void freezeCMD(string operatorID, string replicaID) { }
        public void unfreezeCMD(string operatorID, string replicaID) { }
        public void waitCMD(Int32 wait_ms) { }
        public void test()
        {
            Tuple t1 = new Tuple(new List<object>
            {
                1, "test"
            });
            Tuple t2 = new Tuple(new List<object>
            {
                1, "test2"
            });
            Tuple t3 = new Tuple(new List<object>
            {
                1, "test2"
            });

            Operator op = new CountOperator(1, "1", 0, RoutingOption.PRIMARY, 1, new List<string> { "dummyaddress" });
            op.setInput(t1);
            writeOutput(op.execute());
            op.setInput(t2);
            writeOutput(op.execute());
            op.setInput(t3);
            writeOutput(op.execute());
        }

        public void writeOutput(Tuple t)
        {
            System.Console.WriteLine(t);
        }


        //CONFIG FILE
        public void loadConfigFile()
        {
            BasicFileReader configFile = new BasicFileReader(@"../../doc/dadstorm.config");
            allCMDinfile = configFile.returnFileArray();
            commandRecognizer();
        }

        public ArrayList loadInputFile(string path)
        {
            BasicFileReader inputFile = new BasicFileReader(path);
            return inputFile.returnFileArray();
        }

        public ArrayList parseLineToArray(string text)
        {
            ArrayList parseArray = new ArrayList();
            string[] words = text.Split(' ');
            foreach (string s in words)
                parseArray.Add(s);
            return parseArray;
        }

        public void commandRecognizer()
        {
            //...every line of ArrayList
            foreach (string line in allCMDinfile)
            {
                ArrayList x = parseLineToArray(line);

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
                //PuppetMaster to the stream processing nodes
                if (line.Contains("Interval"))
                    interval(x);
                if (line.Contains("Status"))
                    status();
                if (line.Contains("Start"))
                    start(x);
                if (line.Contains("Crash"))
                    crash(x);
                if (line.Contains("Freeze"))
                    freeze(x);
                if (line.Contains("Wait"))
                    wait(x);
                if (line.Contains("Unfreeze"))
                    unfreeze(x);
            }

        }

        //Commands made by Config File ---> pick data from commands
        public void semantic(ArrayList x)
        {
            string semantic_var = x[1].ToString();
        }
        public void loggingLevel(ArrayList x)
        {
            string logginglevel_var = x[1].ToString();
        }
        public void interval(ArrayList x)
        {
            string operatorID = x[1].ToString();
            Int32 sleep_ms = Convert.ToInt32(x[2].ToString());
            intervalCMD(operatorID, sleep_ms);
        }
        public void status()
        {
            //STATUS of all nodes
        }
        public void start(ArrayList x)
        {
            string operatorID = x[1].ToString();
            startCMD(operatorID);
        }
        public void crash(ArrayList x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            crashCMD(operatorID, replicaID);
        }
        public void freeze(ArrayList x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            freezeCMD(operatorID, replicaID);
        }
        public void wait(ArrayList x)
        {
            Int32 wait_ms = Convert.ToInt32(x[1].ToString());
            waitCMD(wait_ms);
        }
        public void unfreeze(ArrayList x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            unfreezeCMD(operatorID, replicaID);
        }




        //Operator SET UP - from Config File Command
        public void operatorsSetUp(ArrayList x)
        {
            //OPERATOR_ID input ops SOURCE_OP_ID1 | FILEPATH1,. . ., SOURCE_OP_IDn | FILEPATHn
            if (x.Count > 1)
                if (String.Equals(Convert.ToString(x[1]), "input"))
                {
                    string operator_id = Convert.ToString(x[0]);
                    ArrayList operator_source = new ArrayList();
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
                    Console.WriteLine("\n----->operator ID: " + operator_id);
                    foreach (var u in operator_source)
                        Console.WriteLine("----->operator souce: " + u);
                }

            for (int i = 0; i < x.Count; i++)
            {
                //rep fact REPL_FACTOR routing primary|hashing|random
                if ((String.Equals(Convert.ToString(x[i]), "rep")) && (String.Equals(Convert.ToString(x[i + 1]), "fact")))
                {
                    Int32 repl_factor = Convert.ToInt32(x[i + 2]); //number of replicas
                    Int32 routingnumber = 0;
                    string routinginput = Convert.ToString(x[i + 4]);
                    char[] delimiterChars = { '(', ')' };
                    string[] words = routinginput.Split(delimiterChars);
                    string routingfunction = words[0];  //name of routing function
                    if (String.Equals(routingfunction, "hashing"))
                        routingnumber = Convert.ToInt32(words[1]); //set up value, if HASH function
                    //print
                    Console.WriteLine("----->Repl factor: " + repl_factor + " Route-type: " + routingfunction + " Route-number: " + routingnumber);
                }

                //address URL1,. . .,URLn
                if ((String.Equals(Convert.ToString(x[i]), "address")))
                {
                    string input;
                    int counter = 0;
                    ArrayList address_array = new ArrayList(); //array of Address-es
                    do
                    {
                        input = Convert.ToString(x[i + counter + 1]);
                        string[] words2 = input.Split(',');
                        address_array.Add(words2[0]);
                        counter++;
                    } while (Convert.ToString(x[i + counter]).Contains(","));
                    //print
                    foreach (var u in address_array)
                        Console.WriteLine("----->address: " + u);
                }
                //operator spec OPERATOR_TYPE OPERATOR_PARAM1,. . ., OPERATOR_PARAMn
                if ((String.Equals(Convert.ToString(x[i]), "operator")) && (String.Equals(Convert.ToString(x[i + 1]), "spec")))
                {
                    string inputOperator = Convert.ToString(x[i + 2]); //name of input operator
                    Int32 field_number = 0;
                    string condition, value, dll, classvar, method, inputText;
                    condition = value = dll = classvar = method = inputText = "";
                    string[] words3;

                    switch (inputOperator)
                    {
                        case "UNIQ":
                            inputText = Convert.ToString(x[i + 3]);
                            words3 = inputText.Split(',');
                            field_number = Convert.ToInt32(words3[0]);
                            break;
                        case "COUNT":
                            //?
                            break;
                        case "DUP":
                            //?
                            break;
                        case "FILTER":
                            inputText = Convert.ToString(x[i + 3]);
                            words3 = inputText.Split(',');
                            field_number = Convert.ToInt32(words3[0]);
                            condition = words3[1];
                            value = words3[2];
                            break;
                        case "CUSTOM":
                            inputText = Convert.ToString(x[i + 3]);
                            words3 = inputText.Split(',');
                            dll = words3[0];
                            classvar = words3[1];
                            method = words3[2];
                            break;

                    }
Console.WriteLine("-----> input operator: {0}, field_number: {1}, condition: {2}, value: {3}, dll: {4}, class: {5}, method: {6}", inputOperator, field_number, condition, value, dll, classvar, method);
                }
            }
            
        }
    }
}
