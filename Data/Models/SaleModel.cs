using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Data.Models
{
    internal class SaleModel
    {
        public String ManagerName { get; set; } = null!;
        public int Sales { get; set; }
    

        public override string ToString()
        {
            return $"{ManagerName} --- {Sales}";
        }
    }
}
