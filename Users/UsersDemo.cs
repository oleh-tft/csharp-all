using csharp_all.Users.Dal;
using csharp_all.Users.Dal.Entities;
using csharp_all.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace csharp_all.Users
{
    internal record MenuItem(char Key, String Title, Action? Action, bool isAuthorized = false) // primary constructor
    {
        public override string ToString()
        {
            return $"{Key} - {Title}";
        }
    };

    internal class UsersDemo
    {
        private const String savedFileName = "saved.model";
        private DataAccessor accessor = null!;
        private SignInModel? signInModel;

        private MenuItem[] menu => [
            new MenuItem('i', "Інсталювати таблиці БД", () => accessor.Install()),
            new MenuItem('h', "Переінсталювати таблиці БД", () => accessor.Install(isHard:true)),
            new MenuItem('1', "Реєстрація нового користувача", SignUp),
            new MenuItem('2', "Вхід до системи (автентифікація)", SignIn),
            new MenuItem('3', "Одержати персональні дані (авторизація)", GetPersonal, isAuthorized:true),
            new MenuItem('4', "Вийти з авторизованого режиму (Sign out)", SignOut, isAuthorized:true),
            new MenuItem('0', "Вихід", null)
            ];

        public void Run()
        {
            try
            {
                accessor = new();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            if (File.Exists(savedFileName))
            {
                signInModel = JsonSerializer.Deserialize<SignInModel>(
                    File.ReadAllText(savedFileName)
                )!;

                if (signInModel.AccessToken.TokenExp == null ||
                    signInModel.AccessToken.TokenExp > DateTime.Now)
                {
                    var task = accessor.ProlongToken(signInModel.AccessToken.TokenId);
                    Console.WriteLine($"Hello, {signInModel.UserData.UserName}, access restored!");
                    signInModel.AccessToken.TokenExp = task.Result;
                }
                else
                {
                    Console.WriteLine("Saved data expired. New login required");
                    signInModel = null;
                    File.Delete(savedFileName);
                }
            }

            MenuItem? selectedItem;
            do
            {
                foreach (var item in menu)
                {
                    if (!item.isAuthorized || signInModel != null)
                    {
                        Console.WriteLine(item);
                    }
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.WriteLine();
                selectedItem = menu.FirstOrDefault(item => item.Key == keyInfo.KeyChar && (!item.isAuthorized || signInModel != null));
                if (selectedItem is null)
                {
                    Console.WriteLine("Нерозпізнаний вибір");
                }
                else
                {
                    selectedItem.Action?.Invoke();
                }
            } while (selectedItem == null || selectedItem.Action != null);
        }

        private void SignOut()
        {
            Console.Write("Sign out? (y/...)? ");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.KeyChar == 'y' || keyInfo.KeyChar == 'Y')
            {
                signInModel = null;
                File.Delete(savedFileName);
                Console.WriteLine("You have signed out!");
            }
            else
            {
                Console.WriteLine("Signing out was cancelled");
            }
        }

        private void GetPersonal()
        {
            if (signInModel == null) return;
            Console.WriteLine($"Name: {signInModel.UserData.UserName}, Email: {signInModel.UserData.UserEmail}, TokenExp: {signInModel.AccessToken.TokenExp}");
        }

        private void SignIn()
        {
            Console.WriteLine("Authentication in system");
            String userEmail;
            String password;
            bool isEntryCorrect;
            do
            {
                Console.Write("E-mail: ");
                userEmail = Console.ReadLine()!.Trim();
                if (userEmail == String.Empty) return;
                isEntryCorrect = Regex.IsMatch(userEmail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                if (!isEntryCorrect)
                {
                    Console.WriteLine("E-mail не відповідає формату, відкоригуйте");
                }
            } while (!isEntryCorrect);

            Console.Write("Password: ");
            password = Console.ReadLine()!.Trim();

            signInModel = accessor.SignIn(userEmail, password).Result;
            if (signInModel == null)
            {
                Console.WriteLine("Access denied");
                return;
            }

            Console.WriteLine($"Welcome, {signInModel.UserData.UserName}, how is your day? Btw here is your token: {signInModel.AccessToken.TokenId}");
            Console.Write("Remember me? (y/...)? ");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.KeyChar == 'y' || keyInfo.KeyChar == 'Y')
            {
                File.WriteAllText(savedFileName, JsonSerializer.Serialize(signInModel));
                Console.WriteLine("Data was saved");
            }
        }

        private void SignUp()
        {
            UserData userData = new();
            String password;
            bool isEntryCorrect;
            Console.WriteLine("Реєстрація нового користувача (порожній ввід - вихід)");
            do
            {
                Console.Write("Повне Ім'я: ");
                userData.UserName = Console.ReadLine()!;
                if (userData.UserName == String.Empty) return;
                isEntryCorrect = userData.UserName.Length >= 2;
                if (!isEntryCorrect)
                {
                    Console.WriteLine("Занадто коротке, відкоригуйте");
                }
            } while (!isEntryCorrect);

            do
            {
                Console.Write("E-mail: ");
                userData.UserEmail = Console.ReadLine()!.Trim();
                if (userData.UserEmail == String.Empty) return;
                isEntryCorrect = Regex.IsMatch(userData.UserEmail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                if (!isEntryCorrect)
                {
                    Console.WriteLine("E-mail не відповідає формату, відкоригуйте");
                }
            } while (!isEntryCorrect);

            do
            {
                Console.Write("Password: ");
                password = Console.ReadLine()!.Trim();
                if (password == String.Empty) return;
                isEntryCorrect = true;

                if (password.Length < 6)
                {
                    Console.WriteLine("Пароль має містити щонайменше 6 символів");
                    isEntryCorrect = false;
                }

                if (!password.Any(char.IsDigit))
                {
                    Console.WriteLine("Пароль має містити хоча б одну цифру");
                    isEntryCorrect = false;
                }

                if (!password.Any(char.IsLower))
                {
                    Console.WriteLine("Пароль має містити хоча б одну малу літеру");
                    isEntryCorrect = false;
                }

                if (!password.Any(char.IsUpper))
                {
                    Console.WriteLine("Пароль має містити хоча б одну велику літеру");
                    isEntryCorrect = false;
                }

                if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                {
                    Console.WriteLine("Пароль має містити хоча б один спецсимвол");
                    isEntryCorrect = false;
                }

                if (!isEntryCorrect)
                {
                    Console.WriteLine("Спробуйте ще раз.\n");
                }

            } while (!isEntryCorrect);

            try
            {
                accessor.SignUp(userData, password).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine("Registered Successfully. Check your e-mail to verify");
            int cnt = 3;
            bool isConfirmed;
            do
            {
                Console.Write("Verification code: ");
                String code = Console.ReadLine()!;
                isConfirmed = accessor.ConfirmEmailCodeAsync(userData.UserId, code).Result;
                cnt--;
            } while (cnt > 0 && !isConfirmed);

            if (isConfirmed)
            {
                Console.WriteLine("E-mail is verified");
            }
            else
            {
                Console.WriteLine("E-mail was not verified, try verifying after authentication");
            }
        }
    }
}
