using csharp_all.Collect;
using csharp_all.Data;
using csharp_all.Dict;
using csharp_all.Events;
using csharp_all.Events.Notifier;
using csharp_all.Exceptions;
using csharp_all.Extensions;
using csharp_all.Files;
using csharp_all.Fraction;
using csharp_all.Library;
using csharp_all.Vectors;
using System;
using System.Reflection;

Console.OutputEncoding = System.Text.Encoding.Unicode;
Console.InputEncoding = System.Text.Encoding.Unicode;
try
{
    //new VectorDemo().Run();
    //new FractionDemo().Run();
    //new ExceptionsDemo().Run();
    //new FilesDemo().Run();
    //new ExtensionsDemo().Run();
    //new EventsDemo().Run();
    //new NotifierDemo().Run();
    //new CollectionsDemo().Run();
    //new DictDemo().Run();
    new DataDemo().Run();
}
catch (Exception ex)
{
    Console.WriteLine("Не оброблений у програмі виняток: " + ex.ToString());
}

void ShowReflection()
{
    /*
     * Рефлексія (в ООП) - інструментаріій мови\платформи, який
     * дозволяє одержувати відомості про склад типу даних
     */
    Type bookType = typeof(Book);
    FieldInfo[] fields = bookType.GetFields();
    PropertyInfo[] properties = bookType.GetProperties();
    MethodInfo[] methods = bookType.GetMethods();
    EventInfo[] events = bookType.GetEvents();

    if (fields.Length > 0)
    {
        Console.WriteLine("Type 'Book' has fields:");
        foreach (var field in fields)
        {
            Console.WriteLine(field.Name);
        }
    }
    else
    {
        Console.WriteLine("Type 'Book' has no fields");
    }

    if (properties.Length > 0)
    {
        Console.WriteLine("\nType 'Book' has props:");
        foreach (var prop in properties)
        {
            Console.WriteLine("{0}: {1}", prop.Name, prop.PropertyType.Name);
        }
    }
    else
    {
        Console.WriteLine("\nType 'Book' has no props");
    }

    if (methods.Length > 0)
    {
        Console.WriteLine("\nType 'Book' has methods:");
        foreach (var method in methods)
        {
            Console.WriteLine(method.Name);
        }
    }
    else
    {
        Console.WriteLine("\nType 'Book' has no methods");
    }

    if (events.Length > 0)
    {
        Console.WriteLine("\nType 'Book' has events:");
        foreach (var _event in events)
        {
            Console.WriteLine(_event.Name);
        }
    }
    else
    {
        Console.WriteLine("\nType 'Book' has no events");
    }
    /*
     * Рефлексія за об'єктом
     */
    Console.WriteLine("\n\n--------Рефлексія за об'єктом----------");
    Literature j = new Journal()
    {
        Title = "ArgC & ArgC",
        Number = "2(113), 2000",
        Publisher = "https://journals.ua/technology/argc-argv"
    };
    Type jType = j.GetType();
    Console.WriteLine(jType.Name); // Journal - змінна типізується за об'єктом (а не за оголошенням) 
    PropertyInfo? jProp = jType.GetProperty("Number");
    if (jProp != null)
    {
        // prop - відомості про тип даних, а не про об'єкт
        var number = jProp.GetValue(j);
        Console.WriteLine($"Object has 'Number' property with value '{number}'");
    }
    else
    {
        Console.WriteLine("Object has no 'Number' property");
    }

    Library library = new();
    Console.WriteLine("-------printable-------");
    library.ShowPrintable();
    Console.WriteLine("-------color printable-------");
    library.ShowColorPrintable();
    Console.WriteLine("\n-------apa style------");
    library.ShowApaCard();
    Console.WriteLine("\n-------apa style cite------");
    library.PrintApaCards();
    Console.WriteLine("\n-------ieee style cite------");
    library.PrintIeeeCards();

}

void ShowLibrary()
{
    Library library = new();
    library.PrintCatalog();
    Console.WriteLine("\n-----Periodic-----");
    library.PrintPeriodic();
    Console.WriteLine("\n-----NonPeriodic-----");
    library.PrintNonPeriodic();
    Console.WriteLine("\n-------Printable-------");
    library.PrintPrintable();
    Console.WriteLine("\n-----NonPrintable------");
    library.PrintNonPrintable();
    Console.WriteLine("------------------");
}