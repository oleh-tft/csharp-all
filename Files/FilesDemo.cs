using csharp_all.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace csharp_all.Files
{
    internal class FilesDemo
    {

        public string GetTimesTranslation(int value)
        {
            int lastDigit = value.ToString().Last() - '0';
            if (value < 10 || value > 20)
            {
                if (lastDigit == 1)
                {
                    return "раз";
                }
                else if (lastDigit > 1 && lastDigit < 5)
                {
                    return "рази";
                }
            }
            return "разів";
        }

        public void Run()
        {
            string logDir = Directory.GetCurrentDirectory() + "/logs";
            if (!Directory.Exists(logDir))
            {
                try
                {
                    Directory.CreateDirectory(logDir);
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Неможливо створити директорію логування " + ex.Message);
                    return;
                }
            }
            string logFile = "runlogs.txt";
            string logPath = Path.Combine(logDir, logFile);
            if (!File.Exists(logPath))
            {
                try
                {
                    File.Create(logPath).Dispose();
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Неможливо створити файл логування " + ex.Message);
                    return;
                }
            } 
            else
            {
                try
                {
                    String[] lines = File.ReadAllLines(logPath);
                    Console.WriteLine("Програма була виконана {0} {1}", lines.Length, GetTimesTranslation(lines.Length));
                    for (int i = 0; i < lines.Length; i++)
                    {
                        Console.Write(i + 1 + ". ");
                        Console.WriteLine(lines[i]);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Неможливо відкрити файл логування " + ex.Message);
                }
            }
            int fileIndex = 0;

            FileInfo fileInfo = new FileInfo(logPath);

            while (fileInfo.Exists && fileInfo.Length >= 512)
            {
                fileIndex++;
                string newFileName = $"runlogs{fileIndex}.txt";
                logPath = Path.Combine(logDir, newFileName);
                fileInfo = new FileInfo(logPath);

                if (!fileInfo.Exists)
                {
                    try
                    {
                        File.Create(logPath).Dispose();
                        break;
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Неможливо створити новий файл логування " + ex.Message);
                        return;
                    }
                }
            }
            try
            {
                File.AppendAllText(logPath, DateTime.Now.ToString() + "\n");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Помилка логування " + ex.Message);
                return;
            }
        }

        public void RunLib()
        {
            Library.Library library = new();
            library.Init();
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            String lib = JsonSerializer.Serialize(library, options);
            Console.WriteLine(lib);
            File.WriteAllText("library.json", lib);

            Library.Library library2;
            try
            {
                library2 = JsonSerializer.Deserialize<Library.Library>(lib)!;
                // ? throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            library2.PrintCatalog();

            Vectors.Vector v = new() { X = 10, Y = 20 };
            String j = JsonSerializer.Serialize(v);
            Console.WriteLine(j);
            Vectors.Vector v2 = JsonSerializer.Deserialize<Vectors.Vector>(j);
            Console.WriteLine(v2);
        }

        public void Run4()
        {
            string dir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(dir, "file4.txt");
            try
            {
                File.WriteAllText(filepath, "File 4 content");
                File.AppendAllText(filepath, "\nAppend line");
                Console.WriteLine(File.ReadAllText(filepath));
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Run3()
        {
            try
            {
                using var wtr = new StreamWriter("./file3.txt");
                wtr.Write("x = ");
                wtr.Write(20);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                using var rdr = new StreamReader("./file3.txt");
                string content = rdr.ReadToEnd();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void Run2()
        {
            Console.WriteLine("Робота з файлами");
            FileStream? fs = null;
            try
            {
                fs = new FileStream("./file1.txt", FileMode.Create, FileAccess.Write);
                fs.Write(Encoding.UTF8.GetBytes("hello world!"));
                fs.Flush();
                fs.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                fs?.Close();
            }

            try
            {
                using var rfs = new FileStream("./Files/file2.txt", FileMode.Open, FileAccess.Read);
                byte[] buf = new byte[4096];
                rfs.Read(buf, 0, 4096);
                Console.WriteLine(Encoding.UTF8.GetString(buf));
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}


/* Робота з файлами в основі має потоки (Stream)
 * Потоки дозволяють маніпулювати з одним байтом, у т.ч. 
 * одразу мати справу з масивом байтів.
 * Для запису (чи читання) інших типів даних необхідно
 * вживати заходи з їх перетворення до бінарного виду.
 * При перетворенні чисел можливі два підходи:
 *  - пряме представлення (32 біти)
 *  - рядкове представлення "2342"
 *  
 * Потоки (файлові) є некерованими ресурсами і вимагають
 * закриття подачею команди (якщо не закрити, то платформа
 * цього зробити не зможе)
 * using - auto disposable - блок з автоматичним закриттям
 * 
 * Пряма робота з потоками незручна при збереженні різних 
 * типів даних. Тому вживаються "обгортки" StreamReader\StreamWriter
 * 
 * Серіалізація (від англ. - послідовний) - спосіб представити 
 * об'єкт у вигляді послідовності, що може збережена чи передана
 * послідовним каналом. Є різні форми серіалізації: бінарна та текстова.
 * Серед текстових форм найбільш поширена - JSON
 * 
 * Д.З. Реалізувати обмеження на розміри файлів-логів. Якщо файл сягає
 * граничного розміру (0.5кБ), то створюється новий файл з додаванням
 * лічильника (runlogs1, runlogs2, ...). Дії повторюються коли наступний
 * файл також сягає граничного розміру
 */