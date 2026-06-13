namespace Hotel.Modelos
{
    public class Reserva
    {
        // ── Propiedades (mismas que la plantilla original) ───────────────────
        public string   Cliente        { get; set; }
        public int      Habitacion     { get; set; }
        public DateTime FechaIngreso   { get; set; }
        public int      Noches         { get; set; }
        public double   PrecioPorNoche { get; set; }

        // ── Propiedad calculada ──────────────────────────────────────────────
        public DateTime FechaEgreso => FechaIngreso.AddDays(Noches);

        // ── Metodo requerido por la plantilla ────────────────────────────────
        public double CalcularTotal()
        {
            return Noches * PrecioPorNoche;
        }

        // ── Serializar a linea de texto (id|cliente|hab|fecha|noches|precio) ─
        public string ToLinea(int id)
        {
            return $"{id}|{Cliente}|{Habitacion}|{FechaIngreso:yyyy-MM-dd}|{Noches}|{PrecioPorNoche}";
        }

        // ── Deserializar desde linea de texto ────────────────────────────────
        public static (int id, Reserva reserva) FromLinea(string linea)
        {
            string[] p = linea.Split('|');
            if (p.Length != 6)
                throw new FormatException($"Linea invalida: '{linea}'");

            return (
                int.Parse(p[0]),
                new Reserva
                {
                    Cliente        = p[1],
                    Habitacion     = int.Parse(p[2]),
                    FechaIngreso   = DateTime.Parse(p[3]),
                    Noches         = int.Parse(p[4]),
                    PrecioPorNoche = double.Parse(p[5])
                }
            );
        }
    }
}
