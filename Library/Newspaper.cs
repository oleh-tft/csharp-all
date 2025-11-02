using csharp_all.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace csharp_all.Library
{
    public class Newspaper : Literature, IPeriodic
    {
        public DateOnly Date { get; set; }

        public override string GetCard()
        {
            return $"{base.Title} ({base.Publisher}) - {this.Date}";
        }

        public string GetPeriod()
        {
            return "День";
        }
    }
}