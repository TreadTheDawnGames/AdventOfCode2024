using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day2
    {
        
    
        public static void DoWork(string[] rawInput)
        {
            int counter = 0;
            int safeCounter = 0;
            foreach (string rawInputLine in rawInput)
            {
                if (IsLineSafe(ExtractIntsFromLine(rawInputLine)))
                {
                    Console.WriteLine("Safe");

                    safeCounter++;
                }
                else { 
                    for (int i = -1; i < ExtractIntsFromLine(rawInputLine).Count; i++)
                        if (IsLineSafe(ExtractIntsFromLine(rawInputLine), i))
                        {

                            counter++;
                        }
                    if (counter >= ExtractIntsFromLine(rawInputLine).Count)
                    {
                        Console.WriteLine("Safe");

                        safeCounter++;
                    }
                    else
                    {
                        Console.WriteLine("Bad");

                    }
                }
                    counter = 0;
            }
            Console.WriteLine();
            Console.WriteLine("SafeLines: "+safeCounter);// counter.ToString();
        }

        static bool IsLineSafe(List<int> level, int skipIndex)
        {
            int totalChecks = level.Count - 2;
            int checksGreaterThan = 0;
            int checksLessThan = 0;

            //foreach number on the level except the last
            for (int i = 0; i < level.Count - 1; i++)
            {
                if (i == skipIndex)
                {
                    i++;
                    totalChecks--;
                    if (i >= level.Count - 1)
                    {
                        i--;
                    }
                }
                //set the variables
                int j = level[i];
                int k = level[i + 1];

                //filter out bad matches
                if (MathF.Abs(j - k) > 3 || j == k)
                {
                    //  cout << "Continued" << endl;
                    continue;
                }

                //do comparison
                if (j > k)
                {
                    checksLessThan++;
                }
                else if (j < k)
                {
                    checksGreaterThan++;
                }
            }
                //if the checks score correctly
                if (checksLessThan == totalChecks || checksGreaterThan == totalChecks)
                {
                    return true;
                }
                else
                {

                    return false;
                }

            

        }
        static bool IsLineSafe(List<int> level)
        {
            int totalChecks = level.Count - 2;
            int checksGreaterThan = 0;
            int checksLessThan = 0;

            //foreach number on the level except the last
            for (int i = 0; i < level.Count - 1; i++)
            {
               
                //set the variables
                int j = level[i];
                int k = level[i + 1];

                //filter out bad matches
                if (MathF.Abs(j - k) > 3 || j == k)
                {
                    //  cout << "Continued" << endl;
                    continue;
                }

                //do comparison
                if (j > k)
                {
                    checksLessThan++;
                }
                else if (j < k)
                {
                    checksGreaterThan++;
                }
            }
                //if the checks score correctly
                if (checksLessThan == totalChecks || checksGreaterThan == totalChecks)
                {
                    return true;
                }
                else
                {

                    return false;
                }

            

        }

        static List<int> ExtractIntsFromLine(string line)
        {
            List<int> intList = new();
            while (true)
            {
                int splitBeginIndex = line.LastIndexOf(" ");
                string leftLine = "";

                if (splitBeginIndex >= 0)
                {

                    leftLine = line.Substring(splitBeginIndex);
                }
                else
                {
                    leftLine = line;
                }


                intList.Add(int.Parse(leftLine));

                if (splitBeginIndex >= 0)
                    line = line.Remove(splitBeginIndex);
                else
                    break;


            }

                intList.Reverse();
            //cout << endl;
            return intList;

            //leftLine += line until splitIndex
        }
    }

}
