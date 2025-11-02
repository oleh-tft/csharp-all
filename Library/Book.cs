using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Library
{
    public class Book : Literature
    {
        public String Author { get; set; } = null!;
        public int Year { get; set; }

        public override string GetCard()
        {
            return $"{this.Author}. {base.Title} - {base.Publisher} - {this.Year}";
        }

        [ApaStyle]
        public string ApaCard()
        {
            return $"{this.Author} ({this.Year}) {base.Title}. {base.Publisher}";
        }

        [CiteStyle(Style = "APA")]
        public void ApaCite()
        {
            Console.WriteLine($"Book APA Style Cite: {Author}. ({Year}). {Title}. {Publisher}");
        }

        [CiteStyle(Style = "IEEE")]
        public void IeeeCite()
        {
            Console.WriteLine($"Book IEEE Style Cite: {Author}, {Title}. {Publisher}, {Year}");
        }
    }
}