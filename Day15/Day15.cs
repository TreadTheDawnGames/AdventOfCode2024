using AdventOfCode.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode.IWarehouseItem;

namespace AdventOfCode
{
    internal class Day15
    {

        public static void DoWork(string[] rawInput)
        {
            int splitIndex = rawInput.ToList().IndexOf("");
            string[] warehouseString = rawInput.Take(splitIndex).ToArray();

            var warehouse = CreateWarehouse(warehouseString);

            var doubleWarehouse = CreateDoubleWideWarehouse(warehouse);

            Robot r = doubleWarehouse.GetFirstItemOfTypeFromWarehouse<Robot>();
            var instructions = ParseDirections(rawInput.Skip(splitIndex).ToArray());
            Console.SetCursorPosition(0, 0);
                int i = 0;
            foreach (var instruction in instructions)
            {
                r.AttemptToMove(instruction);
                Console.SetCursorPosition(0, 0);
                doubleWarehouse.Print();
                //Console.WriteLine(r.Position.ToString());
               // Console.WriteLine(instruction);
              // Thread.Sleep(10);
               // Console.WriteLine(i++ +"/"+instructions.Length);
            }

           // GetSum(warehouse);
            GetDoubleSum(doubleWarehouse);
        }

        static void GetSum(WarehouseGrid warehouse)
        {
            int gpsSum = 0;

            foreach (var item in warehouse.GetEverySingleItemContainedInAllGridSpaces())
            {
                if (item is Day15MovableGridItem && item is not Robot)
                {
                    int gpsCoord = (item.Position.Y * 100 + item.Position.X);
                    gpsSum += gpsCoord;
                }
            }

            Console.WriteLine("Sum: " + gpsSum);
        }
        
        static void GetDoubleSum(WarehouseGrid warehouse)
        {
            int gpsSum = 0;
            var allItems = warehouse.GetEverySingleItemContainedInAllGridSpaces();
            foreach (var item in allItems)
            {
                if (item is LargeBoxHalf)
                {
                    var half = (LargeBoxHalf)item;
                    if (!half.IsRight)
                    {
                        int gpsCoord = (item.Position.Y * 100) + item.Position.X;
                        gpsSum += gpsCoord;
                    }
                }
            }

            if(gpsSum< 1527926)
            {
                ConsoleTools.WriteGreenLine("Double Sum: " + gpsSum);

            }else
                ConsoleTools.WriteRedLine("Double Sum: " + gpsSum);
        }

       

        static WarehouseGrid.PushDirection[] ParseDirections(string[] input)
        {
            List<WarehouseGrid.PushDirection> instructions = new();
            foreach (string s in input)
            {
                foreach (char c in s)
                {

                    switch (c)
                    {
                        case '^':
                            instructions.Add(WarehouseGrid.PushDirection.Up);
                            break;
                        case 'v':
                            instructions.Add(WarehouseGrid.PushDirection.Down);
                            break;
                        case '<':
                            instructions.Add(WarehouseGrid.PushDirection.Left);
                            break;
                        case '>':
                            instructions.Add(WarehouseGrid.PushDirection.Right);
                            break;
                        default:
                            break;
                    }
                }
                //Console.WriteLine(s);
            }
            return instructions.ToArray();
        }

        public static WarehouseGrid CreateWarehouse(string[] input)
        {
            WarehouseGrid warehouse = new(new(input[0].Length, input.Length));

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    switch (input[y][x])
                    {
                        case 'O':
                            warehouse.Data[x,y].AddItemToSpace(new Day15MovableGridItem(new(x,y), warehouse));
                            break;
                        case '#':
                            warehouse.Data[x,y].AddItemToSpace(new Wall(new(x,y), warehouse));
                            break;
                        case '@':
                            warehouse.Data[x,y].AddItemToSpace(new Robot(new(x,y), warehouse));
                            break;
                        case '.':
                            warehouse.Data[x,y].AddItemToSpace(new Empty(new(x,y), warehouse));
                            break;
                        case '[':
                            var left = new LargeBoxHalf(new(x, y), warehouse, false, null);
                            warehouse.Data[x, y].AddItemToSpace(left);

                            var right = new LargeBoxHalf(new(++x, y), warehouse, true, left);
                            warehouse.Data[x, y].AddItemToSpace(right);

                            left.pair = right;
                            break;
                        default:
                         //   ConsoleTools.WriteRedLine("Default case found while creating warehouse");
                            break;
                    }
                }
            }


