using System.Net.Http;
using System.Text.Json;
using paisModels.Models;

namespace PaisApi.Services
{
    public class PaisService
    {
        private readonly HttpClient client = new HttpClient();

        List<Country> paisesGuardados = new();

        public async Task<Country?> GetCountryAsync(string name)
        {
            try
            {
                var url = $"https://restcountries.com/v3.1/name/{Uri.EscapeDataString(name)}";
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;

                using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var root = doc.RootElement;
                if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0) return null;

                var resultado = root[0];

                string nombre = resultado.GetProperty("name").GetProperty("common").GetString() ?? "";
                string capital = "";

                if (resultado.TryGetProperty("capital", out var cap) && cap.ValueKind == JsonValueKind.Array && cap.GetArrayLength() > 0) capital = cap[0].GetString() ?? "";
                string region = resultado.TryGetProperty("region", out var reg) ? reg.GetString() ?? "" : "";
                long poblacion = resultado.TryGetProperty("population", out var pop) && pop.ValueKind == JsonValueKind.Number ? pop.GetInt64() : 0L;

                return new Country
                {
                    Nombre = nombre,
                    Capital = capital,
                    Region = region,
                    Poblacion = poblacion
                };
            }
            catch
            {
                return null;
            }
        }
        
    }
}