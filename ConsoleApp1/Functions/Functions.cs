using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using paisModels.Models;
using PaisApi.Services;

namespace Paises.Functions
{
    public static class Funciones
    {
        public static async Task buscarPais(PaisService service, List<Country> paisesGuardados)
        {
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrEmpty(nombre))
            {
                Console.WriteLine("Nombre vacío.");
                return;
            }

            var pais = await service.GetCountryAsync(nombre);
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
            }
            Console.Write("¿Quieres guardarlo en tu lista? (s/n): ");
            if (Console.ReadLine()?.Trim().ToLower() == "s")
            {
                paisesGuardados.Add(pais);
                Console.WriteLine("País guardado.");
            }
        }
        public static void ListarPaises(List<Country> paisesGuardados)
        {
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
                    Console.WriteLine($"{i + 1}. {p.Nombre} - Capital: {p.Capital}, Región: {p.Region}, Población: {p.Poblacion}");
                }
            }
        }

        public static async Task guardarPaises(List<Country> paisesGuardados)
        {
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
        }
    }
}