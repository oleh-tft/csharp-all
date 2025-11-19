using csharp_all.Data.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Data
{
    internal class DataAccessor
    {
        private readonly SqlConnection connection;

        public DataAccessor(SqlConnection connection)
        {
            this.connection = connection;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = [];
            String sql = "SELECT * FROM Products";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(Product.FromReader(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
            }

            return products;
        }

        public void Install()
        {
            String sql = "CREATE TABLE Products(" +
                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                "Name NVARCHAR(64) NOT NULL," +
                "Price DECIMAL(14, 2) NOT NULL)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                return;
            }
        }

        public void Seed()
        {
            String sql = "INSERT INTO Products VALUES" +
                "('A743DFDB-A31D-4FC5-97BA-68B2231BB58A', N'Samsung Galaxy S24 Ultra', 200000.00), " +
                "('E6325A5D-C3AE-47C3-ABD1-FCFBA53C53D9', N'Google Pixel 8 Pro', 34000.00), " +
                "('EEEAB50F-5CB7-45DA-A505-5B76179E4FB3', N'iPhone 15 Pro', 40000.00), " +
                "('BE28AA76-C4BA-4A3D-B367-B2A5EC123E95', N'OnePlus 12', 14000.00), " +
                "('ED22791C-BD34-452C-A811-E9742E839A92', N'Samsung Galaxy A14', 9999000.00), " +
                "('DB8DE439-4D2E-490B-90D5-36ADC83A21EC', N'iPhone 7 Plus', 9000.00), " +
                "('35AFD762-9E11-4A8F-9438-7DD0AD2E48EC', N'iPhone 8 Plus', 12000.00), " +
                "('5467CCF4-68F9-41A1-B31F-C2407D8C144E', N'iPhone 9 Unreleased Edition', 19823712.00), " +
                "('A831179B-ED57-49FD-A441-526AC57B2D7C', N'iPhone 15 Pro Max', 44000.00), " +
                "('56156044-7C12-44F2-B892-64CBE3D563FA', N'iPhone xs', 6000.00)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                return;
            }
        }
    }
}
