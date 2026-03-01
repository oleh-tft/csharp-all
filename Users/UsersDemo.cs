using csharp_all.Users.Dal;
using csharp_all.Users.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace csharp_all.Users
{
    internal record MenuItem(char Key, String Title, Action? Action) // primary constructor
    {
        public override string ToString()
        {
            return $"{Key} - {Title}";
        }
    };

    internal class UsersDemo
    {
        private DataAccessor accessor = null!;

        private MenuItem[] menu => [
            new MenuItem('i', "Інсталювати таблиці БД", () => accessor.Install()),
            new MenuItem('h', "Переінсталювати таблиці БД", () => accessor.Install(isHard:true)),
            new MenuItem('g', "Згенерувати одноразовий пароль", () => Console.WriteLine(accessor.GenerateCode(8, DataAccessor.CodeMode.Mixed))),
            new MenuItem('1', "Реєстрація нового користувача", SignUp),
            new MenuItem('2', "Вхід до системи (автентифікація)", SignIn),
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

            MenuItem? selectedItem;
            do
            {
                foreach (var item in menu)
                {
                    Console.WriteLine(item);
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.WriteLine();
                selectedItem = menu.FirstOrDefault(item => item.Key == keyInfo.KeyChar);
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

        private void SignIn()
        {
            Console.WriteLine("SignIn");
        }

        private void SignUp()
        {
            UserData userData = new();
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

        }
    }
}
