using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csharp_all.Library
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(Book), typeDiscriminator: "Book")]
    [JsonDerivedType(typeof(Booklet), typeDiscriminator: "Booklet")]
    [JsonDerivedType(typeof(Hologram), typeDiscriminator: "Hologram")]
    [JsonDerivedType(typeof(Journal), typeDiscriminator: "Journal")]
    [JsonDerivedType(typeof(Newspaper), typeDiscriminator: "Newspaper")]
    [JsonDerivedType(typeof(Poster), typeDiscriminator: "Poster")]
    public abstract class Literature
    {
        public String Publisher { get; set; } = String.Empty;
        public String Title { get; set; } = null!;

        public abstract String GetCard();

    }
}

/*
 * CTS - Common Type System - система спільних типів
 * 
 * 
 */