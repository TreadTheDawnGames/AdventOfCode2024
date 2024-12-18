// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using System.Xml;
using AdventOfCode.Tools;

namespace AdventOfCode;
class AdventOfCode
{
    static void Main(string[] args)
    {
        var starttime = DateTime.Now;
        string[] rawInput = File.ReadAllLines("Input\\Day6Input.txt");
        Day6.DoWork(rawInput);
        Console.WriteLine($"Runtime: {DateTime.Now - starttime}");
    }

    class LabGrid : WarehouseGrid
    {
        public LabGrid(Vector2 dimensions) : base(dimensions)
        {

        }
    }
}