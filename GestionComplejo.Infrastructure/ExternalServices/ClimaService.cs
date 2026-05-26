using System.Text.Json;
using System.Text.Json.Serialization;
using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Responses;
using Microsoft.Extensions.Configuration;

namespace GestionComplejo.Infrastructure.ExternalServices
{
    public class ClimaService : IClimaService
    {
        private static readonly HashSet<int> CodigosLluvia = [51, 53, 55, 61, 63, 65, 80, 81, 82];

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;
        private readonly double _latitud;
        private readonly double _longitud;

        public ClimaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _latitud = -26.82;
            _longitud = -65.22;
        }

        public async Task<bool> EsLluviosoAsync(DateTime fecha)
        {
            var url = FormattableString.Invariant($"/v1/forecast?latitude={_latitud}&longitude={_longitud}&hourly=weather_code&timezone=auto");

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var climaResponse = JsonSerializer.Deserialize<ClimaApiResponse>(json, JsonOptions)
                ?? throw new Exception("La respuesta de la API de clima no pudo ser deserializada.");

            var horaFormateada = fecha.ToString("yyyy-MM-ddTHH:00");
            var indice = climaResponse.Hourly.Time.IndexOf(horaFormateada);

            if (indice == -1)
                return false;

            var codigo = climaResponse.Hourly.WeatherCode[indice];
            return CodigosLluvia.Contains(codigo);
        }
    }
}
