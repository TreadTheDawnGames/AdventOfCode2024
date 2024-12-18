using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Tools
{
    public struct Vector2
    {
        public int X, Y;
        public Vector2 (int x, int y)
        {
            X=x; Y = y;
        }

        public static Vector2 Zero
        {
            get => default;
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2
            {
                X = a.X + b.X,
                Y = a.Y + b.Y
            };
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2
            {
                X = a.X - b.X,
                Y = a.Y - b.Y
            };
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
    internal class Structs
    {
    }
}
