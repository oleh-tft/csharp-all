using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csharp_all.Networking.Orm
{
    internal class MoonPhase
    {
        [JsonPropertyName("phaseName")]
        public String PhaseName { get; set; } = null!;

        [JsonPropertyName("isPhaseLimit")]
        public dynamic IsPhaseLimit { get; set; } = null!;

        [JsonPropertyName("lighting")]
        public double Lighting { get; set; }

        [JsonPropertyName("svg")]
        public String Svg { get; set; } = null!;

        [JsonPropertyName("svgMini")]
        public dynamic SvgMini { get; set; } = null!;

        [JsonPropertyName("timeEvent")]
        public dynamic TimeEvent { get; set; } = null!;

        [JsonPropertyName("dis")]
        public double Distance { get; set; }

        [JsonPropertyName("dayWeek")]
        public int DayWeek { get; set; }

        [JsonPropertyName("npWidget")]
        public String NpWidget { get; set; } = null!;

        public string GetEmoji()
        {
            switch(PhaseName)
            {
                case "Waning": return "🌘";
                case "Waxing": return "🌒";
                case "Full moon": return "🌕";
                case "Last quarter": return "🌗";
                case "First quarter": return "🌓";
                case "New Moon": return "🌑";
                default: return "no emoji found for this phase 😢";
            }
        }
    }
}
