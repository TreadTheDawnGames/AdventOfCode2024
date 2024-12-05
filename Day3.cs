using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day3
    {
        public static void DoWork(string[] rawInputLines)
        {
            //Part1(rawInputLines);
            Part2(rawInputLines);
        }

        static void Part2(string[] rawInputLines)
        {
            Regex mulRegEx = new Regex("mul\\([0-9]{1,3},[0-9]{1,3}\\)|(don't\\(\\))|do\\(\\)");
            List<int> multipliedNums = new List<int>();
            bool multiplyOn = true;

            foreach (string line in rawInputLines)
            {
                foreach (var match in mulRegEx.Matches(line))
                {
                    //Console.WriteLine(match.ToString());
                    string matchString = match.ToString();

                    Regex numsRegEx = new Regex("[0-9]{1,3}");
                    Regex doRegEx = new Regex("do\\(\\)");
                    Regex dontRegEx = new Regex("don't\\(\\)");

                    if (doRegEx.IsMatch(matchString))
                    {
                        multiplyOn = true;
                        //Console.WriteLine("Multiplying ON" + matchString);
                    }

                     if (dontRegEx.IsMatch(matchString))
                    {
                        //Console.WriteLine("Multiplying OFF");
                        multiplyOn = false;
                    }

                     if (numsRegEx.IsMatch(matchString) && multiplyOn)
                    {
                        var numMatches = numsRegEx.Matches(matchString);
                        var mulNum = (int)(int.Parse(numMatches[0].ToString()) * nint.Parse(numMatches[1].ToString()));
                        multipliedNums.Add(mulNum);
                    }
                }
            }
            int outputSum = 0;
            foreach (var num in multipliedNums)
            {
                outputSum += num;
            }
            Console.WriteLine("SumOfMul: " + outputSum);
        }

        static void Part1(string[] rawInputLines)
        {
            Regex mulRegEx = new Regex("mul\\([0-9]{1,3},[0-9]{1,3}\\)");
            List<int> multipliedNums = new List<int>();
            foreach (string line in rawInputLines)
            {

                foreach (var match in mulRegEx.Matches(line))
                {
                    Regex numsRegEx = new Regex("[0-9]{1,3}");

                    var numMatches = numsRegEx.Matches(match.ToString());

                    var mulNum = (int)(int.Parse(numMatches[0].ToString()) * nint.Parse(numMatches[1].ToString()));
                    multipliedNums.Add(mulNum);
                }
            }
            int outputSum = 0;
            foreach (var num in multipliedNums)
            {
                outputSum += num;
            }
            Console.WriteLine("SumOfMul: " + outputSum);
        }

    }
}
