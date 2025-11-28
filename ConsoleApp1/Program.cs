using PaisApi.Services;
using paisModels.Models;
using Paises.Functions;

namespace ApiDePaises
{
    class Program
    {
        static async Task Main(string[] args)
        {
            PaisService service = new PaisService();
            List<Country> paisesGuardados = new List<Country>();
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
                        await Funciones.buscarPais(service, paisesGuardados);
                        break;
                    case "2":
                        Funciones.ListarPaises(paisesGuardados);
                        break;
                    case "3":
                        await Funciones.guardarPaises(paisesGuardados);
                        break;
                    case "4":
                        Console.WriteLine("Adiós.");
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        continue;
                }
            }
        }
    }

}
