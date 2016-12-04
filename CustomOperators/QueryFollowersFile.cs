using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomOperators
{
    public class QueryFollowersFile
    {
        private static Dictionary<string, List<string>> dictTweeterFollower =
           new Dictionary<string, List<string>>();
        private static bool dictLoaded = false;

        private static void loadFollowersDict()
        {
            string followersFilepath = @"..\..\doc\followers.data";

            System.IO.StreamReader followersFile =
            new System.IO.StreamReader(followersFilepath, true);
            string line = followersFile.ReadLine();
            List<string> followers;
            while (line != null)
            {
                if (line[0] != '%')
                {
                    string[] tokens = line.Split(',');
                    followers = new List<string>();
                    if (dictTweeterFollower.ContainsKey(tokens[0]))
                    {
                        followers = dictTweeterFollower[tokens[0]];
                    }
                    for (int i = 1; i < tokens.Length; i++)
                    {
                        followers.Add(tokens[i]);
                    }
                    if (!dictTweeterFollower.ContainsKey(tokens[0]))
                    {
                        dictTweeterFollower.Add(tokens[0], followers);
                    }
                }
                line = followersFile.ReadLine();
            }
            dictLoaded = true;
        }

        public List<List<string>> getFollowers(List<string> inputTuple)
        {
            List<List<string>> outputTuples = new List<List<string>>();
            List<string> tuple;

            if (!dictLoaded) loadFollowersDict();
            if (dictTweeterFollower.ContainsKey(inputTuple[1]))
            {
                foreach (string follower in dictTweeterFollower[inputTuple[1]])
                {
                    tuple = new List<string>();
                    tuple.Add(follower);
                    outputTuples.Add(tuple);
                }
            }
            return outputTuples;
        }
    }
}
