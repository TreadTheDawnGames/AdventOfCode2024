using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Tools;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static AdventOfCode.IWarehouseItem;

namespace AdventOfCode
{
    internal class Day6
    {

        public static void DoWork(string[] rawInput)
        {
            var lab = CreateLabGrid(rawInput);
            //lab.Print();
            Day6Guard guard = lab.GetFirstItemOfTypeFromWarehouse<Day6Guard>();

            guard.WalkUntilWallThenTurn();

            Console.WriteLine("Visited Spaces: " + (lab.GetVisitedSpaces().Count+1));
        }

        

        public static LabGrid CreateLabGrid(string[] input)
        {
            LabGrid warehouse = new(new(input[0].Length, input.Length));

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    switch (input[y][x])
                    {
                        case '#':
                            warehouse.Data[x, y].AddItemToSpace(new Wall(new(x, y), warehouse));
                            break;
                        case '.':
                            warehouse.Data[x, y].AddItemToSpace(new LabEmpty(new(x, y), warehouse, false));
                            break;
                        case '^':
                            warehouse.Data[x,y].AddItemToSpace(new Day6Guard(new(x, y), warehouse));
                            break;
                        default:
                            //   ConsoleTools.WriteRedLine("Default case found while creating warehouse");
                            break;
                    }
                }
            }

            return warehouse;

        }

        public class LabEmpty : IWarehouseItem 
        {
            public Vector2 Position { get; set; }
            public ItemType type { get; set; }
            public WarehouseGrid Warehouse { get; set; }

            public bool Visited { get; set; }
            public LabEmpty(Vector2 position, WarehouseGrid warehouse, bool visited)
            {
                Position = position;
                this.type = ItemType.Empty;
                Warehouse = warehouse;
                Visited = visited;

            }
            public LabEmpty(int x, int y, WarehouseGrid warehouse, bool visited)
            {
                Position = new(x, y);
                this.type = ItemType.Empty;
                Warehouse = warehouse;
                Visited = visited;
            }
            public bool AttemptToMove(WarehouseGrid.PushDirection direction)
            {
                return true;
            }
        }

        public class LabGrid : WarehouseGrid
        {
            public LabGrid(Vector2 dimensions) : base(dimensions)
            {
                Data = new GridSpace<IWarehouseItem>[dimensions.X, dimensions.Y];
                for (int x = 0; x < dimensions.X; x++)
                {
                    for (int y = 0; y < dimensions.Y; y++)
                    {
                        Data[x, y] = new WarehouseSpace(default(IWarehouseItem), x, y);
                    }
                }
            }
            public virtual List<IWarehouseItem> GetEverySingleItemContainedInAllGridSpaces()
            {
                List<IWarehouseItem> values = new List<IWarehouseItem>();

                foreach (var space in Data)
                {
                    foreach (var item in space.ItemsInSpace)
                    {
                        values.Add(item);
                    }
                }

                return values;
            }
            public List<IWarehouseItem> GetVisitedSpaces()
            {
                List<IWarehouseItem> visitedEmpties = new List<IWarehouseItem>();

                foreach(var item in GetEverySingleItemContainedInAllGridSpaces())
                {
                    if(item is Empty)
                    {
                        /*var labEmpty = (LabEmpty)item;
                        if (labEmpty.Visited)
                        {*/
                            visitedEmpties.Add(item);
                        //}
                    }
                }

                return visitedEmpties;
            }

            public override bool MoveItemToSpaceAtCoordinates(Vector2 sourcePosition, Vector2 targetPosition, int itemIndex, bool sendMessage = true)
            {
                var source = GetSpaceAtCoordinates(sourcePosition).ItemsInSpace[0];
                var target = GetSpaceAtCoordinates(targetPosition).ItemsInSpace[0];
                if (base.MoveItemToSpaceAtCoordinates(sourcePosition, targetPosition, itemIndex, sendMessage))
                {

                    source.Position = targetPosition;

                    if (Data[targetPosition.X, targetPosition.Y].ItemsInSpace.Count > 1)
                    {
                        Data[targetPosition.X, targetPosition.Y].ItemsInSpace.Remove(Data[targetPosition.X, targetPosition.Y].ItemsInSpace[0]);
                    }

                    if (Data[sourcePosition.X, sourcePosition.Y].ItemsInSpace.Count == 0)
                    {

                        //   Data[sourcePosition.X, sourcePosition.Y].ItemsInSpace.Clear();
                        Data[sourcePosition.X, sourcePosition.Y].AddItemToSpace(new LabEmpty(sourcePosition, this, true));
                    }

                    WarehouseSpace sourceSpace = (WarehouseSpace)GetSpaceAtCoordinates(sourcePosition);
                    WarehouseSpace targetSpace = (WarehouseSpace)GetSpaceAtCoordinates(targetPosition);

                    sourceSpace?.SetGridIdentifier();
                    targetSpace?.SetGridIdentifier();


                    var shouldBeEmpty = GetSpaceAtCoordinates(sourcePosition).ItemsInSpace[0];
                    var shouldBeSource = GetSpaceAtCoordinates(targetPosition).ItemsInSpace[0];
                    if (shouldBeEmpty is Empty&& shouldBeSource == source)
                    {
                        return true;

                    }

                }
                Data[sourcePosition.X, sourcePosition.Y].AddItemToSpace(source);
                ConsoleTools.WriteRedLine("Failed to move item");

                return false;
            }
        }
        

       

        class Day6Guard : Day15MovableGridItem
        {

            WarehouseGrid.PushDirection facing;
            WarehouseGrid.PushDirection Facing { 
                get 
                {
                    return (WarehouseGrid.PushDirection)((int)facing % 4); 
                } set { facing = value; } }// = WarehouseGrid.PushDirection.Up;
            
            public Day6Guard(Vector2 position, WarehouseGrid warehouse) : base(position, warehouse) 
            {
                Facing = WarehouseGrid.PushDirection.Up;
                Position = position;
                Warehouse = warehouse;
            }

            public void WalkUntilWallThenTurn()
            {
                while (true)
                {

                    try
                    {
                        /*Console.SetCursorPosition(0, 0);
                        Warehouse.Print();*/
                        
                        if (!AttemptToMove(Facing))
                        {
                            Facing++;
                        }
                        

                    }
                    catch (GridItemDoesNotExistException ex)
                    {
                        Console.WriteLine(ex.Message);
                        break; 
                    }
                }
            }

            
        }
    }
}
