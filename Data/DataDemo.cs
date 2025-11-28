using csharp_all.Data.Dto;
using csharp_all.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Data
{
    internal class DataDemo
    {
        public void Run()
        {
            DataAccessor dataAccessor = new();

            Helper.SpecialOperator("===========ByMoney============");
            foreach (var m in dataAccessor.Top3DailyProducts(CompareMode.ByMoney))
            {
                Console.WriteLine(m);
            }
            Helper.SpecialOperator("===========ByChecks============");
            foreach (var m in dataAccessor.Top3DailyProducts(CompareMode.ByChecks))
            {
                Console.WriteLine(m);
            }
            Helper.SpecialOperator("===========ByQuantity============");
            foreach (var m in dataAccessor.Top3DailyProducts(CompareMode.ByQuantity))
            {
                Console.WriteLine(m);
            }
        }
        public void Run5()
        {
            DataAccessor dataAccessor = new();
            foreach (var dep in dataAccessor.EnumAll<Department>())
            {
                Console.WriteLine(dep);
                //String sql = $"SELECT * FROM Managers M WHERE M.DepartmentId = '{dep.Id}'";
                //using SqlCommand cmd = new(sql, dataAccessor.connection);
                //using SqlDataReader reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                //    Console.WriteLine(dataAccessor.FromReader<Manager>(reader));
                //}
            }
            Console.WriteLine("========================");
            foreach (var prod in dataAccessor.EnumAll<Product>())
            {
                Console.WriteLine(prod);
            }
            Console.WriteLine("========================");
            foreach (var prod in dataAccessor.EnumAll<News>())
            {
                Console.WriteLine(prod);
            }
        }

        public void Run4()
        {
            DataAccessor dataAccessor = new();
            List<Department> departments = dataAccessor.GetAll<Department>();
            List<Manager> managers = dataAccessor.GetAll<Manager>();
            List<Access> accesses = dataAccessor.GetAll<Access>();

            foreach (var name in departments.Select(d => d.Name))
            {
                Console.WriteLine(name);
            }
            Helper.SpecialOperator();
            var query1 = managers
                .Join(
                    departments,
                    m => m.DepartmentId,
                    d => d.Id,
                    (m, d) => new
                    {
                        Department = d,
                        Manager = m
                    }
                );
            foreach (var item in query1)
            {
                Console.WriteLine($"{item.Manager.Name} -- {item.Department.Name}");
            }
            Helper.SpecialOperator();
            Console.WriteLine(
                String.Join("\n", 
                departments
                .GroupJoin(
                    managers,
                    d => d.Id,
                    m => m.DepartmentId,
                    (d, m) => new
                    {
                        d.Name,
                        Count = m.Count(),
                        Managers = String.Join(", ", m.Select(m => m.Name))
                    }
                )
                .OrderByDescending(item => item.Count)
                .Select(item => String.Format("{0} ({1} empl): {2}", item.Name, item.Count, item.Managers))
                ));

            var query = accesses
                    .Join(
                        managers,
                        a => a.ManagerId,
                        m => m.Id,
                        (a, m) => new
                        {
                            Access = a,
                            Manager = m
                        }
                    );

            foreach (var item in query)
            {
                Console.WriteLine("{0} - {1}", item.Manager.Name, item.Access.Login);
            }
        }

        public void Run3()
        {
            DataAccessor dataAccessor = new();

            dataAccessor.GetAll<Product>().ForEach(Console.WriteLine);
            Helper.SpecialOperator();
            dataAccessor.GetAll<Department>().ForEach(Console.WriteLine);
            Helper.SpecialOperator();
            dataAccessor.GetAll<Manager>().ForEach(Console.WriteLine);
            Helper.SpecialOperator();
            dataAccessor.GetAll<News>().ForEach(Console.WriteLine);
        }

        public void Run2()
        {
            DataAccessor dataAccessor = new();
            dataAccessor.MonthlySalesByManagersSql(month: 10);
            Helper.SpecialOperator();
            dataAccessor.MonthlySalesByManagersOrm(month: 10).ForEach(Console.WriteLine);
            Helper.SpecialOperator();
            dataAccessor.MonthlySalesByProductsOrm(month: 5).ForEach(Console.WriteLine);
            //Console.WriteLine(dataAccessor.RandomProduct());
            //Console.WriteLine(dataAccessor.RandomDepartment());
            //Console.WriteLine(dataAccessor.RandomManager());
            //Helper.SpecialOperator();
            //dataAccessor.GetProducts().ForEach(Console.WriteLine);

            //Console.Write("Порівняти кількість продажів за місяць (1-12): ");
            //String? inputMonth = Console.ReadLine();
            //if (int.TryParse(inputMonth, out int valueMonth))
            //{
            //    try
            //    {
            //        var count = dataAccessor.GetSalesInfoByMonth(valueMonth);
            //        Console.WriteLine("2025:\t2024:\n");
            //        Console.WriteLine("{0}\t{1}", count.Item1, count.Item2);
            //    }
            //    catch
            //    {
            //        Console.WriteLine("Введене значення не було оброблене");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Введене значення не розпізнано як число");
            //}

            //Console.Write("Кількість продажів за рік: ");
            //String? inputYear = Console.ReadLine();
            //if (int.TryParse(inputYear, out int valueYear))
            //{
            //    try
            //    {
            //        List<int> sales = dataAccessor.GetMonthlySalesByYear(valueYear);
            //        for (int i = 0; i < 12; i++)
            //        {
            //            Console.WriteLine("{0} -- {1}", i + 1, sales[i]);
            //        }
            //        dataAccessor.GetMonthlySalesByYear(valueYear);
            //    }
            //    catch
            //    {
            //        Console.WriteLine("Введене значення не було оброблене");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Введене значення не розпізнано як число");
            //}

            //Console.Write("Кількість продажів за місяць (1-12): ");
            //String? input = Console.ReadLine();
            //if(int.TryParse(input, out int value))
            //{
            //    try
            //    {
            //        Console.WriteLine(dataAccessor.GetSalesCountByMonth(value));
            //    }
            //    catch
            //    {
            //        Console.WriteLine("Введене значення не було оброблене");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Введене значення не розпізнано як число");
            //}

            ////dataAccessor.Install();
            ////dataAccessor.Seed();
            ////dataAccessor.FillSales();

            //List<Product> products = dataAccessor.GetProducts(); 

            //products.ForEach(Console.WriteLine);

            //Helper.SpecialOperator("==============================");
            //foreach (var item in products)
            //{
            //    Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            //}
            //Helper.SpecialOperator("=============За зростанням=================");
            //foreach (var item in products.OrderBy(p => p.Price))
            //{
            //    Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            //}
            //Helper.SpecialOperator("=============За спаданням=================");
            //foreach (var item in products.OrderByDescending(p => p.Price))
            //{
            //    Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            //}
            //Helper.SpecialOperator("=============За абеткою=================");
            //foreach (var item in products.OrderBy(p => p.Name)) 
            //{
            //    Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            //}
            //Helper.SpecialOperator("=============3 найдорожчі товари=================");
            //foreach (var item in products.OrderByDescending(p => p.Price).Take(3))
            //{
            //    Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            //}
            //Helper.SpecialOperator("=============3 найдешевші товари=================");
            //foreach (var item in products.OrderBy(p => p.Price).Take(3))
            //{
            //    Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            //}
            //Helper.SpecialOperator("=============3 випадкові товари=================");
            //for (int i = 0; i < 3; i++)
            //{
            //    Random rand = new();
            //    Product item = products[rand.Next(products.Count - 1)];
            //    Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            //}
        }

        public void Run1()
        {
            Console.WriteLine("Data Demo");

            // I. Запуск
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Адмін\source\repos\csharp-all\Database1.mdf;Integrated Security=True";
            SqlConnection connection = new(connectionString);
            
            try
            {
                connection.Open();
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Connection failed: {0}", ex.Message);
                return;
            }

            // II. Формування та виконання команди (SQL)
            String sql = "SELECT CURRENT_TIMESTAMP";
            using SqlCommand cmd = new(sql, connection);
            Object scalar;
            try
            {
                scalar = cmd.ExecuteScalar();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                return;
            }

            // III. Передача та оброблення даних від БД
            DateTime timestamp;
            timestamp = Convert.ToDateTime(scalar);
            Console.WriteLine("Result: {0}", timestamp);

            // IV. Закриття підключення, перевірка, що всі дані передані
            connection.Close();
        }
    }
}

/*
 * Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Адмін\source\repos\csharp-all\Database1.mdf;Integrated Security=True
 * 
 */