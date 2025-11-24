using csharp_all.Data.Dto;
using csharp_all.Data.Dto.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Data
{
    internal class DataAccessor
    {
        private readonly SqlConnection connection;

        public DataAccessor()
        {
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Адмін\source\repos\csharp-all\Database1.mdf;Integrated Security=True";
            this.connection = new(connectionString);

            try
            {
                this.connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection failed: {0}", ex.Message);
                return;
            }
        }

        public T FromReader<T>(SqlDataReader reader)
        {
            var t = typeof(T);
            var ctr = t.GetConstructor([]);
            T res = (T) ctr!.Invoke(null);
            foreach (var prop in t.GetProperties())
            {
                try
                {
                    Object data = reader.GetValue(prop.Name);
                    if (data.GetType() == typeof(decimal))
                    {
                        prop.SetValue(res, Convert.ToDouble(data));
                    }
                    else
                    {
                        prop.SetValue(res, data);
                    }
                }
                catch { }
            }
            return res;
        }
        public T ExecuteScalar<T>(String sql, Dictionary<String, Object>? sqlParams = null)
        {
            using SqlCommand cmd = new(sql, connection);
            foreach (var param in sqlParams ?? [])
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                return FromReader<T>(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
        }


        public Product RandomProduct()
        {
            return ExecuteScalar<Product>("SELECT TOP 1 * FROM Products ORDER BY NEWID()");
        }
        public Department RandomDepartment()
        {
            return ExecuteScalar<Department>("SELECT TOP 1 * FROM Departments ORDER BY NEWID()");
        }
        public Manager RandomManager()
        {
            return ExecuteScalar<Manager>("SELECT TOP 1 * FROM Managers ORDER BY NEWID()");
        }

        public List<T> ExecuteList<T>(String sql, Dictionary<String, Object>? sqlParams = null)
        {
            List<T> res = [];
            using SqlCommand cmd = new(sql, connection);
            foreach (var param in sqlParams ?? [])
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(FromReader<T>(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
            return res;
        }
        public List<Product> GetProducts()
        {
            return ExecuteList<Product>("SELECT * FROM Products");
        }

        public void Install()
        {
            InstallProducts();
            InstallDepartments();
            InstallManagers();
            InstallSales();
        }
        private void InstallSales()
        {
            String sql = "CREATE TABLE Sales(" +
                "Id        UNIQUEIDENTIFIER PRIMARY KEY," +
                "ManagerId UNIQUEIDENTIFIER NOT NULL," +
                "ProductId UNIQUEIDENTIFIER NOT NULL," +
                "Quantity  INT    NOT NULL DEFAULT 1," +
                "Moment    DATETIME2        NOT NULL  DEFAULT CURRENT_TIMESTAMP)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        private void InstallManagers()
        {
            String sql = "CREATE TABLE Managers(" +
                "Id           UNIQUEIDENTIFIER PRIMARY KEY," +
                "DepartmentId UNIQUEIDENTIFIER NOT NULL," +
                "Name         NVARCHAR(64)     NOT NULL," +
                "WorksFrom    DATETIME2        NOT NULL  DEFAULT CURRENT_TIMESTAMP)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        private void InstallDepartments()
        {
            String sql = "CREATE TABLE Departments(" +
                "Id    UNIQUEIDENTIFIER PRIMARY KEY," +
                "Name  NVARCHAR(64)     NOT NULL)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        private void InstallProducts()
        {
            String sql = "CREATE TABLE Products(" +
                "Id    UNIQUEIDENTIFIER PRIMARY KEY," +
                "Name  NVARCHAR(64)     NOT NULL," +
                "Price DECIMAL(14,2)    NOT NULL)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }

        public void Seed()
        {
            SeedProducts();
            SeedDepartments();
            SeedManagers();
        }
        public void SeedProducts()
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
                "('37D70E98-3CA4-473A-B1C3-F3CE6B632CEA', N'Samsung A1', 24000.00), " +
                "('63F13DDA-353E-4339-BE91-F4C8084D5BA8', N'Nokia 123', 20000.00), " +
                "('6F2EBA9C-A29A-4036-9F7E-2394F54F04BC', N'HUAWEI T510', 14000.00), " +
                "('36675E9F-AA70-4083-B045-5F7A3DBD1247', N'Windows Phone 520', 6000.00), " +
                "('4FBC38C8-10FB-49ED-B250-BEE72BB3B7DF', N'Linux Phone 10', 14000.00), " +
                "('0EFE9157-36CF-4E56-AD37-DDEFA50CBCF5', N'MacOS Phone 16', 66000.00), " +
                "('89335FD6-8898-48C1-901E-2F339585B9C8', N'iPhone 6', 6000.00), " +
                "('86B1FD17-9300-44DB-ACBF-03A2ECDFA679', N'iPhone 7', 7000.00), " +
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
        public void SeedManagers()
        {
            String sql = "INSERT INTO Managers VALUES" +
                "('1A391E34-B922-45FD-A4AD-45595F7F2B49', 'C7727779-9EE3-4127-988E-F7E93A780204', N'Олександр Ковальчу', '2020-06-01')," +
                "('6597CDC7-1754-42B6-B62A-23509A43711B', 'C7727779-9EE3-4127-988E-F7E93A780204', N'Марія Шевченко', '2020-05-02')," +
                "('457B9734-783D-459E-8F61-FE13DAAC6CE2', 'C7727779-9EE3-4127-988E-F7E93A780204', N'Андрій Бондаренко', '2020-04-03')," +
                "('81279EE1-CDA6-44DE-8866-6BF73B1B92F6', 'C7727779-9EE3-4127-988E-F7E93A780204', N'Ірина Дяченко', '2020-03-04')," +
                "('129D87B0-89D9-4C22-985B-3527A2051712', 'C7727779-9EE3-4127-988E-F7E93A780204', N'Сергій Мороз', '2020-02-05')," +
                "('94469239-3B10-4BDA-8DB9-FC6634158E7D', '451DD3B1-2287-4881-B66A-F5B3849B677C', N'Наталія Бойко', '2020-01-06')," +
                "('600BBAF1-C59C-4D5A-841C-F286EB9D604A', '451DD3B1-2287-4881-B66A-F5B3849B677C', N'Володимир Петренко', '2020-02-07')," +
                "('5521B25A-48E4-448B-B5CE-7A20EBEDBFD3', '451DD3B1-2287-4881-B66A-F5B3849B677C', N'Олена Ткаченко', '2020-03-08')," +
                "('C498B1C2-7868-4F12-B9FE-FAF025138F8F', '451DD3B1-2287-4881-B66A-F5B3849B677C', N'Денис Лисенко', '2020-04-09')," +
                "('46C5E4FB-CDED-4207-9B54-FFD92B83A983', '451DD3B1-2287-4881-B66A-F5B3849B677C', N'Христина Савчук', '2020-05-10')," +
                "('3DE2104C-7467-4D4E-955B-C77E2F22ABDB', '451DD3B1-2287-4881-B66A-F5B3849B677C', N'Михайло Поліщук', '2020-06-11')," +
                "('02457ED1-7331-46B7-833A-C5CA1D51688D', '8C51535C-26E3-4B8A-9F7C-7D669C4672AE', N'Анастасія Романюк', '2020-07-12')," +
                "('124BE452-1FE4-40BA-B435-ECE466C7949B', '8C51535C-26E3-4B8A-9F7C-7D669C4672AE', N'Юрій Гуменюк', '2020-08-13')," +
                "('CB1B6913-64AD-4F7A-88FB-4716F7214538', '8C51535C-26E3-4B8A-9F7C-7D669C4672AE', N'Катерина Остапчук', '2020-09-14')," +
                "('01CADDFF-90AE-4ABA-97FC-26C51710D1EE', '8C51535C-26E3-4B8A-9F7C-7D669C4672AE', N'Артем Левченко', '2020-10-15')," +
                "('0E4A0648-E496-4CAB-991D-75750818D666', '8C51535C-26E3-4B8A-9F7C-7D669C4672AE', N'Софія Кравчук', '2020-11-16')," +
                "('E1D87EAD-B647-485A-AA4C-76791DD1B6A8', '8C51535C-26E3-4B8A-9F7C-7D669C4672AE', N'Богдан Мельник', '2020-12-17')," +
                "('BD82F257-A84F-46F3-A391-F8C3AE218C45', 'B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'Дарина Грицай', '2020-11-18')," +
                "('0881EEA7-5D6C-4710-8328-4E99A28FCE37', 'B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'Роман Черненко', '2020-10-19')," +
                "('95AAA566-E465-4E72-8DBB-81045E916029', 'B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'Вікторія Панченко', '2020-09-20')," +
                "('150D7544-8987-4B65-9ECA-9D970B21F2E2', 'B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'Тарас Зінчук', '2020-08-21')," +
                "('4996A67B-D11F-4C6F-A855-F2237D33E0F6', 'B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'Людмила Верес', '2020-07-22')," +
                "('EA88FC4F-B192-4B5D-B74E-30CAD39A6ACE', 'B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'Євген Коваленко', '2020-06-23')," +
                "('E371970A-54DB-49EF-932B-8B8B155EC4F8', 'B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'Марта Гнатюк', '2020-05-24')," +
                "('B40EE754-56D2-4BDF-A938-ABBDED6442AA', 'B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'Павло Скиба', '2020-04-25')," +
                "('5AE5888E-F3AE-4CF9-9BB0-45CF4A3C88C9', 'B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'Зоряна Тимошенко', '2020-03-26')," +
                "('B56EE2E0-9C24-4D93-A8EB-CADCAEA3A376', 'B471180C-B4C0-4DF3-9290-D7DE881C94C7', N'Ігор Чумак', '2020-02-27')," +
                "('DF3B613A-FB08-4839-87E2-EA72254123CE', 'B471180C-B4C0-4DF3-9290-D7DE881C94C7', N'Валерія Слободян', '2020-01-28')";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        public void SeedDepartments()
        {
            String sql = "INSERT INTO Departments VALUES" +
                "('C7727779-9EE3-4127-988E-F7E93A780204', N'Відділ маркетингу')," +
                "('451DD3B1-2287-4881-B66A-F5B3849B677C', N'Відділ реклами')," +
                "('8C51535C-26E3-4B8A-9F7C-7D669C4672AE', N'Відділ продажів')," +
                "('B4C174CC-8C18-46DF-B8B4-F9E6F51EDCEA', N'ІТ відділ')," +
                "('B471180C-B4C0-4DF3-9290-D7DE881C94C7', N'Служба безпеки')";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }

        public void FillSales()
        {
            String sql = "INSERT INTO Sales(Id, ManagerId, ProductId, Quantity, Moment) VALUES" +
                "( NEWID(), " +
                " (SELECT TOP 1 Id FROM Managers ORDER BY NEWID()), " +
                " (SELECT TOP 1 Id FROM Products ORDER BY NEWID()), " +
                " (SELECT 1 + ABS(CHECKSUM(NEWID())) % 10), " +
                " (SELECT DATEADD(MINUTE, ABS(CHECKSUM(NEWID())) % 525600, '2024-01-01')) " +
                ")";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                for (int i = 0; i < 1e5; i++)
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }

        public List<SaleModel> MonthlySalesByManagersOrm(int month, int year = 2025)
        {
            return ExecuteList<SaleModel>(@$"
                        SELECT
	                        MAX(M.Name) AS [{nameof(SaleModel.ManagerName)}],
	                        COUNT(S.Id) AS [{nameof(SaleModel.Sales)}]
                        FROM
	                        Sales S
	                        JOIN Managers M ON S.ManagerId = M.Id
                        WHERE
	                        Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)
                        GROUP BY
	                        M.Id
                        ORDER BY
	                        2 DESC",new() {["@date"] = new DateTime(year, month , 1) });
        }

        public void MonthlySalesByManagersSql(int month, int year = 2025)
        {
            String sql = @"
                        SELECT
	                        MAX(M.Name),
	                        COUNT(S.Id)
                        FROM
	                        Sales S
	                        JOIN Managers M ON S.ManagerId = M.Id
                        WHERE
	                        Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)
                        GROUP BY
	                        M.Id
                        ORDER BY
	                        2 DESC";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@date", new DateTime(year, month, 1));
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("{0} -- {1}", reader[0], reader[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
        }
        public (int, int) GetSalesInfoByMonth(int month)
        {
            String sql = $"SELECT COUNT(*) FROM Sales WHERE Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@date", new DateTime(2025, month, 1));
            using SqlCommand cmd2 = new(sql, connection);
            cmd2.Parameters.AddWithValue("@date", new DateTime(2024, month, 1));
            try
            {
                int cur = Convert.ToInt32(cmd.ExecuteScalar());
                int prev = Convert.ToInt32(cmd2.ExecuteScalar());
                return (cur, prev);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
        }
        public int GetSalesCountByMonth(int month, int year = 2025)
        {
            String sql = $"SELECT COUNT(*) FROM Sales WHERE Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@date", new DateTime(year, month, 1));
            try
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
        }
        public List<int> GetMonthlySalesByYear(int year)
        {
            String sql = @"
                SELECT 
	                YEAR(S.Moment) AS [year], 
	                MONTH(S.Moment) AS [month], 
	                COUNT(*) AS [cnt] 
                FROM 
	                Sales S
                WHERE
	                YEAR(S.Moment) = @year
                GROUP BY 
	                YEAR(S.Moment),
	                MONTH(S.Moment) 
                ORDER BY 
	                1, 2";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@year", year);
            try
            {
                List<int> result = new();
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader.GetInt32(2));
                }
                return result;
                //return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
        }

    }
}
