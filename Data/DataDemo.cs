using csharp_all.Data.Dto;
using Microsoft.Data.SqlClient;
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
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Адмін\source\repos\csharp-all\Database1.mdf;Integrated Security=True";
            SqlConnection connection = new(connectionString);

            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection failed: {0}", ex.Message);
                return;
            }
            DataAccessor dataAccessor = new(connection);
            //dataAccessor.Install();
            //dataAccessor.Seed();

            List<Product> products = dataAccessor.GetProducts(); 

            products.ForEach(Console.WriteLine);

            Helper.SpecialOperator("==============================");
            foreach (var item in products)
            {
                Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            }
            Helper.SpecialOperator("=============За зростанням=================");
            foreach (var item in products.OrderBy(p => p.Price))
            {
                Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            }
            Helper.SpecialOperator("=============За спаданням=================");
            foreach (var item in products.OrderByDescending(p => p.Price))
            {
                Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            }
            Helper.SpecialOperator("=============За абеткою=================");
            foreach (var item in products.OrderBy(p => p.Name)) 
            {
                Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            }
            Helper.SpecialOperator("=============3 найдорожчі товари=================");
            foreach (var item in products.OrderByDescending(p => p.Price).Take(3))
            {
                Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            }
            Helper.SpecialOperator("=============3 найдешевші товари=================");
            foreach (var item in products.OrderBy(p => p.Price).Take(3))
            {
                Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            }
            Helper.SpecialOperator("=============3 випадкові товари=================");
            for (int i = 0; i < 3; i++)
            {
                Random rand = new();
                Product item = products[rand.Next(products.Count - 1)];
                Console.WriteLine("{0} -- {1}", item.Name, item.Price);
            }
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