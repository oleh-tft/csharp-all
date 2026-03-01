using csharp_all.Networking.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_all.Networking.Api
{
    internal class MoonApi
    {

        public async Task<MoonPhase> TodayPhaseAsync()
        {
            int day = DateTime.Now.Day;
            var moonApiResponse = await FetchDateAsync(DateTime.Now.Year, DateTime.Now.Month, day);
            return moonApiResponse.Phase[day.ToString()];
        }

        public async Task<MoonPhase> PhaseByDate(DateOnly date)
        {
            var moonApiResponse = await FetchDateAsync(date.Year, date.Month, date.Day);
            return moonApiResponse.Phase[date.Day.ToString()];
        }

        private async Task<MoonApiResponse> FetchDateAsync(int year, int month, int day)
        {
            using HttpClient httpClient = new();
            String href = $"https://www.icalendar37.net/lunar/api/?year={year}&month={month}&day={day}&shadeColor=gray&size=150&texturize=true";
            return JsonSerializer.Deserialize<MoonApiResponse>(await httpClient.GetStringAsync(href))!;
        }

    }
}
