namespace GestionComplejo.Application.Responses
{
    public class VestuarioResponse
    {
        public Guid Id { get; set; }
        public int NumeroVestuarios { get; set; }
        public bool TieneDuchas { get; set; }
        public int Capacidad { get; set; }
        public Guid CanchaId { get; set; }
    }
}
