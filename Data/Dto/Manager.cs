using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Data.Dto
{
    internal class Manager
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public String Name { get; set; } = null!;
        public DateTime WorksFrom { get; set; }

        public override string? ToString()
        {
            String id = Id.ToString();
            return $"{id[..3]}...{id[^3..]} {Name} - Works from {WorksFrom}";
        }
    }
}
