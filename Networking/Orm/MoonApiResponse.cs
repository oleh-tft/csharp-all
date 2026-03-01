using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csharp_all.Networking.Orm
{
    internal class MoonApiResponse
    {
        [JsonPropertyName("phase")]
        public Dictionary<String, MoonPhase> Phase { get; set; } = [];
    }
}
