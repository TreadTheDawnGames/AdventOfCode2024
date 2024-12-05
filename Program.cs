// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using System.Xml;

namespace AdventOfCode;
class AdventOfCode
{
    static void Main(string[] args)
    {
        string[] rawInput = File.ReadAllLines("testInput.txt");
        Day4.DoWork(rawInput);
    }

    
}