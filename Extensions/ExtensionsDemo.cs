using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csharp_all.Extensions.Str;
using csharp_all.Library;

namespace csharp_all.Extensions
{
    public class ExtensionsDemo
    {
        public void Run()
        {
            Console.WriteLine("Extensions Demo");
            Console.WriteLine("margin: " + 2.px());
            Console.WriteLine("the_snake_case_name".SnakeToCamel());
            Console.WriteLine("width: " + 50.percnt());
            double money = 123412134.5D;
            Console.WriteLine($"{money} -> {money.ToMoney()}");

            var a = (new Journal()
            {
                Title = "ArgC & ArgV",
                Number = "2(113)",
                Publisher = "https://journals.ua/technology/argc-argv/",
                Year = 2000
            });
            Console.WriteLine(a.IsDaily() ? "Щоденна" : "Не щоденна");
            var b = (new Newspaper()
            {
                Title = "Gazette de Leopol",
                Publisher = "First Ukrainian press in Ukraine",
                Date = new DateOnly(1776, 1, 15)
            });
            Console.WriteLine(b.IsDaily() ? "Щоденна" : "Не щоденна");
            DateTime dateTime = DateTime.Now;
            Console.WriteLine($"Original: {dateTime}\t SQL: {dateTime.ToSqlFormat()}");
            dateTime = new DateTime(2025, 1, 1);
            Console.WriteLine($"Original: {dateTime}\t SQL: {dateTime.ToSqlFormat()}");
        }
    }

    public static class PeriodExtension
    {
        public static bool IsDaily(this IPeriodic lit)
        {
            return lit.GetPeriod() == "День";
        }
    }

    public static class DateExtension
    {
        public static String ToSqlFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
