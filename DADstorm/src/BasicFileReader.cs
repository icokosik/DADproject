using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class BasicFileReader
    {
        StreamReader file;
        ArrayList cmdArray = new ArrayList();

        public BasicFileReader(string path)
        {
            uploadFile(path);
        }
        public void uploadFile(string path)
        {
            string line;
            //path
            file = new StreamReader(path);
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

        public ArrayList returnFileArray()
        {
            return cmdArray;
        }
 
    }
}
