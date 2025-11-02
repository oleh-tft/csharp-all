using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Library
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class ColorPrintAttribute : Attribute
    {
        public int Copies { get; set; } = 1;
    }
}