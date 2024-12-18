using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Tools
{
    public  class Grid<T>
    {
        public GridSpace<T>[,] Data { get; protected set; }
        public Grid() { Data = new GridSpace<T>[0,0]; }
        public Grid(int X, int Y)
        {
            Data = new GridSpace<T>[X,Y];
            for (int x =0;  x < X; x++)
            {
                for (int y =0; y < Y; y++)
                {
                    Data[x,y] = new GridSpace<T>(default(T), x,y);
                }
            }
            //GridData = (GridSpace<T>)gridData;
        }
        
        public Grid(Vector2 dimensions)
        {
            Data = new GridSpace<T>[dimensions.X, dimensions.Y];
            for (int x =0;  x < dimensions.X; x++)
            {
                for (int y =0; y < dimensions.Y; y++)
                {
                    Data[x,y] = new GridSpace<T>(default(T), x,y);
                }
            }
            //GridData = (GridSpace<T>)gridData;
        }

        /// <summary>
        /// Returns the GridSpace located at <paramref name="sourcePosition"/> + <paramref name="direction"/> if it exists. Else returns null.
        /// </summary>
        /// <param name="sourcePosition"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public GridSpace<T>? GetItemRelative(Vector2 sourcePosition, Vector2 direction)
        {
            try
            {

                Vector2 targetCoordinates = sourcePosition + direction;
                return Data[targetCoordinates.X, targetCoordinates.Y];
            }
            catch(Exception e)
            {
                ConsoleTools.WriteRedLine(e.Message);
                return null;
            }
        }


        /// <summary>
        /// Replaces the GridSpace at <paramref name="targetPosition"/> with a copy of <paramref name="sourcePosition"/>, and replaces the original with the default value.
        /// </summary>
        /// <param name="sourcePosition"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public virtual bool MoveItemToSpaceAtCoordinates(Vector2 sourcePosition, Vector2 targetPosition, int itemIndex, bool sendMessage = true)
        {
            try
            {
                T item = Data[sourcePosition.X, sourcePosition.Y].ItemsInSpace[itemIndex];

                Data[targetPosition.X, targetPosition.Y].AddItemToSpace(item);//.ItemsInSpace.Add(item);
                Data[sourcePosition.X, sourcePosition.Y].RemoveItemFromSpace(item) ;


                return true;
            }
            catch (Exception e)
            {
                if(sendMessage)
                ConsoleTools.WriteRedLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Returns the space located at <paramref name="position"/> if it exists. Else returns null.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public GridSpace<T>? GetSpaceAtCoordinates(Vector2 position)
        {
            try
            {
                return Data[position.X,position.Y];

            }
            catch( Exception e)
            {
                ConsoleTools.WriteRedLine(e.Message);
                return null;
            }
        }
        
        public GridSpace<T>? GetSpaceAtCoordinates(int X, int Y)
        {
            try
            {
                return Data[X,Y];

            }
            catch( Exception e)
            {
                ConsoleTools.WriteRedLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Sets the GridSpace.Value located at <paramref name="position"/> to <paramref name="what"/> and returns if it was successful.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="what"></param>
        /// <returns></returns>
        public bool SetItemAtPosition(Vector2 position, T what)
        {
            try
            {
                Data[position.X, position.Y].AddItemToSpace(what);
                return true;
            }
            catch (Exception e)
            {
                ConsoleTools.WriteRedLine(e.Message);
                return false;
            }
        }

        public virtual string Print(bool print = true)
        {
            string output = "";
            for (int y = 0; y < Data.GetLength(1); y++)
            {
                for (int x = 0; x < Data.GetLength(0); x++)
                {
                    output += Data[x, y].gridIdentifier;
                }
                output += "\n";
            }
            if(print )
            {
                Console.Write(output);
            }
            return output;
        }

        /// <summary>
        /// Returns every item of type T in the grid. Unless T has custom coordinates you will not be able to use that data.
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetEverySingleItemContainedInAllGridSpaces()
        {
            List<T> values = new List<T>();

            foreach(var space in Data)
            {
                foreach(var item in space.ItemsInSpace)
                {
                    values.Add(item);
                }
            }

            return values;
        }

    }

    public class GridSpace<T> 
    {
        public List<T> ItemsInSpace = new();
        public Vector2 Coordinates;
        public string gridIdentifier { get; set; } = ".";
        public const string defaultGridIdentifier = ".";

        public GridSpace(T? value, int x, int y)
        {
            if(value != null)
            {
                ItemsInSpace.Add(value);
            }
            Coordinates = new(x, y);
        }
        
        public GridSpace(T? value, Vector2 coordinates)
        {

            if (value != null)
            {
                ItemsInSpace.Add(value);

            }
            Coordinates = coordinates;
        }

        public virtual void AddItemToSpace(T? what)
        {

            if (what != null)
                ItemsInSpace.Add(what);

        }public virtual void RemoveItemFromSpace(T? what)
        {

            if (what != null)
                ItemsInSpace.Remove(what);

        }

        public override string ToString()
        {
           return gridIdentifier.ToString();// string.Empty;

        }
    }

}
