using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
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

            foreach (var listOfStrings in allPossibleStraightLines)
            {
                Console.WriteLine("list " + allPossibleStraightLines.IndexOf(listOfStrings));
                foreach (var stringInList in listOfStrings)
                {

                    Console.WriteLine(stringInList);

                }
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

                    var stringLine = arrayOfStrings[linesIndex];

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
                int horIndex =  i;
                int vertIndex = 0;
                while (NextLineExists(arrayOfStrings, horIndex))
                {
                    coordinates.X = vertIndex;
                    coordinates.Y = horIndex;
                    if (coordinates.Y < 0) coordinates.Y = 0;


                    diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);
                    //Console.WriteLine(diagLine);

                    horIndex++;
                    vertIndex++;
                }
                coordinates.X = vertIndex;
                coordinates.Y = horIndex;
                i = originalI;
                diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);
                // (int i = arrayOfStrings.Length-1; i >=0; i--)
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
                int lRIndex = arrayOfStrings[linesIndex].Length-1;

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
                while (NextLineExists(arrayOfStrings, horIndex))
                {
                    coordinates.X = vertIndex;
                    coordinates.Y = horIndex;
                    if (coordinates.Y < 0) coordinates.Y = 0;


                    diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);
                    //Console.WriteLine(diagLine);

                    horIndex++;
                    vertIndex++;
                }
                coordinates.X = vertIndex;
                coordinates.Y = horIndex;
                i = originalI;
                diagLine += GetItemAtCoordinates(arrayOfStrings, coordinates);
                // (int i = arrayOfStrings.Length-1; i >=0; i--)
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