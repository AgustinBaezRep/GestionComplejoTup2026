namespace GestionComplejo.Application.Responses
{
    public class ReservaResponse
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid CanchaId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public double PrecioTotal { get; set; }
    }
}
