namespace GestionComplejo.Domain.Entities
{
    public class Vestuario : BaseEntity
    {
        public int NumeroVestuarios { get; set; }
        public bool TieneDuchas { get; set; }
        public int Capacidad { get; set; }
        public Guid CanchaId { get; set; }
        public Cancha Cancha { get; set; } = null!;
    }
}
