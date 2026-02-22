using csharp_all.Networking.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_all.Networking.Api
{
    internal class NbuApi
    {
        public static List<NbuRate> ListFromJsonString(String json)
        {
            return [..
                JsonSerializer.Deserialize<JsonElement>(json)
                .EnumerateArray()
                .Select(NbuRate.FromJson)
                ];
        }
    }
}
