using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day5
    {
        static Dictionary<int, List<int>> rules = new();
        public static void DoWork(string[] rawInput)
        {
            rules = GetRules(rawInput);
            List<List<int>> pageUpdates = ParseLines(rawInput, ',');
            List<List<int>> ruleFollowingUpdates = new();
            List<List<int>> ruleBreakingUpdates = new();
/*
            foreach (var key in rules.Keys)
            {
                Console.WriteLine("Key: " + key);
                foreach (var i in rules[key])
                {
                    Console.WriteLine(i);
                }
                Console.WriteLine("----");
            }
*/

            foreach (var pageUpdate in pageUpdates)
            {
                if (PageUpdateFollowsRules(pageUpdate))
                {
                    ruleFollowingUpdates.Add(pageUpdate);
                }
                else
                {
                    ruleBreakingUpdates.Add(pageUpdate);

                }
            }

                int sum = 0;
            foreach (var item in ruleBreakingUpdates)
            {
                Console.WriteLine("something");
                {
                    sum += GetCenterInt(ReorderBrokenRule(item));
                }
            }
            Console.WriteLine("Sum of center items for rule breaking updates: " + sum);

            //Part1Sum(ruleFollowingUpdates);

        }
        static List<int> ReorderBrokenRule(List<int> ruleBreakingUpdate)
        {
            List<int> reorderingOfUpdate = new();

            foreach (int i in ruleBreakingUpdate)
            {
                int position = 0;
                reorderingOfUpdate.Insert(position, i);

                while (!PageUpdateFollowsRules(reorderingOfUpdate))
                {
                    
                    reorderingOfUpdate.Remove(i);
                    reorderingOfUpdate.Insert(position++, i);
                    Console.WriteLine("Reordering");
                }
            }

            return reorderingOfUpdate;
                
        }


        private static void Part1Sum(List<List<int>> ruleFollowingUpdates)
        {
            int sum = 0;
            foreach (var ruleFollowingUpdate in ruleFollowingUpdates)
            {
                sum += GetCenterInt(ruleFollowingUpdate);
            }

            Console.WriteLine("Sum of center items for rule following updates: " + sum);
        }

        static bool PageUpdateFollowsRules(List<int> pageUpdate)
        {
            foreach(int pageUpdateNum in pageUpdate)
            {
                if (rules.ContainsKey(pageUpdateNum))
                {
                    foreach (int rule in rules[pageUpdateNum])
                    {
                            int pageUpdateIndexOf = pageUpdate.IndexOf(pageUpdateNum);
                            int ruleIndexOf = pageUpdate.IndexOf(rule);
                        if (pageUpdate.Contains(rule))
                        {

                            if (pageUpdateIndexOf < ruleIndexOf)
                            {
                                continue;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                
                }
            }
            return true;
            //compare index of i in pageUpdate to each of the rules


        }

        static int GetCenterInt(List<int> list)
        {
            return list[list.Count/2];
        }

        static Dictionary<int, List<int>> GetRules(string[] rawInput)
        {
            Dictionary<int, List<int>> rulebook = new();

                foreach (var rule in rawInput)
                {
                    try
                    {
                            string[] stringSplit = rule.Split('|');
                        List<int> split = new List<int>();
                        foreach(string stringSplit2 in stringSplit)
                        {
                            split.Add(int.Parse(stringSplit2));
                        }


                            if (rulebook.ContainsKey(split[0]))
                            {
                                rulebook[split[0]].Add(split[1]);
                                Console.WriteLine($"Adding {split[1]} to list{split[0]}");
                            }
                            else
                            {
                                rulebook.Add(split[0], new List<int>() { split[1] });
                                Console.WriteLine($"Creating new List{split[0]} with staring {split[1]}");

                            }
                    }
                    catch
                    {
                        if (rule.Contains(','))
                        {
                            break;
                        }
                    }
                   
            }
            
            return rulebook;

        }

        static List<List<int>> ParseLines(string[] rawInput, char splitChar)
        {
            List<List<int>> rules = new();// List<string>();
            foreach (var rule in rawInput)
            {
                List<int> ints = new List<int>();
                foreach(var split in rule.Split(splitChar))
                {
                    try
                    {
                        ints.Add(int.Parse(split.Trim()));

                    }
                    catch
                    {
                        break;
                    }
                }
                if (ints.Count > 0)
                rules.Add(ints);
            }
            return rules;
        }
        
    }
}
