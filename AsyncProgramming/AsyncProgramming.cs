using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.AsyncProgramming
{
    internal class AsyncProgramming
    {
        public void Run()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.WriteLine("Async Programming: select an action");
                Console.WriteLine("1. Processes list");
                Console.WriteLine("2. Start notepad");
                Console.WriteLine("3. Process with params");
                Console.WriteLine("4. Thread demo");
                Console.WriteLine("5. Multi Thread demo (percent)");
                Console.WriteLine("6. Multi Thread Array");
                Console.WriteLine("0. Exit program");
                keyInfo = Console.ReadKey();
                Console.WriteLine();
                switch (keyInfo.KeyChar)
                {
                    case '0': return;
                    case '1': ProcessesDemo(); break;
                    case '2': ProcessControlDemo(); break;
                    case '3': ProcessWithParam(); break;
                    case '4': ThreadsDemo(); break;
                    case '5': MultiThread(); break;
                    case '6': MultiThreadArray(); break;
                    default: Console.WriteLine("Wrong choice"); break;
                }
            } while (true);
        }

        private int threadCntArr;
        private int[] randomArr;
        private readonly Object cntArrLocker = new();

        private void MultiThreadArray()
        {
            Console.Write("Введіть кількість елементів для генерування: ");
            int amount = int.Parse(Console.ReadLine()!);
            randomArr = Array.Empty<int>();
            threadCntArr = amount;
            for (int i = 0; i < amount; i++)
            {
                new Thread(AppendArray).Start(i);
            }
        }

        private void AppendArray(Object? element)
        {
            int m = (int)element!;
            Console.WriteLine($"Request sent for element {m}");
            Thread.Sleep(1000);
            Random random = new();
            int generated = random.Next(100);
            lock (randomArr)
            {
                randomArr = randomArr.Append(generated).ToArray();

                Console.Write("[");
                for (int i = 0; i < randomArr.Length; i++)
                {
                    Console.Write(i == randomArr.Length - 1 ? randomArr[i] : randomArr[i] + ", ");
                }
                    
                Console.WriteLine("]");
            }

            bool isCnt0 = false;
            lock (cntArrLocker)
            {
                threadCntArr -= 1;
                if (threadCntArr == 0)
                {
                    isCnt0 = true;
                }
            }
            if (isCnt0)
            {
                Console.Write("Результат: [");
                for (int i = 0; i < randomArr.Length; i++)
                {
                    Console.Write(i == randomArr.Length - 1 ? randomArr[i] : randomArr[i] + ", ");
                }

                Console.WriteLine("]");
            }
        }

        private void MultiThread()
        {
            sum = 100.0;
            threadCnt = 12;
            for (int i = 0; i < 12; i++)
            {
                new Thread(CalcMonth).Start(i + 1);
            }
        }

        private void CalcMonth3(Object? month)
        {
            int m = (int)month!;
            double res;
            Console.WriteLine($"Request sent for month {m}");
            Thread.Sleep(1000);
            double percent = m;
            double k = (1.0 + percent / 100.0);
            lock (sumLocker)
            {
                res = sum *= k;
            }
            Console.WriteLine($"Response got for month {m} sum = {res}");
        }

        private void CalcMonth2(Object? month)
        {
            int m = (int)month!;
            lock (sumLocker)
            {
                double res = sum;
                Console.WriteLine($"Request sent for month {m}");
                Thread.Sleep(1000);
                double percent = m;
                res = res * (1.0 + percent / 100.0);
                sum = res;
                Console.WriteLine($"Response got for month {m} sum = {sum}");
            }
        }

        private void CalcMonth1(Object? month)
        {
            int m = (int)month!;
            double res = sum;
            Console.WriteLine($"Request sent for month {m}");
            Thread.Sleep(1000);
            double percent = m;
            res = res * (1.0 + percent / 100.0);
            sum = res;
            Console.WriteLine($"Response got for month {m} sum = {sum}");
        }

        private readonly Object sumLocker = new();
        private readonly Object cntLocker = new();
        private double sum;
        private int threadCnt;

        private void CalcMonth(Object? month)
        {
            int m = (int)month!;
            double res;
            Console.WriteLine($"Request sent for month {m}");
            Thread.Sleep(1000);
            double percent = m;
            double k = (1.0 + percent / 100.0);
            lock (sumLocker)
            {
                res = sum *= k;
            }
            Console.WriteLine($"Response got for month {m} sum = {res}");

            bool isCnt0 = false;
            lock (cntLocker)
            {
                threadCnt -= 1;
                if (threadCnt == 0)
                {
                    isCnt0 = true;
                }
            }
            if (isCnt0)
            {
                Console.WriteLine($"Result for year: {sum:F2}");
            }
        }

        private void ThreadsDemo()
        {
            Console.WriteLine("Thread created");
            var t = new Thread(ThreadActivity);
            Console.WriteLine("Thread start");
            t.Start();
        }

        private void ThreadActivity()
        {
            Console.WriteLine("ThreadActivity start");
            Thread.Sleep(3000);
            Console.WriteLine("ThreadActivity stop");
        }

        private void ProcessWithParam()
        {
            try
            {
                var p = Process.Start(new ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    Arguments = Path.Combine(".", "AsyncProgramming", "demo.txt"),
                    UseShellExecute = true,
                    WorkingDirectory = Directory.GetCurrentDirectory()
                });
                Console.WriteLine("Press a key");
                Console.ReadKey();
                p.CloseMainWindow();
                p.Kill(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            String[] browsers = { "C:\\Program Files\\Opera\\Launcher.exe", "\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\"", "\"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe\"", "C:\\Program Files\\Internet Explorer\\iexplore.exe" };

            Console.WriteLine("Press a key to open all browser");
            Console.ReadKey();

            foreach (var browser in browsers)
            {
                try
                {
                    var p = Process.Start(new ProcessStartInfo
                    {
                        FileName = browser,
                        Arguments = "github.com"
                    });
                    Console.WriteLine("Press a key");
                    Console.ReadKey();
                    p.CloseMainWindow();
                    p.Kill(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Виникла помилка в шляху {browser}: {ex.Message}");
                }
            }

            try
            {
                var p = Process.Start(new ProcessStartInfo
                {
                    FileName = @"C:\Program Files\Windows Media Player\wmplayer.exe",
                    Arguments = Path.Combine(".", "AsyncProgramming", "video.mp4"),
                    UseShellExecute = true,
                    WorkingDirectory = Directory.GetCurrentDirectory()
                });
                Console.WriteLine("Press a key");
                Console.ReadKey();
                p.CloseMainWindow();
                p.Kill(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ProcessControlDemo()
        {
            try
            {
                Console.WriteLine("Press a key to open notepad");
                Console.ReadKey();
                Process process = Process.Start("notepad.exe");

                //Console.WriteLine("Press a key to close notepad");
                //Console.ReadKey();
                //if (!process.HasExited) process.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Виникла помилка: {ex.Message}");
            }

            //String[] browsers = { "C:\\Program Files\\Opera\\Launcher.exe", "\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\"", "\"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe\"", "C:\\Program Files\\Internet Explorer\\iexplore.exe" };

            //Console.WriteLine("Press a key to open all browser");
            //Console.ReadKey();

            //foreach (var browser in browsers)
            //{
            //    try
            //    {
            //        Process process = Process.Start(browser);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Виникла помилка в шляху {browser}: {ex.Message}");
            //    }
            //}

            //const string calcFile = "calc.exe";
            //try
            //{
            //    Console.WriteLine("Press a key to open calculator");
            //    Console.ReadKey();
            //    Process process = Process.Start(calcFile);

            //    //Console.WriteLine("Press a key to close calculator");
            //    //Console.ReadKey();
            //    //if (!process.HasExited) process.Kill();
            //}
            //catch (Win32Exception ex)
            //{
            //    Console.WriteLine($"Файл не знайдено: {ex}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Виникла помилка: {ex.Message}");
            //}
        }

        private void ProcessesDemo()
        {
            Process[] processes = Process.GetProcesses();
            Dictionary<string, int> uniqueNames = new();
            foreach (var process in processes)
            {
                if (uniqueNames.ContainsKey(process.ProcessName))
                {
                    uniqueNames[process.ProcessName]++;
                } 
                else
                {
                    uniqueNames.Add(process.ProcessName, 1);
                }
            }
            foreach (var pair in uniqueNames.OrderByDescending(p => p.Value).ThenBy(p => p.Key))
            {
                Console.WriteLine($"{pair.Key} --- (x{pair.Value})");
            }
        }
    } 
}
