using System;

namespace Hotel.Modelos
{
    public class Reserva
    {
        public string   Cliente        { get; set; }
        public int      Habitacion     { get; set; }
        public DateTime FechaIngreso   { get; set; }
        public int      Noches         { get; set; }
        public double   PrecioPorNoche { get; set; }

        public DateTime FechaEgreso => FechaIngreso.AddDays(Noches);

        public double CalcularTotal()
        {
            return Noches * PrecioPorNoche;
        }

        public string ToLinea(int id)
        {
            return $"{id}|{Cliente}|{Habitacion}|{FechaIngreso:yyyy-MM-dd}|{Noches}|{PrecioPorNoche}";
        }

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
