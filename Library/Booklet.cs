using csharp_all.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Library
{
    public class Booklet : Literature
    {
        public String Organization { get; set; } = null!;
        public int Year { get; set; }

        public override string GetCard()
        {
            return $"{base.Title} - {base.Publisher} - {this.Year}. {this.Organization}";
        }
        public void Print()
        {
            Console.WriteLine("Printing... " + GetCard());
        }
        [ColorPrint(Copies = 2)]
        public void PrintWithColor(String colorScheme)
        {
            Console.WriteLine($"In colors '{colorScheme}' Printing... {GetCard()}");
        }
    }
}