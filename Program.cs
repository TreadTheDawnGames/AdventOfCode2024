// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using System.Xml;

namespace AdventOfCode;
class AdventOfCode
{
    static void Main(string[] args)
    {
        var starttime = DateTime.Now;
        string[] rawInput = File.ReadAllLines("Input\\Day5Input.txt");
        Day5.DoWork(rawInput);
        Console.WriteLine($"Runtime: {DateTime.Now - starttime}");
    }

    
}