using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day1
    {
        static void DoWork(string[] rawInput)
        {
            List<int> leftList = new List<int>();
            List<int> rightList = new List<int>();


            foreach (string line in rawInput)
            {
                var splitLine = line.Split("   ");
                leftList.Add(int.Parse(splitLine[0]));
                rightList.Add(int.Parse(splitLine[1]));
            }

            leftList.Sort();
            rightList.Sort();

            Console.WriteLine("Total Distance: " + GetTotalDistanceBetweenLists(leftList, rightList));
            Console.WriteLine("Similarity Score: " + GetSimilarityScore(leftList, rightList));

        }

static int GetSimilarityScore(List<int> leftList, List<int> rightList)
{
    int output = 0;

    foreach (int x in leftList)
    {

        output += x * rightList.FindAll(new Predicate<int>((y) => x == y)).Count;
    }
    ///multiply x by n, where n == times n appears in rightList
    ///output += x        
    return output;
}
static int GetTotalDistanceBetweenLists(List<int> leftList, List<int> rightList)
{
    List<int> comparedNums = new();
    int output = 0;
    for (int i = 0; i < leftList.Count; i++)
    {
        comparedNums.Add((int)MathF.Abs(leftList[i] - rightList[i]));
    }

    foreach (int i in comparedNums)
    {
        output += i;
    }

    return output;
}
    }
}
