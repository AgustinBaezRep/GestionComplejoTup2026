namespace GestionComplejo.Domain.Entities
{
    public class Reserva : BaseEntity
    {
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public Guid CanchaId { get; set; }
        public Cancha Cancha { get; set; } = null!;

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public double PrecioTotal { get; set; }
    }
}
