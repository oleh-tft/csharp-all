using csharp_all.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Data.Models
{
    internal class ProdSaleModel
    {
        public Product Product { get; set; } = null!;
        public int Checks { get; set; }
        public int Quantity { get; set; }
        public double Money { get; set; }


        public override string ToString()
        {
            return $"{Product.Name} --- Checks: {Checks} --- Total quantity: {Quantity} --- Total money: {Money}";
        }
    }
}
