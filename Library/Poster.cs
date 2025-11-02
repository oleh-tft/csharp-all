using csharp_all.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Library
{
    public class Poster : Literature, INonPrintable
    {
        public DateOnly Date { get; set; }

        public override string GetCard()
        {
            return $"{base.Publisher} - {base.Title} - {this.Date}";
        }
    }
}