using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Collect
{
    internal class CollectionsDemo
    {
        public void Run()
        {
            Console.WriteLine("Collections Demo");

            String[] arr1 = new String[3];
            String[] arr2 = new String[3] { "1", "2", "3" };
            String[] arr3 = { "1", "2", "3" };
            String[] arr4 = [ "1", "2", "3" ];
            arr1[0] = "Str 1";
            arr1[1] = arr2[0];
        }
    }
}
