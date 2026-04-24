namespace GestionComplejo.Domain.Entities
{
    public class Cancha : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Deporte { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public string Piso { get; set; } = string.Empty;
        public double Precio { get; set; }

        public Vestuario? Vestuario { get; set; }
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
    }
}
