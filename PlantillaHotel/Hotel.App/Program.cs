using System;
using System.Collections.Generic;
using Hotel.Modelos;

namespace Hotel.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Inicializar hotel (carga reservas previas desde archivo)
            Hotel.Modelos.Hotel hotel = new Hotel.Modelos.Hotel("Hotel Gran Vista", "reservas.txt");

            bool ejecutar = true;

            while (ejecutar)
            {
                MostrarMenu();
                string opcion = (Console.ReadLine() ?? "").Trim();

                switch (opcion)
                {
                    case "1": RegistrarReserva(hotel);           break;
                    case "2": ListarReservas(hotel);             break;
                    case "3": MostrarIngresoTotal(hotel);        break;
                    case "4": MostrarMayorDuracion(hotel);       break;
                    case "5":
                        ejecutar = false;
                        Escribir("\n  ¡Hasta luego!\n", ConsoleColor.Cyan);
                        break;
                    default:
                        Escribir("  Opcion invalida. Intenta de nuevo.", ConsoleColor.Red);
                        break;
                }
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // MENU
        // ════════════════════════════════════════════════════════════════════
        static void MostrarMenu()
        {
            Console.WriteLine();
            Escribir("╔══════════════════════════════════════════╗", ConsoleColor.Yellow);
            Escribir("║       SISTEMA DE RESERVAS - HOTEL        ║", ConsoleColor.Yellow);
            Escribir("╚══════════════════════════════════════════╝", ConsoleColor.Yellow);
            Console.WriteLine("  1. Registrar nueva reserva");
            Console.WriteLine("  2. Listar todas las reservas");
            Console.WriteLine("  3. Calcular ingreso total esperado");
            Console.WriteLine("  4. Mostrar reserva de mayor duracion");
            Console.WriteLine("  5. Salir");
            Console.Write("\n  Selecciona una opcion: ");
        }

        // ════════════════════════════════════════════════════════════════════
        // OPCION 1 — Registrar reserva
        // ════════════════════════════════════════════════════════════════════
        static void RegistrarReserva(Hotel.Modelos.Hotel hotel)
        {
            Titulo("REGISTRAR NUEVA RESERVA");
            try
            {
                string cliente = LeerTexto("Nombre del cliente     ");

                Console.Write("  Numero de habitacion  : ");
                int habitacion = int.Parse(Console.ReadLine());

                Console.Write("  Fecha de ingreso (YYYY-MM-DD): ");
                DateTime fecha = DateTime.Parse(Console.ReadLine());

                Console.Write("  Numero de noches      : ");
                int noches = int.Parse(Console.ReadLine());

                Console.Write("  Precio por noche (Q)  : ");
                double precio = double.Parse(Console.ReadLine());

                Reserva nueva = hotel.RegistrarReserva(cliente, habitacion, fecha, noches, precio);

                Escribir("\n  ✔ Reserva registrada correctamente:", ConsoleColor.Green);
                ImprimirReserva(nueva);
            }
            catch (FormatException)
            {
                Escribir("  ✘ Error: ingresa los datos en el formato correcto.", ConsoleColor.Red);
            }
            catch (ArgumentException ex)
            {
                Escribir($"  ✘ {ex.Message}", ConsoleColor.Red);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // OPCION 2 — Listar reservas
        // ════════════════════════════════════════════════════════════════════
        static void ListarReservas(Hotel.Modelos.Hotel hotel)
        {
            Titulo("LISTA DE RESERVAS");
            List<Reserva> lista = hotel.ObtenerTodas();

            if (lista.Count == 0)
            {
                Escribir("  No hay reservas registradas.", ConsoleColor.DarkYellow);
                return;
            }

            for (int i = 0; i < lista.Count; i++)
                ImprimirReserva(lista[i]);

            Console.WriteLine($"\n  Total de reservas registradas: {lista.Count}");
        }

        // ════════════════════════════════════════════════════════════════════
        // OPCION 3 — Ingreso total
        // ════════════════════════════════════════════════════════════════════
        static void MostrarIngresoTotal(Hotel.Modelos.Hotel hotel)
        {
            Titulo("INGRESO TOTAL ESPERADO");

            if (hotel.ObtenerTodas().Count == 0)
            {
                Escribir("  No hay reservas. Ingreso total: Q 0.00", ConsoleColor.DarkYellow);
                return;
            }

            double total = hotel.CalcularIngresoTotal();
            Escribir($"\n  Ingreso total por todas las reservas: Q {total:N2}\n", ConsoleColor.Green);
        }

        // ════════════════════════════════════════════════════════════════════
        // OPCION 4 — Mayor duracion
        // ════════════════════════════════════════════════════════════════════
        static void MostrarMayorDuracion(Hotel.Modelos.Hotel hotel)
        {
            Titulo("RESERVA DE MAYOR DURACION");
            Reserva mayor = hotel.ObtenerReservaMayorDuracion();

            if (mayor == null)
            {
                Escribir("  No hay reservas registradas.", ConsoleColor.DarkYellow);
                return;
            }

            Escribir($"  La reserva mas larga tiene {mayor.Noches} noches:", ConsoleColor.Magenta);
            ImprimirReserva(mayor);
        }

        // ════════════════════════════════════════════════════════════════════
        // HELPERS
        // ════════════════════════════════════════════════════════════════════
        static void ImprimirReserva(Reserva r)
        {
            Console.WriteLine();
            Console.WriteLine($"  ┌──────────────────────────────────────────");
            Console.WriteLine($"  │  Cliente       : {r.Cliente}");
            Console.WriteLine($"  │  Habitacion    : {r.Habitacion}");
            Console.WriteLine($"  │  Ingreso       : {r.FechaIngreso:dd/MM/yyyy}");
            Console.WriteLine($"  │  Egreso        : {r.FechaEgreso:dd/MM/yyyy}");
            Console.WriteLine($"  │  Noches        : {r.Noches}");
            Console.WriteLine($"  │  Precio/noche  : Q {r.PrecioPorNoche:N2}");
            Escribir(          $"  └─ TOTAL         : Q {r.CalcularTotal():N2}", ConsoleColor.Cyan);
        }

        static void Titulo(string texto)
        {
            Console.WriteLine();
            Escribir($"  ── {texto} ──────────────────────", ConsoleColor.Magenta);
        }

        static string LeerTexto(string prompt)
        {
            Console.Write($"  {prompt}: ");
            return (Console.ReadLine() ?? "").Trim();
        }

        static void Escribir(string texto, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(texto);
            Console.ResetColor();
        }
    }
}
