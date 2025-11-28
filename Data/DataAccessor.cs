using csharp_all.Data.Attributes;
using csharp_all.Data.Dto;
using csharp_all.Data.Models;
using csharp_all.Library;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace csharp_all.Data
{
    public enum CompareMode
    {
        ByChecks,
        ByQuantity,
        ByMoney
    }

    internal class DataAccessor
    {
        public SqlConnection connection { get; private set; }

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

        public IEnumerable<ProdSaleModel> Top3DailyProducts(CompareMode compareMode)
        {
            String sql = "";
            if (compareMode == CompareMode.ByQuantity)
            {
                sql = @$"
                    SELECT TOP 3
	                    MAX(P.Name) AS [{nameof(ProdSaleModel.Product.Name)}],
	                    SUM(S.Quantity) AS [{nameof(ProdSaleModel.Quantity)}],
	                    SUM(P.Price) AS [{nameof(ProdSaleModel.Money)}],
	                    COUNT(S.Id) AS [{nameof(ProdSaleModel.Checks)}]
                    FROM
	                    Sales S
	                    JOIN Products P ON S.ProductId = P.Id
                    WHERE
	                    Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)
                    GROUP BY
	                    P.Id
                    ORDER BY
	                    2 DESC";
            } 
            else if (compareMode == CompareMode.ByMoney)
            {
                sql = @$"
                    SELECT TOP 3
	                    MAX(P.Name) AS [{nameof(ProdSaleModel.Product.Name)}],
	                    SUM(S.Quantity) AS [{nameof(ProdSaleModel.Quantity)}],
	                    SUM(P.Price) AS [{nameof(ProdSaleModel.Money)}],
	                    COUNT(S.Id) AS [{nameof(ProdSaleModel.Checks)}]
                    FROM
	                    Sales S
	                    JOIN Products P ON S.ProductId = P.Id
                    WHERE
	                    Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)
                    GROUP BY
	                    P.Id
                    ORDER BY
	                    3 DESC";
            }
            else if (compareMode == CompareMode.ByChecks)
            {
                sql = @$"
                    SELECT TOP 3
	                    MAX(P.Name) AS [{nameof(ProdSaleModel.Product.Name)}],
	                    SUM(S.Quantity) AS [{nameof(ProdSaleModel.Quantity)}],
	                    SUM(P.Price) AS [{nameof(ProdSaleModel.Money)}],
	                    COUNT(S.Id) AS [{nameof(ProdSaleModel.Checks)}]
                    FROM
	                    Sales S
	                    JOIN Products P ON S.ProductId = P.Id
                    WHERE
	                    Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)
                    GROUP BY
	                    P.Id
                    ORDER BY
	                    4 DESC";
            }
            return ExecuteList<ProdSaleModel>(sql, new() { ["@date"] = DateTime.Now });
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
                    if (prop.Name == "Product")
                    {
                        prop.SetValue(res, FromReader<Product>(reader));
                    }
                    else
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

        public List<Product> GetProducts()
        {
            return ExecuteList<Product>("SELECT * FROM Products");
        }
        public List<Department> GetDepartments()
        {
            return ExecuteList<Department>("SELECT * FROM Departments");
        }
        public List<Manager> GetManagers()
        {
            return ExecuteList<Manager>("SELECT * FROM Managers");
        }
        public List<News> GetNews()
        {
            return ExecuteList<News>("SELECT * FROM News");
        }
        public List<T> GetAll<T>()
        {
            var t = typeof(T);
            var attr = t.GetCustomAttribute<TableNameAttribute>();
            String tableName = attr?.Value ?? t.Name + "s";
            return ExecuteList<T>($"SELECT * FROM {tableName}");
        }

        public IEnumerable<Department> EnumDepartments()
        {
            String sql = "SELECT * FROM Departments";
            using SqlCommand cmd = new(sql, connection);
            SqlDataReader? reader;
            try
            {
                reader = cmd.ExecuteReader();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
            while (reader.Read())
            {
                yield return FromReader<Department>(reader);
            }
            reader.Dispose();
        }
        public IEnumerable<Manager> EnumManagers()
        {
            String sql = "SELECT * FROM Managers";
            using SqlCommand cmd = new(sql, connection);
            SqlDataReader? reader;
            try
            {
                reader = cmd.ExecuteReader();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
            while (reader.Read())
            {
                yield return FromReader<Manager>(reader);
            }
            reader.Dispose();
        }
        public IEnumerable<Product> EnumProducts()
        {
            String sql = "SELECT * FROM Products";
            using SqlCommand cmd = new(sql, connection);
            SqlDataReader? reader;
            try
            {
                reader = cmd.ExecuteReader();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
            while (reader.Read())
            {
                yield return FromReader<Product>(reader);
            }
            reader.Dispose();
        }
        public IEnumerable<Sale> EnumSales(int limit = 100)
        {
            String sql = "SELECT * FROM Sales";
            using SqlCommand cmd = new(sql, connection);
            SqlDataReader? reader;
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
            try
            {
                while (reader.Read())
                {
                    yield return FromReader<Sale>(reader);
                    limit -= 1;
                    if (limit == 0)
                    {
                        yield break;
                    }
                }
            }
            finally
            {
                reader.Dispose();
            }
        }
        public IEnumerable<T> EnumAll<T>()
        {
            var t = typeof(T);
            var attr = t.GetCustomAttribute<TableNameAttribute>();
            String tableName = attr?.Value ?? t.Name + "s";
            String sql = $"SELECT * FROM {tableName}";
            using SqlCommand cmd = new(sql, connection);
            SqlDataReader? reader;
            try
            {
                reader = cmd.ExecuteReader();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
            while (reader.Read())
            {
                yield return FromReader<T>(reader);
            }
            reader.Dispose();
        }

        public void Install()
        {
            InstallProducts();
            InstallDepartments();
            InstallManagers();
            InstallSales();
            InstallNews();
            InstallAccesses();
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
        private void InstallNews()
        {
            String sql = "CREATE TABLE News(" +
                "Id    UNIQUEIDENTIFIER PRIMARY KEY," +
                "AuthorId    UNIQUEIDENTIFIER NOT NULL," +
                "Title  NVARCHAR(256)     NOT NULL," +
                "Content NVARCHAR(MAX)    NOT NULL," +
                "Moment DATETIME2    NOT NULL DEFAULT CURRENT_TIMESTAMP)";
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
        private void InstallAccesses()
        {
            String sql = "CREATE TABLE Access(" +
                "Id    UNIQUEIDENTIFIER PRIMARY KEY," +
                "ManagerId    UNIQUEIDENTIFIER NOT NULL," +
                "Login  VARCHAR(32)     NOT NULL," +
                "Salt  CHAR(16)     NOT NULL," +
                "Dk CHAR(32)    NOT NULL)";
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

        public void Seed()
        {
            SeedProducts();
            SeedDepartments();
            SeedManagers();
            SeedNews();
            SeedAccesses();
        }
        private void SeedProducts()
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
        private void SeedManagers()
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
        private void SeedDepartments()
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
        private void SeedNews()
        {
            String sql = "INSERT INTO News VALUES" +
                "('1FA571EE-7C03-4484-85AC-03D9C4043C13', '3DE2104C-7467-4D4E-955B-C77E2F22ABDB', N'Живі організми здатні виживати у відкритому космосі', N'Космічний вакуум вважається найгіршим можливим середовищем для живих організмів, де вижити просто неможливо. Але новий експеримент, результати якого опубліковані в журналі iScience, поставив цю думку під великий сумнів.\r\n\r\nГрупа науковців з університету Хоккайдо (Японія) під керівництвом Томоміті Фудзіти розмістила спори моху на зовнішній частині Міжнародної космічної станції. Там вони перебували кілька місяців без жодного захисту. Весь цей час на них впливали вакуум, ультрафіолетове випромінювання та різкі перепади температур — від значних мінусів до спеки.', '2025-11-20')," +
                "('7A785E4F-C070-4EF2-90A4-491D3873E9E8', '02457ED1-7331-46B7-833A-C5CA1D51688D', N'Власник ноутбука від Lenovo пообіцяв $2 тисячі тому, хто полагодить пристрій', N'Користувач nadimkobeissi оголосив \"полювання на баг\" у його ноутбуці Lenovo Legion Pro 7 і пообіцяв 500 доларів тому, хто полагодить плоский і глухий звук пристрою. Незабаром до ініціативи долучилися ще пятеро людей, і загальна сума винагороди досягла 2000 доларів.\r\n\r\nПроблема полягала в неправильному визначенні Linux аудіокодека Realtek ALC3306 і некоректній інтеграції між кодеком і підсилювачами в ноутбуці. Це спричиняло низьку якість звуку, незважаючи на наявність у системі твітерів і вуферів.\r\n\r\nВиправлення було знайдено приблизно за місяць. Воно працює на ядрі Linux 6.17.8, і Кобеіссі обіцяє підтримувати інструкцію до повної інтеграції фікса в основне ядро. Після встановлення звук функціонує коректно і залишається стабільним після перезавантаження. Сам фікс лежить на GitHub.', '2025-11-22')," +
                "('95E7A53C-E90B-4119-A258-71CA078AA375', '124BE452-1FE4-40BA-B435-ECE466C7949B', N'Ця рослина вижила після девяти місяців перебування у космічному вакуумі', N'Обєктом дослідження став вид Physcomitrium patens, відомий як розгалужений земний мох. У науковому світі ці рослини часто порівнюють із тихоходами – мікроскопічними тваринами, що відомі своєю невразливістю до екстремальних факторів.\r\n\r\nКоманда вчених під керівництвом Томомічі Фуджити з Університету Хоккайдо закріпила капсули зі спорами моху на зовнішній поверхні МКС. Протягом девяти місяців зразки перебували під впливом космічного вакууму, відчуваючи на собі різкі перепади температур та інтенсивне ультрафіолетове випромінювання, згубне для людей.', '2025-11-24')";
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
        private void SeedAccesses()
        {
            String sql = "INSERT INTO Access VALUES" +
                "('40C99C52-E8D8-42BF-AAF4-B06A193EA53D', '3DE2104C-7467-4D4E-955B-C77E2F22ABDB', 'manager1', 'p7Q2fA9mK1tR0xWd', 'd8f1a2c9e4b7f0d3c6a5e8b1c2f9d4a7')," +
                "('C1CDAF2E-6013-485F-B4F4-4D0A154F5415', '02457ED1-7331-46B7-833A-C5CA1D51688D', 'super_manager', 'Zb4uL9qH2yE7sV0n', 'a3c7e1b9f4d2a8c6e0f5b3d7a1c9e4f2')," +
                "('58E2A90B-9DD9-4CDF-B0CB-F837FBD83CA5', '124BE452-1FE4-40BA-B435-ECE466C7949B', 'bad_manager', 'tF3kR8wN6jB1cP5q', 'f9b2d1e7a4c3f8d0b5e6a9c2d7f1a4e8')," +
                "('68047CB6-9566-49A2-9631-D28F265287E6', 'CB1B6913-64AD-4F7A-88FB-4716F7214538', 'manager1990', 'mX7sD2vQ4hT9gL1u', 'c4e9a1f6d3b8e0c7a5f2d9b1e6c3a7f4')";
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
        public List<ProdSaleModel> MonthlySalesByProductsOrm(int month, int year = 2025)
        {
            return ExecuteList<ProdSaleModel>(@$"
                        SELECT
	                        MAX(P.Name) AS [{nameof(ProdSaleModel.Product.Name)}],
	                        COUNT(S.Id) AS [{nameof(ProdSaleModel.Quantity)}]
                        FROM
	                        Sales S
	                        JOIN Products P ON S.ProductId = P.Id
                        WHERE
	                        Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)
                        GROUP BY
	                        P.Id
                        ORDER BY
	                        2 DESC", new() { ["@date"] = new DateTime(year, month, 1) });
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
