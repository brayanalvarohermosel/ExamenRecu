using System.Text.Json.Serialization;

namespace paisModels.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("results")]
        public List<Country> Results { get; set; } = new();
    }
    public class Country
    {
        public string Nombre { get; set; } = "";
        public string Capital { get; set; } = "";
        public string Region { get; set; } = "";
        public long Poblacion { get; set; } = 0;
    }
}
