using System.Text.Json.Serialization;

namespace GestionComplejo.Application.Responses
{
    public class ClimaApiResponse
    {
        public HourlyData Hourly { get; set; } = null!;
    }

    public class HourlyData
    {
        public List<string> Time { get; set; } = [];

        [JsonPropertyName("weather_code")]
        public List<int> WeatherCode { get; set; } = [];
    }
}
