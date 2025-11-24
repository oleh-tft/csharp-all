using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Data.Dto
{
    internal class Department
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = null!;

        public override string? ToString()
        {
            String id = Id.ToString();
            return $"{id[..3]}...{id[^3..]} {Name}";
        }
    }
}
