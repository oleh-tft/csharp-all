using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Data.Dto
{
    internal class Product
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = null!;
        public double Price { get; set; }

        public static Product FromReader(SqlDataReader reader)
        {
            return new()
            {
                Id = reader.GetGuid("Id"),
                Name = reader.GetString("Name"),
                Price = Convert.ToDouble(reader.GetDecimal("Price"))
            };
        }

        public override string? ToString()
        {
            String id = Id.ToString();
            return $"{id[..3]}...{id[^3..]} {Name} {Price:F2}";
        }
    }

    
}
