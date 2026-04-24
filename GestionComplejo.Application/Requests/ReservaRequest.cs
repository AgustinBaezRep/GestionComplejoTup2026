namespace GestionComplejo.Application.Requests
{
    public class ReservaRequest
    {
        public Guid ClienteId { get; set; }
        public Guid CanchaId { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
