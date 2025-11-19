using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace csharp_all.Collect
{
    internal class CollectionsDemo
    {
        class Manager
        {
            public int Id { get; set; }
            public String Name { get; set; } = String.Empty;

            public override string? ToString()
            {
                return $"{Name} (id={Id})";
            }
        }

        class Sale
        {
            public int Id { get; set; }
            public int ManagerId { get; set; }
            public double Price { get; set; }

            public override string? ToString()
            {
                return $"Sale (id={Id}, ManagerId={ManagerId}, Price={Price:F2}) ";
            }
        }

        public void Run()
        {
            Random random = new();
            List<Manager> managers = [
                new() {Id = 1, Name = "John Doe"},
                new() {Id = 2, Name = "July Smith"},
                new() {Id = 3, Name = "Nik Forest"},
                new() {Id = 4, Name = "Barbara White"},
                new() {Id = 5, Name = "Will Black"},
            ];

            List<Sale> sales = [..Enumerable
                .Range(1, 100)
                .Select(x => new Sale {
                    Id = x,
                    ManagerId = 1 + x % managers.Count,
                    Price = random.NextDouble() * 1000
                })
            ];

            sales.Take(10).ToList().ForEach(Console.WriteLine);
            var query1 = sales.Select(s => new
            {
                managers.First(m => m.Id == s.ManagerId).Name,
                s.ManagerId,
                s.Price
            });
            var query2 = sales
                .Join(
                    managers,
                    s => s.ManagerId,
                    m => m.Id,
                    (s, m) => new
                    {
                        Sale = s,
                        Manager = m
                    });

            var query3 = managers
                .GroupJoin(
                    sales,
                    m => m.Id,
                    s => s.ManagerId,
                    (m, ss) => new
                    {
                        Manager = m,
                        Sum = ss.Sum(s => s.Price)
                    })
                .Select(item => new
                {
                    Name = item.Manager.Name,
                    Total = item.Sum
                })
                .OrderBy(item => item.Total);

            var query4 = managers
               .GroupJoin(
                   sales,
                   m => m.Id,
                   s => s.ManagerId,
                   (m, ss) => new
                   {
                       Manager = m,
                       Sum = ss.Sum(s => s.Price)
                   })
               .Select(item => new
               {
                   Name = item.Manager.Name,
                   Total = item.Sum
               })
               .OrderByDescending(item => item.Total)
               .ToList();

            var query5 = managers
               .GroupJoin(
                   sales,
                   m => m.Id,
                   s => s.ManagerId,
                   (m, ss) => new
                   {
                       Manager = m,
                       Amount = ss.Where(item => item.Price > 500).Count()
                   })
               .Select(item => new
               {
                   Name = item.Manager.Name,
                   Amount = item.Amount
               })
               .OrderByDescending(item => item.Amount)
               .ToList();

            var query6 = managers
               .GroupJoin(
                   sales,
                   m => m.Id,
                   s => s.ManagerId,
                   (m, ss) => new
                   {
                       Manager = m,
                       Amount = ss.Where(item => item.Price < 100).Count()
                   })
               .Select(item => new
               {
                   Name = item.Manager.Name,
                   Amount = item.Amount
               })
               .OrderByDescending(item => item.Amount)
               .ToList();

            //foreach (var item in query1)
            //{
            //    Console.WriteLine("{0} (ID:{1}) {2:F2}", item.Name, item.ManagerId, item.Price);
            //}
            //foreach (var item in query2)
            //{
            //    Console.WriteLine("{0} {1}", item.Manager, item.Sale);
            //}

            foreach (var item in query3)
            {
                Console.WriteLine("{0} -- {1:F2}", item.Name, item.Total);
            }
            Console.WriteLine("====================");
            var theWorst = query3.First();
            Console.WriteLine("Найгірший результат {0:F2} у {1}", theWorst.Total, theWorst.Name);

            Console.WriteLine("====================");

            foreach (var w in query3.Take(3))
            {
                Console.WriteLine("{0} -- {1:F2}", w.Name, w.Total);
            }

            Console.WriteLine("\n====================");
            var theBest = query4.First();
            Console.WriteLine("Найкращій результат {0:F2} у {1}", theBest.Total, theBest.Name);

            Console.WriteLine("====================");

            for (int i = 0; i < 3; i++)
            {
                var w = query4[i];
                int bonus = 10 - (i * 3) + i/2;
                Console.WriteLine("{0} -- {1:F2}. Премія {2}% - {3:F2}", w.Name, w.Total, bonus, w.Total * (bonus / 100.0));
            }

            Console.WriteLine("========Кількість продажів суми яких перевищують 500.00============");
            foreach (var item in query5)
            {
                Console.WriteLine("{0} -- {1}", item.Name, item.Amount);
            }
            Console.WriteLine("========Кількість продажів суми яких менші за 100.00============");
            foreach (var item in query6)
            {
                Console.WriteLine("{0} -- {1}", item.Name, item.Amount);
            }
        }

        public void RunL()
        {
            List<String> strings = [];
            for (int i = 0; i < 10; i++)
            {
                strings.Add("String " + i);
            }
            var query =
                from s in strings
                where s[^1] == '2' || s[^1] == '3'
                select s;

            //Console.WriteLine(query);
            foreach (var str in query)
            {
                Console.WriteLine(str);
            }

            var query2 = strings
                .Where(s => ((int)s[^1] & 1) == 1)
                .Select(s => s.ToUpper());
                //.ToList()

            Console.WriteLine("------------------");
            foreach (String str in query2)
            {
                Console.WriteLine(str);
            }

            //Predicate - функція або вираз, яка повертає true or false
        }

        public void RunList()
        {
            List<String> strings = [];
            for (int i = 0; i < 10; i++)
            {
                strings.Add("String " + i);
            }

            foreach (String str in strings)
            {
                Console.WriteLine(str);
            }
            strings.Add("New 7");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("-----------------");
            Console.ForegroundColor = ConsoleColor.White;
            strings.ForEach(Console.WriteLine);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("-----------------");
            Console.ForegroundColor = ConsoleColor.White;

            List<String> removes = [];
            foreach (var str in strings)
            {
                char c = str[^1];
                if (c <= '9' && c>= '0')
                {
                    int n = (int)c;
                    if ((n & 1) == 1)
                    {
                        removes.Add(str);
                    }
                }
            }
            foreach (String rem in removes)
            {
                strings.Remove(rem);
            }
            strings.ForEach(Console.WriteLine);
            Console.WriteLine("-----------------");
        }

        public void RunArr()
        {
            Console.WriteLine("Collections Demo");

            String[] arr1 = new String[3];
            String[] arr2 = new String[3] { "1", "2", "3" };
            String[] arr3 = { "1", "2", "3" };
            String[] arr4 = [ "1", "2", "3" ];
            arr1[0] = "Str 1";
            arr1[1] = arr2[0];

            List<Object> list;
            LinkedList<Object> linkedList;
            Dictionary<Object, Object> dictionary;
        }
    }
}