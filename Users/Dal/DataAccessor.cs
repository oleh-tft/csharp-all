using csharp_all.Users.Dal.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_all.Users.Dal
{
    internal class DataAccessor
    {
        private SqlConnection connection;
        private readonly Random rand = new();

        public DataAccessor()
        {
            String settingsFilename = "appsettings.json";
            if (!File.Exists(settingsFilename))
            {
                throw new Exception("Не знайдено файл конфігурації. Прочитайте README");
            }
            var settings = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(settingsFilename));
            String userDb;
            try
            {
                var csSection = settings.GetProperty("ConnectionStrings");
                userDb = csSection.GetProperty("UserDb").GetString()!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка визначення конфігурації: {ex.Message}");
            }
            connection = new(userDb);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка підключення БД: {ex.Message}");
            }
        }

        public void SignUp(UserData userData)
        {
            if (userData.UserId == default)
            {
                userData.UserId = Guid.NewGuid();
            }
            userData.UserEmailCode = rand.Next(100000, 1000000).ToString();
        }

        public void Install(bool isHard = false)
        {
            if (isHard)
            {
                connection.Execute("DROP TABLE IF EXISTS UserData");
            }
            connection.Execute(@"CREATE TABLE UserData (
                UserId          UNIQUEIDENTIFIER    PRIMARY KEY, 
                UserName        NVARCHAR(128)       NOT NULL,
                UserEmail       NVARCHAR(256)       NOT NULL,
                UserEmailCode   VARCHAR(16)             NULL,
                UserDelAt       DATETIME2               NULL
            )");
            if (isHard)
            {
                connection.Execute("DROP TABLE IF EXISTS UserAccess");
            }
            connection.Execute(@"CREATE TABLE UserAccess (
                AccessId        UNIQUEIDENTIFIER    PRIMARY KEY, 
                UserId          UNIQUEIDENTIFIER    NOT NULL, 
                RoleId          UNIQUEIDENTIFIER        NULL, 
                AccessLogin     NVARCHAR(64)        NOT NULL,
                AccessSalt      CHAR(16)            NOT NULL,
                AccessDk        CHAR(32)                NULL
            )");
            if (isHard)
            {
                connection.Execute("DROP TABLE IF EXISTS AccessToken");
            }
            connection.Execute(@"CREATE TABLE AccessToken (
                TokenId         UNIQUEIDENTIFIER    PRIMARY KEY, 
                AccessId        UNIQUEIDENTIFIER    NOT NULL,
                TokenIat        DATETIME2           NOT NULL,
                TokenExp        DATETIME2               NULL
            )");
        }

        public enum CodeMode
        {
            Digits,
            Letters,
            Mixed
        }

        public string GenerateCode(int length, CodeMode mode)
        {

            String chars = String.Empty;
            switch (mode)
            {
                case CodeMode.Digits: chars = "0123456789"; break;
                case CodeMode.Letters: chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"; break;
                case CodeMode.Mixed: chars = "23456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz"; break;
            };

            var code = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = rand.Next(chars.Length);
                code.Append(chars[index]);
            }

            return code.ToString();
        }


    }
}
