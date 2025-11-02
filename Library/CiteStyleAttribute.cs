using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Library
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CiteStyleAttribute : Attribute
    {
        public String Style { get; set; } = null!;
    }
}