using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "977513fc5b2f69b1ad73bb8f90ddfff7";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&appid={chave}&units=metric&lang=pt_br";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    // Verifica se o token existe antes de converter
                    double sunriseSeconds = rascunho["sys"]?["sunrise"]?.Value<double>() ?? 0;
                    double sunsetSeconds = rascunho["sys"]?["sunset"]?.Value<double>() ?? 0;

                    DateTime baseTime = DateTime.UnixEpoch;
                    DateTime sunrise = baseTime.AddSeconds(sunriseSeconds).ToLocalTime();
                    DateTime sunset = baseTime.AddSeconds(sunsetSeconds).ToLocalTime();

                    t = new()
                    {
                        lat = rascunho?["coord"]?["lat"]?.Value<double>() ?? 0,
                        lon = rascunho?["coord"]?["lon"]?.Value<double>() ?? 0,
                        description = rascunho?["weather"]?[0]?["description"]?.Value<string>() ?? "",
                        main = rascunho?["weather"]?[0]?["main"]?.Value<string>() ?? "",
                        temp_min = rascunho?["main"]?["temp_min"]?.Value<double>() ?? 0,
                        temp_max = rascunho?["main"]?["temp_max"]?.Value<double>() ?? 0,
                        visibility = rascunho?["visibility"]?.Value<int>() ?? 0,
                        speed = rascunho?["wind"]?["speed"]?.Value<double>() ?? 0,
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString()
                    };
                }

                return t;
            }
        }
    }
}
