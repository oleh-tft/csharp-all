using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Exceptions
{
    internal class ExceptionsDemo
    {
        public ExceptionsDemo()
        {
            return;
        }

        public void Run()
        {
            try
            {
                Console.Write("Enter a number: ");
                String str = Console.ReadLine()!;
                Console.WriteLine("Sqrt of {0} = {1}", str, SqrtFromString(str));
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Аргумент не може бути перетворений до числа");
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
            {
                Console.WriteLine("Зафіксовано NULL або порожній рядок в аргументі");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Від'ємні числа не підтримуються");
            }
            finally
            {
                Run();
            }
        }

        public void Run1()
        {
            Console.WriteLine("Виняткові ситуації. Винятки. Exceptions");
            // this.ThrowableCode();
            try
            {
                this.ThrowableCode();
            }
            catch(ApplicationException ex)
            {
                Console.WriteLine("Виникла виняткова ситуація " +  ex.Message); 
            }
            catch (IOException)
            {
                Console.WriteLine("Unexpected exception");
            }
            catch
            {
                throw;
            }
            finally
            {
                Console.WriteLine("Finally actions");
            }
        }

        public void ThrowableCode()
        {
            //throw new ApplicationException("Launch of ThrowableCode");
            throw new LiteratureParseException("Unrecognized literature type");
        }

        private double SqrtFromString(String str)
        {
            ArgumentNullException.ThrowIfNull(str);
            //if (str == null)
            //{
            //    throw new ArgumentNullException(nameof(str));
            //}
            str = str.Trim();
            if (str == String.Empty)
            {
                throw new ArgumentException("Blank or empty data passed");
            }
            double result;
            try
            {
                result = Double.Parse(str);
            }
            catch
            {
                throw new ArgumentOutOfRangeException(nameof(str), "Argument 'str' must be valid float number");
            }
            if (result < 0)
            {
                throw new InvalidOperationException("Negative values unsupported");
            }
            return Math.Sqrt(result);
        }
    }
}

/*
 * Винятки (Exceptions)
 * Спосіб організації процесу виконання коду за якого процес може бути
 * зупинений та переведений до режиму оброблення винятку.
 * 
 * У ранніх мовах програмування винятків не було, перевірка помилок
 * здійснювалась через запит спец. функций
 */