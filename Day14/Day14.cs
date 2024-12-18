using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Tools;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode
{
    internal class Day14
    {

        public static void DoWork(string[] rawInput)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            GuardGrid<Day14Guard> grid = ParseRawInput(rawInput, new Vector2(101, 103));

            SetGridStateForCounting(grid);

            int TL=0;
                int TR=0;
                int BL=0;
                int BR=0;

            foreach (var space in grid.Data)
            {
                if (space.Coordinates.X < grid.Data.GetLength(0) / 2 && space.Coordinates.Y < grid.Data.GetLength(1) / 2)
                {
                    TL += space.ItemsInSpace.Count;
                }
                else if (space.Coordinates.X < grid.Data.GetLength(0) / 2 && space.Coordinates.Y > grid.Data.GetLength(1) / 2)
                {
                    TR += space.ItemsInSpace.Count;

                }
                else if (space.Coordinates.X > grid.Data.GetLength(0) / 2 && space.Coordinates.Y < grid.Data.GetLength(1) / 2)
                {
                    BL += space.ItemsInSpace.Count;

                }
                else if (space.Coordinates.X > grid.Data.GetLength(0) / 2 && space.Coordinates.Y > grid.Data.GetLength(1) / 2)
                {
                    BR += space.ItemsInSpace.Count;

                }
            }


            Console.WriteLine($"Output: {TL * TR * BL * BR}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void SetGridStateForCounting(GuardGrid<Day14Guard> grid)
        {
            //SetGridIdentifierAndPrint(grid);
            int counter = 0;
            int iteration = 0;

           

           while (iteration >=0)// 100)
            {
                Console.WriteLine(iteration++);
                counter += 1;
                for (int x = 0; x < grid.Data.GetLength(0); x++)// var gridSpace in grid.Data)
                {
                    for (int y = 0; y < grid.Data.GetLength(1); y++)
                    {
                        GridSpace<Day14Guard> gridSpace = grid.Data[x, y];
                        // var gridSpace in grid.Data)
                        if (gridSpace.ItemsInSpace.Count == 0) continue;
                        for (int j = gridSpace.ItemsInSpace.Count; j >= 0; j--)// var guard in gridSpace.ItemsInSpace)
                        {
                            if (j > gridSpace.ItemsInSpace.Count - 1) continue;
                            Day14Guard guard = gridSpace.ItemsInSpace[j];
                            if (!guard.MovedThisPhase)
                            {

                                if (grid.MoveItemToSpaceAtCoordinates(guard.Position, guard.Position + guard.Velocity, gridSpace.ItemsInSpace.IndexOf(guard)))
                                {

                                    var fixedPos = grid.FixCoordinates(guard.Position + guard.Velocity);
                                    guard.Position = fixedPos;
                                    //Console.WriteLine(guard.Position.ToString());

                                    guard.MovedThisPhase = true;
                                    //x = y = 0;
                                    //ConsoleTools.WriteGreenLine("Good");




                                }
                                else
                                {
                                    ConsoleTools.WriteRedLine("Bad");
                                }
                            }
                            else
                            {
                                // Console.WriteLine("This item has been moved");
                                // Console.WriteLine("This item has been moved");
                            }

                        }
                    }
                }
                //Console.WriteLine("Moved " + counter + " items");
                foreach (var gridSpace in grid.Data)
                {
                    //if (gridSpace.ItemsInSpace.Count > 0)
                    foreach (var guard in gridSpace.ItemsInSpace)
                    {
                        guard.MovedThisPhase = false;
                        //ConsoleTools.WriteColoredLine(guard.Position.ToString(), ConsoleColor.Magenta);

                        //ConsoleTools.WriteGreenLine("Good");
                    }
                }
                //Console.WriteLine();
                Console.SetCursorPosition(0, 0);
                SetGridIdentifierAndPrint(grid);
                Console.WriteLine("Seconds: " + counter);
            }
        }
       
        private static void SetGridIdentifierAndPrint(GuardGrid<Day14Guard> grid)
        {
            foreach (var gridSpace in grid.Data)
            {
                if (gridSpace.ItemsInSpace.Count > 0)
                {

                    gridSpace.gridIdentifier = "█";
                    //gridSpace.ItemsInSpace.Count.ToString();
                }
                else
                {
                    gridSpace.gridIdentifier = " ";
                }
            }
            grid.Print();
        }

        //parser
        static GuardGrid<Day14Guard> ParseRawInput(string[] rawInput, Vector2 gridSize)
        {
            GuardGrid<Day14Guard> grid = new(gridSize);
            
            foreach (string input in rawInput)
            {
                string workingInput = input.Replace('v', '|');
                Regex v = new("\\|=-?[0-9]{1,3},-?[0-9]{1,3}");
                Regex p = new("p=[0-9]{1,3},[0-9]{1,3}");

                string commadNums = Regex.Replace(workingInput, "[^-?[0-9,|]", "");
                var both = commadNums.Split("|");

                var position = both[0].Split(",");
                Vector2 positionVec = new Vector2(int.Parse(position[0]), int.Parse(position[1]));
                var velocity = both[1].Split(",");
                Vector2 velocityVec = new Vector2(int.Parse(velocity[0]), int.Parse(velocity[1]));

                Day14Guard guard = new Day14Guard(positionVec, velocityVec);
                grid.SetItemAtPosition(positionVec, guard);
            }

            return grid;
        }


    }

    class Day14Guard
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity;
        public bool MovedThisPhase = false;
        public Day14Guard(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }

    internal class GuardGrid<T> : Grid<T>
    {
        public GuardGrid(Vector2 dimensions)
        {
                Data = new GridSpace<T>[dimensions.X, dimensions.Y];
                for (int x = 0; x < dimensions.X; x++)
                {
                    for (int y = 0; y < dimensions.Y; y++)
                    {
                        Data[x, y] = new GridSpace<T>(default(T), x, y);
                    }
                }
                //GridData = (GridSpace<T>)gridData;
        }

        public Vector2 FixCoordinates(Vector2 putItemAtHere)
        {
            if (putItemAtHere.X >= Data.GetLength(0))
            {
                while(putItemAtHere.X >= Data.GetLength(0))
                    putItemAtHere.X -= Data.GetLength(0) ;
            }
            else if (putItemAtHere.X < 0)
            {
                while (putItemAtHere.X < 0)
                    putItemAtHere.X += Data.GetLength(0) ;

            }


            if (putItemAtHere.Y >= Data.GetLength(1))
            {
                while (putItemAtHere.Y >= Data.GetLength(1))
                    putItemAtHere.Y -= Data.GetLength(1);

            }
            else if (putItemAtHere.Y < 0)
            {
                while (putItemAtHere.Y < 0)
                    putItemAtHere.Y += Data.GetLength(1);
            }
            return putItemAtHere;
        }

        public override bool MoveItemToSpaceAtCoordinates(Vector2 sourcePosition, Vector2 putItemAtHere, int itemIndex, bool sendMessage = true)
        {
          
            try
            {

                if (base.MoveItemToSpaceAtCoordinates(sourcePosition, putItemAtHere, itemIndex, false))
                    return true;
                else
                {
                    
                    putItemAtHere = FixCoordinates(putItemAtHere);

                   if(base.MoveItemToSpaceAtCoordinates(sourcePosition, putItemAtHere, itemIndex))
                        return true;
                    else
                    {

                        return false;
                    }
                    //return true;
                }

            }
            catch (Exception e)
            {
                
                
                    if(sendMessage) 
                    ConsoleTools.WriteRedLine(e.Message);
                    return false;
                
            }
            
        }
    }
}
