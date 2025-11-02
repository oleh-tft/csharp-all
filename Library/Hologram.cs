using csharp_all.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Library
{
    public class Hologram : Literature, INonPrintable
    {
        public String ArtItem { get; set; } = null!;

        public override string GetCard()
        {
            return $"{Title}: Hologram of {ArtItem} by {Publisher}";
        }
    }
}