using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute(String value) : Attribute
    {
        public String Value { get; init; } = value!;
    }
}
