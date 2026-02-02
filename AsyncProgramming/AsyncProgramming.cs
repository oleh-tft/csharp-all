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
            //ProcessesDemo();
            ProcessControlDemo();
        }

        private void ProcessControlDemo()
        {
            try
            {
                Console.WriteLine("Press a key to open notepad");
                Console.ReadKey();
                Process process = Process.Start("notepad.exe");

                Console.WriteLine("Press a key to close notepad");
                Console.ReadKey();
                if (!process.HasExited) process.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Виникла помилка: {ex.Message}");
            }

            String[] browsers = { "C:\\Program Files\\Opera\\Launcher.exe", "\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\"", "\"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe\"", "C:\\Program Files\\Internet Explorer\\iexplore.exe" };

            Console.WriteLine("Press a key to open all browser");
            Console.ReadKey();

            foreach (var browser in browsers)
            {
                try
                {
                    Process process = Process.Start(browser);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Виникла помилка в шляху {browser}: {ex.Message}");
                }
            }

            const string calcFile = "calc.exe";
            try
            {
                Console.WriteLine("Press a key to open calculator");
                Console.ReadKey();
                Process process = Process.Start(calcFile);

                Console.WriteLine("Press a key to close calculator");
                Console.ReadKey();
                if (!process.HasExited) process.Kill();
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine($"Файл не знайдено: {ex}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Виникла помилка: {ex.Message}");
            }
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
