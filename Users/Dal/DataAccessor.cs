using csharp_all.Data.Dto;
using csharp_all.Services.Email;
using csharp_all.Services.Kdf;
using csharp_all.Users.Dal.Entities;
using csharp_all.Users.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
        private readonly IEmailService emailService = new GmailService();
        private readonly IKdfService kdfService = new PbKdfService();
        private const double tokenPeriodMinutes = 5.0;

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

        public async Task<DateTime> ProlongToken(Guid tokenId)
        {
            DateTime TokenExp = DateTime.Now.AddMinutes(tokenPeriodMinutes);
            await connection.ExecuteAsync("UPDATE AccessToken SET TokenExp = @TokenExp WHERE TokenId = @TokenId", new
            {
                TokenExp,
                tokenId
            });
            return TokenExp;
        }

        public async Task SignUp(UserData userData, String password)
        {
            if (userData.UserId == default)
            {
                userData.UserId = Guid.NewGuid();
            }
            userData.UserEmailCode = GenerateCode(6, CodeMode.Mixed);

            MailMessage mailMessage = new()
            {
                IsBodyHtml = true,
                Subject = "Wanna register?",
                Body = $"<html><h1>Here is your code!</h1><h2 style=\"color: #00FF00;\">{userData.UserEmailCode}</h2></html>"
            };
            mailMessage.To.Add(new MailAddress(userData.UserEmail));
            Task emailTask = emailService.SendAsync(mailMessage);

            Task dbTask = connection.ExecuteAsync(@"INSERT INTO UserData(UserId, UserName, UserEmail, UserEmailCode)
                VALUES(@UserId, @UserName, @UserEmail, @UserEmailCode)", userData);

            String salt = Guid.NewGuid().ToString()[..16];
            String dk = kdfService.Dk(salt, password);
            Task accessTask = connection.ExecuteAsync(@"INSERT INTO UserAccess(AccessId, UserId, AccessLogin, AccessSalt, AccessDk)
                VALUES(@AccessId, @UserId, @AccessLogin, @AccessSalt, @AccessDk)", new
            {
                AccessId = Guid.NewGuid(),
                UserId = userData.UserId,
                AccessLogin = userData.UserEmail,
                AccessSalt = salt,
                AccessDk = dk
            });

            await Task.WhenAll(emailTask, dbTask, accessTask);
        }

        public async Task<SignInModel?> SignIn(String login, String password)
        {
            UserAccess? userAccess = await connection.QuerySingleOrDefaultAsync<UserAccess>(
                "SELECT * FROM UserAccess u WHERE u.AccessLogin = @AccessLogin", new
                {
                    AccessLogin = login
                });

            if (userAccess == null || 
                kdfService.Dk(userAccess.AccessSalt, password) != userAccess.AccessDk)
            {
                return null;
            }

            SignInModel ret = new()
            {
                UserAccess = userAccess
            };

            var userDataTask = connection.QuerySingleAsync<UserData>(
                "SELECT * FROM UserData u WHERE u.UserId = @UserId", new
                {
                    UserId = userAccess.UserId
                });

            AccessToken? accessToken = await connection.QuerySingleOrDefaultAsync<AccessToken>(
            @"SELECT TOP 1 * FROM AccessToken WHERE AccessId = @AccessId AND TokenExp > @Now ORDER BY TokenExp DESC", new
            {
                AccessId = userAccess.AccessId,
                Now = DateTime.Now
            });
            if (accessToken == null)
            {
                accessToken = new()
                {
                    TokenId = Guid.NewGuid(),
                    AccessId = userAccess.AccessId,
                    TokenIat = DateTime.Now,
                    TokenExp = DateTime.Now.AddMinutes(tokenPeriodMinutes)
                };

                await connection.ExecuteAsync(@"INSERT INTO AccessToken(TokenId, AccessId, TokenIat, TokenExp) 
                VALUES(@TokenId, @AccessId, @TokenIat, @TokenExp)", accessToken);
            }
            
            ret.UserData = await userDataTask;
            ret.AccessToken = accessToken;

            return ret;
        }

        public async Task<bool> ConfirmEmailCodeAsync(Guid userId, String code)
        {
            UserData userData = await connection.QuerySingleAsync<UserData>(
                "SELECT * FROM UserData u WHERE u.UserId = @UserId", new
                {
                    UserId = userId
                });
            bool isOk = userData.UserEmailCode == code;
            if (isOk)
            {
                await connection.ExecuteAsync("UPDATE * FROM UserData u SET u.UserEmailCode = NULL WHERE UserId = @UserId", new
                {
                    UserId = userId
                });
            }
            return isOk;
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
