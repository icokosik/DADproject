using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class ConfigFileReader
    {
        StreamReader file;
        ArrayList cmdArray = new ArrayList();

        public ConfigFileReader()
        {
            uploadFile();
        }
        public void uploadFile()
        {
            string line;
            //path
            file = new StreamReader(@"../../dadstorm.config");
            //config file to ArrayList
            while ((line = file.ReadLine()) != null)
            {
                //if line is NOT empty
                if (line.Length != 0)
                {
                    //if line is NOT comment
                    if ((!String.Equals(line[0].ToString(), "%")))
                    {
                        cmdArray.Add(line);
                    }
                }
            }
            file.Close();
        }

        public ArrayList returnCMDArray()
        {
            return cmdArray;
        }
 
    }
}
