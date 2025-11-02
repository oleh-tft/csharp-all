using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Vectors
{
    internal class VectorDemo
    {
        public void Run()
        {
            Console.WriteLine("Vectors Demo");
            Vector v1 = new() { X = 10.0, Y = 20.0 };
            Vector v2 = v1;
            v1.X = 30;
            Console.WriteLine("v1 = {0}, v2 = {1}", v1, v2);

            RefVector r1 = new() { X = 10.0, Y = 20.0 };
            RefVector r2 = r1;
            r1.X = 30;
            Console.WriteLine("r1.x = {0}, r2.x = {1}", r1.X, r2.X);

            //оператори з новим типом
            Console.WriteLine("+v1 = {0}", +v1);
            Console.WriteLine("v1 + v2 = {0}", v1 + v2);
            Console.WriteLine("-v1 = {0}", -v1);
            Console.WriteLine("v1 - v2 = {0}", v1 - v2);
            Vector zero = new(0.0, 0.0);
            Console.WriteLine("zero vector is " + (zero ? "true" : "false"));
            Console.WriteLine("v1 vector is " + (v1 ? "true" : "false"));
            Console.WriteLine("!v1 vector is " + (!v1 ? "true" : "false"));

            Console.WriteLine("~v1 = {0}", ~v1);

            v1++;
            Console.WriteLine("v1++ => {0}", v1);
            v1--;
            Console.WriteLine("v1-- => {0}", v1);
            v1 += v2;
            Console.WriteLine("v1 += v2 => {0}", v1);
            v1 -= v2;
            Console.WriteLine("v1 -= v2 => {0}", v1);
            Console.WriteLine("v1 * v2 = {0}", v1 * v2);
            Console.WriteLine("v1 / v2 = {0}", v1 / v2);
            Console.WriteLine("v1[0] = {0}, v1[1] = {1}", v1[0], v1[1]);
            Console.WriteLine("v1[x] = {0}, v1[y] = {1}", v1["X"], v1["y"]);

            Console.WriteLine("v1 == v2 is " + (v1 == v2 ? "true" : "false"));
            Console.WriteLine("v1 != v2 is " + (v1 != v2 ? "true" : "false"));
            Console.WriteLine("v1 > v2 is " + (v1 > v2 ? "true" : "false"));
            Console.WriteLine("v1 < v2 is " + (v1 < v2 ? "true" : "false"));
            Console.WriteLine("v1 >= v2 is " + (v1 >= v2 ? "true" : "false"));
            Console.WriteLine("v1 <= v2 is " + (v1 <= v2 ? "true" : "false"));
        }
    }
}

/*
 * Особливості роботи з Value Types
 * - базовими типами з семантикою "за значенням" є struct та record struct
 * 
 */