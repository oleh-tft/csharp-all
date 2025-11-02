using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Fraction
{
    internal class FractionDemo
    {
        public void Run()
        {
            Console.WriteLine("Fraction Demo");
            Fraction a = new(2, 3);
            Fraction b = new(3, 4);
            Console.WriteLine("a = {0}, b = {1}", a, b);
            Console.WriteLine("a + b = {0}", a + b);
            Console.WriteLine("a - b = {0}", a - b);
            Console.WriteLine("a * b = {0}", a * b);
            Console.WriteLine("a / b = {0}", a / b);

        }
    }
}
