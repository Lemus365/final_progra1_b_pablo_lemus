namespace Hotel.Modelos
{
    /// <summary>
    /// Clase Hotel: administra la coleccion de reservas en memoria
    /// y la persistencia en archivo de texto.
    /// </summary>
    public class Hotel
    {
        // ── Nombre del hotel ─────────────────────────────────────────────────
        public string Nombre { get; private set; }

        // ── Coleccion de reservas en memoria ─────────────────────────────────
        private List<Reserva> _reservas;

        // ── Control de IDs y ruta del archivo ────────────────────────────────
        private int    _ultimoId;
        private string _archivoPath;

        // ── Constructor ──────────────────────────────────────────────────────
        public Hotel(string nombre, string archivoPath = "reservas.txt")
        {
            Nombre       = nombre;
            _archivoPath = archivoPath;
            _reservas    = new List<Reserva>();
            _ultimoId    = 0;

            CargarDesdeArchivo();
        }

        // ════════════════════════════════════════════════════════════════════
        // METODOS PUBLICOS
        // ════════════════════════════════════════════════════════════════════

        /// <summary>Registra una nueva reserva en la coleccion y en archivo.</summary>
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

        /// <summary>Devuelve la lista completa de reservas.</summary>
        public List<Reserva> ObtenerTodas()
        {
            return _reservas;
        }

        /// <summary>Suma el total de todas las reservas.</summary>
        public double CalcularIngresoTotal()
        {
            double total = 0;
            foreach (Reserva r in _reservas)
                total += r.CalcularTotal();
            return total;
        }

        /// <summary>Devuelve la reserva con mas noches, o null si no hay ninguna.</summary>
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

        // ════════════════════════════════════════════════════════════════════
        // METODOS PRIVADOS — ARCHIVO
        // ════════════════════════════════════════════════════════════════════

        private void CargarDesdeArchivo()
        {
            if (!File.Exists(_archivoPath))
                return;

            foreach (string linea in File.ReadAllLines(_archivoPath))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;

                var (id, reserva) = Reserva.FromLinea(linea);
                _reservas.Add(reserva);

                if (id > _ultimoId)
                    _ultimoId = id;
            }
        }

        private void GuardarEnArchivo(int id, Reserva reserva)
        {
            File.AppendAllText(_archivoPath, reserva.ToLinea(id) + Environment.NewLine);
        }

        // ════════════════════════════════════════════════════════════════════
        // METODOS PRIVADOS — VALIDACION
        // ════════════════════════════════════════════════════════════════════

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
