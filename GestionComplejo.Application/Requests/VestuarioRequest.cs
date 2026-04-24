namespace GestionComplejo.Application.Requests
{
    public class VestuarioRequest
    {
        public int NumeroVestuarios { get; set; }
        public bool TieneDuchas { get; set; }
        public int Capacidad { get; set; }
        public Guid CanchaId { get; set; }
    }
}
