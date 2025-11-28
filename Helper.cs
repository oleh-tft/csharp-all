using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all
{
    internal class Helper
    {
        public static void SpecialOperator(Object text)
        {
            Array values = Enum.GetValues(typeof(ConsoleColor));
            Random random = new();
            Console.ForegroundColor = (ConsoleColor)values.GetValue(random.Next(values.Length-1) + 1);
            Console.WriteLine(text.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void SpecialOperator()
        {
            Array values = Enum.GetValues(typeof(ConsoleColor));
            Random random = new();
            Console.ForegroundColor = (ConsoleColor)values.GetValue(random.Next(values.Length - 1) + 1);
            Console.WriteLine("============================");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
