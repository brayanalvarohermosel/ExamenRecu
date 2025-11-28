
using System.Text.Json.Serialization;

namespace paisModels
{
    // JSON raíz: por ejemplo { "results": [ {...}, {...} ] }


    //Llamar directramete en el que queremos empezar

    //Cuando el objeto del Json tiene [ ] usamos list
    //Si solo tiene { } usamos un ojeto Ejemplo en poke api Info
    public class ApiResponse
    {
        [JsonPropertyName("results")]
        public List<Character> Results { get; set; } = new();
    }

    // Datos que vienen de la API
    public class Character
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("species")]
        public string Species { get; set; } = "";

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = "";
    }

    // Datos que tú guardas en tu lista
    public class SavedItem
    {
        public string Name { get; set; } = "";
        public string State { get; set; } = "";
        public string Specie { get; set; } = "";
        public string Gender { get; set; } = "";
    }
}
