using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

HttpClient client = new HttpClient();

List<Country> paisesGuardados = new();

async Task<Country?> GetCountryAsync(string name)
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

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Buscar país por nombre");
    Console.WriteLine("2. Listar mis países guardados");
    Console.WriteLine("3. Guardar lista en archivo JSON");
    Console.WriteLine("4. Salir");

    Console.Write("Elige una opción: ");
    var respuesta = Console.ReadLine()?.Trim();

    switch (respuesta)
    {
        case "1":
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrEmpty(nombre)) { Console.WriteLine("Nombre vacío."); continue; }

            var pais = await GetCountryAsync(nombre);
            if (pais == null)
            {
                Console.WriteLine("País no encontrado.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"Nombre: {pais.Nombre}");
                Console.WriteLine($"Capital: {pais.Capital}");
                Console.WriteLine($"Región: {pais.Region}");
                Console.WriteLine($"Población: {pais.Poblacion}");
                Console.WriteLine();

                Console.Write("¿Quieres guardarlo en tu lista? (s/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "s")
                {
                    paisesGuardados.Add(pais);
                    Console.WriteLine("País guardado.");
                }
            }
            break;
        case "2":
            if (paisesGuardados.Count == 0)
            {
                Console.WriteLine("No tienes países guardados todavía.");
            }
            else
            {
                Console.WriteLine("=== MIS PAÍSES ===");
                for (int i = 0; i < paisesGuardados.Count; i++)
                {
                    var p = paisesGuardados[i];
                    Console.WriteLine($"{i + 1}. {p.Nombre} ({p.Capital}) - {p.Region} - {p.Poblacion} hab.");
                }
            }
            break;
        case "3":
            if (paisesGuardados.Count == 0)
            {
                Console.WriteLine("No tienes países guardados para exportar.");
            }
            else
            {
                var json = JsonSerializer.Serialize(paisesGuardados, new JsonSerializerOptions { WriteIndented = true });
                var filename = "mis_paises.json";
                await System.IO.File.WriteAllTextAsync(filename, json);
                Console.WriteLine($"Lista guardada en {filename}");
            }
            break;
        case "4":
            Console.WriteLine("Adiós.");
            return;
        default:
            Console.WriteLine("Opción no válida.");
            continue;
    }
}

class Country
{
    public string Nombre { get; set; } = "";
    public string Capital { get; set; } = "";
    public string Region { get; set; } = "";
    public long Poblacion { get; set; } = 0;
}
