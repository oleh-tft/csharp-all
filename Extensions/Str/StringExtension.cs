using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Extensions.Str
{
    public static class StringExtension
    {
        /// <summary>
        /// Transforms from "the_snake_case" naming to TheCamelCase style
        /// </summary>
        /// <param name="str">name to transform</param>
        /// <returns>name in Capital Camel Case</returns>
        public static String SnakeToCamel(this String str)
        {
            return String.Join(
                String.Empty,
                str
                .Split('_', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s[0].ToString().ToUpper() + s[1..].ToLower())
            );
        }
    }
}
