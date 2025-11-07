using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Extensions
{
    public static class DoubleExtension
    {
        public static String ToMoney(this Double value)
        {
            String money = $"{value:F2}";
            int start = money.IndexOf('.') == -1 ? money.Length : money.IndexOf('.');

            for (int i = start - 3; i > 0; i -= 3)
            {
                money = money.Insert(i, " ");
            }

            return money;
        }
    }
}
