using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Vectors
{
    internal struct Vector // struct - базовий тип для Value Type
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string? ToString()
        {
            return $"({X:F4};{Y:F4})";
        }

        #region Operators
        public static Vector operator +(Vector a) => new()
        {
            X = a.X,
            Y = a.Y
        };
        public static Vector operator -(Vector a) => new()
        {
            X = -a.X,
            Y = -a.Y
        };
        public static bool operator true(Vector a) => a.X != 0.0 || a.Y != 0.0;
        public static bool operator false(Vector a) => a.X == 0.0 && a.Y == 0.0;

        public static bool operator !(Vector a) => a.X == 0.0 && a.Y == 0.0;
        /// <summary>
        /// Mirrowing Vector in Y line
        /// </summary>
        /// <param name="a">Original Vector</param>
        /// <returns>Reflected Vector</returns>
        public static Vector operator ~(Vector a) => new() { X = -a.X, Y = a.Y };

        public static Vector operator ++(Vector a) => new() { X = a.X + 1, Y = a.Y + 1 };
        public static Vector operator --(Vector a) => new() { X = a.X - 1, Y = a.Y - 1 };

        //нові (від C# 14) скорочені оператори: означається для об'єктів (не static)
        //public void operator +=(Vector a) => (X, Y) = (X + a.X, Y + a.Y);

        public double this[int index]
        {
            //expression - з поверненням значення
            get => index switch { 0 => X, 1 => Y, _ => throw new IndexOutOfRangeException("0 or 1 only") };
            set
            {
                switch (index) //statement - без повернення значення
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    default: throw new IndexOutOfRangeException("0 or 1 only");
                }
            }
        }

        public double this[String index]
        {
            get => index.ToLower() switch { "x" => X, "y" => Y, _ => throw new IndexOutOfRangeException("x or y only") };
            set
            {
                switch (index.ToLower())
                {
                    case "x": X = value; break;
                    case "y": Y = value; break;
                    default: throw new IndexOutOfRangeException("x or y only");
                }
            }
        }

        public static Vector operator +(Vector a, Vector b) => new()
        {
            X = a.X + b.X,
            Y = a.Y + b.Y
        };
        public static Vector operator -(Vector a, Vector b) => new()
        {
            X = a.X - b.X,
            Y = a.Y - b.Y
        };
        public static double operator *(Vector a, Vector b) => a.X * b.X + a.Y * b.Y;
        // Ділення вважатимемо множенням на обернений елемент: a/b = a * ~b
        public static double operator /(Vector a, Vector b) => a * ~b;

        public static Vector operator %(Vector a, Vector b) => new()
        {
            X = a.X % b.X,
            Y = a.Y % b.Y
        };
        // Оператори бітового зсуву: << >> >>>
        public static Vector operator <<(Vector a, int n) => new()
        {
            X = a.X - n,
            Y = a.Y
        };
        public static Vector operator >>(Vector a, int n) => new()
        {
            X = a.X + n,
            Y = a.Y
        };
        public static Vector operator >>>(Vector a, int n) => new()
        {
            X = a.X + n,
            Y = -a.Y
        };

        public static Vector operator &(Vector a, Vector b) => new()
        {
            X = Math.Max(a.X, b.Y),
            Y = Math.Max(a.Y, a.Y)
        };
        public static Vector operator |(Vector a, Vector b) => new()
        {
            X = Math.Min(a.X, b.Y),
            Y = Math.Min(a.Y, a.Y)
        };
        public static Vector operator ^(Vector a, Vector b) => new()
        {
            X = Math.Abs(Math.Min(a.X, b.Y)),
            Y = Math.Abs(Math.Min(a.Y, a.Y))
        };

        public static bool operator ==(Vector a, Vector b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector a, Vector b) => !(a == b);

        public static bool operator >(Vector a, Vector b) => a.X > b.X && a.Y > b.Y;
        public static bool operator <(Vector a, Vector b) => a.X < b.X && a.Y < b.Y;

        public static bool operator >=(Vector a, Vector b) => a.X >= b.X && a.Y >= b.Y;
        public static bool operator <=(Vector a, Vector b) => a.X <= b.X && a.Y <= b.Y;

        #endregion
    }
}