using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class Day4
    {
        public static void DoWork(string[] rawInput)
        {
            List<List<string>> allPossibleStraightLines = new List<List<string>>();
            List<string> horizontalLines = new();
            List<string> verticalLines = new();
            List<string> diagLRLines = new();
            List<string> diagRLLines = new();

            foreach (string line in rawInput)
            {
                horizontalLines.Add(line);
            }

            verticalLines = FindVerticalLines(rawInput);
            diagLRLines = GetBottomToTopRightToLeftDiagonalLines(rawInput);
            diagRLLines = GetTopToBottomsLeftToRightDiagonalLines(rawInput);

            allPossibleStraightLines.Add(horizontalLines);
            allPossibleStraightLines.Add(verticalLines);
            allPossibleStraightLines.Add(diagLRLines);
            allPossibleStraightLines.Add(diagRLLines);

            FindXDashMASes(diagLRLines, diagRLLines);
        }

        static void FindXDashMASes(List<string> diagLR, List<string> diagRL)
        {

            int foundInCurrentString = 0;
            List<Vector2> indexOfAs = new();
            List<Vector2> secondIndexOfAs = new();


            foreach (string line in diagLR)
            {

                foreach (var match in Regex.Matches(line, "(MAS)|(SAM)"))
                {
                    indexOfAs.Add(new Vector2(diagLR.IndexOf(line), line.IndexOf(match.ToString()) + 1));

                }
            }

            foreach (string line in diagRL)
            {
                foreach (var match in Regex.Matches(line, "(MAS)|(SAM)"))
                {
                    secondIndexOfAs.Add(new Vector2(diagRL.IndexOf(line), line.IndexOf(match.ToString()) + 1));


                }

            }
            //diagLR and RL have differing coordinate systems, so you can't compare them directly.
            //you'll have to convert them to global or something
            //they meet in the exact center, which is why the 3x3 worked

            foreach (var i in indexOfAs)
            {
                foreach (var j in secondIndexOfAs)
                {
                    Console.WriteLine($"{i.ToString()} ?= {j.ToString()}");
                    if (i.Equals(j))
                    {
                        Console.WriteLine($"Yes");
                        foundInCurrentString++;
                    }
                    else
                        Console.WriteLine($"No");
                }
            }

            Console.WriteLine("X-MASes found: " + foundInCurrentString);

        }

        static void FindXMASes(List<List<string>> allPossibleStraightLines)
        {
            int XMASesFound = 0;

            foreach (var listOfStrings in allPossibleStraightLines)
            {
                // Console.WriteLine("list " + allPossibleStraightLines.IndexOf(listOfStrings));
                foreach (string stringInList in listOfStrings)
                {
                    int foundInCurrentString = 0;
                    foreach (var match in Regex.Matches(stringInList, "(XMAS)"))
                    {

                        foundInCurrentString++;
                    }

                    foreach (var match in Regex.Matches(stringInList, "(SAMX)"))
                    {
                        foundInCurrentString++;
                    }
                    Console.WriteLine($"Found {foundInCurrentString} in line {listOfStrings.IndexOf(stringInList)}: {stringInList}");
                    XMASesFound += foundInCurrentString;
                }
            }

            Console.WriteLine("XMASes found: " + XMASesFound);
        }

        static bool StringMatchesXMAS(string input, string detecting = "XMAS")
        {
            if (input.Contains(detecting))
            {
                return true;
            }
            else
            {
                input.Reverse();
                if (input.Contains(detecting))
                {
                    return true;
                }
                else return false;
            }
        }

        static List<string> FindVerticalLines(string[] rawInput)
        {
            List<string> verticalLines = new();
            for (int i = 0; i < rawInput.Length; i++)
            {
                string verticalLine = "";

                foreach (var line in rawInput)
                {
                    verticalLine += line[i];
                }
                verticalLines.Add(verticalLine);
            }
            return verticalLines;
        }

        static List<string> GetTopToBottomsLeftToRightDiagonalLines(string[] arrayOfStrings)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < arrayOfStrings.Length; i++)
            {
                string diagLine = "";
                int originalI = i;
                Vector2 coordinates = Vector2.Zero;
                int lRIndex = 0;
                int linesIndex = i;
                while (NextLineExists(arrayOfStrings, linesIndex))
                {
                    coordinates.X = linesIndex;
                    coordinates.Y = lRIndex;
                    if (coordinates.Y < 0) coordinates.Y = 0;

                    diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);

                    lRIndex++;
                    linesIndex++;
                }
                coordinates.X = linesIndex;
                coordinates.Y = lRIndex;
                i = originalI;
                diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);
                // (int i = arrayOfStrings.Length-1; i >=0; i--)
                result.Add(diagLine);
            }

            string anyString = arrayOfStrings[0];
            for (int i = 1; i < anyString.Length; i++)
            {
                string diagLine = "";
                int originalI = i;
                Vector2 coordinates = Vector2.Zero;
                int horIndex = i;
                int vertIndex = 0;
                while (horIndex + 1 < anyString.Length && horIndex < anyString.Length)
                {
                    coordinates.X = vertIndex;
                    coordinates.Y = horIndex;
                    if (coordinates.Y < 0) coordinates.Y = 0;

                    diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);

                    horIndex++;
                    vertIndex++;
                }
                coordinates.X = vertIndex;
                coordinates.Y = horIndex;
                i = originalI;
                diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);
                result.Add(diagLine);
            }

            return result;
        }

        static List<string> GetBottomToTopRightToLeftDiagonalLines(string[] arrayOfStrings)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < arrayOfStrings.Length; i++)
            {
                string diagLine = "";
                int originalI = i;
                Vector2 coordinates = Vector2.Zero;
                int linesIndex = i;
                int lRIndex = arrayOfStrings[linesIndex].Length - 1;

                while (NextLineExists(arrayOfStrings, linesIndex))
                {
                    coordinates.X = linesIndex;
                    coordinates.Y = lRIndex;
                    if (coordinates.Y < 0) coordinates.Y = 0;

                    diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);

                    lRIndex--;
                    linesIndex++;
                }
                coordinates.X = linesIndex;
                coordinates.Y = lRIndex;
                i = originalI;
                diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);
                result.Add(diagLine);
            }

            string anyString = arrayOfStrings[0];
            for (int i = anyString.Length - 2; i >= 0; i--)
            {
                string diagLine = "";
                int originalI = i;
                Vector2 coordinates = Vector2.Zero;
                int horIndex = i;
                int vertIndex = 0;
                while (horIndex - 1 >= 0 && horIndex < anyString.Length && NextLineExists(arrayOfStrings, vertIndex))
                {
                    coordinates.X = vertIndex;
                    coordinates.Y = horIndex;
                    if (coordinates.Y < 0) coordinates.Y = 0;

                    diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);

                    horIndex--;
                    vertIndex++;
                }
                coordinates.X = vertIndex;
                coordinates.Y = horIndex;
                i = originalI;
                diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);
                result.Add(diagLine);
            }

            return result;
        }

        static char GetItemAtCoordinates(string[] arrayOfStrings, Vector2 coords)
        {
            return arrayOfStrings[(int)coords.X][(int)coords.Y];
        }

        static bool NextLineExists(string[] listToCheck, int currentIndex)
        {
            return currentIndex + 1 < listToCheck.Length;
        }

    }
}