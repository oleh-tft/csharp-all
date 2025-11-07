using csharp_all.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Extensions
{
    public static class IntExtension
    {
        public static String px(this Int32 value)
        {
            return value + "px";
        }

        public static String percnt(this Int32 value)
        {
            return value + "%";
        }
    }
}
