namespace GestionComplejo.Domain.Entities
{
    public class Cancha
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Deporte { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public double Precio { get; set; }
        public bool IsDeleted { get; set; }
    }
}
