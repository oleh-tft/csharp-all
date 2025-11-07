using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csharp_all.Library
{
    public class Library
    {
        [JsonInclude]
        private List<Literature> Funds { get; set; } = [];

        public Library()
        {
            
        }

        public void Init()
        {
            Funds.Add(new Book()
            {
                Author = "D. Knuth",
                Title = "The art of programming",
                Publisher = "Київ, Наукова Думка",
                Year = 2000
            });
            Funds.Add(new Book()
            {
                Author = "J. Richter",
                Title = "CLR via C#",
                Publisher = "Microsoft Press",
                Year = 2013
            });
            Funds.Add(new Journal()
            {
                Title = "ArgC & ArgV",
                Number = "2(113)",
                Publisher = "https://journals.ua/technology/argc-argv/",
                Year = 2000
            });
            Funds.Add(new Newspaper()
            {
                Title = "Gazette de Leopol",
                Publisher = "First Ukrainian press in Ukraine",
                Date = new DateOnly(1776, 1, 15)
            });
            Funds.Add(new Booklet()
            {
                Title = "Безпечний інтернет для дітей",
                Publisher = "ГО «Центр цифрової грамотності»",
                Year = 2024,
                Organization = "Міністерство освіти і науки України"
            });
            Funds.Add(new Hologram()
            {
                Title = "Скіфське мистецтво",
                ArtItem = "Пектораль",
                Publisher = "Студія 'Лазер'"

            });
            Funds.Add(new Poster()
            {
                Title = "Неонова Ніч",
                Publisher = "Mistral Films & Black Forest Productions",
                Date = new DateOnly(2025, 11, 1)
            });
        }

        public void PrintCatalog()
        {
            foreach (Literature literature in Funds)
            {
                Console.WriteLine(literature.GetCard());
            }
        }

        public void PrintPeriodic()
        {
            foreach (Literature literature in Funds)
            {
                if (literature is IPeriodic lit)
                {
                    Console.WriteLine($"Раз у {lit.GetPeriod()}: ");
                    Console.WriteLine(literature.GetCard());
                }
            }
        }

        public void PrintNonPeriodic()
        {
            foreach (Literature literature in Funds)
            {
                if (literature is not IPeriodic)
                {
                    Console.WriteLine(literature.GetCard());
                }
            }
        }

        public void PrintPrintable()
        {
            foreach (Literature literature in Funds)
            {
                if (literature is not INonPrintable)
                {
                    Console.WriteLine(literature.GetCard());
                }
            }
        }

        public void PrintNonPrintable()
        {
            foreach (Literature literature in Funds)
            {
                if (literature is INonPrintable)
                {
                    Console.WriteLine(literature.GetCard());
                }
            }
        }

        public void ShowColorPrintable()
        {
            //Пошук за атрибутами - метаданими, що супроводжують методи
            foreach (Literature literature in Funds)
            {
                foreach (var method in literature.GetType().GetMethods())
                {
                    var attr = method.GetCustomAttribute<ColorPrintAttribute>();
                    if (attr != null)
                    {
                        //вилучення додаткових даних з атрибуту
                        for (int i = 0; i < attr.Copies; i++)
                        {
                            method.Invoke(literature, ["RGB"]);
                        }
                    }
                }
            }
        }

        public void ShowApaCard()
        {
            foreach (Literature literature in Funds)
            {
                foreach (var method in literature.GetType().GetMethods())
                {
                    var attr = method.GetCustomAttribute<ApaStyleAttribute>();
                    if (attr != null)
                    {
                        Console.WriteLine(method.Invoke(literature, null));
                    }
                }
            }
        }

        public void ShowPrintable()
        {
            foreach (Literature literature in Funds)
            {
                MethodInfo? printMethod = literature.GetType().GetMethod("Print");
                if (printMethod != null)
                {
                    printMethod.Invoke(literature, null);
                }
            }
        }

        public void PrintApaCards()
        {
            foreach (Literature literature in Funds)
            {
                foreach (var method in literature.GetType().GetMethods())
                {
                    var attr = method.GetCustomAttribute<CiteStyleAttribute>();
                    if (attr != null)
                    {
                        if (attr.Style.Equals("APA"))
                        {
                            method.Invoke(literature, null);
                        }
                    }
                }
            }
        }

        public void PrintIeeeCards()
        {
            foreach (Literature literature in Funds)
            {
                foreach (var method in literature.GetType().GetMethods())
                {
                    var attr = method.GetCustomAttribute<CiteStyleAttribute>();
                    if (attr != null)
                    {
                        if (attr.Style.Equals("IEEE"))
                        {
                            method.Invoke(literature, null);
                        }
                    }
                }
            }
        }
    }
}