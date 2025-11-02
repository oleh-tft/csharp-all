using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Library
{
    public abstract class Literature
    {
        public String Publisher { get; set; } = String.Empty;
        public String Title { get; set; } = null!;

        public abstract String GetCard();

    }
}

/*
 * CTS - Common Type System - система спільних типів
 * 
 * 
 */