using System;
using System.Collections.Generic;
using System.IO;

namespace Hotel.Modelos
{
    // Clase principal del hotel que administra la coleccion de reservas en memoria y la persistencia en archivo
    public class Hotel
    {
        // Nombre del hotel
        public string Nombre { get; private set; }

        // Coleccion de reservas almacenadas en memoria
        private List<Reserva> _reservas;

        // Contador de IDs autoincrementable
        private int    _ultimoId;

        // Ruta del archivo de texto donde se guardan las reservas
        private string _archivoPath;

        // Constructor: inicializa el hotel y carga las reservas previas desde el archivo
        public Hotel(string nombre, string archivoPath = "reservas.txt")
        {
            Nombre       = nombre;
            _archivoPath = archivoPath;
            _reservas    = new List<Reserva>();
            _ultimoId    = 0;

            CargarDesdeArchivo();
        }

        // Valida los datos recibidos, crea la reserva, la agrega a la lista en memoria y la guarda en archivo
        public Reserva RegistrarReserva(string cliente, int habitacion,
                                        DateTime fechaIngreso, int noches,
                                        double precioPorNoche)
        {
            ValidarDatos(cliente, habitacion, noches, precioPorNoche, fechaIngreso);

            Reserva nueva = new Reserva
            {
                Cliente        = cliente,
                Habitacion     = habitacion,
                FechaIngreso   = fechaIngreso,
                Noches         = noches,
                PrecioPorNoche = precioPorNoche
            };

            _ultimoId++;
            _reservas.Add(nueva);
            GuardarEnArchivo(_ultimoId, nueva);

            return nueva;
        }

        // Devuelve la lista completa de reservas registradas
        public List<Reserva> ObtenerTodas()
        {
            return _reservas;
        }

        // Recorre la coleccion y suma el total a pagar de cada reserva
        public double CalcularIngresoTotal()
        {
            double total = 0;
            foreach (Reserva r in _reservas)
                total += r.CalcularTotal();
            return total;
        }

        // Recorre la lista con un for y retorna la reserva con mayor cantidad de noches
        // Retorna null si no hay reservas registradas
        public Reserva ObtenerReservaMayorDuracion()
        {
            if (_reservas.Count == 0)
                return null;

            Reserva mayor = _reservas[0];
            for (int i = 1; i < _reservas.Count; i++)
            {
                if (_reservas[i].Noches > mayor.Noches)
                    mayor = _reservas[i];
            }
            return mayor;
        }

        // Lee el archivo linea por linea y reconstruye la lista de reservas al iniciar la aplicacion
        private void CargarDesdeArchivo()
        {
            if (!File.Exists(_archivoPath))
                return;

            foreach (string linea in File.ReadAllLines(_archivoPath))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;

                var (id, reserva) = Reserva.FromLinea(linea);
                _reservas.Add(reserva);

                // Sincroniza el contador de IDs con el ultimo ID leido
                if (id > _ultimoId)
                    _ultimoId = id;
            }
        }

        // Agrega una reserva al final del archivo en modo append sin sobreescribir las anteriores
        private void GuardarEnArchivo(int id, Reserva reserva)
        {
            File.AppendAllText(_archivoPath, reserva.ToLinea(id) + Environment.NewLine);
        }

        // Verifica que todos los datos sean validos antes de registrar la reserva
        // Lanza ArgumentException si algun dato no cumple las reglas
        private static void ValidarDatos(string cliente, int habitacion,
                                         int noches, double precio, DateTime fechaIngreso)
        {
            if (string.IsNullOrWhiteSpace(cliente))
                throw new ArgumentException("El nombre del cliente no puede estar vacio.");
            if (habitacion <= 0)
                throw new ArgumentException("El numero de habitacion debe ser mayor a 0.");
            if (noches <= 0)
                throw new ArgumentException("El numero de noches debe ser mayor a 0.");
            if (precio <= 0)
                throw new ArgumentException("El precio por noche debe ser mayor a 0.");
            if (fechaIngreso.Date < DateTime.Today)
                throw new ArgumentException("La fecha de ingreso no puede ser en el pasado.");
        }
    }
}
