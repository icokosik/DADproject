﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    class ConfigFile
    {
        public LoggingLevel logging = LoggingLevel.LIGHT;
        List<Operator> operatorsArray = new List<Operator>();
        List<string> allCMDinfile;

        public ConfigFile()
        {

        }

        //CONFIG FILE
        public void loadConfigFile()
        {
            BasicFileReader configFile = new BasicFileReader(@"../../doc/dadstorm.config");
            allCMDinfile = configFile.returnFileArray();
            commandRecognizer();
        }


        public List<string> parseLineToArray(string text)
        {
            List<string> parseArray = new List<string>();
            string[] words = text.Split(' ');
            foreach (string s in words)
                parseArray.Add(s);
            return parseArray;
        }


        //PuppetMaster Commands !!!!!!!! IMPORTANT - waiting for the rest of program
        public void startCMD(string operatorID) { }
        public void intervalCMD(string operatorID, Int32 sleep_ms) { }
        public void statusCMD() { }
        public void crashCMD(string operatorID, string replicaID) { }
        public void freezeCMD(string operatorID, string replicaID) { }
        public void unfreezeCMD(string operatorID, string replicaID) { }
        public void waitCMD(Int32 wait_ms) { }
        public void test()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    System.Console.WriteLine("int " + i);
                }
            });
            Task.Run(() =>
            {
                for (int i = 1000; i < 1100; i++)
                {
                    System.Console.WriteLine("int " + i);
                }
            });
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
        public void interval(List<string> x)
        {
            string operatorID = x[1].ToString();
            Int32 sleep_ms = Convert.ToInt32(x[2].ToString());
            intervalCMD(operatorID, sleep_ms);
        }
        public void status()
        {
            //STATUS of all nodes
        }
        public void start(List<string> x)
        {
            string operatorID = x[1].ToString();
            startCMD(operatorID);
        }
        public void crash(List<string> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            crashCMD(operatorID, replicaID);
        }
        public void freeze(List<string> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            freezeCMD(operatorID, replicaID);
        }
        public void wait(List<string> x)
        {
            Int32 wait_ms = Convert.ToInt32(x[1].ToString());
            waitCMD(wait_ms);
        }
        public void unfreeze(List<string> x)
        {
            string operatorID = x[1].ToString();
            string replicaID = x[2].ToString();
            unfreezeCMD(operatorID, replicaID);
        }


        public void commandRecognizer()
        {
            //...every line of ArrayList
            foreach (string line in allCMDinfile)
            {
                List<string> x = parseLineToArray(line);

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







        //Operator SET UP - from Config File Command
        public void operatorsSetUp(List<string> x)
        {
            //OPERATOR_ID input ops SOURCE_OP_ID1 | FILEPATH1,. . ., SOURCE_OP_IDn | FILEPATHn
            if (x.Count > 1)
                if (String.Equals(Convert.ToString(x[1]), "input"))
                {
                    string operator_id = Convert.ToString(x[0]);
                    List<string> operator_source = new List<string>();
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
                    List<string> address_array = new List<string>(); //array of Address-es //because of printing
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
                    FilterCondition condition;
                    string value, dll, classvar, method, inputText;
                    value = dll = classvar = method = inputText = "";
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

                    Console.WriteLine("-----> input operator: {0}, field_number: {1}, condition: ENUM TYPE ???, value: {3}, dll: {4}, class: {5}, method: {6}", inputOperator, field_number, value, dll, classvar, method);
                }
            }


        }
    }
}