            return warehouse;




            //WarehouseGrid grid = new WarehouseGrid();
        }

        public static WarehouseGrid CreateDoubleWideWarehouse(WarehouseGrid warehouse)
        {
            string blueprint = warehouse.Print(false);
            blueprint = blueprint.Remove(warehouse.Print(false).Length-1);
            string[] doubleWideBlueprint = blueprint.Split("\n");
            

            for(int i = 0; i< doubleWideBlueprint.Length; i++)
            {
            var builder = new StringBuilder();
                foreach( char c in doubleWideBlueprint[i])
                {
                    if (c != 'O') 
                        builder.Append(c);
    
                    switch(c)
                    {
                        case '#':
                            builder.Append('#');
                            break;
                        case 'O':
                            builder.Append('[');
                            builder.Append(']');
                            break;
                        case '.':
                            builder.Append('.');
                            break;
                        case '@':
                            builder.Append('.');
                            break;
                        case '\n':
                            break;
                        default:
                            break;
                    }
                }
                doubleWideBlueprint[i] = builder.ToString();
                //line = line.Replace("#", "##").Replace("O","[]").Replace(".","..").Replace("@","@.");
            }

            return CreateWarehouse(doubleWideBlueprint);
        }


    }

    public class WarehouseGrid : Grid<IWarehouseItem> 
    {
        public enum PushDirection { Left, Up, Right, Down }
       
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
                    Data[sourcePosition.X, sourcePosition.Y].AddItemToSpace(new Empty(sourcePosition, this));
                }

                WarehouseSpace sourceSpace = (WarehouseSpace)GetSpaceAtCoordinates(sourcePosition);
                WarehouseSpace targetSpace = (WarehouseSpace)GetSpaceAtCoordinates(targetPosition);

                sourceSpace?.SetGridIdentifier();
                targetSpace?.SetGridIdentifier();


                var shouldBeEmpty = GetSpaceAtCoordinates(sourcePosition).ItemsInSpace[0];
                var shouldBeSource = GetSpaceAtCoordinates(targetPosition).ItemsInSpace[0];
                if (shouldBeEmpty is Empty && shouldBeSource == source)
                {
                    return true;

                }

            }
            Data[sourcePosition.X, sourcePosition.Y].AddItemToSpace(source);
            ConsoleTools.WriteRedLine("Failed to move item");

                return false;
        }

        public override string Print(bool print = true)
        {
            for (int x = 0; x < Data.GetLength(0); x++)
                for (int y = 0; y < Data.GetLength(1); y++)
                {
                    WarehouseSpace ws = (WarehouseSpace)GetSpaceAtCoordinates(x, y);
                    ws?.SetGridIdentifier();
                }

            return base.Print(print);
        }

        public IWarehouseItem GetFirstItemOfTypeFromWarehouse<IWarehouseItem>()
        {
            foreach (var item in Data)
            {
                if (item.ItemsInSpace.Count > 0)
                    if (item.ItemsInSpace[0] is IWarehouseItem)
                    {
                        return (IWarehouseItem)item.ItemsInSpace[0];
                    }
            }
            return default(IWarehouseItem);
        }
        public WarehouseGrid(Vector2 dimensions) : base(dimensions)
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
    }

    public class WarehouseSpace : GridSpace<IWarehouseItem>
    {

        public WarehouseSpace(IWarehouseItem? value, int x, int y) : base(value, x, y)
        {
            if (value != null)
                ItemsInSpace.Add(value);
            Coordinates = new(x, y);
        
        }

        public WarehouseSpace(IWarehouseItem? value, Vector2 coordinates, IWarehouseItem.ItemType type) : base(value, coordinates)
        {

            if (value != null)
                ItemsInSpace.Add(value);
            Coordinates = coordinates;
            
        }
        public void SetGridIdentifier()
        {
            if(ItemsInSpace.Count == 0) return;
            switch (ItemsInSpace[0].type)
            {
                case IWarehouseItem.ItemType.Box:
                    gridIdentifier = "O";
                    break;
                case IWarehouseItem.ItemType.Wall:
                    gridIdentifier = "#";
                    break;
                case IWarehouseItem.ItemType.Robot:
                    gridIdentifier = "@";
                    break;
                case IWarehouseItem.ItemType.Empty:
                    gridIdentifier = ".";
                    break;
                case ItemType.LargeBoxL:
                    gridIdentifier = "[";
                    break;
                case ItemType.LargeBoxR:
                    gridIdentifier = "]";
                    break;

                case IWarehouseItem.ItemType.Unknown:
                    gridIdentifier = "X";
                    break;
            }
        }

        public override void AddItemToSpace(IWarehouseItem? what)
        {
            if (what != null)
            {

                base.AddItemToSpace(what);
                what.Position = Coordinates;
                SetGridIdentifier();
            }
        }


        
    }

    public interface IWarehouseItem 
    {
        public enum ItemType { Box, Wall, Robot, Empty, LargeBoxL, LargeBoxR, Unknown}
        public Vector2 Position { get; set; }
        public ItemType type { get; set; }
        public WarehouseGrid Warehouse { get; set; }

        public bool AttemptToMove(WarehouseGrid.PushDirection direction);
    }

    public class Robot : Day15MovableGridItem {
        public Robot(Vector2 position, WarehouseGrid warehouse) : base(position, warehouse)
        {
            Position = position;
            this.type = ItemType.Robot;
            Warehouse = warehouse;
        }

        public void FollowInstructions(WarehouseGrid.PushDirection[] instructions)
        {
            foreach(var i in instructions)
            {
                AttemptToMove(i);

            }
        }
    }
    public class Empty : IWarehouseItem
    {
        public Vector2 Position { get; set; }
        public ItemType type { get; set; }
        public WarehouseGrid Warehouse { get; set; }
        public Empty(Vector2 position, WarehouseGrid warehouse)
        {
            Position = position;
            this.type = ItemType.Empty;
            Warehouse = warehouse;
        }
        public bool AttemptToMove(WarehouseGrid.PushDirection direction)
        {
            return true;
        }
    }
    public class Day15MovableGridItem : IWarehouseItem
    {
        public Vector2 Position { get; set; }
        public ItemType type { get; set; }
        public WarehouseGrid Warehouse { get; set; }

        public Day15MovableGridItem(Vector2 position,  WarehouseGrid warehouse) 
        {
            Position = position;
            this.type = ItemType.Box;
            Warehouse = warehouse;
        }


        public virtual bool AttemptToMove(WarehouseGrid.PushDirection direction)
        {
                Vector2 directionVec;
                switch (direction)
                {
                    case WarehouseGrid.PushDirection.Left:
                        directionVec = new(-1, 0);
                        break;

                    case WarehouseGrid.PushDirection.Right:
                        directionVec = new(1, 0);
                        break;

                    case WarehouseGrid.PushDirection.Up:
                        directionVec = new(0, -1);
                        break;

                    case WarehouseGrid.PushDirection.Down:
                        directionVec = new(0, 1);
                        break;

                    default:
                        directionVec = new(0,0);
                        break;
                }

                var itemRelative = Warehouse.GetItemRelative(Position, directionVec);

                if(itemRelative==null) throw new GridItemDoesNotExistException();

                if (itemRelative.ItemsInSpace[0].AttemptToMove(direction))
                {
                    return Warehouse.MoveItemToSpaceAtCoordinates(Position, Position + directionVec, 0);

                }
                else return false;
            
            
        }
    }

    class GridItemDoesNotExistException : Exception
    {

    }

    public class Wall : IWarehouseItem
    {
        public Vector2 Position { get; set; }
        public ItemType type { get; set; }
        public WarehouseGrid Warehouse { get; set; }

        public Wall(Vector2 position,  WarehouseGrid warehouse) 
        {
            Position = position;
            this.type = ItemType.Wall;
            Warehouse = warehouse;
        }

        public bool AttemptToMove(WarehouseGrid.PushDirection direction)
        {
            return false;
        }


    }

    public class LargeBoxHalf : Day15MovableGridItem
    {
        public LargeBoxHalf(Vector2 position, WarehouseGrid warehouse, bool isRight, LargeBoxHalf pair) : base(position, warehouse)
        {
            Position = position;
            this.type = isRight ? ItemType.LargeBoxR : ItemType.LargeBoxL;
            Warehouse = warehouse;
            this.pair = pair;
            IsRight = isRight;
        }

        public bool IsRight { get; private set; } = false;
        public LargeBoxHalf pair;

        bool CanMove(WarehouseGrid.PushDirection direction, bool checkPair = true)
        {
            bool pairCanMove = true;
            if (checkPair)
            {
                pairCanMove = pair.CanMove(direction, false);
            }
         /*

            if (pairCanMove)
            {
*/

                Vector2 directionVec;
                switch (direction)
                {
                    case WarehouseGrid.PushDirection.Left:
                        directionVec = new(-1, 0);
                        break;

                    case WarehouseGrid.PushDirection.Right:
                        directionVec = new(1, 0);
                        break;

                    case WarehouseGrid.PushDirection.Up:
                        directionVec = new(0, -1);
                        break;

                    case WarehouseGrid.PushDirection.Down:
                        directionVec = new(0, 1);
                        break;

                    default:
                        directionVec = new(0, 0);
                        break;
                }

            /*    var adjacentThing = Warehouse.GetItemRelative(directionVec, directionVec).ItemsInSpace[0];

                if (adjacentThing != null)
                {
                    if (adjacentThing is LargeBoxHalf)
                    {
                        LargeBoxHalf foundBox = (LargeBoxHalf)adjacentThing;
                        return foundBox.CanMove(direction);
                    }
                }
                if (pairCanMove)
                {

                }
            }*/
            var itemInSpace = Warehouse.GetItemRelative(Position, directionVec).ItemsInSpace[0];
            if (itemInSpace is LargeBoxHalf)
            {
                LargeBoxHalf item = (LargeBoxHalf)itemInSpace;
                return item.CanMove(direction) && pairCanMove;
            }
            else
            {
                return Warehouse.GetItemRelative(Position, directionVec).ItemsInSpace[0] is not Wall && pairCanMove;
            }

        }

        public override bool AttemptToMove(WarehouseGrid.PushDirection direction)
        {

            
            switch (direction)
            {
                case WarehouseGrid.PushDirection.Left:
                    return base.AttemptToMove(direction);


                case WarehouseGrid.PushDirection.Right:
                    return base.AttemptToMove(direction);


                case WarehouseGrid.PushDirection.Up:
                    if (CanMove(direction))
                    {
                        return base.AttemptToMove(direction) && pair.PairMove(direction);

                    }
                    else return false;

                case WarehouseGrid.PushDirection.Down:
                    if (CanMove(direction))
                    {
                        return base.AttemptToMove(direction) && pair.PairMove(direction);

                    }
                    else return false;

                default:
                    return false;
            }
            
        }

        bool PairMove(WarehouseGrid.PushDirection direction)
        {
            return base.AttemptToMove(direction);
        }

    }

}
